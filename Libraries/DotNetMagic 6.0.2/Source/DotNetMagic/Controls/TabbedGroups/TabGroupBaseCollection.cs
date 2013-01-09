// *****************************************************************************
// 
//  (c) Crownwood Software Ltd 2004-2006. All rights reserved. 
//	The software and associated documentation supplied hereunder are the 
//	proprietary information of Crownwood Software Ltd, Bracknell, 
//	Berkshire, England and are supplied subject to licence terms.
// 
//  Version 6.0.1.0 	www.crownwood.net
// *****************************************************************************

using System;
using System.Xml;
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Provides a collection container for TabGroupBase instances.
	/// </summary>
    public class TabGroupBaseCollection : CollectionWithEvents
    {
		/// <summary>
		/// Adds the specified TabGroupBase object to the collection.
		/// </summary>
		/// <param name="value">The TabGroupBase object to add to the collection.</param>
		/// <returns>The TabGroupBase object added to the collection.</returns>
        public TabGroupBase Add(TabGroupBase value)
        {
            // Use base class to process actual collection operation
            base.List.Add(value as object);

            return value;
        }

		/// <summary>
		/// Adds an array of TabGroupBase objects to the collection.
		/// </summary>
		/// <param name="values">An array of TabGroupBase objects to add to the collection.</param>
        public void AddRange(TabGroupBase[] values)
        {
            // Use existing method to add each array entry
            foreach(TabGroupBase item in values)
                Add(item);
        }

		/// <summary>
		/// Removes a TabGroupBase from the collection.
		/// </summary>
		/// <param name="value">A TabGroupBase to remove from the collection.</param>
        public void Remove(TabGroupBase value)
        {
            // Use base class to process actual collection operation
            base.List.Remove(value as object);
        }

		/// <summary>
		/// Inserts a TabGroupBase instance into the collection at the specified location.
		/// </summary>
		/// <param name="index">The location in the collection where you want to add the TabGroupBase.</param>
		/// <param name="value">The TabGroupBase object to insert.</param>
        public void Insert(int index, TabGroupBase value)
        {
            // Use base class to process actual collection operation
            base.List.Insert(index, value as object);
        }

		/// <summary>
		/// Determines whether a TabGroupBase is in the collection.
		/// </summary>
		/// <param name="value">The TabGroupBase to locate in the collection.</param>
		/// <returns>true if item is found in the collection; otherwise, false.</returns>
		public bool Contains(TabGroupBase value)
		{
			// Use base class to process actual collection operation
			return base.List.Contains(value as object);
		}
		
		/// <summary>
		/// Determines whether any of a group of TabGroupBase is in the collection.
		/// </summary>
		/// <param name="values">The group of TabGroupBase to locate in the collection.</param>
		/// <returns>true if an item is found in the collection; otherwise, false.</returns>
		public bool Contains(TabGroupBaseCollection values)
        {
			foreach(TabGroupBase c in values)
			{
	            // Use base class to process actual collection operation
				if (Contains(c))
					return true;
			}

			return false;
        }

		/// <summary>
		/// Gets the TabGroupBase at the specified index.
		/// </summary>
        public TabGroupBase this[int index]
        {
            // Use base class to process actual collection operation
            get { return (base.List[index] as TabGroupBase); }
            set { base.List[index] = value; }
        }

		/// <summary>
		/// Returns the index of the first occurrence of the given TabGroupBase.
		/// </summary>
		/// <param name="value">The TabGroupBase to locate.</param>
		/// <returns>Index of object; otherwise -1</returns>
        public int IndexOf(TabGroupBase value)
        {
            // Find the 0 based index of the requested entry
            return base.List.IndexOf(value);
        }
	}
}
