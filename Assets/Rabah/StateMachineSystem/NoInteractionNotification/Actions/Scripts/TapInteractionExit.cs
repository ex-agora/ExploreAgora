using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;


[CreateAssetMenu(fileName = "TapInteractionExit", menuName = "SO/SM/Action/TapInteractionExit", order = 0)]
public class TapInteractionExit : Action
{
    public override void Act<T>(StateControllersManager controllersManager)
    {
        AfterInteractionController interactionController = controllersManager.GetController<AfterInteractionController>();
        interactionController.text.gameObject.SetActive(false);
        interactionController.tapTrigger.IsTapped = false;
    }
}
