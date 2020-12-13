using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// serialized class contains all actions for each state life cycle part
    /// </summary>
    [System.Serializable]
    public class StateBehviour
    {
        #region Fields
        public Action[] stateEnterActions;
        public Action[] stateExitActions;
        public Action[] stateFixedStayActions;
        public Action[] stateLateStayActions;
        public Action[] stateResetActions;
        public Action[] stateStayActions;
        #endregion Fields

        #region Methods
        //OnEnterState this function activates all stateEnterActions(Start)
        public void OnExctueStateEnter<T>(StateControllersManager controllersManager) where T : IStateController
        {
            for (int i = 0; i < stateEnterActions.Length; i++)
            {
                stateEnterActions[i].Act<T>(controllersManager);
            }
        }
        //OnExitState this function activates all stateExitActions
        public void OnExctueStateExit<T>(StateControllersManager controllersManager) where T : IStateController
        {
            for (int i = 0; i < stateExitActions.Length; i++)
            {
                stateExitActions[i].Act<T>(controllersManager);
            }
        }
        //OnFixedStayState this function activates all stateFixedStayActions(FixedUpdate)
        public void OnExctueStateFixedStay<T>(StateControllersManager controllersManager) where T : IStateController
        {
            for (int i = 0; i < stateFixedStayActions.Length; i++)
            {
                stateFixedStayActions[i].Act<T>(controllersManager);
            }
        }

        //OnLateStayState this function activates all stateLateStayActions(LateUpdate)
        public void OnExctueStateLateStay<T>(StateControllersManager controllersManager) where T : IStateController
        {
            for (int i = 0; i < stateLateStayActions.Length; i++)
            {
                stateLateStayActions[i].Act<T>(controllersManager);
            }
        }

        //OnResetState this function activates all stateResetActions(Reset)
        public void OnExctueStateReset<T>(StateControllersManager controllersManager) where T : IStateController
        {
            for (int i = 0; i < stateResetActions.Length; i++)
            {
                stateResetActions[i].Act<T>(controllersManager);
            }
        }

        //OnStayState this function activates all stateStayActions(Update)
        public void OnExctueStateStay<T>(StateControllersManager controllersManager) where T : IStateController
        {
            for (int i = 0; i < stateStayActions.Length; i++)
            {
                stateStayActions[i].Act<T>(controllersManager);
            }
        }
        #endregion Methods
    }
}