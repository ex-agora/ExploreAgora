using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DetermineObject : MonoBehaviour
{
    #region Fields
    [SerializeField] string detectiopossibilities;
    [SerializeField] ScanProperties scanProperties;
    #endregion Fields

    #region Methods
    private void StartScan()
    {
        scanProperties.detectionObjectName = detectiopossibilities.ToString();
        SceneManager.LoadScene("Scan Scene");
    }
    #endregion Methods
}
