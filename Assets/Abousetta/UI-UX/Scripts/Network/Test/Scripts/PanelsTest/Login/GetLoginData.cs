using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetLoginData : GeneralPanelTest
{
    public InputField email;
    public InputField password;
    public InputField deviceType;
    public InputField deviceId;
    public LoginData getLoginData ()
    {
        LoginData l = new LoginData ();
        l.email = email.text;
        l.password = password.text;
        l.deviceType = deviceType.text;
        l.deviceId = deviceId.text;
        return l;
    }
}
