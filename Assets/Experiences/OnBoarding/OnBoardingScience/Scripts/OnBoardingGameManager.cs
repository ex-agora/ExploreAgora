using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;

public class OnBoardingGameManager : MonoBehaviour
{
    [SerializeField] StateMachineManager stateMachine;
    [SerializeField] Camera arCamera;
    [SerializeField] List<HotSpotGroupManager> spotGroupManagers;

    public Camera ArCamera { get => arCamera; set => arCamera = value; }

    private void Start()
    {
        AudioManager.Instance.Play("bg", "Background");
        Invoke(nameof(StartMachine), 2f);
    }

    private void StartMachine()
    {
        stateMachine.StartSM();
    }
    public void AddHotSpotGroupManager(HotSpotGroupManager groupManager)
    {
        spotGroupManagers.Add(groupManager);
    }
}