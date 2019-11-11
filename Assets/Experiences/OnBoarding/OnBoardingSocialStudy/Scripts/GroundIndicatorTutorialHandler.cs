using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundIndicatorTutorialHandler : MonoBehaviour
{
    [SerializeField] Animator groundIndicatorAnimator;

    void ShowIndicator()
    {
        groundIndicatorAnimator.SetTrigger("IsOpened");
    }
    void HideIndicator()
    {
        groundIndicatorAnimator.SetTrigger("IsClosed");
    }

    public void OpenIndicator()
    {
        ShowIndicator();
    }
    public void CloseIndicator()
    {
        HideIndicator();
    }
}
