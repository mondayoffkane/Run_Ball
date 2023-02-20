using UnityEngine;
using UnityEditor;

namespace MondayOFF {
    internal class EverydayMenuItem {
        [MenuItem("Everyday/Open Everyday Settings", false, 100)]
        private static void FocusAdUnitIDs() {
            var adUnitIDs = AssetDatabase.FindAssets("t:EverydaySettings");

            if (adUnitIDs.Length != 1) {
                Debug.LogError("[EVERYDAY] There are zero or more than two Objects! " + adUnitIDs.Length);
                return;
            }

            UnityEditor.Selection.activeObject = AssetDatabase.LoadAssetAtPath<EverydaySettings>(AssetDatabase.GUIDToAssetPath(adUnitIDs[0]));
        }

        [MenuItem("Everyday/ - Clear Player Prefs", false, 800)]
        private static void ClearPlayerPrefs() {
            if (EditorUtility.DisplayDialog("Delete PlayerPrefs", "Clear all Player Prefs?", "Ok", "Cancel")) {
                PlayerPrefs.DeleteAll();
                Debug.Log($"[EVERYDAY] Deleted all PlayerPrefs");
            }
        }

        [MenuItem("Everyday/ - Capture Screen", false, 800)]
        private static void CaptureScreen() {
            var desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            var path = System.IO.Path.Combine(desktopPath, $"Screenshot_{Application.identifier}_{System.DateTime.Now.Ticks}.png");
            ScreenCapture.CaptureScreenshot(path);
            Debug.Log($"[EVERYDAY] Screenshot saved: {path}");
        }
    }
}