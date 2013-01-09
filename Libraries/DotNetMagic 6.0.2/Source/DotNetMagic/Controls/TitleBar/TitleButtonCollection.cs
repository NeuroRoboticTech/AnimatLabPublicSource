// *****************************************************************************
// 
//  (c) Crownwood Software Ltd 2004-2006. All rights reserved. 
//	The software and associated documentation supplied hereunder are the 
//	proprietary information of Crownwood Software Ltd, Bracknell, 
//	Berkshire, England and are supplied subject to licence terms.
// 
//  Version 2.1.0.0 	www.crownwood.net
// *****************************************************************************

using System;
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Provides a collection container for TitleButton instances.
	/// </summary>
    public class TitleButtonCollection : CollectionWithEvents
    {
		/// <summary>
		/// Adds the specified TitleButton object to the collection.
		/// </summary>
		/// <param name="value">The TitleButton object to add to the collection.</param>
		/// <returns>The TitleButton object added to the collection.</returns>
        public TitleButton Add(TitleButton value)
        {
            // Use base class to process actual collection operation
            base.List.Add(value as object);

            return value;
        }

		/// <summary>
		/// Adds an array of TitleButton objects to the collection.
		/// </summary>
		/// <param name="values">An array of TitleButton objects to add to the collection.</param>
        public void AddRange(TitleButton[] values)
        {
            // Use existing method to add each array entry
            foreach(TitleButton button in values)
                Add(button);
        }

		/// <summary>
		/// Removes a TitleButton from the collection.
		/// </summary>
		/// <param name="value">A TitleButton to remove from the collection.</param>
        public void Remove(TitleButton value)
        {
            // Use base class to process actual collection operation
            base.List.Remove(value as object);
        }

		/// <summary>
		/// Inserts a TitleButton instance into the collection at the specified location.
		/// </summary>
		/// <param name="index">The location in the collection where you want to add the TitleButton.</param>
		/// <param name="value">The TitleButton object to insert.</param>
        public void Insert(int index, TitleButton value)
        {
            // Use base class to process actual collection operation
            base.List.Insert(index, value as object);
        }

		/// <summary>
		/// Determines whether a TitleButton is in the collection.
		/// </summary>
		/// <param name="value">The TitleButton to locate in the collection.</param>
		/// <returns>true if item is found in the collection; otherwise, false.</returns>
        public bool Contains(TitleButton value)
        {
            // Use base class to process actual collection operation
            return base.List.Contains(value as object);
        }

		/// <summary>
		/// Gets the TitleButton at the specified index.
		/// </summary>
        public TitleButton this[int index]
        {
            // Use base class to process actual collection operation
            get { return (base.List[index] as TitleButton); }
        }

		/// <summary>
		/// Returns the index of the first occurrence of the given TitleButton.
		/// </summary>
		/// <param name="value">The TitleButton to locate.</param>
		/// <returns>Index of object; otherwise -1</returns>
        public int IndexOf(TitleButton value)
        {
            // Find the 0 based index of the requested entry
            return base.List.IndexOf(value);
        }
    }
}
