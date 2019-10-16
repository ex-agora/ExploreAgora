using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelocateBehavior : MonoBehaviour
{

    public interactions interactionsInstance;
    public PlaneDetectionController planeDetectionController;
    public void relocate()
    {
        interactionsInstance.hideShowArComponents(true);
    }
}
