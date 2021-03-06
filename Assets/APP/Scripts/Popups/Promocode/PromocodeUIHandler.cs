﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromocodeUIHandler : MonoBehaviour
{

    private static PromocodeUIHandler instance;
    public static PromocodeUIHandler Instance { get => instance; set => instance = value; }
    
    [SerializeField] ToolBarHandler popup;
    [SerializeField] ToolBarHandler popupGems;
    [SerializeField] Text gemsTxt;
    [SerializeField] InputField promoIn;
    [SerializeField] ErrorFadingHandler error;
    [SerializeField] ProfileNetworkHandler profile;
    bool isPressed = false;



    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    public void ShowPop() {
        popup.OpenToolBar();
    }
    public void SubmitPromoCode()
    {
        if (isPressed)
            return;
        string code = promoIn.text;
        if (ValidationInputUtility.IsEmptyOrNull(code)) {
            error.ShowErrorMsg("Enter Code!!!");
            error.HideErrorMsgDelay(3f);
            return;
        }
        if (!ValidationInputUtility.IsAlphOrDigit(code))
        {
            error.ShowErrorMsg("Invalid Code Formate");
            error.HideErrorMsgDelay(3f);
            return;
        }
        isPressed = true;
        PromoCodeData promoCodeData = new PromoCodeData();
        promoCodeData.hash = code;
        NetworkManager.Instance.PromoCode(promoCodeData, OnPromoCodeSusccess, OnPromoCodeFailed);
    }
    public void SetCoins(int powerStones) {
        gemsTxt.text = powerStones.ToString(); 
        popupGems.OpenToolBar();
    }
    private void OnPromoCodeSusccess(NetworkParameters obj)
    {
        isPressed = false;
        popup.CloseToolBar();
        PromoCodeResponse promoCode = (PromoCodeResponse)obj.responseData;
        SetCoins(promoCode.powerStones);
        profile.GetProfile(true);
        profile.Profile.stones += promoCode.powerStones;
    }
    private void OnPromoCodeFailed(NetworkParameters obj)
    {
        isPressed = false;
        switch (obj.err.message) {
            case "not supported":
            case "invalid promo code":
                error.ShowErrorMsg("Invalid Promocode");
                error.HideErrorMsgDelay(3f);
                break;
            case "promo code reached max usage":
            case "expired promo code":
                error.ShowErrorMsg("Expired Promocode");
                error.HideErrorMsgDelay(3f);
                break;
            case "consumed before":
                error.ShowErrorMsg("Used Before");
                error.HideErrorMsgDelay(3f);
                break;
        }
        print(obj.err.message);
    }
}
