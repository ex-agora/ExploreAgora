﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    #region Fields
    [SerializeField] float fadeDuration = 0.5f;
    private Material gameObjecMat;
    [SerializeField] bool isMaterialPlaced;
    [SerializeField] Material mat;
    MeshRenderer meshRenderer;
    [SerializeField] GameEvent onFadeComplete;
    SkinnedMeshRenderer skinnedMeshRenderer;
    bool currentState;
    #endregion Fields

    #region Properties
    public GameEvent OnFadeComplete { get => onFadeComplete; set => onFadeComplete = value; }
    #endregion Properties

    #region Methods
    // false means fade out  
    public void fadeInOut(bool state)
    {
        currentState = state;
        StartCoroutine(startFading(state, fadeDuration));
    }
    public void SetFadeAmount(float amount) => gameObjecMat?.SetFloat("_Transparency", amount);
    private void Awake()
    {

        if (isMaterialPlaced) {
            gameObjecMat = mat;
            return;
        }
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null) {
            gameObjecMat = meshRenderer.material;
            return;
        }
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer != null) {
            gameObjecMat = skinnedMeshRenderer.material;
        }
    }
    IEnumerator startFading(bool state, float duration)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.5f);
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            if (state)
                gameObjecMat.SetFloat("_Transparency", Mathf.Lerp(0, 1f, (elapsedTime / duration)));
            else
                gameObjecMat.SetFloat("_Transparency", Mathf.Lerp(1f, 0, (elapsedTime / duration)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        gameObjecMat.SetFloat("_Transparency", state ? 1 : 0);
        //onComplete
        //Debug.LogWarning(OnFadeComplete);
        yield return new WaitForEndOfFrame();
        if (OnFadeComplete != null)
            OnFadeComplete.Raise();
    }
    #endregion Methods
}
