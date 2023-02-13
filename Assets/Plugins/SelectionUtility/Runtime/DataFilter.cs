// Copyright (c) 2021 Nementic Games GmbH. All Rights Reserved.
// Author: Chris Yarbrough

namespace Nementic.SelectionUtility
{
	using System;
	using UnityEngine;

	/// <summary>
	/// An option in the filter toolbar.
	/// </summary>
	/// <example>
	/// A simple data filter that only show GameObjects
	/// that have a specific tag assigned.
	/// <code>
	/// using Nementic.SelectionUtility;
	/// using System.Collections.Generic;
	/// using UnityEditor;
	/// 
	/// public static class SelectionUtilityExtension
	/// {
	/// 	[InitializeOnLoadMethod]
	/// 	private static void RegisterCustomFilter()
	/// 	{
	/// 		SelectionPopupExtensions.FilterModifier = AddCustomFilter;
	/// 	}
	/// 
	/// 	private static IEnumerable<DataFilter> AddCustomFilter(List<DataFilter> filters)
	/// 	{
	/// 		filters.Add(new DataFilter("Obstacles", go => go.CompareTag("Obstacle")));
	/// 		return filters;
	/// 	}
	/// }
	/// </code>
	/// </example>
	public class DataFilter
	{
		/// <summary>
		/// The display name and unique identifier of the filter.
		/// </summary>
		/// <exception cref="ArgumentException">If the provided value is null or empty.</exception>
		public string ShortName
		{
			get { return shortName; }
			set
			{
				if (string.IsNullOrEmpty(value))
					throw new ArgumentException("DataFilter must have a name.");

				shortName = value;
			}
		}

		private string shortName;

		/// <summary>
		/// The filter function that is run on each GameObject
		/// in the list of all objects under the mouse.
		/// </summary>
		/// <exception cref="ArgumentException">If the provided value is null.</exception>
		public FilterFunction Filter
		{
			get { return filter; }
			set
			{
				if (value == null)
				{
					throw new ArgumentException(
						"DataFilter function cannot be null. Use a no-op implementation instead.");
				}

				filter = value;
			}
		}

		private FilterFunction filter;

		public DataFilter(string name, FilterFunction filter)
		{
			this.ShortName = name;
			this.Filter = filter;
		}

		internal bool IsAllowed(GameObject go)
		{
			return Filter.Invoke(go);
		}

		/// <summary>
		/// A filter named "All" that allows any item.
		/// </summary>
		public static readonly DataFilter PassThrough = new DataFilter("All", go => true);
	}
}
