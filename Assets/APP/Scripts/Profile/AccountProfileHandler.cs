using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccountProfileHandler : MonoBehaviour
{
    [SerializeField] private Text nicknameText;
    [SerializeField] private Text rankText;
    [SerializeField] private Text pointsText;
    [SerializeField] private Text keysText1;
    [SerializeField] private Text streakText1;
    [SerializeField] private Text stoneText1;

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

    public ProfileInfoContainer ProfileInfo { get => profileInfo; set => profileInfo = value; }

    public void ConfirmChangeProfilePicture()
    {
        profileSpriteIndex = profilePictureHandler.ChangeProfilePicture();
        chosenForProfile = profilePictureHandler.GetProfileSprite(profileSpriteIndex);
        mainProfileImage.sprite = chosenForProfile;
        shopProfileImage.sprite = chosenForProfile;
        bookProfileImage.sprite = chosenForProfile;
        insideBundleProfileImage.sprite = chosenForProfile;
        outsideBundleProfileImage.sprite = chosenForProfile;
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
    }

    public void UpdateRankPoints()
    {
        var rank = ranks.GetRank(profileInfo.points);
        rankText.text = rank.Key;
        pointsText.text = $"{AbbrevationUtility.AbbreviateNumber(profileInfo.points)}/{AbbrevationUtility.AbbreviateNumber(rank.Value.max)}";
    }
}