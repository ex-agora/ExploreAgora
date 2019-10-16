using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DetermineObject : MonoBehaviour
{
    [SerializeField] Detectiopossibilities detectiopossibilities;
    [SerializeField] ScanProperties scanProperties;

    private void Awake()
    {
        scanProperties.detectionObjectName = detectiopossibilities.ToString();
        SceneManager.LoadScene("Scan Scene");
    }
}
