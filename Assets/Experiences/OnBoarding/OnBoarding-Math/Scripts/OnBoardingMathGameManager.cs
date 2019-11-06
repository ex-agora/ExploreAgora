using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnBoardingMathGameManager : MonoBehaviour, ITriggable, IMenuHandler
{
    public enum Phases
    {
        FirstPhase,
        SecondPhase
    }
    public Phases phases;

    private bool nextState;
    static OnBoardingMathGameManager instance;
    [SerializeField] StateMachineManager stateMachine;
    [SerializeField] ToolBarHandler barHandler;
    [SerializeField] TutorialPanelController tutorial;

    [SerializeField] Transform[] HotSpots;
    [SerializeField] List<Transform> hotSpotsPivots;

    [SerializeField] RuntimeAnimatorController[] animators;

    [SerializeField] SpeechBubbleController bubbleController;

    public static OnBoardingMathGameManager Instance { get => instance; set => instance = value; }

    public GameObject canvas;
    public RectTransform canvasRect; 
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

    }

    private void Start()
    {
        //AudioManager.Instance.Play("bg", "Background");
        //Invoke(nameof(StartMachine), 2f);
    }

    #region PreDefined_Methods
    private void StartMachine()
    {
        stateMachine.StartSM();
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

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion






    #region Private_Methods

    void StartTutorial()
    {
        tutorial.OpenTutorial();
    }


    #endregion

    #region public_Methods


    public void showFooterPanel()
    {
        barHandler.OpenToolBar();
    }


    public void showCommands()
    {

        nextState = true;
        if (phases == Phases.FirstPhase)
        {
            foreach (var item in hotSpotsPivots)
            {
                if (item.name.Contains("Powder"))
                {
                    canvas = Instantiate(HotSpots[1].gameObject, Vector3.zero, HotSpots[1].rotation, item);
                }

            }
            tutorial.GetComponent<Animator>().runtimeAnimatorController = animators[0];
        }
        else if (phases == Phases.SecondPhase)
        {
            foreach (var item in hotSpotsPivots)
            {
                if (item.name.Contains("Snapping"))
                {
                    canvas = Instantiate(HotSpots[0].gameObject, Vector3.zero, HotSpots[0].rotation, item);
                }

            }

            tutorial.GetComponent<Animator>().runtimeAnimatorController = animators[1];

        }
        canvasRect = canvas.GetComponent<RectTransform>();
        canvasRect.localPosition = new Vector3(0, 0.04f, 0);
    }


    public void Tutorial()
    {
        Invoke(nameof(StartTutorial), 2f);
    }



    public void AddHotSpotPivots(Transform pivot)
    {
        hotSpotsPivots.Add(pivot);
    }


    public void HideFinishedHotSpot()
    {
        if (phases == Phases.FirstPhase)
        {

            foreach (var item in hotSpotsPivots)
            {
                if (item.name.Contains("Powder"))
                    item.gameObject.SetActive(false);
            }
        }
        else if (phases == Phases.SecondPhase)
        {
            foreach (var item in hotSpotsPivots)
            {
                if (item.name.Contains("Snapping"))
                    item.gameObject.SetActive(false);
            }
        }

    }
    #endregion


    public void MoveToSecondPhases()
    {
        phases = Phases.SecondPhase;

    }

    public void hideFooterPanel()
    {
        barHandler.CloseToolBar();
    }

}
