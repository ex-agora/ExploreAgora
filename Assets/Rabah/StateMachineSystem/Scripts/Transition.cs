namespace StateMachine
{
    /// <summary>
    /// Serializable class contains the decision, trueState and falseState
    /// if decision is true it makes Transition to trueState
    /// else go to falseState
    /// </summary>
    [System.Serializable]
    public class Transition
    {
        #region Fields
        public Decision decision;
        public State falseState;
        public State trueState;
        #endregion Fields

        #region Methods
        //Decide the next state according to the decision
        public State Decide<T>(StateMachineManager stateMachine, StateControllersManager controllersManager) where T : IStateController
        {
            if (decision.MakeDecision<T>(stateMachine, controllersManager)) return trueState;
            else return falseState;
        }
        #endregion Methods
    }
}