using StateMachine;
using UnityEngine;
/// <summary>
/// check if Writing Finished to decide write the next character or end writing then go to next state
/// </summary>
[CreateAssetMenu (fileName = "Writing Finished", menuName = "SO/SM/Decision/Writing Finished", order = 0)]
//check if all text is written
public class CharByCharWritingDecision : Decision
{
    #region Methods
    public override bool MakeDecision<T>(StateMachineManager stateMachine, StateControllersManager controllersManager)
    {
        return IsWritingFinishedDecision<T>(stateMachine, controllersManager);
    }
    bool IsWritingFinishedDecision<T>(StateMachineManager stateMachine, StateControllersManager controllersManager) where T : IStateController
    {
        CharByCharController CharByCharController = controllersManager.GetController<CharByCharController>();
        if (CharByCharController == null)
        {
            return false;
        }
        return CharByCharController.isFinishedWriting();
    }
    #endregion Methods
}