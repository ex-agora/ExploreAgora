using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
public class PlantPartsGameManager : MonoBehaviour
{
    [SerializeField] StateMachineManager stateMachine;
    static PlantPartsGameManager instance;
    [SerializeField] Camera arCamera;

    public static PlantPartsGameManager Instance { get => instance; set => instance = value; }
    public Camera ArCamera { get => arCamera; set => arCamera = value; }

    private void Awake ()
    {
        if ( Instance == null )
            Instance = this;
    }
    private void Start()
    {
        Invoke(nameof(StartMachin), 2f);
    }

    private void StartMachin()
    {
        stateMachine.StartSM();
    }
}
