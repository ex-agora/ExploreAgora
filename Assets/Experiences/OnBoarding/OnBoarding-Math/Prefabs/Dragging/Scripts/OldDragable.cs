﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class OldDragable : MonoBehaviour
{
    #region public variables
    public LayerMask GroundLayers;
    public Collider Target;
    public bool Snap;
    public bool TriggerEventsOnMouseUp = false;
    Vector3 hitpPointVec;
    [SerializeField] DraggableAxis axis;
    public UnityEvent OnTargetHit; //When the draggable object is dragged to the target position.
    public UnityEvent OnTargetMiss; //When the draggable object is dragged to a wrong position.
    [SerializeField] bool isReturn;
    [SerializeField] GameEvent onDragEnd;

    [SerializeField] Transform clippingTargetMin;
    [SerializeField] Transform clippingTargetMax;
    #endregion

    #region private variables
    private Vector3 initialPosition;
    Vector3 dir;
    private RaycastHit hitInfo;
    private bool dragged;
    private bool insideDraggingArea;
    private bool canBeDragged = true;
    private Coroutine ReturnToPositionCoroutine;
    bool isDragStop  = false;
    public bool CanBeDragged
    {
        get
        {
            return canBeDragged;
        }

        set
        {
            canBeDragged = value;
        }
    }

    public bool IsDragStop { get => isDragStop; set => isDragStop = value; }
    #endregion

    private void OnEnable()
    {
        initialPosition = transform.localPosition;
    }
    public void StopDrag() {
        IsDragStop = true;
    }
    private void OnMouseDown()
    {
        if (IsDragStop)
            return;
        if (CanBeDragged)
            AudioManager.Instance.Play ("UIAction", "UI");

    }
    void OnMouseDrag()
    {
        if (IsDragStop)
            return;

        if (CanBeDragged)
        {
            Ray ray = interactions.Instance.SessionOrigin.camera.ScreenPointToRay(Input.mousePosition);
            Vector3 origin = interactions.Instance.SessionOrigin.camera.ScreenToWorldPoint(Input.mousePosition);
            /*  if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, ~GroundLayers))
                  return;*/
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, GroundLayers))
            {
                //hitpPointVec = transform.worldToLocalMatrix.MultiplyPoint3x4(hitInfo.point);
                //hitpPointVec = hitInfo.collider.transform.InverseTransformPoint(hitInfo.point);
                float ang = Vector3.Angle(hitInfo.transform.forward, Vector3.forward);
                hitpPointVec = Quaternion.Euler(ang * Vector3.up) * hitpPointVec;
                hitpPointVec = hitInfo.point;
                hitpPointVec.x -= hitInfo.transform.position.x;
                hitpPointVec.y -= hitInfo.transform.position.y;
                hitpPointVec.z -= hitInfo.transform.position.z;
                //hitpPointVec.Scale(-dir);
                //Debug.LogError(ang);
                //Debug.Log(hitpPointVec.z);
                switch (axis)
                {
                    case DraggableAxis.X_Axis:
                        hitpPointVec = new Vector3(hitpPointVec.x - hitInfo.transform.position.x, transform.localPosition.y , transform.localPosition.z);
                        //transform.Rotate(Vector3.forward, ang);
                        //transform.Rotate(Vector3.forward, -ang);
                        //hitpPointVec = Quaternion.Euler(-ang * Vector3.up) * hitpPointVec;
                        //hitpPointVec.Scale(dir);
                        //var f = hitInfo.normal;
                        transform.localPosition = hitpPointVec;
                        break;
                    case DraggableAxis.Y_Axis:
                        hitpPointVec = new Vector3(transform.position.x, hitpPointVec.y+ hitInfo.transform.position.y , transform.position.z);
                        //hitpPointVec = Quaternion.AngleAxis(ang, Vector3.right) * hitpPointVec;
                        transform.position = hitpPointVec;
                        break;
                    case DraggableAxis.Z_Axis:
                        hitpPointVec = new Vector3(transform.localPosition.x, transform.localPosition.y, hitpPointVec.z - hitInfo.transform.localPosition.z);
                        //hitpPointVec = Quaternion.AngleAxis(ang, Vector3.up) * hitpPointVec;
                        //transform.position = Mathf.Clamp(cursorPosition.z, clippingTargetMin.position.z, clippingTargetMax.position.z);
                        transform.localPosition = hitpPointVec;
                        break;
                    case DraggableAxis.XZ_Surface:
                        //hitpPointVec = Quaternion.Euler(-ang * Vector3.up) * hitpPointVec;
                        hitpPointVec = new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z);
                        transform.position = hitpPointVec;
                        break;
                }
                //transform.position = hitpPointVec;
                //transform.position = new Vector3(transform.position.x, transform.position.y, hitInfo.point.z);
                //transform.up = hitInfo.normal;
            }
        }

    }

    public void Exit()
    {
        if ((TriggerEventsOnMouseUp && insideDraggingArea))
        {
            if (ReturnToPositionCoroutine != null) StopCoroutine(ReturnToPositionCoroutine);
            dragged = true;
            CanBeDragged = false;
            OnTargetHit.Invoke();
            if (Snap) transform.position = Target.transform.position;
            //AudioManager.Instance?.Play("placeObject", "Activity");
        }
        else if (!dragged && CanBeDragged)
        {
            if (ReturnToPositionCoroutine != null) StopCoroutine(ReturnToPositionCoroutine);
            if (isReturn)
                ReturnToPositionCoroutine = StartCoroutine(returnToPosition(initialPosition));
            OnTargetMiss.Invoke();
        }
    }


    private void OnMouseUp()
    {
        if (IsDragStop)
            return;
        if (onDragEnd != null)
            onDragEnd.Raise();
        Exit();
        //if (TriggerEventsOnMouseUp && insideDraggingArea)
        //{
        //    if (ReturnToPositionCoroutine != null) StopCoroutine(ReturnToPositionCoroutine);
        //    dragged = true;
        //    canBeDragged = false;
        //    OnTargetHit.Invoke();
        //    if (Snap) transform.position = Target.transform.position;
        //}
        //else if (!dragged && canBeDragged)
        //{
        //    if (ReturnToPositionCoroutine != null) StopCoroutine(ReturnToPositionCoroutine);
        //    ReturnToPositionCoroutine = StartCoroutine(returnToPosition(initialPosition));
        //    OnTargetMiss.Invoke();
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Target == null)
            return;
        if (TriggerEventsOnMouseUp)
        {
            if (other.gameObject.Equals(Target.gameObject) && CanBeDragged)
            {
                insideDraggingArea = true;
            }
        }
        else
        {
            if (other.gameObject.Equals(Target.gameObject) && CanBeDragged)
            {
                if (ReturnToPositionCoroutine != null) StopCoroutine(ReturnToPositionCoroutine);
                dragged = true;
                CanBeDragged = false;
                OnTargetHit.Invoke();
                if (Snap) transform.position = Target.transform.position;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Target == null)
            return;
        if (other.gameObject.Equals(Target.gameObject))
        {
            insideDraggingArea = false;
        }
    }

    IEnumerator returnToPosition(Vector3 pos)
    {
        CanBeDragged = false;
        float duration = 0.5f;
        float startTime = Time.time;
        Vector3 currPosition = transform.localPosition;
        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            transform.localPosition = Vector3.Lerp(currPosition, pos, t);
            yield return null;
        }
        transform.localPosition = pos;
        CanBeDragged = true;
    }
}
