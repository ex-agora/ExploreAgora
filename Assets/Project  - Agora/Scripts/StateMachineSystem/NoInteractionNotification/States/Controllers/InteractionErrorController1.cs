using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StateMachine;


public class InteractionErrorController1 : MonoBehaviour, IStateController
{
    #region Fields
    public TapTrigger tapTrigger;
    public Text text;
    [SerializeField] string errorText;
    #endregion Fields

    #region Properties
    public string ErrorText { get => errorText; set => errorText = value; }
    #endregion Properties
}
