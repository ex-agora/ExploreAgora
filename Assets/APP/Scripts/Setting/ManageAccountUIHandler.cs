using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageAccountUIHandler : MonoBehaviour
{
    [SerializeField] ProfileInfoContainer profile;
    [SerializeField] Text fName;
    [SerializeField] Text lName;
    [SerializeField] Text email;
    private void OnEnable()
    {
        fName.text = profile.fName;
        lName.text = profile.lName;
        email.text = profile.email;
    }
    public void Logout()
    {
        NetworkManager.Instance.DeleteToken();
        //DailyStrikesHandler.Instance.DeleteTimestamp();
        AddressableScenesManager.Instance.ReloadScene();
    }
    
}
