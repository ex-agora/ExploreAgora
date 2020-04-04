using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = " NetworkManagerData" , menuName = "SO/App/Network" , order = 0)]
public class NetworkManagerData : ScriptableObject
{
    public string serverURL;
    public string login;
    public string signup;
    public string createDummyAccount;
    public string updateProfile;
    public string getProfile;
    public string linkAccount;
    public string verifyEmail;
    public string resendActivationCode;
    public string resetPasswordRequest;
    public string deleteForTest;
    public string checkResetPasswordToken;
    public string resetPassword;
    public string changePassword;
    public string experiencePlayed;
    public string experienceData;
    public string detectObj;
    public string completeCheckout;
    public string bundle;
    public string collectBundleToken;
    public string experienceBundles;
    public string gameData;
    public string experienceRate;
    public string promoCode;
    public string GetSignupURL ()
    {
        return serverURL + "/" + signup;
    }
    public string GetLoginURL ()
    {
        return serverURL + "/" + login;
    }
    public string GetCreateDummyAccountURL ()
    {
        return serverURL + "/" + createDummyAccount;
    }    
    public string GetUpdateProfileURL ()
    {
        return serverURL + "/" + updateProfile;
    }
    public string GetGetProfileURL ()
    {
        return serverURL + "/" + getProfile;
    }    
    public string GetLinkAccountURL ()
    {
        return serverURL + "/" + linkAccount;
    }
    public string GetVerifyEmailURL ()
    {
        return serverURL + "/" + verifyEmail;
    }    
    public string GetResendActivationCodeURL ()
    {
        return serverURL + "/" + resendActivationCode;
    }
    public string GetDeleteTestURL ()
    {
        return serverURL + "/" + deleteForTest;
    }    
    public string GetResetPasswordRequestURL ()
    {
        return serverURL + "/" + resetPasswordRequest;
    }    
    public string GetCheckResetPasswordTokenURL ()
    {
        return serverURL + "/" + checkResetPasswordToken;
    }
    public string GetResetPasswordURL ()
    {
        return serverURL + "/" + resetPassword;
    }    
    public string GetChangePasswordURL ()
    {
        return serverURL + "/" + changePassword;
    }    
    public string GetUpdateExperienceStatusURL ()
    {
        return serverURL + "/" + experiencePlayed;
    }   
    public string GetExperienceStatusURL ()
    {
        return serverURL + "/" + experienceData;
    }    
    public string GetDetecObjectURL ()
    {
        return serverURL + "/" + detectObj;
    }    
    public string GetCompleteCheckoutURL ()
    {
        return serverURL + "/" + completeCheckout;
    }
    public string GetBundleURL ()
    {
        return serverURL + "/" + bundle;
    }    
    public string GetCollectBundleTokenURL ()
    {
        return serverURL + "/" + collectBundleToken;
    }
    public string GetGameDataURL ()
    {
        return serverURL + "/" + gameData;
    }
    public string GetExperienceBundlesURL ()
    {
        return serverURL + "/" + experienceBundles;
    }
    public string GetExperienceRateURL ()
    {
        return serverURL + "/" + experienceRate;
    }
    public string GetPromoCodeURL ()
    {
        return serverURL + "/" + promoCode;
    }
}
