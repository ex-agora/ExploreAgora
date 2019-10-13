using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;


[CreateAssetMenu(fileName = "Reset Index", menuName = "SO/SM/Action/Reset Index", order = 0)]
public class CharByCharResetStateAction : Action
{
    public override void Act<T>(StateControllersManager controllersManager)
    {
        ResetStringIndex<T>(controllersManager);
    }
    void ResetStringIndex<T>(StateControllersManager controllersManager) where T : IStateController
    {
        CharByCharHndlerStateController charByCharHndlerStateController = controllersManager.GetController<CharByCharHndlerStateController>();
        charByCharHndlerStateController.CharByCharController.CharIndex = 0;
    }
}
