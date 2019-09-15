using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

public class MyBehaviour : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GetAssetBundle());
    }

    // IEnumerator GetAssetBundle()
    // {
    //     UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle("https://rabahagoratest.000webhostapp.com/sphere");
    //     yield return www.SendWebRequest();

    //     if (www.isNetworkError || www.isHttpError)
    //     {
    //         Debug.Log(www.error);
    //     }
    //     else
    //     {
    //         AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
    //         Instantiate(bundle);
    //     }
    // }
    IEnumerator GetAssetBundle()
    {
        WWW download;
        string url = "https://rabahagoratest.000webhostapp.com/sphere.manifest";

        while (!Caching.ready)
            yield return null;

        download = WWW.LoadFromCacheOrDownload(url, 0);

        yield return download;

        AssetBundle assetBundle = download.assetBundle;
        if (assetBundle != null)
        {
            // Alternatively you can also load an asset by name (assetBundle.Load("my asset name"))
            Object go = assetBundle.mainAsset;

            if (go != null)
            {
                Instantiate(go);
                print(go.name);
            }
            else
                Debug.Log("Couldn't load resource");
        }
        else
        {
            Debug.Log("Couldn't load resource");
        }
    }
}