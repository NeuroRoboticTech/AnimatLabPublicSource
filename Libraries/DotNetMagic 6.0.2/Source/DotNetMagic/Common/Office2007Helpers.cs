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
using System.Windows.Forms;
using System.Collections.Generic;

namespace Crownwood.DotNetMagic.Common
{
    /// <summary>
    /// Specifies an Office 2007 theme.
    /// </summary>
    public enum Office2007Theme
    {
        /// <summary>Specified the Office 2007 standard Blue theme.</summary>
        Blue = 0x00,

        /// <summary>Specified the Office 2007 standard Silver theme.</summary>
        Silver = 0x01,

        /// <summary>Specified the Office 2007 standard Black theme.</summary>
        Black = 0x02
    }

    /// <summary>
    /// Specifies a Media Player theme.
    /// </summary>
    public enum MediaPlayerTheme
    {
        /// <summary>Specified the Media Player Blue theme.</summary>
        Blue = 0x00,

        /// <summary>Specified the Media Player Orange theme.</summary>
        Orange = 0x01,

        /// <summary>Specified the Media Player Purple theme.</summary>
        Purple = 0x02
    }

    /// <summary>
    /// Set the SmoothingMode=AntiAlias until instance disposed.
    /// </summary>
    public class UseAntiAlias : IDisposable
    {
        #region Instance Fields
        private Graphics _g;
        private SmoothingMode _old;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the UseAntiAlias class.
        /// </summary>
        /// <param name="g">Graphics instance.</param>
        public UseAntiAlias(Graphics g)
        {
            _g = g;
            _old = _g.SmoothingMode;
            _g.SmoothingMode = SmoothingMode.AntiAlias;
        }

        /// <summary>
        /// Revert the SmoothingMode back to original setting.
        /// </summary>
        public void Dispose()
        {
            _g.SmoothingMode = _old;
        }
        #endregion
    }

    /// <summary>
    /// Set the TextRenderingHint.ClearTypeGridFit until instance disposed.
    /// </summary>
    public class UseClearTypeGridFit : IDisposable
    {
        #region Instance Fields
        private Graphics _g;
        private TextRenderingHint _old;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the UseClearTypeGridFit class.
        /// </summary>
        /// <param name="g">Graphics instance.</param>
        public UseClearTypeGridFit(Graphics g)
        {
            _g = g;
            _old = _g.TextRenderingHint;
            _g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        }

        /// <summary>
        /// Revert the TextRenderingHint back to original setting.
        /// </summary>
        public void Dispose()
        {
            _g.TextRenderingHint = _old;
        }
        #endregion
    }

    /// <summary>
    /// Set the clipping region until instance disposed.
    /// </summary>
    public class UseClipping : IDisposable
    {
        #region Instance Fields
        private Graphics _g;
        private Region _old;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the UseClipping class.
        /// </summary>
        /// <param name="g">Graphics instance.</param>
        /// <param name="path">Clipping path.</param>
        public UseClipping(Graphics g, GraphicsPath path)
        {
            _g = g;
            _old = g.Clip;
            Region clip = _old.Clone();
            clip.Intersect(path);
            _g.Clip = clip;
        }

        /// <summary>
        /// Initialize a new instance of the UseClipping class.
        /// </summary>
        /// <param name="g">Graphics instance.</param>
        /// <param name="region">Clipping region.</param>
        public UseClipping(Graphics g, Region region)
        {
            _g = g;
            _old = g.Clip;
            Region clip = _old.Clone();
            clip.Intersect(region);
            _g.Clip = clip;
        }

        /// <summary>
        /// Revert clipping back to origina setting.
        /// </summary>
        public void Dispose()
        {
            _g.Clip = _old;
        }
        #endregion
    }

}
