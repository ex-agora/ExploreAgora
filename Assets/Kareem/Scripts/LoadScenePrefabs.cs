using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenePrefabs : MonoBehaviour
{
    #region Fields
    [SerializeField] ScenesPrefabsIntializers @scenesPrefabsIntializers;
    #endregion Fields

    #region Methods
    public void back()
    {
        SceneManager.LoadSceneAsync(0);
    }

    private void Start()
    {
        Instantiate (@scenesPrefabsIntializers.nextExperienceContainerHolder.experiencePrefab);
    }
    #endregion Methods
}