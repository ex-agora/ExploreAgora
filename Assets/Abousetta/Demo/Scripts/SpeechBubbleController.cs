using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UnityEngine.UI;

public class SpeechBubbleController : MonoBehaviour, IStateController
{
    #region Fields
    [SerializeField] Animator bubbleAnimator;
    [SerializeField] BubbleTextHolder bubbleTextHolder;
    [SerializeField] Button characterBtn;
    [SerializeField] CharByCharController charByCharController;
    bool isOpened;
    BubbleTextInfo savedBubbleInfo;
    #endregion Fields

    #region Methods
    /// Attached to the character OnClick.
    /// Show and hide speech bubble.
    public void CommandBubbleToggle()
    {
        if (isOpened)
        {
            HideBubble();

        }
        else
        {
            ShowCommandBubble();
        }
    }

    /// Hide speech bubble.
    /// Called whenever needed to hide speech bubble.
    public void HideBubble()
    {
        if (isOpened)
        {
            CloseBubble();
            isOpened = false;
        }
    }

    /// Show next commaned.
    /// Called whenever needed to show next command.
    public string NextBubble()
    {
        //characterBtn.interactable = false;
        var tempText = bubbleTextHolder.GetNextInfo();
        if (tempText.BubbleType == BubbleType.Command)
        {
            savedBubbleInfo = tempText;
        }
        return tempText.TextInfo;
    }

    public void RunSpeech()
    {
        characterBtn.interactable = true;
    }

    public void SetHintText(int index)
    {
        bubbleTextHolder.SetNextHint(index);
    }

    public void ShowNextBubble()
    {
        characterBtn.interactable = false;
        var tempText = bubbleTextHolder.GetNextInfo();
        if (tempText.BubbleType == BubbleType.Command)
        {
            savedBubbleInfo = tempText;
        }

        charByCharController.CharIndex = 0;
        charByCharController.TextString = tempText.TextInfo;
        charByCharController.OutputText = string.Empty;
        if (!isOpened)
        {
            OpenBubble();
            Invoke(nameof(PlaySFX), 0.5f);
            isOpened = true;
        }
    }

    public void StopSpeech()
    {
        HideBubble();
        characterBtn.interactable = false;
    }

    /// Close speech bubble.
    /// Called in HideBubble function.
    void CloseBubble()
    {
        bubbleAnimator.SetTrigger("IsClosed");
        characterBtn.interactable = true;
    }

    /// Open speech bubble.
    /// Called in ShowNextBubble function.
    /// Called in ShowCommandBubble function.
    void OpenBubble()
    {
        bubbleAnimator.SetTrigger("IsOpened");
        
    }

    void PlaySFX() {
        if (AudioManager.Instance != null)
            AudioManager.Instance.Play("speechOpen", "UI");
    }
    /// Show saved commaned.
    /// Called in CommandBubbleToggle.   
    void ShowCommandBubble()
    {
        charByCharController.OutputText = savedBubbleInfo.TextInfo;
        if (!isOpened)
        {
            OpenBubble();
            Invoke(nameof(PlaySFX), 0.554f);
            isOpened = true;
        }
    }
    #endregion Methods
}