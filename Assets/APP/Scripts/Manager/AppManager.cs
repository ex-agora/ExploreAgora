using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{

    public int currentBoardingIndex;
    public bool[] isCurrentLevelDone;
    public bool[] isCurrentLevelPrizeDone;
    public OnBoardingPhases boardingPhases;

    private static AppManager instance;
    private bool isSplashScreenDone;
 

    public static AppManager Instance { get => instance; set => instance = value; }
    public bool IsSplashScreenDone { get => isSplashScreenDone; set => isSplashScreenDone = value; }
 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        CheckForBoardingUpdates();
    }

    private void TestSceneRemove()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    void CheckForBoardingUpdates()
    {
        if (SaveLoadBoardingProgress.Load() != null)
        {
            currentBoardingIndex = SaveLoadBoardingProgress.Load().LevelIndex;
            isCurrentLevelDone = SaveLoadBoardingProgress.Load().LevelIndicators;
            isCurrentLevelPrizeDone = SaveLoadBoardingProgress.Load().LevelPrizeIndicators;
            boardingPhases = SaveLoadBoardingProgress.Load().boardingPhases;
        }
        else
        {
            currentBoardingIndex = 0;
            isCurrentLevelDone = new bool[4];
            isCurrentLevelPrizeDone = new bool[4];
            boardingPhases = OnBoardingPhases.Map;
            saveOnBoardingProgress();
        }
    }

    public void saveOnBoardingProgress()
    {
       SaveLoadBoardingProgress.Save(this);
    }
}