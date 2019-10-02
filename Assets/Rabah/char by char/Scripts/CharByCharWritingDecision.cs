using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

[CreateAssetMenu(fileName = "Writing Finished", menuName = "SO/SM/Decision/Writing Finished", order = 0)]
public class CharByCharWritingDecision : Decision
{
    public override bool MakeDecision<T>(StateMachineManager stateMachine, StateControllersManager controllersManager)
    {
        return isWritingFinishedDecision<T>(stateMachine, controllersManager);
    }
    bool isWritingFinishedDecision<T>(StateMachineManager stateMachine, StateControllersManager controllersManager) where T : IStateController
    {
        CharByCharController CharByCharController = controllersManager.GetController<CharByCharController>();
        if (CharByCharController == null)
        {
            return false;
        }
        return CharByCharController.isFinishedWriting();
    }
}