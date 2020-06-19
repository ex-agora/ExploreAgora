using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StatisticalCharts2GameManager : MonoBehaviour, ITriggable, IMenuHandler
{
    [SerializeField] StateMachineManager stateMachine;

    [SerializeField] MenuUIHandler menu;
    [SerializeField] private SpeechBubbleController bubbleController;
    [SerializeField] private TutorialPanelController tutorialPanelController;

    [SerializeField] private SummaryHandler summaryHandler;
    [SerializeField] private ToolBarHandler toolBarHandler;
    [SerializeField] private ToolBarHandler checkBtn;
    [SerializeField] private ToolBarHandler indicator;

    [SerializeField] private Button snapPanelCheckBtn;

    static StatisticalCharts2GameManager instance;
    int wrongTrialCount;
    private int counter = 0;
    bool nextState;

    public static StatisticalCharts2GameManager Instance { get => instance; set => instance = value; }
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

    public void CheckCounter()
    {
        if (counter >= 4)
        {
            snapPanelCheckBtn.interactable = true;
        }
    }

    public void CounterIncrement()
    {
        Counter++;
    }

    public void FirstPhase()
    {
        nextState = true;
        toolBarHandler.OpenToolBar();
    }

    public void FinishFirstPhase()
    {
        AudioManager.Instance?.Play("UIAction", "UI");
        toolBarHandler.CloseToolBar();
        StatisticalChartsObjectManager.Instance.OpenGraphPar();
        StartTutorial();
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
        indicator.OpenToolBar();
    }

    public void Check()
    {
        AudioManager.Instance?.Play("UIAction", "UI");
        var finalResult = StatisticalChartsObjectManager.Instance.CompareResult();
        if (finalResult)
        {
            checkBtn.CloseToolBar();
            indicator.CloseToolBar();
            Invoke(nameof(ShowSummary), 2f);
        }
    }


    public void ShowSummary()
    {
        summaryHandler.ViewSummary();
    }

}
