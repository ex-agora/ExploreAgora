using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnBoardingScenarios : MonoBehaviour
{
    #region Fields
    [SerializeField] GameObject comics, Map, Profile , book , shop , setting;
    //[SerializeField] List<Transform> mapInstructionsPanels;
    //[SerializeField] List<Transform> popUpInstructionsPanels;
    [SerializeField] List<Button> mapButtons;
    [SerializeField] List<GameObject> footerButtons;
    [SerializeField] List<string> onBoardingSceneNames;
    [SerializeField] GameObject giftsPanel;
    [SerializeField] Button backMission;
    //[SerializeField] StagesIndicator stagesIndicator;
    private int currentIndex;
    //index of current state


    //[SerializeField] OnBoardingPhases tesst;
    #endregion


    #region Public_Methods


    public void OnBoardingFlowStates()
    {
        Debug.Log((int)AppManager.Instance.boardingPhases);
        switch (AppManager.Instance.boardingPhases)
        {

            case OnBoardingPhases.Map:
                currentIndex = AppManager.Instance.currentBoardingIndex;
                MapScenearios();
                break;
            case OnBoardingPhases.Book:
                footerButtons[1].GetComponentInChildren<Button>().interactable = true;  
                footerButtons[1].SetActive(true);
                footerButtons[1].GetComponent<FooterButtonsBehavior>().EnableInstructionPanel();
                LockPreviousStages((int)AppManager.Instance.boardingPhases);
                break;
            case OnBoardingPhases.Profile:
                LockPreviousStages((int)AppManager.Instance.boardingPhases);
                footerButtons[2].SetActive(true);
                footerButtons[2].GetComponentInChildren<Button>().interactable = true;
                footerButtons[2].GetComponent<FooterButtonsBehavior>().EnableInstructionPanel();
                break;
            case OnBoardingPhases.Shop:
                LockPreviousStages((int)AppManager.Instance.boardingPhases);
                footerButtons[3].SetActive(true);
                footerButtons[3].GetComponentInChildren<Button>().interactable = true;
                footerButtons[3].GetComponent<FooterButtonsBehavior>().EnableInstructionPanel();
                break;
            case OnBoardingPhases.Setting:
                LockPreviousStages((int)AppManager.Instance.boardingPhases);
                footerButtons[4].SetActive(true);
                footerButtons[4].GetComponent<FooterButtonsBehavior>().EnableInstructionPanel();
                break;

            case OnBoardingPhases.Quests:
                footerButtons[0].SetActive(true);
                footerButtons[0].GetComponentInChildren<Button>().interactable = true;
                footerButtons[0].SetActive(true);
                footerButtons[0].GetComponent<FooterButtonsBehavior>().EnableInstructionPanel();
                backMission.interactable = true;
                for (int i = 1; i < footerButtons.Count; i++)
                {
                    footerButtons[i].SetActive(true);
                    footerButtons[i].GetComponentInChildren<Button>().interactable = false;
                }
                break;

            default:
                print("something else");
                break;
        }
    }




    public void Giftbutton()
    {
        AppManager.Instance.isCurrentLevelPrizeDone[currentIndex] = true;
        AppManager.Instance.saveOnBoardingProgress();
        mapButtons[currentIndex].GetComponent<MapButtonsBehavior>().ShowAfterGiftUI();
        mapButtons[currentIndex].GetComponent<MapButtonsBehavior>().ChangeButtonSprite();
    }



    public void Nextbutton()
    {
        AppManager.Instance.currentBoardingIndex++;
        AppManager.Instance.saveOnBoardingProgress();
        if(AppManager.Instance.currentBoardingIndex <= 3 )
         currentIndex = AppManager.Instance.currentBoardingIndex;
        MapScenearios();
    }



    public void ChangeProfileToShop()
    {
        ChangeStage(OnBoardingPhases.Shop);
    } public void ChangeMapToBook()
    {
        ChangeStage(OnBoardingPhases.Book);
    }

    public void ChangeBookToProfile()
    {
        ChangeStage(OnBoardingPhases.Profile);
    }

    public void ChangeSettingToQuests()
    {
        ChangeStage(OnBoardingPhases.Quests);
    }
    public void ChangeShopToSetting()
    {
        ChangeStage(OnBoardingPhases.Setting);
    } 
    public void ChangeStage(OnBoardingPhases onBoardingPhase)
    {
        AppManager.Instance.boardingPhases = onBoardingPhase;
        AppManager.Instance.saveOnBoardingProgress();
    }

    public void ChangeButtonsSprite(Sprite s, Image img)
    {
        img.sprite = s;
    }

    #endregion


    #region Private_Methods
    private void Awake()
    {
        OnBoardingFlowStates();
    }


    void MapScenearios()
    {

        
        if (currentIndex > 0 && currentIndex < 4)
        {
            mapButtons[0].GetComponent<MapButtonsBehavior>().PlayActions();
            Debug.Log("11111QQQ");
            for (int i = 1; i < currentIndex + 1; i++)
            {
                Debug.Log("QQQ");
                if (AppManager.Instance.isCurrentLevelDone[i] == false && AppManager.Instance.isCurrentLevelPrizeDone[i] == false)
                {
                    mapButtons[i].interactable = true;
                    mapButtons[i].GetComponent<MapButtonsBehavior>().OpenButtonFirstTime();
                    mapButtons[i].GetComponent<MapButtonsBehavior>().ShowBeforePlayingUI();
                    Debug.Log("1");
                    break;
                }
                else if (AppManager.Instance.isCurrentLevelDone[i] == true && AppManager.Instance.isCurrentLevelPrizeDone[i] == false)
                {
                    //lock
                    mapButtons[i].interactable = false;
                    ShowGiftPanel(i);
                    //show last panel
                    Debug.Log("Gifts");
                    break;
                }
                else if (AppManager.Instance.isCurrentLevelDone[i] == true && AppManager.Instance.isCurrentLevelPrizeDone[i] == true)
                {
                    print("HEREEEE   " + i );
                    if (i != mapButtons.Count - 1)
                    {
                        mapButtons[i].interactable = false;

                        mapButtons[i + 1].interactable = true;

                    }
                    else
                    {
                        mapButtons[i].interactable = false;

                        // change enum
                        //ChangeStage(OnBoardingPhases.Book);
                        //OnBoardingFlowStates();
                    }

                    mapButtons[i].GetComponent<MapButtonsBehavior>().ChangeButtonSprite();
                    mapButtons[i].GetComponent<MapButtonsBehavior>().PlayActions();
                }
            }
        }
        else if (currentIndex == 0)
        {
            if (AppManager.Instance.isCurrentLevelDone[0] == false)
            {
                mapButtons[0].interactable = true;
                mapButtons[0].GetComponent<MapButtonsBehavior>().OpenButtonFirstTime();
                print("xzcdferre");
            }
            else
            {
                print("kjkjkjkj");
                mapButtons[0].interactable = false;
                mapButtons[1].interactable = true;
            }
        }
        else
        {
            print("PQWEOQW");
        }

    }



   


    void LockPreviousStages(int num)
    {

        for (int i = 0; i < num-1; i++)
        {
            print(footerButtons[i].name + "     HEREEE");
            footerButtons[i].SetActive(true);
            footerButtons[i].GetComponentInChildren<Button>().interactable = false;
        }
    }

    void ShowGiftPanel(int i)
    {
        giftsPanel.SetActive(true);

    }

    public void FinishOnBoarding() {
        UXFlowManager.Instance.FinishOnBoarding();
    }
    #endregion


   

    //public void ChangeScene
}
