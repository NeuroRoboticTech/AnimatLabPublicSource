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
using System.Runtime.InteropServices;

namespace Crownwood.DotNetMagic.Win32
{
	/// <summary>
	/// Access to Uxtheme functions.
	/// </summary>
    internal class Uxtheme
    {
		/// <summary>
		/// IsAppThemed function of uxtheme
		/// </summary>
		[DllImport("uxtheme.dll", CharSet=CharSet.Auto)]
        internal static extern bool IsAppThemed();

		/// <summary>
		/// IsThemeActive function of uxtheme
		/// </summary>
		[DllImport("uxtheme.dll", CharSet=CharSet.Auto)]
        internal static extern bool IsThemeActive();

		/// <summary>
		/// GetCurrentThemeName function of uxtheme
		/// </summary>
		[DllImport("uxtheme.dll", CharSet=CharSet.Auto)]
        internal static extern bool GetCurrentThemeName(char[] themeName, int nameSize, 
													    char[] colorName, int colorSize,
													    char[] sizeName, int sizeSize);
													  
		/// <summary>
		/// OpenThemeData function of uxtheme
		/// </summary>
		[DllImport("uxtheme.dll", CharSet=CharSet.Auto)]
        internal static extern IntPtr OpenThemeData(IntPtr hWnd, string classList);

		/// <summary>
		/// CloseThemeData function of uxtheme
		/// </summary>
		[DllImport("uxtheme.dll", CharSet=CharSet.Auto)]
        internal static extern int CloseThemeData(IntPtr hTheme);

		/// <summary>
		/// GetWindowTheme function of uxtheme
		/// </summary>
		[DllImport("uxtheme.dll", CharSet=CharSet.Auto)]
        internal static extern IntPtr GetWindowTheme(IntPtr hWnd);

        /// <summary>
        /// SetWindowTheme function of uxtheme
        /// </summary>
        [DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
        internal static extern int SetWindowTheme(IntPtr hWnd, String subAppName, String subIdList);

		/// <summary>
		/// DrawThemeBackground function of uxtheme
		/// </summary>
		[DllImport("uxtheme.dll", CharSet=CharSet.Auto)]
        internal static extern int DrawThemeBackground(IntPtr hTheme, 
													   IntPtr hDC, 
													   int partId, 
													   int stateId,
													   ref Win32.RECT rect,
													   IntPtr clip);
													 

		/// <summary>
		/// GetThemePartSize function of uxtheme
		/// </summary>
		[DllImport("uxtheme.dll", CharSet=CharSet.Auto)]
        internal static extern int GetThemePartSize(IntPtr hTheme, 
												    IntPtr hDC, 
												    int partId, 
												    int stateId,
												    IntPtr rect,
												    Win32.THEMESIZE themeSize,
												    ref Win32.SIZE size);
	}
}
