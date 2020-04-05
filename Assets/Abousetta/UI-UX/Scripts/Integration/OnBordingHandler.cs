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
    [SerializeField] ProfileNetworkHandler networkHandler;
    bool isReadCondition;
    bool isPressed;
    public void StartOnBording() {
        if (!isReadCondition)
            return;
        if (isPressed)
            return;
        isPressed = true;
        CreateDummyAccount();
        
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
    public void CreateDummyAccount()
    {
        
        CreateDummyAccountData createDummyAccountData = new CreateDummyAccountData();
        createDummyAccountData.deviceId = SystemInfo.deviceUniqueIdentifier;
        createDummyAccountData.deviceType = "Android";//Application.platform.ToString() ;
        NetworkManager.Instance.CreateDummyAccount(createDummyAccountData, OnCreateDummyAccountSusccess, OnCreateDummyAccountFailed);
    }
    private void OnCreateDummyAccountSusccess(NetworkParameters obj)
    {
        CreateDummyAccountResponse response = (CreateDummyAccountResponse)obj.responseData;
        NetworkManager.Instance.SaveToken(response.token);
        networkHandler.GetProfile();
        onbordingPanel.SetActive(true);
        story.gameObject.SetActive(true);
        story.StartStories();
        isPressed = false;
    }
    private void OnCreateDummyAccountFailed(NetworkParameters obj)
    {
        isPressed = false;
        print(obj.err.message);
    }
}
