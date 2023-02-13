#if UNITY_2019_2_OR_NEWER

namespace CodeStage.AntiCheat.EditorCode
{
	using System.Linq;
	using Common;
	using ObscuredTypes;
	using UnityEditor;
	using UnityEngine;

	public delegate void InvalidPropertyFound(Object target, SerializedProperty sp, string location, string type);
	
	public static class ObscuredTypesValidator
	{
		private static InvalidPropertyFound lastPassedCallback;
		
		/// <summary>
		/// Traverses all prefabs and scriptable objects in the project and checks if they contain any Obscured types with anomalies.
		/// </summary>
		/// <param name="callback">Pass callback if you wish to process invalid objects.</param>
		public static void ValidateProjectAssets(InvalidPropertyFound callback = null)
		{
			if (!EditorUtility.DisplayDialog("ACTk Obscured types validation",
					"Are you sure you wish to scan all assets (except scenes) in your project and validate all found obscured types?\n" +
					"This can take some time to complete.",
					"Yes", "No"))
			{
				Debug.Log(ACTk.LogPrefix + "Obscured types validation was canceled by user.");
				return;
			}

			var types = TypeCache.GetTypesDerivedFrom<IObscuredType>().Where(t => !t.IsAbstract && !t.IsInterface)
				.Select(type => type.Name).ToArray();
			lastPassedCallback = callback;
			EditorTools.TraverseSerializedScriptsAssets(ValidateProperty, types);
			lastPassedCallback = null;
		}

		/// <summary>
		/// Traverse all currently opened scenes and checks if they contain any Components with non-valid Obscured types.
		/// </summary>
		/// <param name="callback">Pass callback if you wish to process invalid objects.</param>
		public static void ValidateOpenedScenes(InvalidPropertyFound callback = null)
		{
			if (!EditorUtility.DisplayDialog("ACTk Obscured types validation",
				    "Are you sure you wish to scan all opened Scenes and validate all found obscured types?\n" +
				    "This can take some time to complete.",
				    "Yes", "No"))
			{
				Debug.Log(ACTk.LogPrefix + "Obscured types validation was canceled by user.");
				return;
			}

			var types = TypeCache.GetTypesDerivedFrom<IObscuredType>().Where(t => !t.IsAbstract && !t.IsInterface)
				.Select(type => type.Name).ToArray();
			lastPassedCallback = callback;
			EditorTools.TraverseSerializedScriptsInScenes(ValidateProperty, types);
			lastPassedCallback = null;
		}
		
		private static bool ValidateProperty(Object target, SerializedProperty sp, string location, string type)
		{
			var obscured = sp.GetValue<IObscuredType>();
			if (obscured is ObscuredBigInteger obi && !obi.IsDataValid ||
			    obscured is ObscuredBool ob && !ob.IsDataValid ||
			    obscured is ObscuredDouble od && !od.IsDataValid ||
			    obscured is ObscuredFloat of && !of.IsDataValid ||
			    obscured is ObscuredInt oi && !oi.IsDataValid ||
			    obscured is ObscuredLong ol && !ol.IsDataValid ||
			    obscured is ObscuredQuaternion oq && !oq.IsDataValid ||
			    obscured is ObscuredShort os && !os.IsDataValid ||
			    obscured is ObscuredString ost && !ost.IsDataValid ||
			    obscured is ObscuredUInt oui && !oui.IsDataValid ||
			    obscured is ObscuredULong oul && !oul.IsDataValid ||
			    obscured is ObscuredVector2 ov2 && !ov2.IsDataValid ||
			    obscured is ObscuredVector2Int ov2Int && !ov2Int.IsDataValid ||
			    obscured is ObscuredVector3 ov3 && !ov3.IsDataValid ||
			    obscured is ObscuredVector3Int ov3Int && !ov3Int.IsDataValid)
			{
				lastPassedCallback?.Invoke(target, sp, location, type);
				target = EditorTools.GetPingableObject(target);
				Debug.LogError($"{ACTk.LogPrefix}Obscured Types Validator found invalid Property [{sp.displayName}] of type [{type}] at:\n{location}", target);
			}

			return false;
		}
	}
}

#endif