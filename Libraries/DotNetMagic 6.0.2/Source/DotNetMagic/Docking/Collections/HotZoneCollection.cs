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
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Docking
{
	/// <summary>
	/// Provides a collection container for HotZone instances.
	/// </summary>
    public class HotZoneCollection : CollectionWithEvents
    {
		/// <summary>
		/// Adds the specified HotZone object to the collection.
		/// </summary>
		/// <param name="value">The HotZone object to add to the collection.</param>
		/// <returns>The HotZone object added to the collection.</returns>
        public HotZone Add(HotZone value)
        {
            // Use base class to process actual collection operation
            base.List.Add(value as object);

            return value;
        }

		/// <summary>
		/// Adds an array of HotZone objects to the collection.
		/// </summary>
		/// <param name="values">An array of HotZone objects to add to the collection.</param>
		public void AddRange(HotZone[] values)
        {
            // Use existing method to add each array entry
            foreach(HotZone page in values)
                Add(page);
        }

		/// <summary>
		/// Removes a HotZone from the collection.
		/// </summary>
		/// <param name="value">A HotZone to remove from the collection.</param>
		public void Remove(HotZone value)
        {
            // Use base class to process actual collection operation
            base.List.Remove(value as object);
        }

		/// <summary>
		/// Inserts a HotZone instance into the collection at the specified location.
		/// </summary>
		/// <param name="index">The location in the collection where you want to add the HotZone.</param>
		/// <param name="value">The HotZone object to insert.</param>
		public void Insert(int index, HotZone value)
        {
            // Use base class to process actual collection operation
            base.List.Insert(index, value as object);
        }

		/// <summary>
		/// Determines whether a HotZone is in the collection.
		/// </summary>
		/// <param name="value">The HotZone to locate in the collection.</param>
		/// <returns>true if item is found in the collection; otherwise, false.</returns>
		public bool Contains(HotZone value)
        {
            // Use base class to process actual collection operation
            return base.List.Contains(value as object);
        }

		/// <summary>
		/// Determines whether a HotZone intersects the given Point.
		/// </summary>
		/// <param name="pt">The Point to test against each HotZone.</param>
		/// <returns>HotZone if match is found; otherwise null</returns>
		public HotZone Contains(Point pt)
		{
			foreach(HotZone hz in base.List)
			{
				if (hz.HotArea.Contains(pt))
					return hz;
			}

			return null;
		}

		/// <summary>
		/// Gets the HotZone at the specified index.
		/// </summary>
        public HotZone this[int index]
        {
            // Use base class to process actual collection operation
            get 
			{ 
				// Limit check incoming index
				if ((index < 0) || (index >= base.List.Count))
					return null;		

				return (base.List[index] as HotZone); 
			}
        }

		/// <summary>
		/// Returns the index of the first occurrence of the given HotZone.
		/// </summary>
		/// <param name="value">The HotZone to locate.</param>
		/// <returns>Index of object; otherwise -1</returns>
		public int IndexOf(HotZone value)
        {
            // Find the 0 based index of the requested entry
            return base.List.IndexOf(value);
        }
	}
}
