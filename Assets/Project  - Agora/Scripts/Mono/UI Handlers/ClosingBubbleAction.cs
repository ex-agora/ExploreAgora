using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

[CreateAssetMenu(fileName = "ClosingBubbleAction", menuName = "SO/SM/Bubble/Action/ClosingBubbleAction", order = 0)]
public class ClosingBubbleAction : Action
{
    #region Methods
    public override void Act<T>(StateControllersManager controllersManager)
    {
        CloseBubble<T>(controllersManager);
    }
    /// <summary>
    /// Close speech bubble.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="controllersManager"></param>
    void CloseBubble<T>(StateControllersManager controllersManager) where T : IStateController
    {
        var controller = controllersManager.GetController<SpeechBubbleController>();
        controller.HideBubble();
    }
    #endregion Methods
}