using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class OnBoardingDataSerializer
{
    public int currentBoardingIndex;
    public bool[] doneIndicators;
    public bool[] donePrizeIndicators;
    public bool[] isNextPressed;
    public OnBoardingPhases boardingPhases;

    public OnBoardingDataSerializer(AppManager dc)
    {
        currentBoardingIndex = dc.currentBoardingIndex;
        doneIndicators = dc.isCurrentLevelDone;
        donePrizeIndicators = dc.isCurrentLevelPrizeDone;
        isNextPressed = dc.isNextPressed;
        boardingPhases = dc.boardingPhases;
    }
}