using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PDInformationPanel
{
    [SerializeField]
    string panelName;
    [SerializeField]
    RuntimeAnimatorController anim;
    [SerializeField]
    Sprite firstFrame;

    public RuntimeAnimatorController Anim { get => anim; set => anim = value; }
    public Sprite FirstFrame { get => firstFrame; set => firstFrame = value; }
    public string PanelName { get => panelName; set => panelName = value; }
}
