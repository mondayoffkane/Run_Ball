// Copyright (c) 2020 Nementic Games GmbH.

namespace Nementic.SelectionUtility
{
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	/// <summary>
	/// A popup which displays a list of selectable GameObjects.
	/// </summary>
	internal sealed class SelectionPopup : PopupWindowContent
	{
		private readonly DataSource dataSource;
		private readonly IconCache iconCache;
		private readonly OutlineManager outlineManager;

		private float buttonAndIconsWidth;
		private float buttonWidth;
		private float iconWidth;

		private bool styleNeedsUpdate;
		private Styles styles;
		private Vector2 scroll;
		private Rect contentRect;
		private GameObject hoverTarget;

		public SelectionPopup(IList<GameObject> options)
		{
			this.dataSource = new DataSource(options);
			this.iconCache = new IconCache();
			this.outlineManager = new OutlineManager();
		}

		public override void OnOpen()
		{
			base.OnOpen();
			editorWindow.wantsMouseMove = true;
			styles = new Styles();
			// Queue a rebuild of the styles during 
			// the next OnGUI to handle skin changes.
			styleNeedsUpdate = true;
			CalculateRequiredWidth();
			dataSource.FocusSearch();

			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		}

		private void OnPlayModeStateChanged(PlayModeStateChange mode)
		{
			if (mode == PlayModeStateChange.ExitingPlayMode)
				ClosePopup(throwExitGUI: false);
		}

		public override void OnClose()
		{
			if (outlineManager != null)
				outlineManager.Clear();

			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
		}

		private void CalculateRequiredWidth()
		{
			buttonWidth = 0;

			IList<GameObject> items = dataSource.items;

			for (int i = 0; i < items.Count; i++)
			{
				if (items[i] == null)
					continue;

				var style = styles.LabelStyle(items[i]);
				float width = items[i] != null ? style.CalcSize(new GUIContent(items[i].name)).x : 0f;

				// If a GameObject name is excessively long, clip it.
				const int maxWidth = 250;
				if (width > maxWidth)
					width = maxWidth;

				if (width > this.buttonWidth)
					this.buttonWidth = width;
			}

			// After button, add small space.
			this.buttonWidth += EditorGUIUtility.standardVerticalSpacing;

			iconWidth = 0;

			for (int i = 0; i < items.Count; i++)
			{
				if (items[i] == null)
					continue;

				int iconCount = iconCache.CacheIcons(items[i]);

				float iconWidthTmp = (18 * iconCount);

				if (iconWidthTmp > this.iconWidth)
					this.iconWidth = iconWidthTmp;
			}

			this.buttonAndIconsWidth =
				this.buttonWidth + this.iconWidth + EditorGUIUtility.standardVerticalSpacing;

			// Ensure the window is wide enough to contain the filter toolbar.
			this.buttonAndIconsWidth = Mathf.Max(buttonAndIconsWidth, DataSource.MinimumWidth());
		}

		public override void OnGUI(Rect rect)
		{
			UpdateStyle();

			Event current = Event.current;

			HandleKeyboardEvents(current);

			bool repaintEvent = current.type == EventType.Repaint;

			if (repaintEvent && DataSource.CountValidItems(dataSource.items) == 0)
			{
				// If all GameObjects have been destroyed while the popup was open,
				// e.g. during scene change, the popup can be closed.
				ClosePopup();
				return;
			}

			// Account for the 1px gray border at the top of the window.
			rect.yMin += 1;

			rect = dataSource.DrawFilterModes(rect, RowHeight);
			rect = dataSource.SearchFieldGUI(rect, RowHeight);

			scroll = GUI.BeginScrollView(
				rect, scroll, contentRect, GUIStyle.none, GUI.skin.verticalScrollbar);

			DrawScrollViewContent(rect, current, repaintEvent);

			GUI.EndScrollView();

			if (current.type == EventType.MouseMove)
				editorWindow.Repaint();
		}

		private void UpdateStyle()
		{
			if (styleNeedsUpdate || styles == null)
			{
				// Especially the prefab label style must be recreated
				// on the next OnGUI call after a skin change.
				styles = new Styles();
				styleNeedsUpdate = false;
			}

			styles.Update();
		}

		private void HandleKeyboardEvents(Event current)
		{
			if (CloseKeyPressed(current))
				ClosePopup();
		}

		private void ClosePopup(bool throwExitGUI = true)
		{
			if (editorWindow)
				editorWindow.Close();

			if (throwExitGUI)
				GUIUtility.ExitGUI();
		}

		private void DrawScrollViewContent(Rect rect, Event current, bool repaintEvent)
		{
			rect.height = RowHeight;
			rect.xMin += 2;
			rect.xMax -= 2;

			IList<GameObject> items = dataSource.filteredItems;
			int count = DataSource.CountValidItems(dataSource.filteredItems);

			hoverTarget = null;

			using (new EditorGUIUtility.IconSizeScope(styles.iconSize))
			{
				for (int i = 0; i < count; i++)
				{
					if (items[i] == null)
						continue;

					DrawRow(rect, current, items[i]);

					if (i < count && repaintEvent)
						DrawSplitter(rect);

					rect.y += RowHeight;
				}
			}

			// Check for the currently hovered item in DrawRow,
			// then clear if nothing was hovered this frame.
			if (hoverTarget != null)
				outlineManager.SetOutlineTarget(hoverTarget);
			else
				outlineManager.Clear();
		}

		private static bool CloseKeyPressed(Event current)
		{
			return current.type == EventType.KeyDown &&
			       current.keyCode == KeyCode.Escape;
		}

		private void DrawSplitter(Rect rect)
		{
			rect.height = 1;
			rect.y -= 1;
			rect.xMin = 0;
			rect.width += 4;
			EditorGUI.DrawRect(rect, styles.splitterColor);
		}

		private void DrawRow(Rect rect, Event current, GameObject item)
		{
			if (IsMouseHover(rect, current))
			{
				Rect background = rect;
				background.xMin -= 1;
				background.xMax += 1;
				EditorGUI.DrawRect(background, styles.rowHoverColor);

				hoverTarget = item;
			}

			Rect originalRect = rect;
			Texture2D icon = AssetPreview.GetMiniThumbnail(item);
			Rect iconRect = rect;
			iconRect.width = 20;

			EditorGUI.LabelField(iconRect, styles.TempContent(null, icon));

			rect.x = iconRect.xMax;
			rect.width = buttonWidth;

			GUIContent nameContent = styles.TempContent(item != null ? item.name : "Null", null);
			EditorGUI.LabelField(rect, nameContent, styles.LabelStyle(item));

			if (ObjectSelector.TrySelectObject(current, originalRect, item))
				ClosePopup();

			if (item == null)
				return;

			DrawIcons(rect, item);
		}

		private static bool IsMouseHover(Rect rect, Event current)
		{
			return rect.Contains(current.mousePosition) &&
			       current.type != EventType.MouseDrag;
		}

		private void DrawIcons(Rect rect, GameObject item)
		{
			Rect componentIconRect = rect;
			componentIconRect.x = rect.xMax;
			componentIconRect.width = rect.height;

			Texture2D[] icons = iconCache.ForGameObject(item);
			for (int i = 0; i < icons.Length; i++)
			{
				componentIconRect.width = 16;
				GUI.DrawTexture(componentIconRect, icons[i], ScaleMode.ScaleToFit, true);
				componentIconRect.x = componentIconRect.xMax + 2;
			}
		}

		private float RowHeight
		{
			get { return 20; }
		}

		public override Vector2 GetWindowSize()
		{
			float totalHeight = 0;

			if (UserPrefs.ShowSearchField)
				totalHeight += RowHeight + 2;

			// Filter mode row.
			if (UserPrefs.ShowFilterToolbar)
				totalHeight += RowHeight + 2;

			int itemCount = DataSource.CountValidItems(dataSource.filteredItems);
			totalHeight += RowHeight * itemCount;

			float iconBeforeLabelWidth = 22;
			Vector2 windowSize = new Vector2(iconBeforeLabelWidth + buttonAndIconsWidth, totalHeight);

			// Content refers to all item rows without the search field and is used by the scroll view.
			float yStartPosition = UserPrefs.ShowSearchField ? RowHeight + 2 : 0;

			// Filter mode row content size.
			if (UserPrefs.ShowFilterToolbar)
				yStartPosition += RowHeight + 2;

			Vector2 contentSize = new Vector2(windowSize.x, windowSize.y - yStartPosition);

			// Account for the offset by the search field and the border line at the top of the window.
			float contentYOffset = (UserPrefs.ShowSearchField ? RowHeight + 1 : 0) + 1;

			// Filter mode row offset.
			if (UserPrefs.ShowFilterToolbar)
				contentYOffset += RowHeight + 1;

			this.contentRect = new Rect(new Vector2(0, contentYOffset), contentSize);

			// The popup window has a 1px gray border that covers the top and bottom.
			// Make enough room for the content to fit perfectly within.
			totalHeight += 2;
			windowSize.y = totalHeight;

			int maxHeight = Mathf.Min(Screen.currentResolution.height, 700);

			if (totalHeight > maxHeight)
			{
				// Window is clamped and must show scroll bars for its content.
				windowSize.y = maxHeight;

				// Extra size to fit vertical scroll bar without clipping icons.
				windowSize.x += 14;
			}

			return windowSize;
		}
	}
}
