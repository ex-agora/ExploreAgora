/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

#if PLAYFAB_PAYPAL || PLAYFAB_VALIDATION
#define PLAYFAB
#endif
#if SIS_IAP
using UnityEngine.Purchasing;
#endif
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

#pragma warning disable 0162, 0414
namespace SIS
{
    /// <summary>
    /// Unity IAP cross-platform wrapper for real money purchases, as well as for virtual ingame purchases (for virtual currency).
    /// Initializes the Unity IAP billing system, handles different store interfaces and integrates their callbacks respectively.
    /// </summary>
    #if SIS_IAP
	public class IAPManager : MonoBehaviour, IStoreListener
    #else
    public class IAPManager : MonoBehaviour
    #endif
    {
        /// <summary>
        /// Toggle that defines whether the IAPManager should initialize itself on first load.
        /// </summary>
        public bool autoInitialize = true;

        /// <summary>
        /// Debug messages are enabled in Development build automatically.
        /// </summary>
        public static bool isDebug = false;

        /// <summary>
        /// Keys required for some store initialization processes.
        /// </summary>
        public StoreKeys storeKeys = new StoreKeys();

        /// <summary>
        /// In app products, set in the IAP Settings editor
        /// </summary>
        [HideInInspector]
        public List<IAPGroup> IAPs = new List<IAPGroup>();

        /// <summary>
        /// list of virtual currency,
        /// set in the IAP Settings editor
        /// </summary>
        [HideInInspector]
        public List<IAPCurrency> currencies = new List<IAPCurrency>();

        /// <summary>
        /// dictionary of product ids,
        /// mapped to the corresponding IAPObject for quick lookup
        /// </summary>
        public Dictionary<string, IAPObject> IAPObjects = new Dictionary<string, IAPObject>();

        /// <summary>
        /// fired when a purchase succeeds, delivering its product id
        /// </summary>
        public static event Action<string> purchaseSucceededEvent;

        /// <summary>
        /// fired when a purchase fails, delivering its product id
        /// </summary>
        public static event Action<string> purchaseFailedEvent;

        //disable platform specific warnings, because Unity throws them
        //for unused variables however they are used in this context
        #if SIS_IAP
        public static IStoreController controller;
		public static IExtensionProvider extensions;
        private static ConfigurationBuilder builder;
        private float initializeTime = -1f;
        private bool isRestoringTransactions = false;
        #endif

        //static reference to this script
        private static IAPManager instance;

        //array of real money IAP ids
        private string[] realIDs = null;

        //client/server IAP receipt verificator
        private ReceiptValidator validator;

        //reference to the PlayFabManager, if attached
        private PlayfabManager playfabManager;


        //initialize IAPs, billing systems and database,
        //as well as shop components in this order
        void Awake()
        {
            //make sure we keep one instance of this script in the game
            if (instance)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(this);
            isDebug = Debug.isDebugBuild;
            
            //set static reference
            instance = this;
            validator = GetComponent<ReceiptValidator>();
            GetComponent<DBManager>().Init();
            InitIds();
            
            #if PLAYFAB
            playfabManager = null;
            GameObject playfabGO = GameObject.Find("PlayfabManager");
            if (!playfabGO)
            {
                Debug.LogWarning("IAPManager: Playfab is enabled, but could not find PlayfabManager prefab. Have you placed it in the first scene "
                               + "of your app and started from there? Instantiating temporary copy...");
                playfabGO = Instantiate(Resources.Load("PlayfabManager", typeof(GameObject))) as GameObject;
                //remove clone tag from its name. not necessary, but nice to have
                playfabGO.name = playfabGO.name.Replace("(Clone)", "");
            }

            playfabManager = playfabGO.GetComponent<PlayfabManager>();
            if(validator == null)
                validator = gameObject.AddComponent<ReceiptValidatorService>();
            #endif

            //do not self-initialize when using PlayFab services (full):
            //wait for its login callback for querying store later
            #if PLAYFAB && !PLAYFAB_VALIDATION
                return;
            #endif

            if(autoInitialize)
                Initialize();
        }


        /// <summary>
        /// Initializes the IAP service with a platform-dependant billing connection.
        /// </summary>
        public void Initialize()
        {
            #if SIS_IAP
            //initialized already
            if(controller != null)
                return;
            #endif

            //let the ShopManager instantiate items with local data
            SceneManager.sceneLoaded += OnSceneWasLoaded;
            InitIds(); //done earlier already, here to update again

            //do not self-initialize when using PlayFab services (full)
            //wait for its login callback for querying store later
            #if PLAYFAB && !PLAYFAB_VALIDATION
                GetComponent<DBManager>().memoryOnly = true;
                if(isDebug)
                    Debug.Log("PlayFab (online mode) is enabled: IAP data will not be saved on devices.");
            #endif

            #if SIS_IAP
            //create Unity IAP builder
            #if PLAYFAB_STEAM || STEAM_IAP
                builder = ConfigurationBuilder.Instance(new SISPurchasingModule());
            #else
                builder = ConfigurationBuilder.Instance(new SISPurchasingModule(), StandardPurchasingModule.Instance());
 
                builder.Configure<IGooglePlayConfiguration>().SetPublicKey(storeKeys.googleKey);
                //builder.Configure<IMoolahConfiguration>().appKey = storeKeys.moolahKey;
                //builder.Configure<IMoolahConfiguration>().hashKey = storeKeys.moolahHash;

                if (isDebug)
                {
                    builder.Configure<IMicrosoftConfiguration>().useMockBillingSystem = true;
                    // Write out our Amazon Sandbox JSON file.
                    // This has no effect when the Amazon billing service is not in use.
                    builder.Configure<IAmazonConfiguration>().WriteSandboxJSON(builder.products);
                    // Enable "developer mode" for purchases, not requiring real-world money
                    builder.Configure<ISamsungAppsConfiguration>().SetMode(SamsungAppsMode.AlwaysSucceed);
                    //builder.Configure<IMoolahConfiguration>().SetMode(CloudMoolahMode.AlwaysSucceed);
                }
            #endif
            
            RequestProductData(builder);
            //now we're ready to initialize Unity IAP
            UnityPurchasing.Initialize(this, builder);
            #endif

            //we use PlayFab (full) on a platform that does not utilize the PlayfabStore class,
            //yet we still want to request PlayFab's catalog items for remote overwrites
            #if PLAYFAB && !PLAYFAB_PAYPAL && !PLAYFAB_VALIDATION
            new PlayfabStore().RetrieveProducts(null);
            #endif
        }


        /// <summary>
        /// Detect and initiate the ShopManager initialization on scene changes.
        /// </summary>
        public void OnSceneWasLoaded(Scene scene, LoadSceneMode m)
        {
            if (instance != this)
                return;

            ShopManager shop = null;
            GameObject shopGO = GameObject.Find("ShopManager");
            if (shopGO) shop = shopGO.GetComponent<ShopManager>();
            if (shop)
            {
                shop.Init();
                #if SIS_IAP && (!UNITY_EDITOR || PLAYFAB)
                    if(controller != null)
                        ShopManager.OverwriteWithFetch(controller.products.all);
                #endif
            }
        }


        /// <summary>
        /// Returns a static reference to this script.
        /// </summary>
        public static IAPManager GetInstance()
        {
            return instance;
        }


        //initialize IAP ids:
        //populate IAP dictionary and arrays with product ids
        private void InitIds()
        {
            //create a list only for real money purchases
            List<string> ids = new List<string>();

            if (IAPs.Count == 0)
                Debug.LogError("Initializing IAPManager, but IAP List is empty. Did you set up IAPs in the IAP Settings?");

            //loop over all groups
            for (int i = 0; i < IAPs.Count; i++)
            {
                //cache current group
                IAPGroup group = IAPs[i];
                //loop over items in this group
                for (int j = 0; j < group.items.Count; j++)
                {
                    //cache item
                    IAPObject obj = group.items[j];

                    if (string.IsNullOrEmpty(obj.id))
                    {
                        Debug.LogError("Found IAP Object in IAP Settings without an identifier. Skipping product.");
                        continue;
                    }
                    else if(!IAPObjects.ContainsKey(obj.id))
                    {
                        //add this IAPObject to the dictionary of id <> IAPObject
                        IAPObjects.Add(obj.id, obj);
                    }

                    //if it's an IAP for real money, add it to the id list
                    //on PlayFab we also add virtual products for cloud save, except in validation mode
                    #if PLAYFAB
                    if (obj.editorType == IAPType.Virtual)
                    {
                        #if PLAYFAB_VALIDATION
                            continue;
                        #endif

                        //removing free products because PlayFab does not keep them in their store
                        if (obj.virtualPrice.Where(x => x.amount > 0).FirstOrDefault() == null)
                            continue;
                    }
                    #else
                    if (obj.editorType == IAPType.Virtual) continue;
                    #endif
                    
                    ids.Add(obj.id);
                }
            }

            //don't add the restore button to the list of online purchases
            if (ids.Contains("restore")) ids.Remove("restore");

            //convert and store list of real money IAP ids as string array 
            realIDs = ids.ToArray();
        }


        #if SIS_IAP
        //construct IAP product data with their App Store identifiers
        private void RequestProductData(ConfigurationBuilder builder)
        {
            for (int i = 0; i < realIDs.Length; i++)
			{
                IAPObject obj = GetIAPObject(realIDs[i]);
				builder.AddProduct(obj.id, obj.type, obj.GetIDs());
			}
        }


        /// <summary>
        ///  Initialization callback of Unity IAP. Optionally: verify old purchases online.
        ///  Once we've received the product list, we overwrite the existing shop item values with this online data.
        /// </summary>
        public void OnInitialized(IStoreController ctrl, IExtensionProvider ext)
        {
            initializeTime = Time.unscaledTime;
            controller = ctrl;
            extensions = ext;

            if (validator && validator.shouldValidate(VerificationType.onStart))
                validator.Validate();

            if (ShopManager.GetInstance())
                ShopManager.OverwriteWithFetch(controller.products.all);
        }
		#endif


        /*
        public void CloudMoolahButton()
        {
            extensions.GetExtension<IMoolahExtension>().FastRegister(DBManager.GetDeviceId(), (string moolahName) =>
            {
                //register succeeded
                extensions.GetExtension<IMoolahExtension>().Login(moolahName, DBManager.GetDeviceId(), (LoginResultState loginResult, string error) =>
                {
                    //login succeeded?
                    Debug.Log("Moolah Login State: " + loginResult + ", " + error);
                });
            }, (FastRegisterError registerResult, string error) =>
            {
                //register failed
                Debug.Log("Moolah Register Error: " + registerResult + ", " + error);
            });
        }
        */

			
        /// <summary>
        /// Purchase product based on its product id. If the productId matches "restore", we restore transactions instead.
        /// Our delegates then fire the appropriate succeeded/fail/restore event.
        /// </summary>
        public static void PurchaseProduct(string productId)
        {
            #if SIS_IAP
            if (productId == "restore")
            {
                RestoreTransactions();
                return;
            }
            #endif

            IAPObject obj = GetIAPObject(productId);
			if(obj == null) 
			{
			    if(isDebug) Debug.LogError("Product " + productId + " not found in IAP Settings.");
				return;
		    }

            //distinguish between virtual and real products
            if (obj.editorType == IAPType.Virtual)
            {
                PurchaseProduct(obj);
                return;
            }

            #if SIS_IAP
            if(controller == null)
            {
                if(ShopManager.GetInstance())
                {
                    ShopManager.ShowMessage("Billing is not available.");
                }
                Debug.LogError("Unity IAP is not initialized correctly! Please check your billing settings.");
                return;
            }

            controller.InitiatePurchase(controller.products.WithID(productId));
            #endif
        }
		
		
		/// <summary>
        /// Overload for purchasing virtual product based on its product identifier.
        /// </summary>
        public static void PurchaseProduct(IAPObject obj)
        {	
			string productId = obj.id;
			//product is set to already owned, this should not happen
			if(obj.type != ProductType.Consumable && DBManager.GetPurchase(productId) > 0)
			{
				OnPurchaseFailed("Product already owned.");
                return;
			}

            #if PLAYFAB && !PLAYFAB_VALIDATION
                new PlayfabStore().Purchase(obj);
                return;
            #endif
		
            //check whether the player has enough funds
            bool didSucceed = DBManager.VerifyVirtualPurchase(obj);
            if (isDebug) Debug.Log("Purchasing virtual product " + productId + ", result: " + didSucceed);
            //on success, non-consumables are saved to the database. This automatically
			//saves the new substracted fund value, then and fire the succeeded event
            if (didSucceed)
            {
                if (obj.type == ProductType.Consumable) DBManager.IncreasePlayerData(productId, obj.usageCount);
                else DBManager.SetPurchase(obj.id);
                purchaseSucceededEvent(productId);
            }
            else
                OnPurchaseFailed("Insufficient funds.");
        }

		
        #if SIS_IAP
		/// <summary>
		/// This will be called when a purchase completes.
		/// Optional: verify new product receipt.
		/// </summary>
		public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
		{
            if (IAPManager.GetIAPObject(e.purchasedProduct.definition.id) == null)
            {
                return PurchaseProcessingResult.Complete;
            }

            /*
            //IN DEVELOPMENT until CloudMoolah comes out of beta
            //CloudMoolah payment request
            if(Application.platform == RuntimePlatform.Android && StandardPurchasingModule.Instance().androidStore == AndroidStore.CloudMoolah)
            {
                extensions.GetExtension<IMoolahExtension>().RequestPayOut(e.purchasedProduct.transactionID, (string transactionID, RequestPayOutState state, string message) =>
                {
                    Debug.Log("RequestPayOut callback: " + transactionID + ", " + state + ", " + message);

                    if (state == RequestPayOutState.RequestPayOutSucceed)
                    {
                        controller.ConfirmPendingPurchase(e.purchasedProduct);
                        PurchaseVerified(e.purchasedProduct.definition.id);
                    }
                });

                return PurchaseProcessingResult.Pending;
            }
            */

            #if UNITY_IOS || UNITY_TVOS
            if(validator && validator.shouldValidate(VerificationType.onStart))
            {
                DBManager.SaveReceipt("Receipt", e.purchasedProduct.receipt);
            }
            #endif

            //when auto-restoring transactions on first app launch, the time between OnInitialized and
            //this method will be very short. So check that and do not validate for restored products
            //also do not validate when using PlayFab and restoring transactions on Apple as there is
            //currently a Unity IAP bug that causes PlayFab to decline any restore transactions
            if ((Time.unscaledTime - initializeTime) > 0.1f && !isRestoringTransactions 
                && validator && validator.shouldValidate(VerificationType.onPurchase))
            {
                validator.Validate(e.purchasedProduct);
                if (!(validator is ReceiptValidatorClient))
                    return PurchaseProcessingResult.Pending;
            }
            else
            {
                PurchaseVerified(e.purchasedProduct.definition.id);
            }
                
            // Indicate we have handled this purchase, we will not be informed of it again
            return PurchaseProcessingResult.Complete;
		}
        #endif


        /// <summary>
        /// Sets a product to purchased after successful verification (or without).
        /// This alters the database entry for non-consumable or products with usage as well.
        /// </summary>
        public void PurchaseVerified(string id)
        {
            if (!IAPObjects.ContainsKey(id))
                id = GetIAPIdentifier(id);
            if (!IAPObjects.ContainsKey(id)) return;
            IAPObject obj = IAPObjects[id];

            switch (obj.editorType)
            {
                case IAPType.Currency:
                    foreach(IAPCurrency cur in obj.virtualPrice)
                    {
                        DBManager.IncreaseFunds(cur.name, cur.amount);
                    }
                    break;

                default:
                    //for consumables, add the defined usage count to tracking in player data
                    //on non-consumables, don't continue if the product is already purchased,
                    //for example if we just want to verify an existing product again
                    switch(obj.type)
                    {
                        case ProductType.Consumable:
                            if(obj.usageCount > 0)
                            {
                                DBManager.IncreasePlayerData(id, obj.usageCount);

                                //update server player data
                                #if PLAYFAB && !PLAYFAB_VALIDATION
                                    PlayfabManager.SetPlayerData();
                                #endif
                            }
                            break;

                        default:
                            if (DBManager.GetPurchase(id) > 0)
                                return;
                            
                            DBManager.IncreasePurchase(id, 1);
                            break;
                    }
                    break;
            }

            purchaseSucceededEvent(id);
        }

        #if SIS_IAP
        /// <summary>
        /// Restore already purchased user's transactions for non consumable IAPs.
        /// </summary>
        public static void RestoreTransactions()
        {
            #if UNITY_IOS
                #if PLAYFAB
                    instance.isRestoringTransactions = true;
                #endif
				extensions.GetExtension<IAppleExtensions>().RestoreTransactions(OnTransactionsRestored);
            #elif UNITY_ANDROID
                switch(StandardPurchasingModule.Instance().appStore)
                {
                    case AppStore.SamsungApps:
                        extensions.GetExtension<ISamsungAppsExtensions>().RestoreTransactions(OnTransactionsRestored);
                        break;
                
                    /*
                    case AndroidStore.CloudMoolah:
                        extensions.GetExtension<IMoolahExtension>().RestoreTransactionID((RestoreTransactionIDState state) =>
                        {
                            OnTransactionsRestored(state != RestoreTransactionIDState.NotKnown && state != RestoreTransactionIDState.RestoreFailed);
                        });
                        break;
                    */
                }
            #endif
        }


        /// <summary>
        /// Callback invoked after initiating a restore attempt.
		/// </summary>
        public static void OnTransactionsRestored(bool success)
        {
            instance.isRestoringTransactions = false;

			if(!success)
			{
				string error = "Restore failed.";
                if (isDebug) Debug.Log("IAPManager reports: " + error);
    			purchaseFailedEvent(error);
				return;
			}

            #if PLAYFAB && !PLAYFAB_VALIDATION
                List<string> restoreProducts = DBManager.GetAllPurchased(true);
                restoreProducts.RemoveAll(p => controller.products.WithID(p) == null);
                GetInstance().StartCoroutine(PlayfabManager.SetPurchase(restoreProducts));
            #endif
			
            purchaseSucceededEvent("restore");
        }


        /// <summary>
        /// Callback of Unity IAP returning why the store could not be initialized to begin with.
        /// </summary>
        public void OnInitializeFailed(InitializationFailureReason error)
		{
			switch (error)
			{
				case InitializationFailureReason.AppNotKnown:
					Debug.LogError("Is your App correctly uploaded on the relevant publisher console?");
					break;
				case InitializationFailureReason.PurchasingUnavailable:
					// Ask the user if billing is disabled in device settings.
					Debug.LogWarning("Billing disabled!");
					break;
				case InitializationFailureReason.NoProductsAvailable:
					// Developer configuration error; check product metadata.
					Debug.LogWarning("No products available for purchase!");
					break;
			}
		}

		
		/// <summary>
		/// This will be called when an attempted purchase fails.
		/// </summary>
		public void OnPurchaseFailed(Product item, PurchaseFailureReason reason)
		{
            if (isDebug) Debug.Log("IAPManager reports: PurchaseFailed. Error: " + reason);
            purchaseFailedEvent(reason.ToString());
		}
		#endif

		
		/// <summary>
		/// Overload for failed purchases with string error message.
		/// </summary>
        public static void OnPurchaseFailed(string error)
        {
            if (isDebug) Debug.Log("IAPManager reports: PurchaseFailed. Error: " + error);
            purchaseFailedEvent(error);
        }

	
		/// <summary>
        /// Returns a list of all upgrade ids associated to a product.
        /// </summary>
        public static List<string> GetIAPUpgrades(string productId)
        {
            List<string> list = new List<string>();
            IAPObject obj = GetIAPObject(productId);

            if (obj == null)
            {
                if (isDebug)
                    Debug.LogError("Product " + productId + " not found in IAP Settings. Make sure "
                                   + "to remove your app from the device before deploying it again!");
            }
            else
            {
                while (obj != null && !string.IsNullOrEmpty(obj.req.nextId))
                {
                    list.Add(obj.req.nextId);
                    obj = GetIAPObject(obj.req.nextId);
                }
            }
           
            return list;
        }


        /// <summary>
        /// Returns the last purchased upgrade id of a product,
        /// or the main product itself if it hasn't been purchased yet.
        /// </summary>
        public static string GetCurrentUpgrade(string productId)
        {
            if (DBManager.GetPurchase(productId) == 0)
                return productId;

            string id = productId;
            List<string> upgrades = GetIAPUpgrades(productId);

            for (int i = upgrades.Count - 1; i >= 0; i--)
            {
                if (DBManager.GetPurchase(upgrades[i]) > 0)
                {
                    id = upgrades[i];
                    break;
                }
            }

            return id;
        }


        /// <summary>
        /// Returns the next unpurchased upgrade id of a product.
        /// </summary>
        public static string GetNextUpgrade(string productId)
        {
            string id = GetCurrentUpgrade(productId);
            IAPObject obj = GetIAPObject(id);

            if (DBManager.GetPurchase(id) == 0 || obj == null || string.IsNullOrEmpty(obj.req.nextId)) return id;
            else return obj.req.nextId;
        }
		

        /// <summary>
        /// Returns the global identifier of an in-app product, specified in the IAP Settings editor.
        /// </summary>
        public static string GetIAPIdentifier(string storeId)
        {
            #if SIS_IAP
            if(controller != null && controller.products != null)
            {
                Product p = controller.products.WithStoreSpecificID(storeId);
                if (p != null)
                    return p.definition.id;
            }
            #endif

            //fallback in case Unity IAP has not been initialized yet
            foreach (IAPObject obj in instance.IAPObjects.Values)
            {
                if (obj.editorType == IAPType.Virtual)
                    continue;

                if (obj.storeIDs.Any(x => x.id == storeId))
                    return obj.id;
            }

            return storeId;
        }


        /// <summary>
        /// Returns the list of currencies defined in the IAP Settings editor.
        /// </summary>
        public static List<IAPCurrency> GetCurrencies()
        {
            return instance.currencies;
        }
        
        
        #if SIS_IAP
        /// <summary>
        /// Returns the list of products used when initializing Unity IAP.
        /// </summary>
        public static ProductDefinition[] GetProductDefinitions()
        {
            if (builder == null || builder.products == null)
                return new ProductDefinition[0];
            else
                return builder.products.ToArray();
        }
        #endif


        /// <summary>
        /// Returns a string array of all IAP ids.
        /// Used by DBManager.
        /// </summary>
        public static string[] GetIAPKeys()
        {
            string[] ids = new string[instance.IAPObjects.Count];
            instance.IAPObjects.Keys.CopyTo(ids, 0);
            return ids;
        }
		
		
		/// <summary>
        /// Returns a string array of all real money IAP ids only.
        /// </summary>
        public static string[] GetRealKeys()
        {
            return instance.realIDs;
        }


        /// <summary>
        /// Returns the IAPObject with a specific id.
        /// </summary>
        public static IAPObject GetIAPObject(string id)
        {
            if (!instance || !instance.IAPObjects.ContainsKey(id))
                return null;
            return instance.IAPObjects[id];
        }


        /// <summary>
        /// Returns the group name of a specific product id.
        /// Used by DBManager.
        /// <summary>
        public static string GetIAPObjectGroupName(string id)
        {
            if (instance.IAPObjects.ContainsKey(id))
            {
                IAPObject obj = GetIAPObject(id);
                //loop over groups to find the product id,
                //then return the name of the group
                for (int i = 0; i < instance.IAPs.Count; i++)
                    if (instance.IAPs[i].items.Contains(obj))
                        return instance.IAPs[i].name;
            }
			
            //if the corresponding group has not been found
            return null;
        }
    }


    /// <summary>
    /// Supported billing stores.
    /// </summary>
    public enum IAPPlatform
    {
        None = -1,
        GooglePlay = 0,
		AppleAppStore = 1,
		MacAppStore = 2,
		WinRT = 3,
        AmazonApps = 4,
        SamsungApps = 5,
        FacebookStore = 7,
        OculusStore = 8,
        SteamStore = 9
        //MoolahAppStore = 
    }


    /// <summary>
    /// Store-specific initialization keys.
    /// </summary>
    [System.Serializable]
    public class StoreKeys
    {
        /// <summary>
        /// Google Play App developer license key.
        /// </summary>
        public string googleKey;

        //public string moolahKey;
        //public string moolahHash;
    }


    /// <summary>
    /// IAP Settings editor group properties. Each group holds a list of IAPObject.
    /// </summary>
    [System.Serializable]
    public class IAPGroup
    {
        /// <summary>
        /// The unique group id for identifying mappings to an IAPContainer in the scene.
        /// </summary>
        public string id;

        /// <summary>
        /// The unique name of the group.
        /// </summary>
        public string name;

        /// <summary>
        /// The list of IAPObject in this group.
        /// </summary>
        public List<IAPObject> items = new List<IAPObject>();

        /// <summary>
        /// Bool for collapsing/expanding display in editor.
        /// </summary>
        public bool foldout = true;
    }


    /// <summary>
    /// IAP object properties. This is a meta-class for an IAP item.
    /// </summary>
    [System.Serializable]
    public class IAPObject
    {
        public string id;
        public List<StoreID> storeIDs = new List<StoreID>();
        public ProductType type = ProductType.Consumable;
        public string title;
        public string description;
        public string realPrice;
        public bool fetch = false;
        
        public Sprite icon;
        public int usageCount = 1;
        public List<IAPCurrency> virtualPrice = new List<IAPCurrency>();
        public IAPRequirement req = new IAPRequirement();

        public IAPType editorType = IAPType.Default;
        public bool platformFoldout = false;


        #if SIS_IAP
        public IDs GetIDs()
        {
			IDs productIDs = new IDs();
            int count = storeIDs.Count;

			for(int i = count - 1; i >= 0; i--)
			{
                IAPPlatform platform = (IAPPlatform)System.Enum.Parse(typeof(IAPPlatform), storeIDs[i].store);
                if (platform == IAPPlatform.None || string.IsNullOrEmpty(storeIDs[i].id))
                {
                    storeIDs.RemoveAt(i);
                    continue;
                }

				productIDs.Add(storeIDs[i].id, storeIDs[i].store);
			}

            if (productIDs.Count() > 0) return productIDs;
            else return null;
        }
        #endif
    }


    /*
    /// <summary>
    /// ...
    /// </summary>
    [System.Serializable]
    public class VirtualPrice
    {
        public IAPCurrency currency = new IAPCurrency();
        public int amount;
    }
    */


    #if !SIS_IAP
    public enum ProductType
    {
        Consumable = 0,
        NonConsumable = 1
    }


    public class StoreID
    {
        public string id;
        public string store;

        public StoreID(string id, string store) {}
    }


    public class Product {}
    #endif

    
    /// <summary>
    /// Type of in-app purchase triggering different workflows.
    /// </summary>
    public enum IAPType
    {
        Default,
        Currency,
        Virtual
    }
    

    /// <summary>
    /// IAP currency, defined in the IAP Setting editor.
    /// </summary>
    [System.Serializable]
    public class IAPCurrency
    {
        /// <summary>
        /// Currency name.
        /// </summary>
        public string name = "";

        /// <summary>
        /// Default starting value for new players.
        /// </summary>
        public int amount = 0;
    }


    /// <summary>
    /// IAP unlock requirement, stored in the database.
    /// </summary>
    [System.Serializable]
    public class IAPRequirement
    {
		/// <summary>
		/// Database key name for the target value.
		/// </summary>
        public string entry;
		
		/// <summary>
		/// Value to reach for unlocking this requirement.
		/// </summary>
        public int target;
		
		/// <summary>
		/// Optional label text that describes the requirement.
		/// </summary>
        public string labelText;
		
		/// <summary>
		///	Product identifier for the following upgrade.
		/// </summary>
        public string nextId;
    }
}