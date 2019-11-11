using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeaderBarHandler : MonoBehaviour
{
    [SerializeField] Animator headerAnim;

    void ShowBar()
    {
        headerAnim.SetTrigger("IsOpened");
    }
    void HideBar()
    {
        headerAnim.SetTrigger("IsClosed");
    }

    public void OpenBar()
    {
        ShowBar();
    }
    public void CloseBar()
    {
        HideBar();
    }
}