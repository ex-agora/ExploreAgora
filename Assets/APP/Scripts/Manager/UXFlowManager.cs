﻿using System.Collections;
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
    [SerializeField] private ProfileNetworkHandler _ProfileNetowrkHandler;
    [SerializeField] private ExperiencesStateHandler  _ExperiencesStates;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
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
            _ExperiencesStates.HandleExperiencesStates();
            //AcceptLogin();
            //quickFadeHandler.FadeIn();
        }
        else {
            quickFadeLoginHandler.FadeIn();
        }
    }
    public void FadeInProfile() {
        
        quickFadeProfileHandler.FadeIn();
    }
    public void FadeInProfileDellay(float delay)
    {
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