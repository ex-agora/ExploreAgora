using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnBoardingSocialStudyGameManager : MonoBehaviour, ITriggable, IMenuHandler
{
    [SerializeField] GroundIndicatorTutorialHandler groundIndicatorTutorialHandler;
    [SerializeField] RealTimeTutorialHandler realTimeTutorialHandler;
    [SerializeField] SpeechBubbleController speechBubbleController;
    [SerializeField] int[] groundIndicatorIndecies;

    [SerializeField] float duration = 10f;
    float elapsedTime = 0f;
    float updateRate = 1f;
    int groundIndicatorIndex;
    private bool nextState;

    private void Start()
    {
        groundIndicatorIndex = 0;
        groundIndicatorTutorialHandler.OpenIndicator();
        InvokeRepeating(nameof(CustomUpdate), 0, updateRate);
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

    public void StartFirstPhase()
    {
        realTimeTutorialHandler.OpenIndicator();
        nextState = true;
    }

    public void FoundSurface()
    {
        groundIndicatorTutorialHandler.CloseIndicator();
        if (IsInvoking(nameof(CustomUpdate)))
        {
            CancelInvoke(nameof(CustomUpdate));
        }
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoTOHome()
    {
        //TODO
    }

    public bool GetTrigger()
    {
        bool up = nextState;
        nextState = false;
        return up;
    }
}
