﻿using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CountryHolder :MonoBehaviour
{
    CountriesCodedInfo countries;
    static CountryHolder instance;

    public static CountryHolder Instance { get => instance; set => instance = value; }
    private void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }
  
    void CountryHolderCreating(){
        //string clubDataProjectFilePath =Path.Combine( Application.streamingAssetsPath,"countries without changes.json");
        //string filePath = clubDataProjectFilePath;
        //if (File.Exists(filePath))
        //{
        //    string dataAsJson = File.ReadAllText(filePath);
        //    countries = new CountriesCodedInfo();
        //    countries.countries = JsonMapper.ToObject<CountryCodedInfo[]>(dataAsJson);           
        //}
        TextAsset file = Resources.Load("Countries/countries without changes") as TextAsset;
        string dataAsJson = file.ToString();
        countries = new CountriesCodedInfo();
        countries.countries = JsonMapper.ToObject<CountryCodedInfo[]>(dataAsJson);    
    }
    public CountriesCodedInfo GetCountries() {
        CountryHolderCreating();
        return countries;
    }
}


[System.Serializable]
public class CountryCodedInfo {
    public string country;
    public string code;
}
[System.Serializable]
public class CountriesCodedInfo
{
    public CountryCodedInfo[] countries;
}