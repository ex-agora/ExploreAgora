using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelocateBehavior : MonoBehaviour
{

    #region Fields
    public interactions interactionsInstance;
    public PlaneDetectionController planeDetectionController;
    #endregion Fields

    #region Methods
    public void relocate()
    {
        interactionsInstance.hideShowArComponents(true);
    }
    #endregion Methods
}
