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
	/// Access to USER32 functions.
	/// </summary>
    internal class User32
    {
        /// <summary>
        /// GetWindowDC function of USER32
        /// </summary>
        [DllImport("User32", CharSet = CharSet.Auto)]
        internal static extern IntPtr GetWindowDC(IntPtr hwnd);

        /// <summary>
        /// AdjustWindowRectEx function of USER32
        /// </summary>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        internal static extern void AdjustWindowRectEx(ref RECT rect, int dwStyle, bool hasMenu, int dwExSytle);

        /// <summary>
        /// DisableProcessWindowsGhosting function of USER32
        /// </summary>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        internal static extern void DisableProcessWindowsGhosting();

        /// <summary>
        /// RedrawWindow function of USER32
        /// </summary>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        internal static extern bool RedrawWindow(IntPtr hWnd, IntPtr rectUpdate, IntPtr hRgnUpdate, uint uFlags);

		/// <summary>
		/// LockWindowUpdate function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern bool LockWindowUpdate(IntPtr hWnd);
			
		/// <summary>
		/// GetClientRect function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern bool GetClientRect(IntPtr hWnd, ref RECT rect);

		/// <summary>
		/// GetWindowLong function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

		/// <summary>
		/// SetWindowLong function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, int newLong);
            
		/// <summary>
		/// SystemParametersInfo function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref int bRetValue, uint fWinINI);

		/// <summary>
		/// AnimateWindow function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern bool AnimateWindow(IntPtr hWnd, uint dwTime, uint dwFlags);

		/// <summary>
		/// InvalidateRect function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern bool InvalidateRect(IntPtr hWnd, ref RECT rect, bool erase);

		/// <summary>
		/// LoadCursor function of USER32
		/// </summary>
		[DllImport("User32.dll", EntryPoint="InvalidateRect", CharSet=CharSet.Auto)]
        internal static extern bool InvalidateRectInt(IntPtr hWnd, IntPtr hRect, bool erase);

		/// <summary>
		/// LoadCursor function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern IntPtr LoadCursor(IntPtr hInstance, uint cursor);

		/// <summary>
		/// SetCursor function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern IntPtr SetCursor(IntPtr hCursor);

		/// <summary>
		/// GetFocus function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern IntPtr GetFocus();

		/// <summary>
		/// SetFocus function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern IntPtr SetFocus(IntPtr hWnd);

		/// <summary>
		/// ReleaseCapture function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern bool ReleaseCapture();

		/// <summary>
		/// WaitMessage function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern bool WaitMessage();

		/// <summary>
		/// TranslateMessage function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern bool TranslateMessage(ref MSG msg);

		/// <summary>
		/// DispatchMessage function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern bool DispatchMessage(ref MSG msg);

		/// <summary>
		/// PostMessage function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern bool PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

		/// <summary>
		/// SendMessage function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern uint SendMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

		/// <summary>
		/// GetMessage function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern bool GetMessage(ref MSG msg, int hWnd, uint wFilterMin, uint wFilterMax);
	
		/// <summary>
		/// PeekMessage function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern bool PeekMessage(ref MSG msg, int hWnd, uint wFilterMin, uint wFilterMax, uint wFlag);

		/// <summary>
		/// BeginPaint function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

		/// <summary>
		/// EndPaint function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

		/// <summary>
		/// GetDC function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern IntPtr GetDC(IntPtr hWnd);

        /// <summary>
        /// GetDCEx function of USER32
        /// </summary>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr GetDCEx(IntPtr hWnd, IntPtr hRgnClip, uint fdwOptions);

		/// <summary>
		/// ReleaseDC function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

		/// <summary>
		/// ShowWindow function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern int ShowWindow(IntPtr hWnd, short cmdShow);

		/// <summary>
		/// MoveWindow function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern bool MoveWindow(IntPtr hWnd, int x, int y, int width, int height, bool repaint);

		/// <summary>
		/// SetWindowPos function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndAfter, int X, int Y, int Width, int Height, uint flags);

		/// <summary>
		/// UpdateLayeredWindow function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref POINT pptDst, ref SIZE psize, IntPtr hdcSrc, ref POINT pprSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

		/// <summary>
		/// GetWindowRect function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern bool GetWindowRect(IntPtr hWnd, ref RECT rect);

		/// <summary>
		/// ClientToScreen function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern bool ClientToScreen(IntPtr hWnd, ref POINT pt);

		/// <summary>
		/// ScreenToClient function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern bool ScreenToClient(IntPtr hWnd, ref POINT pt);

		/// <summary>
		/// TrackMouseEvent function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern bool TrackMouseEvent(ref TRACKMOUSEEVENTS tme);

		/// <summary>
		/// SetWindowRgn function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern bool SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool redraw);

		/// <summary>
		/// GetKeyState function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern ushort GetKeyState(int virtKey);

		/// <summary>
		/// GetParent function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern IntPtr GetParent(IntPtr hWnd);

		/// <summary>
		/// DrawFocusRect function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern bool DrawFocusRect(IntPtr hWnd, ref RECT rect);

		/// <summary>
		/// HideCaret function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern bool HideCaret(IntPtr hWnd);

		/// <summary>
		/// ShowCaret function of USER32
		/// </summary>
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        internal static extern bool ShowCaret(IntPtr hWnd);

		/// <summary>
		/// GetSysColor function of USER32
		/// </summary>
		[DllImport("User32.dll")]
        internal static extern uint GetSysColor(SysColors colorIndex);
    }
}
