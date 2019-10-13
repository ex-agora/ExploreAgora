using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(AgoraCategories))]
public class AgoraCategoriesEditor : Editor
{
    public Text input;
    List<string> list = new List<string>();

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        AgoraCategories category = (AgoraCategories)target;
        if (GUILayout.Button("AddNewCategory"))
        {
            category.AddNewCategory();
        }
        if (GUILayout.Button("Delete Category"))
        {
            category.DeleteCategory();
        }
        EditorGUILayout.LabelField("Categories", EditorStyles.boldLabel);
        if (AgoraCategories.categories.Count > 0)
        {
            list = AgoraCategories.categories;
            for (int i = 0; i < list.Count; i++)
            {
                EditorGUILayout.LabelField(list[i]);
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}
