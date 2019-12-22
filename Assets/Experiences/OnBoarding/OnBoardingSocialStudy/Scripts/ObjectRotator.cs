using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField] bool isAutoTotate;
    [SerializeField] Vector3 rotVac;
    [SerializeField] float speed;
    float ang;
    void Start()
    {
        ang = speed;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!isAutoTotate)
            return;
        //ang = Mathf.Lerp(ang, ang + speed, 0.2f);
        if (ang >= 180)
            ang = 0;
        Rotate(ang);
    }
    public void Rotate(float _Ang) {
        rotVac.Normalize();
        transform.Rotate(rotVac, _Ang);
    }
}
