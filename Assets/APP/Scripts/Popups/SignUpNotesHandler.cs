using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignUpNotesHandler : MonoBehaviour
{
    [SerializeField] ToolBarHandler reminderPopup;
    [SerializeField] ProfileInfoContainer profile;
    [SerializeField] Button settingsBtn;
    [SerializeField] List<Text> notes;
    private void DisableNotes() {
        for (int i = 0; i < notes.Count; i++)
        {
            notes[i].gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        if (profile.playerType == "registered" && profile.isConfirmed)
        {
            DisableNotes();
        }
    }
    public void RemindSignup() {
        if (profile.playerType != "registered" || !profile.isConfirmed) {
            reminderPopup.OpenToolBar();
        }
    }
    public void GoToSignupPage() {
        settingsBtn.onClick.Invoke();
    }
}
