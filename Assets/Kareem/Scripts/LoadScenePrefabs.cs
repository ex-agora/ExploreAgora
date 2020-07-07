using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public class LoadScenePrefabs : MonoBehaviour
{
    #region Fields
    // [SerializeField] SOTest @sceneNavManager;
    AsyncOperationHandle<GameObject> asyncOperation;
    #endregion Fields

    #region Methods
    public void back()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public ExperienceContainerHolder GetExperience() => ExperienceTransitionHolder.Instance.NextExperienceContainerHolder;
    public string GetBundleID() => ExperienceTransitionHolder.Instance.BundleID;
    private IEnumerator Start()
    {
        //Debug.Log(@sceneNavManager.nextExperienceContainerHolder.experiencePrefab);
        //if (@sceneNavManager == null)
        //{
        //    Debug.Log("SMV");
        //    return;
        //}
        //if (@sceneNavManager.nextExperienceContainerHolder == null)
        //{
        //    Debug.Log("Ex");
        //    return;
        //}
        //if (@sceneNavManager.nextExperienceContainerHolder.experiencePrefab == null)
        //{
        //    Debug.Log("Pre");
        //    return;
        //}
        SceneLoader.Instance.Loading(true);
        Resources.UnloadUnusedAssets();
        GameObject.Destroy(new Object());
        System.GC.Collect();
        yield return new WaitForEndOfFrame();
        //ExperienceTransitionHolder.Instance.NextExperienceContainerHolder.experiencePrefab.InstantiateAsync().Completed += LoadDone;
        asyncOperation = Addressables.LoadAssetAsync<GameObject>(ExperienceTransitionHolder.Instance.NextExperienceContainerHolder.experiencePrefabPath);
        asyncOperation.Completed += LoadDone;
        //StartCoroutine(updateLoading());
        //var g = Resources.Load(@sceneNavManager.nextExperienceContainerHolder.experiencePrefab);
        //Instantiate(g);

        //Resources.UnloadUnusedAssets();
        //System.GC.Collect();

    }
    IEnumerator updateLoading()
    {
        while (!asyncOperation.IsDone)
        {
            SceneLoader.Instance.UpdatePrecent(asyncOperation.PercentComplete);
            yield return null;
        }
    }

    void LoadDone(AsyncOperationHandle<GameObject> obj)
    {
        //uxPrefab = obj.Result;
        Debug.Log("finish load asset");
        Instantiate(obj.Result);
        SceneLoader.Instance.HideLoading();
        //gameObject.SetActive(false);
        //Resources.UnloadAsset(prefabe);
        //Addressables.Release(obj);
        //ExperienceTransitionHolder.Instance.NextExperienceContainerHolder.experiencePrefab.ReleaseAsset();
        asyncOperation = obj;
        Resources.UnloadUnusedAssets();
        GameObject.Destroy(new Object());
        System.GC.Collect();
    }
    private void OnDestroy()
    {
        //Addressables.Release(asyncOperation.Result);
        //Resources.UnloadUnusedAssets();
        GameObject.Destroy(new Object());
        System.GC.Collect();
    }

    #endregion Methods
}