using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : SteerBase
{
    [SerializeField] public SteerBase target;
    private Quaternion targetRotation;
    private float str;
    private float strength = 0.5f;
   
    protected override Vector3 SteerForce()
    {
        return ((target.position) - (position)) - (velocity);
    }

    private void FixedUpdate()
    {
        UpdateSteer();
        if (isRotDir) {
            targetRotation = Quaternion.LookRotation(target.position - transform.position);
            str = Mathf.Min(strength * Time.deltaTime, 1);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);
        }
    }
}
