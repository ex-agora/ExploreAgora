using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendShapeHandler : MonoBehaviour
{

    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;
    Mesh skinnedMesh;
    [SerializeField] Material mat;
    [Range(1,100)][SerializeField]float kayValue = 0f;
    [SerializeField] int maxKeyValue;
    public float KayValue { get => kayValue; set {kayValue = value; HandleModel(); } }
    public int MaxKeyValue { get => maxKeyValue; set => maxKeyValue = value; }

    void Awake()
    {

        skinnedMesh = skinnedMeshRenderer.sharedMesh;
        
    }
    private void Start()
    {
        KayValue = MaxKeyValue;
    }
    void HandleModel() {
        skinnedMeshRenderer.SetBlendShapeWeight(0, kayValue);
        mat.SetFloat("_Alpha", kayValue / 100f);
    }
    private void OnValidate()
    {
        KayValue = kayValue;
    }
}
