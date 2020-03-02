using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditeProfileHandler : MonoBehaviour
{
    [SerializeField] DOBHandler dOB;
    [SerializeField] GenderHandler gender;
    [SerializeField] InputField nickname;
    [SerializeField] Image profile;
    [SerializeField] ProfilePictureHandler pictureHandler;
    [SerializeField] AccountProfileHandler account;
    int profileIndex;
    public void ConfirmChanges() {
        account.ProfileInfo.DOB = dOB.DOB;
        account.ProfileInfo.gender = gender.PlayerGender;
        account.ProfileInfo.nickname = nickname.text;
        account.ProfileInfo.profileImgIndex = pictureHandler.ChangeProfilePicture();
        account.ConfirmChangeProfilePicture();
    }
    public void SetInfo() {
        dOB.DOB = account.ProfileInfo.DOB;
        gender.PlayerGender = account.ProfileInfo.gender;
        nickname.text = account.ProfileInfo.nickname;
        profileIndex = account.ProfileInfo.profileImgIndex;
        profile.sprite = pictureHandler.GetProfileSprite(profileIndex);
        pictureHandler.SetActiveFrameIndex(profileIndex);
    }
    public void ConfirmImg() {
        profileIndex = pictureHandler.ChangeProfilePicture();
        profile.sprite = pictureHandler.GetProfileSprite(profileIndex);
    }
}
