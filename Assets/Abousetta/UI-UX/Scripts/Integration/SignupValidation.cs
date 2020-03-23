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
    [SerializeField] private CountryHandler country;
    [SerializeField] private Button signUpBtn;
    [SerializeField] private ErrorFadingHandler fNameError;
    [SerializeField] private ErrorFadingHandler lNameError;
    [SerializeField] private ErrorFadingHandler emailError;
    [SerializeField] private ErrorFadingHandler pwError;
    [SerializeField] private ErrorFadingHandler tremsAndPolicyError;
    [SerializeField] CheckBoxToggle termsCheck;
    [SerializeField] CheckBoxToggle policyCheck;
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
        /////very importnt 
        NetworkManager.Instance.SaveToken(signupResponse.token);
    }
    private void OnSignupFailed(NetworkParameters obj)
    {
        
    }
}

