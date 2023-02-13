// Copyright (c) 2020 Nementic Games GmbH.

namespace Nementic.SelectionUtility
{
	using UnityEngine;
	using UnityEditor;

	internal class BoolSetting : Setting<bool>
	{
		public BoolSetting(string key, bool defaultValue = false) : base(key, defaultValue)
		{
		}

		protected override bool DrawProperty(GUIContent label, bool value)
		{
			return EditorGUILayout.Toggle(label, value);
		}

		protected override bool LoadValue()
		{
			return EditorPrefs.GetBool(key, defaultValue);
		}

		protected override void SaveValue(bool value)
		{
			EditorPrefs.SetBool(key, value);
		}
	}
}
