using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomDragging : MonoBehaviour
{
    public void ResetAtomPosition ()
    {
        transform.position = GetComponent<DraggableOnSurface> ().MyPosition;
    }
}
