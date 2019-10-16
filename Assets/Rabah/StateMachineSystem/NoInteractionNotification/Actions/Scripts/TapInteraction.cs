using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;


[CreateAssetMenu(fileName = "Tap Interaction", menuName = "SO/SM/Action/Tap Interaction", order = 0)]
public class TapInteraction : Action
{
    public override void Act<T>(StateControllersManager controllersManager)
    {
        InteractionController interactionController = controllersManager.GetController<InteractionController>();
        interactionController.isTappedText.text = interactionController.InteractionText;
        interactionController.isTappedText.gameObject.SetActive(true);
    }
}
