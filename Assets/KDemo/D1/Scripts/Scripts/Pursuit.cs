using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursuit : Seek
{
    [SerializeField] float pointShift;
    Vector3 SeekPoint() {
        Vector3 pos = target.transform.position + target.velocity * pointShift;
        return pos;
    }
    protected override Vector3 SteerForce()
    {
        return ((SeekPoint()) - (position)) - (velocity);
    }
}
