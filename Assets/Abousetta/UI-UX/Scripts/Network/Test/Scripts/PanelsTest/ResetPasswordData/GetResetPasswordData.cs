using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetResetPasswordData : GeneralPanelTest
{
    public InputField email;
    public InputField password;
    public InputField token;
    public ResetPasswordData getResetPasswordData ()
    {
        ResetPasswordData r = new ResetPasswordData ();
        r.email = email.text;
        r.password = password.text;
        r.token = token.text;
        return r;
    }
}
