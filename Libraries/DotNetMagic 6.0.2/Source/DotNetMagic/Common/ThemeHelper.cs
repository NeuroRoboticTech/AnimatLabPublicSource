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
using Crownwood.DotNetMagic.Win32;

namespace Crownwood.DotNetMagic.Common
{
	/// <summary>
	/// Helper class for applying theme to a control instance.
	/// </summary>
    internal class ThemeHelper : IDisposable
    {
		// Instance fields
		private IntPtr _hTheme;
		private Control _control;
		private string _classList;
		
		/// <summary>
		/// Occurs when theme has been opened.
		/// </summary>
		public event EventHandler ThemeOpened;

		/// <summary>
		/// Occurs when theme has been closed.
		/// </summary>
		public event EventHandler ThemeClosed;
		
		/// <summary>
		/// Initialize a new instance of the ThemeHelper class.
		/// </summary>
		/// <param name="control">Control instance associated with theme.</param>
		/// <param name="classList">Class names of the theme to use.</param>
		public ThemeHelper(Control control, string classList)
		{
			// Init Win32 handle
			_hTheme = IntPtr.Zero;
		
			// Remember control details
			_control = control;
			_classList = classList;
			
			// We need to know when the control is created or destroyed
			_control.HandleCreated += new EventHandler(OnControlCreated);
			_control.HandleDestroyed += new EventHandler(OnControlDestroyed);
		}

		/// <summary>
		/// Releases all resources used by class.
		/// </summary>
		public void Dispose()
		{
			// Must release any Win32 handle
			CloseTheme();

			// Unhook from events
			_control.HandleCreated -= new EventHandler(OnControlCreated);
			_control.HandleDestroyed -= new EventHandler(OnControlDestroyed);
		}
		
		/// <summary>
		/// Gets a value indicating if the control is currently themed.
		/// </summary>
		public bool IsControlThemed
		{
			get { return (_hTheme != IntPtr.Zero); }
		}
		
		/// <summary>
		/// Draw a themed background element.
		/// </summary>
		/// <param name="g">Graphics object reference.</param>
		/// <param name="draw">Rectangle for drawing.</param>
		/// <param name="part">Theme part.</param>
		/// <param name="state">Theme state of part.</param>
		public void DrawThemeBackground(Graphics g, Rectangle draw, int part, int state)
		{
			if (IsControlThemed)
			{
				// Create a Win32 version of the drawing Rectangle
				Win32.RECT drawWin32 = new Win32.RECT();
				drawWin32.left = draw.X;
				drawWin32.top = draw.Y;
				drawWin32.right = draw.Right;
				drawWin32.bottom = draw.Bottom;

				// Get access to the underlying HDC
				IntPtr hDC = g.GetHdc();

				// Perform actual drawing work
				Uxtheme.DrawThemeBackground(_hTheme, hDC, part, state, ref drawWin32, IntPtr.Zero);

				// Must release the resource to prevent leaks!
				g.ReleaseHdc(hDC);
			}
		}

		/// <summary>
		/// Draw a themed background element.
		/// </summary>
		/// <param name="g">Graphics object reference.</param>
		/// <param name="draw">Rectangle for drawing.</param>
		/// <param name="exclude">Rectangle to exclude from drawing.</param>
		/// <param name="part">Theme part.</param>
		/// <param name="state">Theme state of part.</param>
		public void DrawThemeBackground(Graphics g, Rectangle draw, Rectangle exclude, int part, int state)
		{
			if (IsControlThemed)
			{
				// Create a Win32 version of the drawing Rectangle
				Win32.RECT drawWin32 = new Win32.RECT();
				drawWin32.left = draw.X;
				drawWin32.top = draw.Y;
				drawWin32.right = draw.Right;
				drawWin32.bottom = draw.Bottom;
			
				// Create a Win32 version of the clipping Rectangle
				Win32.RECT excludeWin32 = new Win32.RECT();
				excludeWin32.left = exclude.X;
				excludeWin32.top = exclude.Y;
				excludeWin32.right = exclude.Right;
				excludeWin32.bottom = exclude.Bottom;

				// Get access to the underlying HDC
				IntPtr hDC = g.GetHdc();

				// Make a note of the original clipping region
				IntPtr hOldClipRgn = IntPtr.Zero;
				Gdi32.GetClipRgn(hDC, ref hOldClipRgn);

				// Create region that excludes area from drawing rectangle
				IntPtr hDrawRgn = Gdi32.CreateRectRgnIndirect(ref drawWin32);
				IntPtr hExcludeRgn = Gdi32.CreateRectRgnIndirect(ref excludeWin32);
				Gdi32.CombineRgn(hExcludeRgn, hDrawRgn, hExcludeRgn, (int)CombineFlags.RGN_DIFF);

				// Define required clipping rectangle
				Gdi32.SelectClipRgn(hDC, hExcludeRgn);

				// Perform actual drawing work
				Uxtheme.DrawThemeBackground(_hTheme, hDC, part, state, ref drawWin32, IntPtr.Zero);

				// Put clipping region back again
				Gdi32.SelectClipRgn(hDC, hOldClipRgn);
				
				// Delete objects no longer needed
				Gdi32.DeleteObject(hDrawRgn);
				Gdi32.DeleteObject(hExcludeRgn);

				// Must release the resource to prevent leaks!
				g.ReleaseHdc(hDC);
			}
		}
		
		/// <summary>
		/// Get the size of a themed part in a givenstate.
		/// </summary>
		/// <param name="g">Graphics object reference.</param>
		/// <param name="part">Theme part.</param>
		/// <param name="state">Theme state of part.</param>
		/// <param name="themeSize">How to calculate the size.</param>
		/// <returns>new Size if themed; otherwise Size.Empty</returns>
		public Size GetThemePartSize(Graphics g, int part, int state, THEMESIZE themeSize)
		{
			Size retSize = Size.Empty;
			
			if (IsControlThemed)
			{
				Win32.SIZE size = new Win32.SIZE();
				
				IntPtr hDC = g.GetHdc();
				Uxtheme.GetThemePartSize(_hTheme, hDC, part, state, IntPtr.Zero, themeSize, ref size);
				g.ReleaseHdc(hDC);
				
				// Copy back results
				retSize.Width = size.cx;
				retSize.Height= size.cy;
			}
			
			return retSize;
		}		
		
		/// <summary>
		/// Processes Windows messages.
		/// </summary>
		/// <param name="m">The Windows Message to process.</param>
		public void WndProc(ref Message m)
		{
			// Has there been a change in the theme?
			if (m.Msg == (int)Win32.Msgs.WM_THEMECHANGED)
			{
				// Change in theme means we must close an existing theme handle
				CloseTheme();
				
				// Attempt to open theme data
				OpenTheme();
			}
		}

		/// <summary>
		/// Gets a value indicating if the application is themed.
		/// </summary>
		public static bool IsAppThemed
		{
			get 
			{ 
				try
				{
					return Uxtheme.IsAppThemed(); 
				}
				catch
				{
					// Any exception is because the Uxtheme.dll does not
					// exist, in which case the application is not themed
					return false;
				}
			}
		}

		/// <summary>
		/// Gets a value indicating if the theme is active.
		/// </summary>
		public static bool IsThemeActive
		{
			get 
			{ 
				try
				{
					return Uxtheme.IsThemeActive(); 
				}
				catch
				{
					// Any exception is because the Uxtheme.dll does not
					// exist, in which case the application is not themed
					return false;
				}
			}
		}
		
		/// <summary>
		/// Get the theme names.
		/// </summary>
		/// <param name="theme">Name of the theme.</param>
		/// <param name="color">Name of the colors.</param>
		/// <param name="size">Name of the size.</param>
		/// <returns>true if theme information was recovered; otherwise false.</returns>
		public static bool GetCurrentThemeName(ref string theme,
											   ref string color,
											   ref string size)
		{
			try
			{
				// Create arrays to receive information from call
				char[] themeChars = new char[256];
				char[] colorChars = new char[256];
				char[] sizeChars = new char[256];

				// Get names back from Uxtheme dll
				Uxtheme.GetCurrentThemeName(themeChars, 255, colorChars, 255, sizeChars, 255);

				int themeLength = 0;
				int colorLength = 0;
				int sizeLength = 0;

				// Find the number of characters in each returned string			
				for(themeLength=0; themeLength<256; themeLength++)
					if (themeChars[themeLength] == 0)
						break;
				for(colorLength=0; colorLength<256; colorLength++)
					if (colorChars[colorLength] == 0)
						break;
				for(sizeLength=0; sizeLength<256; sizeLength++)
					if (sizeChars[sizeLength] == 0)
						break;

				// Return names to caller
				theme = new string(themeChars, 0, themeLength);
				color = new string(colorChars, 0, colorLength);
				size = new string(sizeChars, 0, sizeLength);
				
				return true;
			}
			catch
			{
				// Any exception is because the Uxtheme.dll does not
				// exist, in which case the application is not themed
				return false;
			}
		}
	
		/// <summary>
		/// Raises the ThemeOpened event.
		/// </summary>
		protected void OnThemeOpened()
		{
			if (ThemeOpened != null)
				ThemeOpened(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the ThemeClosed event.
		/// </summary>
		protected void OnThemeClosed()
		{
			if (ThemeClosed != null)
				ThemeClosed(this, EventArgs.Empty);
		}

		private void OnControlCreated(object sender, EventArgs e)
		{
			// Attempt to open theme data
			OpenTheme();
		}

		private void OnControlDestroyed(object sender, EventArgs e)
		{
			// Must close down theme handle
			CloseTheme();
		}

		private void OpenTheme()
		{
			// Can onyl create theme information if themes are active
			if (IsAppThemed && IsThemeActive)
			{
				// Attempt to create theme data for control, of provided class type
				_hTheme = Uxtheme.OpenThemeData(_control.Handle, _classList);
				
				// Raises event
				OnThemeOpened();
			}
		}
				
		private void CloseTheme()
		{
			// Must close down theme handle
			if (IsControlThemed)
			{
				// Close down the Win32 handle
				Uxtheme.CloseThemeData(_hTheme);
				
				// No longer have a valid theme handle
				_hTheme = IntPtr.Zero;

				// Raises event
				OnThemeClosed();
			}
		}
	}
}


