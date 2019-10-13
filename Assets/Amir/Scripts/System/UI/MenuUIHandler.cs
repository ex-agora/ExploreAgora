using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] Sprite openMenuSprite;
    [SerializeField] Sprite cloesMenuSprite;
    [SerializeField] Image menuBtnImg;
    [SerializeField] Animator menuAnimator;
    bool isOpen;

    public bool IsOpen { get => isOpen; set { isOpen = value; HandleMenu(); } }

    // Start is called before the first frame update
    void Start()
    {
        IsOpen = false;
    }
   
    // Update is called once per frame
    void Update()
    {

    }
    void HandleMenusprite() {
        if (IsOpen) { menuBtnImg.sprite = cloesMenuSprite; }
        else { menuBtnImg.sprite = openMenuSprite; }
        menuAnimator.SetBool("IsOpen", IsOpen);
    }
    void HandleMenu() {
        HandleMenusprite();
    }
    public void ClickToggleMenuPressed() {
        IsOpen = !IsOpen;
    }
}
