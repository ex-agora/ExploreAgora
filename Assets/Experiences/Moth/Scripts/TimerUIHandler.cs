using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUIHandler : MonoBehaviour
{
    float duration;
    float updateRate;
    float elapseTime;

    [SerializeField] Animator timerAnim;
    [SerializeField] Image timerImgae;
    [SerializeField] GameEvent onTimerEnd;

    public float Duration { get => duration; set => duration = value; }

    private void ShowTimerBar()
    {
        timerImgae.fillAmount = 1;
        timerAnim.SetTrigger("IsOpen");
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
            CancelInvoke(nameof(CustomUpdate));
            onTimerEnd.Raise();
        }
    }

    public void ViewBar()
    {
        ShowTimerBar();
    }
    public void HideBar()
    {
        if (IsInvoking(nameof(CustomUpdate)))
            CancelInvoke(nameof(CustomUpdate));
        HideTimerBar();
    }
}