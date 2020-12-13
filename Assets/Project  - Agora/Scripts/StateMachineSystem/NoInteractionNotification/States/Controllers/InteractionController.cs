using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StateMachine;


public class InteractionController : MonoBehaviour, IStateController
{
    #region Fields
    public Text isNotTappedText;
    public Text isTappedText;
    public StateMachineManager stateMachineManager;
    public TapTrigger tapTrigger;
    [SerializeField] string interactionText;
    #endregion Fields

    #region Properties
    public string InteractionText { get => interactionText; set => interactionText = value; }
    #endregion Properties
}
