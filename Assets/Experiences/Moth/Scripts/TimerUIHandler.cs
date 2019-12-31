using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUIHandler : MonoBehaviour
{
    float duration;
    float updateRate = 0.1f;
    float elapseTime;
    bool isOpen = false;
    [SerializeField] Animator timerAnim;
    [SerializeField] Image timerImgae;
    [SerializeField] GameEvent onTimerEnd;

    public float Duration { get => duration; set => duration = value; }

    private void ShowTimerBar()
    {
        timerImgae.fillAmount = 1;
        timerAnim.SetTrigger("IsOpened");
    }

    private void HideTimerBar()
    {
        timerAnim.SetTrigger("IsClosed");
    }
    public void StartTimer()
    {
        elapseTime = 0;
        InvokeRepeating(nameof(CustomUpdate), 0, updateRate);
    }

    void CustomUpdate()
    {
        elapseTime += updateRate;
        timerImgae.fillAmount = Mathf.Clamp(((duration - elapseTime) / duration), 0f, 1f);
        if (elapseTime >= duration)
        {
            onTimerEnd.Raise();
            CancelInvoke(nameof(CustomUpdate));
        }
    }

    public void ViewBar()
    {
        if (isOpen)
            return;
        ShowTimerBar();
        isOpen = true;
    }
    public void HideBar()
    {
        if (!isOpen)
            return;
        HideTimerBar();
        isOpen = false;
        if (IsInvoking(nameof(CustomUpdate)))
            CancelInvoke(nameof(CustomUpdate));
    }
}