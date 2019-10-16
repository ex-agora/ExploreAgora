using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;


[CreateAssetMenu(fileName = "Non Tap Interaction err4", menuName = "SO/SM/Action/Non Tap Interaction err4", order = 0)]
public class NonTapInteraction4 : Action
{
    public override void Act<T>(StateControllersManager controllersManager)
    {
        InteractionErrorController4 interactionController = controllersManager.GetController<InteractionErrorController4>();
        interactionController.text.text = interactionController.ErrorText;
        interactionController.text.gameObject.SetActive(true);
    }
}
