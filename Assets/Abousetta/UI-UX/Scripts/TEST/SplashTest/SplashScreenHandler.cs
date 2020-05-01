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
        Invoke(nameof(PlaySS1), 0.1f);
        stonesAnimator.SetTrigger("PlayStonesAnim");
        Invoke(nameof(PlaySS2), 1.1f);
        fadeAnimator.SetTrigger("PlayFadeAnim");
        Invoke(nameof(PlaySS3), 2.1f);
        Invoke(nameof(DeactiveAnim), 7f);
    }
    void PlaySS1() {
    AudioManager.Instance?.Play("SS1", "Activity");
    }
    void PlaySS2()
    {
        AudioManager.Instance?.Play("SS2", "Activity");
    }
    void PlaySS3()
    {
        AudioManager.Instance?.Play("SS3", "Activity");
    }
    public void DeactiveAnim()
    {
        splashAnimator.gameObject.SetActive(false);
        stonesAnimator.gameObject.SetActive(false);
        UXFlowManager.Instance.CanvasChecker();
        this.gameObject.SetActive(false);
    }
}