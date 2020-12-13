using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SS17GameManager : MonoBehaviour, ITriggable, IMenuHandler
{
    #region singletone
    static SS17GameManager instance;
    public static SS17GameManager Instance { get => instance; set => instance = value; }
    //public SS17UpperButtonHandler BarHandler { get => barHandler; set => barHandler = value; }
    public SummaryHandler FinalSummary { get => finalSummary; set => finalSummary = value; }
    public SS17UpperButtonHandler Diplomatic { get => diplomatic; set => diplomatic = value; }
    public SS17UpperButtonHandler Trade { get => trade; set => trade = value; }
    public SS17UpperButtonHandler Colonization { get => colonization; set => colonization = value; }
    public Button MatchButton { get => matchButton; set => matchButton = value; }
    public bool IsTutorialFinished { get => isTutorialFinished; set => isTutorialFinished = value; }
    public SummaryHandler MidSummary { get => midSummary; set => midSummary = value; }
    public Animator MidSummaryAnimator { get => midSummaryAnimator; set => midSummaryAnimator = value; }
    public bool AllSuumariesFinished { get => allSuumariesFinished; set => allSuumariesFinished = value; }
    #endregion
    #region Fields
    //[SerializeField]SS17UpperButtonHandler barHandler;

    private bool nextState;
    bool isTutorialFinished;
    bool allSuumariesFinished;
    [SerializeField] Button matchButton;
    [SerializeField] Animator midSummaryAnimator;
    [SerializeField] SS17UpperButtonHandler colonization;
    [SerializeField] SS17UpperButtonHandler trade;
    [SerializeField] SS17UpperButtonHandler diplomatic;
    [SerializeField] SummaryHandler finalSummary;
    [SerializeField] SummaryHandler midSummary;
    [SerializeField] StateMachineManager stateMachine;
    [SerializeField] SpeechBubbleController bubbleController;
    [SerializeField] MenuUIHandler menu;
    [SerializeField] ToolBarHandler toolBar;
    [SerializeField] TutorialPanelController tutorial;
    [SerializeField] ToolBarHandler gotItBtn;
    #endregion Fields

    #region Methods
    public bool GetTrigger ()
    {
        bool up = nextState;
        nextState = false;
        return up;
    }

    public void GoTOHome ()
    {
        FinishExperiencesHandler.Instance.GotoHome ();
    }
    public void FinishExperience ()
    {
        FinishExperiencesHandler.Instance.FinshExperience (3);
    }
    public void FinishExperienceWithStay () { FinishExperiencesHandler.Instance.FinshExperience (3 , true); }

    public void ResetLevel ()
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }

    private void Awake ()
    {
        if ( Instance == null )
            Instance = this;
    }
    private void Start ()
    {
        AudioManager.Instance.Play ("bg" , "Background");
        Invoke (nameof (StartMachine) , 2f);
    }
    private void StartMachine ()
    {
        stateMachine.StartSM ();
    }
    void StopMenuInteraction ()
    {
        bubbleController.StopSpeech ();
        menu.StopMenuInteraction ();
    }
    void StartMenuInteraction ()
    {
        bubbleController.RunSpeech ();
        menu.StopMenuInteraction ();
    }
    public void FarAlert ()
    {
        bubbleController.SetHintText (3);
        nextState = true;
    }
    public void NearAlert ()
    {
        bubbleController.SetHintText (4);
        nextState = true;
    }
    public void TryAgain ()
    {
        bubbleController.SetHintText (2);
        nextState = true;
    }
    public void StartFirstPhase ()
    {
        toolBar.OpenToolBar ();
        StartTutorial ();
    }
    void ShowBubble () { nextState = true; }
    void StartTutorial ()
    {
        tutorial.TutorialTextStr = bubbleController.NextBubble ();
        tutorial.OpenTutorial ();
    }
    public void Match ()
    {
        SS17Manager.Instance.Match ();
    }
    public void ShowGotiTBtn ()
    {
        Invoke (nameof (OpenGotItBtn) , 1f);
    }
    void OpenGotItBtn ()
    {
        if ( !AllSuumariesFinished )
            gotItBtn.OpenToolBar ();
        //draggingManager.SetActive (true);
    }
    public void CloseGotItBtn ()
    {
        gotItBtn.CloseToolBar ();
    }
    public void ShowFinalSummery ()
    {
        //draggingManager.SetActive (false);
        //Invoke (nameof (OpenSummery) , 4f);
        if ( AllSuumariesFinished )
            FinalSummary.ViewSummary ();
    }
    public void OpenMidSummery (SS17MidSummary summary)
    {
        midSummary.ContentSprite = summary.FirstFrame;
        midSummaryAnimator.runtimeAnimatorController = summary.AnimatorController;
        MidSummary.ViewSummary ();
    }
    public void ShowHint (int index)
    {
        bubbleController.SetHintText (index);
        nextState = true;
    }
    #endregion Methods
}
