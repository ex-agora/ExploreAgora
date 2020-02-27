/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

#if SIS_IAP
using UnityEngine.Purchasing;
#endif

using UnityEngine;
using UnityEngine.UI;

namespace SIS
{
    /// <summary>
    /// Shop item properties: this class basically stores all necessary variables for visualizing a product in the shop UI.
    /// </summary>
    public class IAPItem : MonoBehaviour
    {
        /// <summary>
        /// ID of the product. Do not enter if you are letting
        /// the ShopManager instantiate your shop items
        /// </summary>
        public string productId;

        /// <summary>
        /// Product name or title.
        /// </summary>
        public Text title;

        /// <summary>
        /// Product description.
        /// </summary>
        public Text description;

        /// <summary>
        /// Boolean for setting all labels to uppercase.
        /// </summary>
        public bool uppercase = false;

        /// <summary>
        /// Icon sprite for better visualization.
        /// </summary>
        public Image icon;

        /// <summary>
        /// Array of price labels, as there could be more than one currency for virtual game purchases.
        /// </summary>
        public Text[] price;

        /// <summary>
        /// Buy button that invokes the actual purchase.
        /// </summary>
        public GameObject buyButton;

        /// <summary>
        /// Buy trigger, used for making the buy button visible.
        /// (optional - could be used for 'double tap to purchase')
        /// </summary>
        public GameObject buyTrigger;

        /// <summary>
        /// Label that displays text while this item is locked.
        /// </summary>
        public Text lockedLabel;

        /// <summary>
        /// UI elements that will be de-activated when unlocking this item.
        /// </summary>
        public GameObject[] hideOnUnlock;

        /// <summary>
        /// UI elements that will be activated when unlocking this item.
        /// </summary>
        public GameObject[] showOnUnlock;

        /// <summary>
        /// Additional UI elements that will be activated on sold items.
        /// </summary>
        public GameObject sold;

        /// <summary>
        /// Additional UI elements that will be activated on selected items.
        /// </summary>
        public GameObject selected;

        /// <summary>
        /// Button for selecting this item.
        /// </summary>
        public GameObject selectButton;

        /// <summary>
        /// Button for deselecting this item.
        /// </summary>
        public GameObject deselectButton;

        //selection checkbox, cached for triggering other checkboxes
        //in the same group on selection/deselection
        private Toggle selCheck;


        //set up delegates and selection checkboxes
        void Start()
        {
            //if a selection of this item is possible
            if (selectButton)
            {
                //get checkbox component
                selCheck = selectButton.GetComponent<Toggle>();

                if (selCheck) selCheck.group = transform.parent.GetComponent<ToggleGroup>();
            }
        }


        //if we have a possible purchase confirmation set up or pending,
        //hide the buy button when disabling this item to reset it
        void OnDisable()
        {
            if (buyTrigger)
                ConfirmPurchase(false);
        }


        /// <summary>
        /// Initialize virtual or real item properties based on IAPObject set in IAP Settings editor.
        /// Called by ShopManager.
        /// </summary>
        public void Init(IAPObject obj)
        {
            //cache
            string name = obj.title;
            string descr = obj.description.Replace("\\n", "\n");
            string lockText = obj.req.labelText;

            //store the item id for later purposes
            productId = obj.id;
            //set icon to the matching sprite
            if (icon) icon.sprite = obj.icon;

            //when 'uppercase' has been checked,
            //convert title and description text to uppercase,
            //otherwise just keep and set them as they are
            if (uppercase)
            {
                name = name.ToUpper();
                descr = descr.ToUpper();
                if(!string.IsNullOrEmpty(lockText))
                    lockText = lockText.ToUpper();
            }

            if (title) title.text = name;
            if (description) description.text = descr;

            if (obj.editorType != IAPType.Virtual)
            {
                //purchases for real money have only one price value,
                //so we just use the first entry of the price label array
                if (price.Length > 0) price[0].text = obj.realPrice;
            }
            else if (obj.virtualPrice.Count > 0)
            {
                //purchases for virtual currency could have more than one price value,
                //so we loop over all entries and set the corresponding price label
                for (int i = 0; i < price.Length; i++)
                    if (price[i]) price[i].text = obj.virtualPrice[i].amount.ToString();
            }

            //set locked label text in case a requirement has been set
            if (lockedLabel && !string.IsNullOrEmpty(obj.req.entry)
                && !string.IsNullOrEmpty(obj.req.labelText))
                lockedLabel.text = lockText;
        }


        #if SIS_IAP
        /// <summary>
        /// Overwrite real money item properties with online data received from the billing plugin initialization callback.
        /// Called by ShopManager.
        /// </summary>
        public void Init(Product product)
        {
            //cache
            string name = product.metadata.localizedTitle;
            string descr = product.metadata.localizedDescription;
            string pr = product.metadata.localizedPriceString;

            //always check for empty strings (product missing on store)
            if (!string.IsNullOrEmpty(name))
            {
                //do not populate item with fake data from Unity IAP in test mode
                if (name.StartsWith("FAKE", System.StringComparison.OrdinalIgnoreCase))
                    return;

                //normally, the online item name received from the App Store
                //has the application name attached, so we remove that here
                int cap = name.IndexOf("(");
                if (cap > 0)
                    name = name.Substring(0, cap - 1);

                //when 'uppercase' has been checked,
                //convert title and description text to uppercase,
                //otherwise just keep and set them as they are
                if (uppercase) name = name.ToUpper();
                if (title) title.text = name;
            }
            
            if(!string.IsNullOrEmpty(descr))
            {
                //replace line breaks with proper formatting
                descr = descr.Replace("\\n", "\n");

                if (uppercase) descr = descr.ToUpper();
                if (description) description.text = descr;
            }        

            //purchases for real money only have one price value,
            //so we just use the first entry of the price label array
            if (!string.IsNullOrEmpty(pr) && price.Length > 0)
            {
                price[0].text = pr;
            }
        }
        #endif


        /// <summary>
        /// Unlocks this item by hiding the 'locked' gameobject and setting up the default state.
        /// Called by ShopManager.
        /// </summary>
        public void Unlock()
        {
            for (int i = 0; i < hideOnUnlock.Length; i++)
                hideOnUnlock[i].SetActive(false);

            for (int i = 0; i < showOnUnlock.Length; i++)
                showOnUnlock[i].SetActive(true);
        }


        /// <summary>
        /// Show the buy button based on the bool passed in. This simulates 'double tap to purchase' behavior,
        /// and only works when setting a buyTrigger button.
        /// </summary>
        public void ConfirmPurchase(bool selected)
        {
            if (!selected)
                buyButton.SetActive(false);
        }


        /// <summary>
        /// When the buy button has been clicked, here we try to purchase this item.
        /// This calls into the corresponding billing workflow of the IAPManager.
        /// </summary>
        public void Purchase()
        {
			IAPManager.PurchaseProduct(this.productId);
		
            //hide buy button once a purchase was made
            //only when an additional buy trigger was set
            if (buyTrigger)
                ConfirmPurchase(false);
        }


        /// <summary>
        /// Set this item to 'purchased' state (true), or unpurchased state (false) for fake purchases.
        /// </summary>
        public void Purchased(bool state)
        {
            //in case we restored an old purchase on a
            //locked item, we have to unlock it first
            Unlock();

            //back to unpurchased state, deselect
            if (!state) Deselect();
            //activate the select button
            else if (selectButton) selectButton.SetActive(state);

            //initialize variables for a product with upgrades
            IAPObject obj = IAPManager.GetIAPObject(productId);
            bool hasUpgrade = false;
            string nextUpgrade = obj.req.nextId;

            //in case this good has upgrades, here we find the next upgrade
            //and replace displayed item data in the store with its upgrade details
            if (!string.IsNullOrEmpty(nextUpgrade))
            {
                hasUpgrade = true;
                Init(IAPManager.GetIAPObject(nextUpgrade));
            }

            //take upgrade state into account
            if (state && hasUpgrade)
                state = !hasUpgrade;

            //activate the sold gameobject
            if (sold) sold.SetActive(state);

            //hide both buy trigger and buy button, for ignoring further purchase clicks.
            //but don't do that for subscriptions, so that the user could easily renew it
            if ((int)obj.type > 1) return;

            if (buyTrigger)
            {
                buyTrigger.SetActive(!state);
                buyButton.SetActive(false);
            }
            else
                buyButton.SetActive(!state);
        }


        /// <summary>
        /// Handles selection state for this item, but this method gets called on other radio buttons within the same group too.
        /// Called by selectButton's Toggle component.
        /// </summary>
        public void IsSelected(bool thisSelect)
        {
            //if this object has been selected
            if (thisSelect)
            {
                //tell our ShopManager to change the database entry
                ShopManager.SetToSelected(this);

                //if we have a deselect button or a 'selected' gameobject, show them
                //and hide the select button for ignoring further selections              
                if (deselectButton) deselectButton.SetActive(true);
                if (selected) selected.SetActive(true);

                Toggle toggle = selectButton.GetComponent<Toggle>();
                if (toggle.group)
                {
                    //hacky way to deselect all other toggles, even deactivated ones
                    //(toggles on deactivated gameobjects do not receive onValueChanged events)
                    IAPItem[] others = toggle.group.GetComponentsInChildren<IAPItem>(true);
                    for (int i = 0; i < others.Length; i++)
                    {
                        if (others[i].selCheck.isOn && others[i] != this)
                        {
                            others[i].IsSelected(false);
                            break;
                        }
                    }
                }

                toggle.isOn = true;
                selectButton.SetActive(false);
            }
            else
            {
                //if another object has been selected, show the
                //select button for this item and hide the 'selected' state
                if (!deselectButton) selectButton.SetActive(true);
                if (selected) selected.SetActive(false);
            }
        }

        /// <summary>
        /// Called when deselecting this item via the deselectButton.
		/// <summary>
        public void Deselect()
        {
            //hide the deselect button and 'selected' state
            deselectButton.SetActive(false);
            if (selected) selected.SetActive(false);

            //tell our checkbox component that this object isn't checked
            if (selCheck) selCheck.isOn = false;
            //tell our ShopManager to change the database entry accordingly
            ShopManager.SetToDeselected(this);
            //re-show the select button
            selectButton.SetActive(true);
        }
    }
}
