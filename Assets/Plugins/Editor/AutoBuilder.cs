using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.Linq;

public class AutoBuilder
{
    static string[] SCENES = FindEnabledEditorScenes();
    static string APP_NAME = "앱 이름";
    static string TARGET_DIR;
    [MenuItem("Build/Android_APK")]
    public static void PerformBuildAPK()
    {
        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = "Assets/Plugins/MondayOFF_Keystore.keystore";
        PlayerSettings.Android.keystorePass = "MondayOFF!@";
        PlayerSettings.Android.keyaliasName = "mondayoff";
        PlayerSettings.Android.keyaliasPass = "MondayOFF!@";

        APP_NAME = PlayerSettings.productName + ".apk";
        TARGET_DIR = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/')) + "/Output";
        Directory.CreateDirectory(TARGET_DIR);
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_4_6);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
        EditorUserBuildSettings.androidBuildType = AndroidBuildType.Release;
        PlayerSettings.SplashScreen.show = false;
        BuildAndroid(SCENES, TARGET_DIR + "/" + APP_NAME, BuildTargetGroup.Android, BuildTarget.Android, BuildOptions.CompressWithLz4HC);
    }
    [MenuItem("Build/Android_APK_AutoRun_Development")]
    public static void PerformBuildAPKDevelopmentPhone()
    {
        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = "Assets/Plugins/MondayOFF_Keystore.keystore";
        PlayerSettings.Android.keystorePass = "MondayOFF!@";
        PlayerSettings.Android.keyaliasName = "mondayoff";
        PlayerSettings.Android.keyaliasPass = "MondayOFF!@";

        APP_NAME = PlayerSettings.productName + ".apk";
        TARGET_DIR = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/')) + "/Output";
        Directory.CreateDirectory(TARGET_DIR);
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_4_6);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
        PlayerSettings.SplashScreen.show = false;

        EditorUserBuildSettings.development = true;
        EditorUserBuildSettings.connectProfiler = true;
        EditorUserBuildSettings.buildWithDeepProfilingSupport = true;
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
        EditorUserBuildSettings.androidBuildType = AndroidBuildType.Development;
        BuildAndroid(SCENES, TARGET_DIR + "/" + APP_NAME, BuildTargetGroup.Android, BuildTarget.Android, BuildOptions.CompressWithLz4HC | BuildOptions.AutoRunPlayer | BuildOptions.Development | BuildOptions.EnableDeepProfilingSupport);
    }
    private static void BuildAndroid(string[] scenes, string app_target, BuildTargetGroup build_target_group, BuildTarget build_target, BuildOptions build_options)
    {
        //현 세팅이 안드로이드 아니면 안드로이드로 바꿔주기
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        EditorUserBuildSettings.buildAppBundle = false;
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = app_target;
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = build_options;
        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    [MenuItem("Build/Android_AAB")]
    public static void PerformBuildAAB()
    {
        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = "Assets/Plugins/MondayOFF_Keystore.keystore";
        PlayerSettings.Android.keystorePass = "MondayOFF!@";
        PlayerSettings.Android.keyaliasName = "mondayoff";
        PlayerSettings.Android.keyaliasPass = "MondayOFF!@";

        APP_NAME = PlayerSettings.productName + ".aab";
        TARGET_DIR = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/')) + "/Output";
        Directory.CreateDirectory(TARGET_DIR);
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_4_6);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
        EditorUserBuildSettings.androidBuildType = AndroidBuildType.Release;
        PlayerSettings.SplashScreen.show = false;

        BuildAndroidAAB(SCENES, TARGET_DIR + "/" + APP_NAME, BuildTargetGroup.Android, BuildTarget.Android, BuildOptions.CompressWithLz4HC /*| BuildOptions.*/);
    }

    [MenuItem("Build/Android_AAB_AutoRun")]
    public static void PerformBuildAABAutoRun()
    {
        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = "Assets/Plugins/MondayOFF_Keystore.keystore";
        PlayerSettings.Android.keystorePass = "MondayOFF!@";
        PlayerSettings.Android.keyaliasName = "mondayoff";
        PlayerSettings.Android.keyaliasPass = "MondayOFF!@";

        APP_NAME = PlayerSettings.productName + ".aab";
        TARGET_DIR = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/')) + "/Output";
        Directory.CreateDirectory(TARGET_DIR);
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_4_6);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
        EditorUserBuildSettings.androidBuildType = AndroidBuildType.Release;
        PlayerSettings.SplashScreen.show = false;

        BuildAndroidAAB(SCENES, TARGET_DIR + "/" + APP_NAME, BuildTargetGroup.Android, BuildTarget.Android, BuildOptions.CompressWithLz4HC | BuildOptions.AutoRunPlayer);
    }

    private static void BuildAndroidAAB(string[] scenes, string app_target, BuildTargetGroup build_target_group, BuildTarget build_target, BuildOptions build_options)
    {
        //현 세팅이 안드로이드 아니면 안드로이드로 바꿔주기
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        EditorUserBuildSettings.buildAppBundle = true;
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = app_target;
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = build_options;
        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
    }


    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();

        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }

        return EditorScenes.ToArray();
    }


    /// <summary>
    /// Current project source path
    /// </summary>
    public static string APP_FOLDER = Directory.GetCurrentDirectory();

    /// <summary>
    /// iOS files path
    /// </summary>
    public static string IOS_FOLDER = string.Format("{0}/Builds/iOS/", APP_FOLDER);

    /// <summary>
    /// Run iOS development build
    /// </summary>
    [MenuItem("Build/IOS")]
    static void PerformBuildIOS()
    {
        PlayerSettings.iOS.appleDeveloperTeamID = "NSR93K7Q22";
        PlayerSettings.iOS.appleEnableAutomaticSigning = true;
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
        PlayerSettings.SplashScreen.show = false;

        // PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, "UNITY_PURCHASING");
        BuildIOS(SCENES, IOS_FOLDER, BuildTarget.iOS, BuildOptions.None);
    }

    static void BuildIOS(string[] scenes, string target_path, BuildTarget build_target, BuildOptions build_options)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, build_target);
        BuildReport res = BuildPipeline.BuildPlayer(scenes, target_path, build_target, build_options);
        if (res.name.Length > 0) { throw new Exception("BuildPlayer failure: " + res); }
    }
}