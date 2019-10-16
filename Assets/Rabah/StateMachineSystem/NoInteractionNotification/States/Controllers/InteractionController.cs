using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StateMachine;


public class InteractionController : MonoBehaviour, IStateController
{
    public Text isTappedText;
    public Text isNotTappedText;
    public TapTrigger tapTrigger;
    public StateMachineManager stateMachineManager;

    [SerializeField] string interactionText;

    public string InteractionText { get => interactionText; set => interactionText = value; }
}
