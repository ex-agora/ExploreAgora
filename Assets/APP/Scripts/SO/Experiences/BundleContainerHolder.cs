using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Bundle Data" , menuName = "SO/App/Experience/BundleData" , order = 0)]

public class BundleContainerHolder : ScriptableObject
{
    public string bundleName;
    public string bundleCode;
    [TextArea]
    public string bundleDescription;
    public ExperienceContainerHolder [] experiences;
    public uint reqiredCoins;
    public int MissionNumber { get => experiences.Length; }
    public string [] subjects;
    public string [] requiredObjectsToScan;
}
