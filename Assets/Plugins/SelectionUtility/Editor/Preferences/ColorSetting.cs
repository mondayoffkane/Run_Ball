// Copyright (c) 2020 Nementic Games GmbH.

namespace Nementic.SelectionUtility
{
    using UnityEditor;
    using UnityEngine;

    internal class ColorSetting : Setting<Color>
    {
        public ColorSetting(string key, Color defaultValue = default(Color)) : base(key, defaultValue)
        {
        }

        protected override Color DrawProperty(GUIContent label, Color value)
        {
            return EditorGUILayout.ColorField(label, value);
        }

        protected override Color LoadValue()
        {
            string html = EditorPrefs.GetString(key);
            
            if (html == string.Empty)
                return defaultValue;
            
            Color color;
            return ColorUtility.TryParseHtmlString("#" + html, out color) ? color : Color.white;
        }

        protected override void SaveValue(Color value)
        {
            string html = ColorUtility.ToHtmlStringRGBA(value);
            EditorPrefs.SetString(key, html);
        }
    }
}
