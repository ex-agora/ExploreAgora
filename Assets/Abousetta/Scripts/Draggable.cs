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
    GameObject temp;
    #region Private Variables
    private Vector3 screenPoint;
    //Distance between tapped point and gameobject after changing tap from screen to worldspace.
    private Vector3 offset;
    private Vector3 cursorScreenPoint;
    private Vector3 cursorPosition;
    [SerializeField] Transform clippingTargetMin;
    [SerializeField] Transform clippingTargetMax;
    [SerializeField] bool isclipping;
    [SerializeField] GameEvent @onEndDragOnly;

    private Vector3 myPosition;
    #endregion
    private void Start()
    {
        temp = new GameObject();
    }
    #region Methods
    private void OnEnable ()
    {
        
    }
    Vector3 ManualLookAt(Vector3 target, Vector3 current)
    {


        Vector3 viewVector = target - current;


        //thisTransform.rotation = Quaternion.identity;
        var dot = Vector3.Dot(viewVector.normalized, target.normalized);


        var angle = Mathf.Acos(dot);
        angle *= Mathf.Rad2Deg;

        var cross = Vector3.Cross(viewVector.normalized, target.normalized);


        if (cross.y >= 0.0f)
            angle *= -1;
        return Vector3.RotateTowards(current,target,0.0f, angle);


    }
    public void OnBeginDrag (PointerEventData eventData)
    {
        MyPosition = transform.position;
        screenPoint = interactions.Instance.Arcamera.camera.WorldToScreenPoint (gameObject.transform.position);
        offset = gameObject.transform.position - interactions.Instance.Arcamera.camera.ScreenToWorldPoint (
            new Vector3 (eventData.position.x , screenPoint.y , screenPoint.z));
        @onBeginDrag?.Raise ();
    }

    public void OnDrag (PointerEventData eventData)
    {
        cursorScreenPoint.x = eventData.position.x;
        cursorScreenPoint.y = eventData.position.y;
        cursorScreenPoint.z = screenPoint.z;

        //-------------------------------------------------------
        cursorPosition = interactions.Instance.Arcamera.camera.ScreenToWorldPoint (cursorScreenPoint);
        cursorPosition.x += offset.x;
        cursorPosition.y += offset.y;
        cursorPosition.z += offset.z;
        temp.transform.rotation = Quaternion.identity;
        temp.transform.transform.position = cursorPosition;
        temp.transform.LookAt(transform.position.normalized);
        cursorPosition = temp.transform.position;
       // cursorPosition.Normalize();
       // cursorPosition.lookat(transform.forward);
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
        EndDragAction (draggingMode);
    }
    public GameObject EndDragAction(DraggingModes mode)
    {
        switch (mode)
        {
            case DraggingModes.DraggingOnly:
                @onEndDragOnly?.Raise();
                print("I am Dragging Only ");
                return null;
            case DraggingModes.DraggingToCorrectPosition:
                return dragObjectCheck.CheckCorrectPosition();
            default:
                return null;
        }
    }
    #endregion
}