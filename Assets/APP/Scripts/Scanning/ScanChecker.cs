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

    public void CheckScannedObject()
    {
        int counter = bundleHandler.GetScannedCounter(objectToScanName);
        if (counter > 0)
        {
            stateImage.sprite = unlockedState;

            for (int i = 0; i < experiences.Count; i++)
                experiences[i].UnlockExperience();
        }
    }
}