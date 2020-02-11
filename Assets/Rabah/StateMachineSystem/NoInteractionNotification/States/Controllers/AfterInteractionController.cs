using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StateMachine;


public class AfterInteractionController : MonoBehaviour, IStateController
{
    #region Fields
    public TapTrigger tapTrigger;
    public Text text;
    [SerializeField] string afterText;
    #endregion Fields

    #region Properties
    public string AfterText { get => afterText; set => afterText = value; }
    #endregion Properties
}
