using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GetNextScenePrefab : MonoBehaviour
{
    [SerializeField] ScenesPrefabsIntializers @scenesPrefabsIntializers;
    [SerializeField] string SceneName;

    public void Test (GameObject NextScenePrefab)
    {
        SceneManager.LoadScene (SceneName);
        @scenesPrefabsIntializers.NextSceneRequiredPrefab = NextScenePrefab;
    }
}