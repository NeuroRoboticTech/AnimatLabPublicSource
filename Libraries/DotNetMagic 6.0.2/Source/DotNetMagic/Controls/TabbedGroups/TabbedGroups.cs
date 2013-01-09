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
using System.Drawing.Text;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Provide a Tabbed MDI appearance to multiple tab pages.
	/// </summary>
    [ToolboxBitmap(typeof(TabbedGroups))]
    public class TabbedGroups : UserControl, ISupportInitialize
	{
		/// <summary>
		/// Provide access to dragging date.
		/// </summary>
	    public class DragProvider
	    {
			// Instance fields
            private object _tag;
            
			/// <summary>
			/// Initializes a new instance of the DragProvider class.
			/// </summary>
            public DragProvider()
            {
                _tag = null;
            }
            
			/// <summary>
			/// Initializes a new instance of the DragProvider class.
			/// </summary>
			/// <param name="tag">Initial tag to be used.</param>
            public DragProvider(object tag)
            {
                _tag = tag;
            }
            
			/// <summary>
			/// Gets or sets the Tag property.
			/// </summary>
            public object Tag
            {
                get { return _tag; }
                set { _tag = value; }
            }
	    }
	
	    // Instance fields
	    private int _numLeafs;
        private int _defMinWidth;
        private int _defMinHeight;
        private int _resizeBarVector;
        private int _suspendLeafCount;
		private int _suspendTabChange;
        private string _closeMenuText;
        private string _prominentMenuText;
        private string _rebalanceMenuText;
        private string _movePreviousMenuText;
        private string _moveNextMenuText;
        private string _newVerticalMenuText;
        private string _newHorizontalMenuText;
        private ImageList _imageList;
        private bool _dirty;
        private bool _autoCalculateDirty;
        private bool _saveControls;
        private bool _initializing;
        private bool _exiting;
        private bool _atLeastOneLeaf;
        private bool _autoCompact;
        private bool _compacting;
        private bool _resizeBarLock;
        private bool _layoutLock;
        private bool _pageCloseWhenEmpty;
		private bool _ignoreKeys;
		private bool _prominentOnly;
		private bool _prominentColors;
		private OfficeStyle _officeNormal;
		private OfficeStyle _officeSelected;
		private OfficeStyle _officeProminent;
        private MediaPlayerStyle _mediaPlayerNormal;
        private MediaPlayerStyle _mediaPlayerSelected;
        private MediaPlayerStyle _mediaPlayerProminent;
        private IDE2005Style _ide2005Normal;
		private IDE2005Style _ide2005Selected;
		private IDE2005Style _ide2005Prominent;
		private HotkeyPrefix _hotkeyPrefix;
        private Color _resizeBarColor;
		private Color _prominentBackColor;
		private Color _prominentForeColor;
        private Keys _closeShortcut;
        private Keys _prominentShortcut;
        private Keys _rebalanceShortcut;
        private Keys _movePreviousShortcut;
        private Keys _moveNextShortcut;
        private Keys _splitVerticalShortcut;
        private Keys _splitHorizontalShortcut;
        private CompactFlags _compactOptions;
        private DisplayTabModes _displayTabMode;
		private Controls.TabPage _activeTabPage;
        private TabGroupLeaf _prominentLeaf;
        private TabGroupLeaf _activeLeaf;
        private TabGroupSequence _root;
        private VisualStyle _style;
        private VisualStyle _externalStyle;
		private DragFeedbackStyle _feedbackStyle;
		private Type _dropType;

        /// <summary>
        /// Represents the method that will handle the TabControlType event.
        /// </summary>
        public delegate void TabControlTypeHandler(TabbedGroups tg, TGTabControlType e);
        
        /// <summary>
		/// Represents the method that will handle the TabControlCreated event.
		/// </summary>
	    public delegate void TabControlCreatedHandler(TabbedGroups tg, Controls.TabControl tc);
	    
		/// <summary>
		/// Represents the method that will handle the TabControlRemoved event.
		/// </summary>
		public delegate void TabControlRemovedHandler(TabbedGroups tg, Controls.TabControl tc);

		/// <summary>
		/// Represents the method that will handle the PageCloseRequest event.
		/// </summary>
		public delegate void PageCloseRequestHandler(TabbedGroups tg, TGCloseRequestEventArgs e);
        
		/// <summary>
		/// Represents the method that will handle the PageContextMenu event.
		/// </summary>
		public delegate void PageContextMenuHandler(TabbedGroups tg, TGContextMenuEventArgs e);
        
		/// <summary>
		/// Represents the method that will handle the GlobalSaving event.
		/// </summary>
		public delegate void GlobalSavingHandler(TabbedGroups tg, XmlTextWriter xmlOut);
        
		/// <summary>
		/// Represents the method that will handle the GlobalLoading event.
		/// </summary>
		public delegate void GlobalLoadingHandler(TabbedGroups tg, XmlTextReader xmlIn);
        
		/// <summary>
		/// Represents the method that will handle the PageSaving event.
		/// </summary>
		public delegate void PageSavingHandler(TabbedGroups tg, TGPageSavingEventArgs e);
        
		/// <summary>
		/// Represents the method that will handle the PageLoading event.
		/// </summary>
		public delegate void PageLoadingHandler(TabbedGroups tg, TGPageLoadingEventArgs e);

        /// <summary>
        /// Represents the method that will handle the ExternalDrag event.
        /// </summary>
		public delegate bool ExternalDragHandler(TabbedGroups tg, TabGroupLeaf tgl, Controls.TabControl tc, DragEventArgs e);

		/// <summary>
		/// Represents the method that will handle the ExternalDrop event.
		/// </summary>
		public delegate void ExternalDropHandler(TabbedGroups tg, TabGroupLeaf tgl, Controls.TabControl tc, DragProvider dp);
		
		/// <summary>
		/// Represents the method that will handle the ExternalDropType event.
		/// </summary>
		public delegate void ExternalDropTypeHandler(TabbedGroups tg, TabGroupLeaf tgl, Controls.TabControl tc, object data);

		/// <summary>
		/// Represents the method that will handle the ExternalDropRaw event.
		/// </summary>
		public delegate void ExternalDropRawHandler(TabbedGroups tg, TabGroupLeaf tgl, Controls.TabControl tc, DragEventArgs raw);

		/// <summary>
		/// Represents the method that will handle the PageChanged event.
		/// </summary>
		public delegate void PageChangeHandler(TabbedGroups tg, Controls.TabPage tp);

		/// <summary>
		/// Represents the method that will handle resizing finished events.
		/// </summary>
		public delegate void ResizeFinishedHandler(TabbedGroups tg, TabGroupSequence tgs);

		/// <summary>
		/// Represents the method that will handle resizing start events.
		/// </summary>
		public delegate void ResizeStartHandler(TabbedGroups tg, TabGroupSequence tgs, CancelEventArgs ce);

        /// <summary>
        /// Occurs when a new TabControl needs creating and requests the actual type to create.
        /// </summary>
        public event TabControlTypeHandler TabControlType;

        /// <summary>
		/// Occurs when a new TabControl has been created.
		/// </summary>
	    public event TabControlCreatedHandler TabControlCreated;
	    
		/// <summary>
		/// Occurs when a TabControl is being removed.
		/// </summary>
		public event TabControlRemovedHandler TabControlRemoved;

		/// <summary>
		/// Occurs when a request to close a TabPage is being made.
		/// </summary>
		public event PageCloseRequestHandler PageCloseRequest;

		/// <summary>
		/// Occurs when a context menu is about to be shown.
		/// </summary>
        public event PageContextMenuHandler PageContextMenu;

		/// <summary>
		/// Occurs during the saving of control state.
		/// </summary>
        public event GlobalSavingHandler GlobalSaving;

		/// <summary>
		/// Occurs during the loading of control state.
		/// </summary>
        public event GlobalLoadingHandler GlobalLoading;

		/// <summary>
		/// Occurs during the saving of each tab page.
		/// </summary>
        public event PageSavingHandler PageSaving;

		/// <summary>
		/// Occurs during the loading of each tab page.
		/// </summary>
        public event PageLoadingHandler PageLoading;

		/// <summary>
		/// Occurs when the prominent leaf changes.
		/// </summary>
        public event EventHandler ProminentLeafChanged;

		/// <summary>
		/// Occurs when the active leaf changes.
		/// </summary>
        public event EventHandler ActiveLeafChanged;

		/// <summary>
		/// Occurs when the dirty state changes.
		/// </summary>
        public event EventHandler DirtyChanged;

		/// <summary>
		/// Occurs when drag into tab header is occuring.
		/// </summary>
		public event ExternalDragHandler ExternalDragEnter;

		/// <summary>
		/// Occurs when drag over tab header is occuring.
		/// </summary>
		public event ExternalDragHandler ExternalDragOver;

		/// <summary>
		/// Occurs when an external drag drop of standard type occurs.
		/// </summary>
        public event ExternalDropHandler ExternalDrop;

		/// <summary>
		/// Occurs when an external drag drop of user specified type occurs.
		/// </summary>
		public event ExternalDropTypeHandler ExternalDropType;

		/// <summary>
		/// Occurs when an external drag drop of raw data occurs.
		/// </summary>
		public event ExternalDropRawHandler ExternalDropRaw;

		/// <summary>
		/// Occurs when the selected tab page changes.
		/// </summary>
		public event PageChangeHandler PageChanged;
	
		/// <summary>
		/// Occurs when a resize operation starts within a sequence.
		/// </summary>
		public event ResizeStartHandler ResizeStart;
	
		/// <summary>
		/// Occurs when a resize operation finishes within a sequence.
		/// </summary>
		public event ResizeFinishedHandler ResizeFinish;

		/// <summary>
		/// Initializes a new instance of the TabbedGroups class.
		/// </summary>
        public TabbedGroups()
        {
            InternalConstruct(VisualStyle.Office2007Blue);
        }
            
		/// <summary>
		/// Initializes a new instance of the TabbedGroups class.
		/// </summary>
		/// <param name="style">Define initial visual style.</param>
        public TabbedGroups(VisualStyle style)
		{
		    InternalConstruct(style);
        }
        
        private void InternalConstruct(VisualStyle style)
		{
			// NAG processing
			NAG.NAG_Start();
			
			// Prevent flicker with double buffering and all painting inside WM_PAINT
            SetStyle(ControlStyles.OptimizedDoubleBuffer | 
					 ControlStyles.AllPaintingInWmPaint |
					 ControlStyles.UserPaint |
					 ControlStyles.SupportsTransparentBackColor, true);
		
		    // We want to act as a drop target
		    this.AllowDrop = true;
		
		    // Remember parameters
            _externalStyle = style;
		    _style = ColorHelper.ValidateStyle(style);
		    
		    // Define initial state
		    _numLeafs = 0;
		    _compacting = false;
		    _initializing = false;
		    _exiting = false;
		    _suspendLeafCount = 0;
			_suspendTabChange = 0;
		    _activeTabPage = null;
			_feedbackStyle = DragFeedbackStyle.Squares;

            // Create the root sequence that always exists
            _root = new TabGroupSequence(this);
		    
            // Define default settings
		    ResetProminentLeaf();
		    ResetResizeBarVector();
		    ResetResizeBarColor();
			ResetProminentBackColor();
			ResetProminentForeColor();
			ResetProminentColors();
			ResetResizeBarLock();
            ResetLayoutLock();
            ResetCompactOptions();
		    ResetDefaultGroupMinimumWidth();
            ResetDefaultGroupMinimumHeight();
            ResetActiveLeaf();
            ResetAutoCompact();
            ResetAtLeastOneLeaf();
            ResetCloseMenuText();
            ResetProminentMenuText();
            ResetRebalanceMenuText();
            ResetMovePreviousMenuText();
            ResetMoveNextMenuText();
            ResetNewVerticalMenuText();
            ResetNewHorizontalMenuText();
            ResetCloseShortcut();
            ResetProminentShortcut();
            ResetRebalanceShortcut();
            ResetMovePreviousShortcut();
            ResetMoveNextShortcut();
            ResetSplitVerticalShortcut();
            ResetSplitHorizontalShortcut();
            ResetImageList();
            ResetDisplayTabMode();
            ResetSaveControls();
            ResetAutoCalculateDirty();
            ResetDirty();
            ResetPageCloseWhenEmpty();
			ResetIgnoreKeys();
			ResetProminentOnly();
			ResetDropType();
			ResetHotkeyPrefix();
			ResetOfficeStyleNormal();
			ResetOfficeStyleSelected();
			ResetOfficeStyleProminent();
            ResetMediaPlayerStyleNormal();
            ResetMediaPlayerStyleSelected();
            ResetMediaPlayerStyleProminent();
            ResetIDE2005StyleNormal();
			ResetIDE2005StyleSelected();
			ResetIDE2005StyleProminent();
		}
        
		/// <summary>
		/// Releases all resources used by the Control.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			_exiting = true;
			
			if (disposing)
			{
				if (RootSequence != null)
				{
					_root.Dispose();
					_root = null;
				}
			}

			base.Dispose(disposing);
		}
		
		/// <summary>
		/// Gets or sets the visual style for the control.
		/// </summary>
		[Category("TabbedGroups")]
        [DefaultValue(typeof(VisualStyle), "Office2007Blue")]
        public VisualStyle Style
        {
            get { return _externalStyle; }
            
            set
            {
                if (_externalStyle != value)
                {
                    _externalStyle = value;
                    _style = ColorHelper.ValidateStyle(value);
                    
                    // Propogate to all children
                    Notify(TabGroupBase.NotifyCode.StyleChanged);
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
		/// Gets the feedback style used when dragging.
		/// </summary>
		[Category("TabbedGroups")]
        [DefaultValue(typeof(DragFeedbackStyle), "Squares")]
		public DragFeedbackStyle FeedbackStyle
		{
			get { return _feedbackStyle; }
			set { _feedbackStyle = value; }
		}

		/// <summary>
		/// Resets the FeedbackStyle property to its default value.
		/// </summary>
        public void ResetFeedbackStyle()
        {
            FeedbackStyle = DragFeedbackStyle.Squares;
        }

		/// <summary>
		/// Gets or sets the drawing used for Office2003 style.
		/// </summary>
		[Category("TabbedGroups")]
		[DefaultValue(typeof(OfficeStyle), "SoftWhite")]
		public OfficeStyle OfficeStyleNormal
		{
			get { return _officeNormal; }
            
			set
			{
				if (_officeNormal != value)
				{   
					_officeNormal = value;
                    
					// Propogate to all children
					Notify(TabGroupBase.NotifyCode.OfficeStyleChanged);
				}
			}
		}

		/// <summary>
		/// Resets the OfficeStyleNormal property to its default value.
		/// </summary>
		public void ResetOfficeStyleNormal()
		{
			OfficeStyleNormal = OfficeStyle.SoftWhite;
		}
        
		/// <summary>
		/// Gets or sets the drawing used for Office2003 style.
		/// </summary>
		[Category("TabbedGroups")]
		[DefaultValue(typeof(OfficeStyle), "SoftEnhanced")]
		public OfficeStyle OfficeStyleSelected
		{
			get { return _officeSelected; }
            
			set
			{
				if (_officeSelected != value)
				{   
					_officeSelected = value;
                    
					// Propogate to all children
					Notify(TabGroupBase.NotifyCode.OfficeStyleChanged);
				}
			}
		}

		/// <summary>
		/// Resets the OfficeStyleSelected property to its default value.
		/// </summary>
		public void ResetOfficeStyleSelected()
		{
			OfficeStyleSelected = OfficeStyle.SoftEnhanced;
		}

		/// <summary>
		/// Gets or sets the drawing used for Office2003/Office2007 styles.
		/// </summary>
		[Category("TabbedGroups")]
		[DefaultValue(typeof(OfficeStyle), "DarkEnhanced")]
		public OfficeStyle OfficeStyleProminent
		{
			get { return _officeProminent; }
            
			set
			{
				if (_officeProminent != value)
				{   
					_officeProminent = value;
                    
					// Propogate to all children
					Notify(TabGroupBase.NotifyCode.OfficeStyleChanged);
				}
			}
		}

		/// <summary>
		/// Resets the OfficeStyleProminent property to its default value.
		/// </summary>
		public void ResetOfficeStyleProminent()
		{
			OfficeStyleProminent = OfficeStyle.DarkEnhanced;
		}

        /// <summary>
        /// Gets or sets the drawing used for Media Player style.
        /// </summary>
        [Category("TabbedGroups")]
        [DefaultValue(typeof(MediaPlayerStyle), "SoftWhite")]
        public MediaPlayerStyle MediaPlayerStyleNormal
        {
            get { return _mediaPlayerNormal; }

            set
            {
                if (_mediaPlayerNormal != value)
                {
                    _mediaPlayerNormal = value;

                    // Propogate to all children
                    Notify(TabGroupBase.NotifyCode.MediaPlayerStyleChanged);
                }
            }
        }

        /// <summary>
        /// Resets the MediaPlayerStyleNormal property to its default value.
        /// </summary>
        public void ResetMediaPlayerStyleNormal()
        {
            MediaPlayerStyleNormal = MediaPlayerStyle.SoftWhite;
        }

        /// <summary>
        /// Gets or sets the drawing used for Media Player style.
        /// </summary>
        [Category("TabbedGroups")]
        [DefaultValue(typeof(MediaPlayerStyle), "SoftEnhanced")]
        public MediaPlayerStyle MediaPlayerStyleSelected
        {
            get { return _mediaPlayerSelected; }

            set
            {
                if (_mediaPlayerSelected != value)
                {
                    _mediaPlayerSelected = value;

                    // Propogate to all children
                    Notify(TabGroupBase.NotifyCode.MediaPlayerStyleChanged);
                }
            }
        }

        /// <summary>
        /// Resets the MediaPlayerStyleSelected property to its default value.
        /// </summary>
        public void ResetMediaPlayerStyleSelected()
        {
            MediaPlayerStyleSelected = MediaPlayerStyle.SoftEnhanced;
        }

        /// <summary>
        /// Gets or sets the drawing used for Media Player styles.
        /// </summary>
        [Category("TabbedGroups")]
        [DefaultValue(typeof(MediaPlayerStyle), "DarkEnhanced")]
        public MediaPlayerStyle MediaPlayerStyleProminent
        {
            get { return _mediaPlayerProminent; }

            set
            {
                if (_mediaPlayerProminent != value)
                {
                    _mediaPlayerProminent = value;

                    // Propogate to all children
                    Notify(TabGroupBase.NotifyCode.MediaPlayerStyleChanged);
                }
            }
        }

        /// <summary>
        /// Resets the MediaPlayerStyleProminent property to its default value.
        /// </summary>
        public void ResetMediaPlayerStyleProminent()
        {
            MediaPlayerStyleProminent = MediaPlayerStyle.DarkEnhanced;
        }
        
        /// <summary>
		/// Gets or sets the drawing used for IDE2005 style.
		/// </summary>
		[Category("TabbedGroups")]
		[DefaultValue(typeof(IDE2005Style), "Standard")]
		public IDE2005Style IDE2005StyleNormal
		{
			get { return _ide2005Normal; }
            
			set
			{
				if (_ide2005Normal != value)
				{   
					_ide2005Normal = value;
                    
					// Propogate to all children
					Notify(TabGroupBase.NotifyCode.IDE2005StyleChanged);
				}
			}
		}

		/// <summary>
		/// Resets the IDE2005StyleNormal property to its default value.
		/// </summary>
		public void ResetIDE2005StyleNormal()
		{
			IDE2005StyleNormal = IDE2005Style.Standard;
		}
        
		/// <summary>
		/// Gets or sets the drawing used for IDE2005 style.
		/// </summary>
		[Category("TabbedGroups")]
		[DefaultValue(typeof(IDE2005Style), "Enhanced")]
		public IDE2005Style IDE2005StyleSelected
		{
			get { return _ide2005Selected; }
            
			set
			{
				if (_ide2005Selected != value)
				{   
					_ide2005Selected = value;
                    
					// Propogate to all children
					Notify(TabGroupBase.NotifyCode.IDE2005StyleChanged);
				}
			}
		}

		/// <summary>
		/// Resets the IDE2005StyleSelected property to its default value.
		/// </summary>
		public void ResetIDE2005StyleSelected()
		{
			IDE2005StyleSelected = IDE2005Style.Enhanced;
		}

		/// <summary>
		/// Gets or sets the drawing used for IDE2005 style.
		/// </summary>
		[Category("TabbedGroups")]
		[DefaultValue(typeof(IDE2005Style), "EnhancedDark")]
		public IDE2005Style IDE2005StyleProminent
		{
			get { return _ide2005Prominent; }
            
			set
			{
				if (_ide2005Prominent != value)
				{   
					_ide2005Prominent = value;
                    
					// Propogate to all children
					Notify(TabGroupBase.NotifyCode.IDE2005StyleChanged);
				}
			}
		}

		/// <summary>
		/// Resets the IDE2005StyleProminent property to its default value.
		/// </summary>
		public void ResetIDE2005StyleProminent()
		{
			IDE2005StyleProminent = IDE2005Style.EnhancedDark;
		}

		/// <summary>
		/// Gets the root sequence of contents.
		/// </summary>
        [Browsable(false)]
        public TabGroupSequence RootSequence
        {
            get { return _root; }
            
            set
            {
                // Only interested in a change in value
                if (value != null)
                {
                    // The incoming sequence must be empty
                    if (value.Count > 0)
                        throw new ApplicationException("New RootSequence must be empty");

                    // If there is an existing root sequence
                    if (_root != null)
                    {
                        bool compacting = _compacting;

                        // If not already compacting then use its flag to prevent the
                        // 'at least one leaf' mechanism from creating a new leaf
                        if (!compacting)
                            _compacting = true;

                        // Must remove the controls it has added as children
                        _root.Clear();

                        // Only turn off compacting flag if we turned it on
                        if (!compacting)
                            _compacting = false;
                    }

                    // Use the new root sequence
                    _root = value;

                    // Inform the root sequence to reposition itself
                    _root.Reposition();
                }
            }
        }

        /// <summary>
        /// Gets the root sequence of contents.
        /// </summary>
        [Category("TabbedGroups")]
        [DefaultValue(typeof(LayoutDirection), "Horizontal")]
        public LayoutDirection RootDirection
        {
            get { return _root.Direction; }
            
            set
            {
                // Update with the new direction
                _root.Direction = value;
            }
        }

        /// <summary>
        /// Reset the RootDirection property.
        /// </summary>
        public void ResetRootDirection()
        {
            RootDirection = LayoutDirection.Horizontal;
        }

		/// <summary>
		/// Gets or sets the vector used for the resize bars.
		/// </summary>
        [Category("TabbedGroups")]
        [DefaultValue(-1)]
        public int ResizeBarVector
        {
            get { return _resizeBarVector; }
            
            set
            {
                if (_resizeBarVector != value)
                {
                    _resizeBarVector = value;
                    
                    // Propogate to all children
                    Notify(TabGroupBase.NotifyCode.ResizeBarVectorChanged);
                }
            }
        }
        
		/// <summary>
		/// Resets the ResizeBarVector property to its default value.
		/// </summary>
        public void ResetResizeBarVector()
        {
            ResizeBarVector = -1;
        }
        
		/// <summary>
		/// Gets or sets the color used for the resize bars.
		/// </summary>
        [Category("TabbedGroups")]
        public Color ResizeBarColor
        {
            get { return _resizeBarColor; }
            
            set
            {
                if (!_resizeBarColor.Equals(value))
                {
                    _resizeBarColor = value;
                    
                    // Propogate to all children
                    Notify(TabGroupBase.NotifyCode.ResizeBarColorChanged);
                }
            }
        }

		/// <summary>
		/// Decide if the resize bar color needs to be persisted.
		/// </summary>
		/// <returns>Should color be persisted.</returns>
        protected bool ShouldSerializeResizeBackColor()
        {
            return _resizeBarColor != base.BackColor;
        }

		/// <summary>
		/// Resets the ResetResizeBarColor property to its default value.
		/// </summary>
		public void ResetResizeBarColor()
		{
			ResizeBarColor = base.BackColor;
		}

		/// <summary>
		/// Gets or sets the color used for the prominent background.
		/// </summary>
		[Category("TabbedGroups")]
		public Color ProminentBackColor
		{
			get { return _prominentBackColor; }
            
			set
			{
				if (!_prominentBackColor.Equals(value))
				{
					_prominentBackColor = value;
                    
					// Propogate to all children
					Notify(TabGroupBase.NotifyCode.ProminentBackColorChanged);
				}
			}
		}

		/// <summary>
		/// Decide if the prominent back color needs to be persisted.
		/// </summary>
		/// <returns>Should color be persisted.</returns>
		protected bool ShouldSerializeProminentBackColor()
		{
			return ProminentBackColor != SystemColors.ControlDark;
		}

		/// <summary>
		/// Resets the ProminentBackColor property to its default value.
		/// </summary>
        public void ResetProminentBackColor()
        {
            ProminentBackColor = SystemColors.ControlDark;
        }
        
		/// <summary>
		/// Gets or sets the color used for the prominent foreground.
		/// </summary>
		[Category("TabbedGroups")]
		public Color ProminentForeColor
		{
			get { return _prominentForeColor; }
            
			set
			{
				if (!_prominentForeColor.Equals(value))
				{
					_prominentForeColor = value;
                    
					// Propogate to all children
					Notify(TabGroupBase.NotifyCode.ProminentForeColorChanged);
				}
			}
		}

		/// <summary>
		/// Decide if the prominent foreground color needs to be persisted.
		/// </summary>
		/// <returns>Should color be persisted.</returns>
		protected bool ShouldSerializeProminentForeColor()
		{
			return ProminentForeColor != SystemColors.ControlLight;
		}

		/// <summary>
		/// Resets the ProminentForeColor property to its default value.
		/// </summary>
		public void ResetProminentForeColor()
		{
			ProminentForeColor = SystemColors.ControlLight;
		}

		/// <summary>
		/// Gets or sets the ability to resize using the bars.
		/// </summary>
        [Category("TabbedGroups")]
        [DefaultValue(false)]
        public bool ResizeBarLock
        {
            get { return _resizeBarLock; }
            set { _resizeBarLock = value; }
        }
        
		/// <summary>
		/// Resets the ResizeBarLock property to its default value.
		/// </summary>
        public void ResetResizeBarLock()
        {
            ResizeBarLock = false;
        }
        
		/// <summary>
		/// Gets or sets the ability to move pages around.
		/// </summary>
        [Category("TabbedGroups")]
        [DefaultValue(false)]
        public bool LayoutLock
        {
            get { return _layoutLock; }
            set { _layoutLock = value; }
        }
        
		/// <summary>
		/// Resets the LayoutLock property to its default value.
		/// </summary>
        public void ResetLayoutLock()
        {
            LayoutLock = false;
        }

		/// <summary>
		/// Gets or sets the leaf that is currently prominent.
		/// </summary>
        [Browsable(false)]
        public TabGroupLeaf ProminentLeaf
        {
            get { return _prominentLeaf; }
            
            set
            {
                if (_prominentLeaf != value)
                {
                    _prominentLeaf = value;

                    // Mark layout as dirty
                    if (_autoCalculateDirty)
                        _dirty = true;

                    // Propogate to all children
                    Notify(TabGroupBase.NotifyCode.ProminentChanged);
                    
                    OnProminentLeafChanged(EventArgs.Empty);
                }
            }
        }
        
		/// <summary>
		/// Resets the ProminentLeaf property to its default value.
		/// </summary>
		public void ResetProminentLeaf()
		{
			ProminentLeaf = null;
		}
        
		/// <summary>
		/// Gets or sets a value indicating if when prominent then only the prominent leaf is visible at all.
		/// </summary>
		[Category("TabbedGroups")]
		[DefaultValue(false)]
		public bool ProminentOnly
		{
			get { return _prominentOnly; }
			
			set 
			{ 
				if (_prominentOnly != value)
				{
					_prominentOnly = value;

					// If we are currently in prominent mode then reflect change immediately
					if (ProminentLeaf != null)
					{
						// Mark layout as dirty
						if (_autoCalculateDirty)
							_dirty = true;

						// Inform the root sequence to reposition itself
						_root.Reposition();
					}
				}
			}
		}
        
		/// <summary>
		/// Resets the ProminentOnly property to its default value.
		/// </summary>
		public void ResetProminentOnly()
		{
			ProminentOnly = false;
		}

		/// <summary>
		/// Gets or sets a value indicating if when prominent the tab control changes color.
		/// </summary>
		[Category("TabbedGroups")]
		[DefaultValue(true)]
		public bool ProminentColors
		{
			get { return _prominentColors; }
			set { _prominentColors = value; }
		}
        
		/// <summary>
		/// Resets the ProminentColors property to its default value.
		/// </summary>
		public void ResetProminentColors()
		{
			ProminentColors = true;
		}

		/// <summary>
		/// Gets or sets the compacting options.
		/// </summary>
        [Category("TabbedGroups")]
        [DefaultValue(typeof(CompactFlags), "All")]
        public CompactFlags CompactOptions
        {
            get { return _compactOptions; }
            set { _compactOptions = value; }
        }

		/// <summary>
		/// Resets the CompactOptions property to its default value.
		/// </summary>
        public void ResetCompactOptions()
        {
            CompactOptions = CompactFlags.All;
        }
        
		/// <summary>
		/// Gets or sets a user defined type allowed to be dropped on tab leaf headers.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(null)]
		public Type DropType
		{
			get { return _dropType; }
			set { _dropType = value; }
		}

		/// <summary>
		/// Resets the DropType property to its default value.
		/// </summary>
		public void ResetDropType()
		{
			DropType = null;
		}

		/// <summary>
		/// Gets or sets the minimum width a group is allowed to be.
		/// </summary>
        [Category("TabbedGroups")]
        [DefaultValue(4)]
        public int DefaultGroupMinimumWidth 
        {
            get { return _defMinWidth; }
            
            set
            {
                if (_defMinWidth != value)
                {
                    _defMinWidth = value;
                    
                    // Propogate to all children
                    Notify(TabGroupBase.NotifyCode.MinimumSizeChanged);
                }
            }
        }        
        
		/// <summary>
		/// Resets the DefaultGroupMinimumWidth property to its default value.
		/// </summary>
        public void ResetDefaultGroupMinimumWidth()
        {
            DefaultGroupMinimumWidth = 4;
        }
        
		/// <summary>
		/// Gets or sets the minimum height a group is allowed to be.
		/// </summary>
        [Category("TabbedGroups")]
        [DefaultValue(4)]
        public int DefaultGroupMinimumHeight
        {
            get { return _defMinHeight; }
            
            set
            {
                if (_defMinHeight != value)
                {
                    _defMinHeight = value;
                    
                    // Propogate to all children
                    Notify(TabGroupBase.NotifyCode.MinimumSizeChanged);
                }
            }
        }        
        
		/// <summary>
		/// Resets the DefaultGroupMinimumHeight property to its default value.
		/// </summary>
        public void ResetDefaultGroupMinimumHeight()
        {
            DefaultGroupMinimumHeight = 4;
        }

		/// <summary>
		/// Gets or sets the text used for the Close menu option.
		/// </summary>
        [Localizable(true)]
        [Category("Text String")]
        [DefaultValue("&Close")]
        public string CloseMenuText
        {
            get { return _closeMenuText; }
            set { _closeMenuText = value; }
        }
        
		/// <summary>
		/// Resets the CloseMenuText property to its default value.
		/// </summary>
        public void ResetCloseMenuText()
        {
            CloseMenuText = "&Close";
        }

		/// <summary>
		/// Gets or sets the text used for the Prominent menu option.
		/// </summary>
        [Localizable(true)]
        [Category("Text String")]
        [DefaultValue("Pro&minent")]
        public string ProminentMenuText
        {
            get { return _prominentMenuText; }
            set { _prominentMenuText = value; }
        }

		/// <summary>
		/// Resets the ProminentMenuText property to its default value.
		/// </summary>
        public void ResetProminentMenuText()
        {
            ProminentMenuText = "Pro&minent";
        }

		/// <summary>
		/// Gets or sets the text used for the Rebalance menu option.
		/// </summary>
        [Localizable(true)]
        [Category("Text String")]
        [DefaultValue("&Rebalance")]
        public string RebalanceMenuText
        {
            get { return _rebalanceMenuText; }
            set { _rebalanceMenuText = value; }
        }

		/// <summary>
		/// Resets the RebalanceMenuText property to its default value. 
		/// </summary>
        public void ResetRebalanceMenuText()
        {
            RebalanceMenuText = "&Rebalance";
        }

		/// <summary>
		/// Gets or sets the text used for the MovePrevious menu option.
		/// </summary>
        [Localizable(true)]
        [Category("Text String")]
        [DefaultValue("Move to &Previous Tab Group")]
        public string MovePreviousMenuText
        {
            get { return _movePreviousMenuText; }
            set { _movePreviousMenuText = value; }
        }

		/// <summary>
		/// Resets the MovePreviousMenuText property to its default value. 
		/// </summary>
        public void ResetMovePreviousMenuText()
        {
            MovePreviousMenuText = "Move to &Previous Tab Group";
        }

		/// <summary>
		/// Gets or sets the text used for the MoveNext menu option.
		/// </summary>
        [Localizable(true)]
        [Category("Text String")]
        [DefaultValue("Move to &Next Tab Group")]
        public string MoveNextMenuText
        {
            get { return _moveNextMenuText; }
            set { _moveNextMenuText = value; }
        }
        
		/// <summary>
		/// Resets the MoveNextMenuText property to its default value. 
		/// </summary>
        public void ResetMoveNextMenuText()
        {
            MoveNextMenuText = "Move to &Next Tab Group";
        }

		/// <summary>
		/// Gets or sets the text used for the NewVertical menu option.
		/// </summary>
        [Localizable(true)]
        [Category("Text String")]
        [DefaultValue("New &Vertical Tab Group")]
        public string NewVerticalMenuText
        {
            get { return _newVerticalMenuText; }
            set { _newVerticalMenuText = value; }
        }

		/// <summary>
		/// Resets the NewVerticalMenuText property to its default value.
		/// </summary>
        public void ResetNewVerticalMenuText()
        {
            NewVerticalMenuText = "New &Vertical Tab Group";
        }

		/// <summary>
		/// Gets or sets the text used for the NewHorizontal menu option.
		/// </summary>
        [Localizable(true)]
        [Category("Text String")]
        [DefaultValue("New &Horizontal Tab Group")]
        public string NewHorizontalMenuText
        {
            get { return _newHorizontalMenuText; }
            set { _newHorizontalMenuText = value; }
        }
        
		/// <summary>
		/// Resets the NewHorizontalMenuText property to its default value.
		/// </summary>
        public void ResetNewHorizontalMenuText()
        {
            NewHorizontalMenuText = "New &Horizontal Tab Group";
        }

		/// <summary>
		/// Gets or sets the shortcut key combination for close.
		/// </summary>
        [Category("Shortcuts")]
        public Keys CloseShortcut
        {
            get { return _closeShortcut; }
            set { _closeShortcut = value; }
        }

		/// <summary>
		/// Decide if close shortcut needs to be persisted.
		/// </summary>
		/// <returns>Should the shortcut be persisted.</returns>
        protected bool ShouldSerializeCloseShortcut()
        {
            return !_closeShortcut.Equals(Keys.Control | Keys.Shift | Keys.C);
        }

		/// <summary>
		/// Resets the CloseShortcut property to its default value.
		/// </summary>
        public void ResetCloseShortcut()
        {
            CloseShortcut = Keys.Control | Keys.Shift | Keys.C;
        }
        
		/// <summary>
		/// Gets or sets the shortcut key combination for prominent.
		/// </summary>
        [Category("Shortcuts")]
        public Keys ProminentShortcut
        {
            get { return _prominentShortcut; }
            set { _prominentShortcut = value; }
        }

		/// <summary>
		/// Decide if prominent shortcut needs to be persisted.
		/// </summary>
		/// <returns></returns>
        protected bool ShouldSerializeProminentShortcut()
        {
            return !_prominentShortcut.Equals(Keys.Control | Keys.Shift | Keys.T);
        }

		/// <summary>
		/// Resets the ProminentShortcut property to its default value.
		/// </summary>
        public void ResetProminentShortcut()
        {
            ProminentShortcut = Keys.Control | Keys.Shift | Keys.T;  
        }
        
		/// <summary>
		/// Gets or sets the shortcut key combination for rebalance.
		/// </summary>
        [Category("Shortcuts")]
        public Keys RebalanceShortcut
        {
            get { return _rebalanceShortcut; }
            set { _rebalanceShortcut = value; }
        }

		/// <summary>
		/// Decide if rebalance shortcut needs to be persisted.
		/// </summary>
		/// <returns></returns>
        protected bool ShouldSerializeRebalanceShortcut()
        {
            return !_rebalanceShortcut.Equals(Keys.Control | Keys.Shift | Keys.R);
        }

		/// <summary>
		/// Resets the RebalanceShortcut property to its default value.
		/// </summary>
        public void ResetRebalanceShortcut()
        {
            RebalanceShortcut = Keys.Control | Keys.Shift | Keys.R;
        }

		/// <summary>
		/// Gets or sets the shortcut key combination for move previous.
		/// </summary>
        [Category("Shortcuts")]
        public Keys MovePreviousShortcut
        {
            get { return _movePreviousShortcut; }
            set { _movePreviousShortcut = value; }
        }

		/// <summary>
		/// Decide if move previous shortcut needs to be persisted.
		/// </summary>
		/// <returns></returns>
        protected bool ShouldSerializeMovePreviousShortcut()
        {
            return !_movePreviousShortcut.Equals(Keys.Control | Keys.Shift | Keys.P);
        }

		/// <summary>
		/// Resets the MovePreviousShortcut property to its default value.
		/// </summary>
        public void ResetMovePreviousShortcut()
        {
            MovePreviousShortcut = Keys.Control | Keys.Shift | Keys.P;
        }
        
		/// <summary>
		/// Gets or sets the shortcut key combination for move next.
		/// </summary>
        [Category("Shortcuts")]
        public Keys MoveNextShortcut
        {
            get { return _moveNextShortcut; }
            set { _moveNextShortcut = value; }
        }

		/// <summary>
		/// Decide if move next shortcut needs to be persisted.
		/// </summary>
		/// <returns></returns>
        protected bool ShouldSerializeMoveNextShortcut()
        {
            return !_moveNextShortcut.Equals(Keys.Control | Keys.Shift | Keys.N);
        }

		/// <summary>
		/// Resets the MoveNextShortcut property to its default value.
		/// </summary>
        public void ResetMoveNextShortcut()
        {
            MoveNextShortcut = Keys.Control | Keys.Shift | Keys.N;
        }
        
		/// <summary>
		/// Gets or sets the shortcut key combination for split vertical.
		/// </summary>
        [Category("Shortcuts")]
        public Keys SplitVerticalShortcut
        {
            get { return _splitVerticalShortcut; }
            set { _splitVerticalShortcut = value; }
        }

		/// <summary>
		/// Decide if split vertical shortcut needs to be persisted.
		/// </summary>
		/// <returns></returns>
        protected bool ShouldSerializeSplitVerticalShortcut()
        {
            return !_splitVerticalShortcut.Equals(Keys.Control | Keys.Shift | Keys.V);
        }

		/// <summary>
		/// Resets the SplitVerticalShortcut property to its default value.
		/// </summary>
        public void ResetSplitVerticalShortcut()
        {
            SplitVerticalShortcut = Keys.Control | Keys.Shift | Keys.V;
        }
        
		/// <summary>
		/// Gets or sets the shortcut key combination for split horizontal.
		/// </summary>
        [Category("Shortcuts")]
        public Keys SplitHorizontalShortcut
        {
            get { return _splitHorizontalShortcut; }
            set { _splitHorizontalShortcut = value; }
        }

		/// <summary>
		/// Decide if split horizontal shortcut needs to be persisted.
		/// </summary>
		/// <returns></returns>
        protected bool ShouldSerializeSplitHorizontalShortcut()
        {
            return !_splitHorizontalShortcut.Equals(Keys.Control | Keys.Shift | Keys.H);
        }

		/// <summary>
		/// Resets the SplitHorizontalShortcut property to its default value.
		/// </summary>
        public void ResetSplitHorizontalShortcut()
        {
            SplitHorizontalShortcut = Keys.Control | Keys.Shift | Keys.H;
        }
        
		/// <summary>
		/// Gets or sets the source of images that can be used by tab pages.
		/// </summary>
        [Category("TabbedGroups")]
        public ImageList ImageList
        {
            get { return _imageList; }
            
            set 
            { 
                if (_imageList != value)
                {
                    // Propogate to all children
                    Notify(TabGroupBase.NotifyCode.ImageListChanging);

                    _imageList = value;
                    
                    // Propogate to all children
                    Notify(TabGroupBase.NotifyCode.ImageListChanged);
                }
            }
        }
        
		/// <summary>
		/// Decide if image list needs to be persisted.
		/// </summary>
		/// <returns></returns>
        protected bool ShouldSerializeImageList()
        {
            return _imageList != null;
        }
        
		/// <summary>
		/// Resets the ResetImageList property to its default value.
		/// </summary>
        public void ResetImageList()
        {
            ImageList = null;
        }
        
		/// <summary>
		/// Gets or sets the display mode for tab headers.
		/// </summary>
        [Category("TabbedGroups")]
        [DefaultValue(typeof(DisplayTabModes), "ShowAll")]
        public DisplayTabModes DisplayTabMode
        {
            get { return _displayTabMode; }
            
            set
            {
                if (_displayTabMode != value)
                {
                    _displayTabMode = value;
                    
                    // Propogate to all children
                    Notify(TabGroupBase.NotifyCode.DisplayTabMode);
                }
            }
        }
        
		/// <summary>
		/// Resets the ResetDisplayTabMode property to its default value.
		/// </summary>
        public void ResetDisplayTabMode()
        {
            DisplayTabMode = DisplayTabModes.ShowAll;
        }

		/// <summary>
		/// Gets or sets the vector used for the resize bars.
		/// </summary>
		[Category("TabbedGroups")]
		[DefaultValue(typeof(HotkeyPrefix), "Show")]
		public HotkeyPrefix HotkeyPrefix
		{
			get { return _hotkeyPrefix; }
            
			set
			{
				if (_hotkeyPrefix != value)
				{
					_hotkeyPrefix = value;
                    
					// Propogate to all children
					Notify(TabGroupBase.NotifyCode.HotkeyPrefixChanged);
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
		/// Gets or sets the need to save information about contained controls.
		/// </summary>
        [Category("TabbedGroups")]
        [DefaultValue(true)]
        public bool SaveControls
        {
            get { return _saveControls; }
            set { _saveControls = value; }
        }
        
		/// <summary>
		/// Resets the SaveControls property to its default value.
		/// </summary>
        public void ResetSaveControls()
        {
            SaveControls = true;
        }

		/// <summary>
		/// Gets or sets an indication of any changes to the control contents.
		/// </summary>
        [Category("TabbedGroups")]
        public bool Dirty
        {
            get { return _dirty; }
            
            set 
            {
                if (_dirty != value)
                {
                    _dirty = value;
                    
                    OnDirtyChanged(EventArgs.Empty);
                }
            }
        }
        
		/// <summary>
		/// Never want to persist this information.
		/// </summary>
		/// <returns>returns false always</returns>
        protected bool ShouldSerializeDirty()
        {
            return false;
        }
        
		/// <summary>
		/// Resets the ResetDirty property to its default value.
		/// </summary>
        public void ResetDirty()
        {
            Dirty = false;
        }

		/// <summary>
		/// Gets or sets whether PageClose events are generated for empty leafs.
		/// </summary>
        [Category("TabbedGroups")]
        [DefaultValue(false)]
        public bool PageCloseWhenEmpty
        {
            get { return _pageCloseWhenEmpty; }
            set { _pageCloseWhenEmpty = value; }
        }
        
		/// <summary>
		/// Resets the PageCloseWhenEmpty property to its default value.
		/// </summary>
        public void ResetPageCloseWhenEmpty()
        {
            PageCloseWhenEmpty = false;
        }

		/// <summary>
		/// Gets or sets if the shortcut and tabbing keys should be ignored.
		/// </summary>
		[Category("TabbedGroups")]
		[DefaultValue(false)]
		public bool IgnoreKeys
		{
			get { return _ignoreKeys; }
			set { _ignoreKeys = value; }
		}
        
		/// <summary>
		/// Resets the IgnoreKeys property to its default value.
		/// </summary>
		public void ResetIgnoreKeys()
		{
			IgnoreKeys = false;
		}

		/// <summary>
		/// Gets or sets whether the Dirty property is updated automatically.
		/// </summary>
        [Category("TabbedGroups")]
        [DefaultValue(true)]
        public bool AutoCalculateDirty
        {
            get { return _autoCalculateDirty; }
            set { _autoCalculateDirty = value; }
        }
        
		/// <summary>
		/// Resets the AutoCalculateDirty property to its default value.
		/// </summary>
        public void ResetAutoCalculateDirty()
        {
            AutoCalculateDirty = true;
        }

		/// <summary>
		/// Gets or sets which leaf is currently active.
		/// </summary>
		[Category("TabbedGroups")]
        public TabGroupLeaf ActiveLeaf
        {
            get { return _activeLeaf; }
            
            set
            {
				if (_activeLeaf != value)
                {
					// Cannot get rid of the active leaf when there must be one
					if (_atLeastOneLeaf && (value == null))
						return;

                    // Mark layout as dirty
                    if (_autoCalculateDirty)
                        _dirty = true;

                    // Remove selection highlight from old leaf
                    if (_activeLeaf != null)
                    {
                        // Old entry might have been disposed already
                        if (_activeLeaf.GroupControl != null)
                        {
                            // Get access to the contained tab control
                            TabControl tc = _activeLeaf.GroupControl as Controls.TabControl;
                            
                            // Remove bold text for the selected page
                            tc.BoldSelectedPage = false;
							tc.OfficeStyle = OfficeStyleNormal;
                            tc.MediaPlayerStyle = MediaPlayerStyleNormal;
                            tc.IDE2005Style = IDE2005StyleNormal;
						}
                                                
                        _activeLeaf = null;
                    }

                    // Set selection highlight on new active leaf            
                    if (value != null)
                    {
                        // Get access to the contained tab control
                        TabControl tc = value.GroupControl as Controls.TabControl;
                        
                        // Remove bold text for the selected page
                        tc.BoldSelectedPage = true;
                        
                        if (tc.TabPages.Count > 0)
						{
							tc.OfficeStyle = OfficeStyleSelected;
                            tc.MediaPlayerStyle = MediaPlayerStyleSelected;
                            tc.IDE2005Style = IDE2005StyleSelected;
						}
                        
                        _activeLeaf = value;
                    }

                    // Is the tab mode dependant on the active leaf value
                    if ((_displayTabMode == DisplayTabModes.ShowActiveLeaf) ||
                        (_displayTabMode == DisplayTabModes.ShowActiveAndMouseOver))
                    {
                        // Yes, better notify a change in value so it can be applied
                        Notify(TabGroupBase.NotifyCode.DisplayTabMode);
                    }
                        					
					OnActiveLeafChanged(EventArgs.Empty);
				
					// Test for a change in tab page
					CheckForTabPageChange();
				}
            }
        }
        
		/// <summary>
		/// Never want to persist this information.
		/// </summary>
		/// <returns>always returns false</returns>
		protected bool ShouldSerializeActiveLeaf()
		{
			// Never persist this information
			return false;
		}

		/// <summary>
		/// Resets the ActiveLeaf property to its default value.
		/// </summary>
        public void ResetActiveLeaf()
        {
            ActiveLeaf = null;
        }

		/// <summary>
		/// Gets the tab page that is currently active.
		/// </summary>
		[Category("TabbedGroups")]
		public Controls.TabPage ActiveTabPage
		{
			get { return _activeTabPage; }
		}

		/// <summary>
		/// Gets and sets if at least one leaf should be present at all times.
		/// </summary>
        [Category("TabbedGroups")]
        public bool AtLeastOneLeaf
        {
            get { return _atLeastOneLeaf; }
            
            set
            {
                if (_atLeastOneLeaf != value)
                {
                    _atLeastOneLeaf = value;
                    
                    // Do always need at least one leaf?
                    if (_atLeastOneLeaf)
                    {
                        // Is there at least one?
                        if (_numLeafs == 0)
                        {
                            // No, create a default entry for the root sequence
                            _root.AddNewLeaf();

							// Update the active leaf
							ActiveLeaf = FirstLeaf();

                            // Mark layout as dirty
                            if (_autoCalculateDirty)
                                _dirty = true;
                        }
                    }
                    else
                    {
                        // Are there some potential leaves not needed
                        if (_numLeafs > 0)
                        {
                            // Use compaction so only needed ones are retained
                            if (_autoCompact)
                                Compact();
                        }
                    }
                }
            }
        }
        
		/// <summary>
		/// Resets the AtLeastOneLeaf property to its default value.
		/// </summary>
        public void ResetAtLeastOneLeaf()
        {
            AtLeastOneLeaf = true;
        }
        
		/// <summary>
		/// Gets and sets if the control should automatically compact after changes.
		/// </summary>
        [Category("TabbedGroups")]
        [DefaultValue(true)]
        public bool AutoCompact
        {
            get { return _autoCompact; }
            set { _autoCompact = value; }
        }

		/// <summary>
		/// Resets the AutoCompact property to its default value.
		/// </summary>
        public void ResetAutoCompact()
        {
            AutoCompact = true;
        }

		/// <summary>
		/// Restore equal proportional spacing to all groups recursively.
		/// </summary>
        public void Rebalance()
        {
            _root.Rebalance(true);
        }

		/// <summary>
		/// Restore equal proportional spacing to all groups.
		/// </summary>
		/// <param name="recurse">Should recurse into all groups.</param>
        public void Rebalance(bool recurse)
        {
            _root.Rebalance(recurse);
        }
        
		/// <summary>
		/// Perform compacting of hierarchy using default flags.
		/// </summary>
        public void Compact()
        {
			// We never compact when in the process of exiting
			if (!_exiting)
				Compact(_compactOptions);
        }
        
		/// <summary>
		/// Perform compacting of hierarchy using specified flags.
		/// </summary>
		/// <param name="flags">Compacting options to be applied.</param>
        public void Compact(CompactFlags flags)
        {
            // When  entries are removed because of compacting this may cause the container object 
            // to start a compacting request. Prevent this recursion by using a simple varible.
            if (!_compacting)
            {
                // We never compact when loading/initializing the contents
                if (!_initializing)
                {
					// We never compact when in the process of exiting
					if (!_exiting)
					{
						_compacting = true;
						_root.Compact(flags);
						_compacting = false;
	                    
						EnforceAtLeastOneLeaf();
					}
                }
            }
        }

		/// <summary>
		/// Find the leaf that contains the provided page.
		/// </summary>
		/// <param name="page">TabPage to search for.</param>
		/// <returns>Leaf containing page;otherwise null.</returns>
		public TabGroupLeaf LeafForPage(TabPage page)
		{
			// Default to having not found the page
			TabGroupLeaf ret = null;

			// Do we have a valid page to look for?
			if (page != null)
			{
				// Start search from first leaf
				TabGroupLeaf examine = FirstLeaf();	

				// Keep going till all leafs examined
				while(examine != null)
				{
					// Is this page inside the this leaf?
					if (examine.TabPages.Contains(page))
					{
						// Exit with found group
						ret = examine;
						break;
					}

					// Move to next leaf in sequence
					examine = NextLeaf(examine);
				}
			}

			return ret;
		}
        
		/// <summary>
		/// Return reference to first leaf group.
		/// </summary>
		/// <returns>First leaf;otherwise null.</returns>
        public TabGroupLeaf FirstLeaf()
        {
            return RecursiveFindLeafInSequence(_root, true);
        }

		/// <summary>
		/// Return reference to last leaf group.
		/// </summary>
		/// <returns>Last leaf;otherwise null.</returns>
        public TabGroupLeaf LastLeaf()
        {
            return RecursiveFindLeafInSequence(_root, false);
        }

		/// <summary>
		/// Return reference to next leaf group in hierarchy starting from specified leaf.
		/// </summary>
		/// <param name="current">Starting leaf.</param>
		/// <returns>Next leaf;otherwise null.</returns>
        public TabGroupLeaf NextLeaf(TabGroupLeaf current)
        {
            // Get parent of the provided leaf
            TabGroupSequence tgs = current.Parent as TabGroupSequence;
            
            // Must have a valid parent sequence
            if (tgs != null)
                return RecursiveFindLeafInSequence(tgs, current, true);
            else
                return null;
        }
        
		/// <summary>
		/// Return reference to previous leaf group in hierarchy starting from specified leaf.
		/// </summary>
		/// <param name="current">Starting leaf.</param>
		/// <returns>Previous leaf;otherwise null.</returns>
        public TabGroupLeaf PreviousLeaf(TabGroupLeaf current)
        {
            // Get parent of the provided leaf
            TabGroupSequence tgs = current.Parent as TabGroupSequence;
            
            // Must have a valid parent sequence
            if (tgs != null)
                return RecursiveFindLeafInSequence(tgs, current, false);
            else
                return null;
        }

		/// <summary>
		/// Signals the object that initialization is starting.
		/// </summary>
		public void BeginInit()
		{
			_initializing = true;
		}

		/// <summary>
		/// Signals the object that initialization is complete.
		/// </summary>
		public void EndInit()
		{
			EndInit(true);
		}
		
        /// <summary>
        /// Signals the object that initialization is complete.
        /// </summary>
        /// <param name="events">Should events be generated.</param>
		public void EndInit(bool events)
		{
			_initializing = false;
            
			// If don't need a default leaf then compact to get rid of extras
			if (AtLeastOneLeaf == false)
				Compact();

			// Do we need to test for event generation
			if (events)
			{
				// Has anyone registered for the event?
				if (TabControlCreated != null)
				{
					// Get the first (if any) leaf
					TabGroupLeaf leaf = FirstLeaf();

					// Generate event for each leaf
					while(leaf != null)
					{
						TabControlCreated(this, leaf.TabControl);

						// Keep going till no more found
						leaf = NextLeaf(leaf);
					}
				}
			}
			
			// Inform the root sequence to reposition itself
			_root.Reposition();
		}
        
		/// <summary>
		/// Saves layout information into an array of bytes using Unicode Encoding.
		/// </summary>
		/// <returns>Array of created bytes.</returns>
        public byte[] SaveConfigToArray()
        {
            return SaveConfigToArray(Encoding.Unicode);	
        }

		/// <summary>
		/// Saves layout information into an array of bytes.
		/// </summary>
		/// <param name="encoding">Required encoding.</param>
		/// <returns>Array of created bytes.</returns>
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
		/// Saves layout information into a named file using Unicode Encoding.
		/// </summary>
		/// <param name="filename">Name of file to save to.</param>
        public void SaveConfigToFile(string filename)
        {
            SaveConfigToFile(filename, Encoding.Unicode);
        }

		/// <summary>
		/// Saves layout information into a named file.
		/// </summary>
		/// <param name="filename">Name of file to save to.</param>
		/// <param name="encoding">Required encoding.</param>
        public void SaveConfigToFile(string filename, Encoding encoding)
        {
            // Create/Overwrite existing file
            FileStream fs = new FileStream(filename, FileMode.Create);
			
			try
			{
				// Save into the file stream
				SaveConfigToStream(fs, encoding);		
			}
			catch
			{
				// Must remember to close
				fs.Close();
			}
        }

		/// <summary>
		/// Saves layout information into a stream object.
		/// </summary>
		/// <param name="stream">Stream object.</param>
		/// <param name="encoding">Required encoding.</param>
        public void SaveConfigToStream(Stream stream, Encoding encoding)
        {
            XmlTextWriter xmlOut = new XmlTextWriter(stream, encoding); 

            // Use indenting for readability
            xmlOut.Formatting = Formatting.Indented;
			
            // Always begin file with identification and warning
            xmlOut.WriteStartDocument();
            xmlOut.WriteComment(" DotNetMagic, The User Interface library for .NET (www.crownwood.net) ");
            xmlOut.WriteComment(" Modifying this generated file will probably render it invalid ");

			// Use exsting method to save information into xml
			SaveConfigToXml(xmlOut);

            // This should flush all actions and close the file
			xmlOut.WriteEndDocument();
			xmlOut.Close();			
        }

		/// <summary>
		/// Saves layout information using a provider xml writer.
		/// </summary>
		/// <param name="xmlOut">Xml writer object.</param>
		public void SaveConfigToXml(XmlTextWriter xmlOut)
		{
            // Remember the current culture setting
            CultureInfo culture = Thread.CurrentThread.CurrentCulture;

            try
            {
			    // Associate a version number with the root element so that future version of the code
			    // will be able to be backwards compatible or at least recognise out of date versions
			    xmlOut.WriteStartElement("TabbedGroups");
			    xmlOut.WriteAttributeString("FormatVersion", "1");
                
			    if (_activeLeaf != null)
				    xmlOut.WriteAttributeString("ActiveLeaf", _activeLeaf.Unique.ToString());
			    else
				    xmlOut.WriteAttributeString("ActiveLeaf", "-1");

			    // Give handlers chance to embed custom data
			    xmlOut.WriteStartElement("CustomGlobalData"); 
			    OnGlobalSaving(xmlOut);
			    xmlOut.WriteEndElement();

			    // Save the root sequence
			    _root.SaveToXml(xmlOut);

			    // Terminate the root element and document        
			    xmlOut.WriteEndElement();
                
			    // Saved, so cannot be dirty any more
			    if (_autoCalculateDirty)
				    _dirty = false;
            }
            finally
            {
                // Put back the old culture before existing routine
                Thread.CurrentThread.CurrentCulture = culture;
            }
		}

		/// <summary>
		/// Loads layout information from given array of bytes.
		/// </summary>
		/// <param name="buffer">Array of source bytes.</param>
        public void LoadConfigFromArray(byte[] buffer)
        {
            // Create a memory based stream
            MemoryStream ms = new MemoryStream(buffer);
			
            // Save into the file stream
            LoadConfigFromStream(ms);

            // Must remember to close
            ms.Close();
        }

		/// <summary>
		/// Loads layout information from given filename.
		/// </summary>
		/// <param name="filename">Name of file to read from.</param>
        public void LoadConfigFromFile(string filename)
        {
            // Open existing file
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
			
			try
			{
				// Load from the file stream
				LoadConfigFromStream(fs);		
			}
			catch
			{
				// Must remember to close
				fs.Close();
			}
        }

		/// <summary>
		/// Loads layout information from given stream object.
		/// </summary>
		/// <param name="stream">Stream object.</param>
        public void LoadConfigFromStream(Stream stream)
        {
            XmlTextReader xmlIn = new XmlTextReader(stream); 

            // Ignore whitespace, not interested
            xmlIn.WhitespaceHandling = WhitespaceHandling.None;

            // Moves the reader to the root element.
            xmlIn.MoveToContent();

			// Use existing method to load from xml
			LoadConfigFromXml(xmlIn);
                        
            xmlIn.Close();			
        }
        
		/// <summary>
		/// Loads layout information using the provided xml reader.
		/// </summary>
		/// <param name="xmlIn">Xml reader object.</param>
		public void LoadConfigFromXml(XmlTextReader xmlIn)
		{
            // Remember the current culture setting
            CultureInfo culture = Thread.CurrentThread.CurrentCulture;

            try
            {
			    // Double check this has the correct element name
			    if (xmlIn.Name != "TabbedGroups")
				    throw new ArgumentException("Root element must be 'TabbedGroups'");

			    // Load the format version number
			    string version = xmlIn.GetAttribute(0);
			    string rawActiveLeaf = xmlIn.GetAttribute(1);

			    // Convert format version from string to double
			    int formatVersion = (int)Convert.ToDouble(version);
			    int activeLeaf = Convert.ToInt32(rawActiveLeaf);
                
			    // We can only load 1 upward version formats
			    if (formatVersion < 1)
				    throw new ArgumentException("Can only load Version 1 and upwards TabbedGroups Configuration files");

			    try
			    {
				    // Prevent compacting and reposition of children
				    BeginInit();
                    
				    // Remove all existing contents
				    _root.Clear();
                    
				    // Read to custom data element
				    if (!xmlIn.Read())
					    throw new ArgumentException("An element was expected but could not be read in");

				    if (xmlIn.Name != "CustomGlobalData")
					    throw new ArgumentException("Expected 'CustomGlobalData' element was not found");

				    bool finished = xmlIn.IsEmptyElement;

				    // Give handlers chance to reload custom saved data
				    OnGlobalLoading(xmlIn);

				    // Read everything until we get the end of custom data marker
				    while(!finished)
				    {
					    // Check it has the expected name
					    if (xmlIn.NodeType == XmlNodeType.EndElement)
						    finished = (xmlIn.Name == "CustomGlobalData");

					    if (!finished)
					    {
						    if (!xmlIn.Read())
							    throw new ArgumentException("An element was expected but could not be read in");
					    }
				    } 

				    // Read the next well known lement
				    if (!xmlIn.Read())
					    throw new ArgumentException("An element was expected but could not be read in");

				    // Is it the expected element?
				    if (xmlIn.Name != "Sequence")
					    throw new ArgumentException("Element 'Sequence' was expected but not found");
                    
				    // Reload the root sequence
				    _root.LoadFromXml(xmlIn);

				    // Move past the end element
				    if (!xmlIn.Read())
					    throw new ArgumentException("Could not read in next expected node");

				    // Check it has the expected name
				    if (xmlIn.NodeType != XmlNodeType.EndElement)
					    throw new ArgumentException("EndElement expected but not found");
			    }
			    finally
			    {
				    TabGroupLeaf newActive = null;
                
				    // Reset the active leaf correctly
				    TabGroupLeaf current = FirstLeaf();
                    
				    while(current != null)
				    {
					    // Default to the first leaf if we cannot find a match
					    if (newActive == null)
						    newActive = current;
                            
					    // Find an exact match?
					    if (current.Unique == activeLeaf)
					    {
						    newActive = current;
						    break;
					    }
                        
					    current = NextLeaf(current);
				    }
                    
				    // Reinstate the active leaf indication
				    if (newActive != null)
					    ActiveLeaf = newActive;
                
				    // Allow normal operation
				    EndInit(false);
			    }
                            
			    // Just loaded, so cannot be dirty
			    if (_autoCalculateDirty)
				    _dirty = false;
            }
            finally
            {
                // Put back the old culture before existing routine
                Thread.CurrentThread.CurrentCulture = culture;
            }
        }

		/// <summary>
		/// Get the implementation class to create when a new TabControl is created.
		/// </summary>
        /// <returns>Type to create.</returns>
        public virtual Type OnTabControlType()
        {
            TGTabControlType e = new TGTabControlType();

            if (TabControlType != null)
                TabControlType(this, e);

            return e.TabControlType;
        }

		/// <summary>
		/// Define default settings for a newly created TabControl instance.
		/// </summary>
		/// <param name="tc">TabControl instance just created.</param>
		public virtual void OnTabControlCreated(Controls.TabControl tc)
		{
			// Only modify leaf count when not suspended
			if (_suspendLeafCount == 0)
			{
				// Remember how many leafs there are
				_numLeafs++;
			}
        
			// Define default values
			tc.Appearance = VisualAppearance.MultiDocument;
			tc.HotkeyPrefix = _hotkeyPrefix;
			tc.BoldSelectedPage = false;
			tc.DragOverSelect = false;
			tc.IDEPixelBorder = true;
			tc.OfficeStyle = OfficeStyleNormal;
            tc.OfficeHeaderBorder = true;
            tc.MediaPlayerStyle = MediaPlayerStyleNormal;
            tc.MediaPlayerHeaderBorder = true;
            tc.IDE2005Style = IDE2005StyleNormal;
            tc.ImageList = _imageList;
			tc.Style = _externalStyle;

			// Apply the current display tab mode setting            
			switch(_displayTabMode)
			{
				case DisplayTabModes.ShowAll:
					tc.HideTabsMode = HideTabsModes.ShowAlways;
					break;
				case DisplayTabModes.HideAll:
					tc.HideTabsMode = HideTabsModes.HideAlways;
					break;
                case DisplayTabModes.ShowOnlyMultipleTabs:
                    tc.HideTabsMode = HideTabsModes.HideUsingLogic;
                    break;
			}
            
			// Hook into the change of tab event
			tc.SelectionChanged += new SelectTabHandler(OnSelectedChanged);

			// Has anyone registered for the event?
			if (TabControlCreated != null)
				TabControlCreated(this, tc);
		}
        
		/// <summary>
		/// Raises the TabControlRemove event to let user unsubscribe event handlers.
		/// </summary>
		/// <param name="tc">TabControl instance about to be removed.</param>
		public virtual void OnTabControlRemoved(Controls.TabControl tc)
		{
			if(TabControlRemoved != null)
				TabControlRemoved(this, tc);
		}

		/// <summary>
		/// Raises the PageCloseRequest event.
		/// </summary>
		/// <param name="e">A TGCloseRequestEventArgs that contains the event data.</param>
		public virtual void OnPageCloseRequested(TGCloseRequestEventArgs e)
		{
			// Has anyone registered for the event?
			if (PageCloseRequest != null)
				PageCloseRequest(this, e);
		}

		/// <summary>
		/// Raises the PageContextMenu event.
		/// </summary>
		/// <param name="e">A TGContextMenuEventArgs that contains the event data.</param>
		public virtual void OnPageContextMenu(TGContextMenuEventArgs e)
		{
			// Has anyone registered for the event?
			if (PageContextMenu != null)
				PageContextMenu(this, e);
		}

		/// <summary>
		/// Raises the GlobalSaving event.
		/// </summary>
		/// <param name="xmlOut">A XmlTextWriter that contains the event data.</param>
		public virtual void OnGlobalSaving(XmlTextWriter xmlOut)
		{
			// Has anyone registered for the event?
			if (GlobalSaving != null)
				GlobalSaving(this, xmlOut);
		}
        
		/// <summary>
		/// Raises the GlobalLoading event.
		/// </summary>
		/// <param name="xmlIn">A XmlTextReader that contains the event data.</param>
		public virtual void OnGlobalLoading(XmlTextReader xmlIn)
		{
			// Has anyone registered for the event?
			if (GlobalLoading != null)
				GlobalLoading(this, xmlIn);
		}
        
		/// <summary>
		/// Raises the PageSaving event.
		/// </summary>
		/// <param name="e">A TGPageSavingEventArgs that contains the event data.</param>
		public virtual void OnPageSaving(TGPageSavingEventArgs e)
		{
			// Has anyone registered for the event?
			if (PageSaving != null)
				PageSaving(this, e);
		}
        
		/// <summary>
		/// Raises the PageLoading event.
		/// </summary>
		/// <param name="e">A TGPageLoadingEventArgs that contains the event data.</param>
		public virtual void OnPageLoading(TGPageLoadingEventArgs e)
		{
			// Has anyone registered for the event?
			if (PageLoading != null)
				PageLoading(this, e);
		}

		/// <summary>
		/// Raises the ProminentLeafChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		public virtual void OnProminentLeafChanged(EventArgs e)
		{
			// Has anyone registered for the event?
			if (ProminentLeafChanged != null)
				ProminentLeafChanged(this, e);
		}
        
		/// <summary>
		/// Raises the ActiveLeafChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		public virtual void OnActiveLeafChanged(EventArgs e)
		{
			// Has anyone registered for the event?
			if (ActiveLeafChanged != null)
				ActiveLeafChanged(this, e);
		}
        
		/// <summary>
		/// Raises the DirtyChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		public virtual void OnDirtyChanged(EventArgs e)
		{
			// Has anyone registered for the event?
			if (DirtyChanged != null)
				DirtyChanged(this, e);
		}

		/// <summary>
		/// Raises the ExternalDrop event.
		/// </summary>
		/// <param name="tgl">Source TabGroupLeaf instance.</param>
		/// <param name="tc">Source TabControl instance.</param>
		/// <param name="dp">Source DragProvider instance.</param>
		public virtual void OnExternalDrop(TabGroupLeaf tgl, 
										   Controls.TabControl tc, 
										   DragProvider dp)
		{
			// Has anyone registered for the event?
			if (ExternalDrop != null)
				ExternalDrop(this, tgl, tc, dp);
		}
		
		/// <summary>
		/// Raises the ExternalDropType event.
		/// </summary>
		/// <param name="tgl">Source TabGroupLeaf instance.</param>
		/// <param name="tc">Source TabControl instance.</param>
		/// <param name="data">Source typed data.</param>
		public virtual void OnExternalDropType(TabGroupLeaf tgl, 
											   Controls.TabControl tc, 
											   object data)
		{
			// Has anyone registered for the event?
			if (ExternalDropType != null)
				ExternalDropType(this, tgl, tc, data);
		}

		/// <summary>
		/// Raises the ExternalDropRaw event.
		/// </summary>
		/// <param name="tgl">Source TabGroupLeaf instance.</param>
		/// <param name="tc">Source TabControl instance.</param>
		/// <param name="rawData">Source raw data.</param>
		public virtual void OnExternalDropRaw(TabGroupLeaf tgl, 
											  Controls.TabControl tc, 
											  DragEventArgs rawData)
		{
			// Has anyone registered for the event?
			if (ExternalDropRaw != null)
				ExternalDropRaw(this, tgl, tc, rawData);
		}

		/// <summary>
		/// Raises the ExternalDragEnter event to test if drag is allowed into the tab headers.
		/// </summary>
		/// <param name="tgl">Source TabGroupLeaf instance.</param>
		/// <param name="tc">Source TabControl instance.</param>
		/// <param name="e">An DragEventArgs than contains the event data.</param>
		/// <returns>true if allowed to drag provided data; otherwise false.</returns>
		public bool OnExternalDragEnter(TabGroupLeaf tgl, 
										Controls.TabControl tc, 
										DragEventArgs e)
		{
			// By default we fail to provide a valid type
			bool valid = false;

			// If event it defined then we ask it to process
			if (ExternalDragEnter != null)
				valid = ExternalDragEnter(this, tgl, tc, e);

			return valid;
		}

		/// <summary>
		/// Raises the ExternalDragOver event to test if drag is allowed over the tab headers.
		/// </summary>
		/// <param name="tgl">Source TabGroupLeaf instance.</param>
		/// <param name="tc">Source TabControl instance.</param>
		/// <param name="e">An DragEventArgs than contains the event data.</param>
		/// <returns>true if allowed to drag provided data; otherwise false.</returns>
		public bool OnExternalDragOver(TabGroupLeaf tgl, 
										Controls.TabControl tc, 
										DragEventArgs e)
		{
			// By default we fail to provide a valid type
			bool valid = false;

			// If event it defined then we ask it to process
			if (ExternalDragOver != null)
				valid = ExternalDragOver(this, tgl, tc, e);

			return valid;
		}

		/// <summary>
		/// Raises the PageChanged event.
		/// </summary>
		/// <param name="tp">Source TabPage instance.</param>
		public virtual void OnPageChanged(Controls.TabPage tp)
		{
			// Has anyone registered for the event?
			if (PageChanged != null)
				PageChanged(this, tp);
		}

		/// <summary>
		/// Raises the ResizeStart event.
		/// </summary>
		/// <param name="tgs">TabGroupSequence that contains the resize operation.</param>
		public virtual bool OnResizeStart(TabGroupSequence tgs)
		{
			// By default we do not cancel resize
			CancelEventArgs ce = new CancelEventArgs(false);

			// Has anyone registered for the event?
			if (ResizeStart != null)
				ResizeStart(this, tgs, ce);

			return ce.Cancel;
		}

		/// <summary>
		/// Raises the ResizeFinish event.
		/// </summary>
		/// <param name="tgs">TabGroupSequence that contains the resize operation.</param>
		public virtual void OnResizeFinish(TabGroupSequence tgs)
		{
			// Has anyone registered for the event?
			if (ResizeFinish != null)
				ResizeFinish(this, tgs);
		}

		/// <summary>
		/// Processes a dialog key.
		/// </summary>
		/// <returns>true if key processed; otherwise false.</returns>
		protected override bool ProcessDialogKey(Keys keyData)
		{
			// If we should ignore keys then just return now
			if (_ignoreKeys)
				return base.ProcessDialogKey(keyData);

			if (this.Enabled)
			{
				// Basic code we are looking for is the key pressed
				int code = (int)keyData & ~(int)(Keys.Shift | Keys.Control |  Keys.Alt);

				// Is SHIFT pressed?
				bool shiftPressed = ((keyData & Keys.Shift) == Keys.Shift);

				// Is CONTROL pressed?
				bool controlPressed = ((keyData & Keys.Control) == Keys.Control);

				// Was the TAB key pressed?
				if ((code == (int)Keys.Tab) && controlPressed)
				{
					if (shiftPressed)
						return SelectPreviousTab();
					else
						return SelectNextTab();
				}
				else
				{
					// Plus the modifier for SHIFT...
					if (shiftPressed)
						code += 0x00010000;

					// Plus the modifier for CONTROL
					if (controlPressed)
						code += 0x00020000;

					// Construct keycode from keystate and keychar
					Keys keys = (Keys)(code);

					// Search for a matching command
                    if (TestShortcutKeys(keys))
						return true;
				}
			}

			// We have not intercepted key press
			return base.ProcessDialogKey(keyData);
		}

		internal void MoveActiveToNearestFromLeaf(TabGroupBase oldLeaf)
		{
			// Must have a reference to begin movement
			if (oldLeaf != null)
			{
				// Find the parent sequence of leaf, remember that a 
				// leaf must be contained within a sequence instance
				TabGroupSequence tgs = oldLeaf.Parent as TabGroupSequence;
                
				// Must be valid, but had better check anyway
				if (tgs != null)
				{
					// Move relative to given base in the sequence
					MoveActiveInSequence(tgs, oldLeaf);
				}
			}
		}
        
		internal void MoveActiveToNearestFromSequence(TabGroupSequence tgs)
		{
			// Is active leaf being moved from root sequence
			if (_root == tgs)
			{
				// Then make nothing active
				ActiveLeaf = null;
			}
			else
			{
				// Find the parent sequence of given sequence
				TabGroupSequence tgsParent = tgs.Parent as TabGroupSequence;
            
				// Must be valid, but had better check anyway
				if (tgs != null)
				{
					// Move relative to given base in the sequence
					MoveActiveInSequence(tgsParent, tgs);
				}
			}
		}
        
		internal bool Initializing
		{
			get { return _initializing; }
		}

		internal void SuspendTabChange()
		{
			_suspendTabChange++;
		}
        
		internal void ResumeTabChange()
		{
			_suspendTabChange--;
			
			CheckForTabPageChange();
		}
		
		internal void SuspendLeafCount()
		{
			_suspendLeafCount++;
		}

		internal void ResumeLeafCount(int adjust)
		{
			_suspendLeafCount--;

			// Apply adjustment factor
			_numLeafs += adjust;
		}

		internal void EnforceAtLeastOneLeaf()
		{
			// Should not add items during compacting operation
			if (!_compacting && !_initializing && !_exiting)
			{
				// Ensure we enfore policy of at least one leaf
				if (_atLeastOneLeaf)
				{
					// Is there at least one?
					if (_numLeafs == 0)
					{
						// No, create a default entry for the root sequence
						_root.AddNewLeaf();
                        
						// Update the active leaf
						ActiveLeaf = FirstLeaf();

						// Mark layout as dirty
						if (_autoCalculateDirty)
							_dirty = true;
					}
				}
			}
		}
        
		internal void GroupRemoved(TabGroupBase tgb, bool recurse)
		{
			// Only modify leaf count when not suspended
			if (_suspendLeafCount == 0)
			{	
				// If removing the prominent leaf
				if (tgb == _prominentLeaf)
				{
					// Need to reset prominent leaf value
					ProminentLeaf = null;
				}

				// Decrease count of leafs entries for each leaf that exists
				// which in the hierarchy that is being removed
                
				if (tgb.IsLeaf)
				{
					_numLeafs--;

					// Was last leaf removed?
					if (_numLeafs == 0)
					{
						// If at least one leaf then set the value manually so that when the
						// new one is created and set as active it does not try to process the
						// old value that has already been destroyed.
						if (_atLeastOneLeaf)
						{
							// Need to get rid of active leaf value
							ActiveLeaf = null;
						}
					}
				}
				else
				{
					if (recurse)
					{
						TabGroupSequence tgs = tgb as TabGroupSequence;
	                
						// Recurse into processing each child item
						for(int i=0; i<tgs.Count; i++)
							GroupRemoved(tgs[i], recurse);
					}
				}
                
				// Mark layout as dirty
				if (_autoCalculateDirty)
					_dirty = true;
			}
		}
         
		private TabGroupLeaf RecursiveFindLeafInSequence(TabGroupSequence tgs, bool forwards)
        {
            int count = tgs.Count;
        
            for(int i=0; i<count; i++)
            {
                // Index depends on which direction we are processing
                int index = (forwards == true) ? i : (tgs.Count - i - 1);
                
                // Is this the needed leaf node?
                if (tgs[index].IsLeaf)
                    return tgs[index] as TabGroupLeaf;
                else
                {
                    // Need to make a recursive check inside group
                    TabGroupLeaf leaf = RecursiveFindLeafInSequence(tgs[index] as TabGroupSequence, forwards);

                    if (leaf != null)
                        return leaf;
                }
            }
            
            // Still no luck
            return null;
        }

        private TabGroupLeaf RecursiveFindLeafInSequence(TabGroupSequence tgs, TabGroupBase tgb, bool forwards)
        {
            int count = tgs.Count;
            int index = tgs.IndexOf(tgb);
        
            // Are we look for entries after the provided one?
            if (forwards)
            {
                for(int i=index+1; i<count; i++)
                {
                    // Is this the needed leaf node?
                    if (tgs[i].IsLeaf)
                        return tgs[i] as TabGroupLeaf;
                    else
                    {
                        TabGroupLeaf leaf = RecursiveFindLeafInSequence(tgs[i] as TabGroupSequence, forwards);
                    
                        if (leaf != null)
                            return leaf;
                    }
                }
            }
            else
            {
                // Now try each entry before that given
                for(int i=index-1; i>=0; i--)
                {
                    // Is this the needed leaf node?
                    if (tgs[i].IsLeaf)
                        return tgs[i] as TabGroupLeaf;
                    else
                    {
                        TabGroupLeaf leaf = RecursiveFindLeafInSequence(tgs[i] as TabGroupSequence, forwards);
                    
                        if (leaf != null)
                            return leaf;
                    }
                }
            }
                        
            // Still no luck, try our own parent
            if (tgs.Parent != null)
                return RecursiveFindLeafInSequence(tgs.Parent as TabGroupSequence, tgs, forwards);
            else
                return null;
        }

        private void MoveActiveInSequence(TabGroupSequence tgs, TabGroupBase child)
        {
            int count = tgs.Count;
            int index = tgs.IndexOf(child);
        
            // First try each entry after that given
            for(int i=index+1; i<count; i++)
            {
                // Is this the needed leaf node?
                if (tgs[i].IsLeaf)
                {
                    // Make it active, and finish
                    ActiveLeaf = tgs[i] as TabGroupLeaf;
                    return;  
                }
                else
                {
                    // Need to make a recursive check inside group
                    if (RecursiveActiveInSequence(tgs[i] as TabGroupSequence, true))
                        return;
                }
            }
            
            // Now try each entry before that given
            for(int i=index-1; i>=0; i--)
            {
                // Is this the needed leaf node?
                if (tgs[i].IsLeaf)
                {
                    // Make it active, and finish
                    ActiveLeaf = tgs[i] as TabGroupLeaf;
                    return;  
                }
                else
                {
                    // Need to make a recursive check inside group
                    if (RecursiveActiveInSequence(tgs[i] as TabGroupSequence, false))
                        return;
                }
            }
            
            // Still no luck, try our own parent
            if (tgs.Parent != null)
                MoveActiveInSequence(tgs.Parent as TabGroupSequence, tgs);
        }
        
        private bool RecursiveActiveInSequence(TabGroupSequence tgs, bool forwards)
        {
            int count = tgs.Count;
        
            for(int i=0; i<count; i++)
            {
                // Index depends on which direction we are processing
                int index = (forwards == true) ? i : (tgs.Count - i - 1);
                
                // Is this the needed leaf node?
                if (tgs[index].IsLeaf)
                {
                    // Make it active, and finish
                    ActiveLeaf = tgs[index] as TabGroupLeaf;
                    return true;
                }
                else
                {
                    // Need to make a recursive check inside group
                    if (RecursiveActiveInSequence(tgs[index] as TabGroupSequence, forwards))
                        return true;
                }
            }
            
            // Still no luck
            return false;
        }
        
        private void Notify(TabGroupBase.NotifyCode notifyCode)
        {
            // Propogate change notification only is we have a root sequence
            if (_root != null)
                _root.Notify(notifyCode);
        }
        
        private bool TestShortcutKeys(Keys keys)
        {
            bool result = false;
        
            // Must have an active leaf for shortcuts to operate against and
			// cannot change the layout if we are defined as layout locked
            if ((_activeLeaf != null) && !LayoutLock)
            {
                Controls.TabControl tc = _activeLeaf.GroupControl as Controls.TabControl;
            
                // Must have an active tab for these shortcuts to work against
                if (tc.SelectedTab != null)
                {
                    // Close selected page requested?
                    if (keys == _closeShortcut)
                    {
						// Remove any prominent setting
						if (ProminentLeaf != null)
							ProminentLeaf = null;

                        _activeLeaf.OnClose(_activeLeaf, EventArgs.Empty);
                        result = true;
                    }

                    // Toggle the prominence state?
                    if (keys == _prominentShortcut)
                    {
						_activeLeaf.OnToggleProminent(_activeLeaf, EventArgs.Empty);
                        result = true;
                    }
                        
                    // Move page to the next group?
                    if (keys == _moveNextShortcut)
                    {
						// Remove any prominent setting
						if (ProminentLeaf != null)
							ProminentLeaf = null;

						_activeLeaf.OnMoveNext(_activeLeaf, EventArgs.Empty);
                        result = true;
                    }
                
                    // Move page to the previous group?
                    if (keys == _movePreviousShortcut)
                    {
						// Remove any prominent setting
						if (ProminentLeaf != null)
							ProminentLeaf = null;

						_activeLeaf.OnMovePrevious(_activeLeaf, EventArgs.Empty);
                        result = true;
                    }
                
                    // Cannot split a group unless at least two entries exist                
                    if (tc.TabPages.Count > 1)
                    {
                        // Create two vertical groups
                        if (keys == _splitVerticalShortcut)
                        {
							// Remove any prominent setting
							if (ProminentLeaf != null)
								ProminentLeaf = null;

							_activeLeaf.NewHorizontalGroup(_activeLeaf, false);
                            result = true;
                        }

                        // Create two horizontal groups
                        if (keys == _splitHorizontalShortcut)
                        {
							// Remove any prominent setting
							if (ProminentLeaf != null)
								ProminentLeaf = null;

							_activeLeaf.NewVerticalGroup(_activeLeaf, false);
                            result = true;
                        }
                    }
                }
                
                // Request to rebalance all spacing
                if (keys == _rebalanceShortcut)
                {
					// Remove any prominent setting
					if (ProminentLeaf != null)
						ProminentLeaf = null;

					_activeLeaf.OnRebalance(_activeLeaf, EventArgs.Empty);
                    result = true;
                }
            }

            return result;
        }
        
        private bool SelectNextTab()
        {
            // If no active leaf...
            if (_activeLeaf == null)
                SelectFirstPage();
            else
            {
                bool selectFirst = false;
                TabGroupLeaf startLeaf = _activeLeaf;
                TabGroupLeaf thisLeaf = startLeaf;
                
                do
                {
                    // Access to the embedded tab control
                    Controls.TabControl tc = thisLeaf.GroupControl as Controls.TabControl;
                
                    // Does it have any pages?
                    if (tc.TabPages.Count > 0)
                    {
                        // Are we allowed to select the first page?
                        if (selectFirst)
                        {
                            // Do it and exit loop
                            tc.SelectedIndex = 0;
                            
                            // Must ensure this becomes the active leaf
                            if (thisLeaf != _activeLeaf)
                                ActiveLeaf = thisLeaf;
                                
                            break;
                        }
                        else
                        {
                            // Is there another page after the selected one?
                            if (tc.SelectedIndex < tc.TabPages.Count - 1)
                            {
                                // Select new page and exit loop
                                tc.SelectedIndex = tc.SelectedIndex + 1;
                                break;
                            }         
                        }           
                    }
                    
                    selectFirst = true;
                    
                    // Find the next leaf in sequence
                    thisLeaf = NextLeaf(thisLeaf);
                    
                    // No more leafs, wrap back to first
                    if (thisLeaf == null)
                        thisLeaf = FirstLeaf();

                    // Back at starting leaf?
                    if (thisLeaf == startLeaf)
                    {
                        // If it was not the first page that we started from
                        if (tc.SelectedIndex > 0)
                        {
                            // Then we have circles all the way around, select first page
                            tc.SelectedIndex = 0;
                        }
                    }

                } while(thisLeaf != startLeaf);
            }
            
            return true;
        }

        private bool SelectPreviousTab()
        {
            // If no active leaf...
            if (_activeLeaf == null)
                SelectLastPage();
            else
            {
                bool selectLast = false;
                TabGroupLeaf startLeaf = _activeLeaf;
                TabGroupLeaf thisLeaf = startLeaf;
                
                do
                {
                    // Access to the embedded tab control
                    Controls.TabControl tc = thisLeaf.GroupControl as Controls.TabControl;
                
                    // Does it have any pages?
                    if (tc.TabPages.Count > 0)
                    {
                        // Are we allowed to select the last page?
                        if (selectLast)
                        {
                            // Do it and exit loop
                            tc.SelectedIndex = tc.TabPages.Count - 1;
                            
                            // Must ensure this becomes the active leaf
                            if (thisLeaf != _activeLeaf)
                                ActiveLeaf = thisLeaf;
                                
                            break;
                        }
                        else
                        {
                            // Is there another page before the selected one?
                            if (tc.SelectedIndex > 0)
                            {
                                // Select previous page and exit loop
                                tc.SelectedIndex = tc.SelectedIndex - 1;
                                break;
                            }         
                        }           
                    }
                    
                    selectLast = true;
                    
                    // Find the previous leaf in sequence
                    thisLeaf = PreviousLeaf(thisLeaf);
                    
                    // No more leafs, wrap back to first
                    if (thisLeaf == null)
                        thisLeaf = LastLeaf();

                    // Back at starting leaf?
                    if (thisLeaf == startLeaf)
                    {
                        // If it was not the first page that we started from
                        if (tc.SelectedIndex == 0)
                        {
                            // Then we have circles all the way around, select last page
                            tc.SelectedIndex = tc.TabPages.Count - 1;
                        }
                    }

                } while(thisLeaf != startLeaf);
            }
            
            return true;
        }

        private void SelectFirstPage()
        {
            // Find the first leaf
            ActiveLeaf = FirstLeaf();
                    
            // Did we find a leaf?
            if (_activeLeaf != null)
            {
                // Is there a page that can be selected?
                if (_activeLeaf.TabPages.Count > 0)
                    _activeLeaf.TabPages[0].Selected = true;
            }
        }
        
        private void SelectLastPage()
        {
            // Find the first leaf
            ActiveLeaf = LastLeaf();
                    
            // Did we find a leaf?
            if (_activeLeaf != null)
            {
                // Is there a page that can be selected?
                if (_activeLeaf.TabPages.Count > 0)
                    _activeLeaf.TabPages[_activeLeaf.TabPages.Count - 1].Selected = true;
            }
        }

		private void OnSelectedChanged(Crownwood.DotNetMagic.Controls.TabControl tabControl, 
									   Crownwood.DotNetMagic.Controls.TabPage oldPage, 
									   Crownwood.DotNetMagic.Controls.TabPage newPage)
		{
			// Process a change in tab page
			CheckForTabPageChange();
		}

		private void CheckForTabPageChange()
		{
			// Are we allowed to check for a change?
			if (_suspendTabChange == 0)
			{
				// Currently selected tab page
				Controls.TabPage tp = null;

				// Find the current page that is selected
				TabGroupLeaf tgl = ActiveLeaf;

				// Have a valid leaf?
				if (tgl != null)
				{
					// Get access to the active tab control instance
					Controls.TabControl tc = tgl.GroupControl as Controls.TabControl;

					// Is there a page selected?
					if (tc.SelectedIndex != -1)
						tp = tc.TabPages[tc.SelectedIndex];
				}
			
				// Change in active page?
				if (_activeTabPage != tp)
				{
					_activeTabPage = tp;

					// Generate event to notify change in active page
					OnPageChanged(_activeTabPage);
				}
			}
		}
    }

	/// <summary>
	/// Specifies how tab headers are displayed.
	/// </summary>
	public enum DisplayTabModes
	{
		/// <summary>
		/// Specifies tab headers are not shown.
		/// </summary>
		HideAll,

		/// <summary>
		/// Specifies tab headers are always shown.
		/// </summary>
		ShowAll,

		/// <summary>
		/// Specifies tab headers are shown for the active leaf only.
		/// </summary>
		ShowActiveLeaf,

		/// <summary>
		/// Specifies tab headers are shown only when mouse is over them.
		/// </summary>
		ShowMouseOver,

		/// <summary>
		/// Specifies tab headers are shown when mouse over or active.
		/// </summary>
		ShowActiveAndMouseOver,

        /// <summary>
        /// Specifies tab headers are shown only for leafs containing multiple pages
        /// </summary>
        ShowOnlyMultipleTabs
	}
	
	/// <summary>
	/// Specifies how compacting effects the contents.
	/// </summary>
	public enum CompactFlags
	{
		/// <summary>
		/// Specifies empty leafs are removed.
		/// </summary>
		RemoveEmptyTabLeaf = 1,

		/// <summary>
		/// Specifies empty sequences are removed.
		/// </summary>
		RemoveEmptyTabSequence = 2,

		/// <summary>
		/// Specifies single entries in a sequence replace the sequence.
		/// </summary>
		ReduceSingleEntries = 4,

		/// <summary>
		/// Specifies multiple sequences in same direction are merged.
		/// </summary>
		ReduceSameDirection = 8,

		/// <summary>
		/// Specifies that all compacting flags are applied.
		/// </summary>
		All = 15
	}
}
