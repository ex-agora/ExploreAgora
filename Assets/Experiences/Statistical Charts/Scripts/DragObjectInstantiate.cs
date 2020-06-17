using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DragObjectInstantiate : MonoBehaviour
{
    [SerializeField] private string targetString;
    [SerializeField] private Transform targetParent = null;
    private Collider placedObjectCollider;

    private void Start()
    {
        placedObjectCollider = GetComponent<Collider>();
    }

    public string TargetString { get => targetString; set => targetString = value; }

    public void PlaceObject(GameObject _object, bool _overrideScaleWithParent = false)
    {
        if (targetParent == null)
            targetParent = transform;

        if (_overrideScaleWithParent)
            Instantiate(_object, targetParent);
        else
        {
            var obj = Instantiate(_object, targetParent.position,Quaternion.identity);
            obj.transform.parent = targetParent;
        }

        placedObjectCollider.enabled = false;
        this.enabled = false;
    }
}