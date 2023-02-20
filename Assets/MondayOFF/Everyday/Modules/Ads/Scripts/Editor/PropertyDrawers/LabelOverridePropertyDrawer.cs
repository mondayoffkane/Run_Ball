using UnityEngine;
using UnityEditor;

namespace MondayOFF {
    [CustomPropertyDrawer(typeof(LabelOverrideAttribute))]
    internal class LabelOverridePropertyDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            try {
                LabelOverrideAttribute propertyAttribute = (LabelOverrideAttribute)this.attribute;
                if (!IsArray(property)) {
                    label.text = propertyAttribute.displayName;
                } else {
                    Debug.LogWarning($"{typeof(LabelOverrideAttribute).Name}(\"{propertyAttribute.displayName}\") doesn't support arrays");
                }
                EditorGUI.PropertyField(position, property, label);
            } catch (System.Exception e) {
                Debug.LogException(e);
            }
        }

        private bool IsArray(SerializedProperty property) {
            string path = property.propertyPath;
            int idot = path.IndexOf('.');
            if (idot == -1) return false;
            string propName = path.Substring(0, idot);
            SerializedProperty p = property.serializedObject.FindProperty(propName);
            return p.isArray;
        }
    }
}