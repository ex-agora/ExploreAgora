using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UXFlowManager : MonoBehaviour
{
    public static UXFlowManager Instance;
    [SerializeField] private QuickFadeHandler quickFadeLoginHandler;
    [SerializeField] private QuickFadeHandler quickFadeProfileHandler;
    [SerializeField] private Canvas uIDefaultCanvas;
    [SerializeField] private Canvas onBoardingCanvas;
    [SerializeField] private GameObject loginRootPanel;
    [SerializeField] private GameObject footerPanel;
    [SerializeField] private SignupVerificationCode conformationPanel;
    [SerializeField] private ExperienceRateHandler rateHandler;
    [SerializeField] private ProfileNetworkHandler _ProfileNetowrkHandler;
    [SerializeField] private ExperiencesStateHandler  _ExperiencesStates;
    [SerializeField] private FooterPanelHandler missionFooterHandler;
    [SerializeField] private FooterPanelManager footerManager;
    [SerializeField] private SettingUIHandler setting;
    [SerializeField] private BundleNavUIHandler bundleNavUI;
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
        setting.SetSoundSettings(true, true);
    }
    IEnumerator StartUX() {
        yield return new WaitForSeconds(1.5f);
        uIDefaultCanvas.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        CanvasChecker();
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
            AudioManager.Instance.Play("appBG", "Background");
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
        LoginFadeIn();
        Invoke(nameof(ChangeFooter), 1.1f);
    }
    void ChangeFooter() {
        footerManager.transform.parent.gameObject.SetActive(true);
        footerManager.ActivePanel(missionFooterHandler);
    }
    public void AcceptLogin() {
        loginRootPanel.SetActive(false);
        footerPanel.SetActive(true);
        if (AppManager.Instance.BundleNum > -1) {
            ChangeFooter();
            bundleNavUI.OpenCurrentBundle(AppManager.Instance.BundleNum);
        }
        if (!AudioManager.Instance.IsPlay("appBG", "Background"))
            AudioManager.Instance.Play("appBG", "Background");
    }
    public void ShowConformationPanel(bool _isSignUp=false) {
        conformationPanel.Open(_isSignUp);
    }
    public void AcceptConformation() {
        conformationPanel.gameObject.SetActive(false);
    }
    public void TapSound() {
        AudioManager.Instance?.Play("UIAction", "UI");
    }
    public void SetCurrentBundle(int _current) => AppManager.Instance.BundleNum = _current;

    void NOInternet() {
        InternetPopupHandler.Instance.OpenPopup();
    }

    void UnauthorizedAttack() {
        NetworkManager.Instance.DeleteToken();
        AppManager.Instance.DeleteBoardFile();
        Application.Quit(-1);
    }

    public bool IsThereNetworkError(NetworkErrorTypes errorTypes) {
        switch (errorTypes)
        {
            case NetworkErrorTypes.None:
                return false;
            case NetworkErrorTypes.AuthenticationError:
               UnauthorizedAttack();
                return true;
            case NetworkErrorTypes.NetworkError:
              NOInternet();
                return true;
            case NetworkErrorTypes.HttpError:
              NOInternet();
                return true;
        }
        return false;
    }
}