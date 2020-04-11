using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnBoardingSocialStudyGameManager : MonoBehaviour, ITriggable, IMenuHandler
{
    #region Fields
    [SerializeField] float duration = 10f;
    [SerializeField] StateMachineManager stateMachine;
    float elapsedTime = 0f;
    [SerializeField] int[] groundIndicatorIndecies;
    int groundIndicatorIndex;
    [SerializeField] GroundIndicatorTutorialHandler groundIndicatorTutorialHandler;
    private bool nextState;
    [SerializeField] RealTimeTutorialHandler realTimeTutorialHandler;
    [SerializeField] GameEvent showHotspotEvent;
    [SerializeField] SpeechBubbleController speechBubbleController;
    [SerializeField] TutorialPanelController tutorial;
    [SerializeField] SummaryHandler summary;
    float updateRate = 1f;
    #endregion Fields

    #region Methods
    private void StartMachine()
    {
        stateMachine.StartSM();
    }
    public void FoundSurface()
    {
        groundIndicatorTutorialHandler.CloseIndicator();
        if (IsInvoking(nameof(CustomUpdate)))
        {
            CancelInvoke(nameof(CustomUpdate));
        }
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
        FinishExperiencesHandler.Instance.FinshExperience(3);
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartFirstPhase()
    {
        realTimeTutorialHandler.OpenIndicator();
        nextState = true;
    }
    public void EndFirstPhase() {
        nextState = true;
    }
    public void EndSecondPhase()
    {
        nextState = true;
    }
    public void StartSecondPhase()
    {
        nextState = true;
        showHotspotEvent?.Raise();
    }
    public void TutorialStep()
    {
        Invoke(nameof(ShowTutorial), 1.5f);
    }
    private void CustomUpdate()
    {
        elapsedTime += updateRate;
        if (elapsedTime >= duration)
        {
            speechBubbleController.SetHintText(groundIndicatorIndecies[groundIndicatorIndex]);
            groundIndicatorIndex = (groundIndicatorIndex + 1) % groundIndicatorIndecies.Length;
            nextState = true;
            elapsedTime = 0;
        }
    }
    private void ShowSummary()
    {
        summary.ViewSummary();
    }
    void ShowTutorial()
    {
        tutorial.TutorialTextStr = speechBubbleController.NextBubble();
        tutorial.OpenTutorial();
    }
    private void Start()
    {
        groundIndicatorIndex = 0;
        groundIndicatorTutorialHandler.OpenIndicator();
        AudioManager.Instance.Play("bg", "Background");
        Invoke(nameof(StartMachine), 2f);
        InvokeRepeating(nameof(CustomUpdate), 0, updateRate);
    }
    public void FinalPhase() { Invoke(nameof(ShowSummary), 2f); }
    #endregion Methods
}