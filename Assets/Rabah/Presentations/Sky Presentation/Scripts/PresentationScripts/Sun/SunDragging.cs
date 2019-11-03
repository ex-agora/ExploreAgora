using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunDragging : MonoBehaviour
{
    public void ResetSunPosition ()
    {
        transform.position = GetComponent<Draggable> ().MyPosition;
    }
}
