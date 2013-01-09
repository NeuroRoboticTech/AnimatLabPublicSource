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
using System.Text;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Crownwood.DotNetMagic.Controls;
using Crownwood.DotNetMagic.Common;
using Crownwood.DotNetMagic.Win32;

namespace Crownwood.DotNetMagic.Forms
{
    /// <summary>
    /// Base class that allows a form to have custom chrome applied. You should derive 
    /// a class from this that performs the specific chrome drawing that is required.
    /// </summary>
    public class WindowChrome : Form
    {
        #region Static Fields
        private static bool _themedApp;
        #endregion

        #region Instance Fields
        private bool _active;
        private bool _trackingMouse;
        private bool _applyCustomChrome;
        private IntPtr _screenDC;
        #endregion

        #region Identity
        static WindowChrome()
        {
            try
            {
                // Is this application in an OS that is capable of themes and is currently themed
                _themedApp = (VisualStyleInformation.IsEnabledByUser && !string.IsNullOrEmpty(VisualStyleInformation.ColorScheme));
            }
            catch
            {
            }
        }

        /// <summary>
        /// Initialize a new instance of the WindowChrome class. 
        /// </summary>
        public WindowChrome()
        {
            // Reduce flicker by using double buffering for painting
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            // We need a create and cache a device context compatible with the display
            _screenDC = Gdi32.CreateCompatibleDC(IntPtr.Zero);
        }
        #endregion

        #region Protected
        /// <summary>
        /// Gets and sets a value indicating if custom chrome should be applied.
        /// </summary>
        internal protected bool ApplyCustomChrome
        {
            get { return _applyCustomChrome; }

            set
            {
                // Only interested in changed values
                if (_applyCustomChrome != value)
                {
                    // Store the new setting
                    _applyCustomChrome = value;

                    // If we need custom chrome drawing...
                    if (_applyCustomChrome)
                    {
                        try
                        {
                            // Set back to false in case we decide that the operating system 
                            // is not capable of supporting our custom chrome implementation
                            _applyCustomChrome = false;

                            // Only need to remove the window theme, if there is one
                            if (Uxtheme.IsAppThemed() && Uxtheme.IsThemeActive())
                            {
                                // Remove any theme that is currently drawing chrome
                                Uxtheme.SetWindowTheme(Handle, string.Empty, string.Empty);

                                // If we made it this far then we can provide custom chrome
                                _applyCustomChrome = true;

                                // Call virtual method for initializing own chrome
                                WindowChromeStart();
                            }
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        try
                        {
                            // Restore the application to previous theme setting
                            Uxtheme.SetWindowTheme(Handle, null, null);

                            // Call virtual method to reverse own chrome setup
                            WindowChromeEnd();
                        }
                        catch { }
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating if the window is currently active.
        /// </summary>
        protected bool WindowActive
        {
            get { return _active; }
        }

        /// <summary>
        /// Request that the non client border be recalculated.
        /// </summary>
        protected void ForceNonClientRecalc()
        {
            if (ApplyCustomChrome)
            {
                // Use platform invoke to request the window receive another WM_NCCALCSIZE
                // but without changing the size, position or zorder of the window.
                User32.SetWindowPos(Handle, IntPtr.Zero,
                                    0, 0, 0, 0,
                                    (int)(Win32.SetWindowPosFlags.SWP_NOMOVE | Win32.SetWindowPosFlags.SWP_NOSIZE |
                                          Win32.SetWindowPosFlags.SWP_NOZORDER | Win32.SetWindowPosFlags.SWP_NOOWNERZORDER |
                                          Win32.SetWindowPosFlags.SWP_NOACTIVATE | Win32.SetWindowPosFlags.SWP_FRAMECHANGED));
            }
        }

        /// <summary>
        /// Send the provided system command to the window for processing.
        /// </summary>
        /// <param name="sysCommand">System command.</param>
        internal void SendSysCommand(Win32.SysCommand sysCommand)
        {
            // Send window message to ourself
            User32.SendMessage(Handle, (int)Msgs.WM_SYSCOMMAND, (uint)sysCommand, 0);
        }

        /// <summary>
        /// Convert a screen location to a 
        /// </summary>
        /// <param name="screenPt">Screen point.</param>
        /// <returns>Point in window coordinates.</returns>
        protected Point ScreenToWindow(Point screenPt)
        {
            // First of all convert to client coordinates
            Point clientPt = PointToClient(screenPt);

            // Now adjust to take into account the top and left borders
            Padding borders = RealWindowBorders();
            clientPt.Offset(borders.Left, borders.Top);

            return clientPt;
        }

        /// <summary>
        /// Request the non-client area be repainted.
        /// </summary>
        protected void InvalidateNonClient()
        {
            if (!IsDisposed && !Disposing && IsHandleCreated)
            {
                User32.SetWindowPos(Handle, IntPtr.Zero, 0, 0, 0, 0,
                                    (int)(SetWindowPosFlags.SWP_NOACTIVATE | SetWindowPosFlags.SWP_NOMOVE |
                                          SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOZORDER |
                                          SetWindowPosFlags.SWP_NOOWNERZORDER | SetWindowPosFlags.SWP_FRAMECHANGED));

                User32.RedrawWindow(Handle, IntPtr.Zero, IntPtr.Zero,
                                    (int)(RedrawWindow.RDW_FRAME | 
                                          RedrawWindow.RDW_UPDATENOW | 
                                          RedrawWindow.RDW_INVALIDATE));
            }
        }

        /// <summary>
        /// Gets rectangle that is the real window rectangle based on Win32 API call.
        /// </summary>
        /// <returns>Window rectangle.</returns>
        protected Rectangle RealWindowRectangle()
        {
            // Grab the actual current size of the window, this is more accurate than using
            // the 'this.Size' which is out of date when performing a resize of the window.
            RECT windowRect = new RECT();
            User32.GetWindowRect(Handle, ref windowRect);

            // Create rectangle that encloses the entire window
            return new Rectangle(0, 0,
                                 windowRect.right - windowRect.left,
                                 windowRect.bottom - windowRect.top);
        }

        /// <summary>
        /// Gets the size of the borders requested by the real window.
        /// </summary>
        /// <returns>Border sizing.</returns>
        internal protected Padding RealWindowBorders()
        {
            RECT rect = new RECT();

            // Start with a zero sized rectangle
            rect.left = 0;
            rect.right = 0;
            rect.top = 0;
            rect.bottom = 0;

            // Adjust rectangle to add on the borders required
            User32.AdjustWindowRectEx(ref rect, CreateParams.Style, false, CreateParams.ExStyle);

            // Return the per side border values
            return new Padding(-rect.left, -rect.top, rect.right, rect.bottom);
        }
        #endregion

        #region Protected Override
        /// <summary>
        /// Process Windows-based messages.
        /// </summary>
        /// <param name="m">A Windows-based message.</param>
        protected override void WndProc(ref Message m)
        {
            bool processed = false;

            // We do not process the message if on an MDI child, because doing so prevents the 
            // LayoutMdi call on the parent from working and cascading/tiling the children
            if ((m.Msg == (int)Msgs.WM_NCCALCSIZE) && _themedApp &&
                ((MdiParent == null) || ApplyCustomChrome))
                processed = OnWM_NCCALCSIZE(ref m);

            // Do we need to override message processing?
            if (ApplyCustomChrome && !IsDisposed && !Disposing)
            {
                switch (m.Msg)
                {
                    case (int)Msgs.WM_NCACTIVATE:
                        processed = OnWM_NCACTIVATE(ref m);
                        break;
                    case (int)Msgs.WM_NCHITTEST:
                        processed = OnWM_NCHITTEST(ref m);
                        break;
                    case (int)Msgs.WM_NCPAINT:
                        processed = OnWM_NCPAINT(ref m);
                        break;
                    case (int)Msgs.WM_NCMOUSEMOVE:
                        processed = OnWM_NCMOUSEMOVE(ref m);
                        break;
                    case (int)Msgs.WM_MOUSEMOVE:
                        processed = OnWM_MOUSEMOVE(ref m);
                        break;
                    case (int)Msgs.WM_NCLBUTTONDOWN:
                        processed = OnWM_NCLBUTTONDOWN(ref m);
                        break;
                    case (int)Msgs.WM_LBUTTONUP:
                        processed = OnWM_LBUTTONUP(ref m);
                        break;
                    case (int)Msgs.WM_NCMOUSELEAVE:
                        processed = OnWM_NCMOUSELEAVE(ref m);
                        break;
                    case (int)Msgs.WM_SYSCOMMAND:
                    case (int)Msgs.WM_INITMENU:
                    case (int)Msgs.WM_SETTEXT:
                    case (int)Msgs.WM_HELP:
                        processed = OnPaintNonClient(ref m);
                        break;
                    case 0x00AE:
                        // Mystery message causes title bar buttons to draw, we want to 
                        // prevent that and ignoring the messages seems to do no harm.
                        processed = true;
                        break;
                }
            }

            // If the message has not been handled, let base class process it
            if (!processed)
                base.WndProc(ref m);
        }

        /// <summary>
        /// Raises the HandleCreated event.
        /// </summary>
        /// <param name="e">An EventArgs containing the event data.</param>
        protected override void OnHandleCreated(EventArgs e)
        {
            // Can fail on versions before XP SP1
            try
            {
                // Prevent the OS from drawing the non-client area in classic look
                // if the application stops responding to windows messages
                User32.DisableProcessWindowsGhosting();
            }
            catch { }

            base.OnHandleCreated(e);
        }


        /// <summary>
        /// Raises the Activated event.
        /// </summary>
        /// <param name="e">An EventArgs containing the event data.</param>
        protected override void OnActivated(EventArgs e)
        {
            _active = true;
            base.OnActivated(e);
        }

        /// <summary>
        /// Raises the Deactivate event.
        /// </summary>
        /// <param name="e">An EventArgs containing the event data.</param>
        protected override void  OnDeactivate(EventArgs e)
        {
            _active = false;
            base.OnDeactivate(e);
        }
        #endregion

        #region Protected Virtual
        /// <summary>
        /// Process the WM_NCCALCSIZE message when overriding window chrome.
        /// </summary>
        /// <param name="m">A Windows-based message.</param>
        /// <returns>True if the message was processed; otherwise false.</returns>
        protected virtual bool OnWM_NCCALCSIZE(ref Message m)
        {
            Padding borders;

            // Get the border sizing needed around the client area
            if (FormBorderStyle == FormBorderStyle.None)
                borders = Padding.Empty;
            else
                borders = RealWindowBorders();

            // Does the LParam contain a RECT or an NCCALCSIZE_PARAMS
            if (m.WParam == IntPtr.Zero)
            {
                // Extract the Win32 RECT structure from LPARAM
                RECT rect = (RECT)m.GetLParam(typeof(RECT));

                // Reduce provided RECT by the borders
                rect.left += borders.Left;
                rect.top += borders.Top;
                rect.right -= borders.Right;
                rect.bottom -= borders.Bottom;

                // Put back the modified rectangle
                Marshal.StructureToPtr(rect, m.LParam, false);
            }
            else
            {
                // Extract the Win32 NCCALCSIZE_PARAMS structure from LPARAM
                NCCALCSIZE_PARAMS calcsize = (NCCALCSIZE_PARAMS)m.GetLParam(typeof(NCCALCSIZE_PARAMS));

                // Reduce provided RECT by the borders
                calcsize.rectProposed.left += borders.Left;
                calcsize.rectProposed.top += borders.Top;
                calcsize.rectProposed.right -= borders.Right;
                calcsize.rectProposed.bottom -= borders.Bottom;

                // Put back the modified structure
                Marshal.StructureToPtr(calcsize, m.LParam, false);
            }

            // Message processed, do not pass onto base class for processing
            return true;
        }

        /// <summary>
        /// Process the WM_NCPAINT message when overriding window chrome.
        /// </summary>
        /// <param name="m">A Windows-based message.</param>
        /// <returns>True if the message was processed; otherwise false.</returns>
        protected virtual bool OnWM_NCPAINT(ref Message m)
        {
            // Perform actual paint operation
            OnNonClientPaint(m.HWnd);

            // We have handled the message
            m.Result = (IntPtr)(1);

            // Message processed, do not pass onto base class for processing
            return true;
        }

        /// <summary>
        /// Process the WM_NCHITTEST message when overriding window chrome.
        /// </summary>
        /// <param name="m">A Windows-based message.</param>
        /// <returns>True if the message was processed; otherwise false.</returns>
        protected virtual bool OnWM_NCHITTEST(ref Message m)
        {
            // Extract the point in screen coordinates
            Point screenPoint = new Point(m.LParam.ToInt32());

            // Convert to window coordinates
            Point windowPoint = ScreenToWindow(screenPoint);

            // Perform hit testing
            m.Result = WindowChromeHitTest(windowPoint);

            // Message processed, do not pass onto base class for processing
            return true;
        }

        /// <summary>
        /// Process the WM_NCACTIVATE message when overriding window chrome.
        /// </summary>
        /// <param name="m">A Windows-based message.</param>
        /// <returns>True if the message was processed; otherwise false.</returns>
        protected virtual bool OnWM_NCACTIVATE(ref Message m)
        {
            // Cache the new active state
            _active = (m.WParam == (IntPtr)(1));

            // Need a repaint to show change
            InvalidateNonClient();

            // Allow default processing of activation change
            m.Result = (IntPtr)(1);

            // Message processed, do not pass onto base class for processing
            return true;
        }

        /// <summary>
        /// Process a windows message that requires the non client area be repainted.
        /// </summary>
        /// <param name="m">A Windows-based message.</param>
        /// <returns>True if the message was processed; otherwise false.</returns>
        protected virtual bool OnPaintNonClient(ref Message m)
        {
            // Let window be updated with new text
            DefWndProc(ref m);

            // Need a repaint to show change
            InvalidateNonClient();

            // Message processed, do not pass onto base class for processing
            return true;
        }

        /// <summary>
        /// Process the WM_NCMOUSEMOVE message when overriding window chrome.
        /// </summary>
        /// <param name="m">A Windows-based message.</param>
        /// <returns>True if the message was processed; otherwise false.</returns>
        protected virtual bool OnWM_NCMOUSEMOVE(ref Message m)
        {
            // Extract the point in screen coordinates
            Point screenPoint = new Point(m.LParam.ToInt32());

            // Convert to window coordinates
            Point windowPoint = ScreenToWindow(screenPoint);

            // Perform actual mouse movement actions
            WindowChromeNonClientMouseMove(windowPoint);

            // If we are not tracking when the mouse leaves the non-client window
            if (!_trackingMouse)
            {
                TRACKMOUSEEVENTS tme = new TRACKMOUSEEVENTS();

                // This structure needs to know its own size in bytes
                tme.cbSize = (uint)Marshal.SizeOf(typeof(TRACKMOUSEEVENTS));
                tme.dwHoverTime = 100;

                // We need to know then the mouse leaves the non client window area
                tme.dwFlags = (int)(TrackerEventFlags.TME_LEAVE |
                                    TrackerEventFlags.TME_NONCLIENT);

                // We want to track our own window
                tme.hWnd = Handle;

                // Call Win32 API to start tracking
                User32.TrackMouseEvent(ref tme);

                // Do not need to track again until mouse reenters the window
                _trackingMouse = true;
            }

            // Indicate that we processed the message
            m.Result = IntPtr.Zero;

            // Message processed, do not pass onto base class for processing
            return true;
        }

        /// <summary>
        /// Process the WM_MOUSEMOVE message when overriding window chrome.
        /// </summary>
        /// <param name="m">A Windows-based message.</param>
        /// <returns>True if the message was processed; otherwise false.</returns>
        protected virtual bool OnWM_MOUSEMOVE(ref Message m)
        {
            // Extract the point in client coordinates
            Point clientPoint = new Point(m.LParam.ToInt32());

            // Perform actual mouse movement actions
            return WindowChromeClientMouseMove(clientPoint);
        }

        /// <summary>
        /// Process the WM_NCLBUTTONDOWN message when overriding window chrome.
        /// </summary>
        /// <param name="m">A Windows-based message.</param>
        /// <returns>True if the message was processed; otherwise false.</returns>
        protected virtual bool OnWM_NCLBUTTONDOWN(ref Message m)
        {
            // Extract the point in screen coordinates
            Point screenPoint = new Point(m.LParam.ToInt32());

            // Convert to window coordinates
            Point windowPoint = ScreenToWindow(screenPoint);

            // Perform actual mouse down processing
            return WindowChromeLeftMouseDown(windowPoint);
        }

        /// <summary>
        /// Process the WM_LBUTTONUP message when overriding window chrome.
        /// </summary>
        /// <param name="m">A Windows-based message.</param>
        /// <returns>True if the message was processed; otherwise false.</returns>
        protected virtual bool OnWM_LBUTTONUP(ref Message m)
        {
            // Extract the point in client coordinates
            Point clientPoint = new Point(m.LParam.ToInt32());

            // Perform actual mouse up processing
            return WindowChromeLeftMouseUp(clientPoint);
        }

        /// <summary>
        /// Process the WM_NCMOUSELEAVE message when overriding window chrome.
        /// </summary>
        /// <param name="m">A Windows-based message.</param>
        /// <returns>True if the message was processed; otherwise false.</returns>
        protected virtual bool OnWM_NCMOUSELEAVE(ref Message m)
        {
            // Next time the mouse enters the window we need to track it leaving
            _trackingMouse = false;

            // Perform actual mouse leave actions
            WindowChromeMouseLeave();

            // Indicate that we processed the message
            m.Result = IntPtr.Zero;

            // Need a repaint to show change
            InvalidateNonClient();

            // Message processed, do not pass onto base class for processing
            return true;
        }

        /// <summary>
        /// Perform chrome window painting in the non-client areas.
        /// </summary>
        /// <param name="hWnd">Window handle of window being painted.</param>
        protected virtual void OnNonClientPaint(IntPtr hWnd)
        {
            // Create rectangle that encloses the entire window
            Rectangle windowBounds = RealWindowRectangle();

            // We can only draw a window that has some size
            if ((windowBounds.Width > 0) && (windowBounds.Height > 0))
            {
                // Get the device context for this window
                IntPtr hDC = User32.GetWindowDC(Handle);

                // If we managed to get a device context
                if (hDC != IntPtr.Zero)
                {
                    try
                    {
                        // Create one the correct size and cache for future drawing
                        IntPtr hBitmap = Gdi32.CreateCompatibleBitmap(hDC, Width, Height);

                        // If we managed to get a compatible bitmap
                        if (hBitmap != IntPtr.Zero)
                        {
                            try
                            {
                                // Find the rectangle that covers the client area of the form
                                Padding borders = RealWindowBorders();
                                Rectangle clipClientRect = new Rectangle(borders.Left, borders.Top,
                                                                         Width - borders.Horizontal,
                                                                         Height - borders.Vertical);

                                bool minimized = IsMinimized;

                                // After excluding the client area, is there anything left to draw?
                                if (minimized || ((clipClientRect.Width > 0) && (clipClientRect.Height > 0)))
                                {
                                    // If not minimized we need to clip the client area
                                    if (!minimized)
                                    {
                                        // Exclude client area from being drawn into and blit blitted
                                        Gdi32.ExcludeClipRect(hDC, clipClientRect.Left, clipClientRect.Top,
                                                                   clipClientRect.Right, clipClientRect.Bottom);
                                    }

                                    // Must use the screen device context for the bitmap when drawing into the 
                                    // bitmap otherwise the Opacity and RightToLeftLayout will not work correctly.
                                    Gdi32.SelectObject(_screenDC, hBitmap);

                                    // Drawing is easier when using a Graphics instance
                                    using (Graphics g = Graphics.FromHdc(_screenDC))
                                    {
                                        // If minimized...
                                        if (WindowState == FormWindowState.Minimized)
                                        {
                                            // ...then draw over client as well, this is needed in an MDI
                                            // child window that is minized, it needs to draw everything.
                                            WindowChromePaint(g, windowBounds);
                                        }
                                        else
                                        {
                                            Region clipRegion;

                                            // Drawing is clipped to the region that represents the non-client
                                            // window edge, or the whole of the window if no region is specified
                                            if (Region == null)
                                                clipRegion = new Region(windowBounds);
                                            else
                                                clipRegion = Region.Clone();

                                            // Clip drawing to just the non-client window chrome
                                            using (UseClipping clip = new UseClipping(g, clipRegion))
                                            {
                                                // Perform actual drawing
                                                WindowChromePaint(g, windowBounds);
                                            }

                                            // Dispose of clip region to prevent memory leak
                                            clipRegion.Dispose();
                                        }
                                    }

                                    // Now blit from the bitmap to the screen
                                    Gdi32.BitBlt(hDC, 0, 0, Width, Height, _screenDC, 0, 0, 0xCC0020);
                                }
                            }
                            finally
                            {
                                // Delete the temporary bitmap
                                Gdi32.DeleteObject(hBitmap);
                            }
                        }
                    }
                    finally
                    {
                        // Must always release the device context
                        User32.ReleaseDC(Handle, hDC);
                    }
                }
            }
        }
        #endregion

        #region Protected WindowChromePaint
        /// <summary>
        /// Perform setup for custom chrome.
        /// </summary>
        protected virtual void WindowChromeStart()
        {
        }

        /// <summary>
        /// Perform cleanup when custom chrome ending.
        /// </summary>
        protected virtual void WindowChromeEnd()
        {
        }

        /// <summary>
        /// Perform hit testing.
        /// </summary>
        /// <param name="pt">Point in window coordinates.</param>
        /// <returns></returns>
        protected virtual IntPtr WindowChromeHitTest(Point pt)
        {
            return (IntPtr)HitTest.HTCLIENT;
        }

        /// <summary>
        /// Perform painting of the window chrome.
        /// </summary>
        /// <param name="g">Graphics instance to use for drawing.</param>
        /// <param name="bounds">Bounds enclosing the window chrome.</param>
        protected virtual void WindowChromePaint(Graphics g, Rectangle bounds)
        {
        }

        /// <summary>
        /// Perform non-client mouse movement processing.
        /// </summary>
        /// <param name="pt">Point in window coordinates.</param>
        protected virtual void WindowChromeNonClientMouseMove(Point pt)
        {
        }

        /// <summary>
        /// Perform client mouse movement processing.
        /// </summary>
        /// <param name="pt">Point in window coordinates.</param>
        /// <returns>True if the message is handled; otherwise false.</returns>
        protected virtual bool WindowChromeClientMouseMove(Point pt)
        {
            return false;
        }

        /// <summary>
        /// Process the left mouse down event.
        /// </summary>
        /// <param name="pt">Window coordinate of the mouse up.</param>
        /// <returns>True if event is processed; otherwise false.</returns>
        protected virtual bool WindowChromeLeftMouseDown(Point pt)
        {
            // By default, we have not handled the mouse down event
            return false;
        }

        /// <summary>
        /// Process the left mouse up event.
        /// </summary>
        /// <param name="pt">Window coordinate of the mouse up.</param>
        /// <returns>True if event is processed; otherwise false.</returns>
        protected virtual bool WindowChromeLeftMouseUp(Point pt)
        {
            // By default, we have not handled the mouse up event
            return false;
        }

        /// <summary>
        /// Perform mouse leave processing.
        /// </summary>
        protected virtual void WindowChromeMouseLeave()
        { 
        }
        #endregion
        
        #region Implementation
        private bool IsMinimized
        {
            get
            {
                // Get the current window style (cannot use the 
                // WindowState property as it can be slightly out of date)
                uint style = User32.GetWindowLong(Handle, (int)GetWindowLongFlags.GWL_STYLE);

                return ((style &= (uint)WindowStyles.WS_MINIMIZE) != 0);
            }
        }
        #endregion
    }

    /// <summary>
    /// Draw the window chrome and toolstrips using the specified VisualStyle
    /// </summary>
    public class DotNetMagicForm : WindowChrome
    {
        #region ChromeButton
        private abstract class ChromeButton
        {
            #region Instance Fields
            private ImageAttributes _attribs;
            private Image[] _buttonA;
            private Image[] _buttonAH;
            private Image[] _buttonAP;
            private Image[] _buttonI;
            private Image[] _buttonTracking;
            private Image[] _buttonPressed;
            private Image[] _buttonTrackingS;
            private Image[] _buttonPressedS;
            private Rectangle _bounds;
            private bool _visible;
            private bool _tracking;
            private bool _pressed;
            #endregion

            #region Identity
            /// <summary>
            /// Initialize a new instance of the ChromeButton class.
            /// </summary>
            /// <param name="buttonA">Theme specific active images.</param>
            /// <param name="buttonAH">Theme specific active and hot tracking images.</param>
            /// <param name="buttonAP">Theme specific active and pressed images.</param>
            /// <param name="buttonI">Theme specific button content inactive images.</param>
            /// <param name="buttonTracking">Theme specific hot tracking background images.</param>
            /// <param name="buttonPressed">Theme specific pressed background images.</param>
            /// <param name="buttonTrackingS">Theme specific small hot tracking background images.</param>
            /// <param name="buttonPressedS">Theme specific small pressed background images.</param>
            public ChromeButton(Image[] buttonA,
                                Image[] buttonAH,
                                Image[] buttonAP,
                                Image[] buttonI,
                                Image[] buttonTracking,
                                Image[] buttonPressed,
                                Image[] buttonTrackingS,
                                Image[] buttonPressedS)
            {
                _buttonA = buttonA;
                _buttonAH = buttonAH;
                _buttonAP = buttonAP;
                _buttonI = buttonI;
                _buttonTracking = buttonTracking;
                _buttonPressed = buttonPressed;
                _buttonTrackingS = buttonTrackingS;
                _buttonPressedS = buttonPressedS;

                ColorMap remap = new ColorMap();
                remap.OldColor = Color.Magenta;
                remap.NewColor = Color.Transparent;
                _attribs = new ImageAttributes();
                _attribs.SetRemapTable(new ColorMap[] { remap });
            }
            #endregion

            #region Public
            /// <summary>
            /// Gets and sets the button drawing rectangle.
            /// </summary>
            public Rectangle Bounds
            {
                get { return _bounds; }
                set { _bounds = value; }
            }

            /// <summary>
            /// Gets and sets the button drawing rectangle.
            /// </summary>
            public bool Visible
            {
                get { return _visible; }

                protected set 
                { 
                    _visible = value; 
                    
                    // Cannot be tracking if not visible
                    if (!_visible)
                        _tracking = false;
                }
            }

            /// <summary>
            /// Gets and sets the pressed state of the button.
            /// </summary>
            public bool Pressed
            {
                get { return _pressed; }
                set { _pressed = value; }
            }

            /// <summary>
            /// Update the visible state of the button based on the new mouse position.
            /// </summary>
            /// <param name="parent">Parent form instance.</param>
            public abstract void CalculateVisibleState(Form parent);

            /// <summary>
            /// Update the mouse state of the button based on the parent form state.
            /// </summary>
            /// <param name="pt">Point in window coordinates.</param>
            /// <remarks>True if a change of state needs a repaint; otherwise false.</remarks>
            public bool CalculateMouseMove(Point pt)
            {
                // Find new tracking state
                bool tracking = (Visible && _bounds.Contains(pt));

                // Is this a change in state?
                if (_tracking != tracking)
                {
                    // Store new value
                    _tracking = tracking;

                    // Request a repaint to show the change in state
                    return true;
                }

                return false;
            }

            /// <summary>
            /// Update the mouse state of the button based on the mouse leaving the form.
            /// </summary>
            /// <remarks>True if a change of state needs a repaint; otherwise false.</remarks>
            public bool CalculateMouseLeave()
            {
                // If we are currently tracking
                if (_tracking)
                {
                    // Reset state
                    _tracking = false;

                    // Request a repaint to show the change in state
                    return true;
                }

                return false;
            }

            /// <summary>
            /// Draw the button in its current state.
            /// </summary>
            /// <param name="g">Graphics reference.</param>
            /// <param name="state">State of the owning form.</param>
            /// <param name="active">Is the owning form active.</param>
            /// <param name="themeIndex">Theme index.</param>
            public void Draw(Graphics g, 
                             FormWindowState state,
                             bool active,
                             int themeIndex)
            {
                // Do we need to draw the tracking/pressed background image?
                if (_tracking)
                {
                    // Get the appropriate tracking/pressed image
                    Image button;
                    
                    if (_bounds.Height > 16)
                        button = (_pressed ? _buttonPressed[themeIndex] : _buttonTracking[themeIndex]);
                    else
                        button = (_pressed ? _buttonPressedS[themeIndex] : _buttonTrackingS[themeIndex]);

                    // Draw the tracking background
                    g.DrawImage(button,
                                _bounds,
                                0, 0, button.Width, button.Height,
                                GraphicsUnit.Pixel,
                                _attribs);
                }

                // Get the content image to draw in the center
                Image buttonContent = ButtonContent(state, active, _tracking, _pressed, themeIndex);

                // Find the x and y position for the content4
                int x = _bounds.X + (_bounds.Width - buttonContent.Width) / 2;
                int yExtra = _bounds.Height - buttonContent.Height;
                int y = _bounds.Y + (yExtra -(yExtra / 2));

                // Draw the button content
                g.DrawImage(buttonContent,
                            new Rectangle(x, y, buttonContent.Width, buttonContent.Height),
                            0, 0, buttonContent.Width, buttonContent.Height,
                            GraphicsUnit.Pixel,
                            _attribs);
            }

            /// <summary>
            /// Returns the hit test result for this button.
            /// </summary>
            public abstract IntPtr HitTest { get; }

            /// <summary>
            /// Gets the system command to execute when button is pressed.
            /// </summary>
            /// <param name="parent">Parent form instance.</param>
            /// <returns>SystemCommand value.</returns>
            public abstract Win32.SysCommand SysCommand(Form parent);
            #endregion

            #region Protected
            /// <summary>
            /// Gets the content image based on the current state.
            /// </summary>
            /// <param name="state">State of the owning form.</param>
            /// <param name="active">Active state of the button.</param>
            /// <param name="tracking">Is button being hot tracked.</param>
            /// <param name="pressed">Is button being pressed.</param>
            /// <param name="themeIndex">Theme index.</param>
            /// <returns>Image to use when drawing.</returns>
            protected virtual Image ButtonContent(FormWindowState state,
                                                  bool active,
                                                  bool tracking,
                                                  bool pressed,
                                                  int themeIndex)
            {
                if (tracking)
                {
                    if (Pressed)
                        return _buttonAP[themeIndex];
                    else
                        return _buttonAH[themeIndex];
                }
                else
                {
                    if (active)
                        return _buttonA[themeIndex];
                    else
                        return _buttonI[themeIndex];
                }
            }
            #endregion
        }

        private class ChromeButtonClose : ChromeButton
        {
            #region Identity
            /// <summary>
            /// Initialize a new instance of the ChromeButtonClose class.
            /// </summary>
            /// <param name="buttonA">Theme specific active images.</param>
            /// <param name="buttonAH">Theme specific active and hot tracking images.</param>
            /// <param name="buttonAP">Theme specific active and pressed images.</param>
            /// <param name="buttonI">Theme specific button content inactive images.</param>
            /// <param name="buttonTracking">Theme specific hot tracking background images.</param>
            /// <param name="buttonPressed">Theme specific pressed background images.</param>
            /// <param name="buttonTrackingS">Theme specific small hot tracking background images.</param>
            /// <param name="buttonPressedS">Theme specific small pressed background images.</param>
            public ChromeButtonClose(Image[] buttonA,
                                     Image[] buttonAH,
                                     Image[] buttonAP,
                                     Image[] buttonI,
                                     Image[] buttonTracking,
                                     Image[] buttonPressed,
                                     Image[] buttonTrackingS,
                                     Image[] buttonPressedS)
                : base(buttonA, buttonAH, buttonAP,
                       buttonI, buttonTracking, buttonPressed,
                       buttonTrackingS, buttonPressedS)
            {
            }
            #endregion

            #region Public
            /// <summary>
            /// Update the visible state of the button based on the parent form state.
            /// </summary>
            /// <param name="parent">Parent form instance.</param>
            public override void CalculateVisibleState(Form parent)
            {
                // Default to not being visible
                bool visible = false;

                // Is the close button supposed to be visible?
                if (parent.ControlBox)
                {
                    if (parent.FormBorderStyle != FormBorderStyle.None)
                        visible = true;
                }

                Visible = visible;
            }

            /// <summary>
            /// Returns the hit test result for this button.
            /// </summary>
            public override IntPtr HitTest 
            {
                get { return (IntPtr)Win32.HitTest.HTCLOSE; }
            }

            /// <summary>
            /// Gets the system command to execute when button is pressed.
            /// </summary>
            /// <param name="parent">Parent form instance.</param>
            /// <returns>SystemCommand value.</returns>
            public override Win32.SysCommand SysCommand(Form parent)
            {
                return Win32.SysCommand.SC_CLOSE;
            }
            #endregion
        }

        private class ChromeButtonMin : ChromeButton
        {
            #region Instance Fields
            private Image[] _buttonRestoreA;
            private Image[] _buttonRestoreAH;
            private Image[] _buttonRestoreAP;
            private Image[] _buttonRestoreI;
            #endregion

            #region Identity
            /// <summary>
            /// Initialize a new instance of the ChromeButtonMin class.
            /// </summary>
            /// <param name="buttonMinA">Theme specific minimize active images.</param>
            /// <param name="buttonRestoreA">Theme specific restore active images.</param>
            /// <param name="buttonMinAH">Theme specific minimize active and hot tracking images.</param>
            /// <param name="buttonRestoreAH">Theme specific restore active and hot tracking images.</param>
            /// <param name="buttonMinAP">Theme specific minimize active and pressed images.</param>
            /// <param name="buttonRestoreAP">Theme specific restore active and pressed images.</param>
            /// <param name="buttonMinI">Theme specific minimize button content inactive images.</param>
            /// <param name="buttonRestoreI">Theme specific restore button content inactive images.</param>
            /// <param name="buttonTracking">Theme specific hot tracking background images.</param>
            /// <param name="buttonPressed">Theme specific pressed background images.</param>
            /// <param name="buttonTrackingS">Theme specific small hot tracking background images.</param>
            /// <param name="buttonPressedS">Theme specific small pressed background images.</param>
            public ChromeButtonMin(Image[] buttonMinA, Image[] buttonRestoreA,
                                   Image[] buttonMinAH, Image[] buttonRestoreAH,
                                   Image[] buttonMinAP, Image[] buttonRestoreAP,
                                   Image[] buttonMinI, Image[] buttonRestoreI,
                                   Image[] buttonTracking,
                                   Image[] buttonPressed,
                                   Image[] buttonTrackingS,
                                   Image[] buttonPressedS)
                : base(buttonMinA, buttonMinAH, buttonMinAP, buttonMinI,
                       buttonTracking, buttonPressed,
                       buttonTrackingS, buttonPressedS)
            {
                _buttonRestoreA = buttonRestoreA;
                _buttonRestoreAH = buttonRestoreAH;
                _buttonRestoreAP = buttonRestoreAP;
                _buttonRestoreI = buttonRestoreI;
            }
            #endregion

            #region Public
            /// <summary>
            /// Update the visible state of the button based on the parent form state.
            /// </summary>
            /// <param name="parent">Parent form instance.</param>
            public override void CalculateVisibleState(Form parent)
            {
                // Default to not being visible
                bool visible = false;

                // Is the minimize button supposed to be visible?
                if (parent.ControlBox && parent.MinimizeBox)
                {
                    if ((parent.FormBorderStyle == FormBorderStyle.Sizable) ||
                        (parent.FormBorderStyle == FormBorderStyle.FixedSingle) ||
                        (parent.FormBorderStyle == FormBorderStyle.FixedDialog) ||
                        (parent.FormBorderStyle == FormBorderStyle.Fixed3D))
                        visible = true;
                }

                Visible = visible;
            }

            /// <summary>
            /// Returns the hit test result for this button.
            /// </summary>
            public override IntPtr HitTest
            {
                get { return (IntPtr)Win32.HitTest.HTMINBUTTON; }
            }

            /// <summary>
            /// Gets the system command to execute when button is pressed.
            /// </summary>
            /// <param name="parent">Parent form instance.</param>
            /// <returns>SystemCommand value.</returns>
            public override Win32.SysCommand SysCommand(Form parent)
            {
                if (parent.WindowState == FormWindowState.Minimized)
                    return Win32.SysCommand.SC_RESTORE;
                else
                    return Win32.SysCommand.SC_MINIMIZE;
            }
            #endregion

            #region Protected
            /// <summary>
            /// Gets the content image based on the current state.
            /// </summary>
            /// <param name="state">State of the owning form.</param>
            /// <param name="active">Active state of the button.</param>
            /// <param name="tracking">Is button being hot tracked.</param>
            /// <param name="pressed">Is button being pressed.</param>
            /// <param name="themeIndex">Theme index.</param>
            /// <returns>Image to use when drawing.</returns>
            protected override Image ButtonContent(FormWindowState state,
                                                   bool active,
                                                   bool tracking,
                                                   bool pressed,
                                                   int themeIndex)
            {
                // If minimized then we show the restore set of images
                if (state == FormWindowState.Minimized)
                {
                    if (tracking)
                    {
                        if (Pressed)
                            return _buttonRestoreAP[themeIndex];
                        else
                            return _buttonRestoreAH[themeIndex];
                    }
                    else
                    {
                        if (active)
                            return _buttonRestoreA[themeIndex];
                        else
                            return _buttonRestoreI[themeIndex];
                    }
                }
                else
                    return base.ButtonContent(state, active, tracking, pressed, themeIndex);
            }
            #endregion
        }

        private class ChromeButtonMax : ChromeButton
        {
            #region Instance Fields
            private Image[] _buttonRestoreA;
            private Image[] _buttonRestoreAH;
            private Image[] _buttonRestoreAP;
            private Image[] _buttonRestoreI;
            #endregion

            #region Identity
            /// <summary>
            /// Initialize a new instance of the ChromeButtonMax class.
            /// </summary>
            /// <param name="buttonMaxA">Theme specific maximize active images.</param>
            /// <param name="buttonRestoreA">Theme specific restore active images.</param>
            /// <param name="buttonMaxAH">Theme specific maximize active and hot tracking images.</param>
            /// <param name="buttonRestoreAH">Theme specific restore active and hot tracking images.</param>
            /// <param name="buttonMaxAP">Theme specific maximize active and pressed images.</param>
            /// <param name="buttonRestoreAP">Theme specific restore active and pressed images.</param>
            /// <param name="buttonMaxI">Theme specific maximize button content inactive images.</param>
            /// <param name="buttonRestoreI">Theme specific restore button content inactive images.</param>
            /// <param name="buttonTracking">Theme specific hot tracking background images.</param>
            /// <param name="buttonPressed">Theme specific pressed background images.</param>
            /// <param name="buttonTrackingS">Theme specific small hot tracking background images.</param>
            /// <param name="buttonPressedS">Theme specific small pressed background images.</param>
            public ChromeButtonMax(Image[] buttonMaxA, Image[] buttonRestoreA,
                                   Image[] buttonMaxAH, Image[] buttonRestoreAH,
                                   Image[] buttonMaxAP, Image[] buttonRestoreAP,
                                   Image[] buttonMaxI, Image[] buttonRestoreI,
                                   Image[] buttonTracking,
                                   Image[] buttonPressed,
                                   Image[] buttonTrackingS,
                                   Image[] buttonPressedS)
                : base(buttonMaxA, buttonMaxAH, buttonMaxAP, buttonMaxI,
                       buttonTracking, buttonPressed,
                       buttonTrackingS, buttonPressedS)
            {
                _buttonRestoreA = buttonRestoreA;
                _buttonRestoreAH = buttonRestoreAH;
                _buttonRestoreAP = buttonRestoreAP;
                _buttonRestoreI = buttonRestoreI;
            }
            #endregion

            #region Public
            /// <summary>
            /// Update the visible state of the button based on the parent form state.
            /// </summary>
            /// <param name="parent">Parent form instance.</param>
            public override void CalculateVisibleState(Form parent)
            {
                // Default to not being visible
                bool visible = false;

                // Is the maximize button supposed to be visible?
                if (parent.ControlBox && parent.MaximizeBox)
                {
                    if ((parent.FormBorderStyle == FormBorderStyle.Sizable) ||
                        (parent.FormBorderStyle == FormBorderStyle.FixedSingle) ||
                        (parent.FormBorderStyle == FormBorderStyle.FixedDialog) ||
                        (parent.FormBorderStyle == FormBorderStyle.Fixed3D))
                        visible = true;
                }

                Visible = visible;
            }

            /// <summary>
            /// Returns the hit test result for this button.
            /// </summary>
            public override IntPtr HitTest
            {
                get { return (IntPtr)Win32.HitTest.HTMAXBUTTON; }
            }

            /// <summary>
            /// Gets the system command to execute when button is pressed.
            /// </summary>
            /// <param name="parent">Parent form instance.</param>
            /// <returns>SystemCommand value.</returns>
            public override Win32.SysCommand SysCommand(Form parent)
            {
                if (parent.WindowState == FormWindowState.Maximized)
                    return Win32.SysCommand.SC_RESTORE;
                else
                    return Win32.SysCommand.SC_MAXIMIZE;
            }
            #endregion

            #region Protected
            /// <summary>
            /// Gets the content image based on the current state.
            /// </summary>
            /// <param name="state">State of the owning form.</param>
            /// <param name="active">Active state of the button.</param>
            /// <param name="tracking">Is button being hot tracked.</param>
            /// <param name="pressed">Is button being pressed.</param>
            /// <param name="themeIndex">Theme index.</param>
            /// <returns>Image to use when drawing.</returns>
            protected override Image ButtonContent(FormWindowState state,
                                                   bool active,
                                                   bool tracking,
                                                   bool pressed,
                                                   int themeIndex)
            {
                // If maximized then we show the restore set of images
                if (state == FormWindowState.Maximized)
                {
                    if (tracking)
                    {
                        if (Pressed)
                            return _buttonRestoreAP[themeIndex];
                        else
                            return _buttonRestoreAH[themeIndex];
                    }
                    else
                    {
                        if (active)
                            return _buttonRestoreA[themeIndex];
                        else
                            return _buttonRestoreI[themeIndex];
                    }
                }
                else
                    return base.ButtonContent(state, active, tracking, pressed, themeIndex);
            }
            #endregion
        }
        #endregion

        #region Static Fields
        private static int _iconGap = 2;
        private static int _captionBottomGap = 2;
        private static int _htCorner = 8;
        private static Color[] _captionBorderActive = new Color[] { Color.FromArgb(59, 90, 130), Color.FromArgb(152, 152, 152), Color.FromArgb(47, 47, 47), Color.Black, Color.Black, Color.Black };
        private static Color[] _captionBorderInactive = new Color[] { Color.FromArgb(151, 165, 183), Color.FromArgb(204, 204, 204), Color.FromArgb(146, 146, 146), Color.Black, Color.Black, Color.Black };
        private static Color[] _captionTextActive = new Color[] { Color.FromArgb(59, 90, 130), Color.FromArgb(53, 110, 176), Color.FromArgb(174, 209, 226), Color.White, Color.White, Color.White };
        private static Color[] _captionTextInactive = new Color[] { Color.FromArgb(151, 165, 183), Color.FromArgb(138, 138, 138), Color.FromArgb(225, 225, 213), Color.FromArgb(160, 160, 160), Color.FromArgb(160, 160, 160), Color.FromArgb(160, 160, 160) };
        private static Color[] _outsideBorderActive = new Color[] { Color.FromArgb(59, 90, 130), Color.FromArgb(114, 120, 128), Color.FromArgb(47, 47, 47), Color.Black, Color.Black, Color.Black };
        private static Color[] _outsideBorderInactive = new Color[] { Color.FromArgb(192, 198, 206), Color.FromArgb(180, 185, 192), Color.FromArgb(146, 146, 146), Color.Black, Color.Black, Color.Black };
        private static Color[] _insideBorderActive = new Color[] { Color.FromArgb(177, 198, 225), Color.FromArgb(222, 221, 222), Color.FromArgb(77, 77, 77), Color.FromArgb(99, 108, 135), Color.FromArgb(99, 108, 135), Color.FromArgb(99, 108, 135) };
        private static Color[] _insideBorderInactive = new Color[] { Color.FromArgb(204, 214, 226), Color.FromArgb(240, 240, 240), Color.FromArgb(159, 159, 159), Color.FromArgb(99, 108, 135), Color.FromArgb(99, 108, 135), Color.FromArgb(99, 108, 135) };
        private static Color[] _middleBorderActive = new Color[] { Color.FromArgb(194, 217, 247), Color.FromArgb(187, 186, 186), Color.FromArgb(102, 102, 102), Color.FromArgb(99, 108, 135), Color.FromArgb(99, 108, 135), Color.FromArgb(99, 108, 135) };
        private static Color[] _middleBorderInactive = new Color[] { Color.FromArgb(212, 222, 236), Color.FromArgb(224, 224, 224), Color.FromArgb(171, 171, 171), Color.FromArgb(99, 108, 135), Color.FromArgb(99, 108, 135), Color.FromArgb(99, 108, 135) };
        private static Color[] _lastBorderInactive = new Color[] { Color.FromArgb(204, 216, 232), Color.FromArgb(230, 229, 229), Color.FromArgb(153, 153, 153), Color.FromArgb(99, 108, 135), Color.FromArgb(99, 108, 135), Color.FromArgb(99, 108, 135) };
        #endregion

        #region Instance Fields
        private int _buttonLength;
        private int _themeIndex;
        private int _pressedButtonIndex;
        private StatusStrip _statusStrip;
        private StatusStrip _renderStrip;
        private bool _mergeStatusStrip;
        private bool _applyStyleToChrome;
        private bool _applyStyleToToolStrips;
        private bool _apply2007ClearType;
        private bool _applyMediaPlayerClearType;
        private ChromeButton[] _buttons;
        private VisualStyle _style;
        private FormWindowState _regionWindowState;
        private Image[] _buttonTracking, _buttonPressed;
        private Image[] _buttonTrackingS, _buttonPressedS;
        private Image[] _buttonCloseA, _buttonCloseAH, _buttonCloseAP, _buttonCloseI;
        private Image[] _buttonMaxA, _buttonMaxAH, _buttonMaxAP, _buttonMaxI;
        private Image[] _buttonRestoreA, _buttonRestoreAH, _buttonRestoreAP, _buttonRestoreI;
        private Image[] _buttonMinA, _buttonMinAH, _buttonMinAP, _buttonMinI;
        private Image[] _captionImageActive, _captionImageInactive;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the Office2007Form class.
        /// </summary>
        public DotNetMagicForm()
        {
            // NAG processing
            NAG.NAG_Start();

            // Load images into each instance to avoid thread access issues 
            // it other instances are created inside a different UI thread.
            LoadImages();

            // Set the default visual style
            _style = VisualStyle.Office2007Blue;

            // By default there is no associated status strip control
            _statusStrip = null;
            _mergeStatusStrip = false;
            _regionWindowState = FormWindowState.Normal;

            // Create a status strip we can position for rendering
            _renderStrip = new StatusStrip();

            // By default, no button is being pressed down
            _buttonLength = 24;
            _pressedButtonIndex = -1;

            // Create the three caption buttons
            _buttons = new ChromeButton[]{ new ChromeButtonClose(_buttonCloseA, _buttonCloseAH, _buttonCloseAP, _buttonCloseI, _buttonTracking, _buttonPressed, _buttonTrackingS, _buttonPressedS),
                                           new ChromeButtonMax(_buttonMaxA, _buttonRestoreA, _buttonMaxAH, _buttonRestoreAH, _buttonMaxAP, _buttonRestoreAP, _buttonMaxI, _buttonRestoreI, _buttonTracking, _buttonPressed, _buttonTrackingS, _buttonPressedS),
                                           new ChromeButtonMin(_buttonMinA, _buttonRestoreA, _buttonMinAH, _buttonRestoreAH, _buttonMinAP, _buttonRestoreAP, _buttonMinI, _buttonRestoreI, _buttonTracking, _buttonPressed, _buttonTrackingS, _buttonPressedS) };

            // By default we apply the style to both the chrome and toolstrips
            ApplyStyleToChrome = true;
            ApplyStyleToToolStrips = true;
            Apply2007ClearType = true;
            ApplyMediaPlayerClearType = true;
        }

        /// <summary>
        /// Releases all resources used by the Control. 
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_renderStrip != null)
                {
                    _renderStrip.Dispose();
                    _renderStrip = null;
                }
            }

            base.Dispose(disposing);
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets and sets a value indicating if the Office 2007 renderer draw text using ClearType.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(true)]
        [Description("Should the Office 2007 renderer draw text using ClearType.")]
        public bool Apply2007ClearType
        {
            get { return _apply2007ClearType; }

            set
            {
                // Only interested in changes
                if (_apply2007ClearType != value)
                {
                    // Cache the new value
                    _apply2007ClearType = value;

                    // Apply new setting to chrome and toolstrips
                    UpdateChromeToolStrips();
                }
            }
        }

        /// <summary>
        /// Gets and sets a value indicating if the Media Player renderer draw text using ClearType.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(true)]
        [Description("Should the Media Player renderer draw text using ClearType.")]
        public bool ApplyMediaPlayerClearType
        {
            get { return _applyMediaPlayerClearType; }

            set
            {
                // Only interested in changes
                if (_applyMediaPlayerClearType != value)
                {
                    // Cache the new value
                    _applyMediaPlayerClearType = value;

                    // Apply new setting to chrome and toolstrips
                    UpdateChromeToolStrips();
                }
            }
        }

        /// <summary>
        /// Gets and sets a value indicating if the Style should be used to determine window chrome.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(true)]
        [Description("Should the Style be applied to the window chrome.")]
        public bool ApplyStyleToChrome
        {
            get { return _applyStyleToChrome; }

            set
            {
                // Only interested in changes
                if (_applyStyleToChrome != value)
                {
                    // Cache the new value
                    _applyStyleToChrome = value;

                    // Apply new setting to chrome and toolstrips
                    UpdateChromeToolStrips();
                }
            }
        }

        /// <summary>
        /// Gets and sets a value indicating if the Style should be used to render ToolStrips.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(true)]
        [Description("Should the Style be used to render ToolStrips.")]
        public bool ApplyStyleToToolStrips
        {
            get { return _applyStyleToToolStrips; }

            set
            {
                // Only interested in changes
                if (_applyStyleToToolStrips != value)
                {
                    // Cache the new value
                    _applyStyleToToolStrips = value;

                    // Apply new setting to chrome and toolstrips
                    UpdateChromeToolStrips();
                }
            }
        }

        /// <summary>
        /// Gets and sets the visual style to use for the Form.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(typeof(VisualStyle), "Office2007Blue")]
        [Description("VisualStyle that shoudl be applied to the Form.")]
        public virtual VisualStyle Style
        {
            get { return _style; }

            set
            {
                // Only interested in changes
                if (_style != value)
                {
                    // Cache the new values
                    _style = value;

                    // Apply new setting to chrome and toolstrips
                    UpdateChromeToolStrips();
                }
            }
        }
        #endregion

        #region Protected Override
        /// <summary>
        /// Raises the ControlAdded event.
        /// </summary>
        /// <param name="e">An EventArgs containing event data.</param>
        protected override void OnControlAdded(ControlEventArgs e)
        {
            // Is this the type of control we need to watch?
            if (e.Control is StatusStrip)
            {
                // Unhook from any existing cached reference
                UnmonitorStatusStrip();

                // Cache reference used during border drawing
                _statusStrip = e.Control as StatusStrip;

                // Monitor the visible and docking properties of the status strip
                _statusStrip.VisibleChanged += new EventHandler(OnStatusVisibleChanged);
                _statusStrip.DockChanged += new EventHandler(OnStatusDockChanged);

                // Force a recalculation of the client size
                ForceNonClientRecalc();
            }

            base.OnControlAdded(e);
        }

        /// <summary>
        /// Raises the ControlRemoved event.
        /// </summary>
        /// <param name="e">An EventArgs containing event data.</param>
        protected override void OnControlRemoved(ControlEventArgs e)
        {
            // Is the cached reference being removed?
            if (_statusStrip == e.Control)
            {
                // Unhook from status strip events
                UnmonitorStatusStrip();

                // Force a recalculation of the client size
                ForceNonClientRecalc();
            } 
            
            base.OnControlRemoved(e);
        }

        /// <summary>
        /// Raises the Resize event.
        /// </summary>
        /// <param name="e">An EventArgs containing event data.</param>
        protected override void OnResize(EventArgs e)
        {
            // Only update region if drawing custom chrome
            if (ApplyCustomChrome)
            {
                // Create a new region based on the current window size
                UpdateBorderRegion(CreateBorderRegion(new Rectangle(0, 0, Width, Height)));
            }

            base.OnResize(e);
        }

        /// <summary>
        /// Raises the Load event.
        /// </summary>
        /// <param name="e">An EventArgs containing event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            // Let base class perform standard actions such as calculating the 
            // correct initial size and position of the window when first shown
            base.OnLoad(e);

            // We only apply custom chrome/rendering when control is already created and positioned
            UpdateChromeToolStrips();

            // Force the border region and border sizing to be recalculated
            ForceNonClientRecalc();
        }
        #endregion

        #region Protected WindowChromePaint
        /// <summary>
        /// Perform setup for custom chrome.
        /// </summary>
        protected override void WindowChromeStart()
        {
            // Create a new region based on the current window size
            UpdateBorderRegion(CreateBorderRegion(new Rectangle(0, 0, Width, Height)));
        }

        /// <summary>
        /// Perform cleanup when custom chrome ending.
        /// </summary>
        protected override void WindowChromeEnd()
        {
            // Remove use of a region for the form
            UpdateBorderRegion(null);
        }

        /// <summary>
        /// Perform hit testing.
        /// </summary>
        /// <param name="pt">Point in window coordinates.</param>
        /// <returns></returns>
        protected override IntPtr WindowChromeHitTest(Point pt)
        {
            // Ask the three buttons if they match the hit test
            for (int i = 0; i < _buttons.Length; i++)
            {
                // Only interested in visible buttons
                if (_buttons[i].Visible)
                {
                    // Is the point in the buttons area
                    if (_buttons[i].Bounds.Contains(pt))
                    {
                        // Return the hit test value for the button
                        return _buttons[i].HitTest;
                    }
                }
            }

            Padding borders = RealWindowBorders();
            int captionHeight = borders.Top;

            // Cache the size of the window borders to use for hit testing
            switch (FormBorderStyle)
            {
                case FormBorderStyle.None:
                case FormBorderStyle.Fixed3D:
                case FormBorderStyle.FixedDialog:
                case FormBorderStyle.FixedSingle:
                case FormBorderStyle.FixedToolWindow:
                    borders = Padding.Empty;
                    break;
            }

            // Restrict the top border to the same size as the left as we are using
            // the values for the size of the border hit testing for resizing the window
            // and not the size of the border for drawing purposes.
            if (borders.Top > borders.Left)
                borders.Top = borders.Left;

            // Is point over the left border?
            if (pt.X <= borders.Left)
            {
                if (pt.Y <= _htCorner)
                    return (IntPtr)HitTest.HTTOPLEFT;

                if (pt.Y >= (Height - _htCorner))
                    return (IntPtr)HitTest.HTBOTTOMLEFT;

                return (IntPtr)HitTest.HTLEFT;
            }

            // Is point over the right border?
            if (pt.X >= (Width - borders.Right))
            {
                if (pt.Y <= _htCorner)
                    return (IntPtr)HitTest.HTTOPRIGHT;

                if (pt.Y >= (Height - _htCorner))
                    return (IntPtr)HitTest.HTBOTTOMRIGHT;

                return (IntPtr)HitTest.HTRIGHT;
            }

            // Is point over the bottom border?
            if (pt.Y >= (Height - borders.Bottom))
            {
                if (pt.X <= _htCorner)
                    return (IntPtr)HitTest.HTBOTTOMLEFT;

                if (pt.X >= (Width - _htCorner))
                    return (IntPtr)HitTest.HTBOTTOMRIGHT;

                return (IntPtr)HitTest.HTBOTTOM;
            }

            // Is point over the top border?
            if (pt.Y <= borders.Top)
            {
                if (pt.X <= _htCorner)
                    return (IntPtr)HitTest.HTTOPLEFT;

                if (pt.X >= (Width - _htCorner))
                    return (IntPtr)HitTest.HTTOPRIGHT;

                return (IntPtr)HitTest.HTTOP;
            }

            // Only hit test the icon area if there is an icon being drawn
            if (ShouldDrawIcon())
            {
                // Get the rectangle that will contain the icon
                Rectangle bounds = new Rectangle(0, 0, Width, captionHeight);
                Rectangle iconRect = CalcIconRectangle(ref bounds);

                // Is the point over the icon?
                if (iconRect.Contains(pt))
                    return (IntPtr)HitTest.HTSYSMENU;
            }

            // Is point over the caption area?
            if (pt.Y <= captionHeight)
                return (IntPtr)HitTest.HTCAPTION;

            return base.WindowChromeHitTest(pt);
        }

        /// <summary>
        /// Perform painting of the window chrome.
        /// </summary>
        /// <param name="g">Graphics instance to use for drawing.</param>
        /// <param name="bounds">Bounds enclosing the window chrome.</param>
        protected override void WindowChromePaint(Graphics g, Rectangle bounds)
        {
            if ((GetWindowState() == FormWindowState.Maximized) &&
                (_regionWindowState != FormWindowState.Maximized))
            {
                // Update the region to reflect the maximized state
                UpdateBorderRegion(CreateBorderRegion(new Rectangle(0, 0, Width, Height)));
            }

            // Update the merging status
            _mergeStatusStrip = StatusStripMerging;

            // Get custom border sizing
            Padding borders = RealWindowBorders();

            // Get appropriate colors, image etc for the active state
            Image captionImage = (WindowActive ? _captionImageActive[_themeIndex] : _captionImageInactive[_themeIndex]);
            Color captionBorder = (WindowActive ? _captionBorderActive[_themeIndex] : _captionBorderInactive[_themeIndex]);
            Color captionText = (WindowActive ? _captionTextActive[_themeIndex] : _captionTextInactive[_themeIndex]);
            Color outsideBorder = (WindowActive ? _outsideBorderActive[_themeIndex] : _outsideBorderInactive[_themeIndex]);
            Color insideBorder = (WindowActive ? _insideBorderActive[_themeIndex] : _insideBorderInactive[_themeIndex]);
            Color middleBorder = (WindowActive ? _middleBorderActive[_themeIndex] : _middleBorderInactive[_themeIndex]);
            Color lastBorder = (WindowActive ? _insideBorderActive[_themeIndex] : _lastBorderInactive[_themeIndex]);

            // Update the buttons with the latest bounds
            int buttonsWidth = UpdateButtonStateAndPositions(bounds);

            // Only draw the border around the caption if not minimized
            if (GetWindowState() != FormWindowState.Minimized)
            {
                // Draw the border lines below the caption
                using (SolidBrush outsideBrush = new SolidBrush(outsideBorder),
                                  insideBrush = new SolidBrush(insideBorder),
                                  middleBrush = new SolidBrush(middleBorder),
                                  lastBrush = new SolidBrush(lastBorder))
                {
                    g.FillRectangle(outsideBrush, bounds.Left, bounds.Top + borders.Top, bounds.Width, bounds.Height - borders.Top);
                    g.FillRectangle(insideBrush, bounds.Left + 1, bounds.Top + borders.Top, bounds.Width - 2, bounds.Height - borders.Top - Math.Min(1, borders.Bottom));
                    g.FillRectangle(middleBrush, bounds.Left + 2, bounds.Top + borders.Top, bounds.Width - 4, bounds.Height - borders.Top - Math.Min(2, borders.Bottom));
                    g.FillRectangle(lastBrush, bounds.Left + 3, bounds.Top + borders.Top, bounds.Width - 6, bounds.Height - borders.Top - Math.Min(3, borders.Bottom));

                    // Do we overdraw some of the border to extend the status strip into the border?
                    if (_mergeStatusStrip)
                    {
                        // Do we have a status strip to try and merge?
                        if ((_statusStrip != null) && (_renderStrip != null))
                        {
                            // Is the status strip using the global renderer?
                            if (_statusStrip.RenderMode == ToolStripRenderMode.ManagerRenderMode)
                            {
                                // Find the size of the borders around the form
                                Padding realBorder = RealWindowBorders();

                                // Grab the global renderer to use for painting
                                ToolStripRenderer renderer = ToolStripManager.Renderer;

                                // Size the render strip to the apparent size when merged into borders
                                _renderStrip.Width = Width;
                                _renderStrip.Height = _statusStrip.Height + realBorder.Bottom;

                                // Find vertical start of the status strip
                                int y = _statusStrip.Top + realBorder.Top;

                                try
                                {
                                    // We need to transform downwards from drawing at 0,0 to actual required position
                                    g.TranslateTransform(0, y);

                                    // Use the tool strip renderer to draw the correct status strip border/background
                                    renderer.DrawToolStripBorder(new ToolStripRenderEventArgs(g, _renderStrip));
                                    renderer.DrawToolStripBackground(new ToolStripRenderEventArgs(g, _renderStrip));
                                }
                                finally
                                {
                                    // Make sure that even a crash in the renderer does not prevent the transform reversal
                                    g.TranslateTransform(0, -y);
                                }

                                // Draw the caption area border
                                using (GraphicsPath borderPath = CreateLowerPath(new Rectangle(bounds.Left, y, bounds.Width, _renderStrip.Height - 1)))
                                {
                                    using (Pen captionPen = new Pen(outsideBorder))
                                    {
                                        // Draw using a smoothed look
                                        using (UseAntiAlias uaa = new UseAntiAlias(g))
                                            g.DrawPath(captionPen, borderPath);
                                    }
                                }
                            }
                        }
                    }
                }

                // Draw the caption background
                switch (_themeIndex)
                {
                    case 3:
                    case 4:
                    case 5:
                        using(SolidBrush captionBrush = new SolidBrush(Color.FromArgb(99, 108, 135)))
                            g.FillRectangle(captionBrush, new Rectangle(bounds.X + 1, bounds.Y + 1, bounds.Width - 2, borders.Top));

                        MediaPlayerRenderer.DrawStatusBarBackground(g, VisualStyle.MediaPlayerBlue, new Rectangle(bounds.X, bounds.Y + 1, bounds.Width, borders.Top - 1), this, false, false); 
                        break;
                    default:
                        g.DrawImage(captionImage, new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, borders.Top),
                                                  new Rectangle(0, 0, 1, captionImage.Height),
                                                  GraphicsUnit.Pixel);
                        break;
                }

                // Draw the caption area border
                using (GraphicsPath borderPath = CreateUpperPath(new Rectangle(bounds.Left, bounds.Top, bounds.Width, borders.Top)))
                {
                    using (Pen captionPen = new Pen(outsideBorder))
                    {
                        // Draw using a smoothed look
                        using (UseAntiAlias uaa = new UseAntiAlias(g))
                            g.DrawPath(captionPen, borderPath);
                    }
                }
            }
            else
            {
                // Draw the entire area in the caption bitmap
                g.DrawImage(captionImage, bounds, new Rectangle(0, 0, 1, captionImage.Height), GraphicsUnit.Pixel);
            }

            // Reduce the bounds to the content part of the caption
            bounds.X = borders.Left;
            bounds.Y = borders.Left;
            bounds.Height = borders.Top - borders.Left - _captionBottomGap;
            bounds.Width -= borders.Horizontal;

            // Do we need to draw the icon at all?
            if (ShouldDrawIcon())
            {
                // Reduce the bounds by space allocated for the icon, and return icon drawing rect
                Rectangle iconRect = CalcIconRectangle(ref bounds);

                // Draw the actual icon an resize it into the provided space
                g.DrawIcon(Icon, iconRect);
            }

            // Reduce available space by that used up by buttons
            bounds.Width -= buttonsWidth;

            // If there is any window text to be shown?
            if (!string.IsNullOrEmpty(Text))
            {
                // If text is too big then use ellipses
                using (StringFormat sf = new StringFormat())
                {
                    sf.Trimming = StringTrimming.EllipsisCharacter;
                    sf.FormatFlags = StringFormatFlags.NoWrap;
                    sf.Alignment = (RightToLeft == RightToLeft.Yes ? StringAlignment.Far : StringAlignment.Near);
                    sf.LineAlignment = StringAlignment.Center;

                    // Convert the remaining bounds space into a RectangleF
                    RectangleF rectF = new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height);

                    // Get the correct color for the text
                    using (SolidBrush textBrush = new SolidBrush(captionText))
                        using (UseClearTypeGridFit clearType = new UseClearTypeGridFit(g))
                        {
                            // If we have plenty of room for caption text
                            if (rectF.Height > 16)
                            {
                                // Then use only 80% of it
                                float reduce = rectF.Height / 10;
                                rectF.Y = (int)(rectF.Y + reduce);
                                rectF.Height = (int)(rectF.Height - reduce * 2);
                            }

                            // Find font size needed for the pixel space available
                            float point = 72 / g.DpiY * (rectF.Height / 1.333f);

                            // No point having a font smaller than 3 points
                            if (point > 3)
                            {
                                using (Font font = new Font("Segoe UI", point, FontStyle.Regular))
                                    g.DrawString(Text, font, textBrush, rectF, sf);
                            }
                        }
                }
            }

            // Ask the buttons to draw themselves in appropriate state
            for (int i = 0; i < _buttons.Length; i++)
            {
                // Only draw the button if it is visible
                if (_buttons[i].Visible)
                    _buttons[i].Draw(g, GetWindowState(), WindowActive, _themeIndex);
            }
        }

        /// <summary>
        /// Perform non-client mouse movement processing.
        /// </summary>
        /// <param name="pt">Point in window coordinates.</param>
        protected override void WindowChromeNonClientMouseMove(Point pt)
        {
            bool changed = false;

            // Only interested if no button is currently pressed down
            if (_pressedButtonIndex == -1)
            {
                // Ask each button to process mouse movement
                for (int i = 0; i < _buttons.Length; i++)
                    changed |= _buttons[i].CalculateMouseMove(pt);
            }

            // Do we need to repaint to show a change in state?
            if (changed)
                InvalidateNonClient();
        }

        /// <summary>
        /// Perform non-client mouse movement processing.
        /// </summary>
        /// <param name="pt">Point in window coordinates.</param>
        /// <returns>True if the message is handled; otherwise false.</returns>
        protected override bool WindowChromeClientMouseMove(Point pt)
        {
            bool changed = false;
            bool handled = false;

            // Only interested if a button is current pressed down
            if (_pressedButtonIndex >= 0)
            {
                // Convert from client to window coorindates
                Padding borders = RealWindowBorders();
                pt.Y += borders.Top;
                pt.X += borders.Left;

                bool beforePressed = _buttons[_pressedButtonIndex].Pressed;
                bool afterPressed = _buttons[_pressedButtonIndex].Bounds.Contains(pt);

                // Is there a change in the pressed state?
                if (beforePressed != afterPressed)
                {
                    _buttons[_pressedButtonIndex].Pressed = afterPressed;
                    changed = true;
                }

                handled = true;
            }

            // Do we need to repaint to show a change in state?
            if (changed)
                InvalidateNonClient();
            
            return handled;
        }

        /// <summary>
        /// Process the left mouse down event.
        /// </summary>
        /// <param name="pt">Window coordinate of the mouse up.</param>
        /// <returns>True if event is processed; otherwise false.</returns>
        protected override bool WindowChromeLeftMouseDown(Point pt)
        {
            // Find button this point is inside
            for (int i = 0; i < _buttons.Length; i++)
            {
                // Only interested in visible buttons
                if (_buttons[i].Visible)
                {
                    // If the point is inside the button area
                    if (_buttons[i].Bounds.Contains(pt))
                    {
                        // Then tell the button it is being pressed
                        _buttons[i].Pressed = true;

                        // Remember this button for future events
                        _pressedButtonIndex = i;

                        // Capture all mouse input until the left button is released
                        Capture = true;

                        // Force a repaint to show the changed button state
                        InvalidateNonClient();

                        // Tell caller, we have processed the event
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Process the left mouse up event.
        /// </summary>
        /// <param name="pt">Window coordinate of the mouse up.</param>
        /// <returns>True if event is processed; otherwise false.</returns>
        protected override bool WindowChromeLeftMouseUp(Point pt)
        {
            // Are we tracking input for a button?
            if (_pressedButtonIndex >= 0)
            {
                int oldPressed = _pressedButtonIndex;

                // Convert from client to window coorindates
                Padding borders = RealWindowBorders();
                pt.Y += borders.Top;
                pt.X += borders.Left;

                // Tell button it is no longer pressed
                _buttons[_pressedButtonIndex].Pressed = false;

                // No longer need to capture mouse input
                Capture = false;

                // No longer tracking a pressed button
                _pressedButtonIndex = -1;

                // If the mouse is still over the button
                if (_buttons[oldPressed].Bounds.Contains(pt))
                {
                    // Ensure the point is not over any button
                    pt = new Point(-1, -1);

                    // Calculation will mark the button as not tracking
                    _buttons[oldPressed].CalculateMouseMove(pt);

                    // Execute the command associated with the button
                    SendSysCommand(_buttons[oldPressed].SysCommand(this));
                }
                else
                    _buttons[oldPressed].CalculateMouseMove(pt);

                // Force a repaint to show the changed button state
                if (!IsDisposed && !Disposing)
                    InvalidateNonClient();
                
                // Tell caller, we have processed the event
                return true;
            }

            // By default, we have not handled the mouse up event
            return false;
        }

        /// <summary>
        /// Perform mouse leave processing.
        /// </summary>
        protected override void WindowChromeMouseLeave()
        {
            // When left mouse down occurs over a button we capture mouse input for the window
            // and so the mouse leave occurs for the non-client area. But in that case we do
            // not want to process a mouse leave for the caption button. So ignore event.
            if (_pressedButtonIndex == -1)
            {
                bool changed = false;

                // Ask each button to process mouse leave
                for (int i = 0; i < _buttons.Length; i++)
                    changed |= _buttons[i].CalculateMouseLeave();

                // Do we need to repaint to show a change in state?
                if (changed)
                    InvalidateNonClient();
            }                
        }
        #endregion

        #region Implementation
        private void UpdateChromeToolStrips()
        {
            // Never apply chrome/toolstrips renderering until after control is created
            if (IsHandleCreated)
            {
                bool newApplyCustomChrome = false;

                // Apply the style to the form and renderer
                switch (_style)
                {
                    case VisualStyle.Plain:
                        if (_applyStyleToToolStrips)
                            ToolStripManager.Renderer = new PlainRenderer();
                        break;
                    case VisualStyle.IDE2005:
                        if (_applyStyleToToolStrips)
                        {
                            ProfessionalColorTable pct = new ProfessionalColorTable();
                            pct.UseSystemColors = true;
                            ToolStripManager.Renderer = new ToolStripProfessionalRenderer(pct);
                        }
                        break;
                    case VisualStyle.Office2003:
                        if (_applyStyleToToolStrips)
                        {
                            // On Vista the ProfessionalColorTable does not use Office 2003 colors, 
                            // so instead we to the same display as the system settings
                            if (Environment.OSVersion.Version.Major >= 6)
                            {
                                ProfessionalColorTable pct = new ProfessionalColorTable();
                                pct.UseSystemColors = true;
                                ToolStripManager.Renderer = new ToolStripProfessionalRenderer(pct);
                            }
                            else
                                ToolStripManager.RenderMode = ToolStripManagerRenderMode.Professional;
                        }
                        break;
                    case VisualStyle.Office2007Blue:
                        if (_applyStyleToToolStrips)
                            ToolStripManager.Renderer = new Office2007Renderer(Office2007Theme.Blue, Apply2007ClearType);
                        newApplyCustomChrome = ApplyStyleToChrome;
                        _themeIndex = 0;
                        break;
                    case VisualStyle.Office2007Silver:
                        if (_applyStyleToToolStrips)
                            ToolStripManager.Renderer = new Office2007Renderer(Office2007Theme.Silver, Apply2007ClearType);
                        newApplyCustomChrome = ApplyStyleToChrome;
                        _themeIndex = 1;
                        break;
                    case VisualStyle.Office2007Black:
                        if (_applyStyleToToolStrips)
                            ToolStripManager.Renderer = new Office2007Renderer(Office2007Theme.Black, Apply2007ClearType);
                        newApplyCustomChrome = ApplyStyleToChrome;
                        _themeIndex = 2;
                        break;
                    case VisualStyle.MediaPlayerBlue:
                        if (_applyStyleToToolStrips)
                            ToolStripManager.Renderer = new MediaPlayerRenderer(MediaPlayerTheme.Blue, ApplyMediaPlayerClearType);
                        newApplyCustomChrome = ApplyStyleToChrome;
                        _themeIndex = 3;
                        break;
                    case VisualStyle.MediaPlayerOrange:
                        if (_applyStyleToToolStrips)
                            ToolStripManager.Renderer = new MediaPlayerRenderer(MediaPlayerTheme.Orange, ApplyMediaPlayerClearType);
                        newApplyCustomChrome = ApplyStyleToChrome;
                        _themeIndex = 4;
                        break;
                    case VisualStyle.MediaPlayerPurple:
                        if (_applyStyleToToolStrips)
                            ToolStripManager.Renderer = new MediaPlayerRenderer(MediaPlayerTheme.Purple, ApplyMediaPlayerClearType);
                        newApplyCustomChrome = ApplyStyleToChrome;
                        _themeIndex = 5;
                        break;
                }

                // Do we need to apply a change in custom chrome handling?
                if (newApplyCustomChrome != ApplyCustomChrome)
                {
                    // Use new custom chrome setting
                    ApplyCustomChrome = newApplyCustomChrome;
                }

                // Must repaint to reflect the change in visual style
                if (Created && !IsDisposed && !Disposing)
                    InvalidateNonClient();
            }
        }

        private GraphicsPath CreateAllPath(Rectangle bounds)
        {
            if (StatusStripMerging)
            {
                GraphicsPath borderPath = new GraphicsPath();
                borderPath.AddLine(bounds.Left, bounds.Bottom - 5, bounds.Left, bounds.Top + 4);
                borderPath.AddLine(bounds.Left, bounds.Top + 4, bounds.Left + 1, bounds.Top + 2);
                borderPath.AddLine(bounds.Left + 1, bounds.Top + 2, bounds.Left + 2, bounds.Top + 1);
                borderPath.AddLine(bounds.Left + 2, bounds.Top + 1, bounds.Left + 4, bounds.Top);
                borderPath.AddLine(bounds.Left + 4, bounds.Top, bounds.Right - 5, bounds.Top);
                borderPath.AddLine(bounds.Right - 5, bounds.Top, bounds.Right - 3, bounds.Top + 1);
                borderPath.AddLine(bounds.Right - 3, bounds.Top + 1, bounds.Right - 2, bounds.Top + 2);
                borderPath.AddLine(bounds.Right - 2, bounds.Top + 2, bounds.Right - 1, bounds.Top + 4);
                borderPath.AddLine(bounds.Right - 1, bounds.Top + 4, bounds.Right - 1, bounds.Bottom - 6);
                borderPath.AddLine(bounds.Right - 1, bounds.Bottom - 6, bounds.Right - 2, bounds.Bottom - 5);
                borderPath.AddLine(bounds.Right - 2, bounds.Bottom - 5, bounds.Right - 3, bounds.Bottom - 3);
                borderPath.AddLine(bounds.Right - 3, bounds.Bottom - 3, bounds.Right - 6, bounds.Bottom - 1);
                borderPath.AddLine(bounds.Right - 6, bounds.Bottom - 1, bounds.Left + 4, bounds.Bottom - 2);
                borderPath.AddLine(bounds.Left + 4, bounds.Bottom - 2, bounds.Left + 2, bounds.Bottom - 3);
                borderPath.AddLine(bounds.Left + 2, bounds.Bottom - 3, bounds.Left + 1, bounds.Bottom - 4);
                borderPath.AddLine(bounds.Left + 1, bounds.Bottom - 4, bounds.Left, bounds.Bottom - 6);
                borderPath.AddLine(bounds.Left, bounds.Bottom - 6, bounds.Left, bounds.Top + 4);
                return borderPath;
            }
            else
                return CreateUpperPath(bounds);
        }

        private GraphicsPath CreateUpperPath(Rectangle bounds)
        {
            GraphicsPath borderPath = new GraphicsPath();
            borderPath.AddLine(bounds.Left, bounds.Bottom - 1, bounds.Left, bounds.Top + 4);
            borderPath.AddLine(bounds.Left, bounds.Top + 4, bounds.Left + 1, bounds.Top + 2);
            borderPath.AddLine(bounds.Left + 1, bounds.Top + 2, bounds.Left + 2, bounds.Top + 1);
            borderPath.AddLine(bounds.Left + 2, bounds.Top + 1, bounds.Left + 4, bounds.Top);
            borderPath.AddLine(bounds.Left + 4, bounds.Top, bounds.Right - 5, bounds.Top);
            borderPath.AddLine(bounds.Right - 5, bounds.Top, bounds.Right - 3, bounds.Top + 1);
            borderPath.AddLine(bounds.Right - 3, bounds.Top + 1, bounds.Right - 2, bounds.Top + 2);
            borderPath.AddLine(bounds.Right - 2, bounds.Top + 2, bounds.Right - 1, bounds.Top + 4);
            borderPath.AddLine(bounds.Right - 1, bounds.Top + 4, bounds.Right - 1, bounds.Bottom - 1);
            return borderPath;
        }

        private GraphicsPath CreateLowerPath(Rectangle bounds)
        {
            GraphicsPath borderPath = new GraphicsPath();
            borderPath.AddLine(bounds.Left, bounds.Top, bounds.Left, bounds.Bottom - 4);
            borderPath.AddLine(bounds.Left, bounds.Bottom - 4, bounds.Left + 1, bounds.Bottom - 2);
            borderPath.AddLine(bounds.Left + 1, bounds.Bottom - 2, bounds.Left + 2, bounds.Bottom - 1);
            borderPath.AddLine(bounds.Left + 2, bounds.Bottom - 1, bounds.Left + 4, bounds.Bottom);
            borderPath.AddLine(bounds.Left + 4, bounds.Bottom, bounds.Right - 5, bounds.Bottom);
            borderPath.AddLine(bounds.Right - 5, bounds.Bottom, bounds.Right - 3, bounds.Bottom - 1);
            borderPath.AddLine(bounds.Right - 3, bounds.Bottom - 1, bounds.Right - 2, bounds.Bottom - 2);
            borderPath.AddLine(bounds.Right - 2, bounds.Bottom - 2, bounds.Right - 1, bounds.Bottom - 4);
            borderPath.AddLine(bounds.Right - 1, bounds.Bottom - 4, bounds.Right - 1, bounds.Top);
            return borderPath;
        }

        private Region CreateBorderRegion(Rectangle bounds)
        {
            // We do not need a region if there is no border
            if (FormBorderStyle == FormBorderStyle.None)
                return null;

            // Remember the window state used to create the region
            _regionWindowState = GetWindowState();

            if (GetWindowState() == FormWindowState.Maximized)
            {
                // Get the size of each window border
                Padding padding = RealWindowBorders();

                // Reduce the Bounds by the padding on all but the top
                Rectangle maximizedRect = new Rectangle(padding.Left, padding.Left,
                                                        Width - padding.Horizontal,
                                                        Height - padding.Left - padding.Bottom);

                return new Region(maximizedRect);
            }
            else
            {
                // Bounds must be one bigger on the right and bottom for path creation to work
                bounds.Width++;
                bounds.Height++;

                // Convert the border path to a region, appropriate for the Form.Region property
                return new Region(CreateAllPath(bounds));
            }
        }

        private void UpdateBorderRegion(Region newRegion)
        {
            // Cache the current region setting
            Region oldRegion = Region;

            // Use the new region
            Region = newRegion;

            // Cleanup old region gracefully
            if (oldRegion != null)
                oldRegion.Dispose();
        }

        private int UpdateButtonStateAndPositions(Rectangle bounds)
        {
            // Do we need to position the buttons in the opposite order?
            bool rtl = ((RightToLeft == RightToLeft.Yes) && RightToLeftLayout);

            // Calculate size of the buttons
            Padding borders = RealWindowBorders();
            _buttonLength = borders.Top - borders.Left - _captionBottomGap;

            // Find starting x and y positions
            int x = (rtl ? borders.Left : bounds.Right - borders.Right - _buttonLength);
            int y = borders.Top - _captionBottomGap - _buttonLength;

            // Space taken up with the buttons
            int buttonsWidth = 0;

            // Position the close/max/min buttons
            for (int i = 0; i < _buttons.Length; i++)
            {
                // Ask button to decide if it should be visible
                _buttons[i].CalculateVisibleState(this);

                // Only position the button if it is visible
                if (_buttons[i].Visible)
                {
                    // Define new position of the button
                    _buttons[i].Bounds = new Rectangle(x, y, _buttonLength, _buttonLength);

                    // Move to the next button position
                    if (rtl)
                        x += _buttonLength;
                    else
                        x -= _buttonLength;

                    buttonsWidth += _buttonLength;
                }
            }

            // If any buttons are showing
            if (buttonsWidth > 0)
            {
                // Add a spacing gap between buttons and text area
                buttonsWidth += _captionBottomGap;
            }

            return buttonsWidth;
        }

        private FormWindowState GetWindowState()
        {
            // Get the current window style (cannot use the 
            // WindowState property as it can be slightly out of date)
            uint style = User32.GetWindowLong(Handle, (int)GetWindowLongFlags.GWL_STYLE);

            if ((style & (uint)WindowStyles.WS_MINIMIZE) != 0)
                return FormWindowState.Minimized;
            else if ((style & (uint)WindowStyles.WS_MAXIMIZE) != 0)
                return FormWindowState.Maximized;
            else
                return FormWindowState.Normal;
        }

        private bool ShouldDrawIcon()
        {
            // If there is an Icon present and application wants to show it
            // (we never draw an icon if there is no control box)
            if (ShowIcon && (Icon != null) && ControlBox)
            {
                // Only some of the border styles allow for the presense of an icon
                switch (FormBorderStyle)
                {
                    case FormBorderStyle.FixedSingle:
                    case FormBorderStyle.Fixed3D:
                    case FormBorderStyle.Sizable:
                        return true;
                }
            }

            return false;
        }

        private Rectangle CalcIconRectangle(ref Rectangle bounds)
        {
            Rectangle iconRect;

            // Size of the icon comes from the system information
            Size iconSize = SystemInformation.SmallIconSize;

            // Find vertical offset to center the icon
            int y = ((bounds.Height - iconSize.Height) / 2) - 1;

            // How much space do we remove from the bounds?
            int space = iconSize.Width + _iconGap * 2;

            // Do we need to reverse the positioning for RTL settings?
            if ((RightToLeft == RightToLeft.Yes) && RightToLeftLayout)
            {
                // Position icon on the right side of the bounds area
                iconRect = new Rectangle(bounds.Right - iconSize.Width - _iconGap,
                                         bounds.Y + y,
                                         iconSize.Width,
                                         iconSize.Height);
            }
            else
            {
                // Position icon on the left side of the bounds area
                iconRect = new Rectangle(bounds.X + _iconGap, 
                                         bounds.Y + y,
                                         iconSize.Width,
                                         iconSize.Height);

                // Shift across by allocated space
                bounds.X += space;
            }

            // Reduce remaining space by that allocated for icon
            bounds.Width -= space;

            return iconRect;
        }

        private int GetMergedStatusHeight()
        {
            if ((_statusStrip != null) && _statusStrip.Visible &&
                (_statusStrip.Dock == DockStyle.Bottom))
                return _statusStrip.Height;
            else
                return 0;
        }

        private bool StatusStripMerging
        {
            get
            {
                return ((_statusStrip != null) && _statusStrip.Visible &&
                       (_statusStrip.Dock == DockStyle.Bottom));
            }
        }

        private void UnmonitorStatusStrip()
        {
            if (_statusStrip != null)
            {
                // Unhook event handlers
                _statusStrip.VisibleChanged += new EventHandler(OnStatusVisibleChanged);
                _statusStrip.DockChanged += new EventHandler(OnStatusDockChanged);
                _statusStrip = null;
            }
        }

        private void OnStatusDockChanged(object sender, EventArgs e)
        {
            if (_mergeStatusStrip != StatusStripMerging)
                ForceNonClientRecalc();
        }

        private void OnStatusVisibleChanged(object sender, EventArgs e)
        {
            if (_mergeStatusStrip != StatusStripMerging)
                ForceNonClientRecalc();
        }

        private void LoadImages()
        {
            // Load all the images used for drawing the caption and the min/max/close buttons in 
            // different states and in each of the three (Blue, Silver, Black) Office 2007 themes

            _buttonTracking = new Image[] { global::Crownwood.DotNetMagic.Properties.Resources.BlueButtonTracking,
                                            global::Crownwood.DotNetMagic.Properties.Resources.SilverButtonTracking,
                                            global::Crownwood.DotNetMagic.Properties.Resources.BlackButtonTracking,
                                            global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonTracking,
                                            global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonTracking,
                                            global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonTracking};

            _buttonPressed = new Image[] { global::Crownwood.DotNetMagic.Properties.Resources.BlueButtonPressed,
                                           global::Crownwood.DotNetMagic.Properties.Resources.SilverButtonPressed,
                                           global::Crownwood.DotNetMagic.Properties.Resources.BlackButtonPressed,
                                           global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonBluePressed,
                                           global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonOrangePressed,
                                           global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonPurplePressed };

            _buttonTrackingS = new Image[] { global::Crownwood.DotNetMagic.Properties.Resources.BlueButtonTrackingS,
                                             global::Crownwood.DotNetMagic.Properties.Resources.SilverButtonTrackingS,
                                             global::Crownwood.DotNetMagic.Properties.Resources.BlackButtonTrackingS,
                                             global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonTrackingS,
                                             global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonTrackingS,
                                             global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonTrackingS};

            _buttonPressedS = new Image[] { global::Crownwood.DotNetMagic.Properties.Resources.BlueButtonPressedS,
                                            global::Crownwood.DotNetMagic.Properties.Resources.SilverButtonPressedS,
                                            global::Crownwood.DotNetMagic.Properties.Resources.BlackButtonPressedS,
                                            global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonBluePressedS,
                                            global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonOrangePressedS,
                                            global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonPurplePressedS };

            _buttonCloseA = new Image[] { global::Crownwood.DotNetMagic.Properties.Resources.BlueButtonCloseA,
                                          global::Crownwood.DotNetMagic.Properties.Resources.SilverButtonCloseA,
                                          global::Crownwood.DotNetMagic.Properties.Resources.BlackButtonCloseA,
                                          global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonCloseA,
                                          global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonCloseA,
                                          global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonCloseA };

            _buttonCloseAH = new Image[] { global::Crownwood.DotNetMagic.Properties.Resources.BlueButtonCloseA,
                                           global::Crownwood.DotNetMagic.Properties.Resources.SilverButtonCloseA,
                                           global::Crownwood.DotNetMagic.Properties.Resources.BlackButtonCloseAH,
                                           global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonCloseA,
                                           global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonCloseA,
                                           global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonCloseA };

            _buttonCloseAP = new Image[] { global::Crownwood.DotNetMagic.Properties.Resources.BlueButtonCloseA,
                                           global::Crownwood.DotNetMagic.Properties.Resources.SilverButtonCloseA,
                                           global::Crownwood.DotNetMagic.Properties.Resources.BlackButtonCloseA,
                                           global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonCloseA,
                                           global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonCloseA,
                                           global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonCloseA};

            _buttonCloseI = new Image[] { global::Crownwood.DotNetMagic.Properties.Resources.BlueButtonCloseI,
                                          global::Crownwood.DotNetMagic.Properties.Resources.SilverButtonCloseI,
                                          global::Crownwood.DotNetMagic.Properties.Resources.BlackButtonCloseI,
                                          global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonCloseI,
                                          global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonCloseI,
                                          global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonCloseI};

            _buttonMaxA = new Image[] { global::Crownwood.DotNetMagic.Properties.Resources.BlueButtonMaxA,
                                        global::Crownwood.DotNetMagic.Properties.Resources.SilverButtonMaxA,
                                        global::Crownwood.DotNetMagic.Properties.Resources.BlackButtonMaxA,
                                        global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonMaxA,
                                        global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonMaxA,
                                        global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonMaxA};

            _buttonMaxAH = new Image[] { global::Crownwood.DotNetMagic.Properties.Resources.BlueButtonMaxA,
                                         global::Crownwood.DotNetMagic.Properties.Resources.SilverButtonMaxA,
                                         global::Crownwood.DotNetMagic.Properties.Resources.BlackButtonMaxAH,
                                         global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonMaxA,
                                         global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonMaxA,
                                         global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonMaxA};

            _buttonMaxAP = new Image[] { global::Crownwood.DotNetMagic.Properties.Resources.BlueButtonMaxA,
                                         global::Crownwood.DotNetMagic.Properties.Resources.SilverButtonMaxA,
                                         global::Crownwood.DotNetMagic.Properties.Resources.BlackButtonMaxA,
                                         global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonMaxA,
                                         global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonMaxA,
                                         global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonMaxA};

            _buttonMaxI = new Image[] { global::Crownwood.DotNetMagic.Properties.Resources.BlueButtonMaxI,
                                        global::Crownwood.DotNetMagic.Properties.Resources.SilverButtonMaxI,
                                        global::Crownwood.DotNetMagic.Properties.Resources.BlackButtonMaxI,
                                        global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonMaxI,
                                        global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonMaxI,
                                        global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonMaxI};

            _buttonRestoreA = new Image[] { global::Crownwood.DotNetMagic.Properties.Resources.BlueButtonRestoreA,
                                            global::Crownwood.DotNetMagic.Properties.Resources.SilverButtonRestoreA,
                                            global::Crownwood.DotNetMagic.Properties.Resources.BlackButtonRestoreA,
                                            global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonRestoreA,
                                            global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonRestoreA,
                                            global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonRestoreA};

            _buttonRestoreAH = new Image[] { global::Crownwood.DotNetMagic.Properties.Resources.BlueButtonRestoreA,
                                             global::Crownwood.DotNetMagic.Properties.Resources.SilverButtonRestoreA,
                                             global::Crownwood.DotNetMagic.Properties.Resources.BlackButtonRestoreAH,
                                             global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonRestoreA,
                                             global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonRestoreA,
                                             global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonRestoreA};

            _buttonRestoreAP = new Image[] { global::Crownwood.DotNetMagic.Properties.Resources.BlueButtonRestoreA,
                                             global::Crownwood.DotNetMagic.Properties.Resources.SilverButtonRestoreA,
                                             global::Crownwood.DotNetMagic.Properties.Resources.BlackButtonRestoreA,
                                             global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonRestoreA,
                                             global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonRestoreA,
                                             global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonRestoreA};

            _buttonRestoreI = new Image[] { global::Crownwood.DotNetMagic.Properties.Resources.BlueButtonRestoreI,
                                            global::Crownwood.DotNetMagic.Properties.Resources.SilverButtonRestoreI,
                                            global::Crownwood.DotNetMagic.Properties.Resources.BlackButtonRestoreI,
                                            global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonRestoreI,
                                            global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonRestoreI,
                                            global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonRestoreI};

            _buttonMinA = new Image[] { global::Crownwood.DotNetMagic.Properties.Resources.BlueButtonMinA,
                                        global::Crownwood.DotNetMagic.Properties.Resources.SilverButtonMinA,
                                        global::Crownwood.DotNetMagic.Properties.Resources.BlackButtonMinA,
                                        global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonMinA,
                                        global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonMinA,
                                        global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonMinA};

            _buttonMinAH = new Image[] { global::Crownwood.DotNetMagic.Properties.Resources.BlueButtonMinA,
                                         global::Crownwood.DotNetMagic.Properties.Resources.SilverButtonMinA,
                                         global::Crownwood.DotNetMagic.Properties.Resources.BlackButtonMinAH,
                                         global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonMinA,
                                         global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonMinA,
                                         global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonMinA};

            _buttonMinAP = new Image[] { global::Crownwood.DotNetMagic.Properties.Resources.BlueButtonMinA,
                                         global::Crownwood.DotNetMagic.Properties.Resources.SilverButtonMinA,
                                         global::Crownwood.DotNetMagic.Properties.Resources.BlackButtonMinA,
                                         global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonMinA,
                                         global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonMinA,
                                         global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonMinA};

            _buttonMinI = new Image[] { global::Crownwood.DotNetMagic.Properties.Resources.BlueButtonMinI,
                                        global::Crownwood.DotNetMagic.Properties.Resources.SilverButtonMinI,
                                        global::Crownwood.DotNetMagic.Properties.Resources.BlackButtonMinI,
                                        global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonMinI,
                                        global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonMinI,
                                        global::Crownwood.DotNetMagic.Properties.Resources.SparkleButtonMinI};

            _captionImageActive = new Image[] { global::Crownwood.DotNetMagic.Properties.Resources.BlueCaptionA,
                                                global::Crownwood.DotNetMagic.Properties.Resources.SilverCaptionA,
                                                global::Crownwood.DotNetMagic.Properties.Resources.BlackCaptionA,
                                                global::Crownwood.DotNetMagic.Properties.Resources.BlackCaptionA,
                                                global::Crownwood.DotNetMagic.Properties.Resources.BlackCaptionA,
                                                global::Crownwood.DotNetMagic.Properties.Resources.BlackCaptionA };

            _captionImageInactive = new Image[] { global::Crownwood.DotNetMagic.Properties.Resources.BlueCaptionI,
                                                  global::Crownwood.DotNetMagic.Properties.Resources.SilverCaptionI,
                                                  global::Crownwood.DotNetMagic.Properties.Resources.BlackCaptionI,
                                                  global::Crownwood.DotNetMagic.Properties.Resources.BlackCaptionI,
                                                  global::Crownwood.DotNetMagic.Properties.Resources.BlackCaptionI,
                                                  global::Crownwood.DotNetMagic.Properties.Resources.BlackCaptionI };
        }
        #endregion
    }
}
