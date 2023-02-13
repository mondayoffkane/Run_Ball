#if UNITY_IOS
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;
public static class SignInWithApplePostprocessor
{
    [PostProcessBuildAttribute(1)]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
        if (target != BuildTarget.iOS)
            return;

        string pbxPath = PBXProject.GetPBXProjectPath(path);
        var proj = new PBXProject();
        proj.ReadFromFile(pbxPath);
        var guid = proj.GetUnityMainTargetGuid();
 
        string[] idArray = Application.identifier.Split('.');
        var entitlementsPath = $"Unity-iPhone/{idArray[idArray.Length - 1]}.entitlements";
 
        var capManager = new ProjectCapabilityManager(pbxPath, entitlementsPath, null, guid);
 
        // 넣고 싶은 Capability 넣어주기 /// 사용하면 주석 풀어주기
        capManager.AddPushNotifications(true);
        // capManager.AddSignInWithApple();
 
        // 저장 /// 사용하면 주석 풀기
        capManager.WriteToFile();
    }
}
#endif