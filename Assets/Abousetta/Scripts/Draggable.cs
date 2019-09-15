using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    #region Private Variables
    private Vector3 screenPoint;
    //Distance between tapped point and gameobject after changing tap from screen to worldspace.
    private Vector3 offset;
    private Vector3 cursorScreenPoint;
    private Vector3 cursorPosition;
    #endregion

    #region Methods
    public void OnBeginDrag(PointerEventData eventData)
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(
            new Vector3(eventData.position.x, eventData.position.y, screenPoint.z));
    }

    public void OnDrag(PointerEventData eventData)
    {
        //-------------------------------------------------------
        cursorScreenPoint.x = eventData.position.x;
        cursorScreenPoint.y = eventData.position.y;
        cursorScreenPoint.z = screenPoint.z;
        //-------------------------------------------------------
        cursorPosition = Camera.main.ScreenToWorldPoint(cursorScreenPoint);
        cursorPosition.x += offset.x;
        cursorPosition.y += offset.y;
        cursorPosition.z += offset.z;
        //----------------------------------------------------------
        cursorPosition.y = this.transform.localScale.y / 2f;
        transform.position = cursorPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        screenPoint = Vector3.zero;
        offset = Vector3.zero;
        cursorScreenPoint = Vector3.zero;
        cursorPosition = Vector3.zero;
    }
    #endregion
}