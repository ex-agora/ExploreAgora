using StateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MothGameManager : MonoBehaviour, ITriggable, IMenuHandler
{
    #region Fields
    private static MothGameManager instance;
    [SerializeField] private ToolBarHandler barHandler;
    private int blackMoothCount;
    [SerializeField] private SummaryHandler bubbleAfter;
    [SerializeField] private SummaryHandler bubbleBefore;
    [SerializeField] private SpeechBubbleController bubbleController;
    [SerializeField] private GameEvent continueFirstPhase;
    [SerializeField] private GameEvent continueSecondPhase;
    [SerializeField] private CounterUIHandler counterUIHandler;
    [SerializeField] private SummaryHandler finalSummary;
    private bool isFirstPhase;
    [SerializeField] private MothScoreHandler mothScoreHandler;
    private bool nextState;
    [SerializeField] private GameEvent prepareSecondPhase;
    [SerializeField] private GameEvent repeateFirstPhase;
    [SerializeField] private GameEvent repeateSecondPhase;
    [SerializeField] private ToolBarHandler secondPhaseBtn;
    [SerializeField] private GameEvent startFirstPhase;
    [SerializeField] private GameEvent startSecondPhase;
    [SerializeField] private StateMachineManager stateMachine;
    [SerializeField] private TimerUIHandler timerUIHandler;
    [SerializeField] private TutorialPanelController tutorialPanelController;
    private int whiteMoothCount;
    bool isRelocatePressed;
    bool isSecondBuffer;

    bool isFirstStartBuffer;
    bool isSecondStartBuffer;
    #endregion Fields

    #region Properties
    public static MothGameManager Instance { get => instance; set => instance = value; }
    #endregion Properties

    #region Methods
    public void BlackMoothHit()
    {
        blackMoothCount++;
        UpdateCounter();
    }

    public void ContinueFirstPhase()
    {
        isFirstPhase = false;
        Invoke(nameof(ShowFirstSummary), 1.5f);
        //ShowFirstSummary();
    }

    public void ContinueSecondPhase()
    {
        Invoke(nameof(ShowSecondSummary), 1.5f);
        //ShowSecondSummary();
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
            timerUIHandler.HideBar();
            mothScoreHandler.ShowSummary(false, "Before Industrialization", "Next!", blackMoothCount.ToString(), whiteMoothCount.ToString(), "Players usually hunt more black moths than white moths as they are easier to see. Let's see how this happens.");
        }
        else if (blackMoothCount > whiteMoothCount)
        {
            timerUIHandler.HideBar();
            mothScoreHandler.ShowSummary(false, "Before Industrialization", "Next!", blackMoothCount.ToString(), whiteMoothCount.ToString(), string.Empty);
        }
    }

    public void EndPhase()
    {
        
        counterUIHandler.HideCounter();
        if (isFirstPhase)
        {
            EndFirstPhase();
        }
        else
        {
            EndSecondPhase();
        }
    }
    public void ToggleRelocate() {
        isRelocatePressed = !isRelocatePressed;
        if (!isRelocatePressed) {
            CheckBuffer ();
        }
    }

    public void EndSecondPhase()
    {
        mothScoreHandler.Repeat = repeateSecondPhase;
        mothScoreHandler.ContinueFlow = continueSecondPhase;

        if (blackMoothCount == 0 && whiteMoothCount == 0)
        {
            mothScoreHandler.ShowSummary(true, "After Industrialization", "Start!", blackMoothCount.ToString(), whiteMoothCount.ToString(), "You didn't catch any of the moths on the tree. Try again!.");
            StartSecondPhase();
        }
        else if (whiteMoothCount <= blackMoothCount)
        {
            timerUIHandler.HideBar();
            mothScoreHandler.ShowSummary(false, "After Industrialization", "Next!", blackMoothCount.ToString(), whiteMoothCount.ToString(), "Players usually hunt more white moths than black moths as they are easier to see. Let's see how this happens.");
        }
        else if (blackMoothCount < whiteMoothCount)
        {
            timerUIHandler.HideBar();
            mothScoreHandler.ShowSummary(false, "After Industrialization", "Next!", blackMoothCount.ToString(), whiteMoothCount.ToString(), string.Empty);
        }
    }

    public void FarAlert()
    {
        bubbleController.SetHintText(7);
        nextState = true;
    }

    public void FinalSummary()
    {
        Invoke(nameof(ShowFinalSummary), 1f);
    }
    void ShowFinalSummary() {
        finalSummary.ViewSummary();
    }

    public bool GetTrigger()
    {
        bool up = nextState;
        nextState = false;
        return up;
    }

    public void GoTOHome()
    {
        //TODO
    }

    public void GoToNextBubbleState()
    {
        nextState = true;
    }

    public void NearAlert()
    {
        bubbleController.SetHintText(6);
        nextState = true;
    }

    public void PrepareFirstPhase()
    {
        nextState = true;
        //Debug.LogError("sjhdjshd");
        Invoke(nameof(ShowStaticTutorial), 12f);
        //Debug.LogError("12121223");
    }

    public void PrepareSecondPhase()
    {
        nextState = true;
        Invoke(nameof(CreateSecondPhase), 4f);
    }
    void CreateSecondPhase() {
        if (isRelocatePressed)
        {
            isSecondBuffer = true;
            return;
        }
        isSecondBuffer = false;
        prepareSecondPhase.Raise();
        nextState = true;
        barHandler.OpenToolBar();
    }
    void CheckBuffer() {
        if (isFirstStartBuffer)
            StartFirstPhase();
        if (isSecondBuffer)
            CreateSecondPhase();
        if (isSecondStartBuffer)
            StartSecondPhase();
    }
    public void VFXStopped() {
        barHandler.CloseToolBar();
        nextState = true;
        Invoke(nameof(ShowButtonSecondPhase), 4.5f);
    }
    void ShowButtonSecondPhase() {
        secondPhaseBtn.OpenToolBar();
    }
    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowFirstSummary()
    {
        bubbleBefore.ViewSummary();
    }

    public void ShowSecondSummary()
    {
        bubbleAfter.ViewSummary();
    }

    public void StartFirstPhase()
    {
        if (isRelocatePressed)
        {
            isFirstStartBuffer = true;
            return;
        }
        isFirstStartBuffer = false;
        isFirstPhase = true;
        whiteMoothCount = 0;
        blackMoothCount = 0;
        timerUIHandler.Duration = 30;
        timerUIHandler.ViewBar();
        timerUIHandler.StartTimer();
        counterUIHandler.TextCounterStr = "0";
        counterUIHandler.ShowCounter();
        startFirstPhase.Raise();
    }

    public void StartSecondPhase()
    {
        if (isRelocatePressed)
        {
            isSecondStartBuffer = true;
            return;
        }
        isSecondStartBuffer = false;
        barHandler.CloseToolBar();
        startSecondPhase.Raise();
        whiteMoothCount = 0;
        blackMoothCount = 0;
        timerUIHandler.Duration = 30;
        timerUIHandler.ViewBar();
        timerUIHandler.StartTimer();
        counterUIHandler.TextCounterStr = "0";
        counterUIHandler.ShowCounter();
    }

    public void UpdateCounter()
    {
        counterUIHandler.TextCounterStr = $"{blackMoothCount + whiteMoothCount}";
    }

    public void WhiteMoothHit()
    {
        whiteMoothCount++;
        UpdateCounter();
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void ShowStaticTutorial()
    {
        Debug.LogError("1223");

        tutorialPanelController.TutorialTextStr = bubbleController.NextBubble();
        tutorialPanelController.OpenTutorial();
        Debug.LogError("5555");
    }

    private void Start()
    {
        AudioManager.Instance.Play("birds", "Background");
        isRelocatePressed = true;
        Invoke(nameof(StartMachine), 2f);
    }
    private void OnDisable()
    {
        stateMachine.StopSM();
    }
    private void StartMachine()
    {
        stateMachine.StartSM();
    }
    #endregion Methods
}