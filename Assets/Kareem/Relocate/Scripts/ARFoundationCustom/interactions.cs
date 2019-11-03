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
    [SerializeField] ARSessionOrigin Arcamera;
    [SerializeField] Material[] mats;
    [SerializeField] GameEvent objectedPlaced;
    public ARRaycastManager arOrigin;
    public ARPlaneManager aRPlaneManager;
    [SerializeField] PlaneDetectionController planeDetectionController;
    [SerializeField] Button relocate;
    private Pose targetPose;
    bool planeFound = false;
    public bool canSet;
    bool firstTime = true;


    private void Start()
    {
        /*  arOrigin = FindObjectOfType<ARRaycastManager> ();
          aRPlaneManager = FindObjectOfType<ARPlaneManager> ();*/
        // Arcamera.transform.localScale = new Vector3 (targetSize.x, targetSize.y, Arcamera.transform.localScale.z);
        indicator.transform.localScale = new Vector3(targetSize.x, targetSize.y, indicator.transform.localScale.z);
    }
    void Update()
    {
        UpdateTargetPoSe();
    }

    public void placeTheObject()
    {
        objectToPlaceParent.SetActive(true);
        if (firstTime)
        {
            objectToPlaceParent.transform.position = targetPose.position;
            objectToPlaceParent.transform.rotation = targetPose.rotation;
            GameObject obj = Instantiate(objectToPlace, targetPose.position, targetPose.rotation);
            //obj.transform.localScale = new Vector3(targetSize.x, 0.2f, targetSize.y);
            obj.transform.parent = objectToPlaceParent.transform;
            firstTime = false;
            objectedPlaced.Raise();
        }
        else
        {
            objectToPlaceParent.transform.position = targetPose.position;
            objectToPlaceParent.transform.rotation = targetPose.rotation;
        }
        planeDetectionController.TogglePlaneDetection();
        planeTarget.SetActive(false);
        relocate.interactable = true;
        AudioManager.Instance.Play("placeObject", "Activity");
    }

    private void UpdateTargetPoSe()
    {
        var screenCenter = Arcamera.camera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        arOrigin.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);
        planeFound = hits.Count > 0;

        if (planeFound)
        {
            //planeTarget.SetActive(true);
            planeTarget.transform.SetPositionAndRotation(targetPose.position, targetPose.rotation);
            //Debug.Log(planeFound);
            targetPose = hits[0].pose;
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            targetPose.rotation = Quaternion.LookRotation(cameraBearing);
            ARPlane rrr = aRPlaneManager.GetPlane(hits[0].trackableId);
            if (rrr.size.x >= targetSize.x && rrr.size.y >= targetSize.y)
            {
                // state.text = "Found";
                planeTarget.GetComponentInChildren<MeshRenderer>().material = mats[0];
                canSet = true;
            }
            else
            {
                //Debug.Log(rrr.size);
                // state.text = "Lost";
                canSet = false;
                planeTarget.GetComponentInChildren<MeshRenderer>().material = mats[1];
            }

        }
    }


    public void hideShowArComponents(bool state)
    {
        objectToPlace.SetActive(state);
        planeTarget.SetActive(state);
    }
}