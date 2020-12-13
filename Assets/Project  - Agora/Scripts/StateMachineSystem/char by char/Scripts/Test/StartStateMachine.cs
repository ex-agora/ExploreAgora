using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStateMachine : MonoBehaviour
{
    #region Fields
    [SerializeField]
    StateMachine.StateMachineManager SMManager;
    #endregion Fields

    #region Methods
    // Start is called before the first frame update
    [ContextMenu("Test SM")]
    public void StartSM()
    {
        SMManager.StartSM();
    }
    #endregion Methods
}