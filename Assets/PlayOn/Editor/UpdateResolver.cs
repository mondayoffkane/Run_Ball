using System.IO;
using UnityEditor;
using UnityEngine;

public class UpdateResolver
{
    private const string SDK_ANDROID_LIB_PATH = "PlayOn/Plugins/Android";
    private const string MEDIATION_ANDROID_LIB_PATH = "PlayOn/Mediations/Plugins/Android";

    private const string SDK_ANDROID_FILENAME_PREFIX = "playon-";
    private const string MEDIATION_ANDROID_APPLOVIN_FILENAME_PREFIX = "playon-android-network-applovin-";
    private const string MEDIATION_ANDROID_ADMOB_FILENAME_PREFIX = "playon-network-admob-";
    
    [InitializeOnLoadMethod]
    private static void CheckVersions()
    {
        if(Application.isPlaying || Application.isBatchMode)
            return;

        CheckSdkVersion();
        CheckMediationVersion();
    }
    
    private static void CheckSdkVersion()
    {
        if(!IsShouldCheckSDKVersion())
            return;

        string libPath = EditorHelper.GetAssetBasedPath(SDK_ANDROID_LIB_PATH);
        if (!Directory.Exists(libPath))
            return;
        
        string[] files = Directory.GetFiles(libPath);

        string highestVersionFilePath = GetHighestVersionFilepath(files, SDK_ANDROID_FILENAME_PREFIX);
        DeleteUnmatchedLibraries(files, highestVersionFilePath);
    }

    private static void CheckMediationVersion()
    {
        if(!IsShouldCheckMediationVersion())
            return;
        
        string libPath = EditorHelper.GetAssetBasedPath(MEDIATION_ANDROID_LIB_PATH);
        if (!Directory.Exists(libPath))
            return;
        
        string[] files = Directory.GetFiles(libPath);
        string highestVersionFilePath = "";
        
        highestVersionFilePath = GetHighestVersionFilepath(files, MEDIATION_ANDROID_APPLOVIN_FILENAME_PREFIX);
        DeleteUnmatchedLibraries(files, highestVersionFilePath);
        
        highestVersionFilePath = GetHighestVersionFilepath(files, MEDIATION_ANDROID_ADMOB_FILENAME_PREFIX);
        DeleteUnmatchedLibraries(files, highestVersionFilePath);
    }

    private static string GetHighestVersionFilepath(string[] files, string filenamePrefix)
    {
        int highestVersion = -1;
        string highestVersionFilePath = "";

        for (int i = 0; i < files.Length; i++)
        {
            if (!files[i].EndsWith(".aar"))
                continue;

            if (files[i].Contains(filenamePrefix))
            {
                int libVersion = GetVersionAsInt(GetVersionStringFromFilename(files[i]));
                if (libVersion > highestVersion)
                {
                    highestVersion = libVersion;
                    highestVersionFilePath = files[i];
                }
            }
        }

        return highestVersionFilePath;
    }

    private static void DeleteUnmatchedLibraries(string[] files, string properFilename)
    {
        if(string.IsNullOrEmpty(properFilename))
            return;
        
        for (int i = 0; i < files.Length; i++)
        {
            if (!files[i].EndsWith(".aar"))
                continue;

            if (!files[i].Equals(properFilename))
            {
                DeleteFileWithMeta(files[i]);
            }
        }
    }

    private static void DeleteFileWithMeta(string filepath)
    {
        Debug.Log("-- PlayOn SDK: Delete old library file: " + filepath);
        Debug.Log("-- PlayOn SDK: Delete old library file meta: " + filepath + ".meta");
        
        if(File.Exists(filepath))
            File.Delete(filepath);
        
        if(File.Exists(filepath + ".meta"))
            File.Delete(filepath + ".meta");
    }
    
    private static bool IsShouldCheckSDKVersion()
    {
        return true;
    }

    private static bool IsShouldCheckMediationVersion()
    {
        return true;
    }

    private static string GetVersionStringFromFilename(string filename)
    {
        return filename.Substring(filename.Length - 9, 5);
    }
    
    private static int GetVersionAsInt(string version)
    {
        bool isSuccess = int.TryParse(version.Replace(".", ""), out int result);
        return isSuccess ? result : -1;
    }
}
