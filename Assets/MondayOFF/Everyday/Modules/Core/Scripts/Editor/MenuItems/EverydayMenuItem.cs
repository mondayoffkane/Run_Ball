using UnityEngine;
using UnityEditor;

namespace MondayOFF {
    internal class EverydayMenuItem {
        [MenuItem("Everyday/Open Everyday Settings Window", false, 100)]
        private static void OpenSettingsWindow() {
            EverydaySettingsWindow.Open();
        }

        [MenuItem("Everyday/ - Clear Player Prefs", false, 800)]
        private static void ClearPlayerPrefs() {
            if (EditorUtility.DisplayDialog("Delete PlayerPrefs", "Clear all Player Prefs?", "Ok", "Cancel")) {
                PlayerPrefs.DeleteAll();
                EverydayLogger.Info($"Deleted all PlayerPrefs");
            }
        }

        [MenuItem("Everyday/ - Capture Screen", false, 800)]
        private static void CaptureScreen() {
            var desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            var path = System.IO.Path.Combine(desktopPath, $"Screenshot_{Application.identifier}_{System.DateTime.Now.Ticks}.png");
            ScreenCapture.CaptureScreenshot(path);
            EverydayLogger.Info($"Screenshot saved: {path}");
        }
    }
}