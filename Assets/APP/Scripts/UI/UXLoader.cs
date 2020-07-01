using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.EventSystems;
public class UXLoader : MonoBehaviour
{
    public AssetReference prefabAdd;
    public string prefabPath;
    AsyncOperationHandle<GameObject> asyncOperation;
    //[SerializeField] GameObject uxPrefab;
    void Start()
    {
        SceneLoader.Instance.Loading(true);
        Invoke(nameof(LoadedAsset), 0.2f);
    }
    void LoadedAsset()
    {

        //prefabAdd.InstantiateAsync().Completed += LoadDone;
        asyncOperation = Addressables.LoadAssetAsync<GameObject>(prefabPath);
        asyncOperation.Completed += LoadDone;
        //Addressables.LoadAssetAsync<GameObject>(prefabPath).Completed += LoadDone;
       
        //StartCoroutine(updateLoading());

        // Instantiate(uxPrefab);
        // Resources.Unlo.dUnusedAssets();
        //.GC.Collect();
    }
    IEnumerator updateLoading() {
        while (!asyncOperation.IsDone)
        {
            SceneLoader.Instance.UpdatePrecent(asyncOperation.PercentComplete);
            yield return null;
        }
    }
    void LoadDone(AsyncOperationHandle<GameObject> obj)
    {
        //uxPrefab = obj.Result;
        //if (obj.IsDone)
        Instantiate(obj.Result);
        SceneLoader.Instance.HideLoading();
        Resources.UnloadUnusedAssets();
        //asyncOperation = obj;
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