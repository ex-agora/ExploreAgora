using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnBoardingSocialStudyGameManager : MonoBehaviour, ITriggable, IMenuHandler
{
    #region Fields
    [SerializeField] float duration = 10f;
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
        //TODO
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

    public void StartSecondPhase()
    {
        nextState = true;
        showHotspotEvent?.Raise();
    }
    public void TutorialStep() {
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
    public void ShowSummary() {
        summary.ViewSummary();
    }
    void ShowTutorial() {
        tutorial.OpenTutorial();
    }
    private void Start()
    {
        groundIndicatorIndex = 0;
        groundIndicatorTutorialHandler.OpenIndicator();
        InvokeRepeating(nameof(CustomUpdate), 0, updateRate);
    }
    #endregion Methods
}
