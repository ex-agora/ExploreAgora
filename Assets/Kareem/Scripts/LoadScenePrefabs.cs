using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

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
        GameObject prefabe = Resources.Load<GameObject>(@sceneNavManager.nextExperienceContainerHolder.experiencePrefab);
        Instantiate (prefabe);
        gameObject.SetActive (false);
        //Resources.UnloadAsset(prefabe);
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }
    #endregion Methods
}