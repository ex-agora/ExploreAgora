﻿using UnityEngine;
using StateMachine;


[CreateAssetMenu (fileName = "TimeHandler" , menuName = "SO/SM/Action/TimeHandler" , order = 0)]
public class CharByCharHandlerStateAction : Action
{
    #region Methods
    public override void Act<T> (StateControllersManager controllersManager)
    {
        AddDurationTime<T> (controllersManager);
    }
    //Calculate the character duration
    void AddDurationTime<T> (StateControllersManager controllersManager) where T : IStateController
    {
        CharByCharHndlerStateController charByCharHndlerStateController = controllersManager.GetController<CharByCharHndlerStateController> ();
        TimePassedController handler = controllersManager.GetController<TimePassedController> ();
        handler.Duration =
        charByCharHndlerStateController.CharByCharController.TextDuration / charByCharHndlerStateController.CharByCharController.TextString.Length;
    }
    #endregion Methods
}
