using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingManager : MonoBehaviour
{
    private static TestingManager _instance;
    [SerializeField] private bool isTestStart;
    public static TestingManager Instance
    {
        get => _instance;
        set => _instance = value;
    }

    public bool IsTestStart
    {
        get => isTestStart;
        set => isTestStart = value;
    }

    private void Awake()
    {
        if (_instance is null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
