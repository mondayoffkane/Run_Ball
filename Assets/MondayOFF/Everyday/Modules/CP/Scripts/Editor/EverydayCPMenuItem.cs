using UnityEngine;
using UnityEditor;

namespace MondayOFF {
    internal class EverydayCPMenuItem {
        [MenuItem("Everyday/Open Cross Promotion Settings", false, 200)]
        private static void FocusCPVideoList() {
            var cpVidList = AssetDatabase.FindAssets("t:CPVideoList");

            if (cpVidList.Length != 1) {
                Debug.LogError("[CP] There are zero or more than two Objects! " + cpVidList.Length);
                return;
            }

            UnityEditor.Selection.activeObject = AssetDatabase.LoadAssetAtPath<CPVideoList>(AssetDatabase.GUIDToAssetPath(cpVidList[0]));
        }
    }
}