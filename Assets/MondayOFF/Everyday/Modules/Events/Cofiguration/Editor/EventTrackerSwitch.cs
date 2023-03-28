using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor.Callbacks;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.Compilation;

namespace MondayOFF {
    internal class EventTrackerSwitch : AssetPostprocessor {
        const string FIREBASE_ENABLED = "FIREBASE_ENABLED";
        const string CONFIG_FILE_NAME =
#if UNITY_IOS
"GoogleService-Info.plist"
#else
"google-services.json"
#endif
;

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths
#if UNITY_2021_2_OR_NEWER
        , bool didDomainReload
#endif
        ) {
#if FIREBASE_ENABLED
            foreach (var item in deletedAssets) {
                if (item.Contains(CONFIG_FILE_NAME)) {
                    DidReloadScripts();
                    return;
                }
            }
#else
            foreach (var item in importedAssets) {
                if (item.Contains(CONFIG_FILE_NAME)) {
                    DidReloadScripts();
                    return;
                }
            }
#endif
        }

        [DidReloadScripts(0)]
        private static void DidReloadScripts() {
            // Assembly integrated
            System.Reflection.Assembly firebaseAssembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "Firebase.App");
            // Configfiles exist
            bool hasConfigFile = File.Exists(Path.Combine(Application.dataPath, CONFIG_FILE_NAME));
            bool isAdd = firebaseAssembly != null && hasConfigFile;
#if FIREBASE_ENABLED
            if (!isAdd)
#else
            if (isAdd)
#endif
            {
                Debug.Log($"[EVERYDAY] Project has valid Firebase: {isAdd}");
                ModifyScriptingDefineSymbol(FIREBASE_ENABLED, isAdd);
            }
        }

        private static void ModifyScriptingDefineSymbol(string symbol, bool isAdd) {
            BuildTargetGroup[] groups = new BuildTargetGroup[]{
                BuildTargetGroup.iOS, BuildTargetGroup.Android, BuildTargetGroup.Standalone
            };

            foreach (var currentGroup in groups) {
                bool hasChanged = false;
                var definedSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentGroup);
                var defines = new List<string>(definedSymbols.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                if (isAdd) {
                    if (!defines.Contains(symbol)) {
                        defines.Add(symbol);
                        hasChanged = true;
                    }
                } else {
                    if (defines.Contains(symbol)) {
                        defines.Remove(symbol);
                        hasChanged = true;
                    }
                }

                if (hasChanged) {
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(currentGroup, string.Join(";", defines.ToArray()));
                    CompilationPipeline.RequestScriptCompilation();
                }
            }
        }
    }
}