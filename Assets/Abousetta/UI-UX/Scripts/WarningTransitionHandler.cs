using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WarningTransitionHandler : MonoBehaviour
{
    [SerializeField] private QuickFadeHandler quickFadeHandler;
    [SerializeField] private QuickFadeHandler rootFadehandler;
    private void Start()
    {
        rootFadehandler.FadeIn();
        Invoke(nameof(ShowSplashScreen), 0.5f);  
    }

    private void ShowSplashScreen() {
        SplashScreenHandler.Instance.PlayAnim();
    }
    public void ShowWarning() {
        Resources.UnloadUnusedAssets();
        Invoke(nameof(OpenWarning), 1f);
        Invoke(nameof(HideWarning), 4f);
    }
    private void OpenWarning() => quickFadeHandler.FadeIn();
    private void HideWarning() {
        quickFadeHandler.FadeOut();
        Invoke(nameof(NextScene), 1f);
    }
    private void NextScene()
    {
        Application.backgroundLoadingPriority = ThreadPriority.High;
        AddressableScenesManager.Instance.LoadScene("UI-UX");
        quickFadeHandler.FadeOut();
        //SceneManager.LoadSceneAsync(1,LoadSceneMode.Single);

    }
   
}
