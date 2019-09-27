using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(ExperienceSystemHandler))]
public class CategoryEditor : Editor
{
    public Text input;
    List<string> list = new List<string>();

    public override void OnInspectorGUI()
    {
        ExperienceSystemHandler category = (ExperienceSystemHandler)target;
        DrawDefaultInspector();
        if (GUILayout.Button("AddNewCategory"))
        {
            category.AddNewCategory();
        }
        serializedObject.ApplyModifiedProperties();
    }
}
