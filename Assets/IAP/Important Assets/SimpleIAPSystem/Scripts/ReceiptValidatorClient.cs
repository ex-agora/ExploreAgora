/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

#if SIS_IAP
using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

namespace SIS
{
    /// <summary>
    /// IAP receipt verification on the client (local, on the device) using Unity IAPs validator class.
    /// Only supported on purchase.
    /// </summary>
	public class ReceiptValidatorClient : ReceiptValidator
    {
        /// <summary>
        /// Constructor for setting a default value.
        /// </summary>
        public ReceiptValidatorClient()
        {
            this.verificationType = VerificationType.onPurchase;  
        }

        /*
        #if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_TVOS
        /// <summary>
        /// Overriding the base method to only trigger on Unity IAP supported platforms.
        /// </summary>
        public override bool shouldValidate(VerificationType verificationType)
        {
            //when running on Android, validation is only supported on Google Play
            if (Application.platform == RuntimePlatform.Android && StandardPurchasingModule.Instance().androidStore != AndroidStore.GooglePlay)
                return false;

            if (this.verificationType == verificationType || this.verificationType == VerificationType.both)
                return true;

            return false;
        }


        /// <summary>
        /// Overriding the base method for constructing Unity IAP's CrossPlatformValidator and passing in purchase receipts.
        /// The validation result will either grant the item (success) or remove it from the inventory if granted already (failed).
        /// </summary>
        public override void Validate(Product p = null)
        {
            Product[] products = new Product[]{ p };
            if (p == null)
                products = IAPManager.controller.products.all;

            CrossPlatformValidator validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);

            for (int i = 0; i < products.Length; i++)
            {
                //do not validate virtual items and skip not purchased ones
                if (string.IsNullOrEmpty(p.receipt))
                    continue;

                //we found a receipt for this product on the device, initiate client receipt verification.
                //if we haven't found a receipt for this item, yet it is set to purchased. This can't be,
                //maybe the database contains fake data. Only pass the id to verification so it will fail
                try
                {
                    // On Google Play, result will have a single product Id.
                    // On Apple stores receipts contain multiple products.
                    validator.Validate(products[i].receipt);
                    IAPManager.GetInstance().PurchaseVerified(products[i].definition.id);

                    if (IAPManager.isDebug) Debug.Log("Local Receipt Validation passed for: " + products[i].definition.id);
                }
                catch (Exception ex)
                {
                    if (IAPManager.isDebug) Debug.Log("Local Receipt Validation failed for: " + products[i].definition.id + ". Exception: " + ex + ", " + ex.Message);

                    if ((ex is NullReferenceException || ex is IAPSecurityException) &&
                        DBManager.GetPurchase(products[i].definition.id) > 0)
                    {
                        IAPItem item = null;
                        if (ShopManager.GetInstance())
                            item = ShopManager.GetIAPItem(products[i].definition.id);
                        if (item) item.Purchased(false);
                        DBManager.RemovePurchase(products[i].definition.id);
                    }
                };
            }
        }
        #endif
        */
    }
}
#endif