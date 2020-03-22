using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class OnBoardingDataSerializer
{
    public int currentBoardingIndex;
    public bool [] doneIndicators;
    public bool [] donePrizeIndicators;
    public OnBoardingPhases boardingPhases;

    public OnBoardingDataSerializer(AppManager dc)
    {
        currentBoardingIndex = dc.currentBoardingIndex;
        doneIndicators = dc.isCurrentLevelDone;
        donePrizeIndicators = dc.isCurrentLevelPrizeDone;
        boardingPhases = dc.boardingPhases;
    }
}