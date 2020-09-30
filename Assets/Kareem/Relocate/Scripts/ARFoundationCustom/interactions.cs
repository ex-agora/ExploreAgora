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
    bool isPlaced;
    [SerializeField] bool isFoundedOnce;
    [SerializeField] QuickFadeHandler fadeHandler = null;
    static interactions instance;
    private MeshRenderer meshRenderer;

    public ARSessionOrigin SessionOrigin { get => sessionOrigin; set => sessionOrigin = value; }
    public static interactions Instance { get => instance; set => instance = value; }

    private void Awake ()
    {

        if (Instance == null)
            Instance = this;
    }
    private void Start ()
    {
        meshRenderer = planeTarget.GetComponentInChildren<MeshRenderer> ();
        /*  arOrigin = FindObjectOfType<ARRaycastManager> ();
          aRPlaneManager = FindObjectOfType<ARPlaneManager> ();*/
        // Arcamera.transform.localScale = new Vector3 (targetSize.x, targetSize.y, Arcamera.transform.localScale.z);
        indicator.transform.localScale = new Vector3 (targetSize.x, targetSize.y, indicator.transform.localScale.z);
    }

    private void Update ()
    {
        UpdateTargetPoSe ();
    }

    public void placeTheObject ()
    {
        isPlaced = true;
        if (firstTime)
        {
            objectToPlaceParent.transform.position = targetPose.position;
            objectToPlaceParent.transform.rotation = targetPose.rotation;
            GameObject obj = Instantiate (objectToPlace, targetPose.position, targetPose.rotation);
            //obj.transform.localScale = new Vector3(targetSize.x, 0.2f, targetSize.y);
            objectToPlace = null;
            Destroy(new GameObject());
            Resources.UnloadUnusedAssets();
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
        if (fadeHandler)
            fadeHandler.FadeOut();
        planeDetectionController.TogglePlaneDetection ();
        planeTarget.SetActive (false);
        if (!(relocate is null))
        {
            relocate.interactable = true;
            relocate.GetComponent<RelocateEventEnableDisableAction>()?.FireEvent();
        }

        AudioManager.Instance.Play ("placeObject", "Activity");
    }

    public void OpenDetection()
    { 
        planeDetectionController.TogglePlaneDetection ();
        planeTarget.SetActive (true);
    }
    public void CloseDetection()
    { 
        planeDetectionController.TogglePlaneDetection ();
        planeTarget.SetActive (false);
    }
    private void UpdateTargetPoSe ()
    {
        if (SessionOrigin is null || arOrigin is null)
            return;
        var screenCenter = SessionOrigin.camera.ViewportToScreenPoint (new Vector3 (0.5f, 0.5f));
        var hits = new List<ARRaycastHit> ();
        arOrigin.Raycast (screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);
        planeFound = hits.Count > 0;

        if (planeFound)
        {
            if ((!isFoundedOnce || !isSurfaceFound) && !(foundSurface is null))
            {
                isSurfaceFound = true;
                if (foundSurface)
                    foundSurface.Raise();
            }
            //planeTarget.SetActive(true);
            planeTarget.transform.SetPositionAndRotation (targetPose.position, targetPose.rotation);
            //Debug.Log(planeFound);
            targetPose = hits [0].pose;
            var cameraForward = SessionOrigin.camera.transform.forward;
            var cameraBearing = new Vector3 (cameraForward.x, 0, cameraForward.z).normalized;
            targetPose.rotation = Quaternion.LookRotation (cameraBearing);
            ARPlane rrr = aRPlaneManager.GetPlane (hits [0].trackableId);
            if (rrr.size.x >= targetSize.x && rrr.size.y >= targetSize.y)
            {
                // state.text = "Found";
                if (!isPlaced)
                    if ((fadeHandler))
                        fadeHandler.FadeIn();
                meshRenderer.material = mats [0];
                canSet = true;
            }
            else
            {
                //Debug.Log(rrr.size);
                // state.text = "Lost";
                if ((fadeHandler))
                    fadeHandler.FadeOut();
                canSet = false;
                meshRenderer.material = mats [1];
            }

        }
    }

    public void hideShowArComponents (bool state)
    {
        //objectToPlace.SetActive (state);
        planeTarget.SetActive (state);
    }
}