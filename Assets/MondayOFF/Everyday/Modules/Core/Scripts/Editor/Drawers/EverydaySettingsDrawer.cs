using UnityEngine;
using UnityEditor;

namespace MondayOFF {
    [CustomEditor(typeof(EverydaySettings))]
    public class EverydaySettingsDrawer : Editor {
        const float IMAGE_HEIGHT = 200f;

        public override void OnInspectorGUI() {
            var logo = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/MondayOFF/Everyday/Textures/Editor/mondayoff_logo_transparency.png");
            GUI.DrawTexture(new Rect(0, 0, EditorGUIUtility.currentViewWidth, IMAGE_HEIGHT), logo, ScaleMode.ScaleToFit, true, 0f);
            GUILayout.Space(IMAGE_HEIGHT - 50f);

            var versionLabel = new GUIStyle(EditorStyles.largeLabel);
            versionLabel.fontSize = 12;
            versionLabel.fontStyle = FontStyle.Italic;
            versionLabel.alignment = TextAnchor.MiddleCenter;
            EditorGUILayout.LabelField($"v{EveryDay.Version}", versionLabel);
            GUILayout.Space(EditorGUIUtility.singleLineHeight * 4);

            var boldLabel = new GUIStyle(EditorStyles.largeLabel);
            boldLabel.fontSize = 16;
            boldLabel.fontStyle = FontStyle.Bold;
            boldLabel.alignment = TextAnchor.MiddleCenter;
            boldLabel.fixedHeight = EditorGUIUtility.singleLineHeight * 1.5f;
            EditorGUILayout.LabelField("Please click the button below to open settings window", boldLabel);
            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            var buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.alignment = TextAnchor.MiddleCenter;
            buttonStyle.fontSize = 16;
            buttonStyle.fontStyle = FontStyle.Bold;
            if (GUILayout.Button("Open Settings Window", buttonStyle, GUILayout.Height(100f))) {
                EverydaySettingsWindow.Open();
            }
        }
    }
}