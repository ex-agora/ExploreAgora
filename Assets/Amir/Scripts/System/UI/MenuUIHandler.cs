using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] Button menuBtn;
    [SerializeField] Button relocateBtn;
    [SerializeField] Sprite openMenuSprite;
    [SerializeField] Sprite cloesMenuSprite;
    [SerializeField] Image menuBtnImg;
    [SerializeField] Animator menuAnimator;
    [SerializeField] Animator menuBtnAnimator;
    [SerializeField] Sprite[] audioSprites;
    [TypeConstraint(typeof(IMenuHandler))] [SerializeField] GameObject handler;
    IMenuHandler menuHandler;
    int indexSoundSprite;
    bool isOpen;
    public bool IsOpen { get => isOpen; set { isOpen = value; HandleMenu(); } }

    // Start is called before the first frame update
    void Start()
    {
        indexSoundSprite = 0;
        IsOpen = false;
        menuHandler = handler.GetComponent<IMenuHandler>();
    }
   
    // Update is called once per frame
    void Update()
    {

    }
    void HandleMenusprite() {
        //if (IsOpen) { menuBtnImg.sprite = cloesMenuSprite; }
        //else { menuBtnImg.sprite = openMenuSprite; }
        menuAnimator.SetBool("IsOpen", IsOpen);
        menuBtnAnimator.SetBool("IsOpen", IsOpen);


    }
    void HandleMenu() {
        HandleMenusprite();
    }
    public void ClickToggleMenuPressed() {
        IsOpen = !IsOpen;
        if (AudioManager.Instance != null)
        {
            if (isOpen)
            {
                AudioManager.Instance.Play("openOption", "UI");
            }
            else
            {
                AudioManager.Instance.Play("closeOption", "UI");
            }
        }
    }
    public void HandleSoundBtn(Image image) {
        indexSoundSprite = (indexSoundSprite + 1) % audioSprites.Length;
        image.sprite = audioSprites[indexSoundSprite];
        if (AudioManager.Instance != null) {
            AudioManager.Instance.AudioController(indexSoundSprite);
        }
    }
    public void StopMenuInteraction() {
        IsOpen = false;
        menuBtn.interactable = false;
        relocateBtn.interactable = false;
    }
    public void ResetLevel() => menuHandler.ResetLevel();
    public void GoToHome() => menuHandler.GoTOHome();
}
