using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealTimeTutorialHandler : MonoBehaviour
{
    [SerializeField] Animator RTTutorialAnimator;
    [SerializeField] bool isTarageted;
    [SerializeField] RectTransform hand;
    [SerializeField] float duration = 1;
    float updateRate = 0.05f;
    float elpTime;
    void ShowIndicator()
    {
        RTTutorialAnimator.SetTrigger("IsOpened");
        if (isTarageted)
        {
            elpTime = 0;
            InvokeRepeating(nameof(CustomUpdate), 2, updateRate);
        }
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
    void CustomUpdate() {
        //hand.offsetMax = Vector2.Lerp(hand.offsetMax, Vector2.zero, elpTime /duration);
        //hand.offsetMin = Vector2.Lerp(hand.offsetMin, Vector2.zero, elpTime / duration);
        hand.anchoredPosition = Vector3.Lerp(hand.anchoredPosition, Vector3.zero, (elpTime / duration));
        if (elpTime >= duration) {
            //hand.offsetMax = Vector2.zero;
            //hand.offsetMin = Vector2.zero;
            hand.anchoredPosition = Vector3.zero;
            CancelInvoke(nameof(CustomUpdate));
        }
        elpTime += updateRate;
    }
}
