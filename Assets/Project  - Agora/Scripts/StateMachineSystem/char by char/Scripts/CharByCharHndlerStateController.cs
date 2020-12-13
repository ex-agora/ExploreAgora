using StateMachine;
using UnityEngine;
/// <summary>
/// Holdes the CharByChar and TimePassed Controllers
/// </summary> 
public class CharByCharHndlerStateController : MonoBehaviour, IStateController
{
    #region Fields
    [SerializeField] CharByCharController charByCharController;
    [SerializeField] TimePassedController timePassedController;
    #endregion Fields

    #region Properties
    public CharByCharController CharByCharController { get => charByCharController; set => charByCharController = value; }
    public TimePassedController TimePassedController { get => timePassedController; set => timePassedController = value; }
    #endregion Properties

}