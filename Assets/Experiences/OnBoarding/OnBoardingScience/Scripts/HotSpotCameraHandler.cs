using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotSpotCameraHandler : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    void OnEnable()
    {
        canvas.worldCamera = OnBoardingGameManager.Instance.ArCamera;
    }   
}
