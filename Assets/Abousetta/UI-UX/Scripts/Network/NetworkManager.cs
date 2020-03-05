using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager instance;

    public static NetworkManager Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool CheckInternetConnectivity()
    {
        // Check internet connection.
        //TODO
        return true;
    }

    public bool Login(string _username,string _password)
    {
        return true;
    }

    public bool ValidatePromocode(string _promocode)
    {
        return true;
    }
}