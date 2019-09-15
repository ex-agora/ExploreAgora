using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class First : MonoBehaviour
{
    // string assetName = "cube (1)";
    // string assetName1 = "Cube";
    // string assetName = "factory";
    string path;
    GameObject anyObj;
    public Transform CAM;
    TextAsset bundleLoadRequestText2;
    // Start is called before the first frame update
    void Start()
    {
        // print("AnaFirst");
        path = Application.persistentDataPath + "/Test Asset Bundle/" + "sphere" + ".txt";
        print(path);
        // path = "/Test Asset Bundle/" + name;
        // path = name;

        bundleLoadRequestText2 = Resources.Load("sphere") as TextAsset;
        File.WriteAllBytes(path, bundleLoadRequestText2.bytes);
        // LoadAssetBundle(assetName1);
    }

    [ContextMenu("sss")]
    public void LoadAssetBundle()
    {
        byte[] hh;
        if (File.Exists(path))
        {
            print("YES");
        }
        else
        {
            print("NO");
        }
        hh = File.ReadAllBytes(path);
        // AssetBundle bundleLoadRequest2 = AssetBundle.LoadAsset("cube (1)");
        // AssetBundle bundleLoadRequest2 = AssetBundle.LoadFromFile("E:\\UnityProjects\\Hypatia\\ExploreAgora\\Assets\\Resources\\cube (1)");

        AssetBundle bundleLoadRequest2 = AssetBundle.LoadFromMemory(hh);
      

        // AssetBundle bundleLoadRequest2 = AssetBundle.LoadFromFile(path);
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
    [ContextMenu("sss")]
    public void LoadAssetBundle(bool ss)
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
        var prefab2 = bundleLoadRequest2.LoadAsset<GameObject>("sphere");
        prefab2 = Instantiate(prefab2);
        prefab2.name = "sphere";
        bundleLoadRequest2.Unload(false);
    }
}