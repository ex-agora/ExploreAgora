using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSS132ShowTutorial : MonoBehaviour
{
    [SerializeField] float waitingTime;
    [SerializeField] TutorialPanelController tutorial;
    public void ShowTutorialWithTime ()
    {
        Invoke (nameof (OpenTutorial) , waitingTime);
    }
    void OpenTutorial ()
    {
        tutorial.OpenTutorial ();
    }
}
