using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;
public class PlantPartsGameManager : MonoBehaviour, ITriggable, IMenuHandler
{
    [SerializeField] StateMachineManager stateMachine;
    [SerializeField] CounterUIHandler counterUIHandler;
    [SerializeField] SummaryHandler midSummary;
    [SerializeField] SummaryHandler finalSummary;
    [SerializeField] ToolBarHandler barHandler;
    static PlantPartsGameManager instance;
    [SerializeField] Camera arCamera;
    [SerializeField] List<HotSpotGroupManager> spotGroupManagers;
    [SerializeField] CounterUIHandler uICounterHandler;
    [SerializeField] TutorialPanelController tutorial;
    int expolreCount;
    int wrongTrialCount;
    [SerializeField] bool nextState;
    [SerializeField] SpeechBubbleController bubbleController;
    [SerializeField] MenuUIHandler menu;
    int maxQuizPart;
    int correctAnswer;
    bool isFirstDone;
    public static PlantPartsGameManager Instance { get => instance; set => instance = value; }
    public Camera ArCamera { get => arCamera; set => arCamera = value; }
    public int WrongTrialCount { get => wrongTrialCount; set => wrongTrialCount = value; }
    public int MaxQuizPart { get => maxQuizPart; set => maxQuizPart = value; }
    public int CorrectAnswer { get => correctAnswer; set { correctAnswer = value; UpdateUICounterQuiz(); } }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        WrongTrialCount = 0;
    }
    private void Start()
    {
        AudioManager.Instance.Play("bg", "Background");
        Invoke(nameof(StartMachine), 2f);
        expolreCount = 0;
        isFirstDone = false;
    }

    private void StartMachine()
    {
        stateMachine.StartSM();
    }
    public void AddHotSpotGroupManager(HotSpotGroupManager groupManager) {
        spotGroupManagers.Add(groupManager);
    }
    public void ShowCounter() {
        UpdateUICounter();
        uICounterHandler.ShowCounter();
    }
    public void CheckExploraState()
    {
        expolreCount++;
        if (expolreCount == spotGroupManagers.Count) { Invoke(nameof(EndFirstPhase), 2f);  }
        else
        {
            nextState = true;
            for (int i = 0; i < spotGroupManagers.Count; i++)
            {
                spotGroupManagers[i].gameObject.SetActive(true);
            }
        }
    }
    void EndFirstPhase() {
        menu.StopMenuInteraction();
        bubbleController.StopSpeech();
        midSummary.ViewSummary();
    }
    public void UpdateUICounter()
    {
        int maxSpot = 0;
        int openedSpot = 0;
        for (int i = 0; i < spotGroupManagers.Count; i++)
        {
            maxSpot += spotGroupManagers[i].HotSpotMaxCounter;
            openedSpot += spotGroupManagers[i].HotSpotOpenedCounter;
        }
        uICounterHandler.TextCounterStr = $"{openedSpot} / {maxSpot}";
    }
    public void UpdateUICounterQuiz() {
        uICounterHandler.TextCounterStr = $"{correctAnswer} / {maxQuizPart}";
        if (correctAnswer == maxQuizPart) {
            Invoke(nameof(EndSecondPhase), 2f);
        }
    }

    void EndSecondPhase() {
        barHandler.CloseToolBar();
        uICounterHandler.HideCounter();
        menu.StopMenuInteraction();
        bubbleController.StopSpeech();
        finalSummary.ViewSummary();
    }
    void PrepaireQuiz()
    {
        for (int i = 0; i < spotGroupManagers.Count; i++)
        {
            spotGroupManagers[i].PrepaireQuiz();
        }
    }
    public void FarAlert()
    {
        bubbleController.SetHintText(8);
        nextState = true;
    }
    public void NearAlert()
    {
        bubbleController.SetHintText(7);
        nextState = true;
    }
    public void StartFirstPhase()
    {
        //counterUIHandler.ShowCounter();
        if (!isFirstDone)
        {
            isFirstDone = true;
            nextState = true;
        }
    }
    public void StartQuiz() {
        menu.RunMenuInteraction();
        bubbleController.RunSpeech();
        barHandler.OpenToolBar();
    }

    public void StartSecondPhase()
    {
        PrepaireQuiz();
        UpdateUICounterQuiz();
        Invoke(nameof(StartTutorial), 4f);
    }
    void StartTutorial() {
        tutorial.TutorialTextStr = bubbleController.NextBubble();
        tutorial.OpenTutorial();
        bubbleController.NextBubble();
    }
    public bool GetTrigger()
    {
        bool up = nextState;
        nextState = false;
        return up;
    }
    public void GoToNextBubbleState() {
        nextState = true;
    }

    public void ResetLevel()
    {
        FinishExperiencesHandler.Instance.Reload();
    }

    public void GoTOHome()
    {
        FinishExperiencesHandler.Instance.GotoHome();
    }
    public void FinishExperience()
    {
        int score = 0;
        if (WrongTrialCount == 0)
            score = 3;
        else if (WrongTrialCount > 0 && WrongTrialCount < 3)
            score = 2;
        else
            score = 1;
        FinishExperiencesHandler.Instance.FinshExperience(score);
    }
}
