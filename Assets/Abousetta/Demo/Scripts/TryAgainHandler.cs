using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryAgainHandler : MonoBehaviour
{
    #region Fields
    [SerializeField] Animator tryAgainAnimator;
    #endregion Fields

    #region Methods
    /// Called to close try again bubble.
    public void HideBubble()
    {
        CloseBubble();
    }

    /// Called to open try again bubble.
    public void ShowBubble()
    {
        OpenBubble();
    }

    /// Close try again panel.
    /// Called in HideBubble function.
    void CloseBubble()
    {
        tryAgainAnimator.SetTrigger("IsClosed");
    }

    /// Open try again panel.
    /// Called in ShowBubble function.
    void OpenBubble()
    {
        tryAgainAnimator.SetTrigger("IsOpened");
    }
    #endregion Methods
}