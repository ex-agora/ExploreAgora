﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalObjHandler : MonoBehaviour
{
    [SerializeField] GameEvent coverNextPhaseEvent;
    [SerializeField] FadeInOut cover;
    [SerializeField] FadeInOut pestal;
    [SerializeField] FadeInOut hand;
    [SerializeField] FadeInOut grinderPart;
    public void CoalFade()
    {
        Invoke(nameof(FadeGrinder), 0.15f);
    }
    void FadeGrinder() {
        pestal.fadeInOut(true);
        grinderPart.fadeInOut(true);
        hand.fadeInOut(true);
    }
    public void SetCoverEvent() {
        Invoke(nameof(ChangeEvent), 2f);
    }
    void ChangeEvent() {
        cover.OnFadeComplete = coverNextPhaseEvent;
    }
}
