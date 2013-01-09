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
using System.IO;
using System.Drawing;
using System.Resources;
using System.Reflection;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Represents a single StatusPanelBorder inside the StatusBarControl.
	/// </summary>
    [ToolboxItem(false)]
    public class StatusPanelBorder : Panel
    {
		// Instance fields
		private StatusBarControl _parent;
		private StatusPanel _statusPanel;

		/// <summary>
		/// Occurs when the AutoSize property changes.
		/// </summary>
		public event PaintEventHandler PaintBackground;

		/// <summary>
		/// Initializes a new instance of the StatusPanel class.
		/// </summary>
		/// <param name="parent">Back reference to recover drawing information.</param>
        public StatusPanelBorder(StatusBarControl parent)
        {
            // Prevent drawing flicker by blitting from memory in WM_PAINT
            SetStyle(ControlStyles.OptimizedDoubleBuffer | 
				     ControlStyles.AllPaintingInWmPaint |
					 ControlStyles.UserPaint, true);

			// Remember back pointer
			_parent = parent;

			// Not hosting a panel to start with
			_statusPanel = null;
		}

		/// <summary>
		/// Gets and sets the hosted StatusPanel
		/// </summary>
		public StatusPanel StatusPanel
		{
			get { return _statusPanel; }

			set
			{
				if (_statusPanel != value)
				{
					// Remove any existing control
					if (_statusPanel != null)
						Controls.Remove(_statusPanel);

					// Assign across new reference
					_statusPanel = value;

					// Add new control
					if (_statusPanel != null)
					{
						// Make sure the control is positioned exactly and not using dock style
						_statusPanel.Dock = DockStyle.None;

						Controls.Add(_statusPanel);
					}

					// Set correct position for the child control
					PositionChild();
				}
			}
		}

		/// <summary>
		/// Raises the Resize event.
		/// </summary>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected override void OnResize(EventArgs e)
		{
			// Make sure child control is positioned correctly
			PositionChild();
			Invalidate();
			base.OnResize(e);
		}

		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		/// <param name="pevent">A PaintEventArgs structure contained event data.</param>
		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			base.OnPaintBackground(pevent);

			// Raise any required events for painting			
			if (PaintBackground != null)
				PaintBackground(this, pevent);
		}

		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		/// <param name="e">A PaintEventArgs structure contained event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			// Let base class do the standard stuff first
			base.OnPaint(e);

			if (_statusPanel != null)
			{
                Color borderColor = ControlPaint.Light(BackColor);
				PanelBorder border = _statusPanel.PanelBorder;

                switch(_parent.Style)
                {
                    case VisualStyle.IDE2005:
                    case VisualStyle.Office2003:
                        borderColor = _parent.ColorDetails.MenuSeparatorColor;
                        break;
                    case VisualStyle.Office2007Blue:
                    case VisualStyle.Office2007Silver:
                    case VisualStyle.Office2007Black:
                    case VisualStyle.MediaPlayerBlue:
                    case VisualStyle.MediaPlayerOrange:
                    case VisualStyle.MediaPlayerPurple:
                        borderColor = Color.Transparent;
                        break;
                }

				// Paint border according to requested style
				switch(border)
				{
					case PanelBorder.Sunken:
                        ControlPaint.DrawBorder(e.Graphics, ClientRectangle, borderColor, ButtonBorderStyle.Inset);
						break;
					case PanelBorder.Raised:
                        ControlPaint.DrawBorder(e.Graphics, ClientRectangle, borderColor, ButtonBorderStyle.Outset);
						break;
					case PanelBorder.Dotted:
                        ControlPaint.DrawBorder(e.Graphics, ClientRectangle, borderColor, ButtonBorderStyle.Dotted);
						break;
					case PanelBorder.Dashed:
                        ControlPaint.DrawBorder(e.Graphics, ClientRectangle, borderColor, ButtonBorderStyle.Dashed);
						break;
					case PanelBorder.Solid:
                        switch (_parent.Style)
                        {
                            case VisualStyle.IDE2005:
                            case VisualStyle.Office2003:
                            case VisualStyle.Office2007Blue:
                            case VisualStyle.Office2007Silver:
                            case VisualStyle.Office2007Black:
                            case VisualStyle.MediaPlayerBlue:
                            case VisualStyle.MediaPlayerOrange:
                            case VisualStyle.MediaPlayerPurple:
                                using (Pen borderPen = new Pen(borderColor))
                                    e.Graphics.DrawRectangle(borderPen, 0, 0, Width - 1, Height - 1);
                                break;
                            default:
                                ControlPaint.DrawBorder(e.Graphics, ClientRectangle, ControlPaint.Dark(BackColor), ButtonBorderStyle.Solid);
                                break;
                        }
                        break;
				}
			}
		}

		private void PositionChild()
		{
			if (_statusPanel != null)
				_statusPanel.SetBounds(2, 2, Width - 4, Height - 4);
		}
    }
}
