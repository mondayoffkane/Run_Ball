using UnityEngine;
using UnityEditor;

namespace MondayOFF {
    internal class EverydayIAPMenuItem {
        [MenuItem("Everyday/Open In-App Purchase Products", false, 300)]
        private static void FocusProducts() {
            var products = AssetDatabase.FindAssets("t:EverydayProducts");

            if (products.Length != 1) {
                Debug.LogError("There are zero or more than two Objects! " + products.Length);
                return;
            }

            UnityEditor.Selection.activeObject = AssetDatabase.LoadAssetAtPath<EverydayProducts>(AssetDatabase.GUIDToAssetPath(products[0]));
        }
    }
}