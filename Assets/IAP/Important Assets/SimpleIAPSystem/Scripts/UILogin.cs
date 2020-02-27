using UnityEngine;
using UnityEngine.UI;

namespace SIS
{
    /// <summary>
    /// Sample UI script for registering new accounts and logging in with PlayFab.
    /// The current implementation makes use of email addresses for creating new users.
    /// </summary>
    public class UILogin : MonoBehaviour
    {
        /// <summary>
        /// Scene to load immediately after successfully logging in.
        /// </summary>
        public string nextScene;

        /// <summary>
        /// Loading screen game object to activate between login attempts.
        /// </summary>
        public GameObject loadingScreen;

        /// <summary>
        /// Email address field to register or log in.
        /// </summary>
        public InputField emailField;

        /// <summary>
        /// Password field to register or log in.
        /// </summary>
        public InputField passwordField;

        /// <summary>
        /// Error text displayed in case of login issues.
        /// </summary>
        public Text errorText;


        [Header("DO NOT USE (in development)")]
        public Text userText;
        public Text keyText;
        public InputField userField;
        public InputField[] keyFields;

        //PlayerPref key used for storing the email address
        private const string emailPref = "AccountEmail";


        #if !PLAYFAB
        void Awake()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
        }
        #endif


        #if PLAYFAB
        //pre-load login values
        void Start()
        {
            #if !UNITY_FACEBOOK
            if (PlayerPrefs.HasKey(emailPref))
                emailField.text = PlayerPrefs.GetString(emailPref);
            #else
                loadingScreen.SetActive(true);
            #endif
        }


        void OnEnable()
        {
            PlayfabManager.loginSucceededEvent += OnLoggedIn;
            PlayfabManager.loginFailedEvent += OnLoginFail;
            PlayfabManager.linkSucceededEvent += OnLinkSucceeded;
        }


        void OnDisable()
        {
            PlayfabManager.loginSucceededEvent -= OnLoggedIn;
            PlayfabManager.loginFailedEvent -= OnLoginFail;
            PlayfabManager.linkSucceededEvent -= OnLinkSucceeded;
        }


        //loads the desired scene immediately after loggin in
        private void OnLoggedIn()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
        }


        //hides the loading screen in case of failed login, so the user can try again
        private void OnLoginFail(string error)
        {
            loadingScreen.SetActive(false);
            errorText.text = error;
        }


        /// <summary>
        /// Registers a new account with PlayFab, mapped to a UI button.
        /// </summary>
        public void RegisterAccount()
        {
            errorText.text = "";

            if (emailField.text.Length == 0 || passwordField.text.Length == 0)
            {
                errorText.text = "All fields are required.";
                return;
            }

            if (passwordField.text.Length <= 5)
            {
                errorText.text = "Password must be longer than 5 characters.";
                return;
            }

            loadingScreen.SetActive(true);
            PlayerPrefs.SetString(emailPref, emailField.text);
            PlayfabManager.RegisterAccount(emailField.text, passwordField.text);
        }


        /// <summary>
        /// Tries to login via email, mapped to a UI button.
        /// </summary>
        public void LoginWithEmail()
        {
            errorText.text = "";

            if (emailField.text.Length == 0 || passwordField.text.Length == 0 || passwordField.text.Length <= 5)
            {
                errorText.text = "Email or Password is invalid. Check credentials and try again.";
                return;
            }

            loadingScreen.SetActive(true);
            PlayerPrefs.SetString(emailPref, emailField.text);
            PlayfabManager.LoginWithEmail(emailField.text, passwordField.text);
        }


        public void LoginWithDevice()
        {
            if (PlayfabManager.GetInstance())
                PlayfabManager.GetInstance().LoginWithDevice();
        }


        /// <summary>
        /// Requests a new password, mapped to a UI button.
        /// </summary>
        public void ForgotPassword()
        {
            errorText.text = "";

            if(emailField.text.Length == 0)
            {
                errorText.text = "Please enter your email and retry.";
                return;
            }
            
            PlayfabManager.ForgotPassword(emailField.text);
        }


        public void LinkAccountOther()
        {
            PlayfabManager.LinkAccountOther();
        }


        public void UnlinkOther()
        {
            PlayfabManager.UnlinkCustomId();
        }


        public void LoginAccountOther()
        {
            string user = userField.text;
            string key = "";
            for (int i = 0; i < keyFields.Length; i++)
            {
                if (string.IsNullOrEmpty(keyFields[i].text) || keyFields[i].text.Length < 4)
                {
                    Debug.Log("Key does not meet requirements.");
                    break;
                }

                key += keyFields[i].text;
            }

            key = key.Replace(" ", "").ToUpper();
            user = userField.text.Replace(" ", "").ToUpper();
            PlayfabManager.LoginAccountOther(user, key);
        }


        void OnLinkSucceeded(string key)
        {
            if(!string.IsNullOrEmpty(key))
            {
                userText.text = PlayfabManager.userId;
                keyText.text = key;
            }

            Debug.Log("OnLinkSucceeded.");
        }

        
        void OnLinkFailed()
        {

        }
        #endif
    }
}
