using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetVerifyMailData : GeneralPanelTest
{
    public InputField activationCode;

    public VerifyEmailData getVerifyEmailData ()
    {
        VerifyEmailData v = new VerifyEmailData ();
        v.activationCode = activationCode.text;
        return v;
    }

}
