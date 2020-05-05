using System;
using UnityEditor;
using UnityEngine;

namespace HeurekaGames.AssetHunterPRO
{
    public class AH_EditorData : ScriptableObject
    {
        private static AH_EditorData m_instance;
        public static AH_EditorData Instance
        {
            get
            {
                if (!m_instance)
                {
                    m_instance = loadData();
                }

                return m_instance;
            }
        }

        private static AH_EditorData loadData()
        {
            //LOGO ON WINDOW
            string[] configData = AssetDatabase.FindAssets("EditorData t:" + typeof(AH_EditorData).ToString(), null);
            if (configData.Length >= 1)
            {
                return AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(configData[0]), typeof(AH_EditorData)) as AH_EditorData;
            }

            Debug.LogError("Failed to find config data");
            return null;
        }

        public delegate void EditorDataRefreshDelegate();
        public static event EditorDataRefreshDelegate OnEditorDataRefresh;

        public GUIStyle HeadlineStyle;
        public UnityEditor.DefaultAsset Documentation;

        [SerializeField] public ConfigurableIcon WindowPaneIcon = new ConfigurableIcon();
        [SerializeField] public ConfigurableIcon WindowHeaderIcon = new ConfigurableIcon();
        [SerializeField] public ConfigurableIcon SceneIcon = new ConfigurableIcon();
        [SerializeField] public ConfigurableIcon Settings = new ConfigurableIcon();
        [SerializeField] public ConfigurableIcon LoadLogIcon = new ConfigurableIcon();
        [SerializeField] public ConfigurableIcon GenerateIcon = new ConfigurableIcon();
        [SerializeField] public ConfigurableIcon RefreshIcon = new ConfigurableIcon();
        [SerializeField] public ConfigurableIcon MergerIcon = new ConfigurableIcon();
        [SerializeField] public ConfigurableIcon HelpIcon = new ConfigurableIcon();
        [SerializeField] public ConfigurableIcon AchievementIcon = new ConfigurableIcon();
        [SerializeField] public ConfigurableIcon ReportIcon = new ConfigurableIcon();
        [SerializeField] public ConfigurableIcon DeleteIcon = new ConfigurableIcon();
        

        internal void RefreshData()
        {
            if (OnEditorDataRefresh != null)
                OnEditorDataRefresh();
        }
    }

    [System.Serializable]
    public class ConfigurableIcon
    {
        [SerializeField] private bool isUsingDarkSkin = false;

        [SerializeField] string buildInIconName = "";
        [SerializeField] private Texture iconCached = null;

        [SerializeField] Texture m_iconNormalOverride = null;
        [SerializeField] Texture m_iconProSkinOverride = null;

        public ConfigurableIcon()
        {
            AH_EditorData.OnEditorDataRefresh += onEditorDataRefresh;
        }

        private void onEditorDataRefresh()
        {
            iconCached = null;
        }

        public Texture Icon
        {
            get
            {
                //TODO A way to make sure we update, if the user have changed skin
                if (isUsingDarkSkin != EditorGUIUtility.isProSkin)
                {
                    iconCached = null;
                    isUsingDarkSkin = EditorGUIUtility.isProSkin;
                }
                return (iconCached != null) ? iconCached : (iconCached = getInvertedForProSkin());
            }
        }

        [SerializeField]
        bool m_darkSkinInvert = false;

        private Texture getInvertedForProSkin()
        {
            Texture imageToUse = (EditorGUIUtility.isProSkin) ? m_iconProSkinOverride : m_iconNormalOverride;

            //If we want to use default unity icons and nothing has been setup to override
            if (imageToUse == null && !string.IsNullOrEmpty(buildInIconName))
                if (EditorGUIUtility.IconContent(buildInIconName) != null)
                    imageToUse = EditorGUIUtility.IconContent(buildInIconName).image;

            //Return current image if we dont have proskin, or dont want to invert
            if (!EditorGUIUtility.isProSkin || (EditorGUIUtility.isProSkin && !m_darkSkinInvert))
                return imageToUse;

            Texture2D readableTexture = getReadableTexture(imageToUse);
            Texture2D inverted = new Texture2D(readableTexture.width, readableTexture.height, TextureFormat.ARGB32, false);
            for (int x = 0; x < readableTexture.width; x++)
            {
                for (int y = 0; y < readableTexture.height; y++)
                {
                    Color origColor = readableTexture.GetPixel(x, y);
                    Color invertedColor = new Color(1 - origColor.r, 1 - origColor.g, 1 - origColor.b, origColor.a);
                    inverted.SetPixel(x, y, (origColor.a > 0) ? invertedColor : origColor);
                }
            }
            inverted.Apply();
            return inverted;
        }

        private Texture2D getReadableTexture(Texture imageToUse)
        {
            // Create a temporary RenderTexture of the same size as the texture
            RenderTexture tmp = RenderTexture.GetTemporary(
                                imageToUse.width,
                                imageToUse.height,
                                0,
                                RenderTextureFormat.Default,
                                RenderTextureReadWrite.Linear);

            // Blit the pixels on texture to the RenderTexture
            Graphics.Blit(imageToUse, tmp);
            // Backup the currently set RenderTexture
            RenderTexture previous = RenderTexture.active;
            // Set the current RenderTexture to the temporary one we created
            RenderTexture.active = tmp;
            // Create a new readable Texture2D to copy the pixels to it
            Texture2D myTexture2D = new Texture2D(imageToUse.width, imageToUse.height);
            // Copy the pixels from the RenderTexture to the new Texture
            myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
            myTexture2D.Apply();
            // Reset the active RenderTexture
            RenderTexture.active = previous;
            // Release the temporary RenderTexture
            RenderTexture.ReleaseTemporary(tmp);

            return myTexture2D;
        }
    }
}