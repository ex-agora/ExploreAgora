﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnBoardingFlowHandler : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] ScanProperties scanProperties;
    [SerializeField] Sprite objActiveSprite;
    [SerializeField] ExperienceContainerHolder ssEx;
    [SerializeField] ExperienceContainerHolder scEx;
    [SerializeField] ExperienceContainerHolder mEx;
    [SerializeField] ExperienceRouteHandler route;
    [SerializeField] ProfileInfoContainer profile;
    [SerializeField] InventoryObjectHolder inventory;
    [SerializeField] ProfilePictureHandler pictureHandler;
    public void StartScene()
    {
        sceneLoader.LoadExperience(sceneName);
    }
    public void StartScan()
    {
        NetworkManager.Instance.CheckInternetConnectivity(OnSuccessScan, OnFailedScan);
    }

    public void GoToSSEX() { route.Transit(ssEx, null,sceneLoader); }
    public void GoToScEX() { route.Transit(scEx, null, sceneLoader); }
    public void GoToMEX() { route.Transit(mEx, null, sceneLoader); }
    void OnSuccessScan(NetworkParameters np)
    {
        scanProperties.detectionObjectName = "book";
        scanProperties.detectionObjectSp = objActiveSprite;
        sceneLoader.LoadExperience("Scan Scene");
    }
    void OnFailedScan(NetworkParameters np)
    {
        Debug.Log(np.err.message);
    }

    public void ChangeComicToMap()
    {
        AppManager.Instance.boardingPhases = OnBoardingPhases.Map;
        AppManager.Instance.saveOnBoardingProgress();
    }
    public void ConfirmChange() {

        profile.profileImgIndex = pictureHandler.ChangeProfilePicture();
        UpdateProfile();
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
        if (profile.Achievements.Count > 0)
        {
            ss.achievementsData = new AchievementsData();
            ss.achievementsData.achievements = profile.Achievements;
        }
        NetworkManager.Instance.UpdateProfile(ss, OnUpdateProfileSuccess, OnUpdateProfileFailed);
    }
    private void OnUpdateProfileSuccess(NetworkParameters obj)
    {
    }
    private void OnUpdateProfileFailed(NetworkParameters obj)
    {
    }
}