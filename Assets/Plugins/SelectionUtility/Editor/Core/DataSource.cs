// Copyright (c) 2020 Nementic Games GmbH.

namespace Nementic.SelectionUtility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text.RegularExpressions;
	using UnityEditor;
	using UnityEditor.IMGUI.Controls;
	using UnityEngine;

	/// <summary>
	/// Holds the collection of GameObjects that can be picked and
	/// applies search string filtering to it.
	/// </summary>
	/// <remarks>
	/// Remember that GameObjects in these lists can be destroyed and become null at any time.
	/// </remarks>
	internal class DataSource
	{
		/// <summary>
		/// The original collection of all GameObjects at the mouse position.
		/// </summary>
		public readonly IList<GameObject> items;

		/// <summary>
		/// The collection of GameObjects after the search filter has been applied.
		/// </summary>
		public readonly List<GameObject> filteredItems;

		private int FilterIndex
		{
			get { return filterIndex; }
			set
			{
				if (value == filterIndex)
					return;

				filterIndex = Mathf.Clamp(value, 0, filters.Count);
				EditorPrefs.SetString("SelectionUtility.DataSourceFilter", filters[value].ShortName);
				OnSearchChanged();
			}
		}

		private int filterIndex;

		private DataFilter CurrentFilter
		{
			get
			{
				if(UserPrefs.ShowFilterToolbar)
					return filters[filterIndex];

				return DataFilter.PassThrough;
			}
		}

		/// <summary>
		/// All currently used filters. At runtime start, the list contains the default
		/// filters, but clients may add new ones during InitializeOnLoad.
		/// </summary>
		private static List<DataFilter> filters;

		private static readonly List<DataFilter> defaultFilters = new List<DataFilter>
		{
			// Default filter that lets everything through.
			DataFilter.PassThrough,

			// Models, characters and primitives.
			new DataFilter("3D", go =>
				go.GetComponent<MeshRenderer>() != null ||
				go.GetComponent<SkinnedMeshRenderer>() != null),

			// A category for sprites (maybe Tilemap in the future).
			new DataFilter("2D", go => go.GetComponent<SpriteRenderer>() != null),

			// For the Unity 4.6 UI consider everything that has a RectTransform.
			// This will include 3D objects that are part of the UI hierarchy.
			// Alternatively we could only consider CanvasRenderer.
			new DataFilter("UI", go => go.transform is RectTransform)
		};

		private string searchString = string.Empty;
		private string searchStringPrevious = string.Empty;
		private readonly SearchField searchField = new SearchField();
		private readonly GUIContent[] filterNames;

		private static GUI.ToolbarButtonSize buttonSize
		{
			get
			{
				if (filters.All(x => x.ShortName.Length < 5))
					return GUI.ToolbarButtonSize.Fixed;
				else
					return GUI.ToolbarButtonSize.FitToContents;
			}
		}

		public DataSource(IList<GameObject> options)
		{
			this.items = options;
			this.filteredItems = options.ToList();

			filters = CustomOrDefaultFilters();
			filterIndex = RefreshSelectedFilter(filters);
			filterNames = filters.Select(x => new GUIContent(x.ShortName)).ToArray();

			OnSearchChanged();
		}

		private static List<DataFilter> CustomOrDefaultFilters()
		{
			var filters = defaultFilters;
			if (SelectionPopupExtensions.FilterModifier != null)
			{
				var customFilters = SelectionPopupExtensions.FilterModifier
					.Invoke(defaultFilters.ToList())
					.Where(x => x != null).ToList();

				if (customFilters.Count == 0)
				{
					Debug.LogError("Must provide at least one filter. " +
					               "Turn off the filter toolbar via the preferences item " +
					               "if you wish to disable filtering entirely.");
				}
				else
				{
					filters = customFilters;
				}
			}

			return filters;
		}

		private static int RefreshSelectedFilter(List<DataFilter> filters)
		{
			string selectedFilter = EditorPrefs.GetString("SelectionUtility.DataSourceFilter", "All");
			var index = filters.FindIndex(x => x.ShortName == selectedFilter);
			return index != -1 ? index : 0;
		}

		public static float MinimumWidth()
		{
			float size = 0f;

			for (int i = 0; i < filters.Count; i++)
			{
				float width = EditorStyles.miniButton.CalcSize(
					new GUIContent(filters[i].ShortName)).x;

				if (buttonSize == GUI.ToolbarButtonSize.Fixed)
				{
					if (width > size)
						size = width;
				}
				else
				{
					size += width;
				}
			}

			if (buttonSize == GUI.ToolbarButtonSize.Fixed)
				return size * filters.Count;
			else
				return size;
		}

		public void FocusSearch()
		{
			if (UserPrefs.ShowSearchField)
				searchField.SetFocus();
		}

		public Rect SearchFieldGUI(Rect rect, float height)
		{
			if (UserPrefs.ShowSearchField)
			{
				Rect searchRect = rect;
				searchRect.height = height;
				searchRect.yMin += 3;
				searchRect.xMin += 4;
				searchRect.xMax -= 4;

				EditorGUI.BeginChangeCheck();
				searchString = searchField.OnToolbarGUI(searchRect, searchString);

				// Because of an issue in Unity 2020.2.1 this additional check is required.
				if (EditorGUI.EndChangeCheck() || searchString != searchStringPrevious)
				{
					searchStringPrevious = searchString;
					OnSearchChanged();
				}

				rect.yMin = searchRect.yMax + 1;
			}

			return rect;
		}

		public Rect DrawFilterModes(Rect rect, float rowHeight)
		{
			if (UserPrefs.ShowFilterToolbar)
			{
				DrawToolbar();
				rect.yMin += rowHeight + 2;
			}
			return rect;
		}

		private void DrawToolbar()
		{
			GUILayout.Space(3f);
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			// Due to a small layouting issue the toolbar would be
			// offset from the center otherwise.
			GUILayout.Space(2f);
			if (buttonSize == GUI.ToolbarButtonSize.FitToContents)
				GUILayout.Space(4f);

			FilterIndex = GUILayout.Toolbar(
				FilterIndex,
				filterNames,
				GUI.skin.button,
				buttonSize);

			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		private void OnSearchChanged()
		{
			// To polish the search experience:
			// - Remove white space at the start and end of the search string.
			// - Ignore multiple spaces in a row by collapsing them down to a single one.
			// - Ignore letter case.
			string value = Regex.Replace(searchString.Trim(), @"[ ]+", " ");

			// Extract any component type query. User must enter 't:' followed by a type name,
			// with or without spaces in between. Also match if no type name is specified (yet)
			// so that the search window shows all entries until the type name is started.
			var match = Regex.Match(value, @"t:\s*(\w*)");
			string typeName = null;

			if (match.Success)
			{
				value = value.Replace(match.Value, string.Empty).Trim();
				typeName = match.Groups[1].Value.Trim().ToLower();
			}

			RefreshFilteredItems(typeName, value);
		}

		private void RefreshFilteredItems(string typeName, string searchValue)
		{
			filteredItems.Clear();
			for (int i = 0; i < items.Count; i++)
			{
				if (items[i] == null)
					continue;

				// It would be possible to use GetComponent(string)
				// to check for type match, but this would be case-sensitive.
				// For case-insensitive lookup, compare each component name.
				bool hasMatchingComponent = false;
				if (typeName != null)
				{
					var components = items[i].GetComponents<Component>();
					for (int j = 0; j < components.Length; j++)
					{
						Component comp = components[j];

						if (comp != null && comp.GetType().Name.ToLower() == typeName)
						{
							hasMatchingComponent = true;
							break;
						}
					}
				}

				if (items[i].name.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0 &&
				    (typeName == null || hasMatchingComponent) &&
				    CurrentFilter.IsAllowed(items[i]))
				{
					filteredItems.Add(items[i]);
				}
			}
		}

		public static int CountValidItems(IList<GameObject> items)
		{
			if (items == null)
				return 0;

			int totalCount = items.Count;
			int count = 0;

			for (int i = 0; i < totalCount; i++)
			{
				if (items[i] != null)
					count++;
			}

			return count;
		}
	}
}
