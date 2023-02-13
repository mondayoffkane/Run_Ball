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

	[CustomPropertyDrawer(typeof(ObscuredFloat))]
	internal class ObscuredFloatDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
		{
			var hiddenValue = prop.FindPropertyRelative(nameof(ObscuredFloat.hiddenValue));
			var cryptoKey = prop.FindPropertyRelative(nameof(ObscuredFloat.currentCryptoKey));
			var inited = prop.FindPropertyRelative(nameof(ObscuredFloat.inited));
			var fakeValue = prop.FindPropertyRelative(nameof(ObscuredFloat.fakeValue));
			var fakeValueActive = prop.FindPropertyRelative(nameof(ObscuredFloat.fakeValueActive));

			var currentCryptoKey = cryptoKey.intValue;

			float val = 0;

			if (!inited.boolValue)
			{
				if (currentCryptoKey == 0)
					currentCryptoKey = cryptoKey.intValue = ObscuredFloat.GenerateKey();

				inited.boolValue = true;
				hiddenValue.intValue = ObscuredFloat.Encrypt(0, currentCryptoKey);
			}
			else
			{
				val = ObscuredFloat.Decrypt(hiddenValue.intValue, currentCryptoKey);
			}

			label = EditorGUI.BeginProperty(position, label, prop);

			EditorGUI.BeginChangeCheck();
			val = EditorGUI.DelayedFloatField(position, label, val);
			if (EditorGUI.EndChangeCheck())
			{
				hiddenValue.intValue = ObscuredFloat.Encrypt(val, currentCryptoKey);
				fakeValue.floatValue = val;
				fakeValueActive.boolValue = true;
			}
			EditorGUI.EndProperty();
		}
	}
}