using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileNetworkHandler : MonoBehaviour
{
    [SerializeField] ProfileInfoContainer profile;
    [SerializeField] AccountProfileHandler profileHandler;
    [SerializeField] InventoryObjectHolder inventory;
    public void GetProfile() {
        if(NetworkManager.Instance.CheckTokenExist())
            NetworkManager.Instance.GetProfile(OnGetProfileSuccess, OnGetProfileFailed);
    }
    private void OnGetProfileSuccess(NetworkParameters obj) {
        GetProfileResponse response = (GetProfileResponse)obj.responseData;
        if (!ValidationInputUtility.IsEmptyOrNull(response.profile.country))
            profile.country = response.profile.country;
        if (!ValidationInputUtility.IsEmptyOrNull(response.profile.email))
            profile.email = response.profile.email;
        if (!ValidationInputUtility.IsEmptyOrNull(response.profile.firstName))
            profile.fName = response.profile.firstName;
        if (!ValidationInputUtility.IsEmptyOrNull(response.profile.birthDate))
            profile.DOB.dateTime = System.DateTime.Parse(response.profile.birthDate);
        if (response.profile.gender != null)
        {
            var str = response.profile.gender.ToCharArray();
            str[0] = char.ToUpper(response.profile.gender[0]);
            System.Enum.TryParse(new string(str), out profile.gender);
        }  
        profile.keys = response.profile.keys;
        if (!ValidationInputUtility.IsEmptyOrNull(response.profile.lastName))
            profile.lName = response.profile.lastName;
        profile.nickname = !ValidationInputUtility.IsEmptyOrNull(response.profile.nickName) ? "Agoraien" : response.profile.nickName;
        profile.points = response.profile.points;
        profile.stones = (int)response.profile.powerStones;
        profile.streaks = response.profile.dailyStreaks;
        profile.isConfirmed = response.profile.isConfirmed;
        profile.playerType = response.profile.playerType;
        if (!ValidationInputUtility.IsEmptyOrNull(response.profile.avatarId))
            profile.profileImgIndex = int.Parse(response.profile.avatarId);
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
}
