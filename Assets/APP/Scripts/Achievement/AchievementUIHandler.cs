using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUIHandler : MonoBehaviour
{
    [SerializeField] ToolBarHandler achievementPopup;
    [SerializeField] ScoreOnlyPopupHandler achievementSocreOnlyPopup;
    [SerializeField] RectTransform achievementPanel;
    [SerializeField] GameObject prefabeAchievementUIToken;
    [SerializeField] Text socreText;
    void CheckAchievements() {
        AchiemvenetResult result = AchievementManager.Instance.CalculateAchievements();
        if (result == null)
            return;
        if (result.isScoreOnly)
            ShowAchievementSocreOnlyPopup(result);
        else
            ShowAchievementPopup(result);
    }
    void ShowAchievementSocreOnlyPopup(AchiemvenetResult result) {
        achievementSocreOnlyPopup.ShowPopup(result.score);
    }
    void ShowAchievementPopup(AchiemvenetResult result) {
        socreText.text = $"{result.score.ToString()} XP";
        if (result.badges.Count > 0) {
            for (int i = 0; i < result.badges.Count; i++)
            {
                AchievementUIToken uIToken = Instantiate(prefabeAchievementUIToken, achievementPanel.position, Quaternion.identity).GetComponent<AchievementUIToken>();
                uIToken.transform.parent = achievementPanel;
                uIToken.transform.localScale = Vector3.one;
                uIToken.SetAchievementDate(result.badges[i], "badge");
            }
        }

        if (result.isObjectScanned) {
            AchievementUIToken uIToken = Instantiate(prefabeAchievementUIToken, achievementPanel.position, Quaternion.identity).GetComponent<AchievementUIToken>();
            uIToken.transform.parent = achievementPanel;
            uIToken.transform.localScale = Vector3.one;
            uIToken.SetAchievementDate(result.scannedObjectSp, result.scannedName);
        }

        if (result.isLevelUp) {
            AchievementUIToken uIToken = Instantiate(prefabeAchievementUIToken, achievementPanel.position, Quaternion.identity).GetComponent<AchievementUIToken>();
            uIToken.transform.parent = achievementPanel;
            uIToken.transform.localScale = Vector3.one;
            uIToken.SetAchievementDate(result.levelUpSp, "badge");
        }
        achievementPopup.OpenToolBar();
    }

    private void Start()
    {
        Invoke(nameof(CheckAchievements), 0.5f);
    }
}
