using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

[CreateAssetMenu(fileName ="Far Distance Decision",menuName = "SO/SM/Decision/Far Distance", order =0)]
public class FarDistanceDecision : Decision
{
    #region Methods
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
        return ditanceAlertStateController.ClearFarTrigger();
    }
    #endregion Methods
}
