using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreOnlyPopupHandler : MonoBehaviour
{
    [SerializeField] ToolBarHandler popup;
    [SerializeField] Text scoreTxt;
    public void ShowPopup(ulong score) {
        scoreTxt.text = StringUtility.AbbreviateNumber(score);
        popup.OpenToolBar();
    }
}
