using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

[CreateAssetMenu(fileName = "DTimePassed", menuName = "SO/SM/Decision/TimeHelper/DTimePassed", order = 0)]
public class DTimePassed : Decision
{
    public override bool MakeDecision<T>(StateMachineManager stateMachine, StateControllersManager controllersManager)
    {
        return CheckClick<T>(stateMachine, controllersManager);
    }

    bool CheckClick<T>(StateMachineManager stateMachine, StateControllersManager controllersManager) where T : IStateController
    {
        var timePassedHandler = controllersManager.GetController<TimePassedController>();
        if (timePassedHandler == null)
            return false;
        //Debug.Log(timePassedHandler.Duration);
        return (timePassedHandler.Duration <= stateMachine.ElpTime);
    }
}