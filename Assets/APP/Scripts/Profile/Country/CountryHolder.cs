using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CountryHolder 
{
    CountriesCodedInfo countries;
    static CountryHolder instance = new CountryHolder();

    public static CountryHolder Instance { get => instance; set => instance = value; }

    private CountryHolder() {
        string clubDataProjectFilePath = "/StreamingAssets/countries without changes.json";
        string filePath = Application.dataPath + clubDataProjectFilePath;
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            countries = new CountriesCodedInfo();
            countries.countries = JsonMapper.ToObject<CountryCodedInfo[]>(dataAsJson);           
        }
    }
    public CountriesCodedInfo GetCountries() {
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