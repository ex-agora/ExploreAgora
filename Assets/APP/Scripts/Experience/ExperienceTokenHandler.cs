using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceTokenHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateBundleToken(string tokenName, string bundleID)
    {
        CollectBundleTokenData collectBundleTokenData = new CollectBundleTokenData();
        collectBundleTokenData.bundleId = bundleID;
        collectBundleTokenData.tokenName = tokenName;
        NetworkManager.Instance.UpdateCollectedTokens(collectBundleTokenData, OnUpdateBundleSuccess, OnUpdateBundleFailed);
    }
    private void OnUpdateBundleSuccess(NetworkParameters obj)
    {
        
    }
    private void OnUpdateBundleFailed(NetworkParameters obj)
    {
        print(obj.err.message);
    }
}
