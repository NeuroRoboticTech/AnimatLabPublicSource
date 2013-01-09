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
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Crownwood.DotNetMagic.Win32;

namespace Crownwood.DotNetMagic.Common
{
	/// <summary>
	/// Display a region with a semi-transparent colour.
	/// </summary>
	public class DropSolid : System.Windows.Forms.Form
	{
		// Instance fields
        private VisualStyle _style;
		private Region _solidRegion;
		private Rectangle _solidRect;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Initialize a new instance of the DropSolid class.
		/// </summary>
        /// <param name="style">Drawing style.</param>
		public DropSolid(VisualStyle style)
		{
            // Cache the drawing style
            _style = style;

			// Required for Windows Form Designer support
			InitializeComponent();
		}

		/// <summary>
		/// Show the window without taking activation.
		/// </summary>
		public void ShowWithoutActivate()
		{
			// Show the window without activating it (i.e. do not take focus)
			User32.ShowWindow(this.Handle, (short)Win32.ShowWindowStyles.SW_SHOWNOACTIVATE);
		}

		/// <summary>
		/// Sets the new solid rectangle area.
		/// </summary>
		public Rectangle SolidRect
		{
			set
			{
				// Use new value and redraw
				if (_solidRegion != null)
				{
					_solidRegion.Dispose();
					_solidRegion = null;
				}

				_solidRegion = null;
				_solidRect = value;

				// Set the bounds of the window to enclose the provided rectangle
				DesktopBounds = _solidRect;
				
				Refresh();
			}
		}

		/// <summary>
		/// Sets the new solid region area.
		/// </summary>
		public Rectangle[] SolidRects
		{
			set
			{
				// Clear down use of a single rectangle
				_solidRect = Rectangle.Empty;

				// Dispose of any existing region
				if (_solidRegion != null)
					_solidRegion.Dispose();

				// Create a region to aggregate rects together
				_solidRegion = new Region(value[0]);

				// Union in the extra rectangles
				for(int i=1; i<value.Length; i++)
					_solidRegion.Union(value[i]);
					
				// Set the bounds of the window to enclose the provided rectangles
				using(Graphics g = CreateGraphics())
					DesktopBounds = Rectangle.Ceiling(_solidRegion.GetBounds(g));
					
				Refresh();
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		/// <param name="e">A PaintEventArgs with event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			// Let base class do its stuff
			base.OnPaint(e);

			// If we have a solid rectangle to draw
			if (!_solidRect.IsEmpty)
			{
                Color fillColor;
                Color borderColor;

                switch (_style)
                {
                    case VisualStyle.Office2007Blue:
                    case VisualStyle.Office2007Silver:
                    case VisualStyle.Office2007Black:
                        fillColor = Office2007ColorTable.BorderColor(_style);
                        borderColor = ControlPaint.Dark(Office2007ColorTable.BorderColor(_style));
                        break;
                    case VisualStyle.MediaPlayerBlue:
                    case VisualStyle.MediaPlayerOrange:
                    case VisualStyle.MediaPlayerPurple:
                        fillColor = MediaPlayerColorTable.BorderColor(_style);
                        borderColor = ControlPaint.Dark(MediaPlayerColorTable.BorderColor(_style));
                        break;
                    default:
                        fillColor = SystemColors.ActiveCaption;
                        borderColor = SystemColors.Control;
                        break;
                }

                using(SolidBrush fillBrush = new SolidBrush(fillColor))
                    e.Graphics.FillRectangle(fillBrush, ClientRectangle);

                using (Pen borderPen = new Pen(borderColor))
				    e.Graphics.DrawRectangle(SystemPens.Control, ClientRectangle);
			}
			else if (_solidRegion != null)
			{
				// Use stored region as starting valie
				using(Region internalRegion = _solidRegion.Clone())
				{
					Matrix transform = new Matrix(DesktopBounds, new Point[]{new Point(0, 0), 
																			 new Point(Width, 0), 
																			 new Point(0, Height)});
					// Shift the desktop bounds to the client bounds
					internalRegion.Transform(transform);

					// Draw the actual indicator region
					e.Graphics.FillRegion(SystemBrushes.ActiveCaption, internalRegion);
				}			
			}
		}

		/// <summary>
		/// Processes Windows messages.
		/// </summary>
		/// <param name="m">The Windows Message to process. </param>
		protected override void WndProc(ref Message m)
		{
			// We are a transparent window, so mouse is never over us
			if (m.Msg == (int)Win32.Msgs.WM_NCHITTEST)
			{
				// Allow actions to occur to window beneath us
				m.Result = (IntPtr)Win32.HitTest.HTTRANSPARENT;
			}
			else
				base.WndProc(ref m);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// DropSolid
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Magenta;
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DropSolid";
			this.Opacity = 0.37;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "DropSolid";
			this.TransparencyKey = System.Drawing.Color.Magenta;

		}
		#endregion
	}
}
