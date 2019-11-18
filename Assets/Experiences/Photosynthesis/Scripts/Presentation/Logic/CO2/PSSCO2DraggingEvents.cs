using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSSCO2DraggingEvents : MonoBehaviour
{
    [SerializeField] SmoothScaling co2Atoms;
    [SerializeField] float scalingDuration;
    public void ScaleCO2 ()
    {
        co2Atoms.StartScaling (scalingDuration);
    }
    public void ResetAtomPosition ()
    {
        transform.position = GetComponent<DraggableOnSurface> ().MyPosition;
    }
}
