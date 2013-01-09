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
using Crownwood.DotNetMagic.Forms;
using Microsoft.Win32;

namespace Crownwood.DotNetMagic.Common
{
    /// <summary>
    /// Draw ToolStrip using the Media Player themed appearance.
    /// </summary>
    public class MediaPlayerRenderer : ToolStripProfessionalRenderer
    {
        #region LinearItemColors
        private class LinearItemColors
        {
            #region Public Fields
            public Color Fill1;
            public Color Fill2;
            public Color Border;
            #endregion

            #region Identity
            public LinearItemColors()
            {
            }

            public LinearItemColors(Color fill1, Color fill2, Color border)
            {
                Fill1 = fill1;
                Fill2 = fill2;
                Border = border;
            }
            #endregion
        }

        private class GradientItemColors
        {
            #region Public Fields
            public Color Border;
            public Color Begin;
            public Color End;
            #endregion

            #region Identity
            public GradientItemColors()
            {
            }

            public GradientItemColors(Color border,
                                      Color begin,
                                      Color end)
            {
                Border = border;
                Begin = begin;
                End = end;
            }
            #endregion
        }
        #endregion

        #region Static Fields
        private static readonly int _gripOffset = 1;
        private static readonly int _gripSquare = 2;
        private static readonly int _gripSize = 3;
        private static readonly int _gripMove = 4;
        private static readonly int _gripLines = 3;
        private static readonly int _marginInset = 2;
        private static readonly int _checkInset = 1;
        private static readonly int _separatorInset = 24;
        private static readonly float _cutContextMenu = 0f;
        private static readonly float _cutMenuItemBack = 1.2f;
        private static readonly float _cutToolItemMenu = 1.0f;
        private static readonly Color _tmsBack = Color.FromArgb(99, 108, 135);
        private static readonly Blend _statusStripBlend;
        private static readonly Color _statusStripLight = Color.FromArgb(99, 108, 135);
        private static readonly Color _statusStripDark = Color.Black;
        private static readonly Color _menuItemText = Color.Black;
        private static readonly Color _menuStripText = Color.White;
        private static readonly Color _statusStripText = Color.White;
        private static readonly Color _toolStripText = Color.White;
        private static readonly Color _disabled = Color.FromArgb(167, 167, 167);
        private static readonly Color _glassColorTopD = Color.FromArgb(164, Color.White);
        private static readonly Color _glassColorBottomD = Color.FromArgb(64, Color.White);
        private static readonly LinearItemColors _disabledLinearItem = new LinearItemColors(Color.FromArgb(128, 220, 220, 220), Color.FromArgb(128, 190, 190, 190), Color.FromArgb(128, 172, 172, 172));
        private static readonly GradientItemColors _disabledGradientItem = new GradientItemColors(Color.FromArgb(212, 212, 212), Color.FromArgb(235, 235, 235), Color.FromArgb(235, 235, 235));
        private static readonly ColorMatrix _matrixDisabled = new ColorMatrix(new float[][] { new float[] { 0.3f, 0.3f, 0.3f, 0, 0 }, new float[] { 0.59f, 0.59f, 0.59f, 0, 0 }, new float[] { 0.11f, 0.11f, 0.11f, 0, 0 }, new float[] { 0, 0, 0, 1, 0 }, new float[] { 0, 0, 0, -0.6f, 1 } });
        private static readonly Image _contextMenuChecked = Properties.Resources.SparkleGrayChecked;
        private static readonly Image _contextMenuIndeterminate = Properties.Resources.SparkleGrayIndeterminate;
        private static LinearItemColors[] _linearItem = new LinearItemColors[3];
        private static GradientItemColors[] _gradientItem = new GradientItemColors[] { null, null, null };
        private static GradientItemColors[] _gradientTracking = new GradientItemColors[] { null, null, null };
        private static GradientItemColors[] _gradientPressed = new GradientItemColors[] { null, null, null };
        private static GradientItemColors[] _gradientChecked = new GradientItemColors[] { null, null, null };
        private static GradientItemColors[] _gradientCheckedTracking = new GradientItemColors[] { null, null, null };
        private static Font _menuToolFont;
        private static Font _statusFont;
        #endregion

        #region Instance Fields
        private int _themeIndex;
        private bool _applyClearType;
        #endregion

        #region Identity
        static MediaPlayerRenderer()
        {
            // One time creation of the blend for the status strip gradient brush
            _statusStripBlend = new Blend();
            _statusStripBlend.Factors = new float[] { 0.0f, 0.0f, 0.0f, 1.0f };
            _statusStripBlend.Positions = new float[] { 0.0f, 0.33f, 0.33f, 1.0f };

            UpdateCacheAll();
            DefineFonts();

            // We need to notice when system color settings change
            SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(OnUserPreferenceChanged);
        }

        /// <summary>
        /// Initialize a new instance of the MediaPlayerRenderer class.
        /// </summary>
        /// <param name="theme">Initial theme.</param>
        /// <param name="applyClearType">Should clear type drawing of text be allowed.</param>
        public MediaPlayerRenderer(MediaPlayerTheme theme, bool applyClearType)
            : base(new MediaPlayerColorTable(theme))
        {
            _themeIndex = (int)theme;
            _applyClearType = applyClearType;
        }
        #endregion

        #region OnRenderArrow
        /// <summary>
        /// Raises the RenderArrow event. 
        /// </summary>
        /// <param name="e">An ToolStripArrowRenderEventArgs containing the event data.</param>
        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            // Cannot paint a zero sized area
            if ((e.ArrowRectangle.Width > 0) &&
                (e.ArrowRectangle.Height > 0))
            {
                // Create a path that is used to fill the arrow
                using (GraphicsPath arrowPath = CreateArrowPath(e.Item,
                                                                e.ArrowRectangle,
                                                                e.Direction))
                {
                    // Get the rectangle that encloses the arrow and expand slightly
                    // so that the gradient is always within the expanding bounds
                    RectangleF boundsF = arrowPath.GetBounds();
                    boundsF.Inflate(1f, 1f);

                    // Default to disabled
                    Color color1 = _disabled;
                    Color color2 = _disabled;

                    // If not disabled then need to decide on actual colors
                    if (e.Item.Enabled)
                    {
                        // If the arrow is on a context menu
                        if ((e.Item.Owner is ContextMenuStrip) ||
                            (e.Item.Owner is ToolStripDropDownMenu) ||
                            (e.Item.OwnerItem is ToolStripOverflowButton))
                        {
                            if (((e.Item.Owner is ContextMenuStrip) || (e.Item.Owner is ToolStripDropDownMenu)) ||
                                ((e.Item.OwnerItem is ToolStripOverflowButton) && ((e.Item is ToolStripSplitButton) || (e.Item is ToolStripDropDownButton)) && (!e.Item.Selected || e.Item.Pressed)))
                                color1 = _menuItemText;
                            else
                                color1 = _toolStripText;

                            color2 = color1;
                        }
                        else
                        {
                            if ((e.Item.Owner is ToolStrip) ||
                                (e.Item.Owner is StatusStrip))
                            {
                                if (((e.Item is ToolStripSplitButton) || (e.Item is ToolStripDropDownButton)) && e.Item.Pressed)
                                    color1 = _menuItemText;
                                else
                                    color1 = _toolStripText;

                                color2 = color1;
                            }
                        }
                    }

                    float angle = 0;

                    // Use gradient angle to match the arrow direction
                    switch (e.Direction)
                    {
                        case ArrowDirection.Right:
                            angle = 0;
                            break;
                        case ArrowDirection.Left:
                            angle = 180f;
                            break;
                        case ArrowDirection.Down:
                            angle = 90f;
                            break;
                        case ArrowDirection.Up:
                            angle = 270f;
                            break;
                    }

                    // Draw the actual arrow using a gradient
                    using (LinearGradientBrush arrowBrush = new LinearGradientBrush(boundsF, color1, color2, angle))
                        e.Graphics.FillPath(arrowBrush, arrowPath);
                }
            }
        }
        #endregion

        #region OnRenderButtonBackground
        /// <summary>
        /// Raises the RenderButtonBackground event. 
        /// </summary>
        /// <param name="e">An ToolStripItemRenderEventArgs containing the event data.</param>
        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            // Cast to correct type
            ToolStripButton button = (ToolStripButton)e.Item;

            if (button.Selected || button.Pressed || button.Checked)
                RenderToolButtonBackground(e.Graphics, button, e.ToolStrip);
        }
        #endregion

        #region OnRenderDropDownButtonBackground
        /// <summary>
        /// Raises the RenderDropDownButtonBackground event. 
        /// </summary>
        /// <param name="e">An ToolStripItemRenderEventArgs containing the event data.</param>
        protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Selected || e.Item.Pressed)
                RenderToolDropButtonBackground(e.Graphics, e.Item, e.ToolStrip);
        }
        #endregion

        #region OnRenderItemCheck
        /// <summary>
        /// Raises the RenderItemCheck event. 
        /// </summary>
        /// <param name="e">An ToolStripItemImageRenderEventArgs containing the event data.</param>
        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            // Staring size of the checkbox is the image rectangle
            Rectangle checkBox = e.ImageRectangle;

            // Make the border of the check box 1 pixel bigger on all sides, as a minimum
            checkBox.Inflate(1, 1);

            // Can we extend upwards?
            if (checkBox.Top > _checkInset)
            {
                int diff = checkBox.Top - _checkInset;
                checkBox.Y -= diff;
                checkBox.Height += diff;
            }

            // Can we extend downwards?
            if (checkBox.Height <= (e.Item.Bounds.Height - (_checkInset * 2)))
            {
                int diff = e.Item.Bounds.Height - (_checkInset * 2) - checkBox.Height;
                checkBox.Height += diff;
            }

            // Drawing with anti aliasing to create smoother appearance
            using (UseAntiAlias aa = new UseAntiAlias(e.Graphics))
            {
                // Create border path for the check box
                using (GraphicsPath borderPath = CreateBorderPath(checkBox, _cutMenuItemBack))
                {
                    Color colorFill = ColorTable.CheckBackground;
                    Color colorBorder = ColorHelper.BlackenColor(ColorTable.CheckBackground, 0.89f, 0.88f, 0.98f);
                    if (!e.Item.Enabled)
                    {
                        colorFill = ColorHelper.ColorToBlackAndWhite(colorFill);
                        colorBorder = ColorHelper.ColorToBlackAndWhite(colorBorder);
                    }

                    // Fill the background in a solid color
                    using (SolidBrush fillBrush = new SolidBrush(colorFill))
                        e.Graphics.FillPath(fillBrush, borderPath);

                    // Draw the border around the check box
                    using (Pen borderPen = new Pen(colorBorder))
                        e.Graphics.DrawPath(borderPen, borderPath);

                    // If there is not an image, then we can draw the tick, square etc...
                    if (e.Item.Image == null)
                    {
                        CheckState checkState = CheckState.Unchecked;

                        // Extract the check state from the item
                        if (e.Item is ToolStripMenuItem)
                        {
                            ToolStripMenuItem item = (ToolStripMenuItem)e.Item;
                            checkState = item.CheckState;
                        }

                        // Decide what graphic to draw
                        Image drawImage = null;
                        switch (checkState)
                        {
                            case CheckState.Checked:
                                drawImage = _contextMenuChecked;
                                break;
                            case CheckState.Indeterminate:
                                drawImage = _contextMenuIndeterminate;
                                break;
                        }

                        if (drawImage != null)
                        {
                            // Draw the image centered in the available space
                            int xOffset = e.ImageRectangle.Width - drawImage.Width;
                            int yOffset = e.ImageRectangle.Height - drawImage.Height;
                            Rectangle drawRect = new Rectangle(e.ImageRectangle.X + xOffset,
                                                               e.ImageRectangle.Y + yOffset,
                                                               drawImage.Width,
                                                               drawImage.Height);

                            // Do we need to draw disabled?
                            if (e.Item.Enabled)
                                e.Graphics.DrawImage(drawImage, e.ImageRectangle);
                            else
                            {
                                using (ImageAttributes attribs = new ImageAttributes())
                                {
                                    attribs.SetColorMatrix(_matrixDisabled);

                                    // Draw using the disabled matrix to make it look disabled
                                    e.Graphics.DrawImage(drawImage, e.ImageRectangle,
                                                         0, 0, drawImage.Width, drawImage.Height,
                                                         GraphicsUnit.Pixel, attribs);
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region OnRenderItemText
        /// <summary>
        /// Raises the RenderItemText event. 
        /// </summary>
        /// <param name="e">An ToolStripItemTextRenderEventArgs containing the event data.</param>
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            if ((e.ToolStrip is ToolStrip) || 
                (e.ToolStrip is ContextMenuStrip) ||
                (e.ToolStrip is ToolStripDropDownMenu))
            {
                if (!e.Item.Enabled)
                    e.TextColor = _disabled;
                else
                {
                    if ((e.ToolStrip is MenuStrip) && !e.Item.Pressed)
                        e.TextColor = _menuStripText;
                    else if ((e.ToolStrip is MenuStrip) && (e.Item.Pressed || e.Item.Selected))
                        e.TextColor = _menuItemText;
                    else if ((e.ToolStrip is StatusStrip) && !e.Item.Pressed && !e.Item.Selected)
                        e.TextColor = _statusStripText;
                    else if ((e.ToolStrip is ContextMenuStrip) || (e.ToolStrip is ToolStripDropDownMenu))
                        e.TextColor = _menuItemText;
                    else if (((e.ToolStrip is ToolStripDropDownMenu) || (e.ToolStrip is ToolStripOverflow)) && !e.Item.Selected)
                        e.TextColor = _menuItemText;
                    else if ((e.ToolStrip is ToolStrip) && ((e.Item is ToolStripSplitButton) || (e.Item is ToolStripDropDownButton)) && e.Item.Pressed)
                        e.TextColor = _menuItemText;
                    else
                        e.TextColor = _toolStripText;
                }

                if (_applyClearType)
                {
                    // Ask the system to provide the type of text rendering required
                    using (UseClearTypeGridFit clearTypeGridFit = new UseClearTypeGridFit(e.Graphics))
                        base.OnRenderItemText(e);
                }
                else
                    base.OnRenderItemText(e);
            }
            else
                base.OnRenderItemText(e);
        }
        #endregion

        #region OnRenderItemImage
        /// <summary>
        /// Raises the RenderItemImage event. 
        /// </summary>
        /// <param name="e">An ToolStripItemImageRenderEventArgs containing the event data.</param>
        protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
        {
            // We only override the image drawing for context menus
            if ((e.ToolStrip is ContextMenuStrip) ||
                (e.ToolStrip is ToolStripDropDownMenu))
            {
                if (e.Image != null)
                {
                    if (e.Item.Enabled)
                        e.Graphics.DrawImage(e.Image, e.ImageRectangle);
                    else
                    {
                        using (ImageAttributes attribs = new ImageAttributes())
                        {
                            attribs.SetColorMatrix(_matrixDisabled);

                            // Draw using the disabled matrix to make it look disabled
                            e.Graphics.DrawImage(e.Image, e.ImageRectangle,
                                                 0, 0, e.Image.Width, e.Image.Height,
                                                 GraphicsUnit.Pixel, attribs);
                        }
                    }
                }
            }
            else
            {
                base.OnRenderItemImage(e);
            }
        }
        #endregion

        #region OnRenderMenuItemBackground
        /// <summary>
        /// Raises the RenderMenuItemBackground event. 
        /// </summary>
        /// <param name="e">An ToolStripItemRenderEventArgs containing the event data.</param>
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if ((e.ToolStrip is MenuStrip) ||
                (e.ToolStrip is ContextMenuStrip) ||
                (e.ToolStrip is ToolStripDropDownMenu))
            {
                if ((e.Item.Pressed) && (e.ToolStrip is MenuStrip))
                {
                    // Draw the menu/tool strip as a header for a context menu item
                    DrawContextMenuHeader(e.Graphics, e.Item);
                }
                else
                {
                    // We only draw a background if the item is selected and enabled
                    if (e.Item.Selected)
                    {
                        if (e.Item.Enabled)
                        {
                            if (e.ToolStrip is MenuStrip)
                                DrawGradientToolItem(e.Graphics, e.Item, _gradientTracking[_themeIndex]);
                            else
                                DrawLinearContextMenuItem(e.Graphics, e.Item, _linearItem[_themeIndex]);
                        }
                        else
                        {
                            // Get the mouse position in tool strip coordinates
                            Point mousePos = e.ToolStrip.PointToClient(Control.MousePosition);

                            // If the mouse is not in the item area, then draw disabled
                            if (!e.Item.Bounds.Contains(mousePos))
                                DrawLinearContextMenuItem(e.Graphics, e.Item, _disabledLinearItem);
                        }
                    }
                }
            }
            else
            {
                base.OnRenderMenuItemBackground(e);
            }
        }
        #endregion

        #region OnRenderSplitButtonBackground
        /// <summary>
        /// Raises the RenderSplitButtonBackground event. 
        /// </summary>
        /// <param name="e">An ToolStripItemRenderEventArgs containing the event data.</param>
        protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Selected || e.Item.Pressed)
            {
                // Cast to correct type
                ToolStripSplitButton splitButton = (ToolStripSplitButton)e.Item;

                // Draw the border and background
                RenderToolSplitButtonBackground(e.Graphics, splitButton, e.ToolStrip);

                // Get the rectangle that needs to show the arrow
                Rectangle arrowBounds = splitButton.DropDownButtonBounds;

                // Draw the arrow on top of the background
                OnRenderArrow(new ToolStripArrowRenderEventArgs(e.Graphics,
                                                                splitButton,
                                                                arrowBounds,
                                                                SystemColors.ControlText,
                                                                ArrowDirection.Down));
            }
            else
            {
                base.OnRenderSplitButtonBackground(e);
            }
        }
        #endregion

        #region OnRenderStatusStripSizingGrip
        /// <summary>
        /// Raises the RenderStatusStripSizingGrip event. 
        /// </summary>
        /// <param name="e">An ToolStripRenderEventArgs containing the event data.</param>
        protected override void OnRenderStatusStripSizingGrip(ToolStripRenderEventArgs e)
        {
            using (SolidBrush darkBrush = new SolidBrush(ColorTable.GripDark),
                              lightBrush = new SolidBrush(ColorTable.GripLight))
            {
                // Do we need to invert the drawing edge?
                bool rtl = (e.ToolStrip.RightToLeft == RightToLeft.Yes);

                // Find vertical position of the lowest grip line
                int y = e.AffectedBounds.Bottom - _gripSize * 2 + 1;

                // Draw three lines of grips
                for (int i = _gripLines; i >= 1; i--)
                {
                    // Find the rightmost grip position on the line
                    int x = (rtl ? e.AffectedBounds.Left + 1 :
                                   e.AffectedBounds.Right - _gripSize * 2 + 1);

                    // Draw grips from right to left on line
                    for (int j = 0; j < i; j++)
                    {
                        // Just the single grip glyph
                        DrawGripGlyph(e.Graphics, x, y, darkBrush, lightBrush);

                        // Move left to next grip position
                        x -= (rtl ? -_gripMove : _gripMove);
                    }

                    // Move upwards to next grip line
                    y -= _gripMove;
                }
            }
        }
        #endregion

        #region OnRenderToolStripContentPanelBackground
        /// <summary>
        /// Raises the RenderToolStripContentPanelBackground event. 
        /// </summary>
        /// <param name="e">An ToolStripContentPanelRenderEventArgs containing the event data.</param>
        protected override void OnRenderToolStripContentPanelBackground(ToolStripContentPanelRenderEventArgs e)
        {
            // Must call base class, otherwise the subsequent drawing does not appear!
            base.OnRenderToolStripContentPanelBackground(e);

            // Cannot paint a zero sized area
            if ((e.ToolStripContentPanel.Width > 0) &&
                (e.ToolStripContentPanel.Height > 0))
            {
                using (LinearGradientBrush backBrush = new LinearGradientBrush(e.ToolStripContentPanel.ClientRectangle,
                                                                               ColorTable.ToolStripContentPanelGradientEnd,
                                                                               ColorTable.ToolStripContentPanelGradientBegin,
                                                                               90f))
                {
                    e.Graphics.FillRectangle(backBrush, e.ToolStripContentPanel.ClientRectangle);
                }
            }
        }
        #endregion

        #region OnRenderSeparator
        /// <summary>
        /// Raises the RenderSeparator event. 
        /// </summary>
        /// <param name="e">An ToolStripSeparatorRenderEventArgs containing the event data.</param>
        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            if ((e.ToolStrip is ContextMenuStrip) ||
                (e.ToolStrip is ToolStripDropDownMenu))
            {
                // Create the light and dark line pens
                using (Pen lightPen = new Pen(ColorTable.ImageMarginGradientEnd),
                            darkPen = new Pen(ColorTable.ImageMarginGradientMiddle))
                {
                    DrawSeparator(e.Graphics, e.Vertical, e.Item.Bounds,
                                  lightPen, darkPen, _separatorInset,
                                  (e.ToolStrip.RightToLeft == RightToLeft.Yes));
                }
            }
            else if (e.ToolStrip is StatusStrip)
            {
                // Create the light and dark line pens
                using (Pen lightPen = new Pen(ColorTable.SeparatorLight),
                            darkPen = new Pen(ColorTable.SeparatorDark))
                {
                    DrawSeparator(e.Graphics, e.Vertical, e.Item.Bounds,
                                  lightPen, darkPen, 0, false);
                }
            }
            else
            {
                base.OnRenderSeparator(e);
            }
        }
        #endregion

        #region OnRenderToolStripBackground
        /// <summary>
        /// Raises the RenderToolStripBackground event. 
        /// </summary>
        /// <param name="e">An ToolStripRenderEventArgs containing the event data.</param>
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            // Make sure the font is current
            if (e.ToolStrip.Font != _menuToolFont)
                e.ToolStrip.Font = _menuToolFont;

            if ((e.ToolStrip is ContextMenuStrip) ||
                (e.ToolStrip is ToolStripDropDownMenu))
            {
                // Create border and clipping paths
                using (GraphicsPath borderPath = CreateBorderPath(e.AffectedBounds, _cutContextMenu),
                                      clipPath = CreateClipBorderPath(e.AffectedBounds, _cutContextMenu))
                {
                    // Clip all drawing to within the border path
                    using (UseClipping clipping = new UseClipping(e.Graphics, clipPath))
                    {
                        // Create the background brush
                        using (SolidBrush backBrush = new SolidBrush(ColorTable.ToolStripDropDownBackground))
                            e.Graphics.FillPath(backBrush, borderPath);
                    }
                }
            }
            else if (e.ToolStrip is StatusStrip)
            {
                // Create rectangle that covers the status strip area
                RectangleF backRect = new RectangleF(0, 0, e.ToolStrip.Width, e.ToolStrip.Height);

                Form owner = e.ToolStrip.FindForm();

                // Check if the status strip is inside a DotNetMagicForm and using the Office 2007 renderer, in 
                // which case we want to extend the drawing down into the border area for an integrated look
                if ((owner != null) &&
                    (owner is DotNetMagicForm) &&
                    e.ToolStrip.Visible &&
                    (e.ToolStrip.Dock == DockStyle.Bottom) &&
                    (e.ToolStrip.Bottom == owner.ClientSize.Height) &&
                    (e.ToolStrip.RenderMode == ToolStripRenderMode.ManagerRenderMode) &&
                    (ToolStripManager.Renderer is MediaPlayerRenderer))
                {
                    // Get the window borders
                    DotNetMagicForm dnmForm = (DotNetMagicForm)owner;

                    // Finally check that the actual form is using custom chrome
                    if (dnmForm.ApplyCustomChrome)
                    {
                        backRect.Height += dnmForm.RealWindowBorders().Bottom;
                        backRect.X -= 3;
                        backRect.Width += 6;
                    }
                }

                // Cannot paint a zero sized area
                if ((backRect.Width > 0) && (backRect.Height > 0))
                {
                    // Draw entire background
                    using (SolidBrush backBrush = new SolidBrush(ColorTable.MenuStripGradientBegin))
                        e.Graphics.FillRectangle(backBrush, backRect);

                    // Create path for the rounded bottom edges
                    using (GraphicsPath innerPath = new GraphicsPath())
                    {
                        RectangleF innerRectF = new RectangleF(backRect.X + 2, backRect.Y, backRect.Width - 4, backRect.Height - 2);

                        innerPath.AddLine(innerRectF.Right - 1, innerRectF.Top, innerRectF.Right - 1, innerRectF.Bottom - 7);
                        innerPath.AddArc(innerRectF.Right - 7, innerRectF.Bottom - 7, 6, 6, 0f, 90f);
                        innerPath.AddArc(innerRectF.Left, innerRectF.Bottom - 7, 6, 6, 90f, 90f);
                        innerPath.AddLine(innerRectF.Left, innerRectF.Bottom - 7, innerRectF.Left, innerRectF.Top);

                        // Make the last and first arc join up
                        innerPath.CloseFigure();

                        // Fill with a gradient brush
                        using (LinearGradientBrush innerBrush = new LinearGradientBrush(new Rectangle((int)backRect.X - 1, (int)backRect.Y - 1,
                                                                                                      (int)backRect.Width + 2, (int)backRect.Height + 1),
                                                                                        ColorTable.StatusStripGradientBegin,
                                                                                        ColorTable.StatusStripGradientEnd,
                                                                                        90f))
                        {
                            innerBrush.Blend = _statusStripBlend;

                            using (UseAntiAlias aa = new UseAntiAlias(e.Graphics))
                                e.Graphics.FillPath(innerBrush, innerPath);
                        }
                    }
                }
            }
            else
                base.OnRenderToolStripBackground(e);
        }
        #endregion

        #region OnRenderImageMargin
        /// <summary>
        /// Raises the RenderImageMargin event. 
        /// </summary>
        /// <param name="e">An ToolStripRenderEventArgs containing the event data.</param>
        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            if ((e.ToolStrip is ContextMenuStrip) ||
                (e.ToolStrip is ToolStripDropDownMenu))
            {
                // Start with the total margin area
                Rectangle marginRect = e.AffectedBounds;

                // Do we need to draw with separator on the opposite edge?
                bool rtl = (e.ToolStrip.RightToLeft == RightToLeft.Yes);

                marginRect.Y += _marginInset;
                marginRect.Height -= _marginInset * 2;

                // Reduce so it is inside the border
                if (!rtl)
                    marginRect.X += _marginInset;
                else
                    marginRect.X += _marginInset / 2;

                // Draw the entire margine area in a solid color
                using (SolidBrush backBrush = new SolidBrush(ColorTable.ImageMarginGradientBegin))
                    e.Graphics.FillRectangle(backBrush, marginRect);

                // Create the light and dark line pens
                using (Pen lightPen = new Pen(ColorTable.ImageMarginGradientEnd),
                            darkPen = new Pen(ColorTable.ImageMarginGradientMiddle))
                {
                    if (!rtl)
                    {
                        // Draw the light and dark lines on the right hand side
                        e.Graphics.DrawLine(lightPen, marginRect.Right, marginRect.Top, marginRect.Right, marginRect.Bottom);
                        e.Graphics.DrawLine(darkPen, marginRect.Right - 1, marginRect.Top, marginRect.Right - 1, marginRect.Bottom);
                    }
                    else
                    {
                        // Draw the light and dark lines on the left hand side
                        e.Graphics.DrawLine(lightPen, marginRect.Left - 1, marginRect.Top, marginRect.Left - 1, marginRect.Bottom);
                        e.Graphics.DrawLine(darkPen, marginRect.Left, marginRect.Top, marginRect.Left, marginRect.Bottom);
                    }
                }
            }
            else
            {
                base.OnRenderImageMargin(e);
            }
        }
        #endregion

        #region OnRenderToolStripBorder
        /// <summary>
        /// Raises the RenderToolStripBorder event. 
        /// </summary>
        /// <param name="e">An ToolStripRenderEventArgs containing the event data.</param>
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            if ((e.ToolStrip is ContextMenuStrip) ||
                (e.ToolStrip is ToolStripDropDownMenu))
            {
                // If there is a connected area to be drawn
                if (!e.ConnectedArea.IsEmpty)
                    using (SolidBrush excludeBrush = new SolidBrush(ColorTable.ToolStripDropDownBackground))
                        e.Graphics.FillRectangle(excludeBrush, e.ConnectedArea);

                // Create border and clipping paths
                using (GraphicsPath borderPath = CreateBorderPath(e.AffectedBounds, e.ConnectedArea, _cutContextMenu),
                                    insidePath = CreateInsideBorderPath(e.AffectedBounds, e.ConnectedArea, _cutContextMenu),
                                      clipPath = CreateClipBorderPath(e.AffectedBounds, e.ConnectedArea, _cutContextMenu))
                {
                    // Create the different pen colors we need
                    using (Pen borderPen = new Pen(ColorTable.MenuBorder),
                               insidePen = new Pen(ColorTable.ToolStripDropDownBackground))
                    {
                        // Clip all drawing to within the border path
                        using (UseClipping clipping = new UseClipping(e.Graphics, clipPath))
                        {
                            // Drawing with anti aliasing to create smoother appearance
                            using (UseAntiAlias aa = new UseAntiAlias(e.Graphics))
                            {
                                // Draw the inside area first
                                e.Graphics.DrawPath(insidePen, insidePath);

                                // Draw the border area second, so any overlapping gives it priority
                                e.Graphics.DrawPath(borderPen, borderPath);
                            }

                            // Draw the pixel at the bottom right of the context menu
                            e.Graphics.DrawLine(borderPen, e.AffectedBounds.Right, e.AffectedBounds.Bottom,
                                                           e.AffectedBounds.Right - 1, e.AffectedBounds.Bottom - 1);
                        }
                    }
                }
            }
            else if (!(e.ToolStrip is StatusStrip))
            {
                base.OnRenderToolStripBorder(e);
            }
        }
        #endregion

        #region Static
        /// <summary>
        /// Draw a button in the Office2007 style using provided state information.
        /// </summary>
        /// <param name="g">Graphics instance to use for drawing.</param>
        /// <param name="style">Drawing style.</param>
        /// <param name="direction">Vertical or Horizontal direction.</param>
        /// <param name="drawRect">Client rectangle for drawing.</param>
        /// <param name="state">Button state.</param>
        /// <param name="buttonStyle">Button style.</param>
        /// <param name="pushed">Is the button checked.</param>
        /// <param name="enabled">Is the button enabled.</param>
        public static void DrawButtonCommandBack(Graphics g,
                                                 VisualStyle style, 
                                                 LayoutDirection direction, 
												 Rectangle drawRect, 
                                                 ItemState state, 
												 ButtonStyle buttonStyle, 
                                                 bool pushed,
                                                 bool enabled)
        {
            // Conver the style to a theme index
            int themeIndex = ((int)style) - 6;

            // We only draw a background if the item is selected or being pressed
            if (enabled)
            {
                if ((buttonStyle == ButtonStyle.ToggleButton) && pushed)
                {
                    if (state == ItemState.Pressed)
                        DrawGradientItem(g, drawRect, _gradientPressed[themeIndex]);
                    else if (state == ItemState.HotTrack)
                        DrawGradientItem(g, drawRect, _gradientCheckedTracking[themeIndex]);
                    else
                        DrawGradientItem(g, drawRect, _gradientChecked[themeIndex]);
                }
                else
                {
                    if (state == ItemState.Pressed)
                        DrawGradientItem(g, drawRect, _gradientPressed[themeIndex]);
                    else if (state == ItemState.HotTrack)
                        DrawGradientItem(g, drawRect, _gradientTracking[themeIndex]);
                }
            }
        }

        /// <summary>
        /// Draw the background for a button in the provided style.
        /// </summary>
        /// <param name="g">Graphics instance to use for drawing.</param>
        /// <param name="style">Drawing style.</param>
        /// <param name="drawRect">Client rectangle for drawing.</param>
        public static void DrawButtonBackground(Graphics g,
                                                VisualStyle style,
                                                Rectangle drawRect)
        {
            using (SolidBrush backBrush = new SolidBrush(MediaPlayerColorTable.LightBackground(style)))
                g.FillRectangle(backBrush, drawRect);
        }

        /// <summary>
        /// Draw the border for the button in the provided style.
        /// </summary>
        /// <param name="g">Graphics instance to use for drawing.</param>
        /// <param name="style">Drawing style.</param>
        /// <param name="drawRect">Client rectangle for drawing.</param>
        public static void DrawButtonBorder(Graphics g,
                                            VisualStyle style,
                                            Rectangle drawRect)
        {
            // Conver the style to a theme index
            int themeIndex = ((int)style) - 6;
            
            // Draw the border for a normal state
            DrawSolidBorder(g, drawRect, _gradientItem[themeIndex]);
        }

        /// <summary>
        /// Draw the background for the status bar in the provided style.
        /// </summary>
        /// <param name="g">Graphics instance to use for drawing.</param>
        /// <param name="style">Drawing style.</param>
        /// <param name="drawRect">Client rectangle for drawing.</param>
        /// <param name="c">Reference to status bar control.</param>
        /// <param name="status">Showing in status are or caption area.</param>
        /// <param name="block">Display as a solid block.</param>
        public static void DrawStatusBarBackground(Graphics g,
                                                   VisualStyle style,
                                                   Rectangle drawRect,
                                                   Control c,
                                                   bool status,
                                                   bool block)
        {
            // Conver the style to a theme index
            int themeIndex = ((int)style) - 6;

            // Cannot paint a zero sized area
            if ((drawRect.Width > 0) && (drawRect.Height > 0))
            {
                Form owner = c.FindForm();

                // Check if the status strip is inside a DotNetMagicForm and using the Office 2007 renderer, in 
                // which case we want to extend the drawing down into the border area for an integrated look
                if ((owner != null) &&
                    (owner is DotNetMagicForm) &&
                    (c.Dock == DockStyle.Bottom) &&
                    (c.Bottom == owner.ClientSize.Height) &&
                    (ToolStripManager.Renderer is MediaPlayerRenderer))
                {
                    // Get the window borders
                    DotNetMagicForm dnmForm = (DotNetMagicForm)owner;

                    // Finally check that the actual form is using custom chrome
                    if (dnmForm.ApplyCustomChrome)
                    {
                        Padding realBorders = dnmForm.RealWindowBorders();
                        drawRect.Height += realBorders.Bottom;
                        drawRect.X -= realBorders.Left;
                        drawRect.Width += realBorders.Horizontal;
                    }
                }

                if (block)
                {
                    drawRect.X -= 5;
                    drawRect.Width += 10;
                }

                // Cannot paint a zero sized area
                if ((drawRect.Width > 0) && (drawRect.Height > 0))
                {
                    // Draw entire background
                    using (SolidBrush backBrush = new SolidBrush(_tmsBack))
                        g.FillRectangle(backBrush, drawRect);

                    // Create path for the rounded bottom edges
                    using (GraphicsPath innerPath = new GraphicsPath())
                    {
                        RectangleF innerRectF = new RectangleF(drawRect.X + 2, drawRect.Y, drawRect.Width - 4, drawRect.Height - 2);

                        if (status)
                        {
                            innerPath.AddLine(innerRectF.Right - 1, innerRectF.Top, innerRectF.Right - 1, innerRectF.Bottom - 7);
                            innerPath.AddArc(innerRectF.Right - 7, innerRectF.Bottom - 7, 6, 6, 0f, 90f);
                            innerPath.AddArc(innerRectF.Left, innerRectF.Bottom - 7, 6, 6, 90f, 90f);
                            innerPath.AddLine(innerRectF.Left, innerRectF.Bottom - 7, innerRectF.Left, innerRectF.Top);
                        }
                        else
                        {
                            innerPath.AddLine(innerRectF.Right - 1, innerRectF.Bottom - 1, innerRectF.Right - 1, innerRectF.Top + 1);
                            innerPath.AddArc(innerRectF.Right - 7, innerRectF.Top + 1, 6, 6, 0f, -90f);
                            innerPath.AddArc(innerRectF.Left, innerRectF.Top + 1, 6, 6, 270f, -90f);
                            innerPath.AddLine(innerRectF.Left, innerRectF.Top + 1, innerRectF.Left, innerRectF.Bottom - 1);
                        }

                        // Make the last and first arc join up
                        innerPath.CloseFigure();

                        // Fill with a gradient brush
                        using (LinearGradientBrush innerBrush = new LinearGradientBrush(new Rectangle((int)drawRect.X - 1, (int)drawRect.Y - 1,
                                                                                                      (int)drawRect.Width + 2, (int)drawRect.Height + 1),
                                                                                        (status ? _statusStripLight : _statusStripDark),
                                                                                        (status ? _statusStripDark : _statusStripLight),
                                                                                        90f))
                        {
                            innerBrush.Blend = _statusStripBlend;

                            using (UseAntiAlias aa = new UseAntiAlias(g))
                                g.FillPath(innerBrush, innerPath);
                        }
                    }
                }
            }
        }
        #endregion

        #region Implementation
        private static void DefineFonts()
        {
            // Release existing resources
            if (_menuToolFont != null) _menuToolFont.Dispose();
            if (_statusFont != null) _statusFont.Dispose();

            // Create new font using system information
            _menuToolFont = new Font("Segoe UI", SystemFonts.MenuFont.SizeInPoints, FontStyle.Regular);
            _statusFont = new Font("Segoe UI", SystemFonts.StatusFont.SizeInPoints, FontStyle.Regular);
        }

        private static void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            UpdateCacheAll();
            DefineFonts();
        }

        private static void UpdateCacheAll()
        {
            UpdateCache(new MediaPlayerColorTable(MediaPlayerTheme.Blue), 0);
            UpdateCache(new MediaPlayerColorTable(MediaPlayerTheme.Orange), 1);
            UpdateCache(new MediaPlayerColorTable(MediaPlayerTheme.Purple), 2);
        }

        private static void UpdateCache(ProfessionalColorTable colorTable, int themeIndex)
        {
            _linearItem[themeIndex] = new LinearItemColors(colorTable.ButtonSelectedGradientMiddle,
                                                           ColorHelper.BlackenColor(colorTable.ButtonSelectedGradientMiddle, 0.91f, 0.91f, 0.91f),
                                                           ColorHelper.BlackenColor(colorTable.ButtonSelectedGradientMiddle, 0.75f, 0.75f, 0.75f));

            _gradientItem[themeIndex] = new GradientItemColors(colorTable.CheckBackground,
                                                               colorTable.ButtonSelectedGradientBegin,
                                                               colorTable.ButtonSelectedGradientBegin);

            _gradientTracking[themeIndex] = new GradientItemColors(colorTable.ButtonSelectedBorder,
                                                                   colorTable.ButtonSelectedGradientBegin,
                                                                   colorTable.ButtonSelectedGradientEnd);

            _gradientPressed[themeIndex] = new GradientItemColors(colorTable.ButtonPressedBorder,
                                                                  colorTable.ButtonPressedGradientBegin,
                                                                  colorTable.ButtonPressedGradientEnd);

            _gradientChecked[themeIndex] = new GradientItemColors(colorTable.ButtonPressedBorder,
                                                                  colorTable.ButtonCheckedGradientBegin,
                                                                  colorTable.ButtonCheckedGradientEnd);

            _gradientCheckedTracking[themeIndex] = new GradientItemColors(colorTable.ButtonSelectedBorder,
                                                                          colorTable.ButtonPressedGradientBegin,
                                                                          colorTable.ButtonCheckedGradientEnd);
        }

        private void RenderToolButtonBackground(Graphics g,
                                                ToolStripButton button,
                                                ToolStrip toolstrip)
        {
            // We only draw a background if the item is selected or being pressed
            if (button.Enabled)
            {
                if (button.Checked)
                {
                    if (button.Pressed)
                        DrawGradientToolItem(g, button, _gradientPressed[_themeIndex]);
                    else if (button.Selected)
                        DrawGradientToolItem(g, button, _gradientCheckedTracking[_themeIndex]);
                    else
                        DrawGradientToolItem(g, button, _gradientChecked[_themeIndex]);
                }
                else
                {
                    if (button.Pressed)
                        DrawGradientToolItem(g, button, _gradientPressed[_themeIndex]);
                    else if (button.Selected)
                        DrawGradientToolItem(g, button, _gradientTracking[_themeIndex]);
                }
            }
            else
            {
                if (button.Selected)
                {
                    // Get the mouse position in tool strip coordinates
                    Point mousePos = toolstrip.PointToClient(Control.MousePosition);

                    // If the mouse is not in the item area, then draw disabled
                    if (!button.Bounds.Contains(mousePos))
                        DrawGradientToolItem(g, button, _disabledGradientItem);
                }
            }
        }

        private void RenderToolDropButtonBackground(Graphics g,
                                                    ToolStripItem item,
                                                    ToolStrip toolstrip)
        {
            // We only draw a background if the item is selected or being pressed
            if (item.Selected || item.Pressed)
            {
                if (item.Enabled)
                {
                    if (item.Pressed)
                        DrawContextMenuHeader(g, item);
                    else
                        DrawGradientToolItem(g, item, _gradientTracking[_themeIndex]);
                }
                else
                {
                    // Get the mouse position in tool strip coordinates
                    Point mousePos = toolstrip.PointToClient(Control.MousePosition);

                    // If the mouse is not in the item area, then draw disabled
                    if (!item.Bounds.Contains(mousePos))
                        DrawGradientToolItem(g, item, _disabledGradientItem);
                }
            }
        }

        private void DrawGradientToolSplitItem(Graphics g,
                                               ToolStripSplitButton splitButton,
                                               GradientItemColors colorsButton,
                                               GradientItemColors colorsDrop,
                                               GradientItemColors colorsSplit)
        {
            // Create entire area and just the drop button area rectangles
            Rectangle backRect = new Rectangle(Point.Empty, splitButton.Bounds.Size);
            Rectangle backRectDrop = splitButton.DropDownButtonBounds;

            // Cannot paint zero sized areas
            if ((backRect.Width > 0) && (backRectDrop.Width > 0) &&
                (backRect.Height > 0) && (backRectDrop.Height > 0))
            {
                // Area that is the normal button starts as everything
                Rectangle backRectButton = backRect;

                // The X offset to draw the split line
                int splitOffset;

                // Is the drop button on the right hand side of entire area?
                if (backRectDrop.X > 0)
                    splitOffset = backRectDrop.X;
                else
                    splitOffset = backRectDrop.Right - 1;

                // Create border path around the item
                using (GraphicsPath borderPath = CreateBorderPath(backRect, _cutMenuItemBack))
                {
                    // Draw the entire background area
                    DrawGradientBack(g, backRect, colorsButton);

                    // Draw the split line between the areas
                    using (Pen splitPen = new Pen(colorsSplit.Border))
                        g.DrawLine(splitPen, backRect.X + splitOffset, backRect.Top + 1, backRect.X + splitOffset, backRect.Bottom - 1);

                    // Draw the border of the entire item
                    DrawSolidBorder(g, backRect, colorsButton);
                }
            }
        }

        private void DrawContextMenuHeader(Graphics g, ToolStripItem item)
        {
            // Get the rectangle that is the items area
            Rectangle itemRect = new Rectangle(Point.Empty, item.Bounds.Size);

            // Create border and clipping paths
            using (GraphicsPath borderPath = CreateBorderPath(itemRect, _cutToolItemMenu),
                                insidePath = CreateInsideBorderPath(itemRect, _cutToolItemMenu),
                                  clipPath = CreateClipBorderPath(itemRect, _cutToolItemMenu))
            {
                // Clip all drawing to within the border path
                using (UseClipping clipping = new UseClipping(g, clipPath))
                {
                    // Draw the entire background area first
                    using (SolidBrush backBrush = new SolidBrush(ColorTable.ToolStripDropDownBackground))
                        g.FillPath(backBrush, borderPath);

                    // Draw the border
                    using (Pen borderPen = new Pen(ColorTable.MenuBorder))
                        g.DrawPath(borderPen, borderPath);
                }
            }
        }

        private void DrawGradientToolItem(Graphics g,
                                          ToolStripItem item,
                                          GradientItemColors colors)
        {
            // Perform drawing into the entire background of the item
            DrawGradientItem(g, new Rectangle(Point.Empty, item.Bounds.Size), colors);
        }

        private void RenderToolSplitButtonBackground(Graphics g,
                                                     ToolStripSplitButton splitButton,
                                                     ToolStrip toolstrip)
        {
            // We only draw a background if the item is selected or being pressed
            if (splitButton.Selected || splitButton.Pressed)
            {
                if (splitButton.Enabled)
                {
                    if (!splitButton.Pressed && splitButton.ButtonPressed)
                        DrawGradientToolSplitItem(g, splitButton, _gradientPressed[_themeIndex], _gradientPressed[_themeIndex], _gradientPressed[_themeIndex]);
                    else if (splitButton.Pressed && !splitButton.ButtonPressed)
                        DrawContextMenuHeader(g, splitButton);
                    else
                        DrawGradientToolSplitItem(g, splitButton, _gradientTracking[_themeIndex], _gradientTracking[_themeIndex], _gradientTracking[_themeIndex]);
                }
                else
                {
                    // Get the mouse position in tool strip coordinates
                    Point mousePos = toolstrip.PointToClient(Control.MousePosition);

                    // If the mouse is not in the item area, then draw disabled
                    if (!splitButton.Bounds.Contains(mousePos))
                        DrawGradientToolItem(g, splitButton, _disabledGradientItem);
                }
            }

        }

        private void DrawLinearContextMenuItem(Graphics g,
                                               ToolStripItem item,
                                               LinearItemColors colors)
        {
            // Do we need to draw with separator on the opposite edge?
            Rectangle backRect = new Rectangle(2, 0, item.Bounds.Width - 3, item.Bounds.Height);

            // Perform actual drawing into the background
            DrawLinearGradientItem(g, backRect, colors);
        }

        private static void DrawLinearGradientItem(Graphics g,
                                                   Rectangle backRect,
                                                   LinearItemColors colors)
        {
            // Cannot paint a zero sized area
            if ((backRect.Width > 0) && (backRect.Height > 0))
            {
                // Draw the background of the entire item
                DrawLinearGradientBack(g, backRect, colors);

                // Draw the border of the entire item
                DrawLinearGradientBorder(g, backRect, colors);
            }
        }

        private static void DrawLinearGradientBack(Graphics g,
                                                   Rectangle backRect,
                                                   LinearItemColors colors)
        {
            // Reduce rect draw drawing inside the border
            backRect.Inflate(-1, -1);

            using (LinearGradientBrush backBrush = new LinearGradientBrush(backRect, colors.Fill1, colors.Fill2, 90f))
                g.FillRectangle(backBrush, backRect);
        }

        private static void DrawLinearGradientBorder(Graphics g,
                                                     Rectangle backRect,
                                                     LinearItemColors colors)
        {
            // Drawing with anti aliasing to create smoother appearance
            using (UseAntiAlias aa = new UseAntiAlias(g))
                using (Pen borderPen = new Pen(colors.Border))
                    using (GraphicsPath borderPath = CreateBorderPath(backRect, _cutMenuItemBack))
                        g.DrawPath(borderPen, borderPath);
        }

        private void DrawGradientContextMenuItem(Graphics g,
                                                 ToolStripItem item,
                                                 GradientItemColors colors)
        {
            // Do we need to draw with separator on the opposite edge?
            Rectangle backRect = new Rectangle(2, 0, item.Bounds.Width - 3, item.Bounds.Height);

            // Perform actual drawing into the background
            DrawGradientItem(g, backRect, colors);
        }

        private static void DrawGradientItem(Graphics g,
                                             Rectangle backRect,
                                             GradientItemColors colors)
        {
            // Cannot paint a zero sized area
            if ((backRect.Width > 0) && (backRect.Height > 0))
            {
                // Draw the background of the entire item
                DrawGradientBack(g, backRect, colors);

                // Draw the border of the entire item
                DrawSolidBorder(g, backRect, colors);
            }
        }

        private static void DrawGradientBack(Graphics g,
                                             Rectangle backRect,
                                             GradientItemColors colors)
        {
            backRect.X++;
            backRect.Width -= 1;

            using (GraphicsPath backPath = CreateBorderPath(backRect, _cutMenuItemBack))
            {
                backRect.Width -= 1;
                backRect.Height -= 1;

                using (UseClipping clip = new UseClipping(g, backPath))
                {
                    // Draw the one pixel border around the area
                    LinearGradientBrush entireBrush = null;
                    if ((backRect.Width > 0) && (backRect.Height > 0))
                    {
                        // Create rectangle that covers the enter area
                        RectangleF gradientRect = new RectangleF(backRect.X - 1, backRect.Y - 1,
                                                                 backRect.Width + 2, backRect.Height + 2);

                        // Cannot draw a zero length rectangle
                        if ((gradientRect.Width > 0) && (gradientRect.Height > 0))
                            entireBrush = new LinearGradientBrush(gradientRect, colors.Begin, colors.End, 90f);
                    }

                    if (entireBrush != null)
                        g.FillRectangle(entireBrush, backRect);

                    // Reduce size on all but the upper edge
                    backRect.X += 1;
                    backRect.Width -= 2;
                    backRect.Height -= 1;

                    // Cannot draw a zero length rectangle
                    if ((backRect.Width > 0) && (backRect.Height > 0))
                    {
                        // Draw entire area in a solid color
                        SolidBrush totalBrush = new SolidBrush(colors.Begin);
                        int length = (int)(backRect.Height * 0.45f);

                        // Gradient rectangle is always a little bigger to prevent tiling at edges
                        RectangleF glassRect = new RectangleF(backRect.X, backRect.Y, backRect.Width, length);
                        RectangleF glassGradientRect = new RectangleF(glassRect.X - 1, glassRect.Y - 1, glassRect.Width + 2, glassRect.Height + 2);

                        // Cannot draw a zero length rectangle
                        LinearGradientBrush glassBrush = null;
                        if ((glassGradientRect.Width > 0) && (glassGradientRect.Height > 0))
                            glassBrush = new LinearGradientBrush(glassGradientRect, _glassColorTopD, _glassColorBottomD, 90f);

                        g.FillRectangle(totalBrush, backRect);
                        if (glassBrush != null)
                            g.FillRectangle(glassBrush, glassRect);

                        // Find the bottom area rectangle
                        RectangleF mainRect = new RectangleF(backRect.X, glassRect.Bottom + 1, backRect.Width, backRect.Height - glassRect.Height - 1);

                        // Find the box that encloses the ellipse (ellipses is sized using the factorX, factorY)
                        float mainRectWidth = (mainRect.Width * 3f);
                        float mainRectWidthOffset = (mainRectWidth - mainRect.Width) / 2;
                        float mainRectHeight = (mainRect.Height * 1.1f);
                        float mainRectHeightOffset;

                        // Find orientation specific ellsipe rectangle
                        mainRectHeightOffset = (mainRectHeight - mainRect.Height) / 2;

                        RectangleF doubleRect = new RectangleF(mainRect.X - mainRectWidthOffset,
                                                               mainRect.Y - mainRectHeightOffset,
                                                               mainRectWidth, mainRectHeight * 2);

                        // Cannot draw a path that contains a zero sized element
                        GraphicsPath path = null;
                        PathGradientBrush bottomBrush = null;
                        if ((doubleRect.Width > 0) && (doubleRect.Height > 0))
                        {
                            // We use a path to create an ellipse for the light effect in the bottom of the area
                            path = new GraphicsPath();
                            path.AddEllipse(doubleRect);

                            // Create a brush from the path
                            bottomBrush = new PathGradientBrush(path);
                            bottomBrush.CenterColor = colors.End;
                            bottomBrush.CenterPoint = new PointF(doubleRect.X + (doubleRect.Width / 2), doubleRect.Y + (doubleRect.Height / 2));
                            bottomBrush.SurroundColors = new Color[] { colors.Begin };
                        }

                        if (bottomBrush != null)
                            g.FillRectangle(bottomBrush, mainRect);
                    }
                }
            }
        }

        private static void DrawSolidBorder(Graphics g,
                                            Rectangle backRect,
                                            GradientItemColors colors)
        {
            // Drawing with anti aliasing to create smoother appearance
            using (UseAntiAlias aa = new UseAntiAlias(g))
            {
                Rectangle backRectI = backRect;
                backRectI.Inflate(1, 1);

                // Use solid color for the border
                using (Pen borderPen = new Pen(colors.Border))
                using (GraphicsPath borderPath = CreateBorderPath(backRect, _cutMenuItemBack))
                    g.DrawPath(borderPen, borderPath);
            }
        }

        private void DrawGripGlyph(Graphics g,
                                   int x,
                                   int y,
                                   Brush darkBrush,
                                   Brush lightBrush)
        {
            g.FillRectangle(lightBrush, x + _gripOffset, y + _gripOffset, _gripSquare, _gripSquare);
            g.FillRectangle(darkBrush, x, y, _gripSquare, _gripSquare);
        }

        private void DrawSeparator(Graphics g,
                                   bool vertical,
                                   Rectangle rect,
                                   Pen lightPen,
                                   Pen darkPen,
                                   int horizontalInset,
                                   bool rtl)
        {
            if (vertical)
            {
                int l = rect.Width / 2;
                int t = rect.Y;
                int b = rect.Bottom;

                // Draw vertical lines centered
                g.DrawLine(darkPen, l, t, l, b);
                g.DrawLine(lightPen, l + 1, t, l + 1, b);
            }
            else
            {
                int y = rect.Height / 2;
                int l = rect.X + (rtl ? 0 : horizontalInset);
                int r = rect.Right - (rtl ? horizontalInset : 0);

                // Draw horizontal lines centered
                g.DrawLine(darkPen, l, y, r, y);
                g.DrawLine(lightPen, l, y + 1, r, y + 1);
            }
        }

        private static GraphicsPath CreateBorderPath(Rectangle rect,
                                                     Rectangle exclude,
                                                     float cut)
        {
            // If nothing to exclude, then use quicker method
            if (exclude.IsEmpty)
                return CreateBorderPath(rect, cut);

            // Drawing lines requires we draw inside the area we want
            rect.Width--;
            rect.Height--;

            // Create an array of points to draw lines between
            List<PointF> pts = new List<PointF>();

            float l = rect.X;
            float t = rect.Y;
            float r = rect.Right;
            float b = rect.Bottom;
            float x0 = rect.X + cut;
            float x3 = rect.Right - cut;
            float y0 = rect.Y + cut;
            float y3 = rect.Bottom - cut;
            float cutBack = (cut == 0f ? 1 : cut);

            // Does the exclude intercept the top line
            if ((rect.Y >= exclude.Top) && (rect.Y <= exclude.Bottom))
            {
                float x1 = exclude.X - 1 - cut;
                float x2 = exclude.Right + cut;

                if (x0 <= x1)
                {
                    pts.Add(new PointF(x0, t));
                    pts.Add(new PointF(x1, t));
                    pts.Add(new PointF(x1 + cut, t - cutBack));
                }
                else
                {
                    x1 = exclude.X - 1;
                    pts.Add(new PointF(x1, t));
                    pts.Add(new PointF(x1, t - cutBack));
                }

                if (x3 > x2)
                {
                    pts.Add(new PointF(x2 - cut, t - cutBack));
                    pts.Add(new PointF(x2, t));
                    pts.Add(new PointF(x3, t));
                }
                else
                {
                    x2 = exclude.Right;
                    pts.Add(new PointF(x2, t - cutBack));
                    pts.Add(new PointF(x2, t));
                }
            }
            else
            {
                pts.Add(new PointF(x0, t));
                pts.Add(new PointF(x3, t));
            }

            pts.Add(new PointF(r, y0));
            pts.Add(new PointF(r, y3));
            pts.Add(new PointF(x3, b));
            pts.Add(new PointF(x0, b));
            pts.Add(new PointF(l, y3));
            pts.Add(new PointF(l, y0));

            // Create path using a simple set of lines that cut the corner
            GraphicsPath path = new GraphicsPath();

            // Add a line between each set of points
            for (int i = 1; i < pts.Count; i++)
                path.AddLine(pts[i - 1], pts[i]);

            // Add a line to join the last to the first
            path.AddLine(pts[pts.Count - 1], pts[0]);

            return path;
        }

        private static GraphicsPath CreateBorderPath(Rectangle rect, float cut)
        {
            // Drawing lines requires we draw inside the area we want
            rect.Width--;
            rect.Height--;

            // Create path using a simple set of lines that cut the corner
            GraphicsPath path = new GraphicsPath();
            path.AddLine(rect.Left + cut, rect.Top, rect.Right - cut, rect.Top);
            path.AddLine(rect.Right - cut, rect.Top, rect.Right, rect.Top + cut);
            path.AddLine(rect.Right, rect.Top + cut, rect.Right, rect.Bottom - cut);
            path.AddLine(rect.Right, rect.Bottom - cut, rect.Right - cut, rect.Bottom);
            path.AddLine(rect.Right - cut, rect.Bottom, rect.Left + cut, rect.Bottom);
            path.AddLine(rect.Left + cut, rect.Bottom, rect.Left, rect.Bottom - cut);
            path.AddLine(rect.Left, rect.Bottom - cut, rect.Left, rect.Top + cut);
            path.AddLine(rect.Left, rect.Top + cut, rect.Left + cut, rect.Top);
            return path;
        }

        private static GraphicsPath CreateInsideBorderPath(Rectangle rect, float cut)
        {
            // Adjust rectangle to be 1 pixel inside the original area
            rect.Inflate(-1, -1);

            // Now create a path based on this inner rectangle
            return CreateBorderPath(rect, cut);
        }

        private static GraphicsPath CreateInsideBorderPath(Rectangle rect,
                                                           Rectangle exclude,
                                                           float cut)
        {
            // Adjust rectangle to be 1 pixel inside the original area
            rect.Inflate(-1, -1);

            // Now create a path based on this inner rectangle
            return CreateBorderPath(rect, exclude, cut);
        }

        private static GraphicsPath CreateClipBorderPath(Rectangle rect, float cut)
        {
            // Clipping happens inside the rect, so make 1 wider and taller
            rect.Width++;
            rect.Height++;

            // Now create a path based on this inner rectangle
            return CreateBorderPath(rect, cut);
        }

        private static GraphicsPath CreateClipBorderPath(Rectangle rect,
                                                         Rectangle exclude,
                                                         float cut)
        {
            // Clipping happens inside the rect, so make 1 wider and taller
            rect.Width++;
            rect.Height++;

            // Now create a path based on this inner rectangle
            return CreateBorderPath(rect, exclude, cut);
        }

        private static GraphicsPath CreateArrowPath(ToolStripItem item,
                                                    Rectangle rect,
                                                    ArrowDirection direction)
        {
            int x, y;

            // Find the correct starting position, which depends on direction
            if ((direction == ArrowDirection.Left) ||
                (direction == ArrowDirection.Right))
            {
                x = rect.Right - (rect.Width - 4) / 2;
                y = rect.Y + rect.Height / 2;
            }
            else
            {
                x = rect.X + rect.Width / 2;
                y = rect.Bottom - (rect.Height - 3) / 2;

                // The drop down button is position 1 pixel incorrectly when in RTL
                if ((item is ToolStripDropDownButton) &&
                    (item.RightToLeft == RightToLeft.Yes))
                    x++;
            }

            // Create triangle using a series of lines
            GraphicsPath path = new GraphicsPath();

            switch (direction)
            {
                case ArrowDirection.Right:
                    path.AddLine(x, y, x - 4, y - 4);
                    path.AddLine(x - 4, y - 4, x - 4, y + 4);
                    path.AddLine(x - 4, y + 4, x, y);
                    break;
                case ArrowDirection.Left:
                    path.AddLine(x - 4, y, x, y - 4);
                    path.AddLine(x, y - 4, x, y + 4);
                    path.AddLine(x, y + 4, x - 4, y);
                    break;
                case ArrowDirection.Down:
                    path.AddLine(x + 3f, y - 3f, x - 2f, y - 3f);
                    path.AddLine(x - 2f, y - 3f, x, y);
                    path.AddLine(x, y, x + 3f, y - 3f);
                    break;
                case ArrowDirection.Up:
                    path.AddLine(x + 3f, y, x - 3f, y);
                    path.AddLine(x - 3f, y, x, y - 4f);
                    path.AddLine(x, y - 4f, x + 3f, y);
                    break;
            }

            return path;
        }

        private static GraphicsPath CreateTickPath(Rectangle rect)
        {
            // Get the center point of the rect
            int x = rect.X + rect.Width / 2;
            int y = rect.Y + rect.Height / 2;

            GraphicsPath path = new GraphicsPath();
            path.AddLine(x - 5, y - 1, x - 2, y + 4);
            path.AddLine(x - 2, y + 4, x + 3, y - 5);
            return path;
        }

        private static GraphicsPath CreateIndeterminatePath(Rectangle rect)
        {
            // Get the center point of the rect
            float x = (float)rect.X + ((float)rect.Width - 6) / 2;
            float y = (float)rect.Y + ((float)rect.Height - 6) / 2;

            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(x, y, 6f, 6f);
            return path;
        }
        #endregion
    }
}
