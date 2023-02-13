#region copyright
// ------------------------------------------------------
// Copyright (C) Dmitriy Yukhanov [https://codestage.net]
// ------------------------------------------------------
#endregion

namespace CodeStage.AntiCheat.EditorCode
{
	using Common;
	using System.Runtime.InteropServices;
	using ObscuredTypes;
	using PropertyDrawers;
	using UnityEditor;
	using UnityEngine;
	using Object = UnityEngine.Object;

	/// <summary>
	/// Class with utility functions to help with ACTk migrations after updates.
	/// </summary>
	public static partial class MigrateUtils
	{
		private const string MigrationVersion = "2";

		/// <summary>
		/// Checks all prefabs in project for old version of obscured types and tries to migrate values to the new version.
		/// </summary>
		public static void MigrateObscuredTypesOnPrefabs(params string[] typesToMigrate)
		{
			if (!EditorUtility.DisplayDialog("ACTk Obscured types migration",
				"Are you sure you wish to scan all prefabs in your project and automatically migrate values to the new format?\n" +
				GetWhatMigratesString(typesToMigrate),
				"Yes", "No"))
			{
				Debug.Log(ACTk.LogPrefix + "Obscured types migration was canceled by user.");
				return;
			}

			EditorTools.TraverseSerializedScriptsAssets(ProcessProperty, typesToMigrate);
		}

		/// <summary>
		/// Checks all scenes in project for old version of obscured types and tries to migrate values to the new version.
		/// </summary>
		public static void MigrateObscuredTypesInScene(params string[] typesToMigrate)
		{
			if (!EditorUtility.DisplayDialog("ACTk Obscured types migration",
				"Are you sure you wish to scan all opened scenes and automatically migrate values to the new format?\n" +
				GetWhatMigratesString(typesToMigrate),
				"Yes", "No"))
			{
				Debug.Log(ACTk.LogPrefix + "Obscured types migration was canceled by user.");
				return;
			}
			
			EditorTools.TraverseSerializedScriptsInScenes(ProcessProperty, typesToMigrate);
		}

		private static bool ProcessProperty(Object target, SerializedProperty sp, string label, string type)
		{
			var modified = false;

			switch (type)
			{
				case "ObscuredDouble":
				{
					modified = MigrateObscuredDouble(sp);
					break;
				}
				case "ObscuredFloat":
				{
					modified = MigrateObscuredFloat(sp);
					break;
				}
				case "ObscuredVector2":
				{
					modified = MigrateObscuredVector2(sp);
					break;
				}
				case "ObscuredVector3":
				{
					modified = MigrateObscuredVector3(sp);
					break;
				}
				case "ObscuredQuaternion":
				{
					modified = MigrateObscuredQuaternion(sp);
					break;
				}
				case "ObscuredString":
				{
					modified = MigrateObscuredStringIfNecessary(sp);
					break;
				}
			}
			
			if (modified)
				Debug.Log($"{ACTk.LogPrefix}Migrated property {sp.displayName}:{type} at the object {label}");

			return modified;
		}

		internal static bool MigrateObscuredStringIfNecessary(SerializedProperty sp)
		{
			var hiddenValueProperty = sp.FindPropertyRelative("hiddenValue");
			if (hiddenValueProperty == null) return false;

			var currentCryptoKeyOldProperty = sp.FindPropertyRelative("currentCryptoKey");
			if (currentCryptoKeyOldProperty == null) return false;

			var currentCryptoKeyOld = currentCryptoKeyOldProperty.stringValue;
			if (string.IsNullOrEmpty(currentCryptoKeyOld)) return false;

			var hiddenCharsProperty = sp.FindPropertyRelative("hiddenChars");
			if (hiddenCharsProperty == null) return false;

			var hiddenValue = ObscuredStringDrawer.GetBytesObsolete(hiddenValueProperty);

			var decrypted =
				ObscuredString.EncryptDecryptObsolete(ObscuredString.GetStringObsolete(hiddenValue),
					currentCryptoKeyOld);

			var currentCryptoKey = ObscuredString.GenerateKey();
			var hiddenChars = ObscuredString.InternalEncryptDecrypt(decrypted.ToCharArray(), currentCryptoKey);

			ObscuredStringDrawer.SetChars(hiddenCharsProperty, hiddenChars);
			var currentCryptoKeyProperty = sp.FindPropertyRelative("cryptoKey");
			ObscuredStringDrawer.SetChars(currentCryptoKeyProperty, currentCryptoKey);

			hiddenValueProperty.arraySize = 0;
			currentCryptoKeyOldProperty.stringValue = null;

			return true;
		}

		private static string GetWhatMigratesString(string[] typesToMigrate)
		{
			return string.Join(", ", typesToMigrate) + " will migrated.";
		}

		[StructLayout(LayoutKind.Explicit)]
		private struct LongBytesUnion
		{
			[FieldOffset(0)]
			public readonly long l;

			[FieldOffset(0)]
			public ACTkByte8 b8;
		}

		[StructLayout(LayoutKind.Explicit)]
		private struct FloatIntBytesUnion
		{
			[FieldOffset(0)]
			public int i;

			[FieldOffset(0)]
			public ACTkByte4 b4;
		}
	}
}