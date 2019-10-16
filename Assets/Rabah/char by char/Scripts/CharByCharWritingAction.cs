using StateMachine;
using UnityEngine;
/// <summary>
/// this action write the text with new character and update index
/// </summary>
[CreateAssetMenu(fileName = "Writing", menuName = "SO/SM/Action/Writing", order = 0)]
public class CharByCharWritingAction : Action
{

    public override void Act<T>(StateControllersManager controllersManager)
    {
        WriteCharByChar<T>(controllersManager);
    }
    //add the new character and update the character index
    void WriteCharByChar<T>(StateControllersManager controllersManager) where T : IStateController
    {
        CharByCharController characterController = controllersManager.GetController<CharByCharController>();
        characterController.OutputText += characterController.TextString[characterController.CharIndex];
        characterController.CharIndex++;
    } 
}
