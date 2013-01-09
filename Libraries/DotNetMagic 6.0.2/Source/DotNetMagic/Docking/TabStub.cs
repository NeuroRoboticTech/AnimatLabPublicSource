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
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using Microsoft.Win32;
using Crownwood.DotNetMagic.Common;
using Crownwood.DotNetMagic.Controls;

namespace Crownwood.DotNetMagic.Docking
{
	/// <summary>
	/// Mini container of tab pages.
	/// </summary>
    [ToolboxItem(false)]
    public class TabStub : UserControl
    {
		private class DrawTab
		{
			private int _index;
			private Rectangle _drawRect;
            private Crownwood.DotNetMagic.Controls.TabPage _tabPage;

			public DrawTab(Crownwood.DotNetMagic.Controls.TabPage tabPage, Rectangle drawRect, int index)
			{
				_index = index;
				_tabPage = tabPage;
				_drawRect = drawRect;
			}

			public Crownwood.DotNetMagic.Controls.TabPage TabPage	{ get { return _tabPage; } }
            public Rectangle DrawRect								{ get { return _drawRect; } }
			public int Index										{ get { return _index; } }
		}

        // Class constants
		private static int _imageGap = 3;
		private static int _imageGaps = 6;
        private static int _imageVector = 16;
        private static int _beginGap = 2;
        private static int _endGap = 8;
        private static int _sideGap = 3;
		private static int _hoverInterval = 500;

		// Instance fields
		private Edge _edge;
		private int _textMax;
		private int _hoverOver;
		private int _hoverItem;
		private int _selectedIndex;
    	private bool _defaultFont;
		private bool _defaultColor;
		private bool _stubsShowAll;
        private bool _apply2007ClearType;
        private bool _applyMediaPlayerClearType;
        private Color _backIDE;
        private Timer _hoverTimer;
        private TabPageCollection _tabPages;
		private WindowContentTabbed _wct;
		private ArrayList _drawTabs;
        private VisualStyle _style;
        private ColorDetails _colorDetails;

		/// <summary>
		/// Represents the method that will handle the PageClicked and PageOver events.
		/// </summary>
        public delegate void TabStubIndexHandler(TabStub sender, int pageIndex);

		/// <summary>
		/// Represents the method that will handle the PagesLeave events.
		/// </summary>
        public delegate void TabStubHandler(TabStub sender);

		/// <summary>
		/// Occurs when a page has been clicked.
		/// </summary>
        public event TabStubIndexHandler PageClicked;

		/// <summary>
		/// Occurs when mouse hovers over a page.
		/// </summary>
        public event TabStubIndexHandler PageOver;

		/// <summary>
		/// Occurs when mouse leaves the tab stub control.
		/// </summary>
        public event TabStubHandler PagesLeave;

		/// <summary>
		/// Initializes a new instance of the TabStub class.
		/// </summary>
		/// <param name="style">Visual style for drawing.</param>
		/// <param name="stubsShowAll">Initial stubs value.</param>
		public TabStub(VisualStyle style, bool stubsShowAll)
		{
			// Default state
			_wct = null;
			_style = style;
            _hoverOver = -1;
            _hoverItem = -1;
            _selectedIndex = -1;
            _defaultFont = true;
			_defaultColor = true;
            _apply2007ClearType = true;
            _applyMediaPlayerClearType = true;
            _stubsShowAll = stubsShowAll;
			_edge = Edge.None;
			_drawTabs = new ArrayList();
            _tabPages = new TabPageCollection();
            _colorDetails = new ColorDetails();
			_colorDetails.Style = _style;
            base.Font = new Font(SystemInformation.MenuFont, FontStyle.Regular);

            // Hookup to collection events
            _tabPages.Cleared += new CollectionClear(OnClearedPages);
            _tabPages.Inserted += new CollectionChange(OnInsertedPage);
            _tabPages.Removing += new CollectionChange(OnRemovingPage);
            _tabPages.Removed += new CollectionChange(OnRemovedPage);

            // Need notification when the MenuFont is changed
            Microsoft.Win32.SystemEvents.UserPreferenceChanged += new 
                UserPreferenceChangedEventHandler(OnPreferenceChanged);

			// Default default colors
			DefineBackColor(SystemColors.Control);

			// Create the Timer for handling hovering over items
			_hoverTimer = new Timer();
			_hoverTimer.Interval = _hoverInterval;
			_hoverTimer.Tick += new EventHandler(OnTimerExpire);
		}

		/// <summary>
		/// Releases all resources used by the group.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                // Unhook from all contents instances still present
                foreach (Crownwood.DotNetMagic.Controls.TabPage page in _tabPages)
                {
                    Content c = page.Tag as Content;
                    c.PropertyChanged -= new Content.PropChangeHandler(OnContentChanged);
                }                

				_hoverTimer.Stop();
				_hoverTimer.Dispose();
				
				// Color details has resources that need releasing
				_colorDetails.Dispose();
				
				// Remove notifications
                Microsoft.Win32.SystemEvents.UserPreferenceChanged -= new 
                    UserPreferenceChangedEventHandler(OnPreferenceChanged);
            }
            base.Dispose(disposing);
        }

		/// <summary>
		/// Gets the collection of tab pages.
		/// </summary>
        public TabPageCollection TabPages
        {
            get { return _tabPages; }
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
                ResizeControl();
                Recalculate();
                Invalidate();
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
                ResizeControl();
                Recalculate();
                Invalidate();
            }
        }

        /// <summary>
		/// Gets and sets the edge this stud is to draw against.
		/// </summary>
		public Edge Edging
		{
			get { return _edge; }

			set
			{
				if (value != _edge)
				{
					_edge = value;
					ResizeControl();
					Recalculate();
					Invalidate();
				}
		    }
		}

		/// <summary>
		/// Gets and sets the index of the selected tab page.
		/// </summary>
		public int SelectedIndex
		{
			get { return _selectedIndex; }

			set
			{
				if (value != _selectedIndex)
				{
					_selectedIndex = value;
					Recalculate();
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Gets and sets the font used to draw text.
		/// </summary>
        public override Font Font
        {
            get { return base.Font; }

            set
            {
				if (value != null)
				{
					if (value != base.Font)
					{
						using(Font testFont = new Font(SystemInformation.MenuFont, FontStyle.Regular))
							_defaultFont = testFont.Equals(value);

						base.Font = value;
						ResizeControl();
						Recalculate();
						Invalidate();
					}
				}
            }
        }

		/// <summary>
		/// Gets and sets the background color for drawing.
		/// </summary>
        public override Color BackColor
        {
            get { return base.BackColor; }

            set
            {
                if (this.BackColor != value)
                {
                    _defaultColor = (value == SystemColors.Control);
					DefineBackColor(value);
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Gets and sets an associated windw content used to popout.
		/// </summary>
        public WindowContentTabbed WindowContentTabbed
        {
            get { return _wct; }
            set { _wct = value; }
        }
        
		/// <summary>
		/// Propogate a change in a framework property.
		/// </summary>
		/// <param name="name">Name of property changed.</param>
		/// <param name="value">New value of property.</param>
        public void PropogateNameValue(PropogateName name, object value)
        {
            switch(name)
            {
                case PropogateName.BackColor:
                    this.BackColor = (Color)value;
                    Invalidate();
                    break;
                case PropogateName.InactiveTextColor:
                    this.ForeColor = (Color)value;
                    Invalidate();
                    break;
                case PropogateName.CaptionFont:
                    this.Font = (Font)value;
                    break;
				case PropogateName.StubsShowAll:
					_stubsShowAll = (bool)value;
					ResizeControl();
					Recalculate();
					Invalidate();
					break;
				case PropogateName.Style:
					_style = (VisualStyle)value;
					_colorDetails.Style = _style;
					ResizeControl();
					Recalculate();
					Invalidate();
					break;
                case PropogateName.Apply2007ClearType:
                    _apply2007ClearType = (bool)value;
                    ResizeControl();
                    Recalculate();
                    Invalidate();
                    break;
                case PropogateName.ApplyMediaPlayerClearType:
                    _applyMediaPlayerClearType = (bool)value;
                    ResizeControl();
                    Recalculate();
                    Invalidate();
                    break;
            }
            
            // Pass onto the contained WCT
            _wct.PropogateNameValue(name, value);
        }

		/// <summary>
		/// Calculate the vector needed for drawing the tab stub.
		/// </summary>
		/// <param name="font">Font to use when drawing text.</param>
		/// <returns>Vector needed to correctly draw tab stub.</returns>
		public static int TabStubVector(Font font)
		{
			int fixedVector = _imageVector + _imageGaps;

			int minFontVector = font.Height + _imageGaps;

			// Make sure at least bit enough for the provided font
			if (fixedVector < minFontVector)
				fixedVector = minFontVector;
                
			return fixedVector + _sideGap;
		}

		/// <summary>
		/// Raises the SystemColorsChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data. </param>
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			// If still using the Default color when we were created
			if (_defaultColor)
			{
				this.BackColor = SystemColors.Control;
				Invalidate();
			}

			base.OnSystemColorsChanged(e);
		}
    
		/// <summary>
		/// Raises then MouseMove event.
		/// </summary>
		/// <param name="e">A MouseEventArgs structure containing event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
			// Create a point representing current mouse position
			Point mousePos = new Point(e.X, e.Y);

			int index = 0;
			int count = _drawTabs.Count;

			// Search each draw cell
			for(; index<count; index++)
			{
				DrawTab dt = _drawTabs[index] as DrawTab;

				// Is mouse over this cell?
				if (dt.DrawRect.Contains(mousePos))
				{
					// If the mouse is not over the hover item
					if (_hoverItem != dt.Index)
					{
					    // And we are not already timing this change in hover
					    if (_hoverOver != dt.Index)
					    {
					        // Start timing the hover change
						    _hoverTimer.Start();
						    
						    // Remember which item we are timing
						    _hoverOver = dt.Index;
				        }
					}

    				break;
				}
			}

			// Failed to find an item?
			if (index == count)
			{
				// If we have a hover item or timing a hover change
				if ((_hoverOver != -1) || (_hoverItem != -1))
				{
				    // Stop any timing
				    CancelHoverItem();
				}
			}

			base.OnMouseMove(e);
		}

		/// <summary>
		/// Raises then MouseLeave event.
		/// </summary>
		/// <param name="e">An EventArgs structure containing event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            // Remove any hover state
            CancelHoverItem();
    
			base.OnMouseLeave(e);
		}

		/// <summary>
		/// Raises then MouseDown event.
		/// </summary>
		/// <param name="e">A MouseEventArgs structure containing event data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
        {
            // Only select a button or page when using left mouse button
            if (e.Button == MouseButtons.Left)
			{
                // Create a point representing current mouse position
                Point mousePos = new Point(e.X, e.Y);

				int count = _drawTabs.Count;

				// Search each draw cell
				for(int index=0; index<count; index++)
				{
					DrawTab dt = _drawTabs[index] as DrawTab;

					// Is mouse pressed in this draw cell?
					if (dt.DrawRect.Contains(mousePos))
					{
                        // Prevent any hover timer expiring
                        _hoverTimer.Stop();
						    
                        // This becomes the current hover item
                        _hoverItem = _selectedIndex;
						    
                        // Not timing a hover change
                        _hoverOver = _hoverItem;
                        
                        // Will this cause a change in selection?
						if (_selectedIndex != dt.Index)
						{
                            // Change selection and redraw
                            _selectedIndex = dt.Index;
						
							Recalculate();
							Invalidate();
                        }

                        // Generate event to notify a click occured on the selection
                        OnPageClicked(_selectedIndex);

                        break;
					}
				}
			}
		}

		/// <summary>
		/// Paints the background of the control.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains information about the control to paint.</param>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
		}

		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data. </param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Pen borderPen = new Pen(ControlPaint.LightLight(base.ForeColor));

            switch (_style)
            {
                case VisualStyle.MediaPlayerBlue:
                case VisualStyle.MediaPlayerOrange:
                case VisualStyle.MediaPlayerPurple:
                    using (SolidBrush fillBrush = new SolidBrush(MediaPlayerColorTable.LightBackground(_style)))
                        e.Graphics.FillRectangle(fillBrush, this.ClientRectangle);

                    borderPen.Dispose();
                    borderPen = new Pen(MediaPlayerColorTable.BorderColor(_style));
                    break;
                case VisualStyle.Office2007Blue:
                case VisualStyle.Office2007Silver:
                case VisualStyle.Office2007Black:
                    using (SolidBrush fillBrush = new SolidBrush(Office2007ColorTable.LightBackground(_style)))
                        e.Graphics.FillRectangle(fillBrush, this.ClientRectangle);

                    borderPen.Dispose();
                    borderPen = new Pen(Office2007ColorTable.BorderColor(_style));
                    break;
                case VisualStyle.Office2003:
                case VisualStyle.IDE2005:
                    using (SolidBrush fillBrush = new SolidBrush(_colorDetails.BaseColor))
                        e.Graphics.FillRectangle(fillBrush, this.ClientRectangle);

                    borderPen.Dispose();
                    borderPen = new Pen(_colorDetails.MenuSeparatorColor);
                    break;
                default:
                    using (SolidBrush fillBrush = new SolidBrush(this.BackColor))
                        e.Graphics.FillRectangle(fillBrush, this.ClientRectangle);
                    break;
            }

			// Style specific outline drawing
			DrawOutline(e.Graphics, true);

			// Draw each of the draw objects
			foreach(DrawTab dt in _drawTabs)
			{
				Rectangle drawRect = dt.DrawRect;

				AdjustRectForEdge(ref drawRect);

				// Style specific cell outline drawing
				DrawOutlineForCell(e.Graphics, borderPen, drawRect);

				// Draw the image in the left/top of the cell
				Crownwood.DotNetMagic.Controls.TabPage page = dt.TabPage;

				int xDraw;
				int yDraw;

				switch(_edge)
				{
					case Edge.Left:
					case Edge.Right:
						xDraw = drawRect.Left + (drawRect.Width - _imageVector) / 2;
						yDraw = drawRect.Top + _imageGap;
						break;
					case Edge.Top:
					case Edge.Bottom:
					case Edge.None:
					default:
						xDraw = drawRect.Left + _imageGap;
						yDraw = drawRect.Top + (drawRect.Height - _imageVector) / 2;
						break;
				}

				if ((page.Icon != null) || (page.Image != null) || ((page.ImageIndex != -1) && (page.ImageList != null)))
				{
					if (page.Icon != null)
					{
						// Draw the actual icon
						e.Graphics.DrawIcon(page.Icon, new Rectangle(xDraw, yDraw, _imageVector, _imageVector));
					}
					else
					{
						Image drawImage;
						
						if (page.Image != null)
							drawImage = page.Image;
						else
							drawImage = page.ImageList.Images[page.ImageIndex];

						// Draw the actual image
						e.Graphics.DrawImage(drawImage, new Rectangle(xDraw, yDraw, _imageVector, _imageVector));
						
						// Must dispose of images taken from an image list, as they are copies and not references
						if (page.Image == null)
							drawImage.Dispose();
					}
				}

				// Is anything currently selected
				if ((_selectedIndex != -1) || _stubsShowAll)
				{
					// Is this page selected?
					if ((page == _tabPages[_selectedIndex]) || _stubsShowAll)
					{
						Rectangle textRect;

						using(StringFormat drawFormat = new StringFormat())
						{
							drawFormat.FormatFlags = StringFormatFlags.NoClip | StringFormatFlags.NoWrap;
							drawFormat.Alignment = StringAlignment.Center;
							drawFormat.LineAlignment = StringAlignment.Center;

							// Create text drawing rectangle
							switch(_edge)
							{
								case Edge.Left:
								case Edge.Right:
									textRect = new Rectangle(drawRect.Left, yDraw + _imageVector + _imageGap, 
															 drawRect.Width, drawRect.Height - _imageVector - _imageGap * 2);
															 drawFormat.FormatFlags |= StringFormatFlags.DirectionVertical;
									break;
								case Edge.Top:
								case Edge.Bottom:
								case Edge.None:
								default:
									textRect = new Rectangle(xDraw + _imageVector + _imageGap, drawRect.Top, 
															 drawRect.Width - _imageVector - _imageGap * 2, drawRect.Height);
									break;
							}
							
							Color brushColor;
                            bool clearType;

                            switch (_style)
                            {
                                case VisualStyle.Office2007Blue:
                                case VisualStyle.Office2007Silver:
                                case VisualStyle.Office2007Black:
                                    brushColor = Office2007ColorTable.TitleActiveTextColor(_style);
                                    clearType = _apply2007ClearType;
                                    break;
                                case VisualStyle.MediaPlayerBlue:
                                case VisualStyle.MediaPlayerOrange:
                                case VisualStyle.MediaPlayerPurple:
                                    brushColor = MediaPlayerColorTable.TitleActiveTextColor(_style);
                                    clearType = _applyMediaPlayerClearType;
                                    break;
                                default:
                                    brushColor = this.ForeColor;
                                    clearType = false;
                                    break;
                            }

                            using (SolidBrush drawBrush = new SolidBrush(brushColor))
                            {
                                try
                                {
                                    if (clearType)
                                    {
                                        using(UseClearTypeGridFit clear = new UseClearTypeGridFit(e.Graphics))
                                            e.Graphics.DrawString(page.Title, this.Font, drawBrush, textRect, drawFormat);
                                    }
                                    else
                                    {
                                        e.Graphics.DrawString(page.Title, this.Font, drawBrush, textRect, drawFormat);
                                    }
                                }
                                catch
                                {
                                    RedefineSystemFont();
                                    Invalidate();
                                }
                            }
						}
					}
				}
			}
			
			borderPen.Dispose();

			// Style specific outline drawing
			DrawOutline(e.Graphics, false);
            
			base.OnPaint(e);
		}

		/// <summary>
		/// Raises the PageClicked event.
		/// </summary>
		/// <param name="pageIndex">Page index that has been clicked.</param>
		protected virtual void OnPageClicked(int pageIndex)
		{
			// Has anyone registered for the event?
			if (PageClicked != null)
				PageClicked(this, pageIndex);
		}

		/// <summary>
		/// Raises the PageClicked event.
		/// </summary>
		/// <param name="pageIndex">Page index the mouse is hovering over.</param>
		protected virtual void OnPageOver(int pageIndex)
		{
			// Has anyone registered for the event?
			if (PageOver != null)
				PageOver(this, pageIndex);
		}

		/// <summary>
		/// Raises the PagesLeave event.
		/// </summary>
		protected virtual void OnPagesLeave()
		{
			// Has anyone registered for the event?
			if (PagesLeave != null)
				PagesLeave(this);
		}

		/// <summary>
		/// Raises the Resize event.
		/// </summary>
		/// <param name="e">An EventArgs containing event data.</param>
		protected override void OnResize(EventArgs e)
		{
			ResizeControl();
			Recalculate();
			base.OnResize (e);
		}

		private void ResizeControl()
		{
			// No need to do anything if we are already disposed of
			if (!IsDisposed)
			{
				_textMax = 0;
				bool allText = _stubsShowAll;
                bool clearType;

                switch (_style)
                {
                    case VisualStyle.Office2007Blue:
                    case VisualStyle.Office2007Silver:
                    case VisualStyle.Office2007Black:
                        clearType = _apply2007ClearType;
                        break;
                    case VisualStyle.MediaPlayerBlue:
                    case VisualStyle.MediaPlayerOrange:
                    case VisualStyle.MediaPlayerPurple:
                        clearType = _applyMediaPlayerClearType;
                        break;
                    default:
                        clearType = false;
                        break;
                }

				// Find largest space needed for drawing page text
				using(Graphics g = this.CreateGraphics())
				{
					foreach(Crownwood.DotNetMagic.Controls.TabPage page in _tabPages)
					{
						// Find width of the requested text
						SizeF dimension;

                        try
                        {
                            if (clearType)
                            {
                                using(UseClearTypeGridFit clear = new UseClearTypeGridFit(g))
                                    dimension = g.MeasureString(page.Title, this.Font);
                            }
                            else
                            {
                                dimension = g.MeasureString(page.Title, this.Font);
                            }
                        }
                        catch
                        {
                            RedefineSystemFont();
                            dimension = g.MeasureString(page.Title, this.Font);
                        }

						// If showing all text, add the total text together
						if (allText)
							_textMax += (int)dimension.Width + _imageGap;
						else
						{
							// Otherwise we only want the widest text
							if ((int)dimension.Width > _textMax)
								_textMax = (int)dimension.Width + _imageGap;
						}
					}
				}

				// Calculate total width/height needed
				int variableVector = _tabPages.Count * (_imageVector + _imageGaps) + _textMax;

				// Calculate the fixed direction value
				int fixedVector = TabStubVector(this.Font);

				// Resize the control as appropriate
				switch(_edge)
				{
					case Edge.Left:
					case Edge.Right:
						this.Size = new Size(fixedVector, variableVector + _beginGap + _endGap);
						break;
					case Edge.Top:
					case Edge.Bottom:
					case Edge.None:
					default:
						this.Size = new Size(variableVector + _beginGap + _endGap, fixedVector);
						break;
				}
			}
		}

		private void Recalculate()
		{
			// Create a fresh colleciton for drawing objects
			_drawTabs = new ArrayList();

			// Are we drawing text for all the tabs?
			bool allText = _stubsShowAll;

			// Need start and end position markers
			int cellVector = _imageVector + _imageGaps;
			int posStart = _beginGap;
            bool clearType;

            switch (_style)
            {
                case VisualStyle.Office2007Blue:
                case VisualStyle.Office2007Silver:
                case VisualStyle.Office2007Black:
                    clearType = _apply2007ClearType;
                    break;
                case VisualStyle.MediaPlayerBlue:
                case VisualStyle.MediaPlayerOrange:
                case VisualStyle.MediaPlayerPurple:
                    clearType = _applyMediaPlayerClearType;
                    break;
                default:
                    clearType = false;
                    break;
            }

			// Find largest space needed for drawing page text
			using(Graphics g = this.CreateGraphics())
			{
				// Process each tab page in turn
				for(int index=0; index<_tabPages.Count; index++)
				{
					// Default vector is for just an image and image gaps
					int totalVector = cellVector;

					// Do we need to allocate space for text?
					if ((index ==_selectedIndex) || allText)
					{
						// Do we allocate just the text size?
						if (allText)
						{
							// Find width of the requested text
							SizeF dimension;

                            try
                            {
                                if (clearType)
                                {
                                    using (UseClearTypeGridFit clear = new UseClearTypeGridFit(g))
                                        dimension = g.MeasureString(_tabPages[index].Title, this.Font);
                                }
                                else
                                {
                                    dimension = g.MeasureString(_tabPages[index].Title, this.Font);
                                }
                            }
                            catch
                            {
                                RedefineSystemFont();
                                dimension = g.MeasureString(_tabPages[index].Title, this.Font);
                            }
							
							// Add to the total vector
							totalVector += (int)dimension.Width + _imageGap;
						}
						else
							totalVector += _textMax;
					}

					Rectangle drawRect;

					// Drawing rectangle depends on direction
					switch(_edge)
					{
						case Edge.Left:
							drawRect = new Rectangle(0, posStart, this.Width - _sideGap - 1, totalVector);
							break;
						case Edge.Right:
							drawRect = new Rectangle(_sideGap, posStart, this.Width - _sideGap, totalVector);
							break;
						case Edge.Bottom:
							drawRect = new Rectangle(posStart, _sideGap, totalVector, this.Height - _sideGap);
							break;
						case Edge.Top:
						case Edge.None:
						default:
							drawRect = new Rectangle(posStart, 0, totalVector, this.Height - _sideGap - 1);
							break;
					}

					// Move starting position
					posStart += totalVector;

					// Generate new drawing object for this tab
					_drawTabs.Add(new DrawTab(_tabPages[index], drawRect, index));
				}
			}
		}

		private void AdjustRectForEdge(ref Rectangle rect)
		{
			// Adjust rectangle to exclude desired edge
			switch(_edge)
			{
				case Edge.Left:
					rect.X--;
					rect.Width++;
					break;
				case Edge.Right:
					rect.Width++;
					break;
				case Edge.Bottom:
					rect.Height++;
					break;
				case Edge.Top:
				case Edge.None:
				default:
					rect.Y--;
					rect.Height++;
					break;
			}
		}

		private float AngleForEdge()
		{
			// Adjust rectangle to exclude desired edge
			switch(_edge)
			{
				case Edge.Left:
				case Edge.Right:
					return 0;
				case Edge.Top:
				case Edge.Bottom:
					return 90;
				case Edge.None:
				default:
					return 270;
			}
		}
		
		private void DrawOutline(Graphics g, bool pre)
        {
            Rectangle borderRect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            // Adjust for drawing area
            switch(_edge)
            {
                case Edge.Left:
                    borderRect.Y += _beginGap;
                    borderRect.Height -= _beginGap + _endGap - 1;
                    borderRect.Width -= _sideGap;
                    break;
                case Edge.Right:
                    borderRect.Y += _beginGap;
                    borderRect.Height -= _beginGap + _endGap - 1;
                    borderRect.X += _sideGap;
                    borderRect.Width -= _sideGap;
                    break;
                case Edge.Bottom:
                    borderRect.Y += _sideGap;
                    borderRect.Height -= _sideGap;
                    borderRect.X += _beginGap;
                    borderRect.Width -= _beginGap + _endGap - 1;
                    break;
                case Edge.Top:
                case Edge.None:
                default:
                    borderRect.Height -= _sideGap;
                    borderRect.X += _beginGap;
                    borderRect.Width -= _beginGap + _endGap - 1;
                    break;
            }

            // Remove unwanted drawing edge
            AdjustRectForEdge(ref borderRect);

            if (pre)
            {
                // Is there any rectangle size to draw?
                if ((borderRect.Width > 0) && (borderRect.Height > 0))
                {
                    switch (_style)
                    {
                        case VisualStyle.Office2003:
                            using (Brush fillBrush = new LinearGradientBrush(borderRect, _colorDetails.ActiveTabColor, _colorDetails.BaseColor, AngleForEdge()))
                                g.FillRectangle(fillBrush, borderRect);
                            break;
                        case VisualStyle.Office2007Blue:
                        case VisualStyle.Office2007Silver:
                        case VisualStyle.Office2007Black:
                            using (Brush fillBrush = new LinearGradientBrush(borderRect, Office2007ColorTable.SoftBackground(_style), Office2007ColorTable.DarkBackground(_style), AngleForEdge()))
                                g.FillRectangle(fillBrush, borderRect);
                            break;
                        case VisualStyle.MediaPlayerBlue:
                        case VisualStyle.MediaPlayerOrange:
                        case VisualStyle.MediaPlayerPurple:
                            using (Brush fillBrush = new LinearGradientBrush(borderRect, MediaPlayerColorTable.SoftBackground(_style), MediaPlayerColorTable.DarkBackground(_style), AngleForEdge()))
                                g.FillRectangle(fillBrush, borderRect);
                            break;
                    }
                }
            }
            else
            {
                if (_style == VisualStyle.Plain)
                {
                    using(Pen penL = new Pen(ControlPaint.LightLight(this.BackColor)),
                          penD = new Pen(ControlPaint.Dark(this.BackColor)))
                    {
                        g.DrawLine(penL, borderRect.Left, borderRect.Top, borderRect.Right, borderRect.Top);
                        g.DrawLine(penL, borderRect.Left, borderRect.Top, borderRect.Left, borderRect.Bottom);
                        g.DrawLine(penD, borderRect.Right, borderRect.Top, borderRect.Right, borderRect.Bottom);
                        g.DrawLine(penD, borderRect.Right, borderRect.Bottom, borderRect.Left, borderRect.Bottom);
                    }
                }
            }
        }

        private void DrawOutlineForCell(Graphics g, Pen pen, Rectangle rect)
        {
            switch (_style)
            {
                case VisualStyle.Office2003:
                case VisualStyle.Office2007Blue:
                case VisualStyle.Office2007Silver:
                case VisualStyle.Office2007Black:
                case VisualStyle.MediaPlayerBlue:
                case VisualStyle.MediaPlayerOrange:
                case VisualStyle.MediaPlayerPurple:
                    g.DrawRectangle(pen, rect);
                    break;
                case VisualStyle.IDE2005:
				    Rectangle fillRect = rect;

				    // Adjust fill rectangle so it does not draw outside rounded edges
		            switch(_edge)
		            {
					    case Edge.Left:
						    fillRect.Y++;
						    fillRect.Height--;
						    break;
					    case Edge.Right:
						    fillRect.X++;
						    fillRect.Y++;
						    fillRect.Height--;
						    break;
					    case Edge.Top:
						    fillRect.X++;
						    fillRect.Width--;
						    break;
					    case Edge.Bottom:
						    fillRect.Y++;
						    fillRect.X++;
						    fillRect.Width--;
						    break;
				    }

				    // Fill area in the required gradient background
				    using(Brush fillBrush = new LinearGradientBrush(new Rectangle(fillRect.X-1, fillRect.Y-1, fillRect.Width+2, fillRect.Height+2), _colorDetails.BaseColor2, _colorDetails.BaseColorStub, AngleForEdge()))
					    g.FillRectangle(fillBrush, fillRect);

				    // Draw line around the cell
		            switch(_edge)
		            {
					    case Edge.Left:
						    g.DrawLine(pen, rect.Left, rect.Top, rect.Right - 2, rect.Top);
						    g.DrawLine(pen, rect.Right - 2, rect.Top, rect.Right, rect.Top + 2);
						    g.DrawLine(pen, rect.Right, rect.Top + 2, rect.Right, rect.Bottom - 2);
						    g.DrawLine(pen, rect.Right, rect.Bottom - 2, rect.Right - 2, rect.Bottom);
						    g.DrawLine(pen, rect.Right - 2, rect.Bottom, rect.Left, rect.Bottom);
						    break;
					    case Edge.Right:
						    g.DrawLine(pen, rect.Right, rect.Top, rect.Left + 2, rect.Top);
						    g.DrawLine(pen, rect.Left + 2, rect.Top, rect.Left, rect.Top + 2);
						    g.DrawLine(pen, rect.Left, rect.Top + 2, rect.Left, rect.Bottom - 2);
						    g.DrawLine(pen, rect.Left, rect.Bottom - 2, rect.Left + 2, rect.Bottom);
						    g.DrawLine(pen, rect.Left + 2, rect.Bottom, rect.Right, rect.Bottom);
						    break;
					    case Edge.Top:
						    g.DrawLine(pen, rect.Left, rect.Top, rect.Left, rect.Bottom - 2);
						    g.DrawLine(pen, rect.Left, rect.Bottom - 2, rect.Left + 2, rect.Bottom);
						    g.DrawLine(pen, rect.Left + 2, rect.Bottom, rect.Right - 2, rect.Bottom);
						    g.DrawLine(pen, rect.Right - 2, rect.Bottom, rect.Right, rect.Bottom - 2);
						    g.DrawLine(pen, rect.Right, rect.Bottom - 2, rect.Right, rect.Top);
						    break;
					    case Edge.Bottom:
						    g.DrawLine(pen, rect.Left, rect.Bottom, rect.Left, rect.Top + 2);
						    g.DrawLine(pen, rect.Left, rect.Top + 2, rect.Left + 2, rect.Top);
						    g.DrawLine(pen, rect.Left + 2, rect.Top, rect.Right - 2, rect.Top);
						    g.DrawLine(pen, rect.Right - 2, rect.Top, rect.Right, rect.Top + 2);
						    g.DrawLine(pen, rect.Right, rect.Top + 2, rect.Right, rect.Bottom);
						    break;
				    }
                    break;
                default:
		            switch(_edge)
		            {
		                case Edge.Left:
                        case Edge.Right:
                            g.DrawLine(pen, rect.Left + 1, rect.Bottom, rect.Right, rect.Bottom);       
                            break;                    
                        case Edge.Top:
                        case Edge.Bottom:
                            g.DrawLine(pen, rect.Right, rect.Top + 1, rect.Right, rect.Bottom);       
                            break;                    
                    }
                    break;
		    }
        }

		private void DefineBackColor(Color backColor)
		{
			base.BackColor = backColor;
			
			_backIDE = ColorHelper.TabBackgroundFromBaseColor(backColor);
		}

        private void RedefineSystemFont()
        {
            // Are we using the default menu or a user defined value?
            if (_defaultFont)
            {
                base.Font = new Font(SystemInformation.MenuFont, FontStyle.Regular);
                ResizeControl();
                Recalculate();
            }
        }

		private void OnPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
            RedefineSystemFont();

            // Reset for the latest theme			
			_colorDetails.Reset();
			DefineBackColor(base.BackColor);
			Invalidate();
		}

		private void OnClearedPages()
		{
			// Cancel any hover selection
			CancelHoverItem();

			// Cancel any current selection
			_selectedIndex = -1;

			ResizeControl();
			Recalculate();
			Invalidate();
		}
		
		private void OnInsertedPage(int index, object value)
		{
			// If no page is currently selected
			if (_selectedIndex == -1)
			{
				// Then make the inserted page selected
				_selectedIndex = index;
			}
			else
			{
				// If inserting the new page before the currently selected page, need to dump
				// the selected index to reflect this change in contents
				if (index <= _selectedIndex)
					_selectedIndex++;
			}

			// Cast to correct type
			Crownwood.DotNetMagic.Controls.TabPage page = value as Crownwood.DotNetMagic.Controls.TabPage;

			// Grab the content instance of the page
			Content c = page.Tag as Content;

			// Hook into change in its properties
			c.PropertyChanged += new Content.PropChangeHandler(OnContentChanged);

			ResizeControl();
			Recalculate();
			Invalidate();
		}

		private void OnRemovingPage(int index, object value)
		{
			// Removed page involved in hover calculations?
			if ((_hoverOver == index) || (_hoverItem == index))
				CancelHoverItem();
        
			// Removing the last page?
			if (_tabPages.Count == 1)
			{
				// Get rid of any selection
				_selectedIndex = -1;
			}
			else
			{
				// If removing a page before the selected one...
				if (index < _selectedIndex)
				{
					// ...then the selected index must be decremented to match
					_selectedIndex--;
				}
				else
				{
					// If the selected page is the last one then...
					if (_selectedIndex == (_tabPages.Count-1))
					{
						// Must reduce selected index
						_selectedIndex--;
					}
				}
			}
		}

		private void OnRemovedPage(int index, object value)
		{
			// Cast to correct type
			Crownwood.DotNetMagic.Controls.TabPage page = value as Crownwood.DotNetMagic.Controls.TabPage;

			// Grab the content instance of the page
			Content c = page.Tag as Content;

			// Unhook from change in its properties
			c.PropertyChanged -= new Content.PropChangeHandler(OnContentChanged);

			ResizeControl();
			Recalculate();
			Invalidate();
		}

		private void OnContentChanged(Content obj, Content.Property prop)
		{
			bool update = false;

			// Scan each tab page in turn
			foreach(Crownwood.DotNetMagic.Controls.TabPage page in _tabPages)
			{
				// Is this the page for the changed content?
				if (page.Tag == obj)
				{
					// Property specific processing
					switch(prop)
					{
						case Content.Property.Title:
							page.Title = obj.Title;
							update = true;
							break;
						case Content.Property.ImageList:
							page.ImageList= obj.ImageList;
							update = true;
							break;
						case Content.Property.ImageIndex:
							page.ImageIndex= obj.ImageIndex;
							update = true;
							break;
						case Content.Property.Icon:
							page.Icon= obj.Icon;
							update = true;
							break;
					}
				}

				// Only interested in a single change
				if (update)
					break;
			}

			// Any update required?
			if (update)
			{
				ResizeControl();
				Recalculate();
				Invalidate();
			}
		}

		private void CancelHoverItem()
		{
			// Currently timing a hover change?
			if (_hoverOver != -1)
			{
				// Prevent timer from expiring
				_hoverTimer.Stop();
                
				// No item being timed
				_hoverOver = -1;
			}

			// Any current hover item?
			if (_hoverItem != -1)
			{
				// No item is being hovered
				_hoverItem = -1;
		        
				// Generate event for end of hover
				OnPagesLeave();
			}
		}

		private void OnTimerExpire(object sender, EventArgs e)
		{
			// Prevent the timer from firing again
			_hoverTimer.Stop();

			// A change in hover still valid?
			if (_hoverItem != _hoverOver)
			{
				// This item becomes the current hover item
				_hoverItem = _hoverOver;
                
				// No longer in a timing state
				_hoverOver = -1;

				// Do we need a change in selection?
				if (_selectedIndex != _hoverItem)
				{
					// Change selection and redraw
					_selectedIndex = _hoverItem;

					Recalculate();
					Invalidate();
				}

				// Generate event to notify where mouse is now hovering
				OnPageOver(_selectedIndex);
			}
		}
	}
}

