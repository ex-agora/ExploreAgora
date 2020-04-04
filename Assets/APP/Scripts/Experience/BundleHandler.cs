using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BundleHandler : MonoBehaviour
{
   
    [SerializeField] private InventoryObjectHolder inventoryObjectHolder;
    string bundleID;

    public string BundleID { get => bundleID; set => bundleID = value; }

    public int GetScannedCounter(string _scannedObjectName)
    {
        return inventoryObjectHolder.GetScanedCounter(_scannedObjectName);
    }

    public void ApplyFavoritismScanningKey(string _scannedObjectName) {
        inventoryObjectHolder.SetObject(_scannedObjectName, inventoryObjectHolder.GetScanedCounter(_scannedObjectName) + 1);
    }

}
