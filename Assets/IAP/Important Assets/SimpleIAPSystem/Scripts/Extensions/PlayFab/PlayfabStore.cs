/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

#if PLAYFAB_PAYPAL || PLAYFAB_STEAM
#define PLAYFAB
#endif

#if SIS_IAP && PLAYFAB
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using SIS.SimpleJSON;

using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using PlayFab;
using PlayFab.ClientModels;

namespace SIS
{
    /// <summary>
    /// Represents the public interface of the underlying store system for PlayFab.
    /// This is the store base class other PlayFab billing implementations are making use of.
    /// </summary>
    public class PlayfabStore : IStore
    {
        /// <summary>
        /// Callback for hooking into the native Unity IAP logic.
        /// </summary>
        public IStoreCallback callback;

        /// <summary>
        /// List of products which are declared and retrieved by the billing system.
        /// </summary>
        public Dictionary<string, ProductDescription> products;

        /// <summary>
        /// Declaring the store name used in Unity IAP for product store identifiers.
        /// </summary>
        public string storeId = "StoreName";

        /// <summary>
        /// Keeping track of the order that is currently being processed,
        /// so we can confirm and finish it later on.
        /// </summary>
        public static string orderId;

        //product catalog that has been retrieved from PlayFab
        private static List<CatalogItem> catalog;
        //keeping track of the product that is currently being processed
        private string currentProduct;


        /// <summary>
        /// Initialize the instance using the specified IStoreCallback.
        /// </summary>
        public virtual void Initialize(IStoreCallback callback)
        {
            this.callback = callback;
        }


        /// <summary>
        /// Fetch the latest product metadata asynchronously with results returned via IStoreCallback.
        /// </summary>
        public virtual void RetrieveProducts(ReadOnlyCollection<ProductDefinition> products)
        {
            this.products = new Dictionary<string, ProductDescription>();

            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnCatalogRetrieved, OnSetupFailed);
        }


        //getting the items declared in PlayFab's catalog and converting them to Unity IAP format
        private void OnCatalogRetrieved(GetCatalogItemsResult result)
        {
            catalog = result.Catalog;

            for(int i = 0; i < result.Catalog.Count; i++)
            {
                CatalogItem catalogItem = result.Catalog[i];
                string itemId = IAPManager.GetIAPIdentifier(catalogItem.ItemId);
                if(!IAPManager.GetIAPKeys().Contains(itemId) || products.ContainsKey(itemId))
                    continue;
                
                decimal price = 0;
                string priceString = "";
                string currency = "";
                if(catalogItem.VirtualCurrencyPrices.Count > 0)
                   currency = catalogItem.VirtualCurrencyPrices.Keys.First();

                if (currency == "RM")
                {
                    price = (decimal)catalogItem.VirtualCurrencyPrices[currency] / 100m;
                    priceString = price.ToString("C");
                }
                else if (!string.IsNullOrEmpty(currency))
                {
                    price = catalogItem.VirtualCurrencyPrices[currency];
                    priceString = price.ToString();
                }

                ApplyCatalogItem(itemId, catalogItem, priceString);

                products.Add(itemId, new ProductDescription(itemId, new ProductMetadata(priceString, catalogItem.DisplayName, catalogItem.Description, "USD", price)));
            }

            if(callback != null)
                callback.OnProductsRetrieved(products.Values.ToList());
        }


        /// <summary>
        /// Handle a purchase request from a user.
        /// Developer payload is provided for stores that define such a concept.
        /// </summary>
        public virtual void Purchase(ProductDefinition product, string developerPayload)
        {
            IAPObject obj = IAPManager.GetIAPObject(product.id);
            if(obj.editorType == IAPType.Virtual)
            {
                Purchase(obj);
                return;
            }

            StartPurchaseRequest request = new StartPurchaseRequest()
            {
                Items = new List<ItemPurchaseRequest>()
                {
                    new ItemPurchaseRequest() { ItemId = product.id, Quantity = 1 }
                }
            };

            currentProduct = product.id;
            PlayFabClientAPI.StartPurchase(request, OnPurchaseStarted, OnPurchaseFailed);
        }


        /// <summary>
        /// Purchase overload for virtual products, as they differ in their workflow on PlayFab.
        /// Also the virtual currency funds are checked locally before forwarding the request to PlayFab.
        /// </summary>
        public void Purchase(IAPObject obj)
        {
            int curIndex = 0;
            for (int i = 0; i < obj.virtualPrice.Count; i++)
            {
                if (obj.virtualPrice[i].amount != 0)
                {
                    curIndex = i;
                    break;
                }
            }
            
            //local check before doing the request
            if (DBManager.GetFunds(obj.virtualPrice[curIndex].name) < obj.virtualPrice[curIndex].amount)
            {
                IAPManager.OnPurchaseFailed("Insufficient Funds.");
                return;
            }

            PurchaseItemRequest request = new PurchaseItemRequest()
            {
                ItemId = obj.id,
                VirtualCurrency = obj.virtualPrice[curIndex].name.Substring(0, 2).ToUpper(),
                Price = obj.virtualPrice[curIndex].amount
            };

            PlayFabClientAPI.PurchaseItem(request, OnPurchaseSucceeded, OnVirtualPurchaseFailed);
        }


        /// <summary>
        /// Callback retrieved when an (real money) order on PlayFab's servers has been initiated.
        /// Here the payment request for the order is being sent off, triggering native overlays.
        /// </summary>
        public virtual void OnPurchaseStarted(StartPurchaseResult result)
        {
            orderId = result.OrderId;
            currentProduct = result.Contents[0].ItemId;

            PayForPurchaseRequest request = new PayForPurchaseRequest()
            {
                OrderId = orderId,
                ProviderName = storeId,
                Currency = "RM"
            };

            PlayFabClientAPI.PayForPurchase(request, OnPurchaseResult, OnPurchaseFailed);
        }


        /// <summary>
        /// Callback retrieved when the payment result is received from PlayFab's servers.
        /// The purchase still needs to be acknowledged in this method.
        /// </summary>
        public virtual void OnPurchaseResult(PayForPurchaseResult result)
        {
            /*
            Debug.LogError("Status: " + result.Status + ", Currency: " + result.PurchaseCurrency +
                      ", Price: " + result.PurchasePrice + ", ProviderData: " + result.ProviderData +
                      ", PageURL: " + result.PurchaseConfirmationPageURL);
            */

            ConfirmPurchaseRequest request = new ConfirmPurchaseRequest()
            {
                OrderId = orderId
            };

            PlayFabClientAPI.ConfirmPurchase(request, OnPurchaseSucceeded, OnPurchaseFailed);
        }


        /// <summary>
        /// Callback from the billing system when a (real money) purchase completes successfully.
        /// </summary>
        public void OnPurchaseSucceeded(ConfirmPurchaseResult result)
        {
            orderId = string.Empty;
            ItemInstance item = result.Items[0];

            callback.OnPurchaseSucceeded(item.ItemId, item.PurchaseDate.ToString(), item.ItemInstanceId);
        }


        /// <summary>
        /// Callback from the billing system when a (virtual) purchase completes successfully.
        /// </summary>
        public void OnPurchaseSucceeded(PurchaseItemResult result)
        {
            ItemInstance item = result.Items[0];
            Dictionary<string, int> currencies = DBManager.GetCurrencies();

            //substract purchase price from the virtual currency locally
            //this is only for display purposes, as the funds are maintained on the server
            foreach (KeyValuePair<string, int> pair in currencies)
            {
                if(pair.Key.StartsWith(item.UnitCurrency.ToLower()))
                {
                    DBManager.IncreaseFunds(pair.Key, (int)(-item.UnitPrice));
                    break;
                }
            }

            //products with bundle contents only
            //besides of the main product price, also check for any bundle contents that should
            //be granted locally when this product has been bought such as other currencies
            #if PLAYFAB_VALIDATION
            //the PlayFab product catalog is not available, do a local check only where price is negative
            IAPObject obj = IAPManager.GetIAPObject(IAPManager.GetIAPIdentifier(item.ItemId));
            if(obj != null && obj.virtualPrice.Count > 1)
            {
                for(int i = 0; i < obj.virtualPrice.Count; i++)
                {
                    if(obj.virtualPrice[i].amount < 0)
                        DBManager.IncreaseFunds(obj.virtualPrice[i].name, -obj.virtualPrice[i].amount);
                }
            }
            #else
            //otherwise get bundle contents from retrieved catalog
            CatalogItem catalogItem = catalog.FirstOrDefault(x => x.ItemId == item.ItemId);
            if(catalogItem != null && catalogItem.Bundle != null && catalogItem.Bundle.BundledVirtualCurrencies != null &&
               catalogItem.Bundle.BundledVirtualCurrencies.Count > 0)
            {
                foreach (KeyValuePair<string, uint> dicPair in catalogItem.Bundle.BundledVirtualCurrencies)
                {
                    foreach (KeyValuePair<string, int> pair in currencies)
                    {
                        if (pair.Key.StartsWith(dicPair.Key.ToLower()))
                        {
                            DBManager.IncreaseFunds(pair.Key, (int)(dicPair.Value));
                            break;
                        }
                    }
                }
            }
            #endif

            //can't call the native callback because PlayFab returns the same ItemInstanceId for stackable products
            //this would result in an Unity IAP 'Already recorded transaction' message thus not doing anything
            //callback.OnPurchaseSucceeded(item.ItemId, item.PurchaseDate.ToString(), item.ItemInstanceId);

            //instead we call the finish events ourselves
            IAPManager.GetInstance().PurchaseVerified(item.ItemId);
            FinishTransaction(IAPManager.controller.products.WithID(item.ItemId).definition, item.ItemInstanceId);
        }


        /// <summary>
        /// Called by Unity Purchasing when a transaction has been recorded.
        /// Store systems should perform any housekeeping here, such as closing transactions or consuming consumables.
        /// </summary>
        public virtual void FinishTransaction(ProductDefinition product, string transactionId)
        {
            //IN THE CURRENT VERSION: nothing to do here!
            //consumables are consumed automatically on PlayFab's side after 5 seconds.

            /*
            //do not consume non-consumable products
            if (string.IsNullOrEmpty(transactionId) || product.type != ProductType.Consumable)
                return;
            */

            /*
            //but also do not consume virtual products (power-ups etc.) automatically,
            //if they are not set to be consumed instantly (i.e. their usage count is above zero)
            IAPObject obj = IAPManager.GetIAPObject(product.id);
            if(obj.editorType != IAPType.Currency && obj.usageCount > 0)
                return;
            */

            /*
            ConsumeItemRequest request = new ConsumeItemRequest()
            {
                ItemInstanceId = transactionId,
                ConsumeCount = 1
            };

            PlayFabClientAPI.ConsumeItem(request, OnTransactionFinished, OnPurchaseFailed);
            */
        }


        /*
        private void OnTransactionFinished(ConsumeItemResult result)
        {
            Debug.LogError(result.ItemInstanceId + " consumed.");
        }
        */


        /// <summary>
        /// Indicate that IAP is unavailable for a specific reason, such as IAP being disabled in device settings.
        /// </summary>
        public void OnSetupFailed(PlayFabError error)
        {
            callback.OnSetupFailed(InitializationFailureReason.NoProductsAvailable);
        }


        /// <summary>
        /// Method we are calling for any failed (real money) results in the billing interaction.
        /// </summary>
        public void OnPurchaseFailed(PlayFabError error)
        {
            callback.OnPurchaseFailed(new PurchaseFailureDescription(currentProduct, PurchaseFailureReason.PaymentDeclined, ""));
        }
        
        
        /// <summary>
        /// Method we are calling for any failed (virtual) results in the billing interaction.
        /// </summary>
        public void OnVirtualPurchaseFailed(PlayFabError error)
        {
            IAPManager.OnPurchaseFailed("Error: " + (int)error.Error + ", " + error.ErrorMessage);
        }


        //method for remote catalog config
        //converts a (downloaded) config string for virtual products into JSON nodes and overwrites
        //existing IAP objects with new properties, after doing a null reference check for empty nodes.
        private void ApplyCatalogItem(string id, CatalogItem item, string price)
        {
            IAPObject obj = IAPManager.GetIAPObject(id);
            if(!obj.fetch) return;

            switch(obj.editorType)
            {
                case IAPType.Currency:
                    if (item.Bundle == null || item.Bundle.BundledVirtualCurrencies == null || item.Bundle.BundledVirtualCurrencies.Count == 0)
                        break;

                    foreach(string curKey in item.Bundle.BundledVirtualCurrencies.Keys)
                    {
                        IAPCurrency cur = obj.virtualPrice.Find(x => x.name.StartsWith(curKey, System.StringComparison.OrdinalIgnoreCase));
                        if(cur != null) cur.amount = (int)item.Bundle.BundledVirtualCurrencies[curKey];
                    }
                    break;

                case IAPType.Virtual:
                    if (item.VirtualCurrencyPrices == null || item.VirtualCurrencyPrices.Count == 0)
                        break;

                    foreach(string curKey in item.VirtualCurrencyPrices.Keys)
                    {
                        IAPCurrency cur = obj.virtualPrice.Find(x => x.name.StartsWith(curKey, System.StringComparison.OrdinalIgnoreCase));
                        if(cur != null) cur.amount = (int)item.VirtualCurrencyPrices[curKey];
                    }
                    break;
            }

            //only fetch other details from platforms that do not provide this
            #if PLAYFAB_STEAM || PLAYFAB_PAYPAL
            if(!string.IsNullOrEmpty(item.DisplayName)) obj.title = item.DisplayName;
            if(!string.IsNullOrEmpty(item.Description)) obj.description = item.Description;
            if(!string.IsNullOrEmpty(price)) obj.realPrice = price;
            #endif
            
            if(!string.IsNullOrEmpty(item.CustomData))
            {
                JSONNode data = JSON.Parse(item.CustomData);
                if(!string.IsNullOrEmpty(data["requirement"].ToString()))
                {
                    data = data["requirement"];
                    if(!string.IsNullOrEmpty(data["entry"])) obj.req.entry = data["entry"];
                    if(!string.IsNullOrEmpty(data["labelText"])) obj.req.labelText = data["labelText"];
                    if(!string.IsNullOrEmpty(data["target"])) obj.req.target = data["target"].AsInt;
                }
            }
        }
    }
}
#endif