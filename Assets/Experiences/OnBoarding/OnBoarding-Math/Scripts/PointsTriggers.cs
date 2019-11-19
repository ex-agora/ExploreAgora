using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsTriggers : MonoBehaviour
{

    public Answers answers;  
    [SerializeField] SnappingManager snappingManager;
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("ASDASDADSA  " + transform.name + "  " + other.name);
        if (other.GetComponent<IndicatorType>().type == IndicatorType.Type.indicator)
        {
            snappingManager.setCurrentPoint(transform);
            snappingManager.forwardsBackwardDetector.enabled = false;
            snappingManager.isBetween = false;

        }
    }
    private void OnTriggerStay(Collider other)
    {

        Debug.Log("stay  " + transform.name + "  " + other.name);
        if (other.GetComponent<IndicatorType>().type == IndicatorType.Type.indicator)
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
