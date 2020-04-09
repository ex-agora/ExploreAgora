using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckBoxToggle : MonoBehaviour
{
    [SerializeField] Sprite active;
    [SerializeField] Sprite unactive;
    [SerializeField] Image checkImg;
    bool keepValue =false;
    bool isActiveCheck;

    private void OnEnable()
    {
        if (!keepValue)
            isActiveCheck = false;
        HandleCheckImg();
    }
    public bool IsActiveCheck { get => isActiveCheck; set => isActiveCheck = value; }
    public bool KeepValue { get => keepValue; set => keepValue = value; }

    public void ToggleCheckBox() {
        isActiveCheck = !isActiveCheck;
        HandleCheckImg();
    }
    void HandleCheckImg() {
        if (isActiveCheck)
            checkImg.sprite = active;
        else
            checkImg.sprite = unactive;
    }
}
