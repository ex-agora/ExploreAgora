using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenHandler : MonoBehaviour
{
    private static SplashScreenHandler instance;
    public Animator splashAnimator;
    public Animator stonesAnimator;
    public Animator fadeAnimator;
    [SerializeField] WarningTransitionHandler warning;
    public static SplashScreenHandler Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
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
        warning.ShowWarning();
        this.gameObject.SetActive(false);
        splashAnimator.gameObject.SetActive(false);
        stonesAnimator.gameObject.SetActive(false);
    }
}