using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlantDefenceGameManager : MonoBehaviour, ITriggable, IMenuHandler
{
     bool nextState;
    [SerializeField] SpeechBubbleController bubbleController;
    [SerializeField] StateMachineManager stateMachine;
    [SerializeField] PDInformationPanelManager informationPanelManager;
    [SerializeField] SummaryHandler midSummary;
    static PlantDefenceGameManager instance;
    public PlantdefenceFlowDurations flowDurations;
    public static PlantDefenceGameManager Instance { get => instance; set => instance = value; }
    public CounterUIHandler SheildUI { get => sheildUI; set => sheildUI = value; }
    public SheildUIHander SheildCounter { get => sheildCounter; set => sheildCounter = value; }
    public Animator BubbleAnimator { get => bubbleAnimator; set => bubbleAnimator = value; }
    public SpeechBubbleController BubbleController { get => bubbleController; set => bubbleController = value; }
    public PDInformationPanelManager InformationPanelManager { get => informationPanelManager; set => informationPanelManager = value; }
    public SummaryHandler MidSummary { get => midSummary; set => midSummary = value; }

    float elpTime;
    float updateRate = 0.01f;
    int tapHintCounter = 0;
    [SerializeField] CounterUIHandler sheildUI;
    [SerializeField] SheildUIHander sheildCounter;
    [SerializeField] Animator bubbleAnimator;

    private void Awake ()
    {
        if ( Instance == null )
            Instance = this;
    }

    // Start is called before the first frame update
    void Start ()
    {
        AudioManager.Instance?.Play("bg", "Background");
        Invoke (nameof (StartMachine) , 2f);
    }

    // Update is called once per frame
    void Update ()
    {

    }
    private void StartMachine ()
    {
        stateMachine.StartSM ();
    }
    public bool GetTrigger ()
    {
        bool up = nextState;
        nextState = false;
        return up;
    }
    public void GoToNextBubbleState ()
    {
        nextState = true;
    }

    public void ResetLevel ()
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }

    public void GoTOHome ()
    {
        //TODO 
    }
    void ShowSheild ()
    {
        SheildUI.ShowCounter ();
    }
    public void StartFirstPhase ()
    {
        nextState = true;
        Invoke (nameof (ShowSheild) , flowDurations.showSheild);
        Invoke (nameof (WaitingForTab) , flowDurations.beforeStartWaitingForTap);
    }
    public void WaitingForTab ()
    {
        CancelInvoke (nameof (CustomUpdateForTab));
        elpTime = 0;
        InvokeRepeating (nameof (CustomUpdateForTab) , 0 , updateRate);
    }
    public void StopWaitingForTab ()
    {
        CancelInvoke (nameof (CustomUpdateForTab));
    }
    void CustomUpdateForTab ()
    {
        elpTime += updateRate;
        if (elpTime >= flowDurations.waitingForTap)
        {
            print("Finished Timer");

            if (tapHintCounter == 0)
            {
                PlantDefenceManager.Instance.ShowHotspot();
                ShowHint(2);
            }
            else if (tapHintCounter == 1)
            {
                PlantDefenceManager.Instance.ShowAllHotspots();
            }
            tapHintCounter++;

            CancelInvoke(nameof(CustomUpdateForTab));
            //ShowHint (3);
            //nextState = true;
            print("1");

        }
    }
    public void ShowHint (int index)
    {
        BubbleController.SetHintText (index);
        nextState = true;
    }
    public void AfterMidSummary ()
    {
        PlantDefenceManager.Instance.EnableAllElementsClick ();
    }

    public void HotsoptClicked() {
        ShowHint(4);
        nextState = true;
    }

}
