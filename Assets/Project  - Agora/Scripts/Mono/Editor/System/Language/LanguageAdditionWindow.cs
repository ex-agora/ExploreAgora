using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class LanguageAdditionWindow : EditorWindow
{
   //LanguageConfig temp = ScriptableObject.CreateInstance<LanguageConfig>();
    SystemLanguage systemLanguage;
    LanguageType languageType;
    public static void ShowWindow()
    {
        GetWindow<LanguageAdditionWindow>("Add Language");

    }
    private void OnGUI()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox); //Declaring our first part of our layout, and adding a bit of flare with EditorStyles.

        GUILayout.Label("Languages", EditorStyles.boldLabel); //Making a label in our vertical view, declaring its contents, and adding editor flare.

        EditorGUILayout.BeginHorizontal(); //Adding a horizontal view to indent our next line so that the properties look like children, LOTS of things you can add like toggles and stuff to reduce this clutter.
                                           //configs[i] = (LanguageConfig)EditorGUILayout.ObjectField(configs[i], typeof(LanguageConfig), true); //Declaring our object as an object field, with the type of object we want, and allowing scene object.

        GUILayout.Space(15); //The indent for our next line, in pixels.
                             //the lifeTime property of cubemap can be assigned to a GUI element here.
        systemLanguage = (SystemLanguage)EditorGUILayout.Popup((int)systemLanguage, System.Enum.GetNames(typeof(SystemLanguage))); //Declaring our object as an object field, with the type of object we want, and allowing scene object.
        languageType = (LanguageType)EditorGUILayout.Popup((int)languageType,System.Enum.GetNames(typeof(LanguageType))); //Declaring our object as an object field, with the type of object we want, and allowing scene object.
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        if (GUILayout.Button("ADD")) {
            if (CheckLanguages())
            {
                LanguageConfig t = ScriptableObject.CreateInstance<LanguageConfig>();
                t.LanguageName = systemLanguage;
                t.Type = languageType;
                AssetDatabase.CreateAsset(t, $"Assets/Resources/SO/Language/LanguageConfig-{systemLanguage.ToString()}.asset");
                LanguageManager.Instance?.LanguagesUpdate();
                Close();
            }
            else {
                Debug.LogError($"{systemLanguage.ToString()} is already created before!!");
            }
        }
        
    }
   
    bool CheckLanguages() {
        string[] aFilePaths = Directory.GetFiles("Assets/Resources/SO/Language/");
        foreach (string sFilePath in aFilePaths)
        {
            if (Path.GetExtension(sFilePath) == ".asset")
            {
                Debug.Log(Path.GetExtension(sFilePath));

                var t = AssetDatabase.LoadAssetAtPath<LanguageConfig>(sFilePath);
                if (t.LanguageName == systemLanguage)
                    return false;

                //Here try something like INSTANTIATE(objAsset);
            }
        }
        return true;
    }
    private void OnDestroy()
    {
        GetWindow<LanguageCreatorWindow>("Language Creator");
    }
}
