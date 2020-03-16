using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using StateMachine;
public class PhotosynthesisGameManager : MonoBehaviour, ITriggable, IMenuHandler
{
    List<Lean.Touch.LeanDragTranslate> draggables; 
    List<DraggableOnSurface> atoms; 
    [SerializeField] SpeechBubbleController bubbleController;
    [SerializeField] StateMachineManager stateMachine;
    [SerializeField] SummaryHandler finalSmallSummary;
    [SerializeField] DistanceHandler distanceHandler;
    [SerializeField] Button sunHint;
    [SerializeField] Button cloudHint;
    [SerializeField] Button airHint;
    [SerializeField] Image nightImageEffect;
    [SerializeField] ToolBarHandler upperBar;
    [SerializeField] MenuUIHandler menu;
    [SerializeField] GameObject draggingHandle;
    [SerializeField] RealTimeTutorialHandler rTTHand;
    static PhotosynthesisGameManager instance;

    bool nextState;
    public static PhotosynthesisGameManager Instance { get => instance; set => instance = value; }
    public SummaryHandler FinalSmallSummary { get => finalSmallSummary; set => finalSmallSummary = value; }
    public Button SunHint { get => sunHint; set => sunHint = value; }
    public Button CloudHint { get => cloudHint; set => cloudHint = value; }
    public Button AirHint { get => airHint; set => airHint = value; }
    public Image NightImageEffect { get => nightImageEffect; set => nightImageEffect = value; }
    public List<Lean.Touch.LeanDragTranslate> Draggables { get => draggables; set => draggables = value; }
    public List<DraggableOnSurface> Atoms { get => atoms; set => atoms = value; }

    public bool GetTrigger ()
    {
        bool up = nextState;
        nextState = false;
        return up;
    }
    public void ShowHint (int index)
    {
        bubbleController.SetHintText (index);
        nextState = true;
    }
    public void GoTOHome ()
    {
        //TODO GoHome   
    }
    void FirstPhase () {
        nextState = true;
        Invoke(nameof(ShowUpperBar), 5.3f);
        EnableDisableDraggable ();
    }
    void HideRTTHand() {
        rTTHand.CloseIndicator();
        draggingHandle.SetActive(true);
    }
    void ShowRTTHand() {
        rTTHand.OpenIndicator();
        Invoke(nameof(HideRTTHand), 6.4f);
    }
    void ShowUpperBar() {
        upperBar.OpenToolBar();
        Invoke(nameof(ShowRTTHand), 2f);
    }
    public void ResetLevel ()
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }

    private void Awake ()
    {
        if ( Instance == null )
            Instance = this;
    }
    // Start is called before the first frame update
    void Start ()
    {
        AudioManager.Instance.Play("bg", "Background");
        Invoke(nameof(StartMachine), 2f);
    }
    private void StartMachine()
    {
        stateMachine.StartSM();
    }
   
    void EnableDisableDraggable ()
    {
        print(Draggables.Count + " " + Atoms.Count);
        for (int i = 0; i < Draggables.Count; i++)
        {
            Draggables[i].enabled = !Draggables[i].enabled;
        }
        for (int i = 0; i < Atoms.Count; i++)
        {
            Atoms[i].enabled = !Atoms[i].enabled;
        }
    }
    public void StartSummary ()
    {
        StopMenuAndBubble();
        FinalSmallSummary.ViewSummary ();
    }
    void StopMenuAndBubble() {
        bubbleController.StopSpeech();
        menu.StopMenuInteraction();
    }
    public void CorrectDistanceFlow ()
    {
        //distanceHandler.enabled = false;
        FirstPhase ();
        //AudioManager.Instance.Play ("rain" , "Activity");
    }
    public void UIClicked() {
        AudioManager.Instance?.Play("UIAction", "UI");
    }
}
