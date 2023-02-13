#region copyright
// ------------------------------------------------------
// Copyright (C) Dmitriy Yukhanov [https://codestage.net]
// ------------------------------------------------------
#endregion

namespace CodeStage.AntiCheat.EditorCode.PropertyDrawers
{
	using ObscuredTypes;

	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(ObscuredUInt))]
	internal class ObscuredUIntDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
		{
			var hiddenValue = prop.FindPropertyRelative(nameof(ObscuredUInt.hiddenValue));
			var cryptoKey = prop.FindPropertyRelative(nameof(ObscuredUInt.currentCryptoKey));
			var inited = prop.FindPropertyRelative(nameof(ObscuredUInt.inited));
			var fakeValue = prop.FindPropertyRelative(nameof(ObscuredUInt.fakeValue));
			var fakeValueActive = prop.FindPropertyRelative(nameof(ObscuredUInt.fakeValueActive));

			var currentCryptoKey = (uint)cryptoKey.intValue;
			uint val = 0;

			if (!inited.boolValue)
			{
				if (currentCryptoKey == 0)
					cryptoKey.intValue = (int)(currentCryptoKey = ObscuredUInt.GenerateKey());
				hiddenValue.intValue = (int)ObscuredUInt.Encrypt(0, currentCryptoKey);
				inited.boolValue = true;
			}
			else
			{
				val = ObscuredUInt.Decrypt((uint)hiddenValue.intValue, currentCryptoKey);
			}

			label = EditorGUI.BeginProperty(position, label, prop);

			EditorGUI.BeginChangeCheck();
			val = (uint)EditorGUI.DelayedIntField(position, label, (int)val);
			if (EditorGUI.EndChangeCheck())
			{
				hiddenValue.intValue = (int)ObscuredUInt.Encrypt(val, currentCryptoKey);
				fakeValue.intValue = (int)val;
				fakeValueActive.boolValue = true;
			}
			EditorGUI.EndProperty();
		}
	}
}