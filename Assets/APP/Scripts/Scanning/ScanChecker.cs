using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScanChecker : MonoBehaviour
{
    [SerializeField] private string objectToScanName;
    [SerializeField] private Sprite outlineSp;
    [SerializeField] private List<ExperienceHandler> experiences;
    [SerializeField] private BundleHandler bundleHandler;
    [SerializeField] private Sprite unlockedState;
    [SerializeField] private Image stateImage;
    [SerializeField] ScanProperties scanProperties;
    [SerializeField] Image objImg;
    [SerializeField] Sprite objActiveSprite;
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
            objImg.sprite = objActiveSprite;
            for (int i = 0; i < experiences.Count; i++)
                experiences[i].UnlockExperience();
        }
    }
    public void StartScan()
    {
        scanProperties.detectionObjectName = objectToScanName;
        scanProperties.detectionObjectSp = objActiveSprite;
        scanProperties.outlineSp = outlineSp;
        SceneLoader.Instance.LoadExperience("Scan Scene");
        //NetworkManager.Instance.CheckInternetConnectivity(OnSuccessScan, OnFailedScan);
    }

    void OnSuccessScan(NetworkParameters np)
    {
       
    }
    void OnFailedScan(NetworkParameters np) {
        Debug.Log(np.err.message);
    }

    public void UseFavoritismScanningKey() => bundleHandler.ApplyFavoritismScanningKey(objectToScanName);
}