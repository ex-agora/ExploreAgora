using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignupValidation : MonoBehaviour
{
    [SerializeField] private InputField firstName;
    [SerializeField] private InputField lastName;
    [SerializeField] private InputField email;
    [SerializeField] private InputField password;
    [SerializeField] private Dropdown country;

    public void Signup()
    {
        SignupData s = new SignupData();
        s.firstName = firstName.text;
        s.lastName = lastName.text;
        s.email = email.text;
        s.password = password.text;
        s.country = country.options[country.value].text;
        //s.deviceType = "sasda";
        //s.deviceId = "sasda";
        NetworkManager.Instance.Signup(s, OnSignupSuccess, OnSignupFailed);
    }
    private void OnSignupSuccess(NetworkParameters obj)
    {
        SignupResponse signupResponse = (SignupResponse)obj.responseData;
        /////very importnt 
        NetworkManager.Instance.SaveToken(signupResponse.token);
    }
    private void OnSignupFailed(NetworkParameters obj)
    {
        //ShowErrors
    }
}

