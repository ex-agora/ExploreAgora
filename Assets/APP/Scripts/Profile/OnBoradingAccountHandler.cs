using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnBoradingAccountHandler : MonoBehaviour
{
    [SerializeField] ProfileInfoContainer profile;
    [SerializeField] ProfilePictureHandler profilePicture;
    [SerializeField] Text nicknameTxt;
    [SerializeField] Image profileImg;
    [SerializeField] Image shopProfileImg;
    [SerializeField] Image missionProfileImg;
    void UpdateImgs() {
        Sprite profileSp = profilePicture.GetProfileSprite(profile.profileImgIndex);
        profileImg.sprite = profileSp;
        shopProfileImg.sprite = profileSp;
        missionProfileImg.sprite = profileSp;
    }
    private void OnEnable()
    {
        UpdateProfile();
    }
    public void UpdateProfile() {
        UpdateImgs();
        nicknameTxt.text = profile.nickname;
    }
}
