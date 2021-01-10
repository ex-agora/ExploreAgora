using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidance : Seek
{
    [SerializeField] float maxAvoidForce;
    RaycastHit hitBuffer;
    [SerializeField] float dis;
    [SerializeField]
    Vector3 force;
    int mask;
    private void Start()
    {
        mask = LayerMask.GetMask("Obs");
    }
    protected override Vector3 SteerForce()
    {
       return base.SteerForce() -  collisionForce();
        
    }
    Vector3 collisionForce() {
         force = Vector3.zero;
        Vector3 norDir = velocity;
        Vector3 temp;
        for (int i = -1; i < 2; i++) {
            temp = setAngle(norDir, -20 * i);
            Physics.Raycast(position, temp*dis, out hitBuffer, dis, mask);
            if (hitBuffer.collider !=null)
            {
                force =new Vector3(force.x + temp.x, 0, force.z - temp.z);
               
            }
            Debug.DrawRay(position, temp * dis, Color.red);
        }
        force = Vector3.ClampMagnitude(force, -maxAvoidForce);
        return force*10000;
        
    }

    Vector3 setAngle( Vector3 vec, float angle)
    {
        float len = vec.magnitude;
        vec.x = Mathf.Cos(angle) * len;
        vec.z = Mathf.Sin(angle) * len;
        return vec;
    }
}
