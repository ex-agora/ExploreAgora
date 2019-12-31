using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSS132BarHandler : MonoBehaviour
{
    [SerializeField] MSS132BarItemHandler thriving;
    [SerializeField] MSS132BarItemHandler alive;
    [SerializeField] MSS132BarItemHandler dying;
    public void ActiveThriving ()
    {
        thriving.ActivateImageAnimation ();
        alive.DiactivateImage ();
        dying.DiactivateImage ();
    }
    public void ActiveAlive()
    {
        alive.ActivateImageAnimation ();
        thriving.DiactivateImage ();
        dying.DiactivateImage ();
    }
    public void ActiveDying ()
    {
        dying.ActivateImageAnimation ();
        thriving.DiactivateImage ();
        alive.DiactivateImage ();
    }
}
