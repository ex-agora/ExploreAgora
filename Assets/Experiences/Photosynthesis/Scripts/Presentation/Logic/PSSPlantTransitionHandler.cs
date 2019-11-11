using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSSPlantTransitionHandler : MonoBehaviour
{
    float duration;
    float repeatRate = 0.05f;
    float step;
    [SerializeField] Animator plantAnimator;
    [SerializeField] BlendShapeHandler plantTransition;
    //plantTransition
    public void PlantTransition ()
    {
        duration = 2.5f;
        step = 7.5f / ( duration / repeatRate );
        InvokeRepeating (nameof (CustomUpdate) , 0 , repeatRate);
        plantAnimator.SetBool ("PlantTransition" , true);
    }
    void CustomUpdate ()
    {
        duration -= repeatRate;
        //TODO Smooth Blend Shapes Of Plant and Sugar/O2 Animation

        plantTransition.KayValue = Mathf.Lerp (plantTransition.KayValue , 1 , step);

        print ("Sugar And O2 Animation");
        if ( duration <= 0.0f )
        {
            PhotosynthesisGameManager.Instance.FinalSmallSummary.ViewSummary ();
            CancelInvoke (nameof (CustomUpdate));
        }
    }
}
