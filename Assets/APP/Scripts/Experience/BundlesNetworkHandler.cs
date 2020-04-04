﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BundlesNetworkHandler : MonoBehaviour
{
    [SerializeField] List<BundleStateHandler> bundles;
    Dictionary<string, BundleStateHandler> bundleMap;
    Dictionary<string, string> bundleMapKey;
    private void Start()
    {
        bundleMap = new Dictionary<string, BundleStateHandler>();
        for (int i = 0; i < bundles.Count; i++)
        {
            bundleMap.Add(bundles[i].BundleName, bundles[i]);
        }
        GetAllBunldes();
    }
    public void GetUserBunlde()
    {
        NetworkManager.Instance.GetBundlesData(OntGetBundleSuccess, OntGetBundleFailed);
    }
    private void OntGetBundleSuccess(NetworkParameters obj)
    {
        BundleResponse br = (BundleResponse)obj.responseData;
        for (int i = 0; i < br.bundles.Count; i++)
        {
           
            print(br.bundles[i].bundleId);
            BundleStateHandler _bundle = null;
            string key = string.Empty;
            for (int j = 0; j < br.bundles[i].collectedTokens.Count; j++)
            {
                if (bundleMapKey.TryGetValue(br.bundles[i].bundleId, out key))
                {
                    if (bundleMap.TryGetValue(key, out _bundle))
                    {
                        _bundle.ActiveToken(br.bundles[i].collectedTokens[i]);
                    }
                }
            }

        }
    }
    private void OntGetBundleFailed(NetworkParameters obj)
    {
        print(obj.err.message);
    }

    ////////////////////////////////////////////////////////
    public void GetAllBunldes()
    {
        NetworkManager.Instance.GetExperienceBundlesData(OntGetAllBundleSuccess, OntGetAllBundleFailed);
    }
    private void OntGetAllBundleSuccess(NetworkParameters obj)
    {
        ExperienceBundlesResponse br = (ExperienceBundlesResponse)obj.responseData;
        print(br.bundles);
        BundleStateHandler _bundle = null;
        bundleMapKey = new Dictionary<string, string>();
        for (int i = 0; i < br.bundles.Count; i++)
        {
            if (bundleMap.TryGetValue(br.bundles[i].name, out _bundle)){
                _bundle.Id = br.bundles[i]._id;
                bundleMapKey.Add(_bundle.Id, _bundle.BundleName);

            }
        }
        GetUserBunlde();

    }
    private void OntGetAllBundleFailed(NetworkParameters obj)
    {
        print(obj.err.message);
    }
}
