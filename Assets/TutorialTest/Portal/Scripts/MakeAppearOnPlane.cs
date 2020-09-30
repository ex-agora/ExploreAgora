using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// Moves the ARSessionOrigin in such a way that it makes the given content appear to be
/// at a given location acquired via a raycast.
/// </summary>
[RequireComponent(typeof(ARSessionOrigin))]
[RequireComponent(typeof(ARRaycastManager))]
public class MakeAppearOnPlane : MonoBehaviour
{
    [FormerlySerializedAs("m_Content")]
    [SerializeField]
    [Tooltip("A transform which should be made to appear to be at the touch point.")]
    Transform mContent;

    /// <summary>
    /// A transform which should be made to appear to be at the touch point.
    /// </summary>
    public Transform Content
    {
        get => mContent;
        set => mContent = value;
    }

    [FormerlySerializedAs("m_Rotation")]
    [SerializeField]
    [Tooltip("The rotation the content should appear to have.")]
    Quaternion mRotation;

    /// <summary>
    /// The rotation the content should appear to have.
    /// </summary>
    public Quaternion rotation
    {
        get { return mRotation; }
        set
        {
            mRotation = value;
            if (_mSessionOrigin != null)
                _mSessionOrigin.MakeContentAppearAt(Content, Content.transform.position, mRotation);
        }
    }

    private void Awake()
    {
        _mSessionOrigin = GetComponent<ARSessionOrigin>();
        _mRaycastManager = GetComponent<ARRaycastManager>();
    }

    public void Update()
    {
        if (Input.touchCount == 0 || mContent == null)
            return;

        var touch = Input.GetTouch(0);

        if (!_mRaycastManager.Raycast(touch.position, SHits, TrackableType.PlaneWithinPolygon)) return;
        // Raycast hits are sorted by distance, so the first one
        // will be the closest hit.
        var hitPose = SHits[0].pose;

        // This does not move the content; instead, it moves and orients the ARSessionOrigin
        // such that the content appears to be at the raycast hit position.
        _mSessionOrigin.MakeContentAppearAt(Content, hitPose.position, mRotation);
    }

    static readonly List<ARRaycastHit> SHits = new List<ARRaycastHit>();

    ARSessionOrigin _mSessionOrigin;

    ARRaycastManager _mRaycastManager;
}
