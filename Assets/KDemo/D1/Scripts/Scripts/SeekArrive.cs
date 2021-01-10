using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekArrive : Seek
{
    [SerializeField] float arriveRad = 1.0f;
    Vector3 ve;
    float dis;
    protected override Vector3 SteerForce()
    {
        
        ve = (target.position) - (position);
        dis = ve.magnitude;
        if (dis < arriveRad)
        {
            
            ve *= dis/ arriveRad;
        }
        
        return ve -velocity;
    }
}
