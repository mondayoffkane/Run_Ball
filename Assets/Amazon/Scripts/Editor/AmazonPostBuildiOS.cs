#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;
namespace AmazonInternal.Editor.Postbuild {
    public static class AmazonPostBuildiOS {
        [PostProcessBuildAttribute(1)]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string path) {
            if (buildTarget != BuildTarget.iOS)
                return;
            
            string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
            string pbxProjectPath = PBXProject.GetPBXProjectPath(path);
            PBXProject pbxProject = new PBXProject();

            pbxProject.ReadFromFile(pbxProjectPath);

            string[] targetGuids = new string[2] {
                pbxProject.GetUnityMainTargetGuid(),
                pbxProject.GetUnityFrameworkTargetGuid()
            };
            pbxProject.SetBuildProperty(targetGuids, "ENABLE_BITCODE", "NO");

            pbxProject.WriteToFile(pbxProjectPath);
        }        
    }
}
#endif