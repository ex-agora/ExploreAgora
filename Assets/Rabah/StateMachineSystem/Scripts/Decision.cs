using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// Create SCriptable object path for the inherited actions
    /// T is used to specify type
    /// StateControllersManager returns the controller which contaions the state's variables
    /// StateMachineManager manage the  transition betweeen states 
    /// </summary>
    public abstract class Decision : ScriptableObject
    {
        //The transition is based on whether it returns true or false
        public abstract bool MakeDecision<T>(StateMachineManager stateMachine, StateControllersManager controllersManager) where T : IStateController;
    }
}