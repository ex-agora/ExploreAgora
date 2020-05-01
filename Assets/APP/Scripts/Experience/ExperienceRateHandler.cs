using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceRateHandler : MonoBehaviour
{
    [SerializeField] ToolBarHandler popup;
    [SerializeField] AchievementHolder achievement;
    [SerializeField] ProfileNetworkHandler networkHandler;
    [SerializeField] Button confirmBtn;
    string experienceCode;
    int rate;
    public void Rate(int _rate) => rate = _rate;

    public void ShowRate(string _experienceCode) {
        experienceCode = _experienceCode;
        confirmBtn.interactable = false;
        popup.OpenToolBar();
    }
    public void ActiveBtn() => confirmBtn.interactable = true;
   
    public void SubmitRate()
    {
        ExperienceRateData rateData = new ExperienceRateData();
        rateData.experienceCode = experienceCode;
        rateData.rate = rate.ToString();
        NetworkManager.Instance.RateExperience(rateData, OnRateExperienceSusccess, OnRateExperienceFailed);
    }
    private void OnRateExperienceSusccess(NetworkParameters obj)
    {
        achievement.UpdateCurrent();
        networkHandler.Profile.points += ScorePointsUtility.Rating;
        AchievementManager.Instance.AddScore(ScorePointsUtility.Rating);
        Sprite badge = achievement.GetBadge();
        if (badge != null) {
            AchievementManager.Instance.AddBadge(badge);
        }
        networkHandler.UpdateProfile();
    }
    private void OnRateExperienceFailed(NetworkParameters obj)
    {
        print(obj.err.message);
    }
}
