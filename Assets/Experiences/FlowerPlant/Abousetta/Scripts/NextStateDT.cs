using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

[CreateAssetMenu(fileName = "NextStateDT", menuName = "SO/SM/Decision/NextStateDT", order = 0)]
public class NextStateDT : Decision
{
    public override bool MakeDecision<T>(StateMachineManager stateMachine, StateControllersManager controllersManager)
    {
        return IsNextState<T>(stateMachine, controllersManager);
    }
    bool IsNextState<T>(StateMachineManager stateMachine, StateControllersManager controllersManager) where T : IStateController
    {
        var triggable = controllersManager.GetController<TriggerObjController>();
        if (triggable == null) return false;
        return triggable.GetTrigger();
    }
}
