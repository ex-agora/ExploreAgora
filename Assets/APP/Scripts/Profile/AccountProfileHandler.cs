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

    [SerializeField] private Image editProfileImage;
    [SerializeField] private Image mainProfileImage;

    [SerializeField] private int profileSpriteIndex;
    [SerializeField] private Sprite chosenForProfile;

    [SerializeField] private ProfilePictureHandler profilePictureHandler;
    public void ConfirmChangeProfilePicture()
    {
        profileSpriteIndex =  profilePictureHandler.ChangeProfilePicture();       
        chosenForProfile = profilePictureHandler.GetProfileSprite(profileSpriteIndex);
        mainProfileImage.sprite = chosenForProfile;
        editProfileImage.sprite = chosenForProfile;
    }
}