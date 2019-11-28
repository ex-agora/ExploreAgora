using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using StateMachine;
public class PhotosynthesisGameManager : MonoBehaviour, ITriggable, IMenuHandler
{
    List<Draggable> draggables; 
    List<DraggableOnSurface> atoms; 
    [SerializeField] SpeechBubbleController bubbleController;
    [SerializeField] StateMachineManager stateMachine;
    [SerializeField] SummaryHandler finalSmallSummary;
    [SerializeField] DistanceHandler distanceHandler;
    [SerializeField] Button sunHint;
    [SerializeField] Button cloudHint;
    [SerializeField] Button airHint;
    [SerializeField] Image nightImageEffect;
    static PhotosynthesisGameManager instance;

    bool nextState;
    public static PhotosynthesisGameManager Instance { get => instance; set => instance = value; }
    public SummaryHandler FinalSmallSummary { get => finalSmallSummary; set => finalSmallSummary = value; }
    public Button SunHint { get => sunHint; set => sunHint = value; }
    public Button CloudHint { get => cloudHint; set => cloudHint = value; }
    public Button AirHint { get => airHint; set => airHint = value; }
    public Image NightImageEffect { get => nightImageEffect; set => nightImageEffect = value; }
    public List<Draggable> Draggables { get => draggables; set => draggables = value; }
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
        EnableDisableDraggable ();
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
        stateMachine.StartSM ();
    }

    // Update is called once per frame
    void Update ()
    {

    }
    void EnableDisableDraggable ()
    {
        //print (Draggables.Count +  " " + Atoms.Count);
        //for ( int i = 0 ; i < Draggables.Count ; i++ )
        //{
        //    Draggables [i].enabled = !Draggables [i].enabled;
        //}        
        //for ( int i = 0 ; i < Atoms.Count ; i++ )
        //{
        //    Atoms [i].enabled = !Atoms [i].enabled;
        //}
    }
    public void StartSummary ()
    {
        FinalSmallSummary.ViewSummary ();
    }
    public void CorrectDistanceFlow ()
    {
        distanceHandler.enabled = false;
        FirstPhase ();
        //AudioManager.Instance.Play ("rain" , "Activity");
    }
}
