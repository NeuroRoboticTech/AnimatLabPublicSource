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
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Docking
{
	/// <summary>
	/// Provides a collection container for WindowDetail instances.
	/// </summary>
    public class WindowDetailCollection : CollectionWithEvents
    {
		/// <summary>
		/// Adds the specified WindowDetail object to the collection.
		/// </summary>
		/// <param name="value">The WindowDetail object to add to the collection.</param>
		/// <returns>The WindowDetail object added to the collection.</returns>
        public WindowDetail Add(WindowDetail value)
        {
            // Use base class to process actual collection operation
            base.List.Add(value as object);

            return value;
        }

		/// <summary>
		/// Adds an array of WindowDetail objects to the collection.
		/// </summary>
		/// <param name="values">An array of WindowDetail objects to add to the collection.</param>
        public void AddRange(WindowDetail[] values)
        {
            // Use existing method to add each array entry
            foreach(WindowDetail page in values)
                Add(page);
        }

		/// <summary>
		/// Removes a WindowDetail from the collection.
		/// </summary>
		/// <param name="value">A WindowDetail to remove from the collection.</param>
        public void Remove(WindowDetail value)
        {
            // Use base class to process actual collection operation
            base.List.Remove(value as object);
        }

		/// <summary>
		/// Inserts a WindowDetail instance into the collection at the specified location.
		/// </summary>
		/// <param name="index">The location in the collection where you want to add the WindowDetail.</param>
		/// <param name="value">The WindowDetail object to insert.</param>
        public void Insert(int index, WindowDetail value)
        {
            // Use base class to process actual collection operation
            base.List.Insert(index, value as object);
        }

		/// <summary>
		/// Determines whether a WindowDetail is in the collection.
		/// </summary>
		/// <param name="value">The WindowDetail to locate in the collection.</param>
		/// <returns>true if item is found in the collection; otherwise, false.</returns>
        public bool Contains(WindowDetail value)
        {
            // Use base class to process actual collection operation
            return base.List.Contains(value as object);
        }

		/// <summary>
		/// Gets the WindowDetail at the specified index.
		/// </summary>
        public WindowDetail this[int index]
        {
            // Use base class to process actual collection operation
            get 
			{ 
				// Limit check incoming index
				if ((index < 0) || (index >= base.List.Count))
					return null;		

				return (base.List[index] as WindowDetail); 
			}
        }

		/// <summary>
		/// Returns the index of the first occurrence of the given WindowDetail.
		/// </summary>
		/// <param name="value">The WindowDetail to locate.</param>
		/// <returns>Index of object; otherwise -1</returns>
        public int IndexOf(WindowDetail value)
        {
            // Find the 0 based index of the requested entry
            return base.List.IndexOf(value);
        }
	}
}
