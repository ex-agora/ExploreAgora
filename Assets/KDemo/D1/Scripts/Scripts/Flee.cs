using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : SteerBase 

{
    [SerializeField] public SteerBase target;
    protected override Vector3 SteerForce()
    {
        return ((position) - (target.position)) - (velocity);
    }

    private void FixedUpdate()
    {
        UpdateSteer();
    }
}
