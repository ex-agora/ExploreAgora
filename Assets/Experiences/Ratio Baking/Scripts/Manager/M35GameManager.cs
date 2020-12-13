using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class M35GameManager : MonoBehaviour, ITriggable, IMenuHandler
{
    private bool nextState;
    [SerializeField] StateMachineManager stateMachine;
    [SerializeField] SpeechBubbleController bubbleController;
    [SerializeField] List<IngredientsBarButton> ingredientsButtons;
    [SerializeField] Button checkBtn;
    [SerializeField] SummaryHandler finalSummary;
    [SerializeField] ToolBarHandler toolBar;
    static M35GameManager instance;
    M35PlateRatioBaking currentPlate;
    public static M35GameManager Instance { get => instance; set => instance = value; }
    public StateMachineManager StateMachine { get => stateMachine; set => stateMachine = value; }
    public SpeechBubbleController BubbleController { get => bubbleController; set => bubbleController = value; }
    public HashSet<IngredientType> CurrentIngredientTypes { get => currentIngredientTypes; set => currentIngredientTypes = value; }
    public M35PlateRatioBaking CurrentPlate { get => currentPlate; set => currentPlate = value; }

    HashSet<IngredientType> currentIngredientTypes;
    bool checker;
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
    public void FinishExperienceWithStay() { FinishExperiencesHandler.Instance.FinshExperience(3, true); }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void StartMachine()
    {
        StateMachine.StartSM();
    }
    public void TryAgain()
    {
        BubbleController.SetHintText(2);
        nextState = true;
    }
    void ShowBubble() { nextState = true; }
    public void ShowFinalSummery()
    {
        toolBar.CloseToolBar();
        finalSummary.ViewSummary();
    }
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.Play("bg", "Background");
        Invoke(nameof(StartMachine), 2f);
        currentIngredientTypes = new HashSet<IngredientType>();
        
        //PreparePlateIngrdientButton();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PreparePlateIngrdientButton()
    {
        ResetIngrdientButtons();
        //CurrentPlate?.IngredientCounterHandler.HideLabel(CurrentPlate?.IngrdientComponent);
        CurrentPlate = M35Manager.Instance.GetSelectedPlate();
        CurrentPlate.UnlockPlate();
        checkBtn.interactable = false;
        //CurrentPlate?.IngredientCounterHandler.ShowLabel(CurrentPlate?.IngrdientComponent);
        for (int i = 0; i < ingredientsButtons.Count; i++)
        {
            for (int j = 0; j < CurrentPlate.Ingredients.Count; j++)
            {
                if (ingredientsButtons[i].IngredientBarButtonType == CurrentPlate.Ingredients[j].IngredientType)
                {
                    ingredientsButtons[i].gameObject.SetActive(true);
                }
            }
        }
    }
    void ResetIngrdientButtons()
    {
        for (int i = 0; i < ingredientsButtons.Count; i++)
        {
            ingredientsButtons[i].gameObject.SetActive(false);
        }
    }
    public void AddCounter(IngredientsBarButton ingredientsBarButton)
    {
        M35Manager.Instance.AddCounter(ingredientsBarButton.IngredientBarButtonType);
        CurrentIngredientTypes.Add(ingredientsBarButton.IngredientBarButtonType);
        if (CurrentIngredientTypes.Count == CurrentPlate.Ingredients.Count) checkBtn.interactable = true;
        AudioManager.Instance?.Play("placeObject", "Activity");

    }
    public void ResetCounters()
    {
        CurrentPlate.ReloadPlate();
        checkBtn.interactable = false;
    }
    public void CheckRatio()
    {
        checker = false;
        List<float> inputRatio = new List<float>();
        List<float> answerRatio = new List<float>();
        inputRatio = M35Manager.Instance.CheckRatio();
        answerRatio = CurrentPlate.GetAnswerRatios();
        for (int i = 0; i < inputRatio.Count; i++)
        {
            if (inputRatio[i] == answerRatio[i])
            {
                checker = true;
            }
            else
            {
                checker = false;
                break;
            }
        }
        if (checker)
        {
            CorrectRatio();
        }else
        {
            InCorrectRatio();
        }
    }
    void CorrectRatio()
    {
        print("CorrectRatio");
        ResetIngrdientButtons();
        M35Manager.Instance.CloseMixer();
        M35Manager.Instance.IngrdientLabelText.HideLabel();
        CurrentPlate.HideCounter();
        Invoke(nameof(StartMix), 2.6f);
    }
    void StartMix() {
        AudioManager.Instance?.Play("mixer", "Activity");
        Invoke(nameof(MixDone), 19.9f);
    }
    void MixDone() {
        CurrentPlate.ShowResult();
        M35Manager.Instance.OpenMixer();
        Invoke(nameof(HidePreviousComponent), 0.5f);
        PreparePlateIngrdientButton();
        if (M35Manager.Instance.PlateCounter > 2)
        {
            Invoke(nameof(ShowFinalSummery), 4f);

        }
    }
   
    void InCorrectRatio()
    {
        print("InCorrectRatio");
        ResetCounters();
        TryAgain();
    }
    void HidePreviousComponent()
    {
        CurrentPlate?.IngrdientComponent.gameObject.SetActive(false);
    }
    public void ShowUI()
    {
        toolBar.OpenToolBar();
    }
}
