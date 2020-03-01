using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance;
    private bool isSplashScreenDone;

    public bool IsSplashScreenDone { get => isSplashScreenDone; set => isSplashScreenDone = value; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void TestSceneRemove()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}