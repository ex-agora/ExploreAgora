using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    [SerializeField] private List<ScannedObjectHandler> scannedObjectHandlers;
    [SerializeField] private InventoryObjectHolder inventory;
    Dictionary<string, ScannedObjectHandler> scannedObjectMap;
    private void Start()
    {
        scannedObjectMap = new Dictionary<string, ScannedObjectHandler>();
        for (int i = 0; i < scannedObjectHandlers.Count; i++)
        {
            scannedObjectMap.Add(scannedObjectHandlers[i].ScannedName, scannedObjectHandlers[i]);
        }
        ScannedObjectHandler temp = null;
        foreach (var i in inventory.ScanedObjects)
        {
            if (scannedObjectMap.TryGetValue(i.Key, out temp)) {
                temp.UnlockObject();
            }
        }
    }

}
