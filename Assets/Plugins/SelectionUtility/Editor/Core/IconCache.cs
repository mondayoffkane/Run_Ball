// Copyright (c) 2020 Nementic Games GmbH.

namespace Nementic.SelectionUtility
{
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	internal class IconCache
	{
		/// <summary>
		/// Caches the list of component icons for each GameObject to improve performance.
		/// </summary>
		private readonly Dictionary<GameObject, Texture2D[]> iconLookup = new Dictionary<GameObject, Texture2D[]>(64);

		/// <summary>
		/// The set of icons displayed for the current target.
		/// </summary>
		private readonly List<Texture2D> displayedIcons = new List<Texture2D>();

		private GameObject currentTarget;

		private readonly List<Component> components = new List<Component>(8);

		private readonly int defaultAssetHash = "DefaultAsset".GetHashCode();

		public int CacheIcons(GameObject gameObject)
		{
			gameObject.GetComponents<Component>(components);
			BeginCache(gameObject);

			for (int j = 0; j < components.Count; j++)
				CacheIcon(components[j]);

			return EndCache();
		}

		/// <summary>
		/// Begins icon caching for the provided GameObject in a temporary buffer.
		/// </summary>
		private void BeginCache(GameObject gameObject)
		{
			displayedIcons.Clear();
			currentTarget = gameObject;
		}

		private void CacheIcon(Component component)
		{
			Texture2D icon = null;
			string typeName = null;

			if (component != null)
			{
				icon = AssetPreview.GetMiniThumbnail(component);
				typeName = component.GetType().Name;
			}
			else if (UserPrefs.ShowMissingScriptIcons)
			{
				icon = AssetPreview.GetMiniTypeThumbnail(typeof(DefaultAsset));
				typeName = "DefaultAsset";
			}

			if (icon == null)
				return;

			if (UserPrefs.HiddenIconTypeNames.Contains(typeName))
				return;

			if (UserPrefs.ShowDuplicateIcons == false &&
				displayedIcons.Contains(icon))
			{
				return;
			}

			// The default asset icon is returned if nothing else was found,
			// and since it doesn't add much info, omit it.
			if (icon.name.GetHashCode() == defaultAssetHash)
				return;

			displayedIcons.Add(icon);
		}

		/// <summary>
		/// Ends icon caching for the current target and returns the number of cached icons
		/// </summary>
		private int EndCache()
		{
			iconLookup.Add(currentTarget, displayedIcons.ToArray());
			return displayedIcons.Count;
		}

		/// <summary>
		/// Returns the collection of icons for the provided GameObject.
		/// </summary>
		public Texture2D[] ForGameObject(GameObject gameObject)
		{
			return iconLookup[gameObject];
		}
	}
}
