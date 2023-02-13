// Copyright (c) 2020 Nementic Games GmbH.

namespace Nementic.SelectionUtility
{
	using System;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	/// <summary>
	/// The main entry point of the tool which handles the SceneView callback.
	/// </summary>
	[InitializeOnLoad]
	internal static class SceneViewGuiHandler
	{
		private static bool initialized;
		private static MouseTracker mouseTracker;
		private static int controlIDHint;
		private static List<GameObject> gameObjectBuffer;

		static SceneViewGuiHandler()
		{
			UserPrefs.Enabled.ValueChanged += SetEnabled;
			SetEnabled(UserPrefs.Enabled);
		}

		private static void SetEnabled(bool enabled)
		{
#if UNITY_2019_1_OR_NEWER
			SceneView.beforeSceneGui -= OnSceneGUI;
#else
			SceneView.onSceneGUIDelegate -= OnSceneGUI;
#endif

			if (enabled)
			{
#if UNITY_2019_1_OR_NEWER
				SceneView.beforeSceneGui += OnSceneGUI;
#else
				SceneView.onSceneGUIDelegate += OnSceneGUI;
#endif

				// Lazy-initialize members to avoid allocating memory
				// if the tool has been disabled in user preferences.
				if (initialized == false)
				{
					mouseTracker = new MouseTracker();
					controlIDHint = "NementicSelectionUtility".GetHashCode();
					gameObjectBuffer = new List<GameObject>();
					initialized = true;
				}
			}
		}

		private static void OnSceneGUI(SceneView sceneView)
		{
			try
			{
				Event current = Event.current;
				int id = GUIUtility.GetControlID(controlIDHint, FocusType.Passive);

				if (mouseTracker.ValidContextClick(current, id))
				{
					OnValidContextClick(current);
				}
			}
			catch (Exception ex)
			{
				// When something goes wrong, we need to reset hotControl or else
				// the SceneView mouse cursor will stay stuck as a dragging hand.
				GUIUtility.hotControl = 0;

				// When opening a UnityEditor.PopupWindow EditGUI throws an exception
				// to break out of the GUI loop. We want to ignore this but still log
				// all other unintended exceptions potentially caused by this tool.
				if (ex.GetType() != typeof(ExitGUIException))
					Debug.LogException(ex);
			}
		}

		private static void OnValidContextClick(Event current)
		{
			GUIUtility.hotControl = 0;
			current.Use();

			IList<GameObject> gameObjects = PickGameObjects(current.mousePosition);

			if (gameObjects.Count > 0)
			{
				Rect activatorRect = new Rect(current.mousePosition, Vector2.zero);
				ShowSelectionPopup(activatorRect, gameObjects);
			}
		}

		private static void ShowSelectionPopup(Rect rect, IList<GameObject> options)
		{
			var content = new SelectionPopup(options);
			PopupWindow.Show(rect, content);
		}

		public static Rect GUIToScreenRect(Rect guiRect)
		{
			Vector2 vector = GUIUtility.GUIToScreenPoint(new Vector2(guiRect.x, guiRect.y));
			guiRect.x = vector.x;
			guiRect.y = vector.y;
			return guiRect;
		}

		/// <summary>
		/// Returns all GameObjects under the provided mouse position.
		/// </summary>
		/// <remarks>
		/// Unity does not provide an API to retrieve all GameObjects at a SceneView screen position.
		/// Instead, pick objects one by one since Unity cycles through them. With each iteration
		/// the already picked items are stored in the ignore list that is passed to HandleUtility.PickGameObject.
		/// </remarks>
		public static IList<GameObject> PickGameObjects(Vector2 mousePosition)
		{
			// HandleUtility.PickGameObject only accepts a fixed size array,
			// that may not contain null elements. Hence, the collection needs
			// to be dynamic even if I'd like to optimize this.
			gameObjectBuffer.Clear();

			while (true)
			{
				// This is main performance bottleneck, but it seems impossible
				// to improve due to the implementation of this Unity API.
				GameObject go = HandleUtility.PickGameObject(
					mousePosition,
					selectPrefabRoot: false,
					ignore: gameObjectBuffer.ToArray());

				if (go == null)
					break;

				gameObjectBuffer.Add(go);
			}

			return gameObjectBuffer;
		}
	}
}
