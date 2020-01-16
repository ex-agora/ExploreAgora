
using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// Create SCriptable object path for the inherited actions
    /// T is used to specify type
    /// StateControllersManager returns the controller which contaions the state's variables
    /// </summary>
    public abstract class Action : ScriptableObject
    {
        #region Methods
        //what the state Acts
        public abstract void Act<T>(StateControllersManager controllersManager) where T : IStateController;
        #endregion Methods
    }
}