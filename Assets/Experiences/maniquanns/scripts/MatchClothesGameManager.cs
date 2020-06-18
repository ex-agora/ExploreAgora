using StateMachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchClothesGameManager : MonoBehaviour, ITriggable, IMenuHandler
{

    static MatchClothesGameManager instance;

    SummaryHandler currentSummary;
    public static MatchClothesGameManager Instance { get => instance; set => instance = value; }
    public SummaryHandler FinalSummary { get => finalSummary; set => finalSummary = value; }
    public TryAgainHandler TryAgain { get => tryAgain; set => tryAgain = value; }
    public bool NextState { get => nextState; set => nextState = value; }
    public SpeechBubbleController BubbleController { get => bubbleController; set => bubbleController = value; }

    private bool nextState;
    [SerializeField] StateMachineManager stateMachine;
    [SerializeField] TutorialPanelController tutorial;
    [SerializeField] SummaryHandler finalSummary;
   
    [SerializeField] SpeechBubbleController bubbleController;
    [SerializeField] TryAgainHandler tryAgain;
    FadeInOut fadeinout;
    [SerializeField] MenuUIHandler menu;
    [SerializeField] GameEvent onTutorialReadyPressed;
    [SerializeField] BodyPartsDistanceAlertHandler bodyPartsDistanceAlertHandler;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
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
        bool up = NextState;
        NextState = false;
        return up;
    }

    public void GoTOHome()
    {
        FinishExperiencesHandler.Instance.GotoHome();
    }
    public void FinishExperience()
    {
        FinishExperiencesHandler.Instance.FinshExperience(3);
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion




    void StartTutorial()
    {
        tutorial.TutorialTextStr = BubbleController.NextBubble();
        tutorial.OpenTutorial();
    }
    public void Tutorial()
    {
        Invoke(nameof(StartTutorial), 1f);
    }


   public void StartDistanceHandler()
    {
        BodyPartsDistanceAlertHandler[] b = GameObject.FindObjectsOfType<BodyPartsDistanceAlertHandler>();
        b[0].StartDistanceAlert();
    }

    public void FininshTutorial()
    {
        onTutorialReadyPressed.Raise();
    }

    public void TryAgainBubble()
    {
        tryAgain.ShowBubble(); 
        Invoke(nameof(CloseTryAgain), 2);
    }

    public void CloseTryAgain()
    {
        tryAgain.HideBubble();
    }

    public void FarAlert()
    {
        BubbleController.SetHintText(6);
        NextState = true;
    }

    public void NearAlert()
    {
        BubbleController.SetHintText(5);
        NextState = true;
    }


    public void FinalSummaries()
    {
        currentSummary.ViewSummary();    
    }
    public void OpenSummaries(SummaryHandler s)
    {
        currentSummary = s;
        Invoke(nameof(FinalSummaries), 2);
    }
}
