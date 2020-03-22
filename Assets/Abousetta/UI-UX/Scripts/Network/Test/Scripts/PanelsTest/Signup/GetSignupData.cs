using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetSignupData : GeneralPanelTest
{
    public InputField firstName;
    public InputField lastName;
    public InputField email;
    public InputField password;
    public InputField country;
    public InputField deviceType;
    public InputField deviceId;
    public SignupData getSignupData ()
    {
        SignupData s = new SignupData ();
        s.firstName = firstName.text;
        s.lastName = lastName.text;
        s.email = email.text;
        s.password = password.text;
        s.country = country.text;
        s.deviceType = deviceType.text;
        s.deviceId = deviceId.text;
        return s;
    }
    public SignupData getSignupData (bool t)
    {
        SignupData s = new SignupData ();
        s.firstName = "sasda";
        s.lastName = "sasda";
        s.email = "sasda";
        s.password = "sasda";
        s.country = "sasda";
        s.deviceType = "sasda";
        s.deviceId = "sasda";
        return s;
    }

}
