using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// state is a part of the experience that compines actions and transitions
    /// the transition is decided by the decisions
    /// to check if can go to next state or stay in sae state.
    /// The state has life cycle enter,stay,lateStay,fixedStay,exit,reset
    /// </summary>
    [CreateAssetMenu(fileName = "State", menuName = "SO/SM/State", order = 0)]
    public class State : ScriptableObject
    {
        [Header("Actions Behaviour")]
        [Tooltip("Acions are excuted in order (FIFO)")]
        [SerializeField] StateBehviour behviour;
        [SerializeField] Transition[] transition;

        #region State Life Cycle
        //The Start of the state
        public void OnEnterState<T>(StateControllersManager controllersManager) where T : IStateController
        {
            behviour.OnExctueStateEnter<T>(controllersManager);
        }
        //The Update of the state
        public void OnStayState<T>(StateControllersManager controllersManager) where T : IStateController
        {
            behviour.OnExctueStateStay<T>(controllersManager);
        }
        //The LateUpdate of the state
        public void OnLateStayState<T>(StateControllersManager controllersManager) where T : IStateController
        {
            behviour.OnExctueStateLateStay<T>(controllersManager);
        }
        //The FixedUpdate of the state
        public void OnFixedStayState<T>(StateControllersManager controllersManager) where T : IStateController
        {
            behviour.OnExctueStateFixedStay<T>(controllersManager);
        }
        //The End of the state
        public void OnExitState<T>(StateControllersManager controllersManager) where T : IStateController
        {
            behviour.OnExctueStateExit<T>(controllersManager);
        }
        //The Reset of the state
        public void OnResetState<T>(StateControllersManager controllersManager) where T : IStateController
        {
            behviour.OnExctueStateReset<T>(controllersManager);
        }
        #endregion
        //Check all transitions to decide the next state in StateMachineManager
        public void CheckTransiton<T>(StateMachineManager stateMachineManager, StateControllersManager controllersManager) where T : IStateController
        {
            for (int i = 0; i < transition.Length; i++)
            {
                stateMachineManager.GoTo(transition[i].Decide<T>(stateMachineManager, controllersManager));
            }
        }
    }
}