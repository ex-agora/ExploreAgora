using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

[CreateAssetMenu(fileName = "EpsilonTransition", menuName = "SO/SM/Decision/EpsilonTransition", order = 0)]
public class EpsilonTransition : Decision
{
    public override bool MakeDecision<T>(StateMachineManager stateMachine, StateControllersManager controllersManager)
    {
        return GotoNextStateAutomatic<T>(stateMachine, controllersManager);
    }
    bool GotoNextStateAutomatic<T>(StateMachineManager stateMachine, StateControllersManager controllersManager) where T : IStateController
    {
        return true;
    }
}