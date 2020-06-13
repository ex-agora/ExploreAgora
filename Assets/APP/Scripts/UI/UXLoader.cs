using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.EventSystems;
public class UXLoader : MonoBehaviour
{
    public AssetReference prefabAdd;
    AsyncOperationHandle<GameObject> asyncOperation;
    //[SerializeField] GameObject uxPrefab;
    void Start()
    {
        Invoke(nameof(LoadedAsset), 0.2f);
    }
    void LoadedAsset()
    {
       
        prefabAdd.InstantiateAsync().Completed += LoadDone;
       // Instantiate(uxPrefab);
       // Resources.UnloadUnusedAssets();
        //.GC.Collect();
    }

    void LoadDone(AsyncOperationHandle<GameObject> obj)
    {
        //uxPrefab = obj.Result;
        //Instantiate(obj.Result);
        Resources.UnloadUnusedAssets();
        asyncOperation = obj;
        //Addressables.Release(obj);
        //prefabAdd.ReleaseAsset();
        GameObject.Destroy(new Object());
        System.GC.Collect();
        Debug.Log("finish load asset");
    }
    private void OnDestroy()
    {
        //if ((asyncOperation.Result != null))
        //    Addressables.Release(asyncOperation.Result);

       // Addressables.Release(asyncOperation);
        GameObject.Destroy(new Object());
        System.GC.Collect();
        Debug.Log("finish load asset");
    }

}