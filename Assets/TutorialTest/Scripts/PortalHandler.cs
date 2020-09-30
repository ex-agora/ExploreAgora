using System;
using System.Collections;
using System.Collections.Generic;
using Pixel_Play.Scripts.OffScreenIndicator;
using UnityEngine;

public class PortalHandler : MonoBehaviour
{
    [SerializeField] private Target door;
    [SerializeField] private Collider boxCol;

    private void OnTriggerEnter(Collider other)
    {
        door.enabled = false;
        TutorialFlowManager.Instance.AppleFeedback();
        boxCol.enabled = false;
    }
}
