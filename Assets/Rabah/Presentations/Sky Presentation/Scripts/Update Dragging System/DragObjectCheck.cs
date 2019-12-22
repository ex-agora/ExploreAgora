using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObjectCheck : MonoBehaviour
{
    [SerializeField] CorrectDraggableObject correctDraggableObject;
    [SerializeField] GameEvent @onEndDragToCorrectPosition;
    [SerializeField] GameEvent @onEndDragToIncorrectPosition;
    [SerializeField] GameEvent @onEndDragToAnyPosition;
    [SerializeField] Lean.Touch.LeanSelectable selectable = null;
    [SerializeField] bool isDraggableAgain = false;
    bool isTrueDraggableObject;
    bool isDraggedToObject;
    Vector3 draggingObjectPosition;
    GameObject triggeredObject;

    public bool IsDraggableAgain { get => isDraggableAgain; set => isDraggableAgain = value; }
    public Vector3 DraggingObjectPosition { get => draggingObjectPosition; set => draggingObjectPosition = value; }

    void UpdateDraggableObjectCheck(Collider draggableObjectCollider)
    {
        if (draggableObjectCollider.GetComponent<CorrectDraggableObject>())
        {
            isDraggedToObject = true;
            triggeredObject = draggableObjectCollider.gameObject;
            if (draggableObjectCollider.GetComponent<CorrectDraggableObject>().isTrueObject && draggableObjectCollider.GetComponent<CorrectDraggableObject>() == correctDraggableObject)
            {
                isTrueDraggableObject = true;
            }
            else
            {
                isTrueDraggableObject = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        UpdateDraggableObjectCheck(other);
    }
    private void OnTriggerStay(Collider other)
    {
        UpdateDraggableObjectCheck(other);
    }
    private void OnTriggerExit(Collider other)
    {
        isTrueDraggableObject = false;
        isDraggedToObject = false;
        triggeredObject = null;
    }
    public GameObject CheckCorrectPosition()
    {
        if (isTrueDraggableObject)
        {
            AudioManager.Instance?.Play("placeObject", "Activity");
            @onEndDragToCorrectPosition?.Raise();
            print("I am Correct ");
            return triggeredObject;
        }
        else
        {
            if (isDraggedToObject)
            {
                if (selectable == null || selectable.IsSelected)
                    @onEndDragToIncorrectPosition?.Raise();


                print("I am Incorrect ");
                return triggeredObject;
            }
            else
            {
                if (selectable == null || selectable.IsSelected)
                    @onEndDragToAnyPosition?.Raise();
                print("I am Any Position ");
                return null;
            }
        }
    }
    public void CheckPosition() { CheckCorrectPosition(); }
}
