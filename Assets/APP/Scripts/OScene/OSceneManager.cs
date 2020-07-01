using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSceneManager : MonoBehaviour
{
    [SerializeField] RealTimeTutorialHandler tutorialHandler;
    [SerializeField] GameObject noteTxt;
    [SerializeField] SpeechBubbleController bubbleController;
    [SerializeField] ToolBarHandler nextBtn;
    [SerializeField] ExperienceContainerHolder ssEX;
    [SerializeField] ExperienceRouteHandler routeHandler;
    void Start()
    {
        SceneLoader.Instance.HideLoading();
        Invoke(nameof(ShowHand), 2f);
        
    }
    void ShowHand() {
        tutorialHandler.OpenIndicator();
    }
    public void OggyTapped() {
        nextBtn.OpenToolBar();
        tutorialHandler.CloseIndicator();
        noteTxt.SetActive(false);
        Invoke(nameof(OpenBubble), 1f);
    }
    void OpenBubble() {
        bubbleController.OpenBubble();
    }
    public void RouteTap() {
        routeHandler.Transit(ssEX, null);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
