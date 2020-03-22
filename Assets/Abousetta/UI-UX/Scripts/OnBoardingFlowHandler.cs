using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnBoardingFlowHandler : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private SceneLoader sceneLoader;

    public void StartScene()
    {
        sceneLoader.LoadExperience(sceneName);
    }



}