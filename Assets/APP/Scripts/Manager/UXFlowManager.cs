using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UXFlowManager : MonoBehaviour
{
    public static UXFlowManager Instance;

    [SerializeField] private SplashScreenHandler splashScreenHandler;
    [SerializeField] private QuickFadeHandler quickFadeLoginHandler;
    [SerializeField] private QuickFadeHandler quickFadeProfileHandler;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private Canvas uIDefaultCanvas;
    [SerializeField] private Canvas onBoardingCanvas;
    [SerializeField] private GameObject loginRootPanel;
    [SerializeField] private GameObject footerPanel;
    [SerializeField] private GameObject conformationPanel;
    [SerializeField] private ExperienceRateHandler rateHandler;
    [SerializeField] private ProfileNetworkHandler _ProfileNetowrkHandler;
    [SerializeField] private ExperiencesStateHandler  _ExperiencesStates;
    [SerializeField] private FooterPanelHandler missionFooterHandler;
    [SerializeField] private FooterPanelManager footerManager;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    void FreeAssets() {
        Resources.UnloadUnusedAssets();
    }
    private void Start()
    {
        StartCoroutine(StartUX());
        FreeAssets();
    }
    IEnumerator StartUX() {
        yield return new WaitForSeconds(1.5f);
        uIDefaultCanvas.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        if (!AppManager.Instance.IsSplashScreenDone)
        {
            splashScreenHandler.PlayAnim();
            AppManager.Instance.IsSplashScreenDone = true;
        }
        else
            splashScreenHandler.DeactiveAnim();
    }
    public void LoginFadeIn()
    {
        if (NetworkManager.Instance.CheckTokenExist())
        {
            _ProfileNetowrkHandler.GetProfile();
            
            //AcceptLogin();
            //quickFadeHandler.FadeIn();
        }
        else {
            quickFadeLoginHandler.FadeIn();
        }
    }
          
    public void GetProfile() => _ProfileNetowrkHandler.GetProfile();
    public void FadeInProfile() {
        
        quickFadeProfileHandler.FadeIn();
        Invoke(nameof(CheckRate), 1f);
    }
    void CheckRate() {
        if (AppManager.Instance.IsThereRate) {
            rateHandler.ShowRate(AppManager.Instance.ExperienceCode);
            AppManager.Instance.IsThereRate = false;
            AppManager.Instance.ExperienceCode = string.Empty;
        }
    }
    
    public void FadeInProfileDellay(float delay)
    {
        if (_ProfileNetowrkHandler.ShouldVerify())
            ShowConformationPanel();
        _ExperiencesStates.gameObject.SetActive(true);
        _ExperiencesStates.HandleExperiencesStates();
        
        AcceptLogin();
        Invoke(nameof(FadeInProfile), delay);
    }
    public void CanvasChecker()
    {
        if(AppManager.Instance.boardingPhases != OnBoardingPhases.None)
        {
            onBoardingCanvas.gameObject.SetActive(true);
            uIDefaultCanvas.gameObject.SetActive(false);
        }
        else
        {
            onBoardingCanvas.gameObject.SetActive(false);
            uIDefaultCanvas.gameObject.SetActive(true);
            LoginFadeIn();
        }
    }
    public void FinishOnBoarding() {
        AppManager.Instance.DeleteBoardFile();
        onBoardingCanvas.gameObject.SetActive(false);
        uIDefaultCanvas.gameObject.SetActive(true);
        footerManager.ActivePanel(missionFooterHandler);
    }
    public void AcceptLogin() {
        loginRootPanel.SetActive(false);
        footerPanel.SetActive(true);
    }
    public void ShowConformationPanel() {
        conformationPanel.SetActive(true);
    }
    public void AcceptConformation() {
        conformationPanel.SetActive(false);
    }


}