using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanPropertiesHolder : MonoBehaviour
{
    static ScanPropertiesHolder instance;
    string detectionObjectName;
    Sprite detectionObjectSp;
    Sprite outlineSp;
    List<DetectObjectInfo> objectInfos;
    bool shouldContinueToExperience;
    [SerializeField] List<ScanProperties> properties;
    Dictionary<string, ScanProperties> propertiesObjects;
    public static ScanPropertiesHolder Instance { get => instance; set => instance = value; }
   
    public string DetectionObjectName { get => detectionObjectName; set => detectionObjectName = value; }
    public bool ShouldContinueToExperience { get => shouldContinueToExperience; set => shouldContinueToExperience = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        propertiesObjects = new Dictionary<string, ScanProperties>();
        int size = properties.Count;
        for (int i = 0; i < size; i++) {
            propertiesObjects.Add(properties[i].detectionObjectName, properties[i]);
        }
    }

    public ScanProperties GetPropertie(string objectName) {
        ScanProperties temp;
        if (propertiesObjects.TryGetValue(objectName, out temp))
            return temp;
        return null;
    }

    public ScanProperties GetPropertie()
    {
        ScanProperties temp;
        if (propertiesObjects.TryGetValue(detectionObjectName, out temp))
            return temp;
        return null;
    }
}
