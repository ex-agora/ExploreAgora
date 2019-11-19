using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MothGameManager : MonoBehaviour, ITriggable, IMenuHandler
{
    [SerializeField] StateMachineManager stateMachine;
    [SerializeField] SpeechBubbleController bubbleController;
    [SerializeField] TutorialPanelController tutorialPanelController;
    [SerializeField] TimerUIHandler timerUIHandler;
    [SerializeField] CounterUIHandler counterUIHandler;
    [SerializeField] MothScoreHandler mothScoreHandler;
    [SerializeField] SummaryHandler bubbleBefore;
    [SerializeField] SummaryHandler bubbleAfter;

    [SerializeField] GameEvent repeateFirstPhase;
    [SerializeField] GameEvent continueFirstPhase;

    [SerializeField] GameEvent repeateSecondPhase;
    [SerializeField] GameEvent continueSecondPhase;

    [SerializeField] GameEvent startFirstPhase;
    [SerializeField] GameEvent startSecondPhase;

    private static MothGameManager instance;
    private bool nextState;
    int blackMoothCount;
    int whiteMoothCount;
    private bool isFirstPhase;

    public static MothGameManager Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        AudioManager.Instance.Play("bg", "Background");
        Invoke(nameof(StartMachine), 2f);
    }

    private void StartMachine()
    {
        stateMachine.StartSM();
    }

    public void WhiteMoothHit()
    {
        whiteMoothCount++;
        UpdateCounter();
    }

    public void BlackMoothHit()
    {
        blackMoothCount++;
        UpdateCounter();
    }

    public void UpdateCounter()
    {
        counterUIHandler.TextCounterStr = $"{blackMoothCount + whiteMoothCount}";
    }
    public bool GetTrigger()
    {
        bool up = nextState;
        nextState = false;
        return up;
    }

    public void GoToNextBubbleState()
    {
        nextState = true;
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoTOHome()
    {
        //TODO 
    }

    public void PrepareFirstPhase()
    {
        nextState = true;
        Invoke(nameof(ShowStaticTutorial), 2f);
    }

    public void StartFirstPhase()
    {
        whiteMoothCount = 0;
        blackMoothCount = 0;
        timerUIHandler.Duration = 30;
        timerUIHandler.ViewBar();
        timerUIHandler.StartTimer();
        counterUIHandler.TextCounterStr = "0";
        counterUIHandler.ShowCounter();
    }

    public void EndFirstPhase()
    {
        mothScoreHandler.Repeat = repeateFirstPhase;
        mothScoreHandler.ContinueFlow = continueFirstPhase;
        
        if (blackMoothCount == 0 && whiteMoothCount == 0)
        {
            mothScoreHandler.ShowSummary(true, "Before Industrialization", "Start!", blackMoothCount.ToString(), whiteMoothCount.ToString(), "You didn't catch any of the moths on the tree. Try again!.");
            StartFirstPhase();
        }
        else if (whiteMoothCount >= blackMoothCount)
        {
            mothScoreHandler.ShowSummary(false, "Before Industrialization", "Next!", blackMoothCount.ToString(), whiteMoothCount.ToString(), "Players usually hunt more black moths than white moths as they are easier to see. Let's see how this happens.");
        }
        else if (blackMoothCount < whiteMoothCount)
        {
            mothScoreHandler.ShowSummary(false, "Before Industrialization", "Next!", blackMoothCount.ToString(), whiteMoothCount.ToString(), string.Empty);
        }
    }

    public void EndPhase()
    {
        if (isFirstPhase)
        {
            EndFirstPhase();
        }
    }

    public void ShowFirstSummary()
    {
        bubbleBefore.ViewSummary();
    }

    private void ShowStaticTutorial()
    {
        tutorialPanelController.OpenTutorial();
    }
}