/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using UnityEngine;
using UnityEditor;
using System.Collections;

//our about/help/support editor window
public class AboutSISEditor : EditorWindow
{
    [MenuItem("Window/Simple IAP System/About", false, 5)]
    static void Init()
    {
        AboutSISEditor aboutWindow = (AboutSISEditor)EditorWindow.GetWindowWithRect
                (typeof(AboutSISEditor), new Rect(0, 0, 300, 300), false, "About");
        aboutWindow.Show();
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect((Screen.width / 2) - 60, 20, 250, 100));
        GUILayout.Label("Simple IAP System");
        GUILayout.Label("by Rebound Games");
        GUILayout.EndArea();
        GUILayout.Space(70);

        GUILayout.Label("Info", EditorStyles.boldLabel);
		GUILayout.BeginHorizontal();
        GUILayout.Label("Homepage");
        if (GUILayout.Button("Visit", GUILayout.Width(100)))
        {
            Help.BrowseURL("www.rebound-games.com");
        }
        GUILayout.EndHorizontal();
		
        GUILayout.BeginHorizontal();
        GUILayout.Label("YouTube");
        if (GUILayout.Button("Visit", GUILayout.Width(100)))
        {
            Help.BrowseURL("https://www.youtube.com/user/ReboundGamesTV");
        }
        GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
        GUILayout.Label("Twitter");
        if (GUILayout.Button("Visit", GUILayout.Width(100)))
        {
            Help.BrowseURL("https://twitter.com/Rebound_G");
        }
        GUILayout.EndHorizontal();
		GUILayout.Space(5);

	
        GUILayout.Label("Support", EditorStyles.boldLabel);
		GUILayout.BeginHorizontal();
        GUILayout.Label("Script Reference");
        if (GUILayout.Button("Visit", GUILayout.Width(100)))
        {
            Help.BrowseURL("www.rebound-games.com/docs/sis/");
        }
        GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
        GUILayout.Label("Support Forum");
        if (GUILayout.Button("Visit", GUILayout.Width(100)))
        {
            Help.BrowseURL("www.rebound-games.com/forum/");
        }
        GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
        GUILayout.Label("Unity Forum");
        if (GUILayout.Button("Visit", GUILayout.Width(100)))
        {
            Help.BrowseURL("http://forum.unity3d.com/threads/194975-Simple-IAP-System-(SIS)");
        }
        GUILayout.EndHorizontal();
		GUILayout.Space(5);
		
        GUILayout.Label("Support us!", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Review Asset");

		if (GUILayout.Button("Visit", GUILayout.Width(100)))
        {
            Help.BrowseURL("https://assetstore.unity.com/packages/add-ons/services/billing/simple-iap-system-12343?aid=1011lGiF&pubref=editor_sis");
        }
		GUILayout.EndHorizontal();
		
    }
}