using UnityEngine;
using UnityEditor;

namespace MondayOFF {
    [CustomPropertyDrawer(typeof(PreviewSprite))]
    public class PreviewSpriteDrawer : PropertyDrawer {
        const float imageHeight = 75f;

        public override float GetPropertyHeight(SerializedProperty property,
                                                GUIContent label) {
            if (property.hasMultipleDifferentValues)
                return EditorGUIUtility.singleLineHeight;
            return EditorGUI.GetPropertyHeight(property, label, true)
                 + imageHeight
                  + ((property.objectReferenceValue != null) ? EditorGUIUtility.singleLineHeight : 0f)
                 ;
        }

        public override void OnGUI(Rect rect,
                                    SerializedProperty property,
                                    GUIContent label) {
            var spr = property.objectReferenceValue as Sprite;
            if (property.hasMultipleDifferentValues) {
                return;
            }

            property.objectReferenceValue
                = EditorGUI.ObjectField(
                    rect,
                    property.displayName,
                    spr,
                    typeof(Sprite),
                    false
                );

            if (spr != null) {
                var textColor = new GUIStyle(GUI.skin.label).normal.textColor;
                var style = new GUIStyle() {
                    alignment = TextAnchor.MiddleCenter,
                    normal = { textColor = textColor },
                    fontSize = 14
                };
                // Some magic number :(
                rect.width -= 120f;
                // EditorGUI.DrawRect(rect, Color.black);
                EditorGUI.LabelField(
                    rect,
                    $"{spr.name}\n{spr.rect.width}x{spr.rect.height}",
                    style
                );
            }
        }
    }
}