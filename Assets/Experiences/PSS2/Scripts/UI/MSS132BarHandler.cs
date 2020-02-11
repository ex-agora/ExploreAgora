using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSS132BarHandler : MonoBehaviour
{
    [SerializeField] MSS132BarItemHandler thriving;
    [SerializeField] MSS132BarItemHandler alive;
    [SerializeField] MSS132BarItemHandler dying;
    [SerializeField] MSS132ProgressBarHandler progressBarHandler;
    [SerializeField] bool isProgressBarSet;
    public void ActiveThriving ()
    {
        if (isProgressBarSet) { progressBarHandler.ActiveThriving(); }
        else {
            alive.DiactivateImage();
            dying.DiactivateImage();
            thriving.ActivateImageAnimation();
        }
        
    }
    public void ActiveAlive()
    {
        if (isProgressBarSet) { progressBarHandler.ActiveAlive(); }
        else {
            thriving.DiactivateImage();
            dying.DiactivateImage();
            alive.ActivateImageAnimation();
        }
        
    }
    public void ActiveDying ()
    {
        if (isProgressBarSet) { progressBarHandler.ActiveDying(); }
        else {
            thriving.DiactivateImage();
            alive.DiactivateImage();
            dying.ActivateImageAnimation();
        }
        
    }
}
