using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FooterPanelManager : MonoBehaviour
{
    //[SerializeField] private FooterPanelHandler settings;
    //[SerializeField] private FooterPanelHandler shop;
    [SerializeField] private FooterPanelHandler profile;
    //[SerializeField] private FooterPanelHandler book;
    //[SerializeField] private FooterPanelHandler mission;
    private FooterPanelHandler currentActive;

    private void Start()
    {
        currentActive = profile;
        currentActive.ActiveChoice();
    }
    public void ActivePanel(FooterPanelHandler newCurrent)
    {
        currentActive.InactiveChoice();
        currentActive = newCurrent;
        currentActive.ActiveChoice();
    }
}