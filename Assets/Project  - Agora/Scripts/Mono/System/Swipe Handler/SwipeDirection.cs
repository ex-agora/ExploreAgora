using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[RequireComponent (typeof (SwipeHandler))]
public class SwipeDirection : MonoBehaviour
{
    #region Fields
    SwapEightDirections dir;
    SwapInfo info;
    SwipeHandler swipController;
    #endregion Fields

    #region Properties
    public SwapEightDirections Dir
    {
        get => dir;
        set => dir = value;
    }
    public SwapInfo Info
    {
        get => info;
        set => info = value;
    }
    #endregion Properties

    #region Methods
    public void GetDragDirection()
    {
        Info = swipController.Info;
        float positiveX = Mathf.Abs(Info.direction.x);
        float positiveY = Mathf.Abs(Info.direction.y);
        SwapEightDirections draggedDir;
        if (positiveX > positiveY)
            draggedDir = (Info.direction.x > 0) ? SwapEightDirections.Right : SwapEightDirections.Left;
        else
            draggedDir = (Info.direction.y > 0) ? SwapEightDirections.Up : SwapEightDirections.Down;
        Dir = draggedDir;
    }

    public void GetDragEightDirection()
    {
        Info = swipController.Info;
        SwapEightDirections draggedDir = SwapEightDirections.None;

        #region UP Side

        if (Info.angle > 0 && Info.angle < 20)
            draggedDir = SwapEightDirections.Right;

        else if (Info.angle > 20 && Info.angle <= 70)
            draggedDir = SwapEightDirections.RightFrontCorner;

        else if (Info.angle > 70 && Info.angle <= 110)
            draggedDir = SwapEightDirections.Up;

        else if (Info.angle > 110 && Info.angle <= 160)
            draggedDir = SwapEightDirections.LeftFrontCorner;

        else if (Info.angle > 160 && Info.angle <= 180)
            draggedDir = SwapEightDirections.Left;

        #endregion

        #region Down Side
        if (Info.angle <= 0 && Info.angle > -20)
            draggedDir = SwapEightDirections.Right;

        if (Info.angle < -20 && Info.angle > -70)
            draggedDir = SwapEightDirections.RightBackCorner;

        if (Info.angle < -70 && Info.angle > -110)
            draggedDir = SwapEightDirections.Down;

        if (Info.angle < -110 && Info.angle > -160)
            draggedDir = SwapEightDirections.LeftBackCorner;

        if (Info.angle < -160 && Info.angle <= 180)
            draggedDir = SwapEightDirections.Left;

        #endregion

        Dir = draggedDir;

    }

    private void Start()
    {
        swipController = this.GetComponent<SwipeHandler> ();
    }
    #endregion Methods
}