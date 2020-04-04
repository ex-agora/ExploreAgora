using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUIToken : MonoBehaviour
{
    [SerializeField] Image achievementImg;
    [SerializeField] Text achievementTxt;
    public void SetAchievementDate(Sprite _achievementSp, string _achievementStr) {
        achievementImg.sprite = _achievementSp;
        achievementImg.preserveAspect = true;
        achievementTxt.text = _achievementStr;
    }
}
