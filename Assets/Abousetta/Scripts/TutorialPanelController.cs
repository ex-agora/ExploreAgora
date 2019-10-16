using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanelController : MonoBehaviour
{
    [SerializeField] Animator tutorialPanelAnimator;
    [SerializeField] Button okButton;

    void HideTutorial()
    {
        tutorialPanelAnimator.SetTrigger("IsClosed");
    }
    void ShowTutorial()
    {
        tutorialPanelAnimator.SetTrigger("IsOpened");
    }

    void ActiveButton()
    {
        okButton.interactable = true;
    }
    void DisactiveButton()
    {
        okButton.interactable = false;
    }

    public void CloseTutorial()
    {
        HideTutorial();
        DisactiveButton();
    }
    public void OpenTutorial()
    {
        DisactiveButton();
        ShowTutorial();
    }
    public void ActiveOkButton()
    {
        ActiveButton();
    }

}
