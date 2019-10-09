using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryAgainHandler : MonoBehaviour
{
    [SerializeField] Animator tryAgainAnimator;

    void OpenBubble()
    {
        tryAgainAnimator.SetTrigger("IsOpened");
    }
    void CloseBubble()
    {
        tryAgainAnimator.SetTrigger("IsClosed");
    }

    public void ShowBubble()
    {
        OpenBubble();
    }
    public void HideBubble()
    {
        CloseBubble();
    }


}
