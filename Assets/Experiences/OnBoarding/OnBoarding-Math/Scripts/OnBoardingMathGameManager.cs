using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnBoardingMathGameManager : MonoBehaviour, ITriggable, IMenuHandler
{
    public enum Phases
    {
        FirstPhase,
        SecondPhase
    }
    public Phases phases;
    public Button TestButton;
    private bool nextState;
    static OnBoardingMathGameManager instance;
    public static OnBoardingMathGameManager Instance { get => instance; set => instance = value; }

    [SerializeField] StateMachineManager stateMachine;
    [SerializeField] Texture bookPuzzleTex;
    [SerializeField] Texture bookCurrentTex;
    [SerializeField] ToolBarHandler barHandler;
    [SerializeField] TutorialPanelController tutorial;
    [SerializeField] SummaryHandler finalSummary;
    [SerializeField] RecapImgHandler imgHandler;
    [SerializeField] Transform[] HotSpots;
    [SerializeField] List<Transform> hotSpotsPivots;
    [SerializeField] List<OldDragable> Draggables;

    [SerializeField] RuntimeAnimatorController[] animators;
    [SerializeField] Transform powderParticle, bookParticle;
    [SerializeField] SpeechBubbleController bubbleController;
    [SerializeField] MenuUIHandler menu;
    [SerializeField] GameEvent onTutorialReadyPressed;
    [SerializeField] GameObject dragHnadler;
    public int score;
    GameObject canvas;
    RectTransform canvasRect;
    public GameObject powderParticleTemp, bookParticleTemp;


    public Animator BookAnimator;
    public Transform powderParticleParent, bookParticleParent;
    public Material bookMat;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void OnDisable()
    {
        bookMat.EnableKeyword("_Albedo");
        bookMat.SetTexture("_Albedo", bookCurrentTex);
    }
    private void Start()
    {
        AudioManager.Instance.Play("bg", "Background");
        Invoke(nameof(StartMachine), 2f);
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
        tutorial.TutorialTextStr = bubbleController.NextBubble();
        tutorial.OpenTutorial();
    }

    void hideParticle()
    {
        powderParticleParent.gameObject.SetActive(false);
    }
    #endregion

    #region public_Methods

    public void endingTutorial()
    {
        onTutorialReadyPressed?.Raise();
    }

    public void showFooterPanel()
    {
        barHandler.OpenToolBar();
    }

    public void tutorialSteps()
    {
        if (phases != Phases.FirstPhase)
            return;
        else
        {
            showFooterPanel();
        }
    }

    public void changeCommandText(float delay)
    {
        nextState = true;
    }
    public void showCommands()
    {
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

    public void AddDraggables(OldDragable d)
    {
        Draggables.Add(d);
    }


    public void activateDeactivateDraggables(bool state)
    {
        dragHnadler.SetActive(true);
        Debug.Log("QQWWWSSSZZZ");
        foreach (var item in Draggables)
        {

        Debug.Log("QQWWWSSSZZZ 0000");
            item.enabled = state;
        }
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




    public void MoveToSecondPhases()
    {
        //play Particle
        powderParticleTemp = Instantiate(powderParticle.gameObject, powderParticle.position, powderParticle.rotation, powderParticleParent);
        powderParticleTemp.transform.localPosition = Vector3.zero;
        powderParticle.GetComponent<ParticleSystem>().Play();
        phases = Phases.SecondPhase;
        //play swoosh sound
        AudioManager.Instance.Play("swoosh", "Activity");
        AudioManager.Instance.Play("placeObject", "Activity");
        //unlock Dragging 
        bookMat.EnableKeyword("_Albedo");
        bookMat.SetTexture("_Albedo", bookPuzzleTex);
        Invoke(nameof(hideParticle), 2);
        TestButton.interactable = true;
    }

    public void hideFooterPanel()
    {
        barHandler.CloseToolBar();
    }
    public void checkAnswer()
    {
        if (score != 3)
            return;
        else
        {

            //bookParticleTemp = Instantiate(bookParticle.gameObject, Vector3.zero, bookParticle.rotation, bookParticleParent);
            //bookParticleTemp.transform.localPosition = Vector3.zero;
            //bookParticleTemp.GetComponent<ParticleSystem>().Play();
            //play last sound 
            AudioManager.Instance.Play("openLock", "Activity");
            BookAnimator.SetTrigger("openClip");
            //final summary 
            Invoke(nameof(FinalSummary), 12f);
            //finalSummary.ViewSummary();
            Invoke(nameof(StartAnim), 2f);
        }
    }
    void StartAnim()
    {
        imgHandler.PlayAnimation();
    }

    void FinalSummary()
    {
        bubbleController.StopSpeech();
        menu.StopMenuInteraction();
        finalSummary.ViewSummary();
    }
    //public void testttt()
    //{
    //    AudioManager.Instance.Play("openLock", "Activity");
    //    BookAnimator.SetTrigger("openClip");
    //    Invoke(nameof(FinalSummary), 6);
    //    Invoke(nameof(StartAnim), 2f);
    //    //bookParticleTemp = Instantiate(bookParticle.gameObject, Vector3.zero, bookParticle.rotation, bookParticleParent);
    //    //ookParticleTemp.transform.localPosition = Vector3.zero;
    //    //bookParticleTemp.GetComponent<ParticleSystem>().Play();
    //}

    #endregion

}
