using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.EventSystems;
public class UXLoader : MonoBehaviour
{
    public AssetReference prefabAdd;
    [SerializeField] GameObject uxPrefab;
    void Start()
    {
        Invoke(nameof(LoadedAsset), 1f);
    }
    void LoadedAsset()
    {
        prefabAdd.InstantiateAsync().Completed += LoadDone;
    }

    void LoadDone(AsyncOperationHandle<GameObject> obj)
    {
       //uxPrefab = obj.Result;
       // Instantiate(uxPrefab);
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        Debug.Log("finish load asset");
    }

}
