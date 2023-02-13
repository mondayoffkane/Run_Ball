// Copyright (c) 2020 Nementic Games GmbH.

namespace Nementic.SelectionUtility
{
	using UnityEditor;
	using UnityEngine;

	/// <summary>
	/// Handles the selection of GameObjects from within the popup list.
	/// </summary>
	/// <remarks>
	/// For simplicity's sake, this class only handles simple selection of
	/// a single item for now, but can be extended to allow shift-clicking
	/// multiple items by preventing the popup from closing on the first selection.
	/// </remarks>
	internal static class ObjectSelector
	{
		public static bool TrySelectObject(Event current, Rect originalRect, GameObject gameObject)
		{
			if (ValidListRowClick(current, originalRect))
			{
				if (current.shift || current.control)
					ToggleSelectedObjectAdditive(gameObject);
				else
					SelectObject(gameObject);

				return true;
			}
			return false;
		}

		private static bool ValidListRowClick(Event current, Rect originalRect)
		{
			return current.type == EventType.MouseUp
				&& current.button == 0
				&& originalRect.Contains(current.mousePosition);
		}

		private static void SelectObject(Object selectedObject)
		{
			if (Selection.activeObject != selectedObject)
				Selection.activeObject = selectedObject;
			else
				EditorGUIUtility.PingObject(selectedObject);
		}

		private static void ToggleSelectedObjectAdditive(Object selectedObject)
		{
			var selectedObjects = Selection.objects;

			if (ArrayUtility.Contains(selectedObjects, selectedObject))
				ArrayUtility.Remove(ref selectedObjects, selectedObject);
			else
				ArrayUtility.Add(ref selectedObjects, selectedObject);

			Selection.objects = selectedObjects;
		}
	}
}