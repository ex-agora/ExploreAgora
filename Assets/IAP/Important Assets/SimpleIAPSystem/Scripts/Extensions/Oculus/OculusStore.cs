/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

#if SIS_IAP
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

#if OCULUS_RIFT_IAP || OCULUS_GEAR_IAP
using Oculus.Platform;
using Oculus.Platform.Models;

namespace SIS
{
    /// <summary>
    /// Represents the public interface of the underlying store system for the Oculus Store.
    /// </summary>
    public class OculusStore : IStore
    {      
        /// <summary>
        /// Callback for hooking into the native Unity IAP logic.
        /// </summary>
        public IStoreCallback callback;

        /// <summary>
        /// List of products which are declared and retrieved by the billing system.
        /// </summary>
        public Dictionary<string, ProductDescription> products;

        //keeping track of the product that is currently being processed
        private string currentProduct = "";


        /// <summary>
        /// Initialize the instance using the specified IStoreCallback.
        /// </summary>
        public virtual void Initialize(IStoreCallback callback)
        {
            this.callback = callback;

            Core.AsyncInitialize().OnComplete(OnInitialized);
        }


        //callback when the core system has been initialized
        private void OnInitialized(Message result)
		{
            this.products = new Dictionary<string, ProductDescription>();

            if (result.IsError)
            {
                OnSetupFailed(result.GetError().Code + ", " + result.GetError().HttpCode + ", " + result.GetError().Message + ". " + 
                              result.GetPlatformInitialize().Result.ToString());
                return;
            }

            IAP.GetProductsBySKU(IAPManager.GetRealKeys()).OnComplete(OnProductsRetrieved);
		}


        /// <summary>
        /// Fetch the latest product metadata, including purchase receipts,
        /// asynchronously with results returned via IStoreCallback.
        /// </summary>
        public void RetrieveProducts(ReadOnlyCollection<ProductDefinition> products)
        {
            //nothing to do here since the billing system has its own callback
        }


        //the real implementation of RetrieveProducts, however we are not directly returning
        //the list to Unity IAP here but also fetch the list of user purchases first
        private void OnProductsRetrieved(Message<ProductList> result)
        {
            if(result.IsError)
            {
                OnSetupFailed(result.GetError().Code + ", " + result.GetError().HttpCode + ", " + result.GetError().Message + ".");
                return;
            }
            
            ProductList list = result.GetProductList();
            string globalId = null;
            string storeId = null;

            for(int i = 0; i < list.Count; i++)
            {
                storeId = list[i].Sku;
                globalId = IAPManager.GetIAPIdentifier(storeId);

                if (!products.ContainsKey(globalId))
                {
                    products.Add(globalId, new ProductDescription(storeId, new ProductMetadata(list[i].FormattedPrice, list[i].Name, list[i].Description, "", (decimal)0)));
                }
            }

            IAP.GetViewerPurchases().OnComplete(OnPurchasesRetrieved);
        }


        //processing products & purchases combined and returning the result to Unity IAP
        private void OnPurchasesRetrieved(Message<PurchaseList> result)
        {
            if(result.IsError)
            {
                OnSetupFailed(result.GetError().Code + ", " + result.GetError().HttpCode + ", " + result.GetError().Message + ".");
                return;
            }

            PurchaseList list = result.GetPurchaseList();
            string globalId = null;
            string storeId = null;

            //check for non-consumed consumables
            for (int i = 0; i < list.Count; i++)
            {
                storeId = list[i].Sku;
                globalId = IAPManager.GetIAPIdentifier(storeId);

                IAPObject obj = IAPManager.GetIAPObject(globalId);
                if (obj != null && obj.type == ProductType.Consumable)
                {
                    IAP.ConsumePurchase(storeId);
                    continue;
                }

                products[globalId] = new ProductDescription(storeId, products[globalId].metadata, list[i].GrantTime.ToString(), list[i].ID.ToString());

                #if !UNITY_EDITOR
                //auto restore products in case database does not match
                if (DBManager.GetPurchase(globalId) == 0) DBManager.SetPurchase((globalId));
                #endif
            }

            callback.OnProductsRetrieved(products.Values.ToList());
        }


        /// <summary>
        /// Handle a purchase request from a user.
        /// Developer payload is provided for stores that define such a concept.
        /// </summary>
        public virtual void Purchase(ProductDefinition product, string developerPayload)
        {
            currentProduct = product.storeSpecificId;

            #if UNITY_EDITOR
                IAPManager.GetInstance().GetComponent<IAPListener>().HandleSuccessfulPurchase(product.id);
            #else
                IAP.LaunchCheckoutFlow(currentProduct).OnComplete(OnPurchaseSucceeded);
            #endif
        }


        /// <summary>
        /// Callback from the billing system when a purchase completes (be it successful or not).
        /// </summary>
        public void OnPurchaseSucceeded(Message<Purchase> result)
        {
            if(result.IsError)
            {
                OnPurchaseFailed(result.GetError().Message, result.GetError().Code);
                return;
            }

            string transactionId = result.GetPurchase().ID.ToString();
            if(IAPManager.isDebug)
            {
                //allow for multiple test purchases with unique transactions
                transactionId = (System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds.ToString();
            }

            callback.OnPurchaseSucceeded(result.GetPurchase().Sku, result.GetPurchase().GrantTime.ToString(), transactionId);
        }


        /// <summary>
        /// Called by Unity Purchasing when a transaction has been recorded.
        /// Store systems should perform any housekeeping here, such as closing transactions or consuming consumables.
        /// </summary>
        public virtual void FinishTransaction(ProductDefinition product, string transactionId)
        {
            if (product.type != ProductType.Consumable)
                return;

            IAP.ConsumePurchase(product.storeSpecificId).OnComplete(OnTransactionFinished);
        }


        //the previous consume product callback finished (be it successful or not)
        private void OnTransactionFinished(Message result)
        {
            if (result.IsError)
            {
                OnPurchaseFailed(result.GetError().Message, result.GetError().Code);
                return;
            }
        }


        /// <summary>
        /// Indicate that IAP is unavailable for a specific reason, such as IAP being disabled in device settings.
        /// </summary>
        public void OnSetupFailed(string error)
        {
            callback.OnSetupFailed(InitializationFailureReason.NoProductsAvailable);
        }


        /// <summary>
        /// Method we are calling for any failed results in the billing interaction.
        /// Here error codes are mapped to more user-friendly descriptions shown to them.
        /// </summary>
        public void OnPurchaseFailed(string error, int code)
        {
            PurchaseFailureReason reason = PurchaseFailureReason.Unknown;
            switch(code)
            {
                case 1:
                case 7:
                case -1010:
                    reason = PurchaseFailureReason.ExistingPurchasePending;
                    break;
                case -1005:
                    reason = PurchaseFailureReason.UserCancelled;
                    break;
                case 3:
                case -1009:
                    reason = PurchaseFailureReason.PurchasingUnavailable;
                    break;
                case 4:
                case -1006:
                    reason = PurchaseFailureReason.ProductUnavailable;
                    break;
                case 5:
                case -1002:
                case -1003:
                case -1004:
                    reason = PurchaseFailureReason.SignatureInvalid;
                    break;
            }

            callback.OnPurchaseFailed(new PurchaseFailureDescription(currentProduct, reason, error));
        }
    }
}
#endif
#endif