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
	/// Provides a collection container for Zone instances.
	/// </summary>
    public class ZoneCollection : CollectionWithEvents
    {
		/// <summary>
		/// Adds the specified Zone object to the collection.
		/// </summary>
		/// <param name="value">The Zone object to add to the collection.</param>
		/// <returns>The Zone object added to the collection.</returns>
        public Zone Add(Zone value)
        {
            // Use base class to process actual collection operation
            base.List.Add(value as object);

            return value;
        }

		/// <summary>
		/// Adds an array of Zone objects to the collection.
		/// </summary>
		/// <param name="values">An array of Zone objects to add to the collection.</param>
        public void AddRange(Zone[] values)
        {
            // Use existing method to add each array entry
            foreach(Zone page in values)
                Add(page);
        }

		/// <summary>
		/// Removes a Zone from the collection.
		/// </summary>
		/// <param name="value">A Zone to remove from the collection.</param>
        public void Remove(Zone value)
        {
            // Use base class to process actual collection operation
            base.List.Remove(value as object);
        }

		/// <summary>
		/// Inserts a Zone instance into the collection at the specified location.
		/// </summary>
		/// <param name="index">The location in the collection where you want to add the Zone.</param>
		/// <param name="value">The Zone object to insert.</param>
        public void Insert(int index, Zone value)
        {
            // Use base class to process actual collection operation
            base.List.Insert(index, value as object);
        }

		/// <summary>
		/// Determines whether a Zone is in the collection.
		/// </summary>
		/// <param name="value">The Zone to locate in the collection.</param>
		/// <returns>true if item is found in the collection; otherwise, false.</returns>
        public bool Contains(Zone value)
        {
            // Use base class to process actual collection operation
            return base.List.Contains(value as object);
        }

		/// <summary>
		/// Gets the Zone at the specified index.
		/// </summary>
        public Zone this[int index]
        {
            // Use base class to process actual collection operation
            get 
			{ 
				// Limit check incoming index
				if ((index < 0) || (index >= base.List.Count))
					return null;		

				return (base.List[index] as Zone); 
			}
        }

		/// <summary>
		/// Returns the index of the first occurrence of the given Zone.
		/// </summary>
		/// <param name="value">The Zone to locate.</param>
		/// <returns>Index of object; otherwise -1</returns>
        public int IndexOf(Zone value)
        {
            // Find the 0 based index of the requested entry
            return base.List.IndexOf(value);
        }
    }
}
