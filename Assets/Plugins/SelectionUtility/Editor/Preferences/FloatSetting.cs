// Copyright (c) 2020 Nementic Games GmbH.

namespace Nementic.SelectionUtility
{
    using UnityEngine;
    using UnityEditor;

    internal class FloatSetting : Setting<float>
    {
        private readonly bool clamp;
        private readonly float minValue;
        private readonly float maxValue;

        public FloatSetting(string key, float defaultValue = 0) : base(key, defaultValue)
        {
            this.clamp = false;
        }

        public FloatSetting(string key, float defaultValue, float min, float max) : base(key, defaultValue)
        {
            this.minValue = min;
            this.maxValue = max;
            this.clamp = true;
        }

        protected override float DrawProperty(GUIContent label, float value)
        {
            return EditorGUILayout.FloatField(label, value);
        }

        public override float Value
        {
            get { return base.Value; }
            set
            {
                if (clamp)
                    value = Mathf.Clamp(value, minValue, maxValue);

                base.Value = value;
            }
        }

        protected override float LoadValue()
        {
            return EditorPrefs.GetFloat(key, defaultValue);
        }

        protected override void SaveValue(float value)
        {
            EditorPrefs.SetFloat(key, value);
        }
    }
}