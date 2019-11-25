using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using StateMachine;
public class PhotosynthesisGameManager : MonoBehaviour, ITriggable, IMenuHandler
{
    [SerializeField] List<Draggable> draggables;
    [SerializeField] List<DraggableOnSurface> atoms;
    [SerializeField] SpeechBubbleController bubbleController;
    [SerializeField] StateMachineManager stateMachine;
    [SerializeField] SummaryHandler finalSmallSummary;
    [SerializeField] ToolBarHandler upperBar;
    static PhotosynthesisGameManager instance;

    bool nextState;
    public static PhotosynthesisGameManager Instance { get => instance; set => instance = value; }
    public SummaryHandler FinalSmallSummary { get => finalSmallSummary; set => finalSmallSummary = value; }

    public bool GetTrigger()
    {
        bool up = nextState;
        nextState = false;
        return up;
    }
    public void ShowHint(int index)
    {
        bubbleController.SetHintText(index);
        nextState = true;
    }
    public void GoTOHome()
    {
        //TODO GoHome   
    }
    public void FirstPhase()
    {
        nextState = true;
        EnableDisableDraggable();
    }
    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        stateMachine.StartSM();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void EnableDisableDraggable()
    {
        for (int i = 0; i < draggables.Count; i++)
        {
            draggables[i].enabled = !draggables[i].enabled;
        }
        for (int i = 0; i < atoms.Count; i++)
        {
            atoms[i].enabled = !atoms[i].enabled;
        }
    }

    private void ShowBar()
    {
        upperBar.OpenToolBar();
    }
    void Exploring()
    {
        bubbleController.HideBubble();
        nextState = true;
        Invoke(nameof(ShowBar), 1f);
    }
    public void StartExplore()
    {
        Invoke(nameof(Exploring), 1f);
    }
}
