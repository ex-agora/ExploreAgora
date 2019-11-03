using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class TriggerObjController : MonoBehaviour, IStateController
{
    [TypeConstraint(typeof(ITriggable))] [SerializeField] GameObject triggerd;
    ITriggable triggable;

    void Start()
    {
        triggable = triggerd.GetComponent<ITriggable>();
    }

    public bool GetTrigger()
    {
        return triggable.GetTrigger();
    }
}