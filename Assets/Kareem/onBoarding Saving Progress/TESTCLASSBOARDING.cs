using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TESTCLASSBOARDING : MonoBehaviour
{
   
    public void test(int currentind)
    {
        //int i =  AppManager.Instance.OnBoardingIndexIndicator();
        // stagesIndicator.StagesIndicators[i] = true;

        AppManager.Instance.isCurrentLevelDone[currentind] = true;
        AppManager.Instance.saveOnBoardingProgress();
        SceneManager.LoadSceneAsync(0);
    }
    public void testt()
    {
        AppManager.Instance.currentBoardingIndex = 1;
        AppManager.Instance.isCurrentLevelDone[0] = true;
        AppManager.Instance.isCurrentLevelPrizeDone[0] = true;
        AppManager.Instance.saveOnBoardingProgress();
        //stagesIndicator.StagesIndicators[0] = true;
        SceneManager.LoadSceneAsync(0);
    }
}
