using UnityEngine;

public sealed class Row : MonoBehaviour
{
    [SerializeField] private Tile[] tiles;

    public string word
    {
        get
        {
            string word = "";
            for (int i = 0; i < tiles.Length; i++)
                word += tiles[i].Letter;

            return word;
        }
    }

    public Tile[] Tiles { get { return tiles; } }
}
