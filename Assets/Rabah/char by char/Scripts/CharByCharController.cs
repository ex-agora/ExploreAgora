using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;
using UnityEngine.UI;

public class CharByCharController : MonoBehaviour, IStateController
{
    [SerializeField] string textString;
    [SerializeField] float textDuration;
    [SerializeField] Text textUI;
    [HideInInspector] [SerializeField] string outputText;
    int charIndex;

    public string TextString { get => textString; set => textString = value; }
    public float TextDuration { get => textDuration; set => textDuration = value; }
    public string OutputText { get => outputText; set { outputText = value; UpdateUI(value); } }
    public int CharIndex { get => charIndex; set => charIndex = value; }

    public bool isFinishedWriting()
    {
        bool finished = false;
        if (OutputText.Length >= TextString.Length)
        {
            finished = true;
        }
        return finished;
    }
    void UpdateUI(string str)
    {
        textUI.text = str;
    }
}