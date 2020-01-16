using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBarHandler : MonoBehaviour
{
    #region Fields
    [SerializeField] Animator uiSnapAnimator;
    #endregion Fields

    #region Methods
    public void CloseToolBar()
    {
        HideToolBar();
    }

    public void OpenToolBar()
    {
        ShowToolBar();
    }

    void HideToolBar()
    {
        uiSnapAnimator.SetTrigger("IsClosed");
    }

    void ShowToolBar()
    {
        uiSnapAnimator.SetTrigger("IsOpened");
    }
    #endregion Methods
}
