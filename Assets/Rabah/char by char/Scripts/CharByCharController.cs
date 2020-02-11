using StateMachine;
using UnityEngine;
using UnityEngine.UI;

public class CharByCharController : MonoBehaviour, IStateController
{
    #region Fields
    int charIndex;
    [HideInInspector] [SerializeField] string outputText;
    [SerializeField] float textDuration;
    [SerializeField] string textString;//the text that will be written
    //The time the all text will be written
    [SerializeField] Text textUI;
    #endregion Fields

    //The time the all text will be written
    //the text that is already written
    //the index of character that is already written in textUI

    #region Properties
    public int CharIndex { get => charIndex; set => charIndex = value; }
    public string OutputText { get => outputText; set { outputText = value; UpdateUI(value); } }
    public float TextDuration { get => textDuration; set => textDuration = value; }
    public string TextString { get => textString; set => textString = value; }
    #endregion Properties

    #region Methods
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
    #endregion Methods
}