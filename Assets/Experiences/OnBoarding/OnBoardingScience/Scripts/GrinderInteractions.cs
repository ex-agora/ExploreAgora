﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrinderInteractions : MonoBehaviour
{
    [SerializeField] GameEvent afterGrinderComplete;
    [SerializeField] GameObject coalPowder;
    [SerializeField] GameObject coalPowderHotSpot;
    [SerializeField] Animator coalGrinderAnim;
    [SerializeField] ParticleSystem particle;
    [SerializeField] QuickFadeHandler label;
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
            Invoke(nameof(AfterComplete), 1.788322f);
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
        Invoke(nameof(PlaceCoalPowder), 0.1f);
    }
    public void MoveUp() {
        coalGrinderAnim.SetTrigger("Move");
        Invoke(nameof(PlayPS), 0.4f);
    }
    public void PlayPS() {
        particle.gameObject.SetActive(true);
        particle.Play();
        AudioManager.Instance.Play("revealObject", "Activity");
        Invoke(nameof(ShowFinalSpot), 2f);
    }
    void ShowFinalSpot() {
        particle.gameObject.SetActive(false);
        OnBoardingGameManager.Instance.GoToNextState();
        label.FadeIn();
        coalPowderHotSpot.SetActive(true);
    }
    public void PlayPlaceSFX() {

    }

}