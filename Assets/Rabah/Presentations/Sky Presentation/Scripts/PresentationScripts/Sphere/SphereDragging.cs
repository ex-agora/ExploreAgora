using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereDragging : MonoBehaviour
{
    public void ResetSpherePosition ()
    {
        transform.position = GetComponent<DraggableOnSurface> ().MyPosition;
    }
}
