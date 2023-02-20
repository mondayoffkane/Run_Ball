#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;
using System.Collections.Generic;


public static class FrameworkResolver
{
    private const string FRAMEWORK_ORIGIN_PATH =
        "Assets/PlayOn/Plugins/IOS"; // relative to project folder
    private const string FRAMEWORK_TARGET_PATH =
        "Frameworks"; // relative to build folder

    public const string DefaultUserTrackingDescriptionEnV0 = "Pressing \\\"Allow\\\" uses device info for more relevant ad content";
    public const string DefaultUserTrackingDescriptionEnV1 = "This only uses device info for less annoying, more relevant ads";
 
    
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    {
        
        if (buildTarget != BuildTarget.iOS)
            return;


        string sourcePlayOnPath = Path.Combine(FRAMEWORK_ORIGIN_PATH, "PlayOnSDK.xcframework");
        string sourceOMPath = Path.Combine(FRAMEWORK_ORIGIN_PATH, "OMSDK_Odeeoio.xcframework");

        string destPlayOnPath = Path.Combine(FRAMEWORK_TARGET_PATH, "PlayOnSDK.framework");
        string destOMPath = Path.Combine(FRAMEWORK_TARGET_PATH, "OMSDK_Odeeoio.framework");
 
        string devicePlayOnFrameworkPath = "ios-arm64_armv7/PlayOnSDK.framework";
        string deviceOMFrameworkPath = "ios-arm64_armv7/OMSDK_Odeeoio.framework";

        string simulatorPlayOnFrameworkPath = "ios-i386_x86_64-simulator/PlayOnSDK.framework";
        string simulatorOMFrameworkPath = "ios-arm64_i386_x86_64-simulator/OMSDK_Odeeoio.framework";

        iOSSdkVersion target = PlayerSettings.iOS.sdkVersion;
        if( target == iOSSdkVersion.DeviceSDK){
            sourcePlayOnPath = Path.Combine(sourcePlayOnPath, devicePlayOnFrameworkPath);
            sourceOMPath = Path.Combine(sourceOMPath, deviceOMFrameworkPath);
        } else if (target == iOSSdkVersion.SimulatorSDK){
            sourcePlayOnPath = Path.Combine(sourcePlayOnPath, simulatorPlayOnFrameworkPath);
            sourceOMPath = Path.Combine(sourceOMPath, simulatorOMFrameworkPath);
        }

        CopyDirectory(sourcePlayOnPath, Path.Combine(path, destPlayOnPath));
        CopyDirectory(sourceOMPath, Path.Combine(path, destOMPath));

        string pbxProjectPath = PBXProject.GetPBXProjectPath(path);
        PBXProject project = new PBXProject();

        project.ReadFromFile(pbxProjectPath);

 #if UNITY_2019_3_OR_NEWER
        string targetGuid = project.GetUnityFrameworkTargetGuid();
 #else
        string targetGuid = project.TargetGuidByName(PBXProject.GetUnityTargetName());
 #endif
        string filePlayOnGuid = project.AddFile(destPlayOnPath, destPlayOnPath, PBXSourceTree.Source);
        string fileOMGuid = project.AddFile(destOMPath, destOMPath, PBXSourceTree.Source);

        project.AddFileToBuild(targetGuid, filePlayOnGuid);
        project.AddFileToBuild(targetGuid, fileOMGuid);
        project.AddFrameworkToProject(targetGuid, "PlayOnSDK.framework", false);
        project.AddFrameworkToProject(targetGuid, "OMSDK_Odeeoio.framework", false);
        project.AddBuildProperty(targetGuid, "FRAMEWORK_SEARCH_PATHS", "$(SRCROOT)/Frameworks");
 #if UNITY_2019_3_OR_NEWER
        UnityEditor.iOS.Xcode.Extensions.PBXProjectExtensions.AddFileToEmbedFrameworks(project, project.GetUnityMainTargetGuid(), filePlayOnGuid);
        UnityEditor.iOS.Xcode.Extensions.PBXProjectExtensions.AddFileToEmbedFrameworks(project, project.GetUnityMainTargetGuid(), fileOMGuid);
 #else
        UnityEditor.iOS.Xcode.Extensions.PBXProjectExtensions.AddFileToEmbedFrameworks(project, targetGuid, filePlayOnGuid);
        UnityEditor.iOS.Xcode.Extensions.PBXProjectExtensions.AddFileToEmbedFrameworks(project, targetGuid, fileOMGuid);
 #endif

        LocalizeUserTrackingDescriptionIfNeeded(DefaultUserTrackingDescriptionEnV1,"en", path, project, targetGuid);
        
        project.WriteToFile(pbxProjectPath);
    }
 
    private static void CopyDirectory(string sourcePath, string destPath)
    {
        if(Directory.Exists(destPath)){
            Directory.Delete(destPath,true);
        }
        Directory.CreateDirectory(destPath);
 
        foreach (string file in Directory.GetFiles(sourcePath))
            File.Copy(file, Path.Combine(destPath, Path.GetFileName(file)));
 
        foreach (string dir in Directory.GetDirectories(sourcePath))
            CopyDirectory(dir, Path.Combine(destPath, Path.GetFileName(dir)));
    }

    private static void LocalizeUserTrackingDescriptionIfNeeded(string localizedUserTrackingDescription, string localeCode, string buildPath, PBXProject project, string targetGuid)
        {
            const string resourcesDirectoryName = "Resources";
            var resourcesDirectoryPath = Path.Combine(buildPath, resourcesDirectoryName);
            var localeSpecificDirectoryName = localeCode + ".lproj";
            var localeSpecificDirectoryPath = Path.Combine(resourcesDirectoryPath, localeSpecificDirectoryName);
            var infoPlistStringsFilePath = Path.Combine(localeSpecificDirectoryPath, "InfoPlist.strings");

            if (string.IsNullOrEmpty(localizedUserTrackingDescription))
            {
                if (!File.Exists(infoPlistStringsFilePath)) return;

                File.Delete(infoPlistStringsFilePath);
                return;
            }

            if (!Directory.Exists(resourcesDirectoryPath))
            {
                Directory.CreateDirectory(resourcesDirectoryPath);
            }

            if (!Directory.Exists(localeSpecificDirectoryPath))
            {
                Directory.CreateDirectory(localeSpecificDirectoryPath);
            }

            var localizedDescriptionLine = "\"NSUserTrackingUsageDescription\" = \"" + localizedUserTrackingDescription + "\";\n";
            // File already exists, update it in case the value changed between builds.
            if (File.Exists(infoPlistStringsFilePath))
            {
                var output = new List<string>();
                var lines = File.ReadAllLines(infoPlistStringsFilePath);
                var keyUpdated = false;
                foreach (var line in lines)
                {
                    if (line.Contains("NSUserTrackingUsageDescription"))
                    {
                        output.Add(localizedDescriptionLine);
                        keyUpdated = true;
                    }
                    else
                    {
                        output.Add(line);
                    }
                }

                if (!keyUpdated)
                {
                    output.Add(localizedDescriptionLine);
                }

                File.WriteAllText(infoPlistStringsFilePath, string.Join("\n", output.ToArray()) + "\n");

            } else {
                File.WriteAllText(infoPlistStringsFilePath, "/* Localized versions of Info.plist keys - Generated by PlayOn plugin */\n" + localizedDescriptionLine);
            }
            var guid = project.AddFolderReference(localeSpecificDirectoryPath, Path.Combine(resourcesDirectoryName, localeSpecificDirectoryName));
            project.AddFileToBuild(targetGuid, guid);
        }

}
#endif