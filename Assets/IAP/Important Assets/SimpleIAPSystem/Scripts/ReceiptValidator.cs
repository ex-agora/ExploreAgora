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
    /// Base class for receipt verification implementations.
    /// </summary>
	public class ReceiptValidator : MonoBehaviour
    {    
        /// <summary>
        /// Selected type for IAP verification.
        /// </summary>
        public VerificationType verificationType = VerificationType.none;


        /// <summary>
        /// Determines if a verification is possible on this platform.
        /// </summary>
        public virtual bool shouldValidate(VerificationType verificationType)
        {
            return false;
        }


        /// <summary>
        /// Verification for all products.
        /// </summary>
        public virtual void Validate(Product p = null)
        {
            
        }


        /// <summary>
        /// Verification for unified receipts i.e. Apple App Store.
        /// </summary>
        public virtual void Validate(string receipt)
        {

        }
    }


    /// <summary>
    /// Type of IAP verification, i.e. on app launch or purchase.
    /// </summary>
    public enum VerificationType
    {
        none,
        onStart,
        onPurchase,
        both
    }
}