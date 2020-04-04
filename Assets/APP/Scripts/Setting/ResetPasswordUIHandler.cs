using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetPasswordUIHandler : MonoBehaviour
{
    [SerializeField] InputField email;
    [SerializeField] SeperatedFields code;
    [SerializeField] GameObject emailPanel;
    [SerializeField] GameObject codePanel;
    [SerializeField] GameObject pwPanel;
    [SerializeField] InputField newPW;
    [SerializeField] InputField confirmNewPW;
    [SerializeField] ErrorFadingHandler emailError;
    [SerializeField] ErrorFadingHandler codeError;
    [SerializeField] ErrorFadingHandler newPWError;
    [SerializeField] ErrorFadingHandler confirmNewPWError;
    bool isPressed;
    string vcode;
    private void OnEnable()
    {
        email.text = string.Empty;
        
    }
    public void ResetPasswordRequest()
    {
        if (isPressed)
            return;
        if (!ValidationInputUtility.VerifyEmail(email.text)) {
            emailError.ShowErrorMsg("Invalid Email Format");
            emailError.HideErrorMsgDelay(3f);
            return;
        }
        isPressed = true;
        ResetPasswordRequestData r = new ResetPasswordRequestData();
        r.email = email.text;
        NetworkManager.Instance.ResetPasswordRequest(r, OnResetPasswordRequestSuccess, OnResetPasswordRequestFailed);
    }
    private void OnResetPasswordRequestSuccess(NetworkParameters obj)
    {
        isPressed = false;
        emailPanel.SetActive(false);
        codePanel.SetActive(true);
    }
    private void OnResetPasswordRequestFailed(NetworkParameters obj)
    {
        isPressed = false;
        emailError.ShowErrorMsg("Wrong Email");
        emailError.HideErrorMsgDelay(3f);
        print(obj.err.message);
    }


    public void CheckResetPasswordToken()
    {
        if (isPressed)
            return;
        CheckResetPasswordTokenData c = new CheckResetPasswordTokenData();
        c.email = email.text;
        code.SubmitCode();
        vcode = code.code;
        c.token = vcode;
        isPressed = true;
        NetworkManager.Instance.CheckResetPasswordToken(c, OnCheckResetPasswordTokenSuccess, OnCheckResetPasswordTokenFailed);
    }
    private void OnCheckResetPasswordTokenSuccess(NetworkParameters obj)
    {
        isPressed = false;
        codePanel.SetActive(false);
        pwPanel.SetActive(true);
    }
    private void OnCheckResetPasswordTokenFailed(NetworkParameters obj)
    {
        isPressed = false;
        codeError.ShowErrorMsg("Wrong Code");
        codeError.HideErrorMsgDelay(3f);
        print(obj.err.message);

    }

    public void ResetPassword()
    {
        if (isPressed)
            return;
        if (!ValidationInputUtility.VerifyPassword(newPW.text))
        {
            newPWError.ShowErrorMsg("Invalid Password Length");
            newPWError.HideErrorMsgDelay(3f);
            return;
        }
        if (confirmNewPW.text != newPW.text)
        {
            confirmNewPWError.ShowErrorMsg("The password does not match ");
            newPWError.HideErrorMsgDelay(3f);
            return;
        }
        isPressed = true;
        ResetPasswordData data = new ResetPasswordData();
        data.email = email.text;
        data.token = vcode;
        data.password = newPW.text;
        NetworkManager.Instance.ResetPassword(data, OnResetPasswordSuccess, OnResetPasswordFailed);
    }
    private void OnResetPasswordSuccess(NetworkParameters obj)
    {
        isPressed = false;
        pwPanel.SetActive(false);
    }
    private void OnResetPasswordFailed(NetworkParameters obj)
    {
        isPressed = false;
        print(obj.err.message);
    }
}
