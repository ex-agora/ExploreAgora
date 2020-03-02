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
}