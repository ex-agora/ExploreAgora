using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendShapAnimManager : MonoBehaviour
{
    [SerializeField] List<BlendShapeHandler> shapeHandlers;
    [SerializeField] float duration = 0.1f;
    float elpTime;
    float step;
    float updateRate = 0.5f;
    public void PlayAnim() {
        elpTime = 0;
        step = (duration / shapeHandlers[0].MaxKeyValue) * updateRate;
        InvokeRepeating(nameof(CustomUpdate), 0, updateRate);
    }
    void CustomUpdate() {
        for (int i = 0; i < shapeHandlers.Count; i++) {
            shapeHandlers[i].KayValue += step;
        }
        elpTime += updateRate;
        if (elpTime >= duration) {
            CancelInvoke(nameof(CustomUpdate));
        }
    }
}
