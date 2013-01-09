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
using System.ComponentModel;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Represents a single node in the data hierarchy.
	/// </summary>
	[ToolboxItem(false)]
	[DefaultProperty("Text")]
	[DefaultEvent("TextChanged")]
	public class Node : Component, ICloneable
	{
		// Constants
		private const string _textDefault = "Node";

		// Instance fields - Internal state
		private NodeCache _cache;
		private NodeCollection _parentNodes;

		// Instance fields - Properties
		private string _text;
		private string _tooltip;
		private Font _nodeFont;
		private Font _nodeFontBoldItalic;
		private int _nodeFontHeight;
		private Color _backColor;
		private Color _foreColor;
		private int _imageIndex;
		private int _selectedImageIndex;
		private Icon _icon;
		private Icon _selectedIcon;
		private Image _image;
		private Image _selectedImage;
		private CheckState _checkState;
		private NodeCheckStates _checkStates;
		private Indicator _indicator;
		private Flags _flags;
		private object _key;
		private object _tag;
		private NodeCollection _nodes;
		private INodeVC _vc;
		private Node _original;
		
		[Flags]
		private enum Flags
		{
			Expanded = 1,
			Visible = 2,
			Selectable = 4,
			Removing = 8
		}

		/// <summary>
		/// Occurs after the Text property value has changed.
		/// </summary>
		[Category("Appearance")]
		public event EventHandler TextChanged;

		/// <summary>
		/// Occurs after the Tooltip property value has changed.
		/// </summary>
		[Category("Appearance")]
		public event EventHandler TooltipChanged;

		/// <summary>
		/// Occurs after the Font property value has changed.
		/// </summary>
		[Category("Appearance")]
		public event EventHandler NodeFontChanged;

		/// <summary>
		/// Occurs after the BackColor property value has changed.
		/// </summary>
		[Category("Appearance")]
		public event EventHandler BackColorChanged;

		/// <summary>
		/// Occurs after the ForeColor property value has changed.
		/// </summary>
		[Category("Appearance")]
		public event EventHandler ForeColorChanged;

		/// <summary>
		/// Occurs after the ImageIndex property value has changed.
		/// </summary>
		[Category("Appearance")]
		public event EventHandler ImageIndexChanged;

		/// <summary>
		/// Occurs after the SelectedImageIndex property value has changed.
		/// </summary>
		[Category("Appearance")]
		public event EventHandler SelectedImageIndexChanged;

		/// <summary>
		/// Occurs after the Icon property value has changed.
		/// </summary>
		[Category("Appearance")]
		public event EventHandler IconChanged;

		/// <summary>
		/// Occurs after the SelectedIcon property value has changed.
		/// </summary>
		[Category("Appearance")]
		public event EventHandler SelectedIconChanged;

		/// <summary>
		/// Occurs after the Image property value has changed.
		/// </summary>
		[Category("Appearance")]
		public event EventHandler ImageChanged;

		/// <summary>
		/// Occurs after the SelectedImage property value has changed.
		/// </summary>
		[Category("Appearance")]
		public event EventHandler SelectedImageChanged;

		/// <summary>
		/// Occurs after the Indicator property value has changed.
		/// </summary>
		[Category("Appearance")]
		public event EventHandler IndicatorChanged;

		/// <summary>
		/// Occurs when the value of the CheckState property changes.
		/// </summary>
		[Category("Appearance")]
		public event EventHandler CheckStateChanged;

		/// <summary>
		/// Occurs when the value of the CheckStates property changes.
		/// </summary>
		[Category("Appearance")]
		public event EventHandler CheckStatesChanged;

		/// <summary>
		/// Occurs after the Visible property value has changed.
		/// </summary>
		[Category("Behavior")]
		public event EventHandler VisibleChanged;

		/// <summary>
		/// Occurs after the Selectable property value has changed.
		/// </summary>
		[Category("Behavior")]
		public event EventHandler SelectableChanged;

		/// <summary>
		/// Occurs after the Expanded property value has changed.
		/// </summary>
		[Category("Behavior")]
		public event EventHandler ExpandedChanged;

		/// <summary>
		/// Occurs after the Key property value has changed.
		/// </summary>
		[Category("Data")]
		public event EventHandler KeyChanged;

		/// <summary>
		/// Occurs after the Tag property value has changed.
		/// </summary>
		[Category("Data")]
		public event EventHandler TagChanged;

		/// <summary>
		/// Occurs after the Tag property value has changed.
		/// </summary>
		[Browsable(false)]
		public event EventHandler VCChanged;

		/// <summary>
		/// Initialize a new instance of the Node class.
		/// </summary>
		public Node()
		{
			CommonConstruct();
		}

		/// <summary>
		/// Initialize a new instance of the Node class.
		/// </summary>
		/// <param name="text">Initial Text property.</param>
		public Node(string text)
		{
			CommonConstruct();

			// Initialize with the passed text parameter
			_text = text;
		}

		private void CommonConstruct()
		{
			// Do not have a parent node/collection or TreeControl yet
			_parentNodes = null;

			// Create a collection attached to this instance
			_nodes = new NodeCollection(this);

			// Create object that manages cached information
			_cache = new NodeCache();

			// Do not have a VC defined
			_vc = null;
			
			// This is not a clone
			_original = null;
			
			// Reset the values of standard properties
			ResetText();
			ResetTooltip();
			ResetNodeFont();
			ResetForeColor();
			ResetBackColor();
			ResetIcon();
			ResetImage();
			ResetImageIndex();
			ResetSelectedIcon();
			ResetSelectedImage();
			ResetSelectedImageIndex();
			ResetIndicator();
			ResetCheckStates();
			ResetCheckState();
			ResetExpanded();
			ResetVisible();
			ResetSelectable();
			ResetKey();
			ResetTag();
		}

		/// <summary>
		/// Gets or sets the parent node that contains this node as a sub node.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Node Parent
		{
			get { return Cache.ParentNode; }
		}
		
		/// <summary>
		/// Gets or sets the parent node collection that contains this node as a sub node.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public NodeCollection ParentNodes
		{
			get { return _parentNodes; }
		}

		/// <summary>
		/// Gets reference to the TreeControl instance this node is inside.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TreeControl TreeControl
		{
			get { return Cache.TreeControl; }
		}

		/// <summary>
		/// Gets the TreeControl instance this collection is attached to.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public INodeVC VC
		{
			get 
			{ 
				if (_vc != null)
					return _vc;

				if (Cache.TreeControl != null)
					return Cache.TreeControl.NodeVC;
				
				return null;
			}

			set 
			{ 
				if (_vc != value)
				{
					_vc = value; 
					OnVCChanged();

					// Ask the tree control to redraw us
					if (Cache.TreeControl != null)
						Cache.TreeControl.InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Gets the collection of nodes in this node instance.
		/// </summary>
		[Category("Data")]
		[Description("The collection of child nodes in the node.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Browsable(false)]
		public NodeCollection Nodes
		{
			get { return _nodes; }
		}

		/// <summary>
		/// Gets and sets the text associated with the node instance.
		/// </summary>
		[Category("Appearance")]
		[Description("The text contained in the node.")]
		[Localizable(true)]
		public string Text
		{
			get { return _text; }

			set
			{
				if (_text != value)
				{
					_text = value;
					Cache.InvalidateSize();
					OnTextChanged();

					// Ask the tree control to redraw us
					if (Cache.TreeControl != null)
						Cache.TreeControl.InvalidateNodeDrawing();
				}
			}
		}

		private bool ShouldSerializeText()
		{
			// Only serialize if not the default value
			return (Text != _textDefault);
		}

		/// <summary>
		/// Resets the Text property to its default value.
		/// </summary>
		public void ResetText()
		{
			Text = _textDefault;
		}

		/// <summary>
		/// Gets and sets the tooltip associated with the node instance.
		/// </summary>
		[Category("Appearance")]
		[Description("The tooltip for the node.")]
		[Localizable(true)]
		[DefaultValue("")]
		public string Tooltip
		{
			get { return _tooltip; }

			set
			{
				if (_tooltip != value)
				{
					_tooltip = value;
					OnTooltipChanged();
				}
			}
		}

		/// <summary>
		/// Resets the Tooltip property to its default value.
		/// </summary>
		public void ResetTooltip()
		{
			Tooltip = "";
		}

		/// <summary>
		/// Gets and sets the font used to display Node text.
		/// </summary>
		[Category("Appearance")]
		[Description("The font used to display Node text.")]
		public Font NodeFont
		{
			get 
			{ 
				if (_nodeFont != null)
					return _nodeFont;
				else if (Cache.TreeControl != null)
					return Cache.TreeControl.Font;
				else
					return null;
			}

			set
			{
				if (_nodeFont != value)
				{
					_nodeFont = value;
					_nodeFontBoldItalic = null;
					_nodeFontHeight = -1;
					Cache.InvalidateSize();
					OnNodeFontChanged();

					// Ask the tree control to redraw us
					if (Cache.TreeControl != null)
						Cache.TreeControl.InvalidateNodeDrawing();
				}
			}
		}

		private bool ShouldSerializeNodeFont()
		{
			// Only serialize if not the default value
			return (_nodeFont != null);
		}

		/// <summary>
		/// Resets the NodeFont property to its default value.
		/// </summary>
		public void ResetNodeFont()
		{
			NodeFont = null;
		}

		/// <summary>
		/// Gets and sets the foreground color used to draw text and graphics.
		/// </summary>
		[Category("Appearance")]
		[Description("The foreground color used to draw text and graphics.")]
		public Color ForeColor
		{
			get 
			{ 
				if (_foreColor != Color.Empty)
					return _foreColor;
				else if (Cache.TreeControl != null)
					return Cache.TreeControl.ForeColor;
				else
					return Color.Empty;
			}

			set
			{
				if (_foreColor != value)
				{
					_foreColor = value;
					Cache.InvalidateSize();
					OnForeColorChanged();

					// Ask the tree control to redraw us
					if (Cache.TreeControl != null)
						Cache.TreeControl.InvalidateNodeDrawing();
				}
			}
		}

		private bool ShouldSerializeForeColor()
		{
			// Only serialize if not the default value
			return (_foreColor != Color.Empty);
		}

		/// <summary>
		/// Resets the ForeColor property to its default value.
		/// </summary>
		public void ResetForeColor()
		{
			ForeColor = Color.Empty;
		}

		/// <summary>
		/// Gets and sets the background color used to draw text and graphics.
		/// </summary>
		[Category("Appearance")]
		[Description("The background color used to draw text and graphics.")]
		public Color BackColor
		{
			get 
			{ 
				if (_backColor != Color.Empty)
					return _backColor;
				else if (Cache.TreeControl != null)
					return Cache.TreeControl.BackColor;
				else
					return Color.Empty;
			}

			set
			{
				if (_backColor != value)
				{
					_backColor = value;
					Cache.InvalidateSize();
					OnBackColorChanged();

					// Ask the tree control to redraw us
					if (Cache.TreeControl != null)
						Cache.TreeControl.InvalidateNodeDrawing();
				}
			}
		}

		private bool ShouldSerializeBackColor()
		{
			// Only serialize if not the default value
			return (_backColor != Color.Empty);
		}

		/// <summary>
		/// Resets the BackColor property to its default value.
		/// </summary>
		public void ResetBackColor()
		{
			BackColor = Color.Empty;
		}

		/// <summary>
		/// Gets and sets the image index for node.
		/// </summary>
		[Category("Appearance")]
		[Description("The image index for node.")]
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

					OnImageIndexChanged();

					// Ask the tree control to redraw us
					if (Cache.TreeControl != null)
						Cache.TreeControl.InvalidateNodeDrawing();
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
		/// Gets and sets the selected image index for node.
		/// </summary>
		[Category("Appearance")]
		[Description("The selected image index for node.")]
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

					OnSelectedImageIndexChanged();

					// Ask the tree control to redraw us
					if (Cache.TreeControl != null)
						Cache.TreeControl.InvalidateNodeDrawing();
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
		/// Gets and sets the icon for node.
		/// </summary>
		[Category("Appearance")]
		[Description("The icon for node.")]
		[DefaultValue(null)]
		public Icon Icon
		{
			get { return _icon; }

			set
			{
				if (_icon != value)
				{
					_icon = value;
					Cache.InvalidateSize();
					OnIconChanged();

					// Ask the tree control to redraw us
					if (Cache.TreeControl != null)
						Cache.TreeControl.InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the Icon property to its default value.
		/// </summary>
		public void ResetIcon()
		{
			Icon = null;
		}

		/// <summary>
		/// Gets and sets the selected icon for node.
		/// </summary>
		[Category("Appearance")]
		[Description("The selected icon for node.")]
		[DefaultValue(null)]
		public Icon SelectedIcon
		{
			get { return _selectedIcon; }

			set
			{
				if (_selectedIcon != value)
				{
					_selectedIcon = value;
					Cache.InvalidateSize();
					OnSelectedIconChanged();

					// Ask the tree control to redraw us
					if (Cache.TreeControl != null)
						Cache.TreeControl.InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the Icon property to its default value.
		/// </summary>
		public void ResetSelectedIcon()
		{
			SelectedIcon = null;
		}

		/// <summary>
		/// Gets and sets the image for node.
		/// </summary>
		[Category("Appearance")]
		[Description("The image for node.")]
		[DefaultValue(null)]
		public Image Image
		{
			get { return _image; }

			set
			{
				if (_image != value)
				{
					_image = value;
					Cache.InvalidateSize();
					OnImageChanged();

					// Ask the tree control to redraw us
					if (Cache.TreeControl != null)
						Cache.TreeControl.InvalidateNodeDrawing();
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
		/// Gets and sets the selected image for node.
		/// </summary>
		[Category("Appearance")]
		[Description("The selected image for node.")]
		[DefaultValue(null)]
		public Image SelectedImage
		{
			get { return _selectedImage; }

			set
			{
				if (_selectedImage != value)
				{
					_selectedImage = value;
					Cache.InvalidateSize();
					OnSelectedImageChanged();

					// Ask the tree control to redraw us
					if (Cache.TreeControl != null)
						Cache.TreeControl.InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the SelectedImage property to its default value.
		/// </summary>
		public void ResetSelectedImage()
		{
			SelectedImage = null;
		}

		/// <summary>
		/// Gets and sets the indicator symbol.
		/// </summary>
		[Category("Appearance")]
		[Description("Indicator symbol for this node.")]
		[DefaultValue(typeof(Indicator), "None")]
		public Indicator Indicator
		{
			get { return _indicator; }

			set
			{
				if (_indicator != value)
				{
					_indicator = value;
					OnIndicatorChanged();

					// Ask the tree control to redraw us
					if (Cache.TreeControl != null)
						Cache.TreeControl.InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the Icon property to its default value.
		/// </summary>
		public void ResetIndicator()
		{
			Indicator = Indicator.None;
		}

		/// <summary>
		/// Gets and sets the style of checkboxes.
		/// </summary>
		[Category("Appearance")]
		[Description("Define the style of checkboxes.")]
		[DefaultValue(typeof(NodeCheckStates), "Inherit")]
		public NodeCheckStates CheckStates
		{
			get { return _checkStates; }

			set
			{
				if (_checkStates != value)
				{
					_checkStates = value;
					Cache.InvalidateSize();
					OnCheckStatesChanged();

					// Ask the tree control to redraw us
					if (Cache.TreeControl != null)
						Cache.TreeControl.InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the CheckStates property to its default value.
		/// </summary>
		public void ResetCheckStates()
		{
			CheckStates = NodeCheckStates.Inherit;
		}

		/// <summary>
		/// Gets and sets the check state of the node.
		/// </summary>
		[Category("Appearance")]
		[Description("Check state of the node.")]
		[DefaultValue(typeof(CheckState), "Unchecked")]
		public CheckState CheckState
		{
			get { return _checkState; }

			set
			{
				if (_checkState != value)
				{
					// Are we allowed to change the state?
					if (Cache.TreeControl != null)
					{
						// If not allowed to change then finish now
						if (!Cache.TreeControl.OnBeforeCheck(this))
							return;
					}
				
					_checkState = value;

					if (Cache.TreeControl != null)
					{
						// Raise event to show check state have been changed
						Cache.TreeControl.OnAfterCheck(this);
						
						// Need to redraw the nodes to reflect this change						
						Cache.TreeControl.InvalidateNodeDrawing();
					}

					OnCheckStateChanged();
				}
			}
		}

		/// <summary>
		/// Gets and sets a value if node is checked.
		/// </summary>
		[Browsable(false)]
		public bool Checked
		{
			get { return (CheckState == CheckState.Checked); }
			set { CheckState = (value ? CheckState.Checked : CheckState.Unchecked); }
		}

		/// <summary>
		/// Resets the CheckState property to its default value.
		/// </summary>
		public void ResetCheckState()
		{
			CheckState = CheckState.Unchecked;
		}

		/// <summary>
		/// Gets and sets a value indicating if the Node is visible.
		/// </summary>
		[Category("Behavior")]
		[Description("Should the Node be visible.")]
		[DefaultValue(true)]
		public bool Visible
		{
			get { return GetFlag(Flags.Visible); }

			set
			{
				if (GetFlag(Flags.Visible) != value)
				{
					SetFlag(Flags.Visible, value);
					OnVisibleChanged();

					// Ask the tree control to redraw us
					if (Cache.TreeControl != null)
						Cache.TreeControl.InvalidateNodeDrawing();
				}
			}
		}

		/// <summary>
		/// Resets the Visible property to its default value.
		/// </summary>
		public void ResetVisible()
		{
			Visible = true;
		}

		/// <summary>
		/// </summary>
		[Browsable(false)]
		public bool IsVisible
		{
			get { return Visible; }
		}

		/// <summary>
		/// Make this Node visible.
		/// </summary>
		public void Show()
		{
			Visible = true;
		}

		/// <summary>
		/// Make this Node invisible.
		/// </summary>
		public void Hide()
		{
			Visible = false;
		}

		/// <summary>
		/// Gets and sets a value indicating if the Node is expanded.
		/// </summary>
		[Category("Behavior")]
		[Description("Should the Node be expanded.")]
		[DefaultValue(false)]
		public bool Expanded
		{
			get { return GetFlag(Flags.Expanded); }

			set
			{
				// Try and apply the requested change
				if (value)
					Expand();
				else
					Collapse();
			}
		}

		/// <summary>
		/// Resets the Expanded property to its default value.
		/// </summary>
		public void ResetExpanded()
		{
			Expanded = false;
		}

		/// <summary>
		/// Gets a value indicating if the Node is expanded.
		/// </summary>
		[Browsable(false)]
		public bool IsExpanded
		{
			get { return Expanded; }
		}

		/// <summary>
		/// Expands the Node.
		/// </summary>
		public void Expand()
		{
			if (!IsExpanded)
			{
				// Are we allowed to change the state?
				if (Cache.TreeControl != null)
				{
					// If not allowed to change then finish now
					if (!Cache.TreeControl.OnBeforeExpand(this))
						return;
				}

				SetFlag(Flags.Expanded, true);

				if (Cache.TreeControl != null)
				{
					// Raise event to show expand state have been changed
					Cache.TreeControl.OnAfterExpand(this);

					// Need to redraw the nodes to reflect this change						
					Cache.TreeControl.InvalidateNodeDrawing();
				}

				OnExpandedChanged();
			}
		}

		/// <summary>
		/// Expand Node and all child nodes.
		/// </summary>
		public void ExpandAll()
		{
			// Expand this node and all child nodes recursively
			RecurseExpanded(this, true);
		}

		/// <summary>
		/// Collapses the Node.
		/// </summary>
		public void Collapse()
		{
			if (IsExpanded)
			{
				// Are we allowed to change the state?
				if (Cache.TreeControl != null)
				{
					// If not allowed to change then finish now
					if (!Cache.TreeControl.OnBeforeCollapse(this))
						return;
				}

				SetFlag(Flags.Expanded, false);

				if (Cache.TreeControl != null)
				{
					// Raise event to show expand state have been changed
					Cache.TreeControl.OnAfterCollapse(this);

					// Need to redraw the nodes to reflect this change						
					Cache.TreeControl.InvalidateNodeDrawing();
				}

				OnExpandedChanged();
			}
		}

		/// <summary>
		/// Collapse Node and all child nodes.
		/// </summary>
		public void CollapseAll()
		{
			// Collapse this node and all child nodes recursively
			RecurseExpanded(this, false);
		}

		/// <summary>
		/// Inverts the current Expanded state.
		/// </summary>
		public void Toggle()
		{
			if (IsExpanded)
				Collapse();
			else
				Expand();
		}

		/// <summary>
		/// Gets and sets a value indicating if the Node can be selected.
		/// </summary>
		[Category("Behavior")]
		[Description("Can the Node be selected.")]
		[DefaultValue(true)]
		public bool Selectable
		{
			get { return GetFlag(Flags.Selectable); }

			set
			{
				if (GetFlag(Flags.Selectable) != value)
				{
					SetFlag(Flags.Selectable, value);
					OnSelectableChanged();
				}
			}
		}

		/// <summary>
		/// Resets the Selectable property to its default value.
		/// </summary>
		public void ResetSelectable()
		{
			Selectable = true;
		}

		/// <summary>
		/// Gets a value indicating if the Node is selected.
		/// </summary>
		[Browsable(false)]
		public bool IsSelected
		{
			get 
			{ 
				// If we have a tree reference then ask it the question
				if (Cache.TreeControl != null)
					return Cache.TreeControl.IsNodeSelected(this);

				// Not in a tree, then must be false
				return false;
			}
		}

		/// <summary>
		/// Select this Node.
		/// </summary>
		public void Select()
		{
			// If we have a tree reference...
			if (Cache.TreeControl != null)
			{
				// Is there a change in state?
				if (!IsSelected)
					Cache.TreeControl.SelectNode(this, false, true);
			}
		}

		/// <summary>
		/// Deselect this Node.
		/// </summary>
		public void Deselect()
		{
			// If we have a tree reference...
			if (Cache.TreeControl != null)
			{
				// Is there a change in state?
				if (IsSelected)
					Cache.TreeControl.DeselectNode(this, false);
			}
		}
		
		/// <summary>
		/// Initiates the editing of the node label.
		/// </summary>
		public void BeginEdit()
		{
			INodeVC vc = this.VC;

			// If we have valid references...
			if ((vc != null) && (Cache.TreeControl != null))
			{
				// Request the current node label be edited
				vc.BeginEditNode(Cache.TreeControl, this);
			}
		}
		
		/// <summary>
		/// Gets the path from root node to this node.
		/// </summary>
		[Category("Data")]
		[Description("Get the path from root node to this node.")]
		[Browsable(false)]
		public string FullPath
		{
			get
			{
				// Start with our own name as the path
				string ret = Text;
				
				// If we are part of a tree hierarchy
				if (Cache.TreeControl != null)
				{
					// Cache the separator string
					string separator = Cache.TreeControl.PathSeparator;
					
					// Begin traversing upwards from our parent
					Node next = Parent;
					
					// Keep going up until we have reached the top
					while(next != null)
					{
						// Add extra path entry at start
						ret = next.Text + separator + ret;
					
						// Move up another level
						next = next.Parent;
					}
				}
								
				return ret;
			}
		}

		/// <summary>
		/// Gets or sets a unique key to associate with the node instance.
		/// </summary>
		[Category("Data")]
		[Description("User defined unique key to associate with node.")]
		[DefaultValue(null)]
		[Browsable(false)]
		public object Key
		{
			get { return _key; }
			
			set 
			{ 
				// Only interested in changes in value
				if (_key != value)
				{
					// If we have a tree reference then inform it of change
					if (Cache.TreeControl != null)
						Cache.TreeControl.NodeKeyChanged(this, _key, value);
					
					// Remember new value
					_key = value; 
					
					// Generate property changed event
					OnKeyChanged();
				}
			}
		}
		
		/// <summary>
		/// Resets the Key property to its default value.
		/// </summary>
		public void ResetKey()
		{
			Key = null;
		}

		/// <summary>
		/// Gets or sets the user object that contains node specific information.
		/// </summary>
		[Category("Data")]
		[Description("User defined data associated with node.")]
		[DefaultValue(null)]
		[Browsable(false)]
		public object Tag
		{
			get { return _tag; }
			
			set 
			{ 
				if (_tag != value)
				{
					_tag = value; 
					OnTagChanged();
				}
			}
		}

		/// <summary>
		/// Resets the Tag property to its default value.
		/// </summary>
		public void ResetTag()
		{
			Tag = null;
		}

		/// <summary>
		/// Gets the size of the node.
		/// </summary>
		[Browsable(false)]
		public Size Size
		{
			get { return Cache.Size; }
		}

		/// <summary>
		/// Gets the bounds of the node.
		/// </summary>
		[Browsable(false)]
		public Rectangle Bounds 
		{
			get { return Cache.Bounds; }
		}

		/// <summary>
		/// Gets the bounds of the node including children nodes.
		/// </summary>
		[Browsable(false)]
		public Rectangle ChildBounds 
		{
			get { return Cache.ChildBounds; }
		}

		/// <summary>
		/// Gets a value indicating if the size is dirty.
		/// </summary>
		[Browsable(false)]
		public bool IsSizeDirty
		{
			get { return Cache.IsSizeDirty; }
		}

		/// <summary>
		/// Gets the index of this node in parent collection of nodes.
		/// </summary>
		[Browsable(false)]
		public int Index
		{
			get
			{
				if (ParentNodes == null)
					return -1;
				else
					return ParentNodes.IndexOf(this);
			}
		}

		/// <summary>
		/// Removes this node from the parent node collection.
		/// </summary>
		public void Remove()
		{
			if (ParentNodes != null)
				ParentNodes.Remove(this);
		}

		/// <summary>
		/// Returns the number of child nodes.
		/// </summary>
		/// <returns></returns>
		public int GetNodeCount()
		{
			return Nodes.Count;
		}

		/// <summary>
		/// Gets reference to the first Node in this hierarchy.
		/// </summary>
		[Browsable(false)]
		public Node FirstNode
		{
			get
			{
				// We are always the first Node in our hierarchy
				return this;
			}
		}

		/// <summary>
		/// Gets reference to the first displayed Node in this hierarchy.
		/// </summary>
		[Browsable(false)]
		public Node FirstDisplayedNode
		{
			get
			{
				// Only start with ourself if we are visible
				return Visible ? this : null;
			}
		}

		/// <summary>
		/// Gets reference to the last Node in this hierarchy.
		/// </summary>
		[Browsable(false)]
		public Node LastNode
		{
			get
			{
				Node lastNode = this;

				// Are there any child nodes to process?
				if (Nodes.Count > 0)
				{
					int lastIndex = Nodes.Count - 1;

					// Ask the last node in the collection for its own last node
					lastNode = Nodes[lastIndex].Nodes.GetLastNode();

					// If last node has no children then just use the last node itself
					if (lastNode == null)
						lastNode = Nodes[lastIndex];
				}

				return lastNode;
			}
		}

		/// <summary>
		/// Gets reference to the last displayed Node in this hierarchy.
		/// </summary>
		[Browsable(false)]
		public Node LastDisplayedNode
		{
			get
			{
				// Default to ourself only if we are visible
				Node lastNode = Visible ? this : null;

				// Can we search into the children
				if ((lastNode != null) && Expanded)
				{
					// Discover the last child node
					Node lastChild = Nodes.GetLastDisplayedNode();

					if (lastChild != null)
						lastNode = lastChild;				
				}

				return lastNode;
			}
		}

		/// <summary>
		/// Gets reference to the next Node in entire hierarchy.
		/// </summary>
		[Browsable(false)]
		public Node NextNode
		{
			get
			{
				Node nextNode = null;
				
				// Do we have any child nodes?
				if (Nodes.Count > 0)
				{
					// Then we navigate down to the first child
					nextNode = Nodes.GetFirstNode();
				}
				else
				{
					// Navigate upwards until we find the next node
					Node current = this;

					// Keep searching till we reach the root
					while(current != null)
					{
						// Get our index inside our containing node collection
						int thisIndex = current.ParentNodes.IndexOf(current);

						// If not the last item in the parent collection
						if (current.ParentNodes.Count > (thisIndex + 1))
						{
							// Then the next node is our next sibling
							nextNode = current.ParentNodes[thisIndex+1];
							
							// Found it!
							break;
						}
						else
						{
							// Move upwards one level
							current = current.Parent;
						}
					}
				}

				return nextNode;
			}
		}

		/// <summary>
		/// Gets reference to the next displayed Node in entire hierarchy.
		/// </summary>
		[Browsable(false)]
		public Node NextDisplayedNode
		{
			get
			{
				// Try and navigate down to the first child
				Node nextNode = Expanded ? Nodes.GetFirstDisplayedNode() : null;

				// If there is no child to navigate into...
				if (nextNode == null)
				{
					// Track reference to parent nodes
					Node current = this;

					// Keep searching till we reach the root
					while(current != null)
					{
						// Get our index inside our containing node collection
						int thisIndex = current.ParentNodes.IndexOf(current);

						// If not the last item in the parent collection
						if (current.ParentNodes.Count > (thisIndex + 1))
						{
							// Then search forward until we find the next visible node
							for(int i=thisIndex + 1; i<current.ParentNodes.Count; i++)
							{	
								// Only interested if the node is visible
								if (current.ParentNodes[i].Visible)
								{
									nextNode = current.ParentNodes[i];
									break;
								}
							}

							// Have we found the next displayed node?
							if (nextNode != null)
								break;
						}

						// Move upwards one level
						current = current.Parent;
					}
				}

				return nextNode;
			}
		}

		/// <summary>
		/// Gets reference to the previous Node in entire hierarchy.
		/// </summary>
		[Browsable(false)]
		public Node PreviousNode
		{
			get
			{
				// Are we a child of a parent?
				if (ParentNodes != null)
				{
					// Get our index inside our containing node collection
					int thisIndex = ParentNodes.IndexOf(this);

					// Is there an element before us?
					if (thisIndex > 0)
					{
						// Grab the preceding sibling in parent collection
						Node previousNode = ParentNodes[thisIndex - 1];

						// Then return last element of the previous sibling
						return previousNode.LastNode;
					}

					// Already at end of siblings, so move up another level
					return Parent;
				}

				// Must already be at the start
				return null;
			}
		}

		/// <summary>
		/// Gets reference to the previous displayed Node in entire hierarchy.
		/// </summary>
		[Browsable(false)]
		public Node PreviousDisplayedNode
		{
			get
			{
				// Are we a child of a parent?
				if (ParentNodes != null)
				{
					// Get our index inside our containing node collection
					int thisIndex = ParentNodes.IndexOf(this);
				
					Node previousNode = null;
				
					// Find the previous Node that is visible
					for(int i=thisIndex-1; i>=0; --i)
					{
						// Only interested in node if it is visible
						if (ParentNodes[i].Visible)
						{
							// Remember the matching node
							previousNode = ParentNodes[i];
							break;
						}
					}
				
					// Did we find a valid node?
					if (previousNode != null)
					{
						// Are we allowed to navigate into the node?
						if (previousNode.Expanded)
						{
							// Then return last element of the previous sibling
							previousNode = previousNode.LastDisplayedNode;
						}
						
						// Return this previous sibling or child of sibling						
						return previousNode;
					}

					// Already at start of siblings, so move up another level
					return Parent;
				}

				// Must already be at the start
				return null;
			}
		}

		/// <summary>
		/// Creates a deep copy of the Node.
		/// </summary>
		/// <returns>New Node instance.</returns>
		public object Clone()
		{
			// Create new Node to return
			Node copy = new Node();

			// Copy across each of the fields
			copy._text = _text;
			copy._nodeFont = _nodeFont;
			copy._nodeFontBoldItalic = _nodeFontBoldItalic;
			copy._nodeFontHeight = _nodeFontHeight;
			copy._backColor = _backColor;
			copy._foreColor = _foreColor;
			copy._imageIndex = _imageIndex;
			copy._selectedImageIndex = _selectedImageIndex;
			copy._icon = _icon;
			copy._selectedIcon = _selectedIcon;
			copy._image = _image;
			copy._selectedImage = _selectedImage;
			copy._checkState = _checkState;
			copy._checkStates = _checkStates;
			copy._indicator = _indicator;
			copy._flags = _flags;
			copy._tag = _tag;
			copy._vc = _vc;
			
			// Make the copy point back at the original
			copy._original = this;
			
			// Make deep copy of children
			foreach(Node child in Nodes)
				copy.Nodes.Add((Node)child.Clone());			
		
			return copy;
		}
		
		/// <summary>
		/// Update internal fields from source Node.
		/// </summary>
		/// <param name="source">Source Node to update from.</param>
		public void UpdateInstance(Node source)
		{
			_text = source._text;
			_nodeFont = source._nodeFont;
			_nodeFontBoldItalic = source._nodeFontBoldItalic;
			_nodeFontHeight = source._nodeFontHeight;
			_backColor = source._backColor;
			_foreColor = source._foreColor;
			_imageIndex = source._imageIndex;
			_selectedImageIndex = source._selectedImageIndex;
			_icon = source._icon;
			_selectedIcon = source._selectedIcon;
			_image = source._image;
			_selectedImage = source._selectedImage;
			_checkState = source._checkState;
			_checkStates = source._checkStates;
			_indicator = source._indicator;
			_flags = source._flags;
			_tag = source._tag;
			_vc = source._vc;
		}
		
		/// <summary>
		/// Raises the TextChanged event.
		/// </summary>
		protected virtual void OnTextChanged()
		{
			if (TextChanged != null)
				TextChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the TooltipChanged event.
		/// </summary>
		protected virtual void OnTooltipChanged()
		{
			if (TooltipChanged != null)
				TooltipChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the NodeFontChanged event.
		/// </summary>
		protected virtual void OnNodeFontChanged()
		{
			if (NodeFontChanged != null)
				NodeFontChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the ForeColorChanged event.
		/// </summary>
		protected virtual void OnForeColorChanged()
		{
			if (ForeColorChanged != null)
				ForeColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the BackColorChanged event.
		/// </summary>
		protected virtual void OnBackColorChanged()
		{
			if (BackColorChanged != null)
				BackColorChanged(this, EventArgs.Empty);
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
		/// Raises the ImageIndexChanged event.
		/// </summary>
		protected virtual void OnImageIndexChanged()
		{
			if (ImageIndexChanged != null)
				ImageIndexChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the IndicatorChanged event.
		/// </summary>
		protected virtual void OnIndicatorChanged()
		{
			if (IndicatorChanged != null)
				IndicatorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the IconChanged event.
		/// </summary>
		protected virtual void OnIconChanged()
		{
			if (IconChanged != null)
				IconChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the SelectedIconChanged event.
		/// </summary>
		protected virtual void OnSelectedIconChanged()
		{
			if (SelectedIconChanged != null)
				SelectedIconChanged(this, EventArgs.Empty);
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
		/// Raises the SelectedImageChanged event.
		/// </summary>
		protected virtual void OnSelectedImageChanged()
		{
			if (SelectedImageChanged != null)
				SelectedImageChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the CheckStateChanged event.
		/// </summary>
		protected virtual void OnCheckStateChanged()
		{
			INodeVC vc = this.VC;

			// If we have a valid view controller and parent control
			if ((vc != null) && (Cache.TreeControl != null))
			{
				// Inform controller of change in check state
				vc.NodeCheckStateChanged(Cache.TreeControl, this);
			}

			if (CheckStateChanged != null)
				CheckStateChanged(this, EventArgs.Empty);
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
		/// Raises the VisibleChanged event.
		/// </summary>
		protected virtual void OnVisibleChanged()
		{
			INodeVC vc = this.VC;

			// If we have a valid view controller and parent control
			if ((vc != null) && (Cache.TreeControl != null))
			{
				// Inform controller of change in expanded state
				vc.NodeVisibleChanged(Cache.TreeControl, this);
			}

			if (VisibleChanged != null)
				VisibleChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the ExpandedChanged event.
		/// </summary>
		protected virtual void OnExpandedChanged()
		{
			INodeVC vc = this.VC;

			// If we have a valid view controller and parent control
			if ((vc != null) && (Cache.TreeControl != null))
			{
				// Inform controller of change in expanded state
				vc.NodeExpandedChanged(Cache.TreeControl, this);
			}

			if (ExpandedChanged != null)
				ExpandedChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the SelectableChanged event.
		/// </summary>
		protected virtual void OnSelectableChanged()
		{
			INodeVC vc = this.VC;

			// If we have a valid view controller and parent control
			if ((vc != null) && (Cache.TreeControl != null))
			{
				// Inform controller of change in expanded state
				vc.NodeSelectableChanged(Cache.TreeControl, this);
			}

			if (SelectableChanged != null)
				SelectableChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the KeyChanged event.
		/// </summary>
		protected virtual void OnKeyChanged()
		{
			if (KeyChanged != null)
				KeyChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the TagChanged event.
		/// </summary>
		protected virtual void OnTagChanged()
		{
			if (TagChanged != null)
				TagChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the VCChanged event.
		/// </summary>
		protected virtual void OnVCChanged()
		{
			if (VCChanged != null)
				VCChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Gets a value indicating if the size of the node needs recalculating.
		/// </summary>
		protected internal NodeCache Cache
		{
			get { return _cache; }
		}

		internal Font GetNodeFont()
		{
			return _nodeFont;
		}

		internal Font GetNodeBoldItalicFont()
		{
			if (_nodeFontBoldItalic != null)
				_nodeFontBoldItalic = new Font(_nodeFont, FontStyle.Bold | FontStyle.Italic);

			return _nodeFontBoldItalic;
		}

		internal Color GetNodeForeColor()
		{
			return _foreColor;
		}

		internal void SetTreeControl(TreeControl tc)
		{
			// And we currently have a valid tree control reference
			if (Cache.TreeControl != null)
			{
				// Tell the tree control the node is removed
				Cache.TreeControl.NodeRemoved(this);
			}

			// If we are being added to a tree control
			if (tc != null)
			{
				// Tell the tree control the node is added
				tc.NodeAdded(this);
			}
		
			// Cache the parent references
			Cache.TreeControl = tc;

			// Recurse to tell each of the child nodes as well
			Nodes.SetTreeControl(tc);
		}

		internal void SetReferences(Node parentNode, NodeCollection parentNodes, TreeControl tc)
		{
			// When reference is set the node cannot be in removing state
			Removing = false;

			// And we currently have a valid tree control reference
			if (Cache.TreeControl != null)
			{
				// Tell the tree control the node is removed
				Cache.TreeControl.NodeRemoved(this);
			}

			// If we are being added to a tree control
			if (tc != null)
			{
				// Tell the tree control the node is added
				tc.NodeAdded(this);
			}
		
			// Cache the parent references
			_parentNodes = parentNodes; 
			Cache.ParentNode = parentNode; 
			Cache.TreeControl = tc;

			// Recurse to tell each of the child nodes as well
			Nodes.SetTreeControl(tc);
		}

		internal int GetNodeFontHeight()
		{
			// If inheriting the Font then ask for inherited value
			if (_nodeFont == null)
				return Cache.TreeControl.GetFontHeight();
			else
			{
				// First time around we cache the height
				if (_nodeFontHeight == -1)
					_nodeFontHeight = _nodeFont.Height;

				return _nodeFontHeight;
			}
		}

		internal int GetNodeGroupFontHeight()
		{
			// If inheriting the Font then ask for inherited value
			if (_nodeFont == null)
				return Cache.TreeControl.GetGroupFontHeight();
			else
			{
				// First time around we cache the height
				if (_nodeFontHeight == -1)
					_nodeFontHeight = _nodeFont.Height;

				return _nodeFontHeight;
			}
		}
		
		internal bool Removing
		{
			get { return GetFlag(Flags.Removing); }
			set { SetFlag(Flags.Removing, value); }
		}

		internal Node Original
		{
			get { return _original; }
			set { _original = value; }
		}

		private void RecurseExpanded(Node n, bool expand)
		{
			// Expand/Collapse the node itself
			if (expand)
				n.Expand();
			else
				n.Collapse();	

			// Do the same for all its children
			foreach(Node child in n.Nodes)
				RecurseExpanded(child, expand);
		}

		private bool GetFlag(Flags flag)
		{
			return ((_flags & flag) == flag);
		}
		
		private void SetFlag(Flags flag, bool val)
		{
			if (val)
				_flags |= flag;
			else
				_flags = (Flags)((int)_flags & ~(int)flag);
		}
	}
}
