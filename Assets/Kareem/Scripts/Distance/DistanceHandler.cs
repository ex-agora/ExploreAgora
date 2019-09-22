using UnityEngine;

public class DistanceHandler : MonoBehaviour
{
    #region Fields
    [SerializeField] private Transform ARCamera;
    [SerializeField] private Transform Target;
    [SerializeField] private GameEvent correctDistance_event;
    [SerializeField] private bool isOnce;
    [SerializeField] private float maximumDistance;
    [SerializeField] private GameEvent MaximumDistance_event;
    [SerializeField] private float minimumDistance;
    [SerializeField] private GameEvent MinimumDistance_event;

    private Vector3 finalDistance;
    private float calculatedDistance;
    private bool correctCheck = true, minCheck = true, maxCheck = true;
    private System.Action state;

    #endregion Fields

    #region Methods

    private void CorrectDistance_Behavior()
    {
        print("CorrectDistance     Main Functionality");
        correctDistance_event.Raise();
    }

    private void MaximumDistance__Behavior()
    {
        print("MaxDistance      Main Functionality");
        MaximumDistance_event.Raise();
    }

    private void MinimumDistance_Behavior()
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

        if (!ARCamera || !Target)
            Debug.LogError("AR-camera or target fields cannot be empty");
    }

    // Start is called before the first frame update
    private void Start()
    {
        minimumDistance *= minimumDistance;
        maximumDistance *= maximumDistance;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Target.hasChanged || ARCamera.hasChanged)
        {
            finalDistance = ARCamera.position - Target.position;
            calculatedDistance = finalDistance.sqrMagnitude;

            if (isOnce)
            {
                if (calculatedDistance > maximumDistance && maxCheck)
                {
                    print("max    oneTime");
                    state = MaximumDistance__Behavior;
                    maxCheck = false;
                    minCheck = true;
                    correctCheck = true;
                    state?.Invoke();
                }
                else if (calculatedDistance < minimumDistance && minCheck)
                {
                    print("min     oneTime");
                    state = MinimumDistance_Behavior;
                    minCheck = false;
                    maxCheck = true;
                    correctCheck = true;
                    state?.Invoke();
                }
                else if (calculatedDistance < maximumDistance && calculatedDistance > minimumDistance && correctCheck)
                {
                    print("correct        oneTime");
                    state = CorrectDistance_Behavior;
                    correctCheck = false;
                    minCheck = true;
                    maxCheck = true;
                    state?.Invoke();
                }
            }
            else
            {
                if (calculatedDistance >= maximumDistance)
                {
                    print("max        Not Once");
                    state = MaximumDistance__Behavior;
                }
                else if (calculatedDistance <= minimumDistance)
                {
                    print("min       Not Once");
                    state = MinimumDistance_Behavior;
                }
                else if (calculatedDistance <= maximumDistance && calculatedDistance >= minimumDistance)
                {
                    print("correct          Not Once");
                    state = CorrectDistance_Behavior;
                }
                state?.Invoke();
            }
            Target.hasChanged = false;
            ARCamera.hasChanged = false;
        }
    }

    #endregion Methods
}