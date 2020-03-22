using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.IO;

public class NetworkManager : MonoBehaviour
{
    public NetworkManagerData networkManagerData;
    public NetworkParameters np;
    private static NetworkManager instance;
    bool isSuccess;
    string tokenPath;
    public static NetworkManager Instance { get => instance; set => instance = value; }
    string json;

    private void Awake ()
    {
        np = new NetworkParameters ();
        if ( Instance == null )
        {
            Instance = this;
            DontDestroyOnLoad (gameObject);
        }
        else
            Destroy (gameObject);
        tokenPath = Application.persistentDataPath + "/Token" + ".txt";

    }
    #region Internet connection
    public bool CheckServerConnectivity ()
    {
        // Check internet connection.
        //TODO
        bool networkStatus = false;
        StartCoroutine (checkServerConnection ((isConnected) =>
        {
            // handle connection status here
            if ( isConnected )
            {
                networkStatus = true;
                Debug.Log ("Network status " + networkStatus);
            }
            else
            {
                networkStatus = false;
                Debug.LogError ("Network status " + networkStatus);
            }
        }));
        return networkStatus;
    }
    IEnumerator checkServerConnection (Action<bool> action)
    {
        WWW www = new WWW (networkManagerData.serverURL);
        yield return www;
        if ( www.error != null )
        {
            action (false);
        }
        else
        {
            action (true);
        }
    }
    public bool CheckInternetConnectivity ()
    {
        // Check internet connection.
        //TODO
        bool networkStatus = false;
        StartCoroutine (checkInternetConnection ((isConnected) =>
        {
            // handle connection status here
            if ( isConnected )
            {
                networkStatus = true;
                Debug.Log ("Network status " + networkStatus);
            }
            else
            {
                networkStatus = false;
                Debug.LogError ("Network status " + networkStatus);
            }
        }));
        return networkStatus;
    }
    IEnumerator checkInternetConnection (Action<bool> action)
    {
        WWW www = new WWW ("http://google.com");
        yield return www;
        if ( www.error != null )
        {
            action (false);
        }
        else
        {
            action (true);
        }
    }
    #endregion
    public bool Signup (SignupData signupData , Action<NetworkParameters> onSuccess , Action<NetworkParameters> onFailed)
    {
        WWWForm form = new WWWForm ();
        form.AddField ("firstName" , signupData.firstName);
        form.AddField ("lastName" , signupData.lastName);
        form.AddField ("email" , signupData.email);
        form.AddField ("password" , signupData.password);
        form.AddField ("country" , signupData.country);
        form.AddField ("deviceType" , signupData.deviceType);
        form.AddField ("deviceId" , signupData.deviceId);
        StartCoroutine (PostRequest<SignupResponse> (networkManagerData.GetSignupURL () , form , false , onSuccess , onFailed));
        return isSuccess;
    }
    public bool Login (LoginData loginData , Action<NetworkParameters> onSuccess , Action<NetworkParameters> onFailed)
    {
        WWWForm form = new WWWForm ();
        form.AddField ("email" , loginData.email);
        form.AddField ("password" , loginData.password);
        form.AddField ("deviceType" , loginData.deviceType);
        form.AddField ("deviceId" , loginData.deviceId);
        StartCoroutine (PostRequest<LoginResponse> (networkManagerData.GetLoginURL () , form , false , onSuccess , onFailed));
        return isSuccess;
    }
    public bool CreateDummyAccount (CreateDummyAccountData createDummyData , Action<NetworkParameters> onSuccess , Action<NetworkParameters> onFailed)
    {
        WWWForm form = new WWWForm ();
        form.AddField ("deviceType" , createDummyData.deviceType);
        form.AddField ("deviceId" , createDummyData.deviceId);
        StartCoroutine (PostRequest<CreateDummyAccountResponse> (networkManagerData.GetCreateDummyAccountURL () , form , false , onSuccess , onFailed));
        return isSuccess;
    }

    public bool UpdateProfile (ProfileData updateProfileData , Action<NetworkParameters> onSuccess , Action<NetworkParameters> onFailed)
    {
        WWWForm form = new WWWForm ();
        if ( !String.IsNullOrEmpty (updateProfileData.firstName) )
            form.AddField ("firstName" , updateProfileData.firstName);
        if ( !String.IsNullOrEmpty (updateProfileData.lastName) )
            form.AddField ("lastName" , updateProfileData.lastName);
        if ( !String.IsNullOrEmpty (updateProfileData.nickName) )
            form.AddField ("nickName" , updateProfileData.nickName);
        if ( !String.IsNullOrEmpty (updateProfileData.birthDate) )
            form.AddField ("birthDate" , updateProfileData.birthDate);
        if ( !String.IsNullOrEmpty (updateProfileData.country) )
            form.AddField ("country" , updateProfileData.country);
        if ( !String.IsNullOrEmpty (updateProfileData.gender) )
            form.AddField ("gender" , updateProfileData.gender);
        if ( !String.IsNullOrEmpty (updateProfileData.avatarId) )
            form.AddField ("avatarId" , updateProfileData.avatarId);
        StartCoroutine (PostRequest<UpdateProfileResponse> (networkManagerData.GetUpdateProfileURL () , form , true , onSuccess , onFailed));
        return isSuccess;
    }
    public bool GetProfile (Action<NetworkParameters> onSuccess , Action<NetworkParameters> onFailed)
    {
        StartCoroutine (GetRequest<GetProfileResponse> (networkManagerData.GetGetProfileURL () , onSuccess , onFailed));
        return isSuccess;
    }
    public bool UpdateExperienceStatus (ExperiencePlayData experiencePlayData , Action<NetworkParameters> onSuccess , Action<NetworkParameters> onFailed)
    {
        WWWForm form = new WWWForm ();
        form.AddField ("status" , experiencePlayData.status);
        form.AddField ("experienceName" , experiencePlayData.experienceName);
        form.AddField ("score" , experiencePlayData.score);
        StartCoroutine (PostRequest<ExperienceResponse> (networkManagerData.GetUpdateExperienceStatusURL () , form , true , onSuccess , onFailed));
        return isSuccess;
    }
    public bool GetExperienceStatus (Action<NetworkParameters> onSuccess , Action<NetworkParameters> onFailed)
    {

        StartCoroutine (GetRequest<ExperienceResponse> (networkManagerData.GetExperienceStatusURL () , onSuccess , onFailed));
        return isSuccess;
    }
    public bool LinkAccount (LinkAccountData linkData , Action<NetworkParameters> onSuccess , Action<NetworkParameters> onFailed)
    {
        WWWForm form = new WWWForm ();
        form.AddField ("firstName" , linkData.firstName);
        form.AddField ("lastName" , linkData.lastName);
        form.AddField ("email" , linkData.email);
        form.AddField ("password" , linkData.password);
        form.AddField ("country" , linkData.country);
        StartCoroutine (PostRequest<LinkAccountResponse> (networkManagerData.GetLinkAccountURL () , form , true , onSuccess , onFailed));
        return isSuccess;
    }
    public bool VerifyEmail (VerifyEmailData verifyEmailData , Action<NetworkParameters> onSuccess , Action<NetworkParameters> onFailed)
    {
        WWWForm form = new WWWForm ();
        form.AddField ("activationCode" , verifyEmailData.activationCode);
        StartCoroutine (PostRequest<VerifyEmailResponse> (networkManagerData.GetVerifyEmailURL () , form , true , onSuccess , onFailed));
        return isSuccess;
    }
    public bool ResetPasswordRequest (ResetPasswordRequestData resetPasswordRequestData , Action<NetworkParameters> onSuccess , Action<NetworkParameters> onFailed)
    {
        WWWForm form = new WWWForm ();
        form.AddField ("email" , resetPasswordRequestData.email);
        StartCoroutine (PostRequest<ResetPasswordRequestResponse> (networkManagerData.GetResetPasswordRequestURL () , form , true , onSuccess , onFailed));
        return isSuccess;
    }
    public bool CheckResetPasswordToken (CheckResetPasswordTokenData checkResetPasswordTokenData , Action<NetworkParameters> onSuccess , Action<NetworkParameters> onFailed)
    {
        WWWForm form = new WWWForm ();
        print (checkResetPasswordTokenData.email);
        form.AddField ("email" , checkResetPasswordTokenData.email);
        form.AddField ("token" , checkResetPasswordTokenData.token); //token is activation code 
        StartCoroutine (PostRequest<CheckResetPasswordTokenResponse> (networkManagerData.GetCheckResetPasswordTokenURL () , form , false , onSuccess , onFailed));
        return isSuccess;
    }
    public bool ResetPassword (ResetPasswordData resetPasswordData , Action<NetworkParameters> onSuccess , Action<NetworkParameters> onFailed)
    {
        print (resetPasswordData.email);
        print (resetPasswordData.password);
        print (resetPasswordData.token);
        WWWForm form = new WWWForm ();
        form.AddField ("email" , resetPasswordData.email);
        form.AddField ("password" , resetPasswordData.password);
        form.AddField ("token" , resetPasswordData.token);
        StartCoroutine (PostRequest<ResetPasswordResponse> (networkManagerData.GetResetPasswordURL () , form , false , onSuccess , onFailed));
        return isSuccess;
    }
    public bool ChangePassword (ChangePasswordData changePasswordData , Action<NetworkParameters> onSuccess , Action<NetworkParameters> onFailed)
    {
        WWWForm form = new WWWForm ();
        form.AddField ("oldPassword" , changePasswordData.oldPassword);
        form.AddField ("newPassword" , changePasswordData.newPassword);
        StartCoroutine (PostRequest<ChangePasswordResponse> (networkManagerData.GetChangePasswordURL () , form , true , onSuccess , onFailed));
        return isSuccess;
    }
    public bool ResendActivationCode (Action<NetworkParameters> onSuccess , Action<NetworkParameters> onFailed)
    {
        StartCoroutine (PostRequest<ResendActivationCodeResponse> (networkManagerData.GetResendActivationCodeURL () , true , onSuccess , onFailed));
        return isSuccess;
    }
    private IEnumerator PostRequest<T> (string url , WWWForm form , bool isAuthorizeTokenNeeeded , Action<NetworkParameters> onSuccess , Action<NetworkParameters> onFailed) where T : ResponseData
    {
        Debug.Log (url);
        using ( var w = UnityWebRequest.Post (url , form) )
        {
            if ( isAuthorizeTokenNeeeded )
                AuthorizeWithToken (w);
            yield return w.SendWebRequest ();
            if ( w.isNetworkError || w.isHttpError )
            {
                RequestFailed (w);
                if ( onFailed != null )
                    onFailed.Invoke (np);
                else
                    Debug.LogError ("NetworkParameter onFailed is NULL!!!!");
            }
            else
            {
                Debug.Log (w.downloadHandler.text);

                RequestSucceed<T> (w);
                if ( onSuccess != null )
                    onSuccess.Invoke (np);
                else
                    Debug.LogError ("NetworkParameter onSuccess is NULL!!!!");
            }
        }
    }
    private IEnumerator PostRequest<T> (string url , bool isAuthorizeTokenNeeeded , Action<NetworkParameters> onSuccess , Action<NetworkParameters> onFailed) where T : ResponseData
    {
        using ( var w = UnityWebRequest.Post (url , "") )
        {
            Debug.Log (url);
            if ( isAuthorizeTokenNeeeded )
                AuthorizeWithToken (w);
            yield return w.SendWebRequest ();
            if ( w.isNetworkError || w.isHttpError )
            {
                RequestFailed (w);
                if ( onFailed != null )
                    onFailed.Invoke (np);
                else
                    Debug.LogError ("NetworkParameter onFailed is NULL!!!!");
            }
            else
            {
                RequestSucceed<T> (w);
                if ( onSuccess != null )
                    onSuccess.Invoke (np);
                else
                    Debug.LogError ("NetworkParameter onSuccess is NULL!!!!");
            }
        }
    }
    private IEnumerator GetRequest<T> (string url , Action<NetworkParameters> onSuccess , Action<NetworkParameters> onFailed) where T : ResponseData
    {
        using ( var w = UnityWebRequest.Get (url) )
        {
            Debug.Log (url);
            AuthorizeWithToken (w);
            yield return w.SendWebRequest ();
            if ( w.isNetworkError || w.isHttpError )
            {
                RequestFailed (w);
                if ( onFailed != null )
                    onFailed.Invoke (np);
                else
                    Debug.LogError ("NetworkParameter onFailed is NULL!!!!");
            }
            else
            {
                RequestSucceed<T> (w);
                if ( onSuccess != null )
                    onSuccess.Invoke (np);
                else
                    Debug.LogError ("NetworkParameter onSuccess is NULL!!!!");
            }
        }
    }
    void RequestFailed (UnityWebRequest w)
    {
        isSuccess = false;
        Debug.LogError ("isHttpError " + w.isHttpError + "\nisNetworkError " + w.isNetworkError + "\nResponse: " + w.downloadHandler.text + "\nError " + w.error);
        np.err = JsonUtility.FromJson<NetworkError> (w.downloadHandler.text);
        Debug.LogError (" status: " + np.err.status + " customCode: " + np.err.customCode + " message:  " + np.err.message);
    }
    void RequestSucceed<T> (UnityWebRequest w) where T : ResponseData
    {
        Debug.Log (w.downloadHandler.text);
        json = w.downloadHandler.text;
        GeneralResponse<T> response = new GeneralResponse<T> ();
        response = JsonUtility.FromJson<GeneralResponse<T>> (json);
        np.responseData = response.data;
        np.err = null;
        isSuccess = true;
    }
    void AuthorizeWithToken (UnityWebRequest w)
    {
        w.SetRequestHeader ("authorization" , LoadToken ());
    }
    #region Token
    public void SaveToken (string tokenText)
    {
        System.IO.File.WriteAllText (tokenPath , tokenText);
    }
    public string LoadToken ()
    {
        if ( File.Exists (tokenPath) )
        {
            return File.ReadAllText (tokenPath);
        }
        else
        {
            Debug.LogError ("UnAutorized!!!!");
            return "UnAutorized";
        }
    }
    #endregion
    public void DetectObject (DetectObjectData detectObjectData , Action<NetworkParameters> onSuccess , Action<NetworkParameters> onFailed)
    {
        print ("start detect Obj");
        // Create a Web Form
        WWWForm form = new WWWForm ();
        form.AddField ("score" , "0.8");
        form.AddField ("objectToDetect" , detectObjectData.detectionObjectName);
        form.AddBinaryData ("scannedImg" , detectObjectData.bytes , "screenShot.png" , "image/png");
        print ("DetectObject");
        StartCoroutine (PostRequest<DetectObjectResponse> (networkManagerData.GetDetecObjectURL () , form , true , onSuccess , onFailed));
    }
}