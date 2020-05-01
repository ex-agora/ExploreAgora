using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccountProfileHandler : MonoBehaviour
{
    [SerializeField] private Text nicknameText;
    [SerializeField] private Text rankText;
    [SerializeField] private Text rankBookText;
    [SerializeField] private Text rankShopText;
    [SerializeField] private Text rankMissionText;
    [SerializeField] private Text rankInsideMissionText;
    [SerializeField] private Text pointsText;
    [SerializeField] private Text pointsBookText;
    [SerializeField] private Text pointsShopText;
    [SerializeField] private Text pointsMissionText;
    [SerializeField] private Text pointsInsideMissionText;
    [SerializeField] private Image pointsBookImg;
    [SerializeField] private Image pointsShopImg;
    [SerializeField] private Image pointsMissionImg;
    [SerializeField] private Image pointsInsideMissionImg;

    [SerializeField] private Text keysText1;
    [SerializeField] private Text keysShopText1;
    [SerializeField] private Text keysMissionText1;
    [SerializeField] private Text keysBookText1;
    [SerializeField] private Text keysInsideMissionText1;
    [SerializeField] private Text streakText1;
    [SerializeField] private Text stoneText1;
    [SerializeField] private Text stoneBookText1;
    [SerializeField] private Text stoneShopText1;
    [SerializeField] private Text stoneMissionText1;
    [SerializeField] private Text stoneInsideMissionText1;

    [SerializeField] private Image mainProfileImage;
    [SerializeField] private Image shopProfileImage;
    [SerializeField] private Image bookProfileImage;
    [SerializeField] private Image insideBundleProfileImage;
    [SerializeField] private Image outsideBundleProfileImage;
    [SerializeField] private Image streakImage;
    [SerializeField] private List<Sprite> streakSprites;

    [SerializeField] private int profileSpriteIndex;
    [SerializeField] private Sprite chosenForProfile;

    [SerializeField] private ProfilePictureHandler profilePictureHandler;
    [SerializeField] private ProfileInfoContainer profileInfo;
    [SerializeField] private RanksHolder ranks;

    [SerializeField] private Text fNameText;
    [SerializeField] private Text lNameText;
    [SerializeField] private CountryHandler country;
    public ProfileInfoContainer ProfileInfo { get => profileInfo; set => profileInfo = value; }

    public void ConfirmChangeProfilePicture()
    {
        profileSpriteIndex = profilePictureHandler.ChangeProfilePicture();
        chosenForProfile = profilePictureHandler.GetProfileSprite(ProfileInfo.profileImgIndex);
        mainProfileImage.sprite = chosenForProfile;
        shopProfileImage.sprite = chosenForProfile;
        bookProfileImage.sprite = chosenForProfile;
        insideBundleProfileImage.sprite = chosenForProfile;
        outsideBundleProfileImage.sprite = chosenForProfile;
    }

    public void UpdateSettingsInfo() {
        fNameText.text = profileInfo.fName;
        lNameText.text = profileInfo.lName;
        country?.SetCountry(profileInfo.country);
    }
    public void UpdateNickname()
    {
        nicknameText.text = profileInfo.nickname;
    }
    public void UpdateStreak()
    {
        if (profileInfo.streaks == 0)
        {
            streakImage.gameObject.SetActive(false);
        }
        else
        {
            streakImage.sprite = streakSprites[(profileInfo.streaks - 1) % 7];
            streakImage.gameObject.SetActive(true);
        }
        streakText1.text = profileInfo.streaks.ToString();
    }
    void UpdateKeys()
    {
        keysText1.text = profileInfo.keys.ToString();
        keysShopText1.text = profileInfo.keys.ToString();
        keysMissionText1.text = profileInfo.keys.ToString(); ;
        keysInsideMissionText1.text = profileInfo.keys.ToString(); ;
        keysBookText1.text = profileInfo.keys.ToString();
    }
    void UpdatePowerStones()
    {
        stoneText1.text = profileInfo.stones.ToString();
        stoneBookText1.text = profileInfo.stones.ToString();
        stoneShopText1.text = profileInfo.stones.ToString(); 
        stoneMissionText1.text = profileInfo.stones.ToString();
        stoneInsideMissionText1.text = profileInfo.stones.ToString();
    }
    public void UpdateRankPoints()
    {
        var rank = ranks.GetRank(profileInfo.points);
        rankText.text = rank.Key;
        rankBookText.text = rank.Key;
        rankShopText.text = rank.Key;
        rankMissionText.text = rank.Key;
        rankInsideMissionText.text = rank.Key;
        string pointStr = $"{StringUtility.AbbreviateNumber(profileInfo.points)}/{StringUtility.AbbreviateNumber(rank.Value.max)}";
        pointsText.text = pointStr;
        pointsBookText.text = pointStr;
        pointsShopText.text = pointStr;
        pointsMissionText.text = pointStr;
        pointsInsideMissionText.text = pointStr;
        float amount = (profileInfo.points - rank.Value.min) / (float)(rank.Value.max - rank.Value.min);
        pointsBookImg.fillAmount = amount;
        pointsShopImg.fillAmount = amount;
        pointsMissionImg.fillAmount = amount;
        pointsInsideMissionImg.fillAmount = amount;
    }
    public void UpdateProfile()
    {
        ConfirmChangeProfilePicture();
        UpdateStreak();
        UpdatePowerStones();
        UpdateKeys();
        UpdateSettingsInfo();
        UpdateNickname();
        UpdateRankPoints();
    }
}