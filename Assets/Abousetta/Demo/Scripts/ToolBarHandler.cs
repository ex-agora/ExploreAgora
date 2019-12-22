using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBarHandler : MonoBehaviour
{
    [SerializeField] Animator uiSnapAnimator;

    void ShowToolBar()
    {
        uiSnapAnimator.SetTrigger("IsOpened");
    }
    void HideToolBar()
    {
        uiSnapAnimator.SetTrigger("IsClosed");
    }

    public void OpenToolBar()
    {
        ShowToolBar();
    }
    public void CloseToolBar()
    {
        HideToolBar();
    }
}
