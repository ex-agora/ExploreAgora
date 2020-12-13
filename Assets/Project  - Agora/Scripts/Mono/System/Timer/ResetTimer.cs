using UnityEngine;
using StateMachine;

[CreateAssetMenu(fileName = "Timer Reset", menuName = "SO /Variable/Timer Reset", order = 0)]
public class ResetTimer : Action
{
    #region Methods
    //reset timer image filling                                     
    public override void Act<T>(StateControllersManager controllersManager)
    {
        TimerStateController timerStateController = controllersManager.GetController<TimerStateController>();
        timerStateController.timerImage.fillAmount = 1;
    }
    #endregion Methods
}
