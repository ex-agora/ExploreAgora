using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DailyStrikesHandler : MonoBehaviour
{
    private DateTime unbiasedTimerEndTimestamp;
    [SerializeField] ProfileInfoContainer profile;
    [SerializeField] AchievementHolder achievement;
    [SerializeField] DailyStrikesPopupHandler popupHandler;
    [SerializeField] ProfileNetworkHandler networkHandler;
    static DailyStrikesHandler instance;

    public static DailyStrikesHandler Instance { get => instance; set => instance = value; }

    void Awake()
    {
        if (instance == null)
            instance = this;
        unbiasedTimerEndTimestamp = this.ReadTimestamp("unbiasedTimer", UnbiasedTime.Instance.Now().AddDays(1));
    }

    // Update is called once per frame
    void Update()
    {
        TimeSpan unbiasedRemaining = unbiasedTimerEndTimestamp - UnbiasedTime.Instance.Now();
        if (unbiasedRemaining.TotalDays > -1 && unbiasedRemaining.TotalDays <= 0) {
            profile.streaks++;
            unbiasedTimerEndTimestamp = unbiasedTimerEndTimestamp.AddDays(1);
            if (profile.streaks > achievement.current) {
                achievement.UpdateCurrent();
                Sprite badge = achievement.GetBadge();
                if (badge != null) {
                    AchievementManager.Instance.AddBadge(badge);
                }
            }
            if (profile.streaks % 7 == 0) {
                profile.keys++;
                popupHandler.ShowPopup(0, 7, true);
            }
            else {
                profile.points += (uint)(ScorePointsUtility.DailyStreaks * (profile.streaks % 7));
                popupHandler.ShowPopup((ScorePointsUtility.DailyStreaks * (profile.streaks % 7)), (profile.streaks % 7), false);
            }
                networkHandler.UpdateProfile();
        }
        else if (unbiasedRemaining.TotalDays <= -1) {
            profile.streaks = 0;
            unbiasedTimerEndTimestamp = unbiasedTimerEndTimestamp.AddDays(1);
            networkHandler.UpdateProfile();
        }


    }
    private DateTime ReadTimestamp(string key, DateTime defaultValue)
    {
        long tmp = Convert.ToInt64(PlayerPrefs.GetString(key, "0"));
        if (tmp == 0)
        {
            return defaultValue;
        }
        return DateTime.FromBinary(tmp);
    }
    void OnApplicationPause(bool paused)
    {
        if (paused)
        {
          this.WriteTimestamp("unbiasedTimer", unbiasedTimerEndTimestamp);
        }
        else
        {
            unbiasedTimerEndTimestamp = this.ReadTimestamp("unbiasedTimer", UnbiasedTime.Instance.Now().AddSeconds(60));
        }
    }
    private void WriteTimestamp(string key, DateTime time)
    {
        PlayerPrefs.SetString(key, time.ToBinary().ToString());
    }
    void OnApplicationQuit()
    {
       this.WriteTimestamp("unbiasedTimer", unbiasedTimerEndTimestamp);
    }
    public void DeleteTimestamp() {
        PlayerPrefs.DeleteKey("unbiasedTimer");
    }
}
