using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory Objects", menuName = "SO/App/Profile/InventoryObjects", order = 0)]
public class InventoryObjectHolder : ScriptableObject
{
    [SerializeField] StringIntDictionary scanedObjects;
    public int GetScanedCounter(string _Name) {
        int counter = -1,value;
        if (scanedObjects.TryGetValue(_Name, out value))
            counter = value;
        return counter;
    }
    public void SetObjects(StringIntDictionary _ScanedObjs) => scanedObjects = _ScanedObjs;
    public void SetObject(string _Name, int _Conter) => scanedObjects.Add(_Name, _Conter);
}
