using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Profile Info Container", menuName = "SO/App/Profile/ProfileInfoContainer", order = 0)]
public class ProfileInfoContainer : ScriptableObject
{
    public Gender gender;
    public UDateTime DOB;
    public string nickname;
    public string rank;
    public string nicknameInput;
    public ulong points;
    public int keys;
    public int streaks;
    public int stones;
    public int profileImgIndex;
    public string country;
    public string fName;
    public string lName;
    public string email;
    public bool isConfirmed;
    public string playerType;
    public List<AchievementHolder> holders;

    public List<int> Achievements { get => GetAchievements(); set => SetAchievements(value); }

    void SetAchievements(List<int> arr) {
        for (int i = 0; i < arr.Count; i++)
        {
            holders[i].current = arr[i];
        }
    }
    List<int> GetAchievements() {
        List<int> arr = new List<int>();
        for (int i = 0; i < holders.Count; i++)
        {
            arr.Add(holders[i].current);
        }
        return arr;
    }
}
