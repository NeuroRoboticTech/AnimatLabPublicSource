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
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Crownwood.DotNetMagic.Common;
using Crownwood.DotNetMagic.Win32;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Provide a popup window that gives a shadow effect.
	/// </summary>
	public class PopupShadow : NativeWindow, IDisposable
	{
		// Class constant
		private static readonly int SHADOW_SIZE = 4;

		// Instance fields
		private Pen[] _pens;
		private Brush[] _brushes;
		private Rectangle _showRect;
		private bool _valid;

		/// <summary>
		/// Initialize a new instance of the PopupShadow class. 
		/// </summary>
		public PopupShadow()
		{
			// We cannot be used in 256 color systems because they do not allow alpha blending
			// and also need to check if the OS allows layered windows. This is required because the
			// OS might say it does support it but because your using a remote connection it actually
			// only draws in 256 colors to reduce bandwidth.
			_valid = (OSFeature.Feature.GetVersionPresent(OSFeature.LayeredWindows) != null) &&
					 (ColorHelper.ColorDepth() > 8);

			if (_valid)
			{
				// Default our display rectangle
				_showRect = Rectangle.Empty;

				// Create the arrays for pens/brushes
				_pens = new Pen[SHADOW_SIZE];
				_brushes = new Brush[SHADOW_SIZE];

				// Create each individual pen/brush
				for(int i=0; i<SHADOW_SIZE; i++)
				{
					// Create the pens/brushes getting fainter and fainter
					_pens[i] = new Pen(Color.FromArgb(64 - (i * 16), 0, 0, 0));
					_brushes[i] = new SolidBrush(Color.FromArgb(64 - (i * 16), 0, 0, 0));
				}

				CreateParams cp = new CreateParams();

				// Any old title will do as it will not be shown
				cp.Caption = "NativePopupShadow";
				
				// Define the screen position/size
				cp.X = _showRect.Left;
				cp.Y = _showRect.Top;
				cp.Height = _showRect.Width;
				cp.Width = _showRect.Height;

				// As a top-level window it has no parent
				cp.Parent = IntPtr.Zero;
				
				// Appear as a top-level window
				cp.Style = unchecked((int)(uint)Win32.WindowStyles.WS_POPUP);
				
				// Set styles so that it does not have a caption bar and is above all other 
				// windows in the ZOrder, i.e. TOPMOST
				cp.ExStyle = (int)Win32.WindowExStyles.WS_EX_TOPMOST + 
							 (int)Win32.WindowExStyles.WS_EX_TOOLWINDOW;

				// We are going to use alpha blending on the shadow border and so we 
				// need  to specify the layered window style so the OS can handle it
				cp.ExStyle += (int)Win32.WindowExStyles.WS_EX_LAYERED;

				// Create the actual window
				this.CreateHandle(cp);
			}
		}

		/// <summary>
		/// Make sure the resources are disposed of gracefully.
		/// </summary>
		public void Dispose()
		{
			if (_valid)
			{
				// Kill the window
				DestroyHandle();

				// Release all cached pens
				if (_pens != null)
				{
					// Dispose of the pens
					foreach(Pen pen in _pens)
						pen.Dispose();

					_pens = null;
				}

				// Release all cached brushes
				if (_brushes != null)
				{
					foreach(Brush brush in _brushes)
						brush.Dispose();

					_brushes = null;
				}
			}
		}

		/// <summary>
		/// Gets the length of any showing shadow.
		/// </summary>
		public int ShadowLength
		{
			get { return SHADOW_SIZE; }
		}

		/// <summary>
		/// Gets and sets the displayed screen rectangle
		/// </summary>
		public Rectangle ShowRect
		{
			get { return _showRect; }
			
			set
			{
				if (_valid)
				{
					// Inflate the value by shadow size
					value.Inflate(SHADOW_SIZE, SHADOW_SIZE);

					// Only interested in changes
					if (_showRect != value)
					{
						// Remember new value
						_showRect = value;

						// Update the image for display
						UpdateLayeredWindow(_showRect);
					}
				}
			}
		}

		/// <summary>
		/// Show the window without taking the focus
		/// </summary>
		public virtual void ShowWithoutFocus()
		{
			if (_valid)
			{
				// Update the image for display
				UpdateLayeredWindow(_showRect);

				// Show the window without activating it (i.e. do not take focus)
				User32.ShowWindow(this.Handle, (short)Win32.ShowWindowStyles.SW_SHOWNOACTIVATE);
			}
		}

		private void UpdateLayeredWindow(Rectangle rect)
		{
			// Must have a visible rectangle to render
			if ((rect.Width > 0) && (rect.Height > 0))
			{
				// Create bitmap for drawing onto
				Bitmap memoryBitmap = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);

				using(Graphics g = Graphics.FromImage(memoryBitmap))
				{
					Rectangle area = new Rectangle(0, 0, rect.Width, rect.Height);
			
					// Draw the background area
					DrawShadow(g, area);

					// Get hold of the screen DC
					IntPtr hDC = User32.GetDC(IntPtr.Zero);
					
					// Create a memory based DC compatible with the screen DC
					IntPtr memoryDC = Gdi32.CreateCompatibleDC(hDC);
					
					// Get access to the bitmap handle contained in the Bitmap object
					IntPtr hBitmap = memoryBitmap.GetHbitmap(Color.FromArgb(0));

					// Select this bitmap for updating the window presentation
					IntPtr oldBitmap = Gdi32.SelectObject(memoryDC, hBitmap);

					// New window size
					Win32.SIZE ulwsize;
					ulwsize.cx = rect.Width;
					ulwsize.cy = rect.Height;

					// New window position
					Win32.POINT topPos;
					topPos.x = rect.Left;
					topPos.y = rect.Top;

					// Offset into memory bitmap is always zero
					Win32.POINT pointSource;
					pointSource.x = 0;
					pointSource.y = 0;

					// We want to make the entire bitmap opaque 
					Win32.BLENDFUNCTION blend = new Win32.BLENDFUNCTION();
					blend.BlendOp             = (byte)Win32.AlphaFlags.AC_SRC_OVER;
					blend.BlendFlags          = 0;
					blend.SourceConstantAlpha = 255;
					blend.AlphaFormat         = (byte)Win32.AlphaFlags.AC_SRC_ALPHA;

					// Tell operating system to use our bitmap for painting
					User32.UpdateLayeredWindow(Handle, hDC, ref topPos, ref ulwsize, 
											   memoryDC, ref pointSource, 0, ref blend, 
											   (int)Win32.UpdateLayeredWindowsFlags.ULW_ALPHA);

					// Put back the old bitmap handle
					Gdi32.SelectObject(memoryDC, oldBitmap);

					// Cleanup resources
					User32.ReleaseDC(IntPtr.Zero, hDC);
					Gdi32.DeleteObject(hBitmap);
					Gdi32.DeleteDC(memoryDC);
				}
			}
		}

		private void DrawShadow(Graphics g, Rectangle area)
		{
			DrawVerticalShadow(g, area);
			DrawHorizontalShadow(g, area);
		}

		private void DrawVerticalShadow(Graphics g, Rectangle area)
		{
			int left = area.Right - SHADOW_SIZE;
			int right = area.Right;
			int top = area.Top + (SHADOW_SIZE * 2);
			int bottom = area.Bottom - 1;
			int height = bottom - top;

			// Draw the top blend a line at a time
			for(int i=0; i<SHADOW_SIZE; i++)
			{
				// Draw a gradient from top to bottom
				for(int j=i; j<SHADOW_SIZE; j++)
					g.FillRectangle(_brushes[i + (SHADOW_SIZE - 1 - j)], left + i, top + j, 1, 1);
			}

			// Move down height of shadow before drawing middle section
			top += SHADOW_SIZE;
			height -= SHADOW_SIZE;

			// Draw the middle blend
			if (height > SHADOW_SIZE)
			{
				int down = bottom - SHADOW_SIZE;

				// Draw each of the shadow lines
				for(int i=0; i<SHADOW_SIZE; i++)
					g.DrawLine(_pens[i], left + i, top, left + i, down);
			}

			// Draw the bottom blend
			if (height > SHADOW_SIZE)
			{
				int down = bottom - SHADOW_SIZE + 1;

				// Draw a single pixel using a rectangle
				g.FillRectangle(_brushes[0], left, down, 1, 1);

				// Draw each of the shadow lines
				for(int i=1; i<SHADOW_SIZE; i++)
					g.DrawLine(_pens[i], left + i, down, left + i, down + i);
			}
		}

		private void DrawHorizontalShadow(Graphics g, Rectangle area)
		{
			int left = area.Left + (SHADOW_SIZE * 2);
			int right = area.Right;
			int top = area.Bottom - SHADOW_SIZE;
			int bottom = area.Bottom;
			int width = right - left;

			// Draw the left blend a line at a time
			for(int i=0; i<SHADOW_SIZE; i++)
			{
				// Draw a gradient from left to right
				for(int j=i; j<SHADOW_SIZE; j++)
					g.FillRectangle(_brushes[i + (SHADOW_SIZE - 1 - j)], left + j, top + i, 1, 1);
			}

			// Move right teh width of shadow before drawing middle section
			left += SHADOW_SIZE;
			width -= SHADOW_SIZE;

			// Draw the middle blend
			if (width > SHADOW_SIZE)
			{
				int across = right - SHADOW_SIZE - 1;

				// Draw each of the shadow lines
				for(int i=0; i<SHADOW_SIZE; i++)
					g.DrawLine(_pens[i], left, top + i, across, top + i);
			}

			// Draw the bottom blend
			if (width > SHADOW_SIZE)
			{
				int across = right - SHADOW_SIZE;

				// Draw a single pixel using a rectangle
				g.FillRectangle(_brushes[1], across, top + 1, 1, 1);

				// Draw each of the shadow lines
				for(int i=2; i<SHADOW_SIZE; i++)
					g.DrawLine(_pens[i], across, top + i, across + i - 1, top + i);
			}
		}
	}
}
