using UnityEngine;
using UnityEditor;

namespace MondayOFF {
    [CustomPropertyDrawer(typeof(DisableEditing))]
    public class DisableEditingDrawer : PropertyDrawer {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.PropertyField(rect, property, label, true);
            EditorGUI.EndDisabledGroup();
        }
    }
}