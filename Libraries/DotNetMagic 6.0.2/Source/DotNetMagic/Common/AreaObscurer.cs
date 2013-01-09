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
using System.ComponentModel;
using Crownwood.DotNetMagic.Win32;

namespace Crownwood.DotNetMagic.Common
{
    /// <summary>
    /// Used to obscrure an area of the screen to hide form changes underneath.
    /// </summary>
    [ToolboxItem(false)]
    internal class AreaObscurer : IDisposable
    {
        #region ObscurerForm
        private class ObscurerForm : Form
        {
            #region Delegates
            public delegate void ShowFormDelegate(Rectangle screenRect);
            #endregion

            #region Identity
            public ObscurerForm()
            {
                // Prevent automatic positioning of the window
                StartPosition = FormStartPosition.Manual;

                // We do not want any window chrome
                FormBorderStyle = FormBorderStyle.None;

                // We do not want a taskbar entry for this temporary window
                ShowInTaskbar = false;
            }
            #endregion

            #region Public
            public void ShowForm(Rectangle screenRect)
            {
                if (InvokeRequired)
                    Invoke(new ShowFormDelegate(ShowForm), screenRect);
                else
                {
                    // Our initial position should overlay exactly the container
                    SetBounds(screenRect.X,
                              screenRect.Y,
                              screenRect.Width,
                              screenRect.Height);

                    // Show the window without activating it (i.e. do not take focus)
                    User32.ShowWindow(Handle, (short)Win32.ShowWindowStyles.SW_SHOWNOACTIVATE);
                }
            }
            #endregion

            #region Protected
            /// <summary>
            /// Raises the PaintBackground event.
            /// </summary>
            /// <param name="e">An PaintEventArgs containing the event data.</param>
            protected override void OnPaintBackground(PaintEventArgs e)
            {
                // We do nothing, so the area underneath shows through
            }

            /// <summary>
            /// Raises the Paint event.
            /// </summary>
            /// <param name="e">An PaintEventArgs containing the event data.</param>
            protected override void OnPaint(PaintEventArgs e)
            {
                // We do nothing, so the area underneath shows through
            }
            #endregion
        }
        #endregion

        #region Instance Fields
        private ObscurerForm _obscurer;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the ControlObscurer class.
        /// </summary>
        /// <param name="f">Form to obscure.</param>
        public AreaObscurer(Form f)
        {
            // We need a form to work with!
            if (f != null)
            {
                // Create and show the form
                _obscurer = new ObscurerForm();
                _obscurer.ShowForm(f.Bounds);
            }
        }

        /// <summary>
        /// Initialize a new instance of the ControlObscurer class.
        /// </summary>
        /// <param name="c">Control to obscure.</param>
        public AreaObscurer(Control c)
        {
            // We need a control to work with!
            if (c != null)
            {
                // Create and show the form
                _obscurer = new ObscurerForm();
                _obscurer.ShowForm(c.RectangleToScreen(c.ClientRectangle));
            }
        }

        /// <summary>
        /// Hide the obscurer from display.
        /// </summary>
        public void Dispose()
        {
            if (_obscurer != null)
            {
                _obscurer.Close();
                _obscurer = null;
            }
        }
        #endregion
    }
}
