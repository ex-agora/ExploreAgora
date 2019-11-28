using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCameraOnYAxis : MonoBehaviour
{
    Vector3 startRot;
    //[SerializeField] Transform lookTarget;
    Transform lookTarget;
    Vector3 finalRot;
    // Start is called before the first frame update
    void Start ()
    {
        lookTarget = interactions.Instance.SessionOrigin.camera.transform;
        //startRot = lookTarget.eulerAngles;
        //startRot = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update ()
    {
        //finalRot.x = startRot.x;
        //finalRot.y = lookTarget.eulerAngles.y;
        //finalRot.z = startRot.z;
        //transform.LookAt (finalRot);
        //finalRot.x = startRot.x;
        //finalRot.y = transform.eulerAngles.y;
        //finalRot.z = startRot.z;
        //transform.eulerAngles = finalRot;
        //Vector3 relativePos = lookTarget.position - transform.position;

        //// the second argument, upwards, defaults to Vector3.up
        //Quaternion rotation = Quaternion.LookRotation (relativePos , Vector3.up);
        //transform.rotation = rotation;
        Vector3 relativePos = lookTarget.position - transform.position;
        Quaternion LookAtRotation = Quaternion.LookRotation (relativePos);

        Quaternion LookAtRotationOnly_Y = Quaternion.Euler (transform.rotation.eulerAngles.x , LookAtRotation.eulerAngles.y + 180 , transform.rotation.eulerAngles.z);

        transform.rotation = LookAtRotationOnly_Y;
    }

}
