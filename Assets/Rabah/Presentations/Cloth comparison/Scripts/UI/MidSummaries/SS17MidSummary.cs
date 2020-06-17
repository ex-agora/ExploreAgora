using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS17MidSummary : MonoBehaviour
{
    [SerializeField] Sprite firstFrame;
    [SerializeField] RuntimeAnimatorController animatorController;

    public Sprite FirstFrame { get => firstFrame; set => firstFrame = value; }
    public RuntimeAnimatorController AnimatorController { get => animatorController; set => animatorController = value; }
}
