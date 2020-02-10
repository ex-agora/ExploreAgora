using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MSS132ProgressBarHandler : MonoBehaviour
{
    [SerializeField] Image barIcon;
    [SerializeField] Image progressImg;
    [SerializeField] Color thrivingColor;
    [SerializeField] Color aliveColor;
    [SerializeField] Color dyingColor;
    [SerializeField] float thrivingValue;
    [SerializeField] float aliveValue;
    [SerializeField] float dyingValue;
    [SerializeField] float duration = 1f;
    float updateRate = 0.05f;
    float targetValue;
    float elpTime;
    float step;
    public void ActiveThriving() {
        barIcon.color = thrivingColor;
        progressImg.color = thrivingColor;
        elpTime = 0.0f;
        step = (thrivingValue - progressImg.fillAmount) / duration * updateRate;
        targetValue = thrivingValue;
        if (!IsInvoking(nameof(CustomUpdate)))
            InvokeRepeating(nameof(CustomUpdate), 0, updateRate);
    }
    public void ActiveAlive() {
        barIcon.color = aliveColor;
        progressImg.color = aliveColor;
        elpTime = 0.0f;
        step = (aliveValue - progressImg.fillAmount) / duration * updateRate;
        targetValue = aliveValue;
        if (!IsInvoking(nameof(CustomUpdate)))
            InvokeRepeating(nameof(CustomUpdate), 0, updateRate);
    }
    public void ActiveDying() {
        barIcon.color = dyingColor;
        progressImg.color = dyingColor;
        elpTime = 0.0f;
        step = (dyingValue - progressImg.fillAmount) / duration * updateRate;
        targetValue = dyingValue;
        if (!IsInvoking(nameof(CustomUpdate)))
            InvokeRepeating(nameof(CustomUpdate), 0, updateRate);
    }
    void CustomUpdate() {
        progressImg.fillAmount += step;
        elpTime += updateRate;
        if (elpTime >= duration) {
            CancelInvoke(nameof(CustomUpdate));
            progressImg.fillAmount = targetValue;
        }
    }
}
