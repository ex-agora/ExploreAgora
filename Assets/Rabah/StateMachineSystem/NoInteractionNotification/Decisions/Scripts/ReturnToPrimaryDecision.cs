using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

[CreateAssetMenu(fileName = "ReturnToPrimaryDecision", menuName = "SO/SM/Decision/ReturnToPrimaryDecision", order = 0)]
public class ReturnToPrimaryDecision : Decision
{
    #region Methods
    public override bool MakeDecision<T>(StateMachineManager stateMachine, StateControllersManager controllersManager)
    {
        return CheckTime<T>(stateMachine, controllersManager);
    }

    bool CheckTime<T>(StateMachineManager stateMachine, StateControllersManager controllersManager) where T : IStateController
    {
        InteractionController interactionController = controllersManager.GetController<InteractionController>();
        if (interactionController == null)
            return false;
        return interactionController.tapTrigger.IsTapped;
    }
    #endregion Methods
}

