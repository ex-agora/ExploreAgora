/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

#if SIS_IAP
using UnityEngine.Purchasing;
#endif

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SIS
{
    /// <summary>
    /// Instantiates shop items in the scene, unlocks/locks and selects/deselects shop items based on previous purchases/selections.
    /// Can also be used in a scene with manually placed IAPItem. Initialized after database initialization.
    /// </summary>
    public class ShopManager : MonoBehaviour
    {
        //static reference to this script.
        private static ShopManager instance;
		
        /// <summary>
        /// Window for showing feedback on IAPListener events to the user.
        /// </summary>
        public GameObject errorWindow;

        /// <summary>
        /// Confirmation window for refreshing transactions. Only required when using PayPal.
        /// </summary>
        public GameObject confirmWindow;

        /// <summary>
        /// Text component of the errorWindow gameobject.
        /// </summary>
        public Text message;

        /// <summary>
        /// Store the relation between an IAP Group set in the IAP Settings Editor and its
        /// "parent" transform. This is because IAP Manager is a prefab that exists during
        /// scene changes, thus can't keep scene-specific data like transforms. 
        /// </summary>
        [HideInInspector]
        public List<ShopContainer> containers = new List<ShopContainer>();

        /// <summary>
        /// Instantiated shop items, ordered by their ID.
        /// </summary>
        public Dictionary<string, IAPItem> IAPItems = new Dictionary<string, IAPItem>();

        /// <summary>
        /// Fired when selecting a shop item.
        /// </summary>
        public static event Action<string> itemSelectedEvent;

        /// <summary>
        /// Fired when deselecting a shop item.
        /// </summary>
        public static event Action<string> itemDeselectedEvent;


        //if there is no IAP Manager in the scene, the ShopManager
        //will try to instantiate the prefab itself
        IEnumerator Start()
        {
            if (!IAPManager.GetInstance())
            {
                Debug.LogWarning("ShopManager: Could not find IAPManager prefab. Have you placed it in the first scene "
                               + "of your app and started from there? Instantiating copy...");
                GameObject obj = Instantiate(Resources.Load("IAPManager", typeof(GameObject))) as GameObject;
                //remove clone tag from its name. not necessary, but nice to have
                obj.name = obj.name.Replace("(Clone)", "");

                //trigger self-initialization manually
                yield return new WaitForEndOfFrame();
                IAPManager.GetInstance().OnSceneWasLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);

                //the containers have tried to initialize but disabled themselves already,
                //trigger initialization of UI grid reordering again
                yield return new WaitForEndOfFrame();
                for (int i = 0; i < containers.Count; i++)
                {
                    if (containers[i].parent != null && containers[i].parent.isActiveAndEnabled)
                        containers[i].parent.StartCoroutine(containers[i].parent.Start());
                }
            }
        }


        void OnDisable()
        {
            DBManager.updatedDataEvent -= Refresh;
        }


        /// <summary>
        /// Returns a static reference to this script.
        /// </summary>
        public static ShopManager GetInstance()
        {
            return instance;
        }


		/// <summary>
        /// Initializes all IAPItem in the scene and instantiates them with their correct state.
		/// Called by IAPManager.
		/// </summary>
		public void Init()
        {
			instance = this;
            IAPItems.Clear();
            DBManager.updatedDataEvent += Refresh;

            //get manually placed items in the scene
            IAPItem[] sceneItems = Resources.FindObjectsOfTypeAll(typeof(IAPItem)) as IAPItem[];
            for (int i = 0; i < sceneItems.Length; i++)
            {
                if (string.IsNullOrEmpty(sceneItems[i].productId) || IAPItems.ContainsKey(sceneItems[i].productId))
                    continue;

                #if UNITY_EDITOR
                if (UnityEditor.EditorUtility.IsPersistent(sceneItems[i].gameObject))
                    continue;
                #endif

                IAPItems.Add(sceneItems[i].productId, sceneItems[i]);
            }
                

            //get list of all shop groups from IAPManager
            List<IAPGroup> list = IAPManager.GetInstance().IAPs;
            int index = 0;

            //loop over groups
            for (int i = 0; i < list.Count; i++)
            {
                //cache current group
                IAPGroup group = list[i];
                ShopContainer container = GetContainer(group.id);

                //skip group if prefab or parent wasn't set
                if (container == null || container.prefab == null || container.parent == null)
                {
                    continue;
                }

                //loop over items
                for (int j = 0; j < group.items.Count; j++)
                {
                    //cache item
                    IAPObject obj = group.items[j];
                    //the item has already been placed in the scene manually
                    //dont instantiate it in a container then
                    if (IAPItems.ContainsKey(obj.id))
                        continue;

                    //instantiate shop item in the scene and attach it to the defined parent transform
                    GameObject newItem = (GameObject)Instantiate(container.prefab);
                    newItem.transform.SetParent(container.parent.transform, false);
                    newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    //rename item to force ordering as set in the IAP Settings editor
                    newItem.name = "IAPItem " + string.Format("{0:000}", index + j);
                    //get IAPItem component of the instantiated item
                    IAPItem item = newItem.GetComponent<IAPItem>();
                    if (item == null) continue;

                    //add IAPItem to dictionary for later lookup
                    IAPItems.Add(obj.id, item);

                    //upgrades overwrite, an IAP Item gets replaced with its current level
                    List<string> upgrades = IAPManager.GetIAPUpgrades(obj.id);
                    if (upgrades != null && upgrades.Count > 0)
                    {
                        for (int k = 0; k < upgrades.Count; k++)
                            IAPItems.Add(upgrades[k], item);

                        string currentUpgrade = IAPManager.GetNextUpgrade(obj.id);

                        if (!string.IsNullOrEmpty(currentUpgrade))
                            obj = IAPManager.GetIAPObject(currentUpgrade);
                    }

                    //initialize and set up item properties based on the associated IAPObject
                    //they could get overwritten by online data later
                    item.Init(obj);
                }

                index += group.items.Count;
            }

            //refresh all products initially
            RefreshAll();
        }


        /// <summary>
        /// Refreshes the visual representation of all shop items based on previous actions
        /// or user interaction, meaning we set them to 'purchased' or 'selected' in the GUI.
        /// You can call this manually in case PlayerData (unlock requirements) have changed.
        /// </summary>
        public static void RefreshAll()
        {
            foreach (string key in instance.IAPItems.Keys)
                instance.Refresh(key);
        }


        /// <summary>
        /// Refreshes the visual representation of a specific group in the IAP Settings editor.
        /// Same as RefreshAll(), but only for items within this specific group name.
        /// You can call this manually i.e. when manually changing product selections in a group.
        /// </summary>
        public static void RefreshGroup(string groupName)
        {
            IAPGroup group = null;
            foreach(IAPGroup g in IAPManager.GetInstance().IAPs)
            {
                if(g.name == groupName)
                {
                    group = g;
                    break;
                }
            }

            if(group == null)
            {
                if (IAPManager.isDebug) Debug.LogWarning("ShopManager RefreshGroup: groupName not found.");
                return;
            }

            for (int i = 0; i < group.items.Count; i++)
                instance.Refresh(group.items[i].id);
        }


		/// <summary>
		/// Refreshes the visual representation of a specific shop item.
        /// This is called automatically because of subscribing to the DBManager update event.
        /// It also means saving performance due to not refreshing all items every time.
		/// </summary>
		public void Refresh(string id)
        {
			//this method is based on data from the database,
			//so if we don't have a DBManager instance don't continue
			if (!DBManager.GetInstance()) return;

            IAPObject obj = IAPManager.GetIAPObject(id);
            IAPItem item = instance.IAPItems.ContainsKey(id) ? instance.IAPItems[id] : null;
            if (obj == null || item == null || item.productId != id) return;

            bool isSelected = DBManager.GetSelected(id);
            bool isPurchased = DBManager.GetPurchase(id) > 0;

			//double check that selected items are actually owned
            //if not, correct the entry by setting it to deselected
			if(isSelected && !isPurchased)
            {
				DBManager.SetDeselected(id);
                isSelected = false;
            }

            if(isPurchased)
            {
                item.Purchased(true);

                //in case the item has been selected before, but also auto-select one item per group
				//more items per group can be pre-selected manually e.g. on app launch
				if (isSelected || (item.selectButton && !item.deselectButton
				   && DBManager.GetSelectedGroup(IAPManager.GetIAPObjectGroupName(id)).Count == 0))
                {
                    item.IsSelected(true);
                }
            }
			else if (!string.IsNullOrEmpty(obj.req.entry) && DBManager.isRequirementMet(obj.req))
            {
				//check if a requirement is set up for this item,
				//then unlock if the requirement has been met
				if (IAPManager.isDebug) Debug.Log("requirement met for: " + obj.id);
				item.Unlock();
            }
        }


        #if SIS_IAP
        /// <summary>
        /// Method for overwriting shop item's properties with localized IAP data from the App Store servers. When we receive
        /// the online item list of products from the IAPManager, we loop over our products and check if 'fetch' was checked
        /// in the IAP Settings editor, then simply reinitialize the items by using the new data.
        /// </summary>
        public static void OverwriteWithFetch(Product[] products)
        {
            for (int i = 0; i < products.Length; i++)
            {
                string id = products[i].definition.id;
                IAPObject item = IAPManager.GetIAPObject(id);

                if (item == null || !item.fetch || !instance.IAPItems.ContainsKey(id) || item.editorType == IAPType.Virtual)
                    continue;

                instance.IAPItems[id].Init(products[i]);
            }
        }
        #endif


        /// <summary>
        /// Sets an item to 'selected' in the database.
        /// </summary>
        public static void SetToSelected(IAPItem item)
        {
            //check if the item allows for single or multi selection,
            //this depends on whether the item has a deselect button
            bool single = item.deselectButton ? false : true;
            //pass arguments to DBManager and invoke select event
            bool changed = DBManager.SetSelected(item.productId, single);
            if(changed && itemSelectedEvent != null)
                itemSelectedEvent(item.productId);
        }


        /// <summary>
        /// Sets an item to 'deselected' in the database.
        /// </summary>
        public static void SetToDeselected(IAPItem item)
        {
            //pass argument to DBManager and invoke deselect event
            DBManager.SetDeselected(item.productId);
            if(itemDeselectedEvent != null)
                itemDeselectedEvent(item.productId);
        }


		/// <summary>
        /// Show feedback/error window with text received through an event:
        /// This gets called in IAPListener's HandleSuccessfulPurchase method with some feedback,
        /// or automatically with the error message when a purchase failed at billing.
		/// <summary>
        public static void ShowMessage(string text)
        {
            if (!instance.errorWindow) return;

            if(instance.message) instance.message.text = text;
            instance.errorWindow.SetActive(true);
        }


        /// <summary>
        /// Shows window waiting for transaction confirmation. This gets called by PlayfabPayPalStore
        /// when waiting for the user to confirm his purchase payment with PayPal.
        /// </summary>
        public static void ShowConfirmation()
        {
            if (!instance.confirmWindow) return;

            instance.confirmWindow.SetActive(true);
        }


        /// <summary>
        /// Returns instantiated IAPItem shop item reference.
        /// </summary>
        public static IAPItem GetIAPItem(string id)
        {
            if (instance.IAPItems.ContainsKey(id))
                return instance.IAPItems[id];
            else
                return null;
        }


        /// <summary>
        /// Returns container for a specific group id.
        /// </summary>
        public ShopContainer GetContainer(string id)
        {
            for (int i = 0; i < containers.Count; i++)
            {
                if (containers[i].id.Equals(id))
                    return containers[i];
            }
            return null;
        }
    }


    /// <summary>
    /// Correlation between IAP group and scene-specific properties.
    /// </summary>
    [System.Serializable]
    public class ShopContainer
    {
        /// <summary>
        /// Id mapped to a group id in IAP Settings editor.
        /// </summary>
        public string id;

        /// <summary>
        /// Prefab used for instantiating items in this group.
        /// </summary>
        public GameObject prefab;

        /// <summary>
        /// Parent container for instantiating items as children.
        /// </summary>
        public IAPContainer parent;
    }
}