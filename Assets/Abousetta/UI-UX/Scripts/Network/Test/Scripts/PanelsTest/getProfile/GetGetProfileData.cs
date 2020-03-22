using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetGetProfileData : GeneralPanelTest
{
    public Text firstName;
    public Text lastName;
    public Text nickName;
    public Text avatarId;
    public Text gender;
    public Text birthdate;
    public Text country;

    public void ShowData (string fName , string lName, string nName, string avId, string gen , string bDate, string cou )
    {
        firstName.text = fName;
        lastName.text = lName;
        nickName.text = nName;
        avatarId.text = gen;
        gender.text = fName;
        birthdate.text = bDate;
        country.text = cou;
    }
    public void ShowData (ProfileResponse data)
    {
        firstName.text = data.firstName;
        lastName.text = data.lastName;
        nickName.text = data.nickName;
        avatarId.text = data.avatarId;
        gender.text = data.gender;
        birthdate.text = data.birthDate;
        country.text = data.country;
    }
}
