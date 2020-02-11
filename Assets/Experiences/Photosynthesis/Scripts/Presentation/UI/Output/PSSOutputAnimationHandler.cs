using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSSOutputAnimationHandler : MonoBehaviour
{
    [SerializeField] float outputAnimationsDuration = 5;
    [SerializeField] List<Animator> outputAnimations;
    float elpTime;
    float updateRate = 2.65f/2f;
    public void StartOutputAnimations ()
    {
        Invoke(nameof(PlayAnim), 4f); 
    }
    void PlayAnim() {
        for (int i = 0; i < outputAnimations.Count; i++)
        {
            outputAnimations[i].SetBool("StartFade", true);
        }
        elpTime = 0.0f;
        InvokeRepeating(nameof(CustomUpdate), 0, updateRate);
        //Invoke(nameof(StopOutputAnimations), outputAnimationsDuration);

    }
    void CustomUpdate() {
        elpTime += updateRate;
        AudioManager.Instance?.Play("miniNotification", "Activity");
        if (elpTime >= outputAnimationsDuration) {
            StopOutputAnimations();
            if(IsInvoking(nameof(CustomUpdate)))
            CancelInvoke(nameof(CustomUpdate));
        }
    }
    public void StopOutputAnimations ()
    {
        for ( int i = 0 ; i < outputAnimations.Count ; i++ )
        {
            outputAnimations [i].SetBool ("StartFade" , false);
        }
        PhotosynthesisGameManager.Instance.StartSummary ();
    }

}
