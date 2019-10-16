using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryAgainHandler : MonoBehaviour
{
    [SerializeField] Animator tryAgainAnimator;

    /// Open try again panel.
    /// Called in ShowBubble function.
    void OpenBubble()
    {
        tryAgainAnimator.SetTrigger("IsOpened");
    }

    /// Close try again panel.
    /// Called in HideBubble function.
    void CloseBubble()
    {
        tryAgainAnimator.SetTrigger("IsClosed");
    }

    /// Called to open try again bubble.
    public void ShowBubble()
    {
        OpenBubble();
    }

    /// Called to close try again bubble.
    public void HideBubble()
    {
        CloseBubble();
    }
}