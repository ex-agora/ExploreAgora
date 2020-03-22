using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetResetPasswordRequestData : GeneralPanelTest
{
    public InputField email;
    public ResetPasswordRequestData getResetPasswordRequestData ()
    {
        ResetPasswordRequestData r = new ResetPasswordRequestData ();
        r.email = email.text;
        return r;
    }
}
