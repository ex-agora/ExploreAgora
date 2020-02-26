using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendShapAnimManager : MonoBehaviour
{
    [SerializeField] List<BlendShapeHandler> shapeHandlers;
    [SerializeField] float duration = 0.05f;
    [SerializeField] bool isOnes;
    float elpTime;
    float step;
    float updateRate = 0.05f;
    bool isPlayed = false;
    public void PlayAnim() {
        if (!isPlayed || !isOnes)
        {
            for (int i = 0; i < shapeHandlers.Count; i++)
            {
                shapeHandlers[i].KayValue = shapeHandlers[i].MaxKeyValue;
            }
            isPlayed = true;
            elpTime = 0;
            step = (shapeHandlers[0].MaxKeyValue / duration) * updateRate;
            InvokeRepeating(nameof(CustomUpdate), 0, updateRate);
        }
    }
    void CustomUpdate() {
        for (int i = 0; i < shapeHandlers.Count; i++) {
            shapeHandlers[i].KayValue -= step;
        }
        elpTime += updateRate;
        if (elpTime >= duration) {
            CancelInvoke(nameof(CustomUpdate));
        }
    }
}
