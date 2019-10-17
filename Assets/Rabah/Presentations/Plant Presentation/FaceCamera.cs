using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    Transform target;

    public Transform Target { get => target; set => target = value; }
    private void Start ()
    {
        Target =  PlantPartsGameManager.Instance.ArCamera.transform;
    }

    void Update()
    {
        Vector3 targetPostition = new Vector3(-Target.position.x,
                                                transform.position.y,
                                                -Target.position.z);
        transform.LookAt(targetPostition);
    }
}
