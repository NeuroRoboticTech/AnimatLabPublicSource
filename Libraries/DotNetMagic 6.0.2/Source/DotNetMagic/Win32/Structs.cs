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
using System.Runtime.InteropServices;

namespace Crownwood.DotNetMagic.Win32
{
	/// <summary>
	/// Win32 MSG structure
	/// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct MSG 
    {
		/// <summary>
		/// hwnd field of structure
		/// </summary>
        public IntPtr hwnd;
		
		/// <summary>
		/// message field of structure
		/// </summary>
		public int message;
		
		/// <summary>
		/// wParam field of structure
		/// </summary>
		public IntPtr wParam;
        		
		/// <summary>
		/// lParam field of structure
		/// </summary>
		public IntPtr lParam;
        		
		/// <summary>
		/// time field of structure
		/// </summary>
		public int time;
        		
		/// <summary>
		/// pt_x field of structure
		/// </summary>
		public int pt_x;
        		
		/// <summary>
		/// pt_y field of structure
		/// </summary>
		public int pt_y;
    }

	/// <summary>
	/// Win32 PAINTSTRUCT structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
    internal struct PAINTSTRUCT
    {
		/// <summary>
		/// hdc field of structure
		/// </summary>
		public IntPtr hdc;
                		
		/// <summary>
		/// fErase field of structure
		/// </summary>
		public int fErase;
                		
		/// <summary>
		/// rcPaint field of structure
		/// </summary>
		public Rectangle rcPaint;
                		
		/// <summary>
		/// fRestore field of structure
		/// </summary>
		public int fRestore;
                		
		/// <summary>
		/// fIncUpdate field of structure
		/// </summary>
		public int fIncUpdate;
                		
		/// <summary>
		/// Reserved1 field of structure
		/// </summary>
		public int Reserved1;
                		
		/// <summary>
		/// Reserved2 field of structure
		/// </summary>
		public int Reserved2;
                		
		/// <summary>
		/// Reserved3 field of structure
		/// </summary>
		public int Reserved3;
                		
		/// <summary>
		/// Reserved4 field of structure
		/// </summary>
		public int Reserved4;
                		
		/// <summary>
		/// Reserved5 field of structure
		/// </summary>
		public int Reserved5;
                		
		/// <summary>
		/// Reserved6 field of structure
		/// </summary>
		public int Reserved6;
                		
		/// <summary>
		/// Reserved7 field of structure
		/// </summary>
		public int Reserved7;
                		
		/// <summary>
		/// Reserved8 field of structure
		/// </summary>
		public int Reserved8;
    }

	/// <summary>
	/// Win32 RECT structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
		/// <summary>
		/// left field of structure
		/// </summary>
		public int left;
                        		
		/// <summary>
		/// top field of structure
		/// </summary>
		public int top;
                        		
		/// <summary>
		/// right field of structure
		/// </summary>
		public int right;
                        		
		/// <summary>
		/// bottom field of structure
		/// </summary>
		public int bottom;
    }

	/// <summary>
	/// Win32 POINT structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
    internal struct POINT
    {
		/// <summary>
		/// x field of structure
		/// </summary>
		public int x;
                                		
		/// <summary>
		/// y field of structure
		/// </summary>
		public int y;
    }

	/// <summary>
	/// Win32 SIZE structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
    internal struct SIZE
    {
		/// <summary>
		/// cx field of structure
		/// </summary>
		public int cx;
                                        		
		/// <summary>
		/// cy field of structure
		/// </summary>
		public int cy;
    }

	/// <summary>
	/// Win32 BLENDFUNCTION structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack=1)]
    internal struct BLENDFUNCTION
    {
		/// <summary>
		/// BlendOp field of structure
		/// </summary>
		public byte BlendOp;
                                                		
		/// <summary>
		/// BlendFlags field of structure
		/// </summary>
		public byte BlendFlags;
                                                		
		/// <summary>
		/// SourceConstantAlpha field of structure
		/// </summary>
		public byte SourceConstantAlpha;
                                                		
		/// <summary>
		/// AlphaFormat field of structure
		/// </summary>
		public byte AlphaFormat;
    }

	/// <summary>
	/// Win32 TRACKMOUSEEVENTS structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
    internal struct TRACKMOUSEEVENTS
    {
		/// <summary>
		/// cbSize field of structure
		/// </summary>
		public uint cbSize;
                                                        		
		/// <summary>
		/// dwFlags field of structure
		/// </summary>
		public uint dwFlags;
                                                        		
		/// <summary>
		/// hWnd field of structure
		/// </summary>
		public IntPtr hWnd;
                                                        		
		/// <summary>
		/// dwHoverTime field of structure
		/// </summary>
		public uint dwHoverTime;
    }

	/// <summary>
	/// Win32 LOGBRUSH structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
    internal struct LOGBRUSH
    {
		/// <summary>
		/// lbStyle field of structure
		/// </summary>
		public uint lbStyle; 
                                                                		
		/// <summary>
		/// lbColor field of structure
		/// </summary>
		public uint lbColor; 
                                                                		
		/// <summary>
		/// lbHatch field of structure
		/// </summary>
		public uint lbHatch; 
    }

    /// <summary>
    /// Win32 NCCALCSIZE_PARAMS structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct NCCALCSIZE_PARAMS
    {
        /// <summary>
        /// rectProposed field of structure
        /// </summary>
        public RECT rectProposed;

        /// <summary>
        /// rectBeforeMove field of structure
        /// </summary>
        public RECT rectBeforeMove;

        /// <summary>
        /// rectClientBeforeMove field of structure
        /// </summary>
        public RECT rectClientBeforeMove;
        
        /// <summary>
        /// lpPos field of structure
        /// </summary>
        public int lpPos;
    }

    /// <summary>
    /// Win32 NCCALCSIZWINDOWPOSE_PARAMS structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct WINDOWPOS
    {
        /// <summary>
        /// hwnd field of structure
        /// </summary>
        public IntPtr hwnd;

        /// <summary>
        /// hwndInsertAfter field of structure
        /// </summary>
        public IntPtr hwndInsertAfter;

        /// <summary>
        /// x position field of structure
        /// </summary>
        public int x;

        /// <summary>
        /// y position field of structure
        /// </summary>
        public int y;

        /// <summary>
        /// cx width field of structure
        /// </summary>
        public int cx;

        /// <summary>
        /// cy height field of structure
        /// </summary>
        public int cy;

        /// <summary>
        /// flags field of structure
        /// </summary>
        public uint flags;
    }
}
