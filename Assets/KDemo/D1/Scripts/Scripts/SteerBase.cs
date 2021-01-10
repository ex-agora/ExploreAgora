using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteerBase : MonoBehaviour
{
    public float arriveRad;
    public float maxSteer;
    public float maxVelocity;
    public bool isRotDir;
    [HideInInspector]
    public Vector3 steering;
    [HideInInspector]
    public Vector3 velocity;
    public Vector3 position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    // Update is called once per frame
    protected void UpdateSteer()
    {
        steering = Vector3.ClampMagnitude(SteerForce(), maxSteer);
        velocity = Vector3.ClampMagnitude(velocity + steering * Time.fixedDeltaTime, maxVelocity);
        position += velocity * Time.fixedDeltaTime;
    }

    protected virtual Vector3 SteerForce() { return Vector3.zero; }
}
