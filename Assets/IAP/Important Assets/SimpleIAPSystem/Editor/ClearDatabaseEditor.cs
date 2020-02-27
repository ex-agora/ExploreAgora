/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using UnityEditor;
using UnityEngine;

namespace SIS
{
    public class ClearDatabaseEditor : Editor
    {
        [MenuItem("Window/Simple IAP System/Clear Database", false, 3)]
        public static void Clear()
        {
            if (EditorUtility.DisplayDialog("Clear Local Database Entries",
               "Are you sure you want to clear the PlayerPref data for this project? (This includes Simple IAP System data, but also all other PlayerPrefs)", "Clear", "Cancel"))
            {
                string unityPurchasingPath = System.IO.Path.Combine(System.IO.Path.Combine(Application.persistentDataPath, "Unity"), "UnityPurchasing");
                if(System.IO.Directory.Exists(unityPurchasingPath))
                    System.IO.Directory.Delete(unityPurchasingPath, true);

                DBManager.ClearAll();
                if(DBManager.GetInstance() != null)
                    DBManager.GetInstance().Init();
            }
        }
    }
}
