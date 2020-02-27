/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

#if SIS_IAP && STEAM_IAP
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using Steamworks;

namespace SIS
{
    /// <summary>
    /// Represents the public interface of the underlying store system for the Steam Store.
    /// Using Steam Inventory Services. 
    /// </summary>
    public class SteamStore : IStore
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

        #pragma warning disable 0414
        private SteamInventoryResult_t steamInventoryResult = SteamInventoryResult_t.Invalid;
        private SteamItemDetails_t[] steamItemDetails;
        protected Callback<SteamInventoryResultReady_t> steamInventoryResultReady;
        #pragma warning restore 0414


        /// <summary>
        /// Initialize the instance using the specified IStoreCallback.
        /// </summary>
        public virtual void Initialize(IStoreCallback callback)
        {
            this.callback = callback;

            if (!SteamManager.Initialized)
            {
                return;
            }

            steamInventoryResultReady = Callback<SteamInventoryResultReady_t>.Create(OnInitialized);
            bool result = SteamInventory.GetAllItems(out steamInventoryResult);

            if (!result)
            {
                OnSetupFailed(string.Empty);
                steamInventoryResultReady.Unregister();
                SteamInventory.DestroyResult(steamInventoryResult);
            }
        }


        //callback when the core system has been initialized
        private void OnInitialized(SteamInventoryResultReady_t pCallback)
		{
            if (pCallback.m_result != EResult.k_EResultOK)
            {
                OnSetupFailed(string.Empty);
                return;
            }

            this.products = new Dictionary<string, ProductDescription>();

            steamInventoryResultReady.Unregister();
            OnProductsRetrieved(IAPManager.GetProductDefinitions());
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
        private void OnProductsRetrieved(ProductDefinition[] list)
        {
            uint outItemsArraySize = 0;
            bool result = SteamInventory.GetResultItems(steamInventoryResult, null, ref outItemsArraySize);

            string globalId = null;
            string storeId = null;
            IAPObject obj = null;

            for (int i = 0; i < list.Length; i++)
            {
                storeId = list[i].storeSpecificId;
                globalId = list[i].id;
                obj = IAPManager.GetIAPObject(globalId);

                if (!products.ContainsKey(globalId))
                {
                    products.Add(globalId, new ProductDescription(storeId, new ProductMetadata(obj.realPrice, obj.title, obj.description, "", (decimal)0)));
                }
            }

            //fails when user does not own any items
            if (!result || outItemsArraySize == 0)
            {
                callback.OnProductsRetrieved(products.Values.ToList());
                return;
            }

            SteamItemDetails_t[] steamItemDetails = new SteamItemDetails_t[outItemsArraySize];
            result = SteamInventory.GetResultItems(steamInventoryResult, steamItemDetails, ref outItemsArraySize);

            OnPurchasesRetrieved(steamItemDetails);
        }


        //processing products & purchases combined and returning the result to Unity IAP
        private void OnPurchasesRetrieved(SteamItemDetails_t[] list)
        {
            string globalId = null;
            string storeId = null;
            ProductDefinition[] definitions = IAPManager.GetProductDefinitions();

            for (int i = 0; i < list.Length; i++)
            {
                storeId = list[i].m_iDefinition.ToString();
                globalId = IAPManager.GetIAPIdentifier(storeId);

                //check for non-consumed consumables
                IAPObject obj = IAPManager.GetIAPObject(globalId);
                if (obj != null && obj.type == ProductType.Consumable)
                {
                    FinishTransaction(definitions.First(x => x.id == globalId), list[i].m_itemId.ToString());
                    if (DBManager.GetPurchase(globalId) > 0) DBManager.RemovePurchase(globalId);
                    continue;
                }

                products[globalId] = new ProductDescription(storeId, products[globalId].metadata, list[i].m_itemId.ToString(), list[i].m_itemId.ToString());

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
                int steamProductId = 0;
                if (int.TryParse(currentProduct, out steamProductId))
                {
                    steamInventoryResultReady.Unregister();
                    steamInventoryResultReady = Callback<SteamInventoryResultReady_t>.Create(OnPurchaseSucceeded);
                    SteamInventory.StartPurchase(new SteamItemDef_t[] { (SteamItemDef_t)steamProductId }, new uint[] { 1 }, 1);
                }
                else
                {
                    OnPurchaseFailed("Cannot convert selected Product Id to Steam Item Id.", 4);
                }
            #endif
        }


        /// <summary>
        /// Callback from the billing system when a purchase completes (be it successful or not).
        /// </summary>
        public void OnPurchaseSucceeded(SteamInventoryResultReady_t pCallback)
        {
            if (pCallback.m_result != EResult.k_EResultOK)
            {
                OnPurchaseFailed(pCallback.m_result.ToString(), 0);
                return;
            }

            //get properties of purchased item
            uint grantTime = SteamInventory.GetResultTimestamp(pCallback.m_handle);
            string transactionId = "";
            string sku = currentProduct;

            string ValueBuffer;
            uint ValueBufferSize = 0;
            bool result = SteamInventory.GetResultItemProperty(pCallback.m_handle, 0, null, out ValueBuffer, ref ValueBufferSize);

            if (result)
            {
                ValueBufferSize = 9;
                SteamInventory.GetResultItemProperty(pCallback.m_handle, 0, "itemdefid", out sku, ref ValueBufferSize);

                ValueBufferSize = 64;
                SteamInventory.GetResultItemProperty(pCallback.m_handle, 0, "itemID", out transactionId, ref ValueBufferSize);
            }

            callback.OnPurchaseSucceeded(sku, grantTime.ToString(), transactionId);
        }


        /// <summary>
        /// Called by Unity Purchasing when a transaction has been recorded.
        /// Store systems should perform any housekeeping here, such as closing transactions or consuming consumables.
        /// </summary>
        public virtual void FinishTransaction(ProductDefinition product, string transactionId)
        {
            if (product.type != ProductType.Consumable)
            {
                SteamInventory.DestroyResult(steamInventoryResult);
                return;
            }

            steamInventoryResultReady.Unregister();
            steamInventoryResultReady = Callback<SteamInventoryResultReady_t>.Create(OnTransactionFinished);
            SteamInventory.ConsumeItem(out steamInventoryResult, (SteamItemInstanceID_t)ulong.Parse(transactionId), 1);
        }


        //the previous consume product callback finished (be it successful or not)
        private void OnTransactionFinished(SteamInventoryResultReady_t pCallback)
        {
            if (pCallback.m_result != EResult.k_EResultOK)
            {
                OnPurchaseFailed(pCallback.m_result.ToString(), 0);
                return;
            }

            steamInventoryResultReady.Unregister();
            SteamInventory.DestroyResult(pCallback.m_handle);
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
                    reason = PurchaseFailureReason.ExistingPurchasePending;
                    break;
                case 2:
                    reason = PurchaseFailureReason.UserCancelled;
                    break;
                case 3:
                    reason = PurchaseFailureReason.PurchasingUnavailable;
                    break;
                case 4:
                    reason = PurchaseFailureReason.ProductUnavailable;
                    break;
                case 5:
                    reason = PurchaseFailureReason.SignatureInvalid;
                    break;
            }

            callback.OnPurchaseFailed(new PurchaseFailureDescription(currentProduct, reason, error));
        }
    }
}
#endif