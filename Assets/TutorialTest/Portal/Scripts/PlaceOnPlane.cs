using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// Listens for touch events and performs an AR raycast from the screen touch point.
/// AR raycasts will only hit detected trackables like feature points and planes.
///
/// If a raycast hits a trackable, the <see cref="PlacedPrefab"/> is instantiated
/// and moved to the hit position.
/// </summary>
[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnPlane : MonoBehaviour
{
    [FormerlySerializedAs("m_PlacedPrefab")]
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject mPlacedPrefab;

    private readonly List<ARRaycastHit> _sHits;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject PlacedPrefab
    {
        get => mPlacedPrefab;
        set => mPlacedPrefab = value;
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject SpawnedObject { get; private set; }

    private void Start()
    {
        Invoke(nameof(Update),2f);
        _isSpawnedObjectNull = SpawnedObject == null;
    }

    private void Awake()
    {
        _mRaycastManager = GetComponent<ARRaycastManager>();
    }

    private static bool TryGetTouchPosition(out Vector2 touchPosition)
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            var mousePosition = Input.mousePosition;
            touchPosition = new Vector2(mousePosition.x, mousePosition.y);
            return true;
        }
#else
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
#endif

        touchPosition = default;
        return false;
    }

    private void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        if (_mRaycastManager.Raycast(touchPosition, _sHits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = _sHits[0].pose;
            _isSpawnedObjectNull = !((SpawnedObject) is null);
            if ((_isSpawnedObjectNull))
            {
                SpawnedObject = Instantiate(mPlacedPrefab, hitPose.position, hitPose.rotation);
            }
            else
            {
               if (!(SpawnedObject is null))
                    SpawnedObject.transform.position = hitPose.position;
            }
        }
    }

    

    ARRaycastManager _mRaycastManager;
    private bool _isSpawnedObjectNull;

    public PlaceOnPlane(List<ARRaycastHit> sHits)
    {
        _sHits = sHits;
    }
}
