using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(LanguageTextSupport), true)]
public class LanguageTextSupportEditor : Editor
{
    //public override void OnInspectorGUI()
    //{
    //    serializedObject.Update();
    //    string[] enumNames = System.Enum.GetNames(typeof(Language));
    //    foreach (var enumName in enumNames)
    //    {
    //        DrawlanguageText(enumName);
    //    }
    //    serializedObject.ApplyModifiedProperties();
    //}
    //void DrawlanguageText(string enumName)
    //{
    //    SerializedProperty languageTextProperty = GetlanguageText(enumName);
    //    SerializedProperty valueProperty = languageTextProperty.FindPropertyRelative("translatedText");
    //    EditorGUILayout.PropertyField(valueProperty, new GUIContent(enumName));
    //}
    //SerializedProperty GetlanguageText(string enumName) 
    //{
    //    SerializedProperty languagesProperty = serializedObject.FindProperty("languages");
        
    //    for (int i = 0; i < languagesProperty.arraySize; i++)
    //    {
    //        var languageProperty= languagesProperty.GetArrayElementAtIndex(i);
    //        var nameProperty = languageProperty.FindPropertyRelative("languageName");
    //        if (string.Equals(enumName, nameProperty.stringValue))
    //        {
    //            return languageProperty;
    //        }
    //    }
    //    // Didn't find in array, so add it:
    //    languagesProperty.arraySize++;
    //    var languageeProperty = languagesProperty.GetArrayElementAtIndex(languagesProperty.arraySize - 1);
    //    languageeProperty.FindPropertyRelative("languageName").stringValue = enumName;
    //    return languageeProperty;
    //}
}
