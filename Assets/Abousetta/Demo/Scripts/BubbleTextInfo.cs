using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BubbleTextInfo
{
    [SerializeField] BubbleType bubbleType;
    [SerializeField] string textInfo;

    public BubbleType BubbleType { get => bubbleType; set => bubbleType = value; }
    public string TextInfo { get => textInfo; set => textInfo = value; }
}
