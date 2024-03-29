﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using StateMachine;
using UnityEngine.UI;

public class MSS132GameManager : MonoBehaviour, ITriggable, IMenuHandler
{
    #region singletone
    static MSS132GameManager instance;
    public static MSS132GameManager Instance { get => instance; set => instance = value; }
    public MSS132BarHandler BarHandler { get => barHandler; set => barHandler = value; }
    public SummaryHandler Summary { get => summary; set => summary = value; }
    #endregion
    #region Fields
    [SerializeField] MSS132BarHandler barHandler;

    private bool nextState;
    [SerializeField] GameEvent firstPhaseEvent = null;
    [SerializeField] SummaryHandler summary;
    [SerializeField] StateMachineManager stateMachine;
    [SerializeField] SpeechBubbleController bubbleController;
    [SerializeField] MenuUIHandler menu;
    [SerializeField] ToolBarHandler toolBar;
    [SerializeField] TutorialPanelController tutorial;
    [SerializeField] GameObject draggingManager;
    [SerializeField] ToolBarHandler gotItBtn;
    #endregion Fields

    #region Methods
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
        FinishExperiencesHandler.Instance.FinshExperience(3);
    }
    public void FinishExperienceWithStay() { FinishExperiencesHandler.Instance.FinshExperience(3,true); }

    public void ResetLevel()
    {
        FinishExperiencesHandler.Instance.Reload();
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        AudioManager.Instance.Play("bg", "Background");
        Invoke(nameof(StartMachine), 2f);
    }
    private void StartMachine()
    {
        stateMachine.StartSM();
    }
    void StopMenuInteraction()
    {
        bubbleController.StopSpeech();
        menu.StopMenuInteraction();
    }
    void StartMenuInteraction()
    {
        bubbleController.RunSpeech();
        menu.StopMenuInteraction();
    }
    public void FarAlert()
    {
        bubbleController.SetHintText(4);
        nextState = true;
    }
    public void NearAlert()
    {
        bubbleController.SetHintText(5);
        nextState = true;
    }
    public void StartFirstPhase() {
        toolBar.OpenToolBar();
        Invoke(nameof(PlantAnim), 5f);
    }
    void PlantAnim() {
        firstPhaseEvent?.Raise();
        barHandler.ActiveDying();
        Invoke(nameof(ShowBubble), 2.7f);
        Invoke(nameof(StartTutorial), 10f);
    }
    void ShowBubble() { nextState = true; }
    void StartTutorial() {
        tutorial.TutorialTextStr = bubbleController.NextBubble();
        tutorial.OpenTutorial();
    }
    public void ContinueFirstPhase() {
        draggingManager.SetActive(true);

    }
    
    public void ShowGotiTBtn() {
        Invoke(nameof(OpenGotItBtn), 1f);
    }
    void OpenGotItBtn() {
        gotItBtn.OpenToolBar();
        draggingManager.SetActive(true);
    }
    public void ShowFinalSummery() {
        draggingManager.SetActive(false);
        Invoke(nameof(OpenSummery), 4f);
    }
    void OpenSummery() {
        Summary.ViewSummary();
    }
    #endregion Methods
}
