using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LanguageCreatorWindow : EditorWindow
{
    #region Fields
    private List<LanguageConfig> configs = new List<LanguageConfig>();
    private bool groupEnabled;
    private bool myBool = true;
    private float myFloat = 1.23f;
    #endregion Fields

    #region Methods

    [MenuItem("Window/Language Creator")]
    public static void ShowWindow()
    { GetWindow<LanguageCreatorWindow>("Language Creator"); }

    private void OnGUI()
    {
        configs.Clear();
        GUILayout.Label("Create New Language Config ", EditorStyles.boldLabel);
        if (GUILayout.Button("Add Language"))
        {
            LanguageAdditionWindow.ShowWindow();
            Close();
        }
        string[] aFilePaths = Directory.GetFiles("Assets/Resources/SO/Language/");
        foreach (string sFilePath in aFilePaths)
        {
            if (Path.GetExtension(sFilePath) == ".asset")
            { configs.Add(AssetDatabase.LoadAssetAtPath<LanguageConfig>(sFilePath)); }
        }
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.Label("Languages", EditorStyles.boldLabel);
        for (int i = 0; i < configs.Count; i++)
        {
            if (configs[i] != null)
            {
                EditorGUILayout.BeginHorizontal();
                //configs[i] = (LanguageConfig)EditorGUILayout.ObjectField(configs[i], typeof(LanguageConfig), true); //Declaring our object as an object field, with the type of object we want, and allowing scene object.
                EditorGUILayout.LabelField(configs[i].LanguageName.ToString());
                GUILayout.Space(15);
                if (GUILayout.Button("Delete"))
                {
                    var pathA = AssetDatabase.GetAssetPath(configs[i]);
                    AssetDatabase.DeleteAsset(pathA);
                    LanguageManager.Instance?.LanguagesUpdate();
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndVertical();
    }
    #endregion Methods
}