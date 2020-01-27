using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LabelWorldHandler : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] TMP_Text labelText;
    [SerializeField] bool hasExtraInfo;
    [SerializeField] string extraInfoText;
    [SerializeField] Canvas canvas;
    [SerializeField] bool isTabDisabled;
    bool isOpen;
    public string LabelTextStr { get => labelText.text; }
    public bool HasExtraInfo { get => hasExtraInfo; }
    public string ExtraInfoText { get => extraInfoText; }
    private void Start()
    {
        if(!isTabDisabled)
        canvas.worldCamera = interactions.Instance.SessionOrigin.camera;
    }
    // Start is called before the first frame update
    private void OnEnable ()
    {
        SetlabelAnimState();
    }
    public void EnableTap() {
        isTabDisabled = false;
        canvas.worldCamera = interactions.Instance.SessionOrigin.camera;
    }
 
    private void SetlabelAnimState ()
    {
        if (anim != null)
            anim?.SetBool("isOpen", isOpen);

    }
    public void ShowLabel ()
    {
        isOpen = true;
        SetlabelAnimState();
    }
    public void HidaLabel() {
        isOpen = false;
        SetlabelAnimState();
    }
    public void PrepaireQuiz() {
        labelText.gameObject.SetActive(false);
        ShowLabel();
    }
    public void RightAnswer()
    {
        labelText.gameObject.SetActive(true);
    }
}
