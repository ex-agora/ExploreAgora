/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

#if SIS_IAP && PLAYFAB_PAYPAL
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using PlayFab;
using PlayFab.ClientModels;

namespace SIS
{
    /// <summary>
    /// Store implementation for PayPal, based on the PlayfabStore class.
    /// </summary>
    public class PlayfabPayPalStore : PlayfabStore
    {
        /// <summary>
        /// Reference to this store class, since the user needs to confirm the purchase
        /// transaction manually in-game, thus calling the confirm method of this script.
        /// </summary>
        public static PlayfabPayPalStore instance { get; private set; }

        
        /// <summary>
        /// Setting this store reference on initialization.
        /// </summary>
        public PlayfabPayPalStore()
        {
            instance = this;
        }


        /// <summary>
        /// Overriding the initialization with setting the correct store.
        /// </summary>
        public override void Initialize(IStoreCallback callback)
        {
            storeId = "PayPal";
            this.callback = callback;
        }


        /// <summary>
        /// Overriding the product retrieval process to allow for validation-only behavior.
        /// Even though validation is first happening on purchase, we still need to this as
        /// otherwise Unity IAP would not initialize correctly without any products.
        /// </summary>
        public override void RetrieveProducts(ReadOnlyCollection<ProductDefinition> products)
        {
            #if PLAYFAB_VALIDATION
                this.products = new Dictionary<string, ProductDescription>();

                foreach(IAPObject obj in IAPManager.GetInstance().IAPObjects.Values)
                {
                    this.products.Add(obj.id, new ProductDescription(obj.id, new ProductMetadata(obj.realPrice, obj.title, obj.description, "USD", 0)));
                }

                //skip all catalog calls to PlayFab
                if(callback != null)
                    callback.OnProductsRetrieved(this.products.Values.ToList());
            #else
                base.RetrieveProducts(products);
            #endif
        }


        /// <summary>
        /// Overriding the purchase behavior to allow for validation-only behavior (physical goods).
        /// </summary>
        public override void Purchase(ProductDefinition product, string developerPayload)
        {
            #if PLAYFAB_VALIDATION
            //log in right before purchasing
            if (string.IsNullOrEmpty(PlayfabManager.userId))
            {
                PlayfabManager.GetInstance().LoginWithDevice((result) =>
                { 
                    //login failed, raise error
                    if (result == false)
                    {
                        OnPurchaseFailed(null);
                        return;
                    }

                    //we're logged in with PlayFab now
                    base.Purchase(product, developerPayload);
                });

                return;
            }
            #endif

            //logged in already
            base.Purchase(product, developerPayload);
        }


        /// <summary>
        /// Overriding the payment request for opening the PayPal website in the browser.
        /// </summary>
        public override void OnPurchaseResult(PayForPurchaseResult result)
        {
            ShopManager.ShowConfirmation();

            Application.OpenURL(result.PurchaseConfirmationPageURL);
        }


        /// <summary>
        /// Manually triggering purchase confirmation after a PayPal payment has been made.
        /// This is so that the transaction gets finished and PayPal actually substracts funds.
        /// </summary>
        public void ConfirmPurchase()
        {
            if (string.IsNullOrEmpty(orderId))
                return;

            ConfirmPurchaseRequest request = new ConfirmPurchaseRequest()
            {
                OrderId = orderId
            };

            PlayFabClientAPI.ConfirmPurchase(request, (result) => 
            {
                if (ShopManager.GetInstance() != null && ShopManager.GetInstance().confirmWindow != null)
                    ShopManager.GetInstance().confirmWindow.SetActive(false);

                OnPurchaseSucceeded(result);
            }, OnPurchaseFailed);
        }
    }
}
#endif