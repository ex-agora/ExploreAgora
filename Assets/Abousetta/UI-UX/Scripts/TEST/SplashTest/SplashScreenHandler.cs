using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenHandler : MonoBehaviour
{
    public Animator splashAnimator;
    public Animator stonesAnimator;
    public Animator fadeAnimator;

    public void PlayAnim()
    {
        splashAnimator.SetTrigger("PlayLogoAnim");
        stonesAnimator.SetTrigger("PlayStonesAnim");
        fadeAnimator.SetTrigger("PlayFadeAnim");
        Invoke(nameof(DeactiveAnim), 7f);
    }
    public void DeactiveAnim()
    {
        splashAnimator.gameObject.SetActive(false);
        stonesAnimator.gameObject.SetActive(false);
        UXFlowManager.Instance.LoginFadeIn();
        this.gameObject.SetActive(false);
    }
}