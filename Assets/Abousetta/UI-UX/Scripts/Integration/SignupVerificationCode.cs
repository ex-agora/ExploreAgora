using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignupVerificationCode : MonoBehaviour
{
    [SerializeField] private SeperatedFields codeVerifcation;

    public void VerifyMail()
    {
        VerifyEmailData v = new VerifyEmailData();
        codeVerifcation.SubmitCode();
        v.activationCode = codeVerifcation.code;
        NetworkManager.Instance.VerifyEmail(v, OnVerifyMailSuccess, OnVerifyMailFailed);
    }
    private void OnVerifyMailSuccess(NetworkParameters obj)
    {         //flow  

    }
    private void OnVerifyMailFailed(NetworkParameters obj)
    { 
        print(obj.err.message); 
    }
}