using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // string assetPath = Application.streamingAssetsPath + "/Spheres/Spheres";
    // string assetPath1 = "E:/UnityProjects/Hypatia/ExploreAgora/Assets/Rabah/Asset Bundles"+"/Spheres/sphere.1";

    string sphereName = "Sphere";
    string factory = "factory";
    string GameObject = "GameObject (1)";
    public GameObject sphere;
    public GameObject anyObj;
    public List<Object> assetBundleData;
    string path;
    public Transform CAM;
    public string assetName;
    void Start()
    {
        if (assetName == "")
        {
            assetName = GameObject;
            // anyObj = Resources.Load(assetName) as GameObject;
        }
        path = Application.persistentDataPath + "/Test Asset Bundle/" + assetName;
        // sphereData = new Object[2];

        // sphereData[0] = Resources.Load(sphereName) as Object;
        // sphereData[1] = Resources.Load("New Material") as Material;
        // assetBundleData[0] = Resources.Load("New Material") as Material;
        CreateAssetBundle();
    }
    public void CreateAssetBundle()
    {
        anyObj = Resources.Load(assetName) as GameObject;
        path = Application.persistentDataPath + "/Test Asset Bundle/" + assetName + ".txt";
        // Object[] Data = new Object[assetBundleData.Count];
        // for (int i = 0; i < assetBundleData.Count; i++)
        // {
        //     Data[i] = assetBundleData[i];
        // }
        // BuildPipeline.BuildAssetBundle(anyObj, Data, path, BuildAssetBundleOptions.CollectDependencies, BuildTarget.Android);
        TextAsset bundleLoadRequestText2 = Resources.Load(assetName + ".txt") as TextAsset;
        File.WriteAllBytes(path, bundleLoadRequestText2.bytes);

    }
    // private void OnEnable() {

    // }
    [ContextMenu("sss")]
    public void LoadAssetBundle()
    {
        AssetBundle bundleLoadRequest2 = AssetBundle.LoadFromFile(path);
        if (bundleLoadRequest2 == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }
        else
        {
            Debug.Log("Done!");
        }
        var prefab2 = bundleLoadRequest2.LoadAsset<GameObject>(assetName);
        prefab2 = Instantiate(prefab2);
        prefab2.name = assetName;
        bundleLoadRequest2.Unload(false);
    }
    public void hh(FirstCapsule c)
    {
        print(c.gameObject.name);
    }
    [ContextMenu("sss")]
    public void LoadAssetBundle(bool aa)
    {
        byte[] hh = File.ReadAllBytes(path);
        // AssetBundle bundleLoadRequest2 = AssetBundle.LoadAsset("cube (1)");
        // AssetBundle bundleLoadRequest2 = AssetBundle.LoadFromFile("E:\\UnityProjects\\Hypatia\\ExploreAgora\\Assets\\Resources\\cube (1)");

        AssetBundle bundleLoadRequest2 = AssetBundle.LoadFromMemory(hh);
        if (bundleLoadRequest2 == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }
        else
        {
            Debug.Log("Done!");
        }
        var prefab2 = bundleLoadRequest2.LoadAsset<GameObject>(name);
        prefab2 = Instantiate(prefab2);
        prefab2.name = name;
        prefab2.transform.parent = CAM;
        prefab2.transform.localPosition = Vector3.forward * 10;
        bundleLoadRequest2.Unload(false);
    }
}
