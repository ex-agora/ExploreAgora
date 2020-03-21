using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorFadingHandler : MonoBehaviour
{
    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private Text errorMsgText;

    public void ShowErrorMsg(string _msg)
    {
        errorMsgText.text = _msg;
        fadeAnimator.SetTrigger("IsOpen");
    }
    
    public void HideErrorMsg()
    {
        fadeAnimator.SetTrigger("IsClose");
    }
}
