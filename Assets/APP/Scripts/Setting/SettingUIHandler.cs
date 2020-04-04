using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUIHandler : MonoBehaviour
{
    [SerializeField] ProfileInfoContainer profile;
    [SerializeField] GameObject createAccountPanel;
    [SerializeField] GameObject manageAccountPanel;
    [SerializeField] CheckBoxToggle sfxToggle;
    [SerializeField] CheckBoxToggle bgToggle;
    private void OnEnable()
    {
        if (profile.playerType == "registered") {
            manageAccountPanel.SetActive(true);
            createAccountPanel.SetActive(false);
        } else if (profile.playerType == "dummy") {
            manageAccountPanel.SetActive(false);
            createAccountPanel.SetActive(true);
        }
    }
    public void CheckMusic() { }
    
}
