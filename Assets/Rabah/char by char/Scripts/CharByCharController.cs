using StateMachine;
using UnityEngine;
using UnityEngine.UI;

public class CharByCharController : MonoBehaviour, IStateController
{
    [SerializeField] string textString;//the text that will be written
    [SerializeField] float textDuration;//The time the all text will be written
    [SerializeField] Text textUI;//The time the all text will be written
    [HideInInspector] [SerializeField] string outputText;//the text that is already written
    int charIndex;//the index of character that is already written in textUI

    public string TextString { get => textString; set => textString = value; }
    public float TextDuration { get => textDuration; set => textDuration = value; }
    public string OutputText { get => outputText; set { outputText = value; UpdateUI (value); } }
    public int CharIndex { get => charIndex; set => charIndex = value; }

    //check if the all textString is written in textUI to decide continue writing or not
    public bool isFinishedWriting ()
    {
        bool finished = false;
        if ( OutputText.Length >= TextString.Length )
        {
            finished = true;
        }
        return finished;
    }
    //Update the textUI with new character 
    void UpdateUI (string str)
    {
        textUI.text = str;
    }
}