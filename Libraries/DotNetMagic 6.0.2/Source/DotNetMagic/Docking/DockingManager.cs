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
using System.Xml;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Globalization;
using System.Threading;
using Microsoft.Win32;
using Crownwood.DotNetMagic.Common;
using Crownwood.DotNetMagic.Controls;
using Crownwood.DotNetMagic.Win32;

namespace Crownwood.DotNetMagic.Docking
{
	/// <summary>
	/// Manage a collection of docking content.
	/// </summary>
	public class DockingManager : IDisposable
	{
        // Static Instance fields
        internal static Control _floatingFormContainer;

		// Instance fields
		private bool _zoneMinMax;
		private bool _insideFill;
		private bool _autoResize;
		private bool _firstHalfWidth;
		private bool _firstHalfHeight;
		private bool _floatingInTaskBar;
        private bool _allowTabbed;
        private bool _allowFloating;
		private bool _allowRedocking;
		private bool _allowSideCaptions;
        private bool _allowResize;
		private bool _stubsShowAll;
		private bool _disposed;
        private bool _showUnmatchedOnLoad;
        private bool _apply2007ClearType;
        private bool _applyMediaPlayerClearType;
        private Icon _floatingTaskBarIcon;
		private int _surpressVisibleEvents;
		private int _resizeBarVector;
		private Size _innerMinimum;
		private Color _backColor;
		private Color _activeColor;
		private Color _activeTextColor;
		private Color _inactiveTextColor;
		private Color _resizeBarColor;
		private Font _captionFont;
		private Font _tabControlFont;
		private bool _defaultBackColor;
		private bool _defaultActiveColor;
		private bool _defaultActiveTextColor;
		private bool _defaultInactiveTextColor;
		private bool _defaultResizeBarColor;
		private bool _defaultCaptionFont;
		private bool _defaultTabControlFont;
		private bool _plainTabBorder;
        private string _textHideAll;
        private string _textShowAll;
        private Control _innerControl;
		private Control _outerControl;
		private AutoHidePanel _ahpTop;
		private AutoHidePanel _ahpLeft;
		private AutoHidePanel _ahpBottom;
		private AutoHidePanel _ahpRight;
		private VisualStyle _visualStyle;
        private VisualStyle _externalStyle;
		private DragFeedbackStyle _feedbackStyle;
        private ScrollableControl _container;
		private ManagerContentCollection _contents;
		private OfficeExtender _extender;
        private DockingFactory _factory;

		/// <summary>
		/// Represents the method that will provide content information.
		/// </summary>
		public delegate void ContentHandler(Content c, EventArgs cea);

        /// <summary>
		/// Represents the method that will provide content hiding information.
		/// </summary>
		public delegate void ContentHidingHandler(Content c, CancelEventArgs cea);

		/// <summary>
		/// Represents the method that will provide context menu information.
		/// </summary>
        public delegate void ContextMenuHandler(ContentCollection cc, ContextMenuStrip cms, CancelEventArgs cea);

		/// <summary>
		/// Represents the method that will provide tab control created information.
		/// </summary>
		public delegate void TabControlCreatedHandler(DotNetMagic.Controls.TabControl tabControl);

		/// <summary>
		/// Represents the method that will provide save information.
		/// </summary>
		public delegate void SaveCustomConfigHandler(XmlTextWriter xmlOut);

		/// <summary>
		/// Represents the method that will provide load information.
		/// </summary>
		public delegate void LoadCustomConfigHandler(XmlTextReader xmlIn);

		/// <summary>
		/// Represents the method that handle floating form activation/inactivation.
		/// </summary>
		public delegate void FloatingFormHandler(DockingManager dm, FloatingForm ff);

		/// <summary>
		/// Represents the method that handle window activation/inactivation.
		/// </summary>
		public delegate void WindowHandler(DockingManager dm, Window wd);

		/// <summary>
		/// Represents the method that handle window details activation/inactivation.
		/// </summary>
		public delegate void WindowDetailHandler(DockingManager dm, WindowDetail wd);

		/// <summary>
		/// Represents the method that handles a double clicked caused docking change.
		/// </summary>
		public delegate void DoubleClickDockHandler(DockingManager dm);

        /// <summary>
		/// Occurs when a content is shown.
		/// </summary>
		public event ContentHandler ContentShown;

		/// <summary>
		/// Occurs just after a content has been hidden.
		/// </summary>
		public event ContentHandler ContentHidden;

		/// <summary>
		/// Occurs just before a content is being hidden.
		/// </summary>
		public event ContentHidingHandler ContentHiding;

        /// <summary>
        /// Occurs when a content is about to becoming shown in auto hide mode.
        /// </summary>
        public event ContentHandler ContentAutoHideOpening;

        /// <summary>
        /// Occurs when a content has become invisible in auto hide mode.
        /// </summary>
        public event ContentHandler ContentAutoHideClosed;

        /// <summary>
		/// Occurs when a context menu is required.
		/// </summary>
		public event ContextMenuHandler ContextMenu;

		/// <summary>
		/// Occurs when a new tab control instance is created.
		/// </summary>
		public event TabControlCreatedHandler TabControlCreated;

		/// <summary>
		/// Occurs when a configuration is being saved.
		/// </summary>
		public event SaveCustomConfigHandler SaveCustomConfig;

		/// <summary>
		/// Occurs when a configuration is being loaded.
		/// </summary>
		public event LoadCustomConfigHandler LoadCustomConfig;

		/// <summary>
		/// Occurs when a floating form become active.
		/// </summary>
		public event FloatingFormHandler FloatingFormActivated;

		/// <summary>
		/// Occurs when a window has become active.
		/// </summary>
		public event WindowHandler WindowActivated;

		/// <summary>
		/// Occurs when a window has become inactive.
		/// </summary>
		public event WindowHandler WindowDeactivated;

		/// <summary>
		/// Occurs when a window details has become active.
		/// </summary>
		public event WindowDetailHandler WindowDetailActivated;

		/// <summary>
		/// Occurs when a window details has become inactive.
		/// </summary>
		public event WindowDetailHandler WindowDetailDeactivated;

		/// <summary>
		/// Occurs when a double click has caused docking changes.
		/// </summary>
		public event DoubleClickDockHandler DoubleClickDock;

        /// <summary>
        /// Occurs when the layout is changed.
        /// </summary>
        public event EventHandler LayoutChanged;

        /// <summary>
        /// Occurs when the user double clicks the floating form title area.
        /// </summary>
        public event CancelEventHandler DoubleClickFloatingTitle;
        
        /// <summary>
		/// Initializes a new instance of the DockingManager class.
		/// </summary>
		/// <param name="container">Control to manage.</param>
		/// <param name="vs">Visual style required.</param>
        public DockingManager(ScrollableControl container, VisualStyle vs)
            : this(container, vs, new DockingFactory())
        {
        }

		/// <summary>
		/// Initializes a new instance of the DockingManager class.
		/// </summary>
		/// <param name="container">Control to manage.</param>
		/// <param name="vs">Visual style required.</param>
        /// <param name="factory">Factory used to create control and form instanced.</param>
        public DockingManager(ScrollableControl container, VisualStyle vs, DockingFactory factory)
		{
			// NAG processing
			NAG.NAG_Start();
			
			// Must provide a valid container instance
			if (container == null)
				throw new ArgumentNullException("Container");

            // Remember the initial docking factory
            _factory = factory;

			// Default state
			_container = container;
            _visualStyle = ColorHelper.ValidateStyle(vs);
            _externalStyle = vs;
			_feedbackStyle = DragFeedbackStyle.Squares;
			_innerControl = null;
			_zoneMinMax = true;
			_disposed = false;
			_insideFill = false;
			_autoResize = true;
            _allowTabbed = true;
			_allowFloating = true;
			_allowRedocking = true;
            _allowResize = true;
			_allowSideCaptions = false;
            _showUnmatchedOnLoad = false;
			_stubsShowAll = (vs == VisualStyle.IDE2005);
			_firstHalfWidth = true;
			_firstHalfHeight = true;
			_plainTabBorder = false;
			_floatingInTaskBar = false;
			_floatingTaskBarIcon = null;
			_surpressVisibleEvents = 0;
            _textHideAll = "Hide All";
            _textShowAll = "Show All";
            _apply2007ClearType = true;
            _applyMediaPlayerClearType = true;

			_innerMinimum = new Size(20, 20);
	
			// Default font/resize
			_resizeBarVector = -1;
			_captionFont = new Font(SystemInformation.MenuFont, FontStyle.Regular);
			_tabControlFont = new Font(SystemInformation.MenuFont, FontStyle.Regular);
			_defaultCaptionFont = true;
			_defaultTabControlFont = true;
			
			// Create object to automatically set background color of Controls
			_extender = new OfficeExtender();

			// Create and add hidden auto hide panels
			AddAutoHidePanels();

			// Define initial colors
			ResetColors();

			// Create an object to manage the collection of Content
			_contents = new ManagerContentCollection(this);

			// We want notification when contents are added/removed/cleared
			_contents.Inserted += new CollectionChange(OnContentInserted);
			_contents.Clearing += new CollectionClear(OnContentsClearing);
			_contents.Removed += new CollectionChange(OnContentRemoved);

			// We want to perform special action when container is resized
			_container.Resize += new EventHandler(OnContainerResized);
			
			// A Form can cause the child controls to be reordered after the initialisation
			// but before the Form.Load event. To handle this we hook into the event and force
			// the auto hide panels to be ordered back into their proper place.
			if (_container is Form)
			{   
				Form formContainer = _container as Form;			    
				formContainer.Load += new EventHandler(OnFormLoaded);
			}

			// Need notification when colors change
			Microsoft.Win32.SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(OnPreferenceChanged);
		}

		/// <summary>
		/// Class destructor.
		/// </summary>
		~DockingManager()
		{
			// Must remove resources.
			Dispose(false);
		}

		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			// Release unmanaged resources.
			Dispose(true);

			// No need to finalize as already disposed unmanaged resources.
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Gets the control this docking manager is attached to.
		/// </summary>
        public ScrollableControl Container
		{
			get { return _container; }
		}

        /// <summary>
        /// Gets and sets the factory used to create controls and forms.
        /// </summary>
        public DockingFactory Factory
        {
            get { return _factory; }

            set
            {
                if (value == null)
                    value = new DockingFactory();

                _factory = value;
            }
        }

		/// <summary>
		/// Gets and sets the control inside the edge docked windows.
		/// </summary>
		public Control InnerControl
		{
			get { return _innerControl; }
			set { _innerControl = value; }
		}

		/// <summary>
		/// Gets and sets the first control outside the edge docked windows.
		/// </summary>
		public Control OuterControl
		{
			get { return _outerControl; }
			set 
			{
				if (_outerControl != value)
				{
					_outerControl = value;
				    
					// Use helper routine to ensure panels are in correct positions
					ReorderAutoHidePanels();
				}
			}
		}

		/// <summary>
		/// Gets and sets the collection of content instances.
		/// </summary>
		public ManagerContentCollection Contents
		{
			get { return _contents; }
			
			set 
			{
				_contents.Clear();
				_contents = value;	
			}
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

                // Notify each object in docking hierarchy in case they need to know new value
                PropogateNameValue(PropogateName.Apply2007ClearType, (object)value);
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

                // Notify each object in docking hierarchy in case they need to know new value
                PropogateNameValue(PropogateName.ApplyMediaPlayerClearType, (object)value);
            }
        }
        
        /// <summary>
		/// Gets and sets a value indicating if content can be redocked by the user.
		/// </summary>
		public bool AllowRedocking
		{
			get { return _allowRedocking; }
			set { _allowRedocking = value; }
		}

        /// <summary>
        /// Gets and sets a value indicating if resizing of docking windows is allowed.
        /// </summary>
        public bool AllowResize
        {
            get { return _allowResize; }
            
            set 
            { 
                _allowResize = value;

                // Notify each object in docking hierarchy in case they need to know new value
                PropogateNameValue(PropogateName.AllowResize, (object)value);
            }
        }

		/// <summary>
		/// Gets and sets a value indicating if captions are allowed to be placed at sides.
		/// </summary>
		public bool AllowSideCaptions
		{
			get { return _allowSideCaptions; }
			
			set 
			{ 
				_allowSideCaptions = value; 

				// Notify each object in docking hierarchy in case they need to know new value
				PropogateNameValue(PropogateName.AllowSideCaptions, (object)value);
			}
		}

		/// <summary>
		/// Gets and sets a value indicating if tab stubs show text for all entries or just selected one.
		/// </summary>
		public bool StubsShowAll
		{
			get { return _stubsShowAll; }
			
			set 
			{ 
				_stubsShowAll = value; 

				// Notify each object in docking hierarchy in case they need to know new value
				PropogateNameValue(PropogateName.StubsShowAll, (object)value);
			}
		}

        /// <summary>
        /// Gets and sets a value indicating if a tabbed appearance is allowed for docked content.
        /// </summary>
        public bool AllowTabbed
        {
            get { return _allowTabbed; }
            set { _allowTabbed = value; }
        }

        /// <summary>
        /// Should content instances not found in the loading data be shown or hidde.
        /// </summary>
        public bool ShowUnmatchedOnLoad
        {
            get { return _showUnmatchedOnLoad; }
            set { _showUnmatchedOnLoad = value; }
        }

		/// <summary>
		/// Gets and sets a value indicating if content are allowed in floating state.
		/// </summary>
		public bool AllowFloating
		{
			get { return _allowFloating; }

			set
			{
				if (_allowFloating != value)
				{
					_allowFloating = value;

					// If no longer allows to be floating...
					if (!_allowFloating)
					{
						_container.SuspendLayout();

						ContentCollection cc = new ContentCollection();

						// Make list of all those content that need docking
						foreach(Content c in _contents)
						{
							// Is this content currently floating?
							if (!c.IsDocked)
								cc.Add(c);
						}

						// Process each floating content in turn
						foreach(Content c in cc)
						{
							// Record its current state in case floating is allowed in 
							// the future. Then set its internal state to being docked
							c.RecordFloatingRestore();
							c.Docked = true;
						}

						// Ensure each content is removed from any Parent
						foreach(Content c in cc)
							HideContent(c, false, true, false);
				
						// Now restore each of the Content
						foreach(Content c in cc)
							ShowContent(c);

						UpdateInsideFill();

						_container.ResumeLayout();
					}
				}
			}
		}

		/// <summary>
		/// Gets and sets a value indicating if floating window should appear in taskbar.
		/// </summary>
		public bool FloatingInTaskBar
		{
			get { return _floatingInTaskBar; }
			
			set 
			{ 
				if (_floatingInTaskBar != value)
				{
					_floatingInTaskBar = value;

					// Notify each object in docking hierarchy in case they need to know new value
					PropogateNameValue(PropogateName.FloatingInTaskBar, (object)_floatingInTaskBar);
				}
			}
		}

		/// <summary>
		/// Gets and sets the icon to use when showing a floating window in the taskbar.
		/// </summary>
		public Icon FloatingTaskBarIcon
		{
			get { return _floatingTaskBarIcon; }
			
			set 
			{ 
				if (_floatingTaskBarIcon != value)
				{
					_floatingTaskBarIcon = value;

					// Notify each object in docking hierarchy in case they need to know new value
					PropogateNameValue(PropogateName.FloatingTaskBarIcon, (object)_floatingTaskBarIcon);
				}
			}
		}

		/// <summary>
		/// Gets and sets a value indicating if zones are allowed to maximize windows.
		/// </summary>
		public bool ZoneMinMax
		{
			get { return _zoneMinMax; }

			set 
			{ 
				if (value != _zoneMinMax)
				{
					_zoneMinMax = value;
                
					// Notify each object in docking hierarchy in case they need to know new value
					PropogateNameValue(PropogateName.ZoneMinMax, (object)_zoneMinMax);
				} 
			}
		}

		/// <summary>
		/// Gets and sets a value indicating is the inside area should also be filled by edge docked windows.
		/// </summary>
		public bool InsideFill
		{
			get { return _insideFill; }

			set
			{
				if (_insideFill != value)
				{
					_insideFill = value;

					if (_insideFill)
					{
						// Add Fill style to innermost docking window
						AddInnerFillStyle();
					}
					else
					{
						// Remove Fill style from innermost docking window
						RemoveAnyFillStyle();
						
						// Ensure that inner control can be seen
						OnContainerResized(null, EventArgs.Empty);
					}
				}
			}
		}

		/// <summary>
		/// Gets and sets a value indicating is edge docked windows should resize themselves automatically.
		/// </summary>
		public bool AutoResize
		{
			get { return _autoResize; }
			set { _autoResize = value; }
		}

		/// <summary>
		/// Gets and sets the minimum size we allow for the inner control area.
		/// </summary>
		public Size InnerMinimum
		{
			get { return _innerMinimum; }
			set { _innerMinimum = value; }
		}

		/// <summary>
		/// Gets the visual style.
		/// </summary>
		public VisualStyle Style
		{
			get { return _externalStyle; }
			
			set
			{
                if (_externalStyle != value)
                {
                    // Remove flicker by obscuring the client area during save
                    using (DockingObscurer obscurer = new DockingObscurer(Container))
                    {
                        switch (_visualStyle)
                        {
                            case VisualStyle.Office2003:
                                foreach (Content c in Contents)
                                    _extender.SetOffice2003BackColor(c.Control, OfficeColor.Disable);
                                break;
                            case VisualStyle.Office2007Blue:
                            case VisualStyle.Office2007Silver:
                            case VisualStyle.Office2007Black:
                                foreach (Content c in Contents)
                                    _extender.SetOffice2007BackColor(c.Control, OfficeColor.Disable);
                                break;
                            case VisualStyle.MediaPlayerBlue:
                            case VisualStyle.MediaPlayerOrange:
                            case VisualStyle.MediaPlayerPurple:
                                foreach (Content c in Contents)
                                    _extender.SetMediaPlayerBackColor(c.Control, MediaPlayerColor.Disable);
                                break;
                        }

                        // Use new style value
                        VisualStyle oldStyle = _visualStyle;
                        _visualStyle = ColorHelper.ValidateStyle(value);
                        _externalStyle = value;
                        _extender.Office2007Variant = Office2007VariantFromVisualStyle(_visualStyle);
                        _extender.MediaPlayerVariant = MediaPlayerVariantFromVisualStyle(_visualStyle);

                        // Create the new type of caption details
                        RecreateWindowDetails(oldStyle);

                        switch (_visualStyle)
                        {
                            case VisualStyle.Office2003:
                                foreach (Content c in Contents)
                                    _extender.SetOffice2003BackColor(c.Control, c.Office2003BackColor);
                                break;
                            case VisualStyle.Office2007Blue:
                            case VisualStyle.Office2007Silver:
                            case VisualStyle.Office2007Black:
                                foreach (Content c in Contents)
                                    _extender.SetOffice2007BackColor(c.Control, c.Office2007BackColor);
                                break;
                            case VisualStyle.MediaPlayerBlue:
                            case VisualStyle.MediaPlayerOrange:
                            case VisualStyle.MediaPlayerPurple:
                                foreach (Content c in Contents)
                                    _extender.SetMediaPlayerBackColor(c.Control, c.MediaPlayerBackColor);
                                break;
                        }

                        // Notify each object in docking hierarchy in case they need to know new value
                        PropogateNameValue(PropogateName.Style, (object)_visualStyle);

                        // Update the default stubs value
                        StubsShowAll = (value == VisualStyle.IDE2005);
                    }
                }
			}
		}

		/// <summary>
		/// Gets the feedback style used when dragging.
		/// </summary>
		public DragFeedbackStyle FeedbackStyle
		{
			get { return _feedbackStyle; }
			set { _feedbackStyle = value; }
		}
			
		/// <summary>
		/// Gets and sets the size of resize bar separators.
		/// </summary>
		public int ResizeBarVector
		{
			get { return _resizeBarVector; }
            
			set 
			{
				if (value != _resizeBarVector)
				{
					_resizeBarVector = value;
                    
					// Notify each object in docking hierarchy in case they need to know new value
					PropogateNameValue(PropogateName.ResizeBarVector, (object)_resizeBarVector);
				}
			}
		}

		/// <summary>
		/// Gets and sets the background color.
		/// </summary>
		public Color BackColor
		{
			get { return _backColor; }
            
			set 
			{
				if (value != _backColor)
				{
					_backColor = value;
					_defaultBackColor = (_backColor == SystemColors.Control);
                    
					// Notify each object in docking hierarchy in case they need to know new value
					PropogateNameValue(PropogateName.BackColor, (object)_backColor);
				}
			}
		}
    
		/// <summary>
		/// Gets and sets the active color used in title bars.
		/// </summary>
		public Color ActiveColor
		{
			get { return _activeColor; }
            
			set 
			{
				if (value != _activeColor)
				{
					_activeColor = value;
					_defaultActiveColor = (_activeColor == SystemColors.ActiveCaption);
                    
					// Notify each object in docking hierarchy in case they need to know new value
					PropogateNameValue(PropogateName.ActiveColor, (object)_activeColor);
				}
			}
		}
        
		/// <summary>
		/// Gets and sets the text color used in active title bars.
		/// </summary>
		public Color ActiveTextColor
		{
			get { return _activeTextColor; }
            
			set 
			{
				if (value != _activeTextColor)
				{
					_activeTextColor = value;
					_defaultActiveTextColor = (_activeTextColor == SystemColors.ActiveCaptionText);
                    
					// Notify each object in docking hierarchy in case they need to know new value
					PropogateNameValue(PropogateName.ActiveTextColor, (object)_activeTextColor);
				}
			}
		}

		/// <summary>
		/// Gets and sets the inactive text color using in title bars.
		/// </summary>
		public Color InactiveTextColor
		{
			get { return _inactiveTextColor; }
            
			set 
			{
				if (value != _inactiveTextColor)
				{
					_inactiveTextColor = value;
					_defaultInactiveTextColor = (_inactiveTextColor == SystemColors.ControlText);
                    
					// Notify each object in docking hierarchy in case they need to know new value
					PropogateNameValue(PropogateName.InactiveTextColor, (object)_inactiveTextColor);
				}
			}
		}

		/// <summary>
		/// Gets and sets the color of resize bars.
		/// </summary>
		public Color ResizeBarColor
		{
			get { return _resizeBarColor; }
            
			set 
			{
				if (value != _resizeBarColor)
				{
					_resizeBarColor = value;
					_defaultResizeBarColor = (_resizeBarColor == SystemColors.Control);
                    
					// Notify each object in docking hierarchy in case they need to know new value
					PropogateNameValue(PropogateName.ResizeBarColor, (object)_resizeBarColor);
				}
			}
		}
       
		/// <summary>
		/// Gets and sets the font used in title bars.
		/// </summary>
		public Font CaptionFont
		{
			get { return _captionFont; }
            
			set 
			{
				if (value != _captionFont)
				{
					_captionFont = value;
					
					using(Font testFont = new Font(SystemInformation.MenuFont, FontStyle.Regular))
						_defaultCaptionFont = testFont.Equals(value);
                    
					// Notify each object in docking hierarchy in case they need to know new value
					PropogateNameValue(PropogateName.CaptionFont, (object)_captionFont);
				}
			}
		}

		/// <summary>
		/// Gets and sets the font used tab controls.
		/// </summary>
		public Font TabControlFont
		{
			get { return _tabControlFont; }
            
			set 
			{
				if (value != _tabControlFont)
				{
					_tabControlFont = value;
					
					using(Font testFont = new Font(SystemInformation.MenuFont, FontStyle.Regular))
						_defaultTabControlFont = testFont.Equals(value);
                    
					// Notify each object in docking hierarchy in case they need to know new value
					PropogateNameValue(PropogateName.TabControlFont, (object)_tabControlFont);
				}
			}
		}

		/// <summary>
		/// Gets and sets a value indicating if the tab control in plain style should have a plain border.
		/// </summary>
		public bool PlainTabBorder
		{
			get { return _plainTabBorder; }
            
			set 
			{
				if (value != _plainTabBorder)
				{
					_plainTabBorder = value;
                    
					// Notify each object in docking hierarchy in case they need to know new value
					PropogateNameValue(PropogateName.PlainTabBorder, (object)_plainTabBorder);
				}
			}
		}

		/// <summary>
		/// Reset the colors used by the docking manager.
		/// </summary>
		public void ResetColors()
		{
			_backColor = SystemColors.Control;
			_inactiveTextColor = SystemColors.ControlText;
			_activeColor = SystemColors.ActiveCaption;
			_activeTextColor = SystemColors.ActiveCaptionText;
			_resizeBarColor = SystemColors.Control;
			_defaultBackColor = true;
			_defaultActiveColor = true;
			_defaultActiveTextColor = true;
			_defaultInactiveTextColor = true;
			_defaultResizeBarColor = true;

			PropogateNameValue(PropogateName.BackColor, (object)_backColor);
			PropogateNameValue(PropogateName.ActiveColor, (object)_activeColor);
			PropogateNameValue(PropogateName.ActiveTextColor, (object)_activeTextColor);
			PropogateNameValue(PropogateName.InactiveTextColor, (object)_inactiveTextColor);
			PropogateNameValue(PropogateName.ResizeBarColor, (object)_resizeBarColor);
		}

        /// <summary>
        /// Sets and gets the text to use for the 'Hide All' context menu item.
        /// </summary>
        public string TextHideAll
        {
            get { return _textHideAll; }
            set { _textHideAll = value; }
        }

        /// <summary>
        /// Sets and gets the text to use for the 'Hide All' context menu item.
        /// </summary>
        public string TextShowAll
        {
            get { return _textShowAll; }
            set { _textShowAll = value; }
        }
        
        /// <summary>
		/// Invert the current auto hidden state of the content.
		/// </summary>
		/// <param name="c">Content to invert.</param>
		/// <returns></returns>
		public virtual bool ToggleContentAutoHide(Content c)
		{
			// Content must be visible
			if (!c.Visible)
				return false;

			// Cannot toggle autohide state of a floating window
			// (if visible then content must have a valiud ParentWindowContent)
			if (c.ParentWindowContent.State == State.Floating)
				return false;

			// Is the content currently in auto hide mode?
			if (c.ParentWindowContent.ParentZone == null)
			{
				// Find the hosting panel for the window content instance
				AutoHidePanel ahp = AutoHidePanelForState(c.ParentWindowContent.State);
				
				// Ask the panel to un-autohide
				ahp.InvertAutoHideWindowContent(c.ParentWindowContent as WindowContentTabbed);
				
				// Might need to resize content
				CheckResized();
			}
			else
			{
				// No, so inform the window content to become auto hidden now
				InvertAutoHideWindowContent(c.ParentWindowContent);
			}

			// Success!
			return true;
		}

		/// <summary>
		/// Make the content visible.
		/// </summary>
		/// <param name="c">Content to make visible.</param>
		/// <returns></returns>
		public virtual bool ShowContent(Content c)
		{
			// Validate the incoming Content instance is a valid reference
			// and is a current instance within our internal collection
			if ((c == null) || !_contents.Contains(c))
				return false;
		
			// Remove it from view by removing from current WindowContent container
			if (!c.Visible)
			{
				// Do not generate hiding/hidden/shown events
				_surpressVisibleEvents++;

				// Managing Zones should remove any displayed AutoHide windows
				RemoveShowingAutoHideWindows();
                               
				// Use the assigned restore object to position the Content appropriately
				if (c.Docked || !AllowFloating)
				{
					if (c.AutoHidden)
						c.AutoHideRestore.PerformRestore(this);
					else
						c.DockingRestore.PerformRestore(this);
				}
				else
					c.FloatingRestore.PerformRestore(this);

				// Enable generation hiding/hidden/shown events
				_surpressVisibleEvents--;

				// Generate event
				OnContentShown(c);

				return true;
			}
			else
				return false;
		}

		/// <summary>
		/// Make all content visible.
		/// </summary>
		public virtual void ShowAllContents()
		{
			_container.SuspendLayout();

			foreach(Content c in _contents)
				ShowContent(c);

			UpdateInsideFill();

			_container.ResumeLayout();
		}

		/// <summary>
		/// Make the content invisible.
		/// </summary>
		/// <param name="c">Content to make invisible.</param>
		public virtual void HideContent(Content c)
		{
			HideContent(c, true, true, true);
		}

		/// <summary>
		/// Make the content invisible.
		/// </summary>
		/// <param name="c">Content to make invisible.</param>
		/// <param name="record">Should record its position.</param>
		/// <param name="reorder">Should move to start of content collection.</param>
        /// <param name="closeOnHide">Should the CloseOnHide property be honored.</param>
        public virtual void HideContent(Content c, bool record, bool reorder, bool closeOnHide)
		{
			// Remove it from view by removing from current WindowContent container
			if (c.Visible)
			{
				// Do not generate hiding/hidden/shown events
				_surpressVisibleEvents++;

				// Manageing Zones should remove display AutoHide windows
				RemoveShowingAutoHideWindows();
                
				if (record)
				{
					// Tell the Content to create a new Restore object to record its current location
					c.RecordRestore();
				}

				if (c.AutoHidden)
				{
					// Remove it from its current AutoHidePanel
					c.AutoHidePanel.RemoveContent(c);
				}
				else
				{
					// Remove the Content from its current WindowContent
					c.ParentWindowContent.Contents.Remove(c);
				}
                
				if (reorder)
				{
					// Move the Content to the start of the list
					_contents.SetIndex(0, c); 
				}

				UpdateInsideFill();

				// Enable generation hiding/hidden/shown events
				_surpressVisibleEvents--;

				// Generate event
				OnContentHidden(c);

                // Do we also remove the content?
                if (closeOnHide && c.CloseOnHide)
                {
                    // Remove the content from the collection
                    if (Contents.Contains(c))
                        Contents.Remove(c);

                    // Dispose of the contained control/form
                    if (c.Control != null)
                        c.Control.Dispose();
                }
            }
		}

        /// <summary>
		/// Make all content invisible.
		/// </summary>
        public virtual void HideAllContents()
        {
            HideAllContents(true);
        }

		/// <summary>
		/// Make all content invisible.
		/// </summary>
        /// <param name="closeOnHide">Should close on hide be honored.</param>
		public virtual void HideAllContents(bool closeOnHide)
		{
			_container.SuspendLayout();

			int count = _contents.Count;
			int index = count - 1;

			// Need to loop for each entry that exists
			for(int i=0; i<count; i++)
			{
				// Extract the entry at current index
				Content c = _contents[index];

				// If the content is already hidden				
				if (!c.Visible)
				{
					// Then move one more step towards the front
					index--;
				}
				else
				{
					// Generate event to ask if allowing to hide the content
					if (OnContentHiding(c))
					{
						// Not allowed to hide, so just move to towards the front
						index--;
					}
					else
					{
						// Hide the element and let it be moved to start of collection
                        HideContent(c, true, true, closeOnHide);

                        // If contents have been removed, then 1 less item in the collection
                        if (!Contents.Contains(c))
                            index--;
					}
				}
			}

			UpdateInsideFill();

			_container.ResumeLayout();
		}

		/// <summary>
		/// Create a style specific window to hold the given content.
		/// </summary>
		/// <param name="c">Content to create window for.</param>
		/// <returns>New window instance.</returns>
		public virtual Window CreateWindowForContent(Content c)
		{
			return CreateWindowForContent(c, new EventHandler(OnContentClose), 
											 new EventHandler(OnRestore),
											 new EventHandler(OnInvertAutoHide),
											 new ContextHandler(OnShowContextMenu));
		}

		/// <summary>
		/// Create a style specific window to hold the given content.
		/// </summary>
		/// <param name="c">Content to create window for.</param>
		/// <param name="contentClose">Delegate to handle closing of content.</param>
		/// <param name="restore">Delegate to handle restoring of content.</param>
		/// <param name="invertAutoHide">Delegate to handle inverting auto hidden state.</param>
		/// <param name="showContextMenu">Delegate to handle context menu.</param>
		/// <returns>New window instance.</returns>
		public virtual Window CreateWindowForContent(Content c,
													 EventHandler contentClose,
													 EventHandler restore,
													 EventHandler invertAutoHide,
												 	 ContextHandler showContextMenu)
		{
			// Create new instance with correct style
			WindowContent wc = Factory.CreateWindowContentTabbed(this, _visualStyle);

			WindowDetailCaption wdc;

            switch(_visualStyle)
            {
                case VisualStyle.IDE2005:
    				wdc = Factory.CreateWindowDetailCaptionIDE2005(this, wc, contentClose, restore, invertAutoHide, showContextMenu);
                    break;
                case VisualStyle.Office2003:
    				wdc = Factory.CreateWindowDetailCaptionOffice2003(this, wc, contentClose, restore, invertAutoHide, showContextMenu);
                    break;
                case VisualStyle.Office2007Blue:
                case VisualStyle.Office2007Silver:
                case VisualStyle.Office2007Black:
    				wdc = Factory.CreateWindowDetailCaptionOffice2007(this, wc, contentClose, restore, invertAutoHide, showContextMenu);

                    // Setup the correct clear type drawing setting
                    ((WindowDetailCaptionOffice2007)wdc).Apply2007ClearType = Apply2007ClearType;
                    break;
                case VisualStyle.MediaPlayerBlue:
                case VisualStyle.MediaPlayerOrange:
                case VisualStyle.MediaPlayerPurple:
                    wdc = Factory.CreateWindowDetailCaptionMediaPlayer(this, wc, contentClose, restore, invertAutoHide, showContextMenu);

                    // Setup the correct clear type drawing setting
                    ((WindowDetailCaptionMediaPlayer)wdc).ApplyMediaPlayerClearType = ApplyMediaPlayerClearType;
                    break;
                default:
    				wdc = Factory.CreateWindowDetailCaptionPlain(this, wc, contentClose, restore, invertAutoHide, showContextMenu);
                    break;
            }

            // Add the caption to the window display
			wc.WindowDetails.Add(wdc);

			if (c != null)
			{
				// Add provided Content to this instance
				wc.Contents.Add(c);
			}

			return wc;
		}    
            
		/// <summary>
		/// Create a new zone instance for given state.
		/// </summary>
		/// <param name="zoneState">State of new zone.</param>
		/// <returns>New zone instance.</returns>
		public virtual Zone CreateZoneForContent(State zoneState)
		{
			return CreateZoneForContent(zoneState, _container);
		}

		/// <summary>
		/// Disposes of the resources (other than memory) used by the class.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged; false to release only unmanaged. </param>
		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed && disposing)
			{
				// Do not need to show any slid out autohide windows
				RemoveShowingAutoHideWindows();

				// Remove all the existing content instances
				if (Contents != null)
					Contents.Clear();

				// Is there a container to unhook events from?
				if (Container != null)
				{
					// Always unhook from resize event
					Container.Resize -= new EventHandler(OnContainerResized);
					
					// Unhook from any additional Form specific events
					if (Container is Form)
						(Container as Form).Load -= new EventHandler(OnFormLoaded);
				}

				// No longer hold references to inner/outer controls
				_innerControl = null;
				_outerControl = null;

                // The extender holds onto events so must dispose of it
                _extender.Dispose();

				// Unhook from system event for change in user preferences
				Microsoft.Win32.SystemEvents.UserPreferenceChanged -= 
					new UserPreferenceChangedEventHandler(OnPreferenceChanged);
			}

			// Resources disposed
			_disposed = true;
		}

		/// <summary>
		/// Create a new zone instance for given state.
		/// </summary>
		/// <param name="zoneState">State of new zone.</param>
		/// <param name="destination">Container to attach zone into.</param>
		/// <returns>New zone instance.</returns>
        protected virtual Zone CreateZoneForContent(State zoneState, ScrollableControl destination)
		{
			DockStyle ds;
			LayoutDirection direction;

			// Find relevant values dependant on required state
			ValuesFromState(zoneState, out ds, out direction);

			// Create a new ZoneSequence which can host Content
			ZoneSequence zs = Factory.CreateZoneSequence(this, zoneState, _visualStyle, direction, _zoneMinMax);

			// Set the appropriate docking style
			zs.Dock = ds;

			if (destination != null)
			{
				// Add this Zone to the display
				destination.Controls.Add(zs);
			}

			return zs;
		}

		/// <summary>
		/// Set new content with given initial state but hidden.
		/// </summary>
		/// <param name="c">Content to setup.</param>
		/// <param name="newState">State to setup content in.</param>
		public void SetHiddenState(Content c, State newState)
		{
			// If currently showing the content...
			if (c.Visible)
			{
				// ... then must hide it now
				HideContent(c);
			}

			// Do we want the Content to show in floating mode?
			if (newState == State.Floating)
			{
				// Restore as floating
				if (AllowFloating)
					c.Docked = false;
			}
			else
			{
				// Restore as docked against an edge but not auto hidden
				c.Docked = true;
				c.AutoHidden = false;

				// Create new restore objects for the required edge
				c.AutoHideRestore = new RestoreContentState(newState, c);
				c.DockingRestore = new RestoreContentState(newState, c);
			}
		}

		/// <summary>
		/// Create and show new content with given initial state.
		/// </summary>
		/// <param name="c">Content to show.</param>
		/// <param name="newState">State to show content in.</param>
		/// <returns>New window content instance.</returns>
		public WindowContent AddContentWithState(Content c, State newState)
		{
			// Validate the incoming Content instance is a valid reference
			// and is a current instance within our internal collection
			if ((c == null) || !_contents.Contains(c))
				return null;
		
			// Do not generate hiding/hidden/shown events
			_surpressVisibleEvents++;

			// Prevent floating window being added if not allowed floating windows
			if (!AllowFloating && (newState == State.Floating))
				newState = State.DockLeft;

			// Manageing Zones should remove display AutoHide windows
			RemoveShowingAutoHideWindows();
                
			// Is the window already part of a WindowContent?
			if (c.ParentWindowContent != null)
			{
				// If it used to be in a floating mode, then record state change
				if (c.ParentWindowContent.ParentZone.State == State.Floating)
					c.ContentLeavesFloating();

				// Remove the Content from its current WindowContent
				c.ParentWindowContent.Contents.Remove(c);
			}

			// Create a new Window instance appropriate for hosting a Content object
			Window w = CreateWindowForContent(c);

            ScrollableControl destination = null;

			if (newState != State.Floating)
			{
				destination = _container;
				destination.SuspendLayout();
			}

			// Create a new Zone capable of hosting a WindowContent
			Zone z = CreateZoneForContent(newState, destination);

            if (newState == State.Floating)
            {
                // Content is not in the docked state
                c.Docked = false;

                // destination a new floating form
                DockingManager._floatingFormContainer = this.Container.FindForm();
                destination = Factory.CreateFloatingForm(this, z, new ContextHandler(OnShowContextMenu));

                // ...and its size, add the height of the caption bar to the requested content size
                destination.Size = new Size(c.FloatingSize.Width,
                                            c.FloatingSize.Height + SystemInformation.ToolWindowCaptionHeight);

                // Get the location to show taking into account the number/size of displays might have changed
                destination.Location = ControlHelper.ScreenLocation(c.DisplayLocation, destination.Size);
            }
			
			// Add the Window to the Zone
			z.Windows.Add(w);

			if (newState != State.Floating)
			{
				// Set the Zone to be the least important of our Zones
				ReorderZoneToInnerMost(z);

				UpdateInsideFill();
				CheckResized();

				destination.ResumeLayout();
			}
			else
			{
				// Cast to expected type
				FloatingForm ff = destination as FloatingForm;
				
				// We want it to show now (if possible)
				ff.RequestShow();
			}

			// Enable generation hiding/hidden/shown events
			_surpressVisibleEvents--;

			// Generate event to indicate content is now visible
			OnContentShown(c);

			return w as WindowContent;
		}

		/// <summary>
		/// Add new content to an existing window.
		/// </summary>
		/// <param name="c">Content to be added.</param>
		/// <param name="wc">Existing window to add into.</param>
		/// <returns>Existing window added into.</returns>
		public WindowContent AddContentToWindowContent(Content c, WindowContent wc)
		{
			// Validate the incoming Content instance is a valid reference
			// and is a current instance within our internal collection
			if ((c == null) || !_contents.Contains(c))
				return null;

			// Validate the incoming WindowContent instance is a valid reference
			if (wc == null)
				return null;

			// Is Content already part of given Window then nothing to do
			if (c.ParentWindowContent == wc)
				return wc;
			else
			{
				bool valid = true;

				// Do not generate hiding/hidden/shown events
				_surpressVisibleEvents++;

				// Manageing Zones should remove display AutoHide windows
				RemoveShowingAutoHideWindows();
                
				if (c.ParentWindowContent != null)
				{
					// Is there a change in docking state?
					if (c.ParentWindowContent.ParentZone.State != wc.ParentZone.State)
					{
						// If it used to be in a floating mode, then record state change
						if (c.ParentWindowContent.ParentZone.State == State.Floating)
							c.ContentLeavesFloating();
						else
							c.ContentBecomesFloating();
					}

					// Remove the Content from its current WindowContent
					c.ParentWindowContent.Contents.Remove(c);
				}
				else
				{
					// If a window content is in AutoHide then it will not have a parent zone
					if (wc.ParentZone != null)
					{
						// If adding to a floating window then it is not docked
						if (wc.ParentZone.State == State.Floating)
							c.Docked = false;
					}
					else
					{
						// Cannot dynamically add into an autohide parent
						valid = false;
					}
				}

				if (valid)
				{
					// Add the existing Content to this instance
					wc.Contents.Add(c);
				}

				// Enable generation hiding/hidden/shown events
				_surpressVisibleEvents--;

				if (valid)
				{
					// Generate event to indicate content is now visible
					OnContentShown(c);
				}

				return wc;
			}
		}

		/// <summary>
		/// Create and show a new window inside an existing zone.
		/// </summary>
		/// <param name="c">Content to show.</param>
		/// <param name="z">Zone to add window to.</param>
		/// <param name="index">Index position for insertion.</param>
		/// <returns>New window instance.</returns>
		public Window AddContentToZone(Content c, Zone z, int index)
		{
			// Validate the incoming Content instance is a valid reference
			// and is a current instance within our internal collection
			if ((c == null) || !_contents.Contains(c))
				return null;

			// Validate the incoming Zone instance is a valid reference
			if (z == null) 
				return null;

			// Do not generate hiding/hidden/shown events
			_surpressVisibleEvents++;

			// Manageing Zones should remove display AutoHide windows
			RemoveShowingAutoHideWindows();
                
			// Is the window already part of a WindowContent?
			if (c.ParentWindowContent != null)
			{
				// Is there a change in docking state?
				if (c.ParentWindowContent.ParentZone.State != z.State)
				{
					// If it used to be in a floating mode, then record state change
					if (c.ParentWindowContent.ParentZone.State == State.Floating)
						c.ContentLeavesFloating();
					else
						c.ContentBecomesFloating();
				}

				// Remove the Content from its current WindowContent
				c.ParentWindowContent.Contents.Remove(c);
			}
			else
			{
				// If target zone is floating window then we are no longer docked
				if (z.State == State.Floating)
					c.Docked = false;
			}

			// Create a new WindowContent instance according to our style
			Window w = CreateWindowForContent(c);

			// Add the Window to the Zone at given position
			z.Windows.Insert(index, w);

			// Enable generation hiding/hidden/shown events
			_surpressVisibleEvents--;

			// Generate event to indicate content is now visible
			OnContentShown(c);

			return w;
		}

		/// <summary>
		/// Find remaining inner space left over when all docking windows are taken into account.
		/// </summary>
		/// <param name="source">Extra control to take into account.</param>
		/// <returns>Inner rectangle.</returns>
		public Rectangle InnerResizeRectangle(Control source)
		{
			// Start with a rectangle that represents the entire client area
			Rectangle client = _container.ClientRectangle;

			int count = _container.Controls.Count;
			int inner = _container.Controls.IndexOf(_innerControl);
			int sourceIndex = _container.Controls.IndexOf(source);

			// Process each control outside the inner control
			for(int index=count-1; index>inner; index--)
			{
				Control item = _container.Controls[index];

				// Ignore controls that are not visible
				if (!item.Visible)
					continue;
					
				bool insideSource = (index < sourceIndex);

				switch(item.Dock)
				{
					case DockStyle.Left:
						client.Width -= item.Width;
						client.X += item.Width;

						if (insideSource)
							client.Width -= item.Width;
						break;
					case DockStyle.Right:
						client.Width -= item.Width;

						if (insideSource)
						{
							client.Width -= item.Width;
							client.X += item.Width;
						}
						break;
					case DockStyle.Top:
						client.Height -= item.Height;
						client.Y += item.Height;

						if (insideSource)
							client.Height -= item.Height;
						break;
					case DockStyle.Bottom:
						client.Height -= item.Height;

						if (insideSource)
						{
							client.Height -= item.Height;
							client.Y += item.Height;
						}
						break;
					case DockStyle.Fill:
					case DockStyle.None:
						break;
				}
			}

			return client;
		}

		/// <summary>
		/// Reposition zone to be the innermost control.
		/// </summary>
		/// <param name="zone">Zone to reposition.</param>
		public void ReorderZoneToInnerMost(Zone zone)
		{
			int index = 0;

			// If there is no control specified as the one for all Zones to be placed
			// in front of then simply add the Zone at the start of the list so it is
			// in front of all controls.
			if (_innerControl != null)
			{
				// Find position of specified control and place after it in the list 
				// (hence adding one to the returned value)
				index = _container.Controls.IndexOf(_innerControl) + 1;
			}

			// Find current position of the Zone to be repositioned
			int current = _container.Controls.IndexOf(zone);

			// If the old position is before the new position then we need to 
			// subtract one. As the collection will remove the Control from the
			// old position before inserting it in the new, thus reducing the index
			// by 1 before the insert occurs.
			if (current < index)
				index--;

			// Found a Control that is not a Zone, so need to insert straight it
			_container.Controls.SetChildIndex(zone, index);
            
			// Manageing Zones should remove display AutoHide windows
			RemoveShowingAutoHideWindows();
		}

		/// <summary>
		/// Reposition zone to be the outermost control.
		/// </summary>
		/// <param name="zone">Zone to reposition.</param>
		public void ReorderZoneToOuterMost(Zone zone)
		{
			// Get index of the outer control (minus AutoHidePanel's)
			int index = OuterControlIndex();

			// Find current position of the Zone to be repositioned
			int current = _container.Controls.IndexOf(zone);

			// If the old position is before the new position then we need to 
			// subtract one. As the collection will remove the Control from the
			// old position before inserting it in the new, thus reducing the index
			// by 1 before the insert occurs.
			if (current < index)
				index--;

			// Found a Control that is not a Zone, so need to insert straight it
			_container.Controls.SetChildIndex(zone, index);

			// Manageing Zones should remove display AutoHide windows
			RemoveShowingAutoHideWindows();
		}
        
		/// <summary>
		/// Find the insertion index to maintain the OuterControl property.
		/// </summary>
		/// <returns>Insertion index.</returns>
		public int OuterControlIndex()
		{
			int index = _container.Controls.Count;

			// If there is no control specified as the one for all Zones to be placed behind 
			// then simply add the Zone at the end of the list so it is behind all controls.
			if (_outerControl != null)
			{
				// Find position of specified control and place before it in the list 
				index = _container.Controls.IndexOf(_outerControl);
			}

			// Adjust backwards to prevent being after any AutoHidePanels
			for(; index>0; index--)
				if (!(_container.Controls[index-1] is AutoHidePanel))
					break;
                    
			return index;
		}

		/// <summary>
		/// Retract any showing auto hidden windows.
		/// </summary>
		public void RemoveShowingAutoHideWindows()
		{
			_ahpLeft.RemoveShowingWindow();
			_ahpRight.RemoveShowingWindow();
			_ahpTop.RemoveShowingWindow();
			_ahpBottom.RemoveShowingWindow();
		}
        
		/// <summary>
		/// Make the required content inside an auto hidden window the foremost.
		/// </summary>
		/// <param name="c">Content to bring to the front.</param>
		public void BringAutoHideIntoView(Content c)
		{
			if (_ahpLeft.ContainsContent(c))
				_ahpLeft.BringContentIntoView(c);     

			if (_ahpRight.ContainsContent(c))
				_ahpRight.BringContentIntoView(c);     

			if (_ahpTop.ContainsContent(c))
				_ahpTop.BringContentIntoView(c);     

			if (_ahpBottom.ContainsContent(c))
				_ahpBottom.BringContentIntoView(c);     
		}            
        
		/// <summary>
		/// Saves layout information into an array of bytes using Encoding.Unicode.
		/// </summary>
		/// <returns>Array of bytes.</returns>
		public byte[] SaveConfigToArray()
		{
			return SaveConfigToArray(Encoding.Unicode);	
		}

		/// <summary>
		/// Saves layout information into an array of bytes using caller provided encoding object.
		/// </summary>
		/// <param name="encoding">Encoding object.</param>
		/// <returns>Array of bytes.</returns>
		public byte[] SaveConfigToArray(Encoding encoding)
		{
			// Create a memory based stream
			MemoryStream ms = new MemoryStream();
			
			// Save into the file stream
			SaveConfigToStream(ms, encoding);

			// Must remember to close
			ms.Close();

			// Return an array of bytes that contain the streamed XML
			return ms.GetBuffer();
		}

		/// <summary>
		/// Saves layout information into a named file using Encoding.Unicode.
		/// </summary>
		/// <param name="filename">Filename to create.</param>
		public void SaveConfigToFile(string filename)
		{
			SaveConfigToFile(filename, Encoding.Unicode);
		}

		/// <summary>
		/// Saves layout information into a named file using caller provided encoding object.
		/// </summary>
		/// <param name="filename">Filename to create.</param>
		/// <param name="encoding">Encoding object.</param>
		public void SaveConfigToFile(string filename, Encoding encoding)
		{
			// Create/Overwrite existing file
			FileStream fs = new FileStream(filename, FileMode.Create);
			
			try
			{
				// Save into the file stream
				SaveConfigToStream(fs, encoding);		
			}
			finally
			{
				// Must remember to close
				fs.Close();
			}
		}

		/// <summary>
		/// Saves layout information into a stream object using caller provided encoding object.
		/// </summary>
		/// <param name="stream">Destination stream for saving.</param>
		/// <param name="encoding">Encoding object.</param>
		public void SaveConfigToStream(Stream stream, Encoding encoding)
		{
			XmlTextWriter xmlOut = new XmlTextWriter(stream, encoding); 

			// Use indenting for readability
			xmlOut.Formatting = Formatting.Indented;
			
			// Always begin file with identification and warning
			xmlOut.WriteStartDocument();
			xmlOut.WriteComment(" DotNetMagic, The User Interface library for .NET (www.crownwood.net) ");
			xmlOut.WriteComment(" Modifying this generated file will probably render it invalid ");

            // Use existing method to perform actual output to Xml
            SaveConfigToXml(xmlOut);

			// This should flush all actions and close the file
			xmlOut.WriteEndDocument();
			xmlOut.Close();			
		}

		/// <summary>
		/// Saves layout information using provided xml writer object.
		/// </summary>
		/// <param name="xmlOut">Xml writer object.</param>
		public void SaveConfigToXml(XmlTextWriter xmlOut)
		{
            // Remove flicker by obscuring the client area during save
            using (DockingObscurer obscurer = new DockingObscurer(Container))
            {
                // Remember the current culture setting
                CultureInfo culture = Thread.CurrentThread.CurrentCulture;

                try
                {
                    // Associate a version number with the root element so that future version of the code
                    // will be able to be backwards compatible or at least recognise out of date versions
                    xmlOut.WriteStartElement("DockingConfig");
                    xmlOut.WriteAttributeString("FormatVersion", "10");
                    xmlOut.WriteAttributeString("InsideFill", _insideFill.ToString());
                    xmlOut.WriteAttributeString("InnerMinimum", ConversionHelper.SizeToString(_innerMinimum));
                    xmlOut.WriteAttributeString("SavedAt", DateTime.Now.ToString());

                    // We need to hide all content during the saving process, but then restore
                    // them back again before leaving so the user does not see any change
                    _container.SuspendLayout();

                    // Store a list of those content hidden during processing
                    ContentCollection hideContent = new ContentCollection();

                    // Let create a copy of the current contents in current order, because
                    // we cannot 'foreach' a collection that is going to be altered during its
                    // processing by the 'HideContent'.
                    ContentCollection origContents = _contents.Copy();

                    // Do not generate hiding/hidden/shown events
                    _surpressVisibleEvents++;

                    int count = origContents.Count;

                    // Hide in reverse order so that a ShowAll in forward order gives accurate restore
                    for (int index = count - 1; index >= 0; index--)
                    {
                        Content c = origContents[index];

                        c.RecordRestore();
                        c.SaveToXml(xmlOut);

                        // If visible then need to hide so that subsequent attempts to 
                        // RecordRestore will not take its position into account
                        if (c.Visible)
                        {
                            hideContent.Insert(0, c);
                            HideContent(c, true, true, false);
                        }
                    }

                    // Allow an event handler a chance to add custom information after ours
                    OnSaveCustomConfig(xmlOut);

                    // Put content we hide back again
                    foreach (Content c in hideContent)
                        ShowContent(c);

                    // Enable generation of hiding/hidden/shown events
                    _surpressVisibleEvents--;

                    // Reapply any fill style required
                    AddInnerFillStyle();

                    _container.ResumeLayout();

                    // Terminate the root element and document        
                    xmlOut.WriteEndElement();
                }
                finally
                {
                    // Put back the old culture before existing routine
                    Thread.CurrentThread.CurrentCulture = culture;
                }
            }
        }

		/// <summary>
		/// Loads layout information from given array of bytes.
		/// </summary>
		/// <param name="buffer">Array of byes.</param>
		public void LoadConfigFromArray(byte[] buffer)
		{
			// Create a memory based stream
			MemoryStream ms = new MemoryStream(buffer);

            try
            {
                // Save into the file stream
                LoadConfigFromStream(ms);
            }
            finally
            {
                // Must remember to close
                ms.Close();
            }
		}

		/// <summary>
		/// Loads layout information from given filename.
		/// </summary>
		/// <param name="filename">Filename to open.</param>
		public void LoadConfigFromFile(string filename)
		{
			// Open existing file
			FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
			
			try
			{
				// Load from the file stream
				LoadConfigFromStream(fs);		
			}
			finally
			{
				// Must remember to close
				fs.Close();
			}
		}

		/// <summary>
		/// Loads layout information from given stream object.
		/// </summary>
		/// <param name="stream">Stream to load from.</param>
		public void LoadConfigFromStream(Stream stream)
		{
			XmlTextReader xmlIn = new XmlTextReader(stream); 

			// Ignore whitespace, not interested
			xmlIn.WhitespaceHandling = WhitespaceHandling.None;

			// Moves the reader to the root element.
			xmlIn.MoveToContent();

            try
            {
                // Use existing method to perform actual input from Xml
                LoadConfigFromXml(xmlIn);
            }
            finally
            {
                // Remember to close the no longer needed reader.
                xmlIn.GetRemainder();
                xmlIn.Close();
            }
		}

		/// <summary>
		/// Loads layout information using provided xml reader object.
		/// </summary>
		/// <param name="xmlIn">Reader for loading config.</param>
		public void LoadConfigFromXml(XmlTextReader xmlIn)
		{
            // Remove flicker by obscuring the client area during load
            using (DockingObscurer obscurer = new DockingObscurer(Container))
            {
                // Remember the current culture setting
                CultureInfo culture = Thread.CurrentThread.CurrentCulture;

                try
                {
                    // Double check this has the correct element name
                    if (xmlIn.Name != "DockingConfig")
                        throw new ArgumentException("Root element must be 'DockingConfig'");

                    // Load the format version number
                    string version = xmlIn.GetAttribute(0);
                    string insideFill = xmlIn.GetAttribute(1);
                    string innerSize = xmlIn.GetAttribute(2);

                    // Convert format version from string to double
                    int formatVersion = (int)Convert.ToDouble(version);

                    // We can only load 3 upward version formats
                    if (formatVersion < 3)
                        throw new ArgumentException("Can only load Version 3 and upwards Docking Configuration files");

                    // Convert from string to proper types
                    _insideFill = (bool)Convert.ToBoolean(insideFill);
                    _innerMinimum = ConversionHelper.StringToSize(innerSize);

                    ContentCollection cc = new ContentCollection();

                    if (!xmlIn.IsEmptyElement)
                    {
                        do
                        {
                            // Read the next Element
                            if (!xmlIn.Read())
                                throw new ArgumentException("An element was expected but could not be read in");

                            // Have we reached the end of our element?
                            if ((xmlIn.NodeType == XmlNodeType.EndElement) && (xmlIn.Name == "DockingConfig"))
                                break;

                            // Is the element name 'Content'
                            if (xmlIn.Name == "Content")
                            {
                                // Recreate content instance from XML
                                Content newC = new Content(xmlIn, formatVersion);

                                // Enforce the floating restriction
                                if (!AllowFloating)
                                {
                                    // Make sure it is not going to restore into floating
                                    if (!newC.Docked)
                                        newC.Docked = true;
                                }

                                // Process this Content element
                                cc.Insert(0, newC);
                            }
                            else
                            {
                                // Must have reached end of our code, let the custom handler deal with this
                                OnLoadCustomConfig(xmlIn);

                                // Exit
                                break;
                            }

                        } while (!xmlIn.EOF);
                    }

                    // Reduce flicker during window operations
                    _container.SuspendLayout();

                    // Do not generate hiding/hidden/shown events
                    _surpressVisibleEvents++;

                    // Create a list of all the current content
                    ContentCollection unmatched = Contents.Copy();

                    // Hide all the current content items
                    HideAllContents(false);

                    // Attempt to apply loaded settings
                    foreach (Content loaded in cc)
                    {
                        Content c = null;

                        // Do we have a unique name defined?
                        if ((loaded.UniqueName != null) && (loaded.UniqueName.Length > 0))
                        {
                            // Yes, so search for the same matching unique name
                            c = _contents.FindUniqueName(loaded.UniqueName);
                        }
                        else
                        {
                            // No, so search for the a content with same title instead
                            c = _contents[loaded.Title];
                        }

                        // Do we have any loaded information for this item?
                        if (c != null)
                        {
                            // Copy across the loaded values of interest
                            c.Docked = loaded.Docked;
                            c.AutoHidden = loaded.AutoHidden;
                            c.CaptionBar = loaded.CaptionBar;
                            c.CloseButton = loaded.CloseButton;
                            c.DisplaySize = loaded.DisplaySize;
                            c.DisplayLocation = loaded.DisplayLocation;
                            c.AutoHideSize = loaded.AutoHideSize;
                            c.FloatingSize = loaded.FloatingSize;
                            c.DefaultRestore = loaded.DefaultRestore;
                            c.AutoHideRestore = loaded.AutoHideRestore;
                            c.DockingRestore = loaded.DockingRestore;
                            c.FloatingRestore = loaded.FloatingRestore;

                            // Allow the Restore objects a chance to rehook into object instances
                            c.ReconnectRestore();

                            // Was the loaded item visible?
                            if (loaded.Visible)
                            {
                                // Make it visible now
                                ShowContent(c);
                            }

                            // Remove processed content from unmatched list
                            unmatched.Remove(c);
                        }
                    }

                    // Do we need to show the unmatched entries?
                    if (ShowUnmatchedOnLoad)
                        foreach (Content unmatch in unmatched)
                            ShowContent(unmatch);

                    // Reapply any fill style required
                    AddInnerFillStyle();

                    // Reduce flicker during window operations
                    _container.ResumeLayout();

                    // Enable generate hiding/hidden/shown events
                    _surpressVisibleEvents--;

                    // If any AutoHostPanel's have become visible we need to force a repaint otherwise
                    // the area not occupied by the TabStub instances will be painted the correct color
                    _ahpLeft.Invalidate();
                    _ahpRight.Invalidate();
                    _ahpTop.Invalidate();
                    _ahpBottom.Invalidate();
                }
                finally
                {
                    // Put back the old culture before existing routine
                    Thread.CurrentThread.CurrentCulture = culture;
                }
            }
        }

        /// <summary>
        /// Convert a set of contents into an auto hidden group on one of the docking edges.
        /// </summary>
        /// <param name="cc">Collection of content instances.</param>
        /// <param name="state">Docking edge to become attached to.</param>
        /// <param name="current">Which content should become the selected one.</param>
        public void AutoHideContents(ContentCollection cc, State state, Content current)
        {
            // Cannot convert to floating as floating does not have an auto hidden state
            if (state != State.Floating)
            {
                Form topForm = Container.FindForm();

                // If we are inside an mdi container
                if (topForm.IsMdiContainer)
                {
                    // Shift focus to the active child before removing ourself
                    if ((topForm.ActiveMdiChild != null) && !topForm.ActiveMdiChild.ContainsFocus)
                        User32.SetFocus(topForm.ActiveMdiChild.Handle);
                }

                // Hide all the Content instances. This will cause the restore objects to be 
                // created and so remember the docking positions for when they are restored
                foreach (Content c in cc)
                    HideContent(c, true, true, false);

                AutoHidePanel ahp = AutoHidePanelForState(state);

                // Pass management of Contents into the panel            
                ahp.AddContentsAsGroup(cc, current);

                // Notify a change in the layout
                OnLayoutChanged(EventArgs.Empty);
            }
        }

		internal void OnDoubleClickDock()
		{
			if (DoubleClickDock != null)
				DoubleClickDock(this);
		}

		internal void OnFloatingFormActivated(FloatingForm ff)
		{
			if (FloatingFormActivated != null)
				FloatingFormActivated(this, ff);
		}
	
		internal void OnWindowActivated(Window w)
		{
			if (WindowActivated != null)
				WindowActivated(this, w);
		}
        
		internal void OnWindowDeactivated(Window w)
		{
			if (WindowDeactivated != null)
				WindowDeactivated(this, w);
		}

		internal void OnWindowDetailActivated(WindowDetail wd)
		{
			if (WindowDetailActivated != null)
				WindowDetailActivated(this, wd);
		}
        
		internal void OnWindowDetailDeactivated(WindowDetail wd)
		{
			if (WindowDetailDeactivated != null)
				WindowDetailDeactivated(this, wd);
		}

		internal bool OnContentHiding(Content c)
		{
			CancelEventArgs cea = new CancelEventArgs();

			if (_surpressVisibleEvents == 0)
			{
				// Allow user to prevent hide operation                
				if (ContentHiding != null)
					ContentHiding(c, cea);
			}
            
			// Was action cancelled?                        
			return cea.Cancel;
		}

		internal void OnContentHidden(Content c)
		{
			if (_surpressVisibleEvents == 0)
			{
				// Notify operation has completed
				if (ContentHidden != null)
					ContentHidden(c, EventArgs.Empty);

                // Notify a change in the layout
                OnLayoutChanged(EventArgs.Empty);
			}
		}

		internal void OnContentShown(Content c)
		{
			if (_surpressVisibleEvents == 0)
			{
				// Notify operation has completed
				if (ContentShown != null)
					ContentShown(c, EventArgs.Empty);

                // Notify a change in the layout
                OnLayoutChanged(EventArgs.Empty);
            }
		}

        internal void OnContentAutoHideOpening(Content c)
        {
            if (ContentAutoHideOpening != null)
                ContentAutoHideOpening(c, EventArgs.Empty);
        }

        internal void OnContentAutoHideClosed(Content c)
        {
            if (ContentAutoHideClosed != null)
                ContentAutoHideClosed(c, EventArgs.Empty);
        }
        
        internal void OnTabControlCreated(DotNetMagic.Controls.TabControl tabControl)
		{ 
			// Notify interested parties about creation of a new TabControl instance
			if (TabControlCreated != null)
				TabControlCreated(tabControl);
		}
		
		internal void OnSaveCustomConfig(XmlTextWriter xmlOut)
		{
			// Notify interested parties that they can add their own custom data
			if (SaveCustomConfig != null)
				SaveCustomConfig(xmlOut);
		}

		internal void OnLoadCustomConfig(XmlTextReader xmlIn)
		{
			// Notify interested parties that they can add their own custom data
			if (LoadCustomConfig != null)
				LoadCustomConfig(xmlIn);
		}

        internal void OnLayoutChanged(EventArgs e)
        {
            if (LayoutChanged != null)
                LayoutChanged(this, e);
        }

        internal void OnDoubleClickFloatingTitle(FloatingForm ff, CancelEventArgs ce)
        {
            if (DoubleClickFloatingTitle != null)
                DoubleClickFloatingTitle(ff, ce);
        }

		internal void UpdateInsideFill()
		{
			// Is inside fill ability enabled?
			if (_insideFill)
			{
				// Find the current zone that is 'filler'
				Zone old = FindCurrentFillZone();
				
				// Find the zone that should become the 'filler'
				Zone next = FindFirstFillZone();
				
				// Is there a change to be made?
				if (old != next)
				{
					// Cannot remove from old, if old does not exist!
					if (old != null)
					{
						DockStyle ds;
						LayoutDirection direction;

						// Find relevant values dependant on required state
						ValuesFromState(old.State, out ds, out direction);

						// Prevent the change in docking style from being drawn yet				
						User32.SendMessage(old.Handle, (int)Win32.Msgs.WM_SETREDRAW, 0, 0);

						// Assign old zones correct Dock style
						old.Dock = ds;
					}
					
					// Set new style
					if (next != null)
					{
						// Prevent the change in docking style from being drawn yet				
						User32.SendMessage(next.Handle, (int)Win32.Msgs.WM_SETREDRAW, 0, 0);
						
						// Assign the new fill item
						next.Dock = DockStyle.Fill;
					}
					
					// Now that both docking changes have occured, we can repaint safely				
					if (old != null)
						User32.SendMessage(old.Handle, (int)Win32.Msgs.WM_SETREDRAW, 1, 0);
					
					if (next != null)
						User32.SendMessage(next.Handle, (int)Win32.Msgs.WM_SETREDRAW, 1, 0);

					// Request the contents be invalidated immediately
					RecursiveInvalidate(_container);
				}
			}
		}
		
		internal void RemoveShowingAutoHideWindowsExcept(AutoHidePanel except)
		{
			if (except != _ahpLeft)
				_ahpLeft.RemoveShowingWindow();

			if (except != _ahpRight)
				_ahpRight.RemoveShowingWindow();
            
			if (except != _ahpTop)
				_ahpTop.RemoveShowingWindow();
            
			if (except != _ahpBottom)
				_ahpBottom.RemoveShowingWindow();
		}

		internal void OnContentInserted(int index, object value)
		{
			Content c = value as Content;

			if (c != null)
			{
				// Hook into changes in the Office2003BackColor property
				c.PropertyChanged += new Content.PropChangeHandler(OnContentPropertyChanged);

                switch (_visualStyle)
                {
                    case VisualStyle.Office2003:
                        _extender.SetOffice2003BackColor(c.Control, c.Office2003BackColor);
                        break;
                    case VisualStyle.Office2007Blue:
                    case VisualStyle.Office2007Silver:
                    case VisualStyle.Office2007Black:
                        _extender.SetOffice2007BackColor(c.Control, c.Office2007BackColor);
                        break;
                    case VisualStyle.MediaPlayerBlue:
                    case VisualStyle.MediaPlayerOrange:
                    case VisualStyle.MediaPlayerPurple:
                        _extender.SetMediaPlayerBackColor(c.Control, c.MediaPlayerBackColor);
                        break;
                }
			}
		}
		
		internal void OnContentsClearing()
		{
			_container.SuspendLayout();

			foreach(Content c in _contents)
			{
				// Unhook from property changes
				c.PropertyChanged -= new Content.PropChangeHandler(OnContentPropertyChanged);

				// No longer need the Content inside the extender
				if (_visualStyle == VisualStyle.Office2003)
					_extender.RemoveControl(c.Control);
			}

			// Hide them all will gracefully remove them from view
			HideAllContents();

			_container.ResumeLayout();
		}

		internal void OnContentRemoved(int index, object value)
		{
			_container.SuspendLayout();

			Content c = value as Content;

			// Hide the content will gracefully remove it from view
			if (c != null)
			{
				// Unhook from property changes
				c.PropertyChanged -= new Content.PropChangeHandler(OnContentPropertyChanged);
			
				// No longer need the Content inside the extender
				if (_visualStyle == VisualStyle.Office2003)
					_extender.RemoveControl(c.Control);
			
				HideContent(c, true, false, true);
			}

			_container.ResumeLayout();
		}

		internal void OnContentClose(object sender, EventArgs e)
		{
			WindowDetailCaption wdc = sender as WindowDetailCaption;
            
			// Was Close generated by a Caption detail?
			if (wdc != null)
			{
				WindowContentTabbed wct = wdc.ParentWindow as WindowContentTabbed;
                
				// Is the Caption part of a WindowContentTabbed object?
				if (wct != null)
				{
					// Find the Content object that is the target
					Content c = wct.CurrentContent;
                    
					if (c != null)
					{
						// Was action cancelled?                        
                        if (!OnContentHiding(c))
                        {
                            Form topForm = Container.FindForm();

                            // If we are inside an mdi container
                            if (topForm.IsMdiContainer)
                            {
                                // Shift focus to the active child before removing ourself
                                if ((topForm.ActiveMdiChild != null) && !topForm.ActiveMdiChild.ContainsFocus)
                                    User32.SetFocus(topForm.ActiveMdiChild.Handle);
                            }

                            wct.HideCurrentContent();
                        }
					}
				}
			}
		}
        
		internal void OnInvertAutoHide(object sender, EventArgs e)
		{
			WindowDetail detail = sender as WindowDetail;

			// Get access to Content that initiated AutoHide for its Window
			WindowContent wc = detail.ParentWindow as WindowContent;
                        
			// Make the window content auto hide
			InvertAutoHideWindowContent(wc);
		}

        internal void OnShowContextMenu(ContentCollection cc, Point screenPos)
		{
            ContextMenuStrip context = new ContextMenuStrip();

			// The order of Content displayed in the context menu is not the same as
			// the order of Content in the _contents collection. The latter has its
			// ordering changed to enable Restore functionality to work.
			ContentCollection temp = new ContentCollection();

			foreach(Content c in _contents)
			{
				int count = temp.Count;
				int index = 0;

				// Find best place to add into the temp collection
				for(; index<count; index++)
				{
					if (c.Order < temp[index].Order)
						break;
				}

				temp.Insert(index, c);
			}

			// Create a context menu entry per Content
			foreach(Content t in temp)
			{
                ToolStripMenuItem tsmi = new ToolStripMenuItem(t.Title);
                tsmi.Click += new EventHandler(OnToggleContentVisibility);
                tsmi.Checked = t.Visible;
                tsmi.Tag = t;
                context.Items.Add(tsmi);
			}

			// Add a separator 
            context.Items.Add(new ToolStripSeparator());

			// Add fixed entries to end to effect all content objects
            ToolStripMenuItem showAll = new ToolStripMenuItem(TextShowAll);
            ToolStripMenuItem hideAll = new ToolStripMenuItem(TextHideAll);
            showAll.Click += new EventHandler(OnShowAll);
            hideAll.Click += new EventHandler(OnHideAll);
            context.Items.Add(showAll);
            context.Items.Add(hideAll);

            if (OnContextMenu(cc, context))
				context.Show(screenPos);
		}

		internal void InvertAutoHideWindowContent(WindowContent wc)
		{
			// Do not generate hiding/hidden/shown events
			_surpressVisibleEvents++;

			// Create a collection of the Content in the same window
			ContentCollection cc = new ContentCollection();
	            
			// Add all Content into collection
			foreach(Content c in wc.Contents)
				cc.Add(c);

			// Add to the correct AutoHidePanel
			AutoHideContents(cc, wc.State, wc.CurrentContent);

			// Enable generate hiding/hidden/shown events
			_surpressVisibleEvents--;
        }

		internal AutoHidePanel AutoHidePanelForState(State state)
		{
			AutoHidePanel ahp = null;

			// Grab the correct hosting panel
			switch(state)
			{
				case State.DockLeft:
					ahp = _ahpLeft;
					break;
				case State.DockRight:
					ahp = _ahpRight;
					break;
				case State.DockTop:
					ahp = _ahpTop;
					break;
				case State.DockBottom:
					ahp = _ahpBottom;
					break;
			}

			return ahp;
		}
        
		internal AutoHidePanel AutoHidePanelForContent(Content c)
		{
			if (_ahpLeft.ContainsContent(c))
				return _ahpLeft;     

			if (_ahpRight.ContainsContent(c))
				return _ahpRight;     

			if (_ahpTop.ContainsContent(c))
				return _ahpTop;     

			if (_ahpBottom.ContainsContent(c))
				return _ahpBottom;     
                
			return null;
		}

		internal int SurpressVisibleEvents
		{
			get { return _surpressVisibleEvents; }
			set { _surpressVisibleEvents = value; }
		}

		internal void ValuesFromState(State newState, out DockStyle dockState, out LayoutDirection direction)
		{
			switch(newState)
			{
				case State.Floating:
					dockState = DockStyle.Fill;
					direction = LayoutDirection.Vertical;
					break;
				case State.DockTop:
					dockState = DockStyle.Top;
					direction = LayoutDirection.Horizontal;
					break;
				case State.DockBottom:
					dockState = DockStyle.Bottom;
					direction = LayoutDirection.Horizontal;
					break;
				case State.DockRight:
					dockState = DockStyle.Right;
					direction = LayoutDirection.Vertical;
					break;
				case State.DockLeft:
				default:
					dockState = DockStyle.Left;
					direction = LayoutDirection.Vertical;
					break;
			}
		}

		internal bool DefaultBackColor
		{
			get{ return _defaultBackColor; }
		}
		
		internal bool DefaultActiveColor 
		{
			get { return _defaultActiveColor; }
		}
		
		internal bool DefaultActiveTextColor 
		{
			get { return _defaultActiveTextColor; }
		}
		
		internal bool DefaultInactiveTextColor 
		{
			get { return _defaultInactiveTextColor; }
		}
		
		internal bool DefaultResizeBarColor 
		{
			get { return _defaultResizeBarColor; }
		}
		
		internal bool DefaultCaptionFont 
		{
			get { return _defaultCaptionFont; }
		}

		internal bool DefaultTabControlFont 
		{
			get { return _defaultTabControlFont; }
		}
		
		private void AddAutoHidePanels()
		{
			// Create an instance for each container edge (they default to being hidden)
			_ahpTop = Factory.CreateAutoHidePanel(this, DockStyle.Top);
            _ahpLeft = Factory.CreateAutoHidePanel(this, DockStyle.Left);
            _ahpBottom = Factory.CreateAutoHidePanel(this, DockStyle.Bottom);
            _ahpRight = Factory.CreateAutoHidePanel(this, DockStyle.Right);
        
			_ahpTop.Name = "Top";
			_ahpLeft.Name = "Left";
			_ahpBottom.Name = "Bottom";
			_ahpRight.Name = "Right";
		    
			// Add to the end of the container we manage
			_container.Controls.AddRange(new Control[]{_ahpBottom, _ahpTop, _ahpRight, _ahpLeft});
		}
		            
		private void RepositionControlBefore(Control target, Control source)
		{
			// Find indexs of the two controls
			int targetPos = _container.Controls.IndexOf(target);
			int sourcePos = _container.Controls.IndexOf(source);

			// If the source is being moved further up the list then we must decrement the target index 
			// as the move is carried out in two phases. First the source control is removed from the 
			// collection and then added at the given requested index. So when insertion point needs 
			// ahjusting to reflec the fact the control has been removed before being inserted.
			if (targetPos >= sourcePos)
				targetPos--;

			_container.Controls.SetChildIndex(source, targetPos);			
		}

		private void OnRestore(object sender, EventArgs e)
		{
			WindowDetailCaption wdc = sender as WindowDetailCaption;

			// Was Restore generated by a Caption detail?
			if (wdc != null)
			{
				WindowContent wc = wdc.ParentWindow as WindowContent;

				// Is the Caption part of a WindowContent object?
				if (wc != null)
				{
					// Remember the currently active content
					Content activeContent = wc.CurrentContent;
					
					ContentCollection copy = new ContentCollection();

					// Make every Content of the WindowContent record its
					// current position and remember it for the future
					foreach(Content c in wc.Contents)
					{
						// Make record of its current docking state
						c.RecordRestore();

						// Invert docked status
						c.Docked = (c.Docked == false);

						// Make a note to process this record
						copy.Add(c);
						
						// If using inside fill then stop the window from being painted
						if (InsideFill)
							User32.SendMessage(c.Control.Handle, (int)Win32.Msgs.WM_SETREDRAW, 0, 0);
					}

					int copyCount = copy.Count;

					// Must have at least one!
					if (copyCount >= 1)
					{
						// Remove from current WindowContent and restore its position
						HideContent(copy[0], false, true, false);
						ShowContent(copy[0]);

						// Any other content to be moved along with it?
						if (copyCount >= 2)
						{
							WindowContent newWC = copy[0].ParentWindowContent;

							if (newWC != null)
							{
								// Transfer each one to its new location
								for(int index=1; index<copyCount; index++)
								{
									HideContent(copy[index], false, true, false);
									newWC.Contents.Add(copy[index]);
								}
							}
						}
						
						// Make the old active content active again
						activeContent.BringToFront();
						
						// If using inside fill then stop the window from being painted
						if (InsideFill)
						{
							// Ensure the fill style is correct
							UpdateInsideFill();

							// Now we can paint again
							foreach(Content c in copy)
							{
								// Allow repaint for the content
								User32.SendMessage(c.Control.Handle, (int)Win32.Msgs.WM_SETREDRAW, 1, 0);
								
								// Request the contents be invalidated immediately
								RecursiveInvalidate(c.Control);
							}
						}
					}
				}
			}
		}

		private void RecursiveInvalidate(Control top)
		{
			// Must invalidate the control itself
			top.Invalidate();
			
			// Recurse to invalidate all children as well
			foreach(Control child in top.Controls)
				RecursiveInvalidate(child);
		}

		private void AddInnerFillStyle()
		{
			if (_insideFill)
			{
				Zone z = FindFirstFillZone();
			
				if (z != null)
				{
					// Make it fill all remaining space
					z.Dock = DockStyle.Fill;
				}
			}
		}
		
		private void RemoveAnyFillStyle()
		{
			// Get the Zone that is currently the 'Filler'
			Zone z = FindCurrentFillZone();
			
			// If we found a match...
			if (z != null)
			{
				DockStyle ds;
				LayoutDirection direction;

				// Find relevant values dependant on required state
				ValuesFromState(z.State, out ds, out direction);
			
				// Reassign its correct Dock style
				z.Dock = ds;
			}
		}
		
		private Zone FindFirstFillZone()
		{
			// Find the innermost Zone which must be the first one in the collection
			foreach(Control c in _container.Controls)
			{
				Zone z = c as Zone;

				// Only interested in our Zones
				if (z != null)
					return z;
			}
	
			return null;
		}

		private Zone FindCurrentFillZone()
		{
			// Check each Zone in the container
			foreach(Control c in _container.Controls)
			{
				Zone z = c as Zone;

				if (z != null)
				{
					// Only interested in ones with the Fill dock style
					if (z.Dock == DockStyle.Fill)
						return z;
				}
			}
			
			return null;
		}

		private void OnFormLoaded(object sender, EventArgs e)
		{
			// A Form can cause the child controls to be reordered after the initialisation
			// but before the Form.Load event. To handle this we reorder the auto hide panels
			// on the Form.Load event to ensure they are correctly positioned.
			ReorderAutoHidePanels();
		}

		private void ReorderAutoHidePanels()
		{
			if (_outerControl == null)
			{
				int count = _container.Controls.Count;

				// Position the AutoHidePanel's at end of controls
				_container.Controls.SetChildIndex(_ahpLeft, count - 1);
				_container.Controls.SetChildIndex(_ahpRight, count - 1);
				_container.Controls.SetChildIndex(_ahpTop, count - 1);
				_container.Controls.SetChildIndex(_ahpBottom, count - 1);
			}
			else
			{
				// Position the AutoHidePanel's as last items before OuterControl
				RepositionControlBefore(_outerControl, _ahpBottom);
				RepositionControlBefore(_outerControl, _ahpTop);
				RepositionControlBefore(_outerControl, _ahpRight);
				RepositionControlBefore(_outerControl, _ahpLeft);
			}
		}

		private void OnContainerResized(object sender, EventArgs e)
		{
			CheckResized();
		}
		        
		internal void CheckResized()
		{
			if (_autoResize)
			{
				Rectangle inner = InnerResizeRectangle(null);			

				// Shrink by the minimum size
				inner.Width -= _innerMinimum.Width;
				inner.Height -= _innerMinimum.Height;

				// By default we process the change in size    			
    			bool process = true;
    			
				// The container might be a form itself...
				Form f = _container as Form;
				
				// If not then get the Form the container is inside
                if (f == null)
                {
                    Control parent = _container.Parent;

                    // Climb the parent control chain looking for a form
                    while (parent != null)
                    {
                        if (parent is Form)
                        {
                            f = (Form)parent;
                            break;
                        }

                        parent = parent.Parent;
                    }
                }

				// If we have a starting Form to begin working with
				if (f != null)
				{
					do
					{
						// Ignore resizing because of becoming Minimized
						if (f.WindowState == FormWindowState.Minimized)
						{
							process = false;
							break;
						}
						else if (f.MdiParent != null)
						{
							// We are attached to an MDI child window, so we need to 
							// ignore processing if the parent is minimized
							if (f.MdiParent.WindowState == FormWindowState.Minimized)
							{
								process = false;
								break;
							}
						}

						// Move up a level
						f = f.ParentForm;
					
					} while(f != null);
				}
				
				if (process)
				{
					if ((inner.Width < 0) || (inner.Height < 0))
					{
						_container.SuspendLayout();

						ZoneCollection zcLeft = new ZoneCollection();
						ZoneCollection zcRight = new ZoneCollection();
						ZoneCollection zcTop = new ZoneCollection();
						ZoneCollection zcBottom = new ZoneCollection();

						// Construct a list of the docking windows on the left and right edges
						foreach(Control c in _container.Controls)
						{
							Zone z = c as Zone;

							if (z != null)
							{
								switch(z.State)
								{
									case State.DockLeft:
										zcLeft.Add(z);
										break;
									case State.DockRight:
										zcRight.Add(z);
										break;
									case State.DockTop:
										zcTop.Add(z);
										break;
									case State.DockBottom:
										zcBottom.Add(z);
										break;
								}
							}
						}

						if (inner.Width < 0)
							ResizeDirection(-inner.Width, zcLeft, zcRight, LayoutDirection.Horizontal);

						if (inner.Height < 0)
							ResizeDirection(-inner.Height, zcTop, zcBottom, LayoutDirection.Vertical);

						_container.ResumeLayout();
					}
				}
			}
		}

		private void ResizeDirection(int remainder, ZoneCollection zcAlpha, ZoneCollection zcBeta, LayoutDirection dir)
		{
			bool alter;
			int available;
			int half1, half2;

			// Keep going till all space found or nowhere to get it from
			while((remainder > 0) && ((zcAlpha.Count > 0) || (zcBeta.Count > 0)))
			{
				if (dir == LayoutDirection.Horizontal)
				{
					_firstHalfWidth = (_firstHalfWidth != true);
					alter = _firstHalfWidth;
				}
				else
				{
					_firstHalfHeight = (_firstHalfHeight != true);
					alter = _firstHalfHeight;
				}

				// Alternate between left and right getting the remainder
				if (alter)
				{
					half1 = (remainder / 2) + 1;
					half2 = remainder - half1;
				}
				else
				{
					half2 = (remainder / 2) + 1;
					half1 = remainder - half2;
				}

				// Any Zone of the left to use?
				if (zcAlpha.Count > 0)
				{
					Zone z = zcAlpha[0];

					// Find how much space it can offer up
					if (dir == LayoutDirection.Horizontal)
						available = z.Width - z.MinimumWidth;
					else
						available = z.Height - z.MinimumHeight;

					if (available > 0)
					{
						// Only take away the maximum we need
						if (available > half1)
							available = half1;
						else
							zcAlpha.Remove(z);

						// Resize the control accordingly
						if (dir == LayoutDirection.Horizontal)
							z.Width = z.Width - available;
						else
							z.Height = z.Height - available;

						// Reduce total amount left to allocate
						remainder -= available;
					}
					else
						zcAlpha.Remove(z);
				}

				// Any Zone of the left to use?
				if (zcBeta.Count > 0)
				{
					Zone z = zcBeta[0];

					// Find how much space it can offer up
					if (dir == LayoutDirection.Horizontal)
						available = z.Width - z.MinimumWidth;
					else
						available = z.Height - z.MinimumHeight;

					if (available > 0)
					{
						// Only take away the maximum we need
						if (available > half2)
							available = half2;
						else
							zcBeta.Remove(z);

						// Resize the control accordingly
						if (dir == LayoutDirection.Horizontal)
							z.Width = z.Width - available;
						else
							z.Height = z.Height - available;

						// Reduce total amount left to allocate
						remainder -= available;
					}
					else
						zcBeta.Remove(z);
				}
			}
		}

		private void OnPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			if (_defaultBackColor)
			{
				_backColor = SystemColors.Control;
				PropogateNameValue(PropogateName.BackColor, (object)SystemColors.Control);
			}

			if (_defaultActiveColor)
			{
				_activeColor = SystemColors.ActiveCaption;
				PropogateNameValue(PropogateName.ActiveColor, (object)SystemColors.ActiveCaption);
			}
            
			if (_defaultActiveTextColor)
			{
				_activeTextColor = SystemColors.ActiveCaptionText;
				PropogateNameValue(PropogateName.ActiveTextColor, (object)SystemColors.ActiveCaptionText);
			}

			if (_defaultInactiveTextColor)
			{
				_inactiveTextColor = SystemColors.ControlText;
				PropogateNameValue(PropogateName.InactiveTextColor, (object)SystemColors.ControlText);
			}

			if (_defaultResizeBarColor)
			{
				_resizeBarColor = SystemColors.Control;
				PropogateNameValue(PropogateName.ResizeBarColor, (object)SystemColors.Control);
			}

			if (_defaultCaptionFont)
			{
				_captionFont = new Font(SystemInformation.MenuFont, FontStyle.Regular);
				PropogateNameValue(PropogateName.CaptionFont, _captionFont);
			}

			if (_defaultTabControlFont)
			{
				_tabControlFont = new Font(SystemInformation.MenuFont, FontStyle.Regular);
				PropogateNameValue(PropogateName.TabControlFont, _tabControlFont);
			}
		}

        private bool OnContextMenu(ContentCollection cc, ContextMenuStrip context)
		{
			CancelEventArgs cea = new CancelEventArgs();
        
			if (ContextMenu != null)
				ContextMenu(cc, context, cea);
                
			return !cea.Cancel;
		}

		private void OnToggleContentVisibility(object sender, EventArgs e)
		{
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;

            if (tsmi != null)
			{
                Content c = tsmi.Tag as Content;

				if (c != null)
				{
					if (c.Visible)
						HideContent(c);
					else
					{
						ShowContent(c);
						c.BringToFront();
					}
				}
			}
		}

		private void OnShowAll(object sender, EventArgs e)
		{
			ShowAllContents();
		}

		private void OnHideAll(object sender, EventArgs e)
		{
			HideAllContents();
		}

		private void RecreateWindowDetails(VisualStyle oldStyle)
		{
			// What type of window detail are we searching for
			Type searchType = null;

            if (oldStyle == VisualStyle.Plain)
				searchType = typeof(WindowDetailCaptionPlain);
            else if (oldStyle == VisualStyle.IDE2005)
				searchType = typeof(WindowDetailCaptionIDE2005);
            else if (oldStyle == VisualStyle.Office2003)
				searchType = typeof(WindowDetailCaptionOffice2003);
            else if ((oldStyle == VisualStyle.Office2007Blue) ||
                     (oldStyle == VisualStyle.Office2007Silver) ||
                     (oldStyle == VisualStyle.Office2007Black))
                searchType = typeof(WindowDetailCaptionOffice2007);
            else if ((oldStyle == VisualStyle.MediaPlayerBlue) ||
                     (oldStyle == VisualStyle.MediaPlayerOrange) ||
                     (oldStyle == VisualStyle.MediaPlayerPurple))
                searchType = typeof(WindowDetailCaptionMediaPlayer);
						
			if (searchType != null)
			{
				// Process each content instance in turn
				foreach(Content c in Contents)
				{
					// Get hold of the window content that holds this instance
					WindowContent wc = c.ParentWindowContent;
					
					// Assuming it has one
					if (wc != null)
					{
						// Search for type of interest
						for(int i=0; i<wc.WindowDetails.Count; i++)
						{
							if (wc.WindowDetails[i].GetType() == searchType)
							{
                                WindowDetail remove = wc.WindowDetails[i];

                                // Remove this window detail
								wc.WindowDetails.RemoveAt(i);

                                // Must dispose the detail as it has a message filter then needs removing
                                remove.Dispose();
								
								// Create new detail
								WindowDetailCaption wdc = null;
								
								if (_visualStyle == VisualStyle.Plain)
								{
									wdc = Factory.CreateWindowDetailCaptionPlain(this, 
                                                                                 wc,
																		         new EventHandler(OnContentClose), 
																		         new EventHandler(OnRestore),
																		         new EventHandler(OnInvertAutoHide),
																		         new ContextHandler(OnShowContextMenu));

									// Must inform the detail of the content display text
									wdc.NotifyFullTitleText(wc.CurrentContent.FullTitle);
                                    wdc.NotifyCloseButton(wc.CurrentContent.CloseButton);
                                    wdc.NotifyShowCaptionBar(wc.CurrentContent.CaptionBar);
								}
                                else if (_visualStyle == VisualStyle.IDE2005)
								{
									wdc = Factory.CreateWindowDetailCaptionIDE2005(this,
                                                                                   wc,
                                                                                   new EventHandler(OnContentClose), 
																		           new EventHandler(OnRestore),
																		           new EventHandler(OnInvertAutoHide),
																		           new ContextHandler(OnShowContextMenu));
																		
									// Must inform the detail of the content display text
                                    wdc.NotifyFullTitleText(wc.CurrentContent.FullTitle);
                                    wdc.NotifyCloseButton(wc.CurrentContent.CloseButton);
                                    wdc.NotifyShowCaptionBar(wc.CurrentContent.CaptionBar);
								}
                                else if (_visualStyle == VisualStyle.Office2003)
								{	 
									wdc = Factory.CreateWindowDetailCaptionOffice2003(this,
                                                                                      wc,
                                                                                      new EventHandler(OnContentClose), 
																			          new EventHandler(OnRestore),
																			          new EventHandler(OnInvertAutoHide),
																			          new ContextHandler(OnShowContextMenu));

									// Must inform the detail of the content display text
                                    wdc.NotifyFullTitleText(wc.CurrentContent.FullTitle);
                                    wdc.NotifyCloseButton(wc.CurrentContent.CloseButton);
                                    wdc.NotifyShowCaptionBar(wc.CurrentContent.CaptionBar);
								}
                                else if ((_visualStyle == VisualStyle.Office2007Blue) ||
                                         (_visualStyle == VisualStyle.Office2007Silver) ||
                                         (_visualStyle == VisualStyle.Office2007Black))
                                {
                                    wdc = Factory.CreateWindowDetailCaptionOffice2007(this,
                                                                                      wc,
                                                                                      new EventHandler(OnContentClose),
                                                                                      new EventHandler(OnRestore),
                                                                                      new EventHandler(OnInvertAutoHide),
                                                                                      new ContextHandler(OnShowContextMenu));

                                    // Setup the correct clear type drawing setting
                                    ((WindowDetailCaptionOffice2007)wdc).Apply2007ClearType = Apply2007ClearType;

                                    // Must inform the detail of the content display text
                                    wdc.NotifyFullTitleText(wc.CurrentContent.FullTitle);
                                    wdc.NotifyCloseButton(wc.CurrentContent.CloseButton);
                                    wdc.NotifyShowCaptionBar(wc.CurrentContent.CaptionBar);
                                }
                                else if ((_visualStyle == VisualStyle.MediaPlayerBlue) ||
                                         (_visualStyle == VisualStyle.MediaPlayerOrange) ||
                                         (_visualStyle == VisualStyle.MediaPlayerPurple))
                                {
                                    wdc = Factory.CreateWindowDetailCaptionMediaPlayer(this,
                                                                                       wc,
                                                                                       new EventHandler(OnContentClose),
                                                                                       new EventHandler(OnRestore),
                                                                                       new EventHandler(OnInvertAutoHide),
                                                                                       new ContextHandler(OnShowContextMenu));

                                    // Setup the correct clear type drawing setting
                                    ((WindowDetailCaptionMediaPlayer)wdc).ApplyMediaPlayerClearType = ApplyMediaPlayerClearType;

                                    // Must inform the detail of the content display text
                                    wdc.NotifyFullTitleText(wc.CurrentContent.FullTitle);
                                    wdc.NotifyCloseButton(wc.CurrentContent.CloseButton);
                                    wdc.NotifyShowCaptionBar(wc.CurrentContent.CaptionBar);
                                }
														
								// Add the new detail				    
								wc.WindowDetails.Add(wdc);
							}
						}

                        // If there is just a single window in a floating form
                        if ((wc.ParentZone != null) &&
                            (wc.ParentZone.Windows.Count == 1) &&
                            (wc.ParentForm is FloatingForm))
                        {
                            // Hide the window details
                            wc.HideDetails();
                        }
                    }
				}
			}
		}

		private void PropogateNameValue(PropogateName name, object value)
		{
			foreach(Control c in _container.Controls)
			{
				Zone z = c as Zone;

				// Only interested in our Zones
				if (z != null)
					z.PropogateNameValue(name, value);
			}

			// If the docking manager is created for a Container that does not
			// yet have a parent control then we need to double check before
			// trying to enumerate the owned forms.
			if (_container.FindForm() != null)
			{
				foreach(Form f in _container.FindForm().OwnedForms)
				{
					FloatingForm ff = f as FloatingForm;
                    
					// Only interested in our FloatingForms
					if (ff != null)
						ff.PropogateNameValue(name, value);
				}
			}
            
			// Propogate into the AutoHidePanel objects
			_ahpTop.PropogateNameValue(name, value);
			_ahpLeft.PropogateNameValue(name, value);
			_ahpRight.PropogateNameValue(name, value);
			_ahpBottom.PropogateNameValue(name, value);
		}

		private void OnContentPropertyChanged(Content obj, Content.Property prop)
		{
			if (prop == Content.Property.Office2003BackColor)
			{
				_extender.SetOffice2003BackColor(obj.Control, obj.Office2003BackColor);
			} 
            else if (prop == Content.Property.Office2007BackColor)
            {
				_extender.SetOffice2007BackColor(obj.Control, obj.Office2007BackColor);
            }
		}

        private Office2007Variant Office2007VariantFromVisualStyle(VisualStyle style)
        {
            switch (style)
            {
                default:
                case VisualStyle.Office2007Blue:
                    return Office2007Variant.Blue;
                case VisualStyle.Office2007Silver:
                    return Office2007Variant.Silver;
                case VisualStyle.Office2007Black:
                    return Office2007Variant.Black;
            }
        }

        private MediaPlayerVariant MediaPlayerVariantFromVisualStyle(VisualStyle style)
        {
            switch (style)
            {
                default:
                case VisualStyle.MediaPlayerBlue:
                    return MediaPlayerVariant.Blue;
                case VisualStyle.MediaPlayerOrange:
                    return MediaPlayerVariant.Orange;
                case VisualStyle.MediaPlayerPurple:
                    return MediaPlayerVariant.Purple;
            }
        }
    }
}
