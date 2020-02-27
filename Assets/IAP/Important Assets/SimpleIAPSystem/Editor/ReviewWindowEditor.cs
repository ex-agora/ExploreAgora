using System.IO;
using UnityEditor;
using UnityEngine;

namespace SIS
{
    [InitializeOnLoad]
    public class ReviewWindowEditor : EditorWindow
    {
        private static Texture2D reviewWindowImage;
        private static string imagePath = "/EditorFiles/Asset_smallLogo.png";


        [MenuItem("Window/Simple IAP System/Review Asset", false, 4)]
        static void Init()
        {
            EditorWindow.GetWindowWithRect(typeof(ReviewWindowEditor), new Rect(0, 0, 256, 320), false, "Review Asset");
        }


        void OnGUI()
        {
            if (reviewWindowImage == null)
            {
                var script = MonoScript.FromScriptableObject(this);
                string path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(script));
                reviewWindowImage = AssetDatabase.LoadAssetAtPath(path + imagePath, typeof(Texture2D)) as Texture2D;
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(30);
            GUILayout.Label(reviewWindowImage);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(40);
            EditorGUILayout.LabelField("Review Simple IAP System", EditorStyles.boldLabel, GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Please consider giving us a rating on the");
            EditorGUILayout.LabelField("Unity Asset Store. Your support helps us");
            EditorGUILayout.LabelField("to improve this product. Thank you!");
            EditorGUILayout.Space();

            if (GUILayout.Button("Review now!", GUILayout.Height(40)))
            {
                Help.BrowseURL("https://assetstore.unity.com/packages/add-ons/services/billing/simple-iap-system-12343?aid=1011lGiF&pubref=editor_sis");
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("If you are looking for support, please");
            EditorGUILayout.LabelField("head over to our support forum instead.");
        }
    }
}