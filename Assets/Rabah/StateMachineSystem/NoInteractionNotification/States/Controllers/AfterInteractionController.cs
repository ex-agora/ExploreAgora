using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StateMachine;


public class AfterInteractionController : MonoBehaviour, IStateController
{
    public Text text;
    public TapTrigger tapTrigger;
       
    [SerializeField] string afterText;

    public string AfterText { get => afterText; set => afterText = value; }
}
