using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelFacingRotator : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] bool isFaceOnEnble;
    Vector3 taregPos;
    private void OnEnable ()
    {
        target = target == null ? interactions.Instance.SessionOrigin.camera.transform : target;
        if ( isFaceOnEnble )
            Face ();
    }
    public void Face ()
    {
        taregPos.x = target.position.x;
        taregPos.y = transform.position.y;
        taregPos.z = target.position.z;
        transform.LookAt (taregPos);
    }
}
