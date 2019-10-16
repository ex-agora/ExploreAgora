using LitJson;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CountriesEnumCreatorWindow : EditorWindow
{
    #region Fields
    private List<LanguageConfig> configs = new List<LanguageConfig>();
    private bool groupEnabled;
    private bool myBool = true;
    private float myFloat = 1.23f;
    #endregion Fields

    #region Methods

    [MenuItem("Window/Countries Enum Creator")]
    public static void ShowWindow()
    { GetWindow<CountriesEnumCreatorWindow>("Countries Enum Creator"); }

    private void OnGUI()
    {
        if (GUILayout.Button("Create Enum"))
        {
            CreateEnum();
         
        }
        
       
    }
    void CreateEnum() {
        CountriesInfo countriesInfo = LoadGameData();
        if (countriesInfo == null)
            return;
        string code = "[System.Serializable]\npublic enum  Countries {\n";

        foreach (var i in countriesInfo.countries)
        {
            
            code += "\t" + CheckValueFormate(i.country) + ",\n";
        }
        code += "}\n";
        string filePath = Application.dataPath + "/Amir/Scripts/Enum/ECountries.cs";
        Debug.Log(filePath);
        File.WriteAllText(filePath, code);
    }
    string CheckValueFormate(string s)
    {
        string str = s.Replace(" ", "");
        str = str.Replace("(", "");
        str = str.Replace(")", "");
        str = str.Replace(",", "");
        str = str.Replace(".", "");
        str = str.Replace("-", "");
        return str;
    }
    private CountriesInfo LoadGameData()
    {
        string clubDataProjectFilePath = "/StreamingAssets/Countries.json";
        string filePath = Application.dataPath + clubDataProjectFilePath;
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            CountriesInfo s = JsonMapper.ToObject<CountriesInfo>(dataAsJson);
            Debug.Log(s.countries.Length);
            return s;
        }
        Debug.Log("File does not Exist for Countries!!!");
        return null;
    }
    #endregion Methods
}

[System.Serializable]
class CountryInfo
{
    public string country;
}
[System.Serializable]
class CountriesInfo
{
    public CountryInfo[] countries;
}