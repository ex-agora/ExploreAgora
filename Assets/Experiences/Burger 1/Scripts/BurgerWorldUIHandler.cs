using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BurgerWorldUIHandler : MonoBehaviour
{
    [SerializeField] static BurgerWorldUIHandler instance;
    [SerializeField] Image burgerImg, extrasImg, backButton, checkButton;
    [SerializeField] BoxCollider backButtonInteractable, checkButtonInteractable;
    [SerializeField] List<GameObject> orders;
    [SerializeField] List<Sprite> burgerExtrasIMG;
    [SerializeField] List<Sprite> enableDisableIMG;
    [SerializeField] TMP_Text score;

    public static BurgerWorldUIHandler Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void UpdateUI(int currentOrderNumber, int lastOrderNumber, int burgerRandomNumber, int extrasRandomNumber)
    {
        Debug.LogWarning(burgerRandomNumber + "    " + extrasRandomNumber);
        burgerImg.sprite = burgerExtrasIMG[burgerRandomNumber];
        extrasImg.sprite = burgerExtrasIMG[extrasRandomNumber];
        orders[lastOrderNumber].SetActive(false);
        orders[currentOrderNumber].SetActive(true);
    }

    public void HideAllUI()
    {

    }


    public void BackButtonBehavior()
    {

        SandwichComponentsHandler.Instance.BackwardBehavior();
    }
    public void CheckButtonBehavior()
    {
        if (SandwichComponentsHandler.Instance.BurgerExperineceType == EBurgerExperineceType.WithLocking)
            SandwichComponentsHandler.Instance.CheckAnswers();
        else
            SandwichComponentsHandler.Instance.CheckAnswersForUnLocked();
    }

    public void EnableDisableBackButton(bool state)
    {
        if (state)
        {
            backButton.sprite = enableDisableIMG[1];
            backButtonInteractable.enabled = true;
        }
        else
        {
            backButton.sprite = enableDisableIMG[0];
            backButtonInteractable.enabled = false;
        }
    }
    public void EnableDisableCheckButton(bool state)
    {
        if (state)
        {
            checkButton.sprite = enableDisableIMG[3];
            checkButtonInteractable.enabled = true;
        }
        else
        {
            checkButton.sprite = enableDisableIMG[2];
            checkButtonInteractable.enabled = false;
        }
    }
}

