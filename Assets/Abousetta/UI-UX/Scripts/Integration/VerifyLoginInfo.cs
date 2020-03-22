using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VerifyLoginInfo : MonoBehaviour
{
    [SerializeField] private InputField emailInputField;
    [SerializeField] private InputField passwordInputField;

    public void Login()
    {
        LoginData l = new LoginData();
        l.email = emailInputField.text;
        l.password = passwordInputField.text;
        //l.deviceType = "";
        //l.deviceId = "";
        NetworkManager.Instance.Login(l, OnLoginSuccess, OnLoginFailed);
    }
    private void OnLoginSuccess(NetworkParameters obj)
    {
        //flow
    }
    private void OnLoginFailed(NetworkParameters obj)
    {
        print(obj.err.message);
    }
}