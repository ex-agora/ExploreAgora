using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : Seek
{
    [SerializeField] float circulShift;
    [SerializeField] float angleChangeRate;
    float wanderAngle;
    void setAngle(ref Vector3 vec, float angle) {
        float len = vec.magnitude;
        vec.x = Mathf.Cos(angle) * len;
        vec.z = Mathf.Sin(angle) * len;
    }
    
    protected override Vector3 SteerForce()
    {
        Vector3 wanderCir = velocity.normalized * circulShift;
        Vector3 displacement = new Vector3(0, 0, -1) * circulShift;
        setAngle(ref displacement, wanderAngle);
        wanderAngle = (Random.value * angleChangeRate) - (angleChangeRate * 0.5f);
        Vector3 wanderForce = wanderCir + displacement;
        return ((wanderForce) - (position)) - (velocity);
    }

}
