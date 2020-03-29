using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarningTransitionHandler : MonoBehaviour
{
    [SerializeField] private QuickFadeHandler quickFadeHandler;

    private void Start()
    {
        quickFadeHandler.FadeIn();
        Invoke(nameof(NextScene), 3f);
    }

    private void NextScene()
    {
        quickFadeHandler.FadeOut();
        SceneManager.LoadScene(1);
    }
}
