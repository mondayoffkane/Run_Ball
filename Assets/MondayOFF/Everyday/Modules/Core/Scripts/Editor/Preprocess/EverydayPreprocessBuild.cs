﻿using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.Linq;
using Facebook.Unity.Settings;
using Adverty;

namespace MondayOFF {
    public class EverydayPreprocessBuild : IPreprocessBuildWithReport {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report) {
            // Facebook settings
            // Facebook.Unity.Settings.FacebookSettings.AppLabels = new System.Collections.Generic.List<string> { "" };
            // Facebook.Unity.Settings.FacebookSettings.AppIds = new System.Collections.Generic.List<string> { "" };
            // Facebook.Unity.Settings.FacebookSettings.ClientTokens = new System.Collections.Generic.List<string> { "" };

            var settingAssets = AssetDatabase.FindAssets("t:EverydaySettings");

            if (settingAssets.Length != 1) {
                throw new UnityEditor.Build.BuildFailedException("[EVERYDAY] There should be ONLY 1 settings object.");
            }

            var settings = AssetDatabase.LoadAssetAtPath<EverydaySettings>(AssetDatabase.GUIDToAssetPath(settingAssets[0]));
            AddSettingsToPreload(settings);

            if (string.IsNullOrEmpty(Facebook.Unity.Settings.FacebookSettings.ClientToken)) {
                var fbSettings = AssetDatabase.FindAssets(filter: "t:FacebookSettings");

                if (fbSettings.Length != 1) {
                    Debug.LogError("[EVERYDAY] There are zero or more than two Objects! " + fbSettings.Length);
                    return;
                }

                UnityEditor.Selection.activeObject = AssetDatabase.LoadAssetAtPath<FacebookSettings>(AssetDatabase.GUIDToAssetPath(fbSettings[0]));
                throw new UnityEditor.Build.BuildFailedException("[EVERYDAY] Facebook Client Token is empty!");
            }

            if (AdvertySettings.SandboxMode) {
                if (!EditorUtility.DisplayDialog("Adverty is in Sandbox Mode", "Do you want to build as Adverty Sandbox mode?", "Yes", "Cancel")) {
                    throw new UnityEditor.Build.BuildFailedException("[EVERYDAY] Check Adverty Sandbox mode.");
                }
            }

            // Make sure Sprite Atlasing is enabled?
            // if ((int)UnityEditor.EditorSettings.spritePackerMode < (int)SpritePackerMode.BuildTimeOnlyAtlas) {
            //     UnityEditor.EditorSettings.spritePackerMode = SpritePackerMode.BuildTimeOnlyAtlas;
            // }

#if UNITY_PRO_LICENSE
            UnityEditor.PlayerSettings.SplashScreen.showUnityLogo = false;
#endif
        }

        private void AddSettingsToPreload(EverydaySettings everydaySettings) {
            var preloadedAssets = PlayerSettings.GetPreloadedAssets();
            foreach (var asset in preloadedAssets) {
                if (asset.GetType() == typeof(EverydaySettings)) {
                    Debug.Log("[EVERYDAY] EverydaySettigns is in preload asset");
                    return;
                }
            }

            var list = preloadedAssets.ToList();
            list.Add(everydaySettings);

            PlayerSettings.SetPreloadedAssets(list.ToArray());
            Debug.Log("[EVERYDAY] Adding EverydaySettigns to preload assets");
        }
    }
}