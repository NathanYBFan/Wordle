using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class Tile : MonoBehaviour
{
    [System.Serializable]
    public class State
    {
        public Color fillColor;
        public Color outlineColor;
    }

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image fill;
    [SerializeField] private Outline outline;

    [SerializeField] private State selectedState;
    [SerializeField] private char letter;

    public char Letter { get { return letter; } }
    public State SelectedState { get { return selectedState; } }
    public void SetLetter(char letter)
    {
        this.letter = letter;
        text.text = letter.ToString();
    }

    public void SetState(State state)
    {
        selectedState = state;

        fill.color = state.fillColor;
        outline.effectColor = state.outlineColor;
    }
}
