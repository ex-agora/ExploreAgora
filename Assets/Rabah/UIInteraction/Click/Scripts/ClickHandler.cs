using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ClickHandler : MonoBehaviour, IPointerClickHandler
{
    #region Fields
    [SerializeField] GameEvent AfterClick;
    #endregion Fields

    #region Methods
    public void OnPointerClick(PointerEventData eventData)
    {
        AfterClick.Raise();
        AudioManager.Instance?.Play("UIAction", "UI");
    }
    #endregion Methods
}
