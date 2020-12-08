using System;
using UnityEditor;
using UnityEngine;

namespace HeurekaGames.AssetHunterPRO
{
    public static class AH_WindowStyler
    {
        public static readonly Color clr_Pink = new Color((226f / 256f), (32f / 256f), (140f / 256f), 1);
        public static readonly Color clr_Dark = new Color((48f / 256f), (41f / 256f), (47f / 256f), 1);
        public static readonly Color clr_dBlue = new Color((47f / 256f), (102f / 256f), (144f / 256f), 1);
        public static readonly Color clr_lBlue = new Color((58f / 256f), (124f / 256f), (165f / 256f), 1);
        public static readonly Color clr_White = new Color((217f / 256f), (220f / 256f), (214f / 256f), 1);

        public static void DrawGlobalHeader(EditorWindow window, Color color, string label, bool addVersionNum)
        {
            if (window == null)
                return;

            EditorGUI.DrawRect(new Rect(0, 0, window.position.width, 24), color);
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(AH_EditorData.Instance.WindowHeaderIcon.Icon, GUILayout.MaxHeight(22));
            if (AH_EditorData.Instance.HeadlineStyle != null)
                GUILayout.Label(label, AH_EditorData.Instance.HeadlineStyle);

            if (addVersionNum)
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.Space();
                GUILayout.Label(AH_Window.VERSION, EditorStyles.whiteLabel);
                EditorGUILayout.EndVertical();
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        internal static void DrawCenteredImage(EditorWindow window, Texture image)
        {
            if (window == null)
                return;

            GUI.Box(new Rect((window.position.width * .5f) - (image.width * .5f), (window.position.height * .5f) - (image.height * .5f), image.width, image.height), image, GUIStyle.none);
        }

        internal static void DrawCenteredMessage(EditorWindow window, float msgWidth, float msgHeight, string messsage)
        {
            if (window == null)
                return;

            Vector2 outerBoxSize = new Vector2(msgWidth, msgHeight);
            float frameWidth = 5;
            Vector2 innerBoxSize = new Vector2(outerBoxSize.x - frameWidth * 2, outerBoxSize.y - frameWidth * 2);

            Vector2 rectStartPos = new Vector2((window.position.width * .5f) - (outerBoxSize.x * .5f), (window.position.height * .5f) - (outerBoxSize.y * .5f) + (AH_EditorData.Instance.WindowHeaderIcon.Icon.height * .5f));

            EditorGUI.DrawRect(new Rect(rectStartPos.x, rectStartPos.y, outerBoxSize.x, outerBoxSize.y), AH_WindowStyler.clr_White);
            EditorGUI.DrawRect(new Rect(rectStartPos.x + frameWidth, rectStartPos.y + frameWidth, innerBoxSize.x, innerBoxSize.y), AH_WindowStyler.clr_dBlue);

            float bounds = 20;
            Vector2 logoStartPos = rectStartPos + new Vector2(bounds, bounds);
            GUI.Box(new Rect(logoStartPos.x, logoStartPos.y, AH_EditorData.Instance.WindowHeaderIcon.Icon.width, AH_EditorData.Instance.WindowHeaderIcon.Icon.height), AH_EditorData.Instance.WindowHeaderIcon.Icon, GUIStyle.none);

            Vector2 labelStartPos = logoStartPos + new Vector2(AH_EditorData.Instance.WindowHeaderIcon.Icon.width + frameWidth * 2, 0);
            float textWidth = innerBoxSize.x - AH_EditorData.Instance.WindowHeaderIcon.Icon.width - (bounds * 2);
            float textHeight = innerBoxSize.y - AH_EditorData.Instance.WindowHeaderIcon.Icon.height - (bounds * 2);
            GUI.Label(new Rect(labelStartPos.x, labelStartPos.y, textWidth, textHeight), messsage, AH_EditorData.Instance.HeadlineStyle);
        }
    }
}