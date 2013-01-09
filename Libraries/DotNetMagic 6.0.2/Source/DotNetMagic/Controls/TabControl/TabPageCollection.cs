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

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Provides a collection container for TabPage instances.
	/// </summary>
    public class TabPageCollection : CollectionWithEvents
    {
		/// <summary>
		/// Adds the specified TabPage object to the collection.
		/// </summary>
		/// <param name="value">The TabPage object to add to the collection.</param>
		/// <returns>The TabPage object added to the collection.</returns>
        public TabPage Add(TabPage value)
        {
            // Use base class to process actual collection operation
            base.List.Add(value as object);

            return value;
        }

		/// <summary>
		/// Adds an array of TabPage objects to the collection.
		/// </summary>
		/// <param name="values">An array of TabPage objects to add to the collection.</param>
        public void AddRange(TabPage[] values)
        {
            // Use existing method to add each array entry
            foreach(TabPage page in values)
                Add(page);
        }

		/// <summary>
		/// Removes a TabPage from the collection.
		/// </summary>
		/// <param name="value">A TabPage to remove from the collection.</param>
        public void Remove(TabPage value)
        {
			// Use base class to process actual collection operation
            base.List.Remove(value as object);
        }

		/// <summary>
		/// Inserts a TabPage instance into the collection at the specified location.
		/// </summary>
		/// <param name="index">The location in the collection where you want to add the TabPage.</param>
		/// <param name="value">The TabPage object to insert.</param>
        public void Insert(int index, TabPage value)
        {
            // Use base class to process actual collection operation
            base.List.Insert(index, value as object);
        }

		/// <summary>
		/// Determines whether a TabPage is in the collection.
		/// </summary>
		/// <param name="value">The TabPage to locate in the collection.</param>
		/// <returns>true if item is found in the collection; otherwise, false.</returns>
        public bool Contains(TabPage value)
        {
            // Use base class to process actual collection operation
            return base.List.Contains(value as object);
        }

		/// <summary>
		/// Gets the TabPage at the specified index.
		/// </summary>
        public TabPage this[int index]
        {
            // Use base class to process actual collection operation
            get { return (base.List[index] as TabPage); }
        }

		/// <summary>
		/// Gets the TabPage with the specified title.
		/// </summary>
        public TabPage this[string title]
        {
            get 
            {
                // Search for a Page with a matching title
                foreach(TabPage page in base.List)
                    if (page.Title == title)
                        return page;

                return null;
            }
        }

		/// <summary>
		/// Returns the index of the first occurrence of the given TabPage.
		/// </summary>
		/// <param name="value">The TabPage to locate.</param>
		/// <returns>Index of object; otherwise -1</returns>
        public int IndexOf(TabPage value)
        {
            // Find the 0 based index of the requested entry
            return base.List.IndexOf(value);
        }
    }
}
