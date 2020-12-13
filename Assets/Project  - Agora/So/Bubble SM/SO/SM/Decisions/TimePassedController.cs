using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class TimePassedController : MonoBehaviour, IStateController
{
    #region Fields
    [SerializeField] float duration;
    #endregion Fields

    #region Properties
    public float Duration { get => duration; set => duration = value; }
    #endregion Properties
}
