﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeObjHnadler : MonoBehaviour
{
    [SerializeField] MeshRenderer meshTree;
    [SerializeField] Material afterMatTree;
    [SerializeField] ParticleSystem particle;
    [SerializeField] GameEvent stopVFXEvent = null;
    bool isVFXRunning;
    public void StratVFX() {
        
        particle.gameObject.SetActive(true);
        particle.Play();
        isVFXRunning = true;
        AudioManager.Instance.Play("factory", "Activity");
        Invoke(nameof(StopVFX), 5f);
    }
    void StopVFX() {
        isVFXRunning = false;
        stopVFXEvent?.Raise();
        particle.Stop();
        meshTree.material = afterMatTree;
    }
    private void OnDisable()
    {
        if(isVFXRunning)
            stopVFXEvent?.Raise();
        particle.Stop();
        particle.gameObject.SetActive(false);
    }
}
