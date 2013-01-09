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
using System.Drawing;
using System.Windows.Forms;
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Docking
{
	/// <summary>
	/// Provides a collection container for HotArea instances.
	/// </summary>
    public class HotAreaCollection : CollectionWithEvents
    {
		/// <summary>
		/// Adds the specified HotArea object to the collection.
		/// </summary>
		/// <param name="value">The HotArea object to add to the collection.</param>
		/// <returns>The HotArea object added to the collection.</returns>
        public HotArea Add(HotArea value)
        {
            // Use base class to process actual collection operation
            base.List.Add(value as object);

            return value;
        }

		/// <summary>
		/// Adds an array of HotArea objects to the collection.
		/// </summary>
		/// <param name="values">An array of HotArea objects to add to the collection.</param>
        public void AddRange(HotArea[] values)
        {
            // Use existing method to add each array entry
            foreach(HotArea area in values)
                Add(area);
        }

		/// <summary>
		/// Removes a HotArea from the collection.
		/// </summary>
		/// <param name="value">A HotArea to remove from the collection.</param>
        public void Remove(HotArea value)
        {
            // Use base class to process actual collection operation
            base.List.Remove(value as object);
        }

		/// <summary>
		/// Inserts a HotArea instance into the collection at the specified location.
		/// </summary>
		/// <param name="index">The location in the collection where you want to add the HotArea.</param>
		/// <param name="value">The HotArea object to insert.</param>
        public void Insert(int index, HotArea value)
        {
            // Use base class to process actual collection operation
            base.List.Insert(index, value as object);
        }

		/// <summary>
		/// Determines whether a HotArea is in the collection.
		/// </summary>
		/// <param name="value">The HotArea to locate in the collection.</param>
		/// <returns>true if item is found in the collection; otherwise, false.</returns>
        public bool Contains(HotArea value)
        {
            // Use base class to process actual collection operation
            return base.List.Contains(value as object);
        }

		/// <summary>
		/// Gets the HotArea at the specified index.
		/// </summary>
        public HotArea this[int index]
        {
            // Use base class to process actual collection operation
            get { return (base.List[index] as HotArea); }
        }

		/// <summary>
		/// Returns the index of the first occurrence of the given HotArea.
		/// </summary>
		/// <param name="value">The HotArea to locate.</param>
		/// <returns>Index of object; otherwise -1</returns>
        public int IndexOf(HotArea value)
        {
            // Find the 0 based index of the requested entry
            return base.List.IndexOf(value);
        }
	}
}
