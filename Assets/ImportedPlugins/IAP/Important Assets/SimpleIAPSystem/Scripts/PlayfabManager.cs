/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

#if PLAYFAB_PAYPAL || PLAYFAB_STEAM || PLAYFAB_VALIDATION
#define PLAYFAB
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SIS.SimpleJSON;
#if SIS_IAP
using UnityEngine.Purchasing;
#endif

#if PLAYFAB
using PlayFab;
using PlayFab.ClientModels;
#endif

#if PLAYFAB_STEAM
using Steamworks;
#endif

#if UNITY_FACEBOOK
using Facebook.Unity;
#endif

#pragma warning disable 0162, 0414
namespace SIS
{
    /// <summary>
    /// Manager integrating PlayFab's ClientModels for handling all related web requests, such as
    /// logging in and syncing Simple IAP System storage (purchases, player data, currency) with PlayFab.
    /// </summary>
    public class PlayfabManager : MonoBehaviour
    {
        #if PLAYFAB
        /// <summary>
        /// Static reference to this script.
        /// </summary>
        private static PlayfabManager instance;

        /// <summary>
        /// PlayFab account id of the user logged in.
        /// </summary>
        public static string userId;

        #if PLAYFAB_STEAM
        protected Callback<GetAuthSessionTicketResponse_t> authTicketResponse;
        private byte[] ticket;
        private HAuthTicket authTicket;
        private uint ticketLength;
        private string hexTicket = "";
        #endif

        #if UNITY_FACEBOOK
        private string facebookAccess = "";
        #endif

        /// <summary>
        /// Fired when the user successfully logged in to a PlayFab account.
        /// </summary>
        public static event Action loginSucceededEvent;

        /// <summary>
        /// Fired when logging in fails due to authentication or other issues.
        /// </summary>
        public static event Action<string> loginFailedEvent;

        /// <summary>
        /// Fired when successfully connecting/migrating to a different account.
        /// </summary>
        public static event Action<string> linkSucceededEvent;

        /// <summary>
        /// Fired when connecting to a different account failed.
        /// </summary>
        public static event Action linkFailedEvent;

        //public MergeConflictRule mergeRule = MergeConflictRule.UserPrompt;
        //public static event Action dataConflictEvent;

        private static string linkSecretKey = "";
        private static string linkExpiredKey = "";
        private PlayFab.ClientModels.LoginResult loginResult;
        private JSONNode serverData;
        private GetPlayerCombinedInfoRequestParams accountParams;


        /// <summary>
        /// Returns a static reference to this script.
        /// </summary>
        public static PlayfabManager GetInstance()
        {
            return instance;
        }


        //setting up parameters and callbacks
        void Awake()
        {
            //make sure we keep one instance of this script in the game
            if (instance)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(this);

            //set static reference
            instance = this;
            bool validationOnly = false;

            #if PLAYFAB_VALIDATION
                validationOnly = true;
            #endif

            accountParams = new GetPlayerCombinedInfoRequestParams()
            {
                GetUserInventory = !validationOnly,
                GetUserVirtualCurrency = !validationOnly,
                GetUserData = !validationOnly,
                UserDataKeys = new List<string>() { DBManager.playerKey, DBManager.selectedKey }
            };

            if (validationOnly) return;
            //subscribe to selected events for handling them automatically
			ShopManager.itemSelectedEvent += x => SetSelected();
			ShopManager.itemDeselectedEvent += x => SetSelected();
        }


        //try to do device login
        void Start()
        {
            //no auto-login when only validation is enabled
            #if PLAYFAB_VALIDATION
                return;
            #endif

            #if UNITY_FACEBOOK
            //if (!FB.IsInitialized)
            //    FB.Init(FacebookInitialized);
            #elif PLAYFAB_STEAM
                authTicketResponse = Callback<GetAuthSessionTicketResponse_t>.Create(OnGetAuthSessionTicketResponse);
                ticket = new byte[1024];
                authTicket = SteamUser.GetAuthSessionTicket(ticket, 1024, out ticketLength);
            #else
                //LoginWithDevice();
            #endif
        }


        /// <summary>
        /// Grant non-consumables on the user's inventory on PlayFab. Consumables will be granted only
        /// locally to minimize requests - please call SetFunds or SetPlayerData afterwards if needed.
        /// </summary>
        public static IEnumerator SetPurchase(List<string> productIDs)
        {
            //create a separate list for non-consumables to request
            List<string> cloudProducts = new List<string>();
            for(int i = 0; i < productIDs.Count; i++)
            {
                if((int)IAPManager.GetIAPObject(productIDs[i]).type > 0)
                    cloudProducts.Add(productIDs[i]);
            }            

            bool commit = true;
            if(cloudProducts.Count > 0)
            {
                ExecuteCloudScriptRequest cloudRequest = new ExecuteCloudScriptRequest()
                {
                    FunctionName = "grantItems",
                    FunctionParameter = new { itemIds = cloudProducts.ToArray() }
                };
    
                bool result = false;
                PlayFabClientAPI.ExecuteCloudScript(cloudRequest, (cloudResult) =>
                {
                    result = true; 
                }, (error) =>
                {
                        OnPlayFabError(error);
                        commit = false;
                        result = true;
                });
    
                while(!result)
                {
                    yield return null;
                }
            }

            //only grant products if the cloud request was successful
            if(commit == true)
            {
                for(int i = 0; i < productIDs.Count; i++)
                {
                    if(DBManager.GetPurchase(productIDs[i]) == 0)
					    IAPManager.GetInstance().PurchaseVerified(productIDs[i]);
                }
            }

            yield return null;
        }


        /// <summary>
        /// Uploads local player data to PlayFab. Call this after manipulating player data manually, e.g. via DBManager.IncreasePlayerData.
        /// Note that this method is called automatically for syncing consumable usage counts on product purchases.
        /// </summary>
        public static void SetPlayerData()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add(DBManager.playerKey, DBManager.GetJSON(DBManager.playerKey));

            UpdateUserDataRequest request = new UpdateUserDataRequest()
            {
                Data = dic
            };

            PlayFabClientAPI.UpdateUserData(request, null, OnPlayFabError);
        }


        /// <summary>
        /// Uploads local currency balance to PlayFab. Call this after giving currency manually, e.g. via DBManager.IncreaseFunds.
        /// Note that the virtual currency balance is synced automatically with PlayFab on product purchases.
        /// </summary>
        public static void SetFunds()
        {
            Dictionary<string, int> dic = DBManager.GetCurrencies();
            ExecuteCloudScriptRequest cloudRequest = new ExecuteCloudScriptRequest()
            {
                FunctionName = "addCurrency",
                FunctionParameter = new { data = dic }
            };

            PlayFabClientAPI.ExecuteCloudScript(cloudRequest, null, OnPlayFabError);
        }      


        /// <summary>
        /// Uploads local item selection states to PlayFab. Call this after manual selections, e.g. via DBManager.SetSelected.
        /// Note that selection states are synced automatically when selecting or deseleting items in the shop.
        /// </summary>
        public static void SetSelected()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add(DBManager.selectedKey, DBManager.GetJSON(DBManager.selectedKey));

            UpdateUserDataRequest request = new UpdateUserDataRequest()
            {
                Data = dic
            };

            PlayFabClientAPI.UpdateUserData(request, null, null);
        }


        /// <summary>
        /// Logs in with the user device id, using the correct PlayFab method per platform.
        /// A new account will be created when no account is associated with the device id.
        /// </summary>
        public void LoginWithDevice(Action<bool> resultCallback = null)
        {
            #if UNITY_ANDROID
            LoginWithAndroidDeviceIDRequest request = new LoginWithAndroidDeviceIDRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                AndroidDeviceId = DBManager.GetDeviceId(),
                CreateAccount = true,
                OS = SystemInfo.operatingSystem,
                AndroidDevice = SystemInfo.deviceModel,
                InfoRequestParameters = accountParams
            };

            PlayFabClientAPI.LoginWithAndroidDeviceID(request, (result) =>
            {
                OnLoggedIn(result);
                if(resultCallback != null) resultCallback(true);
            }, (error) =>
            {
                OnPlayFabError(error);
                if(resultCallback != null) resultCallback(false);
            });
            
            #elif UNITY_IOS
            LoginWithIOSDeviceIDRequest request = new LoginWithIOSDeviceIDRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                DeviceId = DBManager.GetDeviceId(),
                CreateAccount = true,
                OS = SystemInfo.operatingSystem,
                DeviceModel = SystemInfo.deviceModel,
                InfoRequestParameters = accountParams
            };

            PlayFabClientAPI.LoginWithIOSDeviceID(request, (result) =>
            {
                OnLoggedIn(result);
                if(resultCallback != null) resultCallback(true);
            }, (error) =>
            {
                OnPlayFabError(error);
                if(resultCallback != null) resultCallback(false);
            });

            #else
            LoginWithCustomIDRequest request = new LoginWithCustomIDRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                CreateAccount = true,
                CustomId = DBManager.GetDeviceId(),
                InfoRequestParameters = accountParams
            };

            PlayFabClientAPI.LoginWithCustomID(request, (result) =>
            {
                OnLoggedIn(result);
                if(resultCallback != null) resultCallback(true);
            }, (error) =>
            {
                OnPlayFabError(error);
                if(resultCallback != null) resultCallback(false);
            });
            #endif
        }


        #if UNITY_FACEBOOK
        private void FacebookInitialized()
        {
            if (FB.IsInitialized)
            {
                Debug.LogError("Facebook SDK Initialized.");
                FB.ActivateApp();
            }
            else
                Debug.LogError("Failed to Initialize the Facebook SDK");

            if (FB.IsLoggedIn)
            {
                facebookAccess = AccessToken.CurrentAccessToken.TokenString;
                Debug.LogError("User was already logged in Facebook");
                LoginWithFacebook();
            }
            else
            {
                var perms = new List<string>() { "public_profile", "email", "user_friends" };
                FB.LogInWithReadPermissions(perms, FacebookAuth);
            }
        }


        private void FacebookAuth(ILoginResult result)
        {
            if (FB.IsLoggedIn)
            {
                facebookAccess = result.AccessToken.TokenString;
            }
            else
            {
                Debug.Log("User cancelled Facebook login");
            }
        }


        /// <summary>
        /// Login within an app on Facebook Canvas or Gameroom by using its current access token.
        /// </summary>
        public void LoginWithFacebook()
        {
            LoginWithFacebookRequest request = new LoginWithFacebookRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                CreateAccount = true,
                AccessToken = facebookAccess,
                InfoRequestParameters = instance.accountParams
            };

            PlayFabClientAPI.LoginWithFacebook(request, OnLoggedIn, OnPlayFabError);
        }
        #endif


        #if PLAYFAB_STEAM
        private void OnGetAuthSessionTicketResponse(GetAuthSessionTicketResponse_t pCallback)
        {
            byte[] tempTicket = new byte[ticketLength];
            for (int i = 0; i < tempTicket.Length; i++)
                tempTicket[i] = ticket[i];

            string hexEncodedTicket = "";
            hexEncodedTicket = System.BitConverter.ToString(tempTicket);
            hexEncodedTicket = hexEncodedTicket.Replace("-", "");
            hexTicket = hexEncodedTicket;

            LoginWithSteam();
        }
        #endif


        /// <summary>
        /// Register a new account by using the email address and password provided.
        /// </summary>
        public static void RegisterAccount(string emailAddress, string password)
        {
            RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                Email = emailAddress,
                Password = password,
                RequireBothUsernameAndEmail = false
            };

            PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisteredResult, OnLoginError);
        }


        /// <summary>
        /// Login via email by using the email address and password provided.
        /// </summary>
        public static void LoginWithEmail(string emailAddress, string password)
        {
            LoginWithEmailAddressRequest request = new LoginWithEmailAddressRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                Email = emailAddress,
                Password = password,
                InfoRequestParameters = instance.accountParams
            };

            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoggedIn, OnLoginError);
        }


        /// <summary>
        /// Request an account recovery email (new password) to the email passed in.
        /// </summary>
        public static void ForgotPassword(string emailAddress)
        {
            SendAccountRecoveryEmailRequest request = new SendAccountRecoveryEmailRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                Email = emailAddress
            };

            PlayFabClientAPI.SendAccountRecoveryEmail(request, OnEmailRecovery, OnLoginError);
        }


        private static void OnEmailRecovery(SendAccountRecoveryEmailResult result)
        {
            if (loginFailedEvent != null)
                loginFailedEvent("Recovery Email sent. Please check your inbox.");
        }


        #if PLAYFAB_STEAM
        public void LoginWithSteam()
        {
            LoginWithSteamRequest request = new LoginWithSteamRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                CreateAccount = true,
                SteamTicket = hexTicket,
                InfoRequestParameters = accountParams
            };

            PlayFabClientAPI.LoginWithSteam(request, OnLoggedIn, OnLoginError);
        }
        #endif

        
        //process the login result and all user data retrieved from PlayFab, such as
        //user inventory with purchased products, virtual currency balances and title data
        private static void OnLoggedIn(PlayFab.ClientModels.LoginResult result)
        {
            instance.loginResult = null;
            userId = result.PlayFabId;
            bool skipPayload = result.InfoResultPayload == null ? true : false;

            if(IAPManager.isDebug)
                Debug.Log("Got PlayFabID: " + userId + (result.NewlyCreated ? " (new account)" : " (existing account)"));
            
            #if PLAYFAB_VALIDATION
                skipPayload = true;
            #endif

            if (skipPayload)
            {
                IAPManager.GetInstance().Initialize();

                if (loginSucceededEvent != null)
                    loginSucceededEvent();

                return;
            }

            instance.serverData = new JSONClass();

            List<ItemInstance> inventory = result.InfoResultPayload.UserInventory;
            if (inventory != null && inventory.Count != 0)
            {
                string itemId = null;
                for (int i = 0; i < inventory.Count; i++)
                {
                    itemId = inventory[i].ItemId;
                    IAPObject obj = IAPManager.GetIAPObject(itemId);

                    Debug.Log(itemId + " ist vom Object: " + obj);

                    if (obj != null)
                    {
                        switch (obj.type)
                        {
                            case ProductType.Consumable:
                                if (obj.editorType == IAPType.Virtual)
                                {
                                    instance.serverData[DBManager.playerKey][obj.id] = new SimpleJSON.JSONData((int)inventory[i].RemainingUses);
                                }
                                break;

                            default:
                                instance.serverData[DBManager.contentKey][obj.id].AsInt = 1;
                                break;
                        }
                    }
                }
            }

            Dictionary<string, int> virtualCurrency = result.InfoResultPayload.UserVirtualCurrency;
            if (virtualCurrency != null && virtualCurrency.Count != 0)
            {
                Dictionary<string, int> currency = DBManager.GetCurrencies();
                foreach (KeyValuePair<string, int> pair in virtualCurrency)
                {
                    //update local data in memory
                    foreach (KeyValuePair<string, int> cur in currency)
                    {
                        if (cur.Key.StartsWith(pair.Key, StringComparison.OrdinalIgnoreCase))
                        {
                            instance.serverData[DBManager.currencyKey][cur.Key].AsInt = pair.Value;
                            break;
                        }
                    }
                }
            }

            Dictionary<string, UserDataRecord> userData = result.InfoResultPayload.UserData;
            if (userData != null && userData.Count != 0)
            {
                string[] userKey = instance.accountParams.UserDataKeys.ToArray();
                for (int i = 0; i < userKey.Length; i++)
                {
					if (userData.ContainsKey(userKey[i]) && !string.IsNullOrEmpty(userData[userKey[i]].Value))
					{
						JSONNode node = JSON.Parse(userData[userKey[i]].Value);
						foreach (string groupKey in node.AsObject.Keys)
						{
                            JSONArray array = node[groupKey].AsArray;

                            //it's a simple value, not an array (usually Player data)
                            if (array == null)
                            {
                                instance.serverData[userKey[i]][groupKey] = node[groupKey].Value;
                                continue;
                            }

                            //it's an array of values (usually Selected group data)
							for (int j = 0; j < array.Count; j++)
							{
								instance.serverData[userKey[i]][groupKey][j] = array[j].Value;
							}
						}
					}
                }
            }

            /*
            //try to resolve easy conflicts automatically in case of empty data,
            //but forward more complex conflicts to the user for resolution,
            //or use pre-defined resolution scenario assigned in the inspector
            if (string.IsNullOrEmpty(DBManager.Read()))
            {
                //local data is empty: get and overwrite with server data
                GetInstance().HandleDataConflict(MergeConflictRule.UseServer);
            }
            else if (string.IsNullOrEmpty(instance.serverData.ToString()))
            {
                //server data is empty: keep local and upload to server
                GetInstance().HandleDataConflict(MergeConflictRule.UseLocal);
            }
            else if (DBManager.Read() != instance.serverData.ToString())
            {

                Debug.Log("On Local: " + DBManager.Read());
                Debug.Log("On Server: " + instance.serverData.ToString());
                Debug.LogWarning("Merge Conflict detected.");

                //conflict detected: local and server data is different
                //use pre-defined resolution rule, if any
                if (instance.mergeRule != MergeConflictRule.UserPrompt)
                    GetInstance().HandleDataConflict(instance.mergeRule);
                else if (dataConflictEvent != null)
                    dataConflictEvent();

                return;
            }
            */

            DBManager.Overwrite(instance.serverData.ToString());
            IAPManager.GetInstance().Initialize();

            if (loginSucceededEvent != null)
                loginSucceededEvent();
        }


        /*
        public void HandleDataConflict(MergeConflictRule rule)
        {
            if (rule == MergeConflictRule.UserPrompt) rule = mergeRule;
            switch(rule)
            {
                case MergeConflictRule.UseLocal:
                    break;

                case MergeConflictRule.UseServer:
                    DBManager.Overwrite(instance.serverData.ToString());
                    DBManager.GetInstance().Init();
                    break;

                case MergeConflictRule.Custom:
                    //your own conflict resolution here!
                    break;
            }

            IAPManager.GetInstance().Initialize();
        }
        */


        /// <summary>
        /// Not implemented yet.
        /// </summary>
        public static void LinkAccountNative()
        {
            //if not logged in,
            //ask for platform login
            
            #if UNITY_ANDROID
            LinkGoogleAccountRequest request = new LinkGoogleAccountRequest()
            {
                ServerAuthCode = "",
                ForceLink = true
            };
            PlayFabClientAPI.LinkGoogleAccount(request, null, AccountAlreadyLinked);
            #elif UNITY_IOS
            LinkGameCenterAccountRequest request = new LinkGameCenterAccountRequest()
            {
                GameCenterId = "",
                ForceLink = true
            };
            PlayFabClientAPI.LinkGameCenterAccount(request, null, AccountAlreadyLinked);
            #endif
        }
        
        
        /// <summary>
        /// Not implemented yet.
        /// </summary>
        public static void LinkAccountOther()
        {
            ExecuteCloudScriptRequest cloudRequest = new ExecuteCloudScriptRequest()
            {
                FunctionName = "generateLink",
                GeneratePlayStreamEvent = true
            };

            PlayFabClientAPI.ExecuteCloudScript(cloudRequest, (cloudResult) =>
            {
                JSONNode node = JSON.Parse(PlayFab.Json.PlayFabSimpleJson.SerializeObject(cloudResult.FunctionResult));
                linkSecretKey = node["secretKey"].Value;
                linkExpiredKey = node["expired"].Value;

                LinkCustomIDRequest linkRequest = new LinkCustomIDRequest()
                {
                    CustomId = userId + linkSecretKey,
                    ForceLink = false
                };

                if(string.IsNullOrEmpty(linkExpiredKey))
                {
                    Debug.Log("Linking new CustomID...");
                    PlayFabClientAPI.LinkCustomID(linkRequest, OnLinkSuccess, OnLinkError);
                    return;
                }

                Debug.Log("Should first unlink this expired key: " + linkExpiredKey);
                UnlinkCustomIDRequest unlinkRequest = new UnlinkCustomIDRequest()
                {
                    CustomId = userId + linkExpiredKey
                };

                PlayFabClientAPI.UnlinkCustomID(unlinkRequest, (unlinkResult) =>
                {
                    Debug.Log("Device ID unlinked");
                    linkExpiredKey = null;
                    PlayFabClientAPI.LinkCustomID(linkRequest, OnLinkSuccess, OnLinkError);
                }, (unlinkError) =>
                {
                    if (unlinkError.Error == PlayFabErrorCode.CustomIdNotLinked)
                    {
                        Debug.Log("The previous device ID was claimed and unlinked already, skipping.");
                        linkExpiredKey = null;
                        PlayFabClientAPI.LinkCustomID(linkRequest, OnLinkSuccess, OnLinkError);
                    }
                    else
                        OnLinkError(unlinkError);
                });
            }, OnLinkError);
        }
        
        
        private static void OnLinkSuccess(PlayFab.SharedModels.PlayFabResultCommon result)
        {
            if (linkSucceededEvent != null)
            {
                 if (result is LinkCustomIDResult)
                    linkSucceededEvent(linkSecretKey);
                else
                    linkSucceededEvent(null);
            }

            /*
            //in UI
            string key = "";
            for (int i = 0; i < (linkSecretKey.Length / 4) - 1; i++)
            {
                key = linkSecretKey.Substring(i * 4, 4) + " - ";
            }
            */

            Debug.Log("Custom Link Now Active for 3 Minutes!");
        }


        private static void AccountAlreadyLinked(PlayFabError error)
        {
            //already linked, tell the user he is already using a different account
        }
        
        
        /// <summary>
        /// Not implemented yet.
        /// </summary>
        public static void LoginAccountNative()
        {
            //prompt for platform login Google Play, Gamecenter etc
            //then call native logins

            #if UNITY_ANDROID
            LoginWithGoogleAccountRequest request = new LoginWithGoogleAccountRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                ServerAuthCode = "",
                InfoRequestParameters = instance.accountParams
            };
            PlayFabClientAPI.LoginWithGoogleAccount(request, OnLoggedIn, OnLoginError);
            #elif UNITY_IOS
            LoginWithGameCenterRequest request = new LoginWithGameCenterRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                PlayerId = "",
                InfoRequestParameters = instance.accountParams
            };
            PlayFabClientAPI.LoginWithGameCenter(request, OnLoggedIn, OnLoginError);
            #endif
        }
        
        
        /// <summary>
        /// Not implemented yet.
        /// </summary>
        public static void LoginAccountOther(string otherUser, string key)
        {
            ExecuteCloudScriptRequest cloudRequest = new ExecuteCloudScriptRequest()
            {
                FunctionName = "getLink",
                FunctionParameter = new { playerId = otherUser, secretKey = key }
            };

            PlayFabClientAPI.ExecuteCloudScript(cloudRequest, (cloudResult) =>
            {
                JSONNode node = JSON.Parse(PlayFab.Json.PlayFabSimpleJson.SerializeObject(cloudResult.FunctionResult));
                Debug.Log("Is Link valid?: " + node["validLink"].Value);
                bool validLink = false;
                bool.TryParse(node["validLink"].Value, out validLink);

                if (!validLink)
                {
                    return;
                }

                linkSecretKey = key;
                LoginWithCustomIDRequest loginRequest = new LoginWithCustomIDRequest()
                {
                    TitleId = PlayFabSettings.TitleId,
                    CreateAccount = false,
                    CustomId = otherUser + key,
                    InfoRequestParameters = instance.accountParams
                };

                PlayFabClientAPI.LoginWithCustomID(loginRequest, (loginResult) =>
                {
                    Debug.Log("Logged into other Player Account");
                    instance.loginResult = loginResult;
                    LinkDevice();
                }, OnLinkError);
            }, OnLinkError);
        }


        /// <summary>
        /// Not implemented yet.
        /// </summary>
        public static void LinkDevice(PlayFab.ClientModels.LoginResult result = null)
        {
            #if UNITY_ANDROID
            LinkAndroidDeviceIDRequest request = new LinkAndroidDeviceIDRequest()
            {
                AndroidDeviceId = DBManager.GetDeviceId(),
                ForceLink = true,
                OS = SystemInfo.operatingSystem,
                AndroidDevice = SystemInfo.deviceModel,
            };
            PlayFabClientAPI.LinkAndroidDeviceID(request, UnlinkCustomId, OnLinkError);
            #elif UNITY_IOS
            LinkIOSDeviceIDRequest request = new LinkIOSDeviceIDRequest()
            {
                    DeviceId = DBManager.GetDeviceId(),
                    ForceLink = true,
                    OS = SystemInfo.operatingSystem,
                    DeviceModel = SystemInfo.deviceModel
            };
            PlayFabClientAPI.LinkIOSDeviceID(request, UnlinkCustomId, OnLinkError);
            #else
            LinkCustomIDRequest request = new LinkCustomIDRequest()
            {
                CustomId = DBManager.GetDeviceId(),
                ForceLink = true
            };
            PlayFabClientAPI.LinkCustomID(request, UnlinkCustomId, OnLinkError);
            #endif
        }


        /// <summary>
        /// Not implemented yet.
        /// </summary>
        public static void UnlinkCustomId(PlayFab.SharedModels.PlayFabResultCommon result = null)
        {
            if (string.IsNullOrEmpty(linkSecretKey))
                return;

            string user = userId;
            if (instance.loginResult != null)
                user = instance.loginResult.PlayFabId;

            UnlinkCustomIDRequest unlinkRequest = new UnlinkCustomIDRequest()
            {
                CustomId = user + linkSecretKey
            };

            PlayFabClientAPI.UnlinkCustomID(unlinkRequest, (unlinkResult) =>
            {
                Debug.Log("Last: Unlinked Id, logging in with new eventually...");
                if(result != null) OnLoggedIn(instance.loginResult);
            }, (unlinkError) =>
            {
                if (unlinkError.Error != PlayFabErrorCode.CustomIdNotLinked)
                    OnLinkError(unlinkError);
            });
        }
     

        //called on successful registration of a new account, directly log in with it
        private static void OnRegisteredResult(RegisterPlayFabUserResult result)
        {
            PlayFab.ClientModels.LoginResult loginResult = new PlayFab.ClientModels.LoginResult();
            loginResult.PlayFabId = result.PlayFabId;
            loginResult.NewlyCreated = true;
            OnLoggedIn(loginResult);
        }


        //called after receiving an error when trying to log in
        private static void OnLoginError(PlayFabError error)
        {
            string errorText = error.ErrorMessage;

            if (error.ErrorDetails != null && error.ErrorDetails.Count > 0)
            {
                foreach (string key in error.ErrorDetails.Keys)
                {
                    errorText += "\n" + error.ErrorDetails[key][0];
                }
            }

            if (loginFailedEvent != null)
                loginFailedEvent(errorText);
        }


        private static void OnLinkError(PlayFabError error)
        {
            Debug.Log(error.ErrorMessage);

            if (linkFailedEvent != null)
                linkFailedEvent();
        }


        private static void OnPlayFabSyncError(PlayFabError error)
        {
            //Debug.Log("Invoking in 10... " + error.CustomData as string);
            GetInstance().Invoke(error.CustomData as string, 10);
        }


        //called after receiving an error for any request
        //(except login methods since they have their own error callback)
        private static void OnPlayFabError(PlayFabError error)
        {
            if (!IAPManager.isDebug) return;

            Debug.Log("Error: " + (int)error.Error + ", " + error.ErrorMessage);
            if (error.ErrorDetails == null || error.ErrorDetails.Count == 0) return;

            foreach (string key in error.ErrorDetails.Keys)
            {
                Debug.Log(key + ": " + error.ErrorDetails[key][0]);
            }
        }
        #endif
    }


    /// <summary>
    /// Not implemented yet.
    /// </summary>
    public enum MergeConflictRule
    {
        UserPrompt = 0,
        UseLocal = 1,
        UseServer = 2,
        Custom = 3
    }
}