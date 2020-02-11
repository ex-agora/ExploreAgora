using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RelocateEventEnableDisableAction : MonoBehaviour
{
    [SerializeField] GameEvent action = null;
    [SerializeField] Button btn;
    public void FireEvent() { action?.Raise(); }
    private void OnEnable()
    {
        FireEvent();
    }
    private void OnDisable()
    {
        FireEvent();
    }
}
