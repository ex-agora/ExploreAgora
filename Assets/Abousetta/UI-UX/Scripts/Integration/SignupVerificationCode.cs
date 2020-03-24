using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignupVerificationCode : MonoBehaviour
{
    [SerializeField] private SeperatedFields codeVerifcation;
    [SerializeField] ErrorFadingHandler error;
    [SerializeField] OnBordingHandler bordingHandler;
    private void OnEnable()
    {
        codeVerifcation.ClearSeperatedFields();
    }
    public void VerifyMail()
    {
        VerifyEmailData v = new VerifyEmailData();
        codeVerifcation.SubmitCode();
        v.activationCode = codeVerifcation.code;
        NetworkManager.Instance.VerifyEmail(v, OnVerifyMailSuccess, OnVerifyMailFailed);
    }
    private void OnVerifyMailSuccess(NetworkParameters obj)
    {
        UXFlowManager.Instance.AcceptConformation();
        bordingHandler.StartOnBording();
    }
    private void OnVerifyMailFailed(NetworkParameters obj)
    {
        error.ShowErrorMsg("Invalid Code");
        error.HideErrorMsgDelay(3f);
        codeVerifcation.ClearSeperatedFields();
    }
}