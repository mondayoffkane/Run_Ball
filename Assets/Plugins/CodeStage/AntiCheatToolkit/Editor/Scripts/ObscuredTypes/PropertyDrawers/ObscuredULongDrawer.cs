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

	[CustomPropertyDrawer(typeof(ObscuredULong))]
	internal class ObscuredULongDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
		{
			var hiddenValue = prop.FindPropertyRelative(nameof(ObscuredULong.hiddenValue));
			var cryptoKey = prop.FindPropertyRelative(nameof(ObscuredULong.currentCryptoKey));
			var inited = prop.FindPropertyRelative(nameof(ObscuredULong.inited));
			var fakeValue = prop.FindPropertyRelative(nameof(ObscuredULong.fakeValue));
			var fakeValueActive = prop.FindPropertyRelative(nameof(ObscuredULong.fakeValueActive));

			var currentCryptoKey = (ulong)cryptoKey.longValue;
			ulong val = 0;

			if (!inited.boolValue)
			{
				if (currentCryptoKey == 0)
					cryptoKey.longValue = (long)(currentCryptoKey = ObscuredULong.GenerateKey());
				hiddenValue.longValue = (long)ObscuredULong.Encrypt(0, currentCryptoKey);
				inited.boolValue = true;
			}
			else
			{
				val = ObscuredULong.Decrypt((ulong)hiddenValue.longValue, currentCryptoKey);
			}

			label = EditorGUI.BeginProperty(position, label, prop);

			EditorGUI.BeginChangeCheck();
			val = (ulong)EditorGUI.LongField(position, label, (long)val);
			if (EditorGUI.EndChangeCheck())
			{
				hiddenValue.longValue = (long)ObscuredULong.Encrypt(val, currentCryptoKey);
				fakeValue.longValue = (long)val;
				fakeValueActive.boolValue = true;
			}
			EditorGUI.EndProperty();
		}
	}
}