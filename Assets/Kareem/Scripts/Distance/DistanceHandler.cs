using UnityEngine;

public class DistanceHandler : MonoBehaviour
{
    #region Fields
    [SerializeField] private Transform ARCamera;
    private float calculatedDistance;
    private bool correctCheck = true, minCheck = true, maxCheck = true;
    [SerializeField] private GameEvent correctDistance_event;
    [SerializeField] DistanceHandlerTypes distanceHandlerTypes;
    private Vector3 finalDistance;
    [SerializeField] private bool isOnce;
    [SerializeField] private float maximumDistance;
    [SerializeField] private GameEvent MaximumDistance_event;
    [SerializeField] private float minimumDistance;
    [SerializeField] private GameEvent MinimumDistance_event;
    private System.Action state;
    [SerializeField] private Transform Target;
    #endregion Fields
    #region Methods
    #endregion Methods

    Vector3 arCameraPosXZ;
    Vector3 targetPosXZ;
    #region Methods

    private void CorrectDistance_Behavior ()
    {
        print ("CorrectDistance     Main Functionality");
        correctDistance_event.Raise ();
    }

    private void MaximumDistance__Behavior ()
    {
        print ("MaxDistance      Main Functionality");
        MaximumDistance_event.Raise ();
    }

    private void MinimumDistance_Behavior ()
    {
        print ("MinDistance      Main Functionality");
        MinimumDistance_event.Raise ();
    }

    private void OnValidate ()
    {
        if ( minimumDistance == maximumDistance )
        {
            maximumDistance += 1;
        }
        if ( minimumDistance > maximumDistance )
        {
            var t = maximumDistance;
            maximumDistance = minimumDistance;
            minimumDistance = t;
        }

        //if ( !ARCamera || !Target )
        //    Debug.LogError ("AR-camera or target fields cannot be empty");
    }

    // Start is called before the first frame update
    private void Start ()
    {
        maxCheck = true;
        minCheck = true;
        correctCheck = true;
        if (ARCamera == null)
            ARCamera = interactions.Instance.SessionOrigin.camera.transform;
        minimumDistance *= minimumDistance;
        maximumDistance *= maximumDistance;
    }

    // Update is called once per frame
    private void Update ()
    {
        if ( Target.hasChanged || ARCamera.hasChanged )
        {
            if ( distanceHandlerTypes == DistanceHandlerTypes.AllAxes )
            {

                finalDistance = ARCamera.position - Target.position;
                calculatedDistance = finalDistance.sqrMagnitude;
            }
            else if ( distanceHandlerTypes == DistanceHandlerTypes.XZAxes )
            {
                arCameraPosXZ = new Vector3 (ARCamera.position.x , 0 , ARCamera.position.z);
                targetPosXZ = new Vector3 (Target.position.x , 0 , Target.position.z);
                finalDistance = arCameraPosXZ - targetPosXZ;
                calculatedDistance = finalDistance.sqrMagnitude;
            }
            if ( isOnce )
            {
                if ( calculatedDistance > maximumDistance && maxCheck )
                {
                    print ("max    oneTime");
                    state = MaximumDistance__Behavior;
                    maxCheck = false;
                    minCheck = true;
                    correctCheck = true;
                    state?.Invoke ();
                }
                else if ( calculatedDistance < minimumDistance && minCheck )
                {
                    print ("min     oneTime");
                    state = MinimumDistance_Behavior;
                    minCheck = false;
                    maxCheck = true;
                    correctCheck = true;
                    state?.Invoke ();
                }
                else if ( calculatedDistance < maximumDistance && calculatedDistance > minimumDistance && correctCheck )
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
                if ( calculatedDistance >= maximumDistance )
                {
                    print ("max        Not Once");
                    state = MaximumDistance__Behavior;
                }
                else if ( calculatedDistance <= minimumDistance )
                {
                    print ("min       Not Once");
                    state = MinimumDistance_Behavior;
                }
                else if ( calculatedDistance <= maximumDistance && calculatedDistance >= minimumDistance )
                {
                    print ("correct          Not Once");
                    state = CorrectDistance_Behavior;
                }
                state?.Invoke ();
            }
            Target.hasChanged = false;
            ARCamera.hasChanged = false;
        }
    }

    #endregion Methods
}