using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartsDistanceAlertHandler : MonoBehaviour
{
    [SerializeField] GameObject DistanceAlert;
    public void StartDistanceAlert()
    {
        DistanceAlert.SetActive(true);
    }
}
