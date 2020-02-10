using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class TestingScript : MonoBehaviour
{

    #region Fields
    public Transform Target, ARcamera;
    float calculatedDistance;
    bool correctCheck = true, minCheck = true, maxCheck = true;
    [SerializeField] GameEvent correctDistance_event;
    Vector3 finalDistance;
    [SerializeField] bool isOnce;
    [SerializeField] float maximumDistance;

    [SerializeField] GameEvent MaximumDistance_event;

    [SerializeField] float minimumDistance;

    [SerializeField] GameEvent MinimumDistance_event;

    StateDelegate state;
    #endregion Fields

    #region Delegates
    delegate void StateDelegate();
    #endregion Delegates

    // [SerializeField] ARSession arSession;

    #region Methods
    void CorrectDistance()
    {
        print("CorrectDistance     Main Functionality");
        correctDistance_event.Raise();
    }

    void MaxDistance()
    {
        print("MaxDistance      Main Functionality");
        MaximumDistance_event.Raise();
    }

    void MinDistance()
    {
        print("MinDistance      Main Functionality");
        MinimumDistance_event.Raise();
    }

    private void OnValidate()
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
            Debug.LogError("asdsaasdaasda");
    }

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
                    state = MaxDistance;
                    maxCheck = false;
                    minCheck = true;
                    correctCheck = true;
                    state?.Invoke ();
                }
                else if (calculatedDistance < minimumDistance && minCheck)
                {
                    print ("min     oneTime");
                    state = MinDistance;
                    minCheck = false;
                    maxCheck = true;
                    correctCheck = true;
                    state?.Invoke ();
                }
                else if (calculatedDistance < maximumDistance && calculatedDistance > minimumDistance && correctCheck)
                {
                    print ("correct        oneTime");
                    state = CorrectDistance;
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
                    state = MaxDistance;
                }
                else if (calculatedDistance <= minimumDistance)
                {
                    print ("min       Not Once");
                    state = MinDistance;
                }
                else if (calculatedDistance <= maximumDistance && calculatedDistance >= minimumDistance)
                {
                    print ("correct          Not Once");
                    state = CorrectDistance;
                }
                state?.Invoke ();
            }
            Target.hasChanged = false;
            ARcamera.hasChanged = false;

        }

    }
    #endregion Methods
}