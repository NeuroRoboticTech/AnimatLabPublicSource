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
using System.IO;
using System.Drawing;
using System.Resources;
using System.Reflection;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Represents a single StatusPanel inside the StatusBarControl.
	/// </summary>
    [ToolboxItem(false)]
    public class StatusPanel : Panel
    {
		// Instance fields
		private Image _image;
		private int _requestedWidth;
		private PanelBorder _panelBorder;
		private StringAlignment _alignment;
		private StatusBarPanelAutoSize _autoSizing;
        private StatusBarControl _parent;
		private bool _defaultPanelBorder;
		private bool _visible;

		/// <summary>
		/// Occurs when the RequestedWidth property changes.
		/// </summary>
		public event EventHandler RequestedWidthChanged;

		/// <summary>
		/// Occurs when the BorderStyle property changes.
		/// </summary>
		public event EventHandler PanelBorderChanged;

		/// <summary>
		/// Occurs when the Alignment property changes.
		/// </summary>
		public event EventHandler AlignmentChanged;

		/// <summary>
		/// Occurs when the Alignment property changes.
		/// </summary>
		public event EventHandler ImageChanged;

		/// <summary>
		/// Occurs when the AutoSizing property changes.
		/// </summary>
		public event EventHandler AutoSizingChanged;

		/// <summary>
		/// Occurs when the PaintBackground property changes.
		/// </summary>
		public event PaintEventHandler PaintBackground;

		/// <summary>
		/// Initializes a new instance of the StatusPanel class.
		/// </summary>
        public StatusPanel()
        {
            // Prevent drawing flicker by blitting from memory in WM_PAINT
            SetStyle(ControlStyles.OptimizedDoubleBuffer | 
				     ControlStyles.AllPaintingInWmPaint |
					 ControlStyles.UserPaint, true);

			// Default to being shown
			_visible = true;

			ResetImage();
			ResetAutoSizing();
			ResetAlignment();
			ResetRequestedWidth();
			ResetPanelBorder();
			
			// Using the defaulted panel border
			_defaultPanelBorder = true;
		}

		/// <summary>
		/// Gets and sets the Size property of the panel.
		/// </summary>
		[Browsable(false)]
		public new bool Visible
		{
			get { return _visible; }
			
			set 
			{ 
				if (_visible != value)
				{
					_visible = value;
					OnVisibleChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Gets and sets the Size property of the panel.
		/// </summary>
		[Browsable(false)]
		public new Size Size
		{
			get { return base.Size; }
			set { base.Size = value; }
		}

		/// <summary>
		/// Gets and sets the Location property of the panel.
		/// </summary>
		[Browsable(false)]
		public new Point Location
		{
			get { return base.Location; }
			set { base.Location = value; }
		}

		/// <summary>
		/// Gets and sets the Dock property of the panel.
		/// </summary>
		[Browsable(false)]
		public new DockStyle Dock
		{
			get { return base.Dock; }
			set { base.Dock = value; }
		}

		/// <summary>
		/// Gets and sets the RightToLeft property of the panel.
		/// </summary>
		[Browsable(false)]
		public new RightToLeft RightToLeft
		{
			get { return base.RightToLeft; }
			set { base.RightToLeft = value; }
		}

		/// <summary>
		/// Gets and sets the TabStop property of the panel.
		/// </summary>
		[Browsable(false)]
		public new bool TabStop
		{
			get { return base.TabStop; }
			set { base.TabStop = value; }
		}

		/// <summary>
		/// Gets and sets the Text property of the panel.
		/// </summary>
		[Browsable(true)]
		public new string Text
		{
			get { return base.Text; }
			
			set
			{ 
				if (base.Text != value)
				{
					base.Text = value;
					OnTextChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Gets and sets the TabStop property of the panel.
		/// </summary>
		[Browsable(false)]
		public new int TabIndex
		{
			get { return base.TabIndex; }
			set { base.TabIndex = value; }
		}

		/// <summary>
		/// Gets and sets the BorderStyle property of the panel.
		/// </summary>
		[Browsable(false)]
		public new BorderStyle BorderStyle
		{
			get { return base.BorderStyle; }
			set { base.BorderStyle = value; }
		}

		/// <summary>
		/// Gets and sets the DockPadding property of the panel.
		/// </summary>
		[Browsable(false)]
		public new DockPaddingEdges DockPadding
		{
			get { return base.DockPadding; }
		}

		/// <summary>
		/// Gets and sets the Anchor property of the panel.
		/// </summary>
		[Browsable(false)]
		public new AnchorStyles Anchor
		{
			get { return base.Anchor; }
			set { base.Anchor = value; }
		}

		/// <summary>
		/// Gets and sets the AutoScroll property of the panel.
		/// </summary>
		[Browsable(false)]
		public new bool AutoScroll
		{
			get { return base.AutoScroll; }
			set { base.AutoScroll = value; }
		}

		/// <summary>
		/// Gets and sets the AutoScrollMinSize property of the panel.
		/// </summary>
		[Browsable(false)]
		public new Size AutoScrollMinSize
		{
			get { return base.AutoScrollMinSize; }
			set { base.AutoScrollMinSize = value; }
		}

		/// <summary>
		/// Gets and sets the AutoScrollMargin property of the panel.
		/// </summary>
		[Browsable(false)]
		public new Size AutoScrollMargin
		{
			get { return base.AutoScrollMargin; }
			set { base.AutoScrollMargin = value; }
		}

		/// <summary>
		/// Gets and sets the requested width of the panel.
		/// </summary>
		[Category("Appearance")]
		[Description("Requested width of the panel.")]
		[DefaultValue(100)]
		public int RequestedWidth
		{
			get { return _requestedWidth; }
			
			set 
			{ 
				if (_requestedWidth != value)
				{
					_requestedWidth = value;
					OnRequestedWidthChanged();
				}
			}
		}

		/// <summary>
		/// Resets the RequestedWidth property to its default value.
		/// </summary>
		public void ResetRequestedWidth()
		{
			RequestedWidth = 100;
		}

		/// <summary>
		/// Gets and sets how the panel border is drawn.
		/// </summary>
		[Category("Appearance")]
		[Description("Determine how the panel border is drawn.")]
		[DefaultValue(typeof(PanelBorder), "Solid")]
		public PanelBorder PanelBorder
		{
			get { return _panelBorder; }
			
			set 
			{ 
				_panelBorder = value;
				_defaultPanelBorder = false;
				OnPanelBorderChanged();
			}
		}
		
		/// <summary>
		/// Resets the PanelBorder property to its default value.
		/// </summary>
		public void ResetPanelBorder()
		{
			PanelBorder = PanelBorder.Solid;
		}

		/// <summary>
		/// Gets and sets how the panel border is drawn.
		/// </summary>
		[Category("Appearance")]
		[Description("Determine how panel elements are drawn.")]
		[DefaultValue(typeof(StringAlignment), "Near")]
		public StringAlignment Alignment
		{
			get { return _alignment; }
			
			set 
			{ 
				if (_alignment != value)
				{
					_alignment = value;
					OnAlignmentChanged();
				}
			}
		}

		/// <summary>
		/// Resets the PanelBorder property to its default value.
		/// </summary>
		public void ResetAlignment()
		{
			Alignment = StringAlignment.Near;
		}

		/// <summary>
		/// Gets and sets the policy for sizing the panel width.
		/// </summary>
		[Category("Appearance")]
		[Description("Policy for sizing the panel width.")]
		[DefaultValue(typeof(StatusBarPanelAutoSize), "None")]
		public StatusBarPanelAutoSize AutoSizing
		{
			get { return _autoSizing; }
			
			set 
			{ 
				if (_autoSizing != value)
				{
					_autoSizing = value;
					OnAutoSizingChanged();
				}
			}
		}

		/// <summary>
		/// Resets the AutoSizing property to its default value.
		/// </summary>
		public void ResetAutoSizing()
		{
			AutoSizing = StatusBarPanelAutoSize.None;
		}

		/// <summary>
		/// Gets and sets the Image to draw inside panel.
		/// </summary>
		[Category("Appearance")]
		[Description("Image to draw inside panel.")]
		[DefaultValue(null)]
		public Image Image
		{
			get { return _image; }
			
			set 
			{ 
				if (_image != value)
				{
					_image = value;
					OnImageChanged();
				}
			}
		}

		/// <summary>
		/// Resets the Image property to its default value.
		/// </summary>
		public void ResetImage()
		{
			Image = null;
		}

		/// <summary>
		/// Raises the Resize event.
		/// </summary>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected override void OnResize(EventArgs e)
		{
			// Must redraw everthing when size changes
			Invalidate();
			base.OnResize(e);
		}

		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		/// <param name="pevent">A PaintEventArgs structure contained event data.</param>
		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			base.OnPaintBackground(pevent);

			// Raise any required events for painting			
			if (PaintBackground != null)
				PaintBackground(this, pevent);
		}

		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		/// <param name="e">A PaintEventArgs structure contained event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			// Let base class do the standard stuff first
			base.OnPaint(e);

			// Start by making all the space available
			int start = 0;
			int width = Width;

			switch(Alignment)
			{
			case StringAlignment.Near:
				{
					// Is there an image to draw
					if (Image != null)
					{
						// Find vertical offset
						int offset = (Height - Image.Height) / 2;
						
						// Draw the image its required size
						e.Graphics.DrawImage(Image, start, offset);
						
						// Reduce available space for text
						start += Image.Width;
						width -= Image.Width;
					}
					
					// Draw text in remainder of space (if there is any)
					if (width > 0)
						DrawTextIntoSpace(e.Graphics, start, width);
					break;
				}
			case StringAlignment.Center:
				{
					// Width of just the text
					int textWidth = FindTextWidth(e.Graphics);
					
					int totalWidth = textWidth;
					
					// Total width includes the image as well
					if (Image != null)
						totalWidth += Image.Width;
						
					// Start across by half the space
					int inset = (Width - totalWidth) / 2;
					
					// Cannot start before start of client edge
					if (inset < 0)
						inset = 0;
					
					if (Image != null)
					{
						// Find vertical offset
						int offset = (Height - Image.Height) / 2;
						
						// Draw the image its required size
						e.Graphics.DrawImage(Image, inset, offset);
						
						// Move across by image width
						inset += Image.Width;
					}
					
					// Always draw the text in the remainder of space
					DrawTextIntoSpace(e.Graphics, inset, Width - inset);
					break;
				}
			case StringAlignment.Far:
				{
					int textWidth = FindTextWidth(e.Graphics);

					// Draw text in remainder of space (if there is any)
					if ((textWidth > 0) && (width > 0))
					{
						// Is there an image as well as text
						if (Image != null)
						{
							// Ensure space for image as well
							int max = width - Image.Width;
							
							// Limit check the drawing width
							if (textWidth > max)
								textWidth = max;
						}
						else
						{
							// Limit check the drawing width
							if (textWidth > width)
								textWidth = width;
						}
					
						// Do the actual text drawing
						DrawTextIntoSpace(e.Graphics, width - textWidth, textWidth);
						
						// Reduce remaining space
						width -= textWidth;
					}
		
					if (Image != null)
					{
						// Find vertical offset
						int offset = (Height - Image.Height) / 2;
						
						// Draw the image its required size
						e.Graphics.DrawImage(Image, width - Image.Width, offset);
					}
					break;
				}
			}
		}

		/// <summary>
		/// Raises the RequestedWidthChanged event.
		/// </summary>
		protected virtual void OnRequestedWidthChanged()
		{
			if (RequestedWidthChanged != null)
				RequestedWidthChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the PanelBorderChanged event.
		/// </summary>
		protected virtual void OnPanelBorderChanged()
		{
			if (PanelBorderChanged != null)
				PanelBorderChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the AlignmentChanged event.
		/// </summary>
		protected virtual void OnAlignmentChanged()
		{
			if (AlignmentChanged != null)
				AlignmentChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the ImageChanged event.
		/// </summary>
		protected virtual void OnImageChanged()
		{
			if (ImageChanged != null)
				ImageChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the AutoSizingChanged event.
		/// </summary>
		protected virtual void OnAutoSizingChanged()
		{
			if (AutoSizingChanged != null)
				AutoSizingChanged(this, EventArgs.Empty);
		}

		internal bool DefaultPanelBorder
		{
			get { return _defaultPanelBorder; }
		}

        internal StatusBarControl StatusBarControl
        {
            get { return _parent; }
            set { _parent = value; }
        }

		private void DrawTextIntoSpace(Graphics g, int start, int width)
		{
			// Is there any text to draw?
			if ((Text != null) && (Text.Length > 0))
			{
				StringFormat sf = new StringFormat();
				sf.LineAlignment = StringAlignment.Center;
				sf.Trimming = StringTrimming.EllipsisCharacter;
				sf.FormatFlags = StringFormatFlags.NoWrap |
								 StringFormatFlags.FitBlackBox;

				// We only align to far end or near end (the center alignment is calculated
				// and performed by the paint code and so here we just need to paint near)
				if (Alignment == StringAlignment.Far)
					sf.Alignment = StringAlignment.Far;
				else
					sf.Alignment = StringAlignment.Near;

				// Create floating version of client rectangle
				RectangleF rectF = new RectangleF(start, 0, width, Height);

                StatusBarControl sbc = null;
                
                // We should be able to find the owning status bar control
                if ((Parent != null) && (Parent.Parent != null))
                    sbc = (StatusBarControl)Parent.Parent;

                Color textColor = ForeColor;

                // If using the default text color
                if ((textColor == DefaultForeColor) && (sbc != null))
                {
                    switch (sbc.Style)
                    {
                        case VisualStyle.Office2007Blue:
                        case VisualStyle.Office2007Silver:
                        case VisualStyle.Office2007Black:
                            textColor = Office2007ColorTable.StatusBarText(sbc.Style);
                            break;
                        case VisualStyle.MediaPlayerBlue:
                        case VisualStyle.MediaPlayerOrange:
                        case VisualStyle.MediaPlayerPurple:
                            textColor = MediaPlayerColorTable.StatusBarText(sbc.Style);
                            break;
                    }
                }

				// Draw text with correct alignment, color and font
                using (SolidBrush textBrush = new SolidBrush(textColor))
                {
                    bool drawn = false;

                    if (sbc != null)
                    {
                        switch (sbc.Style)
                        {
                            case VisualStyle.Office2007Blue:
                            case VisualStyle.Office2007Silver:
                            case VisualStyle.Office2007Black:
                                if ((StatusBarControl != null) && StatusBarControl.Apply2007ClearType)
                                {
                                    using (UseClearTypeGridFit clearType = new UseClearTypeGridFit(g))
                                        g.DrawString(Text, Font, textBrush, rectF, sf);
                                }
                                else
                                    g.DrawString(Text, Font, textBrush, rectF, sf);

                                drawn = true;
                                break;
                            case VisualStyle.MediaPlayerBlue:
                            case VisualStyle.MediaPlayerOrange:
                            case VisualStyle.MediaPlayerPurple:
                                if ((StatusBarControl != null) && StatusBarControl.ApplyMediaPlayerClearType)
                                {
                                    using (UseClearTypeGridFit clearType = new UseClearTypeGridFit(g))
                                        g.DrawString(Text, Font, textBrush, rectF, sf);
                                }
                                else
                                    g.DrawString(Text, Font, textBrush, rectF, sf);

                                drawn = true;
                                break;
                        }
                    }

                    if (!drawn)
                        g.DrawString(Text, Font, textBrush, rectF, sf);
                }

				sf.Dispose();
			}
		}
		
		private int FindTextWidth(Graphics g)
		{
			// Is there any text to draw?
			if ((Text != null) && (Text.Length > 0))
				return (int)g.MeasureString(Text, Font).Width + 1;
			else
				return 0;
		}

		internal int ContentsWidth
		{
			get
			{
				int totalWidth = 0;

				if (!IsDisposed)
				{
					// Add requested width of text
					using(Graphics g = this.CreateGraphics())
						totalWidth += FindTextWidth(g);

					// Add requested width of image
					if (Image != null)
						totalWidth += Image.Width;
				}
				
				return totalWidth;
			}
		}
	}

	/// <summary>
	/// Specifies the method used to draw panel border.
	/// </summary>
	public enum PanelBorder
	{
		/// <summary>
		/// Specifies that no border be drawn.
		/// </summary>
		None,

		/// <summary>
		/// Specifies that a single sunken border be drawn.
		/// </summary>
		Sunken,

		/// <summary>
		/// Specifies that a single raised border be drawn.
		/// </summary>
		Raised,

		/// <summary>
		/// Specifies that a single dotted border be drawn.
		/// </summary>
		Dotted,

		/// <summary>
		/// Specifies that a single dashed border be drawn.
		/// </summary>
		Dashed,

		/// <summary>
		/// Specifies that a single solid border be used.
		/// </summary>
		Solid
	}
}
