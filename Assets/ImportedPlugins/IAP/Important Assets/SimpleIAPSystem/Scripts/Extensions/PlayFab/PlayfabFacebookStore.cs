#if SIS_IAP && UNITY_FACEBOOK && PLAYFAB
using UnityEngine;
using UnityEngine.Purchasing.Extension;
using PlayFab;
using PlayFab.ClientModels;
using Facebook.Unity;

namespace SIS
{
    /// <summary>
    /// Store implementation for Facebook, based on the PlayfabStore class.
    /// </summary>
    public class PlayfabFacebookStore : PlayfabStore
    {
        /// <summary>
        /// Overriding the initialization with setting the correct store.
        /// </summary>
        public override void Initialize(IStoreCallback callback)
        {
            storeId = "Facebook";
            this.callback = callback;
        }


        /// <summary>
        /// Overriding the order initiation with Facebook-specific purchase request over FB.Canvas.
        /// </summary>
        public override void OnPurchaseStarted(StartPurchaseResult result)
        {
            orderId = result.OrderId;

            FB.Canvas.Pay("https://" + PlayFabSettings.TitleId.ToLower() + ".playfabapi.com/OpenGraphProduct/" + PlayFabSettings.TitleId.ToLower() + "/Facebook/1/" + result.Contents[0].ItemId,
                          requestId: orderId, callback: OnPurchaseResult);
        }


        //overriding the payment request with Facebook-specific return type and transaction id.
        private void OnPurchaseResult(IPayResult result)
        {
            if (result.ResultDictionary == null || result.ResultDictionary.Count == 0) return;
            if (!result.ResultDictionary.ContainsKey("payment_id")) return;

            PayForPurchaseRequest request = new PayForPurchaseRequest()
            {
                OrderId = orderId,
                ProviderName = storeId,
                Currency = "RM",
                ProviderTransactionId = result.ResultDictionary["payment_id"].ToString()
            };

            PlayFabClientAPI.PayForPurchase(request, OnPurchaseResult, OnPurchaseFailed);
        }
    }
}
#endif