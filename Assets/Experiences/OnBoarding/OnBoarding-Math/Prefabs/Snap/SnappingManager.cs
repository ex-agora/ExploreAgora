using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnappingManager : MonoBehaviour
{
    public Transform currentTriggeredPoint;
    public Transform NextPointToCurrentPoint;
    [SerializeField] List<Transform> Points;
    public ForwardsBackwardDetector forwardsBackwardDetector;
    public Transform ss , Cube;
    public bool isBetween, isForward;
    int index;

    public void setCurrentPoint(Transform point)
    {
        currentTriggeredPoint = point;
        if (currentTriggeredPoint == Points[Points.Count - 1])
        {
            NextPointToCurrentPoint = Points[Points.Count - 2];
        }
        else if (currentTriggeredPoint == Points[0])
        {
            NextPointToCurrentPoint = Points[1];
        }
        else
        {
            //////backward , forward
            if (isForward)
            {
                index = Points.IndexOf(currentTriggeredPoint);
                NextPointToCurrentPoint = Points[index + 1];
            }
            else
            {
                index = Points.IndexOf(currentTriggeredPoint);
                NextPointToCurrentPoint = Points[index - 1];
            }
        }
    }

    public void EndDraggingAction()
    {
        forwardsBackwardDetector.enabled = false;
        if (isBetween)
        {
            float DistanceToCenter = Vector3.Distance((NextPointToCurrentPoint.position), (currentTriggeredPoint.position));
            float currentDistance = Vector3.Distance((NextPointToCurrentPoint.position), (ss.position));
            Debug.Log("DistanceToCenter: "+ DistanceToCenter  + " - currentDistance: "+ currentDistance + " = " + (DistanceToCenter - currentDistance));
            if ((DistanceToCenter - currentDistance) > currentDistance)
            {
                Debug.Log("snap To next: " + NextPointToCurrentPoint.name);
                Cube.position = new Vector3(Cube.position.x, Cube.position.y, NextPointToCurrentPoint.position.z);
            }
            else
            {
                Debug.Log("snap To current: "+ currentTriggeredPoint.name);
                Cube.position = new Vector3(Cube.position.x, Cube.position.y, currentTriggeredPoint.position.z);
            }
        }
        else
        {
            Debug.Log("snap To current NotBetween: " + currentTriggeredPoint.name);
            Cube.position = new Vector3(Cube.position.x, Cube.position.y, currentTriggeredPoint.position.z);
        }
    }

   
}
