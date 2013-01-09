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
	/// Provides a collection container for StatusPanel instances.
	/// </summary>
    public class StatusPanelCollection : CollectionWithEvents
    {
        private StatusBarControl _parent;

        /// <summary>
        /// Initialize a new instance of the StatusPanelCollection class.
        /// </summary>
        /// <param name="parent">Reference to parent control.</param>
        public StatusPanelCollection(StatusBarControl parent)
        {
            _parent = parent;
        }

        /// <summary>
		/// Adds the specified StatusPanel object to the collection.
		/// </summary>
		/// <param name="value">The StatusPanel object to add to the collection.</param>
		/// <returns>The StatusPanel object added to the collection.</returns>
        public StatusPanel Add(StatusPanel value)
        {
            // Hook up the parent/child relationship
            value.StatusBarControl = _parent;

            // Use base class to process actual collection operation
            base.List.Add(value as object);

            return value;
        }

		/// <summary>
		/// Adds an array of StatusPanel objects to the collection.
		/// </summary>
		/// <param name="values">An array of StatusPanel objects to add to the collection.</param>
        public void AddRange(StatusPanel[] values)
        {
            // Use existing method to add each array entry
            foreach(StatusPanel panel in values)
                Add(panel);
        }

		/// <summary>
		/// Removes a StatusPanel from the collection.
		/// </summary>
		/// <param name="value">A StatusPanel to remove from the collection.</param>
        public void Remove(StatusPanel value)
        {
            // Use base class to process actual collection operation
            base.List.Remove(value as object);
        }

		/// <summary>
		/// Inserts a StatusPanel instance into the collection at the specified location.
		/// </summary>
		/// <param name="index">The location in the collection where you want to add the StatusPanel.</param>
		/// <param name="value">The StatusPanel object to insert.</param>
        public void Insert(int index, StatusPanel value)
        {
            // Use base class to process actual collection operation
            base.List.Insert(index, value as object);
        }

		/// <summary>
		/// Determines whether a StatusPanel is in the collection.
		/// </summary>
		/// <param name="value">The StatusPanel to locate in the collection.</param>
		/// <returns>true if item is found in the collection; otherwise, false.</returns>
        public bool Contains(StatusPanel value)
        {
            // Use base class to process actual collection operation
            return base.List.Contains(value as object);
        }

		/// <summary>
		/// Gets the TabPage at the specified index.
		/// </summary>
        public StatusPanel this[int index]
        {
            // Use base class to process actual collection operation
            get { return (base.List[index] as StatusPanel); }
        }

		/// <summary>
		/// Returns the index of the first occurrence of the given StatusPanel.
		/// </summary>
		/// <param name="value">The StatusPanel to locate.</param>
		/// <returns>Index of object; otherwise -1</returns>
        public int IndexOf(StatusPanel value)
        {
            // Find the 0 based index of the requested entry
            return base.List.IndexOf(value);
        }
    }
}
