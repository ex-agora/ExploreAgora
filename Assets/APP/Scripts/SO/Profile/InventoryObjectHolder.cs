using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory Objects", menuName = "SO/App/Profile/InventoryObjects", order = 0)]
public class InventoryObjectHolder : ScriptableObject
{
    [SerializeField] StringIntDictionary scanedObjects;

    public StringIntDictionary ScanedObjects { get => scanedObjects; set => scanedObjects = value; }

    public int GetScanedCounter(string _Name) {
        int counter = -1,value;
        _Name = _Name.ToLower();
        if (ScanedObjects.TryGetValue(_Name, out value))
            counter = value;
        return counter;
    }
    public void SetObjects(StringIntDictionary _ScanedObjs) => ScanedObjects = _ScanedObjs;
    public void SetObject(string _Name, int _Conter) => ScanedObjects[_Name] = _Conter;
    public void ResetInventory()
    {
        ScanedObjects = new StringIntDictionary();
    }
}
