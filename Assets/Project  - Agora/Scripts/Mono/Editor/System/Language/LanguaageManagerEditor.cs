using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
[CustomEditor(typeof(LanguageManager))]
public class LanguageManagerEdior : Editor
{
    
    int _choiceIndex = 0;

    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        EditorGUILayout.BeginHorizontal(); //Adding a horizontal view to indent our next line so that the properties look like children, LOTS of things you can add like toggles and stuff to reduce this clutter.
        GUILayout.Label("Current Language",EditorStyles.boldLabel); //Making a label in our vertical view, declaring its contents, and adding editor flare.
        GUILayout.Space(15); //The indent for our next line, in pixels.
        var manager = target as LanguageManager;
        _choiceIndex = Mathf.Clamp(_choiceIndex, 0, manager.Configs.Count - 1);
        _choiceIndex = EditorGUILayout.Popup(_choiceIndex, manager.Configs.Select(i => i.LanguageName.ToString()).ToArray());
        // Update the selected choice in the underlying object
        manager.SelectedLanguage = manager.Configs.Count!=0?_choiceIndex:-1;
        // Save the changes back to the object
        EditorUtility.SetDirty(target);                  //the lifeTime property of cubemap can be assigned to a GUI element here.
        EditorGUILayout.EndHorizontal();


    }
}

