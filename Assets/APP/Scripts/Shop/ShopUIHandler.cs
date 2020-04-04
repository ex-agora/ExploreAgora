using SIS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUIHandler : MonoBehaviour
{
    [SerializeField] ProfileInfoContainer profile;
    [SerializeField] GameObject noteText;
    [SerializeField] List<IAPItem> items;
    [SerializeField] AdultVerification verification;
    private void OnEnable()
    {
        if (profile.playerType == "registered" && profile.isConfirmed)
            noteText.SetActive(false);
    }
    public void Buy(int index) {
        if (profile.playerType == "registered" && profile.isConfirmed) {
            verification.ShowPopup(items[index]);
        }
    }

}
