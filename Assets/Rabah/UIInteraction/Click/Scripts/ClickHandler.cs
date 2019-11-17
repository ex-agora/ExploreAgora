using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ClickHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameEvent AfterClick;
    public void OnPointerClick(PointerEventData eventData)
    {
        AfterClick.Raise();
        AudioManager.Instance?.Play("UIAction", "UI");
    }
}
