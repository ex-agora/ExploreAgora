using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WarningTransitionHandler : MonoBehaviour
{
    [SerializeField] private QuickFadeHandler quickFadeHandler;
    
    private void Start()
    {
        Resources.UnloadUnusedAssets();
        quickFadeHandler.FadeIn();
        Invoke(nameof(NextScene), 3f);
    }

    private void NextScene()
    {
        Application.backgroundLoadingPriority = ThreadPriority.High;
        AddressableScenesManager.Instance.LoadScene("UI-UX");
        quickFadeHandler.FadeOut();
        //SceneManager.LoadSceneAsync(1,LoadSceneMode.Single);

    }
   
}
