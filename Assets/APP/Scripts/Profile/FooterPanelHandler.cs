using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FooterPanelHandler : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Button btn;
    [SerializeField] private Text text;
    [SerializeField] private GameObject targetPanel;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;

    private void Awake()
    {
        InactiveChoice();
    }

    public void ActiveChoice()
    {
        image.sprite = activeSprite;
        text.gameObject.SetActive(true);
        targetPanel.SetActive(true);
        image.SetNativeSize();
    }

    public void InactiveChoice()
    {
        image.sprite = inactiveSprite;
        text.gameObject.SetActive(false);
        targetPanel.SetActive(false);
        image.SetNativeSize();
    }
}
