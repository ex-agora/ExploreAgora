using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public struct SwapInfo
{
    #region Fields
    public float angle;

    public Vector3 direction;

    public float timeElapsed;
    #endregion Fields

    #region Constructors
    public SwapInfo(Vector3 _Direction, float _TimeElapsed, float _Angle)
    {
        direction = _Direction;
        timeElapsed = _TimeElapsed;
        angle = _Angle;
    }
    #endregion Constructors
}

public class SwipeHandler : MonoBehaviour, IEndDragHandler, IBeginDragHandler, IDragHandler
{

    #region Fields
    public GameEvent endSwipeEvent;
    float angle;
    Vector3 beginPosition;
    Vector3 endPosition;
    float endTime;
    Vector3 finalPosition;
    SwapInfo info;
    bool isSwiped;
    float takenTimeOfDrag;
    #endregion Fields

    #region Properties
    public SwapInfo Info
    {
        get => info;
    }
    public bool IsSwiped
    {
        
        get => isSwiped;
        set => isSwiped = value;
    }
    #endregion Properties

    #region Methods
    public float DragTakenTime()
    {
        return endTime - takenTimeOfDrag;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
        IsSwiped = false;
        takenTimeOfDrag = 0;
        beginPosition = eventData.position;
        takenTimeOfDrag = Time.time;
        info = default (SwapInfo);
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        endPosition = eventData.position;
        finalPosition = (endPosition - beginPosition).normalized;
        endTime = Time.time;
        info = new SwapInfo (finalPosition, DragTakenTime (), ReturnSwipeAngle ());
        IsSwiped = true;
        endSwipeEvent.Raise ();
    }

    public float ReturnSwipeAngle ()
    {
        angle = Mathf.Atan2 (finalPosition.y, finalPosition.x) * Mathf.Rad2Deg;
        return angle;
    }
    #endregion Methods
}
