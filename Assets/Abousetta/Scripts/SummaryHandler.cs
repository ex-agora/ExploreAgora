using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryHandler : MonoBehaviour
{
    [SerializeField] Text tittleText;
    [SerializeField] string tittleString;
    [SerializeField] Animator bubbleAnimator;
    [SerializeField] Image contentImage;
    [SerializeField] Sprite contentSprite;
    [SerializeField] GameEvent doneEvent;
    public string TittleString { get => tittleString; set { tittleString = value; HandleTittle(); } }

    public Sprite ContentSprite { get => contentSprite; set { contentSprite = value; HandleContent(); } }

    /// Set bubble summary tittle
    /// Called in ViewSummary function.
    void HandleTittle()
    {
        tittleText.text = TittleString;
    }

    /// Set bubble summary content
    /// Called in ViewSummary function.
    void HandleContent()
    {
        contentImage.sprite = contentSprite;
        contentImage.SetNativeSize();
    }

    /// Open summary panel.
    void OpenSummary()
    {
        bubbleAnimator.SetTrigger("IsShown");
    }

    /// Close summary panel.
    void CloseSummary()
    {
        bubbleAnimator.SetTrigger("IsClosed");
    }

    /// Set bubble summary tittle
    /// Set bubble summary content
    /// Called whenever needed to open Summary.
    public void ViewSummary()
    {
        HandleTittle();
        HandleContent();
        OpenSummary();
    }

    /// Called whenever needed to close summary panel.
    public void ConfirmationAction()
    {
        CloseSummary();
        doneEvent.Raise();
    }
}
