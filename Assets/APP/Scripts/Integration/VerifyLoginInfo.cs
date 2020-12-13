using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VerifyLoginInfo : MonoBehaviour
{
    [SerializeField] private InputField emailInputField;
    [SerializeField] private InputField passwordInputField;
    [SerializeField] ErrorFadingHandler error;
   
    bool isPressed =false;
    private void OnEnable()
    {
        emailInputField.text = string.Empty;
        passwordInputField.text = string.Empty;
    }
    public void Login()
    {
        if (isPressed) {
            return;
        }
        
        if (!ValidationInputUtility.VerifyEmail(emailInputField.text))
        {
            error.ShowErrorMsg("Invalid Email Format");
            error.HideErrorMsgDelay(3f);
            return;
        }
        if (!ValidationInputUtility.VerifyPassword(passwordInputField.text))
        {
            error.ShowErrorMsg("Invalid Password Length");
            error.HideErrorMsgDelay(3f);
           
            return;
        }
        isPressed = true;
        LoginData l = new LoginData();
        l.email = emailInputField.text;
        l.password = passwordInputField.text;
#if UNITY_ANDROID
        l.deviceType = "Android";//Application.platform.ToString() ;
#endif
#if UNITY_IOS
        l.deviceType = "IOS";//Application.platform.ToString() ;
#endif
        l.deviceId = SystemInfo.deviceUniqueIdentifier;
        NetworkManager.Instance.Login(l, OnLoginSuccess, OnLoginFailed);
    }
    private void OnLoginSuccess(NetworkParameters obj)
    {
        isPressed = false;
        LoginResponse response = (LoginResponse)obj.responseData;
        NetworkManager.Instance.SaveToken(response.token);
        UXFlowManager.Instance.LoginFadeIn();
 
    }
    private void OnLoginFailed(NetworkParameters obj)
    {
        isPressed = false;
        error.ShowErrorMsg("Wrong Email or Password");
        error.HideErrorMsgDelay(3f);
        print(obj.err.message);
    }
  

}