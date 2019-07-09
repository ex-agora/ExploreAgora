using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenePrefabs : MonoBehaviour
{
    [SerializeField] ScenesPrefabsIntializers @scenesPrefabsIntializers;
    private void Awake ()
    {
        Instantiate (@scenesPrefabsIntializers.NextSceneRequiredPrefab);
    }
    public void back ()
    {
        SceneManager.LoadSceneAsync (0);
    }
}