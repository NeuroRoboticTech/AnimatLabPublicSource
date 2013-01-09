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
	/// Provides a collection container for Content instances.
	/// </summary>
    public class ContentCollection : CollectionWithEvents
    {
		/// <summary>
		/// Adds the specified Content object to the collection.
		/// </summary>
		/// <param name="value">The Content object to add to the collection.</param>
		/// <returns>The Content object added to the collection.</returns>
        public Content Add(Content value)
        {
            // Use base class to process actual collection operation
            base.List.Add(value as object);

            return value;
        }

		/// <summary>
		/// Adds an array of Content objects to the collection.
		/// </summary>
		/// <param name="values">An array of Content objects to add to the collection.</param>
        public void AddRange(Content[] values)
        {
            // Use existing method to add each array entry
            foreach(Content page in values)
                Add(page);
        }

		/// <summary>
		/// Removes a Content from the collection.
		/// </summary>
		/// <param name="value">A Content to remove from the collection.</param>
        public void Remove(Content value)
        {
            // Use base class to process actual collection operation
            base.List.Remove(value as object);
        }

		/// <summary>
		/// Inserts a Content instance into the collection at the specified location.
		/// </summary>
		/// <param name="index">The location in the collection where you want to add the Content.</param>
		/// <param name="value">The Content object to insert.</param>
        public void Insert(int index, Content value)
        {
            // Use base class to process actual collection operation
            base.List.Insert(index, value as object);
        }

		/// <summary>
		/// Determines whether a Content is in the collection.
		/// </summary>
		/// <param name="value">The Content to locate in the collection.</param>
		/// <returns>true if item is found in the collection; otherwise, false.</returns>
        public bool Contains(Content value)
        {
            // Use base class to process actual collection operation
            return base.List.Contains(value as object);
        }

		/// <summary>
		/// Determines whether any of a group of Content is in the collection.
		/// </summary>
		/// <param name="values">The group of Content to locate in the collection.</param>
		/// <returns>true if an item is found in the collection; otherwise, false.</returns>
        public bool Contains(ContentCollection values)
        {
			foreach(Content c in values)
			{
	            // Use base class to process actual collection operation
				if (Contains(c))
					return true;
			}

			return false;
        }

		/// <summary>
		/// Determines whether a Content with a given title is in the collection.
		/// </summary>
		/// <param name="value">The title of the Content to locate in the collection.</param>
		/// <returns>true if item is found in the collection; otherwise, false.</returns>
		public bool Contains(String value)
		{
			foreach(Content c in base.List)
				if (c.Title.Equals(value))
					return true;
					
			return false;			
		}

		/// <summary>
		/// Determines whether a Content with a given title or unique name is in the collection.
		/// </summary>
		/// <param name="value">The title of the Content to locate in the collection.</param>
		/// <returns>true if an item is found in the collection; otherwise, false.</returns>
		public bool ContainsTitleOrUnique(String value)
		{
			// Seach for a Content with a matching title
			if (Contains(value))
				return true;
			else
			{		
				// Search for a Content with a matching unique name
				foreach(Content c in base.List)
					if ((c.UniqueName != null) && 
						(c.UniqueName.Length > 0) && 
						c.UniqueName.Equals(value))
						return true;
			}

			return false;
		}

		/// <summary>
		/// Determines whether a Content with any of a group of titles is in the collection.
		/// </summary>
		/// <param name="values">The group of titles to locate in the collection.</param>
		/// <returns>true if an item is found in the collection; otherwise, false.</returns>
		public bool Contains(StringCollection values)
		{
			foreach(String s in values)
				if (Contains(s))
					return true;

			return false;
		}

		/// <summary>
		/// Determines whether a Content with any of a group of titles or unique names is in the collection.
		/// </summary>
		/// <param name="values">The group of titles to locate in the collection.</param>
		/// <returns>true if an item is found in the collection; otherwise, false.</returns>
		public bool ContainsTitleOrUnique(StringCollection values)
		{
			foreach(String s in values)
			{
				// Seach for a Content with a matching title
				if (Contains(s))
					return true;
				else
				{		
					// Search for a Content with a matching unique name
					foreach(Content c in base.List)
						if ((c.UniqueName != null) && 
							(c.UniqueName.Length > 0) && 
							c.UniqueName.Equals(s))
							return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Gets the Content at the specified index.
		/// </summary>
        public Content this[int index]
        {
            // Use base class to process actual collection operation
            get 
			{ 
				// Limit check incoming index
				if ((index < 0) || (index >= base.List.Count))
					return null;		

				return (base.List[index] as Content); 
			}
        }

		/// <summary>
		/// Gets the Content with the specified title.
		/// </summary>
        public Content this[string title]
        {
            get 
            {
                // Search for a Content with a matching title
                foreach(Content c in base.List)
                    if (c.Title == title)
                        return c;

                return null;
            }
        }

		/// <summary>
		/// Gets the Content with the specified unique name.
		/// </summary>
		public Content FindUniqueName(string uniqueName)
		{
			// Search for a Content with a matching unique name
			foreach(Content c in base.List)
				if ((c.UniqueName != null) && 
					(c.UniqueName.Length > 0) && 
					c.UniqueName.Equals(uniqueName))
					return c;

			return null;
		}

		/// <summary>
		/// Move Content to new indexed position in collection.
		/// </summary>
		/// <param name="newIndex">New indexed position required.</param>
		/// <param name="value">Content instance to move to new position.</param>
		/// <returns>The new indexed position.</returns>
		public int SetIndex(int newIndex, Content value)
		{
			SuspendEvents();

			base.List.Remove(value);
			base.List.Insert(newIndex, value);

			ResumeEvents();

			return newIndex;
		}

		/// <summary>
		/// Returns the index of the first occurrence of the given Content.
		/// </summary>
		/// <param name="value">The Content to locate.</param>
		/// <returns>Index of object; otherwise -1</returns>
        public int IndexOf(Content value)
        {
            // Find the 0 based index of the requested entry
            return base.List.IndexOf(value);
        }

		/// <summary>
		/// Return a new ContentCollection with same content references.
		/// </summary>
		/// <returns>New ContentCollection instance.</returns>
		public ContentCollection Copy()
		{
			ContentCollection clone = new ContentCollection();

			// Copy each reference across
            foreach(Content c in base.List)
				clone.Add(c);

			return clone;
		}
    }
}
