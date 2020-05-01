using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditeProfileHandler : MonoBehaviour
{
    [SerializeField] DOBHandler dOB;
    [SerializeField] GenderHandler gender;
    [SerializeField] InputField nickname;
    [SerializeField] Image _profile;
    [SerializeField] ProfilePictureHandler pictureHandler;
    [SerializeField] AccountProfileHandler account;
    [SerializeField] InventoryObjectHolder inventory;
    [SerializeField] ProfileNetworkHandler profileNetwork;
    int profileIndex;
    public void ConfirmChanges()
    {
        account.ProfileInfo.DOB = dOB.DOB;
        account.ProfileInfo.gender = gender.PlayerGender;
        account.ProfileInfo.nickname = nickname.text;
        account.ProfileInfo.profileImgIndex = pictureHandler.ChangeProfilePicture();
        account.ConfirmChangeProfilePicture();
        UpdateProfile();
    }
    public void SetInfo()
    {
        dOB.DOB = account.ProfileInfo.DOB;
        gender.PlayerGender = account.ProfileInfo.gender;
        nickname.text = account.ProfileInfo.nickname;
        profileIndex = account.ProfileInfo.profileImgIndex;

        _profile.sprite = pictureHandler.GetProfileSprite(profileIndex);
        pictureHandler.SetActiveFrameIndex(profileIndex);
    }
    public void ConfirmImg()
    {
        profileIndex = pictureHandler.ChangeProfilePicture();
        _profile.sprite = pictureHandler.GetProfileSprite(profileIndex);
    }

    public void UpdateProfile()
    {

        ProfileData ss = new ProfileData();
        if (inventory.ScanedObjects.Count > 0)
        {
            ss.scannedObjects = new ScannedObjects();
            ss.scannedObjects.scannedObjects = new List<ScannedObject>();
            foreach (var i in inventory.ScanedObjects)
            {
                ss.scannedObjects.scannedObjects.Add(new ScannedObject(i.Key, i.Value));
            }
        }
        ss.firstName = account.ProfileInfo.fName;
        ss.lastName = account.ProfileInfo.lName;
        ss.nickName = account.ProfileInfo.nickname;
        ss.country = account.ProfileInfo.country;
        ss.birthDate = account.ProfileInfo.DOB.dateTime.ToString();
        ss.avatarId = account.ProfileInfo.profileImgIndex.ToString();
        ss.email = account.ProfileInfo.email;
        ss.gender = account.ProfileInfo.gender.ToString();
        ss.keys = account.ProfileInfo.keys;
        ss.dailyStreaks = account.ProfileInfo.streaks;
        ss.points = account.ProfileInfo.points;
        ss.powerStones = account.ProfileInfo.stones;
        if (account.ProfileInfo.Achievements.Count > 0)
        {
            ss.achievementsData = new AchievementsData();
            ss.achievementsData.achievements = account.ProfileInfo.Achievements;
        }
        NetworkManager.Instance.UpdateProfile(ss, OnUpdateProfileSuccess, OnUpdateProfileFailed);
    }
    private void OnUpdateProfileSuccess(NetworkParameters obj)
    {
        profileNetwork.GetProfile();
    }
    private void OnUpdateProfileFailed(NetworkParameters obj)
    {
        if (UXFlowManager.Instance.IsThereNetworkError(obj.err.errorTypes))
            return;
    }
}
