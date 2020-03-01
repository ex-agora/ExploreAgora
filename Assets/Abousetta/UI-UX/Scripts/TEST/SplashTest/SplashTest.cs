using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashTest : MonoBehaviour
{
    public Animator splashAnimator;
    public Animator stonesAnimator;
    public Animator fadeAnimator;
    // Start is called before the first frame update
    void Start()
    {
        PlayAnim ();
    }
    void PlayAnim ()
    {
        splashAnimator.SetTrigger ("PlayLogoAnim");
        stonesAnimator.SetTrigger ("PlayStonesAnim");
        fadeAnimator.SetTrigger ("PlayFadeAnim");
        Invoke (nameof (StopAnim) , 7f);
    }
    void StopAnim ()
    {
        splashAnimator.gameObject.SetActive (false);
        stonesAnimator.gameObject.SetActive (false);
    }
}
