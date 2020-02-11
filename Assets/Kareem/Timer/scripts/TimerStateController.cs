using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UnityEngine.UI;

public class TimerStateController : MonoBehaviour , IStateController
{
    #region Fields
    //image of timer  
    public Image timerImage;
    //full timer duration
    [SerializeField] float duration;
    //instance of StateMachineManager to get elpapsed Time
    [SerializeField] StateMachineManager stateMachineManager;
    #endregion Fields

    #region Properties
    public float Duration { get => duration; set => duration = value; }
    public StateMachineManager StateMachineManager { get => stateMachineManager; set => stateMachineManager = value; }
    #endregion Properties
}
