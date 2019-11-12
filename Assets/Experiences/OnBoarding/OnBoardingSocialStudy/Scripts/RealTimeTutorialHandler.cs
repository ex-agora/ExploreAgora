using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealTimeTutorialHandler : MonoBehaviour
{
    [SerializeField] Animator RTTutorialAnimator;

    void ShowIndicator()
    {
        RTTutorialAnimator.SetTrigger("IsOpened");
    }
    void HideIndicator()
    {
        RTTutorialAnimator.SetTrigger("IsClosed");
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
