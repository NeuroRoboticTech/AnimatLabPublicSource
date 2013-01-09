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
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.ComponentModel;
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Configurable title bar for presenting user with header information.
	/// </summary>
	[ToolboxBitmap(typeof(TitleBar))]
	[DefaultProperty("Text")]
	public class TitleBar : Control
	{
		// Static fields
		private static ImageList _arrows;

		// Instance fields - Properties
		private int _padLeft;
		private int _padRight;
		private int _padTop;
		private int _padBottom;
		private int _imageToTextGap;
		private int _edgeToImageGap;
		private int _edgeToArrowGap;
		private int _imageIndex;
		private bool _active;
        private bool _painting;
        private bool _apply2007ClearType;
        private bool _applyMediaPlayerClearType;
        private Icon _icon;
		private Image _image;
		private Image _inactiveImage;
		private string _preText;
		private string _postText;
		private string _preSeparator;
		private string _postSeparator;
		private LayoutDirection _direction;
		private ImageList _imageList;
		private ArrowButton _arrowButton;
		private ActAsButton _actAsButton;
		private Color _gradientActiveColor;
		private Color _gradientInactiveColor;
		private Color _inactiveBackColor;
		private Color _inactiveForeColor;
		private Color _mouseOverColor;
		private ImageAttributes _imageAttr;
		private ImageAlignment _imageAlignment;
		private ImageAlignment _arrowAlignment;
		private StringAlignment _textAlignment;
		private StringAlignment _lineAlignment;
		private GradientColoring _gradientColoring;
		private GradientDirection _gradientDirection;
		private TextRenderingHint _textRenderingHint;
		private TitleButtonCollection _titleButtons;
		private ColorDetails _colorDetails;
		private VisualStyle _style;
        private VisualStyle _externalStyle;
		
		// Instance fields - State
		private bool _mouseOver;
		private bool _mouseOverArrow;
		private bool _mouseDown;
		private bool _mouseDownArrow;
    
		/// <summary>
		/// Occurs when the control or arrow has been clicked.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler ButtonClick;

		/// <summary>
		/// Occurs when the Active property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler ActiveChanged;

		/// <summary>
		/// Occurs when the Style property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler VisualStyleChanged;

		/// <summary>
		/// Occurs when the ActAsButton property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler ActAsButtonChanged;

		/// <summary>
		/// Occurs when the InactiveBackColor property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler InactiveBackColorChanged;

		/// <summary>
		/// Occurs when the InactiveForeColor property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler InactiveForeColorChanged;

		/// <summary>
		/// Occurs when the GradientActiveColor property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler GradientActiveColorChanged;

		/// <summary>
		/// Occurs when the GradientInactiveColor property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler GradientInactiveColorChanged;

		/// <summary>
		/// Occurs when the GradientDirection property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler GradientDirectionChanged;

		/// <summary>
		/// Occurs when the GradientColoring property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler GradientColoringChanged;
		
		/// <summary>
		/// Occurs when the TextAlignment property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler TextAlignmentChanged;
    
		/// <summary>
		/// Occurs when the TextAlignment property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler LineAlignmentChanged;

		/// <summary>
		/// Occurs when the PreText property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler PreTextChanged;

		/// <summary>
		/// Occurs when the PostText property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler PostTextChanged;

		/// <summary>
		/// Occurs when the PreSeparator property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler PreSeparatorChanged;

		/// <summary>
		/// Occurs when the PostSeparator property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler PostSeparatorChanged;

		/// <summary>
		/// Occurs when the PadLeft property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler PadLeftChanged;

		/// <summary>
		/// Occurs when the PadRight property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler PadRightChanged;

		/// <summary>
		/// Occurs when the PadTop property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler PadTopChanged;

		/// <summary>
		/// Occurs when the PadBottom property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler PadBottomChanged;
		
		/// <summary>
		/// Occurs when the PadBottom property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler ImageToTextGapChanged;

		/// <summary>
		/// Occurs when the EdgeToImageGap property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler EdgeToImageGapChanged;
		
		/// <summary>
		/// Occurs when the EdgeToArrowGap property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler EdgeToArrowGapChanged;

		/// <summary>
		/// Occurs when the Image property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler ImageChanged;

		/// <summary>
		/// Occurs when the Icon property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler IconChanged;

		/// <summary>
		/// Occurs when the ImageIndex property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler ImageIndexChanged;

		/// <summary>
		/// Occurs when the ImageList property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler ImageListChanged;

		/// <summary>
		/// Occurs when the ImageAlignment property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler ImageAlignmentChanged;

		/// <summary>
		/// Occurs when the ArrowAlignmen property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler ArrowAlignmentChanged;

		/// <summary>
		/// Occurs when the Direction property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler DirectionChanged;

		/// <summary>
		/// Occurs when the Direction property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler ArrowButtonChanged;

		/// <summary>
		/// Occurs when the TextRenderingHint property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler TextRenderingHintChanged;

		/// <summary>
		/// Occurs when the MouseOverColor property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler MouseOverColorChanged;

		static TitleBar()
		{
			// Create image list to hold the arrows
			_arrows = new ImageList();
			_arrows.ColorDepth = ColorDepth.Depth4Bit;
			_arrows.ImageSize = new Size(10, 10);

			// Load each of the images
			Image up = ResourceHelper.LoadBitmap(typeof(TitleBar), "Crownwood.DotNetMagic.Controls.TitleBar.ImageUp.bmp", Point.Empty);
			Image down = ResourceHelper.LoadBitmap(typeof(TitleBar), "Crownwood.DotNetMagic.Controls.TitleBar.ImageDown.bmp", Point.Empty);
			Image left = ResourceHelper.LoadBitmap(typeof(TitleBar), "Crownwood.DotNetMagic.Controls.TitleBar.ImageLeft.bmp", Point.Empty);
			Image right = ResourceHelper.LoadBitmap(typeof(TitleBar), "Crownwood.DotNetMagic.Controls.TitleBar.ImageRight.bmp", Point.Empty);
			Image pinned = ResourceHelper.LoadBitmap(typeof(TitleBar), "Crownwood.DotNetMagic.Controls.TitleBar.Pinned.bmp", Point.Empty);
			Image unpinned = ResourceHelper.LoadBitmap(typeof(TitleBar), "Crownwood.DotNetMagic.Controls.TitleBar.Unpinned.bmp", Point.Empty);

			// Add images to the imagelist
			_arrows.Images.Add(up);
			_arrows.Images.Add(down);
			_arrows.Images.Add(left);
			_arrows.Images.Add(right);
			_arrows.Images.Add(pinned);
			_arrows.Images.Add(unpinned);
		}

		/// <summary>
		/// Initializes a new instance of the TitleBar class.
		/// </summary>
		public TitleBar()
		{
			// NAG processing
			NAG.NAG_Start();

			// Prevent drawing flicker by blitting from memory in WM_PAINT
            SetStyle(ControlStyles.OptimizedDoubleBuffer | 
					 ControlStyles.AllPaintingInWmPaint |
					 ControlStyles.UserPaint, true);

			// Should not be allowed to select this control
			SetStyle(ControlStyles.Selectable, false);

			// Mouse is not over the control
			_mouseOver = false;
			_mouseOverArrow = false;
			_mouseDown = false;
			_mouseDownArrow = false;

			// Create objects
			_imageAttr = new ImageAttributes();
			_colorDetails = new ColorDetails();
			_titleButtons = new TitleButtonCollection();

			// Hook into changes in title buttnos collection
			_titleButtons.Inserted += new CollectionChange(OnTitleButtonInserted);
			_titleButtons.Removed += new CollectionChange(OnTitleButtonRemoved);
			_titleButtons.Clearing += new CollectionClear(OnTitleButtonClearing);

			ResetStyle();
			ResetActive();
			ResetActAsButton();
			ResetDirection();
			ResetBackColor();
			ResetForeColor();
			ResetInactiveBackColor();
			ResetInactiveForeColor();
			ResetGradientActiveColor();
			ResetGradientInactiveColor();
			ResetGradientDirection();
			ResetGradientColoring();
			ResetMouseOverColor();
			ResetTextAlignment();
			ResetLineAlignment();
			ResetPreText();
			ResetPostText();
			ResetPreSeparator();
			ResetPostSeparator();
			ResetPadLeft();
			ResetPadRight();
			ResetPadTop();
			ResetPadBottom();
			ResetImageToTextGap();
			ResetEdgeToImageGap();
			ResetEdgeToArrowGap();
			ResetIcon();
			ResetImage();
			ResetImageIndex();
			ResetImageList();
			ResetImageAlignment();
			ResetArrowButton();
			ResetArrowAlignment();
			ResetTextRenderingHint();
            ResetApply2007ClearType();
		}

		/// <summary>
		/// Dispose of instance resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Color details has resources that need releasing
				_colorDetails.Dispose();
			}
			
			base.Dispose (disposing);
		}

		/// <summary>
		/// Gets and sets the background color used when drawing the control.
		/// </summary>
		[DefaultValue(typeof(Color), "ActiveCaption")]
		public new Color BackColor
		{
			get { return base.BackColor; }
			
			set
			{
				if (base.BackColor != value)
				{
					base.BackColor = value;
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the BackColor property to its default value.
		/// </summary>
		public new void ResetBackColor()
		{
			BackColor = SystemColors.ActiveCaption;
		}
        
		/// <summary>
		/// Gets and sets the foreground color is used when drawing text.
		/// </summary>
		[DefaultValue(typeof(Color), "ActiveCaptionText")]
		public new Color ForeColor
		{
			get { return base.ForeColor; }
			
			set
			{
				if (base.ForeColor != value)
				{
					base.ForeColor = value;
					Invalidate();
				}
			}
		}
        
		/// <summary>
		/// Resets the ForeColor property to its default value.
		/// </summary>
		public new void ResetForeColor()
		{
			ForeColor = SystemColors.ActiveCaptionText;
		}

		/// <summary>
		/// Gets or sets which edge of the parent container a control is docked to.
		/// </summary>
		[DefaultValue(typeof(DockStyle), "None")]
		public new DockStyle Dock
		{
			get { return base.Dock; }

			set
			{
				if (base.Dock != value)
				{
					base.Dock = value;

					// Update the direction to reflect docking position
					switch(value)
					{
						case DockStyle.Top:
						case DockStyle.Bottom:
						case DockStyle.None:
							Direction = LayoutDirection.Horizontal;
							break;
						case DockStyle.Left:
						case DockStyle.Right:
							Direction = LayoutDirection.Vertical;
							break;
					}
				}
			}
		}

		/// <summary>
		/// Resets the Dock property to its default value.
		/// </summary>
		public void ResetDock()
		{
			Dock = DockStyle.None;
		}

		/// <summary>
		/// Gets the collection of title buttons.
		/// </summary>
		[Category("Appearance")]
		[Description("Collection of buttons to show on titlebar.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TitleButtonCollection TitleButtons
		{
			get { return _titleButtons; }
		}

		/// <summary>
		/// Gets or sets the visual style of the control.
		/// </summary>
		[Category("Appearance")]
		[Description("Visual style of the control.")]
        [DefaultValue(typeof(VisualStyle), "Office2007Blue")]
		[RefreshProperties(RefreshProperties.All)]
		public VisualStyle Style
		{
            get { return _externalStyle; }
			
			set
			{
                if (_externalStyle != value)
				{
                    _externalStyle = value;
					_style = ColorHelper.ValidateStyle(value);
		
					// Reset other attributes for the style
					ResetGradientDirection();
					ResetGradientColoring();

					// Pass the style request onto each child button
					foreach(TitleButton tb in _titleButtons)
						if (tb.Control != null)
                            tb.Control.Style = _externalStyle;

					OnVisualStyleChanged();
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
		/// Gets and sets a value indicating if control should act like a button.
		/// </summary>
		[Category("Behavior")]
		[Description("Should control act like a button.")]
		[DefaultValue(typeof(ActAsButton), "JustArrow")]
		public ActAsButton ActAsButton
		{
			get { return _actAsButton; }
			
			set
			{
				if (_actAsButton != value)
				{
					_actAsButton = value;
					OnActAsButtonChanged();
					Invalidate();
				}
			}
		}
		
		/// <summary>
		/// Resets the ActAsButton property to its default value.
		/// </summary>
		public void ResetActAsButton()
		{
			ActAsButton = ActAsButton.JustArrow;
		}

		/// <summary>
		/// Gets and sets the direction that text and images are positioned.
		/// </summary>
		[Category("Appearance")]
		[Description("Direction that text and images are positioned.")]
		[DefaultValue(typeof(LayoutDirection), "Horizontal")]
		public LayoutDirection Direction
		{
			get { return _direction; }
			
			set
			{
				if (_direction != value)
				{
					_direction = value;
					OnDirectionChanged();
					Invalidate();
				}
			}
		}
		
		/// <summary>
		/// Resets the Direction property to its default value.
		/// </summary>
		public void ResetDirection()
		{
			Direction = LayoutDirection.Horizontal;
		}

		/// <summary>
		/// Gets and sets which (if any) arrow button should be displayed.
		/// </summary>
		[Category("Appearance")]
		[Description("Which (if any) arrow button should be displayed.")]
		[DefaultValue(typeof(ArrowButton), "None")]
		public ArrowButton ArrowButton
		{
			get { return _arrowButton; }
			
			set
			{
				if (_arrowButton != value)
				{
					_arrowButton = value;
					OnArrowButtonChanged();
					Invalidate();
				}
			}
		}
		
		/// <summary>
		/// Resets the ArrowButton property to its default value.
		/// </summary>
		public void ResetArrowButton()
		{
			ArrowButton = ArrowButton.None;
		}

		/// <summary>
		/// Gets and sets the background color used when drawing the control inactive.
		/// </summary>
		[Category("Appearance")]
		[Description("Background color used when drawing the control inactive.")]
		[DefaultValue(typeof(Color), "InactiveCaption")]
		public Color InactiveBackColor
		{
			get { return _inactiveBackColor; }
			
			set
			{
				if (_inactiveBackColor != value)
				{
					_inactiveBackColor = value;
					OnInactiveBackColorChanged();
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the InactiveBackColor property to its default value.
		/// </summary>
		public void ResetInactiveBackColor()
		{
			InactiveBackColor = SystemColors.InactiveCaption;
		}
		
		/// <summary>
		/// Gets and sets the policy used for drawing background.
		/// </summary>
		[Category("Appearance")]
		[Description("Policy used to draw the background.")]
		public GradientDirection GradientDirection
		{
			get { return _gradientDirection; }
			
			set
			{
				if (_gradientDirection != value)
				{
					_gradientDirection = value;
					OnGradientDirectionChanged();
					Invalidate();
				}
			}
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
        /// Gets and sets a value indicating if the Media Player style text should use ClearType.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(true)]
        [Description("Should the Media Player styles draw text using ClearType.")]
        public bool ApplyMediaPlayerClearType
        {
            get { return _applyMediaPlayerClearType; }

            set
            {
                if (_applyMediaPlayerClearType != value)
                {
                    _applyMediaPlayerClearType = value;
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

        private bool ShouldSerializeGradientDirection()
		{
            switch (_style)
            {
                case VisualStyle.IDE2005:
                case VisualStyle.Office2003:
                case VisualStyle.Office2007Blue:
                case VisualStyle.Office2007Silver:
                case VisualStyle.Office2007Black:
                case VisualStyle.MediaPlayerBlue:
                case VisualStyle.MediaPlayerOrange:
                case VisualStyle.MediaPlayerPurple:
                    return (GradientDirection != GradientDirection.TopToBottom);
                default:
                    return (GradientDirection != GradientDirection.None);
            }
		}

		/// <summary>
		/// Resets the GradientDirection property to its default value.
		/// </summary>
		public void ResetGradientDirection()
		{
            switch (_style)
            {
                case VisualStyle.IDE2005:
                case VisualStyle.Office2003:
                case VisualStyle.Office2007Blue:
                case VisualStyle.Office2007Silver:
                case VisualStyle.Office2007Black:
                case VisualStyle.MediaPlayerBlue:
                case VisualStyle.MediaPlayerOrange:
                case VisualStyle.MediaPlayerPurple:
                    GradientDirection = GradientDirection.TopToBottom;
                    break;
                default:
    				GradientDirection = GradientDirection.None;
                    break;
            }
		}

		/// <summary>
		/// Gets and sets colors used when drawing a gradient background.
		/// </summary>
		[Category("Appearance")]
		[Description("Determine colors used when drawing a gradient background.")]
		public GradientColoring GradientColoring
		{
			get { return _gradientColoring; }
			
			set
			{
				if (_gradientColoring != value)
				{
					_gradientColoring = value;
					OnGradientColoringChanged();
					Invalidate();
				}
			}
		}

		private bool ShouldSerializeGradientColoring()
		{
			return (GradientColoring != GradientColoring.LightBackToBack);
		}

		/// <summary>
		/// Resets the GradientColoring property to its default value.
		/// </summary>
		public void ResetGradientColoring()
		{
			GradientColoring = GradientColoring.LightBackToDarkBack;
		}

		/// <summary>
		/// Gets and sets colors used when drawing a gradient background.
		/// </summary>
		[Category("Appearance")]
		[Description("Determine colors used when drawing a gradient background.")]
		[DefaultValue(typeof(Color), "Empty")]
		public Color MouseOverColor
		{
			get { return _mouseOverColor; }
			
			set
			{
				if (_mouseOverColor != value)
				{
					_mouseOverColor = value;
					OnMouseOverColorChanged();
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the GradientColoring property to its default value.
		/// </summary>
		public void ResetMouseOverColor()
		{
			MouseOverColor = Color.Empty;
		}

		/// <summary>
		/// The text color used when drawing the control inactive.
		/// </summary>
		[Category("Appearance")]
		[Description("Text color used when drawing the control inactive.")]
		[DefaultValue(typeof(Color), "InactiveCaptionText")]
		public Color InactiveForeColor
		{
			get { return _inactiveForeColor; }
			
			set
			{
				if (_inactiveForeColor != value)
				{
					_inactiveForeColor = value;
					OnInactiveForeColorChanged();
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the InactiveForeColor property to its default value.
		/// </summary>
		public void ResetInactiveForeColor()
		{
			InactiveForeColor = SystemColors.InactiveCaptionText;
		}

		/// <summary>
		/// Gets and sets the second color available for active gradients.
		/// </summary>
		[Category("Appearance")]
		[Description("Second color available for active gradients.")]
		public Color GradientActiveColor
		{
			get { return _gradientActiveColor; }
			
			set
			{
				if (_gradientActiveColor != value)
				{
					_gradientActiveColor = value;
					OnGradientActiveColorChanged();
					Invalidate();
				}
			}
		}

		private bool ShouldSerializeGradientActiveColor()
		{
			return (GradientActiveColor != GetGradientActiveColor());
		}

		/// <summary>
		/// Resets the GradientActiveColor property to its default value.
		/// </summary>
		public void ResetGradientActiveColor()
		{
			GradientActiveColor = GetGradientActiveColor();
		}

		/// <summary>
		/// Gets and sets the second color available for inactive gradients.
		/// </summary>
		[Category("Appearance")]
		[Description("Second color available for inactive gradients.")]
		public Color GradientInactiveColor
		{
			get { return _gradientInactiveColor; }
			
			set
			{
				if (_gradientInactiveColor != value)
				{
					_gradientInactiveColor = value;
					OnGradientInactiveColorChanged();
					Invalidate();
				}
			}
		}

		private bool ShouldSerializeGradientInactiveColor()
		{
			return (GradientInactiveColor != GetGradientInactiveColor());
		}

		/// <summary>
		/// Resets the GradientInactiveColor property to its default value.
		/// </summary>
		public void ResetGradientInactiveColor()
		{
			GradientInactiveColor = GetGradientInactiveColor();
		}

		/// <summary>
		/// Gets and sets where the text is postitioned across the control.
		/// </summary>
		[Category("Appearance")]
		[Description("Determine where the text is postitioned across.")]
		[DefaultValue(typeof(StringAlignment), "Near")]
		public StringAlignment TextAlignment
		{
			get { return _textAlignment; }
			
			set
			{
				if (_textAlignment != value)
				{
					_textAlignment = value;
					OnTextAlignmentChanged();
					Invalidate();
				}
			}
		}
		
		/// <summary>
		/// Resets the TextAlignment property to its default value.
		/// </summary>
		public void ResetTextAlignment()
		{
			TextAlignment = StringAlignment.Near;
		}

		/// <summary>
		/// Gets and sets where items are drawn down the control.
		/// </summary>
		[Category("Appearance")]
		[Description("Determine where items are postitioned downwards.")]
		[DefaultValue(typeof(StringAlignment), "Center")]
		public StringAlignment LineAlignment
		{
			get { return _lineAlignment; }
			
			set
			{
				if (_lineAlignment != value)
				{
					_lineAlignment = value;
					OnLineAlignmentChanged();
					Invalidate();
				}
			}
		}
		
		/// <summary>
		/// Resets the LineAlignment property to its default value.
		/// </summary>
		public void ResetLineAlignment()
		{
			LineAlignment = StringAlignment.Center;
		}

		/// <summary>
		/// Gets and sets the text to prefix the title with.
		/// </summary>
		[Category("Appearance")]
		[Description("Text to prefix the title.")]
		[DefaultValue("")]
		[Localizable(true)]
		public string PreText
		{
			get { return _preText; }
			
			set
			{
				if (_preText != value)
				{
					_preText = value;
					OnPreTextChanged();
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the PreText property to its default value.
		/// </summary>
		public void ResetPreText()
		{
			PreText = string.Empty;
		}
		
		/// <summary>
		/// Gets and sets the text to postfix the title with.
		/// </summary>
		[Category("Appearance")]
		[Description("Text to postfix the title.")]
		[DefaultValue("")]
		[Localizable(true)]
		public string PostText
		{
			get { return _postText; }
			
			set
			{
				if (_postText != value)
				{
					_postText = value;
					OnPostTextChanged();
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the PostText property to its default value.
		/// </summary>
		public void ResetPostText()
		{
			PostText = string.Empty;
		}

		/// <summary>
		/// Gets and sets the separator between PreText and Text.
		/// </summary>
		[Category("Appearance")]
		[Description("Separator between PreText and Text.")]
		[DefaultValue(" - ")]
		[Localizable(true)]
		public string PreSeparator
		{
			get { return _preSeparator; }
			
			set
			{
				if (_preSeparator != value)
				{
					_preSeparator = value;
					OnPreSeparatorChanged();
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the PostSeparator property to its default value.
		/// </summary>
		public void ResetPreSeparator()
		{
			PreSeparator = " - ";
		}

		/// <summary>
		/// Gets and sets the separator between Text and PostText.
		/// </summary>
		[Category("Appearance")]
		[Description("Separator between Text and PostText.")]
		[DefaultValue(" - ")]
		[Localizable(true)]
		public string PostSeparator
		{
			get { return _postSeparator; }
			
			set
			{
				if (_postSeparator != value)
				{
					_postSeparator = value;
					OnPostSeparatorChanged();
					Invalidate();
				}
			}
		}
		/// <summary>
		/// Resets the PostSeparator property to its default value.
		/// </summary>
		public void ResetPostSeparator()
		{
			PostSeparator = " - ";
		}

		/// <summary>
		/// Gets and sets the rendering hint to use when drawing text.
		/// </summary>
		[Category("Appearance")]
		[Description("Specifies the rendering hint to use when drawing text.")]
		[DefaultValue(typeof(TextRenderingHint), "SystemDefault")]		
		public TextRenderingHint TextRenderingHint
		{
			get { return _textRenderingHint; }
			
			set
			{
				if (_textRenderingHint != value)
				{
					_textRenderingHint = value;
					OnTextRenderingHintChanged();
					Invalidate();
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
		/// Gets and sets the gap between left edge and start of drawn items.
		/// </summary>
		[Category("Appearance")]
		[Description("Gap between left edge and start of drawn items.")]
		[DefaultValue(3)]		
		public int PadLeft
		{
			get { return _padLeft; }
			
			set
			{
				if (_padLeft != value)
				{
					_padLeft = value;
					OnPadLeftChanged();
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the PadLeft property to its default value.
		/// </summary>
		public void ResetPadLeft()
		{
			PadLeft = 3;
		}

		/// <summary>
		/// Gets and sets the gap between right edge and end of drawn items.
		/// </summary>
		[Category("Appearance")]
		[Description("Gap between right edge and end of drawn items.")]
		[DefaultValue(3)]		
		public int PadRight
		{
			get { return _padRight; }
			
			set
			{
				if (_padRight != value)
				{
					_padRight = value;
					OnPadRightChanged();
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the PadRight property to its default value.
		/// </summary>
		public void ResetPadRight()
		{
			PadRight = 3;
		}

		/// <summary>
		/// Gets and sets the gap between top edge and start of drawn items.
		/// </summary>
		[Category("Appearance")]
		[Description("Gap between top edge and start of drawn items.")]
		[DefaultValue(3)]		
		public int PadTop
		{
			get { return _padTop; }
			
			set
			{
				if (_padTop != value)
				{
					_padTop = value;
					OnPadTopChanged();
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the PadTop property to its default value.
		/// </summary>
		public void ResetPadTop()
		{
			PadTop = 3;
		}

		/// <summary>
		/// Gets and sets the gap between bottom edge and end of drawn items.
		/// </summary>
		[Category("Appearance")]
		[Description("Gap between bottom edge and end of drawn items.")]
		[DefaultValue(3)]		
		public int PadBottom
		{
			get { return _padBottom; }
			
			set
			{
				if (_padBottom != value)
				{
					_padBottom = value;
					OnPadBottomChanged();
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the PadBottom property to its default value.
		/// </summary>
		public void ResetPadBottom()
		{
			PadBottom = 3;
		}

		/// <summary>
		/// Gets and sets the gap between image and start of text.
		/// </summary>
		[Category("Appearance")]
		[Description("Gap between image and start of text.")]
		[DefaultValue(3)]		
		public int ImageToTextGap
		{
			get { return _imageToTextGap; }
			
			set
			{
				if (_imageToTextGap != value)
				{
					_imageToTextGap = value;
					OnImageToTextGapChanged();
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the ImageToTextGap property to its default value.
		/// </summary>
		public void ResetImageToTextGap()
		{
			ImageToTextGap = 3;
		}

		/// <summary>
		/// Gets and sets the gap between edge and start of image.
		/// </summary>
		[Category("Appearance")]
		[Description("Gap between edge and start of image.")]
		[DefaultValue(0)]		
		public int EdgeToImageGap
		{
			get { return _edgeToImageGap; }
			
			set
			{
				if (_edgeToImageGap != value)
				{
					_edgeToImageGap = value;
					OnEdgeToImageGapChanged();
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the EdgeToImageGap property to its default value.
		/// </summary>
		public void ResetEdgeToImageGap()
		{
			EdgeToImageGap = 0;
		}

		/// <summary>
		/// Gets and sets the gap between edge and start of arrow.
		/// </summary>
		[Category("Appearance")]
		[Description("Gap between edge and start of arrow.")]
		[DefaultValue(3)]		
		public int EdgeToArrowGap
		{
			get { return _edgeToArrowGap; }
			
			set
			{
				if (_edgeToArrowGap != value)
				{
					_edgeToArrowGap = value;
					OnEdgeToArrowGapChanged();
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the EdgeToArrowGap property to its default value.
		/// </summary>
		public void ResetEdgeToArrowGap()
		{
			EdgeToArrowGap = 3;
		}

		/// <summary>
		/// Gets and sets the icon to draw in the title.
		/// </summary>
		[Category("Appearance")]
		[Description("Icon to draw in the title.")]
		[DefaultValue(null)]
		public Icon Icon
		{
			get { return _icon; }
			
			set
			{
				if (_icon != value)
				{
					_icon = value;
					OnIconChanged();
					Invalidate();
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
		/// Gets and sets the image to draw in title.
		/// </summary>
		[Category("Appearance")]
		[Description("Image to draw in the title.")]
		[DefaultValue(null)]
		public Image Image
		{
			get { return _image; }
			
			set
			{
				if (_image != value)
				{
					_image = value;
					_inactiveImage = null;
					OnImageChanged();
					Invalidate();
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
		/// Gets and sets the source of images associated with ImageIndex.
		/// </summary>
		[Category("Appearance")]
		[Description("Source of images associated with ImageIndex.")]
		[DefaultValue(null)]
		public ImageList ImageList
		{
			get { return _imageList; }
			
			set
			{
				if (_imageList != value)
				{
					_imageList = value;
					OnImageListChanged();
					Invalidate();
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
		/// Gets and sets the position of image/icon.
		/// </summary>
		[Category("Appearance")]
		[Description("Position of image/icon.")]
		[DefaultValue(typeof(ImageAlignment), "Near")]
		public ImageAlignment ImageAlignment
		{
			get { return _imageAlignment; }
			
			set
			{
				if (_imageAlignment != value)
				{
					_imageAlignment = value;
					OnImageAlignmentChanged();
					Invalidate();
				}
			}
		}
		
		/// <summary>
		/// Resets the ImageAlignment property to its default value.
		/// </summary>
		public void ResetImageAlignment()
		{
			ImageAlignment = ImageAlignment.Near;
		}

		/// <summary>
		/// Gets and sets the position of arrow.
		/// </summary>
		[Category("Appearance")]
		[Description("Position of arrow.")]
		[DefaultValue(typeof(ImageAlignment), "Far")]
		public ImageAlignment ArrowAlignment
		{
			get { return _arrowAlignment; }
			
			set
			{
				if (_arrowAlignment != value)
				{
					_arrowAlignment = value;
					OnArrowAlignmentChanged();
					Invalidate();
				}
			}
		}
		
		/// <summary>
		/// Resets the ArrowAlignment property to its default value.
		/// </summary>
		public void ResetArrowAlignment()
		{
			ArrowAlignment = ImageAlignment.Far;
		}

		/// <summary>
		/// Gets and sets the index into associated ImageList.
		/// </summary>
		[Category("Appearance")]
		[Description("Index into associated ImageList.")]
		[DefaultValue(0)]
		public int ImageIndex
		{
			get { return _imageIndex; }
			
			set
			{
				if (_imageIndex != value)
				{
					_imageIndex = value;
					OnImageIndexChanged();
					Invalidate();
				}
			}
		}
		
		/// <summary>
		/// Resets the ImageIndex property to its default value.
		/// </summary>
		public void ResetImageIndex()
		{
			ImageIndex = 0;
		}

		/// <summary>
		/// Gets and sets a value indicating if control should be drawn active.
		/// </summary>
		[Category("Appearance")]
		[Description("Value indicating if control should be drawn active.")]
		[DefaultValue(true)]		
		public bool Active
		{
			get { return _active; }
			
			set
			{
				if (_active != value)
				{
					_active = value;
					OnActiveChanged();
					Invalidate();
				}
			}
		}
		
		/// <summary>
		/// Resets the Active property to its default value.
		/// </summary>
		public void ResetActive()
		{
			Active = true;
		}
		
		/// <summary>
		/// Raise the Click event.
		/// </summary>
		public void PerformButtonClick()
		{
			OnButtonClick();
		}

		/// <summary>
		/// Raises the ButtonClick event.
		/// </summary>
		protected virtual void OnButtonClick()
		{
			if (ButtonClick != null)
				ButtonClick(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the VisualStyleChanged event.
		/// </summary>
		protected virtual void OnVisualStyleChanged()
		{
			if (VisualStyleChanged != null)
				VisualStyleChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the ActAsButtonChanged event.
		/// </summary>
		protected virtual void OnActAsButtonChanged()
		{
			if (ActAsButtonChanged != null)
				ActAsButtonChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the ArrowButtonChanged event.
		/// </summary>
		protected virtual void OnArrowButtonChanged()
		{
			if (ArrowButtonChanged != null)
				ArrowButtonChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the TextAlignmentChanged event.
		/// </summary>
		protected virtual void OnTextAlignmentChanged()
		{
			if (TextAlignmentChanged != null)
				TextAlignmentChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the LineAlignmentChanged event.
		/// </summary>
		protected virtual void OnLineAlignmentChanged()
		{
			if (LineAlignmentChanged != null)
				LineAlignmentChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the PreTextChanged event.
		/// </summary>
		protected virtual void OnPreTextChanged()
		{
			if (PreTextChanged != null)
				PreTextChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the PostTextChanged event.
		/// </summary>
		protected virtual void OnPostTextChanged()
		{
			if (PostTextChanged != null)
				PostTextChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the PreSeparatorChanged event.
		/// </summary>
		protected virtual void OnPreSeparatorChanged()
		{
			if (PreSeparatorChanged != null)
				PreSeparatorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the PostSeparator event.
		/// </summary>
		protected virtual void OnPostSeparatorChanged()
		{
			if (PostSeparatorChanged != null)
				PostSeparatorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the PadLeftChanged event.
		/// </summary>
		protected virtual void OnPadLeftChanged()
		{
			if (PadLeftChanged != null)
				PadLeftChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the PadRightChanged event.
		/// </summary>
		protected virtual void OnPadRightChanged()
		{
			if (PadRightChanged != null)
				PadRightChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the PadTopChanged event.
		/// </summary>
		protected virtual void OnPadTopChanged()
		{
			if (PadTopChanged != null)
				PadTopChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the PadBottomChanged event.
		/// </summary>
		protected virtual void OnPadBottomChanged()
		{
			if (PadBottomChanged != null)
				PadBottomChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the ImageToTextGapChanged event.
		/// </summary>
		protected virtual void OnImageToTextGapChanged()
		{
			if (ImageToTextGapChanged != null)
				ImageToTextGapChanged(this, EventArgs.Empty);
		}
		
		/// <summary>
		/// Raises the EdgeToImageGapChanged event.
		/// </summary>
		protected virtual void OnEdgeToImageGapChanged()
		{
			if (EdgeToImageGapChanged != null)
				EdgeToImageGapChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the EdgeToArrowGapChanged event.
		/// </summary>
		protected virtual void OnEdgeToArrowGapChanged()
		{
			if (EdgeToArrowGapChanged != null)
				EdgeToArrowGapChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the ActiveChanged event.
		/// </summary>
		protected virtual void OnActiveChanged()
		{
			if (ActiveChanged != null)
				ActiveChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the InactiveBackColorChanged event.
		/// </summary>
		protected virtual void OnInactiveBackColorChanged()
		{
			if (InactiveBackColorChanged != null)
				InactiveBackColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the InactiveForeColorChanged event.
		/// </summary>
		protected virtual void OnInactiveForeColorChanged()
		{
			if (InactiveForeColorChanged != null)
				InactiveForeColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GradientActiveColorChanged event.
		/// </summary>
		protected virtual void OnGradientActiveColorChanged()
		{
			if (GradientActiveColorChanged != null)
				GradientActiveColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GradientInactiveColorChanged event.
		/// </summary>
		protected virtual void OnGradientInactiveColorChanged()
		{
			if (GradientInactiveColorChanged != null)
				GradientInactiveColorChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GradientDirectionChanged event.
		/// </summary>
		protected virtual void OnGradientDirectionChanged()
		{
			if (GradientDirectionChanged != null)
				GradientDirectionChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the GradientColoringChanged event.
		/// </summary>
		protected virtual void OnGradientColoringChanged()
		{
			if (GradientColoringChanged != null)
				GradientColoringChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the MouseOverColorChanged event.
		/// </summary>
		protected virtual void OnMouseOverColorChanged()
		{
			if (MouseOverColorChanged != null)
				MouseOverColorChanged(this, EventArgs.Empty);
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
		/// Raises the ImageAlignmentChanged event.
		/// </summary>
		protected virtual void OnImageAlignmentChanged()
		{
			if (ImageAlignmentChanged != null)
				ImageAlignmentChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the ArrowAlignmentChanged event.
		/// </summary>
		protected virtual void OnArrowAlignmentChanged()
		{
			if (ArrowAlignmentChanged != null)
				ArrowAlignmentChanged(this, EventArgs.Empty);
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
		/// Raises the DirectionChanged event.
		/// </summary>
		protected virtual void OnDirectionChanged()
		{
			if (DirectionChanged != null)
				DirectionChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the TextRenderingHint event.
		/// </summary>
		protected virtual void OnTextRenderingHintChanged()
		{
			if (TextRenderingHintChanged != null)
				TextRenderingHintChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the TextChanged event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnTextChanged(EventArgs e)
		{
			// Change in text means a redraw and respostion
			Invalidate();
			base.OnTextChanged(e);
		}

		/// <summary>
		/// Raises the Resize event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			// Must repaint all the client area again
			Invalidate();
			base.OnResize(e);
		}

		/// <summary>
		/// Raises the MouseEnter event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnMouseEnter(EventArgs e)
		{
			// Update mouse over state
			_mouseOver = true;

			// Find rectangle that encloses the arrow
			Rectangle arrowRect = CalcArrowRectangle();

			// If no rectangle then no mouse over
			if (arrowRect == Rectangle.Empty)
				_mouseOverArrow = false;
			else
			{
				// Make arrow test area slightly bigger for ease of use
				arrowRect.Inflate(2, 2);

				// If in the rectangle then over the arrow
				_mouseOverArrow = arrowRect.Contains(PointToClient(Control.MousePosition));
			}

			Invalidate();
		
			base.OnMouseEnter(e);
		}

		/// <summary>
		/// Raises the MouseMove event.
		/// </summary>
		/// <param name="e">An MouseEventArgs that contains the event data.</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			// Update mouse over state
			_mouseOver = true;

			// Find rectangle that encloses the arrow
			Rectangle arrowRect = CalcArrowRectangle();

			// If no rectangle then no mouse over
			if (arrowRect == Rectangle.Empty)
				_mouseOverArrow = false;
			else
			{
				// Make arrow test area slightly bigger for ease of use
				arrowRect.Inflate(2, 2);

				// If in the rectangle then over the arrow
				_mouseOverArrow = arrowRect.Contains(new Point(e.X, e.Y));
			}

			Invalidate();
		
			base.OnMouseMove(e);
		}

		/// <summary>
		/// Raises the MouseLeave event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnMouseLeave(EventArgs e)
		{
			// Update mouse over state
			_mouseOver = false;
			_mouseOverArrow = false;
			Invalidate();

			base.OnMouseLeave(e);
		}

		/// <summary>
		/// Raises the MouseDown event.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the event data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			// Left mouse down is start of potential click
			if (e.Button == MouseButtons.Left)
			{
				_mouseDown = true;

				// Find rectangle that encloses the arrow
				Rectangle arrowRect = CalcArrowRectangle();

				// If no rectangle then no mouse over
				if (arrowRect == Rectangle.Empty)
					_mouseDownArrow = false;
				else
				{
					// Make arrow test area slightly bigger for ease of use
					arrowRect.Inflate(2, 2);

					// If in the rectangle then over the arrow
					_mouseDownArrow = arrowRect.Contains(new Point(e.X, e.Y));
				}

				Invalidate();
			}
		
			base.OnMouseDown(e);
		}

		/// <summary>
		/// Raises the MouseUp event.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the event data.</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			// Left mouse up is end of potential click
			if (e.Button == MouseButtons.Left)
			{
				switch(ActAsButton)
				{
					case ActAsButton.No:
						// Do nothing
						break;
					case ActAsButton.JustArrow:
						// Only raise event if over control and pressed down on button
						if (_mouseDown && _mouseOver && _mouseDownArrow)
							PerformButtonClick();
						break;
					case ActAsButton.WholeControl:
						// If over control and pressed on contrl then fire event
						if (_mouseDown && _mouseOver)
							PerformButtonClick();
						break;
				}
			
				_mouseDown = false;
				_mouseDownArrow = false;
								
				Invalidate();
			}

			base.OnMouseUp(e);
		}

		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
            // Prevent reentrancy
            if (!_painting)
            {
                _painting = true;

                // Draw background according to GradientDirection setting
                DrawBackground(e);

                // Set drawing flags according to properties			
                using (StringFormat sf = new StringFormat())
                {
                    sf.Trimming = StringTrimming.EllipsisCharacter;
                    sf.Alignment = TextAlignment;
                    sf.LineAlignment = LineAlignment;
                    sf.FormatFlags = StringFormatFlags.FitBlackBox |
                                     StringFormatFlags.NoWrap;

                    // Draw vertical?
                    if (Direction == LayoutDirection.Vertical)
                    {
                        sf.FormatFlags |= StringFormatFlags.DirectionVertical;

                        // Invert the line alignment to get correct appearance
                        if (sf.LineAlignment == StringAlignment.Near)
                            sf.LineAlignment = StringAlignment.Far;
                        else if (sf.LineAlignment == StringAlignment.Far)
                            sf.LineAlignment = StringAlignment.Near;
                    }

                    // Create rectangle used for drawing items by shrinking client rect by padding
                    Rectangle drawRect = new Rectangle(PadLeft, PadTop,
                                                       Width - PadLeft - PadRight,
                                                       Height - PadTop - PadBottom);

                    // If mouse is pressed and acting like a button with mouse over control
                    if (DepressControl())
                    {
                        // Then shift down and right to show control depressed
                        drawRect.X += 2;
                        drawRect.Y += 2;
                        drawRect.Width -= 2;
                        drawRect.Height -= 2;
                    }

                    // Draw any arrow and shrink left over space
                    DrawArrow(e, ref drawRect);

                    // Draw any image/icon and shrink space left for drawing
                    DrawImageOrIcon(e, ref drawRect);

                    // Position any child buttons
                    PositionTitleButtons(e, ref drawRect);

                    // Use the appropriate text drawing quality
                    e.Graphics.TextRenderingHint = TextRenderingHint;

                    switch (_style)
                    {
                        case VisualStyle.Office2007Blue:
                        case VisualStyle.Office2007Silver:
                        case VisualStyle.Office2007Black:
                            if (Apply2007ClearType)
                            {
                                using (UseClearTypeGridFit clearType = new UseClearTypeGridFit(e.Graphics))
                                    using (SolidBrush foreBrush = new SolidBrush(CalcTextColor()))
                                        e.Graphics.DrawString(CalcDrawText(), Font, foreBrush, drawRect, sf);
                            }
                            else
                            {
                                using (SolidBrush foreBrush = new SolidBrush(CalcTextColor()))
                                    e.Graphics.DrawString(CalcDrawText(), Font, foreBrush, drawRect, sf);
                            }
                            break;
                        case VisualStyle.MediaPlayerBlue:
                        case VisualStyle.MediaPlayerOrange:
                        case VisualStyle.MediaPlayerPurple:
                            if (ApplyMediaPlayerClearType)
                            {
                                using (UseClearTypeGridFit clearType = new UseClearTypeGridFit(e.Graphics))
                                    using (SolidBrush foreBrush = new SolidBrush(CalcTextColor()))
                                        e.Graphics.DrawString(CalcDrawText(), Font, foreBrush, drawRect, sf);
                            }
                            else
                            {
                                using (SolidBrush foreBrush = new SolidBrush(CalcTextColor()))
                                    e.Graphics.DrawString(CalcDrawText(), Font, foreBrush, drawRect, sf);
                            }
                            break;
                        default:
                            using (SolidBrush foreBrush = new SolidBrush(CalcTextColor()))
                                e.Graphics.DrawString(CalcDrawText(), Font, foreBrush, drawRect, sf);
                            break;
                    }
                }

                _painting = false;
            }
		}

		private void OnTitleButtonInserted(int index, object value)
		{
            // Cast to correct type
			TitleButton tb = value as TitleButton;

			// Monitor changes in the button values
			tb.PropertyChanged += new EventHandler(OnTitleButtonPropertyChanged);

			// Create a button instance to represent the definition
            tb.Control = new ButtonWithStyle();
            tb.Control.DrawBackground = false;
			
			// Setup back reference from button to definition
			tb.Control.Tag = tb;

			// Need to know when button is pressed
			tb.Control.Click += new EventHandler(OnTitleButtonClicked);

			// Copy across the definition values
			tb.Control.Enabled = tb.Enabled;
			tb.Control.Visible = tb.Visible;
			tb.Control.Image = (tb.Image == null ? null : (Image)tb.Image.Clone());

			// Make sure the button is transparent so that the 
			// background of the title bar shows through ideally.
			tb.Control.BackColor = Color.Transparent;

			// Add as a child control of the titlebar
			Controls.Add(tb.Control);

			// Set into correct ordering
			Controls.SetChildIndex(tb.Control, index);

			// Must layout the buttons again
			Invalidate();
		}

		private void OnTitleButtonRemoved(int index, object value)
		{
            // Cast to correct type
			TitleButton tb = value as TitleButton;

			// No need to monitor changes in the button values
			tb.PropertyChanged -= new EventHandler(OnTitleButtonPropertyChanged);

			// Unhook button control event
			tb.Control.Click -= new EventHandler(OnTitleButtonClicked);

			// Remove associated control as child
			Controls.Remove(tb.Control);

			// Dispose of it gracefully
			tb.Control.Dispose();
			tb.Control = null;

			// Must layout the buttons again
			Invalidate();
		}

		private void OnTitleButtonClearing()
		{
            // No need to monitor changes in the button values
			foreach(TitleButton tb in _titleButtons)
			{
				if (tb.Control != null)
				{
					tb.PropertyChanged -= new EventHandler(OnTitleButtonPropertyChanged);

					// Unhook button control event
					tb.Control.Click -= new EventHandler(OnTitleButtonClicked);

					// Dispose of it gracefully
					tb.Control.Dispose();
					tb.Control = null;
				}
			}

			// Must layout the buttons again
			Invalidate();
		}

		private void OnTitleButtonPropertyChanged(object sender, EventArgs e)
		{
			// Cast to correct type
			TitleButton tb = sender as TitleButton;

			if (tb.Control != null)
			{
				// Copy across the definition values
				tb.Control.Enabled = tb.Enabled;
				tb.Control.Visible = tb.Visible;
                tb.Control.Image = (tb.Image == null ? null : (Image)tb.Image.Clone());
			}

			// Must layout the buttons again
			Invalidate();
		}

		private void OnTitleButtonClicked(object sender, EventArgs e)
		{
			// Cast to correct type
			ButtonWithStyle bws = sender as ButtonWithStyle;

			if (bws != null)
			{
				// Navigate from button to definition
				TitleButton tb = bws.Tag as TitleButton;

				// Raise the user defined event for definition
				tb.PerformClick();
			}
		}

		private bool DepressControl()
		{
			bool ret;

			switch(ActAsButton)
			{
				case ActAsButton.WholeControl:
					ret = _mouseDown && _mouseOver;
					break;
				case ActAsButton.JustArrow:
				case ActAsButton.No:
				default:
					ret = false;
					break;
			}

			return ret;
		}

		private bool DepressArrow()
		{
			bool ret;

			switch(ActAsButton)
			{
				case ActAsButton.WholeControl:
					ret = _mouseDown && _mouseOver;
					break;
				case ActAsButton.JustArrow:
					ret = _mouseDownArrow && _mouseOverArrow;
					break;
				case ActAsButton.No:
				default:
					ret = false;
					break;
			}

			return ret;
		}

		private Color CalcTextColor()
		{
			// Default to using color dependant on active state
			Color textColor = (Active ? ForeColor : InactiveForeColor);

            switch (_style)
            {
                case VisualStyle.Office2003:
                    if (Active)
                    {
                        if (ForeColor == SystemColors.ActiveCaptionText)
                            textColor = Color.White;
                    }
                    else
                    {
                        if (InactiveForeColor == SystemColors.InactiveCaptionText)
                            textColor = Color.Black;
                    }
                    break;
                case VisualStyle.Office2007Blue:
                case VisualStyle.Office2007Silver:
                case VisualStyle.Office2007Black:
                    if (Active)
                    {
                        if (ForeColor == SystemColors.ActiveCaptionText)
                            textColor = Office2007ColorTable.TitleActiveTextColor(_style);
                    }
                    else
                    {
                        if (InactiveForeColor == SystemColors.InactiveCaptionText)
                            textColor = Office2007ColorTable.TitleInactiveTextColor(_style);
                    }
                    break;
                case VisualStyle.MediaPlayerBlue:
                case VisualStyle.MediaPlayerOrange:
                case VisualStyle.MediaPlayerPurple:
                    if (Active)
                    {
                        if (ForeColor == SystemColors.ActiveCaptionText)
                            textColor = MediaPlayerColorTable.TitleActiveTextColor(_style);
                    }
                    else
                    {
                        if (InactiveForeColor == SystemColors.InactiveCaptionText)
                            textColor = MediaPlayerColorTable.TitleInactiveTextColor(_style);
                    }
                    break;
            }

			// If the mouse is over the control and we have a defined color for this state
			if (_mouseOver && (_mouseOverColor != Color.Empty))
				textColor = _mouseOverColor;

			return textColor;
		}
		
		private void DrawBackground(PaintEventArgs e)
		{
			// Use correct source for colour
			Color backColor = Active ? BackColor : InactiveBackColor;
			
			Color start = backColor;
			Color end = backColor;
            bool ignoreColoring = (_style == VisualStyle.Plain);

            switch (_style)
            {
                case VisualStyle.Office2003:
				    if (Active)
				    {
					    if (BackColor == SystemColors.ActiveCaption)
					    {
						    start = _colorDetails.DarkBaseColor;
						    end = _colorDetails.DarkBaseColor2;
						    ignoreColoring = true;
					    }
				    }
				    else
				    {
					    if (InactiveBackColor == SystemColors.InactiveCaption)
					    {
						    start = _colorDetails.BaseColor1;
						    end = _colorDetails.BaseColor2;
						    ignoreColoring = true;
					    }
				    }
                    break;
                case VisualStyle.Office2007Blue:
                case VisualStyle.Office2007Silver:
                case VisualStyle.Office2007Black:
                    if (Active)
                    {
                        if (BackColor == SystemColors.ActiveCaption)
                        {
                            start = Office2007ColorTable.TitleActiveLight(_style);
                            end = Office2007ColorTable.TitleActiveDark(_style);
                            ignoreColoring = true;
                        }
                    }
                    else
                    {
                        if (InactiveBackColor == SystemColors.InactiveCaption)
                        {
                            start = Office2007ColorTable.TitleInactiveLight(_style);
                            end = Office2007ColorTable.TitleInactiveDark(_style);
                            ignoreColoring = true;
                        }
                    }
                    break;
                case VisualStyle.MediaPlayerBlue:
                case VisualStyle.MediaPlayerOrange:
                case VisualStyle.MediaPlayerPurple:
                    if (Active)
                    {
                        if (BackColor == SystemColors.ActiveCaption)
                        {
                            start = MediaPlayerColorTable.TitleActiveLight(_style);
                            end = MediaPlayerColorTable.TitleActiveDark(_style);
                            ignoreColoring = true;
                        }
                    }
                    else
                    {
                        if (InactiveBackColor == SystemColors.InactiveCaption)
                        {
                            start = MediaPlayerColorTable.TitleInactiveLight(_style);
                            end = MediaPlayerColorTable.TitleInactiveDark(_style);
                            ignoreColoring = true;
                        }
                    }
                    break;
            }

			if (!ignoreColoring)
			{
				// Work out the starting and ending colors for the gradient			
				switch(GradientColoring)
				{
					case GradientColoring.LightBackToBack:
						start = ControlPaint.Light(backColor);
						end = ControlPaint.Light(ControlPaint.Dark(backColor));
						break;
					case GradientColoring.LightBackToDarkBack:
						start = ControlPaint.Light(backColor);
						end = ControlPaint.Dark(backColor);
						break;
					case GradientColoring.BackToDarkBack:
						start = backColor;
						end = ControlPaint.Dark(backColor);
						break;
					case GradientColoring.BackToGradientColor:
						start = backColor;
						end = Active ? GradientActiveColor : GradientInactiveColor;
						break;
					case GradientColoring.LightBackToGradientColor:
						start = ControlPaint.Light(backColor);
						end = Active ? GradientActiveColor : GradientInactiveColor;
						break;
				}
			}

			// Use a fraction bigger rect to get slightly smoother look
			Rectangle gradientRect = ClientRectangle;
			gradientRect.Inflate(1, 1);
			
			// Convert from enum to angle
			float angle = (float)GradientDirection;

			// When vertical we rotate angle around to match new direction
			if (Direction == LayoutDirection.Vertical)
				angle += 90;

			// Is there any rectangle size to draw?
			if ((gradientRect.Width > 0) && (gradientRect.Height > 0))
			{
				// Draw using the calculated colors and direction
				using(LinearGradientBrush backBrush = new LinearGradientBrush(gradientRect, start, end, angle))
					e.Graphics.FillRectangle(backBrush, ClientRectangle);
			}
		}

		private void DrawArrow(PaintEventArgs e, ref Rectangle drawRect)
		{
			// If there is a requirement for a arrow button
			if (ArrowButton != ArrowButton.None)
			{
				ColorMap colorMap = new ColorMap();
			
				// Convert from white to required color
				colorMap.OldColor = Color.White;
				colorMap.NewColor = CalcTextColor();

				// Create remap attributes for use by button
				_imageAttr.SetRemapTable(new ColorMap[]{colorMap}, ColorAdjustType.Bitmap);

				// Work out how far down/across to draw image so its centred
				int vertOffset = (drawRect.Height - _arrows.ImageSize.Height) / 2;
				int horzOffset = (drawRect.Width - _arrows.ImageSize.Width) / 2;

				bool pressed = DepressArrow();

				// Draw the entire image without scaling
				if (ArrowAlignment == ImageAlignment.Near)
				{
					if (Direction == LayoutDirection.Horizontal)				
					{
						Image drawImage = _arrows.Images[(int)ArrowButton - 1];
						e.Graphics.DrawImage(drawImage, 
							new Rectangle(drawRect.Left + EdgeToArrowGap + (pressed ? 1 : 0), 
							drawRect.Top + vertOffset + (pressed ? 1 : 0), 
							_arrows.ImageSize.Width, 
							_arrows.ImageSize.Height),
							0, 0, 
							_arrows.ImageSize.Width, 
							_arrows.ImageSize.Height,
							GraphicsUnit.Pixel,
							_imageAttr);
						drawImage.Dispose();

						// Reduce available space by image and spacing gaps
						int imageAndGap = EdgeToArrowGap + _arrows.ImageSize.Width + ImageToTextGap;
						drawRect.X += imageAndGap;
						drawRect.Width -= imageAndGap;
					}
					else
					{
						Image drawImage = _arrows.Images[(int)ArrowButton - 1];
						e.Graphics.DrawImage(drawImage, 
							new Rectangle(drawRect.Left + horzOffset + (pressed ? 1 : 0), 
							drawRect.Top + EdgeToArrowGap + (pressed ? 1 : 0),
							_arrows.ImageSize.Width, 
							_arrows.ImageSize.Height),
							0, 0, 
							_arrows.ImageSize.Width, 
							_arrows.ImageSize.Height,
							GraphicsUnit.Pixel,
							_imageAttr);
						drawImage.Dispose();

						// Reduce available space by image and spacing gaps
						int imageAndGap = EdgeToArrowGap + _arrows.ImageSize.Height + ImageToTextGap;
						drawRect.Y += imageAndGap;
						drawRect.Height -= imageAndGap;
					}
				}
				else
				{
					// Draw the entire image without scaling
					if (Direction == LayoutDirection.Horizontal)				
					{
						Image drawImage = _arrows.Images[(int)ArrowButton - 1];
						e.Graphics.DrawImage(drawImage, 
							new Rectangle(drawRect.Right - EdgeToArrowGap - _arrows.ImageSize.Width + (pressed ? 1 : 0), 
							drawRect.Top + vertOffset + (pressed ? 1 : 0), 
							_arrows.ImageSize.Width, 
							_arrows.ImageSize.Height),
							0, 0,
							_arrows.ImageSize.Width, 
							_arrows.ImageSize.Height,
							GraphicsUnit.Pixel,
							_imageAttr);
						drawImage.Dispose();

						// Reduce available space by image and spacing gaps
						drawRect.Width -= EdgeToArrowGap + _arrows.ImageSize.Width + ImageToTextGap;
					}
					else
					{
						Image drawImage = _arrows.Images[(int)ArrowButton - 1];
						e.Graphics.DrawImage(drawImage, 
							new Rectangle(drawRect.Left + horzOffset + (pressed ? 1 : 0), 
							drawRect.Bottom - EdgeToArrowGap - _arrows.ImageSize.Height + (pressed ? 1 : 0), 
							_arrows.ImageSize.Width, 
							_arrows.ImageSize.Height),
							0, 0, 
							_arrows.ImageSize.Width, 
							_arrows.ImageSize.Height,
							GraphicsUnit.Pixel,
							_imageAttr);
						drawImage.Dispose();

						// Reduce available space by image and spacing gaps
						drawRect.Height -= EdgeToArrowGap + _arrows.ImageSize.Height + ImageToTextGap;
					}
				}
			}
		}
		
		private void DrawImageOrIcon(PaintEventArgs e, ref Rectangle drawRect)
		{
			// Always draw an icon as first preference
			if (Icon != null)
			{
				// Work out how far down/across to draw image so its centred
				int vertOffset = (drawRect.Height - Icon.Height) / 2;
				int horzOffset = (drawRect.Width - Icon.Width) / 2;

				// Adjust offset if image is not line centered	
				if (LineAlignment == StringAlignment.Near)
				{
					vertOffset = 0;
					horzOffset = (drawRect.Width - Icon.Width);
				}
				else if (LineAlignment == StringAlignment.Far)
				{
					vertOffset = (drawRect.Height - Icon.Height);
					horzOffset = 0;
				}

				// Draw the entire image without scaling
				if (ImageAlignment == ImageAlignment.Near)
				{
					if (Direction == LayoutDirection.Horizontal)				
					{
						e.Graphics.DrawIcon(Icon, drawRect.Left + EdgeToImageGap, drawRect.Top + vertOffset);

						// Reduce available space by image and spacing gaps
						int imageAndGap = EdgeToImageGap + Icon.Width + ImageToTextGap;
						drawRect.X += imageAndGap;
						drawRect.Width -= imageAndGap;
					}
					else
					{
						e.Graphics.DrawIcon(Icon, drawRect.Left + horzOffset, drawRect.Top + EdgeToImageGap);

						// Reduce available space by image and spacing gaps
						int imageAndGap = EdgeToImageGap + Icon.Height + ImageToTextGap;
						drawRect.Y += imageAndGap;
						drawRect.Height -= imageAndGap;
					}
				}
				else
				{
					if (Direction == LayoutDirection.Horizontal)				
					{
						e.Graphics.DrawIcon(Icon, drawRect.Right - EdgeToImageGap - Icon.Width, drawRect.Top + vertOffset);

						// Reduce available space by image and spacing gaps
						drawRect.Width -= EdgeToImageGap + Icon.Width + ImageToTextGap;
					}
					else
					{
						e.Graphics.DrawIcon(Icon, drawRect.Left + horzOffset, drawRect.Bottom - EdgeToImageGap - Icon.Height);

						// Reduce available space by image and spacing gaps
						drawRect.Height -= EdgeToImageGap + Icon.Height + ImageToTextGap;
					}
				}
			}
			else
			{
				// Default to the Image value
				Image sourceImage = Image;
				bool disposeImage = false;
				
				// If we have a source of images and the index into them is valid
				if ((ImageList != null) && (ImageIndex >= 0) && (ImageIndex < ImageList.Images.Count))
				{
					sourceImage = ImageList.Images[ImageIndex];
					disposeImage = true;
				}
			
				// Did we find an image to draw?
				if (sourceImage != null)
				{
					// Work out how far down/across to draw image so its centred
					int vertOffset = (drawRect.Height - sourceImage.Height) / 2;
					int horzOffset = (drawRect.Width - sourceImage.Width) / 2;
					
					// Adjust offset if image is not line centered	
					if (LineAlignment == StringAlignment.Near)
					{
						vertOffset = 0;
						horzOffset = drawRect.Width - sourceImage.Width;
					}
					else if (LineAlignment == StringAlignment.Far)
					{
						vertOffset = (drawRect.Height - sourceImage.Height);
						horzOffset = 0;
					}

					// Draw the entire image without scaling
					if (ImageAlignment == ImageAlignment.Near)
					{
						if (Direction == LayoutDirection.Horizontal)				
						{
							if (Active)
								e.Graphics.DrawImage(sourceImage, drawRect.Left + EdgeToImageGap, drawRect.Top + vertOffset);
							else
							{
								// Make sure we have created the inactive image
								if (_inactiveImage == null)
									_inactiveImage = CommandImage.CreateGrayScaleImage(sourceImage);
								
								e.Graphics.DrawImage(_inactiveImage, drawRect.Left + EdgeToImageGap, drawRect.Top + vertOffset);
							}

					
							// Reduce available space by image and spacing gaps
							int imageAndGap = EdgeToImageGap + sourceImage.Width + ImageToTextGap;
							drawRect.X += imageAndGap;
							drawRect.Width -= imageAndGap;
						}
						else
						{
							if (Active)
								e.Graphics.DrawImage(sourceImage, drawRect.Left + horzOffset, drawRect.Top + EdgeToImageGap);
							else
							{
								// Make sure we have created the inactive image
								if (_inactiveImage == null)
									_inactiveImage = CommandImage.CreateGrayScaleImage(sourceImage);
								
								e.Graphics.DrawImage(_inactiveImage, drawRect.Left + horzOffset, drawRect.Top + EdgeToImageGap);
							}
					
							// Reduce available space by image and spacing gaps
							int imageAndGap = EdgeToImageGap + sourceImage.Height + ImageToTextGap;
							drawRect.Y += imageAndGap;
							drawRect.Height -= imageAndGap;
						}
					}
					else
					{
						if (Direction == LayoutDirection.Horizontal)				
						{
							if (Active)
								e.Graphics.DrawImage(sourceImage, drawRect.Right - EdgeToImageGap - sourceImage.Width, drawRect.Top + vertOffset);
							else
							{
								// Make sure we have created the inactive image
								if (_inactiveImage == null)
									_inactiveImage = CommandImage.CreateGrayScaleImage(sourceImage);
								
								e.Graphics.DrawImage(_inactiveImage, drawRect.Right - EdgeToImageGap - sourceImage.Width, drawRect.Top + vertOffset);
							}
					
							// Reduce available space by image and spacing gaps
							int imageAndGap = EdgeToImageGap + sourceImage.Width + ImageToTextGap;
							drawRect.Width -= imageAndGap;
						}
						else
						{
							if (Active)
								e.Graphics.DrawImage(sourceImage, drawRect.Left + horzOffset, drawRect.Bottom - EdgeToImageGap - sourceImage.Height);
							else
							{
								// Make sure we have created the inactive image
								if (_inactiveImage == null)
									_inactiveImage = CommandImage.CreateGrayScaleImage(sourceImage);
								
								e.Graphics.DrawImage(_inactiveImage, drawRect.Left + horzOffset, drawRect.Bottom - EdgeToImageGap - sourceImage.Height);
							}
					
							// Reduce available space by image and spacing gaps
							drawRect.Height -= EdgeToImageGap + sourceImage.Width + ImageToTextGap;
						}
					}

					if (disposeImage)
						sourceImage.Dispose();
				}
			}
		}

		private void PositionTitleButtons(PaintEventArgs e, ref Rectangle drawRect)
		{
			// Is there anything to position?
			if (_titleButtons.Count > 0)
			{
				// Position the buttons in reverse order
				for(int i=_titleButtons.Count-1; i>=0; i--)
				{	
					// Get the indexed button
					TitleButton titleButton = _titleButtons[i];

					if (titleButton.Visible)
					{
						// Starting size is enough for the borders
						Size buttonSize = new Size(titleButton.PictureBorder * 2,
							                       titleButton.PictureBorder * 2);

						// Add image size if drawing an image
						if (titleButton.Image != null)
						{
                            try
                            {
                                buttonSize.Width += titleButton.Image.Width;
                                buttonSize.Height += titleButton.Image.Height;
                            }
                            catch { }
						}

						// Are we positioning horizontally?
						if (Direction == LayoutDirection.Horizontal)
						{
							// Is there enough space for the button width?
							if (drawRect.Width >= buttonSize.Width)
							{
								// Find vertical offset
								int offset = (drawRect.Height - buttonSize.Height) / 2;

								// Position the button
								titleButton.Control.SetBounds(drawRect.Right - buttonSize.Width,
									drawRect.Top + offset,
									buttonSize.Width,
									buttonSize.Height);

								// Reduce available size
								drawRect.Width -= buttonSize.Width;
							}
						}
						else
						{
							// Is there enough space for the button width?
							if (drawRect.Height >= buttonSize.Height)
							{
								// Find vertical offset
								int offset = (drawRect.Width - buttonSize.Width) / 2;

								// Position the button
								titleButton.Control.SetBounds(drawRect.Left + offset,
									                          drawRect.Bottom - buttonSize.Height,
									                          buttonSize.Width,
									                          buttonSize.Height);

								// Reduce available size
								drawRect.Height -= buttonSize.Height;
							}
						}

						// Make sure child control reflect definition
						titleButton.Control.Visible = true;
						titleButton.Control.Enabled = titleButton.Enabled;
					}
					else
					{
						// Make sure child control reflect definition
						titleButton.Control.Visible = false;
					}
				}
			}
		}

		private string CalcDrawText()
		{
			StringBuilder drawText = new StringBuilder();
			
			// Is there a PreText to show?
			if ((PreText != null) && (PreText.Length > 0))
			{
				// Add the PreText and the separator between it and Text
				drawText.Append(PreText);
				drawText.Append(PreSeparator);
			}
			
			// Always add the Text
			drawText.Append(Text);
			
			// Is there a PostText to show?
			if ((PostText != null) && (PostText.Length > 0))
			{
				// Add the PostText and the separator between Text and PostText
				drawText.Append(PostSeparator);
				drawText.Append(PostText);
			}
			
			return drawText.ToString();
		}

		private Rectangle CalcArrowRectangle()
		{
			Rectangle ret = Rectangle.Empty;

			// If there is a requirement for a arrow button
			if (ArrowButton != ArrowButton.None)
			{
				// Create rectangle used for drawing items by shrinking client rect by padding
				Rectangle drawRect = new Rectangle(PadLeft, PadTop, 
					Width - PadLeft - PadRight,
					Height - PadTop - PadBottom);

				
				// Work out how far down/across to draw image so its centred
				int vertOffset = (drawRect.Height - _arrows.ImageSize.Height) / 2;
				int horzOffset = (drawRect.Width - _arrows.ImageSize.Width) / 2;

				// Draw the entire image without scaling
				if (ArrowAlignment == ImageAlignment.Near)
				{
					if (Direction == LayoutDirection.Horizontal)				
					{
						ret = new Rectangle(drawRect.Left + EdgeToArrowGap, 
							drawRect.Top + vertOffset, 
							_arrows.ImageSize.Width, 
							_arrows.ImageSize.Height);
					}
					else
					{
						ret = new Rectangle(drawRect.Left + horzOffset, 
							drawRect.Top + EdgeToArrowGap,
							_arrows.ImageSize.Width, 
							_arrows.ImageSize.Height);
					}
				}
				else
				{
					if (Direction == LayoutDirection.Horizontal)				
					{
						ret = new Rectangle(drawRect.Right - EdgeToArrowGap - _arrows.ImageSize.Width, 
							drawRect.Top + vertOffset, 
							_arrows.ImageSize.Width, 
							_arrows.ImageSize.Height);
					}
					else
					{
						ret = new Rectangle(drawRect.Left + horzOffset, 
							drawRect.Bottom - EdgeToArrowGap - _arrows.ImageSize.Height, 
							_arrows.ImageSize.Width, 
							_arrows.ImageSize.Height);
					}
				}
			}

			return ret;
		}

		internal Color GetGradientActiveColor()
		{
			// Get the OS defined second color for gradient active caption
			uint rgb = Win32.User32.GetSysColor(Win32.SysColors.COLOR_GRADIENTACTIVECAPTION);
			return Color.FromArgb(255, (byte)rgb, (byte)(rgb >> 8), (byte)(rgb >> 16));
		}

		internal Color GetGradientInactiveColor()
		{
			// Get the OS defined second color for gradient inactive caption
			uint rgb = Win32.User32.GetSysColor(Win32.SysColors.COLOR_GRADIENTINACTIVECAPTION);
			return Color.FromArgb(255, (byte)rgb, (byte)(rgb >> 8), (byte)(rgb >> 16));
		}
	}
    
	/// <summary>
	/// Specifies how control should act as a button.
	/// </summary>
	public enum ActAsButton
	{
		/// <summary>
		/// Specifies that control is not to act like a button.
		/// </summary>
		No,
		/// <summary>
		/// Specifies whole control is to be treated as a button.
		/// </summary>
		WholeControl,
		/// <summary>
		/// Specifies that only the arrow area should be treated as button.
		/// </summary>
		JustArrow
	}

	/// <summary>
	/// Specifies position to draw image/icon relative to control.
	/// </summary>
	public enum ImageAlignment
	{
		/// <summary>
		/// Specifies the image/icon be drawn near side.
		/// </summary>
		Near,
		/// <summary>
		/// Specifies the image/icon be drawn far side.
		/// </summary>
		Far
	}
    
	/// <summary>
	/// Specifies how the background color gradient is drawn.
	/// </summary>
	public enum GradientDirection : int
	{
		/// <summary>
		/// Specifies no gradient but solid color instead.
		/// </summary>
		None = -1,
		
		/// <summary>
		/// Specifies gradient left to right of control.
		/// </summary>
		LeftToRight = 0,
		
		/// <summary>
		/// Specifies gradient top left to bottom right of control.
		/// </summary>
		TopLeftToBottomRight = 45,
		
		/// <summary>
		/// Specifies gradient from top to bottom of control.
		/// </summary>
		TopToBottom = 90,
		
		/// <summary>
		/// Specifies gradient top right to bottom left of control.
		/// </summary>
		TopRightToBottomLeft = 135,
		
		/// <summary>
		/// Specifies gradient right to left of control.
		/// </summary>
		RightToLeft = 180,
		
		/// <summary>
		/// Specifies gradient bottom right to top left of control.
		/// </summary>
		BottomRightToTopLeft = 225,
		
		/// <summary>
		/// Specifies gradient from bottom to top of control.
		/// </summary>
		BottomToTop = 270,
		
		/// <summary>
		/// Specifies gradient bottom left to top right of control.
		/// </summary>
		BottomLeftToTopRight = 315,
	}

	/// <summary>
	/// Specifies source and destination colors when drawing gradient background.
	/// </summary>
	public enum GradientColoring
	{
		/// <summary>
		/// Specifies light version of BackColor to BackColor.
		/// </summary>
		LightBackToBack,
		
		/// <summary>
		/// Specifies light version of BackColor to dark version of BackColor.
		/// </summary>
		LightBackToDarkBack,
		
		/// <summary>
		/// Specifies BackColor to dark version of BackColor.
		/// </summary>
		BackToDarkBack,
		
		/// <summary>
		/// Specifies BackColor to GradientColor
		/// </summary>
		BackToGradientColor,
		
		/// <summary>
		/// Specifies light version of BackColor to GradientColor.
		/// </summary>
		LightBackToGradientColor
	}

	/// <summary>
	/// Specifies any arrow button that should be displayed.
	/// </summary>
	public enum ArrowButton
	{
		/// <summary>
		/// Specifies that no arrow button is needed.
		/// </summary>
		None,
		/// <summary>
		/// Specifies that an up arrow is needed.
		/// </summary>
		UpArrow,
		/// <summary>
		/// Specifies that an down arrow is needed.
		/// </summary>
		DownArrow,
		/// <summary>
		/// Specifies that an left arrow is needed.
		/// </summary>
		LeftArrow,
		/// <summary>
		/// Specifies that an right arrow is needed.
		/// </summary>
		RightArrow,
		/// <summary>
		/// Specifies that a pinned picture appear.
		/// </summary>
		Pinned,
		/// <summary>
		/// Specifies that a unpined picture appear.
		/// </summary>
		Unpinned
	}
}
