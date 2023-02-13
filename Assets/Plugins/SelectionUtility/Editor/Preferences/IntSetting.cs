// Copyright (c) 2020 Nementic Games GmbH.

namespace Nementic.SelectionUtility
{
	using UnityEngine;
	using UnityEditor;

	internal class IntSetting : Setting<int>
	{
		private readonly bool clamp;
		private readonly int minValue;
		private readonly int maxValue;

		public IntSetting(string key, int defaultValue = 0) : base(key, defaultValue)
		{
			this.clamp = false;
		}

		public IntSetting(string key, int defaultValue, int min, int max) : base(key, defaultValue)
		{
			this.minValue = min;
			this.maxValue = max;
			this.clamp = true;
		}

		protected override int DrawProperty(GUIContent label, int value)
		{
			return EditorGUILayout.IntField(label, value);
		}

		public override int Value
		{
			get { return base.Value; }
			set
			{
				if (clamp)
					value = Mathf.Clamp(value, minValue, maxValue);

				base.Value = value;
			}
		}

		protected override int LoadValue()
		{
			return EditorPrefs.GetInt(key, defaultValue);
		}

		protected override void SaveValue(int value)
		{
			EditorPrefs.SetInt(key, value);
		}
	}
}
