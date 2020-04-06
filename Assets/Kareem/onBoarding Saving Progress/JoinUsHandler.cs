using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinUsHandler : MonoBehaviour
{
    [SerializeField] private Button doneButton;
    [SerializeField] private Sprite interactableSprite;
    [SerializeField] private Sprite nonInteractableSprite;
    [SerializeField] private InputField nicknameField;
    [SerializeField] private GenderHandler gender;
    [SerializeField] private DOBHandler dOB;
    [SerializeField] private ProfileInfoContainer profile;

    private void Start()
    {
        doneButton.interactable = false;
        doneButton.image.sprite = nonInteractableSprite;
    }
    public void CheckInputs()
    {
        if (nicknameField.text == "")
        {
            doneButton.interactable = false;
            doneButton.image.sprite = nonInteractableSprite;
        }
        else
        {
            doneButton.interactable = true;
            doneButton.image.sprite = interactableSprite;
        }
    }
    public void CollectData() {
        profile.nickname = nicknameField.text;
        profile.gender = gender.PlayerGender;
        profile.DOB = dOB.DOB;
    }
}
