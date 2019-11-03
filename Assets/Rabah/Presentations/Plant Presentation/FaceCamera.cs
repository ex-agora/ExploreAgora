using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    Transform target;
    Vector3 targetPostition;
    public Transform Target { get => target; set => target = value; }
    private void Start ()
    {
        Target =  PlantPartsGameManager.Instance.ArCamera.transform;
    }

    void Update()
    {
        //targetPostition.x = -Target.position.x;
        //targetPostition.y = transform.position.y;
        //targetPostition.z = -Target.position.z;
        //transform.LookAt(-targetPostition);
        transform.LookAt(transform.position + Target.rotation * Vector3.forward, Target.rotation * Vector3.up);
    }
}
