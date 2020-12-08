/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using UnityEngine;
using UnityEditor;
using System.Collections;

namespace SIS
{
    /// <summary>
    /// Requirement editor. Embedded in the IAP Settings editor,
    /// for defining requirements for locked IAP items
    /// </summary>
    public class RequirementEditor : EditorWindow
    {
        //IAP object to modify, set by IAPEditor
        public IAPObject obj;


        void OnGUI()
        {
            if (obj == null)
                return;

            //draw label for selected IAP name
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("IAP:", GUILayout.Width(40));
            EditorGUILayout.LabelField(obj.id, EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Locked Settings", EditorStyles.boldLabel);
            //draw fields for database entry id and its target to reach
            //when an entry has been entered in its field, color them yellow (active) 
            if (!string.IsNullOrEmpty(obj.req.entry))
                GUI.backgroundColor = Color.yellow;
            obj.req.entry = EditorGUILayout.TextField(new GUIContent("Storage Entry [?]", "Entry in DBManager which stores the required " +
                                                                     "value in your game, e.g. 'level', 'score', etc."), obj.req.entry);
            obj.req.target = EditorGUILayout.IntField(new GUIContent("Target Value [?]", "Value to reach of the entry defined above"), obj.req.target);

            //draw field for defining an "what to do to unlock" text
            GUI.backgroundColor = Color.white;
            obj.req.labelText = EditorGUILayout.TextField(new GUIContent("Locked Text [?]", "Text that describes what the player has to do to fulfill " +
                                                                         "the requirement. Displayed in the prefab's 'lockedLabel' Text component, if set"), obj.req.labelText);

			EditorGUILayout.Space();
            EditorGUILayout.LabelField("Upgrades", EditorStyles.boldLabel);
            if (!string.IsNullOrEmpty(obj.req.nextId))
                GUI.backgroundColor = Color.yellow;
            obj.req.nextId = EditorGUILayout.TextField(new GUIContent("Next product [?]", "Product Id of the upgrade that comes after this one"), obj.req.nextId);
            GUI.backgroundColor = Color.white;
        }
    }
}
