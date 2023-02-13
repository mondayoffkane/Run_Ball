// Copyright (c) 2020 Nementic Games GmbH.

namespace Nementic.SelectionUtility
{
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	/// <summary>
	/// Provides access to the tool settings stored on the local machine.
	/// </summary>
	[InitializeOnLoad]
	internal static class UserPrefs
	{
		[Tooltip("True if the tool should be available when clicking in the SceneView.")]
		public static readonly Setting<bool> Enabled;

		[Tooltip("Mouse movement between click down and up larger than this value will be interpreted as scene view camera panning" +
			"instead of triggering the selection popup.")]
		public static readonly Setting<int> ContextClickPixelThreshold;

		[Tooltip("True if the popup should show a search field.")]
		public static readonly Setting<bool> ShowSearchField;

		[Tooltip("True if duplicate icons should be shown.")]
		public static readonly Setting<bool> ShowDuplicateIcons;

		[Tooltip("True if the icon for null components should be shown. This is useful to discover missing scripts.")]
		public static readonly Setting<bool> ShowMissingScriptIcons;

		[Tooltip("True if the popup should show toolbar buttons to filter the selection to 3D, 2D or UI.")]
		public static readonly Setting<bool> ShowFilterToolbar;
		
		[Tooltip("When hovering over items in the list, GameObjects in the scene are highlighted with an outline.")]
		public static readonly  Setting<bool> EnableHoverSelectionOutline;
		
		public static readonly Setting<float> OutlineThickness;
		
		public static readonly Setting<Color> OutlineColor;

		[Tooltip("These type names are not displayed as component icons in the popup window.")]
		public static readonly StringListSetting HiddenIconTypeNames;

		static UserPrefs()
		{
			// Use static constructor to fixate the draw order of the properties.
			settings = new List<ISetting>();
			Enabled = new BoolSetting("Enabled", true);
			ContextClickPixelThreshold = new IntSetting("ClickDeadZone", defaultValue: 10, min: 0, max: int.MaxValue);
			ShowFilterToolbar = new BoolSetting("ShowFilterToolbar", true);
			ShowSearchField = new BoolSetting("ShowSearchField", true);
			ShowDuplicateIcons = new BoolSetting("ShowDuplicateIcons", false);
			ShowMissingScriptIcons = new BoolSetting("ShowMissingScriptIcons", true);
			
			EnableHoverSelectionOutline = new BoolSetting("EnableHoverSelectionOutline", true);
			OutlineThickness = new FloatSetting("HoverSelectionOutlineThickness", 1f, 0f, 2f);
			OutlineColor = new ColorSetting("HoverSelectionOutlineColor", new Color(1f, 0.62f, 0.25f, 0.09f));

			var types = new List<string> { "Transform", "MeshFilter", "ParticleSystemRenderer" };
			HiddenIconTypeNames = new StringListSetting("HiddenIcons", types);

			LogSettings();
		}

		[System.Diagnostics.Conditional(CustomDebug.conditionString)]
		private static void LogSettings()
		{
			var sb = new System.Text.StringBuilder();
			sb.AppendLine("Initialized user prefs:");
			sb.AppendLine("Enabled: " + Enabled.Value);
			sb.AppendLine("ShowSearchField: " + ShowSearchField.Value);
			sb.AppendLine("ContextClickPixelThreshold: " + ContextClickPixelThreshold.Value);
			sb.AppendLine("HiddenIconTypeNames: " + HiddenIconTypeNames.ToString());

			CustomDebug.Log(sb.ToString());
		}

		/// <summary>
		/// The list to which all settings register to so that they can be iterated.
		/// </summary>
		private static readonly List<ISetting> settings;

		internal static void Register(ISetting setting)
		{
			settings.Add(setting);
		}

#if UNITY_2018_3_OR_NEWER
		[SettingsProvider]
        private static SettingsProvider CreateSettings()
        {
            return new SettingsProvider("Nementic/Selection Utility", SettingsScope.User)
            {
                guiHandler = (searchContext) =>
                {
					DrawSettings();
				},
                keywords = new HashSet<string>(new[] { "Nementic", "Selection", "Utility" })
            };
        }
#else
		[PreferenceItem("Nementic/Selection Utility")]
		private static void OnPreferencesGUI()
		{
			DrawSettings();
		}
#endif

		private static void DrawSettings()
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(8);
			EditorGUILayout.BeginVertical();
			EditorGUIUtility.labelWidth += 55;

			for (int i = 0; i < settings.Count; i++)
				settings[i].DrawProperty();

			EditorGUIUtility.labelWidth -= 55;

			EditorGUILayout.Space();

			if (GUILayout.Button("Use Defaults", GUILayout.Width(120)))
				ResetToDefaults();

			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
		}

		private static void ResetToDefaults()
		{
			for (int i = 0; i < settings.Count; i++)
				settings[i].Reset();

			// Stop text editing if any of the input fields are focused, because
			// otherwise they don't update until the user deselects them.
			GUI.FocusControl(null);
		}

		public static void DeleteAll()
		{
			if (settings != null)
			{
				foreach (var setting in settings)
					setting.Delete();
			}
		}
	}
}
