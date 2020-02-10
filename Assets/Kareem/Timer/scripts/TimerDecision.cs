using UnityEngine;
using StateMachine;


[CreateAssetMenu(fileName = "Timer Decision" , menuName = "SO /Variable/Timer Decision", order = 0)]
public class TimerDecision : Decision
{
    #region Methods
    // return boolean of current timer state either reached dedicated Timer or not to make the Decision
    public override bool MakeDecision<T>(StateMachineManager stateMachine, StateControllersManager controllersManager)
    {
        return CheckTime<T>(stateMachine, controllersManager);
    }

    //check if current time reached dedicated Timer  
    bool CheckTime<T>(StateMachineManager stateMachine, StateControllersManager controllersManager) where T : IStateController
    {
        var timePassedHandler = controllersManager.GetController<TimerStateController>();
        if (timePassedHandler == null)
            return false;
        Debug.Log(timePassedHandler.Duration);
        return (timePassedHandler.Duration <= stateMachine.ElpTime);
    }
    #endregion Methods
}

