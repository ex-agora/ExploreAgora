using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBetweenTriggers : MonoBehaviour
{
    [SerializeField] SnappingManager snappingManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Cube")
            snappingManager.isBetween = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Cube")
            snappingManager.isBetween = false;
    }
}
