// Copyright (c) 2020 Nementic Games GmbH.

namespace Nementic.SelectionUtility
{
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	internal class OutlineManager
	{
		private readonly List<OutlineRenderer> renderers = new List<OutlineRenderer>();
		private GameObject target;

		/// <summary>
		/// Ensures that the provided GameObject is outlined
		/// in the scene view. Safe to call multiple times in a row.
		/// </summary>
		public void SetOutlineTarget(GameObject target)
		{
			if (UserPrefs.EnableHoverSelectionOutline == false)
				return;

			if (target == this.target)
				return;

			Clear();

			this.target = target;

			foreach (Camera camera in SceneView.GetAllSceneCameras())
			{
				if (camera == null)
					continue;

				var renderer = new OutlineRenderer(camera)
				{
					blurRadius = UserPrefs.OutlineThickness,
					outlineColor = UserPrefs.OutlineColor
				};
				var rendererComponents = target.GetComponentsInChildren<Renderer>();
				renderer.AddTargets(rendererComponents);
				renderers.Add(renderer);
			}
		}

		public void Clear()
		{
			target = null;

			if (renderers.Count > 0)
			{
				foreach (var renderer in renderers)
					renderer.Clear();

				renderers.Clear();
				SceneView.RepaintAll();
			}
		}
	}
}