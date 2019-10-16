using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

[CreateAssetMenu(fileName = "OpenBubbleAction", menuName = "SO/SM/Bubble/Action/OpenBubbleAction", order = 0)]
public class OpeningBubbleAction : Action
{
    public override void Act<T>(StateControllersManager controllersManager)
    {
        OpenBubble<T>(controllersManager);
    }

    void OpenBubble<T>(StateControllersManager controllersManager) where T : IStateController
    {
        var controller = controllersManager.GetController<SpeechBubbleController>();
        var controller1 = controllersManager.GetController<TimePassedController>();
        controller1.Duration = 3;
        controller.ShowNextBubble();
    }
}