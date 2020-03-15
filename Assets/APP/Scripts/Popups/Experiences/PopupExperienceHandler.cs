using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupExperienceHandler : MonoBehaviour
{
    [SerializeField] private string titleStr;
    [SerializeField] private string headerStr;
    [SerializeField] private string descriptionStr;

    [SerializeField] private Text titleTxt;
    [SerializeField] private Text headerTxt;
    [SerializeField] private Text descriptionTxt;

    [SerializeField] private Text placeOnTxt;
    [SerializeField] private Text sizeTxt;
    [SerializeField] private Text extraTxt;

    [SerializeField] private List<Sprite> placeOn;
    [SerializeField] private List<Sprite> size;
    [SerializeField] private List<Sprite> extra;

    [SerializeField] private Image placeOnImg;
    [SerializeField] private Image sizeImg;
    [SerializeField] private Image extraImg;

    [SerializeField] private ToolBarHandler popupAnim;

    [SerializeField] private GameObject playNowBtnObject;
    [SerializeField] private GameObject useKeyBtnObject;

    public string HeaderStr { get => headerStr; set => headerStr = value; }
    public string TitleStr { get => titleStr; set => titleStr = value; }
    public string DescriptionStr { get => descriptionStr; set => descriptionStr = value; }

    public void OpenPopup(ExperienceRequiredArea _experienceRequiredArea, string _placeOn, bool _hasExtra, bool _isScannedState)
    {
        switch (_experienceRequiredArea)
        {
            case ExperienceRequiredArea.None:
                break;
            case ExperienceRequiredArea.Small:
                sizeImg.sprite = size[0];
                break;
            case ExperienceRequiredArea.Medium:
                sizeImg.sprite = size[1];
                break;
            case ExperienceRequiredArea.Large:
                sizeImg.sprite = size[2];
                break;
            case ExperienceRequiredArea.XL:
                break;
            default:
                break;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if (_placeOn.ToLower() == "ground")
            placeOnImg.sprite = placeOn[0];
        else if (_placeOn.ToLower() == "table")
            placeOnImg.sprite = placeOn[1];
        else if (_placeOn.ToLower() == "face")
            placeOnImg.sprite = placeOn[2];
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if (_hasExtra)
            extraImg.sprite = extra[1];
        else
            extraImg.sprite = extra[0];
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        titleTxt.text = titleStr;
        headerTxt.text = headerStr;
        descriptionTxt.text = descriptionStr;
        if (_isScannedState)
        {
            playNowBtnObject.SetActive(false);
            useKeyBtnObject.SetActive(true);
        }
        else
        {
            playNowBtnObject.SetActive(true);
            useKeyBtnObject.SetActive(false);
        }

        popupAnim.OpenToolBar();
    }

    public void ClosePopup()
    {
        popupAnim.CloseToolBar();
    }
}