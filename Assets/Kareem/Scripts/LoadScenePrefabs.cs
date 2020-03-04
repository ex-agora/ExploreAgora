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

    private void Start()
    {
        Instantiate (@sceneNavManager.nextExperienceContainerHolder.experiencePrefab);
        gameObject.SetActive (false);
    }
    #endregion Methods
}