using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

    #region Variables
    [Header("Game Loader")]
    public GameObject loadingScreen;
    public Slider slider;
    #endregion
    
    #region Main Methods
    // Use this for initialization
    void Start () {
        StartCoroutine(LoadAsynchronously());
    }	
	// Update is called once per frame
	void Update () {
		
	}  
    IEnumerator LoadAsynchronously ()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(("Game"));
        loadingScreen.gameObject.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            yield return null;
        }
    }
    #endregion
}
