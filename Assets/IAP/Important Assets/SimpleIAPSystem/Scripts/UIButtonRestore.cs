/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using UnityEngine;
#if SIS_IAP
using UnityEngine.Purchasing;
#endif

namespace SIS
{
    /// <summary>
    /// Simple script to handle restoring purchases on platforms requiring it. Restoring purchases is a
    /// requirement by e.g. Apple and your app will be rejected if you do not provide it.
    /// </summary>
    public class UIButtonRestore : MonoBehaviour
    {
        //only show the restore button on platforms supporting it
        void Start()
        {
            bool supportedPlatform = false;

            #if SIS_IAP
	            #if UNITY_ANDROID
	                if (StandardPurchasingModule.Instance().appStore == AppStore.SamsungApps)
	                    supportedPlatform = true;
	            #elif UNITY_IOS
	                supportedPlatform = true;
	            #endif
            #endif

            gameObject.SetActive(supportedPlatform);
        }


        /// <summary>
        /// Calls Unity IAPs RestoreTransactions method.
        /// It makes sense to add this to an UI button event.
        /// </summary>
        public void Restore()
        {
            #if SIS_IAP
                IAPManager.PurchaseProduct("restore");
            #endif
        }
    }
}
