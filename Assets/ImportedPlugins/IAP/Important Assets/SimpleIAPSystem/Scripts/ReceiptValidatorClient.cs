/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

#if SIS_IAP
using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
using System.Collections;
using System.Collections.Generic;

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

        public void Checkout(IPurchaseReceipt p, receipt r)
        {
            GooglePlayReceipt googlePlay = p as GooglePlayReceipt;
            if (p != null)
            {
                CompleteCheckoutData checkoutData = new CompleteCheckoutData();
                checkoutData.packageName = googlePlay.packageName;
                checkoutData.transactionID = googlePlay.transactionID;
                checkoutData.productId = googlePlay.productID;
                checkoutData.purchaseToken = googlePlay.purchaseToken;
                checkoutData.storeType = AndroidStore.GooglePlay.ToString();
                NetworkManager.Instance.CompleteCheckout(checkoutData, OnCheckoutSuccess, OnCheckoutFailed);
            }
            AppleInAppPurchaseReceipt appleInApp = p as AppleInAppPurchaseReceipt;
            if (appleInApp != p)
            {
                CompleteCheckoutData checkoutData = new CompleteCheckoutData();
                checkoutData.packageName = "";
                checkoutData.productId = appleInApp.productID;
                checkoutData.purchaseToken = r.Payload;
                checkoutData.storeType = "IOS";
                checkoutData.transactionID = r.TransactionID;
                NetworkManager.Instance.CompleteCheckout(checkoutData, OnCheckoutSuccess, OnCheckoutFailed);
            }
        }
        public void Checkout(string p, receipt r, IPurchaseReceipt purchaseReceipt =null)
        {
            if (purchaseReceipt != null)
            {
                GooglePlayReceipt googlePlay = purchaseReceipt as GooglePlayReceipt;
                if (purchaseReceipt != null)
                {
                    CompleteCheckoutData checkoutData = new CompleteCheckoutData();
                    checkoutData.packageName = googlePlay.packageName;
                    checkoutData.transactionID = googlePlay.transactionID;
                    checkoutData.productId = googlePlay.productID;
                    checkoutData.purchaseToken = googlePlay.purchaseToken;
                    checkoutData.storeType = AndroidStore.GooglePlay.ToString();
                    NetworkManager.Instance.CompleteCheckout(checkoutData, OnCheckoutSuccess, OnCheckoutFailed);
                }
            }
            else if (p == "Android")
            {
                CompleteCheckoutData checkoutData = new CompleteCheckoutData();
                checkoutData.packageName = "Pouch of Power Stones";
                checkoutData.productId = "5ps1ex1000";
                checkoutData.purchaseToken = r.Payload;
                checkoutData.storeType = AndroidStore.GooglePlay.ToString();
                checkoutData.transactionID = r.TransactionID;
                NetworkManager.Instance.CompleteCheckout(checkoutData, OnCheckoutSuccess, OnCheckoutFailed);
            }
            else if (p == "IOS")
            {
                CompleteCheckoutData checkoutData = new CompleteCheckoutData();
                checkoutData.packageName = "Pouch of Power Stones";
                checkoutData.productId = "5ps1ex1000";
                checkoutData.purchaseToken = r.Payload;
                checkoutData.storeType = "IOS";
                checkoutData.transactionID = r.TransactionID;
                NetworkManager.Instance.CompleteCheckout(checkoutData, OnCheckoutSuccess, OnCheckoutFailed);
            }
        }
        private void OnCheckoutSuccess(NetworkParameters obj)
        {
            PromocodeUIHandler.Instance.SetCoins(1000);
            UXFlowManager.Instance.GetProfile();
        }
        private void OnCheckoutFailed(NetworkParameters obj)
        {
            int x = 0;
            x = 1;
        }
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
            Product[] products = new Product[] { p };
            if (p == null)
                products = IAPManager.controller.products.all;
            
            CrossPlatformValidator validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);
            for (int i = 0; i < products.Length; i++)
            {
                
                //do not validate virtual items and skip not purchased ones
                if (string.IsNullOrEmpty(p.receipt))
                    continue;
            receipt r = JsonUtility.FromJson<receipt>(p.receipt);
                //we found a receipt for this product on the device, initiate client receipt verification.
                //if we haven't found a receipt for this item, yet it is set to purchased. This can't be,
                //maybe the database contains fake data. Only pass the id to verification so it will fail

                //Checkout("Android", r);
                try
                {
                    // On Google Play, result will have a single product Id.
                    // On Apple stores receipts contain multiple products.
                    var receiptResults = validator.Validate(products[i].receipt);
                    IAPManager.GetInstance().PurchaseVerified(products[i].definition.id);

                    foreach (var receiptResult in receiptResults) {
                        Checkout("Android", r, receiptResult);
                    }
                   // StartCoroutine(ValidateServerTest(products[i].receipt));
                    // StartCoroutine(ValidateServerTest(products[i].definition.id));
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

        IEnumerator ValidateServerTest(string receipt)
        {
            WWWForm form = new WWWForm();
            form.AddField("invoiceId", receipt);

            using (var w = UnityEngine.Networking.UnityWebRequest.Post("https://exploreagora.herokuapp.com/player/payment/checkInvoice", form))
            {
                yield return w.SendWebRequest();
                //if error 
                if (w.isNetworkError || w.isHttpError)
                {
                    Debug.Log(w.error);
                    Debug.Log("Error");
                    //outputText.text = w.downloadHandler.text + " " + w.error;
                }
                //if Done
                else
                {
                    Debug.Log("Finished Uploading Screenshot");
                    Debug.Log(w.downloadHandler.text + "      " + receipt);
                    // output = processJson(w.downloadHandler.text);
                    //outputText.text = output;
                    //For Example

                }
                // hide loading Canvas and show mainCanvas

            }
        }



    }



}


[Serializable]
public class receipt {
    public string Store;
    public string TransactionID;
    public string Payload;
}
#endif