using UnityEngine;
using StateMachine;

[CreateAssetMenu(fileName = "Timer Action", menuName = "SO /Variable/Timer Action", order = 0)]
public class TimerControllerAction : Action
{
    // 
    public override void Act<T>(StateControllersManager controllersManager)
    {
        TimerStateController timerStateController = controllersManager.GetController<TimerStateController>();
        timerStateController.timerImage.fillAmount = 1 - timerStateController.StateMachineManager.ElpTime/ timerStateController.Duration;
    }
}
