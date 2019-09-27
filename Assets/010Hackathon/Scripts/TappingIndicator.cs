using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class TappingIndicator : MonoBehaviour
{

    public interactions interactionsInstance;
    [ContextMenu("Test")]
    private void OnMouseDown()
    {
        if (interactionsInstance.canSet)
            interactionsInstance.placeTheObject();
    }
}