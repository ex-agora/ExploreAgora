/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using UnityEngine;
using UnityEditor;

namespace SIS
{
    public class ShowDatabaseEditor : EditorWindow
    {
        private GUIStyle style = new GUIStyle();
        private Vector2 scrollPos;
        private string data;


        [MenuItem("Window/Simple IAP System/Show Database", false, 2)]
        static void Init()
        {
            EditorWindow.GetWindowWithRect(typeof(ShowDatabaseEditor), new Rect(0, 0, 350, 200), false, "DB Display");
        }


        void OnEnable()
        {
            style.contentOffset = new Vector2(5, 0);
            style.wordWrap = true;
            data = PlayerPrefs.GetString(DBManager.prefsKey, "");
            GUIUtility.keyboardControl = 0;
        }


        void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Currently stored on this device:", EditorStyles.boldLabel);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, true);
            float textSize = style.CalcHeight(new GUIContent(data), 345f);
            textSize *= 1.15f;

            if (string.IsNullOrEmpty(data))
                EditorGUILayout.LabelField("- nothing here -");
            else
                EditorGUILayout.SelectableLabel(data, style, GUILayout.MinHeight(textSize), GUILayout.ExpandHeight(true));

            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("Refresh"))
            {
                OnEnable();
            }
        }
    }
}
