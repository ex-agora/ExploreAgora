using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetProfileInformation : MonoBehaviour
{
    public void GetProfile()
    {
        NetworkManager.Instance.GetProfile(OnGetProfileSuccess, OnGetProfileFailed);
    }
    private void OnGetProfileSuccess(NetworkParameters obj)
    {
        GetProfileResponse getProfileResponse = (GetProfileResponse)obj.responseData;
        print(getProfileResponse.profile);
    }
    private void OnGetProfileFailed(NetworkParameters obj)
    {
        print(obj.err.message);
    }
}