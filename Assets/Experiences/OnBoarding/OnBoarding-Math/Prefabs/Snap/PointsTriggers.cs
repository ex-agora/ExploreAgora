using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsTriggers : MonoBehaviour
{
    [SerializeField] SnappingManager snappingManager;
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("ASDASDADSA  " + transform.name);
        if (other.name == "Cube")
        {
            snappingManager.setCurrentPoint(transform);
            snappingManager.forwardsBackwardDetector.enabled = false;
            snappingManager.isBetween = false;

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Cube")
        {
            snappingManager.forwardsBackwardDetector.enabled = false;
            snappingManager.setCurrentPoint(transform);
            snappingManager.isBetween = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        snappingManager.forwardsBackwardDetector.enabled = true;
        snappingManager.isBetween = true;
    }
}
