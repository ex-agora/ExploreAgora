/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

#if SIS_IAP
using UnityEngine.Purchasing;
#endif

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SIS
{
    /// <summary>
    /// IAP Settings editor.
    /// The one-stop solution for managing cross-platform IAP data.
    /// Found under Window > Simple IAP System > IAP Settings
    /// </summary>
    public class IAPEditor : EditorWindow
    {
        //shop reference
        [SerializeField]
        ShopManager shop;
        //manager reference
        [SerializeField]
        IAPManager script;
        //prefab object
        [SerializeField]
        private static Object IAPPrefab;
        //window reference
        private static IAPEditor iapEditor;
        //requirement wizard reference
        private static RequirementEditor reqEditor;
		//should this editor overwrite the prefab once
        [SerializeField]
        private static System.DateTime lastSave;

        //available currency names for selection
		string[] currencyNames = new string[0];
        //currently selected currency index
        int currencyIndex = -1;
        //inspector scrollbar for window
        Vector2 scrollPos;
        //inspector scrollbar for currencies
        Vector2 scrollPosCurrency;

        string[] headerNames = new string[] { "Identifier", "Icon", "Type", "Title", "Description", "Price", "Cost/Earnings", "Usage", "Fetch" };
        string[] productTypes = new string[] { "Create Product:", "In App Purchase", "Virtual Currency", "Virtual Economy" };

        //possible criteria for ordering product ids
        //changing the selected order type will cause a re-order
        private enum OrderType
        {
            none,
            priceAsc,
            priceDesc,
            titleAsc,
            titleDesc,
        }
        private OrderType orderType = OrderType.none;


        //add menu named "IAP Settings" to the window menu
        [MenuItem("Window/Simple IAP System/IAP Settings", false, 1)]
        static void Init()
        {
            //get existing open window or if none, make a new one
            iapEditor = (IAPEditor)EditorWindow.GetWindow(typeof(IAPEditor), false, "IAP Settings");
            //automatically repaint whenever the scene has changed (for caution)
            iapEditor.autoRepaintOnSceneChange = true;
        }


        //when the window gets opened
        void OnEnable()
        {
            //reconnect reference
            if (iapEditor == null) iapEditor = this;
            //get reference to the shop and cache it
            shop = GameObject.FindObjectOfType(typeof(ShopManager)) as ShopManager;

            script = FindIAPManager();

            //could not get prefab, non-existent?
            if (script == null)
                return;

            if (shop)
                RemoveShopContainers();

            //set current currency index from -1 to first one,
            //if currencies were specified
            if (script.currencies.Count > 0)
                currencyIndex = 0;
        }


        //refresh on new scenes
        void OnHierarchyChange()
        {
            OnEnable();
            Repaint();
        }


        //locate IAP Manager prefab in the project
        public static IAPManager FindIAPManager()
        {
            GameObject obj = Resources.Load("IAPManager") as GameObject;

            #if UNITY_2019_1_OR_NEWER
            if (obj != null && PrefabUtility.GetPrefabAssetType(obj) == PrefabAssetType.Regular)
            #else
            if (obj != null && PrefabUtility.GetPrefabType(obj) == PrefabType.Prefab)
            #endif
            {
                //try to get IAP Manager component and return it
                IAPManager iap = obj.GetComponent(typeof(IAPManager)) as IAPManager;
                if (iap != null)
                {
                    IAPPrefab = obj;
                    return iap;
                }
            }

            return null;
        }


        //remove empty IAPGroup references in the scene
        void RemoveShopContainers()
        {
            //get all container objects from the Shop Manager,
            //then populate a list with all IAPGroups
            List<ShopContainer> containers = new List<ShopContainer>(shop.containers);
            List<IAPGroup> allGroups = new List<IAPGroup>(script.IAPs);

            //loop over lists and compare them
            for (int i = 0; i < containers.Count; i++)
            {
                //if we found an IAPGroup in the Shop Manager component
                //that does not exist anymore, remove it from the scene containers
                IAPGroup g = allGroups.Find(x => x.id == containers[i].id);
                if (g == null)
                {
                    shop.containers.Remove(shop.containers.Find(x => x.id == containers[i].id));
                }
            }
            containers.Clear();
        }


        //close windows and save changes on exit
        void OnDestroy()
        {
            if (reqEditor) reqEditor.Close();
            SavePrefab(false);
        }


        void OnGUI()
        {
            if (script == null)
            {
                EditorGUILayout.LabelField("Couldn't find an IAPManager prefab in the project! " +
                                 "Is it located in the Resources folder?");
                return;
            }

            //set the targeted script modified by the GUI for handling undo
            List<Object> objs = new List<Object>() { script };
            if (shop != null) objs.Add(shop);
            Object[] undo = objs.ToArray();
            Undo.RecordObjects(undo, "ChangedSettings");
            
            DrawIAP(script.IAPs);
            //track change as well as undo
            TrackChange();
        }


        //draws the in app purchase editor
        //for a specific OS
        void DrawIAP(List<IAPGroup> list)
        {
            //begin a scrolling view inside this tab, pass in current Vector2 scroll position 
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Import from JSON"))
            {
                string path = EditorUtility.OpenFolderPanel("Import IAP Settings from JSON", "", "");
                
                if (path.Length != 0)
                {
                    script.currencies = IAPEditorExporter.FromJSON(System.IO.File.ReadAllText(path + "/SimpleIAPSystem_Currencies.json"));
                    currencyIndex = script.currencies.Count > 0 ? 0 : -1;
                    script.IAPs = IAPEditorExporter.FromJSON(System.IO.File.ReadAllText(path + "/SimpleIAPSystem_IAPSettings.json"), script.currencies);
                    return;
                }
            }
            
            if (GUILayout.Button("Export to JSON"))
            {
                string path = EditorUtility.SaveFolderPanel("Save IAP Settings as JSON", "", "");
                
                if(path.Length != 0)
                {
                    System.IO.File.WriteAllBytes(path + "/SimpleIAPSystem_IAPSettings.json", System.Text.Encoding.UTF8.GetBytes(IAPEditorExporter.ToJSON(script.IAPs)));
                    System.IO.File.WriteAllBytes(path + "/SimpleIAPSystem_IAPSettings_PlayFab.json", System.Text.Encoding.UTF8.GetBytes(IAPEditorExporter.ToJSON(script.IAPs, true)));
                    System.IO.File.WriteAllBytes(path + "/SimpleIAPSystem_Currencies.json", System.Text.Encoding.UTF8.GetBytes(IAPEditorExporter.ToJSON(script.currencies)));
                }
            }
            EditorGUILayout.EndHorizontal();
            
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Virtual Currencies:", EditorStyles.boldLabel, GUILayout.Width(125), GUILayout.Height(20));

            //button for adding a new currency
            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("Add Currency"))
            {
                //switch current currency selection to the first entry
                currencyIndex = 0;
                //create new currency, then loop over items
                //and add a new currency slot for each of them 
                script.currencies.Add(new IAPCurrency());
                for (int i = 0; i < list.Count; i++)
                    for (int j = 0; j < list[i].items.Count; j++)
                        list[i].items[j].virtualPrice.Add(new IAPCurrency());
                return;
            }
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();

            //begin a scrolling view inside this tab, pass in current Vector2 scroll position 
            scrollPosCurrency = EditorGUILayout.BeginScrollView(scrollPosCurrency, GUILayout.Height(105));

            EditorGUILayout.BeginHorizontal();
            //only draw a box behind currencies if there are any
            if (script.currencies.Count > 0)
                GUI.Box(new Rect(0, 0, iapEditor.maxSize.x, 90), "");

            //loop through currencies
            for (int i = 0; i < script.currencies.Count; i++)
            {
                EditorGUILayout.BeginVertical();
                //draw currency properties,
                //such as name and amount
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Name", GUILayout.Width(44));
                script.currencies[i].name = EditorGUILayout.TextField(script.currencies[i].name, GUILayout.Width(54)).ToLower();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Default", GUILayout.Width(44));
                script.currencies[i].amount = EditorGUILayout.IntField(script.currencies[i].amount, GUILayout.Width(54));
                EditorGUILayout.EndHorizontal();

                //button for deleting a currency
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(52);
                GUI.backgroundColor = Color.gray;
                if (GUILayout.Button("X", GUILayout.Width(54)))
                {
                    //ask again before deleting the currency,
                    //as deleting it could cause angry customers!
                    //it's probably better not to remove currencies in production versions
                    if (EditorUtility.DisplayDialog("Delete Currency?",
                        "Existing users might lose their funds associated with this currency when updating.",
                        "Continue", "Abort"))
                    {
                        //loop over items and remove the
                        //associated currency slot for each of them
						for (int j = 0; j < list.Count; j++)
							for (int k = 0; k < list[j].items.Count; k++) 
							{
								if(list[j].items[k].virtualPrice != null
								   && list[j].items[k].virtualPrice.Count > i)
									list[j].items[k].virtualPrice.RemoveAt (i);
							}
                        //then remove the currency
                        script.currencies.RemoveAt(i);
                        //reposition current currency index
                        if (script.currencies.Count > 0)
                            currencyIndex = 0;
                        else
                            currencyIndex = -1;
                        break;
                    }
                }
                GUI.backgroundColor = Color.white;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            //draw currency selector, if there are any
            if (script.currencies.Count > 0)
            {
                GUILayout.Space(10);
                EditorGUILayout.BeginHorizontal();
                //get all currency names,
                //then draw a popup list for selecting the desired index
                currencyNames = GetCurrencyNames();
                EditorGUILayout.LabelField("Selected Currency:", GUILayout.Width(120));
                currencyIndex = EditorGUILayout.Popup(currencyIndex, currencyNames, GUILayout.Width(140));
                EditorGUILayout.EndHorizontal();
            }

            //ends the scrollview defined above
            EditorGUILayout.EndScrollView();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("In App Purchases:", EditorStyles.boldLabel, GUILayout.Width(125), GUILayout.Height(20));

            //draw yellow button for adding a new IAP group
            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("Add new Category"))
            {
                //create new group, give it a generic name based on
                //the current unix time and add it to the list of groups
                IAPGroup newGroup = new IAPGroup();
                string timestamp = GenerateUnixTime();
                newGroup.name = "Grp " + timestamp;
                newGroup.id = timestamp;
                list.Add(newGroup);
                return;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            GUI.backgroundColor = Color.white;

            //loop over IAP groups for this OS
            for (int i = 0; i < list.Count; i++)
            {
                //cache group
                IAPGroup group = list[i];
                //populate shop container variables if ShopManager is present
                ShopContainer shopContainer = null;
                if (shop)
                {
                    shopContainer = shop.GetContainer(group.id);
                    if (shopContainer == null)
                    {
                        shopContainer = new ShopContainer();
                        shopContainer.id = group.id;
                        shop.containers.Add(shopContainer);
                    }
                }

                EditorGUILayout.BeginHorizontal();
                GUILayout.Box("", GUILayout.Height(15), GUILayout.Width(15));
                Rect groupFoldoutRect = GUILayoutUtility.GetLastRect();
                group.foldout = EditorGUI.Foldout(groupFoldoutRect, group.foldout, "");

                GUI.backgroundColor = Color.yellow;
                int productSelection = 0;
                productSelection = EditorGUILayout.Popup(productSelection, productTypes);

                //button for adding a new IAPObject (product) to this group
                if (productSelection > 0)
                {
                    IAPObject newObj = new IAPObject();

                    switch (productSelection)
                    {
                        case 2:
                            newObj.editorType = IAPType.Currency;
                            for(int j = 0; j < script.currencies.Count; j++)
                                newObj.virtualPrice.Add(new IAPCurrency());
                            break;
                        case 3:
                            newObj.editorType = IAPType.Virtual;
                            for(int j = 0; j < script.currencies.Count; j++)
                                newObj.virtualPrice.Add(new IAPCurrency());
                            break;
                    }

                    group.items.Add(newObj);
                    break;
                }

                //draw group properties
                GUI.backgroundColor = Color.white;
                EditorGUILayout.LabelField("Group:", GUILayout.Width(45));
                group.name = EditorGUILayout.TextField(group.name, GUILayout.Width(90));
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Sort:", GUILayout.Width(35));
                orderType = (OrderType)EditorGUILayout.EnumPopup(orderType, GUILayout.Width(60));
                GUILayout.Space(10);

                if (!shop)
                {
                    GUI.contentColor = Color.yellow;
                    EditorGUILayout.LabelField("No ShopManager prefab found in this scene!", GUILayout.Width(300));
                    GUI.contentColor = Color.white;
                }
                else
                {
                    EditorGUILayout.LabelField("Prefab:", GUILayout.Width(45));
                    shopContainer.prefab = (GameObject)EditorGUILayout.ObjectField(shopContainer.prefab, typeof(GameObject), false, GUILayout.Width(100));
                    GUILayout.Space(10);
                    EditorGUILayout.LabelField("Container:", GUILayout.Width(65));
                    shopContainer.parent = (IAPContainer)EditorGUILayout.ObjectField(shopContainer.parent, typeof(IAPContainer), true, GUILayout.Width(100));
                }
                
                GUILayout.FlexibleSpace();
                //check for order type and, if it
                //isn't equal to 'none', start ordering
                if (orderType != OrderType.none)
                {
                    group.items = orderProducts(group.items);
                    break;
                }

                //button width for up & down buttons
                //these should always be at the same width, so if there's
                //only one button (e.g. if there's only one group),
                //the width must be extended
                int groupUpWidth = 22;
                int groupDownWidth = 22;
                if (i == 0) groupDownWidth = 48;
                if (i == list.Count - 1) groupUpWidth = 48;

                //draw up & down buttons for re-ordering groups
                //this will simply switch references in the list
                //hotControl and keyboardControl unsets current mouse focus
                if (i > 0 && GUILayout.Button("▲", GUILayout.Width(groupUpWidth)))
                {
                    list[i] = list[i - 1];
                    list[i - 1] = group;
                    EditorGUIUtility.hotControl = 0;
                    EditorGUIUtility.keyboardControl = 0;
                }
                if (i < list.Count - 1 && GUILayout.Button("▼", GUILayout.Width(groupDownWidth)))
                {
                    list[i] = list[i + 1];
                    list[i + 1] = group;
                    EditorGUIUtility.hotControl = 0;
                    EditorGUIUtility.keyboardControl = 0;
                }

                //button for removing a group including items
                GUI.backgroundColor = Color.gray;
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    if(shop) shop.containers.Remove(shopContainer);
                    list.RemoveAt(i);
                    break;
                }
                GUI.backgroundColor = Color.white;
                EditorGUILayout.EndHorizontal();
                if (group.foldout == false)
                    continue;

                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
				GUILayout.Space(20);

				List<Rect> headerRect = GetHeaders();
                
                //loop over items in this group
                for (int j = 0; j < group.items.Count; j++)
                {
                    //cache item reference
                    IAPObject obj = group.items[j];
                    EditorGUILayout.BeginHorizontal();

                    if (obj.storeIDs != null && obj.storeIDs.Count > 0)
                        GUI.backgroundColor = Color.yellow;

                    GUILayout.Box("", GUILayout.Height(15), GUILayout.Width(15));
                    Rect foldoutRect = GUILayoutUtility.GetLastRect();
                    GUI.backgroundColor = Color.white;

                    if (obj.editorType != IAPType.Virtual)
                    {
                        obj.platformFoldout = EditorGUI.Foldout(foldoutRect, obj.platformFoldout, "");
                    }

                    //draw IAPObject (item/product) properties
                    obj.id = EditorGUILayout.TextField(obj.id, GUILayout.MaxWidth(150));
                    if(!string.IsNullOrEmpty(obj.id)) obj.id = obj.id.Replace(" ", "");
                    obj.icon = EditorGUILayout.ObjectField(obj.icon, typeof(Sprite), false, GUILayout.MaxWidth(65)) as Sprite;
                    obj.editorType = (IAPType)EditorGUILayout.EnumPopup(obj.editorType, GUILayout.MaxWidth(70));

                    EditorGUI.BeginDisabledGroup(obj.editorType == IAPType.Currency);
                    if (obj.editorType == IAPType.Currency) obj.type = ProductType.Consumable;
                    obj.type = (ProductType)EditorGUILayout.EnumPopup(obj.type, GUILayout.MaxWidth(110));
                    EditorGUI.EndDisabledGroup();

                    obj.title = EditorGUILayout.TextField(obj.title, GUILayout.MaxWidth(180));
                    obj.description = EditorGUILayout.TextField(obj.description);

                    EditorGUI.BeginDisabledGroup(obj.editorType == IAPType.Virtual);
                    obj.realPrice = EditorGUILayout.TextField(obj.realPrice, GUILayout.MaxWidth(55));
                    EditorGUI.EndDisabledGroup();

                    if (obj.editorType == IAPType.Virtual && (int)obj.type > 1)
                    {
                        Debug.LogWarning("Subscriptions are not available for virtual products. Resetting to Consumable.");
                        obj.type = ProductType.Consumable;
                    }

                    EditorGUI.BeginDisabledGroup(obj.editorType == IAPType.Default);
                    IAPCurrency cur = null;
                    if (obj.editorType != IAPType.Default && currencyIndex >= 0 && obj.virtualPrice.Count > currencyIndex)
                        cur = obj.virtualPrice[currencyIndex];
 
                    if (cur == null) EditorGUILayout.LabelField("", GUILayout.MinWidth(75), GUILayout.MaxWidth(104));
                    else
                    {
                        cur.name = currencyNames[currencyIndex];
                        EditorGUILayout.LabelField(cur.name, GUILayout.MinWidth(35), GUILayout.MaxWidth(50));
                        cur.amount = EditorGUILayout.IntField(cur.amount, GUILayout.MinWidth(35), GUILayout.MaxWidth(50));
                    }
                    EditorGUI.EndDisabledGroup();

                    EditorGUI.BeginDisabledGroup(obj.type != ProductType.Consumable || obj.editorType == IAPType.Currency || obj.id == "restore");
                    obj.usageCount = Mathf.Clamp(EditorGUILayout.IntField(obj.usageCount, GUILayout.MaxWidth(35)), 0, int.MaxValue);
                    EditorGUI.EndDisabledGroup();
                    
                    if(obj.type != ProductType.Consumable) obj.usageCount = 1;
                    obj.fetch = EditorGUILayout.Toggle (obj.fetch, GUILayout.MaxWidth(20));

                    //button for adding a requirement to this item
                    if (!string.IsNullOrEmpty(obj.req.entry) || !string.IsNullOrEmpty(obj.req.nextId))
                        GUI.backgroundColor = Color.yellow;
                    if (GUILayout.Button("R", GUILayout.Width(20)))
                    {
                        reqEditor = (RequirementEditor)GetWindowWithRect(typeof(RequirementEditor), new Rect(0, 0, 300, 170), false, "Requirement");
                        reqEditor.obj = obj;
                    }

                    GUI.backgroundColor = Color.white;
                    //do the same here as with the group up & down buttons
                    //(see above)
                    int buttonUpWidth = 22;
                    int buttonDownWidth = 22;
                    if (j == 0) buttonDownWidth = 48;
                    if (j == group.items.Count - 1) buttonUpWidth = 48;

                    //draw up & down buttons for re-ordering items in a group
                    //this will simply switch references in the list
                    if (j > 0 && GUILayout.Button("▲", GUILayout.Width(buttonUpWidth)))
                    {
                        group.items[j] = group.items[j - 1];
                        group.items[j - 1] = obj;
                        EditorGUIUtility.hotControl = 0;
                        EditorGUIUtility.keyboardControl = 0;
                    }
                    if (j < group.items.Count - 1 && GUILayout.Button("▼", GUILayout.Width(buttonDownWidth)))
                    {
                        group.items[j] = group.items[j + 1];
                        group.items[j + 1] = obj;
                        EditorGUIUtility.hotControl = 0;
                        EditorGUIUtility.keyboardControl = 0;
                    }
					
					if(group.items.Count == 1)
                        GUILayout.Space(52);

                    //button for removing an item of the group
                    GUI.backgroundColor = Color.gray;
                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        group.items.RemoveAt(j);
                        break;
                    }
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndHorizontal();

                    //draw platform override foldout
                    if (obj.platformFoldout)
                    {
                        EditorGUILayout.LabelField("Platform Overrides");
                        if (GUILayout.Button("+", GUILayout.MaxWidth(35)))
                        {
                            obj.storeIDs.Add(new StoreID("None", ""));
                        }

                        for (int k = 0; k < obj.storeIDs.Count; k++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(40);
                            obj.storeIDs[k].store = EditorGUILayout.EnumPopup((IAPPlatform)System.Enum.Parse(typeof(IAPPlatform), obj.storeIDs[k].store), GUILayout.MaxWidth(150)).ToString();
                            obj.storeIDs[k].id = EditorGUILayout.TextField(obj.storeIDs[k].id, GUILayout.Width(150));
                            if(!string.IsNullOrEmpty(obj.storeIDs[k].id)) obj.storeIDs[k].id = obj.storeIDs[k].id.Replace(" ", "");

                            GUI.backgroundColor = Color.gray;
                            if (GUILayout.Button("X", GUILayout.MaxWidth(20)))
                            {
                                obj.storeIDs.RemoveAt(k);
                                if (obj.storeIDs.Count == 0)
                                    obj.platformFoldout = false;
                                break;
                            }
                            GUI.backgroundColor = Color.white;
                            EditorGUILayout.EndHorizontal();
                        }

                        GUILayout.Space(10);
                    }
                }

                for (int j = 0; j < headerRect.Count; j++)
                    EditorGUI.LabelField(new Rect(headerRect[j].x, headerRect[j].y - 20, 100, 20), headerNames[j]);

                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
                GUILayout.Space(30);
            }

            //ends the scrollview defined above
            EditorGUILayout.EndScrollView();
        }


		List<Rect> GetHeaders()
		{
			List<Rect> temp = new List<Rect>();
			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.TextField("", GUILayout.MaxHeight(1), GUILayout.Width(15));

			EditorGUILayout.TextField("", GUILayout.MaxHeight(1), GUILayout.MinWidth(50), GUILayout.MaxWidth(150));
			temp.Add(GUILayoutUtility.GetLastRect());

			EditorGUILayout.TextField("", GUILayout.MaxHeight(1), GUILayout.MinWidth(50), GUILayout.MaxWidth(65));
			temp.Add(GUILayoutUtility.GetLastRect());

			EditorGUILayout.TextField("", GUILayout.MaxHeight(1), GUILayout.MinWidth(105), GUILayout.MaxWidth(185));
			temp.Add(GUILayoutUtility.GetLastRect());

			EditorGUILayout.TextField("", GUILayout.MaxHeight(1), GUILayout.MinWidth(50), GUILayout.MaxWidth(180));
			temp.Add(GUILayoutUtility.GetLastRect());

            EditorGUILayout.TextField("", GUILayout.MaxHeight(1), GUILayout.MinWidth(50));
			temp.Add(GUILayoutUtility.GetLastRect());

			EditorGUILayout.TextField("", GUILayout.MaxHeight(1), GUILayout.MinWidth(50), GUILayout.MaxWidth(55));
			temp.Add(GUILayoutUtility.GetLastRect());

            EditorGUILayout.TextField("", GUILayout.MaxHeight(1), GUILayout.MinWidth(80), GUILayout.MaxWidth(105));
            temp.Add(GUILayoutUtility.GetLastRect());

            EditorGUILayout.TextField("", GUILayout.MaxHeight(1), GUILayout.MinWidth(35), GUILayout.MaxWidth(35));
            temp.Add(GUILayoutUtility.GetLastRect());

			EditorGUILayout.TextField("", GUILayout.MaxHeight(1), GUILayout.MinWidth(10), GUILayout.MaxWidth(20));
			temp.Add(GUILayoutUtility.GetLastRect());

			EditorGUILayout.LabelField("", GUILayout.MaxHeight(1), GUILayout.Width(20));

			EditorGUILayout.LabelField("", GUILayout.MaxHeight(1), GUILayout.Width(48));

			EditorGUILayout.LabelField("", GUILayout.MaxHeight(1), GUILayout.Width(20));

			EditorGUILayout.EndHorizontal();

			return temp;
		}


        //orders a list of IAPObjects (products)
        //based on the selected criterion
        List<IAPObject> orderProducts(List<IAPObject> list)
        {
            //create temporary list for sorted IAPObject entries
            //loop over current list and copy reference to the sorted list
            List<IAPObject> sortedList = new List<IAPObject>();
            for (int i = 0; i < list.Count; i++)
                sortedList.Add(list[i]);

            //regular expressions currency string conversion pattern
            //for sorting prices, we only consider numbers, commas and dots
            string pattern = "[^0-9,.]";

            //when ordering prices, first check some requirements
            //for virtual items, there must be a currency to sort on
            //for real money items, check if price values match our pattern
            //and they can actually be converted to decimals
            switch (orderType)
            {
                case OrderType.priceAsc:
                case OrderType.priceDesc:

                    //log warning if price sorting has been selected on virtual items but no currency specified  
                    if (list[0].editorType == IAPType.Virtual)
                    {
                        if (script.currencies.Count <= 0)
                        {
                            Debug.LogWarning("Cannot sort virtual IAPList: No currency specified to sort on.");
                            orderType = OrderType.none;
                            return list;
                        }
                        else
                            break;
                    }

                    foreach (IAPObject obj in list)
                    {
                        //create temporary decimal value
                        //and try parsing the string
                        decimal value;
                        if (decimal.TryParse(Regex.Replace(obj.realPrice, pattern, ""), out value))
                            continue;
                        else
                        {
                            //log warning if string couldn't be converted to decimal value
                            Debug.LogWarning("Sorting IAPList failed: " + obj.title + "'s price contains no number.");
                            orderType = OrderType.none;
                            return list;
                        }
                    }
                    break;
            }

            //start sorting - differ between order types
            //we use lambda expressions for now, but in some cases
            //the sorting methods below aren't that precise
            switch (orderType)
            {
                //ascending price:
                //for virtual items we use the active price -
                //for real money items first convert input string to decimal -
                //and compare with next entry, then take first
                case OrderType.priceAsc:
                    if (list[0].editorType == IAPType.Virtual)
                        sortedList.Sort((a, b) => a.virtualPrice[currencyIndex].amount.CompareTo(b.virtualPrice[currencyIndex].amount));
                    else
                        sortedList.Sort((a, b) => decimal.Parse(Regex.Replace(a.realPrice, pattern, ""))
                                    .CompareTo(decimal.Parse(Regex.Replace(b.realPrice, pattern, ""))));
                    break;
                //descending price:
                //for virtual items we use the active price -
                //for real money items first convert input string to decimal -
                //and compare with next entry, then take second
                case OrderType.priceDesc:
                    if (list[0].editorType == IAPType.Virtual)
                        sortedList.Sort((a, b) => -a.virtualPrice[currencyIndex].amount.CompareTo(b.virtualPrice[currencyIndex].amount));
                    else
                        sortedList.Sort((a, b) => -decimal.Parse(Regex.Replace(a.realPrice, pattern, ""))
                                     .CompareTo(decimal.Parse(Regex.Replace(b.realPrice, pattern, ""))));
                    break;
                //ascending title:
                //compare with next entry, then take first
                case OrderType.titleAsc:
                    sortedList.Sort((a, b) => a.title.CompareTo(b.title));
                    break;
                //descending title:
                //compare with next entry, then take second
                case OrderType.titleDesc:
                    sortedList.Sort((a, b) => -a.title.CompareTo(b.title));
                    break;
            }
            //reset order type
            orderType = OrderType.none;
            //return sorted list reference
            return sortedList;
        }


        //returns an array that holds all currency names
        string[] GetCurrencyNames()
        {
            //get list of currencies
            List<IAPCurrency> list = script.currencies;
            //create new array with the same size, then loop
            //over currencies and populate array with their names
            string[] curs = new string[list.Count];
            for (int i = 0; i < curs.Length; i++)
                curs[i] = list[i].name;
            //return names array
            return curs;
        }


        string GenerateUnixTime()
        {
            var epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            return (System.DateTime.UtcNow - epochStart).TotalSeconds.ToString() + Random.Range(0, 1000);
        }


        private static void SavePrefab(bool focusRequired)
        {
            if (focusRequired && (EditorWindow.focusedWindow == null || EditorWindow.focusedWindow != iapEditor))
                return;
            if (((System.DateTime.Now - lastSave).TotalSeconds < 1) || IAPPrefab == null)
                return;
            lastSave = System.DateTime.Now;

            GameObject go = PrefabUtility.InstantiatePrefab(IAPPrefab) as GameObject;
            #if UNITY_2019_1_OR_NEWER
                PrefabUtility.SaveAsPrefabAsset(go, AssetDatabase.GetAssetPath(IAPPrefab));
            #else
                PrefabUtility.ReplacePrefab(go, IAPPrefab);
            #endif
            DestroyImmediate(go);
        }


        void TrackChange()
        {
            //if we typed in other values in the editor window,
            //we need to repaint it in order to display the new values
            if (GUI.changed)
            {
                //we have to tell Unity that a value of our script has changed
                //http://unity3d.com/support/documentation/ScriptReference/EditorUtility.SetDirty.html
                if(shop) EditorUtility.SetDirty(shop);

                //repaint editor GUI window
                Repaint();
            }
        }


        //track project save state and save changes to prefab on project save
        public class IAPModificationProcessor : UnityEditor.AssetModificationProcessor
        {
            public static string[] OnWillSaveAssets(string[] paths)
            {
                if (IAPEditor.iapEditor)
                    IAPEditor.SavePrefab(true);
                return paths;
            }
        }
    }
}