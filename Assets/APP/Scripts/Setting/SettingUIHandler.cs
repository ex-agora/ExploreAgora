using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUIHandler : MonoBehaviour
{
    [SerializeField] ProfileInfoContainer profile;
    [SerializeField] GameObject createAccountPanel;
    [SerializeField] GameObject manageAccountPanel;
    [SerializeField] GameObject loginBtn;
    [SerializeField] CheckBoxToggle sfxToggle;
    [SerializeField] CheckBoxToggle bgToggle;
    private void OnEnable()
    {
        if (profile.playerType == "registered") {
            manageAccountPanel.SetActive(true);
            createAccountPanel.SetActive(false);
            loginBtn.SetActive(false);
        } else if (profile.playerType == "dummy") {
            manageAccountPanel.SetActive(false);
            createAccountPanel.SetActive(true);
            loginBtn.SetActive(true);
        }
    }
    public void CheckMusic() {
        if (sfxToggle.IsActiveCheck && bgToggle.IsActiveCheck) { AudioManager.Instance?.AudioController(0); }
        else if (sfxToggle.IsActiveCheck && !bgToggle.IsActiveCheck) { AudioManager.Instance?.AudioController(1); }
        else if (!sfxToggle.IsActiveCheck && bgToggle.IsActiveCheck) { AudioManager.Instance?.AudioController(2); }
        else if (!sfxToggle.IsActiveCheck && !bgToggle.IsActiveCheck) { AudioManager.Instance?.AudioController(3); }
    }
    public void SetSoundSettings(bool _sfx, bool _bg) {
        sfxToggle.IsActiveCheck = _sfx;
        bgToggle.IsActiveCheck = _bg;
        sfxToggle.KeepValue = true;
        bgToggle.KeepValue = true;
        CheckMusic();
    }
}
