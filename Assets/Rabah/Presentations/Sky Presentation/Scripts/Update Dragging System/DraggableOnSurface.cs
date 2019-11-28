using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableOnSurface : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] GameEvent @onBeginDrag;

    [Tooltip ("Select the axis you want to drag object on")] [SerializeField] DraggableOnSurfaceAxes axes;
    [Tooltip ("Select the dragging mode")] [SerializeField] DraggingModes draggingMode;
    [SerializeField] DragObjectCheck dragObjectCheck;
    GameObject hitObject;
    public GameObject HitObject { get => hitObject; set => hitObject = value; }
    #region Private Variables
    private Vector3 screenPoint;
    //Distance between tapped point and gameobject after changing tap from screen to worldspace.
    private Vector3 offset;
    private Vector3 cursorScreenPoint;
    private Vector3 cursorPosition;

    private Vector3 myPosition;
    public Vector3 MyPosition { get => myPosition; set => myPosition = value; }
    [SerializeField] GameEvent @onEndDragOnly;
    [SerializeField] Transform clippingTargetMin;
    [SerializeField] Transform clippingTargetMax;
    [SerializeField] bool isclipping;
    #endregion

    #region Methods
    private void OnEnable ()
    {

    }
    public void OnBeginDrag (PointerEventData eventData)
    {
        MyPosition = transform.position;
        screenPoint = interactions.Instance.SessionOrigin.camera.WorldToScreenPoint (gameObject.transform.position);
        offset = gameObject.transform.position - interactions.Instance.SessionOrigin.camera.ScreenToWorldPoint (
            new Vector3 (eventData.position.x , screenPoint.y , screenPoint.z));
        @onBeginDrag?.Raise ();
    }

    public void OnDrag (PointerEventData eventData)
    {
        cursorScreenPoint.x = eventData.position.x;
        cursorScreenPoint.y = eventData.position.y;
        cursorScreenPoint.z = screenPoint.z;

        //-------------------------------------------------------
        cursorPosition = interactions.Instance.SessionOrigin.camera.ScreenToWorldPoint (cursorScreenPoint);
        cursorPosition.x += offset.x;
        cursorPosition.y += offset.y;
        cursorPosition.z += offset.z;
        //----------------------------------------------------------
        switch ( axes )
        {
            case DraggableOnSurfaceAxes.XY_Axes:
                cursorPosition.z = transform.position.z;
                break;
            case DraggableOnSurfaceAxes.YZ_Axes:
                cursorPosition.z += cursorPosition.x;
                cursorPosition.x = transform.position.x;
                break;
            case DraggableOnSurfaceAxes.XZ_Axes:
                cursorPosition.z += cursorPosition.y;
                cursorPosition.y = transform.position.y;
                break;
        }
        if ( isclipping )
        {
            switch ( axes )
            {
                case DraggableOnSurfaceAxes.XY_Axes:
                    cursorPosition.x = Mathf.Clamp (cursorPosition.x , clippingTargetMin.position.x , clippingTargetMax.position.x);
                    cursorPosition.y = Mathf.Clamp (cursorPosition.y , clippingTargetMin.position.y , clippingTargetMax.position.y);
                    break;
                case DraggableOnSurfaceAxes.YZ_Axes:
                    cursorPosition.y = Mathf.Clamp (cursorPosition.y , clippingTargetMin.position.y , clippingTargetMax.position.y);
                    cursorPosition.z = Mathf.Clamp (cursorPosition.z , clippingTargetMin.position.z , clippingTargetMax.position.z);
                    break;
                case DraggableOnSurfaceAxes.XZ_Axes:
                    cursorPosition.x = Mathf.Clamp (cursorPosition.x , clippingTargetMin.position.x , clippingTargetMax.position.x);
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
        HitObject = EndDragAction (draggingMode);
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
                return dragObjectCheck.CheckCorrectPosition ();
            default:
                return null;
        }
    }
    #endregion
}