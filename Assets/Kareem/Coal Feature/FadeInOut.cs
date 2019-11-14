using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    private Material gameObjecMat;
    [SerializeField] GameEvent onFadeComplete;
    [SerializeField] float fadeDuration = 0.5f;
    MeshRenderer meshRenderer;
    SkinnedMeshRenderer skinnedMeshRenderer;
    public GameEvent OnFadeComplete { get => onFadeComplete; set => onFadeComplete = value; }

    private void Awake()
    {

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

    // false means fade out  
    public void fadeInOut(bool state)
    {
        StartCoroutine(startFading(state, fadeDuration));
    }

    IEnumerator startFading(bool state, float duration)
    {
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
        if (OnFadeComplete != null)
            OnFadeComplete.Raise();
    }
}
