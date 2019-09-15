using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class DistanceHandler : MonoBehaviour
{

    float calculatedDistance;
    bool correctCheck = true, minCheck = true, maxCheck = true;
    Vector3 finalDistance;

    delegate void StateDelegate ();
    StateDelegate state;

    [SerializeField] Transform Target;
    [SerializeField] Transform ARcamera;
    [SerializeField] bool isOnce;
    [SerializeField] float minimumDistance;
    [SerializeField] float maximumDistance;
    [SerializeField] GameEvent correctDistance_event;
    [SerializeField] GameEvent MaximumDistance_event;
    [SerializeField] GameEvent MinimumDistance_event;
   

    // Start is called before the first frame update
    void Start ()
    {
        minimumDistance *= minimumDistance;
        maximumDistance *= maximumDistance;
    }

    // Update is called once per frame
    void Update ()
    {
        if (Target.hasChanged || ARcamera.hasChanged)
        {
            finalDistance = ARcamera.position - Target.position;
            calculatedDistance = finalDistance.sqrMagnitude;

            if (isOnce)
            {
                if (calculatedDistance > maximumDistance && maxCheck)
                {
                    print ("max    oneTime");
                    state = MaximumDistance__Behavior;
                    maxCheck = false;
                    minCheck = true;
                    correctCheck = true;
                    state?.Invoke ();
                }
                else if (calculatedDistance < minimumDistance && minCheck)
                {
                    print ("min     oneTime");
                    state = MinimumDistance_Behavior;
                    minCheck = false;
                    maxCheck = true;
                    correctCheck = true;
                    state?.Invoke ();
                }
                else if (calculatedDistance < maximumDistance && calculatedDistance > minimumDistance && correctCheck)
                {
                    print ("correct        oneTime");
                    state = CorrectDistance_Behavior;
                    correctCheck = false;
                    minCheck = true;
                    maxCheck = true;
                    state?.Invoke ();
                }

            }
            else
            {
                if (calculatedDistance >= maximumDistance)
                {
                    print ("max        Not Once");
                    state = MaximumDistance__Behavior;
                }
                else if (calculatedDistance <= minimumDistance)
                {
                    print ("min       Not Once");
                    state = MinimumDistance_Behavior;
                }
                else if (calculatedDistance <= maximumDistance && calculatedDistance >= minimumDistance)
                {
                    print ("correct          Not Once");
                    state = CorrectDistance_Behavior;
                }
                state?.Invoke ();
            }
            Target.hasChanged = false;
            ARcamera.hasChanged = false;

        }

    }
   
    private void OnValidate ()
    {
        if (minimumDistance == maximumDistance)
        {
            maximumDistance += 1;
        }
        if (minimumDistance > maximumDistance)
        {
            var t = maximumDistance;
            maximumDistance = minimumDistance;
            minimumDistance = t;
        }

        if (!ARcamera || !Target)
            Debug.LogError ("AR-camera or target fields cannot be empty");
    }
    void CorrectDistance_Behavior ()
    {
        print ("CorrectDistance     Main Functionality");
        correctDistance_event.Raise ();
    }
    void MinimumDistance_Behavior ()
    {
        print ("MinDistance      Main Functionality");
        MinimumDistance_event.Raise ();
    }
    void MaximumDistance__Behavior ()
    {
        print ("MaxDistance      Main Functionality");
        MaximumDistance_event.Raise ();
    }
}