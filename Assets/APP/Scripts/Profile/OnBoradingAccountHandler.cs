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
    [SerializeField] Image missionInProfileImg;
    [SerializeField] Image bookProfileImg;
    void UpdateImgs() {
        Sprite profileSp = profilePicture.GetProfileSprite(profile.profileImgIndex);
        profileImg.sprite = profileSp;
        shopProfileImg.sprite = profileSp;
        missionProfileImg.sprite = profileSp;
        missionInProfileImg.sprite = profileSp;
        bookProfileImg.sprite = profileSp;
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
