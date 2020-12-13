using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BubbleTextInfo
{
    #region Fields
    [SerializeField] BubbleType bubbleType;
    [SerializeField] string textInfo;
    #endregion Fields

    #region Properties
    public BubbleType BubbleType { get => bubbleType; set => bubbleType = value; }
    public string TextInfo { get => textInfo; set => textInfo = value; }
    #endregion Properties
}
