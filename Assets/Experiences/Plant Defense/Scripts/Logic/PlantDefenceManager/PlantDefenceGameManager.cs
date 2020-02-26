using StateMachine;

using UnityEngine;
using UnityEngine.SceneManagement;

public class PlantDefenceGameManager : MonoBehaviour, ITriggable, IMenuHandler
{
    #region Fields
    [SerializeField] PlantdefenceFlowDurations flowDurations;
    private static PlantDefenceGameManager instance;
    [SerializeField] private Animator bubbleAnimator;
    [SerializeField] private SpeechBubbleController bubbleController;
    private float elpTime;
    [SerializeField] private PDInformationPanelManager informationPanelManager;
    [SerializeField] private SummaryHandler midSummary;
    private bool nextState;
    [SerializeField] private SheildUIHander sheildCounter;
    [SerializeField] private CounterUIHandler sheildUI;
    [SerializeField] private StateMachineManager stateMachine;
    [SerializeField] private GameObject pdInfoManagerPrefab;
    [SerializeField] private SummaryHandler finalSummary;
    [SerializeField] private PDBarFeedbackHandler feedbackHandler;
    private int counter = 0;
    private int tapHintCounter = 0;
    private float updateRate = 0.01f;
    #endregion Fields

    #region Properties
    public static PlantDefenceGameManager Instance
    {
        get => instance;
        set => instance = value;
    }
    public Animator BubbleAnimator
    {
        get => bubbleAnimator;
        set => bubbleAnimator = value;
    }
    public SpeechBubbleController BubbleController
    {
        get => bubbleController;
        set => bubbleController = value;
    }
    public PDInformationPanelManager InformationPanelManager { get => informationPanelManager; set => informationPanelManager = value; }
    public SummaryHandler MidSummary
    {
        get => midSummary;
        set => midSummary = value;
    }
    public SheildUIHander SheildCounter
    {
        get => sheildCounter;
        set => sheildCounter = value;
    }
    public CounterUIHandler SheildUI
    {
        get => sheildUI;
        set => sheildUI = value;
    }
    public PlantdefenceFlowDurations FlowDurations
    {
        get => flowDurations;
        set => flowDurations = value;
    }
    public PDBarFeedbackHandler FeedbackHandler { get => feedbackHandler; set => feedbackHandler = value; }
    #endregion Properties

    #region Methods

    public void AfterMidSummary ()
    {
        PlantDefenceManager.Instance.EnableAllElementsClick ();
    }

    public bool GetTrigger ()
    {
        bool up = nextState;
        nextState = false;
        return up;
    }

    public void GoTOHome ()
    {
        //TODO
    }

    public void GoToNextBubbleState ()
    {
        nextState = true;
    }

    public void HotsoptClicked ()
    {
        ShowHint (4);
        nextState = true;
    }

    public void ResetLevel ()
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }

    public void ShowHint (int index)
    {
        BubbleController.SetHintText (index);
        nextState = true;
    }

    public void StartFirstPhase ()
    {
        nextState = true;
        Invoke (nameof (ShowSheild), FlowDurations.showSheild);
        Invoke (nameof (WaitingForTab), FlowDurations.beforeStartWaitingForTap);
    }

    public void StopWaitingForTab ()
    {
        if (IsInvoking (nameof (CustomUpdateForTab)))
            CancelInvoke (nameof (CustomUpdateForTab));
    }

    public void WaitingForTab ()
    {
        if (IsInvoking (nameof (CustomUpdateForTab)))
            CancelInvoke (nameof (CustomUpdateForTab));
        elpTime = 0;
        if (tapHintCounter <= 1)
            InvokeRepeating (nameof (CustomUpdateForTab), 0, updateRate);
    }

    private void Awake ()
    {
        if (Instance == null)
            Instance = this;
    }

    private void CustomUpdateForTab ()
    {
        elpTime += updateRate;
        if (elpTime >= FlowDurations.waitingForTap)
        {
            print ("Finished Timer");

            if (tapHintCounter == 0)
            {
                PlantDefenceManager.Instance.ShowHotspot ();
                ShowHint (2);
                tapHintCounter++;
            }
            else if (tapHintCounter == 1)
            {
                PlantDefenceManager.Instance.ShowAllHotspots ();
                tapHintCounter++;
            }

            CancelInvoke (nameof (CustomUpdateForTab));
            //ShowHint (3);
            //nextState = true;
            print ("1");
        }
    }

    private void ShowSheild ()
    {
        SheildUI.ShowCounter ();
    }

    // Start is called before the first frame update
    private void Start ()
    {
        AudioManager.Instance?.Play ("bg", "Background");
        Invoke (nameof (StartMachine), 2f);
        informationPanelManager = Instantiate (pdInfoManagerPrefab).GetComponent<PDInformationPanelManager> ();
    }

    private void StartMachine ()
    {
        stateMachine.StartSM ();
    }
    public void CheckFinalSummary ()
    {
        counter++;
        if (counter == 5)
        {
            Invoke (nameof (ShowFinalSummary), 2.5f);
        }
    }
    // Update is called once per frame
    public void ShowFinalSummary ()
    {
        finalSummary.ViewSummary ();
    }

    #endregion Methods
}