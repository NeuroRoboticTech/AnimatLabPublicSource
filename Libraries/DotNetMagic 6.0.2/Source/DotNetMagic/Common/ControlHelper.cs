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
using System.Windows.Forms;

namespace Crownwood.DotNetMagic.Common
{
	/// <summary>
	/// Collection of static methods used to remove child controls and forms.
	/// </summary>
	/// <remarks>
	/// At the time of writing this class there exists a bug that is exposed when
	/// you remove a window that has the focus. In this case window is removed but
	/// the main application window will no longer close. To get around this a
	/// temporary window is created and takes the focus whilst the required window
	/// is actually removed.
	/// </remarks>
    public sealed class ControlHelper
	{
		// Prevent instance from being created.
		private ControlHelper() {}
	
		/// <summary>
		/// Remove all child controls from provided control.
		/// </summary>
		/// <param name="control">Parent control from which to remove all children.</param>
		public static void RemoveAll(Control control)
		{
			if ((control != null) && (control.Controls.Count > 0))
			{
  				// Remove all entries from target
				control.Controls.Clear();
			}
		}

		/// <summary>
		/// Remove specified child item from the provided control collection.
		/// </summary>
		/// <param name="coll">Control collection that child item needs to be removed from.</param>
		/// <param name="item">Child control to remove from collection.</param>
		public static void Remove(Control.ControlCollection coll, Control item)
		{
			if ((coll != null) && (item != null))
			{
				// Remove our target control
				coll.Remove(item);
			}
		}

		/// <summary>
		/// Remove indexed child item from the provided control collection.
		/// </summary>
		/// <param name="coll">Control collection that child item needs to be removed from.</param>
		/// <param name="index">Index into collection that needs removing.</param>
		public static void RemoveAt(Control.ControlCollection coll, int index)
		{
			if (coll != null)
			{
				if ((index >= 0) && (index < coll.Count))
					Remove(coll, coll[index]);
			}
		}
    
		/// <summary>
		/// Remove specified form as a child.
		/// </summary>
		/// <param name="source">Source control from same application as form.</param>
		/// <param name="form">Form to be removed as child.</param>
        public static void RemoveForm(Control source, Form form)
        {
            // Remove Form parent
            form.Parent = null;
        }

        /// <summary>
        /// Gets the new location that is allowed given the requested location and size.
        /// </summary>
        /// <param name="pt">Point that might be form a different display setup.</param>
        /// <param name="size">Size of target window.</param>
        /// <returns>Resolved window location.</returns>
        public static Point ScreenLocation(Point pt, Size size)
        {
            // Get the screen nearest to the point
            Screen target = Screen.FromPoint(pt);

            // Find the right and bottom positions of the request
            int right = pt.X + size.Width;
            int bottom = pt.Y + size.Height;

            // Try and ensure all the window is going to be visible
            if (right > target.WorkingArea.Right)   pt.X = target.WorkingArea.Right - size.Width;
            if (bottom > target.WorkingArea.Bottom) pt.Y = target.WorkingArea.Bottom - size.Height;
            if (pt.X < target.WorkingArea.Left)     pt.X = target.WorkingArea.Left;
            if (pt.Y < target.WorkingArea.Top)      pt.Y = target.WorkingArea.Top;

            // Return the updated point
            return pt;
        }
    }
}
