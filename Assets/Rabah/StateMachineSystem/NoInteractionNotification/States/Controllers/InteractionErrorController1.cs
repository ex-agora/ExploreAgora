using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StateMachine;


public class InteractionErrorController1 : MonoBehaviour, IStateController
{
    public Text text;
    public TapTrigger tapTrigger;
    [SerializeField] string errorText;
    public string ErrorText { get => errorText; set => errorText = value; }
}
