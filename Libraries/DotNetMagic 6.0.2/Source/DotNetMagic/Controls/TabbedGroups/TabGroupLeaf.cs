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
using System.Xml;
using System.Drawing;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Forms;
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Group that represents a terminal leaf.
	/// </summary>
	public class TabGroupLeaf : TabGroupBase
	{
	    // Class constants
	    private const int IMAGE_WIDTH = 16;
        private const int IMAGE_HEIGHT = 16;
        private const int IMAGE_HORZ_SPLIT = 0;
        private const int IMAGE_VERT_SPLIT = 1;
	
        // Class state
        private static ImageList _internalImages;
        
        // Instance fields
	    private ToolStripMenuItem _mcClose;
        private ToolStripSeparator _mcSep1;
        private ToolStripMenuItem _mcProm;
        private ToolStripMenuItem _mcReba;
        private ToolStripSeparator _mcSep2;
        private ToolStripMenuItem _mcPrev;
        private ToolStripMenuItem _mcNext;
        private ToolStripMenuItem _mcVert;
        private ToolStripMenuItem _mcHorz;
        private Cursor _savedCursor;
        private bool _dragEntered;
        private bool _allowDrop;
		private Color _prominentBackColor;
		private Color _prominentForeColor;
		private DragFeedback _dragFeedback;
		private TargetManager _targetManager;
        private Controls.TabControl _tabControl;

        static TabGroupLeaf()
        {
            // Create a strip of images by loading an embedded bitmap resource
            _internalImages = ResourceHelper.LoadBitmapStrip(Type.GetType("Crownwood.DotNetMagic.Controls.TabbedGroups"),
                                                             "Crownwood.DotNetMagic.Resources.ImagesTabbedGroups.bmp",
                                                             new Size(IMAGE_WIDTH, IMAGE_HEIGHT),
                                                             new Point(0,0));
        }
	
		/// <summary>
		/// Initializes a new instance of the TabGroupLeaf class.
		/// </summary>
		/// <param name="tabbedGroups">Parent control instance.</param>
		/// <param name="parent">Parent group instance.</param>
		public TabGroupLeaf(TabbedGroups tabbedGroups, TabGroupBase parent)
		    : base(tabbedGroups, parent)
		{
            // Discover the actual TabControl derived class to create
            Type tabType = tabbedGroups.OnTabControlType();

			// Create our managed tab control instance
            _tabControl = (Controls.TabControl)Activator.CreateInstance(tabType);
		    
		    // We need to monitor attempts to drag into the tab control
			_allowDrop = true;
			_dragEntered = false;
            _tabControl.AllowDrop = true;
            _tabControl.AllowCtrlTab = false;
			_tabControl.DragOutside = true;
			_tabControl.IDE2005PixelBorder = true;
			_tabControl.IDE2005HeaderBorder = true;
            _tabControl.DragDrop += new DragEventHandler(OnControlDragDrop);
            _tabControl.DragEnter += new DragEventHandler(OnControlDragEnter);
            _tabControl.DragOver += new DragEventHandler(OnControlDragOver);
            _tabControl.DragLeave += new EventHandler(OnControlDragLeave);
		    
		    // Need notification when page drag begins
            _tabControl.PageDragStart += new PageDragStartHandler(OnPageDragStart);
            _tabControl.PageDragMove += new MouseEventHandler(OnPageDragMove);
            _tabControl.PageDragEnd += new PageDragHandler(OnPageDragEnd);
            _tabControl.PageDragQuit += new PageDragHandler(OnPageDragQuit);
		    
		    // Hook into tab page collection events
            _tabControl.TabPages.Cleared += new CollectionClear(OnTabPagesCleared);
            _tabControl.TabPages.Inserted += new CollectionChange(OnTabPagesInserted);
            _tabControl.TabPages.Removed += new CollectionChange(OnTabPagesRemoved);
            
            // Hook into page level events
            _tabControl.GotFocus += new EventHandler(OnGainedFocus);
			_tabControl.PageGotFocus += new EventHandler(OnGainedFocus);
            _tabControl.ClosePressed += new EventHandler(OnClose);     
       
            // Manager only created at start of drag operation
            _targetManager = null;
			_dragFeedback = null;
            
            DefinePopupMenuForControl(_tabControl);

            // Setup the correct 'HideTabsMode' for the control
            Notify(TabGroupBase.NotifyCode.DisplayTabMode);

		    // Define the default setup of TabControl and allow developer to customize
		    _tabbedGroups.OnTabControlCreated(_tabControl);

			// Remember the starting back colour
			_prominentBackColor = _tabControl.BackColor;
			_prominentForeColor = _tabControl.ForeColor;

			// If a leaf is prominent (it cannot be us!) then be disabled
			if (_tabbedGroups.ProminentLeaf != null)
				_tabControl.Enabled = false;
		}

		/// <summary>
		/// Releases all resources used by the group.
		/// </summary>
		public override void Dispose()
		{
			// Must unhook from the menu related events
            _mcClose.Click -= new EventHandler(OnClose);
			_mcProm.Click -= new EventHandler(OnToggleProminent);
			_mcReba.Click -= new EventHandler(OnRebalance);
			_mcHorz.Click -= new EventHandler(OnNewVertical);
			_mcVert.Click -= new EventHandler(OnNewHorizontal);
			_mcNext.Click -= new EventHandler(OnMoveNext);
			_mcPrev.Click -= new EventHandler(OnMovePrevious);
			
			// Must remember to dispose of all resources associated with commands
            _mcClose.Dispose();
            _mcProm.Dispose();
			_mcReba.Dispose();
			_mcHorz.Dispose();
			_mcVert.Dispose();
			_mcNext.Dispose();
			_mcPrev.Dispose();

			// If the tab control was created
			if (_tabControl != null)
			{
				// Raise TabControlRemove event
				_tabbedGroups.OnTabControlRemoved(_tabControl);
			
				// Unhook drag related events
				_tabControl.DragDrop -= new DragEventHandler(OnControlDragDrop);
				_tabControl.DragEnter -= new DragEventHandler(OnControlDragEnter);
				_tabControl.DragOver -= new DragEventHandler(OnControlDragOver);
				_tabControl.DragLeave -= new EventHandler(OnControlDragLeave);

				// Unhook custom drag related events
				_tabControl.PageDragStart -= new PageDragStartHandler(OnPageDragStart);
				_tabControl.PageDragMove -= new MouseEventHandler(OnPageDragMove);
				_tabControl.PageDragEnd -= new PageDragHandler(OnPageDragEnd);
				_tabControl.PageDragQuit -= new PageDragHandler(OnPageDragQuit);

				// Unhook tab page collection events
				_tabControl.TabPages.Cleared -= new CollectionClear(OnTabPagesCleared);
				_tabControl.TabPages.Inserted -= new CollectionChange(OnTabPagesInserted);
				_tabControl.TabPages.Removed -= new CollectionChange(OnTabPagesRemoved);

				// Unhook tab page level events
				_tabControl.GotFocus -= new EventHandler(OnGainedFocus);
				_tabControl.PageGotFocus -= new EventHandler(OnGainedFocus);
				_tabControl.ClosePressed -= new EventHandler(OnClose);            
				_tabControl.ContextMenuStripDisplay -= new CancelEventHandler(OnContextMenuStripDisplay);

				// Get rid of the context menu we added
				if (_tabControl.ContextMenu != null)
					_tabControl.ContextMenu.Dispose();

				// Remove it without hitting close form bug
				ControlHelper.RemoveAll(_tabControl);

				// Dispose of the tab control
				_tabControl.Dispose();
				_tabControl = null;
			}
		}

		/// <summary>
		/// Gets a reference to the contained TabControl instance.
		/// </summary>
		public TabControl TabControl
		{
			get { return _tabControl; }
		}

		/// <summary>
		/// Gets the collection of tab pages contained in leaf.
		/// </summary>
        public TabPageCollection TabPages
        {
            get { return _tabControl.TabPages; }
        }

		/// <summary>
		/// Sets and gets if tabs can be dragged onto this leaf.
		/// </summary>
		public bool AllowDrop
		{
			get { return _allowDrop; }
			set { _allowDrop = value; }
		}

		/// <summary>
		/// Informs group of a notification.
		/// </summary>
		/// <param name="code">Which notification has occured.</param>
        public override void Notify(NotifyCode code)
        {
            switch(code)
            {
				case NotifyCode.ProminentBackColorChanged:
					// If we are the prominent tab leaf
					if (this == _tabbedGroups.ProminentLeaf)
					{
						// Only update colour if we are using the prominent colours
						if (_tabbedGroups.ProminentColors)
						{
							// Update tab control to use prominent colour
							_tabControl.BackColor = _tabbedGroups.ProminentBackColor;
						}
					}
					break;
				case NotifyCode.ProminentForeColorChanged:
					// If we are the prominent tab leaf
					if (this == _tabbedGroups.ProminentLeaf)
					{
						// Only update colour if we are using the prominent colours
						if (_tabbedGroups.ProminentColors)
						{
							// Update tab control to use prominent colour
							_tabControl.ForeColor = _tabbedGroups.ProminentForeColor;
						}
					}
					break;
				case NotifyCode.ProminentChanged:
					// If nothing is prominent then we should be enabled
					if (_tabbedGroups.ProminentLeaf == null)
					{
						if (_tabControl.Enabled && _tabbedGroups.ProminentColors)
						{
                            switch (_tabbedGroups.Style)
                            {
                                case VisualStyle.IDE2005:
                                case VisualStyle.Office2003:
                                case VisualStyle.Office2007Blue:
                                case VisualStyle.Office2007Silver:
                                case VisualStyle.Office2007Black:
                                case VisualStyle.MediaPlayerBlue:
                                case VisualStyle.MediaPlayerOrange:
                                case VisualStyle.MediaPlayerPurple:
                                    if ((_tabbedGroups.ActiveLeaf == this) && (_tabControl.TabPages.Count > 0))
								    {
									    _tabControl.OfficeStyle = _tabbedGroups.OfficeStyleSelected;
                                        _tabControl.MediaPlayerStyle = _tabbedGroups.MediaPlayerStyleSelected;
                                        _tabControl.IDE2005Style = _tabbedGroups.IDE2005StyleSelected;
								    }
								    else
								    {
									    _tabControl.OfficeStyle = _tabbedGroups.OfficeStyleNormal;
                                        _tabControl.MediaPlayerStyle = _tabbedGroups.MediaPlayerStyleNormal;
                                        _tabControl.IDE2005Style = _tabbedGroups.IDE2005StyleNormal;
								    }
                                    break;
                                default:
								    _tabControl.BackColor = _prominentBackColor;
								    _tabControl.ForeColor = _prominentForeColor;
                                    break;
							}
						}

						_tabControl.Enabled = true;
					}
					else
					{
						// Only the prominent leaf should be enabled
						if (this == _tabbedGroups.ProminentLeaf)
						{
							_tabControl.Enabled = true;

							// Remember the old colors
							_prominentBackColor = _tabControl.BackColor;
							_prominentForeColor = _tabControl.ForeColor;

							// Only update colours if we are using the prominent colours
							if (_tabbedGroups.ProminentColors)
							{
                                switch (_tabbedGroups.Style)
                                {
                                    case VisualStyle.IDE2005:
                                    case VisualStyle.Office2003:
                                    case VisualStyle.Office2007Blue:
                                    case VisualStyle.Office2007Silver:
                                    case VisualStyle.Office2007Black:
                                    case VisualStyle.MediaPlayerBlue:
                                    case VisualStyle.MediaPlayerOrange:
                                    case VisualStyle.MediaPlayerPurple:
                                        _tabControl.OfficeStyle = _tabbedGroups.OfficeStyleProminent;
                                        _tabControl.MediaPlayerStyle = _tabbedGroups.MediaPlayerStyleProminent;
                                        _tabControl.IDE2005Style = _tabbedGroups.IDE2005StyleProminent;
                                        break;
                                    default:
									    _tabControl.BackColor = _tabbedGroups.ProminentBackColor;
									    _tabControl.ForeColor = _tabbedGroups.ProminentForeColor;
                                        break;
								}
							}
						}
						else
						{
							// Only update colours if we are using the prominent colours
							if (_tabControl.Enabled && _tabbedGroups.ProminentColors)
							{
                                switch (_tabbedGroups.Style)
                                {
                                    case VisualStyle.IDE2005:
                                    case VisualStyle.Office2003:
                                    case VisualStyle.Office2007Blue:
                                    case VisualStyle.Office2007Silver:
                                    case VisualStyle.Office2007Black:
                                    case VisualStyle.MediaPlayerBlue:
                                    case VisualStyle.MediaPlayerOrange:
                                    case VisualStyle.MediaPlayerPurple:
                                        if ((_tabbedGroups.ActiveLeaf == this) && (_tabControl.TabPages.Count > 0))
									    {
										    _tabControl.OfficeStyle = _tabbedGroups.OfficeStyleSelected;
                                            _tabControl.MediaPlayerStyle = _tabbedGroups.MediaPlayerStyleSelected;
                                            _tabControl.IDE2005Style = _tabbedGroups.IDE2005StyleSelected;
									    }
									    else
									    {
										    _tabControl.OfficeStyle = _tabbedGroups.OfficeStyleNormal;
                                            _tabControl.MediaPlayerStyle = _tabbedGroups.MediaPlayerStyleNormal;
                                            _tabControl.IDE2005Style = _tabbedGroups.IDE2005StyleNormal;
									    }
                                        break;
                                    default:
									    _tabControl.BackColor = _prominentBackColor;
									    _tabControl.ForeColor = _prominentForeColor;
                                        break;
								}
							}

							_tabControl.Enabled = false;
						}
					}
					break;
				case NotifyCode.ImageListChanging:
                    // Are we using the group level imagelist?
                    if (_tabbedGroups.ImageList == _tabControl.ImageList)
                    {   
                        // Then remove its use
                        _tabControl.ImageList = null;
                    }
                    break;
                case NotifyCode.ImageListChanged:
                    // If no imagelist defined
                    if (_tabControl.ImageList == null)
                    {   
                        // Then use the group level one
                        _tabControl.ImageList = _tabbedGroups.ImageList;
                    }
                    break;
                case NotifyCode.StyleChanged:
                    // Update tab control with new style
                    _tabControl.Style = _tabbedGroups.Style;
                    
                    // Update the styles
                    if ((_tabbedGroups.ActiveLeaf == this) && (_tabControl.TabPages.Count > 0))
					{
						_tabControl.OfficeStyle = _tabbedGroups.OfficeStyleSelected;
                        _tabControl.MediaPlayerStyle = _tabbedGroups.MediaPlayerStyleSelected;
                        _tabControl.IDE2005Style = _tabbedGroups.IDE2005StyleSelected;
					}
					else
					{
						_tabControl.OfficeStyle = _tabbedGroups.OfficeStyleNormal;
                        _tabControl.MediaPlayerStyle = _tabbedGroups.MediaPlayerStyleNormal;
                        _tabControl.IDE2005Style = _tabbedGroups.IDE2005StyleNormal;
					}
					break;
                case NotifyCode.DisplayTabMode:
                    // Apply the latest mode
                    switch(_tabbedGroups.DisplayTabMode)
                    {
                        case DisplayTabModes.ShowAll:
                            _tabControl.HideTabsMode = HideTabsModes.ShowAlways;
                            break;
                        case DisplayTabModes.HideAll:
                            _tabControl.HideTabsMode = HideTabsModes.HideAlways;
                            break;
                        case DisplayTabModes.ShowActiveLeaf:
                            _tabControl.HideTabsMode = (_tabbedGroups.ActiveLeaf == this ? HideTabsModes.ShowAlways :
                                                                                           HideTabsModes.HideAlways);
                            break;
                        case DisplayTabModes.ShowMouseOver:
                            _tabControl.HideTabsMode = HideTabsModes.HideWithoutMouse;
                            break;
                        case DisplayTabModes.ShowActiveAndMouseOver:
                            _tabControl.HideTabsMode = (_tabbedGroups.ActiveLeaf == this ? HideTabsModes.ShowAlways :
                                                                                           HideTabsModes.HideWithoutMouse);
                            break;
                        case DisplayTabModes.ShowOnlyMultipleTabs:
                            _tabControl.HideTabsMode = HideTabsModes.HideUsingLogic;
                            break;
                    }
                    break;
				case NotifyCode.HotkeyPrefixChanged:
					// Pass latest value onto the tab control
					_tabControl.HotkeyPrefix = _tabbedGroups.HotkeyPrefix;
					break;
            }
        }

		/// <summary>
		/// Gets the number of child items.
		/// </summary>
        public override int Count               
		{ 
			get { return _tabControl.TabPages.Count; } 
		}
        
		/// <summary>
		/// Gets a value indicating whether the group is a leaf.
		/// </summary>
		public override bool IsLeaf             
		{ 
			get { return true; } 
		}
        
		/// <summary>
		/// Gets a value indicating whether the group is a sequence.
		/// </summary>
		public override bool IsSequence         
		{ 
			get { return false; } 
		}
        
		/// <summary>
		/// Gets the parent control instance.
		/// </summary>
		public override Control GroupControl    
		{ 
			get { return _tabControl; } 
		}
      
		/// <summary>
		/// Returns a value indicating whether the group contains the prominent leaf.
		/// </summary>
		/// <param name="recurse">Should the group search child groups.</param>
		/// <returns>true if prominent leaf contained; otherwise false.</returns>
        public override bool ContainsProminent(bool recurse)
        {
            // Cache the currently selected prominent group
            TabGroupLeaf prominent = _tabbedGroups.ProminentLeaf;

            // Valid value to test against?            
            if (prominent != null)
                return (this == prominent);
            else
                return false;
        }

		/// <summary>
		/// Request this group save its information.
		/// </summary>
		/// <param name="xmlOut">Write to save information into.</param>
        public override void SaveToXml(XmlTextWriter xmlOut)
        {
            // Output standard values appropriate for all Sequence instances
            xmlOut.WriteStartElement("Leaf");
            xmlOut.WriteAttributeString("Count", Count.ToString());
            xmlOut.WriteAttributeString("Unique", Unique.ToString());
            xmlOut.WriteAttributeString("Space", ConversionHelper.DecimalToString(_space));

            // Output each tab page
            foreach(Controls.TabPage tp in _tabControl.TabPages)
            {
                string controlType = "null";
                
                if (_tabbedGroups.SaveControls && tp.Control != null)
                    controlType = tp.Control.GetType().AssemblyQualifiedName;

                xmlOut.WriteStartElement("Page");
                xmlOut.WriteAttributeString("Title", tp.Title);
                xmlOut.WriteAttributeString("ImageList", (tp.ImageList != null).ToString());
                xmlOut.WriteAttributeString("ImageIndex", tp.ImageIndex.ToString());
                xmlOut.WriteAttributeString("Selected", tp.Selected.ToString());
                xmlOut.WriteAttributeString("Control", controlType);
                xmlOut.WriteAttributeString("UniqueName", tp.UniqueName);

                // Give handlers chance to reconstruct the page
                xmlOut.WriteStartElement("CustomPageData");
                _tabbedGroups.OnPageSaving(new TGPageSavingEventArgs(tp, xmlOut));
                xmlOut.WriteEndElement();

                xmlOut.WriteEndElement();
            }
                
            xmlOut.WriteEndElement();
        }

		/// <summary>
		/// Request this group load its information.
		/// </summary>
		/// <param name="xmlIn">Reader to load information from.</param>
        public override void LoadFromXml(XmlTextReader xmlIn)
        {
            // Grab the expected attributes
            string rawCount = xmlIn.GetAttribute(0);
            string rawUnique = xmlIn.GetAttribute(1);
            string rawSpace = xmlIn.GetAttribute(2);

            // Convert to correct types
            int count = Convert.ToInt32(rawCount);
            int unique = Convert.ToInt32(rawUnique);
            Decimal space = ConversionHelper.StringToDecimal(rawSpace);
            
            // Update myself with new values
            Unique = unique;
            _space = space;
            
            // Load each of the children
            for(int i=0; i<count; i++)
            {
                // Read to the first page element or 
                if (!xmlIn.Read())
                    throw new ArgumentException("An element was expected but could not be read in");

                // Must always be a page instance
                if (xmlIn.Name == "Page")
                {
                    Controls.TabPage tp = new Controls.TabPage();

                    // Grab the expected attributes
                    string title = xmlIn.GetAttribute(0);
                    string rawImageList = xmlIn.GetAttribute(1);
                    string rawImageIndex = xmlIn.GetAttribute(2);
                    string rawSelected = xmlIn.GetAttribute(3);
                    string controlType = xmlIn.GetAttribute(4);
                    string uniqueName = string.Empty;

                    if (xmlIn.AttributeCount >= 6)
                        uniqueName = xmlIn.GetAttribute(5);

                    // Convert to correct types
                    bool imageList = Convert.ToBoolean(rawImageList);
                    int imageIndex = Convert.ToInt32(rawImageIndex);
                    bool selected = Convert.ToBoolean(rawSelected);

                    // Setup new page instance
                    tp.Title = title;
                    tp.UniqueName = uniqueName;
                    tp.ImageIndex = imageIndex;
                    tp.Selected = selected;
                    
                    if (imageList)
                        tp.ImageList = _tabbedGroups.ImageList;
                    
                    // Is there a type description of required control?
                    if (controlType != "null")
                    {
                        try
                        {
                            // Get type description, if possible
                            Type t = Type.GetType(controlType);
                            
                            // Was a valid, known type?
                            if (t != null)
                            {
                                // Get the assembly this type is defined inside
                                Assembly a = t.Assembly;
                                
                                if (a != null)
                                {
                                    // Create a new instance form the assemnly
                                    object newObj = a.CreateInstance(t.FullName);
                                    
                                    Control newControl = newObj as Control;
                                    
                                    // Must derive from Control to be of use to us
                                    if (newControl != null)
                                        tp.Control = newControl;
                                }
                            }
                        }
                        catch
                        {
                            // We ignore failure to recreate given control type
                        }
                    }
                    
                    // Read to the custom data element
                    if (!xmlIn.Read())
                        throw new ArgumentException("An element was expected but could not be read in");

                    if (xmlIn.Name != "CustomPageData")
                        throw new ArgumentException("Expected 'CustomPageData' element was not found");

                    bool finished = xmlIn.IsEmptyElement;

                    TGPageLoadingEventArgs e = new TGPageLoadingEventArgs(tp, xmlIn);

                    // Give handlers chance to reconstruct per-page information
                    _tabbedGroups.OnPageLoading(e);
                    
                    // Add into the control unless handler cancelled add operation
                    if (!e.Cancel)
                        _tabControl.TabPages.Add(tp);
                    
                    // Read everything until we get the end of custom data marker
                    while(!finished)
                    {
                        // Check it has the expected name
                        if (xmlIn.NodeType == XmlNodeType.EndElement)
                            finished = (xmlIn.Name == "CustomPageData");

                        if (!finished)
                        {
                            if (!xmlIn.Read())
                                throw new ArgumentException("An element was expected but could not be read in");
                        }
                    } 

                    // Read past the end of page element                    
                    if (!xmlIn.Read())
                        throw new ArgumentException("An element was expected but could not be read in");

                    // Check it has the expected name
                    if (xmlIn.NodeType != XmlNodeType.EndElement)
                        throw new ArgumentException("End of 'page' element expected but missing");
                    
                }
                else
                    throw new ArgumentException("Unknown element was encountered");
            }
        }

        internal void OnClose(object sender, EventArgs e)
        {
            // Is there anything to close? (or the control has been set to generate event always)
            if ((_tabControl.TabPages.Count > 0) || _tabbedGroups.PageCloseWhenEmpty)
            {
                TGCloseRequestEventArgs tge = new TGCloseRequestEventArgs(this, _tabControl, _tabControl.SelectedTab);
            
                // Generate event so handlers can perform appropriate action
                _tabbedGroups.OnPageCloseRequested(tge);
                
                // Still want to close the page? (and there is something to close)
				if (!tge.Cancel && (_tabControl.TabPages.Count > 0))
				{
					// Remove any prominent setting
					if (_tabbedGroups.ProminentLeaf != null)
						_tabbedGroups.ProminentLeaf = null;

					_tabControl.TabPages.Remove(_tabControl.SelectedTab);
				}
            }
        }
        
        internal void OnToggleProminent(object sender, EventArgs e)
        {
            // Toggle the prominent mode
            if (_tabbedGroups.ProminentLeaf == this)
                _tabbedGroups.ProminentLeaf = null;
            else
                _tabbedGroups.ProminentLeaf = this;
        }

        internal void OnRebalance(object sender, EventArgs e)
        {
			// Remove any prominent setting
			if (_tabbedGroups.ProminentLeaf != null)
				_tabbedGroups.ProminentLeaf = null;

            _tabbedGroups.Rebalance();
        }
            
        internal void OnMovePrevious(object sender, EventArgs e)
        {
            // Find the previous leaf node
            TabGroupLeaf prev = _tabbedGroups.PreviousLeaf(this);
            
            // Must always be valid!
			if (prev != null)           
			{
				// Remove any prominent setting
				if (_tabbedGroups.ProminentLeaf != null)
					_tabbedGroups.ProminentLeaf = null;

				MovePageToLeaf(prev);
			}
        }

        internal void OnMoveNext(object sender, EventArgs e)
        {
            // Find the previous leaf node
            TabGroupLeaf next = _tabbedGroups.NextLeaf(this);
            
            // Must always be valid!
			if (next != null)      
			{
				// Remove any prominent setting
				if (_tabbedGroups.ProminentLeaf != null)
					_tabbedGroups.ProminentLeaf = null;

				MovePageToLeaf(next);
			}
        }

        internal void OnNewVertical(object sender, EventArgs e)
        {
            NewVerticalGroup(this, false);
        }

        internal void OnNewHorizontal(object sender, EventArgs e)
        {
            NewHorizontalGroup(this, false);    
        }

        internal void NewVerticalGroup(TabGroupLeaf sourceLeaf, bool before)
        {
            TabGroupSequence tgs = this.Parent as TabGroupSequence;
        
            // We must have a parent sequence!
            if (tgs != null)
            {
				// Remove any prominent setting
				if (_tabbedGroups.ProminentLeaf != null)
					_tabbedGroups.ProminentLeaf = null;

                // Are splitting in the same direction as the parent
                if (tgs.Direction == LayoutDirection.Vertical)
                    AddGroupToSequence(tgs, sourceLeaf, before);
                else
                    SplitGroup(tgs, sourceLeaf, before);
            }
        }
        
        internal void NewHorizontalGroup(TabGroupLeaf sourceLeaf, bool before)
        {
            TabGroupSequence tgs = this.Parent as TabGroupSequence;
        
            // We must have a parent sequence!
            if (tgs != null)
            {
				// Remove any prominent setting
				if (_tabbedGroups.ProminentLeaf != null)
					_tabbedGroups.ProminentLeaf = null;
				
				// Are splitting in the same direction as the parent
                if (tgs.Direction == LayoutDirection.Horizontal)
                    AddGroupToSequence(tgs, sourceLeaf, before);
                else
                    SplitGroup(tgs, sourceLeaf, before);
            }
        }
        
        internal void ControlHorizontalGroup(bool before)
        {
            TabGroupSequence tgs = _tabbedGroups.RootSequence;
        
            // We must have a root sequence!
            if (tgs != null)
            {
                // Are splitting in the same direction as the parent
                if (tgs.Direction == LayoutDirection.Horizontal)
                    AddGroupToRoot(tgs, before);
                else
                    SplitRootGroup(tgs, before);
            }
        }
        
        internal void ControlVerticalGroup(bool before)
        {
            TabGroupSequence tgs = _tabbedGroups.RootSequence;
        
            // We must have a root sequence!
            if (tgs != null)
            {
                // Are splitting in the same direction as the parent
                if (tgs.Direction == LayoutDirection.Vertical)
                    AddGroupToRoot(tgs, before);
                else
                    SplitRootGroup(tgs, before);
            }
        }

        internal void MovePageToLeaf(TabGroupLeaf leaf)
        {
			// Do not process tab changes during processing
			_tabbedGroups.SuspendTabChange();

            // Remember original auto compact mode
            bool autoCompact = _tabbedGroups.AutoCompact;

            // Turn mode off as it interferes with reorganisation
            _tabbedGroups.AutoCompact = false;

            // Get the requested tab page to be moved to new leaf
            TabPage tp = _tabControl.SelectedTab;

            // Remove page from ourself
            _tabControl.TabPages.Remove(tp);
            
            // Add into the new leaf
            leaf.TabPages.Add(tp);

            // Make new leaf the active one
            _tabbedGroups.ActiveLeaf = leaf;
                
            TabControl tc = leaf.GroupControl as Controls.TabControl;
                
            // Select the newly added page
            tc.SelectedTab = tp;
            tc.MakePageVisible(tp);

            // Reset compacting mode as we have updated the structure
            _tabbedGroups.AutoCompact = autoCompact;
            
            // Do we need to compact?
            if (_tabbedGroups.AutoCompact)
                _tabbedGroups.Compact();
		
			// Now safe to process tab change
			_tabbedGroups.ResumeTabChange();
		}

		private void DefinePopupMenuForControl(Controls.TabControl tabControl)
		{
			// Add all the standard menus we manage
            _mcClose = new ToolStripMenuItem("");
            _mcSep1 = new ToolStripSeparator();
            _mcProm = new ToolStripMenuItem("");
            _mcReba = new ToolStripMenuItem("");
            _mcSep2 = new ToolStripSeparator();
            _mcHorz = new ToolStripMenuItem("", _internalImages.Images[IMAGE_HORZ_SPLIT]);
            _mcVert = new ToolStripMenuItem("", _internalImages.Images[IMAGE_VERT_SPLIT]);
            _mcNext = new ToolStripMenuItem("");
            _mcPrev = new ToolStripMenuItem("");

            _mcClose.Click += new EventHandler(OnClose);
            _mcProm.Click += new EventHandler(OnToggleProminent);
            _mcReba.Click += new EventHandler(OnRebalance);
            _mcHorz.Click += new EventHandler(OnNewVertical);
            _mcVert.Click += new EventHandler(OnNewHorizontal);
            _mcNext.Click += new EventHandler(OnMoveNext);
            _mcPrev.Click += new EventHandler(OnMovePrevious);

            // Update command states when shown
			tabControl.ContextMenuStripDisplay += new CancelEventHandler(OnContextMenuStripDisplay);
		}
    
		private void OnGainedFocus(object sender, EventArgs e)
        {
            // This tab control has the focus, make it the active leaf
            _tabbedGroups.ActiveLeaf = this;
        }

        private void OnTabPagesCleared()
        {
            // All pages removed, do we need to compact?
            if (_tabbedGroups.AutoCompact)
                _tabbedGroups.Compact();

            // Mark layout as dirty
            if (_tabbedGroups.AutoCalculateDirty)
                _tabbedGroups.Dirty = true;
        }

        private void OnTabPagesInserted(int index, object value)
        {
            // If there is no currently active leaf then make it us
            if (_tabbedGroups.ActiveLeaf == null)
                _tabbedGroups.ActiveLeaf = this;
                
            // If we are the active leaf then ensure we use the correct style
			if (_tabbedGroups.ActiveLeaf == this)
			{
				_tabControl.OfficeStyle = _tabbedGroups.OfficeStyleSelected;
                _tabControl.MediaPlayerStyle = _tabbedGroups.MediaPlayerStyleSelected;
                _tabControl.IDE2005Style = _tabbedGroups.IDE2005StyleSelected;
			}

            // Mark layout as dirty
            if (_tabbedGroups.AutoCalculateDirty)
                _tabbedGroups.Dirty = true;
        }

        private void OnTabPagesRemoved(int index, object value)
        {
            if (_tabControl.TabPages.Count == 0)
            {
				// No pages, then definitely do not want the selected style				
				_tabControl.OfficeStyle = _tabbedGroups.OfficeStyleNormal;
				_tabControl.IDE2005Style = _tabbedGroups.IDE2005StyleNormal;

                // All pages removed, do we need to compact?
                if (_tabbedGroups.AutoCompact)
                    _tabbedGroups.Compact();
            }

            // Mark layout as dirty
            if (_tabbedGroups.AutoCalculateDirty)
                _tabbedGroups.Dirty = true;
        }
        
        private void OnContextMenuStripDisplay(object sender, CancelEventArgs e)
        {
            bool created = false;

            // Create a new menu strip or clear down the existing one
            if (_tabControl.ContextMenuStrip == null)
            {
                _tabControl.ContextMenuStrip = new ContextMenuStrip();
                created = true;
            }
            else
                _tabControl.ContextMenuStrip.Items.Clear();

            _tabControl.ContextMenuStrip.Closed += new ToolStripDropDownClosedEventHandler(OnContextMenuClosed);
                
            // Add our standard set of menus
            _tabControl.ContextMenuStrip.Items.AddRange(new ToolStripItem[]{_mcClose, _mcSep1, 
                                                                            _mcProm, _mcReba, 
                                                                            _mcSep2, _mcHorz, 
                                                                            _mcVert, _mcNext, 
                                                                            _mcPrev});
            if (!_tabbedGroups.LayoutLock)
            {        
                // Are any pages selected
                bool valid = (_tabControl.SelectedIndex != -1);
            
                // Define the latest text string
                _mcClose.Text = _tabbedGroups.CloseMenuText;
                _mcProm.Text = _tabbedGroups.ProminentMenuText;
                _mcReba.Text = _tabbedGroups.RebalanceMenuText;
                _mcPrev.Text = _tabbedGroups.MovePreviousMenuText;
                _mcNext.Text = _tabbedGroups.MoveNextMenuText;
                _mcVert.Text = _tabbedGroups.NewVerticalMenuText;
                _mcHorz.Text = _tabbedGroups.NewHorizontalMenuText;

				// Define the latest shortcuts
				_mcClose.ShortcutKeys = _tabbedGroups.CloseShortcut;
                _mcProm.ShortcutKeys = _tabbedGroups.ProminentShortcut;
                _mcReba.ShortcutKeys = _tabbedGroups.RebalanceShortcut;
                _mcPrev.ShortcutKeys = _tabbedGroups.MovePreviousShortcut;
                _mcNext.ShortcutKeys = _tabbedGroups.MoveNextShortcut;
                _mcVert.ShortcutKeys = _tabbedGroups.SplitVerticalShortcut;
                _mcHorz.ShortcutKeys = _tabbedGroups.SplitHorizontalShortcut;
                
                // Ensure common entries are visible
				_mcProm.Visible = true;
				_mcReba.Visible = true;
                
                // Only need to close option if the tab has close defined
                _mcClose.Visible = _tabControl.ShowClose && valid;
                _mcSep1.Visible = _tabControl.ShowClose && valid;
                
                // Update the radio button for prominent
                _mcProm.Checked = (_tabbedGroups.ProminentLeaf == this);
                
                // Can only create new group if at least two pages exist
                bool split = valid && (_tabControl.TabPages.Count > 1);
                
                _mcVert.Visible = split;
                _mcHorz.Visible = split;
				_mcSep2.Visible = split;

                // Can only how movement if group exists in that direction
                _mcNext.Visible = valid && (_tabbedGroups.NextLeaf(this) != null);
                _mcPrev.Visible = valid && (_tabbedGroups.PreviousLeaf(this) != null);
				_mcSep2.Visible |= _mcNext.Visible | _mcPrev.Visible;
			}
            else
            {
                // Make sure that none of the menu commands are visible
                _mcClose.Visible = false;
                _mcProm.Visible = false;
                _mcReba.Visible = false;
                _mcPrev.Visible = false;
                _mcNext.Visible = false;
                _mcVert.Visible = false;
                _mcHorz.Visible = false;
                _mcSep1.Visible = false;
                _mcSep2.Visible = false;
            }
                    
            TGContextMenuEventArgs tge = new TGContextMenuEventArgs(this, 
                                                                    _tabControl, 
                                                                    _tabControl.SelectedTab,
                                                                    _tabControl.ContextMenuStrip);
            
            // Generate event so handlers can add/remove/cancel menu
            _tabbedGroups.OnPageContextMenu(tge);
            
            int visibleCommands = 0;
            
            // Count how many visible commands left
            foreach (ToolStripItem tsi in _tabControl.ContextMenuStrip.Items)
                if (tsi.Visible)
                    visibleCommands++;
            
            // Pass back cancel value or always cancel if no commands are visible
            e.Cancel = (tge.Cancel || ((visibleCommands == 0) && !created));

            // If we are not going to show the context menu
            if (e.Cancel)
            {
                // Then we need to clear away the context menu entirely, otherwise the
                // default mouse handler for the tab control will then show the ContextMenuStrip
                // associated with the page, and we don't want that to happen!
                _tabControl.ContextMenuStrip.Items.Clear();
                _tabControl.ContextMenuStrip = null;
            }
        }
        
        private void AddGroupToRoot(TabGroupSequence root, bool before)
        {
			// Do not process tab changes during processing
			_tabbedGroups.SuspendTabChange();
			
			// Remember original auto compact mode
            bool autoCompact = _tabbedGroups.AutoCompact;

            // Turn mode off as it interferes with reorganisation
            _tabbedGroups.AutoCompact = false;

            TabGroupLeaf newGroup = null;

            // New group inserted at start? (or end)                
            if (before)
                newGroup = root.InsertNewLeaf(0);
            else
                newGroup = root.AddNewLeaf();
                     
            TabPage tp = _tabControl.SelectedTab;

            // Remove page from ourself
            _tabControl.TabPages.Remove(tp);
                    
            // Add into the new leaf
            newGroup.TabPages.Add(tp);

			// Make new leaf the active one
			_tabbedGroups.ActiveLeaf = newGroup;
                
			// Reset compacting mode as we have updated the structure
            _tabbedGroups.AutoCompact = autoCompact;

            // Do we need to compact?
            if (_tabbedGroups.AutoCompact)
                _tabbedGroups.Compact();
		
			// Now safe to process tab change
			_tabbedGroups.ResumeTabChange();
            if (tp.Control != null)
                tp.Control.Focus();
            else
                tp.Focus();
        }

        private void AddGroupToSequence(TabGroupSequence tgs, TabGroupLeaf sourceLeaf, bool before)
        {
			// Do not process tab changes during processing
			_tabbedGroups.SuspendTabChange();
			
			// Remember original auto compact mode
            bool autoCompact = _tabbedGroups.AutoCompact;

            // Turn mode off as it interferes with reorganisation
            _tabbedGroups.AutoCompact = false;

            // Find our index into parent collection
            int pos = tgs.IndexOf(this);
                
            TabGroupLeaf newGroup = null;

            // New group inserted before existing one?                
            if (before)
                newGroup = tgs.InsertNewLeaf(pos);
            else
            {
                // No, are we at the end of the collection?
                if (pos == (tgs.Count - 1))
                    newGroup = tgs.AddNewLeaf();
                else
                    newGroup = tgs.InsertNewLeaf(pos + 1);
            }
                     
            // Get tab control for source leaf
            Controls.TabControl tc = sourceLeaf.GroupControl as Controls.TabControl;
                        
            TabPage tp = tc.SelectedTab;

            // Remove page from ourself
            tc.TabPages.Remove(tp);
                    
            // Add into the new leaf
            newGroup.TabPages.Add(tp);

			// Make new leaf the active one
			_tabbedGroups.ActiveLeaf = newGroup;
			
			// Reset compacting mode as we have updated the structure
            _tabbedGroups.AutoCompact = autoCompact;

            // Do we need to compact?
            if (_tabbedGroups.AutoCompact)
                _tabbedGroups.Compact();
		
			// Now safe to process tab change
			_tabbedGroups.ResumeTabChange();
            if (tp.Control != null)
                tp.Control.Focus();
            else
                tp.Focus();
        }

        private void SplitRootGroup(TabGroupSequence root, bool before)
        {
			// Do not process tab changes during processing
			_tabbedGroups.SuspendTabChange();
			
			// Remember original settings
            bool autoCompact = _tabbedGroups.AutoCompact;

            // Turn mode off as it interferes with reorganisation
            _tabbedGroups.AutoCompact = false;
            
            // Prevent extra leaf being created during processing
            _tabbedGroups.SuspendLeafCount();

            // Determine opposite direction for new sequence
            LayoutDirection newDirection = (root.Direction == LayoutDirection.Vertical ? LayoutDirection.Horizontal :
																						 LayoutDirection.Vertical);        
                                                                             
            // Create new top level sequence
            TabGroupSequence newTop = new TabGroupSequence(_tabbedGroups, root, newDirection);

            // Create a new sequence to host all the current root contents
            TabGroupSequence newHost = new TabGroupSequence(_tabbedGroups, newTop, root.Direction);
            
            int count = root.Count;
            
            // Move across each item from current root to new sequence
            for(int i=0; i<count; i++)
            {
                // Always grab the first entry
                TabGroupBase tgb = root[0];
                
                // Remove from root sequence without reallocating space
                root.RemoveAt(0, false, false);

				// Update base to point to new parent
				tgb.SetParent(newHost);
                
                // Add into the new sequence, without allocating space
                newHost.Add(tgb, false);
            }
            
            // Add host into the new top level
            newTop.Add(newHost, true);
                        
			TabGroupLeaf newGroup = null;

			// New group inserted at start? (or end)                
			if (before)
				newGroup = newTop.InsertNewLeaf(0);
			else
				newGroup = newTop.AddNewLeaf();
                     
			TabPage tp = _tabControl.SelectedTab;

			// Remove page from ourself
			_tabControl.TabPages.Remove(tp);
                    
			// Add into the new leaf
			newGroup.TabPages.Add(tp);
			
			// Add the new top level sequence to be the only root item
            root.Add(newTop, true);
			
			// Reset the correct cached setting
            _tabbedGroups.ResumeLeafCount(1);

			// Make new leaf the active one
			_tabbedGroups.ActiveLeaf = newGroup;

			// Reset compacting mode as we have updated the structure
            _tabbedGroups.AutoCompact = autoCompact;

            // Do we need to compact?
            if (_tabbedGroups.AutoCompact)
                _tabbedGroups.Compact();

			// Now safe to process tab change
			_tabbedGroups.ResumeTabChange();
            if (tp.Control != null)
                tp.Control.Focus();
            else
                tp.Focus();
        }
                               
        private void SplitGroup(TabGroupSequence tgs, TabGroupLeaf sourceLeaf, bool before)
        {   
			// Do not process tab changes during processing
			_tabbedGroups.SuspendTabChange();
			
			// Remember original auto compact mode
            bool autoCompact = _tabbedGroups.AutoCompact;
            
            // Turn mode off as it interferes with reorganisation
            _tabbedGroups.AutoCompact = false;
            
            // Determine opposite direction for new sequence
            LayoutDirection newDirection = (tgs.Direction == LayoutDirection.Vertical ? LayoutDirection.Horizontal :
																						LayoutDirection.Vertical);
        
            // Create a new sequence 
            TabGroupSequence newS = new TabGroupSequence(_tabbedGroups, tgs, newDirection);
            
            // Add two new leafs to the new squence
            TabGroupLeaf newGroup1 = newS.AddNewLeaf();
            TabGroupLeaf newGroup2 = newS.AddNewLeaf();
            
            // If the new group is supposed to be before the existing one
            if (before)
            {
                // Then switch them around
                TabGroupLeaf temp = newGroup2;
                newGroup2 = newGroup1;
                newGroup1 = temp;
            }
            
            // Get tab control for source leaf
            Controls.TabControl tc = sourceLeaf.GroupControl as Controls.TabControl;

            // Get the requested tab page to be moved to new group
            TabPage tp = tc.SelectedTab;
            TabPage focusTp = tp;

            // Remove page from ourself
            tc.TabPages.Remove(tp);
                    
            // Add into the Second leaf
            newGroup2.TabPages.Add(tp);
            
            int tabCount = _tabControl.TabPages.Count;
            
            // Transfer remaining pages across to first new leaf
            for(int i=0; i<tabCount; i++)
            {
                tp = _tabControl.TabPages[0];
                _tabControl.TabPages.RemoveAt(0);
                newGroup1.TabPages.Add(tp);
            }
                
            // Remove ourself as group from parent sequence
            tgs.Replace(this, newS);
            
            // Update leaf counter by notifying that this group is removed
            _tabbedGroups.GroupRemoved(this, false);

			// Make the second new group the active one
			_tabbedGroups.ActiveLeaf = newGroup2;

			// Reset compacting mode as we have updated the structure
            _tabbedGroups.AutoCompact = autoCompact;

            // Do we need to compact?
            if (_tabbedGroups.AutoCompact)
                _tabbedGroups.Compact();
		
			// Now safe to process tab change
			_tabbedGroups.ResumeTabChange();
            if (focusTp.Control != null)
                focusTp.Control.Focus();
            else
                focusTp.Focus();

			// Remove resources
			this.Dispose();
		}

        private void OnContextMenuClosed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            // Cast to correct type
            ContextMenuStrip cms = (ContextMenuStrip)sender;

            // Quickly clicking the context menu can cause the context menu strip to be null
            if (_tabControl.ContextMenuStrip != null)
            {
                // Unhook from events
                _tabControl.ContextMenuStrip.Closed -= new ToolStripDropDownClosedEventHandler(OnContextMenuClosed);

                // Remove the context menu
                _tabControl.ContextMenuStrip = null;
            }
        }
        
        private void OnPageDragStart(TabControl sender, TabPage movePage, MouseEventArgs mea)
        {
            // Cannot drag tab pages when the layout is locked
            if (!_tabbedGroups.LayoutLock)
            {        
                // Save the current cursor value
                _savedCursor = _tabControl.Cursor;
                
				// Manager will create hot zones and draw dragging rectangle
				switch(_tabbedGroups.FeedbackStyle)
                {
					case DragFeedbackStyle.Outline:
						_targetManager = new TargetManagerOutline(true, _tabbedGroups, this, _tabControl);
						break;
					case DragFeedbackStyle.Solid:
						_targetManager = new TargetManagerSolid(true, _tabbedGroups, this, _tabControl);
						break;
					case DragFeedbackStyle.Squares:
						_targetManager = new TargetManagerAreas(true, _tabbedGroups, this, _tabControl);
						break;
					case DragFeedbackStyle.Diamonds:
						_targetManager = new TargetManagerAreas(false, _tabbedGroups, this, _tabControl);
						break;
				}
            }
        }
 
        private void OnPageDragMove(object sender, MouseEventArgs e)
        {
            // Cannot drag tab pages when the layout is locked
            if (!_tabbedGroups.LayoutLock)
            {        
                // Convert from Control coordinates to screen coordinates
                Point mousePos = _tabControl.PointToScreen(new Point(e.X, e.Y));

                // Let manager decide on drawing rectangles and setting cursor
                _targetManager.MouseMove(mousePos);
            }
        }

        private void OnPageDragEnd(TabControl sender, MouseEventArgs e)
        {
            // Cannot drag tab pages when the layout is locked
            if (!_tabbedGroups.LayoutLock)
            {
                // Reduce flicker during the position change
                using (AreaObscurer obscurer = new AreaObscurer(_tabbedGroups))
                {
                    // Give manager chance to action request and cleanup
                    _targetManager.Exit();
                }

                // No longer need the manager
                _targetManager = null;
                
                if (_tabControl != null)
                {
                    // Restore the original cursor
                    _tabControl.Cursor = _savedCursor;
                }
            }
        }

        private void OnPageDragQuit(TabControl sender, MouseEventArgs e)
        {
            // Cannot drag tab pages when the layout is locked
            if (!_tabbedGroups.LayoutLock)
            {        
                // Give manager chance to cleanup
                _targetManager.Quit();
            
                // No longer need the manager
                _targetManager = null;

                // Restore the original cursor
                _tabControl.Cursor = _savedCursor;
            }
        }

        private void OnControlDragEnter(object sender, DragEventArgs drgevent)
        {
			// No drop allowed
			drgevent.Effect = DragDropEffects.None;
			
			// Drag has not entered by default
			_dragEntered = false;

			// Cannot drag into control when the layout is locked
            if (!_tabbedGroups.LayoutLock)
            {        
                _dragEntered = ValidFormat(drgevent, true);
            
                // Do we allow the drag to occur?
                if (_dragEntered)
                {
					// Create correct drag feedback class
					_dragFeedback = new DragFeedbackOutline();

					// Inform drag class to start operating
					_dragFeedback.Start(_tabbedGroups.Style);

					// Provide drag feedback now
					DrawDragFeedback();

                    // Update the allowed effects
                    drgevent.Effect = DragDropEffects.Copy;
                }
            }
        }

		private void OnControlDragOver(object sender, DragEventArgs drgevent)
		{
			// No drop allowed by default
			drgevent.Effect = DragDropEffects.None;

			// Cannot drag into control when the layout is locked
			if (!_tabbedGroups.LayoutLock)
			{        
				bool dragEntered = ValidFormat(drgevent, false);
            
				// Change in drag entered state?
				if (dragEntered != _dragEntered)
				{
					if (dragEntered)
						DrawDragFeedback();
					else
						_dragFeedback.DragRectangle(Rectangle.Empty);
				}

				// Remember new state
				_dragEntered = dragEntered;

				// Update the allowed effects
				if (_dragEntered)
					drgevent.Effect = DragDropEffects.Copy;
			}
		}
		
		private void OnControlDragDrop(object sender, DragEventArgs drgevent)
        {
            // Do we allow the drop to occur?
            if (_dragEntered)
            {
				// Remove the drag feedback
				_dragFeedback.Quit();
				_dragFeedback = null;

				if (ValidDragProviderFormat(drgevent))
				{
					// Generate an event so caller can perform required action
					_tabbedGroups.OnExternalDrop(this, _tabControl, GetDragProvider(drgevent));
				}
				else
				{
					if (ValidDropTypeFormat(drgevent))
					{
						// Generate an event so caller can perform required action
						_tabbedGroups.OnExternalDropType(this, _tabControl, GetDragDropType(drgevent));
					}
					else
					{
						// Generate an event so caller can perform own processing of source
						_tabbedGroups.OnExternalDropRaw(this, _tabControl, drgevent);
					}
				}
			}

            _dragEntered = false;
        }

        private void OnControlDragLeave(object sender, EventArgs e)
        {
            // Do we need to remove the drag indicator?
            if (_dragEntered)
			{
				// No longer need feedback
				_dragFeedback.Quit();
				_dragFeedback = null;
			}
                
            _dragEntered = false;
        }
        
        private bool ValidFormat(DragEventArgs e, bool enter)
        {
			// Test for the standard drag provider
			bool valid = ValidDragProviderFormat(e);

			// If not found then try getting any user defined type
			if (!valid && (_tabbedGroups.DropType != null))
				valid = ValidDropTypeFormat(e);

			// If not found yet, then as last resort check the custom processing
			if (!valid)
			{
				// Use appropriate event handler
				if (enter)
					valid = _tabbedGroups.OnExternalDragEnter(this, _tabControl, e);
				else
					valid = _tabbedGroups.OnExternalDragOver(this, _tabControl, e);
			}

			return valid;
        }

		private bool ValidDragProviderFormat(DragEventArgs e)
		{
			return e.Data.GetDataPresent(typeof(TabbedGroups.DragProvider));
		}

		private bool ValidDropTypeFormat(DragEventArgs e)
		{
			return e.Data.GetDataPresent(_tabbedGroups.DropType);
		}
        
        private TabbedGroups.DragProvider GetDragProvider(DragEventArgs e)
        {
            return (TabbedGroups.DragProvider)e.Data.GetData(typeof(TabbedGroups.DragProvider));
        }
        
		private object GetDragDropType(DragEventArgs e)
		{
			return (TabbedGroups.DragProvider)e.Data.GetData(_tabbedGroups.DropType);
		}
		
		private void DrawDragFeedback()
        {
            // Create client rectangle
            Rectangle clientRect = new Rectangle(new Point(0,0), _tabControl.ClientSize);

			// Inform the feedback class the rectangle to be shown
			_dragFeedback.DragRectangle(_tabControl.RectangleToScreen(clientRect));
		}
	}
}
