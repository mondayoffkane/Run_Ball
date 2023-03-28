using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MondayOFF {
    [CustomPropertyDrawer(typeof(ScenePickerAttribute))]
    internal class ScenePickerPropertyDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            var sceneCount = SceneManager.sceneCountInBuildSettings;
            if (sceneCount > 0) {
                string[] sceneNames = new string[sceneCount];

                for (int i = 0; i < sceneCount; i++) {
                    if (i == currentSceneIndex) {
                        sceneNames[i] = "-- You cannot transfer to current scene --";
                    } else {
                        sceneNames[i] = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
                    }
                }

                var currentSelection = property.intValue;
                property.intValue = EditorGUI.Popup(position, "Scene to transfer", currentSelection, sceneNames);
            } else {
                EditorGUI.LabelField(position, "ERROR: No scene is listed in the build settings");
            }
        }
    }
}