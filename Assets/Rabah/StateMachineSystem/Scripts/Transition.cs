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
        public Decision decision;
        public State trueState;
        public State falseState;

        //Decide the next state according to the decision
        public State Decide<T>(StateMachineManager stateMachine, StateControllersManager controllersManager) where T : IStateController
        {
            if (decision.MakeDecision<T>(stateMachine, controllersManager)) return trueState;
            else return falseState;
        }
    }
}