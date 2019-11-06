using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotSpotPivotInitializer : MonoBehaviour
{
    private void Awake()
    {
        OnBoardingMathGameManager.Instance.AddHotSpotPivots(transform);
    }
}
