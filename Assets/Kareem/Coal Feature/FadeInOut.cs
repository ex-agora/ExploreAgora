using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    private MeshRenderer gameObjecMat;
    [SerializeField] GameEvent onFadeComplete;

    private void Awake()
    {
        gameObjecMat = GetComponent<MeshRenderer>();
    }

    // false means fade out  
    public void fadeInOut(bool state)
    {
        StartCoroutine(startFading(state, 0.5f));
    }



    

    IEnumerator startFading(bool state, float duration)
    {
        yield return new WaitForSeconds(0.5f);
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            if (state)
                gameObjecMat.material.SetFloat("_Transparency", Mathf.Lerp(0, 1f, (elapsedTime / duration)));
            else
                gameObjecMat.material.SetFloat("_Transparency", Mathf.Lerp(1f, 0, (elapsedTime / duration)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //onComplete
        if (onFadeComplete != null)
            onFadeComplete.Raise();
    }
}
