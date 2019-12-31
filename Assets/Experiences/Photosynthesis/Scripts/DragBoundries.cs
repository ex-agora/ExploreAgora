using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragBoundries : MonoBehaviour
{
    [SerializeField] DraggableAxis axis;
    [SerializeField] Transform minPosition;
    [SerializeField] Transform maxPosition;
    [SerializeField] bool isWorking;
    Vector3 pos = Vector3.zero;
    private void Update()
    {
        if (isWorking) {
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
                    pos.x = transform.position.x;
                    pos.y = transform.position.y;
                    pos.z = Mathf.Clamp(transform.position.z, minPosition.position.z, maxPosition.position.z);
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