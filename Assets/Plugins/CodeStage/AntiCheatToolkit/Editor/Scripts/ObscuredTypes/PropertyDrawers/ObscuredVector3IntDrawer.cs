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

	[CustomPropertyDrawer(typeof(ObscuredVector3Int))]
	internal class ObscuredVector3IntDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
		{
			var hiddenValue = prop.FindPropertyRelative(nameof(ObscuredVector3Int.hiddenValue));
			var cryptoKey = prop.FindPropertyRelative(nameof(ObscuredVector3Int.currentCryptoKey));
			var inited = prop.FindPropertyRelative(nameof(ObscuredVector3Int.inited));
			var fakeValue = prop.FindPropertyRelative(nameof(ObscuredVector3Int.fakeValue));
			var fakeValueActive = prop.FindPropertyRelative(nameof(ObscuredVector3Int.fakeValueActive));
			
			var hiddenValueX = hiddenValue.FindPropertyRelative(nameof(ObscuredVector3Int.RawEncryptedVector3Int.x));
			var hiddenValueY = hiddenValue.FindPropertyRelative(nameof(ObscuredVector3Int.RawEncryptedVector3Int.y));
			var hiddenValueZ = hiddenValue.FindPropertyRelative(nameof(ObscuredVector3Int.RawEncryptedVector3Int.z));

			var currentCryptoKey = cryptoKey.intValue;
			var val = Vector3Int.zero;

			if (!inited.boolValue)
			{
				if (currentCryptoKey == 0)
					currentCryptoKey = cryptoKey.intValue = ObscuredVector3Int.GenerateKey();
				var ev = ObscuredVector3Int.Encrypt(Vector3Int.zero, currentCryptoKey);
				hiddenValueX.intValue = ev.x;
				hiddenValueY.intValue = ev.y;
				hiddenValueZ.intValue = ev.z;
                inited.boolValue = true;

				fakeValue.vector3IntValue = Vector3Int.zero;
			}
			else
			{
				var ev = new ObscuredVector3Int.RawEncryptedVector3Int
				{
					x = hiddenValueX.intValue,
					y = hiddenValueY.intValue,
					z = hiddenValueZ.intValue
				};
				val = ObscuredVector3Int.Decrypt(ev, currentCryptoKey);
			}

			label = EditorGUI.BeginProperty(position, label, prop);

			EditorGUI.BeginChangeCheck();
			val = EditorGUI.Vector3IntField(position, label, val);
			if (EditorGUI.EndChangeCheck())
			{
				var ev = ObscuredVector3Int.Encrypt(val, currentCryptoKey);
				hiddenValueX.intValue = ev.x;
				hiddenValueY.intValue = ev.y;
				hiddenValueZ.intValue = ev.z;

				fakeValue.vector3IntValue = val;
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