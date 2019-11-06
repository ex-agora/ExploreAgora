using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] GameEvent @onBeginDrag;

    [Tooltip ("Select the axis you want to drag object on")] [SerializeField] DraggableAxis axis;
    [Tooltip ("Select the dragging mode")] [SerializeField] DraggingModes draggingMode;
    [SerializeField] DragObjectCheck dragObjectCheck;
    GameObject hitObject;
    public GameObject HitObject { get => hitObject; set => hitObject = value; }
    public Vector3 MyPosition { get => myPosition; set => myPosition = value; }
    #region Private Variables
    private Vector3 screenPoint;
    //Distance between tapped point and gameobject after changing tap from screen to worldspace.
    private Vector3 offset;
    private Vector3 cursorScreenPoint;
    private Vector3 cursorPosition;
    [SerializeField] Transform clippingTargetMin;
    [SerializeField] Transform clippingTargetMax;
    [SerializeField] bool isclipping;

    private Vector3 myPosition;
    #endregion

    #region Methods
    private void OnEnable ()
    {
        
    }
    public void OnBeginDrag (PointerEventData eventData)
    {
        MyPosition = transform.position;
        screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (
            new Vector3 (eventData.position.x , screenPoint.y , screenPoint.z));
        @onBeginDrag?.Raise ();
    }

    public void OnDrag (PointerEventData eventData)
    {
        cursorScreenPoint.x = eventData.position.x;
        cursorScreenPoint.y = eventData.position.y;
        cursorScreenPoint.z = screenPoint.z;

        //-------------------------------------------------------
        cursorPosition = Camera.main.ScreenToWorldPoint (cursorScreenPoint);
        cursorPosition.x += offset.x;
        cursorPosition.y += offset.y;
        cursorPosition.z += offset.z;
        //----------------------------------------------------------
        switch ( axis )
        {
            case DraggableAxis.X_Axis:
                cursorPosition.y = transform.position.y;
                break;
            case DraggableAxis.Y_Axis:
                cursorPosition.x = transform.position.x;
                break;
            case DraggableAxis.Z_Axis:
                cursorPosition.z += cursorPosition.y;
                cursorPosition.x = transform.position.x;
                cursorPosition.y = transform.position.y;
                break;
        }
        if (isclipping)
        {
            switch ( axis )
            {
                case DraggableAxis.X_Axis:
                    cursorPosition.x = Mathf.Clamp(cursorPosition.x,clippingTargetMin.position.x, clippingTargetMax.position.x);
                    break;
                case DraggableAxis.Y_Axis:
                    cursorPosition.y = Mathf.Clamp (cursorPosition.y , clippingTargetMin.position.y , clippingTargetMax.position.y);
                    break;
                case DraggableAxis.Z_Axis:
                    cursorPosition.z = Mathf.Clamp (cursorPosition.z , clippingTargetMin.position.z , clippingTargetMax.position.z);
                    break;
            }
        }
        transform.position = cursorPosition;
    }

    public void OnEndDrag (PointerEventData eventData)
    {
        screenPoint = Vector3.zero;
        offset = Vector3.zero;
        cursorScreenPoint = Vector3.zero;
        cursorPosition = Vector3.zero;
        if(draggingMode == DraggingModes.DraggingToCorrectPosition)
        dragObjectCheck.EndDragAction (draggingMode);
    }
    #endregion
}