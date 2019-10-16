using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;


[CreateAssetMenu(fileName = "Non Tap Interaction Exit", menuName = "SO/SM/Action/Non Tap Interaction Exit", order = 0)]
public class NonTapInteractionExit : Action
{
    public override void Act<T>(StateControllersManager controllersManager)
    {
        InteractionErrorController4 interactionController = controllersManager.GetController<InteractionErrorController4>();
        interactionController.text.text = "IsNotTappedText";
        interactionController.text.gameObject.SetActive(false);
    }
}
