using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace MondayOFF {
    internal class EverydayEventTrackerMenuItem : MonoBehaviour {
        [MenuItem("Everyday/!! Strip Firebase !!", false, 500)]
        private static void StripFirebase() {
            if (!EditorUtility.DisplayDialog("Delete Firebase", "Delete Firebase from the project?", "Ok", "Cancel")) {
                return;
            }

            string[] FirebaseAssets = new string[]{
                "Assets/Editor Default Resources",
                "Assets/Firebase",
                "Assets/Parse",
                "Assets/MondayOFF/ThirdParties/Firebase",
                "Assets/GeneratedLocalRepo"
            };

            bool hasDeleted = false;
            foreach (var item in FirebaseAssets) {
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