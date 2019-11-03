using System.Collections;
using UnityEngine;

public class SmoothScaling : MonoBehaviour
{

    bool state;
    Coroutine C_StartScaling;

    public bool State { get => state; set => state = value; }
    Vector3 endScale;
    private void Start ()
    {
        endScale = transform.localScale;
    }
    public void StartScaling ()
    {
        State = true;
        if ( C_StartScaling != null )
            StopCoroutine (C_StartScaling);
        C_StartScaling = StartCoroutine (startScaling (2.5f));
    }
    public void IncorrectAtom ()
    {
        transform.position = GetComponent<DraggableOnSurface> ().MyPosition;
    }
    public void EndScaling ()
    {
        State = false;
        if ( C_StartScaling != null )
            StopCoroutine (C_StartScaling);
    }
    IEnumerator startScaling (float duration)
    {
        float elapsedTime = 0;

        while ( elapsedTime < duration )
        {
            if ( state )
            {
                transform.localScale = new Vector3 (Mathf.Lerp (transform.localScale.x , endScale.x / 2 , ( elapsedTime / duration )) , Mathf.Lerp (transform.localScale.y , endScale.y / 2 , ( elapsedTime / duration )) , Mathf.Lerp (transform.localScale.z , endScale.z / 2 , ( elapsedTime / duration )));
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        EndScaling ();
    }
}

