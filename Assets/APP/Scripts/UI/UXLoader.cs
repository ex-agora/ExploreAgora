using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UXLoader : MonoBehaviour
{
    [SerializeField] GameObject uxPrefab;
    void Start()
    {
        Invoke(nameof(LoadUX), 0.5f);
    }
    void LoadUX() {
        
        Instantiate(uxPrefab, Vector3.zero, Quaternion.identity);
    }
}
