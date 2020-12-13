using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

[CreateAssetMenu(fileName = "OpenBubbleAction", menuName = "SO/SM/Bubble/Action/OpenBubbleAction", order = 0)]
public class OpeningBubbleAction : Action
{
    #region Methods
    public override void Act<T>(StateControllersManager controllersManager)
    {
        OpenBubble<T>(controllersManager);
    }
    /// <summary>
    /// Open speech bubble.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="controllersManager"></param>
    void OpenBubble<T>(StateControllersManager controllersManager) where T : IStateController
    {
        var controller = controllersManager.GetController<SpeechBubbleController>();
        var controller1 = controllersManager.GetController<TimePassedController>();
        if (controller == null || controller1 == null) return;
        controller1.Duration = 1f;
        controller.ShowNextBubble();
    }
    #endregion Methods
}