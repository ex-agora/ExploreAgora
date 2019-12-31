using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSS132ElementHandler : MonoBehaviour
{
    float duration;
    float repeatRate = 0.05f;
    float step;
    Vector3 defaultPsoition;
    [SerializeField]
    Transform destination;

    public Transform Destination { get => destination; set => destination = value; }


    // Start is called before the first frame update
    void Start ()
    {
        defaultPsoition = transform.position;
    }
    public void ResetPosition ()
    {
        ResetPositionSmoothly ();
    }
    public void SnaptoDestination ()
    {
        transform.position = destination.position;
    }
    public void ResetPositionSmoothly ()
    {
        MSS132Manager.Instance.IsAnimationWorking = true;
        duration = 0.3f;
        step = 2.0f / ( duration / repeatRate );
        InvokeRepeating (nameof (CustomUpdate) , 0 , repeatRate);

    }
    void CustomUpdate ()
    {
        duration -= repeatRate;
        transform.position = Vector3.Lerp (transform.position , defaultPsoition , step);
        if ( duration <= 0.0f )
        {
            CancelInvoke (nameof (CustomUpdate));
            transform.position = defaultPsoition;
            MSS132Manager.Instance.IsAnimationWorking = false;
        }
    }
}
