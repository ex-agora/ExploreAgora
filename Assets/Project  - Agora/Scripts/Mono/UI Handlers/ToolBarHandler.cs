using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBarHandler : MonoBehaviour
{
    #region Fields
    [SerializeField] Animator uiSnapAnimator;
    [SerializeField] bool keepAnimAliveOnDisable =false;
    #endregion Fields

    #region Methods
    private void Start()
    {
        uiSnapAnimator.keepAnimatorControllerStateOnDisable = keepAnimAliveOnDisable;
    }
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
