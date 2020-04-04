using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class NetworkTester : MonoBehaviour
{
    //private void Start ()
    //{


    //    //print (ssss);
    //}

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
    public void TestCheckout ()
    {
        CompleteCheckoutData checkoutData = new CompleteCheckoutData ();
        checkoutData.packageName = "";
        checkoutData.productId = "";
        checkoutData.purchaseToken = "";
        checkoutData.storeType = "";
        NetworkManager.Instance.CompleteCheckout (checkoutData , OnCheckoutSuccess , OnCheckoutFailed);
    }
    private void OnCheckoutSuccess (NetworkParameters obj)
    {
        //flow
    }
    private void OnCheckoutFailed (NetworkParameters obj)
    {
        //flow
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

        ProfileData ss = new ProfileData ();
        ss.scannedObjects = new ScannedObjects ();
        ss.scannedObjects.scannedObjects = new List<ScannedObject> ();
        ScannedObject tree = new ScannedObject ();
        tree.name = "tree";
        tree.counter = 51;
        ScannedObject book = new ScannedObject ();
        book.name = "book";
        book.counter = 22;
        //ss.firstName = "";
        //ss.lastName = "";
        //ss.nickName = "";
        //ss.country = "";
        //ss.birthDate = "YYYY-MM-DD";
        //ss.avatarId = "";
        //ss.email = "";
        //ss.gender = "";
        ss.keys = 10;
        ss.dailyStreaks = 20;
        ss.points = 12;
        ss.powerStones = 30;
        ss.scannedObjects.scannedObjects.Add (tree);
        ss.scannedObjects.scannedObjects.Add (book);
        ss.achievementsData = new AchievementsData ();
        ss.achievementsData.achievements = new List<int> ();
        ss.achievementsData.achievements.Add (1);
        ss.achievementsData.achievements.Add (2);
        ss.achievementsData.achievements.Add (3);
        //string achievementsData = JsonUtility.ToJson (ss.achievementsData);
        //print (achievementsData);
        NetworkManager.Instance.UpdateProfile (ss , OnUpdateProfileSuccess , OnUpdateProfileFailed);
    }
    private void OnUpdateProfileSuccess (NetworkParameters obj)
    {
        //UpdateProfileResponse updateProfileResponse = (UpdateProfileResponse)obj.responseData;
        TestGetProfile ();
        getCompleteProfileData.GoToNextPanel ();
        //flow
    }
    private void OnUpdateProfileFailed (NetworkParameters obj)
    {
        getCompleteProfileData.gameObject.SetActive (true);
        getCompleteProfileData.ShowErrors (obj.err.message);
        //print(obj.err.message);
    }
    public void TestGetProfile ()
    {
        NetworkManager.Instance.GetProfile (OnGetProfileSuccess , OnGetProfileFailed);
    }
    private void OnGetProfileSuccess (NetworkParameters obj)
    {
        GetProfileResponse getProfileResponse = (GetProfileResponse)obj.responseData;
        //print (getProfileResponse.profile.isConfirmed);
        //print (getProfileResponse.profile.points);
        //print (getProfileResponse.profile.dailyStreaks);
        //print (getProfileResponse.profile.keys);
        //print (getProfileResponse.profile.powerStones);
        //for ( int i = 0 ; i < getProfileResponse.profile.scannedObjects.Count ; i++ )
        //{
        //    print (getProfileResponse.profile.scannedObjects [i].name + " " + getProfileResponse.profile.scannedObjects [i].counter);
        //}
        //print (getProfileResponse.profile.email);
        //print (getProfileResponse.profile.country);
        //print (getProfileResponse.profile.avatarId);
        //print (getProfileResponse.profile.firstName);
        //print (getProfileResponse.profile.lastName);
        //print (getProfileResponse.profile.playerType);
        //print (getProfileResponse.profile.birthDate);
        //print (getProfileResponse.profile.gender);
        //print (getProfileResponse.profile.nickName);
        for ( int i = 0 ; i < getProfileResponse.profile.achievements.Count ; i++ )
        {
            print (getProfileResponse.profile.achievements [i]);
        }

        //= getProfileResponse.profile.isConfirmed;
        //= getProfileResponse.profile.points;
        //= getProfileResponse.profile.dailyStreaks;
        //= getProfileResponse.profile.keys;
        //= getProfileResponse.profile.powerStones;
        //for ( int i = 0 ; i < = getProfileResponse.profile.scannedObjects.Count ; i++ )
        //{
        //    = getProfileResponse.profile.scannedObjects [i].name + " " + = getProfileResponse.profile.scannedObjects [i].counter;
        //}
        //= getProfileResponse.profile.email;
        //= getProfileResponse.profile.country;
        //= getProfileResponse.profile.avatarId;
        //= getProfileResponse.profile.firstName;
        //= getProfileResponse.profile.lastName;
        //= getProfileResponse.profile.playerType;
        //= getProfileResponse.profile.birthDate;
        //= getProfileResponse.profile.gender;
        //= getProfileResponse.profile.nickName;
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
        NetworkManager.Instance.UpdateExperienceStatus (getPlayExperienceData.GetPlayExperienceDataHandeler () , OntUpdateExperienceSuccess , OntUpdateExperienceFailed);
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
            exS += "Name :  " + getExperienceResponse.experience [i].experienceCode +
                //" lastPlayedAt " + getExperienceResponse.experience [i].lastPlayedAt +
                " finishedTimesCounter " + getExperienceResponse.experience [i].finishedTimesCounter +
                    " maxScore " + getExperienceResponse.experience [i].maxScore +
                    " playedTiemesCounter " + getExperienceResponse.experience [i].playedTimesCounter + "    \n";
            //" totalPlayedDuration " + getExperienceResponse.experience [i].totalPlayedDuration + "    \n";
        }
        getExperienceData.ShowData (exS);
        for ( int i = 0 ; i < getExperienceResponse.experience.Length ; i++ )
        {
            print (getExperienceResponse.experience[i].rate);
        }
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
    public void TestUpdateBundle ()
    {
        CollectBundleTokenData collectBundleTokenData = new CollectBundleTokenData ();
        collectBundleTokenData.bundleId = "507f191e810c19729de860aa";
        collectBundleTokenData.tokenName = "prinsceEl7sab";
        NetworkManager.Instance.UpdateCollectedTokens (collectBundleTokenData , OnUpdateBundleSuccess , OnUpdateBundleFailed);
    }
    private void OnUpdateBundleSuccess (NetworkParameters obj)
    {
        TestGetBunlde ();
    }
    private void OnUpdateBundleFailed (NetworkParameters obj)
    {
        print (obj.err.message);
    }
    public void TestGetBunlde ()
    {
        NetworkManager.Instance.GetBundlesData (OntGetBundleSuccess , OntGetBundleFailed);
    }
    private void OntGetBundleSuccess (NetworkParameters obj)
    {
        BundleResponse br = (BundleResponse)obj.responseData;
        for ( int i = 0 ; i < br.bundles.Count ; i++ )
        {

            print (br.bundles [i].bundleId);
            for ( int j = 0 ; j < br.bundles [i].collectedTokens.Count ; j++ )
            {

                print (br.bundles [i].collectedTokens [j]);
            }

        }
    }
    private void OntGetBundleFailed (NetworkParameters obj)
    {
        print (obj.err.message);
    }
    public void TestGetAllBunldes ()
    {
        NetworkManager.Instance.GetExperienceBundlesData (OntGetAllBundleSuccess , OntGetAllBundleFailed);
    }
    private void OntGetAllBundleSuccess (NetworkParameters obj)
    {
        ExperienceBundlesResponse br = (ExperienceBundlesResponse)obj.responseData;
        print (br.bundles);
        for ( int i = 0 ; i < br.bundles.Count ; i++ )
        {
            for ( int j = 0 ; j < br.bundles [i].experience.Count ; j++ )
            {
                print (br.bundles [i].experience [j]);

            }
            print (br.bundles [i].name);
            for ( int j = 0 ; j < br.bundles [i].tokens.Count ; j++ )
            {
                print (br.bundles [i].tokens [j]);
            }
            print (br.bundles [i].name);
            print (br.bundles [i]._id);

        }
    }
    private void OntGetAllBundleFailed (NetworkParameters obj)
    {
        print (obj.err.message);
    }
    public void TestCreateDummyAccount ()
    {
        CreateDummyAccountData createDummyAccountData = new CreateDummyAccountData ();
        createDummyAccountData.deviceId = "123456";
        createDummyAccountData.deviceType = "android";
        NetworkManager.Instance.CreateDummyAccount (createDummyAccountData , OnCreateDummyAccountSusccess , OnCreateDummyAccountFailed);
    }
    private void OnCreateDummyAccountSusccess (NetworkParameters obj)
    {
        CreateDummyAccountResponse response = (CreateDummyAccountResponse)obj.responseData;
        NetworkManager.Instance.SaveToken (response.token);
    }
    private void OnCreateDummyAccountFailed (NetworkParameters obj)
    {
        print (obj.err.message);
    }
    public void TestLinkAccount ()
    {
        LinkAccountData linkAccountData = new LinkAccountData ();
        linkAccountData.firstName = "7amo";
        linkAccountData.lastName = "beeka";
        linkAccountData.email = "7amobeeka@maharaganat.com";
        linkAccountData.country = "omEldonia";
        linkAccountData.password = "7amoPassword";
        NetworkManager.Instance.LinkAccount (linkAccountData , OnLinkAccountSusccess , OnLinkAccountFailed);
    }
    private void OnLinkAccountSusccess (NetworkParameters obj)
    {
        LinkAccountResponse response = (LinkAccountResponse)obj.responseData;
        NetworkManager.Instance.SaveToken (response.token);
    }
    private void OnLinkAccountFailed (NetworkParameters obj)
    {
        print (obj.err.message);
    }
    public void TestRateExperience ()
    {
        ExperienceRateData rateData = new ExperienceRateData ();
        rateData.experienceCode = "0Sc";
        rateData.rate = "3";
        NetworkManager.Instance.RateExperience (rateData , OnRateExperienceSusccess , OnRateExperienceFailed);
    }
    private void OnRateExperienceSusccess (NetworkParameters obj)
    {
    }
    private void OnRateExperienceFailed (NetworkParameters obj)
    {
        print (obj.err.message);
    }
}
