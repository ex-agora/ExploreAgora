using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;


[CreateAssetMenu(fileName = "Non Tap Interaction err2", menuName = "SO/SM/Action/Non Tap Interaction err2", order = 0)]
public class NonTapInteraction2 : Action
{
    #region Methods
    public override void Act<T>(StateControllersManager controllersManager)
    {
        InteractionErrorController2 interactionController = controllersManager.GetController<InteractionErrorController2>();
        interactionController.text.text = interactionController.ErrorText;
        interactionController.text.gameObject.SetActive(true);
    }
    #endregion Methods
}
