﻿using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Game {

    /// <summary>
    /// Actions for command line build
    /// </summary>
    public class BuildActions {

        /// <summary>
        /// Get name from here or Config
        /// </summary>
        static string GAME_NAME = "PlayOnSDK";

        /// <summary>
        /// App version
        /// </summary>
        static string VERSION = PlayOnSDK.SDK_VERSION;

        /// <summary>
        /// Current project source path
        /// </summary>
        public static string APP_FOLDER = Directory.GetCurrentDirectory ();

        /// <summary>
        /// Android files path
        /// </summary>
        public static string ANDROID_FOLDER = string.Format ("{0}/Builds/Android/", APP_FOLDER);

        /// <summary>
        /// Android dev filename
        /// </summary>
        public static string ANDROID_DEVELOPMENT_FILE = string.Format ("{0}/Builds/Android/{1}.{2}.development.apk", APP_FOLDER, GAME_NAME, VERSION);

        /// <summary>
        /// Android release filename
        /// </summary>
        public static string ANDROID_RELEASE_FILE = string.Format ("{0}/Builds/Android/{1}.{2}.release.apk", APP_FOLDER, GAME_NAME, VERSION);

        /// <summary>
        /// iOS files path
        /// </summary>
        public static string IOS_FOLDER = string.Format ("{0}/Builds/iOS/", APP_FOLDER);

        /// <summary>
        /// iOS dev path
        /// </summary>
        public static string IOS_DEVELOPMENT_FOLDER = string.Format ("{0}/Builds/iOS/build/development", APP_FOLDER);

        /// <summary>
        /// iOS release path
        /// </summary>
        public static string IOS_RELEASE_FOLDER = string.Format ("{0}/Builds/iOS/build/release", APP_FOLDER);

        /// <summary>
        /// Get active scene list
        /// </summary>
        static string[] GetScenes () {
            List<string> scenes = new List<string> ();
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++) {
                if (EditorBuildSettings.scenes[i].enabled) {
                    scenes.Add (EditorBuildSettings.scenes[i].path);
                }
            }
            return scenes.ToArray ();
        }

        /// <summary>
        /// Run iOS development build
        /// </summary>
        static void IOSBuildIPA () {
            PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
            PlayerSettings.SetScriptingBackend (BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
            EditorUserBuildSettings.SwitchActiveBuildTarget (BuildTargetGroup.iOS, BuildTarget.iOS);
            EditorUserBuildSettings.development = true;
            BuildReport report = BuildPipeline.BuildPlayer (GetScenes(), IOS_FOLDER, BuildTarget.iOS, BuildOptions.None);
            int code = (report.summary.result == BuildResult.Succeeded) ? 0 : 1;
            EditorApplication.Exit (code); 
        }

        /// <summary>
        /// Run android build
        /// </summary>
        static void AndroidBuildAPK () {
            PlayerSettings.SetScriptingBackend (BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
            PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.Android, "DEV");
            EditorUserBuildSettings.SwitchActiveBuildTarget (BuildTargetGroup.Android, BuildTarget.Android);
            EditorUserBuildSettings.development = true;
            EditorUserBuildSettings.androidETC2Fallback = AndroidETC2Fallback.Quality32Bit;
            BuildReport report = BuildPipeline.BuildPlayer (GetScenes(), ANDROID_DEVELOPMENT_FILE, BuildTarget.Android, BuildOptions.None);
            int code = (report.summary.result == BuildResult.Succeeded) ? 0 : 1;
            EditorApplication.Exit (code);    
        }
    }

}