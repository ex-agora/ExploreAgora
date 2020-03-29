using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeysUsageHandler : MonoBehaviour
{
    [SerializeField] private Button keyBtn;
    [SerializeField] private ProfileInfoContainer profile;
    private ScanChecker _scan;

    public ScanChecker _Scan { get => _scan; set => _scan = value; }
    public void CheckProfileKeys() => keyBtn.interactable = profile.keys > 0;
    public void UseKeyForScanning() => _scan.UseFavoritismScanningKey();
}
