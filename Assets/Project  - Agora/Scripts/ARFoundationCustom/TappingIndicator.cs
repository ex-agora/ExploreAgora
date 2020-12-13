using UnityEngine;
using UnityEngine.UI;

public class TappingIndicator : MonoBehaviour
{

    #region Fields
    public interactions interactionsInstance;
    #endregion Fields

    #region Methods
    [ContextMenu("Test")]
    private void OnMouseDown()
    {
        if (interactionsInstance.canSet)
            interactionsInstance.placeTheObject();
    }
    #endregion Methods
}