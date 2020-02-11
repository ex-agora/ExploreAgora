using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ClickHandler : MonoBehaviour, IPointerClickHandler
{
    #region Fields
    [SerializeField] GameEvent AfterClick;
    [SerializeField] bool isPickup;
    #endregion Fields

    #region Methods
    private void Start ()
    {
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        AfterClick.Raise();
        if (isPickup)
            AudioManager.Instance?.Play("pickUp", "Activity");
        else
            AudioManager.Instance?.Play("UIAction", "UI");
    }
    #endregion Methods
}
