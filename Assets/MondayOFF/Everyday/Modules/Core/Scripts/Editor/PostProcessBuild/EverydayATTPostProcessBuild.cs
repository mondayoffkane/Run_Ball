// #if UNITY_IOS
// using System.IO;
// using MondayOFF;
// using UnityEditor;
// using UnityEditor.Callbacks;
// using UnityEditor.iOS.Xcode;
// using UnityEditor.iOS.Xcode.Extensions;

// using UnityEngine;

// public static class EverydayATTPostProcessBuild
// {

//     [PostProcessBuild(int.MaxValue)]
//     public static void OnPostprocessBuild(BuildTarget buildTarget, string buildPath)
//     {
//         PrepareProject(buildPath);
//         PrepareInfoPlist(buildPath);
//     }

//     private static void PrepareProject(string buildPath)
//     {
//         var projPath = Path.Combine(buildPath, "Unity-iPhone.xcodeproj/project.pbxproj");
//         var project = new PBXProject();
//         project.ReadFromString(File.ReadAllText(projPath));

//         // Add frameworks to target UnityFramework
//         project.AddFrameworkToProject(project.GetUnityFrameworkTargetGuid(), "AppTrackingTransparency.framework", false);

//         File.WriteAllText(projPath, project.WriteToString());
//     }

//     private static void PrepareInfoPlist(string buildPath)
//     {
//         // Get plist
//         string plistPath = Path.Combine(buildPath, "Info.plist");
//         PlistDocument plist = new PlistDocument();
//         plist.ReadFromString(File.ReadAllText(plistPath));

//         // Get root
//         PlistElementDict rootDict = plist.root;

//         rootDict.SetString("NSUserTrackingUsageDescription", EverydaySettings.Instance.userTrackingUsageDescription);

//         // Write to file
//         File.WriteAllText(plistPath, plist.WriteToString());
//     }
// }

// #endif