using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JoinUsBehavior : MonoBehaviour
{
    [SerializeField] ProfileInfoContainer profileInfoContainer;
    [SerializeField] UnityEvent onSuccess , onFailed;

   public void UpdateProfile()
    {
        ProfileData profileData = new ProfileData();
        profileData.nickName = profileInfoContainer.nickname;
        profileData.gender = profileInfoContainer.gender.ToString();
        profileInfoContainer.DOB = profileInfoContainer.DOB;
        profileInfoContainer.profileImgIndex = profileInfoContainer.profileImgIndex;

        NetworkManager.Instance.UpdateProfile(profileData, OnUpdateProfileSuccess, OnUpdateProfileFailed);
    }
    private void OnUpdateProfileSuccess(NetworkParameters obj)
    {
        //flow
        onSuccess.Invoke();
    }
    private void OnUpdateProfileFailed(NetworkParameters obj)
    {
        //ShowErrors (obj.err.message);
        onFailed.Invoke();
    }
}
