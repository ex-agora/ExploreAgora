using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragBoundries : MonoBehaviour
{
    [SerializeField] DraggableAxis axis;
    [SerializeField] Transform minPosition;
    [SerializeField] Transform maxPosition;
    [SerializeField] bool isWorking;
    [SerializeField] Lean.Touch.LeanDragTranslate dragTranslate;
    [SerializeField] bool isPosLocal;
    Vector3 pos = Vector3.zero;
    bool swap = false;
    private void OnEnable()
    {
        if (isPosLocal) {
            switch (axis)
            {
                case DraggableAxis.X_Axis:
                    if (minPosition.localPosition.x > maxPosition.localPosition.x)
                    {
                        swap = true;
                    }

                    break;
                case DraggableAxis.Y_Axis:
                    if (minPosition.localPosition.y > maxPosition.localPosition.y)
                    {
                        swap = true;
                    }
                    break;
                case DraggableAxis.Z_Axis:
                    if (minPosition.localPosition.z > maxPosition.localPosition.z)
                    {
                        swap = true;
                    }
                    break;
                case DraggableAxis.XZ_Surface:
                    pos.x = Mathf.Clamp(transform.localPosition.x, minPosition.localPosition.x, maxPosition.localPosition.x);
                    pos.y = transform.localPosition.y;
                    pos.z = Mathf.Clamp(transform.localPosition.z, minPosition.localPosition.z, maxPosition.localPosition.z);
                    break;
            }
        }
        else
        {
            switch (axis)
            {
                case DraggableAxis.X_Axis:
                    if (minPosition.position.x > maxPosition.position.x)
                    {
                        swap = true;
                    }

                    break;
                case DraggableAxis.Y_Axis:
                    if (minPosition.position.y > maxPosition.position.y)
                    {
                        swap = true;
                    }
                    break;
                case DraggableAxis.Z_Axis:
                    if (minPosition.position.z > maxPosition.position.z)
                    {
                        swap = true;
                    }
                    break;
                case DraggableAxis.XZ_Surface:
                    pos.x = Mathf.Clamp(transform.position.x, minPosition.position.x, maxPosition.position.x);
                    pos.y = transform.position.y;
                    pos.z = Mathf.Clamp(transform.position.z, minPosition.position.z, maxPosition.position.z);
                    break;
            }
        }
        if (swap)
        {
            var temp = minPosition;
            minPosition = maxPosition;
            maxPosition = temp;
            swap = false;
        }
    }
    private void Update()
    {
        if (isPosLocal) {
            if (isWorking)
            {
                switch (axis)
                {
                    case DraggableAxis.X_Axis:
                        pos.x = Mathf.Clamp(transform.localPosition.x, minPosition.localPosition.x, maxPosition.localPosition.x);
                        pos.y = transform.localPosition.y;
                        pos.z = transform.localPosition.z;
                        break;
                    case DraggableAxis.Y_Axis:
                        pos.x = transform.localPosition.x;
                        pos.y = Mathf.Clamp(transform.localPosition.y, minPosition.localPosition.y, maxPosition.localPosition.y);
                        pos.z = transform.localPosition.z;
                        break;
                    case DraggableAxis.Z_Axis:
                        pos.x = transform.localPosition.x;
                        pos.y = transform.localPosition.y;
                        pos.z = Mathf.Clamp(transform.localPosition.z, minPosition.localPosition.z, maxPosition.localPosition.z);
                        break;
                    case DraggableAxis.XZ_Surface:
                        pos.x = Mathf.Clamp(transform.localPosition.x, minPosition.localPosition.x, maxPosition.localPosition.x);
                        pos.y = transform.localPosition.y;
                        pos.z = Mathf.Clamp(transform.localPosition.z, minPosition.localPosition.z, maxPosition.localPosition.z);
                        break;
                }
                transform.localPosition = pos;
            }
        }
        else {
            if (isWorking)
            {
                switch (axis)
                {
                    case DraggableAxis.X_Axis:
                        pos.x = Mathf.Clamp(transform.position.x, minPosition.position.x, maxPosition.position.x);
                        pos.y = transform.position.y;
                        pos.z = transform.position.z;
                        break;
                    case DraggableAxis.Y_Axis:
                        pos.x = transform.position.x;
                        pos.y = Mathf.Clamp(transform.position.y, minPosition.position.y, maxPosition.position.y);
                        pos.z = transform.position.z;
                        break;
                    case DraggableAxis.Z_Axis:
                        pos.x = transform.localPosition.x;
                        pos.y = transform.localPosition.y;
                        pos.z = Mathf.Clamp(transform.localPosition.z, minPosition.localPosition.z, maxPosition.localPosition.z);
                        break;
                    case DraggableAxis.XZ_Surface:
                        pos.x = Mathf.Clamp(transform.position.x, minPosition.position.x, maxPosition.position.x);
                        pos.y = transform.position.y;
                        pos.z = Mathf.Clamp(transform.position.z, minPosition.position.z, maxPosition.position.z);
                        break;
                }
                transform.position = pos;
            }
        }
    }
    public void FreezPostion() {
        dragTranslate.RemainingTranslation = Vector3.zero;
    }
}