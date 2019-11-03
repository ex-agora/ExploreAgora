using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObjectCheck : MonoBehaviour
{
    [SerializeField] CorrectDraggableObject correctDraggableObject;
    [SerializeField] GameEvent @onEndDragOnly;
    [SerializeField] GameEvent @onEndDragToCorrectPosition;
    [SerializeField] GameEvent @onEndDragToIncorrectPosition;
    [SerializeField] GameEvent @onEndDragToAnyPosition;
    [SerializeField] bool isDraggableAgain = false;
    bool isTrueDraggableObject;
    bool isDraggedToObject;
    Vector3 draggingObjectPosition;
    GameObject triggeredObject;

    public bool IsDraggableAgain { get => isDraggableAgain; set => isDraggableAgain = value; }
    public Vector3 DraggingObjectPosition { get => draggingObjectPosition; set => draggingObjectPosition = value; }

    void UpdateDraggableObjectCheck (Collider draggableObjectCollider)
    {
        if ( draggableObjectCollider.GetComponent<CorrectDraggableObject> () )
        {
            isDraggedToObject = true;
            triggeredObject = draggableObjectCollider.gameObject;
            if ( draggableObjectCollider.GetComponent<CorrectDraggableObject> ().isTrueObject && draggableObjectCollider.GetComponent<CorrectDraggableObject> () == correctDraggableObject )
            {
                isTrueDraggableObject = true;
            }
            else
            {
                isTrueDraggableObject = false;
            }
        }
    }
    private void OnTriggerEnter (Collider other)
    {
        UpdateDraggableObjectCheck (other);
    }
    private void OnTriggerStay (Collider other)
    {
        UpdateDraggableObjectCheck (other);
    }
    private void OnTriggerExit (Collider other)
    {
        isTrueDraggableObject = false;
        isDraggedToObject = false;
        triggeredObject = null;
    }
    public GameObject EndDragAction (DraggingModes mode)
    {
        switch ( mode )
        {
            case DraggingModes.DraggingOnly:
                @onEndDragOnly?.Raise ();
                print ("I am Dragging Only ");
                return null;
            case DraggingModes.DraggingToCorrectPosition:
                if ( isTrueDraggableObject )
                {
                    @onEndDragToCorrectPosition?.Raise ();
                    print ("I am Correct ");
                    return triggeredObject;
                }
                else
                {
                    if ( isDraggedToObject )
                    {
                        @onEndDragToIncorrectPosition?.Raise ();
                        print ("I am Incorrect ");
                        return triggeredObject;
                    }
                    else
                    {
                        @onEndDragToAnyPosition?.Raise ();
                        print ("I am Any Position ");
                        return null;
                    }
                }
            default:
                return null;
        }
    }
}
