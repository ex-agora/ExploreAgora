using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;
using UnityEngine.UI;

public class CharByCharController : MonoBehaviour, IStateController
{
    public string textString;
    public float textDuration;
    public Text textUI;

    public bool isFinishedWriting()
    {
        bool finished = false;
        if (textUI.text.Length == textString.Length)
        {
            finished = true;
        }
        return finished;
    }
}