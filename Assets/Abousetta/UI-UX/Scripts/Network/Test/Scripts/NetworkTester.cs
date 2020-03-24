using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class NetworkTester : MonoBehaviour
{
    /// 
    private IEnumerator DeleteForTestResponse ()
    {
        using ( var w = UnityWebRequest.Post (NetworkManager.Instance.networkManagerData.GetDeleteTestURL () , "") )
        {
            w.SetRequestHeader ("authorization" , NetworkManager.Instance.LoadToken ());
            yield return w.SendWebRequest ();
        }
    }
    public void DeleteForTest ()
    {
        StartCoroutine (DeleteForTestResponse ());
    }
    /// 
    [SerializeField] GetSignupData getSignupData;
    [SerializeField] GetVerifyMailData getVerifyMailData;
    [SerializeField] GetResetPasswordRequestData getRequestResetPasswordData;
    [SerializeField] GetCheckResetPasswordTokenData getCheckResetPasswordTokenData;
    [SerializeField] GetChangePasswordData getChangePasswordData;
    [SerializeField] GetResetPasswordData getResetPasswordData;
    [SerializeField] GetLoginData getLoginData;
    [SerializeField] GetCompleteProfileData getCompleteProfileData;
    [SerializeField] GetGetProfileData getGetProfileData;
    [SerializeField] GetPlayExperienceData getPlayExperienceData;
    [SerializeField] GetExperienceData getExperienceData;
    byte [] bytes;
    public void TestSignup ()
    {
        NetworkManager.Instance.Signup (getSignupData.getSignupData () , OnSignupSuccess , OnSignupFailed);
    }
    private void OnSignupSuccess (NetworkParameters obj)
    {
        SignupResponse signupResponse = (SignupResponse)obj.responseData;
        NetworkManager.Instance.SaveToken (signupResponse.token);
        getSignupData.GoToNextPanel ();

    }
    private void OnSignupFailed (NetworkParameters obj)
    {
        getSignupData.ShowErrors (obj.err.message);
        getSignupData.gameObject.SetActive (true);
    }
    public void TestVerifyMail ()
    {
        NetworkManager.Instance.VerifyEmail (getVerifyMailData.getVerifyEmailData () , OnVerifyMailSuccess , OnVerifyMailFailed);
    }
    private void OnVerifyMailSuccess (NetworkParameters obj)
    {
        getVerifyMailData.GoToNextPanel ();
    }
    private void OnVerifyMailFailed (NetworkParameters obj)
    {
        getVerifyMailData.ShowErrors (obj.err.message);
        getVerifyMailData.gameObject.SetActive (true);
    }
    public void TestLogin ()
    {
        print (getLoginData.getLoginData ().email);
        NetworkManager.Instance.Login (getLoginData.getLoginData () , OnLoginSuccess , OnLoginFailed);
    }
    private void OnLoginSuccess (NetworkParameters obj)
    {
        LoginResponse loginResponse = (LoginResponse)obj.responseData;
        NetworkManager.Instance.SaveToken (loginResponse.token);
        getLoginData.GoToNextPanel ();
    }
    private void OnLoginFailed (NetworkParameters obj)
    {
        getLoginData.gameObject.SetActive (true);
        getLoginData.ShowErrors (obj.err.message);
    }
    public void TestUpdateProfile ()
    {
        NetworkManager.Instance.UpdateProfile (getCompleteProfileData.getCompleteProfileData () , OnUpdateProfileSuccess , OnUpdateProfileFailed);
    }
    private void OnUpdateProfileSuccess (NetworkParameters obj)
    {
        UpdateProfileResponse updateProfileResponse = (UpdateProfileResponse)obj.responseData;
        TestGetProfile ();
        getCompleteProfileData.GoToNextPanel ();
    }
    private void OnUpdateProfileFailed (NetworkParameters obj)
    {
        getCompleteProfileData.gameObject.SetActive (true);
        getCompleteProfileData.ShowErrors (obj.err.message);
    }
    public void TestGetProfile ()
    {
        NetworkManager.Instance.GetProfile (OnGetProfileSuccess , OnGetProfileFailed);
    }
    private void OnGetProfileSuccess (NetworkParameters obj)
    {
        GetProfileResponse getProfileResponse = (GetProfileResponse)obj.responseData;
        getGetProfileData.ShowData (getProfileResponse.profile);
    }
    private void OnGetProfileFailed (NetworkParameters obj)
    {
        getGetProfileData.gameObject.SetActive (true);
        getGetProfileData.ShowErrors (obj.err.message);
        print (obj.err.message);
    }
    public void TestResetPasswordRequest ()
    {
        NetworkManager.Instance.ResetPasswordRequest (getRequestResetPasswordData.getResetPasswordRequestData () , OnResetPasswordRequestSuccess , OnResetPasswordRequestFailed);
    }
    private void OnResetPasswordRequestSuccess (NetworkParameters obj)
    {
        getRequestResetPasswordData.GoToNextPanel ();
    }
    private void OnResetPasswordRequestFailed (NetworkParameters obj)
    {
        getRequestResetPasswordData.gameObject.SetActive (true);
        getRequestResetPasswordData.ShowErrors (obj.err.message);
    }
    public void TestCheckResetPasswordToken ()
    {
        print (getCheckResetPasswordTokenData.getCheckResetPasswordTokenData ().email);
        NetworkManager.Instance.CheckResetPasswordToken (getCheckResetPasswordTokenData.getCheckResetPasswordTokenData () , OnCheckResetPasswordTokenSuccess , OnCheckResetPasswordTokenFailed);
    }
    private void OnCheckResetPasswordTokenSuccess (NetworkParameters obj)
    {
        getCheckResetPasswordTokenData.GoToNextPanel ();
    }
    private void OnCheckResetPasswordTokenFailed (NetworkParameters obj)
    {
        getCheckResetPasswordTokenData.gameObject.SetActive (true);
        getCheckResetPasswordTokenData.ShowErrors (obj.err.message);
    }
    public void TestChangePassword ()
    {
        NetworkManager.Instance.ChangePassword (getChangePasswordData.getChangePasswordData () , OnChangePasswordSuccess , OnChangePasswordFailed);
    }
    private void OnChangePasswordSuccess (NetworkParameters obj)
    {
        getChangePasswordData.GoToNextPanel ();
    }
    private void OnChangePasswordFailed (NetworkParameters obj)
    {
        getChangePasswordData.gameObject.SetActive (true);
        getChangePasswordData.ShowErrors (obj.err.message);
    }
    public void TestResetPassword ()
    {
        NetworkManager.Instance.ResetPassword (getResetPasswordData.getResetPasswordData () , OnResetPasswordSuccess , OnResetPasswordFailed);
    }
    private void OnResetPasswordSuccess (NetworkParameters obj)
    {
        getResetPasswordData.GoToNextPanel ();
    }
    private void OnResetPasswordFailed (NetworkParameters obj)
    {
        getResetPasswordData.gameObject.SetActive (true);
        getResetPasswordData.ShowErrors (obj.err.message);
    }
    public void TestUpdateExperience ()
    {
        NetworkManager.Instance.UpdateExperienceStatus (getPlayExperienceData.getPlayExperienceData () , OntUpdateExperienceSuccess , OntUpdateExperienceFailed);
    }
    private void OntUpdateExperienceSuccess (NetworkParameters obj)
    {
        TestGetxperiences ();
        getPlayExperienceData.GoToNextPanel ();
    }
    private void OntUpdateExperienceFailed (NetworkParameters obj)
    {

        getPlayExperienceData.gameObject.SetActive (true);
        getPlayExperienceData.ShowErrors (obj.err.message);
    }
    public void TestGetxperiences ()
    {
        NetworkManager.Instance.GetExperienceStatus (OnGetExperiencesSuccess , OnGetExperiencesFailed);
    }
    private void OnGetExperiencesSuccess (NetworkParameters obj)
    {

        ExperienceResponse getExperienceResponse = (ExperienceResponse)obj.responseData;
        string exS = "";
        for ( int i = 0 ; i < getExperienceResponse.experience.Length ; i++ )
        {
            exS += "Name :  " + getExperienceResponse.experience [i].experienceName +
                " lastPlayedAt " + getExperienceResponse.experience [i].lastPlayedAt +
                " finishedTimesCounter " + getExperienceResponse.experience [i].finishedTimesCounter +
                    " maxScore " + getExperienceResponse.experience [i].maxScore +
                    " playedTiemesCounter " + getExperienceResponse.experience [i].playedTiemesCounter +
                    " totalPlayedDuration " + getExperienceResponse.experience [i].totalPlayedDuration + "    \n";
        }
        getExperienceData.ShowData (exS);
    }
    private void OnGetExperiencesFailed (NetworkParameters obj)
    {

        getExperienceData.gameObject.SetActive (true);
        getExperienceData.ShowErrors (obj.err.message);
    }
    DetectObjectData detectObjectData = new DetectObjectData ();
    public void TestDetect ()
    {
        StartCoroutine (TakePicture ());
    }
    public IEnumerator TakePicture ()
    {
        yield return new WaitForEndOfFrame ();
        string path = Application.persistentDataPath + "/Screen-Capture" + ".png";
        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;
        var tex = new Texture2D (width , height , TextureFormat.RGB24 , false);

        // Read screen contents into the texture
        tex.ReadPixels (new Rect (0 , 0 , width , height) , 0 , 0);
        tex.Apply ();

        // Encode texture into PNG
        bytes = tex.EncodeToPNG ();
        detectObjectData.bytes = bytes;
        detectObjectData.score = "0.8";
        detectObjectData.detectionObjectName = "book";
        NetworkManager.Instance.DetectObject (detectObjectData , OnS , OnF);
        Destroy (tex);
    }
    private void OnS (NetworkParameters obj)
    {
        DetectObjectResponse detectObjectResponse = (DetectObjectResponse)obj.responseData;
        print (detectObjectResponse.detected);
    }
    private void OnF (NetworkParameters obj)
    {
        print (obj.err.message);
    }
}
