using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSS132ShowSummary : MonoBehaviour
{
    [SerializeField] float waitingTime;
    [SerializeField] SummaryHandler summary;
    public void ShowTutorialWithTime ()
    {
        Invoke (nameof (OpenSummary) , waitingTime);
    }
    void OpenSummary ()
    {
       summary.ViewSummary ();
    }
}
