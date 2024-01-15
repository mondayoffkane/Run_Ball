#if UNITY_IOS
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace Adverty.PlatformSpecific.Editor
{
    public static class FrameworkIntegrator
    {
        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string buildPath)
        {
            if(buildTarget == BuildTarget.iOS)
            {
                string path = PBXProject.GetPBXProjectPath(buildPath);
                PBXProject project = new PBXProject();
                project.ReadFromFile(path);
                string target = project.GetUnityFrameworkTargetGuid();
                project.AddBuildProperty(target, "OTHER_LDFLAGS", "-lbz2 -lz -liconv");
                project.AddFrameworkToProject(target, "VideoToolbox.framework", false);
                project.WriteToFile(path);
            }
        }
    }
}
#endif
