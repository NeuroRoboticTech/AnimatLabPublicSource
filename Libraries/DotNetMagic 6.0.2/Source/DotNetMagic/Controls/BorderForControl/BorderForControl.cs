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
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Crownwood.DotNetMagic.Docking
{
	/// <summary>
	/// Represents a border around a contained control.
	/// </summary>
    [ToolboxItem(false)]
    public class BorderForControl : UserControl 
    {
        // Instance fields
        private int _borderWidth = 2;

		/// <summary>
		/// Initializes a new instance of the BorderForControl class.
		/// </summary>
        public BorderForControl()
        {
            InternalConstruct(null, _borderWidth);
        }

		/// <summary>
		/// Initializes a new instance of the BorderForControl class.
		/// </summary>
		/// <param name="control">Control that will be contained.</param>
        public BorderForControl(Control control)
        {
            InternalConstruct(control, _borderWidth);
        }

		/// <summary>
		/// Initializes a new instance of the BorderForControl class.
		/// </summary>
		/// <param name="control">Control that will be contained.</param>
		/// <param name="borderWidth">Width of border around contained control.</param>
        public BorderForControl(Control control, int borderWidth)
        {
            InternalConstruct(control, borderWidth);
        }

        private void InternalConstruct(Control control, int borderWidth)
        {
            // Remember parameter
            _borderWidth = borderWidth;
			
            if (control != null)
            {
                // Remove any existing docking style for passed in Control
                control.Dock = DockStyle.None;

                // Add to appearance
                Controls.Add(control);
            }
        }	

		/// <summary>
		/// Gets or sets the border width around the contained control.
		/// </summary>
        public int BorderWidth
        {
            get { return _borderWidth; }
            set { _borderWidth = value; }
        }

		/// <summary>
		/// Raises the Resize event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            ResizeOnlyTheChild();
            base.OnResize(e);
        }
		
		/// <summary>
		/// Raises the Layout event.
		/// </summary>
		/// <param name="e">An LayoutEventArgs that contains the event data.</param>
        protected override void OnLayout(LayoutEventArgs e)	
        {
            ResizeOnlyTheChild();
            base.OnLayout(e);
        }

        private void ResizeOnlyTheChild()
        {
            // Do we have a child control to resize? 
            if (Controls.Count >= 1)
            {
                Size ourSize = this.Size;

                // Get the first child (there should not be any others)
                Control child = this.Controls[0];

                // Define new position/size
				child.SetBounds(_borderWidth, 
								_borderWidth, 
								ourSize.Width - _borderWidth * 2, 
								ourSize.Height - _borderWidth * 2);
            }
        }	
    }
}
