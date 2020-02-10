using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDInformationPanel:MonoBehaviour
{
    #region Fields
    [SerializeField] string panelName;

    [SerializeField] int panelState;

    [SerializeField] Sprite firstFrame;

    #endregion Fields

    #region Properties
    public int PanelState { get => panelState; set => panelState = value; }
    public Sprite FirstFrame { get => firstFrame; set => firstFrame = value; }
    public string PanelName { get => panelName; set => panelName = value; }
    #endregion Properties
}
