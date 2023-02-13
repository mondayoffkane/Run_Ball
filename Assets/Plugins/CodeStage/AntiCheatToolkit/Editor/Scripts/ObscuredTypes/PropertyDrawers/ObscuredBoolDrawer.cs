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

	[CustomPropertyDrawer(typeof(ObscuredBool))]
	internal class ObscuredBoolDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty sp, GUIContent label)
		{
			var hiddenValue = sp.FindPropertyRelative(nameof(ObscuredBool.hiddenValue));
			var cryptoKey = sp.FindPropertyRelative(nameof(ObscuredBool.currentCryptoKey));
			var inited = sp.FindPropertyRelative(nameof(ObscuredBool.inited));
			var fakeValue = sp.FindPropertyRelative(nameof(ObscuredBool.fakeValue));
			var fakeValueActive = sp.FindPropertyRelative(nameof(ObscuredBool.fakeValueActive));

			var currentCryptoKey = cryptoKey.intValue;
			var val = false;

			if (!inited.boolValue)
			{
				if (currentCryptoKey == 0)
					currentCryptoKey = cryptoKey.intValue = ObscuredBool.GenerateKey();
				inited.boolValue = true;
				hiddenValue.intValue = ObscuredBool.Encrypt(false, (byte)currentCryptoKey);
			}
			else
			{
				val = ObscuredBool.Decrypt(hiddenValue.intValue, (byte)currentCryptoKey);
			}

			label = EditorGUI.BeginProperty(position, label, sp);

			EditorGUI.BeginChangeCheck();
			val = EditorGUI.Toggle(position, label, val);
			if (EditorGUI.EndChangeCheck())
			{
				hiddenValue.intValue = ObscuredBool.Encrypt(val, (byte)currentCryptoKey);

				fakeValue.boolValue = val;
				fakeValueActive.boolValue = true;
			}

			EditorGUI.EndProperty();
		}
	}
}