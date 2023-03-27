﻿#if UNITY_ANDROID
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

namespace MondayOFF {
    public class EverydayAssetPostProcessAndroid : AssetPostprocessor {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths
#if UNITY_2021_2_OR_NEWER
        , bool didDomainReload
#endif
        ) {
            CopyAndroidManifest();
            CopyGradleTemplate("gradleTemplate.properties");
            CopyGradleTemplate("mainTemplate.gradle");
            CopyGradleTemplate("launcherTemplate.gradle");
            EnableMultiDex();
        }

        private static void CopyAndroidManifest() {
            const string manifestPath = "Assets/Plugins/Android/AndroidManifest.xml";
            if (!File.Exists(manifestPath)) {
                string unityManifestPath;
#if UNITY_EDITOR_OSX
                unityManifestPath = Directory.GetParent(EditorApplication.applicationPath).FullName;
#else
                unityManifestPath = Path.Combine(Directory.GetParent(EditorApplication.applicationPath).FullName, "Data");
#endif
                unityManifestPath = Path.Combine(unityManifestPath, "PlaybackEngines/AndroidPlayer/Apk");

#if UNITY_2019_1_OR_NEWER
                unityManifestPath = Path.Combine(unityManifestPath, "UnityManifest.xml");
#else
                unityManifestPath = Path.Combine(unityManifestPath, "AndroidManifest.xml");
#endif

                if (!File.Exists(unityManifestPath)) {
                    var playerSettingsWindow = SettingsService.OpenProjectSettings("Project/Player");
                    Debug.LogError("[EVERYDAY] Failed to locate Unity default manifest! Enable ProjectSettings > Player > Publishing Settings > Custom Main Manifest");
                    return;
                }

                File.Copy(unityManifestPath, manifestPath);
                Debug.Log("[EVERYDAY] AndroidManifest.xml not found in Plugins/Android. Copying Unity default manifest to Plugins/Android.");
            }
        }

        private static void CopyGradleTemplate(string fileName) {
            string targetPath = Path.Combine("Assets/Plugins/Android", fileName);
            if (!File.Exists(targetPath)) {
                string unityTemplatePath;
#if UNITY_EDITOR_OSX
                unityTemplatePath = Directory.GetParent(EditorApplication.applicationPath).FullName;
#else
                gradlePropertyFile = Path.Combine(Directory.GetParent(EditorApplication.applicationPath).FullName, "Data");
#endif
                unityTemplatePath = Path.Combine(unityTemplatePath, "PlaybackEngines/AndroidPlayer/Tools/GradleTemplates");
                unityTemplatePath = Path.Combine(unityTemplatePath, fileName);

                if (!File.Exists(unityTemplatePath)) {
                    var playerSettingsWindow = SettingsService.OpenProjectSettings("Project/Player");
                    Debug.LogError($"[EVERYDAY] Failed to locate Unity Default {fileName}! Enable ProjectSettings > Player > Publishing Settings > {fileName}");
                    return;
                }

                File.Copy(unityTemplatePath, targetPath);
                Debug.Log($"[EVERYDAY] {fileName} found in Plugins/Android. Copying Unity default properties to Plugins/Android.");
            }
        }

        private static void EnableMultiDex() {
            try {
                const string targetPath = "Assets/Plugins/Android/launcherTemplate.gradle";

                const string unityLibToken = "implementation project(\':unityLibrary\')";
                const string versionNameToken = "\'**VERSIONNAME**\'";

                const string implementMultidex = "    implementation \"androidx.multidex:multidex:2.0.1\"";
                const string multidexEnabled = "        multiDexEnabled true";

                bool hasMultidexLib = false;
                bool isMultidexEnabled = false;

                var fileContent = File.ReadAllLines(targetPath).ToList();
                foreach (var item in fileContent) {
                    if (item.Contains(implementMultidex)) {
                        hasMultidexLib = true;
                    } else if (item.Contains(multidexEnabled)) {
                        isMultidexEnabled = true;
                    }
                }

                bool isModified = false;
                for (int i = 0; i < fileContent.Count; ++i) {
                    var line = fileContent[i];
                    if (!hasMultidexLib && line.Contains(unityLibToken)) {
                        fileContent.Insert(i + 1, implementMultidex);
                        isModified = true;
                    } else if (!isMultidexEnabled && line.Contains(versionNameToken)) {
                        fileContent.Insert(i + 1, multidexEnabled);
                        isModified = true;
                    }
                }

                if (isModified) {
                    File.WriteAllLines(targetPath, fileContent);
                }
            } catch (System.Exception e) {
                Debug.Log(e.ToString());
            }
        }
    }
}
#endif