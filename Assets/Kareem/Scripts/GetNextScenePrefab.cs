using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
public class GetNextScenePrefab : MonoBehaviour
{
    #region Fields
    [SerializeField] ScenesPrefabsIntializers @scenesPrefabsIntializers;
    [SerializeField] string SceneName;
    #endregion Fields

    #region Methods
    public void Test (GameObject NextScenePrefab)
    {
    
        SceneManager.LoadScene (SceneName);
        @scenesPrefabsIntializers.NextSceneRequiredPrefab = NextScenePrefab;
    }
    #endregion Methods
}