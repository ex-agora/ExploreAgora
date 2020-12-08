/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SIS
{
    /// <summary>
    /// Purchase confirmation for finishing transactions when using PayPal store.
    /// Confirming payments is a manual action so this script should be somewhere in your shop.
    /// </summary>
    public class PlayfabPayPalStoreWindow : MonoBehaviour
    {
        /// <summary>
        /// Button to trigger transaction confirmation on the PayPal store.
        /// </summary>
        public GameObject confirmButton;

        /// <summary>
        /// Button to close the transaction window (eventually without confirming transactions).
        /// </summary>
        public GameObject closeButton;

        /// <summary>
        /// Loading indicator for the user to see that something is going on.
        /// </summary>
        public Image loadingImage;


        #if SIS_IAP && PLAYFAB_PAYPAL
        //start displaying the UI buttons after some time
        void OnEnable()
        {
            StartCoroutine(UpdateStatus());
        }


        //rotate the loading indicator for visual representation
        void Update()
        {
            loadingImage.rectTransform.Rotate(-Vector3.forward * 100 * Time.deltaTime);
        }


        //hide UI buttons for some time to actually give the user the chance for payment
        private IEnumerator UpdateStatus()
        {
			if (PlayfabPayPalStore.instance == null)
            {
                closeButton.SetActive(true);
                yield break;
            }
            
            yield return new WaitForSeconds(20);
            confirmButton.SetActive(true);
            closeButton.SetActive(true);
        }


        /// <summary>
        /// Triggers transaction confirmation on the PayPal store.
        /// Usually assigned to a UI button in-game.
        /// </summary>
        public void ConfirmPurchase()
        {
            PlayfabPayPalStore.instance.ConfirmPurchase();
            StartCoroutine(DelayConfirmation());
        }


        //delay further confirm request within the timeout frame
        private IEnumerator DelayConfirmation()
        {
            confirmButton.SetActive(false);
            yield return new WaitForSeconds(10);
            confirmButton.SetActive(true);
        }


        //reset UI buttons to the original state
        void OnDisable()
        {
            confirmButton.SetActive(false);
            closeButton.SetActive(false);
        }
        #endif
    }
}
