using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;

public class OnBoardingGameManager : MonoBehaviour, ITriggable ,IMenuHandler
{
    [SerializeField] StateMachineManager stateMachine;
    [SerializeField] Camera arCamera;
    [SerializeField] List<HotSpotGroupManager> spotGroupManagers;
    [SerializeField] HeaderBarHandler headerBarHandler;
    [SerializeField] SummaryHandler finalSummary;
    [SerializeField] SpeechBubbleController bubbleController;
    [SerializeField] MenuUIHandler menuUI;
    static OnBoardingGameManager instance;
    private bool isFirstDone;
    private bool nextState;

    public Camera ArCamera { get => arCamera; set => arCamera = value; }
    public static OnBoardingGameManager Instance { get => instance; set => instance = value; }
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        AudioManager.Instance.Play("bg", "Background");
        Invoke(nameof(StartMachine), 2f);
    }
    public void GoToNextState() {
        nextState = true;
    }

    private void StartMachine()
    {
        stateMachine.StartSM();
    }
    public void AddHotSpotGroupManager(HotSpotGroupManager groupManager)
    {
        spotGroupManagers.Add(groupManager);
    }

    public void StartFirstPhase()
    {
        if (!isFirstDone)
        {
            isFirstDone = true;
            nextState = true;
        }
    }
    
    public void StartSecondPhase()
    {
        nextState = true;
        headerBarHandler.OpenBar();
    }

    public void EndSecondPhase()
    {
        headerBarHandler.CloseBar();
        bubbleController.StopSpeech();
        menuUI.StopMenuInteraction();
        finalSummary.ViewSummary();
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoTOHome()
    {
        //TODO
    }

    public bool GetTrigger()
    {
        bool up = nextState;
        nextState = false;
        return up;
    }
}