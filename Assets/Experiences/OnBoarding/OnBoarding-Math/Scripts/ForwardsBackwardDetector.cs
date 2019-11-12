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
    public Vector3 NeztPOS;

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
        NeztPOS.z = transform.position.z;
        if (LastPOS.z < NeztPOS.z)
        {
            //isMovingForward = true;
            //isMovingBackward = false;

            snappingManager.isForward = true;
            Debug.Log("Forward");
        }
        if (LastPOS.z > NeztPOS.z)
        {
            //isMovingBackward = true;
            //isMovingForward = false;

            snappingManager.isForward = false;
            Debug.Log("backward");

        }
        //else if (LastPOS.z == NeztPOS.z)
        //{
        //    isMovingForward = false;
        //    isMovingBackward = false;
        //}

        LastPOS.z = NeztPOS.z;
    }


}
