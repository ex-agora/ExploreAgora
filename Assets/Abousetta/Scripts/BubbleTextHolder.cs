using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleTextHolder : MonoBehaviour
{
    [SerializeField] BubbleTextInfo[] bubbleTextInfoHolder;
    int index = -1;
    public BubbleTextInfo[] BubbleTextInfoHolder { get => bubbleTextInfoHolder; set => bubbleTextInfoHolder = value; }

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
        index = Mathf.Clamp(index + 1, 0, bubbleTextInfoHolder.Length - 1);
        return bubbleTextInfoHolder[index];
    }
}