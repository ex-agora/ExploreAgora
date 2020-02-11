using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIHandler : MonoBehaviour
{
    #region Fields
    [SerializeField] Sprite[] audioSprites;
    [SerializeField] Sprite cloesMenuSprite;
    [TypeConstraint (typeof (IMenuHandler))] [SerializeField] GameObject handler = null;
    int indexSoundSprite;
    bool isOpen;
    [SerializeField] Animator menuAnimator;
    [SerializeField] Button menuBtn;
    [SerializeField] Animator menuBtnAnimator;
    [SerializeField] Image menuBtnImg;
    IMenuHandler menuHandler;
    [SerializeField] Sprite openMenuSprite;
    [SerializeField] Button relocateBtn;
    #endregion Fields

    #region Properties
    public bool IsOpen { get => isOpen; set { isOpen = value; HandleMenu(); } }
    #endregion Properties

    #region Methods
    public void ClickToggleMenuPressed()
    {
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

    public void GoToHome() => menuHandler?.GoTOHome();

    public void HandleSoundBtn(Image image)
    {
        indexSoundSprite = (indexSoundSprite + 1) % audioSprites.Length;
        image.sprite = audioSprites[indexSoundSprite];
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.AudioController(indexSoundSprite);
        }
    }

    public void ResetLevel() => menuHandler?.ResetLevel();

    public void RunMenuInteraction()
    {
        menuBtn.interactable = true;
        relocateBtn.interactable = true;
    }

    public void StopMenuInteraction()
    {
        IsOpen = false;
        menuBtn.interactable = false;
        relocateBtn.interactable = false;
    }

    void HandleMenu()
    {
        HandleMenusprite();
    }

    void HandleMenusprite()
    {
        //if (IsOpen) { menuBtnImg.sprite = cloesMenuSprite; }
        //else { menuBtnImg.sprite = openMenuSprite; }
        menuAnimator.SetBool("IsOpen", IsOpen);
        menuBtnAnimator.SetBool("IsOpen", IsOpen);


    }

    // Start is called before the first frame update
    void Start()
    {
        indexSoundSprite = 0;
        IsOpen = false;
        //menuHandler = handler?.GetComponent<IMenuHandler>();
    }
   
    // Update is called once per frame
    void Update()
    {

    }
    #endregion Methods
}
