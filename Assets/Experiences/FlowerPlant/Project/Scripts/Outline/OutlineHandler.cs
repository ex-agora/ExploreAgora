using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineHandler : MonoBehaviour
{
    [SerializeField] bool isMaterialBased;
    [SerializeField] Outline outlineSpript;
    [SerializeField] MeshRenderer mesh;
    [SerializeField] Material defaultMat;
    [SerializeField] Material outlineMat;
    float duration;
    float repeatRate = 0.05f;
    float step;
    public void SetColor(Color _Color) {
        if (isMaterialBased) {
            outlineMat.SetColor("_Color1", _Color);
        }
        else {
            outlineSpript.OutlineColor = _Color;
        }
    }
    public void ShowOutline() {
        if (isMaterialBased) {
            mesh.material = outlineMat;
        }
        else {
            outlineSpript.OutlineWidth = 2f;
            outlineSpript.enabled = true;
        }
    }
    public void HideOutline() {
        if (isMaterialBased) {
            mesh.material = defaultMat;
        }
        else {
            duration = 0.3f;
            step = 2.0f / (duration / repeatRate);
            InvokeRepeating(nameof(CustomUpdate), 0, repeatRate);
        }
    }
    void CustomUpdate() {
        duration -= repeatRate;
        outlineSpript.OutlineWidth = Mathf.Lerp(outlineSpript.OutlineWidth, 0, step);
        if (duration <= 0.0f) {
            outlineSpript.enabled = false;
            CancelInvoke(nameof(CustomUpdate));
        }
    }
}
