using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileNetworkHandler : MonoBehaviour
{
    [SerializeField] ProfileInfoContainer profile;
    [SerializeField] AccountProfileHandler profileHandler;
    public void GetProfile() {
        if(NetworkManager.Instance.CheckTokenExist())
            NetworkManager.Instance.GetProfile(OnGetProfileSuccess, OnGetProfileFailed);
    }
    private void OnGetProfileSuccess(NetworkParameters obj) {
        GetProfileResponse response = (GetProfileResponse)obj.responseData;
        profile.country = response.profile.country;
        profile.email = response.profile.email;
        profile.fName = response.profile.firstName;
        profile.DOB.dateTime = System.DateTime.Parse(response.profile.birthDate);
        profile.gender = (Gender)System.Enum.Parse(typeof(Gender), response.profile.gender);
        profile.keys = response.profile.keys;
        profile.lName = response.profile.lastName;
        profile.nickname = response.profile.nickName == string.Empty ? "User" : response.profile.nickName;
        profile.points = int.Parse(response.profile.points);
        profile.stones = (int)response.profile.powerStones;
        profile.streaks = response.profile.streaks;
        profile.profileImgIndex = int.Parse(response.profile.avatarId);
        profileHandler.UpdateProfile();
        UXFlowManager.Instance.FadeInProfile();
    }
    private void OnGetProfileFailed(NetworkParameters obj) {
       
    }
}
