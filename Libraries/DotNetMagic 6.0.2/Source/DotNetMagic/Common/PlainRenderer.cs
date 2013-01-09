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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Crownwood.DotNetMagic.Common
{
    /// <summary>
    /// Draw ToolStrip using the Plain themed appearance.
    /// </summary>
    public class PlainRenderer : ToolStripSystemRenderer
    {
        #region OnRenderToolStripBackground
        /// <summary>
        /// Raises the RenderToolStripBackground event. 
        /// </summary>
        /// <param name="e">An ToolStripRenderEventArgs containing the event data.</param>
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip is MenuStrip)
                e.Graphics.FillRectangle(SystemBrushes.Control, e.ToolStrip.Bounds);
            else
                base.OnRenderToolStripBackground(e);
        }
        #endregion
    }
}
