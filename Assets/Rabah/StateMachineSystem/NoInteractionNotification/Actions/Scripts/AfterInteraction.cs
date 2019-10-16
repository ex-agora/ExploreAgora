using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;


[CreateAssetMenu(fileName = "AfterInteraction", menuName = "SO/SM/Action/AfterInteraction", order = 0)]
public class AfterInteraction : Action
{
    public override void Act<T>(StateControllersManager controllersManager)
    {
        AfterInteractionController interactionController = controllersManager.GetController<AfterInteractionController>();
        interactionController.text.text = interactionController.AfterText;
         interactionController.text.gameObject.SetActive(true);
    }
}
