using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DailyStrikesHandler : MonoBehaviour
{
    private DateTime unbiasedTimerEndTimestamp;
    [SerializeField] ProfileInfoContainer profile;
    void Awake()
    {
        unbiasedTimerEndTimestamp = this.ReadTimestamp("unbiasedTimer", UnbiasedTime.Instance.Now().AddDays(1));
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimeSpan unbiasedRemaining = unbiasedTimerEndTimestamp - UnbiasedTime.Instance.Now();
        if (unbiasedRemaining.TotalDays > -1 && unbiasedRemaining.TotalDays <= 0) {

        }
        else if (unbiasedRemaining.TotalDays <= -1) { }
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
}
