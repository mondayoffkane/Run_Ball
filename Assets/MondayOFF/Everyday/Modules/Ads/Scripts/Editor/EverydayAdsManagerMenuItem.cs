using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace MondayOFF {
    internal class EverydayAdsManagerMenuItem : MonoBehaviour {
        [MenuItem("Everyday/!! Strip AdMob !!", false, 500)]
        private static void StripAdMob() {
            if (!EditorUtility.DisplayDialog("Delete AdMob", "Delete AdMob adapter from the project?", "Ok", "Cancel")) {
                return;
            }

            string[] AdMobAssets = new string[]{
                "Assets/MaxSdk/Mediation/Google",
                "Assets/MaxSdk/Mediation/GoogleAdManager",
                "Assets/Plugins/Android/MaxMediationGoogle.androidlib",
            };

            bool hasDeleted = false;
            foreach (var item in AdMobAssets) {
                if (Directory.Exists(item)) {
                    if (Directory.EnumerateFileSystemEntries(item).Any()) {
                        Directory.Delete(item, true);
                        hasDeleted = true;
                    } else {
                        Directory.Delete(item, false);
                    }
                    var metaPath = item + ".meta";
                    if (File.Exists(metaPath)) {
                        File.Delete(item + ".meta");
                        hasDeleted = true;
                    }
                }
            }

            if (hasDeleted) {
                AssetDatabase.Refresh();
            }
        }
    }
}