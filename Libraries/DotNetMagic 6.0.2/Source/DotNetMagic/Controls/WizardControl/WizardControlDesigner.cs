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
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Crownwood.DotNetMagic.Common;
using Crownwood.DotNetMagic.Win32;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Designer used in conjunction with WizardControl class.
	/// </summary>
	public class WizardControlDesigner :  System.Windows.Forms.Design.ParentControlDesigner
    {
		// Instance fields
        private ISelectionService _selectionService = null;

		/// <summary>
		/// Return collection of WizardPages which are child components.
		/// </summary>
        public override ICollection AssociatedComponents
        {
            get 
            {
                if (base.Control is Crownwood.DotNetMagic.Controls.WizardControl)
                    return ((Crownwood.DotNetMagic.Controls.WizardControl)base.Control).WizardPages;
                else
                    return base.AssociatedComponents;
            }
        }

		/// <summary>
		/// Return a selection service reference.
		/// </summary>
		public ISelectionService SelectionService
        {
            get
            {
                // Is this the first time the accessor has been called?
                if (_selectionService == null)
                {
                    // Then grab and cache the required interface
                    _selectionService = (ISelectionService)GetService(typeof(ISelectionService));
                }

                return _selectionService;
            }
        }

		/// <summary>
		/// Gets a value indicating if the grid should be drawn.
		/// </summary>
		protected override bool DrawGrid
		{
			get { return false; }
		}

		/// <summary>
		/// Processes Windows messages.
		/// </summary>
		/// <param name="msg">The Windows Message to process.</param>
		protected override void WndProc(ref Message msg)
        {
            // Test for the left mouse down windows message
            if (msg.Msg == (int)Win32.Msgs.WM_LBUTTONDOWN)
            {
                Crownwood.DotNetMagic.Controls.WizardControl wizardControl = this.SelectionService.PrimarySelection as Crownwood.DotNetMagic.Controls.WizardControl;

                // Check we have a valid object reference
                if (wizardControl != null)
                {
                    Crownwood.DotNetMagic.Controls.TabControl tabControl = wizardControl.TabControl;

                    // Check we have a valid object reference
                    if (tabControl != null)
                    {
                        // Extract the mouse position
                        int xPos = (short)((uint)msg.LParam & 0x0000FFFFU);
                        int yPos = (short)(((uint)msg.LParam & 0xFFFF0000U) >> 16);

                        Point screenCoord = wizardControl.PointToScreen(new Point(xPos, yPos));
                        Point clientCoord = tabControl.PointToClient(screenCoord);

                        // Ask the TabControl to change tabs according to mouse message
                        tabControl.ExternalMouseTest(msg.HWnd, clientCoord);
                    }
                }
            }
            else
            {
                if (msg.Msg == (int)Win32.Msgs.WM_LBUTTONDBLCLK)
                {
                    Crownwood.DotNetMagic.Controls.WizardControl wizardControl = this.SelectionService.PrimarySelection as Crownwood.DotNetMagic.Controls.WizardControl;
                    
                    // Check we have a valid object reference
                    if (wizardControl != null)
                    {
                        Crownwood.DotNetMagic.Controls.TabControl tabControl = wizardControl.TabControl;

                        // Check we have a valid object reference
                        if (tabControl != null)
                        {
                            // Extract the mouse position
                            int xPos = (short)((uint)msg.LParam & 0x0000FFFFU);
                            int yPos = (short)(((uint)msg.LParam & 0xFFFF0000U) >> 16);

                            Point screenCoord = wizardControl.PointToScreen(new Point(xPos, yPos));
                            Point clientCoord = tabControl.PointToClient(screenCoord);

                            // Ask the TabControl to process a double click over an arrow as a simple
                            // click of the arrow button. In which case we return immediately to prevent
                            // the base class from using the double to generate the default event
                            if (tabControl.WantDoubleClick(msg.HWnd, clientCoord))
                                return;
                        }
                    }
                }
            }

            base.WndProc(ref msg);
        }
    }
}
