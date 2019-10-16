using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;


[CreateAssetMenu(fileName = "Non Tap Interaction err1", menuName = "SO/SM/Action/Non Tap Interaction err1", order = 0)]
public class NonTapInteraction1 : Action
{
    public override void Act<T>(StateControllersManager controllersManager)
    {
        InteractionErrorController1 interactionController = controllersManager.GetController<InteractionErrorController1>();
        interactionController.text.text = interactionController.ErrorText;
        interactionController.text.gameObject.SetActive(true);
    }
}
