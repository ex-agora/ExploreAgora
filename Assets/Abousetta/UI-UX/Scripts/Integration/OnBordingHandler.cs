using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnBordingHandler : MonoBehaviour
{
    [SerializeField] GameObject onbordingPanel;
    [SerializeField] StoryHandler story;
    [SerializeField] CheckBoxToggle termsBox;
    [SerializeField] CheckBoxToggle policyBox;
    [SerializeField] Button startBtn;
    [SerializeField] Sprite activeBtnSp;
    [SerializeField] Sprite unactiveBtnSp;
    bool isReadCondition;
    public void StartOnBording() {
        if (!isReadCondition)
            return;
        onbordingPanel.SetActive(true);
        story.gameObject.SetActive(true);
        story.StartStories();
    }
    public void CheckToggle() {
        isReadCondition = termsBox.IsActiveCheck & policyBox.IsActiveCheck;
        if (isReadCondition)
        {
            startBtn.image.sprite = activeBtnSp;
        }
        else {
            startBtn.image.sprite = unactiveBtnSp;
        }
    }
   
}
