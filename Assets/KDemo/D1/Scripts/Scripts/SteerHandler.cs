using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteerHandler 
{
    public float maxSteer;
    public float maxVelocity;
    Transform transform;
    Vector3 steering;
    Vector3 velocity;
    public float arriveRad = 1.0f;
    public Vector3 position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public SteerHandler(Transform t) {
        transform = t;
    }

    // Update is called once per frame
    protected void UpdateSteer()
    {
        steering = Vector3.ClampMagnitude(SteerForce(), maxSteer);
        velocity = Vector3.ClampMagnitude(velocity + steering * Time.fixedDeltaTime, maxVelocity);
        position += velocity * Time.fixedDeltaTime;
    }

    public Vector3 Arrive(Transform target) {
        
        Vector3 ve;
        float dis;
        ve = (target.position) - (position);
        dis = ve.magnitude;
        if (dis < arriveRad)
        {

            ve *= dis / arriveRad;
        }

        return ve - velocity;
    }

    public Vector3 Flee(Transform target) {
        return ((position) - (target.position)) - (velocity);
    }

 
    protected virtual Vector3 SteerForce() { return Vector3.zero; }

}
