using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DanielLochner.Assets.SimpleScrollSnap;
using System;

public class CamouflageGameManger : MonoBehaviour, ITriggable, IMenuHandler
{

    #region Fields

    private bool nextState;
    static CamouflageGameManger instance;
    [SerializeField] SpeechBubbleController bubbleController;
    [SerializeField] ToolBarHandler toolBarHandler;
    [SerializeField] GameEvent onTutorialReadyPressed;
    [SerializeField] TutorialPanelController tutorial;
    [SerializeField] float mothCurrentRatio, currentComparisonRatio;
    [SerializeField] int score, currentIndex;
    [SerializeField] StateMachineManager stateMachine;
    [SerializeField] Button hideButton;
    [SerializeField] Text Counter;
    [SerializeField] List<RuntimeAnimatorController> resultScenarios;
    [SerializeField] List<GameObject> mothPanels;
    [SerializeField] Transform mothPanelContent;
    [SerializeField] Text testText;
    [SerializeField] SummaryHandler finalSummary;
    [SerializeField] SummaryHandler scoreSummary;
    [SerializeField] Text wellHiddenText;
    [SerializeField] Text easyToSpotText;
    [SerializeField] Text exposedText;
    int wellHiddenCounter;
    int easyToSpotCounter;
    int exposedCounter;
    #endregion


    #region Properties
    public static CamouflageGameManger Instance { get => instance; set => instance = value; }

    public float MothCurrentRatio { get => mothCurrentRatio; set => mothCurrentRatio = value; }
    public int CurrentIndex { get => currentIndex; set => currentIndex = value; }
    public int Score
    {
        get => score;
        set
        {
            score = value;
            SetCurrentComparisonRatio();

        }
    }
    #endregion


    #region standard_Methods

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Score = 0;
        wellHiddenCounter = 0;
        easyToSpotCounter = 0;
        exposedCounter = 0;
        AudioManager.Instance.Play("bg", "Background");
        Invoke(nameof(StartMachine), 2f);
        FillingMothPanelContents();
        toolBarHandler.OpenToolBar();
        Invoke(nameof(StartTutorial), 10f);
    }

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
        FinishExperiencesHandler.Instance.GotoHome();
    }
    public void FinishExperience()
    {
        int gem = 0;
        if (wellHiddenCounter >= 4) gem = 3;
        else if (wellHiddenCounter == 3) gem = 2;
        else  gem = 1;
        FinishExperiencesHandler.Instance.FinshExperience(gem);
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion


    #region Public_Mehtod

    public void SetCurrentComparisonRatio()
    {
        if (Score <= 5)
            Counter.text = score + "";
        //switch (Score)
        //{
        //    case 0:
        //        currentComparisonRatio = 0.1f;
        //        break;
        //    case 1:
        //        currentComparisonRatio = 0.3f;
        //        break;
        //    case 2:
        //        currentComparisonRatio = 0.7f;
        //        break;
        //    case 3:
        //        currentComparisonRatio = 0.2f;
        //        break;
        //    case 4:
        //        currentComparisonRatio = 0.2f;
        //        break;
        //    case 5:
        //        Debug.LogError("Show Final dsaasd");
        //        toolBarHandler.CloseToolBar();
        //        Invoke(nameof(FinalSummary), 4);
        //        break;
        //    default:
        //        Debug.LogError("fe 7aga 8lt");
        //        break;
        //}
        if(Score == 5)
        {
            Debug.LogError("Show Final dsaasd");
            Invoke(nameof(HideBar), 3f);
        }
    }
    void HideBar() {
        toolBarHandler.CloseToolBar();
        Invoke(nameof(ShowScore), 2f);
    }
    void ShowScore() {
        wellHiddenText.text = wellHiddenCounter.ToString();
        easyToSpotText.text = easyToSpotCounter.ToString();
        exposedText.text = exposedCounter.ToString();
        scoreSummary.ViewSummary();
    }
    public void SetMothCurrentRatio()
    {
        MothCurrentRatio = mothPanels[CurrentIndex].GetComponent<MothColorRatioHandler>().MothCurrentRatio;
    }

    public void CompareBetweenRatios()
    {
        AudioManager.Instance?.Play("UIAction", "UI");
        Debug.LogError(mothPanels[CurrentIndex].name  + "   " + CurrentIndex);
        mothPanels[CurrentIndex].transform.GetChild(3).GetComponent<Image>().enabled = true;
        mothPanels[CurrentIndex].GetComponent<ColorChecker>().ChackColor(); 
    }
    public void CompareResult(float p,Color currentColor) {
        mothCurrentRatio = p;
        testText.text = mothCurrentRatio.ToString();
        AudioManager.Instance.Play("stamp", "Activity");
        if (mothCurrentRatio >= 0.92f && mothCurrentRatio < 1)
        {
            mothPanels[CurrentIndex].GetComponentInChildren<Animator>().runtimeAnimatorController = resultScenarios[2];
            wellHiddenCounter++;
        }
        else if (mothCurrentRatio >= 0.85f && mothCurrentRatio < 0.919f)
        {
            mothPanels[CurrentIndex].GetComponentInChildren<Animator>().runtimeAnimatorController = resultScenarios[0];
            easyToSpotCounter++;
        }
        else if (mothCurrentRatio < 0.85f)
        {
            mothPanels[CurrentIndex].GetComponentInChildren<Animator>().runtimeAnimatorController = resultScenarios[1];
            exposedCounter++;
        }
        mothPanels[CurrentIndex].GetComponentInChildren<Animator>().SetTrigger("play");
        mothPanels[CurrentIndex].GetComponent<MothColorRatioHandler>().AlreadyPlayed = true;
        mothPanels[CurrentIndex].transform.GetChild(0).GetComponent<Image>().enabled = false;
        mothPanels[CurrentIndex].transform.GetChild(1).GetComponent<Image>().enabled = true;
        mothPanels[CurrentIndex].transform.GetChild(1).GetComponent<Image>().color = currentColor;
        hideButton.interactable = false;
        Score++;
    }
    public void endingTutorial()
    {
        onTutorialReadyPressed?.Raise();
    }
    public void SetCurrentIndex()
    {
        CurrentIndex = SimpleScrollSnap.Instance.TargetPanel;
    }

    public void CheckIfAlreadyPlayed()
    {
        bool isPlayed = mothPanels[CurrentIndex].GetComponent<MothColorRatioHandler>().AlreadyPlayed;
        if (isPlayed)
            hideButton.interactable = false;
        else
            hideButton.interactable = true;
    }
    #endregion

    #region Private_Methods
    private void FillingMothPanelContents()
    {
        for (int i = 0; i < mothPanelContent.childCount; i++)
        {
            mothPanels.Add(mothPanelContent.GetChild(i).gameObject);
        }
    }

    void StartTutorial()
    {
        tutorial.TutorialTextStr = bubbleController.NextBubble();
        tutorial.OpenTutorial();
    }

    public void FinalSummary()
    {
        Invoke(nameof(ShowFinalSummery), 1.3f);
    }
    void ShowFinalSummery() {
        finalSummary.ViewSummary();
    }
    
    #endregion


}
