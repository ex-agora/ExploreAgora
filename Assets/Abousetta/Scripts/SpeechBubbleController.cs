using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class SpeechBubbleController : MonoBehaviour, IStateController
{
    [SerializeField] Animator bubbleAnimator;
    [SerializeField] CharByCharController charByCharController;
    [SerializeField] BubbleTextHolder bubbleTextHolder;
    BubbleTextInfo savedBubbleInfo;
    bool isOpened;

    void OpenBubble()
    {
        bubbleAnimator.SetTrigger("IsOpened");
    }
    void CloseBubble()
    {
        bubbleAnimator.SetTrigger("IsClosed");
    }
    void ShowCommandBubble()
    {
        charByCharController.OutputText = savedBubbleInfo.TextInfo;
        if (!isOpened)
        {
            OpenBubble();
            isOpened = true;
        }
    }
    public void ShowNextBubble()
    {
        var tempText = bubbleTextHolder.GetNextInfo();
        if(tempText.BubbleType == BubbleType.Command)
        {
            savedBubbleInfo = tempText;
        }
       
        charByCharController.CharIndex =0;
        charByCharController.TextString = tempText.TextInfo;
        charByCharController.OutputText = string.Empty;
        if (!isOpened)
        {
            OpenBubble();
            isOpened = true;
        }
    }

    public void HideBubble()
    {
        if (isOpened)
        {
            CloseBubble();
            isOpened = false;
        }
    }

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
}