using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternetPopupHandler : MonoBehaviour
{
    private static InternetPopupHandler instance;
    [SerializeField] ToolBarHandler popup;

    public static InternetPopupHandler Instance { get => instance; set => instance = value; }
    private void Awake()
    {
        if (instance = null)
            instance = this;
    }
    public void OpenPopup() => popup.OpenToolBar();
}
