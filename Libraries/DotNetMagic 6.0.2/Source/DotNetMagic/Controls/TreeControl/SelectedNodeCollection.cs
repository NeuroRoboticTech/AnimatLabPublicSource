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
using System.Collections;
using System.ComponentModel;
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Manage a collection of selected Node instances.
	/// </summary>
	public class SelectedNodeCollection : CollectionWithEvents
    {
		/// <summary>
		/// Initialize a new instance of the SelectedNodeCollection class.
		/// </summary>
		internal SelectedNodeCollection(Hashtable selected)
		{
			// Add each entry from the hash table into the list
			foreach(Node n in selected.Keys)
				base.List.Add(n);
		}

		/// <summary>
		/// Determines whether a Node is in the collection.
		/// </summary>
		/// <param name="value">The Node to locate in the collection.</param>
		/// <returns>true if item is found in the collection; otherwise, false.</returns>
        public bool Contains(Node value)
        {
            // Use base class to process actual collection operation
            return base.List.Contains(value as object);
        }

		/// <summary>
		/// Gets the Node at the specified index.
		/// </summary>
        public Node this[int index]
        {
            // Use base class to process actual collection operation
            get { return (base.List[index] as Node); }
        }

		/// <summary>
		/// Returns the index of the first occurrence of the given Node.
		/// </summary>
		/// <param name="value">The Node to locate.</param>
		/// <returns>Index of object; otherwise -1</returns>
        public int IndexOf(Node value)
        {
            // Find the 0 based index of the requested entry
            return base.List.IndexOf(value);
        }
	}
}
