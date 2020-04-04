using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievmentProperties : MonoBehaviour
{
    public achievement AchievementSO;
    [SerializeField] Image badgeImg;
    [SerializeField] Text titleText;
    [SerializeField] Text descriptionText;
    public bool state;
    

    private void Awake()
    {
        //set default data to title and description Fields of every Achievment
        titleText.text = AchievementSO.Title;
        descriptionText.text = AchievementSO.Description;
    }

    //called when state is false and this mean this achievment is not done yet
    public void lockUI()
    {
        setlock();
    }

    //called when state is true and this mean this achievment is done 
    public void unlockUI()
    {
        setunlock();
    }


    void setlock()
    {
        badgeImg.sprite = AchievementSO.BeforeAchievement;
    }


    void setunlock()
    {
        badgeImg.sprite = AchievementSO.AfterAchievement;
    }

}
