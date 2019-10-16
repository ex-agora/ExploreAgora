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

    public string TittleString { get => tittleString; set { tittleString = value; HandleTittle(); } }

    public Sprite ContentSprite { get => contentSprite; set { contentSprite = value; HandleContent(); } }

    void HandleTittle()
    {
        tittleText.text = TittleString;
    }

    void HandleContent()
    {
        contentImage.sprite = contentSprite;
        contentImage.SetNativeSize();
    }

    void OpenSummary()
    {
        bubbleAnimator.SetTrigger("IsShown");
    }

    void CloseSummary()
    {
        bubbleAnimator.SetTrigger("IsClosed");
    }

    public void ViewSummary()
    {
        HandleTittle();
        HandleContent();
        OpenSummary();
    }

    public void ConfirmationAction()
    {
        CloseSummary();
    }
}
