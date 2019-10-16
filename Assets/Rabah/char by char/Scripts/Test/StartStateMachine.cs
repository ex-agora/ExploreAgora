using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStateMachine : MonoBehaviour
{
    [SerializeField]
    StateMachine.StateMachineManager SMManager;
    // Start is called before the first frame update
    [ContextMenu("Test SM")]
    public void StartSM()
    {
        SMManager.StartSM();
    }
}