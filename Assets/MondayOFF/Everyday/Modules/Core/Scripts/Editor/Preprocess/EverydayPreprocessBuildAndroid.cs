#if UNITY_ANDROID
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace MondayOFF {
    public class EverydayPreprocessBuildAndroid : IPreprocessBuildWithReport {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report) {
            ConfigProguard();
            ConfigAndroidManifest();
        }

        private static void ConfigProguard() {
            List<string> PROGUARD_LIST = new List<string>(){
                "# Singular",
                "-keep class com.singular.sdk.** { *; }",
                "-keep public class com.android.installreferrer.** { *; }",
                "-keep public class com.singular.unitybridge.** { *; }",
                "# Facebook",
                "-keep class com.facebook.** { *; }",
                "# Adverty",
                "-keep class com.unity3d.player.**{ *; }",
                "-keep class com.adverty.**{ *; }"
            };

            string proguardFilename =
#if UNITY_2021_2_OR_NEWER
                "proguard-user.txt"
#else
                "proguard.txt"
#endif
            ;

            string proguardPath = Path.Combine("Assets/Plugins/Android", proguardFilename);

            List<string> lines;
            if (File.Exists(proguardPath)) {
                lines = File.ReadAllLines(proguardPath).Union(PROGUARD_LIST).ToList();
            } else {
                lines = PROGUARD_LIST;
            }

            File.WriteAllLines(proguardPath, lines);
        }

        private static void ConfigAndroidManifest() {
            const string manifestPath = "Assets/Plugins/Android/AndroidManifest.xml";
            var androidManifestDocument = XDocument.Load(manifestPath);

            XNamespace androidNamespace = androidManifestDocument.Root.Attribute(XNamespace.Xmlns + "android").Value;

            var applicationNode = androidManifestDocument.Root
                                        .Descendants("application")
                                        .FirstOrDefault();

            if (applicationNode == null) {
                Debug.LogWarning("[EVERYDAY] Application node doest not exist! Are you sure AndroidManifest.xml is valid?");
                return;
            }

            // Check for FB 
            var clientTokenElement = androidManifestDocument.Root.Descendants().FirstOrDefault(element => element.Name == "meta-data" && element.Attribute(androidNamespace + "name").Value == "com.facebook.sdk.ClientToken");
            if (clientTokenElement == null) {
                var fbSettings = AssetDatabase.FindAssets(filter: "t:FacebookSettings");

                if (fbSettings.Length != 1) {
                    Debug.LogError("[EVERYDAY] There are zero or more than two Objects! " + fbSettings.Length);
                    return;
                }

                UnityEditor.Selection.activeObject = AssetDatabase.LoadAssetAtPath<Facebook.Unity.Settings.FacebookSettings>(AssetDatabase.GUIDToAssetPath(fbSettings[0]));
                throw new UnityEditor.Build.BuildFailedException("[EVERYDAY] Facebook Client Token is empty or AndroidManifest is not regenerated after setting Facebook App info!.");
            }

            bool isModified = false;

            // Add NetworkSecurityConfig
            var networkSecurityName = androidNamespace + "networkSecurityConfig";
            var networkSecurityValue = "@xml/network_security_config";
            var networkSecurityAttribute = applicationNode.Attribute(networkSecurityName);
            if (networkSecurityAttribute == null || !networkSecurityAttribute.Value.Equals(networkSecurityValue)) {
                applicationNode.SetAttributeValue(networkSecurityName, networkSecurityValue);
                isModified = true;
            }

            // Add required permissions
            string[] permissions = new string[]{
                "android.permission.ACCESS_NETWORK_STATE",
                "android.permission.INTERNET",
                "com.google.android.gms.permission.AD_ID"
            };

            foreach (var permission in permissions) {
                var existingPermission
                        = androidManifestDocument.Root
                            .Descendants()
                            .FirstOrDefault(element => element.Name == "uses-permission" && element.Attribute(androidNamespace + "name").Value == permission);

                if (existingPermission == null) {
                    var element = new XElement("uses-permission");
                    element.SetAttributeValue(androidNamespace + "name", permission);
                    applicationNode.Parent.Add(element);
                    isModified = true;
                }
            }

            if (isModified) {
                applicationNode.Document.Save(manifestPath);
                Debug.Log("[EVERYDAY] Updated AndroidManifest.xml");
            }
        }
    }
}
#endif