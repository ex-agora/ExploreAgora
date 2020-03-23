using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckBoxToggle : MonoBehaviour
{
    [SerializeField] Sprite active;
    [SerializeField] Sprite unactive;
    [SerializeField] Button checkBtn;
    bool isActiveCheck;

    private void OnEnable()
    {
        isActiveCheck = false;
    }
    public bool IsActiveCheck { get => isActiveCheck; set => isActiveCheck = value; }
    public void ToggleCheckBox() {
        isActiveCheck = !isActiveCheck;
        if (isActiveCheck)
            checkBtn.image.sprite = active;
        else
            checkBtn.image.sprite = unactive;
    }
}
