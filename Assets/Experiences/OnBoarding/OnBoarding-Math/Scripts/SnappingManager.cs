using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnappingManager : MonoBehaviour
{
    
    public Answers answers;
    public Transform currentTriggeredPoint;
    public Transform NextPointToCurrentPoint;
    [SerializeField] List<Transform> Points;
    public ForwardsBackwardDetector forwardsBackwardDetector;
    public Transform ss , indicator;
    public bool isBetween, isForward;
    int index;
    [SerializeField] GameEvent EndQuiz;
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
            //Debug.Log("DistanceToCenter: "+ DistanceToCenter  + " - currentDistance: "+ currentDistance + " = " + (DistanceToCenter - currentDistance));
            if ((DistanceToCenter - currentDistance) > currentDistance)
            {
                //Debug.Log("snap To next: " + NextPointToCurrentPoint.name);
                indicator.position = new Vector3(indicator.position.x, indicator.position.y, NextPointToCurrentPoint.position.z);
            }
            else
            {
                //Debug.Log("snap To current: "+ currentTriggeredPoint.name);
                indicator.position = new Vector3(indicator.position.x, indicator.position.y, currentTriggeredPoint.position.z);
            }
        }
        else
        {
           // Debug.Log("snap To current NotBetween: " + currentTriggeredPoint.name);
            indicator.position = new Vector3(indicator.position.x, indicator.position.y, currentTriggeredPoint.position.z);
        }
    }

    public void answerCheck()
    {
        
        if (currentTriggeredPoint.GetComponent<PointsTriggers>().answers != answers)
            return;
        else
        {
            OnBoardingMathGameManager.Instance.score++;
            // indicator.GetComponent<OldDragable>().enabled = false;
            Destroy(indicator.GetComponent<OldDragable>());
            EndQuiz?.Raise();
            GetComponent<GameEventListener>().enabled = false;
        }
        
    }
   
}
