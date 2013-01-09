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
using System.Xml;
using System.Windows.Forms;
using System.Runtime.Serialization;
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Docking
{
	/// <summary>
	/// Provides a collection container for Content instances for use by the DockingManager.
	/// </summary>
    public class ManagerContentCollection : CollectionWithEvents
    {
        // Instance fields
        private DockingManager _manager;

		/// <summary>
		/// Initializes a new instance of the ManagerContentCollection class.
		/// </summary>
		/// <param name="manager">Reference to owning DockingManager instance.</param>
        public ManagerContentCollection(DockingManager manager)
		{
            // Must provide a valid manager instance
            if (manager == null)
                throw new ArgumentNullException("DockingManager");

            // Default the state
            _manager = manager;
        }
		
		/// <summary>
		/// Adds a new Content instance to the collection.
		/// </summary>
		/// <returns>The Content object added to the collection.</returns>
  		public Content Add()
        {
            Content c = new Content(_manager);

            // Use base class to process actual collection operation
            base.List.Add(c as object);

            return c;
        }

		/// <summary>
		/// Adds the specified Content object to the collection.
		/// </summary>
		/// <param name="c">The Content object to add to the collection.</param>
		/// <returns>The Content object added to the collection.</returns>
		public Content Add(Content c)
        {
			// Assign correct docking manager to object
			c.DockingManager = _manager;

            // Use base class to process actual collection operation
            base.List.Add(c as object);

            return c;
        }

		/// <summary>
		/// Adds a new Content instance to the collection using provided parameters.
		/// </summary>
		/// <param name="control">The Control instance to when constructing Content instance.</param>
		/// <returns>The Content object added to the collection.</returns>
        public Content Add(Control control)
        {
            Content c = new Content(_manager, control);

            // Use base class to process actual collection operation
            base.List.Add(c as object);

            return c;
        }

		/// <summary>
		/// Adds a new Content instance to the collection using provided parameters.
		/// </summary>
		/// <param name="control">The Control instance to when constructing Content instance.</param>
		/// <param name="title">The Title to use when constructing Content instance.</param>
		/// <returns>The Content object added to the collection.</returns>
        public Content Add(Control control, string title)
        {
            Content c = new Content(_manager, control, title);

            // Use base class to process actual collection operation
            base.List.Add(c as object);

            return c;
        }

		/// <summary>
		/// Adds a new Content instance to the collection using provided parameters.
		/// </summary>
		/// <param name="control">The Control instance to when constructing Content instance.</param>
		/// <param name="title">The Title to use when constructing Content instance.</param>
		/// <param name="imageList">The ImageList to use when constructing Content instance.</param>
		/// <param name="imageIndex">The ImageIndex to use when constructing Content instance.</param>
		/// <returns>The Content object added to the collection.</returns>
        public Content Add(Control control, string title, ImageList imageList, int imageIndex)
        {
            Content c = new Content(_manager, control, title, imageList, imageIndex);

            // Use base class to process actual collection operation
            base.List.Add(c as object);

            return c;
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
