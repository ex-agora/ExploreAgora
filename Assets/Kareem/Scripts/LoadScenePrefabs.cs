using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public class LoadScenePrefabs : MonoBehaviour
{
    #region Fields
    [SerializeField] SOTest @sceneNavManager;
    #endregion Fields

    #region Methods
    public void back()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public ExperienceContainerHolder GetExperience() => sceneNavManager.nextExperienceContainerHolder;
    public string GetBundleID() => sceneNavManager.bundleID;
    private void Start()
    {

        // @sceneNavManager.nextExperienceContainerHolder.experiencePrefab.InstantiateAsync().Completed += LoadDone;
        var g = Resources.Load(@sceneNavManager.nextExperienceContainerHolder.experiencePrefab);
        Instantiate(g);

        Resources.UnloadUnusedAssets();
        System.GC.Collect();

    }


    void LoadDone(AsyncOperationHandle<GameObject> obj)
    {
        //uxPrefab = obj.Result;
        // Instantiate(uxPrefab);
        gameObject.SetActive(false);
        //Resources.UnloadAsset(prefabe);
        Resources.UnloadUnusedAssets();
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        Debug.Log("finish load asset");
    }
    #endregion Methods
}