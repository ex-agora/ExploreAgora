using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class DistanceAlertStateController : MonoBehaviour, IStateController
{
    bool isCorrectDitanceTriggered;
    bool isFarDitanceTriggered;
    bool isCloseDitanceTriggered;

    public bool IsCorrectDitanceTriggered { get => isCorrectDitanceTriggered; set => isCorrectDitanceTriggered = value; }
    public bool IsFarDitanceTriggered { get => isFarDitanceTriggered; set => isFarDitanceTriggered = value; }
    public bool IsCloseDitanceTriggered { get => isCloseDitanceTriggered; set => isCloseDitanceTriggered = value; }

    public bool ClearCorrectTrigger()
    {
        bool up = IsCorrectDitanceTriggered;
        IsCorrectDitanceTriggered = false;
        return up;
    }
    public void SetCorrectTrigget()
    {
        IsCorrectDitanceTriggered = true;
    }
    public bool ClearFarTrigger()
    {
        bool up = IsFarDitanceTriggered;
        IsFarDitanceTriggered = false;
        return up;
    }
    public void SetFarTrigget()
    {
        IsFarDitanceTriggered = true;
    }
    public bool ClearCloseTrigger()
    {
        bool up = IsCloseDitanceTriggered;
        IsCloseDitanceTriggered = false;
        return up;
    }
    public void SetCloseTrigget()
    {
        IsCloseDitanceTriggered = true;
    }
}