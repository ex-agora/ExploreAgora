using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    #region Fields
    [SerializeField] GameEvent @onBeginDrag;
    [SerializeField] GameEvent @onDrag;
    [SerializeField] Camera ArCam;
    [Tooltip ("Select the axis you want to drag object on")] [SerializeField] DraggableAxis axis;
    [Tooltip ("Select the dragging mode")] [SerializeField] DraggingModes draggingMode;
    [SerializeField] DragObjectCheck dragObjectCheck;
    GameObject hitObject;
    #endregion Fields

    #region Properties
    public GameObject HitObject { get => hitObject; set => hitObject = value; }
    public Vector3 MyPosition { get => myPosition; set => myPosition = value; }
    #endregion Properties

    #region Private Variables
    [SerializeField] GameEvent @onEndDragOnly;
    [SerializeField] Transform clippingTargetMax;
    [SerializeField] Transform clippingTargetMin;
    private Vector3 cursorPosition;
    private Vector3 cursorScreenPoint;
    [SerializeField] bool isclipping;
    private Vector3 myPosition;
    //Distance between tapped point and gameobject after changing tap from screen to worldspace.
    private Vector3 offset;

    private Vector3 screenPoint;
    #endregion

    #region Methods
    public GameObject EndDragAction(DraggingModes mode)
    {
        switch (mode)
        {
            case DraggingModes.DraggingOnly:
                @onEndDragOnly?.Raise();
                print("I am Dragging Only ");
                return null;
            case DraggingModes.DraggingToCorrectPosition:
                return dragObjectCheck.CheckCorrectPosition();//asdad
            default:
                return null;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        MyPosition = transform.position;
        screenPoint = interactions.Instance.SessionOrigin.camera.WorldToScreenPoint(gameObject.transform.position);
        //screenPoint = ArCam.WorldToScreenPoint (gameObject.transform.position);
        offset = gameObject.transform.position - interactions.Instance.SessionOrigin.camera.ScreenToWorldPoint(
            new Vector3(eventData.position.x, screenPoint.y, screenPoint.z));
        //offset = gameObject.transform.position - ArCam.ScreenToWorldPoint (
        //    new Vector3 (eventData.position.x , screenPoint.y , screenPoint.z));
        AudioManager.Instance.Play("UIAction", "UI");
        @onBeginDrag?.Raise();
    }

    public void OnDrag(PointerEventData eventData)
    {
        cursorScreenPoint.x = eventData.position.x;
        cursorScreenPoint.y = eventData.position.y;
        cursorScreenPoint.z = screenPoint.z;

        //-------------------------------------------------------
        cursorPosition = interactions.Instance.SessionOrigin.camera.ScreenToWorldPoint(cursorScreenPoint);
        //cursorPosition = ArCam.ScreenToWorldPoint (cursorScreenPoint);
        cursorPosition.x += offset.x;
        cursorPosition.y += offset.y;
        cursorPosition.z += offset.z;
        //----------------------------------------------------------
        switch (axis)
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
            switch (axis)
            {
                case DraggableAxis.X_Axis:
                    cursorPosition.x = Mathf.Clamp(cursorPosition.x, clippingTargetMin.position.x, clippingTargetMax.position.x);
                    break;
                case DraggableAxis.Y_Axis:
                    cursorPosition.y = Mathf.Clamp(cursorPosition.y, clippingTargetMin.position.y, clippingTargetMax.position.y);
                    break;
                case DraggableAxis.Z_Axis:
                    cursorPosition.z = Mathf.Clamp(cursorPosition.z, clippingTargetMin.position.z, clippingTargetMax.position.z);
                    break;
            }
        }
        transform.position = cursorPosition;
        @onDrag?.Raise();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        screenPoint = Vector3.zero;
        offset = Vector3.zero;
        cursorScreenPoint = Vector3.zero;
        cursorPosition = Vector3.zero;
        EndDragAction(draggingMode);
    }

    private void OnEnable()
    {
    }
    #endregion
}