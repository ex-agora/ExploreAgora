using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

[CreateAssetMenu(fileName = "Writing", menuName = "SO/SM/Action/Writing", order = 0)]
public class CharByCharWritingAction : Action
{

    int charIndex = 0;
    public override void Act<T>(StateControllersManager controllersManager)
    {
        WriteCharByChar<T>(controllersManager);
    }
    void WriteCharByChar<T>(StateControllersManager controllersManager) where T : IStateController
    {
        CharByCharController characterController = controllersManager.GetController<CharByCharController>();
        characterController.textUI.text += characterController.textString.Substring(charIndex);
        charIndex++;

    }
}