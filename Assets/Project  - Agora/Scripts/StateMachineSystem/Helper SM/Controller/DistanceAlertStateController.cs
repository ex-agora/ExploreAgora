using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class DistanceAlertStateController : MonoBehaviour, IStateController
{
    #region Fields
    bool isCloseDitanceTriggered;
    bool isCorrectDitanceTriggered;
    bool isFarDitanceTriggered;
    #endregion Fields

    #region Properties
    public bool IsCloseDitanceTriggered { get => isCloseDitanceTriggered; set => isCloseDitanceTriggered = value; }
    public bool IsCorrectDitanceTriggered { get => isCorrectDitanceTriggered; set => isCorrectDitanceTriggered = value; }
    public bool IsFarDitanceTriggered { get => isFarDitanceTriggered; set => isFarDitanceTriggered = value; }
    #endregion Properties

    #region Methods
    public bool ClearCloseTrigger()
    {
        bool up = IsCloseDitanceTriggered;
        IsCloseDitanceTriggered = false;
        return up;
    }

    public bool ClearCorrectTrigger()
    {
        bool up = IsCorrectDitanceTriggered;
        IsCorrectDitanceTriggered = false;
        return up;
    }
    public bool ClearFarTrigger()
    {
        bool up = IsFarDitanceTriggered;
        IsFarDitanceTriggered = false;
        return up;
    }

    public void SetCloseTrigget()
    {
        IsCloseDitanceTriggered = true;
    }

    public void SetCorrectTrigget()
    {
        IsCorrectDitanceTriggered = true;
    }
    public void SetFarTrigget()
    {
        IsFarDitanceTriggered = true;
    }
    #endregion Methods
}