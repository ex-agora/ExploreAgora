using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScanChecker : MonoBehaviour
{
    [SerializeField] private string objectToScanName;
    [SerializeField] private List<ExperienceHandler> experiences;
    [SerializeField] private BundleHandler bundleHandler;
    [SerializeField] private Sprite unlockedState;
    [SerializeField] private Image stateImage;
    [SerializeField] ScanProperties scanProperties;
    private void Start()
    {
        CheckScannedObject();
    }
    public void CheckScannedObject()
    {
        int counter = bundleHandler.GetScannedCounter(objectToScanName);

        for (int i = 0; i < experiences.Count; i++)
            experiences[i].ActiveExperience();

        if (counter > 0)
        {
            stateImage.sprite = unlockedState;

            for (int i = 0; i < experiences.Count; i++)
                experiences[i].UnlockExperience();
        }
    }
    public void StartScan()
    {
        NetworkManager.Instance.CheckInternetConnectivity(OnSuccessScan, OnFailedScan);
    }

    void OnSuccessScan(NetworkParameters np)
    {
        scanProperties.detectionObjectName = objectToScanName;
        SceneLoader.Instance.LoadExperience("Scan Scene");
    }
    void OnFailedScan(NetworkParameters np) {

        Debug.Log(np.err.message);
    }
}