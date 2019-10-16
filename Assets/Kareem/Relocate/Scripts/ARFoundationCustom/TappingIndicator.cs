using UnityEngine;
using UnityEngine.UI;

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