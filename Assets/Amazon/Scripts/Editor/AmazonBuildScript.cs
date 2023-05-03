using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using UnityEditor.Callbacks;

public class AmazonBuildScript
{
    public static string DEFAULT_APK_NAME = "APSUnityTestApp.apk";
    public static string DEFAULT_IOS_BUILD_DIR = "Builds/iOS";

    public static void BuildIos()
    {
        string[] scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
        string outputFileName = GetArg("-output", DEFAULT_IOS_BUILD_DIR);
        BuildPipeline.BuildPlayer(scenes, outputFileName, BuildTarget.iOS, BuildOptions.Development);
    }

    public static void BuildAndroid()
    {
        string[] scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        string outputFileName = GetArg("-output", DEFAULT_APK_NAME);
        BuildPipeline.BuildPlayer(scenes, outputFileName, BuildTarget.Android, BuildOptions.Development);
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
    }

    public static void SwitchToIos()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
    }

    private static string GetArg(string name, string defaultVal)
    {
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == name && args.Length > i + 1)
            {
                return args[i + 1];
            }
        }
        return defaultVal;
    }

    public static void BuildExternalAndroid()
    {
        string[] envvars = new string[]
        {
          "ANDROID_KEYSTORE_NAME", "ANDROID_KEYSTORE_PASSWORD", "ANDROID_KEYALIAS_NAME", "ANDROID_KEYALIAS_PASSWORD", "ANDROID_SDK_ROOT"
        };
        if (EnvironmentVariablesMissing(envvars))
        {
            Environment.ExitCode = -1;
            return; // note, we can not use Environment.Exit(-1) - the buildprocess will just hang afterwards
        }

        //Available Playersettings: https://docs.unity3d.com/ScriptReference/PlayerSettings.Android.html

        //set the internal apk version to the current unix timestamp, so this increases with every build
        PlayerSettings.Android.bundleVersionCode = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

        //set the other settings from environment variables
        PlayerSettings.Android.keystoreName = Environment.GetEnvironmentVariable("ANDROID_KEYSTORE_NAME");
        PlayerSettings.Android.keystorePass = Environment.GetEnvironmentVariable("ANDROID_KEYSTORE_PASSWORD");
        PlayerSettings.Android.keyaliasName = Environment.GetEnvironmentVariable("ANDROID_KEYALIAS_NAME");
        PlayerSettings.Android.keyaliasPass = Environment.GetEnvironmentVariable("ANDROID_KEYALIAS_PASSWORD");

        EditorPrefs.SetString("AndroidSdkRoot", Environment.GetEnvironmentVariable("ANDROID_SDK_ROOT"));

        //Get the apk file to be built from the command line argument
        string outputapk = Environment.GetCommandLineArgs().Last();
        BuildPipeline.BuildPlayer(GetScenePaths(), outputapk, BuildTarget.Android, BuildOptions.None);
    }

    static string[] GetScenePaths()
    {
        string[] scenes = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }
        return scenes;
    }

    static bool EnvironmentVariablesMissing(string[] envvars)
    {
        string value;
        bool missing = false;
        foreach (string envvar in envvars)
        {
            value = Environment.GetEnvironmentVariable(envvar);
            if (value == null)
            {
                Console.Write("BUILD ERROR: Required Environment Variable is not set: ");
                Console.WriteLine(envvar);
                missing = true;
            }
        }

        return missing;
    }
}
