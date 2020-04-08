using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperCoalFixer : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    bool isWork = false;
    private void FixedUpdate()
    {
        if (isWork)
            rb.AddForce(Vector3.up * -9.18f);
    }
    public void ActiveRB() {
        isWork = true;
        rb.useGravity = true;
    }

}
