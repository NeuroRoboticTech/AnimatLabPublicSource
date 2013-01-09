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
using System.Text;
using System.Drawing;
using System.Resources;
using System.Reflection;
using System.Collections;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Imaging;
using Microsoft.Win32;
using Crownwood.DotNetMagic.Win32;
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Manage a collection of tab pages.
	/// </summary>
    [ToolboxBitmap(typeof(TabControl))]
    [DefaultProperty("Appearance")]
    [DefaultEvent("SelectionChanged")]
    [Designer(typeof(Crownwood.DotNetMagic.Controls.TabControlDesigner))]
    public class TabControl : Panel
    {
        // Indexes into the menu images strip
        private enum ImageStrip
        {
            LeftEnabled = 0,
            LeftDisabled = 1,
            RightEnabled = 2,
            RightDisabled = 3,
            Close = 4,
            Error = 5,
            ActiveDisabled = 6,
            ActiveEnabled = 7,
        }

        // Enumeration of Indexes into positioning constants array
        private enum PositionIndex
        {
            BorderTop			= 0,
            BorderLeft			= 1,
            BorderBottom		= 2, 
            BorderRight			= 3,
            ImageGapTop			= 4,
            ImageGapLeft		= 5,
            ImageGapBottom		= 6,
            ImageGapRight		= 7,
            TextOffset			= 8,
            TextGapLeft			= 9,
            TabsBottomGap		= 10,
            ButtonOffset		= 11,
            TabsAreaStartInset	= 12
        }

        // Helper class for handling multiline calculations
        private class MultiRect
        {
            protected Rectangle _rect;
            protected int _index;

            public MultiRect(Rectangle rect, int index)
            {
                _rect = rect;
                _index = index;
            }

            public int Index
            {
                get { return _index; }
            }            
            
            public Rectangle Rect
            {
                get { return _rect; }
                set { _rect = value; }
            }
            
            public int X
            {
                get { return _rect.X; }
                set { _rect.X = value; }
            }

            public int Y
            {
                get { return _rect.Y; }
                set { _rect.Y = value; }
            }

            public int Width
            {
                get { return _rect.Width; }
                set { _rect.Width = value; }
            }

            public int Height
            {
                get { return _rect.Height; }
                set { _rect.Height = value; }
            }
        }
        
        // Class constants for sizing/positioning each style
		private static int[,] _position = { {6, 2, 2, 3, 3, 1, 1, 0, 1, 1, 2, 0, 5},		// Plain
											{1, 1, 0, 2, 3, 2, 1, 1, 2, 1, 3, 2, 0},		// Office2003
											{1, 1, 0, 1, 3, 2, 1, 1, 2, 1, 3, 2, 0},        // IDE2005
                                            {1, 1, 0, 2, 3, 2, 1, 1, 2, 1, 3, 2, 0},        // Office2007 Blue
                                            {1, 1, 0, 2, 3, 2, 1, 1, 2, 1, 3, 2, 0},        // Office2007 Silver
                                            {1, 1, 0, 2, 3, 2, 1, 1, 2, 1, 3, 2, 0},	    // Office2007 Black
                                            {1, 1, 0, 2, 3, 2, 1, 1, 2, 1, 3, 2, 0},        // MediaPlayer Blue
                                            {1, 1, 0, 2, 3, 2, 1, 1, 2, 1, 3, 2, 0},        // MediaPlayer Orange
                                            {1, 1, 0, 2, 3, 2, 1, 1, 2, 1, 3, 2, 0}};	    // MediaPlayer Purple

        // Class constants
        private static int PLAIN_BORDER = 3;
        private static int PLAIN_BORDER_DOUBLE = 6;
        private static int TABS_AREA_END_INSET = 5;
        private static int BUTTON_GAP = 3;
        private static int BUTTON_WIDTH = 14;
        private static int BUTTON_HEIGHT = 14;
        private static int IMAGE_BUTTON_WIDTH = 12;
        private static int IMAGE_BUTTON_HEIGHT = 12;
        private static int MULTIBOX_ADJUSTUP = 2;
        private static int MULTIBOX_ADJUSTDOWN = 1;
        private static int AUTOSCROLL_VECTOR = 20;
		private static int OFFICE_HEADER_INSET = 5;
		private static int IDE2005_HEADER_INSET = 5;
		private static Rectangle _nullPosition = new Rectangle(-999, -999, 0, 0);

        // Class state
        private static ImageList _internalImages;

        // Instance fields - size/positioning
        private int _textHeight;
        private int _imageWidth;
        private int _imageHeight;
        private int _imageGapTopExtra;
        private int _imageGapBottomExtra;
        private Rectangle _pageRect;
        private Rectangle _pageAreaRect;
        private Rectangle _tabsAreaRect;

        // Instance fields - state
        private int _ctrlTopOffset;				// How far from top edge embedded controls should offset
        private int _ctrlLeftOffset;			// How far from left edge embedded controls should offset
        private int _ctrlRightOffset;			// How far from right edgeembedded controls should offset
        private int _ctrlBottomOffset;			// How far from bottom edge embedded controls should offset
        private int _extraOfficeTabInset;		// Extra left inset for the first tab position
        private int _extraMediaPlayerTabInset;	// Extra left inset for the first tab position
        private int _extraIDE2005TabInset;		// Extra left inset for the first tab position
        private int _styleIndex;				// Index into position array
        private int _pageSelected;				// index of currently selected page (-1 is none)
        private int _startPage;					// index of first page to draw, used when scrolling pages
        private int _hotTrackPage;				// which page is currently displayed as being tracked
        private int _topYPos;					// Y position of first line in multiline mode
        private int _bottomYPos;				// Y position of last line in multiline mode
        private int _leaveTimeout;				// How long from leaving to timeout occuring
		private int _withoutMouseVector;		// Vector used when showing tabs only when it has mouse
		private int _mouseDownIndex;			// Which page index the last left mouse down occured on
		private int _mouseDragIndex;			// Current dragging inserted index
		private int _maximumHeaderWidth;		// Maximum width of tab header
        private bool _allowCtrlTab;             // Should Ctrl+Tab be allowed to change active page.
		private bool _toolTips;					// Should tooltips be shown for headers.
		private bool _textTips;					// Should tooltips be shown when text is abbreviated.
		private bool _mouseOver;				// Mouse currently over the control (or child pages)
        private bool _multiline;				// should tabs that cannot fit on a line create new lines
        private bool _multilineFullWidth;		// when in multiline mode, all lines are extended to end 
        private bool _shrinkPagesToFit;			// pages are shrunk so they all fit in control width
        private bool _changed;					// Flag for use when updating contents of collection
        private bool _positionAtTop;			// display tabs at top or bottom of the control
        private bool _showClose;				// should the close button be displayed
        private bool _showArrows;				// should the scroll arrow be displayed
        private bool _showDropSelect;			// should the drop select arrow be displayed
        private bool _insetPlain;				// Show the inset border for controls
        private bool _insetBorderPagesOnly;		// Remove the border entirely for Plain mode
        private bool _selectedTextOnly;			// Only draw text for selected tab
        private bool _rightScroll;				// Should the right scroll button be enabled
        private bool _leftScroll;				// Should the left scroll button be enabled
        private bool _dimUnselected;			// should unselected pages be drawn slightly dimmed
        private bool _boldSelected;				// should selected page use a bold font
        private bool _hotTrack;					// should mouve moving over text hot track it
        private bool _hoverSelect;				// select a page when he mouse hovers over it
        private bool _recalculate;				// flag to indicate recalculation is needed before painting
        private bool _leftMouseDown;			// Is the left mouse button down
        private bool _leftMouseDownDrag;		// Has a drag operation begun
        private bool _ignoreDownDrag;			// When pressed the left button cannot generate two drags
        private bool _defaultColor;				// Is the background color the default one?
        private bool _defaultFont;				// Is the Font the default one?
		private bool _recordFocus;				// Record the control with focus when leaving a page
        private bool _idePixelArea;				// Place a one pixel border at top/bottom of tabs area
        private bool _idePixelBorder;			// Place a one pixel border around control for IDE style
		private bool _officeHeaderBorder;		// Place a one pixel border around control for Office style
        private bool _mediaPlayerHeaderBorder;  // Place a one pixel border around control for Media Player style
        private bool _officePixelBorder;		// Place a one pixel border around control for Office style
        private bool _mediaPlayerPixelBorder;   // Place a one pixel border around control for Media Player style
        private bool _ide2005HeaderBorder;		// Place a one pixel border around control for IDE2005 style
		private bool _ide2005PixelBorder;		// Place a one pixel border around control for IDE2005 style
		private bool _ide2005TabJoined;			// Should the active tab look like it joins the main area
		private bool _officeDockSides;			// Place special fading borders around docking in office
        private bool _mediaPlayerDockSides;     // Place special fading borders around docking in media player
        private bool _dragOverSelect;			// Should dragging over a tab cause selection?
		private bool _dragOutside;				// Should dragging outside tab area generate events?
		private bool _draggingInside;			// Currently dragging inside the control?
		private bool _suspended;				// Is layout and selection suspended?
		private bool _scrollingLeft;			// Is the scroll timer moving things left?
		private bool _allowDragReorder;			// Is the user allowed to reorder pages by draggin?
		private bool _allowSideCaptions;		// Internal for use with docking windows
        private bool _popUpMenuShowing;			// Internal for use with tooltips and menus
        private bool _apply2007ClearType;       // Should clear type be used to draw text in Office 2007 styles
        private bool _applyMediaPlayerClearType;// Should clear type be used to draw text in Media Player styles
        private Point _leftMouseDownPos;		// Initial mouse down position for left mouse button
		private Point _mouseDragOver;			// Mouse position for drag over event
        private Point _mouseMovePt;             // Mouse point cached from last mouse move
        private Color _hotTextColor;			// color for use when drawing text as hot
        private Color _textColor;				// color for use when text not hot
        private Color _textInactiveColor;	    // color for use when text not hot and not the active tab
        private Color _backIDE;					// background drawing color when in IDE appearance
        private Color _buttonActiveColor;		// color for drawing buttons images when active
        private Color _buttonInactiveColor;		// color for drawing buttons images when inactive
        private Color _backLight;				// light variation of the back color
        private Color _backLightLight;			// lightlight variation of the back color
        private Color _backDark;				// dark variation of the back color
        private Color _backDarkDark;			// darkdark variation of the back color
		private PopupTooltipSingle _toolTip;	// Tooltip control to use for headers.
		private Rectangle _tooltipRect;			// Rectangle used to decide if tooltip should be removed.
        private VisualStyle _style;				// which style of use 
        private VisualStyle _externalStyle;		// which style is exposed to the user
        private OfficeStyle _officeStyle;		// which Office2003/2007 style variation to use
        private MediaPlayerStyle _mediaPlayerStyle; // which MediaPlayer style variation to use
        private IDE2005Style _ide2005Style;		// which IDE2005 style variation to use
        private HideTabsModes _hideTabsMode;	// Decide when to hide/show tabs area
        private Timer _overTimer;				// Time when mouse has left control
		private Timer _dragTimer;				// Time when mouse hovers in drag over
		private Timer _hoverTimer;				// Time when mouse hovers in normal mode
		private Timer _scrollTimer;				// Time when mouse hovers in normal mode
		private VisualAppearance _appearance;	// which appearance style
		private HotkeyPrefix _hotkeyPrefix;		// How are hotkeys drawn in tab pages
		private TabPage _oldPage;				// Remember the old page during selection changes
        private ImageList _imageList;			// collection of images for use is tabs
        private ArrayList _tabRects;			// display rectangles for associated page
        private TabPageCollection _tabPages;	// collection of pages
		private TabPage _dragPage;				// Page reference to be moved around
		private Hashtable _controlSet;			// Set of unique controls for showing
		private ColorDetails _colorDetails;		// Helper for obtaining theme colors
		private ImageAttributes _ia;			// Cache of the current image remapping attributes
        private ImageAttributes _iao;			// Cache of the current image remapping attributes

        // Instance fields - buttons
        private ButtonWithStyle _closeButton;
        private ButtonWithStyle _leftArrow;
        private ButtonWithStyle _rightArrow;
        private ButtonWithStyle _activeArrow;

		/// <summary>
		/// Occurs when the close button is pressed.
		/// </summary>
		[Category("Action")]
		public event EventHandler ClosePressed;

		/// <summary>
		/// Occurs just before a change in selected page is processed.
		/// </summary>
		[Category("Behavior")]
		public event SelectTabHandler SelectionChanging;

		/// <summary>
		/// Occurs just after a change in selected page is processed.
		/// </summary>
		[Category("Property Changed")]
		public event SelectTabHandler SelectionChanged;

		/// <summary>
		/// Occurs when a tab page gets the focus.
		/// </summary>
		[Category("Focus")]
		public event EventHandler PageGotFocus;

		/// <summary>
		/// Occurs when a tab page loses the focus.
		/// </summary>
		[Category("Focus")]
		public event EventHandler PageLostFocus;

        /// <summary>
        /// Occurs just before a tooltip is to be displayed.
        /// </summary>
        [Category("Behavior")]
        public event TooltipDisplayHandler TooltipDisplay;

		/// <summary>
		/// Occurs just before a context menu strip is shown.
		/// </summary>
		[Category("Behavior")]
		public event CancelEventHandler ContextMenuStripDisplay;

		/// <summary>
		/// Occurs when the user starts dragging a tab page.
		/// </summary>
		[Category("Drag Drop")]
		public event PageDragStartHandler PageDragStart;

		/// <summary>
		/// Occurs when the dragging page moves.
		/// </summary>
		[Category("Drag Drop")]
		public event MouseEventHandler PageDragMove;

		/// <summary>
		/// Occurs when the user ends dragging a tab page.
		/// </summary>
		[Category("Drag Drop")]
		public event PageDragHandler PageDragEnd;

		/// <summary>
		/// Occurs when the user quits dragging a tab page.
		/// </summary>
		[Category("Drag Drop")]
		public event PageDragHandler PageDragQuit;

		/// <summary>
		/// Occurs after a page has been dragged into a different position.
		/// </summary>
		[Category("Drag Drop")]
		public event PageMovedHandler PageMoved;

		/// <summary>
		/// Occurs when a mouse double click happens on a tab page.
		/// </summary>
		[Category("Action")]
		public event DoubleClickTabHandler DoubleClickTab;

		/// <summary>
		/// Occurs when the TabsAreaRect value changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler TabsAreaRectChanged;

        static TabControl()
        {
            // Create a strip of images by loading an embedded bitmap resource
            _internalImages = ResourceHelper.LoadBitmapStrip(Type.GetType("Crownwood.DotNetMagic.Controls.TabControl"),
                                                             "Crownwood.DotNetMagic.Resources.ImagesTabControl.bmp",
                                                             new Size(IMAGE_BUTTON_WIDTH, IMAGE_BUTTON_HEIGHT),
                                                             new Point(0,0));
		}

		/// <summary>
		/// Initializes a new instance of the TabControl class.
		/// </summary>
        public TabControl()
        {
			// NAG processing
			NAG.NAG_Start();
			
			// Prevent flicker with double buffering and all painting inside WM_PAINT
            SetStyle(ControlStyles.OptimizedDoubleBuffer | 
				     ControlStyles.AllPaintingInWmPaint |
				     ControlStyles.UserPaint, true);

            // Create collections
            _tabRects = new ArrayList();
            _tabPages = new TabPageCollection();

            // Hookup to collection events
            _tabPages.Clearing += new CollectionClear(OnClearingPages);
            _tabPages.Cleared += new CollectionClear(OnClearedPages);
            _tabPages.Inserting += new CollectionChange(OnInsertingPage);
            _tabPages.Inserted += new CollectionChange(OnInsertedPage);
            _tabPages.Removing += new CollectionChange(OnRemovingPage);
            _tabPages.Removed += new CollectionChange(OnRemovedPage);

            // Define the default state of the control
            _startPage = -1;						
            _pageSelected = -1;						
            _hotTrackPage = -1;		
			_withoutMouseVector = -1;
			_mouseDownIndex = -1;
			_maximumHeaderWidth = -1;
			_imageList = null;	
			_ia = null;
            _iao = null;
            _toolTip = null;
            _allowCtrlTab = true;
            _insetPlain = true;
            _multiline = false;
            _multilineFullWidth = false;
            _mouseOver = false;
            _leftScroll = false;
            _defaultFont = true;
            _defaultColor = true;
            _rightScroll = false;
            _hoverSelect = false;
			_showDropSelect = false;
            _allowDragReorder = true;
            _leftMouseDown = false;
            _ignoreDownDrag = true;
            _selectedTextOnly = false;
            _leftMouseDownDrag = false;
            _insetBorderPagesOnly = false;
			_draggingInside = false;
			_dragOutside = false;
			_suspended = false;
			_toolTips = false;
			_textTips = true;
			_ide2005TabJoined = true;
			_dragOverSelect = true;
			_officeHeaderBorder = false;
            _mediaPlayerHeaderBorder = false;
            _ide2005HeaderBorder = false;
            _allowSideCaptions = false;
			_hotkeyPrefix = HotkeyPrefix.Show;
            _hideTabsMode = HideTabsModes.ShowAlways;
			_recordFocus = true;
            _styleIndex = 1;
            _leaveTimeout = 200;
            _ctrlTopOffset = 0;
            _ctrlLeftOffset = 0;
            _ctrlRightOffset = 0;
            _ctrlBottomOffset = 0;
            _extraOfficeTabInset = 0;
            _extraMediaPlayerTabInset = 0;
            _extraIDE2005TabInset = 0;
            _officeDockSides = false;
            _mediaPlayerDockSides = false;
            _style = VisualStyle.Office2007Blue;
            _externalStyle = VisualStyle.Office2007Blue;
            _officeStyle = OfficeStyle.LightWhite;
            _mediaPlayerStyle = MediaPlayerStyle.LightWhite;
			_ide2005Style = IDE2005Style.Standard;
            _buttonActiveColor = Color.FromArgb(128, this.ForeColor);
            _buttonInactiveColor = _buttonActiveColor;
            _textColor = TabControl.DefaultForeColor;	
            _textInactiveColor = Color.FromArgb(128, _textColor);
            _hotTextColor = SystemColors.ActiveCaption;
			_controlSet = new Hashtable();
			_colorDetails = new ColorDetails();

            // Create hover buttons
            _closeButton = new TabButtonWithStyle(this);
            _leftArrow = new TabButtonWithStyle(this);
            _rightArrow = new TabButtonWithStyle(this);
            _activeArrow = new TabButtonWithStyle(this);

			// Do not use gradient effect for office styles
			_closeButton.Office2003GradBack = false;
			_leftArrow.Office2003GradBack = false;
			_rightArrow.Office2003GradBack = false;
			_activeArrow.Office2003GradBack = false;

			// Always draw the images on the button as if enabled
			_closeButton.AlwaysDrawEnabled = true;
			_leftArrow.AlwaysDrawEnabled = true;
			_rightArrow.AlwaysDrawEnabled = true;
			_activeArrow.AlwaysDrawEnabled = true;

			// Define images
			_closeButton.Image = _internalImages.Images[(int)ImageStrip.Close];
			_leftArrow.Image = _internalImages.Images[(int)ImageStrip.LeftEnabled];
			_rightArrow.Image = _internalImages.Images[(int)ImageStrip.RightEnabled];
			_activeArrow.Image = _internalImages.Images[(int)ImageStrip.ActiveEnabled];

            // Hookup to the button events
            _closeButton.Click += new EventHandler(OnCloseButton);
            _leftArrow.Click += new EventHandler(OnLeftArrow);
            _rightArrow.Click += new EventHandler(OnRightArrow);
            _activeArrow.Click += new EventHandler(OnActiveArrow);

            // Set their fixed sizes
            _activeArrow.Size = _leftArrow.Size = _rightArrow.Size = _closeButton.Size = new Size(BUTTON_WIDTH, BUTTON_HEIGHT);

            // Add child controls
            Controls.AddRange(new Control[]{_closeButton, _leftArrow, _rightArrow, _activeArrow});

            // Grab some contant values
            _imageWidth = 16;
            _imageHeight = 16;

            // Default to having a MultiForm usage
            SetAppearance(VisualAppearance.MultiForm);
            
            // Need a timer so that when the mouse leaves, a fractionaly delay occurs before
            // noticing and hiding the tabs area when the appropriate style is set
            _overTimer = new Timer();
            _overTimer.Interval = _leaveTimeout;
            _overTimer.Tick += new EventHandler(OnMouseTick);

			// Need a timer so that when the mouse hovers in drag over mode the page then changes
			_dragTimer = new Timer();
			_dragTimer.Interval = 200;
			_dragTimer.Tick += new EventHandler(OnDragOverTick);

			// Need a timer so that when you drag a page off edge of control it will scroll
			_scrollTimer = new Timer();
			_scrollTimer.Interval = 333;
			_scrollTimer.Tick += new EventHandler(OnScrollTick);

			// Need a timer so that when the mouse hovers in normal mode we can show tooltips
			_hoverTimer = new Timer();
			_hoverTimer.Interval = 500;
			_hoverTimer.Tick += new EventHandler(OnMouseHoverTick);
			
			// Need notification when the MenuFont is changed
            Microsoft.Win32.SystemEvents.UserPreferenceChanged += 
                new UserPreferenceChangedEventHandler(OnPreferenceChanged);

            // Define the default Font, BackColor and Button images
			DefineFont(new Font(SystemInformation.MenuFont, FontStyle.Regular));
            DefineBackColor(SystemColors.Control);
            DefineButtonImages();
			ResetAllowDrop();
            ResetApply2007ClearType();
            ResetApplyMediaPlayerClearType();
            ResetDragOverSelect();
			ResetToolTips();
			ResetIDE2005TabJoined();
            Recalculate();
        }

		/// <summary>
		/// Releases all resources used by the Control.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose( bool disposing )
        {
            if(disposing)
            {
				// Dispose of an existing attributes usage
				if (_ia != null)
				{
					_leftArrow.ImageAttributes = null;
					_rightArrow.ImageAttributes = null;
					_closeButton.ImageAttributes = null;
					_activeArrow.ImageAttributes = null;
					_ia.Dispose();
					_ia = null;
				}

                // Dispose of an existing attributes usage
                if (_iao != null)
                {
                    _leftArrow.ImageAttributesOver = null;
                    _rightArrow.ImageAttributesOver = null;
                    _closeButton.ImageAttributesOver = null;
                    _activeArrow.ImageAttributesOver = null;
                    _iao.Dispose();
                    _iao = null;
                }
                
                // Do not want any timers to expire after controls has been disposed
				_overTimer.Tick -= new EventHandler(OnMouseTick);
				_dragTimer.Tick -= new EventHandler(OnDragOverTick);
				_hoverTimer.Tick -= new EventHandler(OnMouseHoverTick);
				_scrollTimer.Tick -= new EventHandler(OnScrollTick);
				
				try
				{
					_overTimer.Stop();
					_dragTimer.Stop();
					_hoverTimer.Stop();
					_scrollTimer.Stop();
					
					_overTimer.Dispose();
					_dragTimer.Dispose();
					_hoverTimer.Dispose();
					_scrollTimer.Dispose();
				} catch {}

                // Remove notifications
                Microsoft.Win32.SystemEvents.UserPreferenceChanged -= 
                    new UserPreferenceChangedEventHandler(OnPreferenceChanged);

				// Color details has resources that need releasing
				_colorDetails.Dispose();
			}
            
            base.Dispose(disposing);
        }

		/// <summary>
		/// Temporarily suspends the layout logic for the control.
		/// </summary>
		public new void SuspendLayout()
		{
			// Suspend the changing of selection
			_suspended = true;

			// Must remember to call base class
			base.SuspendLayout();
		}

		/// <summary>
		/// Resumes normal layout logic.
		/// </summary>
		public new void ResumeLayout()
		{
			// Use helper method to implement
			ResumeProcess();

			// Must remember to call base class
			base.ResumeLayout();
		}

		/// <summary>
		/// Resumes normal layout logic.
		/// </summary>
		public new void ResumeLayout(bool relayout)
		{
			// Use helper method to implement
			ResumeProcess();

			// Must remember to call base class
			base.ResumeLayout(relayout);
		}

		/// <summary>
		/// DesignerSerializationVisibility
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Control.ControlCollection Controls 
		{
			get { return base.Controls; }
		}

		/// <summary>
		/// Gets the collection of tab pages.
		/// </summary>
		[Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual TabPageCollection TabPages
        {
            get { return _tabPages; }
        }

		/// <summary>
		/// Gets or sets the font of the text displayed by the control.
		/// </summary>
        [Category("Appearance")]
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
							
						DefineFont(value);
						_recalculate = true;
						Invalidate();
					}
				}
            }
        }

		/// <summary>
		/// Decide if the font needs to be persisted.
		/// </summary>
		/// <returns>Should font be persisted.</returns>
        private bool ShouldSerializeFont()
        {
            return !_defaultFont;
        }

		/// <summary>
		/// Gets or sets a value indicating whether the control can accept data that the user drags onto it.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(true)]
		public override bool AllowDrop
		{
			get { return base.AllowDrop; }
			set { base.AllowDrop = value; }
		}

		/// <summary>
		/// Resets the AllowDrop property to its default value.
		/// </summary>
		public void ResetAllowDrop()
		{
			AllowDrop = true;
		}

		/// <summary>
		/// Gets or sets the setting used to determine how hoykeys are drawn in tab headers.
		/// </summary>
		[Category("Appearance")]
		[DefaultValue(typeof(HotkeyPrefix), "Show")]
		public HotkeyPrefix HotkeyPrefix        
		{
			get { return _hotkeyPrefix; }
			
			set 
			{ 
				if (value != _hotkeyPrefix)
				{
					_hotkeyPrefix = value;
					_recalculate = true;
					Invalidate();
				}
			}
		}
		
		/// <summary>
		/// Resets the HotkeyPrefix property to its default value.
		/// </summary>
		public void ResetHotkeyPrefix()
		{
			HotkeyPrefix = HotkeyPrefix.Show;
		}

        /// <summary>
        /// Gets and sets the size of images in the tab headers.
        /// </summary>
        [Category("Appearance")]
        [Description("Size of images that appear in tab headers.")]
        [DefaultValue(typeof(Size), "16,16")]
        public Size ImageSize
        {
            get { return new Size(_imageWidth, _imageHeight); }

            set
            {
                // Store the new values with a minimum of 1 pixel
                _imageWidth = Math.Max(value.Width, 1);
                _imageHeight = Math.Max(value.Height, 1);

                // Request layout be recalculated and a redraw occur
                _recalculate = true;
                Invalidate();
            }
        }

        /// <summary>
        /// Resets the ImageSize property to its default value.
        /// </summary>
        public void ResetImageSize()
        {
            ImageSize = new Size(16, 16);
        }

		/// <summary>
		/// Gets or sets a value indicating whether the user can drag to reorder pages.
		/// </summary>
		[Category("Behavior")]
        [DefaultValue(true)]
		public bool AllowDragReorder        
		{
			get { return _allowDragReorder; }
			set { _allowDragReorder = value; }
		}
		
		/// <summary>
		/// Resets the AllowDragReorder property to its default value.
		/// </summary>
		public void ResetAllowDragReorder()
		{
			AllowDragReorder = true;
		}
        
		/// <summary>
		/// Gets or sets the foreground color of the control.
		/// </summary>
        [Category("Appearance")]
        public override Color ForeColor
        {
            get { return _textColor; }
			
            set 
            {
                if (_textColor != value)
                {
                    _textColor = value;

                    _recalculate = true;
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Decide if the forecolor needs to be persisted.
		/// </summary>
		/// <returns>Should forecolor be persisted.</returns>
        private bool ShouldSerializeForeColor()
        {
            return _textColor != TabControl.DefaultForeColor;
        }

		/// <summary>
		/// Gets or sets the background color for the control.
		/// </summary>
        [Category("Appearance")]
        public override Color BackColor
        {
            get { return base.BackColor; }
			
            set 
            {
                if (this.BackColor != value)
                {
                    _defaultColor = (value == SystemColors.Control);

                    DefineBackColor(value);
		
                    _recalculate = true;
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Decide if the backcolor needs to be persisted.
		/// </summary>
		/// <returns>Should backcolor be persisted.</returns>
        private bool ShouldSerializeBackColor()
        {
            return this.BackColor != SystemColors.Control;
        }

		/// <summary>
		/// Gets or sets the color used for buttons that are active.
		/// </summary>
        [Category("Appearance")]
        public Color ButtonActiveColor
        {
            get { return _buttonActiveColor; }

            set
            {
                if (_buttonActiveColor != value)
                {
                    _buttonActiveColor = value;
                    DefineButtonImages();
                }
            }
        }

		/// <summary>
		/// Decide if the button active color needs to be persisted.
		/// </summary>
		/// <returns>Should button active color be persisted.</returns>
        private bool ShouldSerializeButtonActiveColor()
        {
            return _buttonActiveColor != Color.FromArgb(128, this.ForeColor);
        }

		/// <summary>
		/// Resets the ButtonActiveColor property to its default value.
		/// </summary>
        public void ResetButtonActiveColor()
        {
            ButtonActiveColor = Color.FromArgb(128, this.ForeColor);
        }

		/// <summary>
		/// Gets or sets the color used for buttons that are inactive.
		/// </summary>
        [Category("Appearance")]
        public virtual Color ButtonInactiveColor
        {
            get { return _buttonInactiveColor; }

            set
            {
                if (_buttonInactiveColor != value)
                {
                    _buttonInactiveColor = value;
                    DefineButtonImages();
                }
            }
        }

		/// <summary>
		/// Decide if the button inactive color needs to be persisted.
		/// </summary>
		/// <returns>Should button inactive color be persisted.</returns>
        private bool ShouldSerializeButtonInactiveColor()
        {
            return _buttonInactiveColor != Color.FromArgb(128, this.ForeColor);
        }

		/// <summary>
		/// Resets the ButtonInactiveColor property to its default value.
		/// </summary>
        public void ResetButtonInactiveColor()
        {
            ButtonInactiveColor = Color.FromArgb(128, this.ForeColor); 
        }
        
		/// <summary>
		/// Gets or sets the visual appearance of the control.
		/// </summary>
        [Category("Appearance")]
        [DefaultValue(typeof(VisualAppearance), "MultiForm")]
		[RefreshPropertiesAttribute(RefreshProperties.All)]
        public virtual VisualAppearance Appearance
        {
            get { return _appearance; }
			
            set
            {
                if (_appearance != value)
                {
                    SetAppearance(value);

                    Recalculate();
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Resets the Appearance property to its default value.
		/// </summary>
        public void ResetAppearance()
        {
            Appearance = VisualAppearance.MultiForm;
        }

		/// <summary>
		/// Gets or sets the visual style of the control.
		/// </summary>
        [Category("Appearance")]
        [DefaultValue(typeof(VisualStyle), "Office2007Blue")]
        public virtual VisualStyle Style
        {
            get { return _externalStyle; }
			
            set
            {
                if (_externalStyle != value)
                {
                    _externalStyle = value;
                    _style = ColorHelper.ValidateStyle(value);
                    _colorDetails.Style = _style;
                    _closeButton.Style = _style;
                    _leftArrow.Style = _style;
                    _rightArrow.Style = _style;
                    _activeArrow.Style = _style;

                    // Define the correct style indexer
                    SetStyleIndex();
					DefineButtonImages();
                    Recalculate();
                    Invalidate();
                }
            }
        }
        
		/// <summary>
		/// Resets the Style property to its default value.
		/// </summary>
        public void ResetStyle()
        {
            Style = VisualStyle.Office2007Blue;
        }
        
		/// <summary>
		/// Gets or sets the variation to use when drawing in Office2003/Office2007 style.
		/// </summary>
		[Category("Office")]
		[DefaultValue(typeof(OfficeStyle), "LightWhite")]
		public virtual OfficeStyle OfficeStyle
		{
			get { return _officeStyle; }
			
			set
			{
				if (_officeStyle != value)
				{
					_officeStyle = value;

					// Define the correct style indexer
					SetStyleIndex();
					DefineButtonImages();
					Recalculate();
					Invalidate();
				}
			}
		}
        
		/// <summary>
		/// Resets the OfficeStyle property to its default value.
		/// </summary>
		public void ResetOfficeStyle()
		{
			OfficeStyle = OfficeStyle.LightWhite;
		}

        /// <summary>
        /// Gets or sets the variation to use when drawing in Media Player style.
        /// </summary>
        [Category("MediaPlayer")]
        [DefaultValue(typeof(MediaPlayerStyle), "LightWhite")]
        public virtual MediaPlayerStyle MediaPlayerStyle
        {
            get { return _mediaPlayerStyle; }

            set
            {
                if (_mediaPlayerStyle != value)
                {
                    _mediaPlayerStyle = value;

                    // Define the correct style indexer
                    SetStyleIndex();
                    DefineButtonImages();
                    Recalculate();
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Resets the MediaPlayerStyle property to its default value.
        /// </summary>
        public void ResetMediaPlayerStyle()
        {
            MediaPlayerStyle = MediaPlayerStyle.LightWhite;
        }
        
        /// <summary>
		/// Gets or sets the variation to use when drawing in IDE2005 style.
		/// </summary>
		[Category("IDE2005")]
		[DefaultValue(typeof(IDE2005Style), "Standard")]
		public virtual IDE2005Style IDE2005Style
		{
			get { return _ide2005Style; }
			
			set
			{
				if (_ide2005Style != value)
				{
					_ide2005Style = value;

					// Define the correct style indexer
					SetStyleIndex();
					DefineButtonImages();
					Recalculate();
					Invalidate();
				}
			}
		}
        
		/// <summary>
		/// Resets the IDE2005Style property to its default value.
		/// </summary>
		public void ResetIDE2005Style()
		{
			IDE2005Style = IDE2005Style.Standard;
		}

        /// <summary>
        /// Gets and sets a value indicating if the Office 2007 style text should use ClearType.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(true)]
        [Description("Should the Office 2007 styles draw text using ClearType.")]
        public bool Apply2007ClearType
        {
            get { return _apply2007ClearType; }

            set
            {
                if (_apply2007ClearType != value)
                {
                    _apply2007ClearType = value;
                    Recalculate();
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Resets the Apply2007ClearType property to its default value.
        /// </summary>
        public void ResetApply2007ClearType()
        {
            Apply2007ClearType = true;
        }

        /// <summary>
        /// Gets and sets a value indicating if the MediaPlayer style text should use ClearType.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(true)]
        [Description("Should the MediaPlayer styles draw text using ClearType.")]
        public bool ApplyMediaPlayerClearType
        {
            get { return _applyMediaPlayerClearType; }

            set
            {
                if (_applyMediaPlayerClearType != value)
                {
                    _applyMediaPlayerClearType = value;
                    Recalculate();
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Resets the ApplyMediaPlayerClearType property to its default value.
        /// </summary>
        public void ResetApplyMediaPlayerClearType()
        {
            ApplyMediaPlayerClearType = true;
        }

		/// <summary>
		/// Gets and sets the ability to select pages as user drags over them.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(true)]
		public virtual bool DragOverSelect
		{
			get { return _dragOverSelect; }
			set { _dragOverSelect = value; }
		}

		/// <summary>
		/// Resets the DragOverSelect property to its default value.
		/// </summary>
		public void ResetDragOverSelect()
		{
			DragOverSelect = true;
		}

		/// <summary>
		/// Gets and sets a value indicating if dragging outside the tab area is allowed.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(false)]
		[Description("Indicates if dragging outside the tab area is allowed.")]
		public virtual bool DragOutside
		{
			get { return _dragOutside; }
			set { _dragOutside = value; }
		}

		/// <summary>
		/// Resets the DragOutside property to its default value.
		/// </summary>
		public void ResetDragOutside()
		{
			DragOutside = false;
		}
		
		/// <summary>
		/// Gets and sets the ability show the TabPage.ToolTip when hovering.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(false)]
		public virtual bool ToolTips
		{
			get { return _toolTips; }
			set { _toolTips = value; }
		}

		/// <summary>
		/// Resets the ToolTips property to its default value.
		/// </summary>
		public void ResetToolTips()
		{
			ToolTips = false;
		}

		/// <summary>
		/// Gets and sets the ability to show the full TabPage.Title as tooltip when abbreviated.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(false)]
		public virtual bool TextTips
		{
			get { return _textTips; }
			set { _textTips = value; }
		}

		/// <summary>
		/// Resets the TextTips property to its default value.
		/// </summary>
		public void ResetTextTips()
		{
			TextTips = false;
		}

		/// <summary>
		/// Gets and sets the use of Ctrl+Tab for changing the selected page.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(true)]
		public virtual bool AllowCtrlTab
		{
            get { return _allowCtrlTab; }
            set { _allowCtrlTab = value; }
		}

		/// <summary>
        /// Resets the AllowCtrlTab property to its default value.
		/// </summary>
        public void ResetAllowCtrlTab()
		{
            AllowCtrlTab = true;
		}

        
		/// <summary>
		/// Gets and sets the ability to highlight tab header when mouse is over them.
		/// </summary>
		[Category("Appearance")]
        [DefaultValue(false)]
        public virtual bool HotTrack
        {
            get { return _hotTrack; }
			
            set 
            {
                if (_hotTrack != value)
                {
                    _hotTrack = value;

                    if (!_hotTrack)
                        _hotTrackPage = -1;

                    _recalculate = true;
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Resets the HotTrack property to its default value.
		/// </summary>
        public void ResetHotTrack()
        {
            HotTrack = false;
        }

		/// <summary>
		/// Gets and sets the text color to use when hot tracking tab headers.
		/// </summary>
        [Category("Appearance")]
        public virtual Color HotTextColor
        {
            get { return _hotTextColor; }
			
            set 
            {
                if (_hotTextColor != value)
                {
                    _hotTextColor = value;

                    _recalculate = true;
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Decide if the hot text color needs to be persisted.
		/// </summary>
		/// <returns>Should hot text color be persisted.</returns>
        private bool ShouldSerializeHotTextColor()
        {
            return _hotTextColor != SystemColors.ActiveCaption;
        }
        
		/// <summary>
		/// Resets the HotTextColor property to its default value.
		/// </summary>
        public void ResetHotTextColor()
        {
            HotTextColor = SystemColors.ActiveCaption;
        }

		/// <summary>
		/// Gets and sets the text color to use when drawing tab headers.
		/// </summary>
        [Category("Appearance")]
        public virtual Color TextColor
        {
            get { return _textColor; }
			
            set 
            {
                if (_textColor != value)
                {
                    _textColor = value;

                    _recalculate = true;
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Decide if the text color needs to be persisted.
		/// </summary>
		/// <returns>Should text color be persisted.</returns>
        private bool ShouldSerializeTextColor()
        {
            return _textColor != TabControl.DefaultForeColor;
        }

		/// <summary>
		/// Resets the TextColor property to its default value.
		/// </summary>
        public void ResetTextColor()
        {   
            TextColor = TabControl.DefaultForeColor;
        }

		/// <summary>
		/// Gets and sets the text color to use when drawing inactive tab headers.
		/// </summary>
        [Category("Appearance")]
        public virtual Color TextInactiveColor
        {
            get { return _textInactiveColor; }
			
            set 
            {
                if (_textInactiveColor != value)
                {
                    _textInactiveColor = value;

                    _recalculate = true;
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Decide if the text inactive color needs to be persisted.
		/// </summary>
		/// <returns>Should text inactive color be persisted.</returns>
        private bool ShouldSerializeTextInactiveColor()
        {
            return _textInactiveColor != Color.FromArgb(128, TabControl.DefaultForeColor);
        }

		/// <summary>
		/// Resets the TextInactiveColor property to its default value.
		/// </summary>
        public void ResetTextInactiveColor()
        {
            TextInactiveColor = Color.FromArgb(128, TabControl.DefaultForeColor);
        }

		/// <summary>
		/// Gets the bounding rectangle of the tab headers area.
		/// </summary>
        [Browsable(false)]
        public virtual Rectangle TabsAreaRect
        {
            get { return _tabsAreaRect; }
        }

		/// <summary>
		/// Decide if the bounding rectangle of the tab headers needs to be persisted.
		/// </summary>
		/// <returns>Should bounding rectangle be persisted.</returns>
		private bool ShouldSerializeTabsAreaRect()
		{
			return false;
		}

		/// <summary>
		/// Gets the bounding rectangle of the page area.
		/// </summary>
		[Browsable(false)]
		public virtual Rectangle PageAreaRect
		{
			get { return _pageAreaRect; }
		}

		/// <summary>
		/// Decide if the bounding rectangle of the page area needs to be persisted.
		/// </summary>
		/// <returns>Should bounding rectangle be persisted.</returns>
		private bool ShouldSerializePageAreaRect()
		{
			return false;
		}

		/// <summary>
		/// Gets and sets the image list used to draw tab header pictures.
		/// </summary>
        [Category("Appearance")]
        public virtual ImageList ImageList
        {
            get { return _imageList; }

            set 
            {
                if (_imageList != value)
                {
                    _imageList = value;

                    _recalculate = true;
                    Invalidate();
                }
            }
        }
		
		/// <summary>
		/// Decide if the image list needs to be persisted.
		/// </summary>
		/// <returns>Should the image list be persisted.</returns>
        private bool ShouldSerializeImageList()
        {
            return _imageList != null;
        }
        
		/// <summary>
		/// Resets the ImageList property to its default value.
		/// </summary>
        public void ResetImageList()
        {
            ImageList = null;
        }

		/// <summary>
		/// Gets and sets the position of tab headers.
		/// </summary>
        [Category("Appearance")]
        public virtual bool PositionTop
        {
            get { return _positionAtTop; }
			
            set
            {
                if (_positionAtTop != value)
                {
                    _positionAtTop = value;

                    _recalculate = true;
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Decide if the tab headers position needs to be persisted.
		/// </summary>
		/// <returns>Should the tab headers position be persisted.</returns>
        protected bool ShouldSerializePositionTop()
        {
            switch(_appearance)
            {
                case VisualAppearance.MultiBox:
                case VisualAppearance.MultiForm:
                    return _positionAtTop != false;
                case VisualAppearance.MultiDocument:
                default:
                    return _positionAtTop != true;
            }
        }

		/// <summary>
		/// Resets the PositionTop property to its default value.
		/// </summary>
        public void ResetPositionTop()
        {
            switch(_appearance)
            {
                case VisualAppearance.MultiBox:
                case VisualAppearance.MultiForm:
                    PositionTop = false;
                    break;
                case VisualAppearance.MultiDocument:
                default:
                    PositionTop = true;
                    break;
            }
        }

		/// <summary>
		/// Gets and sets the value indicating if the close button is displayed.
		/// </summary>
        [Category("Appearance")]
        public virtual bool ShowClose
        {
            get { return _showClose; }
			
            set
            {
                if (_showClose != value)
                {
                    _showClose = value;

                    _recalculate = true;
                    Invalidate();
                }
            }
        }
        
		/// <summary>
		/// Decide if displaying the close button needs to be persisted.
		/// </summary>
		/// <returns>Should displaying the close button be persisted.</returns>
        protected bool ShouldSerializeShowClose()
        {
            switch(_appearance)
            {
                case VisualAppearance.MultiBox:
                case VisualAppearance.MultiForm:
                    return _showClose != false;
                case VisualAppearance.MultiDocument:
                default:
                    return _showClose != true;
            }
        }

		/// <summary>
		/// Resets the ShowClose property to its default value.
		/// </summary>
        public void ResetShowClose()
        {
            switch(_appearance)
            {
                case VisualAppearance.MultiBox:
                case VisualAppearance.MultiForm:
                    ShowClose = false;
                    break;
                case VisualAppearance.MultiDocument:
                default:
                    ShowClose = true;
                    break;
            }
        }

		/// <summary>
		/// Gets and sets the value indicating if the arrow buttons are displayed.
		/// </summary>
        [Category("Appearance")]
        public virtual bool ShowArrows
        {
            get { return _showArrows; }
			
            set
            {
                if (_showArrows != value)
                {
                    _showArrows = value;

                    _recalculate = true;
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Decide if displaying the arrow buttons needs to be persisted.
		/// </summary>
		/// <returns>Should displaying the arrow buttons be persisted.</returns>
        protected bool ShouldSerializeShowArrows()
        {
            switch(_appearance)
            {
                case VisualAppearance.MultiBox:
                case VisualAppearance.MultiForm:
                    return _showArrows != false;
                case VisualAppearance.MultiDocument:
                default:
                    return _showArrows != true;
            }
        }
        
		/// <summary>
		/// Resets the ShowArrows property to its default value.
		/// </summary>
        public void ResetShowArrows()
        {
            switch(_appearance)
            {
                case VisualAppearance.MultiBox:
                case VisualAppearance.MultiForm:
                    ShowArrows = false;
                    break;
                case VisualAppearance.MultiDocument:
                default:
                    ShowArrows = true;
                    break;
            }
        }

		/// <summary>
		/// Gets and sets the value indicating if the drop select arrow should be displayed.
		/// </summary>
        [Category("Appearance")]
		[DefaultValue(true)]
        public virtual bool ShowDropSelect
        {
            get { return _showDropSelect; }
			
            set
            {
                if (_showDropSelect != value)
                {
                    _showDropSelect = value;

                    _recalculate = true;
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Resets the ShowDropSelect property to its default value.
		/// </summary>
        public void ResetShowDropSelect()
        {
            ShowDropSelect = true;
        }

		/// <summary>
		/// Gets and sets the value indicating if tab headers are resized to fit control width.
		/// </summary>
        [Category("Appearance")]
        public virtual bool ShrinkPagesToFit
        {
            get { return _shrinkPagesToFit; }
			
            set 
            {
                if (_shrinkPagesToFit != value)
                {
                    _shrinkPagesToFit = value;

                    _recalculate = true;
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Decide if shrinking tab headers needs to be persisted.
		/// </summary>
		/// <returns>Should shrinking tab headers be persisted.</returns>
        protected bool ShouldSerializeShrinkPagesToFit()
        {
            switch(_appearance)
            {
                case VisualAppearance.MultiBox:
                case VisualAppearance.MultiForm:
                    return _shrinkPagesToFit != true;
                case VisualAppearance.MultiDocument:
                default:
                    return _shrinkPagesToFit != false;
            }
        }
        
		/// <summary>
		/// Resets the ShrinkPagesToFit property to its default value.
		/// </summary>
        public void ResetShrinkPagesToFit()
        {
            switch(_appearance)
            {
                case VisualAppearance.MultiBox:
                case VisualAppearance.MultiForm:
                    ShrinkPagesToFit = true;
                    break;
                case VisualAppearance.MultiDocument:
                default:
                    ShrinkPagesToFit = false;
                    break;
            }
        }
		
		/// <summary>
		/// Gets and sets the value indicating Office2003/Office2007 style should added extra dock specific fade lines.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual bool OfficeDockSides
		{
			get { return _officeDockSides; }
			
			set 
			{
				if (_officeDockSides != value)
				{
					_officeDockSides = value;

					_recalculate = true;
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the OfficeDockSides property to its default value.
		/// </summary>
		public void ResetOfficeDockSides()
		{
			OfficeDockSides = false;
		}

        /// <summary>
        /// Gets and sets the value indicating MediaPlayer style should added extra dock specific fade lines.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public virtual bool MediaPlayerDockSides
        {
            get { return _mediaPlayerDockSides; }

            set
            {
                if (_mediaPlayerDockSides != value)
                {
                    _mediaPlayerDockSides = value;

                    _recalculate = true;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Resets the MediaPlayerDockSides property to its default value.
        /// </summary>
        public void ResetMediaPlayerDockSides()
        {
            MediaPlayerDockSides = false;
        }
        
        /// <summary>
		/// Gets and sets the value indicating if the selected page is drawn bold.
		/// </summary>
        [Category("Appearance")]
        public virtual bool BoldSelectedPage
        {
            get { return _boldSelected; }
			
            set 
            {
                if (_boldSelected != value)
                {
                    _boldSelected = value;

                    _recalculate = true;
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Decide if drawing the selected page as bold needs to be persisted.
		/// </summary>
		/// <returns>Should rawing the selected page as bold be persisted.</returns>
        protected bool ShouldSerializeBoldSelectedPage()
        {
            switch(_appearance)
            {
                case VisualAppearance.MultiBox:
                case VisualAppearance.MultiForm:
                    return _boldSelected != false;
                case VisualAppearance.MultiDocument:
                default:
                    return _boldSelected != true;
            }
        }
        
		/// <summary>
		/// Resets the BoldSelectedPage property to its default value.
		/// </summary>
        public void ResetBoldSelectedPage()
        {
            switch(_appearance)
            {
                case VisualAppearance.MultiBox:
                case VisualAppearance.MultiForm:
                    BoldSelectedPage = false;
                    break;
                case VisualAppearance.MultiDocument:
                default:
                    BoldSelectedPage = true;
                    break;
            }
        }

		/// <summary>
		/// Gets and sets a value indicating if tab header lines should stretch full width when in multiline mode.
		/// </summary>
        [Category("Appearance")]
        [DefaultValue(false)]
        public virtual bool MultilineFullWidth
        {
            get { return _multilineFullWidth; }
            
            set
            {
                if (_multilineFullWidth != value)
                {
                    _multilineFullWidth = value;
                    
                    _recalculate = true;
                    Invalidate();
                }
            }
        }
        
		/// <summary>
		/// Resets the MultilineFullWidth property to its default value.
		/// </summary>
        public void ResetMultilineFullWidth()
        {
            MultilineFullWidth = false;
        }

		/// <summary>
		/// Gets and set a value indicating if tab headers should cross multiple lines.
		/// </summary>
        [Category("Appearance")]
        [DefaultValue(false)]
        public virtual bool Multiline
        {
            get { return _multiline; }
			
            set 
            {
                if (_multiline != value)
                {
                    _multiline = value;

                    _recalculate = true;
                    Invalidate();
                }
            }
        }
        
		/// <summary>
		/// Resets the Multiline property to its default value.
		/// </summary>
        public void ResetMultiline()
        {
            Multiline = false;
        }

		/// <summary>
		/// Gets and sets the left offset used for positioning tab page controls.
		/// </summary>
        [Category("Appearance")]
        [DefaultValue(0)]
        public virtual int ControlLeftOffset
        {
            get { return _ctrlLeftOffset; }
			
            set 
            {
                if (_ctrlLeftOffset != value)
                {
                    _ctrlLeftOffset = value;

                    Recalculate();
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Resets the ControlLeftOffset property to its default value.
		/// </summary>
        public void ResetControlLeftOffset()
        {
            ControlLeftOffset = 0;
        }

		/// <summary>
		/// Gets and sets the top offset used for positioning tab page controls.
		/// </summary>
        [Category("Appearance")]
        [DefaultValue(0)]
        public virtual int ControlTopOffset
        {
            get { return _ctrlTopOffset; }
			
            set 
            {
                if (_ctrlTopOffset != value)
                {
                    _ctrlTopOffset = value;

                    Recalculate();
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Resets the ControlTopOffset property to its default value.
		/// </summary>
        public void ResetControlTopOffset()
        {
            ControlTopOffset = 0;
        }

		/// <summary>
		/// Gets and sets the right offset used for positioning tab page controls.
		/// </summary>
        [Category("Appearance")]
        [DefaultValue(0)]
        public virtual int ControlRightOffset
        {
            get { return _ctrlRightOffset; }
			
            set 
            {
                if (_ctrlRightOffset != value)
                {
                    _ctrlRightOffset = value;

                    Recalculate();
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Resets the ControlRightOffset property to its default value.
		/// </summary>
        public void ResetControlRightOffset()
        {
            ControlRightOffset = 0;
        }

		/// <summary>
		/// Gets and sets the bottom offset used for positioning tab page controls.
		/// </summary>
        [Category("Appearance")]
        [DefaultValue(0)]
        public virtual int ControlBottomOffset
        {
            get { return _ctrlBottomOffset; }
			
            set 
            {
                if (_ctrlBottomOffset != value)
                {
                    _ctrlBottomOffset = value;

                    Recalculate();
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Resets the ControlBottomOffset property to its default value.
		/// </summary>
        public void ResetControlBottomOffset()
        {
            ControlBottomOffset = 0;
        }

		/// <summary>
		/// Gets and sets the extra space to inset the first tab from left.
		/// </summary>
		[Category("Office")]
		[DefaultValue(0)]
		public virtual int OfficeExtraTabInset
		{
			get { return _extraOfficeTabInset; }
			
			set 
			{
				if (_extraOfficeTabInset != value)
				{
					_extraOfficeTabInset = value;

					Recalculate();
					Invalidate();
				}
			}
		}

        /// <summary>
        /// Gets and sets the extra space to inset the first tab from left.
        /// </summary>
        [Category("MediaPlayer")]
        [DefaultValue(0)]
        public virtual int MediaPlayerExtraTabInset
        {
            get { return _extraMediaPlayerTabInset; }

            set
            {
                if (_extraMediaPlayerTabInset != value)
                {
                    _extraMediaPlayerTabInset = value;

                    Recalculate();
                    Invalidate();
                }
            }
        }
        
        /// <summary>
		/// Gets and sets the extra space to inset the first tab from left.
		/// </summary>
		[Category("Appearance")]
		[DefaultValue(0)]
		public virtual int IDE2005ExtraTabInset
		{
			get { return _extraIDE2005TabInset; }
			
			set 
			{
				if (_extraIDE2005TabInset != value)
				{
					_extraIDE2005TabInset = value;

					Recalculate();
					Invalidate();
				}
			}
		}

		private int GetExtraInset()
		{
            if (IsOfficeStyle)
			{
				if (_officeHeaderBorder)
					return OFFICE_HEADER_INSET;
				else
					return _extraOfficeTabInset;
			}
            else if (IsMediaPlayerStyle)
            {
                if (_mediaPlayerHeaderBorder)
                    return OFFICE_HEADER_INSET;
                else
                    return _extraOfficeTabInset;
            }
            else if (_style == VisualStyle.IDE2005)
			{
				if (_ide2005HeaderBorder)
					return IDE2005_HEADER_INSET;
				else
					return _extraIDE2005TabInset;
			}

			return 0;
		}

		/// <summary>
		/// Resets the OfficeExtraTabInset property to its default value.
		/// </summary>
		public void ResetOfficeExtraTabInset()
		{
			OfficeExtraTabInset = 0;
		}

		/// <summary>
		/// Gets and sets a value indicating that in plain style if the control area is flat or bumped.
		/// </summary>
        [Category("Appearance")]
        [DefaultValue(true)]
        public virtual bool InsetPlain
        {
            get { return _insetPlain; }
			
            set 
            {
                if (_insetPlain != value)
                {
                    _insetPlain = value;

                    Recalculate();
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Resets the InsetPlain property to its default value.
		/// </summary>
        public void ResetInsetPlain()
        {
            InsetPlain = true;
        }

		/// <summary>
		/// Gets and sets a value indicating that in plain style it will only show the inset border nearest the tabs and not all four borders.
		/// </summary>
        [Category("Appearance")]
        [DefaultValue(false)]
        public virtual bool InsetBorderPagesOnly
        {
            get { return _insetBorderPagesOnly; }
			
            set 
            {
                if (_insetBorderPagesOnly != value)
                {
                    _insetBorderPagesOnly = value;

                    Recalculate();
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Resets the InsetBorderPagesOnly property to its default value.
		/// </summary>
        public void ResetInsetBorderPagesOnly()
        {
            InsetBorderPagesOnly = true;
        }

		/// <summary>
		/// Gets and sets a value indicating that in Office2003/Office2007 style a single pixel border is drawn around the entire control.
		/// </summary>
		[Category("Office")]
		[DefaultValue(true)]
		public virtual bool OfficePixelBorder
		{
			get { return _officePixelBorder; }
			
			set 
			{
				if (_officePixelBorder != value)
				{
					_officePixelBorder = value;

					Recalculate();
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the OfficePixelBorder property to its default value.
		/// </summary>
		public void ResetOfficePixelBorder()
		{
			OfficePixelBorder = true;
		}

        /// <summary>
        /// Gets and sets a value indicating that in Media Player style a single pixel border is drawn around the entire control.
        /// </summary>
        [Category("MediaPlayer")]
        [DefaultValue(true)]
        public virtual bool MediaPlayerPixelBorder
        {
            get { return _mediaPlayerPixelBorder; }

            set
            {
                if (_mediaPlayerPixelBorder != value)
                {
                    _mediaPlayerPixelBorder = value;

                    Recalculate();
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Resets the MediaPlayerPixelBorder property to its default value.
        /// </summary>
        public void ResetMediaPlayerPixelBorder()
        {
            MediaPlayerPixelBorder = true;
        }
        
        /// <summary>
		/// Gets and sets a value indicating that in Office2003/Office2007 style a single pixel border is drawn around the header control.
		/// </summary>
		[Category("Office")]
		[DefaultValue(false)]
		public virtual bool OfficeHeaderBorder
		{
			get { return _officeHeaderBorder; }
			
			set 
			{
				if (_officeHeaderBorder != value)
				{
					_officeHeaderBorder = value;

					Recalculate();
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the OfficeHeaderBorder property to its default value.
		/// </summary>
		public void ResetOfficeHeaderBorder()
		{
			OfficeHeaderBorder = false;
		}

        /// <summary>
        /// Gets and sets a value indicating that in Media Player style a single pixel border is drawn around the header control.
        /// </summary>
        [Category("MediaPlayer")]
        [DefaultValue(false)]
        public virtual bool MediaPlayerHeaderBorder
        {
            get { return _mediaPlayerHeaderBorder; }

            set
            {
                if (_mediaPlayerHeaderBorder != value)
                {
                    _mediaPlayerHeaderBorder = value;

                    Recalculate();
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Resets the MediaPlayerHeaderBorder property to its default value.
        /// </summary>
        public void ResetMediaPlayerHeaderBorder()
        {
            MediaPlayerHeaderBorder = false;
        }
        
        /// <summary>
		/// Gets and sets a value indicating that in IDE style a single pixel border is drawn around the entire control.
		/// </summary>
        [Category("Appearance")]
		[DefaultValue(false)]
        public virtual bool IDEPixelBorder
        {
            get { return _idePixelBorder; }
			
            set 
            {
                if (_idePixelBorder != value)
                {
                    _idePixelBorder = value;

                    Recalculate();
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Resets the IDEPixelBorder property to its default value.
		/// </summary>
        public void ResetIDEPixelBorder()
        {
			IDEPixelBorder = false;
        }

		/// <summary>
		/// Gets and sets a value indicating that in IDE style a single pixel border is drawn on the outside edge of tabs area.
		/// </summary>
        [Category("Appearance")]
		[DefaultValue(true)]
        public virtual bool IDEPixelArea
        {
            get { return _idePixelArea; }
			
            set 
            {
                if (_idePixelArea != value)
                {
                    _idePixelArea = value;

                    Recalculate();
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Resets the IDEPixelArea property to its default value.
		/// </summary>
        public void ResetIDEPixelArea()
        {
            IDEPixelArea = true;
        }

		/// <summary>
		/// Gets and sets a value indicating that in IDE2005 style a single pixel border is drawn around the entire control.
		/// </summary>
		[Category("IDE2005")]
		[DefaultValue(true)]
		public virtual bool IDE2005PixelBorder
		{
			get { return _ide2005PixelBorder; }
			
			set 
			{
				if (_ide2005PixelBorder != value)
				{
					_ide2005PixelBorder = value;

					Recalculate();
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the IDE2005PixelBorder property to its default value.
		/// </summary>
		public void ResetIDE2005PixelBorder()
		{
			IDE2005PixelBorder = true;
		}

		/// <summary>
		/// Gets and sets a value indicating that in IDE2005 style a single pixel border is drawn around the header control.
		/// </summary>
		[Category("IDE2005")]
		[DefaultValue(false)]
		public virtual bool IDE2005HeaderBorder
		{
			get { return _ide2005HeaderBorder; }
			
			set 
			{
				if (_ide2005HeaderBorder != value)
				{
					_ide2005HeaderBorder = value;

					Recalculate();
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the IDE2005HeaderBorder property to its default value.
		/// </summary>
		public void ResetIDE2005HeaderBorder()
		{
			IDE2005HeaderBorder = false;
		}

		/// <summary>
		/// Gets and sets a value indicating that in IDE2005 styles should draw the tab joined to the main area.
		/// </summary>
		[Category("IDE2005")]
		[DefaultValue(true)]
		public virtual bool IDE2005TabJoined
		{
			get { return _ide2005TabJoined; }
			
			set 
			{
				if (_ide2005TabJoined != value)
				{
					_ide2005TabJoined = value;
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the IDE2005TabJoined property to its default value.
		/// </summary>
		public void ResetIDE2005TabJoined()
		{
			IDE2005TabJoined = true;
		}

		/// <summary>
		/// Gets and sets a value indicating if only the selected tab has its text drawn.
		/// </summary>
        [Category("Appearance")]
        [DefaultValue(false)]
        public virtual bool SelectedTextOnly
        {
            get { return _selectedTextOnly; }
			
            set 
            {
                if (_selectedTextOnly != value)
                {
                    _selectedTextOnly = value;

                    _recalculate = true;
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Resets the SelectedTextOnly property to its default value.
		/// </summary>
        public void ResetSelectedTextOnly()
        {
            SelectedTextOnly = false;
        }

		/// <summary>
		/// Gets and sets a delay time used to decide when tab headers are hidden when mouse leaves tab headers and HideTabsMode is defined as HideWithoutMouse.
		/// </summary>
        [Category("Behavior")]
        [DefaultValue(200)]
        public int MouseLeaveTimeout
        {
            get { return _leaveTimeout; }
            
            set 
            { 
                if (_leaveTimeout != value)
                {
                    _leaveTimeout = value;
                    _overTimer.Interval = value;
                }
            }
        }

		/// <summary>
		/// Resets the MouseLeaveTimeout property to its default value.
		/// </summary>
        public void ResetMouseLeaveTimeout()
        {
            MouseLeaveTimeout = 200;
        }
        
		/// <summary>
		/// Gets and sets the current tab header display mode.
		/// </summary>
        [Category("Appearance")]
        [DefaultValue(typeof(HideTabsModes), "ShowAlways")]
        public virtual HideTabsModes HideTabsMode
        {
            get { return _hideTabsMode; }
			
            set 
            {
                if (_hideTabsMode != value)
                {
                    _hideTabsMode = value;

                    Recalculate();
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Determine if the hide tabs mode needs to be persisted.
		/// </summary>
		/// <returns>Should HideTabsMode be persisted.</returns>
        protected bool ShouldSerializeHideTabsMode()
        {
            return HideTabsMode != HideTabsModes.ShowAlways;
        }

		/// <summary>
		/// Resets the HideTabsMode property to its default value.
		/// </summary>
        public void ResetHideTabsMode()
        {
            HideTabsMode = HideTabsModes.ShowAlways;
        }


		/// <summary>
		/// Gets and sets the vector used to test if mouse over should show tab headers.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(-1)]
		[Description("Vector used to test if mouse over should show tab headers")]
		public int WithoutMouseVector
		{
			get { return _withoutMouseVector; }
			set { _withoutMouseVector = value; }
		}

		/// <summary>
		/// Resets the WithoutMouseVector property to its default value.
		/// </summary>
		public void ResetWithoutMouseVector()
		{
			WithoutMouseVector = -1;
		}

		/// <summary>
		/// Gets and sets a value indicating if hovering over a tab header causes its selection.
		/// </summary>
        [Category("Appearance")]
        [DefaultValue(false)]
        public virtual bool HoverSelect
        {
            get { return _hoverSelect; }
			
            set 
            {
                if (_hoverSelect != value)
                {
                    _hoverSelect = value;

                    _recalculate = true;
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Resets the HoverSelect property to its default value.
		/// </summary>
        public void ResetHoverSelect()
        {
            HoverSelect = false;
        }

		/// <summary>
		/// Gets and sets teh maximum width allowed for the tab header.
		/// </summary>
		[Category("Appearance")]
		[DefaultValue(-1)]
		public int MaximumHeaderWidth
		{
			get { return _maximumHeaderWidth; }
			
			set
			{
				if (_maximumHeaderWidth != value)
				{
					_maximumHeaderWidth = value;

					_recalculate = true;
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the MaximumHeaderWidth property to its default value.
		/// </summary>
		public void ResetMaximumHeaderWidth()
		{
			MaximumHeaderWidth = -1;
		}

		/// <summary>
		/// Gets and sets a value indicating if focus is remembered when moving between pages.
		/// </summary>
        [Category("Behavior")]
        [DefaultValue(true)]
        public virtual bool RecordFocus
        {
            get { return _recordFocus; }
			
            set 
            {
                if (_recordFocus != value)
                    _recordFocus = value;
            }
        }

		/// <summary>
		/// Resets the RecordFocus property to its default value.
		/// </summary>
        public void ResetRecordFocus()
        {
            RecordFocus = true;
        }

		/// <summary>
		/// Gets and sets the index of the selected tab page in the TabPages collection.
		/// </summary>
        [Browsable(false)]
        [DefaultValue(-1)]
        public virtual int SelectedIndex
        {
            get { return _pageSelected; }

            set
            {
				if (_suspended)
				{
					// Remember the page required to be selected
					_pageSelected = value;
				}
				else
				{
					if ((value >= 0) && (value < _tabPages.Count))
					{
						if (_pageSelected != value)
						{
							TabPage oldPage = null;
							TabPage newPage = null;

							// Any page currently selected?
							if (_pageSelected != -1)
								oldPage = _tabPages[_pageSelected];

							// Any page to become selected?
							if (value != -1)
								newPage = _tabPages[value];

							// Raise selection changing event
							OnSelectionChanging(oldPage, newPage);

							// Any page currently selected?
							if (oldPage != null)
								DeselectPage(oldPage);

							_pageSelected = value;

							if (newPage != null)
							{
								SelectPage(newPage);

								// Ensure that any newly selected page can be seen
								MakePageVisible(newPage);
							}

							// Change in selection causes tab pages sizes to change
							Recalculate();
							Invalidate();

							// Raise selection change event
							OnSelectionChanged(oldPage, newPage);
						}
					}
				}
            }
        }

		/// <summary>
		/// Gets and sets a reference to the selected tab page in the TabPages collection.
		/// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual TabPage SelectedTab
        {
            get 
            {
                try
                {
                    // If nothing is selected we return null
                    if (_pageSelected == -1)
                        return null;
                    else
                        return _tabPages[_pageSelected];
                }
                catch
                {
                    return null;
                }
            }

            set
            {
                // Cannot change selection to be none of the tabs
                if (value != null)
                {
                    // Get the requested page from the collection
                    int index = _tabPages.IndexOf(value);

                    // If a valid known page then using existing property to perform switch
                    if (index != -1)
                        this.SelectedIndex = index;
                }
            }
        }

		/// <summary>
		/// Get the TabPage that uses the given control for display.
		/// </summary>
		/// <param name="c">Control to search for.</param>
		/// <returns>TabPage that uses this control for display.</returns>
		public TabPage TabPageFromControl(Control c)
		{
			foreach(TabPage page in TabPages)
				if (page.Control == c)
					return page;

			// Not found!
			return null;
		}

		/// <summary>
		/// Find the TabPage that is under the client mouse position in the tab header area.
		/// </summary>
		/// <param name="mousePos">Client point to test.</param>
		/// <returns>TabPage reference if match found; otherwise null.</returns>
		public TabPage TabPageFromPoint(Point mousePos)
		{
			// Mouse inside rectangle of a tabpage header?
			for(int i=0; i<_tabPages.Count; i++)
			{
				Rectangle rect = (Rectangle)_tabRects[i];

				// Does tab page rectangle contain mouse?
				if (rect.Contains(mousePos))
				{
					// Is active button showing?
					if (_activeArrow.Visible)
					{
						// Ignore mouse down over then buttons area
						if (mousePos.X >= _activeArrow.Left)
							return null;
					}
					else
					{
						// Is left scroll button showing?
						if (_leftArrow.Visible)
						{
							// Ignore mouse down over then buttons area
							if (mousePos.X >= _leftArrow.Left)
								return null;
						}
						else
						{
							// No, is the close button showing?
							if (_closeButton.Visible)
							{
								// Ignore mouse down over then close button area
								if (mousePos.X >= _closeButton.Left)
									return null;
							}
						}
					}

					return _tabPages[i];
				}
			}

			// No matching tab header found
			return null;
		}

		/// <summary>
		/// Find the TabPage that is under the client mouse position in the tab header area.
		/// </summary>
		/// <param name="mousePos">Client point to test.</param>
		/// <returns>TabPage reference if match found; otherwise null.</returns>
		public TabPage TabPageFromDraggingPoint(Point mousePos)
		{
			// Page reference to return
			TabPage retPage = null;

			// Track which page was the last one to be shown
			int lastVisible = -1;

			// Cache the right hand side limit of the control
			int rightLimit = Width;

			// Reduce by any showing close button
			if (_closeButton.Visible)
				rightLimit = _closeButton.Left - 1;

			// Reduce by any showing left button
			if (_leftArrow.Visible)
				rightLimit = _leftArrow.Left - 1;

			// Reduce by any showing active button
			if (_activeArrow.Visible)
				rightLimit = _activeArrow.Left - 1;

			// Cache the size of the screen the mouse is on 
			Rectangle screenRect = Screen.GetBounds(mousePos); 

			// Mouse inside rectangle of a tabpage header?
			for(int i=_startPage; i<_tabPages.Count; i++)
			{
				Rectangle rect = (Rectangle)_tabRects[i];

				// Remember this tab if it is visible
				if (rect.Left < rightLimit)
					lastVisible = i;

				// If there is another tab page after this one
				if (i < (_tabPages.Count -1))
				{
					// Grab its rectangle
					Rectangle nextRect = (Rectangle)_tabRects[i+1];

					// Make sure we extend right upto its left edge
					rect.Width = nextRect.Left - rect.Left;
				}

				// Make the test area the full height of screen so we dont miss it!
				rect.Y = screenRect.Top;
				rect.Height = screenRect.Height;

				// Does tab page rectangle contain mouse?
				if (rect.Contains(mousePos))
				{
					// Is active button showing?
					if (_activeArrow.Visible)
					{
						// Ignore mouse down over the buttons area
						if (mousePos.X >= _activeArrow.Left)
							break;
					}
					else
					{
						// Is left scroll button showing?
						if (_leftArrow.Visible)
						{
							// Ignore mouse down over the buttons area
							if (mousePos.X >= _leftArrow.Left)
								break;
						}
						else
						{
							// No, is the close button showing?
							if (_closeButton.Visible)
							{
								// Ignore mouse down over the close button area
								if (mousePos.X >= _closeButton.Left)
									break;
							}
						}
					}

					// Is the page to the left the currently selected one
					if ((_pageSelected != -1) && (_pageSelected == (i-1)))
					{
						// Get the rectangle for the previous page
						Rectangle prevRect = (Rectangle)_tabRects[i-1];

						// Sets its width to be same as this one
						prevRect.Width = rect.Width;

						// Make the test area the full height of screen so we dont miss it!
						prevRect.Y = screenRect.Top;
						prevRect.Height = screenRect.Height;

						// If contained then do not select! stops a fast switching between
						// two adjacent items because one is much wider than the other, prevents
						// the flicker and fast switching that is very annoying!
						if (prevRect.Contains(mousePos))
						{
							retPage = _tabPages[i-1];
							break;
						}
					}

					retPage = _tabPages[i];
					break;
				}
			}

            if (_startPage >= _tabPages.Count)
                _startPage = _tabPages.Count - 1;

			bool needTimer = false;

			// If we did not find a matching tab header or are very close to the left or right edges...
			if ((retPage == null) || (mousePos.X <= AUTOSCROLL_VECTOR) || (mousePos.X >= Width - AUTOSCROLL_VECTOR))
			{
				// If not allowed to drag outside and we have some pages
				if (_tabPages.Count > 0)
				{
					// Get the first visible page rectangle
					Rectangle rectFirst = (Rectangle)_tabRects[_startPage];
					
					// If mouse before the first visible item
					if (mousePos.X <= (rectFirst.Left + AUTOSCROLL_VECTOR))
					{
						// Then return the first visible page instead
						retPage = _tabPages[_startPage];
						
						// If there are pages that could be scrolled left into view
						if (_startPage > 0)
						{
							needTimer = true;
							_scrollingLeft = true;
						}
					}
					else
					{
						// If we have a found last page
						if (lastVisible >= 0)
						{
							// Get the first visible page rectangle
							Rectangle rectLast = (Rectangle)_tabRects[lastVisible];

							// If mouse is after the last item then make last item selected
							if (mousePos.X >= (rectLast.Left - AUTOSCROLL_VECTOR))
							{
								retPage = _tabPages[lastVisible];

								// If we are allowed to scroll further right
								if (_rightScroll)
								{
									// Need timer to scroll to the right
									needTimer = true;
									_scrollingLeft = false;
								}
							}
							else
							{
								// Return the one that started it all
								retPage = _tabPages[_mouseDownIndex];
							}
						}
						else
						{
							// Return the one that started it all
							retPage = _tabPages[_mouseDownIndex];
						}
					}
				}
			}

			// Decide if the timer should be running
			if (_scrollTimer.Enabled != needTimer)
			{
				try
				{
					if (needTimer)
						_scrollTimer.Start();
					else
						_scrollTimer.Stop();
				} 
				catch {}
			}

			return retPage;
		}

		/// <summary>
		/// Bring the TabPage into view in the tab headers.
		/// </summary>
		/// <param name="page">TabPage reference to make visible.</param>
		public virtual void MakePageVisible(TabPage page)
        {
            MakePageVisible(_tabPages.IndexOf(page));
        }

		/// <summary>
		/// Bring the indexed TabPage into view in the tab headers.
		/// </summary>
		/// <param name="index">Index into TabPages collection of page to make visible.</param>
        public virtual void MakePageVisible(int index)
        {
            // Only relevant if we do not shrink all pages to fit and not in multiline
            if (!_shrinkPagesToFit && !_multiline)
            {
                // Range check the request page
                if ((index >= 0) && (index < _tabPages.Count))
                {
                    // Is requested page before those shown?
                    if (index < _startPage)
                    {
                        // Define it as the new start page
                        _startPage = index;

                        _recalculate = true;
                        Invalidate();
                    }
                    else
                    {
                        // Find the last visible position
                        int xMax = GetMaximumDrawPos();

                        Rectangle rect = (Rectangle)_tabRects[index];

                        // Is the page drawn off over the maximum position?
                        if (rect.Right >= xMax)
                        {
                            // Need to find the new start page to bring this one into view
                            int newStart = index;

                            // Space left over for other tabs to be drawn inside
                            int spaceLeft = xMax - rect.Width - _tabsAreaRect.Left - _position[_styleIndex, (int)PositionIndex.TabsAreaStartInset] + GetExtraInset();

                            do
                            {
                                // Is there a previous tab to check?
                                if (newStart == 0)
                                    break;

                                Rectangle rectStart = (Rectangle)_tabRects[newStart - 1];
		
                                // Is there enough space to draw it?
                                if (rectStart.Width > spaceLeft)
                                    break;

                                // Move to new tab and reduce available space left
                                newStart--;
                                spaceLeft -= rectStart.Width;

                            } while(true);

                            // Define the new starting page
                            _startPage = newStart;

                            _recalculate = true;
                            Invalidate();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Used by the TabControlDesigner only.
        /// </summary>
        /// <param name="hWnd">Source window handle.</param>
        /// <param name="mousePos">Client based mouse position.</param>
        /// <returns>true if double click wanted; otherwise false.</returns>
        public bool WantDoubleClick(IntPtr hWnd, Point mousePos)
        {
            return ControlWantDoubleClick(hWnd, mousePos, _leftArrow) ||
                   ControlWantDoubleClick(hWnd, mousePos, _rightArrow) ||
				   ControlWantDoubleClick(hWnd, mousePos, _activeArrow) ||
                   ControlWantDoubleClick(hWnd, mousePos, _closeButton);
        }

        /// <summary>
        /// Used by the TabControlDesigner only.
        /// </summary>
        /// <param name="hWnd">Source window handle.</param>
        /// <param name="mousePos">Client based mouse position.</param>
        public void ExternalMouseTest(IntPtr hWnd, Point mousePos)
        {
            if (!(ControlMouseTest(hWnd, mousePos, _leftArrow) ||
                  ControlMouseTest(hWnd, mousePos, _rightArrow) ||
				  ControlMouseTest(hWnd, mousePos, _activeArrow) ||
                  ControlMouseTest(hWnd, mousePos, _closeButton)))
                InternalMouseDown(mousePos);
        }

		/// <summary>
		/// Processes a dialog key.
		/// </summary>
		/// <param name="keyData">One of the Keys values that represents the key to process.</param>
		/// <returns>true if the key was processed by the control; otherwise, false.</returns>
		protected override bool ProcessDialogKey(Keys keyData)
		{
            if (AllowCtrlTab)
            {
                // Extract the actual key pressed by removing other modifiers
                Keys actualKey = (keyData & ~(Keys.Shift | Keys.Control));

                // Discover if the modifier keys have been pressed
                bool shiftKey = ((keyData & Keys.Shift) == Keys.Shift);
                bool controlKey = ((keyData & Keys.Control) == Keys.Control);

                // Discover if keys we are interested in have been pressed
                bool tab = (actualKey == Keys.Tab);		// NEXT / PREVIOUS

                // We are only interested in use of Ctrl + Tab
                if (tab && controlKey)
                {
                    // Must be more than one page in order to change selection
                    if (TabPages.Count > 1)
                    {
                        // Shift mean we move backwards one page
                        if (shiftKey)
                        {
                            // Move backwards one page
                            int prevPage = SelectedIndex - 1;

                            // Limit check again end of the list
                            if (prevPage < 0)
                                prevPage = TabPages.Count - 1;

                            // Select the new page
                            SelectedIndex = prevPage;
                        }
                        else
                        {
                            // Move forwards one page
                            int nextPage = SelectedIndex + 1;

                            // Limit check again end of the list
                            if (nextPage >= TabPages.Count)
                                nextPage = 0;

                            // Select the new page
                            SelectedIndex = nextPage;
                        }
                    }

                    return true;
                }
            }

            return base.ProcessDialogKey(keyData);
		}

		/// <summary>
		/// Processes a mnemonic character.
		/// </summary>
		/// <param name="key">The character to process. </param>
		/// <returns>true if processed; otherwise false.</returns>
        protected override bool ProcessMnemonic(char key)
        {
            int total = _tabPages.Count;
            int index = this.SelectedIndex + 1;
            
            for(int count=0; count<total; count++, index++)
            {
                // Range check the index
                if (index >= total)
                    index = 0;

                TabPage page = _tabPages[index];
        
                // Did we find a mnemonic indicator?
                if (IsMnemonic(key, page.Title))
                {
                    // Select this page
                    this.SelectedTab = page;
                
                    return true;
                }
            }
                        
            return false;
        }

		/// <summary>
		/// Raises the Resize event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnResize(EventArgs e)
        {
			Recalculate();
            Invalidate();
            
			base.OnResize(e);
        }

		/// <summary>
		/// Raises the SizeChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnSizeChanged(EventArgs e)
        {
			Recalculate();
            Invalidate();

            base.OnSizeChanged(e);
        }

		/// <summary>
		/// Raises the ClosePressed event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
        private void OnCloseButton(object sender, EventArgs e)
        {
            OnClosePressed(EventArgs.Empty);
        }

		/// <summary>
		/// Move the tab headers to show the previous one not showing.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnLeftArrow(object sender, EventArgs e)
        {
            // Set starting page back one
            _startPage--;

            _recalculate = true;
            Invalidate();
        }
	
		/// <summary>
		/// Show context menu so use can select a page to become active
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnActiveArrow(object sender, EventArgs e)
        {
			// If there is at least one page that can be selected
			if (_tabPages.Count > 0)
			{
				// Use a ContextMenuStrip for displaying the options
                ContextMenuStrip cms = new ContextMenuStrip();

                // Create a menu command per tab page
				foreach(TabPage page in _tabPages)
				{
                    ToolStripMenuItem tsmi = new ToolStripMenuItem(page.Title);
					
					// Does the page specify an image to be used?
					if (page.Image != null)
					{
                        tsmi.Image = page.Image;
					}
					else if (page.ImageIndex >= 0)
					{
                        if (page.ImageList != null)
                            tsmi.Image = page.ImageList.Images[page.ImageIndex];
                        else
                            tsmi.Image = ImageList.Images[page.ImageIndex];
					}

					// Is the page selected?
					if (page.Selected)
                        tsmi.Checked = true;

					// Hook into the user selecting this command
                    tsmi.Click += new EventHandler(OnActiveTabClick);

					// Setup association between menu command and tab page
                    tsmi.Tag = page;

                    cms.Items.Add(tsmi);
				}

				// Show the menu below the active button
				cms.Show(PointToScreen(new Point(_activeArrow.Left, _activeArrow.Bottom + 5)));

				// Redraw the active arrow
				_activeArrow.Invalidate();
			}
        }

		private void OnActiveTabClick(object sender, EventArgs e)
		{
			// Cast to correct type
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;

			// Extract the associated tab page
            TabPage page = tsmi.Tag as TabPage;

			// Select it!
			page.Selected = true;
		}

		/// <summary>
		/// Move the tab headers to show the next one not showing.
		/// </summary>
		/// <param name="sender">Left button that caused the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnRightArrow(object sender, EventArgs e)
        {
            // Set starting page forward one
            _startPage++;

            _recalculate = true;
            Invalidate();
        }

		/// <summary>
		/// Calculate background colors from a base color.
		/// </summary>
		/// <param name="newColor">Base color for calculations.</param>
        protected virtual void DefineBackColor(Color newColor)
        {
            base.BackColor = newColor;

            // Calculate the modified colors from this base
            _backLight = ControlPaint.Light(newColor);
            _backLightLight = ControlPaint.LightLight(newColor);
            _backDark = ControlPaint.Dark(newColor);
            _backDarkDark = ControlPaint.DarkDark(newColor);

            _backIDE = ColorHelper.TabBackgroundFromBaseColor(newColor);
        }
				
		/// <summary>
		/// Recalculate the size and position of tab headers and tab page controls.
		/// </summary>
        protected virtual void Recalculate()
        {
			if (IsDisposed)
				return;

            // The height of a tab button is...
            int tabButtonHeight = _position[_styleIndex, (int)PositionIndex.ImageGapTop] + 
                                  _imageGapTopExtra +
                                  _imageHeight + 
                                  _imageGapBottomExtra + 
                                  _position[_styleIndex, (int)PositionIndex.ImageGapBottom] +
                                  _position[_styleIndex, (int)PositionIndex.BorderBottom]; 

            // The height of the tabs area is...
            int tabsAreaHeight = _position[_styleIndex, (int)PositionIndex.BorderTop] + 
                                 tabButtonHeight + _position[_styleIndex, (int)PositionIndex.TabsBottomGap];

			bool hideTabsArea = HideTabsCalculation();

			// Remember the tabs area rectangle that we start with
			Rectangle startTabsRect = new Rectangle(_tabsAreaRect.Location, _tabsAreaRect.Size);

            // Should the tabs area be hidden?
            if (hideTabsArea)
            {
                // ... then do not show the tabs or button controls
                _pageAreaRect = new Rectangle(0, 0, this.Width, this.Height);
                _tabsAreaRect = new Rectangle(0, 0, 0, 0);
            }
            else
            {
                if (_positionAtTop)
                {
                    // Create rectangle that represents the entire tabs area
                    _pageAreaRect = new Rectangle(0, tabsAreaHeight, this.Width, this.Height - tabsAreaHeight);

                    // Create rectangle that represents the entire area for pages
                    _tabsAreaRect = new Rectangle(0, 0, this.Width, tabsAreaHeight);
                }
                else
                {
                    // Create rectangle that represents the entire tabs area
                    _tabsAreaRect = new Rectangle(0, this.Height - tabsAreaHeight, this.Width, tabsAreaHeight);

                    // Create rectangle that represents the entire area for pages
                    _pageAreaRect = new Rectangle(0, 0, this.Width, this.Height - tabsAreaHeight);
                }
            }

            int xEndPos = 0;

            if (!hideTabsArea && _tabPages.Count > 0)
            {
                // The minimum size of a button includes its left and right borders for width,
                // and then fixed height which is based on the size of the image and font
                Rectangle tabPosition;
				
                if (_positionAtTop)
                    tabPosition = new Rectangle(0,		
                                                _tabsAreaRect.Bottom - tabButtonHeight - 
                                                _position[_styleIndex, (int)PositionIndex.BorderTop],
                                                _position[_styleIndex, (int)PositionIndex.BorderLeft] + 
                                                _position[_styleIndex, (int)PositionIndex.BorderRight],
                                                tabButtonHeight);
                else
                    tabPosition = new Rectangle(0,		
                                                _tabsAreaRect.Top + 
                                                _position[_styleIndex, (int)PositionIndex.BorderTop],
                                                _position[_styleIndex, (int)PositionIndex.BorderLeft] + 
                                                _position[_styleIndex, (int)PositionIndex.BorderRight],
                                                tabButtonHeight);

                // Find starting and ending positons for drawing tabs
                int xStartPos = _tabsAreaRect.Left + _position[_styleIndex, (int)PositionIndex.TabsAreaStartInset] + GetExtraInset();
                xEndPos = GetMaximumDrawPos();

                // Available width for tabs is size between start and end positions
                int xWidth = xEndPos - xStartPos;

                if (_multiline)
                    RecalculateMultilineTabs(xStartPos, xEndPos, tabPosition, tabButtonHeight);
                else
                    RecalculateSinglelineTabs(xWidth, xStartPos, tabPosition);
            }

			// No need to position child controls during a dragging of tabs operation
			if (!_leftMouseDownDrag)
			{
				// Position of Controls defaults to the entire page area
				_pageRect = _pageAreaRect;

				// Adjust child controls positions depending on style
				if ((_style == VisualStyle.Plain) && (_appearance != VisualAppearance.MultiBox))
				{
					_pageRect = _pageAreaRect;

					// Shrink by having a border on left,top and right borders
					_pageRect.X += PLAIN_BORDER_DOUBLE;
					_pageRect.Width -= (PLAIN_BORDER_DOUBLE * 2) - 1;

					if (!_positionAtTop)
						_pageRect.Y += PLAIN_BORDER_DOUBLE;

					_pageRect.Height -= PLAIN_BORDER_DOUBLE - 1;
					
					// If hiding the tabs then need to adjust the controls positioning
					if (hideTabsArea)
					{
						_pageRect.Height -= PLAIN_BORDER_DOUBLE;

						if (_positionAtTop)
							_pageRect.Y += PLAIN_BORDER_DOUBLE;
					}
				}

				// Calculate positioning of the child controls/forms
				int leftOffset = _ctrlLeftOffset;
				int rightOffset = _ctrlRightOffset;
				int topOffset = _ctrlTopOffset;
				int bottomOffset = _ctrlBottomOffset;

                if ((_officePixelBorder && IsOfficeStyle) ||
                    (_mediaPlayerPixelBorder && IsMediaPlayerStyle) ||
				    (_ide2005PixelBorder && (_style == VisualStyle.IDE2005)))
				{
					leftOffset += 1;
					rightOffset += 1;

					if (_positionAtTop || hideTabsArea)
						bottomOffset += 1;
		                
					if (!_positionAtTop || hideTabsArea)
						topOffset += 1;
				}
                else if ((_officeDockSides && IsOfficeStyle && !_positionAtTop) ||
                         (_mediaPlayerDockSides && IsMediaPlayerStyle && !_positionAtTop))
				{
					bool vertical = false;

					if (AllowSideCaptions &&
					    (Parent != null) && 
						(Parent.Parent != null) &&
						((Parent.Parent.Dock == DockStyle.Top) ||
						 (Parent.Parent.Dock == DockStyle.Bottom)))
						vertical = true;

					rightOffset += 1;

					if (vertical)
						topOffset += 1;
					else
						leftOffset += 1;

					if (hideTabsArea)
						bottomOffset += 1;
				}
	        
				Point pageLoc = new Point(_pageRect.Left + leftOffset,
										  _pageRect.Top + topOffset);

				Size pageSize = new Size(_pageRect.Width - leftOffset - rightOffset,
										 _pageRect.Height - topOffset - bottomOffset);

				// If in Plain style and requested to only show top or bottom border
				if ((_style == VisualStyle.Plain) && _insetBorderPagesOnly)
				{
					// Then need to increase width to occupy where borders would have been 
					pageLoc.X -= PLAIN_BORDER_DOUBLE;
					pageSize.Width += PLAIN_BORDER_DOUBLE * 2;

					if (hideTabsArea || _positionAtTop)
					{
						// Draw into the bottom border area
						pageSize.Height += PLAIN_BORDER_DOUBLE;
					}

					if (hideTabsArea || !_positionAtTop)
					{
						// Draw into the top border area
						pageLoc.Y -= PLAIN_BORDER_DOUBLE;
						pageSize.Height += PLAIN_BORDER_DOUBLE;
					}
				}

				// Position each page control correctly
				foreach(TabPage page in _tabPages)
				{
					if (page.Control != null)
						page.Control.SetBounds(pageLoc.X, pageLoc.Y, pageSize.Width, pageSize.Height);
					else
						page.SetBounds(pageLoc.X, pageLoc.Y, pageSize.Width, pageSize.Height);
				}

				// If we have any tabs at all
				if (_tabPages.Count > 0)
				{
					Rectangle rect = (Rectangle)_tabRects[_tabPages.Count - 1];
					
					// Determine if the right scrolling button should be enabled
					_rightScroll = (rect.Right > xEndPos);
				}
				else
				{
					// No pages means there can be no right scrolling
					_rightScroll = false;
				}

				// Determine if left scrolling is possible
				_leftScroll = (_startPage > 0);

                // Handle then display and positioning of buttons
				RecalculateButtons();
			}

			// Has the tabs area rect changed?
			if (!startTabsRect.Equals(_tabsAreaRect))
			{
				// Raise an event to notify change in tabs area
				OnTabsAreaRectChanged();
			}

			// Reset the need for a recalculation
			_recalculate = false;
		}

		/// <summary>
		/// Recalculate tab headers when using multiline display.
		/// </summary>
		/// <param name="xStartPos">Left starting position.</param>
		/// <param name="xEndPos">Right maximum position.</param>
		/// <param name="tabPosition">Rectangle for tab position.</param>
		/// <param name="tabButtonHeight">Height of tab buttons.</param>
        protected virtual void RecalculateMultilineTabs(int xStartPos, 
														int xEndPos, 
                                                        Rectangle tabPosition, 
														int tabButtonHeight)
        {
            using (Graphics g = this.CreateGraphics())
            {
                // MultiBox style needs a pixel extra drawing room on right
                if (_appearance == VisualAppearance.MultiBox)
                    xEndPos-=2;
                        
                // How many tabs on this line
                int lineCount = 0;
                            
                // Remember which line is the first displayed
                _topYPos = tabPosition.Y;
                            
                // Next tab starting position
                int xPos = xStartPos;
                int yPos = tabPosition.Y;
                            
                // How many full lines were there
                int fullLines = 0;
                            
                // Line increment value
                int lineIncrement = tabButtonHeight + 1;

                // Track which line has the selection on it                                
                int selectedLine = 0;

                // Vertical adjustment
                int yAdjust = 0;
                        
                // Create array for holding lines of tabs
                ArrayList lineList = new ArrayList();
                            
                // Add the initial line
                lineList.Add(new ArrayList());
                        
                // Process each tag page in turn
                for(int i=0; i<_tabPages.Count; i++)
                {
                    // Get the tab instance for this position
                    TabPage page = _tabPages[i];
                            
                    // Find out the tabs total width
                    int tabWidth = GetTabPageSpace(g, page);
                                
                    // If not the first on the line, then check if newline should be started
                    if (lineCount > 0)
                    {
                        // Does this tab extend pass end of the lines
                        if ((xPos + tabWidth) > xEndPos)
                        {
                            // Next tab position is down a line and back to the start
                            xPos = xStartPos;
                            yPos += lineIncrement;
                                        
                            // Remember which line is the last displayed
                            _bottomYPos = tabPosition.Y;

                            // Increase height of the tabs area
                            _tabsAreaRect.Height += lineIncrement;
                                        
                            // Decrease height of the control area
                            _pageAreaRect.Height -= lineIncrement;
                                        
                            // Moving areas depends on drawing at top or bottom
                            if (_positionAtTop)
                                _pageAreaRect.Y += lineIncrement;
                            else
                            {
                                yAdjust -= lineIncrement;
                                _tabsAreaRect.Y -= lineIncrement;
                            }
                                        
                            // Start a new line 
                            lineList.Add(new ArrayList());
                                        
                            // Make sure the entries are aligned to fill entire line
                            fullLines++;
                        }
                    }

                    // Limit the width of a tab to the whole line
                    if (tabWidth > (xEndPos - xStartPos))
                        tabWidth = xEndPos - xStartPos;
                                
                    // Construct rectangle for representing this tab
                    Rectangle tabRect = new Rectangle(xPos, yPos, tabWidth, tabButtonHeight);
                                
                    // Add this tab to the current line array
                    ArrayList thisLine = lineList[lineList.Count - 1] as ArrayList;
                                
                    // Create entry to represent the sizing of the given page index
                    MultiRect tabEntry = new MultiRect(tabRect, i);
                                
                    thisLine.Add(tabEntry);
                                
                    // Track which line has the selection on it
                    if (i == _pageSelected)
                        selectedLine = fullLines;
                                
                    // Move position of next tab along
                    xPos += tabWidth + 1;
                                
                    // Increment number of tabs on this line
                    lineCount++;
                }

                int line = 0;

                // Do we need all lines to extend full width
                if (_multilineFullWidth)
                    fullLines++;
                            
                // Make each full line stretch the whole line width
                foreach(ArrayList lineArray in lineList)
                {
                    // Only right fill the full lines
                    if (line < fullLines)
                    {
                        // Number of items on this line
                        int numLines = lineArray.Count;
                                
                        // Find ending position of last entry
                        MultiRect itemEntry = (MultiRect)lineArray[numLines - 1];

                        // Is there spare room between last entry and end of line?                            
                        if (itemEntry.Rect.Right < (xEndPos - 1))
                        {
                            // Work out how much extra to give each one
                            int extra = (int)((xEndPos - itemEntry.Rect.Right - 1) / numLines);
                                        
                            // Keep track of how much items need moving across
                            int totalMove = 0;
                                        
                            // Add into each entry
                            for(int i=0; i<numLines; i++)
                            {
                                // Get the entry class instance
                                MultiRect expandEntry = (MultiRect)lineArray[i];
                                        
                                // Move across requried amount
                                expandEntry.X += totalMove;
                                            
                                // Add extra width
                                expandEntry.Width += (int)extra;

                                // All items after this needing moving
                                totalMove += extra;
                            }
                                        
                            // Extend the last position, in case rounding errors means its short
                            itemEntry.Width += (xEndPos - itemEntry.Rect.Right - 1);
                        }
                    }
                                
                    line++;
                }

                if (_positionAtTop)
                {
                    // If the selected line is not the bottom line
                    if (selectedLine != (lineList.Count - 1))
                    {
                        ArrayList lastLine = (ArrayList)(lineList[lineList.Count - 1]);
                                    
                        // Find y offset of last line
                        int lastOffset = ((MultiRect)lastLine[0]).Rect.Y;
                                
                        // Move all lines below it up one
                        for(int lineIndex=selectedLine+1; lineIndex<lineList.Count; lineIndex++)
                        {
                            ArrayList al = (ArrayList)lineList[lineIndex];
                                    
                            for(int item=0; item<al.Count; item++)
                            {
                                MultiRect itemEntry = (MultiRect)al[item];
                                itemEntry.Y -= lineIncrement;
                            }
                        }
                                    
                        // Move selected line to the bottom
                        ArrayList sl = (ArrayList)lineList[selectedLine];
                                    
                        for(int item=0; item<sl.Count; item++)
                        {
                            MultiRect itemEntry = (MultiRect)sl[item];
                            itemEntry.Y = lastOffset;
                        }
                    }
                }
                else
                {
                    // If the selected line is not the top line
                    if (selectedLine != 0)
                    {
                        ArrayList topLine = (ArrayList)(lineList[0]);
                                    
                        // Find y offset of top line
                        int topOffset = ((MultiRect)topLine[0]).Rect.Y;
                                
                        // Move all lines above it down one
                        for(int lineIndex=0; lineIndex<selectedLine; lineIndex++)
                        {
                            ArrayList al = (ArrayList)lineList[lineIndex];
                                    
                            for(int item=0; item<al.Count; item++)
                            {
                                MultiRect itemEntry = (MultiRect)al[item];
                                itemEntry.Y += lineIncrement;
                            }
                        }
                                    
                        // Move selected line to the top
                        ArrayList sl = (ArrayList)lineList[selectedLine];
                                    
                        for(int item=0; item<sl.Count; item++)
                        {
                            MultiRect itemEntry = (MultiRect)sl[item];
                            itemEntry.Y = topOffset;
                        }
                    }
                }

                // Now assignt each lines rectangle to the corresponding structure
                foreach(ArrayList al in lineList)
                {
                    foreach(MultiRect multiEntry in al)
                    {
                        Rectangle newRect = multiEntry.Rect;
                                    
                        // Make the vertical adjustment
                        newRect.Y += yAdjust;
                                    
                        _tabRects[multiEntry.Index] = newRect;
                    }
                }
            }
        }

		/// <summary>
		/// Recalculate tab headers when using single line display.
		/// </summary>
		/// <param name="xWidth">Available width of tab header area.</param>
		/// <param name="xStartPos">Left starting postion.</param>
		/// <param name="tabPosition">Rectangle for tab position.</param>
        protected virtual void RecalculateSinglelineTabs(int xWidth, 
														 int xStartPos, 
														 Rectangle tabPosition)
        {
            using (Graphics g = this.CreateGraphics())
            {
                bool finished;
                int xStartPosCached = xStartPos;

                do
                {
                    // Only need to loop once by default
                    finished = true;

                    // Bring back the original starting position
                    xStartPos = xStartPosCached;

                    // Remember which lines are then first and last displayed
                    _topYPos = tabPosition.Y;
                    _bottomYPos = _topYPos;
                
                    // Set the minimum size for each tab page
                    for(int i=0; i<_tabPages.Count; i++)
                    {
                        // Is this page before those displayed?
                        if (i < _startPage)
                            _tabRects[i] = (object)_nullPosition;  // Yes, position off screen
                        else
                            _tabRects[i]= (object)tabPosition;	 // No, create minimum size
                    }

                    // Subtract the minimum tab sizes already allocated
                    xWidth -= _tabPages.Count * (tabPosition.Width + 1);

                    // Is there any more space left to allocate
                    if (xWidth > 0)
                    {
                        ArrayList listNew = new ArrayList();
                        ArrayList listOld = new ArrayList();

                        // Add all pages to those that need space allocating
                        for(int i=_startPage; i<_tabPages.Count; i++)
                            listNew.Add(_tabPages[i]);
                		
                        // Each tab can have an allowance
                        int xAllowance;
                		
                        do 
                        {
                            // The list generated in the last iteration becomes 
                            // the to be processed in this iteration
                            listOld = listNew;
                	
                            // List of pages that still need more space allocating
                            listNew = new ArrayList();

                            if (_shrinkPagesToFit)
                            {
                                // Each page is allowed a maximum allowance of space
                                // during this iteration. 
                                xAllowance = xWidth / _tabPages.Count;
                            }
                            else
                            {
                                // Allow each page as much space as it wants
                                xAllowance = 999;
                            }

                            // Assign space to each page that is requesting space
                            foreach(TabPage page in listOld)
                            {
                                int index = _tabPages.IndexOf(page);

                                Rectangle rectPos = (Rectangle)_tabRects[index];

                                // Find out how much extra space this page is requesting
                                int xSpace = GetTabPageSpace(g, page) - rectPos.Width;

                                // Does it want more space than its currently allowed to have?
                                if (xSpace > xAllowance)
                                {
                                    // Restrict allowed space
                                    xSpace = xAllowance;

                                    // Add page to ensure it gets processed next time around
                                    listNew.Add(page);
                                }

                                // Give space to tab
                                rectPos.Width += xSpace;

                                _tabRects[index] = (object)rectPos;

                                // Reduce extra left for remaining tabs
                                xWidth -= xSpace;
                            }
                        } while ((listNew.Count > 0) && (xAllowance > 0) && (xWidth > 0));
                    }

                    // Assign the final positions to each tab now we known their sizes
                    for(int i=_startPage; i<_tabPages.Count; i++)
                    {
                        Rectangle rectPos = (Rectangle)_tabRects[i];

                        // Define position of tab page
                        rectPos.X = xStartPos;

                        _tabRects[i] = (object)rectPos;

                        // Next button must be the width of this one across
                        xStartPos += rectPos.Width + 1;
                    }

                    // If there is spare space for another tab, and we are not showing from the first page onwards
                    if ((xWidth > tabPosition.Width) && (_startPage > 0))
                    {
                        // Find size of the previous page that is not showing
                        int previousWidth = GetTabPageSpace(g, _tabPages[_startPage - 1]);

                        // Is there enough room for that previous page?
                        if (xWidth >= previousWidth)
                        {                            
                            // Start showing from the previous page
                            _startPage--;

                            // Go around again and allocate using new starting page
                            finished = false;
                        }
                    }

                } while (!finished);
            }
        }

		/// <summary>
		/// Recalculate the position of tab header buttons.
		/// </summary>
        protected virtual void RecalculateButtons()
        {
            // Should the active arrow be enabled?
            bool enableActiveArrow = (_tabPages.Count > 0);
            
            // Is the active arrow in the wrong enabled state?
            if (_activeArrow.Enabled != enableActiveArrow)
            {
                // Update to new state
                _activeArrow.Enabled = enableActiveArrow;

                // Update the arrow image
                _activeArrow.Image = _internalImages.Images[enableActiveArrow ? (int)ImageStrip.ActiveEnabled : 
                                                                                (int)ImageStrip.ActiveDisabled];
            }

            int buttonTopGap = 0;
            
            if (_multiline)
            {
                // The height of a tab row is
                int tabButtonHeight = _position[_styleIndex, (int)PositionIndex.ImageGapTop] + 
                                      _imageGapTopExtra +
                                      _imageHeight + 
                                      _imageGapBottomExtra + 
                                      _position[_styleIndex, (int)PositionIndex.ImageGapBottom] +
                                      _position[_styleIndex, (int)PositionIndex.BorderBottom]; 
        
                // The height of the tabs area is...
                int tabsAreaHeight = _position[_styleIndex, (int)PositionIndex.BorderTop] + 
                                      tabButtonHeight + _position[_styleIndex, (int)PositionIndex.TabsBottomGap];
                
                // Find offset to place button halfway down the tabs area rectangle
                buttonTopGap = _position[_styleIndex, (int)PositionIndex.ButtonOffset]	+ 
                               (tabsAreaHeight - BUTTON_HEIGHT) / 2;

                // Invert gap position when at bottom
                if (!_positionAtTop)
                    buttonTopGap = _tabsAreaRect.Height - buttonTopGap - BUTTON_HEIGHT;
            }
            else
            {
                // Find offset to place button halfway down the tabs area rectangle
                buttonTopGap = _position[_styleIndex, (int)PositionIndex.ButtonOffset]	+ 
                                (_tabsAreaRect.Height - BUTTON_HEIGHT) / 2;
            }
        
            // Position to place next button
            int xStart = _tabsAreaRect.Right - BUTTON_WIDTH - BUTTON_GAP;

            // Close button should be shown?
            if (_showClose)
            {
                // Define the location
                _closeButton.Location = new Point(xStart, _tabsAreaRect.Top + buttonTopGap);

				if (xStart < 1)
					_closeButton.Hide();
				else
				{
					if ((_closeButton.Bottom >= this.Height) || (_closeButton.Top <= 1))
						_closeButton.Hide();
					else
						_closeButton.Show();
				}

                xStart -= BUTTON_WIDTH;
            }
            else
                _closeButton.Hide();

            // Arrows should be shown?
            if (_showArrows)
            {
                // Position the right arrow first as its more the right hand side
                _rightArrow.Location = new Point(xStart, _tabsAreaRect.Top + buttonTopGap);

				if (xStart < 1)
					_rightArrow.Hide();
				else
				{
					if ((_rightArrow.Bottom >= this.Height) || (_rightArrow.Top <= 1))
						_rightArrow.Hide();
					else
						_rightArrow.Show();
				}
                    
                xStart -= BUTTON_WIDTH;

                _leftArrow.Location = new Point(xStart, _tabsAreaRect.Top + buttonTopGap);

				if (xStart < 1)
					_leftArrow.Hide();
				else
				{
					if ((_leftArrow.Bottom >= this.Height) || (_leftArrow.Top <= 1))
						_leftArrow.Hide();
					else
						_leftArrow.Show();
				}
                    
                xStart -= BUTTON_WIDTH;

				// Define then enabled state of buttons
                if (_leftArrow.Enabled != _leftScroll)
				{
	                _leftArrow.Enabled = _leftScroll;

					if (_leftScroll)
						_leftArrow.Image = _internalImages.Images[(int)ImageStrip.LeftEnabled];
					else
						_leftArrow.Image = _internalImages.Images[(int)ImageStrip.LeftDisabled];
				}

                if (_rightArrow.Enabled != _rightScroll)
				{
					_rightArrow.Enabled = _rightScroll;

					if (_rightScroll)
						_rightArrow.Image = _internalImages.Images[(int)ImageStrip.RightEnabled];
					else
						_rightArrow.Image = _internalImages.Images[(int)ImageStrip.RightDisabled];
				}
            }
            else
            {
                _leftArrow.Hide();
                _rightArrow.Hide();
				_activeArrow.Hide();
            }

			// Should drop select be shown?
			if (_showDropSelect)
			{
                _activeArrow.Location = new Point(xStart, _tabsAreaRect.Top + buttonTopGap);

				if (xStart < 1)
					_activeArrow.Hide();
				else
				{
					if ((_activeArrow.Bottom >= this.Height) || (_activeArrow.Top <= 1))
						_activeArrow.Hide();
					else
						_activeArrow.Show();
				}

				if (_activeArrow.Enabled != (_tabPages.Count > 0))
				{
					_activeArrow.Enabled = (_tabPages.Count > 0);

					if (_activeArrow.Enabled)
						_activeArrow.Image = _internalImages.Images[(int)ImageStrip.ActiveEnabled];
					else
						_activeArrow.Image = _internalImages.Images[(int)ImageStrip.ActiveDisabled];
				}

                xStart -= BUTTON_WIDTH;
			}
			else
				_activeArrow.Hide();

            if (IsOfficeStyle || IsMediaPlayerStyle || (_style == VisualStyle.IDE2005))
			{
				Color backColor = this.BackColor;

				if (_defaultColor)
				{
                    if (IsOfficeStyle)
						backColor = OfficeBackColor;
                    else if (IsMediaPlayerStyle)
                        backColor = MediaPlayerBackColor;
                    else
					{
						switch(IDE2005Style)
						{
							case IDE2005Style.Standard:
							case IDE2005Style.Enhanced:
								backColor = Color.Transparent;
								break;
							case IDE2005Style.StandardDark:
							case IDE2005Style.EnhancedDark:
								backColor = IDE2005BackLeftColor;
								break;

						}
					}
				}
				
				_activeArrow.BackColor = _closeButton.BackColor = _leftArrow.BackColor = _rightArrow.BackColor = backColor;
			}
			else
			{
				if ((_appearance == VisualAppearance.MultiBox) || (_style == VisualStyle.Plain))
					_activeArrow.BackColor = _closeButton.BackColor = _leftArrow.BackColor = _rightArrow.BackColor = Color.Transparent;
				else
					_activeArrow.BackColor = _closeButton.BackColor = _leftArrow.BackColor = _rightArrow.BackColor = _backIDE;
			}
        }

		/// <summary>
		/// Calculate the maximum position allowed for drawing tab headers.
		/// </summary>
		/// <returns>Maximum drawing offset for tab headers.</returns>
        protected virtual int GetMaximumDrawPos()
        {
            int xEndPos = _tabsAreaRect.Right - TABS_AREA_END_INSET;

            // Showing the close button reduces available space
            if (_showClose)
                xEndPos -= BUTTON_WIDTH + BUTTON_GAP;

            // If showing arrows then reduce space for both
            if (_showArrows)
                xEndPos -= BUTTON_WIDTH * 2;
				
            // If showing drop arrow then reduce space
            if (_showDropSelect)
                xEndPos -= BUTTON_WIDTH;

			return xEndPos;
        }

		/// <summary>
		/// Calculate the width of a tab header for given tab page instance.
		/// </summary>
		/// <param name="g">Graphics object needed for measuring strings.</param>
		/// <param name="page">TabPage to calculate width of header.</param>
		/// <returns>Pixel width of tab header.</returns>
        protected virtual int GetTabPageSpace(Graphics g, TabPage page)
        {
			int width = 0;

            // Any icon or image provided?
            if ((page.Icon != null) || (page.Image != null) || (((_imageList != null) || (page.ImageList != null)) && (page.ImageIndex != -1)))
            {
                width += _position[_styleIndex, (int)PositionIndex.ImageGapLeft] +
                         _imageWidth + 
                         _position[_styleIndex, (int)PositionIndex.ImageGapRight];
            }

            // Any text provided?
            if ((page.Title != null) && (page.Title.Length > 0))
            {
                if (!_selectedTextOnly || (_selectedTextOnly && (_pageSelected == _tabPages.IndexOf(page))))
                {
                    Font drawFont = base.Font;
					bool disposeFont = false;

                    if (_boldSelected && page.Selected)
					{
                        drawFont = new Font(drawFont, FontStyle.Bold);
						disposeFont = true;
					}

					try
					{
						SizeF dimension;

                        // Find width of the requested text
                        if ((_style != VisualStyle.IDE2005) &&
                            (_style != VisualStyle.Office2003) &&
                            (_style != VisualStyle.Plain))
                        {
                            if ((IsOfficeStyle && Apply2007ClearType) ||
                                (IsMediaPlayerStyle && ApplyMediaPlayerClearType))
                            {
                                using (UseClearTypeGridFit clearType = new UseClearTypeGridFit(g))
                                    dimension = g.MeasureString(page.Title, drawFont);
                            }
                            else
                                dimension = g.MeasureString(page.Title, drawFont);
                        }
                        else
                            dimension = g.MeasureString(page.Title, drawFont);

						// Convert to integral
						width += _position[_styleIndex, (int)PositionIndex.TextGapLeft] +
														(int)dimension.Width + 1; 
					}
					catch
					{
						RedefineSystemFont();
						_recalculate = true;
						Invalidate();
					}
					
					if (disposeFont)
						drawFont.Dispose();
                }						 
            }
            
            // Is there a maximum width to apply
            if (_maximumHeaderWidth > 0)
			{
				// Limit to the designated maximum
				if (width > _maximumHeaderWidth)
					width = _maximumHeaderWidth;
			}

            // Add the fixed elements of required space
            width += _position[_styleIndex, (int)PositionIndex.BorderLeft] + 
                     _position[_styleIndex, (int)PositionIndex.BorderRight];

            return width;
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
			// Does the state need recalculating before paint can occur?
			if (_recalculate)
				Recalculate();

			// Draw style specific around outer elements            
			DrawOuterBorders(e.Graphics);

			// Draw style specific around inner elements            
			DrawInnerBorders(e.Graphics);

			// Clip the drawing to prevent drawing in unwanted areas
			ClipDrawingTabs(e.Graphics);

			// Paint each tab page
			foreach(TabPage page in _tabPages)
				DrawTab(page, e.Graphics, (_tabPages.IndexOf(page) == _hotTrackPage));

            // Let events be generated so users can draw over the control
            base.OnPaint(e);
        }

		/// <summary>
		/// Draw the borders around the whole control.
		/// </summary>
		/// <param name="g">Reference to drawing graphics instance.</param>
		protected virtual void DrawOuterBorders(Graphics g)
		{
			Color pageAreaColor = this.BackColor;
			Color tabsAreaColor = this.BackColor;

            if (_defaultColor)
            {
                if (_style == VisualStyle.IDE2005)
                {
                    tabsAreaColor = IDE2005BackColor;
                    pageAreaColor = OfficePageColor;
                }
                else if (IsOfficeStyle)
                {
                    tabsAreaColor = OfficeBackColor;
                    pageAreaColor = OfficePageColor;
                }
                else if (IsMediaPlayerStyle)
                {
                    tabsAreaColor = MediaPlayerBackColor;
                    pageAreaColor = MediaPlayerPageColor;
                }
            }

			// Fill backgrounds of the page and tabs areas
			using (SolidBrush pageAreaBrush = new SolidBrush(pageAreaColor))
				g.FillRectangle(pageAreaBrush, _pageAreaRect);

			if ((_style == VisualStyle.Plain) || (_appearance == VisualAppearance.MultiBox))
			{
				using (SolidBrush pageAreaBrush = new SolidBrush(tabsAreaColor))
					g.FillRectangle(pageAreaBrush, _tabsAreaRect);
			}
            else if (IsOfficeStyle || IsMediaPlayerStyle || (_style == VisualStyle.IDE2005))
			{
				using(SolidBrush tabsAreaBrush = new SolidBrush(tabsAreaColor))
					g.FillRectangle(tabsAreaBrush, _tabsAreaRect);
			}
			else
			{				
				using(SolidBrush tabsAreaBrush = new SolidBrush(_backIDE))
					g.FillRectangle(tabsAreaBrush, _tabsAreaRect);
			}
		}
		
		/// <summary>
		/// Gets the active color for Office2003 style.
		/// </summary>
		protected virtual Color Office2003ActiveColor
		{
			get 
			{ 
				if (_defaultColor)
				{
					switch(_officeStyle)
					{
						case OfficeStyle.Light:
                            if (_style == VisualStyle.Office2003)
                                return _colorDetails.BaseColor1;
                            else
                                return Office2007ColorTable.LightBackground(_style);
						case OfficeStyle.Dark:
                            if (_style == VisualStyle.Office2003)
                                return _colorDetails.BaseColor2;
                            else
                                return Office2007ColorTable.DarkBackground(_style);
						case OfficeStyle.SoftEnhanced:
						case OfficeStyle.LightEnhanced:
						case OfficeStyle.DarkEnhanced:
                            if (_style == VisualStyle.Office2003)
                                return _colorDetails.TrackLightColor2;
                            else
                                return Office2007ColorTable.EnhancedBackground(_style);
						case OfficeStyle.SoftWhite:
						case OfficeStyle.LightWhite:
						case OfficeStyle.DarkWhite:
						default:
							return Color.White;
					}
				}
				else
					return ControlPaint.LightLight(base.BackColor);
			}
		}

        /// <summary>
        /// Gets the active color for MediaPlayer style.
        /// </summary>
        protected virtual Color MediaPlayerActiveColor
        {
            get
            {
                if (_defaultColor)
                {
                    switch (_mediaPlayerStyle)
                    {
                        case MediaPlayerStyle.Light:
                            return MediaPlayerColorTable.LightBackground(_style);
                        case MediaPlayerStyle.Dark:
                            return MediaPlayerColorTable.DarkBackground(_style);
                        case MediaPlayerStyle.SoftEnhanced:
                        case MediaPlayerStyle.LightEnhanced:
                        case MediaPlayerStyle.DarkEnhanced:
                            return MediaPlayerColorTable.EnhancedBackground(_style);
                        case MediaPlayerStyle.SoftWhite:
                        case MediaPlayerStyle.LightWhite:
                        case MediaPlayerStyle.DarkWhite:
                        default:
                            return Color.White;
                    }
                }
                else
                    return ControlPaint.LightLight(base.BackColor);
            }
        }
        
        /// <summary>
		/// Gets the active color for IDE2005 style.
		/// </summary>
		protected virtual Color IDE2005ActiveColor
		{
			get 
			{ 
				switch(_ide2005Style)
				{
					case IDE2005Style.Standard:
					case IDE2005Style.StandardDark:
						if (_defaultColor)
							return ControlPaint.LightLight(base.BackColor);
						else
							return SystemColors.Window;
					case IDE2005Style.Enhanced:
					case IDE2005Style.EnhancedDark:
						return _colorDetails.TrackLightColor2; 
					default:
						return Color.White;
				}
			}
		}

		/// <summary>
		/// Gets the active color for IDE2005 style.
		/// </summary>
		protected virtual Color IDE2005BackLeftColor
		{
			get 
			{ 
				switch(_ide2005Style)
				{
					case IDE2005Style.Standard:
					case IDE2005Style.Enhanced:
						return _colorDetails.BaseColor1;
					case IDE2005Style.StandardDark:
					case IDE2005Style.EnhancedDark:
						return _colorDetails.BaseDarkerColor;
					default:
						return Color.White;
				}
			}
		}

		/// <summary>
		/// Gets the active color for IDE2005 style.
		/// </summary>
		protected virtual Color IDE2005BackRightColor
		{
			get 
			{ 
				switch(_ide2005Style)
				{
					case IDE2005Style.Standard:
					case IDE2005Style.Enhanced:
						return _colorDetails.BaseColor2;
					case IDE2005Style.StandardDark:
					case IDE2005Style.EnhancedDark:
						return _colorDetails.BaseDarkerColor;
					default:
						return Color.White;
				}
			}
		}

		/// <summary>
		/// Gets the active color for Office2003/Office2007 style.
		/// </summary>
		protected virtual Color OfficePageColor
		{
			get 
			{ 
				switch(_officeStyle)
				{
					case OfficeStyle.Light:
                        if ((_style == VisualStyle.Office2003) || (_style == VisualStyle.IDE2005))
                            return _colorDetails.BaseColor1; 
                        else
                            return Office2007ColorTable.LightBackground(_style);
                    case OfficeStyle.Dark:
                        if ((_style == VisualStyle.Office2003) || (_style == VisualStyle.IDE2005))
                            return _colorDetails.BaseColor2; 
                        else
                            return Office2007ColorTable.DarkBackground(_style);
					case OfficeStyle.SoftEnhanced:
					case OfficeStyle.LightEnhanced:
					case OfficeStyle.DarkEnhanced:
                        if ((_style == VisualStyle.Office2003) || (_style == VisualStyle.IDE2005))
                            return _colorDetails.TrackLightColor2; 
                        else
                            return Office2007ColorTable.EnhancedBackground(_style);
                    case OfficeStyle.SoftWhite:
					case OfficeStyle.LightWhite:
					case OfficeStyle.DarkWhite:
					default:
						return Color.White;
				}
			}
		}

        /// <summary>
        /// Gets the active color for Media Player style.
        /// </summary>
        protected virtual Color MediaPlayerPageColor
        {
            get
            {
                switch (_mediaPlayerStyle)
                {
                    case MediaPlayerStyle.Light:
                        return MediaPlayerColorTable.LightBackground(_style);
                    case MediaPlayerStyle.Dark:
                        return MediaPlayerColorTable.DarkBackground(_style);
                    case MediaPlayerStyle.SoftEnhanced:
                    case MediaPlayerStyle.LightEnhanced:
                    case MediaPlayerStyle.DarkEnhanced:
                        return MediaPlayerColorTable.EnhancedBackground(_style);
                    case MediaPlayerStyle.SoftWhite:
                    case MediaPlayerStyle.LightWhite:
                    case MediaPlayerStyle.DarkWhite:
                    default:
                        return Color.White;
                }
            }
        }
        
        /// <summary>
		/// Gets the back color for Office2003/Office2007 style.
		/// </summary>
		protected virtual Color OfficeBackColor
		{
			get 
			{ 
				if (_defaultColor)
				{
                    switch (_officeStyle)
                    {
                        case OfficeStyle.Light:
                        case OfficeStyle.DarkWhite:
                        case OfficeStyle.DarkEnhanced:
                            if (_style == VisualStyle.Office2003)
                                return _colorDetails.BaseColor2;
                            else
                                return Office2007ColorTable.DarkBackground(_style);
                        case OfficeStyle.SoftWhite:
                        case OfficeStyle.SoftEnhanced:
                            if (_style == VisualStyle.Office2003)
                                return _colorDetails.OpenBaseColor1;
                            else
                                return Office2007ColorTable.SoftBackground(_style);
                        case OfficeStyle.Dark:
                        case OfficeStyle.LightWhite:
                        case OfficeStyle.LightEnhanced:
                        default:
                            if (_style == VisualStyle.Office2003)
                                return _colorDetails.BaseColor1;
                            else
                                return Office2007ColorTable.LightBackground(_style);
                    }
				}
				else
					return base.BackColor;
			}
		}

        /// <summary>
        /// Gets the back color for Media Player style.
        /// </summary>
        protected virtual Color MediaPlayerBackColor
        {
            get
            {
                if (_defaultColor)
                {
                    switch (_mediaPlayerStyle)
                    {
                        case MediaPlayerStyle.Light:
                        case MediaPlayerStyle.DarkWhite:
                        case MediaPlayerStyle.DarkEnhanced:
                            return MediaPlayerColorTable.DarkBackground(_style);
                        case MediaPlayerStyle.SoftWhite:
                        case MediaPlayerStyle.SoftEnhanced:
                            return MediaPlayerColorTable.SoftBackground(_style);
                        case MediaPlayerStyle.Dark:
                        case MediaPlayerStyle.LightWhite:
                        case MediaPlayerStyle.LightEnhanced:
                        default:
                            return MediaPlayerColorTable.LightBackground(_style);
                    }
                }
                else
                    return base.BackColor;
            }
        }

        /// <summary>
        /// Gets the back color for IDE2005 style.
        /// </summary>
        protected virtual Color IDE2005BackColor
        {
            get
            {
                if (_defaultColor)
                {
                    switch (_ide2005Style)
                    {
                        default:
                        case IDE2005Style.Standard:
                        case IDE2005Style.Enhanced:
                            return _colorDetails.BaseColor;
                        case IDE2005Style.StandardDark:
                        case IDE2005Style.EnhancedDark:
                            return _colorDetails.BaseDarkerColor;
                    }
                }
                else
                    return base.BackColor;
            }
        }
		        
        /// <summary>
        /// Draw the borders needed around the inner elements.
        /// </summary>
        /// <param name="g">Reference to drawing graphics instance.</param>
        protected virtual void DrawInnerBorders(Graphics g)
        {
            // MultiBox appearance does not have any borders
            if (_appearance != VisualAppearance.MultiBox)
            {
                bool hiddenPages = HideTabsCalculation();

                // Draw the borders
                switch(_style)
                {
                    case VisualStyle.Plain:
                        // Height for drawing the border is size of the page area extended 
                        // down to draw the bottom border inside the tabs area
                        int pageHeight = _pageAreaRect.Height + PLAIN_BORDER_DOUBLE;
	
                        int xDraw = _pageAreaRect.Top;

                        // Should the tabs area be hidden?
                        if (hiddenPages)
                        {
                            // Then need to readjust pageHeight
                            pageHeight -= PLAIN_BORDER_DOUBLE;
                        }
                        else
                        {
                            // If drawing at top then overdraw upwards and not down
                            if (_positionAtTop)
                                xDraw -= PLAIN_BORDER_DOUBLE;
                        }

                        if (_insetBorderPagesOnly)
                        {
                            if (!hiddenPages)
                            {
                                // Draw the outer border around the page area			
                                DrawHelper.DrawPlainRaisedBorderTopOrBottom(g, new Rectangle(0, xDraw, this.Width, pageHeight),
                                                                            _backLightLight, base.BackColor, _backDark, _backDarkDark, _positionAtTop);
                            }
                        }
                        else
                        {
                            // Draw the outer border around the page area			
                            DrawHelper.DrawPlainRaisedBorder(g, new Rectangle(_pageAreaRect.Left, xDraw, _pageAreaRect.Width, pageHeight),
                                                             _backLightLight, base.BackColor, _backDark, _backDarkDark);
                        }

                        // Do we have any tabs?
                        if ((_tabPages.Count > 0) && _insetPlain)
                        {
                            // Draw the inner border around the page area
                            Rectangle inner = new Rectangle(_pageAreaRect.Left + PLAIN_BORDER,
                                                            xDraw + PLAIN_BORDER,
                                                            _pageAreaRect.Width - PLAIN_BORDER_DOUBLE,
                                                            pageHeight - PLAIN_BORDER_DOUBLE);

                            if (_insetBorderPagesOnly)
                            {
                                if (!hiddenPages)
                                {
                                    DrawHelper.DrawPlainSunkenBorderTopOrBottom(g, new Rectangle(0, inner.Top, this.Width, inner.Height),
                                                                                _backLightLight, base.BackColor, _backDark, _backDarkDark, _positionAtTop);
                                }                                                                       
                            }
                            else
                            {
                                DrawHelper.DrawPlainSunkenBorder(g, new Rectangle(inner.Left, inner.Top, inner.Width, inner.Height),
                                                                 _backLightLight, base.BackColor, _backDark, _backDarkDark);
                            }

                        }						
                        break;
                    case VisualStyle.Office2003:
                    case VisualStyle.Office2007Blue:
                    case VisualStyle.Office2007Silver:
                    case VisualStyle.Office2007Black:
                        {
                            Color backDarkDark = _backDarkDark;
                            Color back = base.BackColor;
                            Color backCaptionColor2 = base.BackColor;

                            if (_defaultColor)
                            {
                                back = OfficeBackColor;

                                if (_style == VisualStyle.Office2003)
                                {
                                    backDarkDark = _colorDetails.ActiveBorderColor;
                                    backCaptionColor2 = _colorDetails.CaptionColor2;
                                }
                                else
                                {
                                    backDarkDark = Office2007ColorTable.BorderColor(_style);
                                    backCaptionColor2 = backDarkDark;
                                }
                            }

                            // Draw the top and bottom borders to the tabs area
                            using (Pen darkdark = new Pen(backDarkDark))
                            {
                                int borderGap = _position[_styleIndex, (int)PositionIndex.BorderTop];

                                if (_positionAtTop)
                                {
                                    // Fill the border between the tabs and the embedded controls
                                    using (SolidBrush backBrush = new SolidBrush(back))
                                        g.FillRectangle(backBrush, 0, _tabsAreaRect.Bottom - borderGap, _tabsAreaRect.Width, borderGap);

                                    int indent = 0;

                                    // Is a single pixel border required around whole area?                            
                                    if (_officePixelBorder)
                                    {
                                        int indentLine = _tabsAreaRect.Bottom - borderGap;

                                        // If we need a border around the whole control
                                        if (_officeHeaderBorder || hiddenPages)
                                            indentLine = 0;

                                        g.DrawLine(darkdark, 0, this.Height - 1, 0, indentLine);
                                        g.DrawLine(darkdark, 0, indentLine, this.Width - 1, indentLine);
                                        g.DrawLine(darkdark, this.Width - 1, indentLine, this.Width - 1, this.Height - 1);
                                        g.DrawLine(darkdark, this.Width - 1, this.Height - 1, 0, this.Height - 1);
                                        indent++;
                                    }

                                    // Draw bottom border
                                    if (!hiddenPages)
                                        g.DrawLine(darkdark, indent,
                                                             _tabsAreaRect.Bottom - borderGap,
                                                             _tabsAreaRect.Width - (indent * 2),
                                                             _tabsAreaRect.Bottom - borderGap);
                                }
                                else
                                {
                                    // Fill the border between the tabs and the embedded controls
                                    using (SolidBrush backBrush = new SolidBrush(back))
                                        g.FillRectangle(backBrush, 0, _tabsAreaRect.Top, _tabsAreaRect.Width, borderGap);

                                    int indent = 0;

                                    // Is a single pixel border required around whole area?                            
                                    if (_officePixelBorder)
                                    {
                                        int indentLine = _tabsAreaRect.Top;

                                        // If we need a border around the whole control
                                        if (_officeHeaderBorder || hiddenPages)
                                            indentLine = this.Height - 1;

                                        g.DrawLine(darkdark, 0, indentLine, 0, 0);
                                        g.DrawLine(darkdark, 0, 0, this.Width - 1, 0);
                                        g.DrawLine(darkdark, this.Width - 1, 0, this.Width - 1, indentLine);
                                        g.DrawLine(darkdark, this.Width - 1, indentLine, 0, indentLine);
                                    }
                                    else
                                    {
                                        // If drawing the special fading lines
                                        if (_officeDockSides)
                                        {
                                            bool vertical = false;

                                            if (AllowSideCaptions &&
                                                (Parent != null) &&
                                                (Parent.Parent != null) &&
                                                ((Parent.Parent.Dock == DockStyle.Top) ||
                                                 (Parent.Parent.Dock == DockStyle.Bottom)))
                                                vertical = true;

                                            // Draw the left and right fading lines
                                            int indentLine = hiddenPages ? this.Height - 1 : _tabsAreaRect.Top;

                                            using (LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(-1, -1, Width + 1, Height + 1), backCaptionColor2, backDarkDark, 90f))
                                            {
                                                if (vertical)
                                                    g.FillRectangle(brush, 0, 0, this.Width - 1, 1);
                                                else
                                                    g.FillRectangle(brush, 0, 0, 1, indentLine + 1);

                                                g.FillRectangle(brush, Width - 1, 0, 1, indentLine + 1);
                                            }
                                        }
                                    }

                                    // Draw bottom border
                                    if (!hiddenPages)
                                    {
                                        g.DrawLine(darkdark, indent,
                                                             _tabsAreaRect.Top,
                                                             _tabsAreaRect.Width - (indent * 2),
                                                             _tabsAreaRect.Top);
                                    }
                                    else
                                    {
                                        // If drawing the special fading lines
                                        if (_officeDockSides)
                                        {
                                            // Draw the bottom border always
                                            using (LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(-1, -1, Width + 1, Height + 1), backCaptionColor2, backDarkDark, 90f))
                                                g.FillRectangle(brush, 0, this.Height - 1, this.Width, 1);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case VisualStyle.MediaPlayerBlue:
                    case VisualStyle.MediaPlayerOrange:
                    case VisualStyle.MediaPlayerPurple:
                        {
                            Color backDarkDark = _backDarkDark;
                            Color back = base.BackColor;
                            Color backCaptionColor2 = base.BackColor;

                            if (_defaultColor)
                            {
                                back = MediaPlayerBackColor;
                                backDarkDark = MediaPlayerColorTable.BorderColor(_style);
                                backCaptionColor2 = backDarkDark;
                            }

                            // Draw the top and bottom borders to the tabs area
                            using (Pen darkdark = new Pen(backDarkDark))
                            {
                                int borderGap = _position[_styleIndex, (int)PositionIndex.BorderTop];

                                if (_positionAtTop)
                                {
                                    // Fill the border between the tabs and the embedded controls
                                    using (SolidBrush backBrush = new SolidBrush(back))
                                        g.FillRectangle(backBrush, 0, _tabsAreaRect.Bottom - borderGap, _tabsAreaRect.Width, borderGap);

                                    int indent = 0;

                                    // Is a single pixel border required around whole area?                            
                                    if (_mediaPlayerPixelBorder)
                                    {
                                        int indentLine = _tabsAreaRect.Bottom - borderGap;

                                        // If we need a border around the whole control
                                        if (_mediaPlayerHeaderBorder || hiddenPages)
                                            indentLine = 0;

                                        g.DrawLine(darkdark, 0, this.Height - 1, 0, indentLine);
                                        g.DrawLine(darkdark, 0, indentLine, this.Width - 1, indentLine);
                                        g.DrawLine(darkdark, this.Width - 1, indentLine, this.Width - 1, this.Height - 1);
                                        g.DrawLine(darkdark, this.Width - 1, this.Height - 1, 0, this.Height - 1);
                                        indent++;
                                    }

                                    // Draw bottom border
                                    if (!hiddenPages)
                                        g.DrawLine(darkdark, indent,
                                                             _tabsAreaRect.Bottom - borderGap,
                                                             _tabsAreaRect.Width - (indent * 2),
                                                             _tabsAreaRect.Bottom - borderGap);
                                }
                                else
                                {
                                    // Fill the border between the tabs and the embedded controls
                                    using (SolidBrush backBrush = new SolidBrush(back))
                                        g.FillRectangle(backBrush, 0, _tabsAreaRect.Top, _tabsAreaRect.Width, borderGap);

                                    int indent = 0;

                                    // Is a single pixel border required around whole area?                            
                                    if (_mediaPlayerPixelBorder)
                                    {
                                        int indentLine = _tabsAreaRect.Top;

                                        // If we need a border around the whole control
                                        if (_mediaPlayerHeaderBorder || hiddenPages)
                                            indentLine = this.Height - 1;

                                        g.DrawLine(darkdark, 0, indentLine, 0, 0);
                                        g.DrawLine(darkdark, 0, 0, this.Width - 1, 0);
                                        g.DrawLine(darkdark, this.Width - 1, 0, this.Width - 1, indentLine);
                                        g.DrawLine(darkdark, this.Width - 1, indentLine, 0, indentLine);
                                    }
                                    else
                                    {
                                        // If drawing the special fading lines
                                        if (_mediaPlayerDockSides)
                                        {
                                            bool vertical = false;

                                            if (AllowSideCaptions &&
                                                (Parent != null) &&
                                                (Parent.Parent != null) &&
                                                ((Parent.Parent.Dock == DockStyle.Top) ||
                                                 (Parent.Parent.Dock == DockStyle.Bottom)))
                                                vertical = true;

                                            // Draw the left and right fading lines
                                            int indentLine = hiddenPages ? this.Height - 1 : _tabsAreaRect.Top;

                                            using (LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(-1, -1, Width + 1, Height + 1), backCaptionColor2, backDarkDark, 90f))
                                            {
                                                if (vertical)
                                                    g.FillRectangle(brush, 0, 0, this.Width - 1, 1);
                                                else
                                                    g.FillRectangle(brush, 0, 0, 1, indentLine + 1);

                                                g.FillRectangle(brush, Width - 1, 0, 1, indentLine + 1);
                                            }
                                        }
                                    }

                                    // Draw bottom border
                                    if (!hiddenPages)
                                    {
                                        g.DrawLine(darkdark, indent,
                                                             _tabsAreaRect.Top,
                                                             _tabsAreaRect.Width - (indent * 2),
                                                             _tabsAreaRect.Top);
                                    }
                                    else
                                    {
                                        // If drawing the special fading lines
                                        if (_mediaPlayerDockSides)
                                        {
                                            // Draw the bottom border always
                                            using (LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(-1, -1, Width + 1, Height + 1), backCaptionColor2, backDarkDark, 90f))
                                                g.FillRectangle(brush, 0, this.Height - 1, this.Width, 1);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case VisualStyle.IDE2005:
						Color border = _backDarkDark;
					
						// IDE2005 needs to use theme specific colors
						if (_defaultColor)
						{
							// Define the appropriate colours
							border = SystemColors.ControlDark;
						}
					
						// Draw the top and bottom borders to the tabs area
                        using (Pen borderPen = new Pen(border))
                        {
                            int borderGap = _position[_styleIndex, (int)PositionIndex.BorderTop];

                            if (_positionAtTop)
                            {
                                int indent = 0;

                                // Is a single pixel border required around whole area?                            
                                if (_ide2005PixelBorder)
                                {
									int indentLine = _tabsAreaRect.Bottom - borderGap;
									
									// If we need a border around the whole control
									if (_ide2005HeaderBorder || hiddenPages)
										indentLine = 0;
										
                                    g.DrawLine(borderPen, 0, this.Height - 1, 0, indentLine);
									g.DrawLine(borderPen, 0, indentLine, this.Width - 1, indentLine);
									g.DrawLine(borderPen, this.Width - 1, indentLine, this.Width - 1, this.Height - 1);
									g.DrawLine(borderPen, this.Width - 1, this.Height - 1, 0, this.Height - 1);
									indent++;
                                }
                                
                                // Draw bottom border
                                if (!hiddenPages)
                                    g.DrawLine(borderPen, indent, 
                                                          _tabsAreaRect.Bottom - borderGap, 
                                                          _tabsAreaRect.Width - (indent * 2), 
                                                          _tabsAreaRect.Bottom - borderGap);
                            }
                            else
                            {
                                int indent = 0;

                                // Is a single pixel border required around whole area?                            
                                if (_ide2005PixelBorder)
                                {
									int indentLine = _tabsAreaRect.Top;

									// If we need a border around the whole control
									if (_ide2005HeaderBorder || hiddenPages)
										indentLine = this.Height - 1;

									g.DrawLine(borderPen, 0, indentLine, 0, 0);
									g.DrawLine(borderPen, 0, 0, this.Width - 1, 0);
									g.DrawLine(borderPen, this.Width - 1, 0, this.Width - 1, indentLine);
									g.DrawLine(borderPen, this.Width - 1, indentLine, 0, indentLine);
                                }
                                
                                // Draw bottom border
                                if (!hiddenPages)
                                {
                                    g.DrawLine(borderPen, indent, 
														  _tabsAreaRect.Top, 
														  _tabsAreaRect.Width - (indent * 2), 
														  _tabsAreaRect.Top);
								}
                            }
                        }
                        break;
                }
            }
        }

		/// <summary>
		/// Find the clipping rectangle needed for tab header area.
		/// </summary>
		/// <returns>Clipping rectangle.</returns>
        protected virtual Rectangle ClippingRectangle()
        {
            // Calculate how much to reduce width by for clipping rectangle
            int xReduce = _tabsAreaRect.Width - GetMaximumDrawPos();

            // Create clipping rect
            Rectangle clip = new Rectangle(_tabsAreaRect.Left, 
										   _tabsAreaRect.Top,
										   _tabsAreaRect.Width - xReduce,
										   _tabsAreaRect.Height);

			// Need to clip vertically when drawing all except the multibox style
			if (_appearance != VisualAppearance.MultiBox)
			{
				// Cannot draw over last line on the control
				if (clip.Bottom >= (Height -1))
					clip.Height = (Height - 1) - clip.Top;

				// Cannot draw over the top line of the control
				if (clip.Top <= 1)
				{
					// Amount to reduce is how far area extends above control
					int yReduce = -clip.Top + 1;

					// Remember to change top and height
					clip.Y += yReduce;
					clip.Height -= yReduce;
				}
			}

			return clip;
        }

		/// <summary>
		/// Set clipping region to matching clipping rectangle of tab headers area.
		/// </summary>
		/// <param name="g">Graphics object target for clipping.</param>
        protected virtual void ClipDrawingTabs(Graphics g)
        {
            Rectangle clipRect = ClippingRectangle();

            // Restrict drawing to this clipping rectangle
            using(Region region = new Region(clipRect))
				g.Clip = region;
        }

		/// <summary>
		/// Draw an individual tab header.
		/// </summary>
		/// <param name="page">TabPage to draw header for.</param>
		/// <param name="g">Graphics target for draw operations.</param>
		/// <param name="highlightText">Should text be highlighted.</param>
        protected virtual void DrawTab(TabPage page, Graphics g, bool highlightText)
        {
            Int32 pageIndex = _tabPages.IndexOf(page);
            if (pageIndex < 0 || pageIndex >= _tabRects.Count)
                pageIndex = _tabRects.Count - 1;

            Rectangle rectTab = (Rectangle)_tabRects[_tabPages.IndexOf(page)];

            DrawTabBorder(ref rectTab, page, g);

            int xDraw = rectTab.Left + _position[_styleIndex, (int)PositionIndex.BorderLeft];
            int xMax = rectTab.Right - _position[_styleIndex, (int)PositionIndex.BorderRight];

            DrawTabImage(rectTab, page, g, xMax, ref xDraw);            
            DrawTabText(rectTab, page, g, highlightText, xMax, xDraw);
        }

		/// <summary>
		/// Draw the image on an individual tab header.
		/// </summary>
		/// <param name="rectTab">Bounding rectangle of tab header.</param>
		/// <param name="page">TabPage to draw header for.</param>
		/// <param name="g">Graphics target for draw operations.</param>
		/// <param name="xMax">Maximum width allowed for drawing.</param>
		/// <param name="xDraw">Actual width used in drawing.</param>
        protected virtual void DrawTabImage(Rectangle rectTab, 
                                            TabPage page, 
                                            Graphics g, 
                                            int xMax, 
                                            ref int xDraw)
        {
            // Default to using the Icon from the page
            Icon drawIcon = page.Icon;
            Image drawImage = page.Image;
			bool disposeImage = false;
			
            // If there is no valid Icon and the page is requested an image list index...
            if ((drawImage == null) && (drawIcon == null) && (page.ImageIndex != -1))
            {
                try
                {
                    // Default to using an image from the TabPage
                    ImageList imageList = page.ImageList;

                    // If page does not have an ImageList...
                    if (imageList == null)
                        imageList = _imageList;   // ...then use the TabControl one

                    // Do we have an ImageList to select from?
                    if (imageList != null)
                    {
                        // Grab the requested image
                        drawImage = imageList.Images[page.ImageIndex];
						disposeImage = true;
                    }
                }
                catch(Exception)
                {
                    // User supplied ImageList/ImageIndex are invalid, use an error image instead
                    drawImage = _internalImages.Images[(int)ImageStrip.Error];
					disposeImage = true;
                }
            }

            // Draw any image required
            if ((drawImage != null) || (drawIcon != null))
            {
                // Enough room to draw any of the image?
                if ((xDraw + _position[_styleIndex, (int)PositionIndex.ImageGapLeft]) <= xMax)
                {
                    // Move past the left image gap
                    xDraw += _position[_styleIndex, (int)PositionIndex.ImageGapLeft];

                    // Find down position for drawing the image
                    int yDraw = rectTab.Top + 
                                _position[_styleIndex, (int)PositionIndex.ImageGapTop] + 
                                _imageGapTopExtra;

                    // If there is enough room for all of the image?
                    if ((xDraw + _imageWidth) <= xMax)
                    {
                        if (drawImage != null)
                            g.DrawImage(drawImage, new Rectangle(xDraw, yDraw, _imageWidth, _imageHeight));
                        else
                            g.DrawIcon(drawIcon, new Rectangle(xDraw, yDraw, _imageWidth, _imageHeight));

                        // Move past the image and the image gap to the right
                        xDraw += _imageWidth + _position[_styleIndex, (int)PositionIndex.ImageGapRight];
                    }
                    else
                    {
                        // Calculate how much room there is
                        int xSpace = xMax - xDraw;

                        // Any room at all?
                        if (xSpace > 0)
                        {
                            if (drawImage != null)
                            {
                                // Draw only part of the image
                                g.DrawImage(drawImage, 
                                            new Point[]{new Point(xDraw, yDraw), 
                                                        new Point(xDraw + xSpace, yDraw), 
                                                        new Point(xDraw, yDraw + _imageHeight)},
                                            new Rectangle(0, 0, xSpace, 
                                            _imageHeight),
                                            GraphicsUnit.Pixel);
                            }
                            
                            // All space has been used up, nothing left for text
                            xDraw = xMax;
                        }
                    }
                }
            }

			if (disposeImage)
				drawImage.Dispose();
        }

		/// <summary>
		/// Draw the text on an individual tab header.
		/// </summary>
		/// <param name="rectTab">Bounding rectangle of tab header.</param>
		/// <param name="page">TabPage to draw header for.</param>
		/// <param name="g">Graphics target for draw operations.</param>
		/// <param name="highlightText">Should text be drawn highlighted.</param>
		/// <param name="xMax">Maximum width allowed for drawing.</param>
		/// <param name="xDraw">Actual width used in drawing image.</param>
        protected virtual void DrawTabText(Rectangle rectTab, 
                                           TabPage page, 
                                           Graphics g, 
                                           bool highlightText,
                                           int xMax, 
                                           int xDraw)
        {
            try
            {
                if (!_selectedTextOnly || (_selectedTextOnly && page.Selected))
                {
                    // Any space for drawing text?
                    if (xDraw < xMax)
                    {
                        Color drawColor;

                        Font drawFont = base.Font;
                        bool disposeFont = false;

                        // Decide which base color to use
                        if (highlightText)
                        {
                            if (IsOfficeStyle && (_style != VisualStyle.Office2003))
                                drawColor = Office2007ColorTable.TabHighlightTextColor(_style, _officeStyle);
                            else if (IsMediaPlayerStyle)
                                drawColor = MediaPlayerColorTable.TabHighlightTextColor(_style, _mediaPlayerStyle);
                            else
                                drawColor = _hotTextColor;
                        }
                        else
                        {
                            // Text color logic depends on if tab is selected
                            if (page.Selected)
                            {
                                // If page provides explicit text color, then use it, otherwise use default
                                if (page.SelectTextColor != Color.Empty)
                                    drawColor = page.SelectTextColor;
                                else
                                {
                                    if (IsOfficeStyle && (_style != VisualStyle.Office2003) && !ShouldSerializeTextColor())
                                        drawColor = Office2007ColorTable.TabActiveTextColor(_style, _officeStyle);
                                    else if (IsMediaPlayerStyle && !ShouldSerializeTextColor())
                                        drawColor = MediaPlayerColorTable.TabActiveTextColor(_style, _mediaPlayerStyle);
                                    else
                                        drawColor = _textColor;
                                }
                            }
                            else
                            {
                                // If page provides explicit text color, then use it
                                if (page.InactiveTextColor != Color.Empty)
                                    drawColor = page.InactiveTextColor;
                                else
                                {
                                    if (IsMediaPlayerStyle || (IsOfficeStyle && (_style != VisualStyle.Office2003)))
                                    {
                                        if (!ShouldSerializeTextInactiveColor())
                                        {
                                            if (IsMediaPlayerStyle)
                                                drawColor = MediaPlayerColorTable.TabInactiveTextColor(_style, _mediaPlayerStyle);
                                            else
                                                drawColor = Office2007ColorTable.TabInactiveTextColor(_style, _officeStyle);
                                        }
                                        else
                                            drawColor = _textInactiveColor;
                                    }
                                    else if (_dimUnselected && (_style != VisualStyle.IDE2005))
                                    {
                                        drawColor = _textInactiveColor;
                                    }
                                    else
                                        drawColor = _textColor;
                                }
                            }
                        }

                        // Should selected items be drawn in bold?
                        if (_boldSelected && page.Selected)
                        {
                            drawFont = new Font(drawFont, FontStyle.Bold);
                            disposeFont = true;
                        }

                        // Now the color is determined, create solid brush
                        SolidBrush drawBrush = new SolidBrush(drawColor);

                        // Ensure only a single line is draw from then left hand side of the
                        // rectangle and if to large for line to shows ellipsis for us
                        using (StringFormat drawFormat = new StringFormat())
                        {
                            drawFormat.FormatFlags = StringFormatFlags.NoClip | StringFormatFlags.NoWrap;
                            drawFormat.Trimming = StringTrimming.EllipsisCharacter;
                            drawFormat.Alignment = StringAlignment.Center;
                            drawFormat.LineAlignment = StringAlignment.Center;
                            drawFormat.HotkeyPrefix = _hotkeyPrefix;

                            // Find the vertical drawing limits for text
                            int yStart = rectTab.Top + _position[_styleIndex, (int)PositionIndex.ImageGapTop] - 1;

                            int yEnd = rectTab.Bottom -
                                       _position[_styleIndex, (int)PositionIndex.ImageGapBottom] -
                                       _position[_styleIndex, (int)PositionIndex.BorderBottom];

                            // Use text offset to adjust position of text
                            yStart += _position[_styleIndex, (int)PositionIndex.TextOffset];

                            // Across the text left gap
                            xDraw += _position[_styleIndex, (int)PositionIndex.TextGapLeft];

                            // Need at least 1 pixel width before trying to draw
                            if (xDraw < xMax)
                            {
                                // Find drawing rectangle
                                Rectangle drawRect = new Rectangle(xDraw, yStart, xMax - xDraw, yEnd - yStart);

                                // If the page is currently selected
                                if (page.Selected)
                                {
                                    // And the page wants a specific text background color
                                    if (page.SelectTextBackColor != Color.Empty)
                                        using (SolidBrush backBrush = new SolidBrush(page.SelectTextBackColor))
                                            g.FillRectangle(backBrush, drawRect);
                                }
                                else
                                {
                                    // And the page wants a specific text background color
                                    if (page.InactiveTextBackColor != Color.Empty)
                                        using (SolidBrush backBrush = new SolidBrush(page.InactiveTextBackColor))
                                            g.FillRectangle(backBrush, drawRect);
                                }

                                try
                                {
                                    // Finally....draw the string!
                                    if (((_style == VisualStyle.Office2007Blue) ||
                                         (_style == VisualStyle.Office2007Silver) ||
                                         (_style == VisualStyle.Office2007Black)) && Apply2007ClearType)
                                    {

                                        using (UseClearTypeGridFit clearType = new UseClearTypeGridFit(g))
                                            g.DrawString(page.Title, drawFont, drawBrush, drawRect, drawFormat);
                                    }
                                    else if (((_style == VisualStyle.MediaPlayerBlue) ||
                                              (_style == VisualStyle.MediaPlayerOrange) ||
                                              (_style == VisualStyle.MediaPlayerPurple)) && ApplyMediaPlayerClearType)
                                    {

                                        using (UseClearTypeGridFit clearType = new UseClearTypeGridFit(g))
                                            g.DrawString(page.Title, drawFont, drawBrush, drawRect, drawFormat);
                                    }
                                    else
                                    {
                                        g.DrawString(page.Title, drawFont, drawBrush, drawRect, drawFormat);
                                    }
                                }
                                catch
                                {
                                    RedefineSystemFont();
                                    _recalculate = true;
                                    Invalidate();
                                }
                            }

                            // Cleanup resources!
                            if (disposeFont)
                                drawFont.Dispose();

                            drawBrush.Dispose();
                        }
                    }
                }
            }
            catch (ArithmeticException)
            {
                Invalidate(rectTab);
            }
        }

		/// <summary>
		/// Draw the border around a tab header in correct style.
		/// </summary>
		/// <param name="rectTab">Inner rectangle left after drawing border.</param>
		/// <param name="page">TabPage to draw border for.</param>
		/// <param name="g">Graphics object to use in drawing operations.</param>
        protected virtual void DrawTabBorder(ref Rectangle rectTab, TabPage page, Graphics g)
        {
            if (_appearance == VisualAppearance.MultiBox)
            {
                if (_positionAtTop)
                    rectTab.Y -= MULTIBOX_ADJUSTUP;
                else
                    rectTab.Y += MULTIBOX_ADJUSTDOWN;

                // Draw the same regardless of style
                DrawMultiBoxBorder(page, g, rectTab);
            }
            else
            {
                // Drawing the border is style specific
                switch(_style)
                {
                    case VisualStyle.Plain:
                        DrawPlainTabBorder(page, g, rectTab);
                        break;				
					case VisualStyle.IDE2005:
						DrawIDE2005TabBorder(page, g, rectTab);
						break;				
					case VisualStyle.Office2003:
                    case VisualStyle.Office2007Blue:
                    case VisualStyle.Office2007Silver:
                    case VisualStyle.Office2007Black:
                        DrawOfficeTabBorder(page, g, rectTab);
						break;
                    case VisualStyle.MediaPlayerBlue:
                    case VisualStyle.MediaPlayerOrange:
                    case VisualStyle.MediaPlayerPurple:
                        DrawMediaPlayerTabBorder(page, g, rectTab);
                        break;
                }
            }
        }

		/// <summary>
		/// Draw the border around a tab header in multibox style.
		/// </summary>
		/// <param name="page">TabPage to draw border for.</param>
		/// <param name="g">Graphics object to use in drawing operations.</param>
		/// <param name="rectPage">Inner rectangle left after drawing border.</param>
        protected virtual void DrawMultiBoxBorder(TabPage page, Graphics g, Rectangle rectPage)
        {
            if (page.Selected)
            {
				Color ll = _backLightLight;
				Color dd = _backDarkDark;
            
                if (_defaultColor)
                {
                    if (_style == VisualStyle.IDE2005)
                    {
                        ll = IDE2005ActiveColor;
                        dd = SystemColors.ControlDark;
                    }
                    else if (_style == VisualStyle.Office2003)
                    {
                        ll = Office2003ActiveColor;
                        dd = _colorDetails.ActiveBorderColor;
                    }
                    else if (IsOfficeStyle)
                    {
                        ll = Office2003ActiveColor;
                        dd = Office2007ColorTable.BorderColor(_style);
                    }
                    else if (IsMediaPlayerStyle)
                    {
                        ll = MediaPlayerActiveColor;
                        dd = MediaPlayerColorTable.BorderColor(_style);
                    }
                }
            
                using (SolidBrush lightlight = new SolidBrush(ll))
                    g.FillRectangle(lightlight, rectPage);

                using (Pen darkdark = new Pen(dd))
                    g.DrawRectangle(darkdark, rectPage);
            }
            else
            {
				Color b = this.BackColor;
            
				if (IsOfficeStyle && _defaultColor)                         
                    b = OfficeBackColor;

                if (IsMediaPlayerStyle && _defaultColor)
                    b = MediaPlayerBackColor;

                if ((_style == VisualStyle.IDE2005) && _defaultColor)       
                    b = IDE2005BackColor;
				            
                using (SolidBrush backBrush = new SolidBrush(b))
                    g.FillRectangle(backBrush, rectPage.X + 1, rectPage.Y, rectPage.Width - 1, rectPage.Height);
            
                // Find the index into TabPage collection for this page
                int index = _tabPages.IndexOf(page);

                // Decide if the separator should be drawn
                bool drawSeparator = (index == _tabPages.Count - 1) ||
									 (index < (_tabPages.Count - 1)) && 
									 (_tabPages[index+1].Selected != true);
                                         
                // MultiLine mode is slighty more complex
                if (_multiline && !drawSeparator)
                {
                    // By default always draw separator
                    drawSeparator = true;
                    
                    // If we are not the last item
                    if (index < (_tabPages.Count - 1))
                    {
                        // If the next item is selected
                        if (_tabPages[index+1].Selected == true)
                        {
                            Rectangle thisRect = (Rectangle)_tabRects[index];
                            Rectangle nextRect = (Rectangle)_tabRects[index+1];

                            // If we are on the same drawing line then do not draw separator
                            if (thisRect.Y == nextRect.Y)
                                drawSeparator = false;
                        }
                    }
                }

                // Draw tab separator unless the next page after us is selected
                if (drawSeparator)
                {
					Color ll = _backLightLight;
					Color d = _backDark;

					// Use different colours for office 2003/office 2007
                    if (_defaultColor)
                    {
                        if ((_style == VisualStyle.IDE2005) || (_style == VisualStyle.Office2003))
                        {
                            ll = _colorDetails.SepLightColor;
                            d = _colorDetails.SepDarkColor;
                        }
                        else if (IsOfficeStyle)
                        {
                            ll = Office2007ColorTable.SepLightColor(_style, _officeStyle);
                            d = Office2007ColorTable.SepDarkColor(_style, _officeStyle);
                        }
                        else if (IsMediaPlayerStyle)
                        {
                            ll = MediaPlayerColorTable.SepLightColor(_style, _mediaPlayerStyle);
                            d = MediaPlayerColorTable.SepDarkColor(_style, _mediaPlayerStyle);
                        }
                    }
						                
                    using(Pen lightlight = new Pen(ll),
                              dark = new Pen(d))
                    {
                        g.DrawLine(dark, rectPage.Right, rectPage.Top + 2, rectPage.Right, 
                                   rectPage.Bottom - _position[_styleIndex, (int)PositionIndex.TabsBottomGap] - 1);
                        g.DrawLine(lightlight, rectPage.Right+1, rectPage.Top + 2, rectPage.Right+1, 
                                   rectPage.Bottom - _position[_styleIndex, (int)PositionIndex.TabsBottomGap] - 1);
                    }
                }
            }
        }

		/// <summary>
		/// Draw the border around a tab header in plain style.
		/// </summary>
		/// <param name="page">TabPage to draw border for.</param>
		/// <param name="g">Graphics object to use in drawing operations.</param>
		/// <param name="rectPage">Inner rectangle left after drawing border.</param>
		protected virtual void DrawPlainTabBorder(TabPage page, Graphics g, Rectangle rectPage)
        {
			// Define default colors
			Color ll = _backLightLight;
			Color b = base.BackColor;
			Color d = _backDark;
			Color dd = _backDarkDark;
        
			// Calculate which colors to use
			if (page.Selected)
			{
				// If we are provided with a proper color to use
				if (page.SelectBackColor != Color.Empty)
				{
					b = page.SelectBackColor;
					ll = ControlPaint.LightLight(b);
					d = ControlPaint.Dark(b);
					dd = ControlPaint.DarkDark(b);
				}
			}
			else
			{
				// If we are provided with a proper color to use
				if (page.InactiveBackColor != Color.Empty)
				{
					b = page.InactiveBackColor;
					ll = ControlPaint.LightLight(b);
					d = ControlPaint.Dark(b);
					dd = ControlPaint.DarkDark(b);
				}
			}        
        
            using(Pen light = new Pen(ll),
                      dark = new Pen(d),
                      darkdark = new Pen(dd))
            {
                int yLeftOffset = 0;
                int yRightOffset = 0;

                using (SolidBrush backBrush = new SolidBrush(b))
                {
                    // Calculate the rectangle that covers half the top border area
                    int yBorder = rectPage.Top;
                    int addHeight = 0;
					
					if (page.Selected)
					{
						if (_positionAtTop)
							addHeight = (_position[_styleIndex, (int)PositionIndex.BorderTop] / 2);
						else
						{
							yBorder = rectPage.Top - (_position[_styleIndex, (int)PositionIndex.BorderTop] / 2);
							addHeight = (_position[_styleIndex, (int)PositionIndex.BorderTop] / 2);
						}
					}
					
                    // Construct rectangle that covers the outer part of the border
                    Rectangle rectBorder = new Rectangle(rectPage.Left + 1, yBorder, rectPage.Width - 2, rectPage.Height + addHeight);

                    // Blank out area 
                    g.FillRectangle(backBrush, rectBorder);

					if (page.Selected)
					{
                        // Make the left and right border lines extend higher up
                        yLeftOffset = -2;
                        yRightOffset = -1;
                    }
                }

                if (_positionAtTop)
                {
                    // Draw the left border
                    g.DrawLine(light, rectPage.Left, rectPage.Bottom, rectPage.Left, rectPage.Top + 2); 
                    g.DrawLine(light, rectPage.Left + 1 , rectPage.Top + 1, rectPage.Left + 1, rectPage.Top + 2); 

                    // Draw the top border
                    g.DrawLine(light, rectPage.Left + 2, rectPage.Top + 1, rectPage.Right - 2, rectPage.Top + 1); 

                    // Draw the right border
                    g.DrawLine(darkdark, rectPage.Right, rectPage.Bottom - yRightOffset, rectPage.Right, rectPage.Top + 2); 
                    g.DrawLine(dark, rectPage.Right - 1, rectPage.Bottom - yRightOffset, rectPage.Right - 1, rectPage.Top + 2); 
                    g.DrawLine(dark, rectPage.Right - 2, rectPage.Top + 1, rectPage.Right - 2, rectPage.Top + 2); 
                    g.DrawLine(darkdark, rectPage.Right - 2, rectPage.Top, rectPage.Right, rectPage.Top + 2);
                }
                else
                {
                    // Draw the left border
                    g.DrawLine(light, rectPage.Left, rectPage.Top + yLeftOffset, rectPage.Left, rectPage.Bottom - 2); 
                    g.DrawLine(dark, rectPage.Left + 1 , rectPage.Bottom - 1, rectPage.Left + 1, rectPage.Bottom - 2); 

                    // Draw the bottom border
                    g.DrawLine(dark, rectPage.Left + 2, rectPage.Bottom - 1, rectPage.Right - 2, rectPage.Bottom - 1); 
                    g.DrawLine(darkdark, rectPage.Left + 2, rectPage.Bottom, rectPage.Right - 2, rectPage.Bottom); 

                    // Draw the right border
                    g.DrawLine(darkdark, rectPage.Right, rectPage.Top, rectPage.Right, rectPage.Bottom - 2); 
                    g.DrawLine(dark, rectPage.Right - 1, rectPage.Top + yRightOffset, rectPage.Right - 1, rectPage.Bottom - 2); 
                    g.DrawLine(dark, rectPage.Right - 2, rectPage.Bottom - 1, rectPage.Right - 2, rectPage.Bottom - 2); 
                    g.DrawLine(darkdark, rectPage.Right - 2, rectPage.Bottom, rectPage.Right, rectPage.Bottom - 2);
                }
            }
        }

		/// <summary>
		/// Draw the border around a tab header in IDE style.
		/// </summary>
		/// <param name="page">TabPage to draw border for.</param>
		/// <param name="g">Graphics object to use in drawing operations.</param>
		/// <param name="rectPage">Inner rectangle left after drawing border.</param>
		protected virtual void DrawIDETabBorder(TabPage page, Graphics g, Rectangle rectPage)
        {
			// Define default colors
			Color ll = _backLightLight;
			Color b = base.BackColor;
			Color bi = _backIDE;
			Color d = _backDark;
			Color dd = _backDarkDark;
        
			// Calculate which colors to use
			if (page.Selected)
			{
				// If we are provided with a proper color to use
				if (page.SelectBackColor != Color.Empty)
				{
					b = page.SelectBackColor;
					ll = ControlPaint.LightLight(b);
					d = ControlPaint.Dark(b);
					dd = ControlPaint.DarkDark(b);
				}
			}
			else
			{
				// If we are provided with a proper color to use
				if (page.InactiveBackColor != Color.Empty)
					bi = page.InactiveBackColor;
			}
        
            using(Pen lightlight = new Pen(ll),
                      backColor = new Pen(b),
                      dark = new Pen(d),
                      darkdark = new Pen(dd))
            {
                if (page.Selected)
                {
                    // Draw background in selected color
                    using (SolidBrush pageAreaBrush = new SolidBrush(b))
                        g.FillRectangle(pageAreaBrush, rectPage);

                    if (_positionAtTop)
                    {
                        // Overdraw the bottom border
                        g.DrawLine(backColor, rectPage.Left, rectPage.Bottom, rectPage.Right - 1, rectPage.Bottom);

                        // Draw the right border
                        g.DrawLine(darkdark, rectPage.Right, rectPage.Top, rectPage.Right, rectPage.Bottom); 
                    }
                    else
                    {
                        // Draw the left border
                        g.DrawLine(lightlight, rectPage.Left, rectPage.Top - 1 , rectPage.Left, rectPage.Bottom); 

                        // Draw the bottom border
                        g.DrawLine(darkdark, rectPage.Left + 1, rectPage.Bottom, rectPage.Right, rectPage.Bottom); 

                        // Draw the right border
                        g.DrawLine(darkdark, rectPage.Right, rectPage.Top - 1 , rectPage.Right, rectPage.Bottom); 

                        // Overdraw the top border
                        g.DrawLine(backColor, rectPage.Left + 1, rectPage.Top - 1, rectPage.Right - 1, rectPage.Top - 1);
                    }
                }
                else
                {
                    // Draw background in unselected color
                    using(SolidBrush tabsAreaBrush = new SolidBrush(bi))
                        g.FillRectangle(tabsAreaBrush, rectPage);

                    // Find the index into TabPage collection for this page
                    int index = _tabPages.IndexOf(page);

                    // Decide if the separator should be drawn
                    bool drawSeparator = (index == _tabPages.Count - 1) ||
                                         (index < (_tabPages.Count - 1)) && 
                                         (_tabPages[index+1].Selected != true);
                                         
                    // MultiLine mode is slighty more complex
                    if (_multiline && !drawSeparator)
                    {
                        // By default always draw separator
                        drawSeparator = true;
                    
                        // If we are not the last item
                        if (index < (_tabPages.Count - 1))
                        {
                            // If the next item is selected
                            if (_tabPages[index+1].Selected == true)
                            {
                                Rectangle thisRect = (Rectangle)_tabRects[index];
                                Rectangle nextRect = (Rectangle)_tabRects[index+1];

                                // If we are on the same drawing line then do not draw separator
                                if (thisRect.Y == nextRect.Y)
                                    drawSeparator = false;
                            }
                        }
                    }

                    // Draw tab separator unless the next page after us is selected
                    if (drawSeparator)
                    {
                        // Reduce the intensity of the color
                        using(Pen linePen = new Pen(_textInactiveColor))
                            g.DrawLine(linePen, rectPage.Right, rectPage.Top + 2, rectPage.Right, 
									   rectPage.Bottom - _position[_styleIndex, (int)PositionIndex.TabsBottomGap] - 1);
                    }
                }
            }
        }
        
		/// <summary>
		/// Draw the border around a tab header in IDE2005 style.
		/// </summary>
		/// <param name="page">TabPage to draw border for.</param>
		/// <param name="g">Graphics object to use in drawing operations.</param>
		/// <param name="rectPage">Inner rectangle left after drawing border.</param>
		protected virtual void DrawIDE2005TabBorder(TabPage page, Graphics g, Rectangle rectPage)
        {
			if ((rectPage.Width > 0) && (rectPage.Height > 0))
			{
				// Define default colors
				Color b = ControlPaint.Dark(base.BackColor);
				Color a = IDE2005ActiveColor;
				Color iat = ControlPaint.Light(base.BackColor);
				Color iab = a;
				
				// Extend the tab page left one pixel so it overlaps the previous tab
				if (rectPage.Left > 0)
				{
					rectPage.Width++;
					rectPage.X--;
				}

				// Use theme colors only if using default colors
				if (_defaultColor)
				{
					// Define the apropriate colours for office themes
					b = SystemColors.ControlDark;
					iat = _colorDetails.BaseColor2;
					iab = _colorDetails.BaseColor1;
				}
	        
				// Calculate which colors to use
				if (page.Selected)
				{
					// If we are provided with a proper color to use
					if (page.SelectBackColor != Color.Empty)
						a = page.SelectBackColor;
				}
				else
				{
					// If we are provided with a proper color to use
					if (page.InactiveBackColor != Color.Empty)
					{
						iat = page.InactiveBackColor;
						iab = ControlPaint.Light(page.InactiveBackColor);
					}
				}
				
				using(Pen borderPen = new Pen(b))
				{
					// Rectangle used to fill is not the same as the total rectangle
					Rectangle fillRect = rectPage;
					
					// Do not need to draw where the border is going to be drawn
					fillRect.X++;
					fillRect.Width--;
					
					Brush fillBrush;

					// Create appropriate tab brush
					if (page.Selected)
						fillBrush = new SolidBrush(a);
					else
						fillBrush = new LinearGradientBrush(fillRect, iat, iab, 90f);

					if (_positionAtTop)
					{
						fillRect.Y++;

						if (!page.Selected || !_ide2005TabJoined)
							fillRect.Height--;

						g.FillRectangle(fillBrush, fillRect);

						g.DrawLine(borderPen, rectPage.Left, rectPage.Bottom, rectPage.Left, rectPage.Top + 2); 
						g.DrawLine(borderPen, rectPage.Left, rectPage.Top + 2, rectPage.Left + 2, rectPage.Top); 
						g.DrawLine(borderPen, rectPage.Right, rectPage.Bottom, rectPage.Right, rectPage.Top + 2); 
						g.DrawLine(borderPen, rectPage.Right, rectPage.Top + 2, rectPage.Right - 2, rectPage.Top); 
						g.DrawLine(borderPen, rectPage.Left + 2, rectPage.Top, rectPage.Right - 2, rectPage.Top); 
					}
					else
					{
						if (page.Selected && _ide2005TabJoined)
						{
							fillRect.Y--;
							fillRect.Height++;
						}

						g.FillRectangle(fillBrush, fillRect);

						g.DrawLine(borderPen, rectPage.Left, rectPage.Top - 1 , rectPage.Left, rectPage.Bottom - 2); 
						g.DrawLine(borderPen, rectPage.Left, rectPage.Bottom - 2, rectPage.Left + 2, rectPage.Bottom); 
						g.DrawLine(borderPen, rectPage.Right, rectPage.Top - 1 , rectPage.Right, rectPage.Bottom - 2); 
						g.DrawLine(borderPen, rectPage.Right, rectPage.Bottom - 2, rectPage.Right - 2, rectPage.Bottom); 
						g.DrawLine(borderPen, rectPage.Left + 2, rectPage.Bottom, rectPage.Right - 2, rectPage.Bottom); 
					}

					fillBrush.Dispose();
				}
			}
		}
		
		/// <summary>
		/// Draw the border around a tab header in Office2003/Office2007 style.
		/// </summary>
		/// <param name="page">TabPage to draw border for.</param>
		/// <param name="g">Graphics object to use in drawing operations.</param>
		/// <param name="rectPage">Inner rectangle left after drawing border.</param>
		protected virtual void DrawOfficeTabBorder(TabPage page, Graphics g, Rectangle rectPage)
        {
			// Define default colors
			Color dd = _backDarkDark;
			Color a = Office2003ActiveColor;
			Color sd = ControlPaint.Dark(base.BackColor);
			Color sl = ControlPaint.Light(base.BackColor);
			
			// Use theme colors only if using default colors
			if (_defaultColor)
			{
                if (_style == VisualStyle.Office2003)
                {
                    dd = _colorDetails.ActiveBorderColor;
                    sd = _colorDetails.SepDarkColor;
                    sl = _colorDetails.SepLightColor;
                }
                else
                {
                    dd = Office2007ColorTable.BorderColor(_style);
                    sd = Office2007ColorTable.SepDarkColor(_style, _officeStyle);
                    sl = Office2007ColorTable.SepLightColor(_style, _officeStyle);
                }
			}
        
			// Calculate which colors to use
			if (page.Selected)
			{
				// If we are provided with a proper color to use
				if (page.SelectBackColor != Color.Empty)
					a = page.SelectBackColor;
			}
			
			using(Pen darkdark = new Pen(dd),
					  sepdark = new Pen(sd),
					  seplight = new Pen(sl),
					  active = new Pen(a))
			{
				// Rectangle used to fill is not the same as the total rectangle
				Rectangle fillRect = rectPage;
				
				// Do not need to draw where the border is going to be drawn
				fillRect.X++;
				fillRect.Width--;
				
				// Do no draw over the top/bottom border
				if (_positionAtTop)
					fillRect.Y++;
				else
				{
					fillRect.Y--;
					fillRect.Height++;
				}
			
				if (page.Selected)
				{
					// Draw background in selected color
					using (SolidBrush pageAreaBrush = new SolidBrush(a))
						g.FillRectangle(pageAreaBrush, fillRect);

					if (_positionAtTop)
					{
						// Draw the left border
						g.DrawLine(darkdark, rectPage.Left, rectPage.Bottom, rectPage.Left, rectPage.Top + 2); 

						// Draw the left corner border
						g.DrawLine(darkdark, rectPage.Left, rectPage.Top + 2, rectPage.Left + 2, rectPage.Top); 

						// Draw the right border
						g.DrawLine(darkdark, rectPage.Right, rectPage.Bottom, rectPage.Right, rectPage.Top + 2); 

						// Draw the right corner border
						g.DrawLine(darkdark, rectPage.Right, rectPage.Top + 2, rectPage.Right - 2, rectPage.Top); 

						// Draw the top border
						g.DrawLine(darkdark, rectPage.Left + 2, rectPage.Top, rectPage.Right - 2, rectPage.Top); 
					}
					else
					{
						// Draw the left border
						g.DrawLine(darkdark, rectPage.Left, rectPage.Top - 1 , rectPage.Left, rectPage.Bottom - 2); 

						// Draw the left corner border
						g.DrawLine(darkdark, rectPage.Left, rectPage.Bottom - 2, rectPage.Left + 2, rectPage.Bottom); 

						// Draw the right border
						g.DrawLine(darkdark, rectPage.Right, rectPage.Top - 1 , rectPage.Right, rectPage.Bottom - 2); 

						// Draw the right corner border
						g.DrawLine(darkdark, rectPage.Right, rectPage.Bottom - 2, rectPage.Right - 2, rectPage.Bottom); 

						// Draw the bottom border
						g.DrawLine(darkdark, rectPage.Left + 2, rectPage.Bottom, rectPage.Right - 2, rectPage.Bottom); 
					}
				}
				else
				{
					// Find the index into TabPage collection for this page
					int index = _tabPages.IndexOf(page);

					// Decide if the separator should be drawn
					bool drawSeparator = (index < (_tabPages.Count - 1)) && (_tabPages[index+1].Selected != true);
                                         
					// MultiLine mode is slighty more complex
					if (_multiline && !drawSeparator)
					{
						// By default always draw separator
						drawSeparator = true;
                    
						// If we are not the last item
						if (index < (_tabPages.Count - 1))
						{
							// If the next item is selected
							if (_tabPages[index+1].Selected == true)
							{
								Rectangle thisRect = (Rectangle)_tabRects[index];
								Rectangle nextRect = (Rectangle)_tabRects[index+1];

								// If we are on the same drawing line then do not draw separator
								if (thisRect.Y == nextRect.Y)
									drawSeparator = false;
							}
						}
					}

					// Draw tab separator unless the next page after us is selected
					if (drawSeparator)
					{
						g.DrawLine(sepdark, rectPage.Right - 1, rectPage.Top + 2, rectPage.Right - 1, rectPage.Bottom - _position[_styleIndex, (int)PositionIndex.TabsBottomGap] - 2);
						g.DrawLine(seplight, rectPage.Right, rectPage.Top + 3, rectPage.Right, rectPage.Bottom - _position[_styleIndex, (int)PositionIndex.TabsBottomGap] - 1);
					}
				}
			}
		}

        /// <summary>
        /// Draw the border around a tab header in MediaPlayer style.
        /// </summary>
        /// <param name="page">TabPage to draw border for.</param>
        /// <param name="g">Graphics object to use in drawing operations.</param>
        /// <param name="rectPage">Inner rectangle left after drawing border.</param>
        protected virtual void DrawMediaPlayerTabBorder(TabPage page, Graphics g, Rectangle rectPage)
        {
            // Define default colors
            Color dd = _backDarkDark;
            Color a = MediaPlayerActiveColor;
            Color sd = ControlPaint.Dark(base.BackColor);
            Color sl = ControlPaint.Light(base.BackColor);

            // Use theme colors only if using default colors
            if (_defaultColor)
            {
                dd = MediaPlayerColorTable.BorderColor(_style);
                sd = MediaPlayerColorTable.SepDarkColor(_style, _mediaPlayerStyle);
                sl = MediaPlayerColorTable.SepLightColor(_style, _mediaPlayerStyle);
            }

            // Calculate which colors to use
            if (page.Selected)
            {
                // If we are provided with a proper color to use
                if (page.SelectBackColor != Color.Empty)
                    a = page.SelectBackColor;
            }

            using (Pen darkdark = new Pen(dd),
                      sepdark = new Pen(sd),
                      seplight = new Pen(sl),
                      active = new Pen(a))
            {
                // Rectangle used to fill is not the same as the total rectangle
                Rectangle fillRect = rectPage;

                // Do not need to draw where the border is going to be drawn
                fillRect.X++;
                fillRect.Width--;

                // Do no draw over the top/bottom border
                if (_positionAtTop)
                    fillRect.Y++;
                else
                {
                    fillRect.Y--;
                    fillRect.Height++;
                }

                if (page.Selected)
                {
                    // Draw background in selected color
                    using (LinearGradientBrush pageAreaBrush = new LinearGradientBrush(new Rectangle(fillRect.X - 1, fillRect.Y - 1, fillRect.Width + 2, fillRect.Height + 2), (PositionTop ? Color.White : a), (PositionTop ? a : Color.White), 90f))
                        g.FillRectangle(pageAreaBrush, fillRect);

                    if (_positionAtTop)
                    {
                        // Draw the left border
                        g.DrawLine(darkdark, rectPage.Left, rectPage.Bottom, rectPage.Left, rectPage.Top + 2);

                        // Draw the left corner border
                        g.DrawLine(darkdark, rectPage.Left, rectPage.Top + 2, rectPage.Left + 2, rectPage.Top);

                        // Draw the right border
                        g.DrawLine(darkdark, rectPage.Right, rectPage.Bottom, rectPage.Right, rectPage.Top + 2);

                        // Draw the right corner border
                        g.DrawLine(darkdark, rectPage.Right, rectPage.Top + 2, rectPage.Right - 2, rectPage.Top);

                        // Draw the top border
                        g.DrawLine(darkdark, rectPage.Left + 2, rectPage.Top, rectPage.Right - 2, rectPage.Top);
                    }
                    else
                    {
                        // Draw the left border
                        g.DrawLine(darkdark, rectPage.Left, rectPage.Top - 1, rectPage.Left, rectPage.Bottom - 2);

                        // Draw the left corner border
                        g.DrawLine(darkdark, rectPage.Left, rectPage.Bottom - 2, rectPage.Left + 2, rectPage.Bottom);

                        // Draw the right border
                        g.DrawLine(darkdark, rectPage.Right, rectPage.Top - 1, rectPage.Right, rectPage.Bottom - 2);

                        // Draw the right corner border
                        g.DrawLine(darkdark, rectPage.Right, rectPage.Bottom - 2, rectPage.Right - 2, rectPage.Bottom);

                        // Draw the bottom border
                        g.DrawLine(darkdark, rectPage.Left + 2, rectPage.Bottom, rectPage.Right - 2, rectPage.Bottom);
                    }
                }
                else
                {
                    // Find the index into TabPage collection for this page
                    int index = _tabPages.IndexOf(page);

                    // Decide if the separator should be drawn
                    bool drawSeparator = (index < (_tabPages.Count - 1)) && (_tabPages[index + 1].Selected != true);

                    // MultiLine mode is slighty more complex
                    if (_multiline && !drawSeparator)
                    {
                        // By default always draw separator
                        drawSeparator = true;

                        // If we are not the last item
                        if (index < (_tabPages.Count - 1))
                        {
                            // If the next item is selected
                            if (_tabPages[index + 1].Selected == true)
                            {
                                Rectangle thisRect = (Rectangle)_tabRects[index];
                                Rectangle nextRect = (Rectangle)_tabRects[index + 1];

                                // If we are on the same drawing line then do not draw separator
                                if (thisRect.Y == nextRect.Y)
                                    drawSeparator = false;
                            }
                        }
                    }

                    // Draw tab separator unless the next page after us is selected
                    if (drawSeparator)
                    {
                        g.DrawLine(sepdark, rectPage.Right - 1, rectPage.Top + 2, rectPage.Right - 1, rectPage.Bottom - _position[_styleIndex, (int)PositionIndex.TabsBottomGap] - 2);
                        g.DrawLine(seplight, rectPage.Right, rectPage.Top + 3, rectPage.Right, rectPage.Bottom - _position[_styleIndex, (int)PositionIndex.TabsBottomGap] - 1);
                    }
                }
            }
        }
        
        /// <summary>
        /// Raises the TabsAreaRectChanged event.
        /// </summary>
        protected virtual void OnTabsAreaRectChanged()
        {
			if (TabsAreaRectChanged != null)
				TabsAreaRectChanged(this, EventArgs.Empty);
        }

		/// <summary>
		/// Processes when all TabPages are about to be removed.
		/// </summary>
        protected virtual void OnClearingPages()
        {
			_oldPage = null;

            // Is a page currently selected?
            if (_pageSelected != -1)
            {
				// Remember the old page during change of selection
				_oldPage = _tabPages[_pageSelected];

                // Deselect the page
                DeselectPage(_oldPage);

                // Remember that nothing is selected
                _pageSelected = -1;
                _startPage = -1;
            }

			// Raise selection changing event
			OnSelectionChanging(_oldPage, null);

			// Remove all the user controls 
            foreach (TabPage page in _tabPages)
            {
                // Unhook from its events
                page.PropertyChanged -= new TabPage.PropChangeHandler(OnPagePropertyChanged);

                // Remove as a child control
                RemoveTabPage(page);
            }

            // Remove all rectangles associated with TabPage's
            _tabRects.Clear();
        }

		/// <summary>
		/// Processes when all TabPages have been removed.
		/// </summary>
        protected virtual void OnClearedPages()
        {
            // Must recalculate after the pages have been removed and
            // not before as that would calculate based on pages still
            // being present in the list
            Recalculate();
            
            // Must notify a change in selection
            OnSelectionChanged(_oldPage, null);

            // Remove any reference to removed page
            _oldPage = null;
            
            Invalidate();
        }

		/// <summary>
		/// Processes when a new TabPage is being inserted.
		/// </summary>
		protected virtual void OnInsertingPage(int index, object value)
        {
			if (!_leftMouseDownDrag)
			{			
				// If currently suspended then ignore updating selected page
				if (!_suspended)
				{
					// If a page is currently selected?
					if (_pageSelected != -1)
					{
						// Is the selected page going to be after this new one in the list
						if (_pageSelected >= index)
							_pageSelected++;  // then need to update selection index to reflect this
					}
				}
			}
        }

		/// <summary>
		/// Processes when a new TabPage has been inserted.
		/// </summary>
		protected virtual void OnInsertedPage(int index, object value)
        {
			if (!_leftMouseDownDrag)
			{			
				bool selectPage = false;

				TabPage newPage = value as TabPage;

				// Hookup to receive TabPage property changes
				newPage.PropertyChanged += new TabPage.PropChangeHandler(OnPagePropertyChanged);

				// Add the appropriate Control/Form/TabPage to the control
				AddTabPage(newPage);

				// Add new rectangle to match new number of pages, this must be done before
				// the 'SelectPage', 'DeselectPage', 'OnSelectionChanging' or 'OnSelectionChanged' to 
				// ensure the number of _tabRects entries matches the number of _tabPages entries.
				_tabRects.Add((object)new Rectangle());

				TabPage oldPage = null;

				// If currently suspend then we use different logic for selecting a page
				if (_suspended)
				{
					// If page wants to be selected, then select it
					if (newPage.Selected)
						_pageSelected = index;

					// If no page is currently defined as the start page
					if (_startPage == -1)
						_startPage = 0;	 // then must be added then first page
				}
				else
				{
					// Do we want to select this page?
					if ((_pageSelected == -1) || (newPage.Selected))
					{
						// Get the currently selected page
						if (_pageSelected != -1)
							oldPage = _tabPages[_pageSelected];

						// Raise selection changing event
						OnSelectionChanging(oldPage, newPage);

						// Any page currently selected
						if (_pageSelected != -1)
							DeselectPage(_tabPages[_pageSelected]);

						// This becomes the newly selected page
						_pageSelected = _tabPages.IndexOf(newPage);

						// If no page is currently defined as the start page
						if (_startPage == -1)
							_startPage = 0;	 // then must be added then first page

						// Request the page be selected
						selectPage = true;
					}
				}

				// Only change selection is we are not suspended
				if (!_suspended)
				{
					// Cause the new page to be the selected one
					if (selectPage)
					{
						// Must recalculate to ensure the new _tabRects entry above it correctly
						// filled in before the new page is selected, as a change in page selection
						// may cause the _tabRects values ot be interrogated.
						Recalculate();

						SelectPage(newPage);

						// Raise selection change event
						OnSelectionChanged(oldPage, newPage);
					}

					Recalculate();
					Invalidate();
				}
			}
        }

		/// <summary>
		/// Processes when an existing TabPage is being removed.
		/// </summary>
		protected virtual void OnRemovingPage(int index, object value)
        {
			if (!_leftMouseDownDrag)
			{
				// Remember the old page for OnRemovedPage processing
				_oldPage = value as TabPage;

				// Unhook from its events
				_oldPage.PropertyChanged -= new TabPage.PropChangeHandler(OnPagePropertyChanged);

				// Remove the appropriate Control/Form/TabPage to the control
				RemoveTabPage(_oldPage);

				// Notice a change in selected page
				_changed = false;

				// Is this the currently selected page
				if (_pageSelected == index)
				{
					TabPage newPage = null;

					// Removing last page...
					if (index == (_tabPages.Count - 1))
					{
						// Then new selection is the previous page
						if (index > 0)
							newPage = _tabPages[index - 1];
					}
					else
					{
						// New page is the next page
						newPage = _tabPages[index + 1];
					}

					// Raise selection changing event
					OnSelectionChanging(_oldPage, newPage);

					_changed = true;
					DeselectPage(_oldPage);
				}
			}
        }

		/// <summary>
		/// Processes when an existing TabPage has been removed.
		/// </summary>
		protected virtual void OnRemovedPage(int index, object value)
        {
			if (!_leftMouseDownDrag)
			{			
				// Is first displayed page then one being removed?
				if (_startPage >= index)
				{
					// Decrement to use start displaying previous page
					_startPage--;

					// Have we tried to select off the left hand side?
					if (_startPage == -1)
					{
						// Are there still some pages left?
						if (_tabPages.Count > 0)
							_startPage = 0;
					}
				}

				// Is the selected page equal to or after this new one in the list
				if (_pageSelected >= index)
				{
					// Decrement index to reflect this change
					if ((_pageSelected != index) || (_pageSelected >= _tabPages.Count))
						_pageSelected--;

					// Have we tried to select off the left hand side?
					if (_pageSelected == -1)
					{
						// Are there still some pages left?
						if (_tabPages.Count > 0)
							_pageSelected = 0;
					}

					// Is the new selection valid?
					if (_pageSelected != -1)
						SelectPage(_tabPages[_pageSelected]);  // Select it
				}

				// Change in selection causes event generation
				if (_changed)
				{
					// Reset changed flag
					_changed = false;

					TabPage newPage = null;

					// Get the newly selected page
					if (_pageSelected >= 0)
						newPage = _tabPages[_pageSelected];

					// Raise selection change event
					OnSelectionChanged(_oldPage, newPage);
				}

				// Remove a rectangle to match number of pages
				_tabRects.RemoveAt(0);

                // Remove any reference to removed page
                _oldPage = null;

				Recalculate();
				Invalidate();
			}
        }
			
		/// <summary>
		/// Add a new tab page as a child of the control.
		/// </summary>
		/// <param name="page">TabPage to be added.</param>
        protected virtual void AddTabPage(TabPage page)
        {
            // Add user supplied control 
            if (page.Control != null)
            {
				// Has this control already been added?
				if (_controlSet[page.Control] != null)
				{
					int count = (int)_controlSet[page.Control];

					// Bump reference count for control
					_controlSet[page.Control] = (object)(count+1);
				}
				else
				{
					// Add into control set
					_controlSet.Add(page.Control, 1);

					// Has not been shown for the first time yet
					page.Shown = false;
        
					Form controlIsForm = page.Control as Form;

					page.Control.Hide();

					// Adding a Form takes extra effort
					if (controlIsForm == null)
					{
						// Control is sized manually, do not use dock style
						page.Control.Dock = DockStyle.None;

						// Monitor focus changes on the Control
						page.Control.GotFocus += new EventHandler(OnPageEnter);
						page.Control.LostFocus += new EventHandler(OnPageLeave);
						page.Control.MouseEnter += new EventHandler(OnPageMouseEnter);
						page.Control.MouseMove += new MouseEventHandler(OnPageMouseMove);
						page.Control.MouseLeave += new EventHandler(OnPageMouseLeave);

						Controls.Add(page.Control);
					}
					else
					{
						// Monitor activation changes on the TabPage
						controlIsForm.GotFocus += new EventHandler(OnPageEnter);
						controlIsForm.LostFocus += new EventHandler(OnPageLeave);
						controlIsForm.Activated += new EventHandler(OnPageEnter);
						controlIsForm.Deactivate += new EventHandler(OnPageLeave);
						controlIsForm.MouseEnter += new EventHandler(OnPageMouseEnter);
						controlIsForm.MouseMove += new MouseEventHandler(OnPageMouseMove);
						controlIsForm.MouseLeave += new EventHandler(OnPageMouseLeave);
		
						// Have to ensure the Form is not a top level form
						controlIsForm.TopLevel = false;

						// We are the new parent of this form
						controlIsForm.Parent = this;

						// To prevent user resizing the form manually and prevent
						// the caption bar appearing, we use the 'None' border style.
						controlIsForm.FormBorderStyle = FormBorderStyle.None;
					}

					// Need to monitor when the Form/Panel is clicked
					if ((page.Control is Form) || (page.Control is Panel))
						page.Control.MouseDown += new MouseEventHandler(OnPageMouseDown);

					RecursiveMonitor(page.Control, true);
				}
            }
            else
            {
				// Has not been shown for the first time yet
				page.Shown = false;
				
				page.Hide();

				// Monitor focus changes on the TabPage
				page.GotFocus += new EventHandler(OnPageEnter);
				page.LostFocus += new EventHandler(OnPageLeave);
				page.MouseEnter += new EventHandler(OnPageMouseEnter);
				page.MouseMove += new MouseEventHandler(OnPageMouseMove);
				page.MouseLeave += new EventHandler(OnPageMouseLeave);

				// Page is sized manually, do not use dock style
				page.Dock = DockStyle.None;

				// Need to monitor when the Panel is clicked
				page.MouseDown += new MouseEventHandler(OnPageMouseDown);

				RecursiveMonitor(page, true);

				// Add the TabPage itself instead
				Controls.Add(page);
			}
        }

		/// <summary>
		/// Remove a tab page as a child of the control.
		/// </summary>
		/// <param name="page"></param>
		protected virtual void RemoveTabPage(TabPage page)
		{
			// Remove user supplied control 
			if (page.Control != null)
				RemoveTabPageControl(page.Control);
			else
				RemoveTabPagePanel(page);
		}

		/// <summary>
		/// Remove a tab page that contains a control.
		/// </summary>
		/// <param name="c">Control to be removed.</param>
		protected virtual void RemoveTabPageControl(Control c)
        {
			// Get reference count for control
			int count = (int)_controlSet[c];

			// If not the last reference
			if (count > 1)
			{
				// Update count with one less
				_controlSet[c] = --count;
			}
			else
			{
				// Remove from lookup
				_controlSet.Remove(c);

				RecursiveMonitor(c, false);

				Form controlIsForm = c as Form;

				// Need to unhook hooked up event
				if ((c is Form) || (c is Panel))
					c.MouseDown -= new MouseEventHandler(OnPageMouseDown);

				if (controlIsForm == null)
				{
					// Unhook event monitoring
					c.GotFocus -= new EventHandler(OnPageEnter);
					c.LostFocus -= new EventHandler(OnPageLeave);
					c.MouseEnter -= new EventHandler(OnPageMouseEnter);
					c.MouseMove -= new MouseEventHandler(OnPageMouseMove);
					c.MouseLeave -= new EventHandler(OnPageMouseLeave);
		
					// Use helper method to circumvent form Close bug
					ControlHelper.Remove(this.Controls, c);
				}
				else
				{
					// Unhook activation monitoring
					controlIsForm.GotFocus -= new EventHandler(OnPageEnter);
					controlIsForm.LostFocus -= new EventHandler(OnPageLeave);
					controlIsForm.Activated -= new EventHandler(OnPageEnter);
					controlIsForm.Deactivate -= new EventHandler(OnPageLeave);
					controlIsForm.MouseEnter -= new EventHandler(OnPageMouseEnter);
					controlIsForm.MouseMove -= new MouseEventHandler(OnPageMouseMove);
					controlIsForm.MouseLeave -= new EventHandler(OnPageMouseLeave);

					// Remove Form but prevent the Form close bug
					ControlHelper.RemoveForm(this, controlIsForm);
				}
			}
        }

		/// <summary>
		/// Remove a tab page that is just a panel.
		/// </summary>
		/// <param name="page">TabPage that is a panel.</param>
		protected virtual void RemoveTabPagePanel(TabPage page)
		{
			RecursiveMonitor(page, false);

			// Need to unhook hooked up event
			page.MouseDown -= new MouseEventHandler(OnPageMouseDown);

			// Unhook event monitoring
			page.GotFocus -= new EventHandler(OnPageEnter);
			page.LostFocus -= new EventHandler(OnPageLeave);
			page.MouseEnter -= new EventHandler(OnPageMouseEnter);
			page.MouseMove -= new MouseEventHandler(OnPageMouseMove);
			page.MouseLeave -= new EventHandler(OnPageMouseLeave);

			// Use helper method to circumvent form Close bug
			ControlHelper.Remove(this.Controls, page);
		}

		/// <summary>
		/// Raise the PageGotFocus event.
		/// </summary>
		/// <param name="sender">Originator of event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnPageEnter(object sender, EventArgs e)
        {
			TabPage page = sender as TabPage;

			// If focus is going to the tabpage instance
			if (page != null)
			{
				// So focus onto the child that last had it
				if (page.StartFocus != null)
					page.StartFocus.Focus();
			}
			else
			{
				Control c = sender as Control;

				// Is focus going to a control instance
				if (c != null)
				{
					// Is this the top level control for a page
					if (_controlSet[c] != null)
					{
						// Get the tab page that contains this control
						TabPage controlTab = TabPageFromControl(c);

						// So focus onto the child that last had it
						if (controlTab.StartFocus != null)
							controlTab.StartFocus.Focus();
					}
				}
			}

            OnPageGotFocus(e);
        }

		/// <summary>
		/// Raise the PageLostFocus event.
		/// </summary>
		/// <param name="sender">Originator of event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnPageLeave(object sender, EventArgs e)
        {
            OnPageLostFocus(e);
		}
    
		/// <summary>
		/// Process a change that is no longer selected.
		/// </summary>
		/// <param name="page">TabPage losing selection.</param>
        protected virtual void DeselectPage(TabPage page)
        {
            page.Selected = false;

            // Hide any associated control
            if (page.Control != null)
			{
				// Should we remember which control had focus when leaving?
				if (_recordFocus)
				{
					// Record current focus location on Control
					if (page.Control.ContainsFocus)
					{
						// Find the focus control on the control
						Control f = FindFocus(page.Control);

						// Only save if something is selected
						if (f != null)
							page.StartFocus = f;
					}
				}

                page.Control.Hide();
			}
            else
			{
				// Should we remember which control had focus when leaving?
				if (_recordFocus)
				{
					// Record current focus location on Control
					if (page.ContainsFocus)
					{
						// Find the focus control
						Control f = FindFocus(page);

						// Only save if something is selected
						if (f != null)
							page.StartFocus = f;
					}
				}

                page.Hide();
			}
        }

		/// <summary>
		/// Process a change that is becoming selected.
		/// </summary>
		/// <param name="page">TabPage that is becoming selected.</param>
		protected virtual void SelectPage(TabPage page)
        {
            page.Selected = true;

            // Bring the control for this page to the front
            if (page.Control != null)
                HandleShowingTabPage(page, page.Control);
            else
                HandleShowingTabPage(page, page);
        }

		/// <summary>
		/// Handle the selection of tab page that has a child control.
		/// </summary>
		/// <param name="page">TabPage becoming selected.</param>
		/// <param name="c">Child control to be processed.</param>
        protected virtual void HandleShowingTabPage(TabPage page, Control c)
        {
            // Finally, show it!
            c.Show();

            // Restore focus to last known control to have it
            if (page.StartFocus != null)
                page.StartFocus.Focus();
            else
                c.Focus();

			// Raise event to show a page has the focus
			OnPageGotFocus(EventArgs.Empty);
        }
		
		/// <summary>
		/// Handle the selection of tab page that has no child control.
		/// </summary>
		/// <param name="page">TabPage becoming selected.</param>
        protected virtual void MovePageSelection(TabPage page)
        {
            int pageIndex = _tabPages.IndexOf(page);

            if (pageIndex != _pageSelected)
            {
				TabPage oldPage = null;
				TabPage newPage = null;

				// Get the currently selected page
				if (_pageSelected != -1)
					oldPage = _tabPages[_pageSelected];

				// Get the page to become selected
				if (pageIndex != -1)
					newPage = _tabPages[pageIndex];

                // Raise selection changing event
                OnSelectionChanging(oldPage, newPage);

                // Any page currently selected?
                if (oldPage != null)
                    DeselectPage(oldPage);

                _pageSelected = pageIndex;

				if (newPage != null)
                    SelectPage(newPage);

                // Change in selection causes tab pages sizes to change
                if (_boldSelected || _selectedTextOnly || !_shrinkPagesToFit || _multiline)
                {
                    Recalculate();
                    Invalidate();
                }

				// Raise selection change event
                OnSelectionChanged(oldPage, newPage);

                Invalidate();
			}
        }

		/// <summary>
		/// Raises the DoubleClick event.
		/// </summary>
		/// <param name="e">An EventArgs structure that contains the event data.</param>
		protected override void OnDoubleClick(EventArgs e)
		{
			Point pos = TabControl.MousePosition;

			int count = _tabRects.Count;

			for(int index=0; index<count; index++)
			{
				// Get tab drawing rectangle
				Rectangle local = (Rectangle)_tabRects[index];
				
				// If drawing on the control
				if (local != _nullPosition)
				{
					// Convert from Control to screen coordinates
					Rectangle screen = this.RectangleToScreen(local);

					if (screen.Contains(pos))
					{
						// Generate appropriate event
						OnDoubleClickTab(_tabPages[index]);
						break;
					}
				}
			}

			base.OnDoubleClick(e);
		}

		/// <summary>
		/// Raises the MouseUp event.
		/// </summary>
		/// <param name="e">A MouseEventArgs structure that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
			if (_leftMouseDownDrag)
            {
				// Kill any use of the scroll timer
				try
				{
					_scrollTimer.Stop();
				} catch {}

				// If exiting dragging when outside the control
				if (!_draggingInside)
				{
					// Always have to put the page back so we are in valid state
					_pageSelected = _mouseDragIndex;
					TabPages.Insert(_mouseDragIndex, _dragPage);

					// Setting false means that tabpage collection processing is now reenabled
					_leftMouseDownDrag = false;					

					// Allow tab collection events to be fired
					TabPages.ResumeEvents();

					// Did we end up moving the page to a different place?
					if (_mouseDragIndex != _mouseDownIndex)
					{
						// Generate event to indicate where the page has moved to
						OnPageMoved(_tabPages[_mouseDragIndex], _mouseDragIndex);
					}

					if (e.Button != MouseButtons.Left)
						OnPageDragQuit(e);
					else
						OnPageDragEnd(e);

					// Show change (but not refresh straight away)
					Recalculate();
					Invalidate();
				}
				else
				{
					// No longer dragging with left mouse
					_leftMouseDownDrag = false;

					// Allow tab collection events to be fired
					TabPages.ResumeEvents();

					// Did we end up moving the page to a different place?
					if (_mouseDragIndex != _mouseDownIndex)
					{
						// Generate event to indicate where the page has moved to
						OnPageMoved(_tabPages[_mouseDragIndex], _mouseDragIndex);
					}
				}
                
                _ignoreDownDrag = true;
            }

            if (e.Button == MouseButtons.Left)
            {
                // Exit any page dragging attempt
                _leftMouseDown = false;
            }
            else
            {
                // Is it the button that causes context menu to show?
                if (e.Button == MouseButtons.Right)
                {
                    Point mousePos = new Point(e.X, e.Y);
                
                    // Is the mouse in the tab area
                    if (_tabsAreaRect.Contains(mousePos))
                    {
                        CancelEventArgs ce = new CancelEventArgs();
                        
                        // Generate event giving handlers cancel to update/cancel menu
                        OnContextMenuStripDisplay(ce);
                        
                        // Still want the context menu?
                        if (!ce.Cancel)
                        {
                            // Is there any attached menu to show
                            if (ContextMenuStrip != null)
                            {
								KillToolTip();
								_popUpMenuShowing = true;
                                ContextMenuStrip.Show(this.PointToScreen(new Point(e.X, e.Y)));
                                _popUpMenuShowing = false;
                            }
                        }
                    }
                }
            }

            base.OnMouseUp(e);
        }

		/// <summary>
		/// Raises the MouseDown event.
		/// </summary>
		/// <param name="e">A MouseEventArgs structure that contains the event data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
        {
			// Any mouse press when dragging should quit it
			if (_leftMouseDownDrag)
			{
				// Kill any use of the scroll timer
				try
				{
					_scrollTimer.Stop();
				} 
				catch {}

				// If exiting dragging when outside the control
				if (!_draggingInside)
				{
					// Always have to put the page back so we are in valid state
					_pageSelected = _mouseDragIndex;
					TabPages.Insert(_mouseDragIndex, _dragPage);

					// Setting false means that tabpage collection processing is now reenabled
					_leftMouseDownDrag = false;

					// Allow tab collection events to be fired
					TabPages.ResumeEvents();

					OnPageDragQuit(e);

					// Show change (but not refresh straight away)
					Recalculate();
					Invalidate();
				}
				else
				{
					// No longer dragging with left mouse
					_leftMouseDownDrag = false;

					// Allow tab collection events to be fired
					TabPages.ResumeEvents();
				}
                
				_ignoreDownDrag = true;
			}
			else
			{
				// Only select a button or page when using left mouse button
				InternalMouseDown(new Point(e.X, e.Y));
			}

			base.OnMouseDown(e);
        }
        
		/// <summary>
		/// Handle mouse down processing.
		/// </summary>
		/// <param name="mousePos">Client mouse position.</param>
        protected virtual void InternalMouseDown(Point mousePos)
        {
			// Mouse down does not change page if mouse is not in tab header area
			if (_tabsAreaRect.Contains(mousePos))
			{
				// Are the scroll buttons being shown?
				if (_activeArrow.Visible)
				{
					// Ignore mouse down over the buttons area
					if (mousePos.X >= _activeArrow.Left)
						return;
				}
				else
				{
					// Are the scroll buttons being shown?
					if (_leftArrow.Visible)
					{
						// Ignore mouse down over the buttons area
						if (mousePos.X >= _leftArrow.Left)
							return;
					}
					else
					{
						// No, is the close button visible?
						if (_closeButton.Visible)
						{
							// Ignore mouse down over then close button area
							if (mousePos.X >= _closeButton.Left)
								return;
						}
					}
				}

				_mouseDownIndex = -1;

				// Clicked on a tab page?
				for(int i=0; i<_tabPages.Count; i++)
				{
					Rectangle rect = (Rectangle)_tabRects[i];

					// Is the mouse inside the pages rectangle?
					if (rect.Contains(mousePos))
					{
						// We want to select it, so remember index and break out
						_mouseDownIndex = i;
						break;
					}
				}

				// Is there any page to select?
				if ((_mouseDownIndex >= 0) && (_mouseDownIndex < _tabPages.Count))
				{
					// Remember where the left mouse was initially pressed
					if (Control.MouseButtons == MouseButtons.Left)
					{
						_leftMouseDown = true;
						_ignoreDownDrag = false;
						_leftMouseDownDrag = false;
						_leftMouseDownPos = mousePos;
					}

					if (_mouseDownIndex != _pageSelected)
					{
						MovePageSelection(_tabPages[_mouseDownIndex]);
						MakePageVisible(_tabPages[_mouseDownIndex]);

						// Set focus into the selected page contents
						if (_tabPages[_mouseDownIndex].Control != null)
							_tabPages[_mouseDownIndex].Control.Focus();
						else
							_tabPages[_mouseDownIndex].Focus();

						// Raise even to show a page has the focus
						OnPageGotFocus(EventArgs.Empty);
					}
					else
					{
						bool focusSet = false;

						// Set focus into the selected page contents
						if (_tabPages[_mouseDownIndex].Control != null)
						{
							if (!_tabPages[_mouseDownIndex].Control.ContainsFocus)
							{
								_tabPages[_mouseDownIndex].Control.Focus();
								focusSet = true;
							}
						}
						else
						{
							// Make sure the page has the focus
							if (!_tabPages[_mouseDownIndex].ContainsFocus)
							{
								_tabPages[_mouseDownIndex].Focus();
								focusSet = true;
							}
						}

						if (focusSet)
						{
							// Raise even to show a page has the focus
							OnPageGotFocus(EventArgs.Empty);
						}
					}
				}
			}
		}		

		/// <summary>
		/// Raises the MouseMove event.
		/// </summary>
		/// <param name="e">A MouseEventArgs structure that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            Point mousePt = new Point(e.X, e.Y);

            // If the mouse has really moved...
            if (mousePt != _mouseMovePt)
            {
                // Remember last mouse point used in test
                _mouseMovePt = mousePt;

                // Do we need to kill the tooltip
                TestForKillingToolTip();
            }

            if (_leftMouseDown)
            {
                Point thisPosition = mousePt;
        
				if (!_leftMouseDownDrag)
                {
                    bool startDrag = false;

                    // Create starting mouse down position
                    Rectangle dragRect = new Rectangle(_leftMouseDownPos, new Size(0,0));
                    
                    // Expand by size of the double click area
                    dragRect.Inflate(SystemInformation.DoubleClickSize);
                    
                    // Drag when mouse moves outside the double click area
                    startDrag = !dragRect.Contains(thisPosition);

                    if (startDrag && !_ignoreDownDrag)
                    {		
						// Can only start a page reordering if not in 
						// multiline mode and allowed to reorder
						if (!_multiline && _allowDragReorder)
						{
							// Prevent tab collection events being fired
							TabPages.SuspendEvents();

							// Remember the page to be dragged around
							_dragPage = TabPages[_mouseDownIndex];

							// Set the mouse moving page to down index
							_mouseDragIndex = _mouseDownIndex;

							// Enter dragging mode
							_leftMouseDownDrag = true;	

							// Is the mouse inside the tab header area?
							_draggingInside = true;
						}
						else
						{
							// Allowed to drag outside?
							if (_dragOutside)
							{
								// Prevent tab collection events being fired
								TabPages.SuspendEvents();

								// Enter dragging mode
								_leftMouseDownDrag = true;	

								// Is the mouse inside the tab header area?
								_draggingInside = false;

								// Remember the page to be dragged around
								_dragPage = TabPages[_mouseDownIndex];

								// Set the mouse moving page to down index
								_mouseDragIndex = _mouseDownIndex;

								// Generate event before removing the page
								OnPageDragStart(_dragPage, e);

								// Remove the dragging page from view
								_pageSelected = -1;
								TabPages.RemoveAt(_mouseDragIndex);

								// Show change straight away
								Recalculate();
								Refresh();
							}
						}
					}
                }
                else
                {
					// Allowed to be performing reordering?
					if (!_multiline && _allowDragReorder)
					{
						// If we are allowed to be dragging contents around inside tab header
						if (!DragOutside || _draggingInside)
						{
							// Find any page that the mouse is now over
                            TabPage pageOver = TabPageFromDraggingPoint(mousePt);

							int overIndex;

							// If over a valid page...
							if (pageOver != null)
								overIndex = TabPages.IndexOf(pageOver);
							else
								overIndex = -1;

							// Is it different from the current one?
							if (_mouseDragIndex != overIndex)
							{
								// Remove page from current position
								if (_mouseDragIndex >= 0)
								{
									_pageSelected = -1;
									TabPages.RemoveAt(_mouseDragIndex);
								}

								// Save the newly designated position
								_mouseDragIndex = overIndex;

								// Add page into new position
								if (_mouseDragIndex >= 0)
								{
									_pageSelected = _mouseDragIndex;
									TabPages.Insert(_mouseDragIndex, _dragPage);
								}

								// Show change straight away
								Recalculate();
								Refresh();
							}
						}
					
						// Do we need to handle drag outside processing?
						if (DragOutside)
						{
							// Is the mouse currently over the control tab area?
							bool draggingInside = _tabsAreaRect.Contains(thisPosition);

							// Change in state?
							if (draggingInside != _draggingInside)
							{
								// Generate appropriate event
								if (draggingInside)
								{
									// Put the page back again
									_pageSelected = _mouseDragIndex;
									TabPages.Insert(_mouseDragIndex, _dragPage);

									OnPageDragQuit(e);

									// Show change (but not refresh straight away)
									Recalculate();
									Invalidate();
								}
								else
								{
									// Kill any user of the scroll timer
									try
									{
										_scrollTimer.Stop();
									} 
									catch {}

									// Generate event before removing the page
									OnPageDragStart(_dragPage, e);

									// Remove the dragging page from view
									_pageSelected = -1;
									TabPages.RemoveAt(_mouseDragIndex);

									// Show change straight away
									Recalculate();
									Refresh();
								}

								// Remember new state
								_draggingInside = draggingInside;
							}
							else
							{
								// No change in state, but are we outside?
								if (!_draggingInside)
									OnPageDragMove(e);
							}
						}
					}
					else
					{
						OnPageDragMove(e);
					}
                }
            }
            else
            {
                if (_hotTrack || _hoverSelect)
                {
                    int mousePage = -1;
                    bool pageChanged = false;

                    // Create a point representing current mouse position
                    Point mousePos = mousePt;

                    // Find the page this mouse point is inside
                    for(int pos=0; pos<_tabPages.Count; pos++)
                    {
                        Rectangle rect = (Rectangle)_tabRects[pos];

                        if (rect.Contains(mousePos))
                        {
                            mousePage = pos;
                            break;
                        }
                    }

                    // Should moving over a tab cause selection changes?
                    if (_hoverSelect && !_multiline && (mousePage != -1))
                    {
                        // Has the selected page changed?
                        if (mousePage != _pageSelected)
                        {
                            // Move selection to new page
                            MovePageSelection(_tabPages[mousePage]);

                            pageChanged = true;
                        }
                    }

                    if (_hotTrack && !pageChanged && (mousePage != _hotTrackPage))
                    {
                        if (IsMediaPlayerStyle || (IsOfficeStyle && (_style != VisualStyle.Office2003)))
                        {
                            _hotTrackPage = mousePage;
                            Invalidate();
                        }
                        else
                        {
                            Graphics g = this.CreateGraphics();

                            // Clip the drawing to prevent drawing in unwanted areas
                            ClipDrawingTabs(g);

                            // Remove highlight of old page
                            if ((_hotTrackPage != -1) && (_hotTrackPage < _tabPages.Count))
                                DrawTab(_tabPages[_hotTrackPage], g, false);

                            _hotTrackPage = mousePage;

                            // Add highlight to new page
                            if ((_hotTrackPage != -1) && (_hotTrackPage < _tabPages.Count))
                                DrawTab(_tabPages[_hotTrackPage], g, true);

                            // Must correctly release resource
                            g.Dispose();
                        }
                    }
                }
            }

            base.OnMouseMove(e);
        }

		/// <summary>
		/// Raises the MouseEnter event.
		/// </summary>
		/// <param name="e">An EventArgs structure containing the event data.</param>
        protected override void OnMouseEnter(EventArgs e)
        {
			try
			{
                _mouseMovePt = new Point(int.MaxValue, int.MaxValue);
				_mouseOver = true;
				_overTimer.Stop();
	            
				base.OnMouseEnter(e);
			}
			catch(Exception)
			{
			}
		}

		/// <summary>
		/// Raises the MouseLeave event.
		/// </summary>
		/// <param name="e">An EventArgs structure containing the event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
			// Mouse has left control so do not start drag until next mouse down
			_ignoreDownDrag = true;

			// Kill any showing tooltip
			KillToolTip();
			
			if (_hotTrack)
            {
                int newTrackPage = -1;

                if (newTrackPage != _hotTrackPage)
                {
                    if (IsMediaPlayerStyle || (IsOfficeStyle && (_style != VisualStyle.Office2003)))
                    {
                        _hotTrackPage = newTrackPage;
                        Invalidate();
                    }
                    else
                    {
                        Graphics g = this.CreateGraphics();

                        // Clip the drawing to prevent drawing in unwanted areas
                        ClipDrawingTabs(g);

                        // Remove highlight of old page
                        if ((_hotTrackPage != -1) && (_hotTrackPage < _tabPages.Count))
                            DrawTab(_tabPages[_hotTrackPage], g, false);

                        _hotTrackPage = newTrackPage;

                        // Must correctly release resource
                        g.Dispose();
                    }
                }
            }

			try
			{
				_overTimer.Start();
			} catch {}

            base.OnMouseLeave(e);
        }		

		/// <summary>
		/// Raises the DragEnter event.
		/// </summary>
		/// <param name="e">A DragEventArgs structure containing the event data.</param>
		protected override void OnDragEnter(DragEventArgs e)
		{
			// Default to an impossible position
			_mouseDragOver = new Point(-1, -1);

			base.OnDragEnter(e);
		}

		/// <summary>
		/// Raises the DragOver event.
		/// </summary>
		/// <param name="e">A DragEventArgs structure containing the event data.</param>
		protected override void OnDragOver(DragEventArgs e)
		{
			// Allow to select pages on drag over?
			if (_dragOverSelect)
			{
				// Remember mouse position for when timer expires
				Point mouseDragOver = PointToClient(new Point(e.X, e.Y));

				// If a new position has been found then restart timer
				if (mouseDragOver != _mouseDragOver)
				{
					// Use the new position
					_mouseDragOver = mouseDragOver;

					try
					{
						// Stop if already running
						_dragTimer.Stop();

						// Start running from beginning
						_dragTimer.Start();
					} 
					catch {}
				}
			}

			base.OnDragOver(e);
		}

		/// <summary>
		/// Raises the DragLeave event.
		/// </summary>
		/// <param name="e">An EventArgs structure containing the event data.</param>
		protected override void OnDragLeave(EventArgs e)
		{
			// Stop if already running
			try
			{
				_dragTimer.Stop();
			} 
			catch {}

			base.OnDragLeave(e);
		}

		/// <summary>
		/// Raises the GotFocus event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnGotFocus(EventArgs e)
		{
			TabPage selectedPage = SelectedTab;

			// If we have a selected page
			if (selectedPage != null)
			{
				// If the selected page has a requested focus target
				if (selectedPage.StartFocus != null)
					selectedPage.StartFocus.Focus();
				else
				{
					// Otherwise just push focus into the selected page
					if (selectedPage.Control != null)
						selectedPage.Control.Focus();
					else
						selectedPage.Focus();
				}
			}
			else
				base.OnGotFocus(e);
		}

		/// <summary>
		/// Process a change in user preferences.
		/// </summary>
		/// <param name="sender">Object generating event.</param>
		/// <param name="e">A UserPreferenceChangedEventArgs structure containing the event data.</param>
		protected virtual void OnPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			RedefineSystemFont();
			DefineButtonImages();
			Recalculate();
			Invalidate();

			// Must retest for the correct theme
			_colorDetails.Reset();
		}

		/// <summary>
		/// Process a change in system colors.
		/// </summary>
		/// <param name="e">An EventArgs structure containing the event data.</param>
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			// If still using the Default color when we were created
			if (_defaultColor)
				DefineBackColor(TabControl.DefaultBackColor);

			DefineButtonImages();
			Recalculate();
			Invalidate();

			base.OnSystemColorsChanged(e);
		}

        /// <summary>
        /// Raise the TooltipDisplay event.
        /// </summary>
        /// <param name="e">An TooltipDisplayEventArgs that contains the event data.</param>
        protected virtual void OnTooltipDisplay(TooltipDisplayEventArgs e)
        {
            // Has anyone registered for the event?
            if (TooltipDisplay != null)
                TooltipDisplay(this, e);
        }

		/// <summary>
        /// Raises the ContextMenuStripDisplay event.
		/// </summary>
		/// <param name="e">A CancelEventArgs structure containing the event data.</param>
        protected virtual void OnContextMenuStripDisplay(CancelEventArgs e)
		{
			// Has anyone registered for the event?
            if (ContextMenuStripDisplay != null)
                ContextMenuStripDisplay(this, e);
		}

		/// <summary>
		/// Raises the SelectionChanging event.
		/// </summary>
		/// <param name="oldPage">Reference to currently selected page.</param>
		/// <param name="newPage">Reference to page that will become selected.</param>
		protected virtual void OnSelectionChanging(TabPage oldPage, TabPage newPage)
		{
			// Has anyone registered for the event?
			if (SelectionChanging != null)
				SelectionChanging(this, oldPage, newPage);
		}

		/// <summary>
		/// Raises the SelectionChanged event.
		/// </summary>
		/// <param name="oldPage">Reference to page that used to be selected.</param>
		/// <param name="newPage">Reference to currently selected page.</param>
		protected virtual void OnSelectionChanged(TabPage oldPage, TabPage newPage)
		{
			// Has anyone registered for the event?
			if (SelectionChanged != null)
				SelectionChanged(this, oldPage, newPage);
		}

		/// <summary>
		/// Raises the ClosePressed event.
		/// </summary>
		/// <param name="e">An EventArgs structure containing the event data.</param>
		protected virtual void OnClosePressed(EventArgs e)
		{
			// Has anyone registered for the event?
			if (ClosePressed != null)
				ClosePressed(this, e);
		}

		/// <summary>
		/// Raises the PageGotFocus event.
		/// </summary>
		/// <param name="e">An EventArgs structure containing the event data.</param>
		protected virtual void OnPageGotFocus(EventArgs e)
		{
			// Has anyone registered for the event?
			if (PageGotFocus != null)
				PageGotFocus(this, e);
		}
		
		/// <summary>
		/// Raises the PageLostFocus event.
		/// </summary>
		/// <param name="e">An EventArgs structure containing the event data.</param>
		protected virtual void OnPageLostFocus(EventArgs e)
		{
			// Has anyone registered for the event?
			if (PageLostFocus != null)
				PageLostFocus(this, e);
		}

		/// <summary>
		/// Raises the PageMoved event.
		/// </summary>
		/// <param name="page">Page that has been repositioned.</param>
		/// <param name="newIndex">New index in collection of pages.</param>
		protected virtual void OnPageMoved(TabPage page, int newIndex)
		{
			// Has anyone registered for the event?
			if (PageMoved != null)
				PageMoved(this, page, newIndex);
		}

		/// <summary>
		/// Raises the PageDragStart event.
		/// </summary>
		/// <param name="movePage">Page that is being dragged outside.</param>
		/// <param name="mea">A MouseEventArgs structure containing the event data.</param>
		protected virtual void OnPageDragStart(TabPage movePage, MouseEventArgs mea)
		{
			// Allowed to generate drag outside events?
			if (DragOutside)
			{
				// Has anyone registered for the event?
				if (PageDragStart != null)
					PageDragStart(this, movePage, mea);
			}
		}

		/// <summary>
		/// Raises the PageDragMove event.
		/// </summary>
		/// <param name="e">A MouseEventArgs structure containing the event data.</param>
		protected virtual void OnPageDragMove(MouseEventArgs e)
		{
			// Allowed to generate drag outside events?
			if (DragOutside)
			{
				// Has anyone registered for the event?
				if (PageDragMove != null)
					PageDragMove(this, e);
			}
		}

		/// <summary>
		/// Raises the PageDragEnd event.
		/// </summary>
		/// <param name="e">A MouseEventArgs structure containing the event data.</param>
		protected virtual void OnPageDragEnd(MouseEventArgs e)
		{
			// Allowed to generate drag outside events?
			if (DragOutside)
			{
				// Has anyone registered for the event?
				if (PageDragEnd != null)
					PageDragEnd(this, e);
			}
		}

		/// <summary>
		/// Raises the PageDragQuit event.
		/// </summary>
		/// <param name="e">A MouseEventArgs structure containing the event data.</param>
		protected virtual void OnPageDragQuit(MouseEventArgs e)
		{
			// Allowed to generate drag outside events?
			if (DragOutside)
			{
				// Has anyone registered for the event?
				if (PageDragQuit != null)
					PageDragQuit(this, e);
			}
		}
        
		/// <summary>
		/// Raises the DoubleClickTab event.
		/// </summary>
		/// <param name="page">TabPage generating event.</param>
		protected virtual void OnDoubleClickTab(TabPage page)
		{
			// Has anyone registered for the event?
			if (DoubleClickTab != null)
				DoubleClickTab(this, page);
		}

		internal bool AllowSideCaptions
		{
			get { return _allowSideCaptions; }
			set { _allowSideCaptions = value; }
		}

		internal ColorDetails ColorDetails
		{
			get { return _colorDetails; }
		}

		private void RedefineSystemFont()
		{
			// Are we using the default menu or a user defined value?
			if (_defaultFont)
				DefineFont(new Font(SystemInformation.MenuFont, FontStyle.Regular));
		}
	
		private void ResumeProcess()
		{
			// Must be currently suspended
			if (_suspended)
			{
				// Allow normal operation
				_suspended = false;

				// If no page was explicitly selected
				if (_pageSelected == -1)
				{
					// If there are any pages
					if (_tabPages.Count > 0)
					{
						// Pretend nothing is currently selected
						_pageSelected = -1;

						// Then select the first one
						SelectedIndex = 0;
					}
				}
				else
				{
					int newPage = _pageSelected;

					// Bound check the index
					if (newPage > (_tabPages.Count - 1))
						newPage = _tabPages.Count - 1;

					// Pretend nothing is currently selected
					_pageSelected = -1;

					// Select the required page
					SelectedIndex = newPage;
				}

				Recalculate();
				Invalidate();
			}
		}

		private void DefineFont(Font newFont)
		{
			// Use base class for storage of value
			base.Font = newFont;
	
			// Update internal height value using Font
			_textHeight = newFont.Height;

			// Is the font height bigger than the image height?
			if (_imageHeight >= _textHeight)
			{
				// No, do not need extra spacing around the image to fit in text
				_imageGapTopExtra = 0;
				_imageGapBottomExtra = 0;
			}
			else
			{
				// Yes, need to make the image area bigger so that its height calculation
				// matchs that height of the text
				int extraHeight = _textHeight - _imageHeight;

				// Split the extra height between the top and bottom of image
				_imageGapTopExtra = extraHeight / 2;
				_imageGapBottomExtra = extraHeight - _imageGapTopExtra;
			}
		}

		private void DefineButtonImages()
		{
			// Dispose of an existing attributes usage
			if (_ia != null)
			{
				_leftArrow.ImageAttributes = null;
				_rightArrow.ImageAttributes = null;
				_closeButton.ImageAttributes = null;
				_activeArrow.ImageAttributes = null;
				_ia.Dispose();
				_ia = null;
			}

            // Dispose of an existing attributes usage
            if (_iao != null)
            {
                _leftArrow.ImageAttributesOver = null;
                _rightArrow.ImageAttributesOver = null;
                _closeButton.ImageAttributesOver = null;
                _activeArrow.ImageAttributesOver = null;
                _iao.Dispose();
                _iao = null;
            }
            
            _ia = new ImageAttributes();
            _iao = new ImageAttributes();

			ColorMap activeMap = new ColorMap();
            ColorMap activeMapOver = new ColorMap();
            ColorMap inactiveMap = new ColorMap();
            ColorMap inactiveMapOver = new ColorMap();

			// Define the color transformations needed
			activeMap.OldColor = Color.Black;
            activeMapOver.OldColor = Color.Black;
            inactiveMap.OldColor = Color.White;
            inactiveMapOver.OldColor = Color.White;

            if (_defaultColor)
            {
                switch (_style)
                {
                    case VisualStyle.IDE2005:
                        activeMap.NewColor = SystemColors.ControlText;
                        activeMapOver.NewColor = activeMap.NewColor;
                        inactiveMap.NewColor = SystemColors.GrayText;
                        inactiveMapOver.NewColor = inactiveMap.NewColor;
                        break;
                    case VisualStyle.Office2003:
                        activeMap.NewColor = _colorDetails.ActiveTabButtonColor;
                        activeMapOver.NewColor = activeMap.NewColor;
                        inactiveMap.NewColor = _colorDetails.MenuSeparatorColor;
                        inactiveMapOver.NewColor = inactiveMap.NewColor;
                        break;
                    case VisualStyle.Office2007Blue:
                    case VisualStyle.Office2007Silver:
                    case VisualStyle.Office2007Black:
                        activeMap.NewColor = Office2007ColorTable.TabButtonColor(_style, _officeStyle);
                        activeMapOver.NewColor = activeMap.NewColor;
                        inactiveMap.NewColor = Color.FromArgb(128, activeMap.NewColor);
                        inactiveMapOver.NewColor = inactiveMap.NewColor;
                        break;
                    case VisualStyle.MediaPlayerBlue:
                    case VisualStyle.MediaPlayerOrange:
                    case VisualStyle.MediaPlayerPurple:
                        activeMap.NewColor = MediaPlayerColorTable.TabButtonColor(_style, _mediaPlayerStyle);
                        activeMapOver.NewColor = Color.White;
                        inactiveMap.NewColor = Color.FromArgb(128, activeMap.NewColor);
                        inactiveMapOver.NewColor = inactiveMap.NewColor;
                        break;
                    default:
                        activeMap.NewColor = _buttonActiveColor;
                        activeMapOver.NewColor = activeMap.NewColor;
                        inactiveMap.NewColor = _buttonInactiveColor;
                        inactiveMapOver.NewColor = inactiveMap.NewColor;
                        break;
                }
            }
            else
            {
                activeMap.NewColor = _buttonActiveColor;
                activeMapOver.NewColor = activeMap.NewColor;
                inactiveMap.NewColor = _buttonInactiveColor;
                inactiveMapOver.NewColor = inactiveMap.NewColor;
            }

			// Create remap attributes for use by button
			_ia.SetRemapTable(new ColorMap[]{ activeMap, inactiveMap }, ColorAdjustType.Bitmap);
            _iao.SetRemapTable(new ColorMap[] { activeMapOver, inactiveMapOver }, ColorAdjustType.Bitmap);

			// Pass attributes to the buttons
			_leftArrow.ImageAttributes = _ia;
            _leftArrow.ImageAttributesOver = _iao;
            _rightArrow.ImageAttributes = _ia;
            _rightArrow.ImageAttributesOver = _iao;
            _closeButton.ImageAttributes = _ia;
            _closeButton.ImageAttributesOver = _iao;
            _activeArrow.ImageAttributes = _ia;
            _activeArrow.ImageAttributesOver = _iao;
        }

        private bool IsOfficeStyle
        {
            get
            {
                return (_style == VisualStyle.Office2003) ||
                       (_style == VisualStyle.Office2007Blue) ||
                       (_style == VisualStyle.Office2007Silver) ||
                       (_style == VisualStyle.Office2007Black);
            }
        }

        private bool IsMediaPlayerStyle
        {
            get
            {
                return (_style == VisualStyle.MediaPlayerBlue) ||
                       (_style == VisualStyle.MediaPlayerOrange) ||
                       (_style == VisualStyle.MediaPlayerPurple);
            }
        }
        
        private void SetAppearance(VisualAppearance appearance)
		{
			switch(appearance)
			{
				case VisualAppearance.MultiForm:
				case VisualAppearance.MultiBox:
					_shrinkPagesToFit = true;					// shrink tabs to fit width
					_positionAtTop = false;						// draw tabs at bottom of control
					_showClose = false;							// do not show the close button
					_showArrows = false;						// do not show the scroll arrow buttons
					_boldSelected = false;						// do not show selected pages in bold
					_idePixelArea = true;                       // show a one pixel border at top or bottom
					_idePixelBorder = false;                    // do not show a one pixel border round control in IDE style
					_officePixelBorder = true;                  // do not show a one pixel border round control in Office2003 style
                    _mediaPlayerPixelBorder = true;             // do not show a one pixel border round control in MediaPlayer style
                    _ide2005PixelBorder = true;                 // show a one pixel border round control in IDE2005 style
					break;
				case VisualAppearance.MultiDocument:
					_shrinkPagesToFit = false;					// shrink tabs to fit width
					_positionAtTop = true;						// draw tabs at bottom of control
					_showClose = true;							// do not show the close button
					_showArrows = true;						    // do not show the scroll arrow buttons
					_boldSelected = true;						// do not show selected pages in bold
					_idePixelArea = true;                       // show a one pixel border at top or bottom
					_idePixelBorder = false;                    // do not show a one pixel border round control in IDE style
					_officePixelBorder = true;                  // show a one pixel border round control in Office2003 style
                    _mediaPlayerPixelBorder = true;             // do not show a one pixel border round control in MediaPlayer style
                    _ide2005PixelBorder = true;                 // show a one pixel border round control in IDE2005 style
					break;
			}

			// These properties are the same whichever style is selected
			_hotTrack = false;							// do not hot track pages
			_dimUnselected = true;						// draw dimmed non selected pages

			// Define then starting page for drawing
			if (_tabPages.Count > 0)
				_startPage = 0;
			else
				_startPage = -1;

			_appearance = appearance;

			// Define the correct style indexer
			SetStyleIndex();
		}

		private void SetStyleIndex()
		{
			switch(_appearance)
			{
				case VisualAppearance.MultiBox:
					// Always pretend we are plain style
					_styleIndex = 1;
					break;
				case VisualAppearance.MultiForm:
				case VisualAppearance.MultiDocument:
					_styleIndex = (int)_style;
					break;
			}
		}

		private bool HideTabsCalculation()
		{
			bool hideTabs = false;
	
			// Never hide the tabs area when dragging a tab header, this might cause the last tab 
			// header to be hidden temporarily and we do not want to hide the tabs in that case
			if (!_leftMouseDownDrag)
			{
				switch(_hideTabsMode)
				{
					case HideTabsModes.ShowAlways:
						hideTabs = false;
						break;
					case HideTabsModes.HideAlways:
						hideTabs = true;
						break;
					case HideTabsModes.HideUsingLogic:
						hideTabs = (_tabPages.Count <= 1);                            
						break;
					case HideTabsModes.HideWithoutMouse:
						hideTabs = !_mouseOver;
						break;
				}
			}
            
			return hideTabs;
		}

		private void OnPageMouseDown(object sender, MouseEventArgs e)
		{
			Control c = sender as Control;

			// If the mouse has been clicked and it does not have 
			// focus then it should receive the focus immediately.
			if (!c.ContainsFocus)
				c.Focus();
		}

		private void RecursiveMonitor(Control top, bool monitor)
		{
			if (monitor)
			{
				// Monitor children being added
				top.ControlAdded += new ControlEventHandler(OnAddMonitor);
				top.ControlRemoved += new ControlEventHandler(OnRemoveMonitor);
			}
			else
			{
				// Unmonitor children being removed
				top.ControlAdded -= new ControlEventHandler(OnAddMonitor);
				top.ControlRemoved -= new ControlEventHandler(OnRemoveMonitor);
			}

			foreach(Control c in top.Controls)
			{
				if (monitor)
				{
					// Monitor focus changes on the Control
					c.GotFocus += new EventHandler(OnPageEnter);
					c.LostFocus += new EventHandler(OnPageLeave);
					c.MouseEnter += new EventHandler(OnPageMouseEnter);
					c.MouseMove += new MouseEventHandler(OnPageMouseMove);
					c.MouseLeave += new EventHandler(OnPageMouseLeave);
				}
				else
				{
					// Unmonitor focus changes on the Control
					c.GotFocus -= new EventHandler(OnPageEnter);
					c.LostFocus -= new EventHandler(OnPageLeave);
					c.MouseEnter -= new EventHandler(OnPageMouseEnter);
					c.MouseMove -= new MouseEventHandler(OnPageMouseMove);
					c.MouseLeave -= new EventHandler(OnPageMouseLeave);
				}
				
				RecursiveMonitor(c, monitor);
			}
		}

		private void OnAddMonitor(object sender, ControlEventArgs e)
		{
			// Monitor focus changes on the Control
			e.Control.GotFocus += new EventHandler(OnPageEnter);
			e.Control.LostFocus += new EventHandler(OnPageLeave);
			e.Control.MouseEnter += new EventHandler(OnPageMouseEnter);
			e.Control.MouseMove += new MouseEventHandler(OnPageMouseMove);
			e.Control.MouseLeave += new EventHandler(OnPageMouseLeave);

			// Monitor the newly added control
			RecursiveMonitor(e.Control, true);
		}

		private void OnRemoveMonitor(object sender, ControlEventArgs e)
		{
			// Unmonitor focus changes on the Control
			e.Control.GotFocus -= new EventHandler(OnPageEnter);
			e.Control.LostFocus -= new EventHandler(OnPageLeave);
			e.Control.MouseEnter -= new EventHandler(OnPageMouseEnter);
			e.Control.MouseMove -= new MouseEventHandler(OnPageMouseMove);
			e.Control.MouseLeave -= new EventHandler(OnPageMouseLeave);

			// Unmonitor the removed control
			RecursiveMonitor(e.Control, false);
		}

		private void OnPageMouseEnter(object sender, EventArgs e)
		{
			try
			{
				// Is the mouse over area that should show tabs?
				if (IsMouseOverForTabs())
				{
					// Yes, mouse is over valid test area
					_mouseOver = true;

					// Stop any timer that is intended to remove tabs
					try
					{
						_overTimer.Stop();
					} 
					catch {}
				}
	            
				// If showing tabs dependant on mouse position, then update display
				if (_hideTabsMode == HideTabsModes.HideWithoutMouse)
				{
					Recalculate();
					Refresh();
				}
			}
			catch(Exception)
			{
			}
		}
	
		private void OnPageMouseMove(object sender, MouseEventArgs e)
		{
			try
			{
				// Is there a change in the test of them mouse?
				if (_mouseOver != IsMouseOverForTabs())
				{
					// Invert the mouse over to reflect new value
					_mouseOver = !_mouseOver;

					// If now inside the test area...
					if (_mouseOver)
					{
						// Stop any timer that is intended to remove tabs
						try
						{
							_overTimer.Stop();
						} 
						catch {}

						// If showing tabs dependant on mouse position, then update display
						if (_hideTabsMode == HideTabsModes.HideWithoutMouse)
						{
							Recalculate();
							Refresh();
						}
					}
					else
					{
						// Start the timer that is intended to remove tabs
						_overTimer.Start();
					}
				}
			}
			catch(Exception)
			{
			}
		}

		private void OnPageMouseLeave(object sender, EventArgs e)
		{
			try
			{
				// Mouse leaving page means we need to timeout to remove tabs
				_overTimer.Start();
			}
			catch(Exception)
			{
			}
		}

		private bool IsMouseOverForTabs()
		{
			// Get the screen bounding rectangle for the whole control
			Rectangle rect = RectangleToScreen(ClientRectangle);

			// Is the mouse vector to be applied?
			if (WithoutMouseVector >= 0)
			{
				// Are tabs at top of the control?
				if (PositionTop)
				{
					// Shrink the height to the vector
					rect.Height = WithoutMouseVector;
				}
				else
				{
					// Reduce to vector height from bottom
					rect.Y = rect.Bottom - WithoutMouseVector;
					rect.Height = WithoutMouseVector;
				}
			}

			// Return if the mouse is in the test are
			return rect.Contains(Control.MousePosition);
		}

		private void OnMouseTick(object sender, EventArgs e)
		{
			try
			{
				_mouseOver = false;
				_overTimer.Stop();
	            
				if (_hideTabsMode == HideTabsModes.HideWithoutMouse)
				{
					Recalculate();
					Invalidate();
				}
			}
			catch(Exception)
			{
			}
		}

		private void OnDragOverTick(object sender, EventArgs e)
		{
			try
			{
				_dragTimer.Stop();

				// Allow to select pages on drag over?
				if (_dragOverSelect)
				{
					// Find any tab header the mouse is over
					TabPage mouseTab = TabPageFromPoint(_mouseDragOver);

					// Was a tab found?
					if (mouseTab != null)
					{
						// Switch to this tab
						if (!mouseTab.Selected)
							mouseTab.Selected = true;
					}
				}
			}
			catch(Exception)
			{
			}
		}

		private void OnMouseHoverTick(object sender, EventArgs e)
		{
			try
			{
				// Only stop the timer if it is running
				if (_hoverTimer.Enabled)
					_hoverTimer.Stop();

				// Find any tab header the mouse is over
				TabPage hoverTab = TabPageFromPoint(PointToClient(Control.MousePosition));

				// Was a tab found?
				if (hoverTab != null)
				{
					// Is no button is being pressed and not dragging a page
					if (!_leftMouseDown && !_leftMouseDownDrag && !_popUpMenuShowing)
					{
						if (_toolTips)
							ShowTooltip(hoverTab);
						else
						{
							if (_textTips)
							{
								// Need a graphic in order to measure ideal size of the page
								using(Graphics g = this.CreateGraphics())
								{
									// Find the actual rectangle used to draw the tab page
									Rectangle rect = (Rectangle)_tabRects[TabPages.IndexOf(hoverTab)];
									
									// Find the the non text parts to leave the actual text area left over
									int extras = _position[_styleIndex, (int)PositionIndex.BorderLeft] + 
												_position[_styleIndex, (int)PositionIndex.BorderRight];

									// Any icon or image provided?
									if ((hoverTab.Icon != null) || (hoverTab.Image != null) || (((_imageList != null) || (hoverTab.ImageList != null)) && (hoverTab.ImageIndex != -1)))
									{
										extras += _position[_styleIndex, (int)PositionIndex.ImageGapLeft] +
															_imageWidth + 
															_position[_styleIndex, (int)PositionIndex.ImageGapRight];
									}
									
									extras += _position[_styleIndex, (int)PositionIndex.TextGapLeft];								
									
									// Subtract to leave the pure text area
									int textWidth = rect.Width - extras;
									
									Font drawFont = base.Font;

									if (_boldSelected && hoverTab.Selected)
										drawFont = new Font(drawFont, FontStyle.Bold);

									// Find actual width needed to draw the string
									int requiredWidth = (int)g.MeasureString(hoverTab.Title, drawFont, int.MaxValue, StringFormat.GenericTypographic).Width;
									
									// If not enough room to draw all the text
									if (textWidth < requiredWidth)
										ShowTooltip(hoverTab);
								}
							}
						}
					}
				}
			}
			catch(Exception)
			{
			}
		}
		
		private void ShowTooltip(TabPage page)
		{
			// Remember starting mouse position
			_tooltipRect = new Rectangle(Control.MousePosition, Size.Empty);

			// Make twice as big as the double click region
			_tooltipRect.Inflate(SystemInformation.DoubleClickSize.Width * 2,
									SystemInformation.DoubleClickSize.Height * 2);

			// Move left and up to center at mouse point
			_tooltipRect.Offset(-SystemInformation.DoubleClickSize.Width,
								-SystemInformation.DoubleClickSize.Height);

			// Create a tooltip control
			_toolTip = new PopupTooltipSingle(_style);
            _toolTip.Apply2007ClearType = Apply2007ClearType;
            _toolTip.ApplyMediaPlayerClearType = ApplyMediaPlayerClearType;

            // Create event arguments for getting the tooltip text to use
            TooltipDisplayEventArgs e = new TooltipDisplayEventArgs(page);
            OnTooltipDisplay(e);

            // If allowed to continue with showing the tooltip
            if (!e.Cancel)
            {
                // Define string for display
                _toolTip.ToolText = e.Tooltip;

                // Setup its position
                _toolTip.ShowWithoutFocus(new Point(Control.MousePosition.X, Control.MousePosition.Y + 24));
            }
		}

		private void OnScrollTick(object sender, EventArgs e)
		{
			try
			{
                if (_scrollingLeft)
				{
					// Scroll left one place
					_startPage--;
					_mouseDragIndex--;

					// Move the page
					_pageSelected = _mouseDragIndex;
					TabPages.RemoveAt(_mouseDragIndex+1);
					TabPages.Insert(_mouseDragIndex, _dragPage);

					// Need to recalculate based on new starting page
					Recalculate();

					// Calculate correct new scroll button states				
					_leftScroll = (_startPage > 0);

					// Find last possible drawing position for tabs
					int xEnd = GetMaximumDrawPos();
					
					// Find last pages drawing rectangle
					Rectangle rect = (Rectangle)_tabRects[_tabPages.Count - 1];
					
					// Determine if the right scrolling button should be enabled
					_rightScroll = (rect.Right > xEnd);

                    // Set correct arrow buttons states				
					if (_leftArrow.Enabled != _leftScroll)
					{
						_leftArrow.Enabled = _leftScroll;

						if (_leftScroll)
							_leftArrow.Image = _internalImages.Images[(int)ImageStrip.LeftEnabled];
						else
							_leftArrow.Image = _internalImages.Images[(int)ImageStrip.LeftDisabled];
                    }

					if (_rightArrow.Enabled != _rightScroll)
					{
						_rightArrow.Enabled = _rightScroll;

						if (_rightScroll)
							_rightArrow.Image = _internalImages.Images[(int)ImageStrip.RightEnabled];
						else
							_rightArrow.Image = _internalImages.Images[(int)ImageStrip.RightDisabled];
                    }
					
					// If we are now at the start, do not need timer
					if (_startPage <= 0)
						_scrollTimer.Stop();

					// Request a repaint to show changes
					Refresh();
				}
				else
				{
					// Scroll right one place
					_startPage++;

					// Can the selected page me moved right one place?
					if (_mouseDragIndex < (_tabPages.Count - 1))
					{
						// Bump the count
						_mouseDragIndex++;

						// Move the page
						_pageSelected = _mouseDragIndex;
						TabPages.RemoveAt(_mouseDragIndex-1);
						TabPages.Insert(_mouseDragIndex, _dragPage);
					}

					// Need to recalculate based on new starting page
					Recalculate();
                    
                    // Calculate correct new scroll button states				
					_leftScroll = (_startPage > 0);

					// Find last possible drawing position for tabs
					int xEnd = GetMaximumDrawPos();
					
					// Find last pages drawing rectangle
					Rectangle rect = (Rectangle)_tabRects[_tabPages.Count - 1];
					
					// Determine if the right scrolling button should be enabled
					_rightScroll = (rect.Right > xEnd);

                    // Set correct arrow buttons states				
					if (_leftArrow.Enabled != _leftScroll)
					{
						_leftArrow.Enabled = _leftScroll;

						if (_leftScroll)
							_leftArrow.Image = _internalImages.Images[(int)ImageStrip.LeftEnabled];
						else
							_leftArrow.Image = _internalImages.Images[(int)ImageStrip.LeftDisabled];
                    }

					if (_rightArrow.Enabled != _rightScroll)
					{
						_rightArrow.Enabled = _rightScroll;

                        if (_rightScroll)
                            _rightArrow.Image = _internalImages.Images[(int)ImageStrip.RightEnabled];
						else
                            _rightArrow.Image = _internalImages.Images[(int)ImageStrip.RightDisabled];
                    }
					
					// If we now longer need right scroll arrow, stop timer
					if (!_rightScroll)
						_scrollTimer.Stop();
					
					// Request a repaint to show changes
					Refresh();
				}
			}
			catch(Exception)
			{
			}
		}
		
		private void OnPagePropertyChanged(TabPage page, TabPage.Property prop, object oldValue)
		{
			switch(prop)
			{
				case TabPage.Property.Control:
					Control pageControl = oldValue as Control;

					// Remove the page or the page control as appropriate
					if (pageControl != null)
						RemoveTabPageControl(pageControl);
					else
						RemoveTabPagePanel(page);

					// Add the appropriate Control/Form/TabPage to the control
					AddTabPage(page);

					// Is a page currently selected?
					if (_pageSelected != -1)
					{
						// Is the change in Control for this page?
						if (page == _tabPages[_pageSelected])
							SelectPage(page);   // make Control visible
					}

					Recalculate();
					Invalidate();
					break;
				case TabPage.Property.SelectBackColor:
				case TabPage.Property.SelectTextColor:
				case TabPage.Property.SelectTextBackColor:
				case TabPage.Property.InactiveBackColor:
				case TabPage.Property.InactiveTextColor:
				case TabPage.Property.InactiveTextBackColor:
					// Do not need to recalculate, just redraw using the new colors
					Invalidate();
					break;
				case TabPage.Property.Icon:
					bool needRecalc = true;

					// If the existence of an Icon has not changed...
					if ((oldValue != null) && (page.Icon != null))
					{
						Icon oldIcon = (Icon)oldValue;
						
						// and its the same size...
						if ((oldIcon.Size.Width == page.Icon.Width) &&
							(oldIcon.Size.Height == page.Icon.Height))
						{
							// then only need to redraw rather than recalculate as well						
							needRecalc = false;
						}
					}
																	     
					if (needRecalc)
						_recalculate = true;

					Invalidate();
					break;
				case TabPage.Property.Title:
				case TabPage.Property.Image:
				case TabPage.Property.ImageIndex:
				case TabPage.Property.ImageList:
					_recalculate = true;
					Invalidate();
					break;
				case TabPage.Property.Selected:
					// Becoming selected?
					if (page.Selected)
					{
						// Move selection to the new page and update page properties
						MovePageSelection(page);
						MakePageVisible(page);
					}
					break;
			}
		}

		private Control FindFocus(Control root)
		{
			// Does the root control has focus?
			if (root.Focused)
				return root;

			// Check for focus at each child control
			foreach(Control c in root.Controls)
			{
				Control child = FindFocus(c);

				if (child != null)
					return child;
			}

			return null;
		}

		private bool ControlWantDoubleClick(IntPtr hWnd, Point mousePos, Control check)
		{
			// Cannot have double click if control not visible
			if (check.Visible)
			{
				// Is double click for this control?
				if (check.Enabled && (hWnd == check.Handle))
				{
					if (check == _activeArrow)
						OnActiveArrow(null, EventArgs.Empty);

					if (check == _leftArrow)
						OnLeftArrow(null, EventArgs.Empty);
	
					if (check == _rightArrow)
						OnRightArrow(null, EventArgs.Empty);
					
					return true;
				}
				else
				{
					// Create rectangle for control position
					Rectangle checkRect = new Rectangle(check.Location.X,
														check.Location.Y,
														check.Width,
														check.Height);

					// Was double click over a disabled button?
					if (checkRect.Contains(mousePos))
						return true;
				}
			}

			return false;
		}

		private bool ControlMouseTest(IntPtr hWnd, Point mousePos, Control check)
		{
			// Is the mouse down for the left arrow window and is it valid to click?
			if ((hWnd == check.Handle) && check.Visible && check.Enabled)
			{
				// Check if the mouse click is over the left arrow
				if (check.ClientRectangle.Contains(mousePos))
				{
					if (check == _activeArrow)
						OnActiveArrow(null, EventArgs.Empty);

					if (check == _leftArrow)
						OnLeftArrow(null, EventArgs.Empty);
	
					if (check == _rightArrow)
						OnRightArrow(null, EventArgs.Empty);

					return true;
				}
			}

			return false;
		}

		private void TestForKillingToolTip()
		{
			// Is there a tooltip?
			if (_toolTip != null)
			{
				// Is the current mouse pos outside the tooltip move area
				if (!_tooltipRect.Contains(Control.MousePosition))
					KillToolTip();
			}
			else
			{
				try
				{
					// Only stop the timer if it is running
                    if (_hoverTimer.Enabled)
                        _hoverTimer.Stop();

                    // Only start the timer if we are going to show a tooltip/texttip
                    if (_toolTips && _textTips)
                    {
                        Form f = FindForm();

                        // Only allow tooltip if the form we are inside is active
                        if ((f == null) || ((f != null) && f.ContainsFocus))
                            _hoverTimer.Start();
                    }
				} 
				catch {}
			}
		}

		private void KillToolTip()
		{
			// If we are showing a tooltip
			if (_toolTip != null)
			{
				try
				{
					// Only stop the timer if it is running
					if (_hoverTimer.Enabled)
						_hoverTimer.Stop();

					// Then kill it
					_toolTip.Close();
					_toolTip.Dispose();
					_toolTip = null;
				} 
				catch {}
			}
		}
	}

    /// <summary>
    /// Derived class to draw the background to match the caption.
    /// </summary>
    internal class TabButtonWithStyle : ButtonWithStyle
    {
		// Instance fields
		private TabControl _tabControl;
		
		/// <summary>
		/// Initialize a new instance of the TabButtonWithStyle class.
		/// </summary>
		/// <param name="tabControl">Creating instance.</param>
		public TabButtonWithStyle(TabControl tabControl)
		{
			// Remember back pointer
			_tabControl = tabControl;
		}
    
		/// <summary>
		/// Raises the PaintBackground event.
		/// </summary>
		/// <param name="pevent"></param>
		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			base.OnPaintBackground(pevent);
		}
    }

    /// <summary>
    /// Event arguments used when requesting a tool tip string.
    /// </summary>
    public class TooltipDisplayEventArgs : CancelEventArgs
    {
        // Instance Fields
        private TabPage _tabPage;
        private string _tooltip;

        /// <summary>
        /// Initialize a new instance of the TooltipDisplayEventArgs class.
        /// </summary>
        /// <param name="page">The tab page associated with the event.</param>
        public TooltipDisplayEventArgs(TabPage page)
        {
            _tabPage = page;
            _tooltip = page.ToolTip;
        }

        /// <summary>
        /// Gets the TabPage for which the tooltip is being shown.
        /// </summary>
        public TabPage TabPage
        {
            get { return _tabPage; }
        }

        /// <summary>
        /// Gets and sets the text to show inside the tooltip.
        /// </summary>
        public string Tooltip
        {
            get { return _tooltip; }
            set { _tooltip = value; }
        }
    }

	/// <summary>
	/// Specifies visual appearance of tab headers.
	/// </summary>
	public enum VisualAppearance
	{
		/// <summary>
		/// Specifies that tab headers are at the top with scrolling buttons.
		/// </summary>
		MultiDocument = 0,

		/// <summary>
		/// Specifies that tab headers are at the bottom.
		/// </summary>
		MultiForm = 1,

		/// <summary>
		/// Specifies that tab headers as boxes.
		/// </summary>
		MultiBox = 2
	}

	/// <summary>
	/// Specifies how tab headers are displayed.
	/// </summary>
	public enum HideTabsModes
	{
		/// <summary>
		/// Specifies that tab headers are always shown.
		/// </summary>
		ShowAlways,

		/// <summary>
		/// Specifies that tab headers are never shown.
		/// </summary>
		HideAlways,

		/// <summary>
		/// Specifies that tab headers are shown only when more than one page exists.
		/// </summary>
		HideUsingLogic,

		/// <summary>
		/// Specifies that tab headers are shown only when mouse is over area.
		/// </summary>
		HideWithoutMouse
	}
	
	/// <summary>
	/// Specifies how to draw the Office2003/Office2007 styles
	/// </summary>
	public enum OfficeStyle
	{
		/// <summary>
		/// Specifies the tab headers are white with a soft background.
		/// </summary>
		SoftWhite,

		/// <summary>
		/// Specifies the tab headers are white with a light background.
		/// </summary>
		LightWhite,

		/// <summary>
		/// Specifies the tab headers are white with a darker background.
		/// </summary>
		DarkWhite,

		/// <summary>
		/// Specifies the tab headers are in selected color with a soft background.
		/// </summary>
		SoftEnhanced,

		/// <summary>
		/// Specifies the tab headers are in selected color with a light background.
		/// </summary>
		LightEnhanced,
		
		/// <summary>
		/// Specifies the tab headers are in selected color with a darker background.
		/// </summary>
		DarkEnhanced,

		/// <summary>
		/// Specifies the tab headers are a light color with a dark background.
		/// </summary>
		Light,

		/// <summary>
		/// Specifies the tab headers are a dark color with a light background.
		/// </summary>
		Dark,
	}

    /// <summary>
    /// Specifies how to draw the MediaPlayer styles
    /// </summary>
    public enum MediaPlayerStyle
    {
        /// <summary>
        /// Specifies the tab headers are white with a soft background.
        /// </summary>
        SoftWhite,

        /// <summary>
        /// Specifies the tab headers are white with a light background.
        /// </summary>
        LightWhite,

        /// <summary>
        /// Specifies the tab headers are white with a darker background.
        /// </summary>
        DarkWhite,

        /// <summary>
        /// Specifies the tab headers are in selected color with a soft background.
        /// </summary>
        SoftEnhanced,

        /// <summary>
        /// Specifies the tab headers are in selected color with a light background.
        /// </summary>
        LightEnhanced,

        /// <summary>
        /// Specifies the tab headers are in selected color with a darker background.
        /// </summary>
        DarkEnhanced,

        /// <summary>
        /// Specifies the tab headers are a light color with a dark background.
        /// </summary>
        Light,

        /// <summary>
        /// Specifies the tab headers are a dark color with a light background.
        /// </summary>
        Dark,
    }
    
    /// <summary>
	/// Specifies how to draw the IDE2005 style
	/// </summary>
	public enum IDE2005Style
	{
		/// <summary>
		/// Specifies the standard appearance for the active tab.
		/// </summary>
		Standard,

		/// <summary>
		/// Specifies the standard appearance with darker background.
		/// </summary>
		StandardDark,

		/// <summary>
		/// Specifies the enhanced appearance for the active tab.
		/// </summary>
		Enhanced,

		/// <summary>
		/// Specifies the enhanced appearance with darker background.
		/// </summary>
		EnhancedDark,
	}

	/// <summary>
	/// Represents the method that will handle the DoubleClickTab event.
	/// </summary>
	public delegate void DoubleClickTabHandler(TabControl sender, TabPage page);

	/// <summary>
	/// Represents the method that will handle the SelectionChanging and SelectionChanged events.
	/// </summary>
	public delegate void SelectTabHandler(TabControl sender, TabPage oldPage, TabPage newPage);

	/// <summary>
	/// Represents the method that will handle the PageDragStart event.
	/// </summary>
	public delegate void PageDragStartHandler(TabControl sender, TabPage movePage, MouseEventArgs mea);

	/// <summary>
	/// Represents the method that will handle the PageDragQuit event.
	/// </summary>
	public delegate void PageDragHandler(TabControl sender, MouseEventArgs e);

	/// <summary>
	/// Represents the method that will handle the PageMoved event.
	/// </summary>
	public delegate void PageMovedHandler(TabControl sender, TabPage page, int newIndex);

    /// <summary>
    /// Represents the method that will handle the SelectionChanging and SelectionChanged events.
    /// </summary>
    public delegate void TooltipDisplayHandler(TabControl sender, TooltipDisplayEventArgs e);
}
