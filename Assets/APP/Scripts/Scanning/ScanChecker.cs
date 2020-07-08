using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScanChecker : MonoBehaviour
{
    [SerializeField] private string objectToScanName;
    //[SerializeField] private Sprite outlineSp;
    [SerializeField] private List<ExperienceHandler> experiences;
    [SerializeField] private ScanProperties properties;
    [SerializeField] private BundleHandler bundleHandler;
    [SerializeField] private Sprite unlockedState;
    [SerializeField] private Image stateImage;
    //[SerializeField] ScanProperties scanProperties;
    [SerializeField] Image objImg;
    //[SerializeField] Sprite objActiveSprite;
    //[SerializeField] List<DetectObjectInfo> objectInfos;
    [SerializeField] Text scanObjectTxt;
    bool isLoaded;
    private void OnEnable()
    {
        scanObjectTxt.text = StringUtility.LetterCapitalize(objectToScanName);
        if(isLoaded)
            CheckScannedObject();
    }
    public void CheckScannedObject()
    {
        if (!gameObject.activeInHierarchy) {
            isLoaded = true;
            return;
        }
        int counter = bundleHandler.GetScannedCounter(objectToScanName);

        for (int i = 0; i < experiences.Count; i++)
            experiences[i].ActiveExperience();

        if (counter > 0)
        {
            stateImage.sprite = unlockedState;
            objImg.sprite = properties.detectionObjectSp;
            for (int i = 0; i < experiences.Count; i++)
                experiences[i].UnlockExperience();
        }
        isLoaded = false;
    }
    public void StartScan()
    {
        ScanPropertiesHolder.Instance.DetectionObjectName = objectToScanName;
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