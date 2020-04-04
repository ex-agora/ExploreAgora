using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceRateHandler : MonoBehaviour
{
    [SerializeField] ToolBarHandler popup;
    string experienceCode;
    int rate;
    public void Rate(int _rate) => rate = _rate;

    public void ShowRate(string _experienceCode) {
        experienceCode = _experienceCode;
        popup.OpenToolBar();
    }

   
    public void SubmitRate()
    {
        ExperienceRateData rateData = new ExperienceRateData();
        rateData.experienceCode = experienceCode;
        rateData.rate = rate.ToString();
        NetworkManager.Instance.RateExperience(rateData, OnRateExperienceSusccess, OnRateExperienceFailed);
    }
    private void OnRateExperienceSusccess(NetworkParameters obj)
    {

    }
    private void OnRateExperienceFailed(NetworkParameters obj)
    {
        print(obj.err.message);
    }
}
