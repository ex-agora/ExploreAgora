using StateMachine;
using UnityEngine;

[CreateAssetMenu(fileName = "Check Time", menuName = "SO/SM/Decision/Check Time", order = 0)]
public class CheckTimeDecision : Decision
{
    public override bool MakeDecision<T>(StateMachineManager stateMachine, StateControllersManager controllersManager)
    {
        return true;
    }
}