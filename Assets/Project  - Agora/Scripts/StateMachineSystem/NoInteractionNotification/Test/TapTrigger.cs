using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapTrigger : MonoBehaviour
{
    #region Fields
    bool isTapped;
    #endregion Fields

    #region Properties
    public bool IsTapped { get => isTapped; set => isTapped = value; }
    #endregion Properties
}
