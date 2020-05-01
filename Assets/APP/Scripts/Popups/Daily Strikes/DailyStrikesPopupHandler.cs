using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyStrikesPopupHandler : MonoBehaviour
{
    [SerializeField] ToolBarHandler popup;
    [SerializeField] List<Sprite> daysRing;
    [SerializeField] Text scoreTxt;
    [SerializeField] Text dayTxt;
    [SerializeField] GameObject scorePanel;
    [SerializeField] Image ringImg;
    [SerializeField] Image keyImg;
    [SerializeField] Sprite activeKey;
    [SerializeField] Sprite unactiveKey;
    public void ShowPopup(long _points, int dayNum, bool hasKey) {
        if (hasKey) {
            keyImg.sprite = activeKey;
            ringImg.sprite = daysRing[dayNum-1];
            scorePanel.SetActive(false);
            dayTxt.text = $"Day {dayNum}";
            popup.OpenToolBar();
        }
        else {
            keyImg.sprite = unactiveKey;
            ringImg.sprite = daysRing[dayNum-1];
            scorePanel.SetActive(true);
            scoreTxt.text = StringUtility.AbbreviateNumber( _points);
            dayTxt.text = $"Day {dayNum}";
            popup.OpenToolBar();
        }
    }
}
