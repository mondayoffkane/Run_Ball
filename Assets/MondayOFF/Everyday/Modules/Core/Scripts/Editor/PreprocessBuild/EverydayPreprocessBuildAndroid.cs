﻿#if UNITY_ANDROID
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace MondayOFF
{
    public class EverydayPreprocessBuildAndroid : IPreprocessBuildWithReport
    {
        const string DEFAULT_PLUGIN_PATH = "Assets/Plugins/Android";
        const string MONDAYOFF_PLUGIN_PATH = "Assets/MondayOFF/ThirdParties/Singular/Plugins/Android";
        const string SINGULAR_SDK_FILENAME = "singular_sdk.aar";
        const string SINGULAR_UNITYBRIDGE_FILENAME = "SingularUnityBridge.jar";
        const string FIREBASE_CONFIG_PATH = "Assets/StreamingAssets/google-services-desktop.json";

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            ValidateFirebaseConfig();
            VerifySingularSDK(SINGULAR_SDK_FILENAME);
            VerifySingularSDK(SINGULAR_UNITYBRIDGE_FILENAME);

            if (PlayerSettings.Android.minSdkVersion < AndroidSdkVersions.AndroidApiLevel24)
            {
                EverydayLogger.Warn("Minimum Android SDK version is 24. Setting it to 24.");
                PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24;
            }

            ConfigProguard();
            ConfigAndroidManifest();
        }

        private static void ValidateFirebaseConfig()
        {
#if FIREBASE_ENABLED
            File.Delete(FIREBASE_CONFIG_PATH);
            // reimport firebase config
            Firebase.Editor.GenerateXmlFromGoogleServicesJson.ForceJsonUpdate();
#endif
        }

        // Check if file exists in the DEFAULT_PLUGIN_PATH. If there is, move it to MONDAYOFF_PLUGIN_PATH and delete the file in DEFAULT_PLUGIN_PATH.
        // if non of them exist, throw an error.
        private static void VerifySingularSDK(in string filename)
        {
            string defaultFilePath = Path.Combine(DEFAULT_PLUGIN_PATH, filename);
            string mondayoffFilePath = Path.Combine(MONDAYOFF_PLUGIN_PATH, filename);

            if (File.Exists(defaultFilePath))
            {
                if (File.Exists(mondayoffFilePath))
                {
                    File.Delete(defaultFilePath);
                }
                else
                {
                    if (!Directory.Exists(MONDAYOFF_PLUGIN_PATH))
                    {
                        Directory.CreateDirectory(MONDAYOFF_PLUGIN_PATH);
                    }
                    File.Move(defaultFilePath, mondayoffFilePath);
                }
            }
            else if (!File.Exists(mondayoffFilePath))
            {
                throw new UnityEditor.Build.BuildFailedException($"[EVERYDAY] {filename} is missing!");
            }
        }

        private static void ConfigProguard()
        {
            List<string> proguardList = new List<string>(){
                "# Singular",
                "-keep class com.singular.sdk.** { *; }",
                "-keep public class com.android.installreferrer.** { *; }",
                "-keep public class com.singular.unitybridge.** { *; }",
                "# Facebook",
                "-keep class com.facebook.** { *; }",
                "# Adverty",
                "-keep class com.unity3d.player.**{ *; }",
                "-keep class com.adverty.**{ *; }",
                "# Firebase",
                "-keep public class com.google.firebase.analytics.FirebaseAnalytics { public *; }",
                "-keep public class com.google.android.gms.measurement.AppMeasurement { public *; }"
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
            if (File.Exists(proguardPath))
            {
                var currentLines = File.ReadAllLines(proguardPath).ToList();
                // add lines that are not already in the file
                foreach (var line in proguardList)
                {
                    if (!currentLines.Contains(line))
                    {
                        currentLines.Add(line);
                    }
                }
                lines = currentLines.ToList();
            }
            else
            {
                string proguardDirectory = Path.GetDirectoryName(proguardPath);
                if (!Directory.Exists(proguardDirectory))
                {
                    Directory.CreateDirectory(proguardDirectory);
                }
                File.Create(proguardPath).Dispose();
                lines = proguardList;
            }

            File.WriteAllLines(proguardPath, lines);
        }

        private static void ConfigAndroidManifest()
        {
            const string manifestPath = "Assets/Plugins/Android/AndroidManifest.xml";
            string manifestDirectory = Path.GetDirectoryName(manifestPath);
            if (!Directory.Exists(manifestDirectory))
            {
                Directory.CreateDirectory(manifestDirectory);
            }

            Facebook.Unity.Editor.ManifestMod.GenerateManifest();

            var androidManifestDocument = XDocument.Load(manifestPath);

            XNamespace androidNamespace = androidManifestDocument.Root.Attribute(XNamespace.Xmlns + "android").Value;

            var applicationNode = androidManifestDocument.Root
                                        .Descendants("application")
                                        .FirstOrDefault();

            if (applicationNode == null)
            {
                EverydayLogger.Warn("Application node doest not exist! Are you sure AndroidManifest.xml is valid?");
                return;
            }

            bool isModified = false;

            if (applicationNode.Attribute(androidNamespace + "debuggable")?.Value == "true")
            {
                applicationNode.SetAttributeValue(androidNamespace + "debuggable", "false");
                isModified = true;
            }

            // Check if child node with name "activity" and attribute "android:name" with value "com.amazon.device.ads.DTBInterstitialActivity" exists and add if it doesn't
            var apsActivities = new[] { "com.amazon.device.ads.DTBInterstitialActivity", "com.amazon.device.ads.DTBAdActivity" };
            foreach (var activity in apsActivities)
            {
                var activityNode = applicationNode
                                    .Descendants("activity")
                                    .FirstOrDefault(element => element.Attribute(androidNamespace + "name")?.Value == activity);

                if (activityNode == null)
                {
                    activityNode = new XElement("activity");
                    activityNode.SetAttributeValue(androidNamespace + "name", activity);
                    applicationNode.Add(activityNode);
                    isModified = true;
                }
            }

            // Add NetworkSecurityConfig
            var networkSecurityName = androidNamespace + "networkSecurityConfig";
            var networkSecurityValue = "@xml/network_security_config";
            var networkSecurityAttribute = applicationNode.Attribute(networkSecurityName);
            if (networkSecurityAttribute == null || !networkSecurityAttribute.Value.Equals(networkSecurityValue))
            {
                applicationNode.SetAttributeValue(networkSecurityName, networkSecurityValue);
                isModified = true;
            }

            // Add required permissions
            string[] permissions = new string[]{
                "android.permission.ACCESS_NETWORK_STATE",
                "android.permission.ACCESS_WIFI_STATE",
                "android.permission.INTERNET",
                "com.google.android.gms.permission.AD_ID",
                "android.permission.ACCESS_COARSE_LOCATION",
                "android.permission.ACCESS_FINE_LOCATION",
            };

            foreach (var permission in permissions)
            {
                var existingPermission
                        = androidManifestDocument.Root
                            .Descendants()
                            .FirstOrDefault(element => element.Name == "uses-permission" && element.Attribute(androidNamespace + "name")?.Value == permission);

                if (existingPermission == null)
                {
                    var element = new XElement("uses-permission");
                    element.SetAttributeValue(androidNamespace + "name", permission);
                    applicationNode.Parent.Add(element);
                    isModified = true;
                }
            }

            if (isModified)
            {
                applicationNode.Document.Save(manifestPath);
                EverydayLogger.Info("Updated AndroidManifest.xml");
            }
        }
    }
}
#endif