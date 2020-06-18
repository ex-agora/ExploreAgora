using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatisticalChartsManager : MonoBehaviour, ITriggable, IMenuHandler
{
    [SerializeField] StateMachineManager stateMachine;

    [SerializeField] MenuUIHandler menu;
    [SerializeField] private SpeechBubbleController bubbleController;
    [SerializeField] private TutorialPanelController tutorialPanelController;

    [SerializeField] private SummaryHandler summaryHandler;
    [SerializeField] private ToolBarHandler toolBarHandler;
    [SerializeField] private ToolBarHandler checkBtn;
    
    static StatisticalChartsManager instance;
    int wrongTrialCount;
    private int counter = 0;
    bool nextState;

    public static StatisticalChartsManager Instance { get => instance; set => instance = value; }
    public int WrongTrialCount { get => wrongTrialCount; set => wrongTrialCount = value; }
    public int Counter { get => counter; set { counter = value; CheckCounter(); } }

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
    }

    public void StartFirstQuiz()
    {
        nextState = true;
    }

    public bool GetTrigger()
    {
        bool up = nextState;
        nextState = false;
        return up;
    }

    public void GoTOHome()
    {
        //throw new System.NotImplementedException();
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void StartMachine()
    {
        stateMachine.StartSM();
    }

    public void RightChoice()
    {
        nextState = true;
    }

    public void WrongChoice()
    {
        bubbleController.SetHintText(5);
        nextState = true;
    }

    public void ShowFooterBar()
    {
        toolBarHandler.OpenToolBar();
    }

    public void StartSecondPhase()
    {
        nextState = true;
        Counter = 0;
    }

    public void CheckCounter()
    {
        if (counter == 3)
        {
            toolBarHandler.CloseToolBar();
            Invoke(nameof(PrepareForTutorial), 5f);
        }
    }

    public void CounterIncrement()
    {
        Counter++;
    }

    public void PrepareForTutorial()
    {
        StatisticalChartsObjectManager.Instance.ShowOffSugar();
        StatisticalChartsObjectManager.Instance.OpenGraphPar();
        Invoke(nameof(StartTutorial), 2f);
    }

    public void StartTutorial()
    {
        tutorialPanelController.TutorialTextStr = bubbleController.NextBubble();
        tutorialPanelController.OpenTutorial();
        bubbleController.NextBubble();
    }

    public void FinishTutorial()
    {
        StatisticalChartsObjectManager.Instance.OpenModels();
        checkBtn.OpenToolBar();
    }

    public void Check()
    {
        var finalResult = StatisticalChartsObjectManager.Instance.CompareResult();
        if (finalResult)
        {
            checkBtn.CloseToolBar();
            Invoke(nameof(ShowSummary), 2f);
        }
    }

    public void ShowSummary()
    {
        summaryHandler.ViewSummary();
    }
}