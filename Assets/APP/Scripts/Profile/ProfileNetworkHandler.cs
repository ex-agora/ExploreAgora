using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileNetworkHandler : MonoBehaviour
{
    [SerializeField] ProfileInfoContainer profile;
    [SerializeField] AccountProfileHandler profileHandler;
    [SerializeField] InventoryObjectHolder inventory;

    public ProfileInfoContainer Profile { get => profile; set => profile = value; }

    public void GetProfile() {
        if(NetworkManager.Instance.CheckTokenExist())
            NetworkManager.Instance.GetProfile(OnGetProfileSuccess, OnGetProfileFailed);
    }
    private void OnGetProfileSuccess(NetworkParameters obj) {
        GetProfileResponse response = (GetProfileResponse)obj.responseData;
        if (!ValidationInputUtility.IsEmptyOrNull(response.profile.country))
            Profile.country = response.profile.country;
        if (!ValidationInputUtility.IsEmptyOrNull(response.profile.email))
            Profile.email = response.profile.email;
        if (!ValidationInputUtility.IsEmptyOrNull(response.profile.firstName))
            Profile.fName = response.profile.firstName;
        if (!ValidationInputUtility.IsEmptyOrNull(response.profile.birthDate))
            Profile.DOB.dateTime = System.DateTime.Parse(response.profile.birthDate);
        if (response.profile.gender != null)
        {
            var str = response.profile.gender.ToCharArray();
            str[0] = char.ToUpper(response.profile.gender[0]);
            System.Enum.TryParse(new string(str), out Profile.gender);
        }  
        Profile.keys = response.profile.keys;
        if (!ValidationInputUtility.IsEmptyOrNull(response.profile.lastName))
            Profile.lName = response.profile.lastName;
        Profile.nickname = ValidationInputUtility.IsEmptyOrNull(response.profile.nickName) ? "Agoraien" : response.profile.nickName;
        Profile.points = response.profile.points;
        Profile.stones = (int)response.profile.powerStones;
        Profile.streaks = response.profile.dailyStreaks;
        Profile.isConfirmed = response.profile.isConfirmed;
        Profile.playerType = response.profile.playerType;
        if (!ValidationInputUtility.IsEmptyOrNull(response.profile.avatarId))
            Profile.profileImgIndex = int.Parse(response.profile.avatarId);
        StringIntDictionary _scannedObj = new StringIntDictionary();
        for (int i = 0; i < response.profile.scannedObjects.Count; i++)
        {
            _scannedObj.Add(response.profile.scannedObjects[i].name, response.profile.scannedObjects[i].counter);
        }
        inventory.SetObjects(_scannedObj);
        profileHandler.UpdateProfile();

        UXFlowManager.Instance.FadeInProfileDellay(2f);
    }
    private void OnGetProfileFailed(NetworkParameters obj) {
       
    }
    public bool ShouldVerify() => !Profile.isConfirmed && Profile.playerType == "registered";



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
        ss.firstName = profile.fName;
        ss.lastName = profile.lName;
        ss.nickName = profile.nickname;
        ss.country = profile.country;
        ss.birthDate = profile.DOB.dateTime.ToString();
        ss.avatarId = profile.profileImgIndex.ToString();
        ss.email = profile.email;
        ss.gender = profile.gender.ToString();
        ss.keys = profile.keys;
        ss.dailyStreaks = profile.streaks;
        ss.points = profile.points;
        ss.powerStones = profile.stones;
        if (profile.achievements.Count > 0)
        {
            ss.achievementsData = new AchievementsData();
            ss.achievementsData.achievements = profile.achievements;
        }
        NetworkManager.Instance.UpdateProfile(ss, OnUpdateProfileSuccess, OnUpdateProfileFailed);
    }
    private void OnUpdateProfileSuccess(NetworkParameters obj)
    {
        GetProfile();
    }
    private void OnUpdateProfileFailed(NetworkParameters obj)
    {
    }
}
