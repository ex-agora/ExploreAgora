using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class interactions : MonoBehaviour
{

    [SerializeField] GameObject planeTarget, objectToPlace, objectToPlaceParent, indicator;
    // [SerializeField] Text state;
    [SerializeField] Vector2 targetSize;
    [SerializeField] ARSessionOrigin sessionOrigin;
    [SerializeField] Material [] mats;
    [SerializeField] GameEvent objectedPlaced;
    [SerializeField] GameEvent foundSurface;
    public ARRaycastManager arOrigin;
    public ARPlaneManager aRPlaneManager;
    [SerializeField] PlaneDetectionController planeDetectionController;
    [SerializeField] Button relocate;
    private Pose targetPose;
    bool planeFound = false;
    public bool canSet;
    bool isSurfaceFound;
    bool firstTime = true;
    [SerializeField] bool isFoundedOnce;
    [SerializeField] QuickFadeHandler fadeHandler = null;
    static interactions instance;

    public ARSessionOrigin SessionOrigin { get => sessionOrigin; set => sessionOrigin = value; }
    public static interactions Instance { get => instance; set => instance = value; }

    private void Awake ()
    {

        if (Instance == null)
            Instance = this;
    }
    private void Start ()
    {
        /*  arOrigin = FindObjectOfType<ARRaycastManager> ();
          aRPlaneManager = FindObjectOfType<ARPlaneManager> ();*/
        // Arcamera.transform.localScale = new Vector3 (targetSize.x, targetSize.y, Arcamera.transform.localScale.z);
        indicator.transform.localScale = new Vector3 (targetSize.x, targetSize.y, indicator.transform.localScale.z);
    }
    void Update ()
    {
        UpdateTargetPoSe ();
    }

    public void placeTheObject ()
    {
        if (firstTime)
        {
            objectToPlaceParent.transform.position = targetPose.position;
            objectToPlaceParent.transform.rotation = targetPose.rotation;
            GameObject obj = Instantiate (objectToPlace, targetPose.position, targetPose.rotation);
            //obj.transform.localScale = new Vector3(targetSize.x, 0.2f, targetSize.y);
            obj.transform.parent = objectToPlaceParent.transform;
            firstTime = false;
            objectedPlaced.Raise ();
        }
        else
        {
            objectToPlaceParent.transform.position = targetPose.position;
            objectToPlaceParent.transform.rotation = targetPose.rotation;
        }
        objectToPlaceParent.SetActive(true);

        planeDetectionController.TogglePlaneDetection ();
        planeTarget.SetActive (false);
        relocate.interactable = true;
        relocate.GetComponent<RelocateEventEnableDisableAction>()?.FireEvent();
        AudioManager.Instance.Play ("placeObject", "Activity");
    }

    private void UpdateTargetPoSe ()
    {
        var screenCenter = SessionOrigin.camera.ViewportToScreenPoint (new Vector3 (0.5f, 0.5f));
        var hits = new List<ARRaycastHit> ();
        arOrigin.Raycast (screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);
        planeFound = hits.Count > 0;

        if (planeFound)
        {
            if ((!isFoundedOnce || !isSurfaceFound) && foundSurface != null)
            {
                isSurfaceFound = true;
                foundSurface?.Raise ();
            }
            //planeTarget.SetActive(true);
            planeTarget.transform.SetPositionAndRotation (targetPose.position, targetPose.rotation);
            //Debug.Log(planeFound);
            targetPose = hits [0].pose;
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3 (cameraForward.x, 0, cameraForward.z).normalized;
            targetPose.rotation = Quaternion.LookRotation (cameraBearing);
            ARPlane rrr = aRPlaneManager.GetPlane (hits [0].trackableId);
            if (rrr.size.x >= targetSize.x && rrr.size.y >= targetSize.y)
            {
                // state.text = "Found";
                fadeHandler?.FadeIn();
                planeTarget.GetComponentInChildren<MeshRenderer> ().material = mats [0];
                canSet = true;
            }
            else
            {
                //Debug.Log(rrr.size);
                // state.text = "Lost";
                fadeHandler?.FadeOut();
                canSet = false;
                planeTarget.GetComponentInChildren<MeshRenderer> ().material = mats [1];
            }

        }
    }

    public void hideShowArComponents (bool state)
    {
        objectToPlace.SetActive (state);
        planeTarget.SetActive (state);
    }
}