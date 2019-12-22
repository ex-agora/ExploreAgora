using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class TimePassedController : MonoBehaviour, IStateController
{
    [SerializeField] float duration;

    public float Duration { get => duration; set => duration = value; }
}
