using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrinderInteractions : MonoBehaviour
{
    [SerializeField] GameEvent afterGrinderComplete;
    [SerializeField] GameObject coalPowder;
    [SerializeField] GameObject coalPowderHotSpot;
    [SerializeField] Animator coalGrinderAnim;
    [SerializeField] ParticleSystem particle;
    float updateRate = 1;
    float duration = 5;

    void PlaceCoalPowder()
    {
        coalPowder.SetActive(true);
    }

    void CustomUpdate()
    {
        duration -= updateRate;
        if (duration <= 0)
        {
            CancelInvoke(nameof(CustomUpdate));
            coalGrinderAnim.SetTrigger("Fire");
            Invoke(nameof(AfterComplete), 0.788322f);
        }
    }
    void AfterComplete() {
        afterGrinderComplete?.Raise();
    }
    public void StartGrinder()
    {
        duration = 4.3f;
        coalGrinderAnim.SetTrigger("Fire");
        AudioManager.Instance.Play("grinder", "Activity");
        InvokeRepeating(nameof(CustomUpdate), 0, updateRate);
    }
   public void ShowPowder()
    {
        Invoke(nameof(PlaceCoalPowder), 0.65f);
    }
    public void MoveUp() {
        coalGrinderAnim.SetTrigger("Move");
        Invoke(nameof(PlayPS), 4.5f);
    }
    public void PlayPS() {
        particle.gameObject.SetActive(true);
        particle.Play();
    }
    void ShowFinalSpot() {
        OnBoardingGameManager.Instance.GoToNextState();
        coalPowderHotSpot.SetActive(true);
    }
    public void PlayPlaceSFX() {

    }

}