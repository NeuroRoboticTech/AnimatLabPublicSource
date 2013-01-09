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
	/// Provides a collection container for Window instances.
	/// </summary>
    public class WindowCollection : CollectionWithEvents
    {
		/// <summary>
		/// Adds the specified Window object to the collection.
		/// </summary>
		/// <param name="value">The Window object to add to the collection.</param>
		/// <returns>The Window object added to the collection.</returns>
        public Window Add(Window value)
        {
            // Use base class to process actual collection operation
            base.List.Add(value as object);

            return value;
        }

		/// <summary>
		/// Adds an array of Window objects to the collection.
		/// </summary>
		/// <param name="values">An array of Window objects to add to the collection.</param>
        public void AddRange(Window[] values)
        {
            // Use existing method to add each array entry
            foreach(Window page in values)
                Add(page);
        }

		/// <summary>
		/// Removes a Window from the collection.
		/// </summary>
		/// <param name="value">A Window to remove from the collection.</param>
        public void Remove(Window value)
        {
            // Use base class to process actual collection operation
            base.List.Remove(value as object);
        }

		/// <summary>
		/// Inserts a Window instance into the collection at the specified location.
		/// </summary>
		/// <param name="index">The location in the collection where you want to add the Window.</param>
		/// <param name="value">The Window object to insert.</param>
        public void Insert(int index, Window value)
        {
            // Use base class to process actual collection operation
            base.List.Insert(index, value as object);
        }

		/// <summary>
		/// Determines whether a Window is in the collection.
		/// </summary>
		/// <param name="value">The Window to locate in the collection.</param>
		/// <returns>true if item is found in the collection; otherwise, false.</returns>
        public bool Contains(Window value)
        {
            // Use base class to process actual collection operation
            return base.List.Contains(value as object);
        }

		/// <summary>
		/// Gets the Window at the specified index.
		/// </summary>
        public Window this[int index]
        {
            // Use base class to process actual collection operation
            get 
			{ 
				// Limit check incoming index
				if ((index < 0) || (index >= base.List.Count))
					return null;		

				return (base.List[index] as Window); 
			}
        }

		/// <summary>
		/// Returns the index of the first occurrence of the given Window.
		/// </summary>
		/// <param name="value">The Window to locate.</param>
		/// <returns>Index of object; otherwise -1</returns>
        public int IndexOf(Window value)
        {
            // Find the 0 based index of the requested entry
            return base.List.IndexOf(value);
        }
	}
}
