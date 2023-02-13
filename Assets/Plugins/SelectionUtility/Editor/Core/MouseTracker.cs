// Copyright (c) 2020 Nementic Games GmbH.

namespace Nementic.SelectionUtility
{
	using UnityEngine;

	/// <summary>
	/// Notifies callers that the mouse has been moved more than a certain threshold.
	/// Used to differentiate a right-click to pan the scene view camera from
	/// a right-click to open the selection popup.
	/// </summary>
	internal class MouseTracker
	{
		private bool pendingValidClick;
		private Vector2 mouseMovementSinceDown;

		/// <summary>
		/// Returns true during a valid mouse context-click up event.
		/// Returns false when the user is trying to pan the scene view camera with the right mouse button instead.
		/// </summary>
		public bool ValidContextClick(Event current, int controlID)
		{
			// Only listen for right-click mouse events.
			if (current.isMouse == false || current.button != 1)
				return false;

			switch (current.GetTypeForControl(controlID))
			{
				case EventType.MouseDown:
					pendingValidClick = true;
					mouseMovementSinceDown = Vector2.zero;
					// Don't capture the hot control, because
					// that would disable Unity's scene view panning, 
					// but selection utility should play nicely with other tools.
					// However, without hot control we cannot overwrite the left-click.
					break;

				case EventType.MouseMove:
				case EventType.MouseDrag:
					mouseMovementSinceDown += AbsoluteMouseMovement(current);
					if (mouseMovementSinceDown.magnitude > UserPrefs.ContextClickPixelThreshold)
						pendingValidClick = false;
					break;

				case EventType.MouseUp:
					if (pendingValidClick)
					{
						pendingValidClick = false;
						return true;
					}
					else
						return false;
			}

			return false;
		}

		private Vector2 AbsoluteMouseMovement(Event current)
		{
			// Identify the total summed movement in any direction.
			// E.g. move mouse 10 px to the right and 10 px back to the left
			// should be identified as movement.
			return new Vector2(
				Mathf.Abs(current.delta.x),
				Mathf.Abs(current.delta.y));
		}
	}
}
