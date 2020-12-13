using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenHandTutorial : MonoBehaviour
{
    [SerializeField] Animator RTTutorialAnimator;
    [SerializeField] RealTimeTutorialHandler realTimeTutorial;
    private void Awake()
    {
        realTimeTutorial.OpenIndicator();
    }
}
