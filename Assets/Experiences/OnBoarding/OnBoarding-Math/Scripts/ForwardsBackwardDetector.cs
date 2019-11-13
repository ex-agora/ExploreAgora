using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardsBackwardDetector : MonoBehaviour
{

    [SerializeField] SnappingManager snappingManager;
    Rigidbody rigidbody;


    public bool isMovingForward;
    public bool isMovingBackward;
    public Vector3 LastPOS;
    public Vector3 NextPOS;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //var velocity = rigidbody.velocity;
        //var localVel = transform.InverseTransformDirection(velocity);

        //if (localVel.z > 0)
        //{
        //    Debug.Log("QWEQWEQE");
        //    // We're moving forward
        //    snappingManager.isForward = true;
        //}
        //else
        //{
        //    Debug.Log("assadsada");
        //    snappingManager.isForward = false;
        //    // We're moving backward
        //}
    }

    void LateUpdate()
    {
        NextPOS.z = transform.position.z;
        if (LastPOS.z < NextPOS.z)
        {
            //isMovingForward = true;
            //isMovingBackward = false;

            Debug.Log("Forward");
            snappingManager.isForward = false;
        }
        if (LastPOS.z > NextPOS.z)
        {
            snappingManager.isForward = true;
            //isMovingBackward = true;
            //isMovingForward = false;

            Debug.Log("backward");

        }
        //else if (LastPOS.z == NeztPOS.z)
        //{
        //    isMovingForward = false;
        //    isMovingBackward = false;
        //}

        LastPOS.z = NextPOS.z;
    }


}
