using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;
using UnityEngine.UI;

public class CharByCharHndlerStateController : MonoBehaviour, IStateController
{

    [SerializeField] CharByCharController charByCharController;
    [SerializeField] TimePassedController timePassedController;

    public CharByCharController CharByCharController { get => charByCharController; set => charByCharController = value; }
    public TimePassedController TimePassedController { get => timePassedController; set => timePassedController = value; }

}