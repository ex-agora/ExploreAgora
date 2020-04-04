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
    [SerializeField] private InputField confirmPassword;
    [SerializeField] private CountryHandler country;
    [SerializeField] CheckBoxToggle termsCheck;
    [SerializeField] CheckBoxToggle policyCheck;
    [SerializeField] private ErrorFadingHandler fNameError;
    [SerializeField] private ErrorFadingHandler lNameError;
    [SerializeField] private ErrorFadingHandler emailError;
    [SerializeField] private ErrorFadingHandler pwError;
    [SerializeField] private ErrorFadingHandler pwConfirmError;
    [SerializeField] private ErrorFadingHandler tremsAndPolicyError;
    
    bool isPressed = false;
    private void OnEnable()
    {
        isPressed = false;
        firstName.text = string.Empty;
        lastName.text = string.Empty;
        email.text = string.Empty;
        password.text = string.Empty;
    }
    public void Signup()
    {
        if (isPressed)
        {
            return;
        }
        if (ValidationInputUtility.IsEmptyOrNull(firstName.text) || !ValidationInputUtility.IsAlpha(firstName.text))
        {
            fNameError.ShowErrorMsg("Invalid First Name");
            fNameError.HideErrorMsgDelay(3f);
           
            return;
        }
        if (ValidationInputUtility.IsEmptyOrNull(lastName.text) || !ValidationInputUtility.IsAlpha(lastName.text))
        {
            lNameError.ShowErrorMsg("Invalid Last Name");
            lNameError.HideErrorMsgDelay(3f);

            return;
        }
        if (!ValidationInputUtility.VerifyEmail(email.text))
        {
            emailError.ShowErrorMsg("Invalid Email Format");
            emailError.HideErrorMsgDelay(3f);
            return;
        }
        if (!ValidationInputUtility.VerifyPassword(password.text))
        {
            pwError.ShowErrorMsg("Invalid Password Length");
            pwError.HideErrorMsgDelay(3f);

            return;
        }
        if (password.text != confirmPassword.text)
        {
            pwConfirmError.ShowErrorMsg("The Password does not match");
            pwConfirmError.HideErrorMsgDelay(3f);

            return;
        }
        if (!termsCheck.IsActiveCheck || !policyCheck.IsActiveCheck)
        {
            tremsAndPolicyError.ShowErrorMsg("Please Confirm Our Policies");
            tremsAndPolicyError.HideErrorMsgDelay(3f);
            return;
        }
        isPressed = true;
        SignupData s = new SignupData();
        s.firstName = firstName.text;
        s.lastName = lastName.text;
        s.email = email.text;
        s.password = password.text;
        s.country = country.GetCountryName();
        s.deviceType = "Android";//Application.platform.ToString() ;
        s.deviceId = SystemInfo.deviceUniqueIdentifier;
        NetworkManager.Instance.Signup(s, OnSignupSuccess, OnSignupFailed);
    }
    private void OnSignupSuccess(NetworkParameters obj)
    {
        isPressed = false;
        SignupResponse signupResponse = (SignupResponse)obj.responseData;
        NetworkManager.Instance.SaveToken(signupResponse.token);
        UXFlowManager.Instance.ShowConformationPanel();
    }
    private void OnSignupFailed(NetworkParameters obj)
    {
        switch (obj.err.message.ToLower()) {
            case "password must be at least 8 chars long":
                pwError.ShowErrorMsg("Invalid Password Length");
                pwError.HideErrorMsgDelay(3f);
                break;
            case "invalid value":
                lNameError.ShowErrorMsg("Invalid Last Name");
                lNameError.HideErrorMsgDelay(3f);
                break;
            case "firstname must not be empty.":
                fNameError.ShowErrorMsg("Invalid First Name");
                fNameError.HideErrorMsgDelay(3f);
                break;
            case "lastname must not be empty.":
                lNameError.ShowErrorMsg("Invalid Last Name");
                lNameError.HideErrorMsgDelay(3f);
                break;
            case "user already existed":
                emailError.ShowErrorMsg("User Already Existed");
                emailError.HideErrorMsgDelay(3f);
                break;
        }
        isPressed = false;
    }


    public void LinkAccount()
    {
        if (isPressed)
        {
            return;
        }
        if (ValidationInputUtility.IsEmptyOrNull(firstName.text) || !ValidationInputUtility.IsAlpha(firstName.text))
        {
            fNameError.ShowErrorMsg("Invalid First Name");
            fNameError.HideErrorMsgDelay(3f);

            return;
        }
        if (ValidationInputUtility.IsEmptyOrNull(lastName.text) || !ValidationInputUtility.IsAlpha(lastName.text))
        {
            lNameError.ShowErrorMsg("Invalid Last Name");
            lNameError.HideErrorMsgDelay(3f);

            return;
        }
        if (!ValidationInputUtility.VerifyEmail(email.text))
        {
            emailError.ShowErrorMsg("Invalid Email Format");
            emailError.HideErrorMsgDelay(3f);
            return;
        }
        if (!ValidationInputUtility.VerifyPassword(password.text))
        {
            pwError.ShowErrorMsg("Invalid Password Length");
            pwError.HideErrorMsgDelay(3f);

            return;
        }
        if (password.text != confirmPassword.text)
        {
            pwConfirmError.ShowErrorMsg("The Password does not match");
            pwConfirmError.HideErrorMsgDelay(3f);

            return;
        }
        if (!termsCheck.IsActiveCheck || !policyCheck.IsActiveCheck)
        {
            tremsAndPolicyError.ShowErrorMsg("Please Confirm Our Policies");
            tremsAndPolicyError.HideErrorMsgDelay(3f);
            return;
        }
        isPressed = true;
        LinkAccountData s = new LinkAccountData();
        s.firstName = firstName.text;
        s.lastName = lastName.text;
        s.email = email.text;
        s.password = password.text;
        s.country = country.GetCountryName();
        NetworkManager.Instance.LinkAccount(s, OnLinkAccountSusccess, OnLinkAccountFailed);
    }
    private void OnLinkAccountSusccess(NetworkParameters obj)
    {
        isPressed = false;
        LinkAccountResponse response = (LinkAccountResponse)obj.responseData;
        NetworkManager.Instance.SaveToken(response.token);
        UXFlowManager.Instance.ShowConformationPanel();
    }
    private void OnLinkAccountFailed(NetworkParameters obj)
    {
        switch (obj.err.message.ToLower())
        {
            case "password must be at least 8 chars long":
                pwError.ShowErrorMsg("Invalid Password Length");
                pwError.HideErrorMsgDelay(3f);
                break;
            case "invalid value":
                lNameError.ShowErrorMsg("Invalid Last Name");
                lNameError.HideErrorMsgDelay(3f);
                break;
            case "firstname must not be empty.":
                fNameError.ShowErrorMsg("Invalid First Name");
                fNameError.HideErrorMsgDelay(3f);
                break;
            case "lastname must not be empty.":
                lNameError.ShowErrorMsg("Invalid Last Name");
                lNameError.HideErrorMsgDelay(3f);
                break;
            case "user already existed":
                emailError.ShowErrorMsg("User Already Existed");
                emailError.HideErrorMsgDelay(3f);
                break;
            case "User linked to another email ":
                emailError.ShowErrorMsg("User Already Linked to Another Email");
                emailError.HideErrorMsgDelay(3f);
                break;
        }
        isPressed = false;
        print(obj.err.message);
    }
}

