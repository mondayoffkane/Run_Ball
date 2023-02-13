// Copyright (c) 2020 Nementic Games GmbH.

namespace Nementic.SelectionUtility
{
	using UnityEditor;
	using UnityEngine;

	/// <summary>
	/// Defines <see cref="GUIStyle"/> and <see cref="GUIContent"/>
	/// instances for use in the <see cref="SelectionPopup"/>;
	/// </summary>
	internal class Styles
	{
		private bool isProSkin;

		public Styles()
		{
			prefabLabel = new GUIStyle("PR PrefabLabel");
			prefabLabel.alignment = TextAnchor.MiddleLeft;
			RectOffset prefabLabelPadding = prefabLabel.padding;
			prefabLabelPadding.top += 2;
			prefabLabel.padding = prefabLabelPadding;

			label = new GUIStyle(EditorStyles.label);
			label.alignment = TextAnchor.MiddleLeft;
			RectOffset padding = label.padding;
			padding.top -= 1;
			padding.left -= 1;
			label.padding = padding;
		}

		public void Update()
		{
			isProSkin = EditorGUIUtility.isProSkin;
		}

		public GUIStyle LabelStyle(GameObject target)
		{
			if (IsPrefab(target))
				return prefabLabel;
			else
				return label;
		}

		private static bool IsPrefab(GameObject target)
		{
#if UNITY_2018_3_OR_NEWER
			return PrefabUtility.IsPartOfAnyPrefab(target);
#else
			return PrefabUtility.GetPrefabType(target) != PrefabType.None;
#endif
		}

		private GUIStyle label;

		private GUIStyle prefabLabel;

		public Vector2 iconSize = new Vector2(16, 16);

		private static readonly Color splitterDark = new Color(0.12f, 0.12f, 0.12f, 1.333f);
		private static readonly Color splitterLight = new Color(0.6f, 0.6f, 0.6f, 1.333f);

		public Color splitterColor { get { return isProSkin ? splitterDark : splitterLight; } }

		private static readonly Color hoverDark = new Color(0.1f, 0.1f, 0.1f, 0.4f);
		private static readonly Color hoverLight = new Color(0.5f, 0.5f, 0.5f, 0.4f);

		public Color rowHoverColor { get { return isProSkin ? hoverDark : hoverLight; } }

		private GUIContent tempContent = new GUIContent();

		public GUIContent TempContent(string text, Texture2D image)
		{
			tempContent.text = text;
			tempContent.image = image;
			return tempContent;
		}
	}
}
