using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

[CreateAssetMenu(fileName = "Hndler State Decision", menuName = "SO/SM/Decision/Hndler State Decision", order = 0)]
public class CharByCharHndlerStateDecision : Decision
{
    public override bool MakeDecision<T>(StateMachineManager stateMachine, StateControllersManager controllersManager)
    {
        return true;
    }
}