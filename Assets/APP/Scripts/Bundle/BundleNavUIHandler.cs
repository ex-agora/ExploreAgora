using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BundleNavUIHandler : MonoBehaviour
{
    [SerializeField] List<GameObject> bundlePanels;
    [SerializeField] GameObject playNowP;
    [SerializeField] GameObject bundleViewP;
    int currentBundle = -1;

    public int CurrentBundle { get => currentBundle; set => currentBundle = value; }
    public void FreeCurrent() {
        currentBundle = -1;
        UXFlowManager.Instance.SetCurrentBundle(currentBundle);
    }
    public void SetCurrent(int _current)
    {
        currentBundle = _current;
        UXFlowManager.Instance.SetCurrentBundle(currentBundle);
    }
    public void OpenCurrentBundle(int _current) {
        currentBundle = _current;
        bundleViewP.SetActive(false);
        playNowP.SetActive(true);
        for (int i = 0; i < bundlePanels.Count; i++)
        {
            if (i == _current)
                bundlePanels[i].SetActive(true);
            else
                bundlePanels[i].SetActive(false);
        }

    }
}
