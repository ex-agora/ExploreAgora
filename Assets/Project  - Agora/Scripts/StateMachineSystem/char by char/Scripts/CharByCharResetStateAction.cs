using UnityEngine;
using StateMachine;
/// <summary>
/// Reset CharIndex to satrt writing again
/// </summary>
[CreateAssetMenu (fileName = "Reset Index", menuName = "SO/SM/Action/Reset Index", order = 0)]
public class CharByCharResetStateAction : Action
{
    #region Methods
    public override void Act<T>(StateControllersManager controllersManager)
    {
        ResetStringIndex<T>(controllersManager);
    }
    void ResetStringIndex<T>(StateControllersManager controllersManager) where T : IStateController
    {
        CharByCharHndlerStateController charByCharHndlerStateController = controllersManager.GetController<CharByCharHndlerStateController>();
        charByCharHndlerStateController.CharByCharController.CharIndex = 0;
    }
    #endregion Methods
}
