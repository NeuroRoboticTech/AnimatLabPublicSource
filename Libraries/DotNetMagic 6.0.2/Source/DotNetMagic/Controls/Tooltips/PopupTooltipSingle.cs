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
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Crownwood.DotNetMagic.Win32;
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Display a single line of tooltip text.
	/// </summary>
	public class PopupTooltipSingle : PopupBase
	{
		// Class fields
		private static int PADDING = 3;

		// Instance fields
		private Size _size;
		private int _textHeight;
		private string _toolText;
        private bool _fontProvided;
        private bool _apply2007ClearType;
        private bool _applyMediaPlayerClearType;
        private Color _borderColor;
        private Color _backColor2;

		/// <summary>
		/// Initialize a new instance of the PopupTooltipSingle class.
		/// </summary>
		public PopupTooltipSingle()
			: base(VisualStyle.Office2003)
		{
			InternalConstruct(new Font(SystemInformation.MenuFont, FontStyle.Regular), false);
		}

		/// <summary>
		/// Initialize a new instance of the PopupTooltip class.
		/// </summary>
		/// <param name="font">Font for drawing text.</param>
		public PopupTooltipSingle(Font font)
			: base(VisualStyle.Office2003)
		{
			InternalConstruct(font, true);
		}

		/// <summary>
		/// Initialize a new instance of the PopupTooltip class.
		/// </summary>
		/// <param name="style">Required visual style.</param>
		public PopupTooltipSingle(VisualStyle style)
			: base(style)
		{
			InternalConstruct(new Font(SystemInformation.MenuFont, FontStyle.Regular), false);
		}

		/// <summary>
		/// Initialize a new instance of the PopupTooltip class.
		/// </summary>
		/// <param name="style">Required visual style.</param>
		/// <param name="font">Font for drawing text.</param>
		public PopupTooltipSingle(VisualStyle style, Font font)
			: base(style)
		{
			InternalConstruct(font, true);
		}

        /// <summary>
        /// Gets and sets a value indicating if the Office 2007 style text should use ClearType.
        /// </summary>
        public bool Apply2007ClearType
        {
            get { return _apply2007ClearType; }

            set
            {
                _apply2007ClearType = value;
                Refresh();
            }
        }

        /// <summary>
        /// Gets and sets a value indicating if the Media Player style text should use ClearType.
        /// </summary>
        public bool ApplyMediaPlayerClearType
        {
            get { return _applyMediaPlayerClearType; }

            set
            {
                _applyMediaPlayerClearType = value;
                Refresh();
            }
        }

        /// <summary>
        /// Gets and sets the border color.
        /// </summary>
        public Color BorderColor
        {
            get { return _borderColor; }

            set
            {
                _borderColor = value;
                Refresh();
            }
        }

        /// <summary>
        /// Gets and sets the second background color.
        /// </summary>
        public Color BackColor2
        {
            get { return _backColor2; }

            set
            {
                _backColor2 = value;
                Refresh();
            }
        }

		/// <summary>
		/// Gets and sets the tool text to be shown.
		/// </summary>
		public string ToolText
		{
			get { return _toolText; }
			
			set 
			{ 
				// Remember new text
				_toolText = value; 

				// Recalculate the size and position of tooltip to match text
				CalculateSizePosition(Location);

				// Force repaint of contents
				Refresh();
			}
		}

		/// <summary>
		/// Gets and sets the text height.
		/// </summary>
		public int TextHeight
		{
			get { return _textHeight; }
			set { _textHeight = value; }
		}

		/// <summary>
		/// Make the popup visible but without taking the focus
		/// </summary>
		public virtual void ShowWithoutFocus(Point screenPos)
		{
			// Calculate window size and position
			CalculateSizePosition(screenPos);

			// Make sure the tooltip is visible
			ShowWithoutFocus();

			// Force repaint of contents
			Refresh();
		}

        /// <summary>
        /// Raises the PaintBackground event.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Rectangle backRect = ClientRectangle;
            backRect.Inflate(1, 1);

            using (LinearGradientBrush backBrush = new LinearGradientBrush(backRect, BackColor, BackColor2, 90f))
                e.Graphics.FillRectangle(backBrush, ClientRectangle);
        }

		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		/// <param name="e">Event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			// Get size of the total client area
			Rectangle clientRect = this.ClientRectangle;

			// Shrink on right and bottom to draw correctly
			clientRect.Width--;
			clientRect.Height--;

			// Draw border around whole control
            using(Pen borderColor = new Pen(_borderColor))
                e.Graphics.DrawRectangle(borderColor, clientRect);

			using(StringFormat drawFormat = new StringFormat())
			{
				drawFormat.Alignment = StringAlignment.Near;
				drawFormat.LineAlignment = StringAlignment.Center;
                drawFormat.HotkeyPrefix = HotkeyPrefix.Hide;

				// Draw the tool tip text in remaining space
                using (SolidBrush infoBrush = new SolidBrush(ForeColor))
                {
                    switch (Style)
                    {
                        case VisualStyle.Office2007Blue:
                        case VisualStyle.Office2007Silver:
                        case VisualStyle.Office2007Black:
                            if (!_fontProvided)
                            {
                                if (Apply2007ClearType)
                                {
                                    using (UseClearTypeGridFit clearType = new UseClearTypeGridFit(e.Graphics))
                                        e.Graphics.DrawString(ToolText, Font, infoBrush, new RectangleF(PADDING, 0, (float)(Width * 1.25), Height), drawFormat);
                                }
                                else
                                    e.Graphics.DrawString(ToolText, Font, infoBrush, new RectangleF(PADDING, 0, (float)(Width * 1.25), Height), drawFormat);
                            }
                            else
                                e.Graphics.DrawString(ToolText, Font, infoBrush, new RectangleF(PADDING, 0, (float)(Width * 1.25), Height), drawFormat);
                            break;
                        case VisualStyle.MediaPlayerBlue:
                        case VisualStyle.MediaPlayerOrange:
                        case VisualStyle.MediaPlayerPurple:
                            if (!_fontProvided)
                            {
                                if (ApplyMediaPlayerClearType)
                                {
                                    using (UseClearTypeGridFit clearType = new UseClearTypeGridFit(e.Graphics))
                                        e.Graphics.DrawString(ToolText, Font, infoBrush, new RectangleF(PADDING, 0, (float)(Width * 1.25), Height), drawFormat);
                                }
                                else
                                    e.Graphics.DrawString(ToolText, Font, infoBrush, new RectangleF(PADDING, 0, (float)(Width * 1.25), Height), drawFormat);
                            }
                            else
                                e.Graphics.DrawString(ToolText, Font, infoBrush, new RectangleF(PADDING, 0, (float)(Width * 1.25), Height), drawFormat);
                            break;
                        default:
                            e.Graphics.DrawString(ToolText, Font, infoBrush, new RectangleF(PADDING, 0, (float)(Width * 1.25), Height), drawFormat);
                            break;
                    }
                }
			}
		}

		/// <summary>
		/// Processes Windows messages.
		/// </summary>
		/// <param name="m">The Windows Message to process. </param>
		protected override void WndProc(ref Message m)
		{
			// We are a transparent window, let messages flow to whatever
			// is underneath us. Needed especially for the TreeControl because
			// we want clicking the mouse to select the node under the tooltip.
			if (m.Msg == (int)Win32.Msgs.WM_NCHITTEST)
			{
				// Allow actions to occur to window beneath us
				m.Result = (IntPtr)Win32.HitTest.HTTRANSPARENT;
			}
			else
				base.WndProc(ref m);
		}

		private void InternalConstruct(Font font, bool fontProvided)
		{
            _apply2007ClearType = true;
            _applyMediaPlayerClearType = true;

            // Was the font explicitly provided
            _fontProvided = fontProvided;

            switch (Style)
            {
                case VisualStyle.Office2007Blue:
                case VisualStyle.Office2007Silver:
                case VisualStyle.Office2007Black:
                    if (!_fontProvided)
                    {
                        BackColor = Office2007ColorTable.TooltipLight(Style);
                        BackColor2 = Office2007ColorTable.TooltipDark(Style);
                        ForeColor = Office2007ColorTable.TooltipTextColor(Style);
                        Font = new Font("Calibri", font.SizeInPoints, font.Style);
                        _borderColor = Office2007ColorTable.TooltipBorderColor(Style);
                    }
                    else
                    {
                        BackColor = SystemColors.Info;
                        BackColor2 = SystemColors.Info;
                        ForeColor = SystemColors.InfoText;
                        _borderColor = SystemColors.ControlDarkDark;
                    }
                    break;
                case VisualStyle.MediaPlayerBlue:
                case VisualStyle.MediaPlayerOrange:
                case VisualStyle.MediaPlayerPurple:
                    if (!_fontProvided)
                    {
                        BackColor = MediaPlayerColorTable.TooltipLight(Style);
                        BackColor2 = MediaPlayerColorTable.TooltipDark(Style);
                        ForeColor = MediaPlayerColorTable.TooltipTextColor(Style);
                        Font = new Font("Calibri", font.SizeInPoints, font.Style);
                        _borderColor = MediaPlayerColorTable.TooltipBorderColor(Style);
                    }
                    else
                    {
                        BackColor = SystemColors.Info;
                        BackColor2 = SystemColors.Info;
                        ForeColor = SystemColors.InfoText;
                        _borderColor = SystemColors.ControlDarkDark;
                    }
                    break;
                default:
                    BackColor = SystemColors.Info;
                    BackColor2 = SystemColors.Info;
                    ForeColor = SystemColors.InfoText;
                    _borderColor = SystemColors.ControlDarkDark;
                    break;
            }

    		Font = font;

			// By default we autocalculate the text height
			_textHeight = -1;
		}

		private void CalculateSizePosition(Point screenPos)
		{
			// Calculate size required to draw the tool text
			using(Graphics g = CreateGraphics())
			{
                using (StringFormat drawFormat = new StringFormat())
                {
                    drawFormat.Alignment = StringAlignment.Near;
                    drawFormat.LineAlignment = StringAlignment.Center;
                    drawFormat.HotkeyPrefix = HotkeyPrefix.Hide;

                    SizeF rawSize;

                    // Get accurate size of the tooltext
                    switch (Style)
                    {
                        case VisualStyle.Office2007Blue:
                        case VisualStyle.Office2007Silver:
                        case VisualStyle.Office2007Black:
                            if (!_fontProvided)
                            {
                                if (Apply2007ClearType)
                                {
                                    using (UseClearTypeGridFit clearType = new UseClearTypeGridFit(g))
                                        rawSize = g.MeasureString(ToolText, Font, int.MaxValue, drawFormat);
                                }
                                else
                                    rawSize = g.MeasureString(ToolText, Font, int.MaxValue, drawFormat);
                            }
                            else
                                rawSize = g.MeasureString(ToolText, Font, int.MaxValue, drawFormat);
                            break;
                        case VisualStyle.MediaPlayerBlue:
                        case VisualStyle.MediaPlayerOrange:
                        case VisualStyle.MediaPlayerPurple:
                            if (!_fontProvided)
                            {
                                if (ApplyMediaPlayerClearType)
                                {
                                    using (UseClearTypeGridFit clearType = new UseClearTypeGridFit(g))
                                        rawSize = g.MeasureString(ToolText, Font, int.MaxValue, drawFormat);
                                }
                                else
                                    rawSize = g.MeasureString(ToolText, Font, int.MaxValue, drawFormat);
                            }
                            else
                                rawSize = g.MeasureString(ToolText, Font, int.MaxValue, drawFormat);
                            break;
                        default:
                            rawSize = g.MeasureString(ToolText, Font, int.MaxValue, drawFormat);
                            break;
                    }

                    // Convert from floating point to integer
                    _size = new Size((int)rawSize.Width, (int)rawSize.Height);

                    // Do we override the height?
                    if (_textHeight != -1)
                    {
                        // Just exact height provided
                        _size.Height = _textHeight;
                    }
                    else
                    {
                        // Use height as the text plus some padding
                        _size.Height += PADDING * 2;
                    }

                    // Add borders around the text area to get the total size
                    Size total = new Size(_size.Width + PADDING * 3, _size.Height);

                    // Default to no space required for shadows
                    int shadow = 0;

                    // If a shadow is showing then gets its size
                    if (PopupShadow != null)
                        shadow = PopupShadow.ShadowLength;

                    Screen targetScreen = Screen.FromPoint(screenPos);

                    // Check that the position allows it to be shown
                    if ((screenPos.X + total.Width + shadow) > targetScreen.WorkingArea.Right)
                        screenPos.X = targetScreen.WorkingArea.Right - total.Width - shadow;

                    if ((screenPos.Y + total.Height + shadow) > targetScreen.WorkingArea.Bottom)
                        screenPos.Y = targetScreen.WorkingArea.Bottom - total.Height - shadow;

                    // Move the window without activating it (i.e. do not take focus)
                    User32.SetWindowPos(this.Handle,
                                        IntPtr.Zero,
                                        screenPos.X, screenPos.Y,
                                        total.Width, total.Height,
                                        (int)Win32.SetWindowPosFlags.SWP_NOZORDER +
                                        (int)Win32.SetWindowPosFlags.SWP_NOOWNERZORDER +
                                        (int)Win32.SetWindowPosFlags.SWP_NOACTIVATE);
                }
			}
		}
	}
}
