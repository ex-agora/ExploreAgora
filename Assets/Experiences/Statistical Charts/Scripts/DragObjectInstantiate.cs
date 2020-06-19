using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DragObjectInstantiate : MonoBehaviour
{
    [SerializeField] private string targetString;
    [SerializeField] private Transform targetParent = null;
    [SerializeField] private bool isOneChild = false;
    [SerializeField] private bool keepObj = false;
    [SerializeField] private GameEvent objectPlacedEvent = null;
    [SerializeField] private bool isPlacedOnce;
    private bool isPlaced;

    private Collider placedObjectCollider;

    private void Start()
    {
        placedObjectCollider = GetComponent<Collider>();
        isPlaced = false;
    }

    public string TargetString { get => targetString; set => targetString = value; }

    public void PlaceObject(GameObject _object, bool _overrideScaleWithParent = false)
    {
        if (targetParent == null)
            targetParent = transform;

        if (isOneChild && targetParent.childCount > 0)
        {
            foreach (Transform item in targetParent)
                Destroy(item.gameObject);
        }

        if (_overrideScaleWithParent)
            Instantiate(_object, targetParent);
        else
        {
            var obj = Instantiate(_object, targetParent.position, Quaternion.identity);
            obj.transform.parent = targetParent;
        }

        if (!keepObj)
        {
            placedObjectCollider.enabled = false;
            this.enabled = false;
        }

        if (!isPlaced)
        {
            objectPlacedEvent?.Raise();
            isPlaced = isPlacedOnce;
        }
    }

    public void UnEnableObject()
    {
        placedObjectCollider.enabled = false;
        this.enabled = false;
    }
}