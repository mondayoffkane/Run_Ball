#if UNITY_IOS
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.IO;

namespace MondayOFF {
    public class EverydayPreprocessBuildiOS : IPreprocessBuildWithReport {
        const string DEFAULT_PLUGIN_PATH = "Assets/Plugins/iOS";
        const string MONDAYOFF_PLUGIN_PATH = "Assets/MondayOFF/ThirdParties/Singular/Plugins/iOS";

        readonly string[] SINGULAR_SDK_FILENAME = new string[] {
            "Attributes.h",
            "Events.h",
            "libSingular.a",
            "Singular.h",
            "SingularAdData.h",
            "SingularAppDelegate.m",
            "SingularConfig.h",
            "SingularLinkParams.h",
            "SingularStateWrapper.h",
            "SingularStateWrapper.m",
            "SingularSwizzledAppController.m",
            "SingularUnityWrapper.mm"
        };

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report) {
            ValidateiOSTargetVersion();

            foreach (string filename in SINGULAR_SDK_FILENAME) {
                VerifySingularSDK(filename);
            }

            VerifyGVhSettings();
        }

        // Validate iOS target version
        private static void ValidateiOSTargetVersion() {
            // APS requies iOS 12.5 or higher
            var facebookSDKVersion = new System.Version(Facebook.Unity.FacebookSdkVersion.Build);
            if (facebookSDKVersion >= new System.Version(15, 0, 0)) {
                var currentVersion = PlayerSettings.iOS.targetOSVersionString;
                var requiredVersion = new System.Version(12, 5, 0);
                if (string.IsNullOrEmpty(currentVersion) || new System.Version(currentVersion) < requiredVersion) {
                    Debug.LogWarning("[EVERYDAY] iOS target version should be 12.5 or higher.");
                    PlayerSettings.iOS.targetOSVersionString = requiredVersion.ToString(2);
                }
            }
        }

        // Check if file exists in the DEFAULT_PLUGIN_PATH. If there is, move it to MONDAYOFF_PLUGIN_PATH and delete the file in DEFAULT_PLUGIN_PATH.
        // if non of them exist, throw an error.
        private static void VerifySingularSDK(in string filename) {
            string defaultPath = Path.Combine(DEFAULT_PLUGIN_PATH, filename);
            string mondayoffPath = Path.Combine(MONDAYOFF_PLUGIN_PATH, filename);

            if (File.Exists(defaultPath)) {
                if (File.Exists(mondayoffPath)) {
                    File.Delete(defaultPath);
                } else {
                    if (!Directory.Exists(MONDAYOFF_PLUGIN_PATH)) {
                        Directory.CreateDirectory(MONDAYOFF_PLUGIN_PATH);
                    }
                    File.Move(defaultPath, mondayoffPath);
                }
            } else if (!File.Exists(mondayoffPath)) {
                throw new UnityEditor.Build.BuildFailedException($"[EVERYDAY] {filename} is missing!");
            }
        }

        private static void VerifyGVhSettings() {
            Google.IOSResolver.PodfileStaticLinkFrameworks = false;
        }
    }
}
#endif