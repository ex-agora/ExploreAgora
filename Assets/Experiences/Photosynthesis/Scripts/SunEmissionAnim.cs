using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunEmissionAnim : MonoBehaviour
{
    [SerializeField] MeshRenderer coreSun;
    [SerializeField] MeshRenderer raiseSun1;
    [SerializeField] MeshRenderer raiseSun2;
    [SerializeField] float minEmission;
    [SerializeField] float maxEmission;
    [SerializeField] float duration;
    [ColorUsage(true, true)] [SerializeField] Color emissionColor;
    float elpTime;
    float updateRate = 0.05f;
    float step;
    bool up;
    float intensity;
    public void PlayAnim() {
        emissionColor = coreSun.material.GetColor("_EmissionColor");
        elpTime = 0.0f;
        up = true;
        intensity = 1;
        step = ((maxEmission - minEmission)/duration) * 2 * updateRate;
        InvokeRepeating(nameof(CustomUpdate), 0, updateRate);
    }
    void CustomUpdate() {
        elpTime += updateRate;
        if (up && elpTime >= (duration/2)) { step = -step; up = false; }
        intensity += step;
        coreSun.material.SetColor("_EmissionColor",emissionColor * intensity);
        raiseSun1.material.SetColor("_EmissionColor",emissionColor * intensity);
        raiseSun2.material.SetColor("_EmissionColor",emissionColor * intensity);
        if (elpTime >= duration) {
            coreSun.material.SetColor("_EmissionColor", emissionColor);
            raiseSun1.material.SetColor("_EmissionColor", emissionColor);
            raiseSun2.material.SetColor("_EmissionColor", emissionColor);
            CancelInvoke(nameof(CustomUpdate));
        }
    }
}
