using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Close Distance Decision", menuName = "SO/SM/Decision/Close Distance", order = 0)]
public class CloseDistanceDecision : Decision
{
    public override bool MakeDecision<T>(StateMachineManager stateMachine, StateControllersManager controllersManager)
    {
        return isCorrectDistanceDecision<T>(stateMachine, controllersManager);
    }
    bool isCorrectDistanceDecision<T>(StateMachineManager stateMachine, StateControllersManager controllersManager) where T : IStateController
    {
        DistanceAlertStateController ditanceAlertStateController = controllersManager.GetController<DistanceAlertStateController>();
        if (ditanceAlertStateController == null)
        {
            return false;
        }
        return ditanceAlertStateController.ClearCloseTrigger();
    }
}
