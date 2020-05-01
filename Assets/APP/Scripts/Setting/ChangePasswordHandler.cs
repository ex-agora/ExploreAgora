using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePasswordHandler : MonoBehaviour
{
    [SerializeField] InputField oldPW;
    [SerializeField] InputField newPW;
    [SerializeField] InputField confirmNewPW;
    [SerializeField] ErrorFadingHandler oldPWError;
    [SerializeField] ErrorFadingHandler newPWError;
    [SerializeField] ErrorFadingHandler confirmNewPWError;
    [SerializeField] GameObject changePWPanel;
    bool isPressed = false;
    private void OnEnable()
    {
        oldPW.text = string.Empty;
        newPW.text = string.Empty;
        confirmNewPW.text = string.Empty;
    }
    public void ChangePW() {
        if (isPressed)
            return;
        if (!ValidationInputUtility.VerifyPassword(oldPW.text))
        {
            oldPWError.ShowErrorMsg("Invalid Password Length");
            oldPWError.HideErrorMsgDelay(3f);
            return;
        }
        if (oldPW.text == newPW.text) {
            newPWError.ShowErrorMsg("Insert New Password ");
            newPWError.HideErrorMsgDelay(3f);
            return;
        }
        if (!ValidationInputUtility.VerifyPassword(newPW.text)) {
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
        ChangePassword();
    }
    void ChangePassword()
    {
        ChangePasswordData data = new ChangePasswordData();
        data.oldPassword = oldPW.text;
        data.newPassword = newPW.text;
        NetworkManager.Instance.ChangePassword(data, OnChangePasswordSuccess, OnChangePasswordFailed);
    }
    private void OnChangePasswordSuccess(NetworkParameters obj)
    {
        isPressed = false;
        changePWPanel.SetActive(false);
    }
    private void OnChangePasswordFailed(NetworkParameters obj)
    {
        isPressed = false;
        if (UXFlowManager.Instance.IsThereNetworkError(obj.err.errorTypes))
            return;
        oldPWError.ShowErrorMsg("Wrong Password");
        oldPWError.HideErrorMsgDelay(3f);
    }
}
