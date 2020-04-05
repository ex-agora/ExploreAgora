using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishExperiencesHandler : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] LoadScenePrefabs scenePrefabs;
    [SerializeField] AchievementHolder achievementSc;
    [SerializeField] AchievementHolder achievementSS;
    [SerializeField] AchievementHolder achievementMath;
    [SerializeField] ProfileInfoContainer profile;
    [SerializeField] InventoryObjectHolder inventory;
    [SerializeField] ExperienceTokenHandler tokenHandler;
    static FinishExperiencesHandler instance;
    bool isPressed = false;

    public static FinishExperiencesHandler Instance { get => instance; set => instance = value; }
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void FinshExperience(int score) {
        if (scenePrefabs.GetExperience().experienceRate == 0) {
            AppManager.Instance.IsThereRate = true;
            AppManager.Instance.ExperienceCode = scenePrefabs.GetExperience().experienceCode;
        }
        if (scenePrefabs.GetExperience().hasToken) {
            tokenHandler.UpdateBundleToken(scenePrefabs.GetExperience().token.tokenName, scenePrefabs.GetBundleID());
        }
        if (AppManager.Instance.boardingPhases != OnBoardingPhases.None) {
            int currentind = 0;
            switch (scenePrefabs.GetExperience().experienceCode) {
                case "0SS": currentind = 1; break;
                case "0Sc": currentind = 2; break;
                case "0M": currentind = 3; break;
            }
            AppManager.Instance.isCurrentLevelDone[currentind] = true;
            AppManager.Instance.saveOnBoardingProgress();
        }
        AchievementManager.Instance.AddScore((uint) (ScorePointsUtility.ExperienceScorePreGem * score));
        Sprite badge = null;
        if (scenePrefabs.GetExperience().subject == "Maths") {
            achievementMath.UpdateCurrent();
            badge = achievementMath.GetBadge();
        }
        else if (scenePrefabs.GetExperience().subject == "Science")
        {
            achievementSc.UpdateCurrent();
            badge = achievementMath.GetBadge();
        }
        else if (scenePrefabs.GetExperience().subject == "Social Studies")
        {
            achievementSS.UpdateCurrent();
            badge = achievementMath.GetBadge();
        }
        achievementSc.UpdateCurrent();
        if (badge != null)
        {
            AchievementManager.Instance.AddBadge(badge);
        }
        ExperiencePlayData s = new ExperiencePlayData();
        s.status = 2;
        s.experienceCode = scenePrefabs.GetExperience().experienceCode;
        s.score = score;
        NetworkManager.Instance.UpdateExperienceStatus(s, OntUpdateExperienceSuccess, OntUpdateExperienceFailed);
    }
    public void GotoHome() { SceneLoader.Instance.LoadExperience(sceneName); }
    private void OntUpdateExperienceSuccess(NetworkParameters obj)
    {
        isPressed = false;
        UpdateProfile();
        SceneLoader.Instance.LoadExperience(sceneName);
    }
    private void OntUpdateExperienceFailed(NetworkParameters obj)
    {
        isPressed = false;
        SceneLoader.Instance.LoadExperience(sceneName);

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
        if (profile.achievements.Count > 0)
        {
            ss.achievementsData = new AchievementsData();
            ss.achievementsData.achievements = profile.achievements;
        }
        NetworkManager.Instance.UpdateProfile(ss, OnUpdateProfileSuccess, OnUpdateProfileFailed);
    }
    private void OnUpdateProfileSuccess(NetworkParameters obj)
    {
        //GetProfile();
    }
    private void OnUpdateProfileFailed(NetworkParameters obj)
    {
    }
}
