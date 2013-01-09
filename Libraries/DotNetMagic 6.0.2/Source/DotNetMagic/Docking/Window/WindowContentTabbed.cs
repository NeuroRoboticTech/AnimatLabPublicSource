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
using Crownwood.DotNetMagic.Common;
using Crownwood.DotNetMagic.Controls;

namespace Crownwood.DotNetMagic.Docking
{
	/// <summary>
	/// Provide a tabbed appearance to multiple contents in the same window.
	/// </summary>
    [ToolboxItem(false)]
    public class WindowContentTabbed : WindowContent, IHotZoneSource, IMessageFilter
    {
        // Class constants
        private static int _plainBorder = 3;
        private static int _hotAreaInflate = -3;

        // Instance fields
        private int _dragPageIndex;
        private RedockerContent _redocker;
        private DotNetMagic.Controls.TabControl _tabControl;

		/// <summary>
		/// Initializes a new instance of the WindowContent class.
		/// </summary>
		/// <param name="manager">Parent docking manager instance.</param>
		/// <param name="vs">Visual style for drawing.</param>
        public WindowContentTabbed(DockingManager manager, VisualStyle vs)
            : base(manager, vs)
        {
            _redocker = null;
            
            // Create the TabControl used for viewing the Content windows
            _tabControl = new DotNetMagic.Controls.TabControl();

            // Pass on the requested clear type setting
            _tabControl.Apply2007ClearType = manager.Apply2007ClearType;
            _tabControl.ApplyMediaPlayerClearType = manager.ApplyMediaPlayerClearType;

            // It should always occupy the remaining space after all details
            _tabControl.Dock = DockStyle.Fill;

            // Show tabs only if two or more tab pages exist
            _tabControl.HideTabsMode = HideTabsModes.HideUsingLogic;
            
            // Hook into the TabControl notifications
            _tabControl.GotFocus += new EventHandler(OnTabControlGotFocus);
            _tabControl.LostFocus += new EventHandler(OnTabControlLostFocus);
            _tabControl.PageGotFocus += new EventHandler(OnTabControlGotFocus);
            _tabControl.PageLostFocus += new EventHandler(OnTabControlLostFocus);
            _tabControl.SelectionChanged += new SelectTabHandler(OnSelectionChanged);
            _tabControl.PageDragStart += new PageDragStartHandler(OnPageDragStart);
            _tabControl.PageDragMove += new MouseEventHandler(OnPageDragMove);
            _tabControl.PageDragEnd += new PageDragHandler(OnPageDragEnd);
            _tabControl.PageDragQuit += new PageDragHandler(OnPageDragQuit);
			_tabControl.PageMoved += new PageMovedHandler(OnPageMoved);
            _tabControl.DoubleClickTab += new DoubleClickTabHandler(OnDoubleClickTab);
			_tabControl.Font = manager.TabControlFont;
            _tabControl.BackColor = manager.BackColor;
            _tabControl.ForeColor = manager.InactiveTextColor;
            _tabControl.OfficePixelBorder = false;
			_tabControl.OfficeExtraTabInset = 5;
			_tabControl.OfficeDockSides = true;
			_tabControl.OfficeHeaderBorder = true;
            _tabControl.IDE2005PixelBorder = true;
			_tabControl.IDE2005HeaderBorder = false;
			_tabControl.IDE2005TabJoined = false;
			_tabControl.IDE2005ExtraTabInset = 0;
			_tabControl.DragOutside = true;
            _tabControl.RecordFocus = false;

            // Define the visual style required
            _tabControl.Style = vs;

			// Allow developers a chance to override default settings
			manager.OnTabControlCreated(_tabControl);

            switch(vs)
            {
				case VisualStyle.IDE2005:
				case VisualStyle.Office2003:
                case VisualStyle.Office2007Blue:
                case VisualStyle.Office2007Silver:
                case VisualStyle.Office2007Black:
                case VisualStyle.MediaPlayerBlue:
                case VisualStyle.MediaPlayerOrange:
                case VisualStyle.MediaPlayerPurple:
                    Controls.Add(_tabControl);
                    break;
                case VisualStyle.Plain:
                    // Only the border at the pages edge and not around the whole control
                    _tabControl.InsetBorderPagesOnly = !DockingManager.PlainTabBorder;

                    // We want a border around the TabControl so it is indented and looks consistent
                    // with the Plain look and feel, so use the helper Control 'BorderForControl'
                    BorderForControl bfc = new BorderForControl(_tabControl, _plainBorder);

                    // It should always occupy the remaining space after all details
                    bfc.Dock = DockStyle.Fill;

                    // Define the default border border
                    bfc.BackColor = DockingManager.BackColor;

                    // When in 'VisualStyle.Plain' we need to 
                    Controls.Add(bfc);
                    break;
            }

            // Need to hook into message pump so that the ESCAPE key can be 
            // intercepted when in redocking mode
            Application.AddMessageFilter(this);
        }

		/// <summary>
		/// Gets the selected content in the tabbed group.
		/// </summary>
        public override Content CurrentContent
        {
            get
            {
                DotNetMagic.Controls.TabPage tp = _tabControl.SelectedTab;
                
                if (tp != null)
                    return (Content)tp.Tag;
                else
                    return null;
            }
        }

		/// <summary>
		/// Gets access to the tab control used to present contents.
		/// </summary>
		public DotNetMagic.Controls.TabControl TabControl
		{
			get { return _tabControl; } 
		}

		/// <summary>
		/// Hide the currently selected content.
		/// </summary>
        public void HideCurrentContent()
        {
            DotNetMagic.Controls.TabPage tp = _tabControl.SelectedTab;
            
			int count = _tabControl.TabPages.Count;

			// Find currently selected tab
			int index = _tabControl.SelectedIndex;

			// Decide which other tab to make selected instead
			if (count > 1)
			{
				// Move to the next control along
				int newSelect = index + 1;

				// Wrap around to first tab if at end
				if (newSelect == count)
					newSelect = 0;

				// Change selection
				_tabControl.SelectedIndex = newSelect;
			}
			else
			{
				// Hide myself as am about to die
				this.Hide();

				// Ensure the focus goes somewhere else
				DockingManager.Container.Focus();
			}

            if (tp != null)
			{
				Content c = tp.Tag as Content;

				// Have the manager perform the Hide operation for us
				DockingManager.HideContent(c);
			}
        }
        
		/// <summary>
		/// Make the provided content the selected one.
		/// </summary>
		/// <param name="c">Content to select.</param>
		public override void BringContentToFront(Content c)
		{
            // Find the matching Page and select it
            foreach(DotNetMagic.Controls.TabPage page in _tabControl.TabPages)
                if (page.Tag == c)
                {
                    _tabControl.SelectedTab = page;
                    break;
                }
		}

		/// <summary>
		/// Propogate a change in a framework property.
		/// </summary>
		/// <param name="name">Name of property changed.</param>
		/// <param name="value">New value of property.</param>
        public override void PropogateNameValue(PropogateName name, object value)
        {
            base.PropogateNameValue(name, value);
        
            switch(name)
            {
                case PropogateName.BackColor:
                    Color newColor = (Color)value;
            
                    // In Plain style we need to color the intermidiate window as well    
                    if (Style == VisualStyle.Plain)
                    {
                        BorderForControl bfc = this.Controls[0] as BorderForControl;
                        bfc.BackColor = newColor;                               
                    }

                    _tabControl.BackColor = newColor;
                    this.BackColor = newColor;
                    
                    Invalidate();
                    break;
                case PropogateName.InactiveTextColor:
                    _tabControl.ForeColor = (Color)value;
                    break;
                case PropogateName.PlainTabBorder:
                    _tabControl.InsetBorderPagesOnly = !(bool)value;
                    break;
				case PropogateName.TabControlFont:
					_tabControl.Font = (Font)value;
					break;
				case PropogateName.Style:
					_tabControl.Style = (VisualStyle)value;
					break;
                case PropogateName.Apply2007ClearType:
                    _tabControl.Apply2007ClearType = (bool)value;
                    break;
                case PropogateName.ApplyMediaPlayerClearType:
                    _tabControl.ApplyMediaPlayerClearType = (bool)value;
                    break;
            }
        }

		/// <summary>
		/// Disposes of the resources (other than memory) used by the class.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged; false to release only unmanaged. </param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Remove message filter to remove circular reference
				Application.RemoveMessageFilter(this);
			}

			base.Dispose(disposing);
		}

		/// <summary>
		/// Process the removing of all contents.
		/// </summary>
        protected override void OnContentsClearing()
        {
            _tabControl.TabPages.Clear();

            base.OnContentsClearing();

            if (!this.AutoDispose)
            {
                // Inform each detail of the change in title text
                NotifyFullTitleText("");
            }
        }

		/// <summary>
		/// Process the inserting of a new content.
		/// </summary>
		/// <param name="index">Position of new content.</param>
		/// <param name="value">New content instance.</param>
        protected override void OnContentInserted(int index, object value)
        {
            base.OnContentInserted(index, value);

            Content content = value as Content;

            // Create TabPage to represent the Content
            DotNetMagic.Controls.TabPage newPage = new DotNetMagic.Controls.TabPage();

            // Reflect the Content properties int the TabPage
            newPage.Title = content.Title;
            newPage.ImageList = content.ImageList;
            newPage.ImageIndex = content.ImageIndex;
            newPage.Icon = content.Icon;
			newPage.Control = content.Control;
			newPage.Tag = content;
			
            // Reflect same order in TabPages collection as Content collection
            _tabControl.TabPages.Insert(index, newPage);
        }

		/// <summary>
		/// Process just before a content is removed.
		/// </summary>
		/// <param name="index">Index to be removed.</param>
		/// <param name="value">Content to be removed.</param>
		protected override void OnContentRemoving(int index, object value)
        {
            base.OnContentRemoving(index, value);

            Content c = value as Content;

            // Find the matching Page and remove it
            foreach(DotNetMagic.Controls.TabPage page in _tabControl.TabPages)
                if (page.Tag == c)
                {
                    _tabControl.TabPages.Remove(page);
                    break;
                }
        }

		/// <summary>
		/// Process a change in content detail focus.
		/// </summary>
		/// <param name="wd">WindowDetail instance.</param>
        public override void WindowDetailGotFocus(WindowDetail wd)
        {
            // Transfer focus from WindowDetail to the TabControl
            _tabControl.Focus();
        }

		/// <summary>
		/// Add the collection of hot zones.
		/// </summary>
		/// <param name="redock">Reference to a redocker instance.</param>
		/// <param name="collection">Collection of hot zones.</param>
        public void AddHotZones(Redocker redock, HotZoneCollection collection)
        {
            RedockerContent redocker = redock as RedockerContent;

            bool itself = false;
            bool nullZone = false;

            // We process differently for WindowContent to redock into itself!
            if ((redocker.WindowContent != null) && (redocker.WindowContent == this))
                itself = true;

            // We do not allow a Content to redock into its existing container
            if (itself && !Contents.Contains(redocker.Content))
                nullZone = true;

            Rectangle newSize = this.RectangleToScreen(this.ClientRectangle);
            Rectangle hotArea = _tabControl.RectangleToScreen(_tabControl.ClientRectangle);;

            // Find any caption detail and use that area as the hot area
            foreach(WindowDetail wd in WindowDetails)
            {
                WindowDetailCaption wdc = wd as WindowDetailCaption;

                if (wdc != null)
                {
                    hotArea = wdc.RectangleToScreen(wdc.ClientRectangle);
                    hotArea.Inflate(_hotAreaInflate, _hotAreaInflate);
                    break;
                }
            }

            if (nullZone)
                collection.Add(new HotZoneNull(hotArea));
            else
                collection.Add(new HotZoneTabbed(hotArea, newSize, this, itself));				
        }

		/// <summary>
		/// Create a new restore instance for the window.
		/// </summary>
		/// <param name="child">Child object instance.</param>
		/// <returns>New restore instance.</returns>
		public override Restore RecordRestore(object child) 
		{
			// Child of a WindowContent must be a Content object
			Content c = child as Content;

			StringCollection next = new StringCollection();
			StringCollection previous = new StringCollection();

			bool before = true;

			// Fill collections with list of Content before and after parameter
			foreach(Content content in Contents)
			{
				if (content == c)
					before = false;
				else
				{
					if (before)
					{
						if ((content.UniqueName != null) && (content.UniqueName.Length > 0))
							previous.Add(content.UniqueName);
						else
							previous.Add(content.Title);
					}
					else
					{
						if ((content.UniqueName != null) && (content.UniqueName.Length > 0))
							next.Add(content.UniqueName);
						else
							next.Add(content.Title);
					}
				}
			}

			bool selected = false;

			// Is there a selected tab?
			if (_tabControl.SelectedIndex != -1)
			{
				// Get access to the selected Content
				Content selectedContent = _tabControl.SelectedTab.Tag as Content;

				// Need to record if it is selected
				selected = (selectedContent == c);
			}

			// Create a Restore object to handle this WindowContent
			Restore thisRestore = new RestoreWindowContent(null, c, next, previous, selected);

			// Do we have a Zone as our parent?
			if (ParentZone != null)
			{
				// Get the Zone to prepend its own Restore knowledge
				thisRestore = ParentZone.RecordRestore(this, child, thisRestore);
			}

			return thisRestore;
		}

		/// <summary>
		/// Filters out a message before it is dispatched.
		/// </summary>
		/// <param name="m">The message to be dispatched. You cannot modify this message. </param>
		/// <returns>true to filter out; false otherwise.</returns>
		public bool PreFilterMessage(ref Message m)
		{
			// Has a key been pressed?
			if (m.Msg == (int)Win32.Msgs.WM_KEYDOWN)
			{
				// Is it the ESCAPE key?
				if ((int)m.WParam == (int)Win32.VirtualKeys.VK_ESCAPE)
				{                   
					// Are we in redocking mode?
					if (_redocker != null)
					{
						// Cancel the redocking activity
						_redocker.QuitTrackingMode(null);

						// No longer need the object
						_redocker = null;
                        
						return true;
					}
				}
			}
            
			return false;
		}

		/// <summary>
		/// Process a change in property of a contained content.
		/// </summary>
		/// <param name="obj">Content that has a changed property.</param>
		/// <param name="prop">The property that has changed.</param>
        protected override void OnContentChanged(Content obj, Content.Property prop)
        {
            // Find the matching TabPage entry
            foreach(DotNetMagic.Controls.TabPage page in _tabControl.TabPages)
            {
                // Does this page manage our Content?
                if (page.Tag == obj)
                {
                    // Property specific processing
                    switch(prop)
                    {
                        case Content.Property.Control:
                            page.Control = obj.Control;
                            break;
                        case Content.Property.Title:
                            page.Title = obj.Title;
                            break;
                        case Content.Property.FullTitle:
                            // Title changed for the current page?
                            if (_tabControl.SelectedTab == page)
                            {
                                // Update displayed caption text
                                NotifyFullTitleText(obj.FullTitle);
                            }								
                            break;
                        case Content.Property.ImageList:
                            page.ImageList = obj.ImageList;
                            break;
                        case Content.Property.ImageIndex:
                            page.ImageIndex = obj.ImageIndex;
                            break;
						case Content.Property.Icon:
							page.Icon = obj.Icon;
							break;
						case Content.Property.CaptionBar:
                            // Property changed for the current page?
                            if (_tabControl.SelectedTab == page)
                            {
                                Content target = page.Tag as Content;
                                NotifyShowCaptionBar(target.CaptionBar);                                
                            }
                            break;
                        case Content.Property.CloseButton:
                            // Property changed for the current page?
                            if (_tabControl.SelectedTab == page)
                            {
                                Content target = page.Tag as Content;
                                NotifyCloseButton(target.CloseButton);   
                            }
                            break;
                        case Content.Property.HideButton:
                            // Property changed for the current page?
                            if (_tabControl.SelectedTab == page)
                            {
                                Content target = page.Tag as Content;
                                NotifyHideButton(target.HideButton);   
                            }
                            break;
                    }

                    break;
                }
            }
        }

		/// <summary>
		/// Raises the Resize event.
		/// </summary>
		/// <param name="e">An EventArgs structure containing event data.</param>
        protected override void OnResize(EventArgs e)
        {
            // Inform each Content of its actual displayed size
            foreach(Content c in Contents)
            {
                switch(State)
                {
                    case State.DockLeft:
                    case State.DockRight:
						if (this.Dock != DockStyle.Fill)
						{
							// Only update the remembered width
							c.DisplaySize = new Size(this.ClientSize.Width, c.DisplaySize.Height);
						}
                        break;
                    case State.DockTop:
                    case State.DockBottom:
						if (this.Dock != DockStyle.Fill)
						{
							// Only update the remembered height
							c.DisplaySize = new Size(c.DisplaySize.Width, this.ClientSize.Height);
						}
                        break;
                    case State.Floating:
						{
							Form f = this.FindForm();

							// Update width and height
							if (f == null)
								c.DisplaySize = this.ClientSize;
							else
								c.DisplaySize = f.Size;
						}
                        break;
                }
            }

            base.OnResize(e);
        }
                
		private void OnSelectionChanged(Crownwood.DotNetMagic.Controls.TabControl tabControl, 
										Crownwood.DotNetMagic.Controls.TabPage oldPage, 
										Crownwood.DotNetMagic.Controls.TabPage newPage)
		{
			if (tabControl.TabPages.Count == 0)
			{
				// Inform each detail of the change in title text
				NotifyFullTitleText("");
			}
			else
			{
				// Inform each detail of the new title text
				if (tabControl.SelectedIndex != -1)
				{
					Content selectedContent = tabControl.SelectedTab.Tag as Content;
                    
					NotifyAutoHideImage(selectedContent.AutoHidden);
					NotifyCloseButton(selectedContent.CloseButton);
					NotifyHideButton(selectedContent.HideButton);
					NotifyFullTitleText(selectedContent.FullTitle);
					NotifyShowCaptionBar(selectedContent.CaptionBar);
				}
			}
		}

		private void OnTabControlGotFocus(object sender, EventArgs e)
		{
			NotifyContentGotFocus();
		}

		private void OnTabControlLostFocus(object sender, EventArgs e)
		{
			NotifyContentLostFocus();
		}

		private void OnDoubleClickTab(DotNetMagic.Controls.TabControl tc, DotNetMagic.Controls.TabPage page)
		{
			// Is the user allowed to redock?
			if (DockingManager.AllowRedocking)
			{		
				Content c = (Content)page.Tag;

				// Make Content record its current position and remember it for the future 
				c.RecordRestore();

				// Invert docked status
				c.Docked = (c.Docked == false);

				// Remove from current WindowContent and restore its position
				DockingManager.HideContent(c, false, true, false);
				DockingManager.ShowContent(c);

				// Raise event to indicate a double click occured
				DockingManager.OnDoubleClickDock();
			}
		}
		
		private void OnPageDragStart(Crownwood.DotNetMagic.Controls.TabControl sender, 
									 Crownwood.DotNetMagic.Controls.TabPage movePage, 
									 MouseEventArgs e)
		{
			// Is the user allowed to redock?
			if (DockingManager.AllowRedocking)
			{		
				// Use allow to redock this particular content?
				if (this.RedockAllowed)
				{
					// Event page must specify its Content object
					Content c = movePage.Tag as Content;

					// Remember the position of the tab before it is removed
					_dragPageIndex = _tabControl.TabPages.IndexOf(movePage);

					// Force the entire window to redraw to ensure the Redocker does not start drawing
					// the XOR indicator before the window repaints itself. Otherwise the repainted
					// control will interfere with the XOR indicator.
					this.Refresh();

					// Start redocking activity for the single Content of this WindowContent
					_redocker = Redocker.CreateRedocker(DockingManager.FeedbackStyle, _tabControl, 
														c, this, new Point(e.X, e.Y));
				}
			}
		}

		private void OnPageDragMove(object sender, MouseEventArgs e)
		{
			// Redocker object needs mouse movements
			if (_redocker != null)
				_redocker.OnMouseMove(e);
		}

		private void OnPageDragEnd(Crownwood.DotNetMagic.Controls.TabControl sender, MouseEventArgs e)
		{
			// Are we currently in a redocking state?
			if (_redocker != null)
			{
				// Remove the page from the source
				Crownwood.DotNetMagic.Controls.TabPage removedPage = _tabControl.TabPages[_dragPageIndex];
				_tabControl.TabPages.RemoveAt(_dragPageIndex);

				// Let the redocker finish off
				bool moved = _redocker.OnMouseUp(e);

				// If the tab was not positioned somewhere else
				if (!moved)
				{
					// Put back the page that was removed when dragging started
					_tabControl.TabPages.Insert(_dragPageIndex, removedPage);
				}

				// No longer need the object
				_redocker = null;
			}
		}

		private void OnPageDragQuit(Crownwood.DotNetMagic.Controls.TabControl sender, MouseEventArgs e)
		{
			// Are we currently in a redocking state?
			if (_redocker != null)
			{
				// Get redocker to quit
				_redocker.QuitTrackingMode(e);

				// No longer need the object
				_redocker = null;
			}
		}

		private void OnPageMoved(Crownwood.DotNetMagic.Controls.TabControl sender, 
								 Crownwood.DotNetMagic.Controls.TabPage page, 
								 int newIndex)
		{
			// Get the content for the page
			Content c = page.Tag as Content;

			// Move the content to reflect its matching page position
			Contents.SetIndex(newIndex, c);

			// Find its index position within the docking manager collection
			int cIndex = DockingManager.Contents.IndexOf(c);

			// Default to placing at its current position
			int reIndex = cIndex;

			// Search for each of the contents before the moved page
			for(int i=0; i<newIndex; i++)
			{
				// Grab content to from required page
				Content search = sender.TabPages[i].Tag as Content;

				// Find position of content in collection
				int sIndex = DockingManager.Contents.IndexOf(search);

				// Ensure content is placed after all the contents before it in tab control
				if (sIndex > reIndex)
					reIndex = sIndex;
			}

			// Set new position of content to ensure it persists correctly 
			DockingManager.Contents.SetIndex(reIndex, c);
		}
	}
}
