using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    private static readonly KeyCode[] SUPPORTED_KEYS = new KeyCode[]
    {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E,
        KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J,
        KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O,
        KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T,
        KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y,
        KeyCode.Z,
    };

    [SerializeField]
    private Row[] rows;

    [Header("States")]
    public Tile.State EmptyState;
    public Tile.State OccupiedState;
    public Tile.State CorrectState;
    public Tile.State WrongSpotState;
    public Tile.State IncorrectState;

    [Header("UI")]
    [SerializeField]
    private GameObject invalidWordText;

    [SerializeField]
    private GameObject ButtonHolder;
    public GameObject TryAgainButton;


    private string[] solutionWords;
    private string[] validWords;

    private string selectedWord;


    private int rowIndex = 0;
    private int columnIndex = 0;

    private void Start()
    {
        LoadData();
        NewGame();
    }

    private void OnEnable()
    {
        ButtonHolder.SetActive(false);
    }

    private void OnDisable()
    {
        ButtonHolder.SetActive(true);
    }

    private void Update()
    {
        Row currentRow = rows[rowIndex];


        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            columnIndex = Mathf.Max(columnIndex - 1, 0);

            currentRow.Tiles[columnIndex].SetLetter('\0');
            currentRow.Tiles[columnIndex].SetState(EmptyState);
            
            invalidWordText.SetActive(false);
            return;
        }

        if (columnIndex >= rows[rowIndex].Tiles.Length)
        {
            if (Input.GetKeyDown(KeyCode.Return))
                SubmitRow(currentRow);
            return;
        }

        for (int i = 0; i < SUPPORTED_KEYS.Length; i++)
        {
            if (Input.GetKeyDown(SUPPORTED_KEYS[i]))
            {
                currentRow.Tiles[columnIndex].SetLetter((char)SUPPORTED_KEYS[i]);
                currentRow.Tiles[columnIndex].SetState(OccupiedState);

                columnIndex++;
                return;
            }
        }
    }

    public void NewGame()
    {
        SetRandomWord();
        TryAgain();
    }

    public void TryAgain()
    {
        ClearBoard();
        enabled = true;
    }

    private void ClearBoard()
    {
        foreach(Row row in rows)
        {
            foreach(Tile tile in row.Tiles)
            {
                tile.SetLetter('\0');
                tile.SetState(EmptyState);
            }
        }

        rowIndex = 0;
        columnIndex = 0;
    }

    private void LoadData()
    {
        TextAsset allValidWords = Resources.Load("official_wordle_all") as TextAsset;
        validWords = allValidWords.text.Split('\n');


        TextAsset allSolutionWords = Resources.Load("official_wordle_common") as TextAsset;
        solutionWords = allSolutionWords.text.Split('\n');
    }

    private void SetRandomWord() { selectedWord = solutionWords[Random.Range(0, solutionWords.Length)].ToLower().Trim(); }

    private void SubmitRow(Row row)
    {
        if (!isValidWord(row.word))
        {
            invalidWordText.SetActive(true);
            return;
        }

        string remaining = selectedWord;

        // Straight up correct or wrong
        for (int i = 0; i < row.Tiles.Length; i++)
        {
            Tile tiles = row.Tiles[i];
            if (tiles.Letter == selectedWord[i])
            {
                tiles.SetState(CorrectState);

                remaining = remaining.Remove(i, 1);
                remaining = remaining.Insert(i, " ");
            }
            else if (!selectedWord.Contains(tiles.Letter))
                tiles.SetState(IncorrectState);
        }

        // Check incorrect or wrong spot for duplicates
        for (int i = 0; i < row.Tiles.Length; i++)
        {
            Tile tiles = row.Tiles[i];

            if (tiles.SelectedState != CorrectState && tiles.SelectedState != IncorrectState)
            {
                if (remaining.Contains(tiles.Letter))
                {
                    tiles.SetState(WrongSpotState);

                    int index = remaining.IndexOf(tiles.Letter);
                    remaining = remaining.Remove(index, 1);
                    remaining = remaining.Insert(index, " ");
                }
                else
                    tiles.SetState(IncorrectState);
            }
        }

        if (hasWon(row))
        {
            TryAgainButton.SetActive(false);
            enabled = false;
        }

        // Increment
        rowIndex++;
        columnIndex = 0;

        if (rowIndex >= rows.Length)
        {
            TryAgainButton.SetActive(true);
            enabled = false;
        }
    }

    private bool isValidWord(string word)
    {
        if (word.Length <= rows[0].Tiles.Length - 1) return false;

        for (int i = 0; i < validWords.Length; i++)
            if (validWords[i] == word)
                return true;
        return false;
    }

    public bool hasWon(Row row)
    {
        return row.word == selectedWord;
    }

}
