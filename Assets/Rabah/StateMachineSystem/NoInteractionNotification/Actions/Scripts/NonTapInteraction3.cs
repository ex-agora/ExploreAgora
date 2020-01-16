using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;


[CreateAssetMenu(fileName = "Non Tap Interaction err3", menuName = "SO/SM/Action/Non Tap Interaction err3", order = 0)]
public class NonTapInteraction3 : Action
{
    #region Methods
    public override void Act<T>(StateControllersManager controllersManager)
    {
        InteractionErrorController3 interactionController = controllersManager.GetController<InteractionErrorController3>();
        interactionController.text.text = interactionController.ErrorText;
        interactionController.text.gameObject.SetActive(true);
    }
    #endregion Methods
}
