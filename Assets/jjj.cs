using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class jjj : MonoBehaviour
{
    [SerializeField] GetSignupData getSignupData;
    public InputField gg;
    public void TestSignup ()
    {
        SignupData s = new SignupData ();
        s.firstName = "sasda";
        s.lastName = "sasda";
        s.email = "sasda";
        s.password = "sasda";
        s.country = "sasda";
        s.deviceType = "sasda";
        s.deviceId = "sasda";
        NetworkManager.Instance.Signup (s , OnSignupSuccess , OnSignupFailed);
    }
    private void OnSignupSuccess (NetworkParameters obj)
    {
        SignupResponse signupResponse = (SignupResponse)obj.responseData;
        NetworkManager.Instance.SaveToken (signupResponse.token);
        //getSignupData.GoToNextPanel ();

    }
    private void OnSignupFailed (NetworkParameters obj)
    {
        print (obj.err.message);
        //getSignupData.ShowErrors (obj.err.message);
        //getSignupData.gameObject.SetActive (true);
    }
}
