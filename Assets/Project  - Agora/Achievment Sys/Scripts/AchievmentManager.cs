using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievmentManager : MonoBehaviour
{
    [SerializeField]
    //[HideInInspector]
    List<AchievmentProperties> achievmentProperties;
    string tempId;
    
    // Start is called before the first frame update
    void Start()
    {
        //loop over achievment if it is done refresh ui with unlock setting if not refresh ui with lock setting
        for (int i = 0; i < achievmentProperties.Count; i++)
        {
            bool up=LoadAchievment(achievmentProperties[i].AchievementSO.Id);
            if (up)
            {
                achievmentProperties[i].AchievementSO.State = true;
                achievmentProperties[i].unlockUI();
            }
            else
            {
                achievmentProperties[i].AchievementSO.State = false;
                achievmentProperties[i].lockUI();
            }

        }
    }


    // called from achievment systems (observers) when a condition is met
    public void RefreshUI(string id , bool state)
    {
        for (int i = 0; i < achievmentProperties.Count; i++)
        {
            tempId = achievmentProperties[i].AchievementSO.Id;
            
            if (tempId == id)
            {
                achievmentProperties[i].AchievementSO.State = true;
                savingActions(achievmentProperties[i]);
            }
        }
    }


    //save achievment state in player prefs if it is done or not 
    private void savingActions(AchievmentProperties achievmentPro)
    {

        PlayerPrefs.SetString(achievmentPro.AchievementSO.Id, achievmentPro.AchievementSO.State.ToString());
        achievmentPro.unlockUI();
    }


    private bool LoadAchievment(string ID)
    {

        return bool.Parse(PlayerPrefs.GetString(ID,"false"));
    }


    // called from achievment systems (observers) to add its achievment ProPerty
    public void AddAchievments(AchievmentProperties achievment)
    {
        achievmentProperties.Add(achievment);
    }

}
