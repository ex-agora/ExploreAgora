using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenderHandler : MonoBehaviour
{
    private Gender playerGender;
    [SerializeField] private Image male;
    [SerializeField] private Image female;
    [SerializeField] private Image other;
    private Image currentGender;

    public Gender PlayerGender { get => playerGender; set { SetGender(value); } }

    private void Start()
    {
        currentGender = male;
        PlayerGender = Gender.Male;
    }

    public void SelectGender(int newGender)
    {
        currentGender.gameObject.SetActive(false);

        playerGender = (Gender)newGender;
        switch (PlayerGender)
        {
            case Gender.None:
                break;
            case Gender.Male:
                currentGender = male;
                break;
            case Gender.Female:
                currentGender = female;
                break;
            case Gender.Other:
                currentGender = other;
                break;
            case Gender.PreferNotToSay:
                break;
        }
        currentGender.gameObject.SetActive(true);
    }

    private void SetGender(Gender newGender)
    {
        playerGender = newGender;
        if (currentGender != null)
            SelectGender((int)newGender);
    }
}