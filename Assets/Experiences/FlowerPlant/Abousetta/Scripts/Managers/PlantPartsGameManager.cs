using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantPartsGameManager : MonoBehaviour
{
    [SerializeField] StateMachineManager stateMachine;

    private void Start()
    {
        Invoke(nameof(StartMachin), 2f);
    }

    private void StartMachin()
    {
        stateMachine.StartSM();
    }
}
