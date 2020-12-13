using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;


[CreateAssetMenu(fileName = "Non Tap Interaction", menuName = "SO/SM/Action/Non Tap Interaction", order = 0)]
public class NonTapInteraction : Action
{
    #region Methods
    public override void Act<T>(StateControllersManager controllersManager)
    {
        InteractionController InteractionController = controllersManager.GetController<InteractionController>();
        InteractionController.isNotTappedText.gameObject.SetActive(true);
    }
    #endregion Methods
}
