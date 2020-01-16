using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSS132PlantAnimations : MonoBehaviour
{
    [SerializeField] float thrivinvgValue;
    [SerializeField] float aliveValue;
    [SerializeField] float dyingValue;
    [SerializeField] float aliveAnimationValue;
    [SerializeField] float dyingAnimationValue; 
    float repeatRate = 0.05f;
    float step;
    [SerializeField] BlendShapeHandler plantTransition;
    float endKayValue;
    float startKayValue;
    [SerializeField] float duration;
    float durationCounter;
    public float EndKayValue { get => endKayValue; set => endKayValue = value; }
    public float Duration { get => duration; set => duration = value; }
    public float StartKayValue { get => startKayValue; set => startKayValue = value; }
    public float ThrivinvgValue { get => thrivinvgValue; set => thrivinvgValue = value; }
    public float AliveValue { get => aliveValue; set => aliveValue = value; }
    public float DyingValue { get => dyingValue; set => dyingValue = value; }
    public float AliveAnimationValue { get => aliveAnimationValue; set => aliveAnimationValue = value; }
    public float DyingAnimationValue { get => dyingAnimationValue; set => dyingAnimationValue = value; }

    public void StartPlantTransition ()
    {
        durationCounter = duration;
        MSS132Manager.Instance.IsAnimationWorking = true;
        step = ((EndKayValue - plantTransition.KayValue) / duration) * repeatRate;
        if (step > 0) { AudioManager.Instance?.Play("resurrectionDown", "Activity"); } else { AudioManager.Instance?.Play("resurrection", "Activity"); }
        InvokeRepeating (nameof (CustomUpdate) , 0 , repeatRate);
    }
    void CustomUpdate ()
    {
        durationCounter -= repeatRate;
        plantTransition.KayValue+= step;
        if ( durationCounter <= 0.0f )
        {
            plantTransition.KayValue = EndKayValue;
            MSS132Manager.Instance.IsAnimationWorking = false;
            CancelInvoke (nameof (CustomUpdate));
        }
    }
    public void StartPlantTransitionSameState ()
    {
        durationCounter = duration;
        MSS132Manager.Instance.IsAnimationWorking = true;
        step = (((EndKayValue - plantTransition.KayValue) / duration) * repeatRate) / 2.0f;
        InvokeRepeating (nameof (CustomUpdateSameState) , 0 , repeatRate);
    }
    void CustomUpdateSameState ()
    {
        durationCounter -= repeatRate;
        if ( durationCounter >= Duration / 2 )
        {
            plantTransition.KayValue += step;
        }
        else if ( durationCounter < Duration / 2 && durationCounter  > 0)
        {
            plantTransition.KayValue -= step;
        }
        else if ( durationCounter <= 0.0f )
        {
            plantTransition.KayValue = StartKayValue;
            CancelInvoke (nameof (CustomUpdateSameState));
            MSS132Manager.Instance.IsAnimationWorking = false;
        }
    }
}
