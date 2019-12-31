using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSSPlantTransitionHandler : MonoBehaviour
{
    [SerializeField]float duration = 2.5f;
    float repeatRate = 0.05f;
    float step;
    [SerializeField] BlendShapeHandler plantTransition;
    [SerializeField] ParticleSystem particle;
    //plantTransition
    public void PlantTransition ()
    {
        Invoke(nameof(PlayTranaction), 2f);
    }
    void PlayTranaction() {
        step = 5f / (duration / repeatRate);
        particle.gameObject.SetActive(true);
        AudioManager.Instance?.Play("resurrection", "Activity");
        particle.Play();
        InvokeRepeating(nameof(CustomUpdate), 0, repeatRate);
    }
    void CustomUpdate ()
    {
        duration -= repeatRate;
        plantTransition.KayValue = Mathf.Lerp (plantTransition.KayValue , 1 , step);
        if ( duration <= 0.0f )
        {
            CancelInvoke (nameof (CustomUpdate));
        }
    }
}
