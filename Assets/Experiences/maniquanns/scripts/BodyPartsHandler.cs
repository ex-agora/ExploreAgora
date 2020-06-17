using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyPartsHandler : MonoBehaviour
{
    [SerializeField] List<GameObject> upperParts;
    [SerializeField] List<GameObject> middleParts;
    [SerializeField] List<GameObject> lowerParts;
    [SerializeField] List<GameObject> bodyPointers;
    [SerializeField] WorldUIHandler worldUIHandler;


    List<string> currentCorrectAnswers, asiaCorrectAnswers, africaCorrectAnswers, southAmericaCorrectAnswers;
    List<GameObject> currentSelectedObjects;
    bool[] bodyPartsTapIndicator = new bool[3];

    [HideInInspector]
    public List<string> currentAnswers;

    int activeButtonCounter = 0, upperCounter = 0, middleCounter = 0, lowerCounter = 0, wrongCounter = 0, overAllCounter = 0;
    EContinents continents;


    static BodyPartsHandler instance;
    public static BodyPartsHandler Instance { get => instance; set => instance = value; }


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        DeclareAnswersLists();
    }
    private void Start()
    {
        SetCorrectAnswersForEachContinents(continents);
        worldUIHandler.ChangePatterns(continents);
    }

    #region Public-Method

    public void ChangeBodyParts(EBodyParts bodyParts)
    {
        Debug.Log(bodyParts);
        if (bodyParts == EBodyParts.UpperBody)
        {
            bodyPointers[0].SetActive(true);
            upperCounter = ChangingPartsTechnique(upperParts, upperCounter, 0);


        }
        if (bodyParts == EBodyParts.MiddleBody)
        {
            
            bodyPointers[1].SetActive(true);
            middleCounter = ChangingPartsTechnique(middleParts, middleCounter, 1);


        }
        if (bodyParts == EBodyParts.LowerBody)
        {
            
            bodyPointers[2].SetActive(true);
            lowerCounter = ChangingPartsTechnique(lowerParts, lowerCounter, 2);

        }


        if (activeButtonCounter >= 3)
        {        
            worldUIHandler.CheckButtonHandler(true);
        }
    }

    public void ActiveButtonBehavior()
    {


        if (CheckAnswers() == 3)
        {

            worldUIHandler.CheckButtonHandler(false);
            //sfx , vfx
            //هرى تانى كتير كدا 

            Debug.Log("Here");
            //Reset
            ResetSetting();
            //Hide Models

            //change to the next state
            int index = (int)continents;
            index++;
            //counter Adds one 
            overAllCounter = index;
            if(index !=3)
            worldUIHandler.UpdateCounter(index+1);
            else
            worldUIHandler.UpdateCounter(index);

            if (index < 3)
            {
                continents = (EContinents)index;
                SetCorrectAnswersForEachContinents(continents);
                worldUIHandler.ChangePatterns(continents);
            }
            else
            {
                //finisheeeeeeeeed
                Debug.Log("FFFFF");
                MatchClothesGameManager.Instance.FinalSummary.ViewSummary();
            }

        }
        else
        {
            Debug.Log("Here   0");
            wrongCounter++;
            //try again from oggi
            MatchClothesGameManager.Instance.TryAgainBubble();
            
        }
        //reset
    }

    #endregion




    #region Private-Method


    void SetCorrectAnswersForEachContinents(EContinents continents)
    {
        if (continents == EContinents.Asia)
        {
            asiaCorrectAnswers[0] = EOutfit.Hat1.ToString();
            asiaCorrectAnswers[1] = EOutfit.t1.ToString();
            asiaCorrectAnswers[2] = EOutfit.l1.ToString();

        }
        else if (continents == EContinents.Africa)
        {
            africaCorrectAnswers[0] = EOutfit.Hat2.ToString();
            africaCorrectAnswers[1] = EOutfit.t2.ToString();
            africaCorrectAnswers[2] = EOutfit.l2.ToString();
        }
        else if (continents == EContinents.SouthAmerica)
        {
            southAmericaCorrectAnswers[0] = EOutfit.Hat3.ToString();
            southAmericaCorrectAnswers[1] = EOutfit.t3.ToString();
            southAmericaCorrectAnswers[2] = EOutfit.l3.ToString();
        }


    }

    void ResetSetting()
    {
        for (int i = 0; i < 3; i++)
        {
            bodyPartsTapIndicator[i] = false;
            currentAnswers[i] = "";
            upperParts[i].SetActive(false);
            middleParts[i].SetActive(false);
            lowerParts[i].SetActive(false);
            bodyPointers[i].SetActive(false);
        }
        activeButtonCounter = 0;
        upperCounter = 0;
        middleCounter = 0;
        lowerCounter = 0;
    }

    void DeclareAnswersLists()
    {
        asiaCorrectAnswers = new List<string>(3);
        africaCorrectAnswers = new List<string>(3);
        southAmericaCorrectAnswers = new List<string>(3);
        currentCorrectAnswers = new List<string>(3);
        currentAnswers= new List<string>(3);
        currentSelectedObjects = new List<GameObject>(3);
        for (int i = 0; i < 3; i++)
        {
            asiaCorrectAnswers.Add("");
            africaCorrectAnswers.Add("");
            southAmericaCorrectAnswers.Add("");
            currentAnswers.Add("");
           
        }
    }

    int ChangingPartsTechnique(List<GameObject> wantedPart, int currentElement, int bodyPartsTapIndicatorIndex)
    {
        if (bodyPartsTapIndicator[bodyPartsTapIndicatorIndex] == false)
        {
            bodyPartsTapIndicator[bodyPartsTapIndicatorIndex] = true;
            wantedPart[0].SetActive(true);
            wantedPart[0].GetComponent<OutFitsChooser>().SetAnswer();
            activeButtonCounter++;
        }
        else
        {
            currentElement++;
            if (currentElement < wantedPart.Count)
            {
                wantedPart[currentElement - 1].SetActive(false);
                wantedPart[currentElement].SetActive(true);
                wantedPart[currentElement].GetComponent<OutFitsChooser>().SetAnswer();

            }
            else
            {
                currentElement = 0;
                wantedPart[2].SetActive(false);
                wantedPart[0].SetActive(true);
                wantedPart[0].GetComponent<OutFitsChooser>().SetAnswer();

            }
        }
        return currentElement;

    }

    int CheckAnswers()
    {
        int correctCounter = 0;
        if (continents == EContinents.Asia)
            currentCorrectAnswers = asiaCorrectAnswers;

        if (continents == EContinents.Africa)
            currentCorrectAnswers = africaCorrectAnswers;
        if (continents == EContinents.SouthAmerica)

            currentCorrectAnswers = southAmericaCorrectAnswers;


        for (int i = 0; i < currentCorrectAnswers.Count; i++)
        {
            Debug.Log(currentCorrectAnswers[i] + "  " +currentAnswers[i]);
            if (currentCorrectAnswers[i] == currentAnswers[i])
                correctCounter++;
        }
        return correctCounter;
    }

    #endregion
}
