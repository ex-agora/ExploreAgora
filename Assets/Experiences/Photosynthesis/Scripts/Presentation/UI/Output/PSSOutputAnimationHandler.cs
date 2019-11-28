using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSSOutputAnimationHandler : MonoBehaviour
{
    [SerializeField] float outputAnimationsDuration = 5;
    [SerializeField] List<Animator> outputAnimations;
    public void StartOutputAnimations ()
    {
        for ( int i = 0 ; i < outputAnimations.Count ; i++ )
        {
            outputAnimations [i].SetBool ("StartFade" , true);
        }
        Invoke (nameof(StopOutputAnimations) , outputAnimationsDuration);
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
