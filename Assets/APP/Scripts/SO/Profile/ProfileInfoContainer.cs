﻿using System.Collections;
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
    public int points;
    public int keys;
    public int streaks;
    public int stones;
    public int profileImgIndex;
}
