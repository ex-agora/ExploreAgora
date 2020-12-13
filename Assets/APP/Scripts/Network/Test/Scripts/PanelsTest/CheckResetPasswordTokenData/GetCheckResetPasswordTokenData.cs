using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetCheckResetPasswordTokenData : GeneralPanelTest
{
    public InputField email;
    public InputField activationCode;
    public CheckResetPasswordTokenData getCheckResetPasswordTokenData ()
    {
        CheckResetPasswordTokenData c = new CheckResetPasswordTokenData ();
        c.email = email.text;
        c.token = activationCode.text;
        return c;
    }
}
