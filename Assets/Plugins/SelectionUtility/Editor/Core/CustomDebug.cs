// Copyright (c) 2020 Nementic Games GmbH.

namespace Nementic.SelectionUtility
{
#if SELECTION_UTILITY_DEBUG
	using UnityEditor;
#endif
	using UnityEngine;

	internal static class CustomDebug
	{
		public const string conditionString = "SELECTION_UTILITY_DEBUG";

		[System.Diagnostics.Conditional(conditionString)]
		public static void Log(string message)
		{
			Debug.Log(message);
		}

#if SELECTION_UTILITY_DEBUG
		[MenuItem("Tools/Selection Utility/Delete Prefs")]
		public static void DeletePrefs()
		{
			UserPrefs.DeleteAll();
		}
#endif
	}
}