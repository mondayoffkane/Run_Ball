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

	[CustomPropertyDrawer(typeof(ObscuredVector3))]
	internal class ObscuredVector3Drawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
		{
			var hiddenValue = prop.FindPropertyRelative(nameof(ObscuredVector3.hiddenValue));
			var cryptoKey = prop.FindPropertyRelative(nameof(ObscuredVector3.currentCryptoKey));
			var inited = prop.FindPropertyRelative(nameof(ObscuredVector3.inited));
			var fakeValue = prop.FindPropertyRelative(nameof(ObscuredVector3.fakeValue));
			var fakeValueActive = prop.FindPropertyRelative(nameof(ObscuredVector3.fakeValueActive));
			
			var hiddenValueX = hiddenValue.FindPropertyRelative(nameof(ObscuredVector3.RawEncryptedVector3.x));
			var hiddenValueY = hiddenValue.FindPropertyRelative(nameof(ObscuredVector3.RawEncryptedVector3.y));
			var hiddenValueZ = hiddenValue.FindPropertyRelative(nameof(ObscuredVector3.RawEncryptedVector3.z));

			var currentCryptoKey = cryptoKey.intValue;
			var val = Vector3.zero;

			if (!inited.boolValue)
			{
				if (currentCryptoKey == 0)
					currentCryptoKey = cryptoKey.intValue = ObscuredVector3.GenerateKey();
				var ev = ObscuredVector3.Encrypt(Vector3.zero, currentCryptoKey);
				hiddenValueX.intValue = ev.x;
				hiddenValueY.intValue = ev.y;
				hiddenValueZ.intValue = ev.z;
                inited.boolValue = true;

				fakeValue.vector3Value = Vector3.zero;
			}
			else
			{
				var ev = new ObscuredVector3.RawEncryptedVector3
				{
					x = hiddenValueX.intValue,
					y = hiddenValueY.intValue,
					z = hiddenValueZ.intValue
				};
				val = ObscuredVector3.Decrypt(ev, currentCryptoKey);
			}

			label = EditorGUI.BeginProperty(position, label, prop);

			EditorGUI.BeginChangeCheck();
			val = EditorGUI.Vector3Field(position, label, val);
			if (EditorGUI.EndChangeCheck())
			{
				var ev = ObscuredVector3.Encrypt(val, currentCryptoKey);
				hiddenValueX.intValue = ev.x;
				hiddenValueY.intValue = ev.y;
				hiddenValueZ.intValue = ev.z;

				fakeValue.vector3Value = val;
				fakeValueActive.boolValue = true;
			}
			EditorGUI.EndProperty();
        }

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.wideMode ? EditorGUIUtility.singleLineHeight : EditorGUIUtility.singleLineHeight * 2f;
		}
	}
}