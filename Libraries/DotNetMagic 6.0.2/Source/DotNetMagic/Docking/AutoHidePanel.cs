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
using Crownwood.DotNetMagic.Win32;
using Microsoft.Win32;

namespace Crownwood.DotNetMagic.Docking
{
	/// <summary>
	/// Manage a collection of auto hidden groups along a docked edge.
	/// </summary>
    [ToolboxItem(false)]
    public class AutoHidePanel : Panel, IMessageFilter
	{
		/// <summary>
		/// Handle visible display of a single auto hidden group.
		/// </summary>
		[ToolboxItem(false)]
		public class AutoHostPanel : Panel, IResizeSource
		{
			// Instance fields
            private Edge _borderEdge;
            private ResizeAutoBar _resizeAutoBar;
            private AutoHidePanel _autoHidePanel;
            private DockingManager _manager;

			/// <summary>
			/// Initializes a new instance of the AutoHostPanel class.
			/// </summary>
			/// <param name="manager">Parent docking manager.</param>
			/// <param name="autoHidePanel">Parent auto hide panel.</param>
			/// <param name="borderEdge">Edge we are docked against.</param>
			public AutoHostPanel(DockingManager manager, AutoHidePanel autoHidePanel, Edge borderEdge)
			{
				// Remember parameters
                _manager = manager;
                _autoHidePanel = autoHidePanel;
                _borderEdge = borderEdge;
				
				LayoutDirection direction;
				
				if ((borderEdge == Edge.Left) || (borderEdge == Edge.Right))
				    direction = LayoutDirection.Horizontal;
				else
				    direction = LayoutDirection.Vertical;
				
				// Create a resizing bar
				_resizeAutoBar = new ResizeAutoBar(direction, this);
				
				// Add to the display
				Controls.Add(_resizeAutoBar);
				
				// Define correct position based on Edge
				switch(_borderEdge)
				{
				    case Edge.Left:
				        _resizeAutoBar.Dock = DockStyle.Left;
				        break;
                    case Edge.Right:
                        _resizeAutoBar.Dock = DockStyle.Right;
                        break;
                    case Edge.Top:
                        _resizeAutoBar.Dock = DockStyle.Top;
                        break;
                    case Edge.Bottom:
                        _resizeAutoBar.Dock = DockStyle.Bottom;
                        break;
                }
			}

			/// <summary>
			/// Gets the current size of the resize bar.
			/// </summary>
			/// <returns></returns>
            public Size ResizeBarSize()
            {
                return _resizeAutoBar.Size;
            }

			/// <summary>
			/// Gets the calculated minimum width of the auto host panel.
			/// </summary>
            public int MinimumWidth 
            { 
                get { return _resizeAutoBar.Width * 5; }
            }
		
			/// <summary>
			/// Gets the calculated minimum height of the auto host panel.
			/// </summary>
            public int MinimumHeight
            { 
                get { return _resizeAutoBar.Height * 6; } 
            }

			/// <summary>
			/// Gets the color to use for the resize bar.
			/// </summary>
            public Color ResizeBarColor
            {
                get { return _manager.ResizeBarColor; }
            }

			/// <summary>
			/// Gets the resize bar vector length.
			/// </summary>
            public int ResizeBarVector
            {
                get { return _manager.ResizeBarVector; }
            }
            
			/// <summary>
			/// Gets the visual style used for drawing.
			/// </summary>
            public VisualStyle Style 
            { 
                get { return _manager.Style; }
            }
            
			/// <summary>
			/// Gets the background color used for drawing.
			/// </summary>
            public Color BackgroundColor
            {
                get { return _manager.BackColor; }
            }

			/// <summary>
			/// Determines if the ResizeBar instance is allowed to resize.
			/// </summary>
			/// <param name="bar">ResizeBar instance.</param>
			/// <returns>Is the instance allowed to resize.</returns>
            public bool CanResize(ResizeBar bar)
            {
                return _manager.AllowResize;
            }
            
			/// <summary>
			/// Determine the screen rectangle the bar can size into.
			/// </summary>
			/// <param name="bar">ResizeBar instance.</param>
			/// <param name="screenBoundary">Screen bounding rectangle.</param>
			/// <returns>Is the instance allowed to resize.</returns>
			public bool StartResizeOperation(ResizeBar bar, ref Rectangle screenBoundary)
            {
                // Set focus into the WCT to prevent it siding away during resize
                _autoHidePanel.SetFocusToWCT();    
            
                // Define resize boundary as the inner area of the Form containing the Zone
                screenBoundary = this.Parent.RectangleToScreen(_manager.InnerResizeRectangle(this));

                // Find the screen limits of this Zone
                Rectangle panelBoundary = RectangleToScreen(this.ClientRectangle);

                int minHeight = this.MinimumHeight;
                int minWidth = this.MinimumWidth;

                // Restrict resize based on which edge we are attached against
                switch(_borderEdge)
                {
                    case Edge.Bottom:
                        {
                            // Restrict Zone being made smaller than its minimum height
                            int diff = panelBoundary.Top - screenBoundary.Top + minHeight;
                            screenBoundary.Y += diff;
                            screenBoundary.Height -= diff;					

                            // Restrict Zone from making inner control smaller than minimum allowed
                            int innerMinimumWidth = _manager.InnerMinimum.Height;
                            screenBoundary.Height -= innerMinimumWidth;
                        }
                        break;
                    case Edge.Top:
                        {
                            // Restrict Zone being made smaller than its minimum height
                            int diff = panelBoundary.Bottom - screenBoundary.Bottom - minHeight;
                            screenBoundary.Height += diff;					

                            // Restrict Zone from making inner control smaller than minimum allowed
                            int innerMinimumWidth = _manager.InnerMinimum.Height;
                            screenBoundary.Y += innerMinimumWidth;
                            screenBoundary.Height -= innerMinimumWidth;
                        }
                        break;
                    case Edge.Right:
                        {
                            // Restrict Zone being made smaller than its minimum width
                            int diff = panelBoundary.Left - screenBoundary.Left + minWidth;
                            screenBoundary.X += diff;
                            screenBoundary.Width -= diff;					

                            // Restrict Zone from making inner control smaller than minimum allowed
                            int innerMinimumWidth = _manager.InnerMinimum.Width;
                            screenBoundary.Width -=	innerMinimumWidth;
                        }
                        break;
                    case Edge.Left:
                        {
                            // Restrict Zone being made smaller than its minimum width
                            int diff = panelBoundary.Right - screenBoundary.Right - minWidth;
                            screenBoundary.Width += diff;

                            // Restrict Zone from making inner control smaller than minimum allowed
                            int innerMinimumWidth = _manager.InnerMinimum.Width;
                            screenBoundary.X += innerMinimumWidth;
                            screenBoundary.Width -=	innerMinimumWidth;
                        }
                        break;
                }

                return true;
            }
            
			/// <summary>
			/// Resize operation completed with delta change.
			/// </summary>
			/// <param name="bar">ResizeBar instance.</param>
			/// <param name="delta">Delta change to size.</param>
			public void EndResizeOperation(ResizeBar bar, int delta)
            {
                switch(_borderEdge)
                {
                    case Edge.Right:
                        Controls[1].Width += delta;
                        this.Width += delta;
                        _autoHidePanel.UpdateContentSize(Controls[1].Width, true);
                        break;
                    case Edge.Left:
                        Controls[1].Width -= delta;
                        this.Width -= delta;
                        this.Left += delta;
                        _autoHidePanel.UpdateContentSize(Controls[1].Width, true);
                        break;
                    case Edge.Bottom:
                        Controls[1].Height += delta;
                        this.Height += delta;
                        _autoHidePanel.UpdateContentSize(Controls[1].Height, false);
                        break;
                    case Edge.Top:
                        Controls[1].Height -= delta;
                        this.Height -= delta;
                        this.Top += delta;
                        _autoHidePanel.UpdateContentSize(Controls[1].Height, false);
                        break;
                }
                
                _autoHidePanel.DefineRectangles();
            }

			/// <summary>
			/// Process change in environment values.
			/// </summary>
			/// <param name="name">Name of environment variable that has changed.</param>
			/// <param name="value">New value for environment variable.</param>
            public void PropogateNameValue(PropogateName name, object value)
            {
                switch(name)
                {
                    case PropogateName.BackColor:
                        this.BackColor = (Color)value;
                        Invalidate();
                        break;
                }

                // Pass onto the Resize bar control
                _resizeAutoBar.PropogateNameValue(name, value);            
            }

			/// <summary>
			/// Paints the background of the control.
			/// </summary>
			/// <param name="e">A PaintEventArgs that contains information about the control to paint.</param>
			protected override void OnPaintBackground(PaintEventArgs e)
			{
				// Overriden to prevent background being painted
			}

			/// <summary>
			/// Raises the Paint event.
			/// </summary>
			/// <param name="e">A PaintEventArgs that contains the event data. </param>
			protected override void OnPaint(PaintEventArgs e)
			{
				// Overriden to paint just the inward facing edge
			}
		}

	    // Static fields
	    private static int _num = 0;
        private static int _slideSteps = 4;
        private static int _slideInterval = 15;
        private static int _dismissInterval = 1000;
        private static int _removeInterval = 1;
	
	    // Instance fields
	    private int _number;
        private bool _killing;
        private bool _defaultColor;
        private bool _dismissRunning;
        private bool _slideRunning;
        private bool _ignoreDismiss;
        private bool _slideOut;
        private int _slideStep;
        private Timer _slideTimer;
        private Timer _dismissTimer;
        private Timer _removeTimer;
        private Rectangle _slideRect;
        private Rectangle _rememberRect;
        private DockingManager _manager;
        private AutoHostPanel _currentPanel;
        private WindowContentTabbed _currentWCT;
        private ColorDetails _colorDetails;
        
		/// <summary>
		/// Initializes a new instance of the AutoHidePanel class.
		/// </summary>
		/// <param name="manager">Parent docking manager.</param>
		/// <param name="dockEdge">Edge we are docked against.</param>
        public AutoHidePanel(DockingManager manager, DockStyle dockEdge)
		{
            // Define initial state
            _number = _num++;
            _defaultColor = true;
            _dismissRunning = false;
            _slideRunning = false;
            _ignoreDismiss = false;
            _killing = false;
            _manager = manager;
            _currentWCT = null;
            _currentPanel = null;
            _slideRect = new Rectangle();
            _rememberRect = new Rectangle();
            _colorDetails = new ColorDetails();
			_colorDetails.Style = ColorHelper.ValidateStyle(manager.Style);
            
            // Get the minimum vector length used for sizing
            int vector = TabStub.TabStubVector(this.Font);
            
            // Use for both directions, the appropriate one will be ignored because of docking style
            this.Size = new Size(vector, vector);

			// Dock ourself against requested position
			this.Dock = dockEdge;

			// We should be hidden until some Contents are added
			this.Hide();

            // We want to perform special action when container is resized
            _manager.Container.Resize += new EventHandler(OnContainerResized);
            
			// Need to know when new children are added or removed
			_manager.Container.ControlAdded +=new ControlEventHandler(OnContainerChildAdded);
			_manager.Container.ControlRemoved += new ControlEventHandler(OnContainerChildRemoved);

			// We need to monitor the visible state of all container child windows
			foreach(Control child in _manager.Container.Controls)
				child.VisibleChanged += new EventHandler(OnContainerChildChanged);
				
			// Add ourself to the application filtering list
            Application.AddMessageFilter(this);
            
            // Configuration timer objects
            CreateTimers();
            
			// We need to know when the system colours have changed
			Microsoft.Win32.SystemEvents.UserPreferenceChanged += 
				new Microsoft.Win32.UserPreferenceChangedEventHandler(OnUserPreferenceChanged); 
        }
		
		/// <summary>
		/// Process change in environment values.
		/// </summary>
		/// <param name="name">Name of environment variable that has changed.</param>
		/// <param name="value">New value for environment variable.</param>
		public void PropogateNameValue(PropogateName name, object value)
        {
            switch(name)
            {
                case PropogateName.BackColor:
                    this.BackColor = (Color)value;
                    Invalidate();
                    break;
                case PropogateName.CaptionFont:
                    this.Font = (Font)value;
                    
                    // Recalculate the window size
                    int vector = TabStub.TabStubVector(this.Font); 
                    this.Size = new Size(vector, vector);
                    
                    Invalidate();
                    break;
				case PropogateName.Style:
					_colorDetails.Style = ColorHelper.ValidateStyle((VisualStyle)value);
					Invalidate();
					break;
			}
            
            // Pass onto each TabStub instance
            foreach(TabStub ts in Controls)
                ts.PropogateNameValue(name, value);
                
            // Pass onto any current Panel object
            if (_currentPanel != null)
                _currentPanel.PropogateNameValue(name, value);
        }
        
		/// <summary>
		/// Gets or sets the background color for the control.
		/// </summary>
        public override Color BackColor
        {
            get { return base.BackColor; }

            set
            {
                if (this.BackColor != value)
                {
                    _defaultColor = (value == SystemColors.Control);
                    base.BackColor = value;
                    Invalidate();
                }
            }
        }
                  
		/// <summary>
		/// Do we contain the provided content instance in a auto hidden group.
		/// </summary>
		/// <param name="c">Content to search for.</param>
		/// <returns>true if found;false otherwise.</returns>
        public bool ContainsContent(Content c)
        {
            return (TabStubForContent(c) != null);
        }

		/// <summary>
		/// Make the auto hidden panel containing the content visible.
		/// </summary>
		/// <param name="c">Content to search for.</param>
        public void BringContentIntoView(Content c)
        {
            // Test each of the TabStub child controls
            foreach(TabStub ts in this.Controls)
            {
                // Test each page inside the TabStub
                foreach(Crownwood.DotNetMagic.Controls.TabPage page in ts.TabPages)
                {
                    if (c == (page.Tag as Content))
                    {
                        // Are we already showing this content slide out?
                        if (_currentWCT != null)
                        {
                            // Does the WCT contain the page we want displaying?
                            foreach (Content cWCT in _currentWCT.Contents)
                            {
                                if (cWCT == c)
                                {
                                    // If the content is not currently shown
                                    if (_currentWCT.CurrentContent == c)
                                    {
                                        // Finished
                                        return;
                                    }
                                }
                            }
                        }

                        // Remove any existing window
                        RemoveShowingWindow();

                        // Use existing method to cause content to be displayed
                        OnPageClicked(ts, ts.TabPages.IndexOf(page));                    
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Create a restore object for the provided content.
        /// </summary>
        /// <param name="c">Content to search for.</param>
        /// <returns>Restore object that contains position details.</returns>
        public Restore RestoreObjectForContent(Content c)
        {
            StringCollection next = new StringCollection();
            StringCollection previous = new StringCollection();
            StringCollection nextAll = new StringCollection();
            StringCollection previousAll = new StringCollection();

            // Which group has the marked content?
            TabStub marked = TabStubForContent(c);

            // Have we found the marked group yet?
            bool foundGroup = false;
            
            // Found the content in the marked group yet?
            bool foundContent = false;

			// Is this content the currently active one?
			bool currentContent = false;

            int controlCount = this.Controls.Count;
        
            // Process each TabStub in turn
            for(int controlIndex=controlCount-1; controlIndex>=0; controlIndex--)
            {
                TabStub ts = this.Controls[controlIndex] as TabStub;
            
                // Process each Page in the TabStub
                foreach(Crownwood.DotNetMagic.Controls.TabPage page in ts.TabPages)
                {
                    Content content = page.Tag as Content;

                    // Is this the marked group
                    if (marked == ts)
                    {
                        // Add into the 'nextAll' rather than 'previousAll' groups from now on
                        foundGroup = true;

                        // No need to save ourself in our best friends list!
                        if (content == c)
                        {
                            // Add into the 'next' rather than 'previous' contents now
                            foundContent = true;

							// Is this page the currently selected one?
							currentContent = (ts.SelectedIndex == ts.TabPages.IndexOf(page));
                        }
                        else
                        {
                            if (!foundContent)
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
                    else
                    {
                        if (!foundGroup)
                        {
							if ((content.UniqueName != null) && (content.UniqueName.Length > 0))
								previousAll.Add(content.UniqueName);
							else
								previousAll.Add(content.Title);
                        }
                        else
                        {
							if ((content.UniqueName != null) && (content.UniqueName.Length > 0))
								nextAll.Add(content.UniqueName);                    
							else
								nextAll.Add(content.Title);                    
						}
                    }
                }
            }

            // Calculate state from docking value
            State windowState = State.DockLeft;
		    
            // Define stub settings based on our docking position
            switch(this.Dock)
            {
                case DockStyle.Left:
                    windowState = State.DockLeft;
                    break;
                case DockStyle.Right:
                    windowState = State.DockRight;
                    break;
                case DockStyle.Top:
                    windowState = State.DockTop;
                    break;
                case DockStyle.Bottom:
                    windowState = State.DockBottom;
                    break;
            }
            
            return new RestoreAutoHideAffinity(null, windowState, c, next, previous, nextAll, previousAll, currentContent);
        }
        
		/// <summary>
		/// Add a new content into this edge using provided hint information.
		/// </summary>
		/// <param name="content">Content to be added.</param>
		/// <param name="next">Collection of content names.</param>
		/// <param name="previous">Collection of content names.</param>
		/// <param name="nextAll">Collection of content names.</param>
		/// <param name="previousAll">Collection of content names.</param>
		/// <param name="contentCurrent">Should this content become the current one.</param>
		public void AddContent(Content content, 
                               StringCollection next, 
                               StringCollection previous, 
                               StringCollection nextAll, 
                               StringCollection previousAll,
							   bool contentCurrent)
        {
            int nextIndex = 0;
            int previousIndex = 0;
            TabStub nextTabStub = null;
            TabStub previousTabStub = null;
            TabStub nextAllTabStub = null;
            TabStub previousAllTabStub = null;
        
            int controlCount = this.Controls.Count;
        
            // Process each TabStub in turn
            for(int controlIndex=controlCount-1; controlIndex>=0; controlIndex--)
            {
                TabStub ts = this.Controls[controlIndex] as TabStub;

                // Process each Page in the TabStub
                foreach(Crownwood.DotNetMagic.Controls.TabPage page in ts.TabPages)
                {
                    Content c = page.Tag as Content;

					string cLookup = c.Title;
					
					// Use the unique name for lookup if it exists
					if ((c.UniqueName != null) && (c.UniqueName.Length > 0))
						cLookup = c.UniqueName;

                    // Always use the last 'previous' discovered
                    if (previous.Contains(cLookup))
                    {
                        previousIndex = ts.TabPages.IndexOf(page);
                        previousTabStub = ts;
                    }
                    
                    // Only remember the first 'next' discovered
                    if (next.Contains(cLookup))
                    {
                        if (nextTabStub == null)
                        {
                            nextIndex = ts.TabPages.IndexOf(page);
                            nextTabStub = ts;
                        }
                    }

                    // Always use the last 'previousAll' discovered
                    if (previousAll.Contains(cLookup))
                        previousAllTabStub = ts;

                    // Only remember the first 'next' discovered
                    if (nextAll.Contains(cLookup))
                    {
                        if (nextAllTabStub == null)
                            nextAllTabStub = ts;
                    }
                }
            }            

            // If no matches at all found
            if ((previousTabStub == null) && (nextTabStub == null))
            {
                // Default to inserting at end of list
                int insertIndex = Controls.Count;
            
                // If found some friends contents, then insert relative to them
                if (previousAllTabStub != null)
                    insertIndex = Controls.IndexOf(previousAllTabStub);
                else
                {
                    if (nextAllTabStub != null)
                        insertIndex = Controls.IndexOf(nextAllTabStub) + 1;
                }
            
                ContentCollection cs = new ContentCollection();
                
                cs.Add(content);
            
                // Add at end of current list of TabStubs
                AddContentsAsGroup(cs, content, insertIndex);
            }
            else
            {
                if (previousTabStub != null)
                    AddContentIntoTabStub(content, previousTabStub, previousIndex + 1, contentCurrent);
                else
                    AddContentIntoTabStub(content, nextTabStub, nextIndex, contentCurrent);
            }
        }

		/// <summary>
		/// Add a new content into specified stub at specified index.
		/// </summary>
		/// <param name="content">Content to be added.</param>
		/// <param name="ts">Which TabStub to add the content into.</param>
		/// <param name="index">Which indexed position inside tab stub.</param>
		/// <param name="currentContent">Should this content become the current one.</param>
		public void AddContentIntoTabStub(Content content, TabStub ts, int index, bool currentContent)
        {
            // Is focus leaving the entire WindowContentTabbed control?
            if ((_currentWCT != null) && (_currentWCT == ts.WindowContentTabbed))
            {
                // Remove Panel/WCT from display and stop timers
                RemoveDisplayedWindow();
            }                
        
            // Create a new tab page
            Crownwood.DotNetMagic.Controls.TabPage page = new Crownwood.DotNetMagic.Controls.TabPage();
		        
            // Copy across the visual properties
            page.Title = content.Title;
            page.ImageList = content.ImageList;
            page.ImageIndex = content.ImageIndex;
			page.Icon = content.Icon;
                
            // Remember reference to Content it represents
            page.Tag = content;
		        
            // Add into the stub
            ts.TabPages.Insert(index, page);

			// Do we need to make this the selected page?
			if (currentContent)
				ts.SelectedIndex = ts.TabPages.IndexOf(page);
		        
            // Mark Content as being in AutoHide mode
            content.AutoHidePanel = this;
            content.AutoHidden = true;
        
            // Add content into the WCT of the TabStub
            ts.WindowContentTabbed.Contents.Insert(index, content);

            // Make sure this AutoHidePanel is visible
            if (!this.Visible)
                this.Show();
                
            Invalidate();
        }
        
		/// <summary>
		/// Add all the contents of a collection as a new tab stub.
		/// </summary>
		/// <param name="contents">Collection of content to be added.</param>
		/// <param name="current">Initial content to show as selected.</param>
		public void AddContentsAsGroup(ContentCollection contents, Content current)
        {
            // By default, insert new group at the end of display which is start of list
            AddContentsAsGroup(contents, current, 0);
        }
            
		/// <summary>
		/// Add all the contents of a collection as a new tab stub at indexed position.
		/// </summary>
		/// <param name="contents">Collection of content to be added.</param>
		/// <param name="current">Initial content to show as selected.</param>
		/// <param name="index">Indexed position of new tab stub.</param>
        public void AddContentsAsGroup(ContentCollection contents, Content current, int index)
		{
            // Create new TabStub to represent the Contents
            TabStub ts = _manager.Factory.CreateTabStub(ColorHelper.ValidateStyle(_manager.Style), _manager.StubsShowAll);

            // Set manager requested settings
            ts.Font = _manager.CaptionFont;
            ts.BackColor = _manager.BackColor;
            ts.ForeColor = _manager.InactiveTextColor;
            ts.Apply2007ClearType = _manager.Apply2007ClearType;
            ts.ApplyMediaPlayerClearType = _manager.ApplyMediaPlayerClearType;

            // Hook into events
            ts.PageOver += new TabStub.TabStubIndexHandler(OnPageOver);
            ts.PageClicked += new TabStub.TabStubIndexHandler(OnPageClicked);
            ts.PagesLeave += new TabStub.TabStubHandler(OnPagesLeave);
		    
			// Track which of the newly created TabStub pages needs to be the current one
			Crownwood.DotNetMagic.Controls.TabPage currentPage = null;

            // Add a page for each Content instance
            foreach(Content c in contents)
            {
                // Create page object
                Crownwood.DotNetMagic.Controls.TabPage page = new Crownwood.DotNetMagic.Controls.TabPage();
		        
                // Copy across the visual properties
                page.Title = c.Title;
                page.ImageList = c.ImageList;
                page.ImageIndex = c.ImageIndex;
				page.Icon = c.Icon;
                
                // Remember reference to Content it represents
                page.Tag = c;
		        
                // Add into the stub
                ts.TabPages.Add(page);

				// Rememeber the initial one to show
				if (c == current)
					currentPage = page;
		        
                // Mark Content as being in AutoHide mode
                c.AutoHidePanel = this;
                c.AutoHidden = true;
            }

			// Define the correct starting page
			if (currentPage != null)
				ts.SelectedIndex = ts.TabPages.IndexOf(currentPage);

            State windowState = State.DockLeft;
		    
            // Define stub settings based on our docking position
            switch(this.Dock)
            {
                case DockStyle.Left:
                    windowState = State.DockLeft;
                    ts.Edging = Edge.Left;
                    ts.Dock = DockStyle.Top;
                    break;
                case DockStyle.Right:
                    windowState = State.DockRight;
                    ts.Edging = Edge.Right;
                    ts.Dock = DockStyle.Top;
                    break;
                case DockStyle.Top:
                    windowState = State.DockTop;
                    ts.Edging = Edge.Top;
                    ts.Dock = DockStyle.Left;
                    break;
                case DockStyle.Bottom:
                    windowState = State.DockBottom;
                    ts.Edging = Edge.Bottom;
                    ts.Dock = DockStyle.Left;
                    break;
            }
		    
            // Add stub into the view
            Controls.Add(ts);
            
            // Set correct new position
            Controls.SetChildIndex(ts, index);
		    
            // Each TabStub has a WCT created and ready to be shown when needed
            WindowContentTabbed wct = _manager.CreateWindowForContent(null, new EventHandler(OnPageClose),
                                                                      null, new EventHandler(OnPageAutoHide),
                                                                      new ContextHandler(OnPageContextMenu)) as WindowContentTabbed;
            
            // Add each Content instance in turn
            foreach(Content c in contents)
                wct.Contents.Add(c);
                
            // By default the first Content added to a WCT will define the size
            // of the WCT control. We need to override this to use the AutoHideSize
            // from the first Content instead.
            wct.Size = contents[0].AutoHideSize;

            // Ensure Window caption bar reflects correct docking status
            wct.State = windowState;

            // Inform Window it should not allow user initiated redocking
            wct.RedockAllowed = false;

            // Hide tab selection from user
            wct.TabControl.HideTabsMode = HideTabsModes.HideAlways;

            // Associate WCT with matching TabStub
            ts.WindowContentTabbed = wct;
            
            // Make sure this AutoHidePanel is visible
            if (!this.Visible)
			    this.Show();
			    
	        Invalidate();
		}

		/// <summary>
		/// Invert the auto hidden status of a specified content container window.
		/// </summary>
		/// <param name="wct">Window to search for.</param>
		public void InvertAutoHideWindowContent(WindowContentTabbed wct)
		{
			UnAutoHideWindowContent(wct, true);
		}

		/// <summary>
		/// Remove the specified content from its tab stub.
		/// </summary>
		/// <param name="c">Content to search for.</param>
        public void RemoveContent(Content c)
        {
            TabStub targetTS = null;
            Crownwood.DotNetMagic.Controls.TabPage targetPage = null;
        
            // Find the TabStub group this content is inside
            foreach(TabStub ts in this.Controls)
            {
                // Test each page of the TabStub control
                foreach(Crownwood.DotNetMagic.Controls.TabPage page in ts.TabPages)
                {
                    Content pageC = page.Tag as Content;
                    
                    if (pageC == c)
                    {
                        // Remember found target
                        targetTS = ts;
                        targetPage = page;
                        break;
                    }
                }
            }            
            
            // Found a target?
            if ((targetTS != null) && (targetPage != null))
            {
                // Are we removing the last entry in the WCT?
                if (targetTS.TabPages.Count == 1)
                {
                    int count = targetTS.WindowContentTabbed.Contents.Count;
                    
                    // Remove all contents from the WCT
                    for(int i=0; i<count; i++)
                        targetTS.WindowContentTabbed.Contents.RemoveAt(0);

                    // If any panel/WCT showing
                    if (targetTS.WindowContentTabbed == _currentWCT)
                    {
                        // Remove Panel/WCT from display and stop timers
                        KillDisplayedWindow(_currentWCT);
                    }
                                        
                    // Remove the WCT from TabStub
                    ControlHelper.Remove(targetTS.Controls, targetTS.WindowContentTabbed);

                    // Remove the stub from this panel
                    ControlHelper.Remove(this.Controls, targetTS);
                    
                    // Cleanup gracefully
                    targetTS.Dispose();                    
                }
                else
                {
                    // Currently showing some pages?
                    if (targetTS.WindowContentTabbed == _currentWCT)
                    {
                        bool found = false;
                    
                        // Is it our page?
                        foreach(Content cWCT in _currentWCT.Contents)
                        {
                            if (cWCT == c)
                            {
                                // Remove our page from view
                                found = true;                     
                                break;
                            }
                        }
                        
                        // Remove unwanted page
                        if (found)
                        {
                            // Find its position index
                            int index = _currentWCT.Contents.IndexOf(c);
                        
                            // Remove just the selected entry from stub
                            targetTS.TabPages.RemoveAt(index);
                        
                            // Remove the selected entry from WCT
                            _currentWCT.Contents.RemoveAt(index);
                        }
                    }

                    // Reomve the target stub
                    targetTS.TabPages.Remove(targetPage);
                    targetTS.WindowContentTabbed.Contents.Remove(c);
                }

                // No longer inside an auto hidden panel                   
                c.AutoHidePanel = null;
            }

            // If no more contents remain then hide
            if (this.Controls.Count == 0)
                this.Hide();
        }

		/// <summary>
		/// Remove any auto hidden window that is currently popped out.
		/// </summary>
		public void RemoveShowingWindow()
		{
			_ignoreDismiss = false;

			// Is focus leaving the entire WindowContentTabbed control?
			if (_currentWCT != null)
			{
				// Remember current focus
				IntPtr hWnd = User32.GetFocus();

				// Do not slide a window in the process of being removed
				StopDismissTimer();
				StopSlideTimer();
			
				// Remove Panel/WCT from display and stop timers
				RemoveDisplayedWindow();
                
				// No longer considered the shown window
				_currentWCT = null;

                // Replace the focus
				User32.SetFocus(hWnd);
			}	
            
			// Prevent drawing artifacts by invalidating window
			Invalidate();
		}

		/// <summary>
		/// Filters out a message before it is dispatched.
		/// </summary>
		/// <param name="msg">The message to be dispatched. You cannot modify this message. </param>
		/// <returns>true to filter out; false otherwise.</returns>
        public bool PreFilterMessage(ref Message msg)
        {
            Form parentForm = this.FindForm();
            Form parentMdi = (parentForm != null ? parentForm.MdiParent : null);

            // Only interested if the Form we are on contains the focus and we are showing a Panel
            if ((parentForm != null) &&
                ((parentForm == Form.ActiveForm) || (parentMdi != null && parentMdi.ActiveMdiChild == parentForm)) && 
                parentForm.ContainsFocus && (_currentPanel != null) && 
                !_currentPanel.IsDisposed)
            {		
                switch(msg.Msg)
                {
                    case (int)Win32.Msgs.WM_KEYDOWN:
                        // Pressing escape removes the auto hidden window
                        if ((int)msg.WParam == (int)Win32.VirtualKeys.VK_ESCAPE)
                            RemoveShowingWindow();
                        break;
                    case (int)Win32.Msgs.WM_MOUSEMOVE:
                        Win32.POINT screenPos;
                        screenPos.x = (int)((uint)msg.LParam & 0x0000FFFFU);
                        screenPos.y = (int)(((uint)msg.LParam & 0xFFFF0000U) >> 16);

                        // Convert the mouse position to screen coordinates
                        User32.ClientToScreen(msg.HWnd, ref screenPos);

                        // Get the screen rectangle for the showing panel and this object
                        Rectangle panelRect = _currentPanel.RectangleToScreen(_currentPanel.ClientRectangle);
                        Rectangle thisRect = this.RectangleToScreen(this.ClientRectangle);

                        // Do we think the mouse is not over the tab or panel?
                        if (_dismissRunning)
                        {
                            // Is mouse moving over the panel?
                            if (panelRect.Contains(new Point(screenPos.x, screenPos.y)))
                            {
                                // Cancel timer
                                StopDismissTimer();
                           }
                        }
                        else
                        {
                            // If mouse not over the Panel or ourself
                            if (!panelRect.Contains(new Point(screenPos.x, screenPos.y)) &&
                                !thisRect.Contains(new Point(screenPos.x, screenPos.y)))
                            {
                                // Simulate the mouse leaving ourself so that dismiss timer is started
                                OnPagesLeave(null);
                            }
                        }

                        break;
                }
            }

            return false;
		}

		/// <summary>
		/// Raises the SystemColorsChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data. </param>
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			if (_defaultColor)
				Invalidate();
		}

		/// <summary>
		/// Paints the background of the control.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains information about the control to paint.</param>
		protected override void OnPaintBackground(PaintEventArgs e)
        {
            Brush backBrush;

            if (_defaultColor)
            {
                switch (ColorHelper.ValidateStyle(_manager.Style))
                {
                    default:
                    case VisualStyle.IDE2005:
                    case VisualStyle.Office2003:
                        backBrush = new SolidBrush(_colorDetails.BaseColor);
                        break;
                    case VisualStyle.Office2007Blue:
                    case VisualStyle.Office2007Silver:
                    case VisualStyle.Office2007Black:
                        backBrush = new SolidBrush(Office2007ColorTable.LightBackground(ColorHelper.ValidateStyle(_manager.Style)));
                        break;
                    case VisualStyle.MediaPlayerBlue:
                    case VisualStyle.MediaPlayerOrange:
                    case VisualStyle.MediaPlayerPurple:
                        backBrush = new SolidBrush(MediaPlayerColorTable.LightBackground(ColorHelper.ValidateStyle(_manager.Style)));
                        break;
                }
            }
            else
            {
                if (_defaultColor)
                    backBrush = new SolidBrush(SystemColors.Control);
                else
                    backBrush = new SolidBrush(base.BackColor);
            }

            e.Graphics.FillRectangle(backBrush, this.ClientRectangle);
                
			// Dispose of brush no longer needed
			backBrush.Dispose();
        }

		/// <summary>
		/// Disposes of the resources (other than memory) used by the class.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged; false to release only unmanaged. </param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_slideTimer.Stop();
				_dismissTimer.Stop();
				_slideTimer.Dispose();
				_dismissTimer.Dispose();
			
				// Must unhook to alllow garbage collection
				Microsoft.Win32.SystemEvents.UserPreferenceChanged -= 
					new Microsoft.Win32.UserPreferenceChangedEventHandler(OnUserPreferenceChanged); 

				// Color details has resources that need releasing
				_colorDetails.Dispose();
				
				// Unhook from manager instance
				_manager.Container.Resize -= new EventHandler(OnContainerResized);

				// Need to know when new children are added or removed
				_manager.Container.ControlAdded -= new ControlEventHandler(OnContainerChildAdded);
				_manager.Container.ControlRemoved -= new ControlEventHandler(OnContainerChildRemoved);

				// We need to monitor the visible state of all container child windows
				foreach(Control child in _manager.Container.Controls)
					child.VisibleChanged -= new EventHandler(OnContainerChildChanged);

				// Remove hook into the message filter
				Application.RemoveMessageFilter(this);
			}

			base.Dispose(disposing);
		}
		
		private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			// Must reset for the new theme
			_colorDetails.Reset();

			// Redraw using the new theme colors
			Invalidate();
		}

        private void RemoveDisplayedWindow()
        {
            if (_currentPanel != null)
            {
				// Remove snooping of changes to focus 
				MonitorPanel(false);

                Form topForm = _manager.Container.FindForm();

                if (topForm != null)
                {
                    // If we are inside an mdi container
                    if (topForm.IsMdiContainer)
                    {
                        // Shift focus to the active child before removing ourself
                        if ((topForm.ActiveMdiChild != null) && !topForm.ActiveMdiChild.ContainsFocus)
                            User32.SetFocus(topForm.ActiveMdiChild.Handle);
                    }
                }

				// Remove the child WindowContentTabbed
                if (_currentWCT != null)
                    ControlHelper.Remove(_currentPanel.Controls, _currentWCT);
                
                // Remove Panel from managed container
                ControlHelper.Remove(_manager.Container.Controls, _currentPanel);
            }
            
            if (_currentWCT != null)
            {
                // Generate an event to indicate the currently displayed auto hide content has been removed
                _manager.OnContentAutoHideClosed(_currentWCT.Contents[_currentWCT.TabControl.SelectedIndex]);

                // Restore the original sizes
                _currentWCT.Width = _rememberRect.Width;
                _currentWCT.Height = _rememberRect.Height;
            }
                        
            if (_currentPanel != null)
            {
                // Destroy the panel
                _currentPanel.Dispose();
                _currentPanel = null;
            }
        }

        private void KillDisplayedWindow(WindowContentTabbed wct)
        {
            _killing = true;
            
            // If dismiss timer running, then turn it off
            StopDismissTimer();

			// Sometimes this method is called when _currentWCT is not set
			if (wct != null)
			{
                // Get rid of the displayed Panel immediately
                RemoveDisplayedWindow();

                // Remove content objects from WCT to update state
				int count = wct.Contents.Count;
	            
				for(int index=0; index<count; index++)
				{
					// Remove it from collection
					wct.Contents.RemoveAt(0);
				}
	            
				// No longer considered the shown window
				wct.Dispose();

				// Make sure instance reference is nulled where appropriate
				if (wct == _currentWCT)
					_currentWCT = null;
			}
            
            _killing = false;
        }

        private void UpdateContentSize(int newVector, bool isWidth)
        {
            // Ensure we have a Panel to work with
            if (_currentPanel != null)
            {
                // Should always have WCT, but just in case...
                if (_currentWCT != null)
                {
                    // Modify the remembered AutoHide vector
                    foreach(Content c in _currentWCT.Contents)
                    {
                        // Get existing Size
                        Size s = c.AutoHideSize;
                        
                        // Update appropriate vector
                        if (isWidth)
                            s.Width = newVector;
                        else
                            s.Height = newVector;
                            
                        // Save it away
                        c.AutoHideSize = s;
                    }
                }
            }
        }

        private void DefineRectangles()
        {
            // Store original WCT size to be restored later
            _rememberRect = _currentWCT.ClientRectangle;        

            // Default showing size to that requested by WCT            
            _slideRect = _rememberRect;
        }

        private void SetFocusToWCT()
        {
            // Ensure we have a Panel to work with
            if ((_currentPanel != null) && (_currentWCT != null))
            {
                // If the Panel does not already contain the focus...
                if (!_currentWCT.ContainsFocus)
                {
                    _currentWCT.Focus();
                    
                    // Setting the focus could have caused it to be removed!
                    if (_currentWCT != null)
						_currentWCT.Refresh();
                }
            }
        }
        
        private void OnPageClicked(TabStub sender, int pageIndex)
        {
            // Remove any showing auto hide windows except our own
            _manager.RemoveShowingAutoHideWindowsExcept(this);
            
            // A click is the same as an immediate hover over
            OnPageOver(sender, pageIndex);

            // A click implies the panel takes the focus immediately
            SetFocusToWCT();
        }
        
        private void OnPageOver(TabStub sender, int pageIndex)
        {
            // Remove any showing auto hide windows except our own
            _manager.RemoveShowingAutoHideWindowsExcept(this);
            
            // No need for running timer, this action supercedes it
            StopDismissTimer();

            // Hovering over a different TabStub?
            if (_currentWCT != sender.WindowContentTabbed)
            {
                // Remove any currently displayed Panel/WCT
                if (_currentWCT != null)
                    RemoveDisplayedWindow();
            }
            else
            {
                // Different tab in the same TabStub?
                if (pageIndex != _currentWCT.TabControl.SelectedIndex)
                {
                    // Remove any currently displayed Panel/WCT
                    if (_currentWCT != null)
                        RemoveDisplayedWindow();
                }            
                else
                {
                    // Hover over the current window, so do nothing
                    return;
                }

            }

            Edge borderEdge = Edge.None;

            // Define which edge of the host panel shown have a border drawn
            switch(this.Dock)
            {
                case DockStyle.Left:
                    borderEdge = Edge.Right;
                    break;
                case DockStyle.Right:
                    borderEdge = Edge.Left;
                    break;
                case DockStyle.Top:
                    borderEdge = Edge.Bottom;
                    break;
                case DockStyle.Bottom:
                    borderEdge = Edge.Top;
                    break;
            }

            // Create a Panel that will host the actual WindowContentTabbed control,
            // the Panel is resized to slide into/from view. The WCT is a fixed size
            // within the Panel and so only the partial view of the WCT is shown and
            // at any point in time. Cannot resize the WCT into view as it would keep
            // repainting the caption details and effect and docking items inside it.
            _currentPanel = _manager.Factory.CreateAutoHostPanel(_manager, this, borderEdge);
            
            // Do not show it until we have resizing it as needed
            _currentPanel.Hide();                 
            
            // Get access to the WindowContentTabbed that is to be hosted
            _currentWCT = sender.WindowContentTabbed;

            // Select the correct page for view in the WCT
            _currentWCT.TabControl.SelectedIndex = pageIndex;            
                
            // Place the WCT inside the host Panel
            _currentPanel.Controls.Add(_currentWCT);

            // Now add the Panel to the container display
            _manager.Container.Controls.Add(_currentPanel);
		    
            // Make it top of the Z-Order
            _manager.Container.Controls.SetChildIndex(_currentPanel, 0);

            // Define the remember and slide rectangle values
            DefineRectangles();

            // Set the modified WCT size
            _currentWCT.Width = _slideRect.Width;
            _currentWCT.Height = _slideRect.Height;

            Size barSize = _currentPanel.ResizeBarSize();

            // Set the initial size/location of Panel and hosted WCT
            switch(this.Dock)
            {
                case DockStyle.Left:
					_currentPanel.SetBounds(this.Right, this.Top, 0, this.Height);
                    _currentWCT.Height = this.Height;
                    break;
                case DockStyle.Right:
					_currentPanel.SetBounds(this.Left, this.Top, 0, this.Height);
                    _currentWCT.Height = this.Height;
                    break;
                case DockStyle.Top:
					_currentPanel.SetBounds(this.Left, this.Bottom, this.Width, 0);
                    _currentWCT.Width = this.Width;
                    break;
                case DockStyle.Bottom:
					_currentPanel.SetBounds(this.Left, this.Top, this.Width, 0);
                    _currentWCT.Width = this.Width;
                    break;
            }

            // Finally we are ready to show it
            _manager.Container.Resize -= new EventHandler(OnContainerResized);
            _currentPanel.Show();
            _manager.Container.Resize += new EventHandler(OnContainerResized);
                
            // We want to snoop of changes of focus to and from Panel and its children
            MonitorPanel(true);

            // We are showing and not hiding with the timer
            _slideOut = true;
                
            // Kick off the slide timer
            StartSlideTimer();
        }
        
        private void OnPagesLeave(TabStub sender)
        {
            // Do we have anything to dismiss?
            if ((_currentPanel != null) && (_currentWCT != null))
            {
                // Only dimiss if the panel does not have focus
                if (!_currentPanel.ContainsFocus)
                    StartDismissTimer();
            }
        }
                
        private void OnPageClose(object sender, EventArgs e)
        {
            // Find the TabStub instance for the showing WCT
            foreach(TabStub ts in this.Controls)
            {
                // Does this stub match the one showing?
                if (ts.WindowContentTabbed == _currentWCT)
                {
                    ContentCollection cc = new ContentCollection();
                
                    // Get access to Content instance being hidden
                    Content current = _currentWCT.Contents[ts.SelectedIndex];
                    
                    // Check if the hide button is allowed to work
                    if (!_manager.OnContentHiding(current))
                    {
                        // Are we removing the last entry in the WCT?
                        if (ts.TabPages.Count == 1)
                        {
                            // If we still have a current panel
                            if (_currentWCT != null)
                            {
                                // We need to update AutoHide property for all of them
                                foreach (Content c in _currentWCT.Contents)
                                {
                                    // Remember this AutoHide state for persistence
                                    c.RecordAutoHideRestore();

                                    cc.Add(c);
                                }

                                // Remove Panel/WCT from display and stop timers
                                KillDisplayedWindow(_currentWCT);
                            }

                            // Remove the WCT from the WCT
                            ControlHelper.Remove(ts.Controls, ts.WindowContentTabbed);

                            // Remove the stub from this panel
                            ControlHelper.Remove(this.Controls, ts);

                            // Cleanup gracefully
                            ts.Dispose();
                        }
                        else
                        {
                            // Which entry in the stub is currently selected?
                            int index = ts.SelectedIndex;

                            // If we still have a current panel
                            if (_currentWCT != null)
                            {
                                // Remember this AutoHide state for persistence
                                _currentWCT.Contents[index].RecordAutoHideRestore();

                                // Need to update AutoHide property for removed content
                                cc.Add(_currentWCT.Contents[index]);
                            }

                            // Remove just the selected entry from stub
                            ts.TabPages.RemoveAt(index);
                            
                            // If we still have a current panel
                            if (_currentWCT != null)
                            {
                                // Remove the selected entry from WCT
                                _currentWCT.Contents.RemoveAt(index);
                            }
                        }

                        // Content instances no longer in AutoHidden state
                        foreach(Content c in cc)
                        {
                            // No longer in the auto hidden mode                    
                            c.AutoHidden = false;
                            c.AutoHidePanel = null;

                            // Do we need to remove the content entirely?
                            if (c.CloseOnHide)
                            {
                                // Remove the content from the collection
                                if (_manager.Contents.Contains(c))
                                    _manager.Contents.Remove(c);

                                // Dispose of the contained control/form
                                if (c.Control != null)
                                    c.Control.Dispose();
                            }
                        }
                    }
                    
                    // Generate hidden event now content is not visible
                    _manager.OnContentHidden(current);
                    
                    break;
                }
            }
            
            // If no more contents remain then hide
            if (this.Controls.Count == 0)
                this.Hide();
        }
		
		private void OnPageAutoHide(object sender, EventArgs e)
		{
            Form topForm = _manager.Container.FindForm();

            // If we are inside an mdi container
            if (topForm.IsMdiContainer)
            {
                // Shift focus to the active child before removing ourself
                if ((topForm.ActiveMdiChild != null) && !topForm.ActiveMdiChild.ContainsFocus)
                    User32.SetFocus(topForm.ActiveMdiChild.Handle);
            }

			UnAutoHideWindowContent(_currentWCT, false);
			_manager.CheckResized();
			_manager.UpdateInsideFill();
        }

		private void UnAutoHideWindowContent(WindowContentTabbed wct, bool forceKill)
		{
            // Do not generate hiding/hidden/shown events
            _manager.SurpressVisibleEvents += 1;
        
            // Find the TabStub instance for the showing WCT
            foreach(TabStub ts in this.Controls)
            {
                // Does this stub match the one showing?
                if (ts.WindowContentTabbed == wct)
                {
					// Remember which of the contents was the active one
					Content currentContent = ts.TabPages[ts.SelectedIndex].Tag as Content;
					
					int count = ts.TabPages.Count;
                    
                    // Record the auto hide state in reverse order, must record the state
                    // before 'KillDisplayedWindow' as the process of recording state requires
                    // the content to be inside a WindowContent instance
                    for(int i=count-1; i>=0; i--)
                    {
                        // Get access to the content the page represents
                        Content c = ts.TabPages[i].Tag as Content;

                        // Remember this AutoHide state for persistence
                        c.RecordAutoHideRestore();
                    }

                    // Remove Panel/WCT from display and stop timers
					if (forceKill)
						KillDisplayedWindow(wct);
					else
						KillDisplayedWindow(_currentWCT);

                    // Remove the stub from this panel
                    ControlHelper.Remove(this.Controls, ts);
                    
                    // Now that the Window/Panel have been killed we are ready to 
                    // alter the AutoHidden state of each content and restore state
                    for(int i=count-1; i>=0; i--)
                    {
                        // Get access to the content the page represents
                        Content c = ts.TabPages[i].Tag as Content;

                        // No longer in the auto hidden mode                    
                        c.AutoHidden = false;
                        c.AutoHidePanel = null;
                        
                        // Restore into normal docked state
                        _manager.ShowContent(c);
                    }
                        
					// Place the current content in the foreground of restored position
					currentContent.BringToFront();

                    break;
                }
            }
            
            // If no more contents remain then hide
            if (this.Controls.Count == 0)
                this.Hide();

            // Enable generation hiding/hidden/shown events
            _manager.SurpressVisibleEvents -= 1;

            // Notify a change in the layout
            _manager.OnLayoutChanged(EventArgs.Empty);
        }
    
        private void OnPageContextMenu(ContentCollection cc, Point screenPos)
        {
            _manager.OnShowContextMenu(cc, screenPos);
        }

        private void OnSlideTick(object sender, EventArgs e)
        {
            // Is the slide timer supposed to be running?
            if (_slideRunning)
            {            
                // Safety check that timer does not expire after our death
                if (this.IsDisposed || (_currentPanel == null) || 
                    _currentPanel.IsDisposed || (_currentWCT == null) ||
                    _currentWCT.IsDisposed)
                {
                    StopSlideTimer();
                    return;
                }

                // Use the current size/location as the starting point for changes
                Rectangle rect = new Rectangle(_currentPanel.Left, _currentPanel.Top, 
                                               _currentPanel.Width, _currentPanel.Height);
    		    
		        // How big is the resize bar inside the Panel?
                Size barSize = _currentPanel.ResizeBarSize();

                // Is this the last sliding step? 
                // (increase test by 1 because we have not yet incremented it)
                bool lastStep = ((_slideStep+1) >= _slideSteps);

                // Bringing the Panel into view?
                if (_slideOut)
                {
                    // Bring Panel another step into full view
                    switch(this.Dock)
                    {
                        case DockStyle.Left:
                            if (lastStep)
                                rect.Width = _slideRect.Width + barSize.Width;
                            else
                                rect.Width = (_slideRect.Width + barSize.Width) / 
                                              _slideSteps * (_slideStep + 1);
                            
                            // Want the right hand side of WCT showing
                            _currentWCT.Location = new Point(rect.Width - _currentWCT.Width - barSize.Width, 0);
                            break;
                        case DockStyle.Right:
                            int right = _currentPanel.Right;
                            
                            if (lastStep)
                                rect.Width = _slideRect.Width + barSize.Width;
                            else
                                rect.Width = (_slideRect.Width + barSize.Width) / 
                                              _slideSteps * (_slideStep + 1);
                                              
                            rect.X -= rect.Right - right;
                            
                            _currentWCT.Location = new Point(barSize.Width, 0);
                            break;
                        case DockStyle.Top:
                            if (lastStep)
                                rect.Height = _slideRect.Height + barSize.Height;
                            else
                                rect.Height = (_slideRect.Height + barSize.Height) / 
                                               _slideSteps * (_slideStep + 1);
                            
                            // Want the bottom of the WCT showing
                            _currentWCT.Location = new Point(0, rect.Height - _currentWCT.Height - barSize.Height);
                            break;
                        case DockStyle.Bottom:
                            int bottom = _currentPanel.Bottom;
                            
                            if (lastStep)
                                rect.Height = _slideRect.Height + barSize.Height;
                            else
                                rect.Height = (_slideRect.Height + barSize.Height) / 
                                              _slideSteps * (_slideStep + 1);
                                              
                            rect.Y -= rect.Bottom - bottom;

                            _currentWCT.Location = new Point(0, barSize.Height);
                            break;
                    }

                    // Have to use Win32 API call to alter the Panel size and position at the same time, no 
                    // Control method/property is available to do both at the same time. Otherwise you can see
                    // the Panel being moved in two steps which looks naff!
                    User32.MoveWindow(_currentPanel.Handle, rect.Left, rect.Top, rect.Width, rect.Height, true);
                    
                    // Stop timer when all required steps performed		    
                    if (lastStep)
                    {
                        StopSlideTimer();
                        
                        // If sliding into view from bottom
                        if (this.Dock == DockStyle.Top)
                        {
                            // Must cause repaint to prevent artifacts
                            if ((_currentPanel != null) && !_currentPanel.IsDisposed)
								_currentPanel.Refresh();
                        }
                    }
                }
                else
                {
                    int steps = _slideSteps - _slideStep;
                    
                    // Move Window another step towards required position
                    switch(this.Dock)
                    {
                        case DockStyle.Left:
                            if (lastStep)
                                rect.Width = 0;
                            else
                                rect.Width = (_slideRect.Width + barSize.Width) / _slideSteps * steps;
                            break;
                        case DockStyle.Right:
                            int right = _currentPanel.Right;
                            
                            if (lastStep)
                                rect.Width = 0;
                            else
                                rect.Width = (_slideRect.Width + barSize.Width) / _slideSteps * steps;
                                             
                            rect.X += right - rect.Right;
                            break;
                        case DockStyle.Top:
                            if (lastStep)
                                rect.Height = 0;
                            else
                                rect.Height = (_slideRect.Height + barSize.Height) / _slideSteps * steps;
                            break;
                        case DockStyle.Bottom:
                            int bottom = _currentPanel.Bottom;
                            
                            if (lastStep)
                                rect.Height = 0;
                            else
                                rect.Height = (_slideRect.Height + barSize.Height) / _slideSteps * steps;
                                              
                            rect.Y += bottom - rect.Bottom;
                            break;
                    }
                    
                    // Have to use Win32 API call to alter the Panel size and position at the same time, no 
                    // Control method/property is available to do both at the same time. Otherwise you can see
                    // the Panel being moved in two steps which looks naff!
                    User32.MoveWindow(_currentPanel.Handle, rect.Left, rect.Top, rect.Width, rect.Height, true);

                    // Stop timer when all required steps performed		    
                    if (lastStep)
                    {
                        StopSlideTimer();
                        
                        // No longer need to show it
                        RemoveDisplayedWindow();
                        
                        // No longer considered the shown window
                        _currentWCT = null;
                    }
                }
                
                // Increment the step value
                _slideStep++;
            }
        }

        private void OnDismissTick(object sender, EventArgs e)
        {
            // Safety check that timer does not expire after our death
            if (this.IsDisposed || (_currentPanel == null) || _currentPanel.IsDisposed)
            {
                StopDismissTimer();
                return;
            }

            // Should any dismiss attempt from timer be ignored?
            if (!_ignoreDismiss)
            {
                // Are we currently showing a Window?
                if (_currentPanel != null)
                {
                    // Timer is being used to hide the Panel
                    _slideOut = false;
            
                    // Kick off the timer
                    StartSlideTimer();
                }
            }

            // Stop the dismiss timer from reoccuring
            StopDismissTimer();
        }
        
        private void OnContainerResized(object sender, EventArgs e)
        {
            RemoveShowingWindow();
        }        
        
        private void OnContainerChildChanged(object sender, EventArgs e)
        {
			// Cast to correct type
			Control c = sender as Control;
			
			// Only interested if the control is docked
			if (c.Dock != DockStyle.None)
			{
				// Must remove any showing window
				RemoveShowingWindow();
			}
        }
        
		private void OnContainerChildAdded(object sender, ControlEventArgs e)
		{
			e.Control.VisibleChanged += new EventHandler(OnContainerChildChanged);
		}

		private void OnContainerChildRemoved(object sender, ControlEventArgs e)
		{
			e.Control.VisibleChanged -= new EventHandler(OnContainerChildChanged);
		}
		
		private void OnPanelEnter(object sender, EventArgs e)
        {
            _ignoreDismiss = true;
        }

        private void OnPanelLeave(object sender, EventArgs e)
        {
            _ignoreDismiss = false;

            // Is focus leaving the entire WindowContentTabbed control?
            if (!_killing && (_currentWCT != null) && !_currentWCT.ContainsFocus)
            {
                // Ignore the dismiss if the mouse is still within the content window.
                if (_currentWCT.TabControl.ClientRectangle.Contains(_currentWCT.TabControl.PointToClient(Control.MousePosition)))
                    _ignoreDismiss = true;
                else
                {
                    // Do not slide a window in the process of being removed
                    StopDismissTimer();
                    StopSlideTimer();
                    _removeTimer.Start();
                }
            }	
        }

        private void OnRemoveTick(object sender, EventArgs e)
        {
            // Only need the timer to tick once
            _removeTimer.Stop();

            // Remove Panel/WCT from display and stop timers
            RemoveDisplayedWindow();

            // No longer considered the shown window
            _currentWCT = null;
        }
 
        private void MonitorPanel(bool add)
        {
            MonitorControl(_currentPanel, add);
        }

        private void MonitorControl(Control c, bool add)
        {
			try
			{
				if (add)
				{
					// Monitor focus changes on the Control
					c.GotFocus += new EventHandler(OnPanelEnter);
					c.LostFocus += new EventHandler(OnPanelLeave);
				}
				else
				{
					// Unmonitor focus changes on the Control
					c.GotFocus -= new EventHandler(OnPanelEnter);
					c.LostFocus -= new EventHandler(OnPanelLeave);
				}

				foreach(Control child in c.Controls)
					MonitorControl(child, add);
			}
			catch
			{
			}
        }
        
		private void CreateTimers()
		{
			// Define the Sliding timer
			_slideTimer = new Timer();
			_slideTimer.Interval = _slideInterval;
			_slideTimer.Tick += new EventHandler(OnSlideTick);
            
			// Define the Dismiss timer
			_dismissTimer = new Timer();
			_dismissTimer.Interval = _dismissInterval;
			_dismissTimer.Tick += new EventHandler(OnDismissTick);

            // Define the Remove timer
            _removeTimer = new Timer();
            _removeTimer.Interval = _removeInterval;
            _removeTimer.Tick += new EventHandler(OnRemoveTick);
        }

		private void StartDismissTimer()
		{
			// If dismiss timer not running, then start it off
			if (!_dismissRunning)
			{
				// Start the dismiss timer
				_dismissRunning = true;
				_dismissTimer.Start();
			}
		}

		private void StopDismissTimer()
		{
			// Stop the dismiss timer from reoccuring
			_dismissRunning = false;
			_dismissTimer.Stop();
		}
        
		private void StartSlideTimer()
		{
			// If slide timer not running, then start it off
			if (!_slideRunning)
			{
                // If sliding outwards
                if (_slideOut)
                {
                    if (_currentWCT != null)
                    {
                        _manager.OnContentAutoHideOpening(_currentWCT.Contents[_currentWCT.TabControl.SelectedIndex]);
                    }
                }

				// Start the dismiss timer
				_slideStep = 0;
				_slideRunning = true;
				_slideTimer.Start();
			}
		}

		private void StopSlideTimer()
		{
			// Stop the slide timer from reoccuring
			_slideRunning = false;
			_slideTimer.Stop();
		}

		private TabStub TabStubForContent(Content c)
		{
			// Test each of the TabStub child controls
			foreach(TabStub ts in this.Controls)
			{
				// Test each page inside the TabStub
				foreach(Crownwood.DotNetMagic.Controls.TabPage page in ts.TabPages)
				{
					if (c == (page.Tag as Content))
						return ts;
				}
			}
            
			return null;
		}
	}
}
