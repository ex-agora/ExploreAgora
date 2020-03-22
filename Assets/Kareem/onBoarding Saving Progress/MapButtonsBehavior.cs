﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MapButtonsBehavior : MonoBehaviour
{
    [SerializeField] Sprite playedExperince;
    [SerializeField] Image img;
    [SerializeField] UnityEvent buttonAction  ,  openingButton;
   // [SerializeField] 
    public void ChangeButtonSprite()
    {
        img.sprite = playedExperince;
    }

    public void PlayActions()
    {
        buttonAction.Invoke();
    }
    public void OpenButtonFirstTime()
    {
        openingButton.Invoke();
    }

    //public void ShowNextNodePanel()
    //{

    //}
}
