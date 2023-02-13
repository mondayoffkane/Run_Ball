// Copyright (c) 2021 Nementic Games GmbH. All Rights Reserved.
// Author: Chris Yarbrough

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Nementic.SelectionUtility.Editor")]
namespace Nementic.SelectionUtility
{
	using System.Collections.Generic;
	using UnityEngine;

	public static class SelectionPopupExtensions
	{
		/// <summary>
		/// Changes the available filters.
		/// </summary>
		/// <example>
		/// A simple modifier that adds a new filter to the
		/// list of default filters.
		/// <code>
		/// using Nementic.SelectionUtility;
		/// using UnityEditor;
		/// 
		/// public static class SelectionUtilityExtension
		/// {
		/// 	[InitializeOnLoadMethod]
		/// 	private static void RegisterCustomFilter()
		/// 	{
		/// 		SelectionPopupExtensions.FilterModifier = filters =>
		/// 		{
		/// 			filters.Add(new DataFilter("Obstacles", go => go.CompareTag("Obstacle")));
		/// 			return filters;
		/// 		};
		/// 	}
		/// }
		/// </code>
		/// </example>
		public static FilterModifier FilterModifier { get; set; }
	}

	/// <summary>
	/// A method that modifies the default filters.
	/// Must return at least one item.
	/// The provided default filters list can be modified (e.g. add, remove, replace, reorder)
	/// and used as the return value.
	/// </summary>
	/// <param name="defaultFilters">
	/// A copy of the unaltered list of default filters. Can be modified safely.
	/// </param>
	public delegate IEnumerable<DataFilter> FilterModifier(List<DataFilter> defaultFilters);

	/// <summary>
	/// Returns true if the GameObject should be shown
	/// within the associated filter toolbar tab.
	/// A filter function is set on a <see cref="DataFilter"/>
	/// instance and called automatically when the selection
	/// popup shows the associated filter tab.
	/// </summary>
	/// <example>
	/// A simple filter function that only allows GameObjects
	/// with a specific tagged.
	/// <code>
	/// private static bool HasTag(GameObject go)
	/// {
	/// 	return go.CompareTag("MyTag");
	/// }
	/// </code>
	/// </example>
	/// <example>
	/// Filter functions can be passed to the <see cref="DataSource"/>
	/// constructor with lambda syntax.
	/// <code>
	/// var filter = new DataFilter(
	/// 	"Colliders", 
	/// 	go => go.GetComponent<Collider>() != null);
	/// </code>
	/// </example>
	public delegate bool FilterFunction(GameObject go);
}
