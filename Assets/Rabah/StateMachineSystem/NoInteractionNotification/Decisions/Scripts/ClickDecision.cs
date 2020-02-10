using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

[CreateAssetMenu(fileName = "ClickDecision", menuName = "SO/SM/Decision/ClickDecision", order = 0)]
public class ClickDecision : Decision
{
    #region Methods
    public override bool MakeDecision<T>(StateMachineManager stateMachine, StateControllersManager controllersManager)
    {
        return IsTapped<T>(stateMachine, controllersManager);
    }

    bool IsTapped<T>(StateMachineManager stateMachine, StateControllersManager controllersManager) where T : IStateController
    {
        InteractionController interactionController = controllersManager.GetController<InteractionController>();
        if (interactionController == null)
            return false;
        return interactionController.tapTrigger.IsTapped;
    }
    #endregion Methods
}

