using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS17DistanceAlert : MonoBehaviour
{
    [SerializeField] int minDistanceHintIndex;
    [SerializeField] int maxDistanceHintIndex;
    public void OnMaxDitance ()
    {
        SS17GameManager.Instance.ShowHint (maxDistanceHintIndex);
    }
    public void OnMinDitance ()
    {
        SS17GameManager.Instance.ShowHint (minDistanceHintIndex);
    }
    public void OnCorrectDitance ()
    {
        SS17GameManager.Instance.StartFirstPhase ();
    }
}
