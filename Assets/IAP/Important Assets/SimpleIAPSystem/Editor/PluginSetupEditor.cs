/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

namespace SIS
{
    public class PluginSetupEditor : EditorWindow
    {
        private bool isChanged = false;
        private bool isServiceOn = false;
        private bool isServiceImported = false;
        private bool isPackageImported = false;
        private bool isIAPEnabled = false;
        private FileInfo[] purchasingFiles = null;

        #if UNITY_2018_1_OR_NEWER
        private ListRequest pckList;
        #endif

        private BuildTargetIAP targetIAPGroup;
        private string[] iapNames = new string[] { "", "SIS_IAP" };
        private enum BuildTargetIAP
        {
            Standalone = 1,
            iOS = 2,
            Android = 4,
            WebGL = 8,
            WSA = 16,
            tvOS = 32,
            Facebook = 64
        }

        private AndroidPlugin androidPlugin = AndroidPlugin.UnityIAP;
        private string[] androidPluginNames = new string[] { "", "OCULUS_GEAR_IAP" };
        private enum AndroidPlugin
        {
            UnityIAP = 0,
            GearVR = 1
        }

        private DesktopPlugin desktopPlugin = DesktopPlugin.UnityIAP;
        private string[] desktopPluginNames = new string[] { "", "PLAYFAB_PAYPAL", "PLAYFAB_STEAM", "OCULUS_RIFT_IAP", "STEAM_IAP" };
        private enum DesktopPlugin
        {
            UnityIAP = 0,
            PlayfabPaypal = 1,
            PlayfabSteam = 2,
            OculusRift = 3,
            Steam = 4
        }

        private WebPlugin webPlugin = WebPlugin.UnityIAP;
        private string[] webPluginNames = new string[] { "", "PLAYFAB_PAYPAL" };
        private enum WebPlugin
        {
            UnityIAP = 0,
            PlayfabPaypal = 1
        }

        private PlayfabPlugin playfabPlugin = PlayfabPlugin.Disabled;
        private string[] playfabPluginNames = new string[] { "", "PLAYFAB_VALIDATION", "PLAYFAB" };
        private enum PlayfabPlugin
        {
            Disabled = 0,
            ValidationOnly = 1,
            FullSuite = 2
        }


        [MenuItem("Window/Simple IAP System/Plugin Setup", false, 0)]
        static void Init()
        {
            EditorWindow.GetWindowWithRect(typeof(PluginSetupEditor), new Rect(0, 0, 360, 325), false, "Plugin Setup");
        }


        void OnEnable()
        {
            targetIAPGroup = (BuildTargetIAP)(-1);
            androidPlugin = (AndroidPlugin)FindScriptingDefineIndex(BuildTargetGroup.Android, androidPluginNames);
            desktopPlugin = (DesktopPlugin)FindScriptingDefineIndex(BuildTargetGroup.Standalone, desktopPluginNames);
            webPlugin = (WebPlugin)FindScriptingDefineIndex(BuildTargetGroup.WebGL, webPluginNames);

            //check if cross-platform use exists
            playfabPlugin = (PlayfabPlugin)FindScriptingDefineIndex(BuildTargetGroup.Android, playfabPluginNames);

            //check Unity IAP package installation
            isServiceOn = false;
            isServiceImported = false;
            isPackageImported = false;
            isIAPEnabled = false;

            DirectoryInfo rootDir = new DirectoryInfo("Assets/Plugins/UnityPurchasing");
            purchasingFiles = null;
            if(rootDir.Exists)
                purchasingFiles = rootDir.GetFiles("UnityIAP.unitypackage");

            #if UNITY_2018_1_OR_NEWER
                pckList = Client.List(true);
            #endif
        }


        void OnGUI()
        {
            CheckUnityIAP();

            EditorGUI.BeginChangeCheck();
            bool unityIAPActive = isServiceOn && isServiceImported && isPackageImported;
            EditorGUI.BeginDisabledGroup(unityIAPActive == false);

            GUILayout.Space(10);
            EditorGUILayout.LabelField("Unity IAP Activation", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Billing Platforms: ", GUILayout.Width(145));
            targetIAPGroup = (BuildTargetIAP)EditorGUILayout.EnumFlagsField(targetIAPGroup);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Activate Unity IAP: ", GUILayout.Width(145));
            isIAPEnabled = EditorGUILayout.Toggle(isIAPEnabled);
            EditorGUILayout.EndHorizontal();

            EditorGUI.BeginDisabledGroup(isIAPEnabled == false);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Store Plugin Setup", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Choose the store plugins you would like to use per platform:");
            EditorGUILayout.Space();
            
            androidPlugin = (AndroidPlugin)EditorGUILayout.EnumPopup("Android", androidPlugin);
            desktopPlugin = (DesktopPlugin)EditorGUILayout.EnumPopup("Standalone", desktopPlugin);
            webPlugin = (WebPlugin)EditorGUILayout.EnumPopup("WebGL", webPlugin);

            GUILayout.Space(15);
            playfabPlugin = (PlayfabPlugin)EditorGUILayout.EnumPopup("PlayFab Service", playfabPlugin);

            EditorGUI.EndDisabledGroup();
            EditorGUI.EndDisabledGroup();
            if (EditorGUI.EndChangeCheck())
            {
                isChanged = true;
            }

            if (isChanged)
                GUI.color = Color.yellow;

            GUILayout.Space(10);
            if (GUILayout.Button("Apply", GUILayout.Height(30)))
            {
                ApplyScriptingDefines();
                isChanged = false;
            }
        }


        void CheckUnityIAP()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Unity IAP Status", EditorStyles.boldLabel);
            if (GUILayout.Button("Refresh Status"))
            {
                OnEnable();
                return;
            }
            EditorGUILayout.EndHorizontal();

            //Check Services Window
            GUI.contentColor = Color.white;
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Service Enabled: ", GUILayout.Width(115));
            #if UNITY_PURCHASING
                isServiceOn = true;
            #endif
            GUI.contentColor = isServiceOn == true ? Color.green : Color.red;
            GUILayout.Label(isServiceOn == true ? "OK" : "NOT OK");
            EditorGUILayout.EndHorizontal();

            //Check Unity Purchasing folder
            GUI.contentColor = Color.white;
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Plugin Imported: ", GUILayout.Width(115));
            if (purchasingFiles != null && purchasingFiles.Length == 0)
                    isServiceImported = true;

            GUI.contentColor = isServiceImported == true ? Color.green : Color.red;
            GUILayout.Label(isServiceImported == true ? "OK" : "NOT OK");
            EditorGUILayout.EndHorizontal();

            //Check Unity PackageManager package
            GUI.contentColor = Color.white;
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Package Imported: ", GUILayout.Width(115));
            #if UNITY_2018_1_OR_NEWER
            if (pckList == null || !pckList.IsCompleted)
            {
                GUILayout.Label("CHECKING...");
                EditorGUILayout.EndHorizontal();
                return;
            }
            else
            {
                PackageCollection col = pckList.Result;
                foreach (UnityEditor.PackageManager.PackageInfo info in col)
                {
                    if (info.packageId.StartsWith("com.unity.purchasing", System.StringComparison.Ordinal))
                    {
                        isPackageImported = true;
                        break;
                    }
                }
            }
            #else
                isPackageImported = true;
            #endif

            GUI.contentColor = isPackageImported == true ? Color.green : Color.red;
            GUILayout.Label(isPackageImported == true ? "OK" : "NOT OK");
            EditorGUILayout.EndHorizontal();
            GUI.contentColor = Color.white;
        }


        void ApplyScriptingDefines()
        {
            List<BuildTargetIAP> selectedElements = new List<BuildTargetIAP>();
            System.Array arrayElements = System.Enum.GetValues(typeof(BuildTargetIAP));
            for(int i = 0; i < arrayElements.Length; i++)
            {
                int layer = 1 << i;
                if (((int)targetIAPGroup & layer) != 0)
                {
                    selectedElements.Add((BuildTargetIAP)arrayElements.GetValue(i));
                }
            }

            for (int i = 0; i < selectedElements.Count; i++)
                SetScriptingDefine((BuildTargetGroup)System.Enum.Parse(typeof(BuildTargetGroup), selectedElements[i].ToString()), iapNames, isIAPEnabled ? 1 : 0);

            SetScriptingDefine(BuildTargetGroup.Android, androidPluginNames, (int)androidPlugin);
            SetScriptingDefine(BuildTargetGroup.Standalone, desktopPluginNames, (int)desktopPlugin);
            SetScriptingDefine(BuildTargetGroup.WebGL, webPluginNames, (int)webPlugin);

            BuildTargetGroup[] playfabTargets = new BuildTargetGroup[] { BuildTargetGroup.Android, BuildTargetGroup.iOS, BuildTargetGroup.tvOS,
                                                                         BuildTargetGroup.Standalone, BuildTargetGroup.WebGL, BuildTargetGroup.Facebook };

            for (int i = 0; i < playfabTargets.Length; i++)
                SetScriptingDefine(playfabTargets[i], playfabPluginNames, (int)playfabPlugin);
        }


        void SetScriptingDefine(BuildTargetGroup target, string[] oldDefines, int newDefine)
        {
            string str = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);
            List<string> defs = new List<string>(str.Split(';'));
            if (defs.Count == 0 && !string.IsNullOrEmpty(str)) defs.Add(str);

            for (int i = 0; i < oldDefines.Length; i++)
            {
                if (string.IsNullOrEmpty(oldDefines[i])) continue;
                defs.Remove(oldDefines[i]);
            }

            if (newDefine > 0)
                defs.Add(oldDefines[newDefine]);

            str = "";
            for (int i = 0; i < defs.Count; i++)
                str = defs[i] + ";" + str;

            PlayerSettings.SetScriptingDefineSymbolsForGroup(target, str);
        }


        int FindScriptingDefineIndex(BuildTargetGroup group, string[] names)
        {
            string str = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);

            for (int i = 1; i < names.Length; i++)
            {
                if (str.Contains(names[i]))
                {
                    return i;
                }
            }

            return 0;
        }
    }
}