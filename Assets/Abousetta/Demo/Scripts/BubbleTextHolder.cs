using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleTextHolder : MonoBehaviour
{
    [SerializeField] BubbleTextInfo[] bubbleTextInfoHolder;
    int index = -1;
    bool isHintPlaced;
    public BubbleTextInfo[] BubbleTextInfoHolder { get => bubbleTextInfoHolder; set => bubbleTextInfoHolder = value; }
    public bool IsHintPlaced { get => HandleHintBool(); set => isHintPlaced = value; }
    int hintIndex;
    /// <summary>
    /// Hold speech bubble content [Command, Speech, Hint Blink]
    /// </summary>
    /// <returns></returns>
    public BubbleTextInfo GetNextInfo()
    {
        if (bubbleTextInfoHolder.Length <= 0)
        {
            throw new System.Exception("There is no text added for bubble text holder");
        }
        if ( IsHintPlaced )
        {
            return bubbleTextInfoHolder [hintIndex];
        }
        else
        {
            index = Mathf.Clamp (index + 1 , 0 , bubbleTextInfoHolder.Length - 1);
            return bubbleTextInfoHolder [index];
        }
    }
    bool HandleHintBool () {
        bool up = isHintPlaced;
        isHintPlaced = false;
        return up;
    }
    public void SetNextHint (int _index) {
        if ( _index >= 0 && _index < bubbleTextInfoHolder.Length )
        {
            hintIndex = _index;
            IsHintPlaced = true;
        }
    }
}