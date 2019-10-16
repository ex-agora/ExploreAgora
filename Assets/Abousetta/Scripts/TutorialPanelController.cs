using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TutorialPanelController : MonoBehaviour
{
    [SerializeField] Animator tutorialPanelAnimator;
    [SerializeField] Button okButton;

    /// Hide tutorial panel.
    /// Called in CloseTutorial function.
    void HideTutorial()
    {
        tutorialPanelAnimator.SetTrigger("IsClosed");
    }

    /// Open tutorial panel.
    /// Called in OpenTutorial function.
    void ShowTutorial()
    {
        tutorialPanelAnimator.SetTrigger("IsOpened");
    }

    /// Make Ok_Btn interactable.
    /// Called in ActiveOkButton function.
    void ActiveButton()
    {
        okButton.interactable = true;
    }

    /// Make Ok_Btn not interactable.
    /// Called in CloseTutorial and OpenTutorial function.
    void DisactiveButton()
    {
        okButton.interactable = false;
    }

    /// Called whenever need to close the tutorial and make Ok_Btn not interactable.
    public void CloseTutorial()
    {
        HideTutorial();
        DisactiveButton();
    }

    /// Called whenever need to open the tutorial and make Ok_Btn not interactable.
    public void OpenTutorial()
    {
        DisactiveButton();
        ShowTutorial();
    }

    /// Called whenever need to make Ok_Btn interactable.
    public void ActiveOkButton()
    {
        ActiveButton();
    }
}
