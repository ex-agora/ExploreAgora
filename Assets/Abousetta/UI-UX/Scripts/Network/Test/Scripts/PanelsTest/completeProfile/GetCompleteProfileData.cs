using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetCompleteProfileData : GeneralPanelTest
{
    public InputField firstName;
    public InputField lastName;
    public InputField nickName;
    public InputField gender;
    public InputField avatarId;
    public InputField birthDate;
    public InputField country;

    public ProfileData getCompleteProfileData ()
    {
        ProfileData u = new ProfileData ();
        u.firstName = firstName.text;
        u.lastName = lastName.text;
        u.nickName = nickName.text;
        u.birthDate = birthDate.text;
        u.country = country.text;
        u.gender = gender.text;
        u.avatarId = avatarId.text;
        //u.scannedObjects = new Dictionary<string , int> ();
        //u.scannedObjects.Add ("book" , 2);
        u.keys = 400;
        u.points = 1000;
        u.powerStones = 144;
        return u;
    }
}
