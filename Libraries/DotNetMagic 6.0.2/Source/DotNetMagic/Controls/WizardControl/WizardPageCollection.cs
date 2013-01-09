// *****************************************************************************
// 
//  (c) Crownwood Software Ltd 2004-2006. All rights reserved. 
//	The software and associated documentation supplied hereunder are the 
//	proprietary information of Crownwood Software Ltd, Bracknell, 
//	Berkshire, England and are supplied subject to licence terms.
// 
//  Version 6.0.1.0 	www.crownwood.net
// *****************************************************************************

using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Provides a collection container for WizardPage instances.
	/// </summary>
    public class WizardPageCollection : CollectionWithEvents
    {
		/// <summary>
		/// Adds new WizardPage constructed from TabPage details.
		/// </summary>
		/// <param name="value">TabPage used as source of new details.</param>
		/// <returns></returns>
        public WizardPage Add(TabPage value)
        {
            // Create a WizardPage from the TabPage
            WizardPage wp = new WizardPage();
            wp.Title = value.Title;
            wp.Control = value.Control;
            wp.Image = value.Image;
            wp.ImageIndex = value.ImageIndex;
            wp.ImageList = value.ImageList;
            wp.Icon = value.Icon;
            wp.Selected = value.Selected;
            wp.StartFocus = value.StartFocus;

            return Add(wp);           
        }
    
		/// <summary>
		/// Adds the specified WizardPage object to the collection.
		/// </summary>
		/// <param name="value">The WizardPage object to add to the collection.</param>
		/// <returns>The WizardPage object added to the collection.</returns>
        public WizardPage Add(WizardPage value)
        {
            // Use base class to process actual collection operation
            base.List.Add(value as object);

            return value;
        }

		/// <summary>
		/// Adds an array of WizardPage objects to the collection.
		/// </summary>
		/// <param name="values">An array of WizardPage objects to add to the collection.</param>
        public void AddRange(WizardPage[] values)
        {
            // Use existing method to add each array entry
            foreach(WizardPage page in values)
                Add(page);
        }

		/// <summary>
		/// Removes a WizardPage from the collection.
		/// </summary>
		/// <param name="value">A WizardPage to remove from the collection.</param>
        public void Remove(WizardPage value)
        {
            // Use base class to process actual collection operation
            base.List.Remove(value as object);
        }

		/// <summary>
		/// Inserts a WizardPage instance into the collection at the specified location.
		/// </summary>
		/// <param name="index">The location in the collection where you want to add the WizardPage.</param>
		/// <param name="value">The WizardPage object to insert.</param>
        public void Insert(int index, WizardPage value)
        {
            // Use base class to process actual collection operation
            base.List.Insert(index, value as object);
        }

		/// <summary>
		/// Determines whether a WizardPage is in the collection.
		/// </summary>
		/// <param name="value">The WizardPage to locate in the collection.</param>
		/// <returns>true if item is found in the collection; otherwise, false.</returns>
        public bool Contains(WizardPage value)
        {
            // Use base class to process actual collection operation
            return base.List.Contains(value as object);
        }

		/// <summary>
		/// Gets the WizardPage at the specified index.
		/// </summary>
        public WizardPage this[int index]
        {
            // Use base class to process actual collection operation
            get { return (base.List[index] as WizardPage); }
        }

		/// <summary>
		/// Gets the WizardPage with the specified title.
		/// </summary>
        public WizardPage this[string title]
        {
            get 
            {
                // Search for a Page with a matching title
                foreach(WizardPage page in base.List)
                    if (page.Title == title)
                        return page;

                return null;
            }
        }

		/// <summary>
		/// Returns the index of the first occurrence of the given WizardPage.
		/// </summary>
		/// <param name="value">The WizardPage to locate.</param>
		/// <returns>Index of object; otherwise -1</returns>
        public int IndexOf(WizardPage value)
        {
            // Find the 0 based index of the requested entry
            return base.List.IndexOf(value);
        }
    }
}
