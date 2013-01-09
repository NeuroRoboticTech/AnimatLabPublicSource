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
	/// Access to GDI32 functions.
	/// </summary>
    internal class Gdi32
    {
        /// <summary>
        /// BitBlt function of GDI32
        /// </summary>
        [DllImport("Gdi32.dll", CharSet = CharSet.Auto)]
        internal static extern int BitBlt(IntPtr hDestDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        /// <summary>
        /// ExcludeClipRect function of GDI32
        /// </summary>
        [DllImport("Gdi32.dll", CharSet = CharSet.Auto)]
        internal static extern int ExcludeClipRect(IntPtr hDC, int x1, int y1, int x2, int y2);

        /// <summary>
        /// CreateCompatibleBitmap function of GDI32
        /// </summary>
        [DllImport("Gdi32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);
        
        /// <summary>
		/// CombineRgn function of GDI32
		/// </summary>
		[DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        internal static extern int CombineRgn(IntPtr dest, IntPtr src1, IntPtr src2, int flags);

		/// <summary>
		/// CreateRectRgnIndirect function of GDI32
		/// </summary>
		[DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        internal static extern IntPtr CreateRectRgnIndirect(ref Win32.RECT rect); 

		/// <summary>
		/// GetClipBox function of GDI32
		/// </summary>
		[DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        internal static extern int GetClipBox(IntPtr hDC, ref Win32.RECT rectBox); 

		/// <summary>
		/// GetClipRgn function of GDI32
		/// </summary>
		[DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        internal static extern int GetClipRgn(IntPtr hDC, ref IntPtr hRgn); 

		/// <summary>
		/// SelectClipRgn function of GDI32
		/// </summary>
		[DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        internal static extern int SelectClipRgn(IntPtr hDC, IntPtr hRgn); 

		/// <summary>
		/// CreateBrushIndirect function of GDI32
		/// </summary>
		[DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        internal static extern IntPtr CreateBrushIndirect(ref LOGBRUSH brush); 

		/// <summary>
		/// PatBlt function of GDI32
		/// </summary>
		[DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        internal static extern bool PatBlt(IntPtr hDC, int x, int y, int width, int height, uint flags); 

		/// <summary>
		/// DeleteObject function of GDI32
		/// </summary>
		[DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        internal static extern IntPtr DeleteObject(IntPtr hObject);

		/// <summary>
		/// DeleteDC function of GDI32
		/// </summary>
		[DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        internal static extern bool DeleteDC(IntPtr hDC);

		/// <summary>
		/// SelectObject function of GDI32
		/// </summary>
		[DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        internal static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

		/// <summary>
		/// CreateCompatibleDC function of GDI32
		/// </summary>
		[DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        internal static extern IntPtr CreateCompatibleDC(IntPtr hDC);

		/// <summary>
		/// GetDeviceCaps function of GDI32
		/// </summary>
		[DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        internal static extern int GetDeviceCaps(IntPtr hDC, int nIndex); 
	}
}
