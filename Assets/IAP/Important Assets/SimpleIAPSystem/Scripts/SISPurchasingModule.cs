/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

#if SIS_IAP
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace SIS
{
    /// <summary>
    /// Custom Unity IAP purchasing module for overwriting default store subsystems.
    /// </summary>
    public class SISPurchasingModule : AbstractPurchasingModule, IStoreConfiguration
    {
        public override void Configure()
		{
            //PlayFab
            #if PLAYFAB_PAYPAL
                RegisterStore("PayPal", new PlayfabPayPalStore());
            #endif

            #if PLAYFAB_STEAM
                RegisterStore("SteamStore", new PlayfabSteamStore());
            #endif

            //PlayFab does not fully support the Facebook SDK yet
            /*
            #if UNITY_FACEBOOK && PLAYFAB
                RegisterStore(FacebookStore.Name, new PlayfabFacebookStore());
            #endif
            */

            //Native
            #if STEAM_IAP
                RegisterStore("SteamStore", new SteamStore());
            #endif

            //VR
            #if OCULUS_RIFT_IAP || OCULUS_GEAR_IAP
                RegisterStore("OculusStore", new OculusStore());
            #endif
        }
    }
}
#endif