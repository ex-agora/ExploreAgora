using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Transform Target;

    void Update()
    {
        Vector3 targetPostition = new Vector3(Target.position.x,
                                                transform.position.y,
                                                Target.position.z);
        transform.LookAt(targetPostition);
    }
}
