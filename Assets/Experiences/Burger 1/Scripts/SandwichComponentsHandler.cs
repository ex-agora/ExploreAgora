using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SandwichComponentsHandler : MonoBehaviour
{

    #region Fields
    [SerializeField] int timerDuration;
    [SerializeField] int correctCounter, wrongCounter;
    [SerializeField] EOrders orders;
    [SerializeField] ESandwichStages sandwichStages;
    [SerializeField] EBurgerExperineceType burgerExperineceType;
    [SerializeField] List<string> correctAnswers;
    [SerializeField] List<string> playerAnswer;
    [SerializeField] List<GameObject> lastComponent;
    [SerializeField] private TimerUIHandler timerUIHandler;
    [SerializeField] BurgerSandwichScenariosHandler currentFlow;
    private GameObject lastPanel;
    


    int RandomOrder;
    EOrders randomOrder;
    static SandwichComponentsHandler instance;
    #endregion



    #region Properties

    public EBurgerExperineceType BurgerExperineceType { get => burgerExperineceType; set => burgerExperineceType = value; }
    public static SandwichComponentsHandler Instance { get => instance; set => instance = value; }
    public EOrders Orders { get => orders; set => orders = value; }
    public BurgerSandwichScenariosHandler CurrentFlow { get => currentFlow; set => currentFlow = value; }
    public List<string> PlayerAnswer { get => playerAnswer; set => playerAnswer = value; }
    public List<GameObject> LastComponent { get => lastComponent; set => lastComponent = value; }

    public ESandwichStages SandwichStages
    {
        get => sandwichStages;
        set
        {
            sandwichStages = value;
            EnableDisableBackButtonLogic();
        }

    }

    public int CorrectCounter { get => correctCounter; set => correctCounter = value; }
    public int WrongCounter { get => wrongCounter; set => wrongCounter = value; }
    public GameObject LastPanel { get => lastPanel; set => lastPanel = value; }
    #endregion



    #region Standard_Methods

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        DeclareAnswersLists();
        //SetCorrectAnswersForOrders(Orders);
    }
    #endregion



    #region Public_Methods

    public void EnableDisableBackButtonLogic()
    {
        int index = (int)sandwichStages;
        if (index > 0 && index <= 4)
            BurgerWorldUIHandler.Instance.EnableDisableBackButton(true);
        //backButton.interactable = true;
        else
            BurgerWorldUIHandler.Instance.EnableDisableBackButton(false);
        //backButton.interactable = false;
    }

    public void SetCorrectAnswersForOrders(EOrders orders)
    {
        int randomBurger = UnityEngine.Random.Range(1, 3) + 5;
        int randomExtrasAllCases = UnityEngine.Random.Range(1, 4) + 7;
        ESandwichComponents burger = (ESandwichComponents)randomBurger;
        ESandwichComponents extras = (ESandwichComponents)randomExtrasAllCases;
        switch (orders)
        {
            //burger & extras need to be random 
            case EOrders.Regular:

                BurgerWorldUIHandler.Instance.UpdateUI(0, 0, randomBurger - 6, randomExtrasAllCases - 6);
                currentFlow.LockUnLockBehavior(new ESandwichStages[] { ESandwichStages.Bread, ESandwichStages.Cheese }, new int[] { }, new int[] { }, new int[] { 1, 2 }, new int[] { 1, 2 });

                AnswersFilling(
                    ESandwichComponents.Bun1.ToString(),
                    ESandwichComponents.Cheese1.ToString(),
                    burger.ToString(),
                    extras.ToString()
                    );
                break;
            case EOrders.Diabetic:
                int randomExtrasDiabticCase = UnityEngine.Random.Range(2, 4) + 7;
                BurgerWorldUIHandler.Instance.UpdateUI(1, 0, randomBurger - 6, randomExtrasDiabticCase - 6);
                currentFlow.LockUnLockBehavior(new ESandwichStages[] { ESandwichStages.Bread, ESandwichStages.Cheese }, new int[] { 1 }, new int[] { }, new int[] { 2 }, new int[] { 1, 2 });


                extras = (ESandwichComponents)randomExtrasDiabticCase;
                AnswersFilling(
                   ESandwichComponents.Bun2.ToString(),
                   ESandwichComponents.Cheese1.ToString(),
                   burger.ToString(),
                   extras.ToString()
                   );
                break;
            case EOrders.Cholestrol:
                // random bread random burger random extra
                BurgerWorldUIHandler.Instance.UpdateUI(2, 1, randomBurger - 6, randomExtrasAllCases - 6);
                currentFlow.LockUnLockBehavior(new ESandwichStages[] { ESandwichStages.Bread, ESandwichStages.Cheese }, new int[] { }, new int[] { 1 }, new int[] { 2 }, new int[] { 2 });

                AnswersFilling(
                   ESandwichComponents.Bun1.ToString(),
                   ESandwichComponents.Cheese2.ToString(),
                   burger.ToString(),
                   extras.ToString()
                   );
                break;
            case EOrders.Gluten_Free:
                BurgerWorldUIHandler.Instance.UpdateUI(3, 2, randomBurger - 6, randomExtrasAllCases - 6);
                currentFlow.LockUnLockBehavior(new ESandwichStages[] { ESandwichStages.Bread, ESandwichStages.Cheese }, new int[] { 2 }, new int[] { }, new int[] { }, new int[] { 2 });

                AnswersFilling(
                  ESandwichComponents.Bun3.ToString(),
                  ESandwichComponents.Cheese1.ToString(),
                 burger.ToString(),
                    extras.ToString()
                  );
                break;
            case EOrders.Lactose_Intolerance:
                BurgerWorldUIHandler.Instance.UpdateUI(4, 3, randomBurger - 6, randomExtrasAllCases - 6);
                currentFlow.LockUnLockBehavior(new ESandwichStages[] { ESandwichStages.Bread, ESandwichStages.Cheese }, new int[] { }, new int[] { 2 }, new int[] { }, new int[] { });

                AnswersFilling(
                  ESandwichComponents.Bun1.ToString(),
                  ESandwichComponents.Cheese3.ToString(),
                    burger.ToString(),
                    extras.ToString()
                  );
                break;
            default:
                Debug.Log("Something Went Wrong");
                break;

        }
    }

    public void EnableDisableCheckButton()
    {
        for (int i = 0; i < PlayerAnswer.Count; i++)
        {
            if (PlayerAnswer[i] != "")
                BurgerWorldUIHandler.Instance.EnableDisableCheckButton(true);

            else
                BurgerWorldUIHandler.Instance.EnableDisableCheckButton(false);

        }
    }

    public void BackwardBehavior()
    {
        DeletePreviousAnswer();     
        EnableDisableCheckButton();
        BurgerWorldUIHandler.Instance.BackButtonInteractable.enabled = false;
        Invoke(nameof(EnableUndoBtn), 1.4f);
    }
    void EnableUndoBtn()=> BurgerWorldUIHandler.Instance.BackButtonInteractable.enabled = true;
    public void CheckAnswers()
    {

        if (AnswersComparison() == 4)
        {

            ResetSetting();
            EnableDisableCheckButton();
            OrdersHandler();
            SetCorrectAnswersForOrders(Orders);
            AudioManager.Instance?.Play("correctAnswer", "Activity");


            SandwichComponentsBehavior.Instance.ResetPlate();
            SandwichStages = ESandwichStages.Bread;
            SetFlowHolders();

            CorrectCounter++;
            BurgerWorldUIHandler.Instance?.UpdateScoreLockFlowUI();
        }
        else
        {
            //wrongCounter ++ 
            WrongCounter++;
            BurgerWorldUIHandler.Instance?.UpdateScoreLockFlowUIColor(Color.red);
            Invoke(nameof(SetDefaultColor), 2f);
            // oggi message 
            // make sure undo button is on 
        }
        

    }
    void SetDefaultColor()=> BurgerWorldUIHandler.Instance?.UpdateScoreLockFlowUIColor(Color.white);
    public void CheckAnswersForUnLocked()
    {
        if (AnswersComparison() == 4)
        {
            ResetSetting();
            EnableDisableCheckButton();
            CorrectCounter++;

            RandomOrder = UnityEngine.Random.Range(0, 6);
            randomOrder = (EOrders)RandomOrder;
            SetCorrectAnswersForOrders(randomOrder);
            AudioManager.Instance?.Play("correctAnswer", "Activity");
            SandwichComponentsBehavior.Instance.ResetPlate();
            SandwichStages = ESandwichStages.Bread;
            SetFlowHolders();

        }
        else
        {
            WrongCounter++;
            ScreenUIHandler.Instance.onFailure();
            //oggi message
            //back enabled
        }
        ScreenUIHandler.Instance.UpdateUIScore();
    }

    public void OnTimerEndBehavior()
    {
        if (CorrectCounter >= 5)
        {
            //sfx
            FinishExperience();
        }
        else
        {
            ReplayExperienceOnFailure();
        }
    }

    public void ChangeSandwichStagesInHandler(int state)
    {
        int index = (int)SandwichComponentsHandler.Instance.SandwichStages;

        if (state == 1)
        {

            if (index != 4)
                index++;

        }
        else if (state == -1)
        {
            if (index != 0)
                index--;
        }
        else
            Debug.Log(state);
        SandwichComponentsHandler.Instance.SandwichStages = (ESandwichStages)index;

    }

    public void SetFlowHolders()
    {
        CurrentFlow.SetHolders();
    }

    public void StartTimer()
    {
        timerUIHandler.Duration = timerDuration;
        timerUIHandler.ViewBar();
        timerUIHandler.StartTimer();
    }

    public void EndTimer()
    {
        timerUIHandler.HideBar();
       
    }

    public void ReplayExperienceOnFailure()
    {
        ResetSetting();
        RandomOrder = UnityEngine.Random.Range(0, 6);
        randomOrder = (EOrders)RandomOrder;
        SetCorrectAnswersForOrders(randomOrder);
        SandwichComponentsBehavior.Instance.ResetPlate();
        SandwichStages = ESandwichStages.Bread;
        SetFlowHolders();
        ScreenUIHandler.Instance.ResetUI();
        StartTimer();
        Debug.Log("Replay");
    }

    public void StartSummary()
    {
        MatchClothesGameManager.Instance.FinalSummary.ViewSummary();
    }
    #endregion



    #region Private_Methods

    void AnswersFilling(string bread, string cheese, string burger, string extras)
    {
        correctAnswers[0] = bread;
        correctAnswers[1] = cheese;
        correctAnswers[2] = burger;
        correctAnswers[3] = extras;
    }

    void DeclareAnswersLists()
    {
        correctAnswers = new List<string>(3);
        PlayerAnswer = new List<string>(3);
        for (int i = 0; i <= 3; i++)
        {
            correctAnswers.Add("");
            PlayerAnswer.Add("");
        }
    }

    private void DeletePreviousAnswer()
    {
        FadeInOut[] fadeList = null;
        if (SandwichStages == ESandwichStages.Cheese)
        {
            if (playerAnswer[1] != "")
            {
                fadeList= LastComponent[1].GetComponentsInChildren<FadeInOut>();
                for (int i = 0; i < fadeList.Length; i++)
                {
                    fadeList[i].SetFadeAmount(0);
                }
                LastComponent[1].SetActive(false);
                PlayerAnswer[1] = "";
                LastComponent.RemoveAt(1);
            }
            else
            {
                fadeList = LastComponent[0].GetComponentsInChildren<FadeInOut>();
                for (int i = 0; i < fadeList.Length; i++)
                {
                    fadeList[i].SetFadeAmount(0);
                }
                LastComponent[0].SetActive(false);
                PlayerAnswer[0] = "";
                LastComponent.RemoveAt(0);
                ChangeSandwichStagesInHandler(-1);
                SetFlowHolders();
            }
        }
        else if (SandwichStages == ESandwichStages.Burger)
        {
            if (playerAnswer[2] != "")
            {
                fadeList = LastComponent[2].GetComponentsInChildren<FadeInOut>();
                for (int i = 0; i < fadeList.Length; i++)
                {
                    fadeList[i].SetFadeAmount(0);
                }
                LastComponent[2].SetActive(false);
                PlayerAnswer[2] = "";
                LastComponent.RemoveAt(2);
            }
            else
            {
                fadeList = LastComponent[1].GetComponentsInChildren<FadeInOut>();
                for (int i = 0; i < fadeList.Length; i++)
                {
                    fadeList[i].SetFadeAmount(0);
                }
                LastComponent[1].SetActive(false);
                PlayerAnswer[1] = "";
                LastComponent.RemoveAt(1);
                ChangeSandwichStagesInHandler(-1);
                SetFlowHolders();
            }

        }
        else if (SandwichStages == ESandwichStages.Extras)
        {
            if (playerAnswer[3] != "")
            {
                SandwichComponentsBehavior.Instance.CompleteOrderState(false);
                fadeList = LastComponent[3].GetComponentsInChildren<FadeInOut>();
                for (int i = 0; i < fadeList.Length; i++)
                {
                    fadeList[i].SetFadeAmount(0);
                }
                LastComponent[3].SetActive(false);
                PlayerAnswer[3] = "";
                LastComponent.RemoveAt(3);
            }
            else
            {
                fadeList = LastComponent[2].GetComponentsInChildren<FadeInOut>();
                for (int i = 0; i < fadeList.Length; i++)
                {
                    fadeList[i].SetFadeAmount(0);
                }
                LastComponent[2].SetActive(false);
                PlayerAnswer[2] = "";
                LastComponent.RemoveAt(2);
                ChangeSandwichStagesInHandler(-1);
                SetFlowHolders();
            }
        }
        else if (SandwichStages == ESandwichStages.Done) {
            if (playerAnswer[3] != "")
            {
                SandwichComponentsBehavior.Instance.CompleteOrderState(false);
                fadeList = LastComponent[3].GetComponentsInChildren<FadeInOut>();
                for (int i = 0; i < fadeList.Length; i++)
                {
                    fadeList[i].SetFadeAmount(0);
                }
                LastComponent[3].SetActive(false);
                PlayerAnswer[3] = "";
                LastComponent.RemoveAt(3);
                ChangeSandwichStagesInHandler(-1);
                SetFlowHolders();
            }
        }
        else
        {
            Debug.Log(SandwichStages);
        }
    }

    int AnswersComparison()
    {
        int correctCounter = 0;
        for (int i = 0; i < PlayerAnswer.Count; i++)
        {
            Debug.Log(correctAnswers[i] + "  " + PlayerAnswer[i]);
            if (correctAnswers[i] == PlayerAnswer[i])
                correctCounter++;
        }
        return correctCounter;
    }

    void OrdersHandler()
    {
        int index = (int)Orders;
        if (index != 4)
        {
            index++;
            Orders = (EOrders)index;
        }
        else
            FinishExperience();
    }

    void ResetSetting()
    {
        DeclareAnswersLists();
        LastComponent.Clear();
    }

    void FinishExperience()
    {
        Debug.Log("Finished");
        MatchClothesGameManager.Instance.FinalSummary.ViewSummary();
    }

    #endregion









}

