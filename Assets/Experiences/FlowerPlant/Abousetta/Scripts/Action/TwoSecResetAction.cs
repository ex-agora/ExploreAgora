using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
[CreateAssetMenu(fileName = "TwoSecResetAction", menuName = "SO/SM/Action/TwoSecResetAction", order = 0)]
public class TwoSecResetAction : Action
{
    public override void Act<T>(StateControllersManager controllersManager)
    {
        Reset<T>(controllersManager);
    }
    void Reset<T>(StateControllersManager controllersManager) where T : IStateController
    {
        var controller = controllersManager.GetController<TimePassedController>();
        controller.Duration = 2;
    }
}
