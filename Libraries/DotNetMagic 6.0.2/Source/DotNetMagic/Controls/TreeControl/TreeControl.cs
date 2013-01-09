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
using System.Text;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using System.Reflection;
using Crownwood.DotNetMagic.Common;
using Crownwood.DotNetMagic.Win32;
using Microsoft.Win32;

namespace Crownwood.DotNetMagic.Controls
{

    delegate void SelectNodeParameterDelegate (Node n, bool shift, bool ctrl);
    delegate void DeselectNodeParameterDelegate(Node n, bool recurse);

    /// <summary>
	/// Manage a collection of Node instances.
	/// </summary>
	[ToolboxBitmap(typeof(TreeControl))]
	[DefaultProperty("ViewControllers")]
	public class TreeControl : Control
	{	
		// Constants
		private const int WHEEL_DELTA = 120;
		private const int INDICATOR_WIDTH = 13;
		private const int INDICATOR_HEIGHT = 13;
		private const int BUMP_HEIGHT = 15;
		
		// Class fields
		private static ImageList _indicatorImages;
        private static PropertyInfo _cachedShortcutPI;
        private static MethodInfo _cachedShortcutMI;

		// Instance fields - Border
		private bool _borderIs3D;
		private Size _borderSize;
		private Color _borderColor;
		private TreeBorderStyle _borderStyle;
		private Border3DStyle _border3DStyle;
		private IndentPaddingEdges _borderIndent;
		private ButtonBorderStyle _borderButtonStyle;

		// Instance fields - Lines and Boxes
		private DrawStyle _boxDrawStyle;
		private bool _boxShownAlways;
		private int _columnWidth;
		private int _lineWidth;
		private int _boxLength;
		private Color _lineColor;
		private Color _boxSignColor;
		private Color _boxBorderColor;
		private Color _boxInsideColor;
		private LineDashStyle _lineDashStyle;
		private Pen _cacheLineDashPen;
		private Pen _cacheBoxSignPen;
		private Pen _cacheBoxBorderPen;
		private Brush _cacheBoxInsideBrush;
		private LineBoxVisibility _lineVisibility;
		private LineBoxVisibility _boxVisibility;

		// Instance fields - Checkbox
		private CheckStates _checkStates;
		private DrawStyle _checkDrawStyle;
		private int _checkLength;
		private int _checkGapLeft;
		private int _checkGapRight;
		private int _checkBorderWidth;
		private Color _checkBorderColor;
		private Color _checkInsideColor;
		private Color _checkInsideHotColor;
		private Color _checkTickColor;
		private Color _checkTickHotColor;
		private Color _checkMixedColor;
		private Color _checkMixedHotColor;
		private Pen _cacheCheckTickPen;
		private Pen _cacheCheckTickHotPen;
		private Brush _cacheCheckTickBrush;
		private Brush _cacheCheckTickHotBrush;
		private Brush _cacheCheckBorderBrush;
		private Brush _cacheCheckInsideBrush;
		private Brush _cacheCheckInsideHotBrush;
		private Brush _cacheCheckMixedBrush;
		private Brush _cacheCheckMixedHotBrush;

		// Instance fields - Scrollbars
		private Point _offset;
		private Panel _corner;
		private VScrollBar _vBar;
		private HScrollBar _hBar;
		private bool _scrollBarsValid;
		private bool _enableMouseWheel;
        private bool _ignoreEnsureDisplayedH;
        private bool _ignoreEnsureDisplayedV;
        private int _displayHeightScroll;
		private int _displayHeightExScroll;
        private int _mouseWheelUnits;
        private int _mouseUnits;
        private ScrollVisibility _vVisibility;
		private ScrollVisibility _hVisibility;
		private VerticalGranularity _granularity;

		// Instance fields - Images
		private ImageList _imageList;
		private int _imageIndex;
		private int _selectedImageIndex;
		private int _imageGapLeft;
		private int _imageGapRight;

		// Instance fields - Group (General)
		private Font _groupFont;
		private bool _groupArrows;
		private bool _groupAutoEdit;
		private bool _groupHotTrack;
		private bool _groupGradientBack;
		private bool _groupAutoCollapse;
		private bool _groupAutoAllocate;
		private bool _groupNodesSelectable;
		private bool _groupUseHotFontStyle;
		private bool _groupUseSelectedFontStyle;
		private bool _groupExpandOnDragHover;
		private int _groupIndentLeft;
		private int _groupIndentTop;
		private int _groupIndentBottom;
		private int _groupExtraLeft;
		private int _groupExtraHeight;
		private Color _groupBackColor;
		private Color _groupForeColor;
		private Color _groupLineColor;
		private Color _groupHotBackColor;
		private Color _groupHotForeColor;
		private Color _groupSelectedBackColor;
		private Color _groupSelectedForeColor;
		private Color _groupSelectedNoFocusBackColor;
		private GroupColoring _groupColoring;
		private FontStyle _groupHotFontStyle;
		private FontStyle _groupSelectedFontStyle;
		private GroupBorderStyle _groupBorderStyle;
		private TreeGradientColoring _groupGradientColoring;
		private GradientDirection _groupGradientAngle;
		private ClickExpandAction _groupClickExpand;
		private ClickExpandAction _groupDoubleClickExpand;
		private TextRenderingHint _groupTextRenderingHint;
		private Pen _cacheGroupLinePen;

		// Instance fields - Group (ImageBox)
		private int _groupImageBoxWidth;
		private int _groupImageBoxGap;
		private bool _groupImageBox;
		private bool _groupImageBoxColumn;
		private bool _groupImageBoxGradientBack;
		private bool _groupImageBoxBorder;
		private Color _groupImageBoxLineColor;
		private Color _groupImageBoxBackColor;
		private Color _groupImageBoxSelectedBackColor;
		private Color _groupImageBoxColumnColor;
		private TreeGradientColoring _groupImageBoxGradientColoring;
		private GradientDirection _groupImageBoxGradientAngle;
		private Brush _cacheGroupImageBoxColumnBrush;
		private Pen _cacheGroupImageBoxLinePen;
		
		// Instance fields - Nodes
		private Node _hotNode;
		private Node _tooltipNode;
		private Node _focusNode;
		private Point _hotPoint;
		private bool _tooltips;
		private bool _infotips;
		private bool _instantUpdate;
		private bool _autoCollapse;
		private bool _nodesSelectable;
		private bool _extendToRight;
		private bool _useHotFontStyle;
		private bool _useSelectedFontStyle;
		private bool _canUserExpandCollapse;
		private bool _expandOnDragHover;
        private bool _selectSameLevel;
        private int _verticalNodeGap;
		private int _minimumNodeHeight;
		private int _maximumNodeHeight;
		private string _pathSeparator;
		private Color _hotBackColor;
		private Color _hotForeColor;
		private Color _selectedBackColor;
		private Color _selectedForeColor;
		private Color _selectedNoFocusBackColor;
		private ContextMenuStrip _contextMenuNode;
        private ContextMenuStrip _contextMenuSpace;
		private PopupTooltipSingle _tooltip;
		private FontStyle _hotFontStyle;
		private FontStyle _selectedFontStyle;
		private Indicators _indicators;
		private ClickExpandAction _clickExpand;
		private ClickExpandAction _doubleClickExpand;
		private TextRenderingHint _textRenderingHint;
		private Brush _cacheHotBackBrush;
		private Brush _cacheSelectedBackBrush;
		private Brush _cacheSelectedNoFocusBackBrush;
		private Timer _infotipTimer;

		// Instance fields - Label Editing
		private bool _autoEdit;
		private bool _labelEdit;
		private Node _labelEditNode;
		private Node _autoEditNode;
		private TextBox _labelEditBox;
		private Timer _autoEditTimer;
		
		// Instance fields - Selection
		private Node _lastSelectedNode;
		private SelectMode _selectMode;
		private Hashtable _selected;
		
		// Instance fields - Drag and Drop
		private Node _dragNode;
		private bool _bumpUpwards;
		private Timer _dragBumpTimer;
		private Timer _dragHoverTimer;
		private Hashtable _cachedSelected;
		private DragDropEffects _dragEffects;
		private Node _mouseDownNode;
		private Point _mouseDownPt;

		// Instance fields - Cache
		private int _fontHeight;
		private int _groupFontHeight;
		private Font _fontBoldItalic;
		private Font _groupFontBoldItalic;
		private bool _invalidated;
		private bool _nodeSizesDirty;
		private bool _nodeDrawingValid;
		private bool _innerRectangleValid;
		private bool _drawRectangleValid;
		private Rectangle _innerRectangle;
		private Rectangle _drawRectangle;
		private Rectangle _clipRectangle;
		private ArrayList _displayNodes;
		private Hashtable _nodeKeys;

		// Instance fields - Theme handling
		private Size _glyphThemeSize;
		private ThemeHelper _themeTreeView;
		private ThemeHelper _themeCheckbox;
		private ColorDetails _colorDetails;

		// Instance fields - Root node storage
		private NodeCollection _rootNodes;

		// Instance fields - Drawing interfaces
		private INodeVC _nodeVC;
		private INodeVC _defaultNodeVC;
		private INodeVC _groupNodeVC;
		private INodeCollectionVC _collectionVC;
		private INodeCollectionVC _defaultCollectionVC;
		private INodeCollectionVC _groupCollectionVC;
		private ViewControllers _viewControllers;
				
		/// <summary>
		/// Occurs when the value of the BorderIndent property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler BorderIndentChanged;

		/// <summary>
		/// Occurs when the value of the BorderStyle property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler BorderStyleChanged;

		/// <summary>
		/// Occurs when the value of the BorderColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler BorderColorChanged;

		/// <summary>
		/// Occurs when the value of the LineWidth property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler LineWidthChanged;

		/// <summary>
		/// Occurs when the value of the LineColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler LineColorChanged;

		/// <summary>
		/// Occurs when the value of the LineDashStyle property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler LineDashStyleChanged;

		/// <summary>
		/// Occurs when the value of the BoxShownAlways property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler BoxShownAlwaysChanged;

		/// <summary>
		/// Occurs when the value of the BoxSignColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler BoxSignColorChanged;

		/// <summary>
		/// Occurs when the value of the BoxBorderColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler BoxBorderColorChanged;

		/// <summary>
		/// Occurs when the value of the BoxInsideColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler BoxInsideColorChanged;

		/// <summary>
		/// Occurs when the value of the BoxLength property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler BoxLengthChanged;

		/// <summary>
		/// Occurs when the value of the BoxDrawStyle property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler BoxDrawStyleChanged;

		/// <summary>
		/// Occurs when the value of the BoxVisibility property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler BoxVisibilityChanged;

		/// <summary>
		/// Occurs when the value of the LineVisibility property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler LineVisibilityChanged;

		/// <summary>
		/// Occurs when the value of the LinesColumnWidth property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler ColumnWidthChanged;

		/// <summary>
		/// Occurs when the value of the CheckStates property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler CheckStatesChanged;

		/// <summary>
		/// Occurs when the value of the CheckDrawStyle property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler CheckDrawStyleChanged;

		/// <summary>
		/// Occurs when the value of the CheckLength property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler CheckLengthChanged;

		/// <summary>
		/// Occurs when the value of the CheckGapLeft property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler CheckGapLeftChanged;

		/// <summary>
		/// Occurs when the value of the CheckGapRight property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler CheckGapRightChanged;

		/// <summary>
		/// Occurs when the value of the CheckBorderWidth property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler CheckBorderWidthChanged;

		/// <summary>
		/// Occurs when the value of the CheckBorderColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler CheckBorderColorChanged;

		/// <summary>
		/// Occurs when the value of the CheckInsideColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler CheckInsideColorChanged;

		/// <summary>
		/// Occurs when the value of the CheckInsideHotColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler CheckInsideHotColorChanged;

		/// <summary>
		/// Occurs when the value of the CheckTickColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler CheckTickColorChanged;

		/// <summary>
		/// Occurs when the value of the CheckTickHotColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler CheckTickHotColorChanged;

		/// <summary>
		/// Occurs when the value of the CheckMixedColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler CheckMixedColorChanged;

		/// <summary>
		/// Occurs when the value of the CheckMixedHotColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler CheckMixedHotColorChanged;

		/// <summary>
		/// Occurs when the value of the GroupAutoEdit property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupAutoEditChanged;

		/// <summary>
		/// Occurs when the value of the GroupFont property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupFontChanged;

		/// <summary>
		/// Occurs when the value of the GroupArrows property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupArrowsChanged;

		/// <summary>
		/// Occurs when the value of the GroupIndentLeft property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupIndentLeftChanged;

		/// <summary>
		/// Occurs when the value of the GroupIndentLeft property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupIndentTopChanged;

		/// <summary>
		/// Occurs when the value of the GroupIndentLeft property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupIndentBottomChanged;

		/// <summary>
		/// Occurs when the value of the GroupColoring property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupColoringChanged;

		/// <summary>
		/// Occurs when the value of the GroupBackColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupBackColorChanged;

		/// <summary>
		/// Occurs when the value of the GroupForeColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupForeColorChanged;

		/// <summary>
		/// Occurs when the value of the GroupLineColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupLineColorChanged;

		/// <summary>
		/// Occurs when the value of the GroupHotBackColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupHotBackColorChanged;

		/// <summary>
		/// Occurs when the value of the GroupHotForeColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupHotForeColorChanged;

		/// <summary>
		/// Occurs when the value of the GroupSelectedBackColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupSelectedBackColorChanged;

		/// <summary>
		/// Occurs when the value of the GroupSelectedNoFocusBackColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupSelectedNoFocusBackColorChanged;

		/// <summary>
		/// Occurs when the value of the GroupSelectedForeColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupSelectedForeColorChanged;

		/// <summary>
		/// Occurs when the value of the GroupBorderStyle property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupBorderStyleChanged;

		/// <summary>
		/// Occurs when the value of the GroupHotFontStyle property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupHotFontStyleChanged;

		/// <summary>
		/// Occurs when the value of the GroupUseHotFontStyle property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupUseHotFontStyleChanged;

		/// <summary>
		/// Occurs when the value of the GroupSelectedFontStyle property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupSelectedFontStyleChanged;

		/// <summary>
		/// Occurs when the value of the GroupUseSelectedFontStyle property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupUseSelectedFontStyleChanged;

		/// <summary>
		/// Occurs when the value of the GroupGradientAngle property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupGradientAngleChanged;

		/// <summary>
		/// Occurs when the value of the GroupGradientColoring property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupGradientColoringChanged;

		/// <summary>
		/// Occurs when the value of the GroupGradientBack property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupGradientBackChanged;

		/// <summary>
		/// Occurs when the value of the GroupExtraLeft property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupExtraLeftChanged;

		/// <summary>
		/// Occurs when the value of the GroupClickExpand property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupClickExpandChanged;

		/// <summary>
		/// Occurs when the value of the GroupDoubleClickExpand property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupDoubleClickExpandChanged;

		/// <summary>
		/// Occurs when the value of the GroupAutoCollapse property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupAutoCollapseChanged;

		/// <summary>
		/// Occurs when the value of the GroupNodesSelectable property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupNodesSelectableChanged;

		/// <summary>
		/// Occurs when the value of the GroupAutoAllocate property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupAutoAllocateChanged;

		/// <summary>
		/// Occurs when the value of the GroupExtraHeight property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupExtraHeightChanged;

		/// <summary>
		/// Occurs when the value of the GroupHotTrack property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupHotTrackChanged;

		/// <summary>
		/// Occurs when the value of the GroupImageBox property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupImageBoxChanged;

		/// <summary>
		/// Occurs when the value of the GroupImageBoxBorder property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupImageBoxBorderChanged;

		/// <summary>
		/// Occurs when the value of the GroupImageBoxWidth property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupImageBoxWidthChanged;

		/// <summary>
		/// Occurs when the value of the GroupImageBoxGap property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupImageBoxGapChanged;

		/// <summary>
		/// Occurs when the value of the GroupImageBoxLineColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupImageBoxLineColorChanged;

		/// <summary>
		/// Occurs when the value of the GroupImageBoxBackColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupImageBoxBackColorChanged;

		/// <summary>
		/// Occurs when the value of the GroupImageBoxSelectedBackColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupImageBoxSelectedBackColorChanged;

		/// <summary>
		/// Occurs when the value of the GroupImageBoxColumnColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupImageBoxColumnColorChanged;

		/// <summary>
		/// Occurs when the value of the GroupImageBoxGradientBack property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupImageBoxGradientBackChanged;

		/// <summary>
		/// Occurs when the value of the GroupImageBoxGradientColoring property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupImageBoxGradientColoringChanged;

		/// <summary>
		/// Occurs when the value of the GroupImageBoxGradientAngle property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupImageBoxGradientAngleChanged;

		/// <summary>
		/// Occurs when the value of the GroupTextRenderingHint property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupTextRenderingHintChanged;

		/// <summary>
		/// Occurs when the value of the GroupExpandOnDragHover property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler GroupExpandOnDragHoverChanged;
		
		/// <summary>
		/// Occurs when the value of the VerticalScrollbar property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler VerticalScrollbarChanged;

		/// <summary>
		/// Occurs when the value of the HorizontalScrollbar property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler HorizontalScrollbarChanged;

		/// <summary>
		/// Occurs when the value of the VerticalGranularity property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler VerticalGranularityChanged;

		/// <summary>
		/// Occurs when the value of the EnableMouseWheel property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler EnableMouseWheelChanged;

		/// <summary>
		/// Occurs when the value of the ImageList property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler ImageListChanged;

		/// <summary>
		/// Occurs when the value of the ImageIndex property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler ImageIndexChanged;

		/// <summary>
		/// Occurs when the value of the ImageGapLeft property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler ImageGapLeftChanged;

		/// <summary>
		/// Occurs when the value of the ImageGapRight property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler ImageGapRightChanged;

		/// <summary>
		/// Occurs when the value of the SelectedImageIndex property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler SelectedImageIndexChanged;

		/// <summary>
		/// Occurs when the value of the ContextMenuNode property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler ContextMenuNodeChanged;

		/// <summary>
		/// Occurs when the value of the ContextMenuSpace property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler ContextMenuSpaceChanged;

		/// <summary>
		/// Occurs when the value of the ClickExpand property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler ClickExpandChanged;

		/// <summary>
		/// Occurs when the value of the DoubleClickExpand property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler DoubleClickExpandChanged;

		/// <summary>
		/// Occurs when the value of the AutoCollapse property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler AutoCollapseChanged;

		/// <summary>
		/// Occurs when the value of the ExtendToRight property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler ExtendToRightChanged;

		/// <summary>
		/// Occurs when the value of the AutoEdit property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler AutoEditChanged;

		/// <summary>
		/// Occurs when the value of the LabelEdit property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler LabelEditChanged;

		/// <summary>
		/// Occurs when the value of the NodesSelectable property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler NodesSelectableChanged;

		/// <summary>
		/// Occurs when the value of the CanUserExpandCollapse property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler CanUserExpandCollapseChanged;

		/// <summary>
		/// Occurs when the value of the ExpandOnDragHover property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler ExpandOnDragHoverChanged;

		/// <summary>
		/// Occurs when the value of the InstantUpdate property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler InstantUpdateChanged;

		/// <summary>
		/// Occurs when the value of the Tooltips property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler TooltipsChanged;

		/// <summary>
		/// Occurs when the value of the Infotips property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler InfotipsChanged;

		/// <summary>
		/// Occurs when the value of the InstantUpdate property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler IndicatorsChanged;

		/// <summary>
		/// Occurs when the value of the ViewControllers property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler ViewControllersChanged;

		/// <summary>
		/// Occurs when the value of the HotNode property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler HotNodeChanged;

		/// <summary>
		/// Occurs when the value of the VerticalNodeGap property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler VerticalNodeGapChanged;

		/// <summary>
		/// Occurs when the value of the MinimumNodeHeight property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler MinimumNodeHeightChanged;

		/// <summary>
		/// Occurs when the value of the MaximumNodeHeight property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler MaximumNodeHeightChanged;

		/// <summary>
		/// Occurs when the value of the TextRenderingHint property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler TextRenderingHintChanged;

		/// <summary>
		/// Occurs when the value of the HotBackColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler HotBackColorChanged;

		/// <summary>
		/// Occurs when the value of the HotForeColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler HotForeColorChanged;

		/// <summary>
		/// Occurs when the value of the SelectedBackColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler SelectedBackColorChanged;

		/// <summary>
		/// Occurs when the value of the SelectedNoFocusBackColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler SelectedNoFocusBackColorChanged;

		/// <summary>
		/// Occurs when the value of the SelectedForeColor property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler SelectedForeColorChanged;

		/// <summary>
		/// Occurs when the value of the HotFontStyle property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler HotFontStyleChanged;

		/// <summary>
		/// Occurs when the value of the UseHotFontStyle property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler UseHotFontStyleChanged;

		/// <summary>
		/// Occurs when the value of the SelectedFontStyle property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler SelectedFontStyleChanged;

		/// <summary>
		/// Occurs when the value of the UseSelectedFontStyle property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler UseSelectedFontStyleChanged;

		/// <summary>
		/// Occurs when the value of the SelectMode property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler SelectModeChanged;

		/// <summary>
		/// Occurs when the value of the PathSeparator property changes.
		/// </summary>
		[Category("Property Changed")]
		public event EventHandler PathSeparatorChanged;

		/// <summary>
		/// Occurs when user drags into space outside of any nodes.
		/// </summary>
		[Category("Nodes")]
		public event DragEventHandler ClientDragEnter; 

		/// <summary>
		/// Occurs when user drags away from space outside of any nodes.
		/// </summary>
		[Category("Nodes")]
		public event EventHandler ClientDragLeave; 

		/// <summary>
		/// Occurs when user drags over space outside of any nodes.
		/// </summary>
		[Category("Nodes")]
		public event DragEventHandler ClientDragOver; 

		/// <summary>
		/// Occurs when users drops onto space outside of any nodes.
		/// </summary>
		[Category("Nodes")]
		public event DragEventHandler ClientDragDrop; 

		/// <summary>
		/// Occurs when user drags into a Node.
		/// </summary>
		[Category("Nodes")]
		public event NodeDragDropEventHandler NodeDragEnter; 

		/// <summary>
		/// Occurs when user drags away from a Node.
		/// </summary>
		[Category("Nodes")]
		public event NodeEventHandler NodeDragLeave; 

		/// <summary>
		/// Occurs when user drags over a Node.
		/// </summary>
		[Category("Nodes")]
		public event NodeDragDropEventHandler NodeDragOver; 

		/// <summary>
		/// Occurs when users drops onto a Node.
		/// </summary>
		[Category("Nodes")]
		public event NodeDragDropEventHandler NodeDragDrop; 

		/// <summary>
		/// Occurs when user attempts to drag from a node.
		/// </summary>
		[Category("Nodes")]
		public event StartDragEventHandler NodeDrag; 

		/// <summary>
		/// Occurs just before a Node is to become selected.
		/// </summary>
		[Category("Nodes")]
		public event CancelNodeEventHandler BeforeSelect;

        /// <summary>
        /// Occurs just before a Node is to become deselected.
        /// </summary>
        [Category("Nodes")]
        public event NodeEventHandler BeforeDeselect;
        
        /// <summary>
		/// Occurs just after a Node has become selected.
		/// </summary>
		[Category("Nodes")]
		public event NodeEventHandler AfterSelect; 

		/// <summary>
		/// Occurs just after a Node has become deselected.
		/// </summary>
		[Category("Nodes")]
		public event NodeEventHandler AfterDeselect; 

		/// <summary>
		/// Occurs just before a Node is to change its checked state.
		/// </summary>
		[Category("Nodes")]
		public event CancelNodeEventHandler BeforeCheck; 

		/// <summary>
		/// Occurs just after a Node has changed its checked state.
		/// </summary>
		[Category("Nodes")]
		public event NodeEventHandler AfterCheck; 

		/// <summary>
		/// Occurs just before a Node is to be expanded.
		/// </summary>
		[Category("Nodes")]
		public event CancelNodeEventHandler BeforeExpand; 

		/// <summary>
		/// Occurs just after a Node has been expanded.
		/// </summary>
		[Category("Nodes")]
		public event NodeEventHandler AfterExpand; 

		/// <summary>
		/// Occurs just before a Node is to be collapsed.
		/// </summary>
		[Category("Nodes")]
		public event CancelNodeEventHandler BeforeCollapse; 

		/// <summary>
		/// Occurs just after a Node has been collapsed.
		/// </summary>
		[Category("Nodes")]
		public event NodeEventHandler AfterCollapse; 

		/// <summary>
		/// Occurs just before a Node label is edited.
		/// </summary>
		[Category("Nodes")]
		public event LabelEditEventHandler BeforeLabelEdit;

		/// <summary>
		/// Occurs just after a Node label has been edited.
		/// </summary>
		[Category("Nodes")]
		public event LabelEditEventHandler AfterLabelEdit;

        /// <summary>
        /// Occurs when after a change in selection has occured.
        /// </summary>
        [Category("Nodes")]
        public event EventHandler AfterSelectionChanged;

		/// <summary>
		/// Occurs when a label control is created.
		/// </summary>
		[Category("Nodes")]
		public event LabelControlEventHandler LabelControlCreated;

		/// <summary>
		/// Occurs when user right clicks on a Node.
		/// </summary>
		[Category("Nodes")]
		public event CancelNodeEventHandler ShowContextMenuNode;

		/// <summary>
		/// Occurs when user right clicks outside of any Node.
		/// </summary>
		[Category("Nodes")]
		public event CancelEventHandler ShowContextMenuSpace;

		/// <summary>
		/// Occurs when user drags into a Node.
		/// </summary>
		[Category("Group")]
		public event NodeDragDropEventHandler GroupDragEnter; 

		/// <summary>
		/// Occurs when user drags away from a Node.
		/// </summary>
		[Category("Group")]
		public event NodeEventHandler GroupDragLeave; 

		/// <summary>
		/// Occurs when user drags over a Node.
		/// </summary>
		[Category("Group")]
		public event NodeDragDropEventHandler GroupDragOver; 

		/// <summary>
		/// Occurs when users drops onto a Node.
		/// </summary>
		[Category("Group")]
		public event NodeDragDropEventHandler GroupDragDrop; 

		static TreeControl()
		{
			// Load the set of indicator images from resources
			_indicatorImages = ResourceHelper.LoadBitmapStrip(typeof(TreeControl), 
															  "Crownwood.DotNetMagic.Resources.Indicators.bmp", 
															  new Size(INDICATOR_WIDTH, INDICATOR_HEIGHT),
															  new Point(0,0));
		}
		
		/// <summary>
		/// Initializes a new instance of the TreeControl class.
		/// </summary>
		public TreeControl()
		{
			// NAG processing
			NAG.NAG_Start();
			
			// Prevent flicker with double buffering and all painting inside WM_PAINT
			SetStyle(ControlStyles.ResizeRedraw |
                     ControlStyles.OptimizedDoubleBuffer | 
					 ControlStyles.AllPaintingInWmPaint |
					 ControlStyles.UserPaint, true);

			// We want the double click to come through as separate event
			SetStyle(ControlStyles.StandardDoubleClick, true);

			// Create starting objects
			_themeTreeView = new ThemeHelper(this, "TreeView");
			_themeCheckbox = new ThemeHelper(this, "Button");
			_colorDetails = new ColorDetails();
			_rootNodes = new NodeCollection(this);
			_borderIndent = new IndentPaddingEdges();
			
			// Hook into events for sub objects
			_borderIndent.IndentChanged += new EventHandler(OnBorderIndentChanged);
			_themeTreeView.ThemeOpened += new EventHandler(OnThemeChanged);
			_themeTreeView.ThemeClosed += new EventHandler(OnThemeChanged);
			_themeCheckbox.ThemeOpened += new EventHandler(OnThemeChanged);
			_themeCheckbox.ThemeClosed += new EventHandler(OnThemeChanged);

			// Create the node/collection drawing objects
			_defaultNodeVC = new DefaultNodeVC();
			_defaultCollectionVC = new DefaultCollectionVC();
			_groupNodeVC = new GroupNodeVC();
			_groupCollectionVC = new GroupCollectionVC();
			
			// Create timers
			_dragHoverTimer = new Timer();
			_dragHoverTimer.Interval = 1000;
			_dragHoverTimer.Tick += new EventHandler(OnDragHoverTimeout);
			_dragBumpTimer = new Timer();
			_dragBumpTimer.Interval = 250;
			_dragBumpTimer.Tick += new EventHandler(OnDragBumpTimeout);
			_autoEditTimer = new Timer();
			_autoEditTimer.Interval = 500;
			_autoEditTimer.Tick += new EventHandler(OnAutoEditTimeout);
			_infotipTimer = new Timer();
			_infotipTimer.Interval = 500;
			_infotipTimer.Tick += new EventHandler(OnInfoTipTimeout);

			// Create child controls
			CreateChildControls();

			// Initialise fields not exposed
			_offset = Point.Empty;
			_displayNodes = new ArrayList();
			_selected = new Hashtable();
			_nodeKeys = new Hashtable();
			InternalResetFontHeight();
			InternalResetFontBoldItalic();
			InternalResetGroupFontHeight();
			InternalResetGroupFontBoldItalic();
			InternalResetHotPoint();    

			// Define initial internal state
			InvalidateNodeDrawing();
			InvalidateInnerRectangle();

			// Reset exposed properties to default values
			ResetAllProperties();
			ResetImageList();
			ResetImageIndex();
			ResetSelectedImageIndex();
			ResetContextMenuNode();
			ResetContextMenuSpace();
			ResetGroupColoring();
			ResetPathSeparator();
			ResetAutoEdit();
			ResetGroupAutoEdit();
			ResetInstantUpdate();
			ResetTooltips();
			ResetInfotips();
            ResetMouseWheelUnits();
            ResetMouseUnits();
            ResetIgnoreEnsureDisplayedH();
            ResetIgnoreEnsureDisplayedV();

			// Need notification when system colors change
			Microsoft.Win32.SystemEvents.UserPreferenceChanged += 
				new UserPreferenceChangedEventHandler(OnPreferenceChanged);
		}

		/// <summary>
		/// Gets and sets the view controller to use for drawing individual nodes.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public INodeVC NodeVC
		{
			get { return _nodeVC; }

			set
			{
				// Inform current VC that it is being removed
				if (_nodeVC != null)
					_nodeVC.Detaching(this);

				// We never allow a null value
				if (value == null)
				{
					// So use ViewController value
					switch(ViewControllers)
					{
						case ViewControllers.Group:
							_nodeVC = _groupNodeVC;
							break;
						case ViewControllers.Default:
							_nodeVC = _defaultNodeVC;
							break;
					}
				}
				else
					_nodeVC = value;

				// Give collection change to perform initial processing
				_nodeVC.Initialize(this);

				// Must recalculate drawing of all nodes for new font
				MarkAllNodeSizesDirty();

				// Must recalculate all the drawing using new value
				InvalidateNodeDrawing();
			}
		}

		/// <summary>
		/// Resets the NodeVC property to its default value.
		/// </summary>
		public void ResetNodeVC()
		{
			NodeVC = null;
		}

		/// <summary>
		/// Gets and sets the view controller to use for drawing a collection of nodes..
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public INodeCollectionVC CollectionVC
		{
			get { return _collectionVC; }

			set
			{
				// Inform current VC that it is being removed
				if (_collectionVC != null)
					_collectionVC.Detaching(this);
			
				// We never allow a null value
				if (value == null)
				{
					// So use ViewController value
					switch(ViewControllers)
					{
						case ViewControllers.Group:
							_collectionVC = _defaultCollectionVC;
							break;
						case ViewControllers.Default:
							_collectionVC = _groupCollectionVC;
							break;
					}
				}
				else
					_collectionVC = value;

				// Give collection change to perform initial processing
				_collectionVC.Initialize(this);

				// Must recalculate drawing of all nodes for new font
				MarkAllNodeSizesDirty();

				// Must recalculate all the drawing using new value
				InvalidateNodeDrawing();
			}
		}

		/// <summary>
		/// Resets the CollectionVC property to its default value.
		/// </summary>
		public void ResetCollectionVC()
		{
			CollectionVC = null;
		}

		/// <summary>
		/// Gets and sets the view controllers that should be used.
		/// </summary>
		[Category("Nodes")]
		[Description("Which view controllers should be used.")]
		[DefaultValue(typeof(ViewControllers), "Default")]
		public ViewControllers ViewControllers
		{
			get { return _viewControllers; }

			set
			{
				// Use new value
				_viewControllers = value;

				// Swutch to required VC's
				switch(_viewControllers)
				{
					case ViewControllers.Group:
						NodeVC = _groupNodeVC;
						CollectionVC = _groupCollectionVC;
						break;
					case ViewControllers.Default:
						NodeVC = _defaultNodeVC;
						CollectionVC = _defaultCollectionVC;
						break;
				}

				OnViewControllersChanged();

				// Must recalculate drawing of all nodes for new font
				MarkAllNodeSizesDirty();

				// Must recalculate all the drawing using new value
				InvalidateNodeDrawing();
			}
		}

		/// <summary>
		/// Resets the ViewControllers property to its default value.
		/// </summary>
		public void ResetViewControllers()
		{
			ViewControllers = ViewControllers.Default;
		}

		/// <summary>
		/// Gets the collection of currently selected Nodes.
		/// </summary>
		[Browsable(false)]
		public SelectedNodeCollection SelectedNodes
		{
			get { return new SelectedNodeCollection(_selected); }
		}

		/// <summary>
		/// Gets the collection of currently selected Nodes.
		/// </summary>
		[Browsable(false)]
		public Node SelectedNode
		{
			get 
			{
				// If we have a focus node and that node is selected
				if ((_focusNode != null) && (_focusNode.IsSelected))
				{
					// Then we return the focused node
					return _focusNode;
				}
			
				// Otherwise, return the first entry in the hashtable
				if (_selected.Count != 0)
					foreach(Node n in _selected.Keys)
						return n;
				
				// Nothing selected!
				return null; 
			}
			
			set
			{
				// Can only select a valid node reference
				if (value != null)
				{
					// If an entry is allowed to be selected
					if (SelectMode != SelectMode.None)
					{
						// And this node is allowed to be selected
						if (value.VC.CanSelectNode(this, value))
						{
							// Then try and select it
							SingleSelect(value);
						}
					}
				}
			}
		}

		/// <summary>
		/// Deselect all the currently selected Nodes.
		/// </summary>
		public void DeselectAll()
		{
            // Generate the before deselect event
            foreach (Node n in _selected.Keys)
                OnBeforeDeselect(n);

            // Generate the after deselect event
			foreach(Node n in _selected.Keys)
				OnAfterDeselect(n);
            
            ClearSelection();

            //DWC Change
            OnAfterSelectionChanged();
        }

		/// <summary>
		/// Gets and sets the node that has foucs when the control has focus.
		/// </summary>
		[Browsable(false)]
		public Node FocusNode
		{
			get { return _focusNode; }

			set
			{
				// Use existing internal helper
				SetFocusNode(value);
			}
		}


		/// <summary>
		/// Gets the collection of root nodes in the tree.
		/// </summary>
		[Category("Nodes")]
		[Description("The collection of root nodes.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor("Crownwood.DotNetMagic.Controls.NodeCollectionEditor", typeof(UITypeEditor))]
		public NodeCollection Nodes
		{
			get { return _rootNodes; }
		}
        
        /// <summary>
        /// Gets the context menu strip to use on right clicking.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ContextMenuStrip ContextMenuStrip
        {
            get { return null; }

            set
            {
            }
        }

		/// <summary>
		/// Gets the context menu strip to use on right clicking a Node.
		/// </summary>
		[Category("Nodes")]
		[Description("ContextMenuStrip to show on right clicking a Node.")]
		[DefaultValue(null)]
        public ContextMenuStrip ContextMenuNode
		{
			get { return _contextMenuNode; }
			
			set
			{
				if (_contextMenuNode != value)
				{	
					_contextMenuNode = value;
					OnContextMenuNodeChanged();
				}
			}
		}

		/// <summary>
		/// Resets the ContextMenuNode property to its default value.
		/// </summary>
		public void ResetContextMenuNode()
		{
			ContextMenuNode = null;
		}

		/// <summary>
		/// Gets the context menu strip to use on right clicking outside of any Node.
		/// </summary>
		[Category("Nodes")]
		[Description("ContextMenuStrip to show on right clicking outside of any Node.")]
		[DefaultValue(null)]
        public ContextMenuStrip ContextMenuSpace
		{
			get { return _contextMenuSpace; }
			
			set
			{
				if (_contextMenuSpace != value)
				{	
					_contextMenuSpace = value;
					OnContextMenuSpaceChanged();
				}
			}
		}

		/// <summary>
		/// Resets the ContextMenuSpace property to its default value.
		/// </summary>
		public void ResetContextMenuSpace()
		{
			ContextMenuSpace = null;
		}

		/// <summary>
		/// Gets or sets the delimiter string that the node path uses.
		/// </summary>
		[Category("Nodes")]
		[Description("Delimiter string that the node path uses.")]
		[DefaultValue(@"\")]
		public string PathSeparator
		{
			get { return _pathSeparator; }
			
			set
			{
				if (_pathSeparator != value)
				{	
					_pathSeparator = value;
					OnPathSeparatorChanged();
				}
			}
		}

		/// <summary>
		/// Resets the PathSeparator property to its default value.
		/// </summary>
		public void ResetPathSeparator()
		{
			PathSeparator = @"\";
		}

		/// <summary>
		/// Gets and sets the background color.
		/// </summary>
		[DefaultValue(typeof(Color), "Window")]
		public override Color BackColor
		{
			get { return base.BackColor; }
			set { base.BackColor = value; }
		}

		/// <summary>
		/// Resets the BackColor property to its default value.
		/// </summary>
		public override void ResetBackColor()
		{
			BackColor = SystemColors.Window;
		}

		/// <summary>
		/// Gets and sets the drawing style for the control border.
		/// </summary>
		[Category("Border")]
		[Description("Specifies indentation between border and inside drawing")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public IndentPaddingEdges BorderIndent
		{
			get { return _borderIndent; }
		}

		/// <summary>
		/// Resets the BorderIndent property to its default value.
		/// </summary>
		public void ResetBorderIndent()
		{
			_borderIndent.ResetTop();
			_borderIndent.ResetBottom();
			_borderIndent.ResetLeft();
			_borderIndent.ResetRight();
		}

		/// <summary>
		/// Gets and sets the drawing style for the control border.
		/// </summary>
		[Category("Border")]
		[Description("Indicates whether or not the tree control should have a border.")]
		[DefaultValue(typeof(TreeBorderStyle), "Theme")]
		public TreeBorderStyle BorderStyle
		{
			get { return _borderStyle; }

			set
			{
				if (_borderStyle != value)
				{
					_borderStyle = value;

					// Cache new values needed for drawing border
					using(Graphics g = this.CreateGraphics())
						UpdateBorderCache(g);

					// Notify change event
					OnBorderStyleChanged();

					// Must recalculate the inner drawing rectangle to reflect changing border
					InvalidateInnerRectangle();
				}
			}
		}

		/// <summary>
		/// Resets the BorderStyle property to its default value.
		/// </summary>
		public void ResetBorderStyle()
		{
			BorderStyle = TreeBorderStyle.Theme;
		}

		/// <summary>
		/// Gets and sets the drawing color for the control border.
		/// </summary>
		[Category("Border")]
		[Description("The color used to draw the non-3D border styles.")]
		[DefaultValue(typeof(Color), "WindowText")]
		public Color BorderColor
		{
			get { return _borderColor; }

			set
			{
				if (_borderColor != value)
				{
					_borderColor = value;
					OnBorderColorChanged();
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the BorderColor property to its default value.
		/// </summary>
		public void ResetBorderColor()
		{
			BorderColor = SystemColors.WindowText;
		}

		/// <summary>
		/// Gets access to the Panel control display in corner when both scroll bars are shown.
		/// </summary>
		[Browsable(false)]
		public Panel Corner
		{
			get { return _corner; }
		}

		/// <summary>
		/// Gets and sets the width of lines.
		/// </summary>
		[Category("Lines and Boxes")]
		[Description("Defines the width of lines.")]
		[DefaultValue(1)]
		public int LineWidth
		{
			get { return _lineWidth; }

			set
			{
				if (_lineWidth != value)
				{
					_lineWidth = value;

					// Clear cached resources for lines and boxes
					ClearLineBoxCache();

					// Notify change event
					OnLineWidthChanged();
	
					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the LineWidth property to its default value.
		/// </summary>
		public void ResetLineWidth()
		{
			LineWidth = 1;
		}

		/// <summary>
		/// Gets and sets the color of lines.
		/// </summary>
		[Category("Lines and Boxes")]
		[Description("Defines the color of lines.")]
		[DefaultValue(typeof(Color), "GrayText")]
		public Color LineColor
		{
			get { return _lineColor; }

			set
			{
				if (_lineColor != value)
				{
					_lineColor = value;

					// Clear cached resources for lines and boxes
					ClearLineBoxCache();

					// Notify change event
					OnLineColorChanged();
	
					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the LineColor property to its default value.
		/// </summary>
		public void ResetLineColor()
		{
			LineColor = SystemColors.GrayText;
		}

		/// <summary>
		/// Gets and sets the style used for lines
		/// </summary>
		[Category("Lines and Boxes")]
		[Description("Defines the style used for lines.")]
		[DefaultValue(typeof(LineDashStyle), "Dot")]
		public LineDashStyle LineDashStyle
		{
			get { return _lineDashStyle; }

			set
			{
				if (_lineDashStyle != value)
				{
					_lineDashStyle = value;

					// Clear cached resources for lines and boxes
					ClearLineBoxCache();

					// Notify change event
					OnLineDashStyleChanged();
	
					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the LineDashStyle property to its default value.
		/// </summary>
		public void ResetLineDashStyle()
		{
			LineDashStyle = LineDashStyle.Dot;
		}

		/// <summary>
		/// Gets and sets the length used for drawing box.
		/// </summary>
		[Category("Lines and Boxes")]
		[Description("Defines length used for drawing box.")]
		[DefaultValue(9)]
		public int BoxLength
		{
			get { return _boxLength; }

			set
			{
				if (_boxLength != value)
				{
					_boxLength = value;

					// Must be a minimum of 1 to be useful
					if (_boxLength < 1)
						_boxLength = 1;

					// Notify change event
					OnBoxLengthChanged();
	
					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the BoxLength property to its default value.
		/// </summary>
		public void ResetBoxLength()
		{
			BoxLength = 9;
		}

		/// <summary>
		/// Gets and sets how Boxes are drawn.
		/// </summary>
		[Category("Lines and Boxes")]
		[Description("Determines how Boxes are drawn.")]
		[DefaultValue(typeof(DrawStyle), "Themed")]
		public DrawStyle BoxDrawStyle
		{
			get { return _boxDrawStyle; }

			set
			{
				if (_boxDrawStyle != value)
				{
					_boxDrawStyle = value;

					// Notify change event
					OnBoxDrawStyleChanged();
	
					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the BoxDrawStyle property to its default value.
		/// </summary>
		public void ResetBoxDrawStyle()
		{
			BoxDrawStyle = DrawStyle.Themed;
		}

		/// <summary>
		/// Gets and sets a value to indicate if box is drawn when node has no children.
		/// </summary>
		[Category("Lines and Boxes")]
		[Description("Should box be drawn when node has no children.")]
		[DefaultValue(false)]
		public bool BoxShownAlways
		{
			get { return _boxShownAlways; }

			set
			{
				if (_boxShownAlways != value)
				{
					_boxShownAlways = value;

					// Notify change event
					OnBoxShownAlwaysChanged();
	
					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the BoxShownAlways property to its default value.
		/// </summary>
		public void ResetBoxShownAlways()
		{
			BoxShownAlways = false;
		}

		/// <summary>
		/// Gets and sets the color of box signs.
		/// </summary>
		[Category("Lines and Boxes")]
		[Description("Defines the color of box signs.")]
		[DefaultValue(typeof(Color), "WindowText")]
		public Color BoxSignColor
		{
			get { return _boxSignColor; }

			set
			{
				if (_boxSignColor != value)
				{
					_boxSignColor = value;

					// Clear cached resources for lines and boxes
					ClearLineBoxCache();

					// Notify change event
					OnBoxSignColorChanged();
	
					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the BoxSignColor property to its default value.
		/// </summary>
		public void ResetBoxSignColor()
		{
			BoxSignColor = SystemColors.WindowText;
		}

		/// <summary>
		/// Gets and sets the color of box borders.
		/// </summary>
		[Category("Lines and Boxes")]
		[Description("Defines the color of box borders.")]
		[DefaultValue(typeof(Color), "GrayText")]
		public Color BoxBorderColor
		{
			get { return _boxBorderColor; }

			set
			{
				if (_boxBorderColor != value)
				{
					_boxBorderColor = value;

					// Clear cached resources for lines and boxes
					ClearLineBoxCache();

					// Notify change event
					OnBoxBorderColorChanged();
	
					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the BoxBorderColor property to its default value.
		/// </summary>
		public void ResetBoxBorderColor()
		{
			BoxBorderColor = SystemColors.GrayText;
		}

		/// <summary>
		/// Gets and sets the color of box borders.
		/// </summary>
		[Category("Lines and Boxes")]
		[Description("Defines the color of box borders.")]
		[DefaultValue(typeof(Color), "Window")]
		public Color BoxInsideColor
		{
			get { return _boxInsideColor; }

			set
			{
				if (_boxInsideColor != value)
				{
					_boxInsideColor = value;

					// Clear cached resources for lines and boxes
					ClearLineBoxCache();

					// Notify change event
					OnBoxInsideColorChanged();
	
					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the BoxInsideColor property to its default value.
		/// </summary>
		public void ResetBoxInsideColor()
		{
			BoxInsideColor = SystemColors.Window;
		}

		/// <summary>
		/// Gets and sets when Boxes are displayed.
		/// </summary>
		[Category("Lines and Boxes")]
		[Description("Determine when Boxes are displayed.")]
		[DefaultValue(typeof(LineBoxVisibility), "Everywhere")]
		public LineBoxVisibility BoxVisibility
		{
			get { return _boxVisibility; }

			set
			{
				if (_boxVisibility != value)
				{
					_boxVisibility = value;

					// Notify change event
					OnBoxVisibilityChanged();
	
					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the BoxVisibility property to its default value.
		/// </summary>
		public void ResetBoxVisibility()
		{
			BoxVisibility = LineBoxVisibility.Everywhere;
		}

		/// <summary>
		/// Gets and sets when Lines are displayed.
		/// </summary>
		[Category("Lines and Boxes")]
		[Description("Determine when Lines are displayed.")]
		[DefaultValue(typeof(LineBoxVisibility), "Everywhere")]
		public LineBoxVisibility LineVisibility
		{
			get { return _lineVisibility; }

			set
			{
				if (_lineVisibility != value)
				{
					_lineVisibility = value;

					// Notify change event
					OnLineVisibilityChanged();
	
					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the LineVisibility property to its default value.
		/// </summary>
		public void ResetLineVisibility()
		{
			LineVisibility = LineBoxVisibility.Everywhere;
		}

		/// <summary>
		/// Gets and sets the width of the lines column.
		/// </summary>
		[Category("Lines and Boxes")]
		[Description("Defines the width of the lines column.")]
		[DefaultValue(19)]
		public int ColumnWidth
		{
			get { return _columnWidth; }

			set
			{
				if (_columnWidth != value)
				{
					_columnWidth = value;

					// Notify change event
					OnColumnWidthChanged();
	
					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the ColumnWidth property to its default value.
		/// </summary>
		public void ResetColumnWidth()
		{
			ColumnWidth = 19;
		}

		/// <summary>
		/// Gets and sets the style of checkboxes.
		/// </summary>
		[Category("Check")]
		[Description("Define the style of checkboxes.")]
		[DefaultValue(typeof(CheckStates), "None")]
		public CheckStates CheckStates
		{
			get { return _checkStates; }

			set
			{
				if (_checkStates != value)
				{
					_checkStates = value;

					// Notify change event
					OnCheckStatesChanged();
	
					// Must recalculate drawing of all nodes for new font
					MarkAllNodeSizesDirty();

					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the CheckStates property to its default value.
		/// </summary>
		public void ResetCheckStates()
		{
			CheckStates = CheckStates.None;
		}

		/// <summary>
		/// Gets and sets the size for drawing a checkbox..
		/// </summary>
		[Category("Check")]
		[Description("Define size for drawing a checkbox.")]
		[DefaultValue(13)]
		public int CheckLength
		{
			get { return _checkLength; }

			set
			{
				if (_checkLength != value)
				{
					_checkLength = value;

					// Must be a minimum of 9 to be useful
					if (_checkLength < 9)
						_checkLength = 9;

					// Notify change event
					OnCheckLengthChanged();
	
					// Must recalculate drawing of all nodes for new font
					MarkAllNodeSizesDirty();

					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the CheckLength property to its default value.
		/// </summary>
		public void ResetCheckLength()
		{
			CheckLength = 13;
		}

		/// <summary>
		/// Gets and sets the pixel gap between start of node and start of check.
		/// </summary>
		[Category("Check")]
		[Description("Pixel gap between start of node and start of check.")]
		[DefaultValue(1)]
		public int CheckGapLeft
		{
			get { return _checkGapLeft; }

			set
			{
				if (_checkGapLeft != value)
				{
					_checkGapLeft = value;

					// We limit check to ensure number is positive
					if (_checkGapLeft < 0)
						_checkGapLeft = 0;

					// Notify change event
					OnCheckGapLeftChanged();

					// Must recalculate drawing of all nodes for new font
					MarkAllNodeSizesDirty();

					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the CheckGapLeft property to its default value.
		/// </summary>
		public void ResetCheckGapLeft()
		{
			CheckGapLeft = 1;
		}

		/// <summary>
		/// Gets and sets the pixel gap between end of check and start of image.
		/// </summary>
		[Category("Check")]
		[Description("Pixel gap between end of check and start of image.")]
		[DefaultValue(3)]
		public int CheckGapRight
		{
			get { return _checkGapRight; }

			set
			{
				if (_checkGapRight != value)
				{
					_checkGapRight = value;

					// We limit check to ensure number is positive
					if (_checkGapRight < 0)
						_checkGapRight = 0;

					// Notify change event
					OnCheckGapRightChanged();

					// Must recalculate drawing of all nodes for new font
					MarkAllNodeSizesDirty();

					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the CheckGapRight property to its default value.
		/// </summary>
		public void ResetCheckGapRight()
		{
			CheckGapRight = 3;
		}

		/// <summary>
		/// Gets and sets how checkboxes are drawn.
		/// </summary>
		[Category("Check")]
		[Description("Define how checkboxes are drawn.")]
		[DefaultValue(typeof(DrawStyle), "Themed")]
		public DrawStyle CheckDrawStyle
		{
			get { return _checkDrawStyle; }

			set
			{
				if (_checkDrawStyle != value)
				{
					_checkDrawStyle = value;

					// Notify change event
					OnCheckDrawStyleChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}				
			}
		}

		/// <summary>
		/// Resets the CheckDrawStyle property to its default value.
		/// </summary>
		public void ResetCheckDrawStyle()
		{
			CheckDrawStyle = DrawStyle.Themed;
		}

		/// <summary>
		/// Gets and sets the width of the check border.
		/// </summary>
		[Category("Check")]
		[Description("Width of the check border.")]
		[DefaultValue(1)]
		public int CheckBorderWidth
		{
			get { return _checkBorderWidth; }

			set
			{
				if (_checkBorderWidth != value)
				{
					_checkBorderWidth = value;

					// Notify change event
					OnCheckBorderWidthChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}				
			}
		}

		/// <summary>
		/// Resets the CheckBorderWidth property to its default value.
		/// </summary>
		public void ResetCheckBorderWidth()
		{
			CheckBorderWidth = 1;
		}

		/// <summary>
		/// Gets and sets the color of check borders.
		/// </summary>
		[Category("Check")]
		[Description("Defines the color of check borders.")]
		[DefaultValue(typeof(Color), "ControlDarkDark")]
		public Color CheckBorderColor
		{
			get { return _checkBorderColor; }

			set
			{
				if (_checkBorderColor != value)
				{
					_checkBorderColor = value;

					// Clear cached resources for checks
					ClearCheckCache();

					// Notify change event
					OnCheckBorderColorChanged();
	
					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the CheckBorderColor property to its default value.
		/// </summary>
		public void ResetCheckBorderColor()
		{
			CheckBorderColor = SystemColors.ControlDarkDark;
		}

		/// <summary>
		/// Gets and sets the color inside the check box.
		/// </summary>
		[Category("Check")]
		[Description("Defines the color inside the check box.")]
		[DefaultValue(typeof(Color), "Window")]
		public Color CheckInsideColor
		{
			get { return _checkInsideColor; }

			set
			{
				if (_checkInsideColor != value)
				{
					_checkInsideColor = value;

					// Clear cached resources for checks
					ClearCheckCache();

					// Notify change event
					OnCheckInsideColorChanged();
	
					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the CheckInsideColor property to its default value.
		/// </summary>
		public void ResetCheckInsideColor()
		{
			CheckInsideColor = SystemColors.Window;
		}

		/// <summary>
		/// Gets and sets the color inside the check box when hot tracked.
		/// </summary>
		[Category("Check")]
		[Description("Defines the color inside the check box when hot tracked.")]
		[DefaultValue(typeof(Color), "Window")]
		public Color CheckInsideHotColor
		{
			get { return _checkInsideHotColor; }

			set
			{
				if (_checkInsideHotColor != value)
				{
					_checkInsideHotColor = value;

					// Clear cached resources for checks
					ClearCheckCache();

					// Notify change event
					OnCheckInsideHotColorChanged();
	
					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the CheckInsideHotColor property to its default value.
		/// </summary>
		public void ResetCheckInsideHotColor()
		{
			CheckInsideHotColor = SystemColors.Window;
		}

		/// <summary>
		/// Gets and sets the ticked color inside the check box.
		/// </summary>
		[Category("Check")]
		[Description("Defines the ticked color inside the check box.")]
		[DefaultValue(typeof(Color), "ControlText")]
		public Color CheckTickColor
		{
			get { return _checkTickColor; }

			set
			{
				if (_checkTickColor != value)
				{
					_checkTickColor = value;

					// Clear cached resources for checks
					ClearCheckCache();

					// Notify change event
					OnCheckTickColorChanged();
	
					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the CheckTickColor property to its default value.
		/// </summary>
		public void ResetCheckTickColor()
		{
			CheckTickColor = SystemColors.ControlText;
		}

		/// <summary>
		/// Gets and sets the ticked color inside the check box when hot tracking.
		/// </summary>
		[Category("Check")]
		[Description("Defines the ticked color inside the check box when hot tracking.")]
		[DefaultValue(typeof(Color), "ControlText")]
		public Color CheckTickHotColor
		{
			get { return _checkTickHotColor; }

			set
			{
				if (_checkTickHotColor != value)
				{
					_checkTickHotColor = value;

					// Clear cached resources for checks
					ClearCheckCache();

					// Notify change event
					OnCheckTickHotColorChanged();
	
					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the CheckTickHotColor property to its default value.
		/// </summary>
		public void ResetCheckTickHotColor()
		{
			CheckTickHotColor = SystemColors.ControlText;
		}

		/// <summary>
		/// Gets and sets the mixed color inside the check box.
		/// </summary>
		[Category("Check")]
		[Description("Defines the mixed color inside the check box.")]
		[DefaultValue(typeof(Color), "ControlText")]
		public Color CheckMixedColor
		{
			get { return _checkMixedColor; }

			set
			{
				if (_checkMixedColor != value)
				{
					_checkMixedColor = value;

					// Clear cached resources for checks
					ClearCheckCache();

					// Notify change event
					OnCheckMixedColorChanged();
	
					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the CheckMixedColor property to its default value.
		/// </summary>
		public void ResetCheckMixedColor()
		{
			CheckMixedColor = SystemColors.ControlText;
		}

		/// <summary>
		/// Gets and sets the mixed color inside the check box when hot tracking.
		/// </summary>
		[Category("Check")]
		[Description("Defines the mixed color inside the check box when hot tracking.")]
		[DefaultValue(typeof(Color), "ControlText")]
		public Color CheckMixedHotColor
		{
			get { return _checkMixedHotColor; }

			set
			{
				if (_checkMixedHotColor != value)
				{
					_checkMixedHotColor = value;

					// Clear cached resources for checks
					ClearCheckCache();

					// Notify change event
					OnCheckMixedHotColorChanged();
	
					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the CheckMixedHotColor property to its default value.
		/// </summary>
		public void ResetCheckMixedHotColor()
		{
			CheckMixedHotColor = SystemColors.ControlText;
		}

        /// <summary>
        /// Gets and sets the number of pixels to scroll when the mouse wheel is used.
        /// </summary>
        [Category("Scrolling")]
        [Description("Number of pixels to scroll when the mouse wheel is used.")]
        [DefaultValue(10)]
        public int MouseWheelUnits
        {
            get { return _mouseWheelUnits; }
            set { _mouseWheelUnits = value; }
        }

        /// <summary>
        /// Resets the MouseWheelUnits property to its default value.
        /// </summary>
        public void ResetMouseWheelUnits()
        {
            MouseWheelUnits = 10;
        }

        /// <summary>
        /// Gets and sets the number of pixels to scroll when the scrollbar button is used.
        /// </summary>
        [Category("Scrolling")]
        [Description("Number of pixels to scroll when scrollbar button is used.")]
        [DefaultValue(10)]
        public int MouseUnits
        {
            get { return _mouseUnits; }
            set { _mouseUnits = value; }
        }

        /// <summary>
        /// Resets the MouseUnits property to its default value.
        /// </summary>
        public void ResetMouseUnits()
        {
            MouseUnits = 10;
        }

        /// <summary>
		/// Gets and sets the vertical scrolling granularity.
		/// </summary>
		[Category("Scrolling")]
		[Description("Defines the vertical scrolling granularity.")]
		[DefaultValue(typeof(VerticalGranularity), "Node")]
		public VerticalGranularity VerticalGranularity
		{
			get { return _granularity; }

			set
			{
				if (_granularity != value)
				{
					_granularity = value;

					// Reset the scrolling offset
					_offset = Point.Empty;

					// Notify change event
					OnVerticalGranularityChanged();
	
					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the VerticalGranularity property to its default value.
		/// </summary>
		public void ResetVerticalGranularity()
		{
			VerticalGranularity = VerticalGranularity.Node;
		}

        /// <summary>
        /// Gets and sets if the horizontal scrolling be left unchanged when ensuring a node is displayed.
        /// </summary>
        [Category("Scrolling")]
        [Description("Should horizontal scrolling be left unchanged when ensuring a node is displayed.")]
        [DefaultValue(false)]
        public bool IgnoreEnsureDisplayedH
        {
            get { return _ignoreEnsureDisplayedH; }
            set { _ignoreEnsureDisplayedH = value;  }
        }

        /// <summary>
        /// Resets the IgnoreEnsureDisplayedH property to its default value.
        /// </summary>
        public void ResetIgnoreEnsureDisplayedH()
        {
            IgnoreEnsureDisplayedH = false;
        }

        /// <summary>
        /// Gets and sets if the vertical scrolling be left unchanged when ensuring a node is displayed.
        /// </summary>
        [Category("Scrolling")]
        [Description("Should vertical scrolling be left unchanged when ensuring a node is displayed.")]
        [DefaultValue(false)]
        public bool IgnoreEnsureDisplayedV
        {
            get { return _ignoreEnsureDisplayedV; }
            set { _ignoreEnsureDisplayedV = value; }
        }

        /// <summary>
        /// Resets the IgnoreEnsureDisplayedV property to its default value.
        /// </summary>
        public void ResetIgnoreEnsureDisplayedV()
        {
            IgnoreEnsureDisplayedV = false;
        }

        /// <summary>
		/// Gets and sets the vertical scrollbar is visibility.
		/// </summary>
		[Category("Scrolling")]
		[Description("Decide when the vertical scrollbar is shown.")]
		[DefaultValue(typeof(ScrollVisibility), "WhenNeeded")]
		public ScrollVisibility VerticalScrollbar
		{
			get { return _vVisibility; }

			set
			{
				if (_vVisibility != value)
				{
					_vVisibility = value;

					// Notify change event
					OnVerticalScrollbarChanged();
	
					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the VertScrollbar property to its default value.
		/// </summary>
		public void ResetVerticalScrollbar()
		{
			VerticalScrollbar = ScrollVisibility.WhenNeeded;
		}

		/// <summary>
		/// Gets and sets the horizontal scrollbar is visibility.
		/// </summary>
		[Category("Scrolling")]
		[Description("Decide when the horizontal scrollbar is shown.")]
		[DefaultValue(typeof(ScrollVisibility), "WhenNeeded")]
		public ScrollVisibility HorizontalScrollbar
		{
			get { return _hVisibility; }

			set
			{
				if (_hVisibility != value)
				{
					_hVisibility = value;

					// Notify change event
					OnHorizontalScrollbarChanged();
	
					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the HorizontalScrollbar property to its default value.
		/// </summary>
		public void ResetHorizontalScrollbar()
		{
			HorizontalScrollbar = ScrollVisibility.WhenNeeded;
		}

		/// <summary>
		/// Gets and sets a value indicating if mouse wheel should scroll.
		/// </summary>
		[Category("Scrolling")]
		[Description("Should mouse wheel be used to scroll vertically.")]
		[DefaultValue(true)]
		public bool EnableMouseWheel
		{
			get { return _enableMouseWheel; }

			set
			{
				if (_enableMouseWheel != value)
				{
					_enableMouseWheel = value;

					// Notify change event
					OnEnableMouseWheelChanged();
				}
			}
		}

		/// <summary>
		/// Resets the EnableMouseWheel property to its default value.
		/// </summary>
		public void ResetEnableMouseWheel()
		{
			EnableMouseWheel = true;
		}

		/// <summary>
		/// Gets and sets a value indicating if group level nodes can be selected.
		/// </summary>
		[Category("Group")]
		[Description("Can group level nodes be selected.")]
		[DefaultValue(true)]
		public bool GroupNodesSelectable
		{
			get { return _groupNodesSelectable; }

			set
			{
				if (_groupNodesSelectable != value)
				{
					_groupNodesSelectable = value;

					// Notify change event
					OnGroupNodesSelectableChanged();
				}
			}
		}

		/// <summary>
		/// Resets the GroupNodesSelectable property to its default value.
		/// </summary>
		public void ResetGroupNodesSelectable()
		{
			GroupNodesSelectable = true;
		}

		/// <summary>
		/// Gets and sets how group text is rendered.
		/// </summary>
		[Category("Group")]
		[Description("Control how group text is rendered.")]
		[DefaultValue(typeof(TextRenderingHint), "AntiAlias")]
		public TextRenderingHint GroupTextRenderingHint
		{
			get { return _groupTextRenderingHint; }

			set
			{
				if (_groupTextRenderingHint != value)
				{
					_groupTextRenderingHint = value;

					// Notify change event
					OnGroupTextRenderingHintChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupTextRenderingHint property to its default value.
		/// </summary>
		public void ResetGroupTextRenderingHint()
		{
			GroupTextRenderingHint = TextRenderingHint.AntiAlias;
		}

		/// <summary>
		/// Gets and sets the font for drawing group text.
		/// </summary>
		[Category("Group")]
		[Description("Defines the font for drawing group text.")]
		public Font GroupFont
		{
			get { return _groupFont; }

			set
			{
				if (_groupFont != value)
				{
					_groupFont = value;

					// Notify change event
					OnGroupFontChanged();

					// Invalidate the cached group font height/bold font
					InternalResetGroupFontHeight();
					InternalResetGroupFontBoldItalic();

					// Must recalculate drawing of all nodes for new font
					MarkAllNodeSizesDirty();
				}
			}
		}

		private bool ShouldSerializeGroupFont()
		{
			return !GroupFont.Equals(new Font(SystemInformation.MenuFont, FontStyle.Regular));
		}

		/// <summary>
		/// Resets the GroupFont property to its default value.
		/// </summary>
		public void ResetGroupFont()
		{
			GroupFont = new Font(SystemInformation.MenuFont, FontStyle.Regular);
		}

		/// <summary>
		/// Gets and sets a value indicating if group nodes enter editing after a delayed second click.
		/// </summary>
		[Category("Group")]
		[Description("Do group nodes enter editing after a delayed second click.")]
		[DefaultValue(false)]
		public bool GroupAutoEdit
		{
			get { return _groupAutoEdit; }

			set
			{
				if (_groupAutoEdit != value)
				{
					_groupAutoEdit = value;

					// Notify change event
					OnGroupAutoEditChanged();
				}
			}
		}

		/// <summary>
		/// Resets the GroupAutoEdit property to its default value.
		/// </summary>
		public void ResetGroupAutoEdit()
		{
			GroupAutoEdit = false;
		}

		/// <summary>
		/// Gets and sets a value indicating if arrows should be be shown on the group nodes.
		/// </summary>
		[Category("Group")]
		[Description("Should arrows be shown on the group nodes.")]
		[DefaultValue(false)]
		public bool GroupArrows
		{
			get { return _groupArrows; }

			set
			{
				if (_groupArrows != value)
				{
					_groupArrows = value;

					// Notify change event
					OnGroupArrowsChanged();

					// Must recalculate drawing of all nodes for potentially new sizes
					MarkAllNodeSizesDirty();
				}
			}
		}

		/// <summary>
		/// Resets the GroupArrows property to its default value.
		/// </summary>
		public void ResetGroupArrows()
		{
			GroupArrows = false;
		}

		/// <summary>
		/// Gets and sets the extra space on left of group children.
		/// </summary>
		[Category("Group")]
		[Description("Defines extra space on left of group children.")]
		[DefaultValue(2)]
		public int GroupIndentLeft
		{
			get { return _groupIndentLeft; }

			set
			{
				if (_groupIndentLeft != value)
				{
					_groupIndentLeft = value;

					// Notify change event
					OnGroupIndentLeftChanged();

					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the GroupIndentLeft property to its default value.
		/// </summary>
		public void ResetGroupIndentLeft()
		{
			GroupIndentLeft = 2;
		}

		/// <summary>
		/// Gets and sets the extra space at top of group children.
		/// </summary>
		[Category("Group")]
		[Description("Defines extra space at top of group children.")]
		[DefaultValue(5)]
		public int GroupIndentTop
		{
			get { return _groupIndentTop; }

			set
			{
				if (_groupIndentTop != value)
				{
					_groupIndentTop = value;

					// Notify change event
					OnGroupIndentTopChanged();

					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the GroupIndentTop property to its default value.
		/// </summary>
		public void ResetGroupIndentTop()
		{
			GroupIndentTop = 5;
		}

		/// <summary>
		/// Gets and sets the extra space at bottom of group children.
		/// </summary>
		[Category("Group")]
		[Description("Defines extra space at bottom of group children.")]
		[DefaultValue(5)]
		public int GroupIndentBottom
		{
			get { return _groupIndentBottom; }

			set
			{
				if (_groupIndentBottom != value)
				{
					_groupIndentBottom = value;

					// Notify change event
					OnGroupIndentBottomChanged();

					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the GroupIndentBottom property to its default value.
		/// </summary>
		public void ResetGroupIndentBottom()
		{
			GroupIndentBottom = 5;
		}

		/// <summary>
		/// Gets and sets how the groups are colored.
		/// </summary>
		[Category("Group")]
		[Description("Defines how the groups are colored.")]
		[DefaultValue(typeof(GroupColoring), "ControlProperties")]
		public GroupColoring GroupColoring
		{
			get { return _groupColoring; }

			set
			{
				if (_groupColoring != value)
				{
					_groupColoring = value;

					// Notify change event
					OnGroupColoringChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupColoring property to its default value.
		/// </summary>
		public void ResetGroupColoring()
		{
			GroupColoring = GroupColoring.ControlProperties;
		}

		/// <summary>
		/// Gets and sets the group background color.
		/// </summary>
		[Category("Group")]
		[Description("Defines the group background color.")]
		[DefaultValue(typeof(Color), "ControlDark")]
		public Color GroupBackColor
		{
			get { return _groupBackColor; }

			set
			{
				if (_groupBackColor != value)
				{
					_groupBackColor = value;

					// Notify change event
					OnGroupBackColorChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupBackColor property to its default value.
		/// </summary>
		public void ResetGroupBackColor()
		{
			GroupBackColor = SystemColors.ControlDark;
		}

		/// <summary>
		/// Gets and sets the group foreground color.
		/// </summary>
		[Category("Group")]
		[Description("Defines the group foreground color.")]
		[DefaultValue(typeof(Color), "ControlText")]
		public Color GroupForeColor
		{
			get { return _groupForeColor; }

			set
			{
				if (_groupForeColor != value)
				{
					_groupForeColor = value;

					// Notify change event
					OnGroupForeColorChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupForeColor property to its default value.
		/// </summary>
		public void ResetGroupForeColor()
		{
			GroupForeColor = SystemColors.ControlText;
		}

		/// <summary>
		/// Gets and sets the group line color.
		/// </summary>
		[Category("Group")]
		[Description("Defines the group line color.")]
		[DefaultValue(typeof(Color), "ControlText")]
		public Color GroupLineColor
		{
			get { return _groupLineColor; }

			set
			{
				if (_groupLineColor != value)
				{
					_groupLineColor = value;

					// Clear cached resources for checks
					ClearGroupCache();

					// Notify change event
					OnGroupLineColorChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupLineColor( property to its default value.
		/// </summary>
		public void ResetGroupLineColor()
		{
			GroupLineColor = SystemColors.ControlText;
		}

		/// <summary>
		/// Gets and sets the group background color when hot tracking.
		/// </summary>
		[Category("Group")]
		[Description("Defines the group background color when hot tracking.")]
		[DefaultValue(typeof(Color), "HotTrack")]
		public Color GroupHotBackColor
		{
			get { return _groupHotBackColor; }

			set
			{
				if (_groupHotBackColor != value)
				{
					_groupHotBackColor = value;

					// Notify change event
					OnGroupHotBackColorChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupHotBackColor property to its default value.
		/// </summary>
		public void ResetGroupHotBackColor()
		{
			GroupHotBackColor = SystemColors.HotTrack;
		}

		/// <summary>
		/// Gets and sets the group background color when selected.
		/// </summary>
		[Category("Group")]
		[Description("Defines the group background color when selected.")]
		[DefaultValue(typeof(Color), "Highlight")]
		public Color GroupSelectedBackColor
		{
			get { return _groupSelectedBackColor; }

			set
			{
				if (_groupSelectedBackColor != value)
				{
					_groupSelectedBackColor = value;

					// Notify change event
					OnGroupSelectedBackColorChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupSelectedBackColor property to its default value.
		/// </summary>
		public void ResetGroupSelectedBackColor()
		{
			GroupSelectedBackColor = SystemColors.Highlight;
		}

		/// <summary>
		/// Gets and sets the group background color when selected but without focus.
		/// </summary>
		[Category("Group")]
		[Description("Defines the group background color when selected but without focus.")]
		[DefaultValue(typeof(Color), "Highlight")]
		public Color GroupSelectedNoFocusBackColor
		{
			get { return _groupSelectedNoFocusBackColor; }

			set
			{
				if (_groupSelectedNoFocusBackColor != value)
				{
					_groupSelectedNoFocusBackColor = value;

					// Notify change event
					OnGroupSelectedNoFocusBackColorChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupSelectedNoFocusBackColor property to its default value.
		/// </summary>
		public void ResetGroupSelectedNoFocusBackColor()
		{
			GroupSelectedNoFocusBackColor = SystemColors.Highlight;
		}

		/// <summary>
		/// Gets and sets the group foreground color when hot tracking.
		/// </summary>
		[Category("Group")]
		[Description("Defines the group foreground color when hot tracking.")]
		[DefaultValue(typeof(Color), "HighlightText")]
		public Color GroupHotForeColor
		{
			get { return _groupHotForeColor; }

			set
			{
				if (_groupHotForeColor != value)
				{
					_groupHotForeColor = value;

					// Notify change event
					OnGroupHotForeColorChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupForeColor property to its default value.
		/// </summary>
		public void ResetGroupHotForeColor()
		{
			GroupHotForeColor = SystemColors.HighlightText;
		}

		/// <summary>
		/// Gets and sets the group foreground color when selected.
		/// </summary>
		[Category("Group")]
		[Description("Defines the group foreground color when selected.")]
		[DefaultValue(typeof(Color), "HighlightText")]
		public Color GroupSelectedForeColor
		{
			get { return _groupSelectedForeColor; }

			set
			{
				if (_groupSelectedForeColor != value)
				{
					_groupSelectedForeColor = value;

					// Notify change event
					OnGroupSelectedForeColorChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupSelectedForeColor property to its default value.
		/// </summary>
		public void ResetGroupSelectedForeColor()
		{
			GroupSelectedForeColor = SystemColors.HighlightText;
		}

		/// <summary>
		/// Gets and sets a value indicating if group nodes should expand when dragging hovers over them.
		/// </summary>
		[Category("Nodes")]
		[Description("Should group nodes expand when dragging hovers over them.")]
		[DefaultValue(true)]
		public bool GroupExpandOnDragHover
		{
			get { return _groupExpandOnDragHover; }

			set
			{
				if (_groupExpandOnDragHover != value)
				{
					_groupExpandOnDragHover = value;

					// Notify change event
					OnGroupExpandOnDragHoverChanged();
				}
			}
		}

		/// <summary>
		/// Resets the GroupExpandOnDragHover property to its default value.
		/// </summary>
		public void ResetGroupExpandOnDragHover()
		{
			GroupExpandOnDragHover = true;
		}

		/// <summary>
		/// Gets and sets the image box line color.
		/// </summary>
		[Category("Group - ImageBox")]
		[Description("Defines the image box line color.")]
		[DefaultValue(typeof(Color), "InfoText")]
		public Color GroupImageBoxLineColor
		{
			get { return _groupImageBoxLineColor; }

			set
			{
				if (_groupImageBoxLineColor != value)
				{
					_groupImageBoxLineColor = value;

					// Clear cached resources for checks
					ClearGroupCache();

					// Notify change event
					OnGroupImageBoxLineColorChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupImageBoxLineColor property to its default value.
		/// </summary>
		public void ResetGroupImageBoxLineColor()
		{
			GroupImageBoxLineColor = SystemColors.InfoText;
		}

		/// <summary>
		/// Gets and sets the image box background color.
		/// </summary>
		[Category("Group - ImageBox")]
		[Description("Defines the image box background color.")]
		[DefaultValue(typeof(Color), "Info")]
		public Color GroupImageBoxBackColor
		{
			get { return _groupImageBoxBackColor; }

			set
			{
				if (_groupImageBoxBackColor != value)
				{
					_groupImageBoxBackColor = value;

					// Clear cached resources for checks
					ClearGroupCache();

					// Notify change event
					OnGroupImageBoxBackColorChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupImageBoxBackColor property to its default value.
		/// </summary>
		public void ResetGroupImageBoxBackColor()
		{
			GroupImageBoxBackColor = SystemColors.Info;
		}

		/// <summary>
		/// Gets and sets the image box background color when selected.
		/// </summary>
		[Category("Group - ImageBox")]
		[Description("Defines the image box background color when selected.")]
		[DefaultValue(typeof(Color), "Info")]
		public Color GroupImageBoxSelectedBackColor
		{
			get { return _groupImageBoxSelectedBackColor; }

			set
			{
				if (_groupImageBoxSelectedBackColor != value)
				{
					_groupImageBoxSelectedBackColor = value;

					// Clear cached resources for checks
					ClearGroupCache();

					// Notify change event
					OnGroupImageBoxSelectedBackColorChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupImageBoxSelectedBackColor property to its default value.
		/// </summary>
		public void ResetGroupImageBoxSelectedBackColor()
		{
			GroupImageBoxSelectedBackColor = SystemColors.Info;
		}

		/// <summary>
		/// Gets and sets the image box column background color.
		/// </summary>
		[Category("Group - ImageBox")]
		[Description("Defines the image box column background color.")]
		[DefaultValue(typeof(Color), "Control")]
		public Color GroupImageBoxColumnColor
		{
			get { return _groupImageBoxColumnColor; }

			set
			{
				if (_groupImageBoxColumnColor != value)
				{
					_groupImageBoxColumnColor = value;

					// Clear cached resources for checks
					ClearGroupCache();

					// Notify change event
					OnGroupImageBoxColumnColorChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupImageBoxColumnColor property to its default value.
		/// </summary>
		public void ResetGroupImageBoxColumnColor()
		{
			GroupImageBoxColumnColor = SystemColors.Control;
		}

		/// <summary>
		/// Gets and sets if the group image box background be drawn with gradient.
		/// </summary>
		[Category("Group - ImageBox")]
		[Description("Should group image box background be drawn with gradient.")]
		[DefaultValue(false)]
		public bool GroupImageBoxGradientBack
		{
			get { return _groupImageBoxGradientBack; }

			set
			{
				if (_groupImageBoxGradientBack != value)
				{
					_groupImageBoxGradientBack = value;

					// Notify change event
					OnGroupImageBoxGradientBackChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupGradientBack property to its default value.
		/// </summary>
		public void ResetGroupImageBoxGradientBack()
		{
			GroupImageBoxGradientBack = false;
		}

		/// <summary>
		/// Gets and sets the direction of image box gradient when drawing group in gradient effect.
		/// </summary>
		[Category("Group - ImageBox")]
		[Description("Direction of image box gradient when drawing group in gradient effect.")]
		[DefaultValue(typeof(GradientDirection), "TopToBottom")]
		public GradientDirection GroupImageBoxGradientAngle
		{
			get { return _groupImageBoxGradientAngle; }

			set
			{
				if (_groupImageBoxGradientAngle != value)
				{
					_groupImageBoxGradientAngle = value;

					// Notify change event
					OnGroupImageBoxGradientAngleChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupImageBoxGradientAngle property to its default value.
		/// </summary>
		public void ResetGroupImageBoxGradientAngle()
		{
			GroupImageBoxGradientAngle = GradientDirection.TopToBottom;
		}

		/// <summary>
		/// Gets and sets how the image box gradient colour is calculated.
		/// </summary>
		[Category("Group - ImageBox")]
		[Description("Define how the image box gradient colour is calculated.")]
		[DefaultValue(typeof(TreeGradientColoring), "VeryLightToColor")]
		public TreeGradientColoring GroupImageBoxGradientColoring
		{
			get { return _groupImageBoxGradientColoring; }

			set
			{
				if (_groupImageBoxGradientColoring != value)
				{
					_groupImageBoxGradientColoring = value;

					// Notify change event
					OnGroupImageBoxGradientColoringChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupImageBoxGradientColoring property to its default value.
		/// </summary>
		public void ResetGroupImageBoxGradientColoring()
		{
			GroupImageBoxGradientColoring = TreeGradientColoring.VeryLightToColor;
		}

		/// <summary>
		/// Gets and sets how the group borders are drawn.
		/// </summary>
		[Category("Group")]
		[Description("Defines how group borders are drawn.")]
		[DefaultValue(typeof(GroupBorderStyle), "AllEdges")]
		public GroupBorderStyle GroupBorderStyle
		{
			get { return _groupBorderStyle; }

			set
			{
				if (_groupBorderStyle != value)
				{
					_groupBorderStyle = value;

					// Notify change event
					OnGroupBorderStyleChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupBorderStyle( property to its default value.
		/// </summary>
		public void ResetGroupBorderStyle()
		{
			GroupBorderStyle = GroupBorderStyle.AllEdges;
		}

		/// <summary>
		/// Gets and sets the group font style to apply when hot tracking.
		/// </summary>
		[Category("Group")]
		[Description("Group font style to apply when hot tracking.")]
		[DefaultValue(typeof(FontStyle), "Regular")]
		public FontStyle GroupHotFontStyle
		{
			get { return _groupHotFontStyle; }

			set
			{
				if (_groupHotFontStyle != value)
				{
					_groupHotFontStyle = value;

					// Notify change event
					OnGroupHotFontStyleChanged();

					// Need to redraw to reflect change
					MarkAllNodeSizesDirty();
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupHotFontStyle property to its default value.
		/// </summary>
		public void ResetGroupHotFontStyle()
		{
			GroupHotFontStyle = FontStyle.Regular;
		}

		/// <summary>
		/// Gets and sets a value indicating if the GroupHotFontStyle should be used.
		/// </summary>
		[Category("Group")]
		[Description("Should the GroupHotFontStyle be used.")]
		[DefaultValue(false)]
		public bool GroupUseHotFontStyle
		{
			get { return _groupUseHotFontStyle; }

			set
			{
				if (_groupUseHotFontStyle != value)
				{
					_groupUseHotFontStyle = value;

					// Notify change event
					OnGroupUseHotFontStyleChanged();

					// Need to redraw to reflect change
					MarkAllNodeSizesDirty();
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupUseHotFontStyle property to its default value.
		/// </summary>
		public void ResetGroupUseHotFontStyle()
		{
			GroupUseHotFontStyle = false;
		}

		/// <summary>
		/// Gets and sets the group font style to apply when selected.
		/// </summary>
		[Category("Group")]
		[Description("Group font style to apply when selected.")]
		[DefaultValue(typeof(FontStyle), "Regular")]
		public FontStyle GroupSelectedFontStyle
		{
			get { return _groupSelectedFontStyle; }

			set
			{
				if (_groupSelectedFontStyle != value)
				{
					_groupSelectedFontStyle = value;

					// Notify change event
					OnGroupSelectedFontStyleChanged();

					// Need to redraw to reflect change
					MarkAllNodeSizesDirty();
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupSelectedFontStyle property to its default value.
		/// </summary>
		public void ResetGroupSelectedFontStyle()
		{
			GroupSelectedFontStyle = FontStyle.Regular;
		}

		/// <summary>
		/// Gets and sets a value indicating if the GroupSelectedFontStyle should be used.
		/// </summary>
		[Category("Group")]
		[Description("Should the GroupSelectedFontStyle be used.")]
		[DefaultValue(false)]
		public bool GroupUseSelectedFontStyle
		{
			get { return _groupUseSelectedFontStyle; }

			set
			{
				if (_groupUseSelectedFontStyle != value)
				{
					_groupUseSelectedFontStyle = value;

					// Notify change event
					OnGroupUseSelectedFontStyleChanged();

					// Need to redraw to reflect change
					MarkAllNodeSizesDirty();
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupUseSelectedFontStyle property to its default value.
		/// </summary>
		public void ResetGroupUseSelectedFontStyle()
		{
			GroupUseSelectedFontStyle = false;
		}

		/// <summary>
		/// Gets and sets the direction of gradient when drawing group in gradient effect.
		/// </summary>
		[Category("Group")]
		[Description("Direction of gradient when drawing group in gradient effect.")]
		[DefaultValue(typeof(GradientDirection), "TopToBottom")]
		public GradientDirection GroupGradientAngle
		{
			get { return _groupGradientAngle; }

			set
			{
				if (_groupGradientAngle != value)
				{
					_groupGradientAngle = value;

					// Notify change event
					OnGroupGradientAngleChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupGradientAngle property to its default value.
		/// </summary>
		public void ResetGroupGradientAngle()
		{
			GroupGradientAngle = GradientDirection.TopToBottom;
		}

		/// <summary>
		/// Gets and sets how the gradient colour is calculated.
		/// </summary>
		[Category("Group")]
		[Description("Define how the gradient colour is calculated.")]
		[DefaultValue(typeof(TreeGradientColoring), "VeryLightToColor")]
		public TreeGradientColoring GroupGradientColoring
		{
			get { return _groupGradientColoring; }

			set
			{
				if (_groupGradientColoring != value)
				{
					_groupGradientColoring = value;

					// Notify change event
					OnGroupGradientColoringChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupGradientColoring property to its default value.
		/// </summary>
		public void ResetGroupGradientColoring()
		{
			GroupGradientColoring = TreeGradientColoring.VeryLightToColor;
		}

		/// <summary>
		/// Gets and sets if the group background be drawn with gradient.
		/// </summary>
		[Category("Group")]
		[Description("Should group background be drawn with gradient.")]
		[DefaultValue(true)]
		public bool GroupGradientBack
		{
			get { return _groupGradientBack; }

			set
			{
				if (_groupGradientBack != value)
				{
					_groupGradientBack = value;

					// Notify change event
					OnGroupGradientBackChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupGradientBack property to its default value.
		/// </summary>
		public void ResetGroupGradientBack()
		{
			GroupGradientBack = true;
		}

		/// <summary>
		/// Gets and sets extra padding on left of group.
		/// </summary>
		[Category("Group")]
		[Description("Extra padding on left of group.")]
		[DefaultValue(3)]
		public int GroupExtraLeft
		{
			get { return _groupExtraLeft; }

			set
			{
				if (_groupExtraLeft != value)
				{
					// Limit check, must be positive
					if (value < 0)
						value = 0;

					_groupExtraLeft = value;

					// Notify change event
					OnGroupExtraLeftChanged();

					// Must recalculate drawing of all nodes for new font
					MarkAllNodeSizesDirty();

					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the GroupExtraLeft property to its default value.
		/// </summary>
		public void ResetGroupExtraLeft()
		{
			GroupExtraLeft = 3;
		}

		/// <summary>
		/// Gets and sets extra padding added to height of group.
		/// </summary>
		[Category("Group")]
		[Description("Extra padding added to height of group.")]
		[DefaultValue(5)]
		public int GroupExtraHeight
		{
			get { return _groupExtraHeight; }

			set
			{
				if (_groupExtraHeight != value)
				{
					// Limit check, must be positive
					if (value < 0)
						value = 0;

					_groupExtraHeight = value;

					// Notify change event
					OnGroupExtraHeightChanged();

					// Must recalculate drawing of all nodes for new font
					MarkAllNodeSizesDirty();

					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the GroupExtraLeft property to its default value.
		/// </summary>
		public void ResetGroupExtraHeight()
		{
			GroupExtraHeight = 5;
		}

		/// <summary>
		/// Gets and sets a value indicating if group should be hot tracked.
		/// </summary>
		[Category("Group")]
		[Description("Determine if group should be hot tracked.")]
		[DefaultValue(true)]
		public bool GroupHotTrack
		{
			get { return _groupHotTrack; }

			set
			{
				if (_groupHotTrack != value)
				{
					_groupHotTrack = value;

					// Notify change event
					OnGroupHotTrackChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupExtraLeft property to its default value.
		/// </summary>
		public void ResetGroupHotTrack()
		{
			GroupHotTrack = true;
		}

		/// <summary>
		/// Gets and sets the expand action to perform when clicking a group node.
		/// </summary>
		[Category("Group")]
		[Description("Expand action to perform when clicking a group node.")]
		[DefaultValue(typeof(ClickExpandAction), "Toggle")]
		public ClickExpandAction GroupClickExpand
		{
			get { return _groupClickExpand; }

			set
			{
				if (_groupClickExpand != value)
				{
					_groupClickExpand = value;

					// Notify change event
					OnGroupClickExpandChanged();

				}
			}
		}

		/// <summary>
		/// Resets the GroupClickExpand property to its default value.
		/// </summary>
		public void ResetGroupClickExpand()
		{
			GroupClickExpand = ClickExpandAction.Toggle;
		}

		/// <summary>
		/// Gets and sets the expand action to perform when double clicking a group node.
		/// </summary>
		[Category("Group")]
		[Description("Expand action to perform when double clicking a group node.")]
		[DefaultValue(typeof(ClickExpandAction), "Expand")]
		public ClickExpandAction GroupDoubleClickExpand
		{
			get { return _groupDoubleClickExpand; }

			set
			{
				if (_groupDoubleClickExpand != value)
				{
					_groupDoubleClickExpand = value;

					// Notify change event
					OnGroupDoubleClickExpandChanged();
				}
			}
		}

		/// <summary>
		/// Resets the GroupDoubleClickExpand property to its default value.
		/// </summary>
		public void ResetGroupDoubleClickExpand()
		{
			GroupDoubleClickExpand = ClickExpandAction.Expand;
		}

		/// <summary>
		/// Gets and sets a a value indicating if nodes at same level should collapse when group node is expanded.
		/// </summary>
		[Category("Group")]
		[Description("Should other nodes at same level collapse when group node is expanded.")]
		[DefaultValue(false)]
		public bool GroupAutoCollapse
		{
			get { return _groupAutoCollapse; }

			set
			{
				if (_groupAutoCollapse != value)
				{
					_groupAutoCollapse = value;

					// If auto collapsing then ensure at least one top group is open
					if (_groupAutoCollapse)
					{
						int i = 0;
						Node expand = null;

						// Search all the top nodes
						for(i=0; i<Nodes.Count; i++)
						{
							// Get indexed nodes
							Node n = Nodes[i];

							// If the node is visible, then of interest
							if (n.Visible)
							{
								// If expanded then nothing more to do
								if (n.Expanded)
									break;

								// If it has at least one visible child
								if ((n.Nodes.VisibleCount > 0) && (expand == null))
									expand = n;
							}
						}

						// If nothing is expanded and one of them can be...do it!
						if ((i == Nodes.Count) && (expand != null))
							expand.Expand();
					}

					// Notify change event
					OnGroupAutoCollapseChanged();

					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the GroupAutoCollapse property to its default value.
		/// </summary>
		public void ResetGroupAutoCollapse()
		{
			GroupAutoCollapse = false;
		}

		/// <summary>
		/// Gets and sets a a value indicating if any spare space between last node and end of control should be allocated to expanded group.
		/// </summary>
		[Category("Group")]
		[Description("Allocate any spare space between last node and end of control to expanded group.")]
		[DefaultValue(false)]
		public bool GroupAutoAllocate
		{
			get { return _groupAutoAllocate; }

			set
			{
				if (_groupAutoAllocate != value)
				{
					_groupAutoAllocate = value;

					// Notify change event
					OnGroupAutoAllocateChanged();

					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the GroupAutoAllocate property to its default value.
		/// </summary>
		public void ResetGroupAutoAllocate()
		{
			GroupAutoAllocate = false;
		}

		/// <summary>
		/// Gets and sets a value indicating if an image box should be shown at start of group nodes.
		/// </summary>
		[Category("Group - ImageBox")]
		[Description("Show an image box at start of group nodes.")]
		[DefaultValue(false)]
		public bool GroupImageBox
		{
			get { return _groupImageBox; }

			set
			{
				if (_groupImageBox != value)
				{
					_groupImageBox = value;

					// Notify change event
					OnGroupImageBoxChanged();

					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the GroupBackColor property to its default value.
		/// </summary>
		public void ResetGroupImageBox()
		{
			GroupImageBox = false;
		}

		/// <summary>
		/// Gets and sets a value indicating if the image box column be drawn.
		/// </summary>
		[Category("Group - ImageBox")]
		[Description("Should the image box column be drawn.")]
		[DefaultValue(true)]
		public bool GroupImageBoxColumn
		{
			get { return _groupImageBoxColumn; }

			set
			{
				if (_groupImageBoxColumn != value)
				{
					_groupImageBoxColumn = value;

					// Notify change event
					OnGroupImageBoxColumnChanged();

					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the GroupBackColor property to its default value.
		/// </summary>
		public void ResetGroupImageBoxColumn()
		{
			GroupImageBoxColumn = true;
		}

		/// <summary>
		/// Gets and sets a value indicating if the image box border should be drawn.
		/// </summary>
		[Category("Group - ImageBox")]
		[Description("Should the image box border be drawn.")]
		[DefaultValue(true)]
		public bool GroupImageBoxBorder
		{
			get { return _groupImageBoxBorder; }

			set
			{
				if (_groupImageBoxBorder != value)
				{
					_groupImageBoxBorder = value;

					// Notify change event
					OnGroupImageBoxBorderChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupImageBoxBorder property to its default value.
		/// </summary>
		public void ResetGroupImageBoxBorder()
		{
			GroupImageBoxBorder = true;
		}

		/// <summary>
		/// Gets and sets the width of the image box area.
		/// </summary>
		[Category("Group - ImageBox")]
		[Description("Width of image box area.")]
		[DefaultValue(20)]
		public int GroupImageBoxWidth
		{
			get { return _groupImageBoxWidth; }

			set
			{
				// Limit check to a positive value
				if (value < 1)
					value = 1;
			
				if (_groupImageBoxWidth != value)
				{
					_groupImageBoxWidth = value;

					// Notify change event
					OnGroupImageBoxWidthChanged();

					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the GroupBackColor property to its default value.
		/// </summary>
		public void ResetGroupImageBoxWidth()
		{
			GroupImageBoxWidth = 20;
		}

		/// <summary>
		/// Gets and sets the spacing gap between the image box and group text.
		/// </summary>
		[Category("Group - ImageBox")]
		[Description("Spacing gap between the image box and group text.")]
		[DefaultValue(6)]
		public int GroupImageBoxGap
		{
			get { return _groupImageBoxGap; }

			set
			{
				// Limit check to a positive value
				if (value < 1)
					value = 1;
			
				if (_groupImageBoxGap != value)
				{
					_groupImageBoxGap = value;

					// Notify change event
					OnGroupImageBoxGapChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the GroupBackColor property to its default value.
		/// </summary>
		public void ResetGroupImageBoxGap()
		{
			GroupImageBoxGap = 6;
		}

		/// <summary>
		/// Gets and sets then expand action to take when node is clicked.
		/// </summary>
		[Category("Nodes")]
		[Description("Expand action to take when node is clicked.")]
		[DefaultValue(typeof(ClickExpandAction), "None")]
		public ClickExpandAction ClickExpand
		{
			get { return _clickExpand; }

			set
			{
				if (_clickExpand != value)
				{
					_clickExpand = value;

					// Notify change event
					OnClickExpandChanged();
				}
			}
		}

		/// <summary>
		/// Resets the ExpandOnClick property to its default value.
		/// </summary>
		public void ResetClickExpand()
		{
			ClickExpand = ClickExpandAction.None;
		}

		/// <summary>
		/// Gets and sets then expand action to take when node is double clicked.
		/// </summary>
		[Category("Nodes")]
		[Description("Expand action to take when node is double clicked.")]
		[DefaultValue(typeof(ClickExpandAction), "Toggle")]
		public ClickExpandAction DoubleClickExpand
		{
			get { return _doubleClickExpand; }

			set
			{
				if (_doubleClickExpand != value)
				{
					_doubleClickExpand = value;

					// Notify change event
					OnDoubleClickExpandChanged();
				}
			}
		}

		/// <summary>
		/// Resets the DoubleClickExpand property to its default value.
		/// </summary>
		public void ResetDoubleClickExpand()
		{
			DoubleClickExpand = ClickExpandAction.Toggle;
		}

		/// <summary>
		/// Gets and sets a a value indicating if nodes at same level should collapse when node is expanded.
		/// </summary>
		[Category("Nodes")]
		[Description("Should other nodes at same level collapse when node is expanded.")]
		[DefaultValue(false)]
		public bool AutoCollapse
		{
			get { return _autoCollapse; }

			set
			{
				if (_autoCollapse != value)
				{
					_autoCollapse = value;

					// Notify change event
					OnAutoCollapseChanged();

					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the AutoCollapse property to its default value.
		/// </summary>
		public void ResetAutoCollapse()
		{
			AutoCollapse = false;
		}

		/// <summary>
		/// Gets and sets a a value indicating if nodes should be selectable and drawn to right extent.
		/// </summary>
		[Category("Nodes")]
		[Description("Should a node be selectable and drawn to right extent.")]
		[DefaultValue(false)]
		public bool ExtendToRight
		{
			get { return _extendToRight; }

			set
			{
				if (_extendToRight != value)
				{
					_extendToRight = value;

					// Notify change event
					OnExtendToRightChanged();

					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the ExtendToRight property to its default value.
		/// </summary>
		public void ResetExtendToRight()
		{
			ExtendToRight = false;
		}

		/// <summary>
		/// Gets and sets a value indicating if each change be shown immediately.
		/// </summary>
		[Category("Nodes")]
		[Description("Should each change be shown immediately.")]
		[DefaultValue(false)]
		public bool InstantUpdate
		{
			get { return _instantUpdate; }

			set
			{
				if (_instantUpdate != value)
				{
					_instantUpdate = value;

					// Notify change event
					OnInstantUpdateChanged();
				}
			}
		}

		/// <summary>
		/// Resets the InstantUpdate property to its default value.
		/// </summary>
		public void ResetInstantUpdate()
		{
			InstantUpdate = false;
		}

		/// <summary>
		/// Gets and sets a value indicating if tooltips should be shown when node cannot be fully seen.
		/// </summary>
		[Category("Nodes")]
		[Description("Should tooltips be shown when node cannot be fully seen.")]
		[DefaultValue(true)]
		public bool Tooltips
		{
			get { return _tooltips; }

			set
			{
				if (_tooltips != value)
				{
					_tooltips = value;

					// Notify change event
					OnTooltipsChanged();
				}
			}
		}

		/// <summary>
		/// Resets the Tooltips property to its default value.
		/// </summary>
		public void ResetTooltips()
		{
			Tooltips = true;
		}

		/// <summary>
		/// Gets and sets a value indicating if node specific tooltips be used if defined for the individual node.
		/// </summary>
		[Category("Nodes")]
		[Description("Should node specific tooltips be used if defined for the individual node.")]
		[DefaultValue(true)]
		public bool Infotips
		{
			get { return _infotips; }

			set
			{
				if (_infotips != value)
				{
					_infotips = value;

					// Notify change event
					OnInfotipsChanged();
				}
			}
		}

        /// <summary>
        /// Resets the Infotips property to its default value.
        /// </summary>
        public void ResetInfotips()
        {
            Infotips = true;
        }

		/// <summary>
		/// Gets and sets a value indicating if nodes can be selected.
		/// </summary>
		[Category("Nodes")]
		[Description("Can nodes be selected.")]
		[DefaultValue(true)]
		public bool NodesSelectable
		{
			get { return _nodesSelectable; }

			set
			{
				if (_nodesSelectable != value)
				{
					_nodesSelectable = value;

					// Notify change event
					OnNodesSelectableChanged();
				}
			}
		}

		/// <summary>
		/// Resets the NodesSelectable property to its default value.
		/// </summary>
		public void ResetNodesSelectable()
		{
			NodesSelectable = true;
		}

		/// <summary>
		/// Gets and sets a value indicating if nodes enter editing after a delayed second click.
		/// </summary>
		[Category("Nodes")]
		[Description("Do nodes enter editing after a delayed second click.")]
		[DefaultValue(true)]
		public bool AutoEdit
		{
			get { return _autoEdit; }

			set
			{
				if (_autoEdit != value)
				{
					_autoEdit = value;

					// Notify change event
					OnAutoEditChanged();
				}
			}
		}

		/// <summary>
		/// Resets the AutoEdit property to its default value.
		/// </summary>
		public void ResetAutoEdit()
		{
			AutoEdit = true;
		}

		/// <summary>
		/// Gets and sets a value indicating if nodes labels can be edited.
		/// </summary>
		[Category("Nodes")]
		[Description("Can nodes labels be edited.")]
		[DefaultValue(true)]
		public bool LabelEdit
		{
			get { return _labelEdit; }

			set
			{
				if (_labelEdit != value)
				{
					_labelEdit = value;

					// Notify change event
					OnLabelEditChanged();
				}
			}
		}

		/// <summary>
		/// Resets the LabelEdit property to its default value.
		/// </summary>
		public void ResetLabelEdit()
		{
			LabelEdit = true;
		}

		/// <summary>
		/// Gets and sets a value indicating if user can expand or collapse nodes.
		/// </summary>
		[Category("Nodes")]
		[Description("Can user expand or collapse nodes.")]
		[DefaultValue(true)]
		public bool CanUserExpandCollapse
		{
			get { return _canUserExpandCollapse; }

			set
			{
				if (_canUserExpandCollapse != value)
				{
					_canUserExpandCollapse = value;

					// Notify change event
					OnCanUserExpandCollapseChanged();
				}
			}
		}

		/// <summary>
		/// Resets the CanUserExpandCollapse property to its default value.
		/// </summary>
		public void ResetCanUserExpandCollapse()
		{
			CanUserExpandCollapse = true;
		}
		
		/// <summary>
		/// Gets and sets a value indicating if nodes should expand when dragging hovers over them.
		/// </summary>
		[Category("Nodes")]
		[Description("Should nodes expand when dragging hovers over them.")]
		[DefaultValue(true)]
		public bool ExpandOnDragHover
		{
			get { return _expandOnDragHover; }

			set
			{
				if (_expandOnDragHover != value)
				{
					_expandOnDragHover = value;

					// Notify change event
					OnExpandOnDragHoverChanged();
				}
			}
		}

		/// <summary>
		/// Resets the ExpandOnDragHover property to its default value.
		/// </summary>
		public void ResetExpandOnDragHover()
		{
			ExpandOnDragHover = true;
		}

		/// <summary>
		/// Gets and sets how indicators should be shown.
		/// </summary>
		[Category("Nodes")]
		[Description("How should indicators be shown.")]
		[DefaultValue(typeof(Indicators), "None")]
		public Indicators Indicators
		{
			get { return _indicators; }

			set
			{
				if (_indicators != value)
				{
					_indicators = value;

					// Notify change event
					OnIndicatorsChanged();

					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the Indicators property to its default value.
		/// </summary>
		public void ResetIndicators()
		{
			Indicators = Indicators.None;
		}

		/// <summary>
		/// Gets and sets the vertical pixel gap between nodes.
		/// </summary>
		[Category("Nodes")]
		[Description("Defines the vertical pixel gap between nodes.")]
		[DefaultValue(0)]
		public int VerticalNodeGap
		{
			get { return _verticalNodeGap; }

			set
			{
				if (_verticalNodeGap != value)
				{
					_verticalNodeGap = value;

					// Notify change event
					OnVerticalNodeGapChanged();

					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the VerticalNodeGap property to its default value.
		/// </summary>
		public void ResetVerticalNodeGap()
		{
			VerticalNodeGap = 0;
		}

		/// <summary>
		/// Gets and sets the minimum height for a node.
		/// </summary>
		[Category("Nodes")]
		[Description("Defines the minimum height for a node.")]
		public int MinimumNodeHeight
		{
			get { return _minimumNodeHeight; }

			set
			{
				if (_minimumNodeHeight != value)
				{
					_minimumNodeHeight = value;

					// Notify change event
					OnMinimumNodeHeightChanged();

					// Must recalculate drawing of all nodes for new font
					MarkAllNodeSizesDirty();

					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		private bool ShouldSerializeMinimumNodeHeight()
		{
			return (MinimumNodeHeight != Font.Height);
		}

		/// <summary>
		/// Resets the MinimumNodeHeight property to its default value.
		/// </summary>
		public void ResetMinimumNodeHeight()
		{
			MinimumNodeHeight = Font.Height;
		}

		/// <summary>
		/// Gets and sets the maximum height for a node.
		/// </summary>
		[Category("Nodes")]
		[Description("Defines the maximum height for a node.")]
		[DefaultValue(9999)]
		public int MaximumNodeHeight
		{
			get { return _maximumNodeHeight; }

			set
			{
				if (_maximumNodeHeight != value)
				{
					_maximumNodeHeight = value;

					// Notify change event
					OnMaximumNodeHeightChanged();

					// Must recalculate drawing of all nodes for new font
					MarkAllNodeSizesDirty();

					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the MaximumNodeHeight property to its default value.
		/// </summary>
		public void ResetMaximumNodeHeight()
		{
			MaximumNodeHeight = 9999;
		}

		/// <summary>
		/// Gets and sets the image list from which node images are taken.
		/// </summary>
		[Category("Images")]
		[Description("The image list from which node images are taken.")]
		[DefaultValue(null)]
		public ImageList ImageList
		{
			get { return _imageList; }

			set
			{
				if (_imageList != value)
				{
					_imageList = value;

					// Notify change event
					OnImageListChanged();

					// Must recalculate drawing of all nodes for new font
					MarkAllNodeSizesDirty();

					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the ImageList property to its default value.
		/// </summary>
		public void ResetImageList()
		{
			ImageList = null;
		}

		/// <summary>
		/// Gets and sets the default image index for nodes.
		/// </summary>
		[Category("Images")]
		[Description("The default image index for nodes.")]
		[DefaultValue(-1)]
		public int ImageIndex
		{
			get { return _imageIndex; }

			set
			{
				if (_imageIndex != value)
				{
					_imageIndex = value;

					// Cannot define anything smaller than -1
					if (_imageIndex < -1)
						_imageIndex = -1;

					// Notify change event
					OnImageIndexChanged();

					// Must recalculate drawing of all nodes for new font
					MarkAllNodeSizesDirty();

					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the ImageIndex property to its default value.
		/// </summary>
		public void ResetImageIndex()
		{
			ImageIndex = -1;
		}

		/// <summary>
		/// Gets and sets the default selected image index for nodes.
		/// </summary>
		[Category("Images")]
		[Description("The default selected image index for nodes.")]
		[DefaultValue(-1)]
		public int SelectedImageIndex
		{
			get { return _selectedImageIndex; }

			set
			{
				if (_selectedImageIndex != value)
				{
					_selectedImageIndex = value;

					// Cannot define anything smaller than -1
					if (_selectedImageIndex < -1)
						_selectedImageIndex = -1;

					// Notify change event
					OnSelectedImageIndexChanged();

					// Must recalculate drawing of all nodes for new font
					MarkAllNodeSizesDirty();

					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the SelectedImageIndex property to its default value.
		/// </summary>
		public void ResetSelectedImageIndex()
		{
			SelectedImageIndex = -1;
		}

		/// <summary>
		/// Gets and sets the pixel gap between start of node and start of image.
		/// </summary>
		[Category("Images")]
		[Description("Pixel gap between start of node and start of image.")]
		[DefaultValue(0)]
		public int ImageGapLeft
		{
			get { return _imageGapLeft; }

			set
			{
				if (_imageGapLeft != value)
				{
					_imageGapLeft = value;

					// We limit check to ensure number is positive
					if (_imageGapLeft < 0)
						_imageGapLeft = 0;

					// Notify change event
					OnImageGapLeftChanged();

					// Must recalculate drawing of all nodes for new font
					MarkAllNodeSizesDirty();

					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the ImageGapLeft property to its default value.
		/// </summary>
		public void ResetImageGapLeft()
		{
			ImageGapLeft = 0;
		}

		/// <summary>
		/// Gets and sets the pixel gap between end of image and start of text.
		/// </summary>
		[Category("Images")]
		[Description("Pixel gap between end of image and start of text.")]
		[DefaultValue(3)]
		public int ImageGapRight
		{
			get { return _imageGapRight; }

			set
			{
				if (_imageGapRight != value)
				{
					_imageGapRight = value;

					// We limit check to ensure number is positive
					if (_imageGapRight < 0)
						_imageGapRight = 0;

					// Notify change event
					OnImageGapRightChanged();

					// Must recalculate drawing of all nodes for new font
					MarkAllNodeSizesDirty();

					// Need to redraw to reflect change
					InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the ImageGapRight property to its default value.
		/// </summary>
		public void ResetImageGapRight()
		{
			ImageGapRight = 3;
		}

		/// <summary>
		/// Gets and sets how text is rendered.
		/// </summary>
		[Category("Nodes")]
		[Description("Control how text is rendered.")]
		[DefaultValue(typeof(TextRenderingHint), "SystemDefault")]
		public TextRenderingHint TextRenderingHint
		{
			get { return _textRenderingHint; }

			set
			{
				if (_textRenderingHint != value)
				{
					_textRenderingHint = value;

					// Notify change event
					OnTextRenderingHintChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the TextRenderingHint property to its default value.
		/// </summary>
		public void ResetTextRenderingHint()
		{
			TextRenderingHint = TextRenderingHint.SystemDefault;
		}

		/// <summary>
		/// Gets and sets the node background color for hot tracking.
		/// </summary>
		[Category("Nodes")]
		[Description("Node background color for hot tracking.")]
		[DefaultValue(typeof(Color), "Empty")]
		public Color HotBackColor
		{
			get { return _hotBackColor; }

			set
			{
				if (_hotBackColor != value)
				{
					_hotBackColor = value;

					// Clear cached resources for checks
					ClearNodeCache();

					// Notify change event
					OnHotBackColorChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the HotBackColor property to its default value.
		/// </summary>
		public void ResetHotBackColor()
		{
			HotBackColor = Color.Empty;
		}

		/// <summary>
		/// Gets and sets the node foreground color for hot tracking.
		/// </summary>
		[Category("Nodes")]
		[Description("Node foreground color for hot tracking.")]
		[DefaultValue(typeof(Color), "Empty")]
		public Color HotForeColor
		{
			get { return _hotForeColor; }

			set
			{
				if (_hotForeColor != value)
				{
					_hotForeColor = value;

					// Notify change event
					OnHotForeColorChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the HotForeColor property to its default value.
		/// </summary>
		public void ResetHotForeColor()
		{
			HotForeColor = Color.Empty;
		}

		/// <summary>
		/// Gets and sets the node background color for selected nodes.
		/// </summary>
		[Category("Nodes")]
		[Description("Node background color for selected nodes.")]
		[DefaultValue(typeof(Color), "Highlight")]
		public Color SelectedBackColor
		{
			get { return _selectedBackColor; }

			set
			{
				if (_selectedBackColor != value)
				{
					_selectedBackColor = value;

					// Clear cached resources for checks
					ClearNodeCache();

					// Notify change event
					OnSelectedBackColorChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the SelectedBackColor property to its default value.
		/// </summary>
		public void ResetSelectedBackColor()
		{
			SelectedBackColor = SystemColors.Highlight;
		}

		/// <summary>
		/// Gets and sets the node background color for selected nodes without focus.
		/// </summary>
		[Category("Nodes")]
		[Description("Node background color for selected nodes without focus.")]
		[DefaultValue(typeof(Color), "Color")]
		public Color SelectedNoFocusBackColor
		{
			get { return _selectedNoFocusBackColor; }

			set
			{
				if (_selectedNoFocusBackColor != value)
				{
					_selectedNoFocusBackColor = value;

					// Clear cached resources for checks
					ClearNodeCache();

					// Notify change event
					OnSelectedNoFocusBackColorChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the SelectedNoFocusBackColor property to its default value.
		/// </summary>
		public void ResetSelectedNoFocusBackColor()
		{
			SelectedNoFocusBackColor = SystemColors.Control;
		}

		/// <summary>
		/// Gets and sets the node foreground color for selected nodes.
		/// </summary>
		[Category("Nodes")]
		[Description("Node foreground color for selected nodes.")]
		[DefaultValue(typeof(Color), "HighlightText")]
		public Color SelectedForeColor
		{
			get { return _selectedForeColor; }

			set
			{
				if (_selectedForeColor != value)
				{
					_selectedForeColor = value;

					// Notify change event
					OnSelectedForeColorChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the SelectedForeColor property to its default value.
		/// </summary>
		public void ResetSelectedForeColor()
		{
			SelectedForeColor = SystemColors.HighlightText;
		}

		/// <summary>
		/// Gets and sets the node font style to apply when hot tracking.
		/// </summary>
		[Category("Nodes")]
		[Description("Node font style to apply when hot tracking.")]
		[DefaultValue(typeof(FontStyle), "Regular")]
		public FontStyle HotFontStyle
		{
			get { return _hotFontStyle; }

			set
			{
				if (_hotFontStyle != value)
				{
					_hotFontStyle = value;

					// Notify change event
					OnHotFontStyleChanged();

					// Need to redraw to reflect change
					MarkAllNodeSizesDirty();
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the HotFontStyle property to its default value.
		/// </summary>
		public void ResetHotFontStyle()
		{
			HotFontStyle = FontStyle.Regular;
		}

		/// <summary>
		/// Gets and sets a value indicating if the HotFontStyle should be used.
		/// </summary>
		[Category("Nodes")]
		[Description("Should the HotFontStyle be used.")]
		[DefaultValue(false)]
		public bool UseHotFontStyle
		{
			get { return _useHotFontStyle; }

			set
			{
				if (_useHotFontStyle != value)
				{
					_useHotFontStyle = value;

					// Notify change event
					OnUseHotFontStyleChanged();

					// Need to redraw to reflect change
					MarkAllNodeSizesDirty();
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the UseHotFontStyle property to its default value.
		/// </summary>
		public void ResetUseHotFontStyle()
		{
			UseHotFontStyle = false;
		}

		/// <summary>
		/// Gets and sets how selection operates.
		/// </summary>
		[Category("Nodes")]
		[Description("Specify how selection operates.")]
		[DefaultValue(typeof(SelectMode), "Multiple")]
		public SelectMode SelectMode
		{
			get { return _selectMode; }
			
			set
			{
				if (_selectMode != value)
				{
					_selectMode = value;
					
					// Enforce the requested mode
					switch(_selectMode)
					{
						case SelectMode.None:
							SelectedNodeCollection clear = SelectedNodes;
							foreach(Node n in clear)
								DeselectNode(n, false);
							break;
						case SelectMode.Single:
							if (_selected.Count > 1)
							{
								SelectedNodeCollection remove = SelectedNodes;
								for(int i=1; i<remove.Count; i++)
									DeselectNode(remove[i] as Node, false);
							}
							break;
					}
					
					// Notify change event
					OnSelectModeChanged();

					// Need to redraw to reflect change
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the SelectMode property to its default value.
		/// </summary>
		public void ResetSelectMode()
		{
			SelectMode = SelectMode.Multiple;
		}

        /// <summary>
        /// Gets the number of selected nodes.
        /// </summary>
        [Browsable(false)]
        public int SelectedCount
        {
            get { return _selected.Count; }
        }

        /// <summary>
        /// Discover if the node is currently selected.
        /// </summary>
        /// <param name="n">Node to test.</param>
        /// <returns>Trus if part of the selection; otherwise false.</returns>
        public bool IsNodeSelected(Node n)
        {
            return _selected.ContainsKey(n);
        }

        /// <summary>
        /// Select the provided node.
        /// </summary>
        /// <param name="n">Node to be selected.</param>
        public void SelectNode(Node n)
        {
            SelectNode(n, KeyHelper.SHIFTPressed, KeyHelper.CTRLPressed);
        }

        /// <summary>
        /// Select the provided node using shift/ctrl key semantics.
        /// </summary>
        /// <param name="n">Node to be selected.</param>
        /// <param name="shift">Should act as if the shift key is pressed.</param>
        /// <param name="ctrl">Should act as if the control key is pressed.</param>
        public void SelectNode(Node n, bool shift, bool ctrl)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new SelectNodeParameterDelegate(SelectNode), new object[] { n, shift, ctrl });
                return;
            }

            switch (SelectMode)
            {
                case SelectMode.None:
                    SetFocusNode(n);
                    break;
                case SelectMode.Single:
                    SingleSelect(n);
                    break;
                case SelectMode.Multiple:
                    if (ctrl)
                        CtrlSelect(n);
                    else if (shift)
                        ShiftSelect(n);
                    else
                        SingleSelect(n);
                    break;
            }
        }

        /// <summary>
        /// Deselect the provided node and optionally all the child nodes it contains.
        /// </summary>
        /// <param name="n">Node to be deselected.</param>
        /// <param name="recurse">Should deselect all child nodes as well.</param>
        public void DeselectNode(Node n, bool recurse)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new DeselectNodeParameterDelegate(DeselectNode), new object[] { n, recurse });
                return;
            }

            // Do not have a deselected node as the last selected node
            if (_lastSelectedNode == n)
                _lastSelectedNode = null;

            // Is this node currently selected?
            if (_selected.ContainsKey(n))
            {
                // Remove it from selection
                _selected.Remove(n);

                // Notify before/after deselection
                OnBeforeDeselect(n);
                OnAfterDeselect(n);

                // Redraw the node in deselected state
                InvalidateNode(n);

                //DWC Change
                OnAfterSelectionChanged();
            }

            // Do we need to process all children as well?
            if (recurse)
            {
                foreach (Node child in n.Nodes)
                    DeselectNode(child, true);
            }
        }

        /// <summary>
        /// Clears the node selection.
        /// </summary>
        public void ClearSelection()
        {
            // Any current selection?
            if (_selected.Count > 0)
            {
                // Request all current entries are redrawn
                foreach (Node n in _selected.Keys)
                    InvalidateNode(n);

                // Clear down selection
                _selected.Clear();
            }
        }

        /// <summary>
        /// Gets and sets a value indicating if only nodes at the saem level should be selectable.
        /// </summary>
        [Category("Nodes")]
        [Description("Should only nodes at the same level be selected.")]
        [DefaultValue(false)]
        public bool SelectSameLevel
        {
            get { return _selectSameLevel; }
            set { _selectSameLevel = value; }
        }

        /// <summary>
        /// Resets the SelectSameLevel property to its default value.
        /// </summary>
        public void ResetSelectSameLevel()
        {
            SelectSameLevel = false;
        }
        
        /// <summary>
		/// Gets and sets the node font style to apply for selected nodes.
		/// </summary>
		[Category("Nodes")]
		[Description("Node font style to apply for selected nodes.")]
		[DefaultValue(typeof(FontStyle), "Regular")]
		public FontStyle SelectedFontStyle
		{
			get { return _selectedFontStyle; }

			set
			{
				if (_selectedFontStyle != value)
				{
					_selectedFontStyle = value;

					// Notify change event
					OnSelectedFontStyleChanged();

					// Need to redraw to reflect change
					MarkAllNodeSizesDirty();
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the SelectedFontStyle property to its default value.
		/// </summary>
		public void ResetSelectedFontStyle()
		{
			SelectedFontStyle = FontStyle.Regular;
		}

		/// <summary>
		/// Gets and sets a value indicating if the SelectedFontStyle should be used.
		/// </summary>
		[Category("Nodes")]
		[Description("Should the SelectedFontStyle be used.")]
		[DefaultValue(false)]
		public bool UseSelectedFontStyle
		{
			get { return _useSelectedFontStyle; }

			set
			{
				if (_useSelectedFontStyle != value)
				{
					_useSelectedFontStyle = value;

					// Notify change event
					OnUseSelectedFontStyleChanged();

					// Need to redraw to reflect change
					MarkAllNodeSizesDirty();
					InvalidateAll();
				}
			}
		}

		/// <summary>
		/// Resets the UseSelectedFontStyle property to its default value.
		/// </summary>
		public void ResetUseSelectedFontStyle()
		{
			UseSelectedFontStyle = false;
		}

		/// <summary>
		/// Expand all root level nodes.
		/// </summary>
		public void Expand()
		{
			foreach(Node n in Nodes)
				n.Expand();

			// Need to redraw to reflect change
			InvalidateNodeDrawing();
		}

		/// <summary>
		/// Expand all nodes.
		/// </summary>
		public void ExpandAll()
		{
			foreach(Node n in Nodes)
				n.ExpandAll();

			// Need to redraw to reflect change
			InvalidateNodeDrawing();
		}

		/// <summary>
		/// Collapse all root level nodes.
		/// </summary>
		public void Collapse()
		{
			foreach(Node n in Nodes)
				n.Collapse();
					
			// Need to redraw to reflect change
			InvalidateNodeDrawing();
		}

		/// <summary>
		/// Collapse all nodes.
		/// </summary>
		public void CollapseAll()
		{
			foreach(Node n in Nodes)
				n.CollapseAll();

			// Need to redraw to reflect change
			InvalidateNodeDrawing();
		}

		/// <summary>
		/// Modify control properties to match required style.
		/// </summary>
		/// <param name="style">Style to be defined.</param>
		public void SetTreeControlStyle(TreeControlStyles style)
		{
            // Under vista we do not provide Office 2003 styles
            if (Environment.OSVersion.Version.Major >= 6)
            {
                switch (style)
                {
                    case TreeControlStyles.GroupOfficeLight:
                    case TreeControlStyles.GroupOfficeDark:
                        style = TreeControlStyles.Group;
                        break;
                }
            }

			switch(style)
			{
				case TreeControlStyles.StandardThemed:
					ResetAllProperties();
					break;
				case TreeControlStyles.StandardPlain:
					ResetAllProperties();

					// Override the properties that are standardplain specific
					BoxDrawStyle = DrawStyle.Plain;
					CheckDrawStyle = DrawStyle.Plain;
					BorderStyle = TreeBorderStyle.Solid;
					BorderIndent.Top = 1;
					BorderIndent.Left = 1;
					BorderIndent.Bottom = 1;
					BorderIndent.Right = 1;
					break;
				case TreeControlStyles.Explorer:
					ResetAllProperties();

					// Override the properties that are explorer specific
					DoubleClickExpand = ClickExpandAction.Expand;
					HotFontStyle = FontStyle.Underline;
					UseHotFontStyle = true;
					HotForeColor = SystemColors.HotTrack;
					ClickExpand = ClickExpandAction.Expand;
					LineVisibility = LineBoxVisibility.Nowhere;
					SelectMode = SelectMode.Single;
					break;
				case TreeControlStyles.Navigator:
					ResetAllProperties();

					// Group controllers needed
					ViewControllers = ViewControllers.Group;

					// General appearance and operation
					BorderStyle = TreeBorderStyle.None;
					BoxDrawStyle = DrawStyle.Plain;
					VerticalGranularity = VerticalGranularity.Pixel;

					// Group operation
					GroupClickExpand = ClickExpandAction.Expand;
					GroupAutoCollapse = true;
					GroupAutoAllocate = true;
					GroupNodesSelectable = false;

					// Group appearance
					GroupFont = new Font(this.Font.Name, this.Font.Size + 3, FontStyle.Bold);
					GroupGradientColoring = TreeGradientColoring.VeryLightToColor;
					GroupBackColor = SystemColors.ActiveCaption;
					GroupForeColor = SystemColors.ActiveCaptionText;
					GroupSelectedBackColor = SystemColors.Info;	
					GroupSelectedNoFocusBackColor = SystemColors.Info;	
					GroupSelectedForeColor = SystemColors.InfoText;	
					GroupSelectedFontStyle = FontStyle.Bold;
					GroupHotFontStyle = FontStyle.Bold;

					// Group image box
					GroupImageBox = true;
					GroupImageBoxBackColor = SystemColors.InactiveCaption;
					GroupImageBoxSelectedBackColor = SystemColors.InactiveCaption;
					GroupImageBoxWidth = (int)(GroupFont.Height * 1.5);
					GroupImageBoxLineColor = SystemColors.ControlText;
					GroupImageBoxGradientBack = true;
					break;
				case TreeControlStyles.Group:
					ResetAllProperties();

					// Override the properties that are group specific
					BorderStyle = TreeBorderStyle.Solid;
					BoxDrawStyle = DrawStyle.Themed;
					ViewControllers = ViewControllers.Group;
					VerticalGranularity = VerticalGranularity.Pixel;
                    Font = new Font(SystemInformation.MenuFont.FontFamily, SystemInformation.MenuFont.SizeInPoints, FontStyle.Regular);
					GroupFont = new Font(this.Font.Name, this.Font.Size, FontStyle.Regular);
					GroupBorderStyle = GroupBorderStyle.BottomEdge;
					GroupSelectedFontStyle = FontStyle.Bold;
					GroupHotFontStyle = FontStyle.Bold;
					GroupArrows = true;
					break;
                case TreeControlStyles.GroupOfficeBlueLight:
                case TreeControlStyles.GroupOfficeSilverLight:
                case TreeControlStyles.GroupOfficeBlackLight:
                case TreeControlStyles.GroupMediaBlueLight:
                case TreeControlStyles.GroupMediaOrangeLight:
                case TreeControlStyles.GroupMediaPurpleLight:
                    ResetAllProperties();

                    // Override the properties that are group specific
                    BorderStyle = TreeBorderStyle.Solid;
                    BoxDrawStyle = DrawStyle.Themed;
                    ViewControllers = ViewControllers.Group;
                    VerticalGranularity = VerticalGranularity.Pixel;
                    Font = new Font("Calibri", SystemInformation.MenuFont.SizeInPoints, FontStyle.Regular);
                    GroupFont = new Font("Calibri", SystemInformation.MenuFont.SizeInPoints, FontStyle.Regular);
                    GroupBorderStyle = GroupBorderStyle.VerticalEdges;
                    GroupSelectedFontStyle = FontStyle.Bold;
                    GroupHotFontStyle = FontStyle.Bold;

                    switch (style)
                    {
                        case TreeControlStyles.GroupOfficeBlueLight:
                            GroupColoring = GroupColoring.Office2007BlueLight;
                            BorderColor = Office2007ColorTable.TitleBorderColorDark(VisualStyleFromTreeControlStyles(style));
                            GroupForeColor = Office2007ColorTable.TitleInactiveTextColor(VisualStyleFromTreeControlStyles(style));
                            GroupSelectedForeColor = Color.Black;
                            break;
                        case TreeControlStyles.GroupOfficeSilverLight:
                            GroupColoring = GroupColoring.Office2007SilverLight;
                            BorderColor = Office2007ColorTable.TitleBorderColorDark(VisualStyleFromTreeControlStyles(style));
                            GroupForeColor = Office2007ColorTable.TitleInactiveTextColor(VisualStyleFromTreeControlStyles(style));
                            GroupSelectedForeColor = Color.Black;
                            break;
                        case TreeControlStyles.GroupOfficeBlackLight:
                            GroupColoring = GroupColoring.Office2007BlackLight;
                            BorderColor = Office2007ColorTable.TitleBorderColorDark(VisualStyleFromTreeControlStyles(style));
                            GroupForeColor = Office2007ColorTable.TitleInactiveTextColor(VisualStyleFromTreeControlStyles(style));
                            GroupSelectedForeColor = Color.Black;
                            break;
                        case TreeControlStyles.GroupMediaBlueLight:
                            GroupColoring = GroupColoring.MediaPlayerBlueLight;
                            BorderColor = MediaPlayerColorTable.TitleBorderColorDark(VisualStyleFromTreeControlStyles(style));
                            GroupForeColor = MediaPlayerColorTable.TitleInactiveTextColor(VisualStyleFromTreeControlStyles(style));
                            GroupSelectedForeColor = Color.White;
                            break;
                        case TreeControlStyles.GroupMediaOrangeLight:
                            GroupColoring = GroupColoring.MediaPlayerOrangeLight;
                            BorderColor = MediaPlayerColorTable.TitleBorderColorDark(VisualStyleFromTreeControlStyles(style));
                            GroupForeColor = MediaPlayerColorTable.TitleInactiveTextColor(VisualStyleFromTreeControlStyles(style));
                            GroupSelectedForeColor = Color.White;
                            break;
                        case TreeControlStyles.GroupMediaPurpleLight:
                            GroupColoring = GroupColoring.MediaPlayerPurpleLight;
                            BorderColor = MediaPlayerColorTable.TitleBorderColorDark(VisualStyleFromTreeControlStyles(style));
                            GroupForeColor = MediaPlayerColorTable.TitleInactiveTextColor(VisualStyleFromTreeControlStyles(style));
                            GroupSelectedForeColor = Color.White;
                            break;
                    }

                    TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                    GroupTextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                    GroupHotTrack = false;
                    GroupArrows = true;
                    break;
                case TreeControlStyles.GroupOfficeBlueDark:
                case TreeControlStyles.GroupOfficeSilverDark:
                case TreeControlStyles.GroupOfficeBlackDark:
                case TreeControlStyles.GroupMediaBlueDark:
                case TreeControlStyles.GroupMediaOrangeDark:
                case TreeControlStyles.GroupMediaPurpleDark:
                    ResetAllProperties();

                    // Override the properties that are group specific
                    BorderStyle = TreeBorderStyle.Solid;
                    BoxDrawStyle = DrawStyle.Themed;
                    ViewControllers = ViewControllers.Group;
                    VerticalGranularity = VerticalGranularity.Pixel;
                    Font = new Font("Calibri", SystemInformation.MenuFont.SizeInPoints, FontStyle.Regular);
                    GroupFont = new Font("Calibri", 11f, FontStyle.Bold);
                    GroupBorderStyle = GroupBorderStyle.VerticalEdges;
                    GroupSelectedFontStyle = FontStyle.Bold;
                    GroupHotFontStyle = FontStyle.Bold;

                    switch (style)
                    {
                        case TreeControlStyles.GroupOfficeBlueDark:
                            GroupColoring = GroupColoring.Office2007BlueDark;
                            BorderColor = Office2007ColorTable.TitleBorderColorDark(VisualStyleFromTreeControlStyles(style));
                            GroupForeColor = Office2007ColorTable.TitleActiveTextColor(VisualStyleFromTreeControlStyles(style));
                            GroupSelectedForeColor = Color.Black;
                            break;
                        case TreeControlStyles.GroupOfficeSilverDark:
                            GroupColoring = GroupColoring.Office2007SilverDark;
                            BorderColor = Office2007ColorTable.TitleBorderColorDark(VisualStyleFromTreeControlStyles(style));
                            GroupForeColor = Office2007ColorTable.TitleActiveTextColor(VisualStyleFromTreeControlStyles(style));
                            GroupSelectedForeColor = Color.Black;
                            break;
                        case TreeControlStyles.GroupOfficeBlackDark:
                            GroupColoring = GroupColoring.Office2007BlackDark;
                            BorderColor = Office2007ColorTable.TitleBorderColorDark(VisualStyleFromTreeControlStyles(style));
                            GroupForeColor = Office2007ColorTable.TitleActiveTextColor(VisualStyleFromTreeControlStyles(style));
                            GroupSelectedForeColor = Color.Black;
                            break;
                        case TreeControlStyles.GroupMediaBlueDark:
                            GroupColoring = GroupColoring.MediaPlayerBlueDark;
                            BorderColor = MediaPlayerColorTable.TitleBorderColorDark(VisualStyleFromTreeControlStyles(style));
                            GroupForeColor = MediaPlayerColorTable.TitleActiveTextColor(VisualStyleFromTreeControlStyles(style));
                            GroupSelectedForeColor = Color.White;
                            break;
                        case TreeControlStyles.GroupMediaOrangeDark:
                            GroupColoring = GroupColoring.MediaPlayerOrangeDark;
                            BorderColor = MediaPlayerColorTable.TitleBorderColorDark(VisualStyleFromTreeControlStyles(style));
                            GroupForeColor = MediaPlayerColorTable.TitleActiveTextColor(VisualStyleFromTreeControlStyles(style));
                            GroupSelectedForeColor = Color.White;
                            break;
                        case TreeControlStyles.GroupMediaPurpleDark:
                            GroupColoring = GroupColoring.MediaPlayerPurpleDark;
                            BorderColor = MediaPlayerColorTable.TitleBorderColorDark(VisualStyleFromTreeControlStyles(style));
                            GroupForeColor = MediaPlayerColorTable.TitleActiveTextColor(VisualStyleFromTreeControlStyles(style));
                            GroupSelectedForeColor = Color.White;
                            break;
                    }

                    TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                    GroupTextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                    GroupHotTrack = false;
                    GroupArrows = true;
                    break;
                case TreeControlStyles.GroupOfficeLight:
					ResetAllProperties();

					// Override the properties that are group specific
					BorderStyle = TreeBorderStyle.Solid;
					BoxDrawStyle = DrawStyle.Themed;
					ViewControllers = ViewControllers.Group;
					VerticalGranularity = VerticalGranularity.Pixel;
                    GroupFont = new Font("Tahoma", SystemInformation.MenuFont.SizeInPoints, FontStyle.Regular);
					GroupForeColor = Color.Black;
					GroupBorderStyle = GroupBorderStyle.VerticalEdges;
					GroupSelectedForeColor = Color.Black;
					GroupSelectedFontStyle = FontStyle.Bold;
					GroupHotFontStyle = FontStyle.Bold;
					GroupColoring = GroupColoring.Office2003Light;
					GroupTextRenderingHint = TextRenderingHint.SystemDefault;
					GroupHotTrack = false;
					GroupArrows = true;
					break;
				case TreeControlStyles.GroupOfficeDark:
					ResetAllProperties();

					// Override the properties that are group specific
					BorderStyle = TreeBorderStyle.Solid;
					BoxDrawStyle = DrawStyle.Themed;
					ViewControllers = ViewControllers.Group;
					VerticalGranularity = VerticalGranularity.Pixel;
                    GroupFont = new Font("Tahoma", 11f, FontStyle.Bold);
					
					// Text colour depends on themes
					if (ThemeHelper.IsAppThemed)
						GroupForeColor = Color.White;
					else
						GroupForeColor = SystemColors.ControlText;
						
					GroupSelectedForeColor = Color.Black;
					GroupBorderStyle = GroupBorderStyle.VerticalEdges;
					GroupSelectedFontStyle = FontStyle.Bold;
					GroupHotFontStyle = FontStyle.Bold;
					GroupColoring = GroupColoring.Office2003Dark;
                    GroupTextRenderingHint = TextRenderingHint.SystemDefault;
                    GroupHotTrack = false;
					GroupArrows = true;
					break;
				case TreeControlStyles.List:
					ResetAllProperties();

					// Override the properties that are list specific
					ColumnWidth = 0;
					BorderIndent.Top = 1;
					BorderIndent.Left = 1;
					BorderIndent.Bottom = 1;
					BorderIndent.Right = 1;
					ClickExpand = ClickExpandAction.None;
					ExtendToRight = true;
					CanUserExpandCollapse = false;
					break;
			}
		}

		/// <summary>
		/// Request that all nodes be redrawn.
		/// </summary>
		public void InvalidateNodeDrawing()
		{
			// Need to recalculate node drawing
			_nodeDrawingValid = false;

			// Change in size means updating scroll bars
			_scrollBarsValid = false;

			// Do we reflect change instantly, or wait for paint event
			if (InstantUpdate)
				Refresh();

			// Always do another invalidate
			InvalidateAll();
		}

		/// <summary>
		/// Request that the given Node instance is redrawn if visible.
		/// </summary>
		/// <param name="n">Node instance to redraw.</param>
		public void InvalidateNode(Node n)
		{
			// Use existing method and default to only redrawing the node itself
			InvalidateNode(n, false);
		}
		
		/// <summary>
		/// Request that the given Node instance is redrawn if visible.
		/// </summary>
		/// <param name="n">Node instance to redraw.</param>
		/// <param name="recurse">Should all children be invalidated recursively.</param>
		public void InvalidateNode(Node n, bool recurse)
		{
			// Can only invalidate if the node is visible
			if (n.Visible)
			{
				// Get correct bounding rectangle
				Rectangle bounds = recurse ? n.Cache.ChildBounds : n.Cache.Bounds;
			
				// Convert from node to client space
				bounds = NodeSpaceToClient(bounds);
			
				// Expand on all edges to cover rounding erros
				bounds.Inflate(2, 2);
			
				// Extend to right hand side of control
				bounds.Width = Width - bounds.Left;
			
				// Invalidate appropriate client area
				Invalidate(bounds);
			}
		}

		/// <summary>
		/// Request that the entire line that the given Node instance is redrawn if visible.
		/// </summary>
		/// <param name="n">Node instance to redraw.</param>
		/// <param name="recurse">Should all children be invalidated recursively.</param>
		public void InvalidateNodeLine(Node n, bool recurse)
		{
			// Can only invalidate if the node is visible
			if (n.Visible)
			{
				Rectangle rect;

				if (recurse)
					rect = NodeSpaceToClient(n.Cache.ChildBounds);
				else
					rect = NodeSpaceToClient(n.Cache.Bounds);

				// Stretch the rectangle to cover entire line, not just node
				rect.X = 0;
				rect.Width = Width;

				Invalidate(rect);
			}
		}

		/// <summary>
		/// Request that the given NodeCollection instance is redrawn if visible.
		/// </summary>
		/// <param name="nc">NodeCollection instance to redraw.</param>
		public void InvalidateNodeCollection(NodeCollection nc)
		{
			Invalidate(NodeSpaceToClient(nc.Cache.Bounds));
		}

		/// <summary>
		/// Converts a point from client space coordinates to node space.
		/// </summary>
		/// <param name="point">Client point.</param>
		/// <returns>Point converted to node space.</returns>
		public Point ClientPointToNode(Point point)
		{
			return ClientToNodeSpace(point);
		}

		/// <summary>
		/// Converts a rectangle from client space coordinates to node space.
		/// </summary>
		/// <param name="rect">Client rectangle.</param>
		/// <returns>Rectangle converted to node space.</returns>
		public Rectangle ClientRectToNode(Rectangle rect)
		{
			return ClientToNodeSpace(rect);
		}

		/// <summary>
		/// Converts a point from node space coordinates to client space.
		/// </summary>
		/// <param name="point">Node point.</param>
		/// <returns>Point converted to client space.</returns>
		public Point NodePointToClient(Point point)
		{
			return NodeSpaceToClient(point);
		}

		/// <summary>
		/// Converts a rectangle from node space to client space.
		/// </summary>
		/// <param name="rect">Node rectangle.</param>
		/// <returns>Rectangle converted to client space.</returns>
		public Rectangle NodeRectToClient(Rectangle rect)
		{
			return NodeSpaceToClient(rect);
		}

		/// <summary>
		/// Ensure this node is displayed and make parent nodes visible and expanded if needed.
		/// </summary>
		/// <param name="n">Node to ensure is displayed.</param>
		/// <returns>true if node is now displayed; otherwise false.</returns>
		public bool EnsureDisplayed(Node n)
		{
			return EnsureDisplayed(n, true, true);
		}
		
		/// <summary>
		/// Ensure this node is displayed.
		/// </summary>
		/// <param name="n">Node to ensure is displayed.</param>
		/// <param name="expand">Should nodes be expanded to ensure shown.</param>
		/// <param name="visible">Should nodes be made visible to ensure shown.</param>
		/// <returns>true if node is now displayed; otherwise false.</returns>
		public bool EnsureDisplayed(Node n, bool expand, bool visible)
		{
			// Ensure the parent chain is expanded and made visible, where needed
			if (!EnsureParentChain(n, expand, visible))
				return false;

			// If the node is not in the display list then it has been added but
			// not processed yet to appear in the display list. So we need to perform
			// the required processing to get it into the list.
			if (FindDisplayIndex(n) == -1)
			{
				// Perform a full recalculation of nodes and scroll bars
				using(Graphics g = CreateGraphics())
					CalculationCycle(g);
			}			

			// Get the current client based position of the node
			Rectangle bounds = NodeSpaceToClient(n.Cache.Bounds);
			
			bool scrollChange = false;

			// Find which edges are
			bool lEdge = (bounds.Left < _drawRectangle.Left);
			bool rEdge = (bounds.Right > _drawRectangle.Right);
			bool tEdge = (bounds.Top < _drawRectangle.Top);
			bool bEdge = (bounds.Bottom > _drawRectangle.Bottom);

            // Are we allowed to make a horizontal scrolling change?
            if (!IgnoreEnsureDisplayedH)
            {
                // Do we need to make a horizontal scrolling change?
                if (lEdge || rEdge)
                {
                    // Is the width of the node wider than possible display?
                    if (bounds.Width > _drawRectangle.Width)
                    {
                        // Then show the node from left onwards
                        lEdge = true;
                    }

                    // Do we need to alter scrolling leftwards?
                    if (lEdge)
                    {
                        // Can we fit all the item on the display if 
                        // we scroll entirely back to the left hand side
                        if (n.Cache.Bounds.Right < _drawRectangle.Width)
                            _offset.X = 0;
                        else
                            _offset.X -= _drawRectangle.Left - bounds.Left;

                        scrollChange = true;
                    }
                    else
                    {
                        // Do we need to alter scrolling rightwards?
                        if (rEdge)
                        {
                            _offset.X += bounds.Right - _drawRectangle.Right;
                            scrollChange = true;
                        }
                    }
                }
                else
                {
                    // Can we fit all the item on the display if 
                    // we scroll entirely back to the left hand side
                    if (n.Cache.Bounds.Right < _drawRectangle.Width)
                    {
                        _offset.X = 0;
                        scrollChange = true;
                    }
                }
            }

            // Are we allowed to make a vertical scrolling change?
            if (!IgnoreEnsureDisplayedV)
            {
                // Do we need to make a vertical scrolling change?
                if (tEdge || bEdge)
                {
                    // Is the height of the node taller than possible display?
                    if (bounds.Height > _drawRectangle.Height)
                    {
                        // Then show the node from top onwards
                        tEdge = true;
                    }

                    // Do we need to alter scrolling upwards?
                    if (tEdge)
                    {
                        // Are we using pixel level scrolling
                        if (VerticalGranularity == VerticalGranularity.Pixel)
                            _offset.Y -= _drawRectangle.Top - bounds.Top;
                        else
                        {
                            // Node level, so just find the index of item in display list of nodes
                            _offset.Y = FindDisplayIndex(n);
                        }

                        scrollChange = true;
                    }
                    else if (bEdge)
                    {
                        // Are we using pixel level scrolling
                        if (VerticalGranularity == VerticalGranularity.Pixel)
                            _offset.Y += bounds.Bottom - _drawRectangle.Bottom;
                        else
                        {
                            // Start by finding the node index in list of display nodes
                            int index = FindDisplayIndex(n);

                            // Find number of complete items from this point
                            _offset.Y = InsidePageUpIndex(index, _drawRectangle);
                        }

                        scrollChange = true;
                    }
                }
            }

			// Do we need to reprocess scroll bar positions?
			if (scrollChange)
			{
				// Any change in scrolling must remove the tooltip
				SetTooltipNode(null);

				bool previous = _scrollBarsValid;
				_scrollBarsValid = false;
				UpdateScrollBars();
				_scrollBarsValid = previous;
			}
			
			return true;
		}
		
		/// <summary>
		/// Attempt to show given node at the top of display.
		/// </summary>
		/// <param name="n">Node to try and display.</param>
		/// <param name="expand">Should nodes be expanded to ensure shown.</param>
		/// <param name="visible">Should nodes be made visible to ensure shown.</param>
		/// <returns>true if node is now displayed; otherwise false.</returns>
		public void SetTopNode(Node n, bool expand, bool visible)
		{
			// Ensure the parent chain is expanded and made visible, where needed
			if (!EnsureParentChain(n, expand, visible))
				return;

			// If the node is not in the display list then it has been added but
			// not processed yet to appear in the display list. So we need to perform
			// the required processing to get it into the list.
			if (FindDisplayIndex(n) == -1)
			{
				// Perform a full recalculation of nodes and scroll bars
				using(Graphics g = CreateGraphics())
					CalculationCycle(g);
			}			

			// Get the current client based position of the node
			Rectangle bounds = NodeSpaceToClient(n.Cache.Bounds);
			
			bool scrollChange = false;

			// Find which edges are
			bool lEdge = (bounds.Left < _drawRectangle.Left);
			bool rEdge = (bounds.Right > _drawRectangle.Right);
			bool tEdge = (bounds.Top < _drawRectangle.Top);
			bool bEdge = (bounds.Bottom > _drawRectangle.Bottom);

			// Do we need to make a horizontal scrolling change?
			if (lEdge || rEdge)
			{
				// Is the width of the node wider than possible display?
				if (bounds.Width > _drawRectangle.Width)
				{
					// Then show the node from left onwards
					lEdge = true;
				}
			
				// Do we need to alter scrolling leftwards?
				if (lEdge)
				{
					// Can we fit all the item on the display if 
					// we scroll entirely back to the left hand side
					if (n.Cache.Bounds.Right < _drawRectangle.Width)
						_offset.X = 0;
					else
						_offset.X -= _drawRectangle.Left - bounds.Left;
					
					scrollChange = true;
				}
				else
				{
					// Do we need to alter scrolling rightwards?
					if (rEdge)
					{
						_offset.X += bounds.Right - _drawRectangle.Right;
						scrollChange = true;
					}
				}
			}
			else
			{
				// Can we fit all the item on the display if 
				// we scroll entirely back to the left hand side
				if (n.Cache.Bounds.Right < _drawRectangle.Width)
				{
					_offset.X = 0;
					scrollChange = true;
				}
			}

			// Are we using pixel level scrolling
			if (VerticalGranularity == VerticalGranularity.Pixel)
			{
				if (_offset.Y != bounds.Top)
				{
					_offset.Y = bounds.Top;
					scrollChange = true;
				}
			}
			else
			{
				int index = FindDisplayIndex(n);
				
				// Node level, so just find the index of item in display list of nodes
				if (_offset.Y != index)
				{
					_offset.Y = index;
					scrollChange = true;
				}
			}

			// Do we need to reprocess scroll bar positions?
			if (scrollChange)
			{
				// Any change in scrolling must remove the tooltip
				SetTooltipNode(null);

				bool previous = _scrollBarsValid;
				_scrollBarsValid = false;
				UpdateScrollBars();
				_scrollBarsValid = previous;
			}
		}

		/// <summary>
		/// Retrieves the node that is at the specified vertical location.
		/// </summary>
		/// <param name="y">Y pixel position.</param>
		/// <returns>The Node at the specified vertical location; otherwise null.</returns>
		public Node GetNodeAt(int y)
		{
			// Find the node the contains the given vertical client point
			return FindDisplayNodeFromY(y);
		}

		/// <summary>
		/// Retrieves the node that is at the specified location.
		/// </summary>
		/// <param name="x">X pixel position.</param>
		/// <param name="y">Y pixel position.</param>
		/// <returns>The Node at the specified location; otherwise null.</returns>
		public Node GetNodeAt(int x, int y)
		{
			// Just use the existing method
			return GetNodeAt(new Point(x, y));
		}

		/// <summary>
		/// Retrieves the node that is at the specified location.
		/// </summary>
		/// <param name="pt">Pixel position.</param>
		/// <returns>The Node at the specified location; otherwise null.</returns>
		public Node GetNodeAt(Point pt)
		{
			// Find the node that contains the given client point
			return FindDisplayNodeFromPoint(pt);
		}

		/// <summary>
		/// Gets reference to the first Node in entire hierarchy.
		/// </summary>
		/// <returns>null if no Node found; otherwise reference to node.</returns>
		public Node GetFirstNode()
		{
			// Delegate work onto the root level collection
			return Nodes.GetFirstNode();
		}

		/// <summary>
		/// Gets reference to the first displayed Node in entire hierarchy.
		/// </summary>
		/// <returns>null if no Node found; otherwise reference to node.</returns>
		public Node GetFirstDisplayedNode()
		{
			// Delegate work onto the root level collection
			return Nodes.GetFirstDisplayedNode();
		}

		/// <summary>
		/// Gets reference to the last Node in entire hierarchy.
		/// </summary>
		/// <returns>null if no Node found; otherwise reference to node.</returns>
		public Node GetLastNode()
		{
			// Delegate work onto the root level collection
			return Nodes.GetLastNode();
		}

		/// <summary>
		/// Gets reference to the last Node in entire hierarchy.
		/// </summary>
		/// <returns>null if no Node found; otherwise reference to node.</returns>
		public Node GetLastDisplayedNode()
		{
			// Delegate work onto the root level collection
			return Nodes.GetLastDisplayedNode();
		}
		
		/// <summary>
		/// Gets the node instance with the specified unique key.
		/// </summary>
		/// <param name="key">Unique key value.</param>
		/// <returns>node matching key; otherwise null if no match is found.</returns>
		public Node GetNodeFromKey(object key)
		{
			// Cannot use a null key in a hashtable
			if (key == null)
				return null;
				
			return (_nodeKeys[key] as Node);
		}


        /// <summary>
        /// End the editing of a node lable.
        /// </summary>
        /// <param name="quit">Should quit and cancel changes.</param>
        public void EndEdit(bool quit)
        {
            // Only quit if currently editing a node
            if (_labelEditBox != null)
                EndEditLabel(quit);
        }

		/// <summary>
		/// Raises the ShowContextMenuNode event.
		/// </summary>
		public virtual bool OnShowContextMenuNode(Node n)
		{
			CancelNodeEventArgs cea = new CancelNodeEventArgs(n);
		
			if (ShowContextMenuNode != null)
				ShowContextMenuNode(this, cea);
				
			return !cea.Cancel;
		}

		/// <summary>
		/// Raises the ClientDragEnter event.
		/// </summary>
		/// <param name="e">A DragEventArgs that contains the event data.</param>
		public virtual void OnClientDragEnter(DragEventArgs e)
		{
			if (ClientDragEnter != null)
				ClientDragEnter(this, e);
		}

		/// <summary>
		/// Raises the ClientDragLeave event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		public virtual void OnClientDragLeave(EventArgs e)
		{
			if (ClientDragLeave != null)
				ClientDragLeave(this, e);
		}

		/// <summary>
		/// Raises the ClientDragOver event.
		/// </summary>
		/// <param name="e">A DragEventArgs that contains the event data.</param>
		public virtual void OnClientDragOver(DragEventArgs e)
		{
			if (ClientDragOver != null)
				ClientDragOver(this, e);
		}

		/// <summary>
		/// Raises the ClientDragDrop event.
		/// </summary>
		/// <param name="e">A DragEventArgs that contains the event data.</param>
		public virtual void OnClientDragDrop(DragEventArgs e)
		{
			if (ClientDragDrop != null)
				ClientDragDrop(this, e);
		}
		
		/// <summary>
		/// Raises the NodeDragEnter event.
		/// </summary>
		/// <param name="n">Context Node this event is for.</param>
		/// <param name="e">A DragEventArgs that contains the event data.</param>
		public virtual void OnNodeDragEnter(Node n, DragEventArgs e)
		{
			if (NodeDragEnter != null)
				NodeDragEnter(this, n, e);
		}

		/// <summary>
		/// Raises the NodeDragLeave event.
		/// </summary>
		/// <param name="n">Context Node this event is for.</param>
		public virtual void OnNodeDragLeave(Node n)
		{
			if (NodeDragLeave != null)
				NodeDragLeave(this, new NodeEventArgs(n));
		}

		/// <summary>
		/// Raises the NodeDragOver event.
		/// </summary>
		/// <param name="n">Context Node this event is for.</param>
		/// <param name="e">A DragEventArgs that contains the event data.</param>
		public virtual void OnNodeDragOver(Node n, DragEventArgs e)
		{
			if (NodeDragOver != null)
				NodeDragOver(this, n, e);
		}

		/// <summary>
		/// Raises the NodeDragDrop event.
		/// </summary>
		/// <param name="n">Context Node this event is for.</param>
		/// <param name="e">A DragEventArgs that contains the event data.</param>
		public virtual void OnNodeDragDrop(Node n, DragEventArgs e)
		{
			if (NodeDragDrop != null)
				NodeDragDrop(this, n, e);
		}
		
		/// <summary>
		/// Raises the NodeDrag event.
		/// </summary>
		/// <param name="e">An StartDragEventArgs structure containing event data.</param>
		public virtual void OnNodeDrag(StartDragEventArgs e)
		{
			if (NodeDrag != null)
				NodeDrag(this, e);
		}

		/// <summary>
		/// Raises the GroupDragEnter event.
		/// </summary>
		/// <param name="n">Context Node this event is for.</param>
		/// <param name="e">A DragEventArgs that contains the event data.</param>
		public virtual void OnGroupDragEnter(Node n, DragEventArgs e)
		{
			if (GroupDragEnter != null)
				GroupDragEnter(this, n, e);
		}

		/// <summary>
		/// Raises the GroupDragLeave event.
		/// </summary>
		/// <param name="n">Context Node this event is for.</param>
		public virtual void OnGroupDragLeave(Node n)
		{
			if (GroupDragLeave != null)
				GroupDragLeave(this, new NodeEventArgs(n));
		}

		/// <summary>
		/// Raises the GroupDragOver event.
		/// </summary>
		/// <param name="n">Context Node this event is for.</param>
		/// <param name="e">A DragEventArgs that contains the event data.</param>
		public virtual void OnGroupDragOver(Node n, DragEventArgs e)
		{
			if (GroupDragOver != null)
				GroupDragOver(this, n, e);
		}

		/// <summary>
		/// Raises the GroupDragDrop event.
		/// </summary>
		/// <param name="n">Context Node this event is for.</param>
		/// <param name="e">A DragEventArgs that contains the event data.</param>
		public virtual void OnGroupDragDrop(Node n, DragEventArgs e)
		{
			if (GroupDragDrop != null)
				GroupDragDrop(this, n, e);
		}

		/// <summary>
		/// Raises the BeforeSelect event.
		/// </summary>
		public virtual bool OnBeforeSelect(Node n)
		{
			CancelNodeEventArgs cea = new CancelNodeEventArgs(n);
		
			if (BeforeSelect != null)
				BeforeSelect(this, cea);
				
			return !cea.Cancel;
		}

        /// <summary>
        /// Raises the BeforeDeselect event.
        /// </summary>
        public virtual void OnBeforeDeselect(Node n)
        {
            NodeEventArgs cea = new NodeEventArgs(n);

            if (BeforeDeselect != null)
                BeforeDeselect(this, cea);
        }
        
        /// <summary>
		/// Raises the AfterSelect event.
		/// </summary>
		public virtual void OnAfterSelect(Node n)
		{
			NodeEventArgs cea = new NodeEventArgs(n);
		
			if (AfterSelect != null)
				AfterSelect(this, cea);
		}

		/// <summary>
		/// Raises the AfterDeselect event.
		/// </summary>
		public virtual void OnAfterDeselect(Node n)
		{
			NodeEventArgs cea = new NodeEventArgs(n);
		
			if (AfterDeselect != null)
				AfterDeselect(this, cea);
		}

		/// <summary>
		/// Raises the BeforeCheck event.
		/// </summary>
		public virtual bool OnBeforeCheck(Node n)
		{
			CancelNodeEventArgs cea = new CancelNodeEventArgs(n);
		
			if (BeforeCheck != null)
				BeforeCheck(this, cea);
				
			return !cea.Cancel;
		}

		/// <summary>
		/// Raises the AfterCheck event.
		/// </summary>
		public virtual void OnAfterCheck(Node n)
		{
			NodeEventArgs cea = new NodeEventArgs(n);
		
			if (AfterCheck != null)
				AfterCheck(this, cea);
		}

		/// <summary>
		/// Raises the BeforeExpand event.
		/// </summary>
		public virtual bool OnBeforeExpand(Node n)
		{
			CancelNodeEventArgs cea = new CancelNodeEventArgs(n);
		
			if (BeforeExpand != null)
				BeforeExpand(this, cea);
				
			return !cea.Cancel;
		}

		/// <summary>
		/// Raises the AfterExpand event.
		/// </summary>
		public virtual void OnAfterExpand(Node n)
		{
			NodeEventArgs cea = new NodeEventArgs(n);
		
			if (AfterExpand != null)
				AfterExpand(this, cea);
		}

		/// <summary>
		/// Raises the BeforeCollapse event.
		/// </summary>
		public virtual bool OnBeforeCollapse(Node n)
		{
			CancelNodeEventArgs cea = new CancelNodeEventArgs(n);
		
			if (BeforeCollapse != null)
				BeforeCollapse(this, cea);
				
			return !cea.Cancel;
		}

		/// <summary>
		/// Raises the AfterCollapse event.
		/// </summary>
		public virtual void OnAfterCollapse(Node n)
		{
			NodeEventArgs cea = new NodeEventArgs(n);
		
			if (AfterCollapse != null)
				AfterCollapse(this, cea);
		}

		/// <summary>
		/// Raises the BeforeLabelEdit event.
		/// </summary>
		public virtual bool OnBeforeLabelEdit(Node n, ref string label)
		{
			LabelEditEventArgs cea = new LabelEditEventArgs(n, label);
		
			if (BeforeLabelEdit != null)
				BeforeLabelEdit(this, cea);
				
			// Update with any changed label text
			label = cea.Label;
			
			return !cea.Cancel;
		}

		/// <summary>
		/// Raises the AfterLabelEdit event.
		/// </summary>
		public virtual bool OnAfterLabelEdit(Node n, ref string label)
		{
			LabelEditEventArgs cea = new LabelEditEventArgs(n, label);
		
			if (AfterLabelEdit != null)
				AfterLabelEdit(this, cea);
				
			// Update with any changed label text
			label = cea.Label;
			
			return !cea.Cancel;
		}

        /// <summary>
        /// Raises the AfterSelectionChanged event.
        /// </summary>
        public virtual void OnAfterSelectionChanged()
        {
            if (AfterSelectionChanged != null)
                AfterSelectionChanged(this, EventArgs.Empty);
        }

		/// <summary>
		/// Raises the ShowContextMenuSpace event.
		/// </summary>
		public virtual bool OnShowContextMenuSpace()
		{
			CancelEventArgs cea = new CancelEventArgs();
		
			if (ShowContextMenuSpace != null)
				ShowContextMenuSpace(this, cea);
				
			return !cea.Cancel;
		}

        public virtual void Sort()
        {
            this.Nodes.SortTree();
        }

		/// <summary>
		/// Releases all resources used by the Control.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				// Clear cached resources for lines and boxes
				ClearLineBoxCache();
				
 				// Remove any showing tooltip
 				RemoveAnyToolTip();
				
				// Dispose of any timers
				_dragBumpTimer.Stop();
				_dragHoverTimer.Stop();
				_autoEditTimer.Stop();
				_infotipTimer.Stop();
				_dragBumpTimer.Dispose();
				_dragHoverTimer.Dispose();
				_autoEditTimer.Dispose();
				_infotipTimer.Dispose();

				// Unattach from events for sub objects/controls
				_vBar.ValueChanged -= new EventHandler(OnVertValueChanged);
				_hBar.ValueChanged -= new EventHandler(OnHorzValueChanged);
				_themeTreeView.ThemeOpened -= new EventHandler(OnThemeChanged);
				_themeTreeView.ThemeClosed -= new EventHandler(OnThemeChanged);
				_themeCheckbox.ThemeOpened -= new EventHandler(OnThemeChanged);
				_themeCheckbox.ThemeClosed -= new EventHandler(OnThemeChanged);
				_borderIndent.IndentChanged -= new EventHandler(OnBorderIndentChanged);

				Controls.Remove(_corner);
				Controls.Remove(_hBar);
				Controls.Remove(_vBar);

				// Unattach from notifications that would prevent control being removed
				Microsoft.Win32.SystemEvents.UserPreferenceChanged -= 
					new UserPreferenceChangedEventHandler(OnPreferenceChanged);

                // Clear down any caching of node instances
                if (_displayNodes != null)
                    _displayNodes.Clear();
                if (_nodeKeys != null)
                    _nodeKeys.Clear();
                if (_selected != null)
                    _selected.Clear();
                if (_cachedSelected != null)
                    _cachedSelected.Clear();
                if (_rootNodes != null)
                    _rootNodes.Clear();
                _dragNode = null;
                _mouseDownNode = null;
                _labelEditNode = null;
                _autoEditNode = null;
                _lastSelectedNode = null;

                // Color details has resources that need releasing
				_colorDetails.Dispose();

				// Must tell theme objects to dispose any Win32 handles it has
				_themeTreeView.Dispose();
				_themeCheckbox.Dispose();
			}

			base.Dispose(disposing);
		}

		/// <summary>
		/// Raises the SizeChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnSizeChanged(EventArgs e)
		{
			// Inform the collection and node that size has changed
			NodeVC.SizeChanged(this);			
			CollectionVC.SizeChanged(this);			

			// Must recalculate the inner drawing rectangle to reflect changing size
			InvalidateInnerRectangle();
			
			base.OnSizeChanged(e);
		}

		/// <summary>
		/// Raises the FontChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnFontChanged(EventArgs e)
		{
			// Invalidate the cached font height/bold font
			InternalResetFontHeight();
			InternalResetFontBoldItalic();

			// The minimum node height is based on Font, so reset
			ResetMinimumNodeHeight();

			// Must recalculate drawing of all nodes for new font
			MarkAllNodeSizesDirty();

			base.OnFontChanged(e);
		}

		/// <summary>
		/// Raises the GotFocus event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnGotFocus(EventArgs e)
		{
			// Are we currently focused on a Node?
			if (_focusNode == null)
			{
				// No, then find the first displayed node
				Node first = Nodes.GetFirstDisplayedNode();

				// If there is a node visible...				
				if (first != null)
				{
					// We prefert to select the node if possible
					if (first.VC.CanSelectNode(this, first))
						SingleSelect(first);
					else
					{
						// Otherwise make do with just focusing it
						SetFocusNode(first);
					}
				}
			}
		
			InvalidateSelection();
			InvalidateFocus();
			base.OnGotFocus(e);
		}

		/// <summary>
		/// Raises the LostFocus event. 
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnLostFocus(EventArgs e)
		{
			InvalidateSelection();
			InvalidateFocus();
			base.OnLostFocus(e);
		}

		/// <summary>
		/// Raises the EnabledChangled event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnEnabledChanged(EventArgs e)
		{
			// Cause a redraw to reflect changed state
			InvalidateAll();
			base.OnEnabledChanged(e);
		}

        /// <summary>
        /// Processes a command key.
        /// </summary>
        /// <param name="msg">A Message, passed by reference, that represents the window message to process.</param>
        /// <param name="keyData">One of the Keys values that represents the key to process.</param>
        /// <returns>True is handled; otherwise false.</returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((SelectedCount > 0) && (ContextMenuNode != null))
            {
                if (CheckContextMenuForShortcut(ContextMenuNode, ref msg, keyData))
                    return true;
            }
            else if ((SelectedCount == 0) && (ContextMenuSpace != null))
            {
                if (CheckContextMenuForShortcut(ContextMenuSpace, ref msg, keyData))
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        
        /// <summary>
		/// Processes a dialog key.
		/// </summary>
		/// <param name="keyData">One of the Keys values that represents the key to process.</param>
		/// <returns>true if the key was processed by the control; otherwise, false.</returns>
		protected override bool ProcessDialogKey(Keys keyData)
		{
            // Not handled by default
			bool ret = false;

			// Find state of keys from bit flag
			Keys basic = (keyData & ~(Keys.Shift | Keys.Control));
			bool shiftKey = (keyData & Keys.Shift) == Keys.Shift;
			bool controlKey = (keyData & Keys.Control) == Keys.Control;

			if (basic == Keys.Up)
				ret = ProcessUpKey(shiftKey, controlKey);
			else if (basic == Keys.Down)
				ret = ProcessDownKey(shiftKey, controlKey);
			else if (basic == Keys.Left)
				ret = ProcessLeftKey(shiftKey, controlKey);
			else if (basic == Keys.Right)
				ret = ProcessRightKey(shiftKey, controlKey);
			else if (basic == Keys.Home)
				ret = ProcessHomeKey(shiftKey, controlKey);
			else if (basic == Keys.End)
				ret = ProcessEndKey(shiftKey, controlKey);
			else if (basic == Keys.PageUp)
				ret = ProcessPageUpKey(shiftKey, controlKey);
			else if (basic == Keys.PageDown)
				ret = ProcessPageDownKey(shiftKey, controlKey);
			else if (basic == Keys.Add)
				ret = ProcessPlusKey();
			else if (basic == Keys.Multiply)
				ret = ProcessPlusKey();
			else if (basic == Keys.Subtract)
				ret = ProcessMinusKey();

			// If not handled, then let parent handle it
			if (!ret)
				ret = base.ProcessDialogKey(keyData);

			return ret;
		}
		
		/// <summary>
		/// Raises the MouseWheel event.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the event data.</param>
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			// Only allow scrolling up and down if we have vertical scroll bar
			if (EnableMouseWheel && _vBar.Enabled && _vBar.Visible)
			{
				// Find number of notches the mouse wheel has moved
				int detents = e.Delta / WHEEL_DELTA;

				// Find new position by moving one place per notch
				int newValue = _vBar.Value - (detents * MouseWheelUnits);

				// Maximum allowed value needs to account for showing last page
				int maxLimit = _vBar.Maximum - _vBar.LargeChange + 1;

				// Limit check
				if (newValue > maxLimit) newValue = maxLimit;
				if (newValue < _vBar.Minimum) newValue = _vBar.Minimum;

				// Use new value
				if (_vBar.Value != newValue)
					_vBar.Value = newValue;
			}

			base.OnMouseWheel(e);
		}

		/// <summary>
		/// Raises the MouseEnter event.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the event data. </param>
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
		}

		/// <summary>
		/// Raises the MouseDown event.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the event data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			// Make sure we have the focus
			Focus();
			
			// If we managed to get the focus
			if (ContainsFocus)
			{
				Point pt = new Point(e.X, e.Y);

				// Reset the mouse down information used to start drag
				_mouseDownNode = null;
				_mouseDownPt = Point.Empty;

				// Define the hot point in node space
				HotPoint = ClientToNodeSpace(pt);

				// Find the node under the point
				Node downNode = FindDisplayNodeFromY(pt.Y);

				bool handled = false;

				// If we found a valid node, then pass event onto it
				if ((downNode != null) && (downNode.VC != null))
				{
                    // If the Node does not want or process the event
                    if (!downNode.VC.MouseDown(this, downNode, e.Button, HotPoint))
                    {
                        // Navigate up to the parent collection of nodes
                        NodeCollection nc = downNode.ParentNodes;

                        // Let each collection process until we reach the top
                        while (true)
                        {
                            // Break out when a collection handles it
                            if ((nc.VC != null) && (nc.VC.MouseDown(this, nc, downNode, e.Button, HotPoint)))
                            {
                                handled = true;
                                break;
                            }

                            if (nc.ParentNode != null)
                            {
                                // Move up to the next higher collection
                                nc = nc.ParentNode.ParentNodes;

                                // Node is no longer a direct child of the collection
                                downNode = null;
                            }
                            else
                            {
                                // Must be at root collection, leave!
                                break;
                            }
                        }
                    }
                    else
                    {
                        handled = true;

                        // The node has processed the event, but was it the left mouse
                        if (e.Button == MouseButtons.Left)
                        {
                            // Then need to remember the node and the point, 
                            // just in case this is the start of a drag attempt
                            _mouseDownNode = downNode;
                            _mouseDownPt = pt;
                        }
                    }
				}

				if (!handled)
				{
					// Only right mouse button will show context menu
					if (e.Button == MouseButtons.Right)
					{
						// Do we need to show context menu for remainder space?
						if (OnShowContextMenuSpace())
						{
							// If we have a menu to show...
							if (ContextMenuSpace != null)
							{
								//...then show it now!
								ContextMenuSpace.Show(this, pt);
							}
						}
					}				
				}
			}
									
			base.OnMouseDown(e);
		}
		
		/// <summary>
		/// Raises the MouseUp event.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the event data.</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
            Point pt = new Point(e.X, e.Y);

			// Reset the mouse down information used to start drag
			_mouseDownNode = null;
			_mouseDownPt = Point.Empty;

			// Find the node under the point
			Node upNode = FindDisplayNodeFromY(pt.Y);

			// If we found a valid node, then pass event onto it
			if ((upNode != null) && (upNode.VC != null))
                upNode.VC.MouseUp(this, upNode, e.Button, HotPoint);
				
			base.OnMouseUp(e);
		}

		/// <summary>
		/// Raises the MouseEnter event.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the event data.</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			Point pt = new Point(e.X, e.Y);

			bool dragging = false;
			
			// If we are tracking a node as a potential drag attempt...
			if (_mouseDownNode != null)
			{
				// Then create rectangle for drag area
				Rectangle dragRect = new Rectangle(_mouseDownPt, Size.Empty);
				dragRect.Inflate(SystemInformation.DragSize);
				
				// Has the mouse been moved outside this rectangle?
				if (!dragRect.Contains(pt))
				{
					// Create events class that can be modified
					StartDragEventArgs args = new StartDragEventArgs(_mouseDownNode, DragDropEffects.All);

					// Let any interested hook modify drag attempt
					OnNodeDrag(args);
					
					if (!args.Cancel)
					{
						// Do not obscure the display with a tooltip
						RemoveAnyToolTip();
						dragging = true;

						// Perform a drag and drop from this node
						DoDragDrop(args.Object, args.Effect);
					}
										
					// Null out any further attempt at drag and drop till another mouse down
					_mouseDownNode = null;
					_mouseDownPt = Point.Empty;
				}
			}
			
			// Define the hot point in node space
			HotPoint = ClientToNodeSpace(pt);

			Form parentForm = FindForm();
			if ((parentForm != null) && parentForm.ContainsFocus)
			{
				// Set the hot node as the one the mouse is now over
				SetHotNode(FindDisplayNodeFromY(pt.Y));
				
				// Do not show tooltip if dragging
				if (!dragging)
					SetTooltipNode(FindDisplayNodeFromPoint(pt));
			}
			else
			{
                // Do not perform hot tracking when out application is not active
				SetHotNode(null);
				SetTooltipNode(null);
			}
			
			base.OnMouseMove (e);
		}

		/// <summary>
		/// Raises the MouseLeave event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnMouseLeave(EventArgs e)
		{
			// The hot point is not valid any more
			InternalResetHotPoint();

			// Cannot have a hot node when mouse has left control
			SetHotNode(null);
			SetTooltipNode(null);

			base.OnMouseLeave(e);
		}
		
		/// <summary>
		/// Raises the DoubleClick event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnDoubleClick(EventArgs e)
		{
			Point pt = PointToClient(Control.MousePosition);

			// Define the hot point in node space
			HotPoint = ClientToNodeSpace(pt);

			// Find the node under the point
			Node downNode = FindDisplayNodeFromY(pt.Y);

			// If we found a valid node, then pass event onto it
			if ((downNode != null) && (downNode.VC != null))
			{
				// If the Node does not want or process the event
				if (!downNode.VC.DoubleClick(this, downNode, HotPoint))
				{
					// Navigate up to the parent collection of nodes
					NodeCollection nc = downNode.ParentNodes;

					// Let each collection process until we reach the top
					while(true)
					{
						// Break out when a collection handles it
						if ((nc.VC != null) && (nc.VC.DoubleClick(this, nc, downNode, HotPoint)))
							break;

						if (nc.ParentNode != null)
						{
							// Move up to the next higher collection
							nc = nc.ParentNode.ParentNodes;

							// Node is no longer a direct child of the collection
							downNode = null;
						}
						else
						{
							// Must be at root collection, leave!
							break;
						}
					}
				}
			}

			base.OnDoubleClick(e);
		}

		/// <summary>
		/// Raises the DragEnter event.
		/// </summary>
		/// <param name="drgevent">An DragEventArgs that contains the event data.</param>
		protected override void OnDragEnter(DragEventArgs drgevent)
		{
			// Store the currently selected nodes
			_cachedSelected = _selected;
			
			// Clear down the current selection
			_selected = new Hashtable();
			
			// Need redraw the whole display
			InvalidateDrawRectangle();
		
			// Create point for the current mouse position
			Point pt = PointToClient(new Point(drgevent.X, drgevent.Y));

			// Convert from client to the node space
			HotPoint = ClientToNodeSpace(pt);

			// Find the node under the point
			Node downNode = NodeVC.DragOverNodeFromPoint(this, pt);
			
			// Moving mouse over any right scroll bar counts as client area
			if (pt.X > _drawRectangle.Right)
				downNode = null;
			else
			{
				// Moving mouse over any bottom scroll bar counts as client area
				if (pt.Y > _drawRectangle.Bottom)
					downNode = null;
			}

			// Always allow drag operation
			drgevent.Effect = DragDropEffects.None;

			// If we are over a node...
			if ((downNode != null) && (downNode.VC != null))
			{
				// Tell the node VC to generate any event required
				downNode.VC.DragEnter(this, downNode, drgevent);
				
				// Start the hover timer for autoexpanding nodes
				_dragHoverTimer.Start();
			}
			else
			{
				// Generate any enter event for the remainder area
				OnClientDragEnter(drgevent);
			}
			
			// Remember which is the current drag node
			_dragNode = downNode;
			
			// Remember the requested drag effect for the drag node
			_dragEffects = drgevent.Effect;
			
			base.OnDragEnter(drgevent);
		}

		/// <summary>
		/// Raises the DragLeave event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnDragLeave(EventArgs e)
		{
			// No longer need to bump timer
			_dragBumpTimer.Stop();

			// If we are over a node...
			if (_dragNode != null)
			{
				// Stop any running hover timer
				_dragHoverTimer.Stop();

				// Tell the node VC to generate any leave event required
				_dragNode.VC.DragLeave(this, _dragNode);
				
				// No drag node to track anymore
				_dragNode = null;
			}
			else
			{
				// Generate a leave event for the remainder area
				OnClientDragLeave(e);
			}

			// Put back the original selection
			_selected = _cachedSelected;
			
			// No longer we need to remember anything
			_cachedSelected = null;
		
			// Need redraw the whole display
			InvalidateDrawRectangle();

			base.OnDragLeave(e);
		}

		/// <summary>
		/// Raises te DragOver event.
		/// </summary>
		/// <param name="drgevent">An DragEventArgs that contains the event data.</param>
		protected override void OnDragOver(DragEventArgs drgevent)
		{
			// Create point for the current mouse position
			Point pt = PointToClient(new Point(drgevent.X, drgevent.Y));

			// Convert from client to the node space
			HotPoint = ClientToNodeSpace(pt);

			// Find the node under the point
			Node downNode = NodeVC.DragOverNodeFromPoint(this, pt);
			
			// Moving mouse over any right scroll bar counts as client area
			if (pt.X > _drawRectangle.Right)
				downNode = null;
			else
			{
				// Moving mouse over any bottom scroll bar counts as client area
				if (pt.Y > _drawRectangle.Bottom)
					downNode = null;
			}
			
			// Always to not allowing drag operation
			drgevent.Effect = DragDropEffects.None;

			// If there has not been any change in the drag node...
			if (downNode == _dragNode)
			{
				// And if we are over a drag node
				if (_dragNode != null)
				{
					// Tell the node VC to generate any over event required
					_dragNode.VC.DragOver(this, _dragNode, drgevent);
				}
				else
				{
					// Generate an over event for the remainder area
					OnClientDragOver(drgevent);
				}
			}
			else
			{
				// If we are moving from no drag node to having one...
				if (_dragNode == null)
				{
					// Then generate a leave event for the remainder space
					OnClientDragLeave(EventArgs.Empty);

					// And any enter event required for the new drag node
					downNode.VC.DragEnter(this, downNode, drgevent);

					// Start the hover timer for autoexpanding nodes
					_dragHoverTimer.Start();
				}
				else
				{
					// If we are moving from a drag node to none...
					if (downNode == null)
					{
						// Stop any running hover timer
						_dragHoverTimer.Stop();

						// And any leave event required for the old drag node
						_dragNode.VC.DragLeave(this, _dragNode);

						// Then generate an enter event for the remainder space
						OnClientDragEnter(drgevent);
					}
					else
					{
						// Stop any running hover timer
						_dragHoverTimer.Stop();

						// Must be changing from one drag node to a different node
						_dragNode.VC.DragLeave(this, _dragNode);

						// And any enter event required for the new drag node
						downNode.VC.DragEnter(this, downNode, drgevent);

						// Start the hover timer for autoexpanding nodes
						_dragHoverTimer.Start();
					}
				}
				
				// Remember the new drag node
				_dragNode = downNode;
			}
			
			// Find the top and bottom bump boundaries
			int topBoundary = _drawRectangle.Top + BUMP_HEIGHT;
			int bottomBoundary = _drawRectangle.Bottom - BUMP_HEIGHT;
			
			// If the mouse is inside the top or bottom dump areas
			if (pt.Y < topBoundary)
			{
				_bumpUpwards = true;
				_dragBumpTimer.Start();
			}
			else if ((pt.Y > bottomBoundary) && (pt.Y < _drawRectangle.Bottom))
			{
				_bumpUpwards = false;
				_dragBumpTimer.Start();
			}
			else
			{
				// Do not need to bump timer
				_dragBumpTimer.Stop();
			}
			
			// Remember the requested drag effect for the drag node
			_dragEffects = drgevent.Effect;

			// Need redraw the whole display
			InvalidateDrawRectangle();
			
			base.OnDragOver(drgevent);
		}

		/// <summary>
		/// Raises the DragDrop event.
		/// </summary>
		/// <param name="drgevent">An DragEventArgs that contains the event data.</param>
		protected override void OnDragDrop(DragEventArgs drgevent)
		{
			// No longer need to bump timer
			_dragBumpTimer.Stop();
			
			// Put back the original selection
			_selected = _cachedSelected;
			
			// No longer we need to remember anything
			_cachedSelected = null;

			// Are we dropping onto a node?
			if (_dragNode != null)
			{
				// Stop any running hover timer
				_dragHoverTimer.Stop();

				// Let node generate any drop event required
				_dragNode.VC.DragDrop(this, _dragNode, drgevent);

				// No need to track a drag node anymore
				_dragNode = null;
			}
			else
			{
				// Generate an event for the remainder space
				OnClientDragDrop(drgevent);
			}
			
			// Need redraw the whole display
			InvalidateDrawRectangle();

			base.OnDragDrop(drgevent);
		}
		
		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			// Fill entire clip area with background color
			base.OnPaint(e);

            lock (((ICollection)Nodes).SyncRoot)
            {
                // Use the rendering hint defined by the user
                e.Graphics.TextRenderingHint = TextRenderingHint;

                // Perform a calculation cycle for nodes and scroll bars
                CalculationCycle(e.Graphics);

                // Draw the border according to border style
                DrawControlBorder(e);

                using (Region region = new Region(_drawRectangle))
                {
                    // New clipping region is the intersection of the provided area and drawing area
                    e.Graphics.SetClip(region, CombineMode.Intersect);

                    // Find total drawing offset for scrolling and inner edges
                    Point drawingOffset = NodeSpaceToClient(Point.Empty);

                    // Cache the client clipping rectangle in node space
                    _clipRectangle = ClientToNodeSpace(_drawRectangle);

                    // Adjust drawing to reflect change for us
                    e.Graphics.TranslateTransform(drawingOffset.X, drawingOffset.Y);

                    // Draw the nodes and collections
                    DrawAllNodes(e);

                    // Give the collection/node VC's a final chance to draw on surface
                    NodeVC.PostDrawNodes(this, e.Graphics, _displayNodes);
                    CollectionVC.PostDrawNodes(this, e.Graphics, _displayNodes);

                    // Put the graphics back to original state
                    e.Graphics.TranslateTransform(-drawingOffset.X, -drawingOffset.Y);

                    // Client is no longer invalidated
                    _invalidated = false;
                }
            }
		}
		
		/// <summary>
		/// Processes Windows messages.
		/// </summary>
		/// <param name="m">The Windows Message to process.</param>
		protected override void WndProc(ref Message m)
		{
            // Request for a context menu...
            if (m.Msg == (int)Msgs.WM_CONTEXTMENU)
            {
                // Extract the x,y position of the mouse
				uint x = ((uint)m.LParam & 0x0000FFFFU);
				uint y = (((uint)m.LParam & 0xFFFF0000U) >> 16);

                // We are only interested in -1,-1 because of SHIFT+F10 / VK_APPS keys
                if ((x == 65535) && (y == 65535))
                {
                    Node selectedNode = SelectedNode;

                    if (selectedNode != null)
                    {
                        // Do we need to show context menu for selected node?
                        if (OnShowContextMenuNode(selectedNode))
                        {
                            // If we have a menu to show...
                            if (ContextMenuNode != null)
                            {
                                //...then show it now!
                                ContextMenuNode.Show(this, new Point(selectedNode.Bounds.X + 2, selectedNode.Bounds.Bottom + 4));
                            }
                        }
                    }
                    else
                    {
                        // Do we need to show context menu for remainder space?
                        if (OnShowContextMenuSpace())
                        {
                            // If we have a menu to show...
                            if (ContextMenuSpace != null)
                            {
                                //...then show it now!
                                ContextMenuSpace.Show(this, new Point(2, 2));
                            }
                        }
                    }
                }

                return;
            }

			// Give theme helper chance to look for messages of interest
			_themeTreeView.WndProc(ref m);
			_themeCheckbox.WndProc(ref m);
			base.WndProc(ref m);
		}

		/// <summary>
		/// Raises the ViewControllersChanged event.
		/// </summary>
		protected virtual void OnViewControllersChanged()
		{
			if (ViewControllersChanged != null)
				ViewControllersChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the ContextMenuNodeChanged event.
		/// </summary>
		protected virtual void OnContextMenuNodeChanged()
		{
			if (ContextMenuNodeChanged != null)
				ContextMenuNodeChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the ContextMenuSpaceChanged event.
		/// </summary>
		protected virtual void OnContextMenuSpaceChanged()
		{
			if (ContextMenuSpaceChanged != null)
				ContextMenuSpaceChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the PathSeparatorChanged event.
		/// </summary>
		protected virtual void OnPathSeparatorChanged()
		{
			if (PathSeparatorChanged != null)
				PathSeparatorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the BorderIndentChanged event.
		/// </summary>
		protected virtual void OnBorderIndentChanged()
		{
			if (BorderIndentChanged != null)
				BorderIndentChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the BorderStyleChanged event.
		/// </summary>
		protected virtual void OnBorderStyleChanged()
		{
			if (BorderStyleChanged != null)
				BorderStyleChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the BorderColorChanged event.
		/// </summary>
		protected virtual void OnBorderColorChanged()
		{
			if (BorderColorChanged != null)
				BorderColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the LineWidthChanged event.
		/// </summary>
		protected virtual void OnLineWidthChanged()
		{
			if (LineWidthChanged != null)
				LineWidthChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the LineColorChanged event.
		/// </summary>
		protected virtual void OnLineColorChanged()
		{
			if (LineColorChanged != null)
				LineColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the LineDashStyleChanged event.
		/// </summary>
		protected virtual void OnLineDashStyleChanged()
		{
			if (LineDashStyleChanged != null)
				LineDashStyleChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the BoxLengthChanged event.
		/// </summary>
		protected virtual void OnBoxLengthChanged()
		{
			if (BoxLengthChanged != null)
				BoxLengthChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the BoxDrawStyleChanged event.
		/// </summary>
		protected virtual void OnBoxDrawStyleChanged()
		{
			if (BoxDrawStyleChanged != null)
				BoxDrawStyleChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the BoxShownAlwaysChanged event.
		/// </summary>
		protected virtual void OnBoxShownAlwaysChanged()
		{
			if (BoxShownAlwaysChanged != null)
				BoxShownAlwaysChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the BoxSignColorChanged event.
		/// </summary>
		protected virtual void OnBoxSignColorChanged()
		{
			if (BoxSignColorChanged != null)
				BoxSignColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the BoxBorderColorChanged event.
		/// </summary>
		protected virtual void OnBoxBorderColorChanged()
		{
			if (BoxBorderColorChanged != null)
				BoxBorderColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the BoxInsideColorChanged event.
		/// </summary>
		protected virtual void OnBoxInsideColorChanged()
		{
			if (BoxInsideColorChanged != null)
				BoxInsideColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the BoxVisibilityChanged event.
		/// </summary>
		protected virtual void OnBoxVisibilityChanged()
		{
			if (BoxVisibilityChanged != null)
				BoxVisibilityChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the LineVisibilityChanged event.
		/// </summary>
		protected virtual void OnLineVisibilityChanged()
		{
			if (LineVisibilityChanged != null)
				LineVisibilityChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the ColumnWidthChanged event.
		/// </summary>
		protected virtual void OnColumnWidthChanged()
		{
			if (ColumnWidthChanged != null)
				ColumnWidthChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the CheckStatesChanged event.
		/// </summary>
		protected virtual void OnCheckStatesChanged()
		{
			if (CheckStatesChanged != null)
				CheckStatesChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the CheckLengthChanged event.
		/// </summary>
		protected virtual void OnCheckLengthChanged()
		{
			if (CheckLengthChanged != null)
				CheckLengthChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the CheckGapLeftChanged event.
		/// </summary>
		protected virtual void OnCheckGapLeftChanged()
		{
			if (CheckGapLeftChanged != null)
				CheckGapLeftChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the CheckGapRightChanged event.
		/// </summary>
		protected virtual void OnCheckGapRightChanged()
		{
			if (CheckGapRightChanged != null)
				CheckGapRightChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the CheckDrawStyleChanged event.
		/// </summary>
		protected virtual void OnCheckDrawStyleChanged()
		{
			if (CheckDrawStyleChanged != null)
				CheckDrawStyleChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the CheckBorderColorChanged event.
		/// </summary>
		protected virtual void OnCheckBorderColorChanged()
		{
			if (CheckBorderColorChanged != null)
				CheckBorderColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the CheckInsideColorChanged event.
		/// </summary>
		protected virtual void OnCheckInsideColorChanged()
		{
			if (CheckInsideColorChanged != null)
				CheckInsideColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the CheckInsideColorChanged event.
		/// </summary>
		protected virtual void OnCheckInsideHotColorChanged()
		{
			if (CheckInsideHotColorChanged != null)
				CheckInsideHotColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the CheckTickColorChanged event.
		/// </summary>
		protected virtual void OnCheckTickColorChanged()
		{
			if (CheckTickColorChanged != null)
				CheckTickColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the CheckTickHotColorChanged event.
		/// </summary>
		protected virtual void OnCheckTickHotColorChanged()
		{
			if (CheckTickHotColorChanged != null)
				CheckTickHotColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the CheckMixedColorChanged event.
		/// </summary>
		protected virtual void OnCheckMixedColorChanged()
		{
			if (CheckMixedColorChanged != null)
				CheckMixedColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the CheckMixedHotColorChanged event.
		/// </summary>
		protected virtual void OnCheckMixedHotColorChanged()
		{
			if (CheckMixedHotColorChanged != null)
				CheckMixedHotColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the CheckBorderWidthChanged event.
		/// </summary>
		protected virtual void OnCheckBorderWidthChanged()
		{
			if (CheckBorderWidthChanged != null)
				CheckBorderWidthChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the VerticalScrollbarChanged event.
		/// </summary>
		protected virtual void OnVerticalScrollbarChanged()
		{
			if (VerticalScrollbarChanged != null)
				VerticalScrollbarChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the HorizontalScrollbarChanged event.
		/// </summary>
		protected virtual void OnHorizontalScrollbarChanged()
		{
			if (HorizontalScrollbarChanged != null)
				HorizontalScrollbarChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the VerticalGranularityChanged event.
		/// </summary>
		protected virtual void OnVerticalGranularityChanged()
		{
			if (VerticalGranularityChanged != null)
				VerticalGranularityChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the EnableMouseWheelChanged event.
		/// </summary>
		protected virtual void OnEnableMouseWheelChanged()
		{
			if (EnableMouseWheelChanged != null)
				EnableMouseWheelChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupFontChanged event.
		/// </summary>
		protected virtual void OnGroupFontChanged()
		{
			if (GroupFontChanged != null)
				GroupFontChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupArrowshanged event.
		/// </summary>
		protected virtual void OnGroupArrowsChanged()
		{
			if (GroupArrowsChanged != null)
				GroupArrowsChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupHotFontStyleChanged event.
		/// </summary>
		protected virtual void OnGroupHotFontStyleChanged()
		{
			if (GroupHotFontStyleChanged != null)
				GroupHotFontStyleChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupUseHotFontStyleChanged event.
		/// </summary>
		protected virtual void OnGroupUseHotFontStyleChanged()
		{
			if (GroupUseHotFontStyleChanged != null)
				GroupUseHotFontStyleChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupSelectedFontStyleChanged event.
		/// </summary>
		protected virtual void OnGroupSelectedFontStyleChanged()
		{
			if (GroupSelectedFontStyleChanged != null)
				GroupSelectedFontStyleChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupUseSelectedFontStyleChanged event.
		/// </summary>
		protected virtual void OnGroupUseSelectedFontStyleChanged()
		{
			if (GroupUseSelectedFontStyleChanged != null)
				GroupUseSelectedFontStyleChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupBorderStyleChanged event.
		/// </summary>
		protected virtual void OnGroupBorderStyleChanged()
		{
			if (GroupBorderStyleChanged != null)
				GroupBorderStyleChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupGradientAngleChanged event.
		/// </summary>
		protected virtual void OnGroupGradientAngleChanged()
		{
			if (GroupGradientAngleChanged != null)
				GroupGradientAngleChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupGradientColoringChanged event.
		/// </summary>
		protected virtual void OnGroupGradientColoringChanged()
		{
			if (GroupGradientColoringChanged != null)
				GroupGradientColoringChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupGradientBackChanged event.
		/// </summary>
		protected virtual void OnGroupGradientBackChanged()
		{
			if (GroupGradientBackChanged != null)
				GroupGradientBackChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupIndentLeftChanged event.
		/// </summary>
		protected virtual void OnGroupIndentLeftChanged()
		{
			if (GroupIndentLeftChanged != null)
				GroupIndentLeftChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupIndentTopChanged event.
		/// </summary>
		protected virtual void OnGroupIndentTopChanged()
		{
			if (GroupIndentTopChanged != null)
				GroupIndentTopChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupIndentBottomChanged event.
		/// </summary>
		protected virtual void OnGroupIndentBottomChanged()
		{
			if (GroupIndentBottomChanged != null)
				GroupIndentBottomChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupColoringChanged event.
		/// </summary>
		protected virtual void OnGroupColoringChanged()
		{
			if (GroupColoringChanged != null)
				GroupColoringChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupBackColorChanged event.
		/// </summary>
		protected virtual void OnGroupBackColorChanged()
		{
			if (GroupBackColorChanged != null)
				GroupBackColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupForeColorChanged event.
		/// </summary>
		protected virtual void OnGroupForeColorChanged()
		{
			if (GroupForeColorChanged != null)
				GroupForeColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupLineColorChanged event.
		/// </summary>
		protected virtual void OnGroupLineColorChanged()
		{
			if (GroupLineColorChanged != null)
				GroupLineColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupHotBackColorChanged event.
		/// </summary>
		protected virtual void OnGroupHotBackColorChanged()
		{
			if (GroupHotBackColorChanged != null)
				GroupHotBackColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupHotForeColorChanged event.
		/// </summary>
		protected virtual void OnGroupHotForeColorChanged()
		{
			if (GroupHotForeColorChanged != null)
				GroupHotForeColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupSelectedBackColorChanged event.
		/// </summary>
		protected virtual void OnGroupSelectedBackColorChanged()
		{
			if (GroupSelectedBackColorChanged != null)
				GroupSelectedBackColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupSelectedNoFocusBackColorChanged event.
		/// </summary>
		protected virtual void OnGroupSelectedNoFocusBackColorChanged()
		{
			if (GroupSelectedNoFocusBackColorChanged != null)
				GroupSelectedNoFocusBackColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupSelectedForeColorChanged event.
		/// </summary>
		protected virtual void OnGroupSelectedForeColorChanged()
		{
			if (GroupSelectedForeColorChanged != null)
				GroupSelectedForeColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupExtraLeftChanged event.
		/// </summary>
		protected virtual void OnGroupExtraLeftChanged()
		{
			if (GroupExtraLeftChanged != null)
				GroupExtraLeftChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupExtraHeightChanged event.
		/// </summary>
		protected virtual void OnGroupExtraHeightChanged()
		{
			if (GroupExtraHeightChanged != null)
				GroupExtraHeightChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupHotTrackChanged event.
		/// </summary>
		protected virtual void OnGroupHotTrackChanged()
		{
			if (GroupHotTrackChanged != null)
				GroupHotTrackChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupImageBoxChanged event.
		/// </summary>
		protected virtual void OnGroupImageBoxChanged()
		{
			if (GroupImageBoxChanged != null)
				GroupImageBoxChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupImageBoxChanged event.
		/// </summary>
		protected virtual void OnGroupImageBoxColumnChanged()
		{
			if (GroupImageBoxChanged != null)
				GroupImageBoxChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupImageBoxWidthChanged event.
		/// </summary>
		protected virtual void OnGroupImageBoxWidthChanged()
		{
			if (GroupImageBoxWidthChanged != null)
				GroupImageBoxWidthChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupImageBoxGapChanged event.
		/// </summary>
		protected virtual void OnGroupImageBoxGapChanged()
		{
			if (GroupImageBoxGapChanged != null)
				GroupImageBoxGapChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupImageBoxBorderChanged event.
		/// </summary>
		protected virtual void OnGroupImageBoxBorderChanged()
		{
			if (GroupImageBoxBorderChanged != null)
				GroupImageBoxBorderChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupImageBoxLineColorChanged event.
		/// </summary>
		protected virtual void OnGroupImageBoxLineColorChanged()
		{
			if (GroupImageBoxLineColorChanged != null)
				GroupImageBoxLineColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupImageBoxBackColorChanged event.
		/// </summary>
		protected virtual void OnGroupImageBoxBackColorChanged()
		{
			if (GroupImageBoxBackColorChanged != null)
				GroupImageBoxBackColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupImageBoxSelectedBackColorChanged event.
		/// </summary>
		protected virtual void OnGroupImageBoxSelectedBackColorChanged()
		{
			if (GroupImageBoxSelectedBackColorChanged != null)
				GroupImageBoxSelectedBackColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupImageBoxColumnColorChanged event.
		/// </summary>
		protected virtual void OnGroupImageBoxColumnColorChanged()
		{
			if (GroupImageBoxColumnColorChanged != null)
				GroupImageBoxColumnColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupImageBoxGradientBackChanged event.
		/// </summary>
		protected virtual void OnGroupImageBoxGradientBackChanged()
		{
			if (GroupImageBoxGradientBackChanged != null)
				GroupImageBoxGradientBackChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupImageBoxGradientAngleChanged event.
		/// </summary>
		protected virtual void OnGroupImageBoxGradientAngleChanged()
		{
			if (GroupImageBoxGradientAngleChanged != null)
				GroupImageBoxGradientAngleChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupImageBoxGradientColoringChanged event.
		/// </summary>
		protected virtual void OnGroupImageBoxGradientColoringChanged()
		{
			if (GroupImageBoxGradientColoringChanged != null)
				GroupImageBoxGradientColoringChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupClickExpandChanged event.
		/// </summary>
		protected virtual void OnGroupClickExpandChanged()
		{
			if (GroupClickExpandChanged != null)
				GroupClickExpandChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupAutoEditChanged event.
		/// </summary>
		protected virtual void OnGroupAutoEditChanged()
		{
			if (GroupAutoEditChanged != null)
				GroupAutoEditChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupNodesSelectableChanged event.
		/// </summary>
		protected virtual void OnGroupNodesSelectableChanged()
		{
			if (GroupNodesSelectableChanged != null)
				GroupNodesSelectableChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupDoubleClickExpandChanged event.
		/// </summary>
		protected virtual void OnGroupDoubleClickExpandChanged()
		{
			if (GroupDoubleClickExpandChanged != null)
				GroupDoubleClickExpandChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupAutoCollapseChanged event.
		/// </summary>
		protected virtual void OnGroupAutoCollapseChanged()
		{
			if (GroupAutoCollapseChanged != null)
				GroupAutoCollapseChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupAutoAllocateChanged event.
		/// </summary>
		protected virtual void OnGroupAutoAllocateChanged()
		{
			if (GroupAutoAllocateChanged != null)
				GroupAutoAllocateChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupExpandOnDragHoverChanged event.
		/// </summary>
		protected virtual void OnGroupExpandOnDragHoverChanged()
		{
			if (GroupExpandOnDragHoverChanged != null)
				GroupExpandOnDragHoverChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GroupTextRenderingHintChanged event.
		/// </summary>
		protected virtual void OnGroupTextRenderingHintChanged()
		{
			if (GroupTextRenderingHintChanged != null)
				GroupTextRenderingHintChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the ClickExpandChanged event.
		/// </summary>
		protected virtual void OnClickExpandChanged()
		{
			if (ClickExpandChanged != null)
				ClickExpandChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the DoubleClickExpandChanged event.
		/// </summary>
		protected virtual void OnDoubleClickExpandChanged()
		{
			if (DoubleClickExpandChanged != null)
				DoubleClickExpandChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the AutoCollapseChanged event.
		/// </summary>
		protected virtual void OnAutoCollapseChanged()
		{
			if (AutoCollapseChanged != null)
				AutoCollapseChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the ExtendToRightChanged event.
		/// </summary>
		protected virtual void OnExtendToRightChanged()
		{
			if (ExtendToRightChanged != null)
				ExtendToRightChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the InstantUpdateChanged event.
		/// </summary>
		protected virtual void OnInstantUpdateChanged()
		{
			if (InstantUpdateChanged != null)
				InstantUpdateChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the TooltipsChanged event.
		/// </summary>
		protected virtual void OnTooltipsChanged()
		{
			if (TooltipsChanged != null)
				TooltipsChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the InfotipsChanged event.
		/// </summary>
		protected virtual void OnInfotipsChanged()
		{
			if (InfotipsChanged != null)
				InfotipsChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the NodesSelectableChanged event.
		/// </summary>
		protected virtual void OnNodesSelectableChanged()
		{
			if (NodesSelectableChanged != null)
				NodesSelectableChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the AutoEditChanged event.
		/// </summary>
		protected virtual void OnAutoEditChanged()
		{
			if (AutoEditChanged != null)
				AutoEditChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the LabelEditChanged event.
		/// </summary>
		protected virtual void OnLabelEditChanged()
		{
			if (LabelEditChanged != null)
				LabelEditChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the CanUserExpandCollapseChanged event.
		/// </summary>
		protected virtual void OnCanUserExpandCollapseChanged()
		{
			if (CanUserExpandCollapseChanged != null)
				CanUserExpandCollapseChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the ExpandOnDragHoverChanged event.
		/// </summary>
		protected virtual void OnExpandOnDragHoverChanged()
		{
			if (ExpandOnDragHoverChanged != null)
				ExpandOnDragHoverChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the IndicatorsChanged( event.
		/// </summary>
		protected virtual void OnIndicatorsChanged()
		{
			if (IndicatorsChanged != null)
				IndicatorsChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the HotNodeChanged event.
		/// </summary>
		protected virtual void OnHotNodeChanged()
		{
			if (HotNodeChanged != null)
				HotNodeChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the VerticalNodeGapChanged event.
		/// </summary>
		protected virtual void OnVerticalNodeGapChanged()
		{
			if (VerticalNodeGapChanged != null)
				VerticalNodeGapChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the MinimumNodeHeightChanged event.
		/// </summary>
		protected virtual void OnMinimumNodeHeightChanged()
		{
			if (MinimumNodeHeightChanged != null)
				MinimumNodeHeightChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the MaximumNodeHeightChanged event.
		/// </summary>
		protected virtual void OnMaximumNodeHeightChanged()
		{
			if (MaximumNodeHeightChanged != null)
				MaximumNodeHeightChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the TextRenderingHintChanged event.
		/// </summary>
		protected virtual void OnTextRenderingHintChanged()
		{
			if (TextRenderingHintChanged != null)
				TextRenderingHintChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the HotBackColorChanged event.
		/// </summary>
		protected virtual void OnHotBackColorChanged()
		{
			if (HotBackColorChanged != null)
				HotBackColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the HotForeColorChanged event.
		/// </summary>
		protected virtual void OnHotForeColorChanged()
		{
			if (HotForeColorChanged != null)
				HotForeColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the SelectedBackColorChanged event.
		/// </summary>
		protected virtual void OnSelectedBackColorChanged()
		{
			if (SelectedBackColorChanged != null)
				SelectedBackColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the SelectedNoFocusBackColorChanged event.
		/// </summary>
		protected virtual void OnSelectedNoFocusBackColorChanged()
		{
			if (SelectedNoFocusBackColorChanged != null)
				SelectedNoFocusBackColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the SelectedForeColorChanged event.
		/// </summary>
		protected virtual void OnSelectedForeColorChanged()
		{
			if (SelectedForeColorChanged != null)
				SelectedForeColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the SelectModeChanged event.
		/// </summary>
		protected virtual void OnSelectModeChanged()
		{
			if (SelectModeChanged != null)
				SelectModeChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the HotFontStyleChanged event.
		/// </summary>
		protected virtual void OnHotFontStyleChanged()
		{
			if (HotFontStyleChanged != null)
				HotFontStyleChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the UseHotFontStyleChanged event.
		/// </summary>
		protected virtual void OnUseHotFontStyleChanged()
		{
			if (UseHotFontStyleChanged != null)
				UseHotFontStyleChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the SelectedFontStyleChanged event.
		/// </summary>
		protected virtual void OnSelectedFontStyleChanged()
		{
			if (SelectedFontStyleChanged != null)
				SelectedFontStyleChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the UseSelectedFontStyleChanged event.
		/// </summary>
		protected virtual void OnUseSelectedFontStyleChanged()
		{
			if (UseSelectedFontStyleChanged != null)
				UseSelectedFontStyleChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the ImageListChanged event.
		/// </summary>
		protected virtual void OnImageListChanged()
		{
			if (ImageListChanged != null)
				ImageListChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the ImageIndexChanged event.
		/// </summary>
		protected virtual void OnImageIndexChanged()
		{
			if (ImageIndexChanged != null)
				ImageIndexChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the SelectedImageIndexChanged event.
		/// </summary>
		protected virtual void OnSelectedImageIndexChanged()
		{
			if (SelectedImageIndexChanged != null)
				SelectedImageIndexChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the ImageGapLeftChanged event.
		/// </summary>
		protected virtual void OnImageGapLeftChanged()
		{
			if (ImageGapLeftChanged != null)
				ImageGapLeftChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the ImageGapRightChanged event.
		/// </summary>
		protected virtual void OnImageGapRightChanged()
		{
			if (ImageGapRightChanged != null)
				ImageGapRightChanged(this, EventArgs.Empty);
		}
		
		/// <summary>
		/// Raises the LabelControlCreated event.
		/// </summary>
		protected virtual void OnLabelControlCreated(TextBox textBox)
		{
			if (LabelControlCreated != null)
				LabelControlCreated(this, new LabelControlEventArgs(textBox));
		}
		
		internal Node LabelEditNode
		{
			get { return _labelEditNode; }
		}

		internal void BeginAutoEdit(Node n)
		{
			// Cancel any existing auto edit request
			_autoEditTimer.Stop();

			// Remember the node particulars
			_autoEditNode = n;

			// Start timer before auto edit begins
			_autoEditTimer.Start();
		}

		internal void CancelAutoEdit()
		{
			// Stop the timer from expiring
			_autoEditTimer.Stop();

			// No need for any node instance to be remembered
			_autoEditNode = null;
		}

		internal void BeginEditLabel(Node n, Rectangle textRect)
		{
			// Cannot edit if already editing another node
			if (_labelEditNode == null)
			{
				// Give event chance to modify starting label
				string label = n.Text;
				
				// Generate event and decide if we should continue with edit
				if (OnBeforeLabelEdit(n, ref label))
				{
					// Do not obscure the display with a tooltip
					RemoveAnyToolTip();
				
					// Remember which node is being edited
					_labelEditNode = n;
				
					// We need to create a textbox for the editing
					_labelEditBox = new TextBox();
					_labelEditBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
					
					// We need to define its font from the VC
					_labelEditBox.Font = n.VC.GetNodeFont(this, n);
					
					// Set the actual initial label text
					_labelEditBox.Text = label;
					
					// Convert from Node to client space
					textRect = NodeSpaceToClient(textRect);
					
					// Limit check the right hand side to the edge of the drawing area
					if (textRect.Right > _innerRectangle.Right)
						textRect.Width -= (textRect.Right - _innerRectangle.Right);
					
					// Define its size and position
					_labelEditBox.SetBounds(textRect.X, textRect.Y, textRect.Width, textRect.Height);
					
					// If the font causes the label to be bigger or smaller than expected
					if (_labelEditBox.Height != textRect.Height)
					{
						// Move so centered vertically
						int halfDiff = (_labelEditBox.Height - textRect.Height) / 2;
						
						// Move box to new position
						_labelEditBox.Top -= halfDiff;
					}
					
					// We need to know as soon as the control loses focus or user presses keys
					_labelEditBox.KeyDown += new KeyEventHandler(OnLabelEditBoxKeyDown);
					_labelEditBox.LostFocus += new EventHandler(OnLabelEditBoxLostFocus);
					_labelEditBox.TextChanged += new EventHandler(OnLabelEditTextChanged);
					
					// Add as a child so it becomes visible
					Controls.Add(_labelEditBox);
					
					// Raise event so user can modify the text box or hook into events
					OnLabelControlCreated(_labelEditBox);
					
					// Set the focus into the edit box
					_labelEditBox.Focus();
				}
			}
		}
		
		internal void EndEditLabel(bool quit)
		{	
			// Take the value from the edit box
			string label = _labelEditBox.Text;
			
			// Unhook from control events
			_labelEditBox.TextChanged -= new EventHandler(OnLabelEditTextChanged);
			_labelEditBox.LostFocus -= new EventHandler(OnLabelEditBoxLostFocus);
			_labelEditBox.KeyDown -= new KeyEventHandler(OnLabelEditBoxKeyDown);
			
			// Remove it from child controls
			Controls.Remove(_labelEditBox);

			Node editNode = _labelEditNode;
			_labelEditNode = null;

			// If user did not quit
			if (!quit)
			{			
				// Generate event and decide if we should use new label value
				if (OnAfterLabelEdit(editNode, ref label))
					editNode.Text = label;
			}						
		}
		
		internal void NodeAdded(Node n)
		{
			// Check the node has a valid key
			if (n.Key != null)
			{
				// Check the key is not already known
				if (!_nodeKeys.Contains(n.Key))
				{
					// It is safe to add this key
					_nodeKeys.Add(n.Key, n);
				}
			}
		}
		
		internal void NodeRemoved(Node n)
		{
			// Check the node has a valid key
			if (n.Key != null)
			{
				// Check the key is known
				if (_nodeKeys.Contains(n.Key))
				{
					// It is safe to remove this key
					_nodeKeys.Remove(n.Key);
				}
			}
		}
		
		internal void NodeKeyChanged(Node n, object oldKey, object newKey)
		{
			// Check the old key is valid
			if (oldKey != null)
			{
				// Check the key is known
				if (_nodeKeys.Contains(oldKey))
				{
					// It is safe to remove this key
					_nodeKeys.Remove(oldKey);
				}
			}

			// Check the new key is valid
			if (newKey != null)
			{
				// Check the key is not already known
				if (!_nodeKeys.Contains(newKey))
				{
					// It is safe to add this key
					_nodeKeys.Add(newKey, n);
				}
			}
		}

		internal void SetFocusNode(Node n)
		{
			// Is there a change in focused node?
			if (_focusNode != n)
			{
				// Redraw the old focused node
				if (_focusNode != null)
					InvalidateNode(_focusNode);
				
				// Change focus
				_focusNode = n;
				
				// Redraw the new focused node
				if (_focusNode != null)
				{
					InvalidateNode(n);
					EnsureDisplayed(n);
				}
			}
		}
		
		internal Node DragOverNode
		{
			get 
			{ 
				if (_dragEffects != DragDropEffects.None)
					return _dragNode;
				else
					return null;
			}
		}
		
		internal void InvalidateSelection()
		{		
			// Any current selection?
			if (_selected.Count > 0)
			{
				// Request all current entries are redrawn
				foreach(Node n in _selected.Keys)
					InvalidateNode(n);
			}
		}
		
		internal void NodeExpandedChanged(Node n)
		{
			if (!n.Expanded)
			{
				// Must process the focus node to ensure it is still visible
				EnsureFocusAfterExpand();			
			}
			
			// Must redraw the nodes to reflect change
			InvalidateNodeDrawing();
		}
				
		internal void NodeVisibleChanged(Node n, bool makeSelected)
		{
			if (!n.Visible)
			{
				// Must process the focus node to ensure it is still visible
				EnsureFocusAfterHidden(makeSelected);			
			}
			
			// Must redraw the nodes to reflect change
			InvalidateNodeDrawing();
		}

		internal void NodeContentRemoved(bool makeSelected)
		{
			// Must process the focus node to ensure it is still visible
			EnsureFocusAfterHidden(makeSelected);			
		}
		
		internal void NodeContentCleared(bool makeSelected)
		{
			// Must process the focus node to ensure it is still visible
			EnsureFocusAfterClear(makeSelected);	
			InvalidateAll();
		}

		internal Size IndicatorSize
		{
			get { return new Size(INDICATOR_WIDTH, INDICATOR_HEIGHT); }
		}
		
		internal Image GetIndicatorImage(Indicator ind)
		{
			return _indicatorImages.Images[(int)ind];
		}

		internal Rectangle DrawRectangle
		{
			get { return _drawRectangle; }
		}

		internal Rectangle InnerRectangle
		{
			get { return _innerRectangle; }
		}

		internal Node HotNode
		{
			get { return _hotNode; }
		}

		internal Node TooltipNode
		{
			get { return _tooltipNode; }
		}

		internal Point HotPoint
		{
			get { return _hotPoint; }
			set { _hotPoint = value; }
		}

		internal void InternalResetHotPoint()
		{
			HotPoint = new Point(-1, -1);
		}

		internal int GetFontHeight()
		{
			// If not cached, get current Font height
			if (_fontHeight == -1)
				_fontHeight = Font.Height;

			return _fontHeight;
		}

		internal int GetGroupFontHeight()
		{
			// If not cached, get current GroupFont height
			if (_groupFontHeight == -1)
				_groupFontHeight = GroupFont.Height;

			return _groupFontHeight;
		}

		internal Font GetFontBoldItalic()
		{
			if (_fontBoldItalic == null)
				_fontBoldItalic = new Font(Font, FontStyle.Bold | FontStyle.Italic);

			return _fontBoldItalic;
		}

		internal Font GetGroupFontBoldItalic()
		{
			if (_groupFontBoldItalic == null)
				_groupFontBoldItalic = new Font(GroupFont, FontStyle.Bold | FontStyle.Italic);

			return _groupFontBoldItalic;
		}

		internal void InternalResetFontHeight()
		{
			_fontHeight = -1;
		}

		internal void InternalResetGroupFontHeight()
		{
			_groupFontHeight = -1;
		}

		internal void InternalResetFontBoldItalic()
		{
			if (_fontBoldItalic != null)
			{
				_fontBoldItalic.Dispose();
				_fontBoldItalic = null;
			}
		}

		internal void InternalResetGroupFontBoldItalic()
		{
			if (_groupFontBoldItalic != null)
			{
				_groupFontBoldItalic.Dispose();
				_groupFontBoldItalic = null;
			}
		}

		internal Pen GetCacheLineDashPen()
		{
			// If we do not have a cached pen
			if (_cacheLineDashPen == null)
			{
				// Create new pen instance
				_cacheLineDashPen = new Pen(LineColor, LineWidth);

				// Use the appropriate drawing style
				switch(LineDashStyle)
				{
					case LineDashStyle.Dot:
						_cacheLineDashPen.DashStyle = DashStyle.Dot;
						break;
					case LineDashStyle.Dash:
						_cacheLineDashPen.DashStyle = DashStyle.Dash;
						break;
				}			
			}

			return _cacheLineDashPen;
		}

		internal Pen GetCacheBoxSignPen()
		{
			if (_cacheBoxSignPen == null)
				_cacheBoxSignPen = new Pen(BoxSignColor, 1);

			return _cacheBoxSignPen;
		}

		internal Pen GetCacheBoxBorderPen()
		{
			if (_cacheBoxBorderPen == null)
				_cacheBoxBorderPen = new Pen(BoxBorderColor, 1);

			return _cacheBoxBorderPen;
		}

		internal Brush GetCacheBoxInsideBrush()
		{
			if (_cacheBoxInsideBrush == null)
				_cacheBoxInsideBrush = new SolidBrush(BoxInsideColor);

			return _cacheBoxInsideBrush;
		}

		internal Pen GetCacheCheckTickPen()
		{
			if (_cacheCheckTickPen == null)
				_cacheCheckTickPen = new Pen(CheckTickColor, 1);

			return _cacheCheckTickPen;
		}

		internal Brush GetCacheCheckTickBrush()
		{
			if (_cacheCheckTickBrush == null)
				_cacheCheckTickBrush = new SolidBrush(CheckTickColor);

			return _cacheCheckTickBrush;
		}

		internal Pen GetCacheCheckTickHotPen()
		{
			if (_cacheCheckTickHotPen == null)
				_cacheCheckTickHotPen = new Pen(CheckTickHotColor, 1);

			return _cacheCheckTickHotPen;
		}

		internal Brush GetCacheCheckTickHotBrush()
		{
			if (_cacheCheckTickHotBrush == null)
				_cacheCheckTickHotBrush = new SolidBrush(CheckTickHotColor);

			return _cacheCheckTickHotBrush;
		}

		internal Brush GetCacheCheckBorderBrush()
		{
			if (_cacheCheckBorderBrush == null)
				_cacheCheckBorderBrush = new SolidBrush(CheckBorderColor);

			return _cacheCheckBorderBrush;
		}

		internal Brush GetCacheCheckInsideBrush()
		{
			if (_cacheCheckInsideBrush == null)
				_cacheCheckInsideBrush = new SolidBrush(CheckInsideColor);

			return _cacheCheckInsideBrush;
		}

		internal Brush GetCacheCheckInsideHotBrush()
		{
			if (_cacheCheckInsideHotBrush == null)
				_cacheCheckInsideHotBrush = new SolidBrush(CheckInsideHotColor);

			return _cacheCheckInsideHotBrush;
		}

		internal Brush GetCacheCheckMixedBrush()
		{
			if (_cacheCheckMixedBrush == null)
				_cacheCheckMixedBrush = new SolidBrush(CheckMixedColor);

			return _cacheCheckMixedBrush;
		}

		internal Brush GetCacheCheckMixedHotBrush()
		{
			if (_cacheCheckMixedHotBrush == null)
				_cacheCheckMixedHotBrush = new SolidBrush(CheckMixedHotColor);

			return _cacheCheckMixedHotBrush;
		}
		
		internal Pen GetCacheGroupLinePen()
		{
			if (_cacheGroupLinePen == null)
				_cacheGroupLinePen = new Pen(GroupLineColor);

			return _cacheGroupLinePen;
		}
		
		internal Pen GetCacheGroupImageBoxLinePen()
		{
			if (_cacheGroupImageBoxLinePen == null)
				_cacheGroupImageBoxLinePen = new Pen(GroupImageBoxLineColor);

			return _cacheGroupImageBoxLinePen;
		}

		internal Brush GetCacheGroupImageBoxColumnBrush()
		{
			if (_cacheGroupImageBoxColumnBrush == null)
				_cacheGroupImageBoxColumnBrush = new SolidBrush(GroupImageBoxColumnColor);

			return _cacheGroupImageBoxColumnBrush;
		}

		internal Brush GetCacheHotBackBrush()
		{
			if (_cacheHotBackBrush == null)
				_cacheHotBackBrush = new SolidBrush(HotBackColor);

			return _cacheHotBackBrush;
		}

		internal Brush GetCacheSelectedBackBrush()
		{
			if (_cacheSelectedBackBrush == null)
				_cacheSelectedBackBrush = new SolidBrush(SelectedBackColor);

			return _cacheSelectedBackBrush;
		}

		internal Brush GetCacheSelectedNoFocusBackBrush()
		{
			if (_cacheSelectedNoFocusBackBrush == null)
				_cacheSelectedNoFocusBackBrush = new SolidBrush(SelectedNoFocusBackColor);

			return _cacheSelectedNoFocusBackBrush;
		}

		internal void GradientColors(TreeGradientColoring scheme, 
			Color back,
			ref Color start, 
			ref Color end)
		{
			switch(scheme)
			{
				case TreeGradientColoring.LightToColor:
					start = ControlPaint.Light(back);
					end = back;
					break;
				case TreeGradientColoring.LightToDark:
					start = ControlPaint.Light(back);
					end = ControlPaint.Dark(back);
					break;
				case TreeGradientColoring.LightToVeryDark:
					start = ControlPaint.Light(back);
					end = ControlPaint.DarkDark(back);
					break;
				case TreeGradientColoring.VeryLightToColor:
					start = ControlPaint.LightLight(back);
					end = back;
					break;
				case TreeGradientColoring.VeryLightToVeryDark:
					start = ControlPaint.LightLight(back);
					end = ControlPaint.DarkDark(back);
					break;
			}
		}
		
		internal bool IsControlThemed
		{
			get 
			{
				return _themeTreeView.IsControlThemed && 
					_themeCheckbox.IsControlThemed; 
			}
		}

		internal ColorDetails ColorDetails
		{
			get { return _colorDetails; }
		}

		internal Size GlyphThemeSize
		{
			get { return _glyphThemeSize; }
		}

		internal void DrawThemedBox(Graphics g, Rectangle rect, bool expanded)
		{
			if (IsControlThemed)
			{
				// Need to adjust the rectangle as the graphics transformation
				// is ignore when the raw HDC to passed to the theming DLL.
				rect = NodeSpaceToClient(rect);

				ThemeTreeViewGlyphState glyph;

				// Determine correct state from the expanded value
				if (expanded)
					glyph = ThemeTreeViewGlyphState.Open;
				else
					glyph = ThemeTreeViewGlyphState.Closed;

				// Draw the open or closed glpyh in theme
				_themeTreeView.DrawThemeBackground(g, 
					rect,
					(int)ThemeTreeViewPart.Glyph,
					(int)glyph);
			}			
		}

		internal void DrawThemedCheckbox(Graphics g, Rectangle rect, CheckState state, CheckStates states, bool hotTrack)
		{
			if (IsControlThemed)
			{
				// Need to adjust the rectangle as the graphics transformation
				// is ignore when the raw HDC to passed to the theming DLL.
				rect = NodeSpaceToClient(rect);

				ThemeButtonCheckboxState checkState;
				ThemeButtonPart themePart;

				if (states == CheckStates.Radio)
					themePart = ThemeButtonPart.RadioButton;
				else
					themePart = ThemeButtonPart.Checkbox;

				switch(state)
				{
					case CheckState.Checked:
						if (hotTrack)
							checkState = ThemeButtonCheckboxState.CheckedHot;
						else
							checkState = ThemeButtonCheckboxState.CheckedNormal;
						break;
					case CheckState.Mixed:
						if (hotTrack)
							checkState = ThemeButtonCheckboxState.MixedHot;
						else
							checkState = ThemeButtonCheckboxState.MixedNormal;
						break;
					case CheckState.Unchecked:
					default:
						if (hotTrack)
							checkState = ThemeButtonCheckboxState.UncheckedHot;
						else
							checkState = ThemeButtonCheckboxState.UncheckedNormal;
						break;
				}

				// Draw the open or closed glpyh in theme
				_themeCheckbox.DrawThemeBackground(g, 
					rect,
					(int)themePart,
					(int)checkState);
			}			
		}

		internal bool IsFirstDisplayedNode(Node n)
		{
			// If there are no visible nodes then it cannot be the first one!
			if (_displayNodes.Count == 0)
				return false;
				
			return (_displayNodes[0] as Node) == n;
		}
		
		internal bool IsLastDisplayedNode(Node n)
		{
			// If there are no visible nodes then it cannot be the last one!
			if (_displayNodes.Count == 0)
				return false;
				
			return (_displayNodes[_displayNodes.Count - 1] as Node) == n;
		}

		internal bool IsNodeDisplayed(Node n)
		{
			// Sanity check
			if (n == null)
				return false;
		
			do
			{
				// Climb the parent chain
				n = n.Parent;
				
				// If null then reach the top
				if (n == null)
					break;				
					
				// A parent node is not visible or not expaned, either way it 
				// means the node parameter cannot be visible within the control
				if (!n.Visible || !n.Expanded)
					return false;
			
			} while(true);
			
			// Did not fail on the way, so must be visible		
			return true;
		}

		internal Point NodeSpaceToClient(Point point)
		{
			// Move to reflect inner edges and scroll bars
			point.Offset(_drawRectangle.X, _drawRectangle.Y);

			// Move to relect the scrolling offset
			if (VerticalGranularity == VerticalGranularity.Pixel)
				point.Offset(-_offset.X, -_offset.Y);
			else
			{
				// Are there any nodes to adjust for
				if (_displayNodes.Count > 0)
				{
					// Grab the node for give Y offset
					Node n = _displayNodes[_offset.Y] as Node;

					// Use position of Y Node to offset vertically
					point.Offset(-_offset.X, -n.Cache.Bounds.Top);
				}
			}

			return point;
		}

		internal Point ClientToNodeSpace(Point point)
		{
			// Move to relect the scrolling offset
			if (VerticalGranularity == VerticalGranularity.Pixel)
				point.Offset(_offset.X, _offset.Y);
			else
			{
				// Are there any nodes to adjust for
				if (_displayNodes.Count > 0)
				{
					// Grab the node for give Y offset
					Node n = _displayNodes[_offset.Y] as Node;

					// Use position of Y Node to offset vertically
					point.Offset(_offset.X, n.Cache.Bounds.Top);
				}
			}

			// Move to reflect inner edges and scroll bars
			point.Offset(-_drawRectangle.X, -_drawRectangle.Y);

			return point;
		}

		internal Rectangle NodeSpaceToClient(Rectangle bounds)
		{
			// Move to reflect inner edges and scroll bars
			bounds.Offset(_drawRectangle.X, _drawRectangle.Y);

			// Move to relect the scrolling offset
			if (VerticalGranularity == VerticalGranularity.Pixel)
				bounds.Offset(-_offset.X, -_offset.Y);
			else
			{
				// Can only adjust for vertical offset, if we have nodes
				if (_displayNodes.Count > 0)
				{
					// Grab the node for give Y offset
					Node n = _displayNodes[_offset.Y] as Node;

					// Use position of Y Node to offset vertically
					bounds.Offset(-_offset.X, -n.Cache.Bounds.Top);
				}
			}
			
			return bounds;
		}

		internal Rectangle ClientToNodeSpace(Rectangle bounds)
		{
			// Move to relect the scrolling offset
			if (VerticalGranularity == VerticalGranularity.Pixel)
				bounds.Offset(_offset.X, _offset.Y);
			else
			{
				// Can only adjust for vertical offset, if we have nodes
				if (_displayNodes.Count > 0)
				{
					// Grab the node for give Y offset
					Node n = _displayNodes[_offset.Y] as Node;

					// Use position of Y Node to offset vertically
					bounds.Offset(_offset.X, n.Cache.Bounds.Top);
				}
			}

			// Move to reflect inner edges and scroll bars
			bounds.Offset(-_drawRectangle.X, -_drawRectangle.Y);

			return bounds;
		}

		internal Node FindDisplayNodeFromPoint(Point clientPt)
		{
			// Get a candidiate node from the vertical position
			Node n = FindDisplayNodeFromY(clientPt.Y);
			
			// If we found a match
			if (n != null)
			{
				// Convert into node space
				Point nodePt = ClientToNodeSpace(clientPt);
				
				// If it fails to match horizontally
				if ((nodePt.X < n.Cache.Bounds.Left) ||
					(nodePt.X > n.Cache.Bounds.Right))
					n = null;
			}
			
			return n;
		}

		internal int FindDisplayFromY(int y)
		{
			// If we have no children, then must be positioned at start
			if (_displayNodes.Count == 0)
				return 0;

			// If Y position is before first time, then found nothing
			if (y < (_displayNodes[0] as Node).Cache.Bounds.Top)
				return -1;

			// Find first and last valid testing indexes
			int top = 0;
			int bottom = _displayNodes.Count - 1;

			do
			{
				// Find the halfway point for testing
				int test = (top + (bottom - top) / 2);

				// Get value at the testing position
				int testY = (_displayNodes[test] as Node).Cache.Bounds.Bottom;

				// Is the range just a single item?
				if (top == bottom)
				{
					if (testY < y)
						return -1;
					else
						return bottom;
				}					

				// Bottom of the child before the testing position?
				if (testY < y)
					top = test + 1; // Yes, so we search the bottom half
				else
					bottom = test;	// No, so search the top half

			} while(true);
		}

		internal int FindDisplayIndex(Node n)
		{
			return _displayNodes.IndexOf(n);
		}

		internal Node FindDisplayNodeFromY(int Y)
		{
			// Only interested in points inside the inner rectangle
			if ((Y < _innerRectangle.Top) || (Y > _innerRectangle.Bottom))
				return null;
		
			// Convert into node space
			Point nodePt = ClientToNodeSpace(new Point(0, Y));
			
			// Limit check the Y coordinate against displayed nodes
			int displayIndex = FindDisplayFromY(nodePt.Y);

			// If mouse is not over a displayed node
			if ((displayIndex < 0) || (displayIndex >= _displayNodes.Count))
				return null;
				
			// Get access to the matching node
			Node n = _displayNodes[displayIndex] as Node;

			// Is the point really inside here?
			if ((nodePt.Y >= n.Cache.Bounds.Top) && 
				(nodePt.Y <= n.Cache.Bounds.Bottom))
			{
				// Yes, so we have an exact match	
				return n;
			} 
			else
			{
				// No, so this node is just the closest one found to the point
				return null;
			}
		}

		internal void MarkAllNodeSizesDirty()
		{
			_nodeSizesDirty = true;

			// Request that drawing sizes be recalculated
			InvalidateNodeDrawing();
		}
		
		private void ResetAllProperties()
		{
			ResetNodeVC();
			ResetCollectionVC();
			ResetViewControllers();
			ResetBackColor();
			ResetBorderStyle();
			ResetBorderColor();
			ResetBorderIndent();
			ResetLineColor();
			ResetLineWidth();
			ResetLineDashStyle();
			ResetBoxLength();
			ResetBoxDrawStyle();
			ResetBoxSignColor();
			ResetBoxBorderColor();
			ResetBoxInsideColor();
			ResetBoxVisibility();
			ResetBoxShownAlways();
			ResetLineVisibility();
			ResetColumnWidth();
			ResetCheckStates();
			ResetCheckLength();
			ResetCheckGapLeft();
			ResetCheckGapRight();
			ResetCheckBorderWidth();
			ResetCheckDrawStyle();
			ResetCheckBorderColor();
			ResetCheckInsideColor();
			ResetCheckInsideHotColor();
			ResetCheckTickColor();
			ResetCheckTickHotColor();
			ResetCheckMixedColor();
			ResetCheckMixedHotColor();
			ResetVerticalScrollbar();
			ResetHorizontalScrollbar();
			ResetVerticalGranularity();
			ResetEnableMouseWheel();
			ResetImageGapLeft();
			ResetImageGapRight();
			ResetGroupFont();
			ResetGroupArrows();
			ResetGroupIndentLeft();
			ResetGroupIndentTop();
			ResetGroupIndentBottom();
			ResetGroupBackColor();
			ResetGroupForeColor();
			ResetGroupLineColor();
			ResetGroupColoring();
			ResetGroupHotBackColor();
			ResetGroupHotForeColor();
			ResetGroupHotFontStyle();
			ResetGroupUseHotFontStyle();
			ResetGroupSelectedBackColor();
			ResetGroupSelectedNoFocusBackColor();
			ResetGroupSelectedForeColor();
			ResetGroupSelectedFontStyle();
			ResetGroupUseSelectedFontStyle();
			ResetGroupGradientColoring();
			ResetGroupGradientAngle();
			ResetGroupGradientBack();
			ResetGroupExtraLeft();
			ResetGroupExtraHeight();
			ResetGroupHotTrack();
			ResetGroupBorderStyle();
			ResetGroupClickExpand();
			ResetGroupDoubleClickExpand();
			ResetGroupAutoCollapse();
			ResetGroupAutoAllocate();
			ResetGroupImageBox();
			ResetGroupImageBoxColumn();
			ResetGroupImageBoxWidth();
			ResetGroupImageBoxGap();
			ResetGroupImageBoxBorder();
			ResetGroupImageBoxLineColor();
			ResetGroupImageBoxBackColor();
			ResetGroupImageBoxSelectedBackColor();
			ResetGroupImageBoxColumnColor();
			ResetGroupImageBoxGradientBack();
			ResetGroupImageBoxGradientAngle();
			ResetGroupImageBoxGradientColoring();
			ResetGroupTextRenderingHint();
			ResetGroupNodesSelectable();
			ResetGroupExpandOnDragHover();
			ResetIndicators();
			ResetVerticalNodeGap();
			ResetMinimumNodeHeight();
			ResetMaximumNodeHeight();
			ResetTextRenderingHint();
			ResetHotBackColor();
			ResetHotForeColor();
			ResetHotFontStyle();
			ResetUseHotFontStyle();
			ResetSelectMode();
			ResetSelectedBackColor();
			ResetSelectedNoFocusBackColor();
			ResetSelectedForeColor();
			ResetSelectedFontStyle();
			ResetUseSelectedFontStyle();
			ResetClickExpand();
			ResetDoubleClickExpand();
			ResetAutoCollapse();
			ResetExtendToRight();
			ResetNodesSelectable();
			ResetCanUserExpandCollapse();
			ResetExpandOnDragHover();
			ResetLabelEdit();
            ResetFont();
		}

		private void EnsureFocusAfterExpand()
		{
			// Is there a focused node?
			if (_focusNode != null)
			{
				// Find the location of the current focus node
				int[] location = GetNodeLocation(_focusNode);

				Node lastNode = null;
				NodeCollection lastCollection = Nodes;
				
				foreach(int index in location)
				{
					// Break out if we has invalid index
					if (index == -1)
						break;
						
					// Find the index into the collection
					Node n = lastCollection[index];
					
					// Only interest if the node is visible
					if (n.Visible)
					{
						// Use this node as last valid one found
						lastNode = n;
						
						// Can only process downwards if the node is expanded
						if (!lastNode.Expanded)
							break;
						
						// Get access to its collection of children
						lastCollection = lastNode.Nodes;
					}
					else
					{
						// Otherwise we are finished searching
						break;
					}
				}
				
				if (lastNode != null)
				{
					if (lastNode != _focusNode)
						SetFocusNode(lastNode);
				}
				else
				{
					// Find the first displayed node
					Node first = Nodes.GetFirstDisplayedNode();
				
					// If there is one, then use that
					if (first != null)
						SetFocusNode(first);
				}
			}
		}

		private void EnsureFocusAfterHidden(bool makeSelected)
		{
			// Is there a focused node?
			if (_focusNode != null)
			{
				// If there are no Nodes left
				if (Nodes.Count == 0)
				{
					// Then nothing can be focused
					_focusNode = null;
				}
				else
				{
					// Find the location of the current focus node
					int[] location = GetNodeLocation(_focusNode);

					Node lastNode = null;
					NodeCollection lastCollection = Nodes;
					
					foreach(int index in location)
					{
                        if ((index == -1) || (lastCollection.Count <= index))
                        {
                            _focusNode = null;
                            lastNode = null;
                            break;
                        }

						// Find the index into the collection
						Node n = lastCollection[index];
						
						// Is the node visible?
						if (n.Visible && !n.Removing)
						{
							// Use this node as last visible one found
							lastNode = n;
							
							// Can only process downwards if the node is expanded
							if (!lastNode.Expanded)
								break;
							
							// Get access to its collection of children
							lastCollection = lastNode.Nodes;
						}
						else
						{
							bool after = false;
							
							// Search for the next visible node in this collection
							for(int i=index+1; i<lastCollection.Count; i++)
							{
								if (lastCollection[i].Visible && !lastCollection[i].Removing)
								{
									// Found one visible, use this as new focus
									lastNode = lastCollection[i];
									after = true;
									break;
								}
							}
							
							// Not found before?
							if (!after)
							{
								// Search for the previous visible node in this collection
								for(int i=index-1; i>=0; --i)
								{
									if (lastCollection[i].Visible && !lastCollection[i].Removing)
									{
										// Found one visible, use this as new focus
										lastNode = lastCollection[i];
										break;
									}
								}
							}

							// No other visible nodes in this collection, so just use this node
							break;
						}
					}
					
					// Do we have a calculated Node to become focused?
					if (lastNode != null)
					{
						if (lastNode != _focusNode)
						{
							// Should Node be selected as well as focused?
							if (makeSelected)
								SelectNode(lastNode);
								
							SetFocusNode(lastNode);
						}
					}
					else
					{
						// Find the first displayed node
						Node first = Nodes.GetFirstDisplayedNode();
					
						// If there is one, then use that
						if (first != null)
						{
							// If this node is being removed
							if (first.Removing)
								first = first.NextDisplayedNode;
								
							// If still have a node to focus
							if (first != null)
							{						
								// Should Node be selected as well as focused?
								if (makeSelected)
									SelectNode(first);

								SetFocusNode(first);
							}
						}
					}
				}
			}
		}

		private void EnsureFocusAfterClear(bool makeSelected)
		{
			// Is there a focused node?
			if (_focusNode != null)
			{
				// If there are no Nodes left
				if (Nodes.Count == 0)
				{
					// Then nothing can be focused
					_focusNode = null;
				}
				else
				{
					// Find the location of the current focus node
					int[] location = GetNodeLocation(_focusNode);

					Node lastNode = null;
					NodeCollection lastCollection = Nodes;
					
					foreach(int index in location)
					{
						if (index == -1)
						{
							_focusNode = null;
							lastNode = null;
							break;
						}
						
						// Find the index into the collection
						Node n = lastCollection[index];
						
						// Is the node visible?
						if (n.Visible && !n.Removing)
						{
							// Use this node as last visible one found
							lastNode = n;
							
							// Can only process downwards if the node is expanded
							if (!lastNode.Expanded)
								break;
							
							// Get access to its collection of children
							lastCollection = lastNode.Nodes;
						}
						else
						{
							bool after = false;
						
							// Search for the next visible node in this collection
							for(int i=index; i<lastCollection.Count; i++)
							{
								if (lastCollection[i].Visible && !lastCollection[i].Removing)
								{
									// Found one visible, use this as new focus
									lastNode = lastCollection[i];
									after = true;
									break;
								}
							}
							
							// Not found afterwards?
							if (!after)
							{
								// Search for the previous visible node in this collection
								for(int i=index-1; i>=0; --i)
								{
									if (lastCollection[i].Visible && !lastCollection[i].Removing)
									{
										// Found one visible, use this as new focus
										lastNode = lastCollection[i];
										break;
									}
								}
							}

							// No other visible nodes in this collection, so just use this node
							break;
						}
					}
					
					// Do we have a calculated Node to become focused?
					if (lastNode != null)
					{
						if (lastNode != _focusNode)
						{
							// Should Node be selected as well as focused?
							if (makeSelected)
								SelectNode(lastNode);

							SetFocusNode(lastNode);
						}
					}
					else
					{
						// Find the first displayed node
						Node first = Nodes.GetFirstDisplayedNode();
					
						// If there is one, then use that
						if (first != null)
						{
							// If this node is being removed
							if (first.Removing)
								first = first.NextDisplayedNode;
								
							// If still have a node to focus
							if (first != null)
							{						
								// Should Node be selected as well as focused?
								if (makeSelected)
									SelectNode(first);

								SetFocusNode(first);
							}
						}
					}
				}
			}
		}

		private bool EnsureParentChain(Node n, bool expand, bool visible)
		{
			// Is there any need to walk the parent chain?
			if (expand || visible)
			{
				// Start walking parent chain from provided node
				Node test = n;
			
				bool displayChange = false;
				
				// Keep moving up the chain until we reach the top
				while(true)
				{
					// If the node is not visible...
					if (!test.Visible)
					{
						// ...and we are not allowed to make it so, then failed
						if (!visible)
							return false;
							
						// Make it visible now
						test.Visible = true;
						
						// Must track when change to appearance has happened
						displayChange = true;
					}
					
					// If the node is not expanded...
					if (!test.Expanded)
					{
						// And it is not the starting node...
						if (test != n)
						{
							// ...and we are not allowed to make it so, then failed
							if (!expand)
								return false;
							
							// Expand the node now
							test.Expand();

							// Must track when change to appearance has happened
							displayChange = true;
						}
					}
					
					// If we have reached the top, then break out
					if (test.Parent == null)
						break;

					// Walk up the chain
					test = test.Parent;
				}

				// Have we caused a change in the number of items displayed?
				if (displayChange)
				{
					// Set flag so that calculation cycle recalculates everything
					_nodeDrawingValid = false;

					// Perform a full recalculation of nodes and scroll bars
					using(Graphics g = CreateGraphics())
						CalculationCycle(g);

					// We need to repaint to show changed state
					InvalidateAll();
				}
			}

			// Yes, we processed without problems
			return true;
		}

		private int FindFirstFullDisplayedNode(Rectangle bounds)
		{
			// Are we using node level scrolling...
			if (VerticalGranularity == VerticalGranularity.Node)
			{
				//...yes, so the index is just the vertical scroll position
				return _offset.Y;
			}
			else
			{
				//...no, so lets find the node that contains the top pixel line
				int index = FindDisplayFromY(_offset.Y);

				// If we found a node then
				if (index >= 0)
				{
					// Grab the actual node instance
					Node top = _displayNodes[index] as Node;

					// Is the vertical offset after the first pixel of the node
					if (top.Cache.Bounds.Top < _offset.Y)
					{
						// No, then the top node is only partially showing. So return the
						// next node in list if there is one.
						if (index < (_displayNodes.Count - 1))
							return index+1;
					}

					return index;
				}
			}

			return -1;
		}

		private int FindLastFullDisplayedNode(Rectangle bounds)
		{
			// Are we using node level scrolling...
			if (VerticalGranularity == VerticalGranularity.Node)
			{
				// Get the top most node
				Node top = _displayNodes[_offset.Y] as Node;

				// Calculate the last displayed pixel
				int lastPixel = bounds.Height + top.Cache.Bounds.Top;

				// Find the node index for the bottom pixel of display
				int index = FindDisplayFromY(lastPixel);

				// If we found a node then
				if (index >= 0)
				{
					// Grab the actual node instance
					Node bottom = _displayNodes[index] as Node;

					// Is the last pixel of the node after end of the display
					if (bottom.Cache.Bounds.Bottom >= lastPixel)
					{
						// Yes, then the bottom node is only partially showing. So 
						// return the previous node in list if there is one.
						if (index > 0)
							return index-1;
					}

					return index;
				}
				else
				{
					// Past end of the displayed nodes, so return the last one
					return _displayNodes.Count - 1;
				}
			}
			else
			{
				// Calculate the last displayed pixel
				int lastPixel = bounds.Height + _offset.Y;

				// Find the node index for the bottom pixel of display
				int index = FindDisplayFromY(lastPixel);

				// If we found a node then
				if (index >= 0)
				{
					// Grab the actual node instance
					Node bottom = _displayNodes[index] as Node;

					// Is the last pixel of the node after end of the display
					if (bottom.Cache.Bounds.Bottom >= lastPixel)
					{
						// Yes, then the bottom node is only partially showing. So 
						// return the previous node in list if there is one.
						if (index > 0)
							return index-1;
					}

					return index;
				}
				else
				{
					// Past end of the displayed nodes, so return the last one
					return _displayNodes.Count - 1;
				}
			}
		}

		private int InsidePageUpIndex(int index, Rectangle bounds)
		{
			// Track total space available
			int space = bounds.Height;

			// Grab the item that needs to be last on the page
			Node displayNode = _displayNodes[index] as Node;

			// Subtract the space for the last item immediately
			space -= displayNode.Cache.Bounds.Height;

			// Keep going till we run out of space
			while((space > 0) && (index > 0))
			{
				// Grab the previous item in the list, to see if it will fit in remaining space
				displayNode = _displayNodes[index-1] as Node;

				// Subtract the space from the remaining visible space
				space -= (displayNode.Cache.Bounds.Height + VerticalNodeGap);

				// Does the whole of the item fit in?
				if (space > 0)
				{
					// Safe to move upto the next item
					index--;
				}
			}

			return index;
		}

		private Node OutsidePageUpNode(Node node, Rectangle bounds)
		{
			// Find index of the node in the display collection
			int index = _displayNodes.IndexOf(node);

			// The node must be in the display for success
			if (index == -1)
				return null;

			// Track total space available
			int space = bounds.Height;

			// Keep going till we run out of space
			while((space > 0) && (index > 0))
			{
				// Grab the previous item in the list, to see if it will fit in remaining space
				Node displayNode = _displayNodes[index-1] as Node;

				// Subtract the space from the remaining visible space
				space -= (displayNode.Cache.Bounds.Height + VerticalNodeGap);

				// Does the whole of the item fit in?
				if (space > 0)
				{
					// Safe to move upto the next item
					index--;
				}
			}

			// Return the node and not the index
			return _displayNodes[index] as Node;
		}

		private Node OutsidePageDownNode(Node node, Rectangle bounds)
		{
			// Find index of the node in the display collection
			int index = _displayNodes.IndexOf(node);

			// The node must be in the display for success
			if (index == -1)
				return null;

			// Track total space available
			int space = bounds.Height;

			// Keep going till we run out of space
			while((space > 0) && (index < (_displayNodes.Count - 1)))
			{
				// Grab the next item in the list, to see if it will fit in remaining space
				Node displayNode = _displayNodes[index+1] as Node;

				// Subtract the space from the remaining visible space
				space -= (displayNode.Cache.Bounds.Height + VerticalNodeGap);

				// Does the whole of the item fit in?
				if (space > 0)
				{
					// Safe to move upto the next item
					index++;
				}
			}

			// Return the node and not the index
			return _displayNodes[index] as Node;
		}

		private bool ProcessUpKey(bool shiftKey, bool controlKey)
		{
			// Need to find the next upper node
			Node nextUpNode = null;
				
			// Do we have a focused node?
			if (_focusNode != null)
			{
				// Then just get the previous displayed one
				nextUpNode = _focusNode.PreviousDisplayedNode;
			}
			else
			{
				// Get the first displayed node
				nextUpNode = Nodes.GetFirstDisplayedNode();
			}
				
			// If we managed to find a node to move to
			if (nextUpNode != null)
			{
				// Check for the special case of using the Ctrl key
				if (controlKey)
				{
					// Using Ctrl + Up moves the focus but does not change selection
					SetFocusNode(nextUpNode);
				}
				else
					SelectNode(nextUpNode, shiftKey, controlKey);
			}
					
			// Key processed
			return true;
		}

		private bool ProcessDownKey(bool shiftKey, bool controlKey)
		{
			// Need to find the next down node
			Node nextDownNode = null;
				
			// Do we have a focused node?
			if (_focusNode != null)
			{
				// Then just get the next displayed one
				nextDownNode = _focusNode.NextDisplayedNode;
			}
			else
			{
				// Get the first displayed node
				nextDownNode = Nodes.GetFirstDisplayedNode();
			}
				
			// If we managed to find a node to move to
			if (nextDownNode != null)
			{
				// Check for the special case of using the Ctrl key
				if (controlKey)
				{
					// Using Ctrl + Down moves the focus but does not change selection
					SetFocusNode(nextDownNode);
				}
				else
					SelectNode(nextDownNode, shiftKey, controlKey);
			}

			// Key processed
			return true;
		}

		private bool ProcessLeftKey(bool shiftKey, bool controlKey)
		{
			// Only interested if the modifiers are not pressed
			if (!shiftKey && !controlKey)
			{
				// Are we positioned on a node?
				if (_focusNode != null)
				{
					// Is it expanded?
					if (_focusNode.Expanded)
					{
						// Close it up if allowed
						if (_focusNode.VC.CanCollapseNode(this, _focusNode, true, false))
							_focusNode.Collapse();
					}
					else
					{
						Node parent = _focusNode.Parent;
						
						// Does it have a parent node?
                        if (parent != null)
                        {
                            // Are we allowed to select nodes?
                            if (SelectMode == SelectMode.None)
                            {
                                // No, so just set the focus instead
                                SetFocusNode(parent);
                            }
                            else
                            {
                                // Yes, so select it now
                                SingleSelect(parent);
                            }
                        }
                        else
                            ProcessUpKey(shiftKey, controlKey);
					}
				}
			}

			// Key processed
			return true;
		}
			
		private bool ProcessRightKey(bool shiftKey, bool controlKey)
		{
			// Only interested if the modifiers are not pressed
			if (!shiftKey && !controlKey)
			{
				// Are we positioned on a node?
				if (_focusNode != null)
				{
					// Is it closed up?
					if (!_focusNode.Expanded)
					{
						// Open it up if allowed
						if (_focusNode.VC.CanExpandNode(this, _focusNode, true, false))
							_focusNode.Expand();
					}
					else
					{
						Node child = _focusNode.Nodes.GetFirstDisplayedNode();
						
						// Does it have any children?
						if (child != null)
						{
							// Are we allowed to select nodes?
							if (SelectMode == SelectMode.None)
							{
								// No, so just set the focus instead
								SetFocusNode(child);
							}
							else
							{
								// Yes, so select it now
								SingleSelect(child);
							}
						}
					}
				}
			}
		
			// Key processed
			return true;
		}

		private bool ProcessHomeKey(bool shiftKey, bool controlKey)
		{
			// Get the first displayed node
			Node homeNode = Nodes.GetFirstDisplayedNode();
				
			// If we managed to find a node to move to
			if (homeNode != null)
			{
				// Check for the special case of using the Ctrl key
				if (controlKey || (SelectMode == SelectMode.None))
				{
					// Using Ctrl + Home moves the focus but does not change selection
					SetFocusNode(homeNode);
				}
				else
					SelectNode(homeNode, shiftKey, controlKey);
			}
					
			// Key processed
			return true;
		}

		private bool ProcessEndKey(bool shiftKey, bool controlKey)
		{
			// Get the first displayed node
			Node endNode = Nodes.GetLastDisplayedNode();
				
			// If we managed to find a node to move to
			if (endNode != null)
			{
				// Check for the special case of using the Ctrl key
				if (controlKey || (SelectMode == SelectMode.None))
				{
					// Using Ctrl + End moves the focus but does not change selection
					SetFocusNode(endNode);
				}
				else
					SelectNode(endNode, shiftKey, controlKey);
			}
					
			// Key processed
			return true;
		}

		private bool ProcessPageUpKey(bool shiftKey, bool controlKey)
		{	
			// Need to find the next up node
			Node pageUpNode = null;
				
			// Do we have a focused node?
			if (_focusNode != null)
			{
				// Find the index of the first one being displayed
				int index = FindFirstFullDisplayedNode(_drawRectangle);
		
				// Should never be an invalid node
				if (index >= 0)
				{
					// If the focus node not the first one?
					if (_displayNodes.IndexOf(_focusNode) > index)		
					{
						// Then we just move upwards to top one
						pageUpNode = _displayNodes[index] as Node;
					}
					else
					{
						// Then just get next page up node
						pageUpNode = OutsidePageUpNode(_focusNode, _drawRectangle);
					}
				}
			}
			else
			{
				// Get the first displayed node
				pageUpNode = Nodes.GetFirstDisplayedNode();
			}

			// If we managed to find a node to move to
			if (pageUpNode != null)
			{
				// Check for the special case of using the Ctrl key
				if (controlKey || (SelectMode == SelectMode.None))
				{
					// Using Page + Up moves the focus but does not change selection
					SetFocusNode(pageUpNode);
				}
				else
					SelectNode(pageUpNode, shiftKey, controlKey);
			}

			// Key processed
			return true;
		}

		private bool ProcessPageDownKey(bool shiftKey, bool controlKey)
		{
			// Need to find the next down node
			Node pageDownNode = null;
				
			// Do we have a focused node?
			if (_focusNode != null)
			{
				// Find the index of the first one being displayed
				int index = FindLastFullDisplayedNode(_drawRectangle);
		
				// Should never be an invalid node
				if (index >= 0)
				{
					// If the focus node not the first one?
					if (_displayNodes.IndexOf(_focusNode) < index)		
					{
						// Then we just move upwards to top one
						pageDownNode = _displayNodes[index] as Node;
					}
					else
					{
						// Then get the page down node
						pageDownNode = OutsidePageDownNode(_focusNode, _drawRectangle);
					}
				}
			}
			else
			{
				// Get the last displayed node
				pageDownNode = Nodes.GetLastDisplayedNode();
			}

			// If we managed to find a node to move to
			if (pageDownNode != null)
			{
				// Check for the special case of using the Ctrl key
				if (controlKey || (SelectMode == SelectMode.None))
				{
					// Using Page + Down moves the focus but does not change selection
					SetFocusNode(pageDownNode);
				}
				else
					SelectNode(pageDownNode, shiftKey, controlKey);
			}

			// Key processed
			return true;
		}

		private bool ProcessPlusKey()
		{
			// Are we positioned on a node?
			if (_focusNode != null)
			{
				// Is it closed up?
				if (!_focusNode.Expanded)
				{
					// Open it up if allowed
					if (_focusNode.VC.CanExpandNode(this, _focusNode, true, false))
						_focusNode.Expand();
				}
			}

			// Key processed
			return true;
		}

		private bool ProcessMinusKey()
		{
			// Are we positioned on a node?
			if (_focusNode != null)
			{
				// Is it opened up?
				if (_focusNode.Expanded)
				{
					// Close it up if allowed
					if (_focusNode.VC.CanCollapseNode(this, _focusNode, true, false))
						_focusNode.Collapse();
				}
			}

			// Key processed
			return true;
		}

		private void CtrlSelect(Node n)
		{
			bool afterSelectEvent = false;
			bool afterDeselectEvent = false;

			if (n.VC.CanSelectNode(this, n))
			{
				// Toggle selected state of this node
				if (!_selected.ContainsKey(n))
				{
					// Allowed to select even after generating event?
					if (OnBeforeSelect(n))
					{
						// Add to selection
						_selected.Add(n, n);
						
						// Need to generate the 'after selection' event
						afterSelectEvent = true;
					}
				}
				else
				{
					// Remove from selection
					_selected.Remove(n);

					// Need to generate the 'after deselection' event
					afterDeselectEvent = true;
				}
							
				// This was the last node to be selected
				_lastSelectedNode = n;
							
				// Cause selection change to be drawn
				InvalidateNode(n);
			}
		
			// Can always set focus to the node, even if cannot select it
			SetFocusNode(n);			

            if (afterDeselectEvent)
                OnBeforeDeselect(n);

			if (afterSelectEvent)
				OnAfterSelect(n);

            if (afterDeselectEvent)
                OnAfterDeselect(n);

            if (afterSelectEvent || afterDeselectEvent)
                OnAfterSelectionChanged();
		}
		
		private void ShiftSelect(Node n)
		{
			// Do we have a last selection?
			if (_lastSelectedNode != null)
			{
				// And this node is still being displayed?
				if (IsNodeDisplayed(_lastSelectedNode))
				{
					// Are both ends of the shift select, actually the same?
					if (_lastSelectedNode == n)
					{
						// Then just perform a single select
						SingleSelect(n);
						SetFocusNode(n);			
					}
					else
					{	
						// Remember the current selection
						Hashtable original = (Hashtable)_selected.Clone();
					
						// Remove all the current selected nodes
						ClearSelection();
					
						// Find location of last and newly selected nodes
						int[] lastLocation = GetNodeLocation(_lastSelectedNode);
						int[] newLocation = GetNodeLocation(n);

						// Assume that node is before the last selection in order
						Node first  = n;
						Node second = _lastSelectedNode;
						
						// Are we moving downwards?
						if (LocationIsBefore(lastLocation, newLocation))
						{
							// No, so reverse the nodes and use same algorithm
							first = _lastSelectedNode;
							second = n;
						}

						bool foundLast = false;
						ArrayList selectedNodes = new ArrayList();

						do
						{
							// If the node is not already selected
							bool wasSelected = original.ContainsKey(first);

							// Are we allowed to actually select the node?
							if (first.VC.CanSelectNode(this, first))
							{
								// Allowed to select even after generating event?
								if (wasSelected || OnBeforeSelect(first))
								{
									// Add this node to selection
									_selected.Add(first, first);
									
									// Need to repaint its state
									InvalidateNode(first);
									
									if (!wasSelected)
									{
										// Keep track of all nodes added
										selectedNodes.Add(first);
									}
								}
							}
							
							// Only break once the last node has been selected
							if (foundLast)
								break;
							
							// Move down to next node in order
							first = first.NextDisplayedNode;
							
							// Do we mark the loop to exit?
							if (first == second)
								foundLast = true;
						
						} while(first != null);
												
						// Can always set focus to the node, even if cannot select it
						SetFocusNode(n);

                        bool change = false;

                        // Generate before deselect event for each node removed
                        foreach (Node before in original.Keys)
                            if (!_selected.Contains(before))
                                OnBeforeDeselect(before);

                        // Generate after deselect event for each node removed
                        foreach (Node before in original.Keys)
                            if (!_selected.Contains(before))
                            {
                                OnAfterDeselect(before);
                                change = true;
                            }
						
						// Generate event for each node added
                        foreach (Node after in selectedNodes)
                        {
                            OnAfterSelect(after);
                            change = true;
                        }

                        if (change)
                            OnAfterSelectionChanged();
					}
				}
				else
				{
					// Not displayed, so reset value
					_lastSelectedNode = null;
				}
			}
			
			// If we could not perform a shift select, then revert to ctrl select
			if (_lastSelectedNode == null)
				CtrlSelect(n);
		}

		private void SingleSelect(Node n)
		{
			bool afterSelect = false;
            bool afterDeselect = false;
            bool wasSelected = _selected.ContainsKey(n);

			// Remember the current selection
			Hashtable original = (Hashtable)_selected.Clone();

			// Remove all current contents
			ClearSelection();

            if (n.VC != null)
            {
                if (n.VC.CanSelectNode(this, n))
                {
                    // Allowed to select even after generating event?
                    if (wasSelected || OnBeforeSelect(n))
                    {
                        // Add this single entry
                        _selected.Add(n, n);

                        // This was the last not to be selected
                        _lastSelectedNode = n;

                        // Cause new selection to be redrawn
                        InvalidateNode(n);

                        if (!wasSelected)
                        {
                            // Need to generate the selection events
                            afterSelect = true;
                        }
                        else
                        {
                            // If the target node does not have the focus,
                            // then move the focus to it immediately
                            if (_focusNode != n)
                                afterSelect = true;
                        }
                    }
                }
            }
			
			// Change of focus only needed when the node becomes selected
			if (afterSelect)
				SetFocusNode(n);			

			// Remove new selection entries from old selection
			foreach(Node select in _selected.Keys)
				original.Remove(select);

			// Any old nodes that need deselect event?
			if (original.Count > 0)
			{
                // Generate the before deselect event for each entry
                foreach (Node old in original.Keys)
                    OnBeforeDeselect(old);
                
                // Generate the after deselect event for each entry
				foreach(Node old in original.Keys)
					OnAfterDeselect(old);

                afterDeselect = true;
			}

			if (afterSelect)
				OnAfterSelect(n);

            if (afterSelect || afterDeselect)
                OnAfterSelectionChanged();
        }
				
		private int[] GetNodeLocation(Node n)
		{
			// Construct list of collection indexes
			ArrayList indexes = new ArrayList();
			
			// Keep constructing location until we reach the root
			while(n != null)
			{
				// Add location to start of list
				indexes.Insert(0, n.Index);
				
				// Move up a level
				n = n.Parent;
			}
			
			// Create fixed size array
			int[] location = new int[indexes.Count];
			
			// Copy across the actual indexes
			for(int i=0; i<indexes.Count; i++)
				location[i] = (int)indexes[i];
				
			return location;
		}

		private bool LocationIsBefore(int[] first, int[] second)
		{
			// Find number of identical entries to test against
			int length = first.Length < second.Length ? first.Length : second.Length;
			
			// Test each of the common entries
			for(int i=0; i<length; i++)
			{
				// If the first location is a smaller index than the second
				// location then the first location is before in the ordering
				if (first[i] < second[i])
					return true;
				else
				{
					// If the first location is a greater index than the second
					// location then the first location is after in the ordering
					if (first[i] > second[i])
						return false;
				}
			}
			
			// All common entries are identical, so the shortest 
			// location is the first in terms of ordering
			if (first.Length < second.Length)
				return true;
			else
				return false;
		}
		
		private void InvalidateFocus()
		{
			// Redraw any focused node
			if (_focusNode != null)
				InvalidateNode(_focusNode);
		}
		
		private void ClearLineBoxCache()
		{
			if (_cacheLineDashPen != null)
			{
				_cacheLineDashPen.Dispose();
				_cacheLineDashPen = null;
			}

			if (_cacheBoxSignPen != null)
			{
				_cacheBoxSignPen.Dispose();
				_cacheBoxSignPen = null;
			}

			if (_cacheBoxBorderPen != null)
			{
				_cacheBoxBorderPen.Dispose();
				_cacheBoxBorderPen = null;
			}

			if (_cacheBoxInsideBrush != null)
			{
				_cacheBoxInsideBrush.Dispose();
				_cacheBoxInsideBrush = null;
			}
		}

		private void ClearCheckCache()
		{
			if (_cacheCheckTickPen != null)
			{
				_cacheCheckTickPen.Dispose();
				_cacheCheckTickPen = null;
			}

			if (_cacheCheckTickBrush != null)
			{
				_cacheCheckTickBrush.Dispose();
				_cacheCheckTickBrush = null;
			}

			if (_cacheCheckTickHotPen != null)
			{
				_cacheCheckTickHotPen.Dispose();
				_cacheCheckTickHotPen = null;
			}

			if (_cacheCheckTickHotBrush != null)
			{
				_cacheCheckTickHotBrush.Dispose();
				_cacheCheckTickHotBrush = null;
			}

			if (_cacheCheckBorderBrush != null)
			{
				_cacheCheckBorderBrush.Dispose();
				_cacheCheckBorderBrush = null;
			}

			if (_cacheCheckMixedBrush != null)
			{
				_cacheCheckMixedBrush.Dispose();
				_cacheCheckMixedBrush = null;
			}

			if (_cacheCheckMixedHotBrush != null)
			{
				_cacheCheckMixedHotBrush.Dispose();
				_cacheCheckMixedHotBrush = null;
			}

			if (_cacheCheckInsideBrush != null)
			{
				_cacheCheckInsideBrush.Dispose();
				_cacheCheckInsideBrush = null;
			}

			if (_cacheCheckInsideHotBrush != null)
			{
				_cacheCheckInsideHotBrush.Dispose();
				_cacheCheckInsideHotBrush = null;
			}
		}
		
		private void ClearGroupCache()
		{
			if (_cacheGroupLinePen != null)
			{
				_cacheGroupLinePen.Dispose();
				_cacheGroupLinePen = null;
			}

			if (_cacheGroupImageBoxLinePen != null)
			{
				_cacheGroupImageBoxLinePen.Dispose();
				_cacheGroupImageBoxLinePen = null;
			}

			if (_cacheGroupImageBoxColumnBrush != null)
			{
				_cacheGroupImageBoxColumnBrush.Dispose();
				_cacheGroupImageBoxColumnBrush = null;
			}
		}

		private void ClearNodeCache()
		{
			if (_cacheHotBackBrush != null)
			{
				_cacheHotBackBrush.Dispose();
				_cacheHotBackBrush = null;
			}

			if (_cacheSelectedBackBrush != null)
			{
				_cacheSelectedBackBrush.Dispose();
				_cacheSelectedBackBrush = null;
			}

			if (_cacheSelectedNoFocusBackBrush != null)
			{
				_cacheSelectedNoFocusBackBrush.Dispose();
				_cacheSelectedNoFocusBackBrush = null;
			}
		}
		
		private void CreateChildControls()
		{
			// Create scrollbars
			_vBar = new VScrollBar();
			_hBar = new HScrollBar();
			
			// Hook into scrolling events
			_vBar.ValueChanged += new EventHandler(OnVertValueChanged);
			_hBar.ValueChanged += new EventHandler(OnHorzValueChanged);

			// Create the corner panel to fill gap
			_corner = new Panel();

			// The corner is just the standard control background color
			_corner.BackColor = SystemColors.Control;

			// Make them invisible to start with
			_vBar.Visible = false;
			_hBar.Visible = false;
			_corner.Visible = false;
			
			// Add to collection of child controls
			Controls.AddRange(new Control[]{_vBar, _hBar, _corner});
		}
		
		private void InvalidateInnerRectangle()
		{
			// Need to recalculate inner rectangle
			_innerRectangleValid = false;

			// Change in size means updating scroll bars
			_scrollBarsValid = false;

			// Any change to inner rectangle must make draw rectangle invalid
			InvalidateDrawRectangle();
		}

		private void InvalidateDrawRectangle()
		{
			// Need to recalculate node drawing rectangles
			_drawRectangleValid = false;

			// Need to repaint to reflect change
			InvalidateAll();
		}

		private void SetAllNodeSizesDirty(NodeCollection nc)
		{
			foreach(Node n in nc)
			{
				// Set the size of the node as invalid
				n.Cache.InvalidateSize();
				
				// Recurse into children
				SetAllNodeSizesDirty(n.Nodes);
			}
		}
		
		private void CalculationCycle(Graphics g)
		{
			// Make sure calculations for drawing nodes is up to date
			CalculateAllNodes(g);

			// Calculate inner rectangle based on border and indent
			CalculateInnerRectangle();

			// Update display of scrolling bars
			UpdateScrollBars();
				
			// Adjust the inner rectangle to take account of scrollbars
			CalculateDrawRectangle();

			// Last chance to modify the nodes sizes using the VC's
			PostCalculateNodes();
		}

		private void CalculateAllNodes(Graphics g)
		{
			if (!_nodeDrawingValid)
			{
				// Clear down existing list of displayed nodes
				_displayNodes.Clear();

				// Do we need to mark all the nodes as dirty?
				if (_nodeSizesDirty)
				{
					SetAllNodeSizesDirty(Nodes);
					_nodeSizesDirty = false;
				}

				// Start processing with the root collection
				CalculateNodeCollection(g, Nodes, Point.Empty);
			}
		}

		private Rectangle CalculateNodeCollection(Graphics g, NodeCollection nc, Point topLeft)
		{
			// Get the extra space required for edges around the collection
			Edges edges = nc.VC.MeasureEdges(this, nc, g);

			// Set starting position of first child as collection plus collection top and left edges
			Point nodeTopLeft = new Point(topLeft.X, topLeft.Y);
			nodeTopLeft.X += edges.Left;
			nodeTopLeft.Y += edges.Top;

			// If we already have at least one node showing
			if ((_displayNodes.Count > 0) && (nc.VisibleCount > 0))
			{
				// Then add spacing gap between previous node and first in the collection
				nodeTopLeft.Y += VerticalNodeGap;
			}

			// Track the maximum child positioned
			int maxWidth = 0;

			// Find the size and then position each child node in turn
			foreach(Node n in nc)
			{
				// Only include the child Node if it is visible
				if (n.Visible)
				{
					// Add node to list of those displayed
					_displayNodes.Add(n);

					// Find the size that the node (and any children) require
					Size nodeSize = CalculateNode(g, n, nodeTopLeft); 

					// Move point down to start of next node
					nodeTopLeft.Y += nodeSize.Height;

					// Add the spacing gap between nodes
					nodeTopLeft.Y += VerticalNodeGap;

					// Track widest child
					if (nodeSize.Width > maxWidth)
						maxWidth = nodeSize.Width;
				}
				else
				{
					// Position the node but do not move downwards
					n.Cache.Size = Size.Empty;
					n.VC.SetPosition(this, n, nodeTopLeft);
					n.Cache.ChildBounds = n.Cache.Bounds;
				}
			}

			// Calculate bounding rectangle for the collection
			Rectangle bounds = new Rectangle(topLeft.X, 
											 topLeft.Y, 
											 maxWidth + edges.Left + edges.Right,
											 nodeTopLeft.Y - topLeft.Y + edges.Bottom);

			// Set into the collection itself
			nc.VC.SetBounds(this, nc, bounds);

			// Return to caller
			return bounds;
		}

		private Size CalculateNode(Graphics g, Node n, Point nodeTopLeft)
		{
			// Get view controller for the node
			INodeVC nVC = n.VC;

			// Find the size of just the node itself
			Size nodeSize = nVC.MeasureSize(this, n, g);
			
			// Cache the new size in the node
			n.Cache.Size = nodeSize;

			// Position the node according to its size
			Rectangle bounds = nVC.SetPosition(this, n, nodeTopLeft);

			// If expanded and the node has children
			if (n.IsExpanded)
			{
				// Find space needed to show the collection of child nodes
				Rectangle children = CalculateNodeCollection(g, n.Nodes, new Point(bounds.Left, bounds.Bottom));

				// Add extra height
				bounds.Height += children.Height;

				// Make sure we have enough width to handle children as well
				if (bounds.Width < children.Width)
					bounds.Width = children.Width;
			}

			// Tell node the bounds of itself and all of its children as well
			nVC.SetChildBounds(this, n, bounds);

			return bounds.Size;
		}

		private void CalculateInnerRectangle()
		{
			if (!_innerRectangleValid)
			{
				// Start with the client rectangle
				_innerRectangle = ClientRectangle;

				// Reduce by the border size needed
				_innerRectangle.Inflate(-_borderSize.Width, -_borderSize.Height);

				// Reduce by the indent padding
				_innerRectangle.Y += _borderIndent.Top;
				_innerRectangle.X += _borderIndent.Left;
				_innerRectangle.Width -= (_borderIndent.Left + _borderIndent.Right);
				_innerRectangle.Height -= (_borderIndent.Top + _borderIndent.Bottom);
				
				// The inner rectangle is now valid
				_innerRectangleValid = true;
			}
		}

		private void UpdateScrollBars()
		{
			if (!_scrollBarsValid)
			{
				// Size needed to show everything
				Size drawSize;

				// Space available for drawing
				Size innerSize;

				// Are we using Node granularity?
				if (VerticalGranularity == VerticalGranularity.Node)
				{
					// Cache the number of items on a page for scrolling
					CalculateVerticalPageSizes();

					// Vertical uses one scroll position per Node
					drawSize = new Size(Nodes.Cache.Bounds.Size.Width, _displayNodes.Count);
					innerSize = new Size(_innerRectangle.Size.Width, _displayHeightExScroll);
				}
				else
				{
					// Vertical uses one scroll position per pixel
					drawSize = Nodes.Cache.Bounds.Size;
					innerSize = _innerRectangle.Size;
				}
				
				// Default to not needing either scroll bar
				bool showVert = false;
				bool showHorz = false;
				bool enableVert = false;
				bool enableHorz = false;

				// Must be provide space for scroll bars
				bool enforceVert = (VerticalScrollbar == ScrollVisibility.Always);
				bool reduceVert = (VerticalScrollbar == ScrollVisibility.Never);
				bool enforceHorz = (HorizontalScrollbar == ScrollVisibility.Always);
				bool reduceHorz = (HorizontalScrollbar == ScrollVisibility.Never);

				// Reduce inner size according to enforced vertical bar
				if (enforceVert)
					innerSize.Width -= SystemInformation.VerticalScrollBarWidth;

				// Reduce inner size according to enforced horizontal bar
				if (enforceHorz)
				{
					if (VerticalGranularity == VerticalGranularity.Node)
						innerSize.Height = _displayHeightScroll;
					else
						innerSize.Height -= SystemInformation.HorizontalScrollBarHeight;
				}

				Point offsetMax = Point.Empty;

				// Do we need a vertical scroll bar?
				if (drawSize.Height > innerSize.Height)
				{
					// Request we show the vert scroll bar
					showVert = true;
					enableVert = true;

					// Reduce available horizontal space
					if (!enforceVert && !reduceVert)
						innerSize.Width -= SystemInformation.VerticalScrollBarWidth;
				}

				// Do we need a horizontal scroll bar?
				if (drawSize.Width > innerSize.Width)
				{
					// Request we show the horiz scroll bar
					showHorz = true;
					enableHorz = true;

					// Reduce available height space
					if (!enforceHorz && !reduceHorz)
					{
						if (VerticalGranularity == VerticalGranularity.Node)
							innerSize.Height = _displayHeightScroll;
						else
							innerSize.Height -= SystemInformation.HorizontalScrollBarHeight;
					}

					if (!showVert)
					{
						// Must retest vertical space, just in case having the horz scroll bar
						// now causes there to be too little space vertically
						if (drawSize.Height > innerSize.Height)
						{
							// Request we show the vert scroll bar
							showVert = true;
							enableVert = true;

							// Reduce available horizontal space
							if (!enforceVert && !enforceVert)
								innerSize.Width -= SystemInformation.VerticalScrollBarWidth;
						}
					}
				}

				// Calculate the scrolling values
				if (showVert)
				{
					// Define maximum value allowed
					offsetMax.Y = drawSize.Height - innerSize.Height;

					// Limit check actual offset
					if (_offset.Y > offsetMax.Y)
						_offset.Y = offsetMax.Y;
				}
				else
				{
					// Reset offsets
					_offset.Y = 0;
					offsetMax.Y = 0;
				}
						
				if (showHorz)
				{
					// Define maximum value allowed
					offsetMax.X = drawSize.Width - innerSize.Width;

					// Limit check actual offset
					if (_offset.X > offsetMax.X)
						_offset.X = offsetMax.X;
				}
				else
				{
					// Reset offsets
					_offset.X = 0;
					offsetMax.X = 0;
				}

				// New sizes of the scroll controls
				Size vertSize = Size.Empty;
				Size horzSize = Size.Empty;

				// Need to update the vertical scroll values? 
				if (showVert)
				{
					if ((drawSize.Height > 1) && (innerSize.Height > 1))
					{
                        // By default a small change is one unit
                        int smallChange = 1;

                        // If moving pixels at a time, then use pixel units for moving
                        if (VerticalGranularity == VerticalGranularity.Pixel)
                            smallChange = MouseUnits;

                        // Limit check the small change
                        smallChange = Math.Min(smallChange, innerSize.Height);

						// Define the range
						_vBar.Minimum = 0;
						_vBar.Maximum = drawSize.Height - 1;
                        _vBar.SmallChange = smallChange;
						_vBar.LargeChange = innerSize.Height;
						_vBar.Value = _offset.Y;
					}
					else
					{
						_vBar.Minimum = 0;
						_vBar.Maximum = 0;
						_vBar.Value = 0;
						showVert = false;
					}
				}

				// Need to update the horizontal scroll values?
				if (showHorz)
				{
					if ((drawSize.Width > 1) && (innerSize.Width > 1))
					{
                        // By default a small change is one unit
                        int smallChange = 1;

                        // If moving pixels at a time, then use pixel units for moving
                        if (VerticalGranularity == VerticalGranularity.Pixel)
                            smallChange = MouseUnits;

                        // Limit check the small change
                        smallChange = Math.Min(smallChange, innerSize.Width);

                        // Define the range
						_hBar.Minimum = 0;
						_hBar.Maximum = drawSize.Width - 1;
                        _hBar.SmallChange = smallChange;
						_hBar.LargeChange = innerSize.Width;
						_hBar.Value = _offset.X;
					}
					else
					{
						_hBar.Minimum = 0;
						_hBar.Maximum = 0;
						_hBar.Value = 0;
						showHorz = false;
					}
				}

				// Take into account user preferences on visibility
				switch(VerticalScrollbar)
				{
					case ScrollVisibility.Never:
						showVert = false;
						break;
					case ScrollVisibility.Always:
						showVert = true;
						break;
					case ScrollVisibility.WhenNeeded:
						// Do nothing, as that is the default processing anyway
						break;
				}

				switch(HorizontalScrollbar)
				{
					case ScrollVisibility.Never:
						showHorz = false;
						break;
					case ScrollVisibility.Always:
						showHorz = true;
						break;
					case ScrollVisibility.WhenNeeded:
						// Do nothing, as that is the default processing anyway
						break;
				}

				// Need to show the vertical scroll bar?
				if (showVert)
				{
					// Position/Size the vertical scroll bar
					_vBar.Location = new Point(this.Width - _vBar.Width - _borderSize.Width, _borderSize.Height);
					vertSize = new Size(_vBar.Width, this.Height - (_borderSize.Height * 2));
				}
				
				// Need to size and position the vertical scrollbar?
				if (showHorz)
				{
					// Position/Size the horizontal scroll bar
					_hBar.Location = new Point(_borderSize.Width, this.Height - _hBar.Height - _borderSize.Height);
					horzSize = new Size(this.Width - (_borderSize.Width * 2), _hBar.Height);

					// Showing both scroll bars
					if (showVert)
					{
						// Prevent them overlapping on bottom right of control
						vertSize.Height -= _hBar.Height;
						horzSize.Width -= _vBar.Width;
					}
				}

				// Update scroll bar sizes if they need changing
				if (vertSize != Size.Empty) _vBar.Size = vertSize;
				if (horzSize != Size.Empty) _hBar.Size = horzSize;

				// Update scroll bar enabled state
				if (_vBar.Enabled != enableVert) _vBar.Enabled = enableVert;
				if (_hBar.Enabled != enableHorz) _hBar.Enabled = enableHorz;

				// If we need the corner panel...
				if (showVert && showHorz)
				{
					// ...then define its size and position now
					_corner.Size = new Size(_vBar.Width, _hBar.Height);
					_corner.Location = new Point(_vBar.Left, _hBar.Top);
				}

				// Do we need to cover up the corner panel
				_corner.Visible = (showVert && showHorz);

				if (_vBar.Visible != showVert)
				{
					_vBar.Visible = showVert;
					InvalidateDrawRectangle();
				}

				if (_hBar.Visible != showHorz)
				{
					_hBar.Visible = showHorz;
					InvalidateDrawRectangle();
				}

				// Do not update scroll bar visibility/values next time
				_scrollBarsValid = true;
			}
		}

		private void CalculateDrawRectangle()
		{
			if (!_drawRectangleValid)
			{
				// Start with the inner rectangle
				_drawRectangle = _innerRectangle;

				// Reduce by scroll bars
				if (_vBar.Visible) _drawRectangle.Width -= _vBar.Width;
				if (_hBar.Visible) _drawRectangle.Height -= _hBar.Height;

				// The draw rectangle is now valid
				_drawRectangleValid = true;
			}
		}

		private void PostCalculateNodes()
		{
			if (!_nodeDrawingValid)
			{
				// Give the collection/node VC's a change to post process calculations
				NodeVC.PostCalculateNodes(this, _displayNodes);
				CollectionVC.PostCalculateNodes(this, _displayNodes);
				
				// The node drawing is now valid
				_nodeDrawingValid = true;
			}
		}

		private void DrawControlBorder(PaintEventArgs e)
		{		
			// Is there any border to be drawn?
			if (BorderStyle != TreeBorderStyle.None)
			{
				// If we are requested to use a theme and there is an active theme
				if ((BorderStyle == TreeBorderStyle.Theme) && IsControlThemed)
				{
					ThemeTreeViewItemState state;
				
					// Decide what state to draw border with
					if (Enabled)
						state = ThemeTreeViewItemState.Normal;
					else
						state = ThemeTreeViewItemState.Disabled;
	
					// Ask for a full border to be drawn around client rectangle
					_themeTreeView.DrawThemeBackground(e.Graphics, 
						ClientRectangle,
						_drawRectangle,
						(int)ThemeTreeViewPart.TreeItem, 
						(int)state);
				}
				else
				{
					// Otherwise we use a standard non-theme drawing method
					if (_borderIs3D)
						ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, _border3DStyle);
					else
						ControlPaint.DrawBorder(e.Graphics, ClientRectangle, BorderColor, _borderButtonStyle);
				}
			}
		}

		private void DrawAllNodes(PaintEventArgs e)
		{
			DrawNodeCollection(e.Graphics, Nodes);
		}

		private void DrawNodeCollection(Graphics g, NodeCollection nc)
		{
			// Get the view controller for the node
			INodeCollectionVC ncVC = nc.VC;

			// Only draw collection if some part of it is in visible area
			if (ncVC.IntersectsWith(this, nc, _clipRectangle))
			{
				// Use view controller to draw the actual node collection (pre-children)
				ncVC.Draw(this, nc, g, _clipRectangle, true);

				// Work out which node to start drawing with
				for(int i=nc.ChildFromY(_clipRectangle.Top); i<nc.Count; i++)
				{
					// Keep going until the next child is below clipping rectangle
					if (nc[i].Cache.ChildBounds.Top > _clipRectangle.Bottom)
						break;

					// Only draw node if it is visible
					if (nc[i].Visible)
						DrawNode(g, nc[i]);
				}

				// Use view controller to draw the actual node collection (post-children)
				ncVC.Draw(this, nc, g, _clipRectangle, false);
			}
		}

		private void DrawNode(Graphics g, Node n)
		{	
			// Can only draw a node if it is visible
			if (n.Visible)
			{
				// Get the view controller for the node
				INodeVC nVC = n.VC;

				// Only draw node if some part of it is in visible area
				if (nVC.IntersectsWith(this, n, _clipRectangle, false))
				{
					// Use view controller to draw the actual node
					nVC.Draw(this, n, g, _clipRectangle, 0, 0);
				}

				// Only draw child nodes if some part of them is in visible area
				if (nVC.IntersectsWith(this, n, _clipRectangle, true))
				{
					// If expanded and there are children
					if (n.IsExpanded)
						DrawNodeCollection(g, n.Nodes);
				}
			}
		}

		private void SetTooltipNode(Node n)
		{
			// Only interested in changes
			if (n != _tooltipNode)
			{
				// Need to redraw the old hot node
				if (_tooltipNode != null)
					RemoveAnyToolTip();

				// Is the node allowed to have a tooltip?
				if ((n != null) && (n.VC != null) && !n.VC.CanToolTip(this, n))
					n = null;

				// Remember new node
				_tooltipNode = n;

				// Need to redraw the new hot node
				if (_tooltipNode != null)
					TestForNeedingToolTip();
			}
		}

		private void SetHotNode(Node n)
		{
			// Only interested in changes
			if (n != _hotNode)
			{
				// Need to redraw the old hot node
				if (_hotNode != null)
					InvalidateNodeLine(_hotNode, false);

				// Remember new node
				_hotNode = n;

				// Need to redraw the new hot node
				if (_hotNode != null)
					InvalidateNodeLine(_hotNode, false);

				// Raise event to notify change
				OnHotNodeChanged();
			}
			else if (_hotNode != null)
			{
				// Need to redraw the existing hot node
				InvalidateNode(_hotNode, false);
			}
		}

		private void RemoveAnyToolTip()
		{
			if (_infotipTimer.Enabled)
				_infotipTimer.Stop();
			
			// Is there a tooltip currently being used?
			if (_tooltip != null)
			{
				// Kill it
				_tooltip.Hide();
				_tooltip.Dispose();
				_tooltip = null;

				// No node is currently the tooltip node
				_tooltipNode = null;
			}
		}

		private void TestForNeedingToolTip()
		{
			// Is it possible we need a tool or info tip?
			if (Tooltips || Infotips)
			{
				Form f = FindForm();

				// Only allow tooltip if the form we are inside is active
				if ((f == null) || ((f != null) && f.ContainsFocus))
				{
					// The tooltip node is always the hot node
					Node test = TooltipNode;

					// Must always have a node to test
					if (test != null)
					{
						// If we are authorized to use info tips and we have one?
						if (Infotips && (test.Tooltip != null) && (test.Tooltip.Length > 0))
						{
							// Kick off timer for showing it
							_infotipTimer.Start();
							return;
						}

						// Should we check if standard tooltip is possible?
						if (Tooltips && (test.Cache != null) && (test.VC != null))
						{
							// Get the current client based position of the node
							Rectangle bounds = NodeSpaceToClient(test.Cache.Bounds);

							// Is the node obscured on the left or right hand side?
							if ((bounds.Left < _drawRectangle.Left) || 
								(bounds.Right > _drawRectangle.Right))
							{
                                VisualStyle tooltipStyle;

                                switch (GroupColoring)
                                {
                                    case GroupColoring.Office2003Light:
                                    case GroupColoring.Office2003Dark:
                                        tooltipStyle = VisualStyle.Office2003;
                                        break;
                                    case GroupColoring.Office2007BlueLight:
                                    case GroupColoring.Office2007BlueDark:
                                        tooltipStyle = VisualStyle.Office2007Blue;
                                        break;
                                    case GroupColoring.Office2007SilverLight:
                                    case GroupColoring.Office2007SilverDark:
                                        tooltipStyle = VisualStyle.Office2007Silver;
                                        break;
                                    case GroupColoring.Office2007BlackLight:
                                    case GroupColoring.Office2007BlackDark:
                                        tooltipStyle = VisualStyle.Office2007Black;
                                        break;
                                    case GroupColoring.MediaPlayerBlueLight:
                                    case GroupColoring.MediaPlayerBlueDark:
                                        tooltipStyle = VisualStyle.MediaPlayerBlue;
                                        break;
                                    case GroupColoring.MediaPlayerOrangeLight:
                                    case GroupColoring.MediaPlayerOrangeDark:
                                        tooltipStyle = VisualStyle.MediaPlayerOrange;
                                        break;
                                    case GroupColoring.MediaPlayerPurpleLight:
                                    case GroupColoring.MediaPlayerPurpleDark:
                                        tooltipStyle = VisualStyle.MediaPlayerPurple;
                                        break;
                                    default:
                                        tooltipStyle = VisualStyle.Plain;
                                        break;
                                }

								// Create a tooltip for this node entry
                                _tooltip = new PopupTooltipSingle(tooltipStyle, test.GetNodeFont());
                                _tooltip.Apply2007ClearType = (TextRenderingHint == TextRenderingHint.ClearTypeGridFit);
                                _tooltip.ApplyMediaPlayerClearType = (TextRenderingHint == TextRenderingHint.ClearTypeGridFit);
					
								// Get the node rectangle that covers just the text
								Rectangle textRect = test.VC.GetTextRectangle(this, test);

								// Convert from node to client coordinates
								textRect = NodeSpaceToClient(textRect);

								// Convert from client to global coordinates
								textRect = this.RectangleToScreen(textRect);

								// We need to know when the mouse enters/leaves the tooltip
								_tooltip.TextHeight = textRect.Height;

								// Define the text to be shown
								_tooltip.ToolText = test.Text;

								// Make it visible!
								_tooltip.ShowWithoutFocus(new Point(textRect.X - 3, textRect.Y));
							}
						}
					}
				}
			}
		}

		private void CalculateVerticalPageSizes()
		{
			// Only need values when scrolling with Node granularity
			if (VerticalGranularity == VerticalGranularity.Node)
			{
				// We want to find how many items at end will completely fill the 
				// inner rectangle with and without the horizontal scroll bar showing
				int innerExHeight = _innerRectangle.Size.Height;
				int innerHeight = innerExHeight - SystemInformation.HorizontalScrollBarHeight;

				// We always include the last node to ensure height is at least one node
				_displayHeightExScroll = 0;
				_displayHeightScroll = 0;

				if (_displayNodes.Count > 0)
				{
					// Y Extent, is bottom of the root nodes
					int yExtent = Nodes.Cache.Bounds.Bottom;

					// Count from bottom of node list up towards the top
					for(int i=_displayNodes.Count-1; i>=0; --i)
					{
						// Total height so far is the distance between top of this node
						// and the y total extent as taken as the bottom most position.
						int totalHeight = yExtent - (_displayNodes[i] as Node).Cache.Bounds.Top;

						// Is there space left when scrollbar is in place?
						if (innerHeight >= totalHeight)
						{
							// Must be space enough for both areas
							_displayHeightScroll++;
							_displayHeightExScroll++;
						}
						else
						{
							// Is there more space left without the scrollbar?
							if (innerExHeight >= totalHeight)
							{
								// Only reduce the area without scrollbar
								_displayHeightExScroll++;
							}
							else
								break;
						}
					}
				}

				// Limit check, we always have a minimum page size of 1
				if (_displayHeightExScroll == 0) _displayHeightExScroll = 1;
				if (_displayHeightScroll == 0) _displayHeightScroll = 1;
			}
		}

		private void OnDragHoverTimeout(object sender, EventArgs e)
		{
			// Stop the timer from expiring again
			_dragHoverTimer.Stop();
		
			// We should still have an dragging node
			if (_dragNode != null)
			{
				// Ask the node to perform anything specific when hovered over
				_dragNode.VC.DragHover(this, _dragNode);
			}
		}

		private void OnDragBumpTimeout(object sender, EventArgs e)
		{
			// Modify the scrolling position for bumping direction
			if (_bumpUpwards)
			{
				// If already at the top limit
				if (_offset.Y == 0)
				{
					// Stop bump timer from expiring again
					_dragBumpTimer.Stop();
				}
				else
				{
                    // How to change offset depends on granularity
                    if (VerticalGranularity == VerticalGranularity.Pixel)
                        _offset.Y -= MouseUnits;
                    else
                        _offset.Y--;

                    // Limit check the offset
                    _offset.Y = Math.Max(0, _offset.Y);

					// Must recalculate scrolling and redraw
					InvalidateNodeDrawing();
				}
			}
			else
			{
                // How to change offset depends on granularity
                if (VerticalGranularity == VerticalGranularity.Pixel)
                    _offset.Y += MouseUnits;
                else
                    _offset.Y++;

				// Must recalculate scrolling and redraw
				InvalidateNodeDrawing();
			}
		}

		private void OnAutoEditTimeout(object sender, EventArgs e)
		{
			// Always stop the timer
			_autoEditTimer.Stop();

			// Is there a valid node to edit?
			if (_autoEditNode != null)
			{
				// If there is only a single selection and its the same node
				if ((SelectedCount == 1) && (_autoEditNode.IsSelected))
					_autoEditNode.BeginEdit();

				// Remove remembered values
				_autoEditNode = null;
			}
		}

		private void OnInfoTipTimeout(object sender, EventArgs e)
		{
			// Always stop the timer
			_infotipTimer.Stop();

            VisualStyle tooltipStyle;

            switch (GroupColoring)
            {
                case GroupColoring.Office2003Light:
                case GroupColoring.Office2003Dark:
                    tooltipStyle = VisualStyle.Office2003;
                    break;
                case GroupColoring.Office2007BlueLight:
                case GroupColoring.Office2007BlueDark:
                    tooltipStyle = VisualStyle.Office2007Blue;
                    break;
                case GroupColoring.Office2007SilverLight:
                case GroupColoring.Office2007SilverDark:
                    tooltipStyle = VisualStyle.Office2007Silver;
                    break;
                case GroupColoring.Office2007BlackLight:
                case GroupColoring.Office2007BlackDark:
                    tooltipStyle = VisualStyle.Office2007Black;
                    break;
                case GroupColoring.MediaPlayerBlueLight:
                case GroupColoring.MediaPlayerBlueDark:
                    tooltipStyle = VisualStyle.MediaPlayerBlue;
                    break;
                case GroupColoring.MediaPlayerOrangeLight:
                case GroupColoring.MediaPlayerOrangeDark:
                    tooltipStyle = VisualStyle.MediaPlayerOrange;
                    break;
                case GroupColoring.MediaPlayerPurpleLight:
                case GroupColoring.MediaPlayerPurpleDark:
                    tooltipStyle = VisualStyle.MediaPlayerPurple;
                    break;
                default:
                    tooltipStyle = VisualStyle.Plain;
                    break;
            }
            
            // Create a tooltip for this node entry
            _tooltip = new PopupTooltipSingle(tooltipStyle);
            _tooltip.Apply2007ClearType = (TextRenderingHint == TextRenderingHint.ClearTypeGridFit);
            _tooltip.ApplyMediaPlayerClearType = (TextRenderingHint == TextRenderingHint.ClearTypeGridFit);

			// Define string for display
			_tooltip.ToolText = TooltipNode.Tooltip;
					
			// Make it visible!
			_tooltip.ShowWithoutFocus(new Point(Control.MousePosition.X, Control.MousePosition.Y + 24));
		}

		private void OnLabelEditBoxLostFocus(object sender, EventArgs e)
		{
			// Losing focus means we end the label editing
			EndEditLabel(false);
		}

		private void OnLabelEditBoxKeyDown(object sender, KeyEventArgs e)
		{
			// User wants to quite from editing
			if (e.KeyCode == Keys.Escape)
				EndEditLabel(true);
				
			// User wants to accept current value
			if (e.KeyCode == Keys.Enter)
				EndEditLabel(false);
		}

		private void OnLabelEditTextChanged(object sender, EventArgs e)
		{
			// A change in the contents of the textbox mean we should
			// recalcualte the width needed for the box, so it expands
			// and contracts with the contents (within min and max limits)
			
			// Need graphics instance to measure text size
			using(Graphics g = _labelEditBox.CreateGraphics())
			{
				// Simple measurement of node text size
				SizeF sizeF = g.MeasureString(_labelEditBox.Text + "W", _labelEditBox.Font);
							
				// Find the width of a minmum string
				SizeF minF = g.MeasureString("01234", _labelEditBox.Font);
							
				// Use the largest of the two
				if (minF.Width > sizeF.Width)
					sizeF.Width = minF.Width;
					
				// Calculate the current textbox rectangle
				Rectangle textRect = new Rectangle(_labelEditBox.Location, _labelEditBox.Size);
				
				// Update for the new size
				textRect.Width = (int)(sizeF.Width) + (SystemInformation.BorderSize.Width * 2);
			
				// Limit check the right hand side to the edge of the drawing area
				if (textRect.Right > _innerRectangle.Right)
					textRect.Width -= (textRect.Right - _innerRectangle.Right);
					
				// Define its size and position
				_labelEditBox.SetBounds(textRect.X, textRect.Y, textRect.Width, textRect.Height);
			}
		}

		private void OnVertValueChanged(object sender, EventArgs e)
		{
			// Update scrolling offset
			_offset.Y = _vBar.Value;

			// Change in scrolling should end any label editing
			if (LabelEditNode != null)
				EndEditLabel(false);

			// Must repaint the change straight away
			Refresh();
		}

		private void OnHorzValueChanged(object sender, EventArgs e)
		{
			// Update scrolling offset
			_offset.X = _hBar.Value;

			// Change in scrolling should end any label editing
			if (LabelEditNode != null)
				EndEditLabel(false);

			// Must repaint the change straight away
			Refresh();
		}

		private void UpdateBorderCache(Graphics g)
		{
			if (BorderStyle == TreeBorderStyle.None)
			{
				// Cache the correct border size
				_borderSize = Size.Empty;
			}
			else
			{
				bool calculate = true;
				
				// Cache the drawing style (3D or ButtonSytle)
				switch(BorderStyle)
				{
					case TreeBorderStyle.Adjust3D:
						_borderIs3D = true;
						_border3DStyle = Border3DStyle.Adjust;
						break;
					case TreeBorderStyle.Bump3D:
						_borderIs3D = true;
						_border3DStyle = Border3DStyle.Bump;
						break;
					case TreeBorderStyle.Etched3D:
						_borderIs3D = true;
						_border3DStyle = Border3DStyle.Etched;
						break;
					case TreeBorderStyle.Flat3D:
						_borderIs3D = true;
						_border3DStyle = Border3DStyle.Flat;
						break;
					case TreeBorderStyle.Raised3D:
						_borderIs3D = true;
						_border3DStyle = Border3DStyle.Raised;
						break;
					case TreeBorderStyle.RaisedInner3D:
						_borderIs3D = true;
						_border3DStyle = Border3DStyle.RaisedInner;
						break;
					case TreeBorderStyle.RaisedOuter3D:
						_borderIs3D = true;
						_border3DStyle = Border3DStyle.RaisedOuter;
						break;
					case TreeBorderStyle.Sunken3D:
						_borderIs3D = true;
						_border3DStyle = Border3DStyle.Sunken;
						break;
					case TreeBorderStyle.SunkenInner3D:
						_borderIs3D = true;
						_border3DStyle = Border3DStyle.SunkenInner;
						break;
					case TreeBorderStyle.SunkenOuter3D:
						_borderIs3D = true;
						_border3DStyle = Border3DStyle.SunkenOuter;
						break;
					case TreeBorderStyle.Dashed:
						_borderIs3D = false;
						_borderButtonStyle = ButtonBorderStyle.Dashed;
						break;
					case TreeBorderStyle.Dotted:
						_borderIs3D = false;
						_borderButtonStyle = ButtonBorderStyle.Dotted;
						break;
					case TreeBorderStyle.Solid:
						_borderIs3D = false;
						_borderButtonStyle = ButtonBorderStyle.Solid;
						break;
					case TreeBorderStyle.Theme:
						// Is there a theme to use?
						if (IsControlThemed)
						{
							// Find the size needed to draw a full border
							Size size = _themeTreeView.GetThemePartSize(g,
																		(int)ThemeTreeViewPart.TreeItem, 
																		(int)ThemeTreeViewItemState.Normal,
																		Win32.THEMESIZE.TS_TRUE);
																
							// Border is half the total width and height plus an extra pixel so
							// we have a gap between borders and the scroll bars, inner rectangle																
							_borderSize = new Size(size.Width / 2 + 1, size.Height / 2 + 1);
							
							// No need to get size from system information
							calculate = false;
						}
						else
						{
							// Nope, so default to the default BorderStyle value
							_borderIs3D = true;
							_border3DStyle = Border3DStyle.Sunken;
						}
						break;
				}
				
				if (calculate)
				{
					// Cache the correct border size
					if (_borderIs3D)
						_borderSize = SystemInformation.Border3DSize;
					else
						_borderSize = SystemInformation.BorderSize;
				}
			}
		}

		private void OnBorderIndentChanged(object sender, EventArgs e)
		{
			// Must recalculate the inner drawing rectangle to reflect changing indent
			InvalidateInnerRectangle();
		}

		private void OnPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			// Must reset for the new theme
			_colorDetails.Reset();

			// Redraw using latest system defined colors and fonts
 			InvalidateAll();
		}

		private void InvalidateAll()
		{
			// Only request Invalidate when required
			if (!_invalidated)
			{
				_invalidated = true;
				Invalidate();
			}
		}

        private VisualStyle VisualStyleFromTreeControlStyles(TreeControlStyles tcs)
        {
            switch (tcs)
            {
                case TreeControlStyles.GroupMediaBlueLight:
                case TreeControlStyles.GroupMediaBlueDark:
                    return VisualStyle.MediaPlayerBlue;
                case TreeControlStyles.GroupMediaOrangeLight:
                case TreeControlStyles.GroupMediaOrangeDark:
                    return VisualStyle.MediaPlayerOrange;
                case TreeControlStyles.GroupMediaPurpleLight:
                case TreeControlStyles.GroupMediaPurpleDark:
                    return VisualStyle.MediaPlayerPurple;
                case TreeControlStyles.GroupOfficeBlueDark:
                case TreeControlStyles.GroupOfficeBlueLight:
                    return VisualStyle.Office2007Blue;
                case TreeControlStyles.GroupOfficeSilverDark:
                case TreeControlStyles.GroupOfficeSilverLight:
                    return VisualStyle.Office2007Silver;
                case TreeControlStyles.GroupOfficeBlackDark:
                case TreeControlStyles.GroupOfficeBlackLight:
                    return VisualStyle.Office2007Black;
                case TreeControlStyles.GroupOfficeDark:
                case TreeControlStyles.GroupOfficeLight:
                    return VisualStyle.Office2003;
                default:
                    return VisualStyle.Plain;
            }
        }

		private void OnThemeChanged(object sender, EventArgs e)
		{
			// Update border cached values in case of themed border
			using(Graphics g = this.CreateGraphics())
			{
				UpdateBorderCache(g);

				// If we are currently themed, then cache the glyph size	
				if (IsControlThemed)
				{
					// Find the size needed to draw a full border
					_glyphThemeSize = _themeTreeView.GetThemePartSize(g,
																	  (int)ThemeTreeViewPart.Glyph, 
																	  (int)ThemeTreeViewGlyphState.Closed,
																	  Win32.THEMESIZE.TS_TRUE);
				}
			}
			
			// Reset any caches pens/brushes
			ClearLineBoxCache();
			ClearCheckCache();
			ClearGroupCache();
			ClearNodeCache();

			// A change of theme might cause a change in border size
			// and/or box drawing; so invalidate everything.
			InvalidateInnerRectangle();
			InvalidateNodeDrawing();
		}

        private static bool CheckContextMenuForShortcut(ContextMenuStrip cms,
                                                       ref Message msg,
                                                       Keys keyData)
        {
            if (cms != null)
            {
                // Cache the info needed to sneak access to the context menu strip
                if (_cachedShortcutPI == null)
                {
                    _cachedShortcutPI = typeof(ToolStrip).GetProperty("Shortcuts",
                                                                      BindingFlags.Instance |
                                                                      BindingFlags.GetProperty |
                                                                      BindingFlags.NonPublic);

                    _cachedShortcutMI = typeof(ToolStripMenuItem).GetMethod("ProcessCmdKey",
                                                                            BindingFlags.Instance |
                                                                            BindingFlags.NonPublic);
                }

                // Get any menu item from context strip that matches the shortcut key combination
                Hashtable shortcuts = (Hashtable)_cachedShortcutPI.GetValue(cms, null);
                ToolStripMenuItem menuItem = (ToolStripMenuItem)shortcuts[keyData];

                // If we found a match...
                if (menuItem != null)
                {
                    // Get the menu item to process the shortcut
                    object ret = _cachedShortcutMI.Invoke(menuItem, new object[] { msg, keyData });

                    // Return the 'ProcessCmdKey' result
                    if (ret != null)
                        return (bool)ret;
                }
            }

            return false;
        }

	}
}
