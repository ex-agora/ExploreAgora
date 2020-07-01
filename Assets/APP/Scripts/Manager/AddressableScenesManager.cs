using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
public class AddressableScenesManager : MonoBehaviour
{
    [SerializeField] List<AssetReference> scenesRefList;
    [SerializeField] List<string> scenesNameList;
    Dictionary<string, AssetReference> scenes;
    static AddressableScenesManager instance;
    SceneInstance currentScene = default;
    public static AddressableScenesManager Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        scenes = new Dictionary<string, AssetReference>();
        if (scenesRefList.Count == scenesNameList.Count) {
            int size = scenesRefList.Count;
            for (int i = 0; i < size; i++)
            {
                scenes.Add(scenesNameList[i], scenesRefList[i]);
            }
        }
    }
    public void ReloadScene()
    {
        if (currentScene.Equals(default))
            return;
        AssetReference sceneRef = null;
        if (scenes.TryGetValue(currentScene.Scene.name, out sceneRef))
        {
            //if (!currentScene.Equals(default))
            //    Addressables.UnloadSceneAsync(currentScene);
            Addressables.LoadSceneAsync(sceneRef,priority:10000).Completed += LoadDone;
        }
        else
        {
            // Wrong Scene name 
        }
    }
    public void LoadScene(string sceneName) {
        if (ValidationInputUtility.IsEmptyOrNull(sceneName))
            return;
        AssetReference sceneRef = null;
        if (scenes.TryGetValue(sceneName, out sceneRef))
        {
            //if (!currentScene.Equals(default))
            //    Addressables.UnloadSceneAsync(currentScene);
            Addressables.LoadSceneAsync(sceneRef).Completed += LoadDone;
        }
        else {
            // Wrong Scene name 
        }
    }
    void LoadDone(AsyncOperationHandle<SceneInstance> obj)
    {
        currentScene = obj.Result;
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        //Debug.Log("finish load asset");
    }
}
