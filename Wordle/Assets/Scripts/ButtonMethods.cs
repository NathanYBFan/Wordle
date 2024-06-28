using UnityEngine;

public class ButtonMethods : MonoBehaviour
{
    [SerializeField]
    private Board gameBoard;

    public void TryAgainPressed()
    {
        gameBoard.TryAgain();
    }

    public void NewWordPressed()
    {
        gameBoard.NewGame();
    }
}
