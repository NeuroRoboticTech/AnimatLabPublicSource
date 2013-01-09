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
using System.Diagnostics;
using Crownwood.DotNetMagic.Common;
using Crownwood.DotNetMagic.Win32;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Configurable title bar for presenting user with header information.
	/// </summary>
    [ToolboxBitmap(typeof(SlidingTitleBar))]
	[DefaultProperty("TitleBarText")]
	[DefaultEvent("TitleBarTextChanged")]
	public class SlidingTitleBar : Control, ISupportInitialize, IMessageFilter
    {
		// Instance fields 
		private bool _init;

		// Instance fields - Exposed properties
		private bool _open;
		private bool _arrows;
		private bool _internalOpen;
		private int _titleBarLength;
		private TitleEdge _titleEdge;
        private VisualStyle _externalStyle;

		// Instance fields - Child Controls
		private Panel _slide;
		private Panel _innerPanel;
		private TitleBar _titleBar;
		
		// Instance fields - Border
		private BorderStyle _borderStyle;
		private Color _borderColor;
		private Size _borderSize;
		
		// Instance fields - Sliding
		private bool _sliding;
		private bool _actionInValid;	
		private bool _actionOutValid;	
		private bool _slideOnHover;
		private int _slideStep;
		private int _slideSteps;
		private int _slideDuration;
		private Timer _timer;
		private Timer _actionIn;
		private Timer _actionOut;
		
		/// <summary>
		/// Occurs when the Edge property changes.
		/// </summary>
		[Category("SlidingTitleBar")]
		public event EventHandler EdgeChanged;

		/// <summary>
		/// Occurs when the Arrows property changes.
		/// </summary>
		[Category("SlidingTitleBar")]
		public event EventHandler ArrowsChanged;

		/// <summary>
		/// Occurs when the Length property changes.
		/// </summary>
		[Category("SlidingTitleBar")]
		public event EventHandler LengthChanged;

		/// <summary>
		/// Occurs when the Open property changes.
		/// </summary>
		[Category("SlidingTitleBar")]
		public event EventHandler OpenChanged;

		/// <summary>
		/// Occurs when the SlideSteps property changes.
		/// </summary>
		[Category("SlidingTitleBar")]
		public event EventHandler SlideOnHoverChanged;

		/// <summary>
		/// Occurs when the SlideSteps property changes.
		/// </summary>
		[Category("SlidingTitleBar")]
		public event EventHandler SlideStepsChanged;

		/// <summary>
		/// Occurs when the SlideDuration property changes.
		/// </summary>
		[Category("SlidingTitleBar")]
		public event EventHandler SlideDurationChanged;

		/// <summary>
		/// Occurs when the HoverDelay property changes.
		/// </summary>
		[Category("SlidingTitleBar")]
		public event EventHandler HoverDelayChanged;

		/// <summary>
		/// Occurs when the BorderStyle property changes.
		/// </summary>
		[Category("Appearance")]
		public event EventHandler BorderStyleChanged;

		/// <summary>
		/// Occurs when the BorderColor property changes.
		/// </summary>
		[Category("Appearance")]
		public event EventHandler BorderColorChanged;

		/// <summary>
		/// Occurs when the TitleText property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler TitleTextChanged;
    
		/// <summary>
		/// Occurs when the Style property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler VisualStyleChanged;

		/// <summary>
		/// Occurs when the TitleForeColor property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler TitleForeColorChanged;

		/// <summary>
		/// Occurs when the TitleBackColor property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler TitleBackColorChanged;

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
		/// Occurs when the MouseOverColor property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler MouseOverColorChanged;

		/// <summary>
		/// Occurs when the TextRenderingHint property changes.
		/// </summary>
		[Category("TitleBar")]
		public event EventHandler TextRenderingHintChanged;

		/// <summary>
		/// Initializes a new instance of the SlidingTitleBar class.
		/// </summary>
        public SlidingTitleBar()
        {
			// NAG processing
			NAG.NAG_Start();
			
            // Prevent drawing flicker by blitting from memory in WM_PAINT
            SetStyle(ControlStyles.OptimizedDoubleBuffer | 
				     ControlStyles.AllPaintingInWmPaint |
					 ControlStyles.UserPaint | 
					 ControlStyles.ResizeRedraw, true);

			// We default to being opened up
			_open = true;
			_internalOpen = true;
			
			CreateTimers();
			CreateInnerPanel();
			CreateTitleBar();
			CreateChildPanel();
			
			// Default the sliding specific properties
			BeginInit();
			ResetLength();
			ResetEdge();
			ResetOpen();
			ResetBorderStyle();
			ResetBorderColor();
			ResetSlideSteps();
			ResetSlideDuration();
			ResetSlideOnHover();
			ResetHoverDelay();
			
			// Default the TitleBar proxy properties
			ResetText();
			ResetTextAlignment();
			ResetLineAlignment();
			ResetTitleForeColor();
			ResetTitleBackColor();
			ResetTitleFont();
			ResetStyle();
			ResetPreText();
			ResetPostText();
			ResetPreSeparator();
			ResetPostSeparator();
			ResetPadLeft();
			ResetPadRight();
			ResetPadTop();
			ResetPadBottom();
			ResetMouseOverColor();
			ResetInactiveBackColor();
			ResetInactiveForeColor();
			ResetIcon();
			ResetImage();
			ResetImageList();
			ResetImageIndex();
			ResetImageAlignment();
			ResetImageToTextGap();
			ResetGradientDirection();
			ResetGradientColoring();
			ResetGradientActiveColor();
			ResetGradientInactiveColor();
			ResetEdgeToImageGap();
			ResetEdgeToArrowGap();
			ResetArrowAlignment();
			ResetTextRenderingHint();
			
			// Give ourself a sensible default size
			Size = new Size(200, TitleFont.Height * 2);
	
			EndInit();

			// Define correct initial positioning			
			PositionChildren();

			// Ensure arrow is pointing correct way
			UpdateTitleBarArrow();
			
			// Add ourself to the application filtering list
			Application.AddMessageFilter(this);
		}

		/// <summary>
		/// Signals the object that initialization is starting.
		/// </summary>
		public void BeginInit()
		{
			// Do not position children during initialization
			_init = true;
		}

		/// <summary>
		/// Signals the object that initialization is complete.
		/// </summary>
		public void EndInit()
		{
			_init = false;

			// Must position children correctly for final size			
			PositionChildren();
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
		/// Gets and sets the child control to display.
		/// </summary>
		[Browsable(false)]
		[Category("Appearance")]
		[Description("Child panel displayed by the title bar.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Panel Panel
		{
			get { return _slide; }
		}
		
		/// <summary>
		/// Gets and sets a value indicating if child control is opened.
		/// </summary>
		[Category("Appearance")]
		[Description("Is the child control opened for viewing.")]
		[DefaultValue(true)]
		public bool Open
		{
			get { return _open; }
			
			set
			{
				if (_open != value)
				{
					_open = value;
					_internalOpen = value;

					// Stop any current timing		
					_timer.Stop();
					_actionIn.Stop();
					_actionOut.Stop();
					
					// Automatically at the last step
					_slideStep = _slideSteps - 1;

					// Use tick handler to effect change
					OnTimerTick(_timer, EventArgs.Empty);
								
					// Raise change notification event
					OnOpenChanged();
				}
			}
		}

		/// <summary>
		/// Resets the Open property to its default value.
		/// </summary>
		public void ResetOpen()
		{
			Open = true;
		}
		
		/// <summary>
		/// If the control is slide out and displayed it is closed up.
		/// </summary>
		public void CloseUp()
		{
			// If not in open mode but it is slide out for display
			if (!Open && _internalOpen)
			{
				// Set to not being open any more
				_internalOpen = false;

				// Stop any current timing		
				_timer.Stop();
				_actionIn.Stop();
				_actionOut.Stop();
					
				// Automatically at the last step
				_slideStep = _slideSteps - 1;

				// Use tick handler to effect change
				OnTimerTick(_timer, EventArgs.Empty);
			}
		}
		
		/// <summary>
		/// Gets and sets a value indicating if mouse hover causes slide out.
		/// </summary>
		[Category("Appearance")]
		[Description("Slide out when mouse hovers over control.")]
		[DefaultValue(true)]
		public bool SlideOnHover
		{
			get { return _slideOnHover; }
			
			set
			{
				if (_slideOnHover != value)
				{
					_slideOnHover = value;

					// Raise change notification event
					OnSlideOnHoverChanged();
				}
			}
		}
		
		/// <summary>
		/// Resets the SlideOnHover property to its default value.
		/// </summary>
		public void ResetSlideOnHover()
		{
			SlideOnHover = true;
		}

		/// <summary>
		/// Gets and sets the number of steps when sliding child.
		/// </summary>
		[Category("Appearance")]
		[Description("Number of steps when sliding child.")]
		[DefaultValue(4)]
		public int SlideSteps
		{
			get { return _slideSteps; }
			
			set
			{
				// Cannot have less than one step
				if (value < 1)
					value = 1;

				if (_slideSteps != value)
				{
					_slideSteps = value;

					// Raise change notification event
					OnSlideStepsChanged();
				}
			}
		}
		
		/// <summary>
		/// Resets the SlideSteps property to its default value.
		/// </summary>
		public void ResetSlideSteps()
		{
			SlideSteps = 4;
		}

		/// <summary>
		/// Gets and sets the number of milliseconds for toal slide duration.
		/// </summary>
		[Category("Appearance")]
		[Description("Number of milliseconds for toal slide duration.")]
		[DefaultValue(150)]
		public int SlideDuration
		{
			get { return _slideDuration; }
			
			set
			{
				if (_slideDuration != value)
				{
					// Sensible limit check
					if (value < 1)
						value = 1;
						
					_slideDuration = value;
					OnSlideDurationChanged();
				}
			}
		}
		
		/// <summary>
		/// Resets the SlideSteps property to its default value.
		/// </summary>
		public void ResetSlideDuration()
		{
			SlideDuration = 150;
		}

		/// <summary>
		/// Gets and sets the number of milliseconds before hovering takes effect.
		/// </summary>
		[Category("Appearance")]
		[Description("Number of milliseconds before hovering takes effect.")]
		[DefaultValue(333)]
		public int HoverDelay
		{
			get { return _actionOut.Interval; }
			
			set
			{
				if (_actionOut.Interval != value)
				{
					_actionOut.Interval = value;
					_actionIn.Interval = value;
					OnHoverDelayChanged();
				}
			}
		}
		
		/// <summary>
		/// Resets the HoverDelay property to its default value.
		/// </summary>
		public void ResetHoverDelay()
		{
			HoverDelay = 333;
		}

		/// <summary>
		/// Gets and sets the presence of a border around the control.
		/// </summary>
		[Category("Appearance")]
		[Description("Indicates the presence of a border around the control.")]
		[DefaultValue(typeof(BorderStyle), "FixedSingle")]
		public BorderStyle BorderStyle
		{
			get { return _borderStyle; }
			
			set
			{
				if (_borderStyle != value)
				{
					_borderStyle = value;
					_borderSize = Size.Empty;
					
					switch(_borderStyle)
					{
						case BorderStyle.FixedSingle:
							_borderSize = SystemInformation.BorderSize;
							break;
						case BorderStyle.Fixed3D:
							_borderSize = SystemInformation.Border3DSize;
							break;
					}
					
					// Must position children correctly for final size			
					PositionChildren();
					
					// Raise change notification
					OnBorderStyleChanged();
					
					// Must redraw to reflect the change
					Invalidate();
				}
			}
		}
		
		/// <summary>
		/// Resets the BorderStyle property to its default value.
		/// </summary>
		public void ResetBorderStyle()
		{
			BorderStyle = BorderStyle.FixedSingle;
		}

		/// <summary>
		/// Gets and sets the color used to draw the border.
		/// </summary>
		[Category("Appearance")]
		[Description("Color used to draw the border.")]
		[DefaultValue(typeof(Color), "WindowFrame")]
		public Color BorderColor
		{
			get { return _borderColor; }
			
			set
			{
				if (_borderColor != value)
				{
					_borderColor = value;

					// Raise change notification
					OnBorderColorChanged();
					
					// Must redraw to reflect the change
					Invalidate();
				}
			}
		}
		
		/// <summary>
		/// Resets the BorderColor property to its default value.
		/// </summary>
		public void ResetBorderColor()
		{
			BorderColor = SystemColors.WindowFrame;
		}

		/// <summary>
		/// Gets and sets the font used in the TitleBar.
		/// </summary>
		[Category("TitleBar")]
		[Description("Defines the font to use in the TitleBar.")]
		public Font TitleFont
		{
			get { return _titleBar.Font; }
			set { _titleBar.Font = value; }
		}
		
		/// <summary>
		/// Resets the TitleFont property to its default value.
		/// </summary>
		public void ResetTitleFont()
		{
			_titleBar.ResetFont();
		}

		/// <summary>
		/// Gets and sets the length used to size the TitleBar.
		/// </summary>
		[Category("TitleBar")]
		[Description("Length used for the TitleBar.")]
		public int Length
		{
			get { return _titleBarLength; }

			set
			{
				_titleBarLength = value;
				
				// Must position children correctly for final size			
				PositionChildren();

				// Raise change notification
				OnLengthChanged();
			}
		}
		
		private bool ShouldSerializeLength()
		{
			return (Length != _titleBar.Font.Height * 2);
		}
		
		/// <summary>
		/// Resets the Length property to its default value.
		/// </summary>
		public void ResetLength()
		{
			Length = _titleBar.Font.Height * 2;
		}
		
		/// <summary>
		/// Gets and sets the edge to place the TitleBar against.
		/// </summary>
		[Category("TitleBar")]
		[Description("Edge to place the TitleBar against.")]
		[DefaultValue(typeof(TitleEdge), "Top")]
		public TitleEdge Edge
		{
			get { return _titleEdge; }
			
			set
			{
				if (_titleEdge != value)
				{
					// Remember starting state
					bool wasOpen = Open;
					
					if (!wasOpen)
						Open = true;
						
					_titleEdge = value;
					
					// Correct the direction for drawing title bar
					if ((_titleEdge == TitleEdge.Top) || (_titleEdge == TitleEdge.Bottom))
						_titleBar.Direction = LayoutDirection.Horizontal;
					else
						_titleBar.Direction = LayoutDirection.Vertical;
					
					// Define the anchoring to help prevent bad flicker
					// when sliding in the right or bottom positions
					switch(_titleEdge)
					{
						case TitleEdge.Top:
							_titleBar.Dock = DockStyle.Top;
							break;
						case TitleEdge.Left:
							_titleBar.Dock = DockStyle.Left;
							break;
						case TitleEdge.Right:
							_titleBar.Dock = DockStyle.Right;
							break;
						case TitleEdge.Bottom:
							_titleBar.Dock = DockStyle.Bottom;
							break;
					}

					// Put the open state back again
					if (!wasOpen)
					{
						// Must position children correctly for the Open change		
						PositionChildren();

						Open = false;
					}
					
					// Must position children correctly for final size			
					PositionChildren();

					// Ensure arrow is pointing correct way
					UpdateTitleBarArrow();

					// Raise change notification
					OnEdgeChanged();
				}
			}
		}
		
		/// <summary>
		/// Resets the Edge property to its default value.
		/// </summary>
		public void ResetEdge()
		{
			Edge = TitleEdge.Top;
		}

		/// <summary>
		/// Gets and sets a value indicating if arrows or a pin should be shown.
		/// </summary>
		[Category("Appearance")]
		[Description("Should arrows or a pin be shown.")]
		[DefaultValue(false)]
		public bool Arrows
		{
			get { return _arrows; }
			
			set
			{
				if (_arrows != value)
				{
					_arrows = value;

					// Ensure arrow is pointing correct way
					UpdateTitleBarArrow();

					// Raise change notification
					OnArrowsChanged();
				}
			}
		}
		
		/// <summary>
		/// Resets the Arrows property to its default value.
		/// </summary>
		public void ResetArrows()
		{
			Arrows = false;
		}

		/// <summary>
		/// Gets and sets the text display in the TitleBar.
		/// </summary>
		[Category("TitleBar")]
		[DefaultValue("Sliding TitleBar")]
		public new string Text
		{
			get { return _titleBar.Text; }
			set { _titleBar.Text = value; }
		}
		
		/// <summary>
		/// Resets the Text property to its default value.
		/// </summary>
		public new void ResetText()
		{
			Text = "Sliding TitleBar";
		}
		
		/// <summary>
		/// Gets and sets the background color is used when drawing text.
		/// </summary>
		[Category("TitleBar")]
		[DefaultValue(typeof(Color), "ActiveCaption")]
		public Color TitleBackColor
		{
			get { return _titleBar.BackColor; }
			set { _titleBar.BackColor = value; }
		}
        
		/// <summary>
		/// Resets the TitleBackColor property to its default value.
		/// </summary>
		public void ResetTitleBackColor()
		{
			TitleBackColor = SystemColors.ActiveCaption;;
		}

		/// <summary>
		/// Gets and sets the foreground color is used when drawing text.
		/// </summary>
		[Category("TitleBar")]
		[DefaultValue(typeof(Color), "ActiveCaptionText")]
		public Color TitleForeColor
		{
			get { return _titleBar.ForeColor; }
			set { _titleBar.ForeColor = value; }
		}
        
		/// <summary>
		/// Resets the TitleForeColor property to its default value.
		/// </summary>
		public void ResetTitleForeColor()
		{
			TitleForeColor = SystemColors.ActiveCaptionText;
		}
		
		/// <summary>
		/// Gets and sets where the text is postitioned across the control.
		/// </summary>
		[Category("TitleBar")]
		[Description("Determine where the text is postitioned across.")]
		[DefaultValue(typeof(StringAlignment), "Near")]
		public StringAlignment TextAlignment
		{
			get { return _titleBar.TextAlignment; }
			set { _titleBar.TextAlignment = value; }
		}
		
		/// <summary>
		/// Resets the TextAlignment property to its default value.
		/// </summary>
		public void ResetTextAlignment()
		{
			TextAlignment = StringAlignment.Near;
		}
		
		/// <summary>
		/// Gets or sets the visual style of the control.
		/// </summary>
		[Category("TitleBar")]
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
                    _titleBar.Style = ColorHelper.ValidateStyle(value);
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
		/// Gets and sets the text to prefix the title with.
		/// </summary>
		[Category("TitleBar")]
		[Description("Text to prefix the title.")]
		[DefaultValue("")]
		[Localizable(true)]
		public string PreText
		{
			get { return _titleBar.PreText; }
			set { _titleBar.PreText = value; }
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
		[Category("TitleBar")]
		[Description("Text to postfix the title.")]
		[DefaultValue("")]
		[Localizable(true)]
		public string PostText
		{
			get { return _titleBar.PostText; }
			set { _titleBar.PostText = value; }
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
		[Category("TitleBar")]
		[Description("Separator between PreText and Text.")]
		[DefaultValue(" - ")]
		[Localizable(true)]
		public string PreSeparator
		{
			get { return _titleBar.PreSeparator; }
			set { _titleBar.PreSeparator = value; }
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
		[Category("TitleBar")]
		[Description("Separator between Text and PostText.")]
		[DefaultValue(" - ")]
		[Localizable(true)]
		public string PostSeparator
		{
			get { return _titleBar.PostSeparator; }
			set { _titleBar.PostSeparator = value; }
		}
		/// <summary>
		/// Resets the PostSeparator property to its default value.
		/// </summary>
		public void ResetPostSeparator()
		{
			PostSeparator = " - ";
		}

		/// <summary>
		/// Gets and sets the gap between left edge and start of drawn items.
		/// </summary>
		[Category("TitleBar")]
		[Description("Gap between left edge and start of drawn items.")]
		[DefaultValue(3)]		
		public int PadLeft
		{
			get { return _titleBar.PadLeft; }
			set { _titleBar.PadLeft = value; }
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
		[Category("TitleBar")]
		[Description("Gap between right edge and end of drawn items.")]
		[DefaultValue(3)]		
		public int PadRight
		{
			get { return _titleBar.PadRight; }
			set { _titleBar.PadRight = value; }
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
		[Category("TitleBar")]
		[Description("Gap between top edge and start of drawn items.")]
		[DefaultValue(3)]		
		public int PadTop
		{
			get { return _titleBar.PadTop; }
			set { _titleBar.PadTop = value; }
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
		[Category("TitleBar")]
		[Description("Gap between bottom edge and end of drawn items.")]
		[DefaultValue(3)]		
		public int PadBottom
		{
			get { return _titleBar.PadBottom; }
			set { _titleBar.PadBottom = value; }
		}

		/// <summary>
		/// Resets the PadBottom property to its default value.
		/// </summary>
		public void ResetPadBottom()
		{
			PadBottom = 3;
		}
			
		/// <summary>
		/// Gets and sets colors used when drawing a gradient background.
		/// </summary>
		[Category("TitleBar")]
		[Description("Determine colors used when drawing a gradient background.")]
		[DefaultValue(typeof(Color), "Empty")]
		public Color MouseOverColor
		{
			get { return _titleBar.MouseOverColor; }
			set { _titleBar.MouseOverColor = value; }
		}

		/// <summary>
		/// Resets the GradientColoring property to its default value.
		/// </summary>
		public void ResetMouseOverColor()
		{
			MouseOverColor = Color.Empty;
		}

		/// <summary>
		/// Gets and sets the rendering hint to use when drawing text.
		/// </summary>
		[Category("TitleBar")]
		[Description("Specifies the rendering hint to use when drawing text.")]
		[DefaultValue(typeof(TextRenderingHint), "SystemDefault")]
		public TextRenderingHint TextRenderingHint
		{
			get { return _titleBar.TextRenderingHint; }
			set { _titleBar.TextRenderingHint = value; }
		}

		/// <summary>
		/// Resets the TextRenderingHint property to its default value.
		/// </summary>
		public void ResetTextRenderingHint()
		{
			TextRenderingHint = TextRenderingHint.SystemDefault;
		}

		/// <summary>
		/// Gets and sets where items are drawn down the control.
		/// </summary>
		[Category("TitleBar")]
		[Description("Determine where items are postitioned downwards.")]
		[DefaultValue(typeof(StringAlignment), "Center")]
		public StringAlignment LineAlignment
		{
			get { return _titleBar.LineAlignment; }
			set { _titleBar.LineAlignment = value; }
		}
		
		/// <summary>
		/// Resets the LineAlignment property to its default value.
		/// </summary>
		public void ResetLineAlignment()
		{
			LineAlignment = StringAlignment.Center;
		}

		/// <summary>
		/// Gets and sets the background color used when drawing the control inactive.
		/// </summary>
		[Category("TitleBar")]
		[Description("Background color used when drawing the control inactive.")]
		[DefaultValue(typeof(Color), "InactiveCaption")]
		public Color InactiveBackColor
		{
			get { return _titleBar.InactiveBackColor; }
			set { _titleBar.InactiveBackColor = value; }
		}

		/// <summary>
		/// Resets the InactiveBackColor property to its default value.
		/// </summary>
		public void ResetInactiveBackColor()
		{
			InactiveBackColor = SystemColors.InactiveCaption;
		}

		/// <summary>
		/// The text color used when drawing the control inactive.
		/// </summary>
		[Category("TitleBar")]
		[Description("Text color used when drawing the control inactive.")]
		[DefaultValue(typeof(Color), "InactiveCaptionText")]
		public Color InactiveForeColor
		{
			get { return _titleBar.InactiveForeColor; }
			set { _titleBar.InactiveForeColor = value; }
		}

		/// <summary>
		/// Resets the InactiveForeColor property to its default value.
		/// </summary>
		public void ResetInactiveForeColor()
		{
			InactiveForeColor = SystemColors.InactiveCaptionText;
		}

		/// <summary>
		/// Gets and sets the icon to draw in the title.
		/// </summary>
		[Category("TitleBar")]
		[Description("Icon to draw in the title.")]
		[DefaultValue(null)]
		public Icon Icon
		{
			get { return _titleBar.Icon; }
			set { _titleBar.Icon = value; }
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
		[Category("TitleBar")]
		[Description("Image to draw in the title.")]
		[DefaultValue(null)]
		public Image Image
		{
			get { return _titleBar.Image; }
			set { _titleBar.Image = value; }
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
		[Category("TitleBar")]
		[Description("Source of images associated with ImageIndex.")]
		[DefaultValue(null)]
		public ImageList ImageList
		{
			get { return _titleBar.ImageList; }
			set { _titleBar.ImageList = value; }
		}
		
		/// <summary>
		/// Resets the ImageList property to its default value.
		/// </summary>
		public void ResetImageList()
		{
			ImageList = null;
		}

		/// <summary>
		/// Gets and sets the index into associated ImageList.
		/// </summary>
		[Category("TitleBar")]
		[Description("Index into associated ImageList.")]
		[DefaultValue(0)]
		public int ImageIndex
		{
			get { return _titleBar.ImageIndex; }
			set { _titleBar.ImageIndex = value; }
		}
		
		/// <summary>
		/// Resets the ImageIndex property to its default value.
		/// </summary>
		public void ResetImageIndex()
		{
			ImageIndex = 0;
		}

		/// <summary>
		/// Gets and sets the position of image/icon.
		/// </summary>
		[Category("TitleBar")]
		[Description("Position of image/icon.")]
		[DefaultValue(typeof(ImageAlignment), "Near")]
		public ImageAlignment ImageAlignment
		{
			get { return _titleBar.ImageAlignment; }
			set { _titleBar.ImageAlignment = value; }
		}
		
		/// <summary>
		/// Resets the ImageAlignment property to its default value.
		/// </summary>
		public void ResetImageAlignment()
		{
			ImageAlignment = ImageAlignment.Near;
		}

		/// <summary>
		/// Gets and sets the gap between image and start of text.
		/// </summary>
		[Category("TitleBar")]
		[Description("Gap between image and start of text.")]
		[DefaultValue(3)]		
		public int ImageToTextGap
		{
			get { return _titleBar.ImageToTextGap; }
			set { _titleBar.ImageToTextGap = value; }
		}

		/// <summary>
		/// Resets the ImageToTextGap property to its default value.
		/// </summary>
		public void ResetImageToTextGap()
		{
			ImageToTextGap = 3;
		}

		/// <summary>
		/// Gets and sets the policy used for drawing background.
		/// </summary>
		[Category("TitleBar")]
		[Description("Policy used to draw the background.")]
		public GradientDirection GradientDirection
		{
			get { return _titleBar.GradientDirection; }
			set { _titleBar.GradientDirection = value; }
		}

		private bool ShouldSerializeGradientDirection()
		{
			return (GradientDirection != GradientDirection.None);
		}

		/// <summary>
		/// Resets the GradientDirection property to its default value.
		/// </summary>
		public void ResetGradientDirection()
		{
			_titleBar.ResetGradientDirection();
		}

		/// <summary>
		/// Gets and sets colors used when drawing a gradient background.
		/// </summary>
		[Category("TitleBar")]
		[Description("Determine colors used when drawing a gradient background.")]
		public GradientColoring GradientColoring
		{
			get { return _titleBar.GradientColoring; }
			set { _titleBar.GradientColoring = value; }
		}

		private bool ShouldSerializeGradientColoring()
		{
			return (GradientColoring != GradientColoring.LightBackToDarkBack);
		}

		/// <summary>
		/// Resets the GradientColoring property to its default value.
		/// </summary>
		public void ResetGradientColoring()
		{
			_titleBar.ResetGradientColoring();
		}

		/// <summary>
		/// Gets and sets the second color available for active gradients.
		/// </summary>
		[Category("TitleBar")]
		[Description("Second color available for active gradients.")]
		public Color GradientActiveColor
		{
			get { return _titleBar.GradientActiveColor; }
			set { _titleBar.GradientActiveColor = value; }
		}

		private bool ShouldSerializeGradientActiveColor()
		{
			return (GradientActiveColor != _titleBar.GetGradientActiveColor());
		}

		/// <summary>
		/// Resets the GradientActiveColor property to its default value.
		/// </summary>
		public void ResetGradientActiveColor()
		{
			_titleBar.ResetGradientActiveColor();
		}

		/// <summary>
		/// Gets and sets the second color available for inactive gradients.
		/// </summary>
		[Category("TitleBar")]
		[Description("Second color available for inactive gradients.")]
		public Color GradientInactiveColor
		{
			get { return _titleBar.GradientInactiveColor; }
			set { _titleBar.GradientInactiveColor = value; }
		}

		private bool ShouldSerializeGradientInactiveColor()
		{
			return (GradientInactiveColor != _titleBar.GetGradientInactiveColor());
		}

		/// <summary>
		/// Resets the GradientInactiveColor property to its default value.
		/// </summary>
		public void ResetGradientInactiveColor()
		{
			_titleBar.ResetGradientInactiveColor();
		}

		/// <summary>
		/// Gets and sets the gap between edge and start of image.
		/// </summary>
		[Category("TitleBar")]
		[Description("Gap between edge and start of image.")]
		[DefaultValue(0)]		
		public int EdgeToImageGap
		{
			get { return _titleBar.EdgeToImageGap; }
			set { _titleBar.EdgeToImageGap = value; }
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
		[Category("TitleBar")]
		[Description("Gap between edge and start of arrow.")]
		[DefaultValue(3)]		
		public int EdgeToArrowGap
		{
			get { return _titleBar.EdgeToArrowGap; }
			set { _titleBar.EdgeToArrowGap = value; }
		}

		/// <summary>
		/// Resets the EdgeToArrowGap property to its default value.
		/// </summary>
		public void ResetEdgeToArrowGap()
		{
			EdgeToArrowGap = 3;
		}

		/// <summary>
		/// Gets and sets the position of arrow.
		/// </summary>
		[Category("TitleBar")]
		[Description("Position of arrow.")]
		[DefaultValue(typeof(ImageAlignment), "Far")]
		public ImageAlignment ArrowAlignment
		{
			get { return _titleBar.ArrowAlignment; }
			set { _titleBar.ArrowAlignment = value; }
		}
		
		/// <summary>
		/// Resets the ArrowAlignment property to its default value.
		/// </summary>
		public void ResetArrowAlignment()
		{
			ArrowAlignment = ImageAlignment.Far;
		}

		/// <summary>
		/// Filters out a message before it is dispatched.
		/// </summary>
		/// <param name="msg">The message to be dispatched. You cannot modify this message. </param>
		/// <returns>true to filter out; false otherwise.</returns>
		public bool PreFilterMessage(ref Message msg)
		{
			Form parentForm = this.FindForm();

			// Only interested if the Form we are on contains the focus and we have not been disposed
			if ((parentForm != null) && (parentForm == Form.ActiveForm) && 
				parentForm.ContainsFocus && !IsDisposed)
			{		
				switch(msg.Msg)
				{
					case (int)Win32.Msgs.WM_MOUSEMOVE:
						// Should the control be closed up unless mouse is over it?
						if (!Open)
						{
							Win32.POINT screenPos;
							screenPos.x = (int)((uint)msg.LParam & 0x0000FFFFU);
							screenPos.y = (int)(((uint)msg.LParam & 0xFFFF0000U) >> 16);

							// Convert the mouse position to screen coordinates
							User32.ClientToScreen(msg.HWnd, ref screenPos);

							// Get the screen rectangle for ourself
							Rectangle panelRect = RectangleToScreen(ClientRectangle);

							// Is mouse moving over the control?
							if (panelRect.Contains(new Point(screenPos.x, screenPos.y)))
							{
								// If the dismiss timer is running...
								if (_actionInValid)
								{
									// ...then stop it now, we do not want to dismiss the
									// sliding panel whilst the mouse is over the control
									_actionIn.Stop();
									_actionInValid = false;
								}
							}
							else
							{
								// If mouse is not over the control and the slide has not completed
								if (!_actionInValid && _internalOpen)
								{
									// ...then need to restart the timer to get the slide to restart
									_actionIn.Start();
									_actionInValid = true;
								}
							}
						}
						break;
				}
			}

			return false;
		}

        /// <summary>
        /// Gets direct access to the embedded TitleBar instance.
        /// </summary>
        protected TitleBar TitleBar
        {
            get { return _titleBar; }
        }

        /// <summary>
		/// Releases all resources used by the SliderTitleBar.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Kill the timers
				_timer.Stop();
				_actionIn.Stop();
				_actionOut.Stop();
				
				// Dispose of timers
				_timer.Dispose();
				_actionIn.Dispose();
				_actionOut.Dispose();
			
				// Remove ourself from the application filtering list
				Application.RemoveMessageFilter(this);

				// Unhook all events from titlebar
				_titleBar.TextChanged -= new EventHandler(OnTitleBarTextChanged);
				_titleBar.TextAlignmentChanged -= new EventHandler(OnTitleBarTextAlignmentChanged);
				_titleBar.LineAlignmentChanged -= new EventHandler(OnTitleBarLineAlignmentChanged);
				_titleBar.ForeColorChanged -= new EventHandler(OnTitleBarForeColorChanged);
				_titleBar.BackColorChanged -= new EventHandler(OnTitleBarBackColorChanged);
				_titleBar.VisualStyleChanged -= new EventHandler(OnTitleBarStyleChanged);
				_titleBar.PreTextChanged -= new EventHandler(OnTitleBarPreTextChanged);
				_titleBar.PostTextChanged -= new EventHandler(OnTitleBarPostTextChanged);
				_titleBar.PreSeparatorChanged -= new EventHandler(OnTitleBarPreSeparatorChanged);
				_titleBar.PostSeparatorChanged -= new EventHandler(OnTitleBarPostSeparatorChanged);
				_titleBar.PadLeftChanged -= new EventHandler(OnTitleBarPadLeftChanged);
				_titleBar.PadRightChanged -= new EventHandler(OnTitleBarPadRightChanged);
				_titleBar.PadTopChanged -= new EventHandler(OnTitleBarPadTopChanged);
				_titleBar.PadBottomChanged -= new EventHandler(OnTitleBarPadBottomChanged);
				_titleBar.MouseOverColorChanged -= new EventHandler(OnTitleBarMouseOverColorChanged);
				_titleBar.InactiveBackColorChanged -= new EventHandler(OnTitleBarInactiveBackColorChanged);
				_titleBar.InactiveForeColorChanged -= new EventHandler(OnTitleBarInactiveForeColorChanged);
				_titleBar.IconChanged -= new EventHandler(OnTitleBarIconChanged);
				_titleBar.ImageChanged -= new EventHandler(OnTitleBarImageChanged);
				_titleBar.ImageListChanged -= new EventHandler(OnTitleBarImageListChanged);
				_titleBar.ImageIndexChanged -= new EventHandler(OnTitleBarImageIndexChanged);
				_titleBar.ImageAlignmentChanged -= new EventHandler(OnTitleBarImageAlignmentChanged);
				_titleBar.ImageToTextGapChanged	-= new EventHandler(OnTitleBarImageToTextGapChanged);
				_titleBar.GradientDirectionChanged -= new EventHandler(OnTitleBarGradientDirectionChanged);
				_titleBar.GradientColoringChanged -= new EventHandler(OnTitleBarGradientColoringChanged);
				_titleBar.GradientActiveColorChanged -= new EventHandler(OnTitleBarGradientActiveColorChanged);
				_titleBar.GradientInactiveColorChanged -= new EventHandler(OnTitleBarGradientInactiveColorChanged);
				_titleBar.EdgeToImageGapChanged -= new EventHandler(OnTitleBarEdgeToImageGapChanged);
				_titleBar.EdgeToArrowGapChanged -= new EventHandler(OnTitleBarEdgeToArrowGapChanged);
				_titleBar.ArrowAlignmentChanged -= new EventHandler(OnTitleBarArrowAlignmentChanged);
				_titleBar.TextRenderingHintChanged -= new EventHandler(OnTitleBarTextRenderingHintChanged);
			}
		
			base.Dispose(disposing);
		}

		/// <summary>
		/// Raises the Resize event.
		/// </summary>
		/// <param name="levent">An EventArgs that contains the event data. </param>
		protected override void OnResize(EventArgs levent)
		{
			base.OnResize(levent);

			// Must position children correctly for final size			
			PositionChildren();
		}

		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			// Must remember to call base class first
			base.OnPaint(e);

			// Draw any required border style			
			switch(BorderStyle)
			{
				case BorderStyle.FixedSingle:
                    Color borderColor = BorderColor;

                    // If the border color is defaulted
                    if (borderColor == SystemColors.WindowFrame)
                    {
                        // Then some styles use a calculated color instead
                        switch (_titleBar.Style)
                        {
                            case VisualStyle.Office2007Blue:
                            case VisualStyle.Office2007Silver:
                            case VisualStyle.Office2007Black:
                                borderColor = Office2007ColorTable.TitleBorderColorDark(Style);
                                break;
                            case VisualStyle.MediaPlayerBlue:
                            case VisualStyle.MediaPlayerOrange:
                            case VisualStyle.MediaPlayerPurple:
                                borderColor = MediaPlayerColorTable.TitleBorderColorDark(Style);
                                break;
                        }
                    }

                    ControlPaint.DrawBorder(e.Graphics, ClientRectangle, borderColor, ButtonBorderStyle.Solid);
					break;
				case BorderStyle.Fixed3D:
					ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, Border3DStyle.Sunken);
					break;
			}
		}

		/// <summary>
		/// Raises the OpenChanged event.
		/// </summary>
		protected virtual void OnOpenChanged()
		{
			if (OpenChanged != null)
				OpenChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the EdgeChanged event.
		/// </summary>
		protected virtual void OnEdgeChanged()
		{
			if (EdgeChanged != null)
				EdgeChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the ArrowsChanged event.
		/// </summary>
		protected virtual void OnArrowsChanged()
		{
			if (ArrowsChanged != null)
				ArrowsChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the LengthChanged event.
		/// </summary>
		protected virtual void OnLengthChanged()
		{
			if (LengthChanged != null)
				LengthChanged(this, EventArgs.Empty);
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
		/// Raises the SlideStepsChanged event.
		/// </summary>
		protected virtual void OnSlideStepsChanged()
		{
			if (SlideStepsChanged != null)
				SlideStepsChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the SlideDurationChanged event.
		/// </summary>
		protected virtual void OnSlideDurationChanged()
		{
			if (SlideDurationChanged != null)
				SlideDurationChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the HoverDelayChanged event.
		/// </summary>
		protected virtual void OnHoverDelayChanged()
		{
			if (HoverDelayChanged != null)
				HoverDelayChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the SlideOnHoverChanged event.
		/// </summary>
		protected virtual void OnSlideOnHoverChanged()
		{
			if (SlideOnHoverChanged != null)
				SlideOnHoverChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the TextChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarTextChanged(object sender, EventArgs e)
		{
			if (TitleTextChanged != null)
				TitleTextChanged(this, e);
		}

		/// <summary>
		/// Raises the TextAlignmentChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarTextAlignmentChanged(object sender, EventArgs e)
		{
			if (TextAlignmentChanged != null)
				TextAlignmentChanged(this, e);
		}

		/// <summary>
		/// Raises the LineAlignmentChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarLineAlignmentChanged(object sender, EventArgs e)
		{
			if (LineAlignmentChanged != null)
				LineAlignmentChanged(this, e);
		}

		/// <summary>
		/// Raises the TitleForeColorChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarForeColorChanged(object sender, EventArgs e)
		{
			if (TitleForeColorChanged != null)
				TitleForeColorChanged(this, e);
		}

		/// <summary>
		/// Raises the TitleBackColorChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarBackColorChanged(object sender, EventArgs e)
		{
			if (TitleBackColorChanged != null)
				TitleBackColorChanged(this, e);
		}

		/// <summary>
		/// Raises the StyleChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		private void OnTitleBarStyleChanged(object sender, EventArgs e)
		{
			if (VisualStyleChanged != null)
				VisualStyleChanged(this, e);
		}

		/// <summary>
		/// Raises the LineAlignmentChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarSizeChanged(object sender, EventArgs e)
		{
			if (LineAlignmentChanged != null)
				LineAlignmentChanged(this, e);
		}

		/// <summary>
		/// Raises the PreTextChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarPreTextChanged(object sender, EventArgs e)
		{
			if (PreTextChanged != null)
				PreTextChanged(this, e);
		}

		/// <summary>
		/// Raises the PostTextChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarPostTextChanged(object sender, EventArgs e)
		{
			if (PostTextChanged != null)
				PostTextChanged(this, e);
		}

		/// <summary>
		/// Raises the PreSeparatorChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarPreSeparatorChanged(object sender, EventArgs e)
		{
			if (PreSeparatorChanged != null)
				PreSeparatorChanged(this, e);
		}

		/// <summary>
		/// Raises the PostSeparatorChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarPostSeparatorChanged(object sender, EventArgs e)
		{
			if (PostSeparatorChanged != null)
				PostSeparatorChanged(this, e);
		}

		/// <summary>
		/// Raises the PadLeftChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarPadLeftChanged(object sender, EventArgs e)
		{
			if (PadLeftChanged != null)
				PadLeftChanged(this, e);
		}

		/// <summary>
		/// Raises the PadRightChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarPadRightChanged(object sender, EventArgs e)
		{
			if (PadRightChanged != null)
				PadRightChanged(this, e);
		}

		/// <summary>
		/// Raises the PadTopChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarPadTopChanged(object sender, EventArgs e)
		{
			if (PadTopChanged != null)
				PadTopChanged(this, e);
		}

		/// <summary>
		/// Raises the PadBottomChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarPadBottomChanged(object sender, EventArgs e)
		{
			if (PadBottomChanged != null)
				PadBottomChanged(this, e);
		}

		/// <summary>
		/// Raises the MouseOverColorChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarMouseOverColorChanged(object sender, EventArgs e)
		{
			if (MouseOverColorChanged != null)
				MouseOverColorChanged(this, e);
		}

		/// <summary>
		/// Raises the TextRenderingHintChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarTextRenderingHintChanged(object sender, EventArgs e)
		{
			if (TextRenderingHintChanged != null)
				TextRenderingHintChanged(this, e);
		}

		/// <summary>
		/// Raises the InactiveBackColorChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarInactiveBackColorChanged(object sender, EventArgs e)
		{
			if (InactiveBackColorChanged != null)
				InactiveBackColorChanged(this, e);
		}

		/// <summary>
		/// Raises the InactiveForeColorChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarInactiveForeColorChanged(object sender, EventArgs e)
		{
			if (InactiveForeColorChanged != null)
				InactiveForeColorChanged(this, e);
		}

		/// <summary>
		/// Raises the IconChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarIconChanged(object sender, EventArgs e)
		{
			if (IconChanged != null)
				IconChanged(this, e);
		}

		/// <summary>
		/// Raises the ImageChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarImageChanged(object sender, EventArgs e)
		{
			if (ImageChanged != null)
				ImageChanged(this, e);
		}

		/// <summary>
		/// Raises the ImageListChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarImageListChanged(object sender, EventArgs e)
		{
			if (ImageListChanged != null)
				ImageListChanged(this, e);
		}

		/// <summary>
		/// Raises the ImageIndexChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarImageIndexChanged(object sender, EventArgs e)
		{
			if (ImageIndexChanged != null)
				ImageIndexChanged(this, e);
		}

		/// <summary>
		/// Raises the ImageAlignmentChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarImageAlignmentChanged(object sender, EventArgs e)
		{
			if (ImageAlignmentChanged != null)
				ImageAlignmentChanged(this, e);
		}

		/// <summary>
		/// Raises the ImageToTextGapChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarImageToTextGapChanged(object sender, EventArgs e)
		{
			if (ImageToTextGapChanged != null)
				ImageToTextGapChanged(this, e);
		}

		/// <summary>
		/// Raises the GradientDirectionChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarGradientDirectionChanged(object sender, EventArgs e)
		{
			if (GradientDirectionChanged != null)
				GradientDirectionChanged(this, e);
		}

		/// <summary>
		/// Raises the GradientColoringChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarGradientColoringChanged(object sender, EventArgs e)
		{
			if (GradientColoringChanged != null)
				GradientColoringChanged(this, e);
		}

		/// <summary>
		/// Raises the GradientActiveColorChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarGradientActiveColorChanged(object sender, EventArgs e)
		{
			if (GradientActiveColorChanged != null)
				GradientActiveColorChanged(this, e);
		}

		/// <summary>
		/// Raises the GradientInactiveColorChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarGradientInactiveColorChanged(object sender, EventArgs e)
		{
			if (GradientInactiveColorChanged != null)
				GradientInactiveColorChanged(this, e);
		}

		/// <summary>
		/// Raises the EdgeToArrowGapChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarEdgeToArrowGapChanged(object sender, EventArgs e)
		{
			if (EdgeToArrowGapChanged != null)
				EdgeToArrowGapChanged(this, e);
		}

		/// <summary>
		/// Raises the EdgeToImageGapChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarEdgeToImageGapChanged(object sender, EventArgs e)
		{
			if (EdgeToImageGapChanged != null)
				EdgeToImageGapChanged(this, e);
		}

		/// <summary>
		/// Raises the ArrowAlignmentChanged event.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected virtual void OnTitleBarArrowAlignmentChanged(object sender, EventArgs e)
		{
			if (ArrowAlignmentChanged != null)
				ArrowAlignmentChanged(this, e);
		}
		
		private void CreateTimers()
		{
			_timer = new Timer();
			_timer.Tick += new EventHandler(OnTimerTick);
			_actionOut = new Timer();
			_actionOut.Tick += new EventHandler(OnActionOutTick);
			_actionIn = new Timer();
			_actionIn.Tick += new EventHandler(OnActionInTick);	
		}

		private void CreateInnerPanel()
		{
			// Create panel for placing the title bar and child panel onto
			_innerPanel = new Panel();
			
			// Add as the only child of ourself
			Controls.Add(_innerPanel);
		}

		private void CreateTitleBar()
		{
			// Create our title bar instance
			_titleBar = new TitleBar();

			// Hook into the property change events
			_titleBar.MouseEnter += new EventHandler(OnTitleBarMouseEnter);
			_titleBar.MouseLeave += new EventHandler(OnTitleBarMouseLeave);
			_titleBar.TextChanged += new EventHandler(OnTitleBarTextChanged);
			_titleBar.TextAlignmentChanged += new EventHandler(OnTitleBarTextAlignmentChanged);
			_titleBar.LineAlignmentChanged += new EventHandler(OnTitleBarLineAlignmentChanged);
			_titleBar.ForeColorChanged += new EventHandler(OnTitleBarForeColorChanged);
			_titleBar.BackColorChanged += new EventHandler(OnTitleBarBackColorChanged);
			_titleBar.VisualStyleChanged += new EventHandler(OnTitleBarStyleChanged);
			_titleBar.PreTextChanged += new EventHandler(OnTitleBarPreTextChanged);
			_titleBar.PostTextChanged += new EventHandler(OnTitleBarPostTextChanged);
			_titleBar.PreSeparatorChanged += new EventHandler(OnTitleBarPreSeparatorChanged);
			_titleBar.PostSeparatorChanged += new EventHandler(OnTitleBarPostSeparatorChanged);
			_titleBar.PadLeftChanged += new EventHandler(OnTitleBarPadLeftChanged);
			_titleBar.PadRightChanged += new EventHandler(OnTitleBarPadRightChanged);
			_titleBar.PadTopChanged += new EventHandler(OnTitleBarPadTopChanged);
			_titleBar.PadBottomChanged += new EventHandler(OnTitleBarPadBottomChanged);
			_titleBar.MouseOverColorChanged += new EventHandler(OnTitleBarMouseOverColorChanged);
			_titleBar.InactiveBackColorChanged += new EventHandler(OnTitleBarInactiveBackColorChanged);
			_titleBar.InactiveForeColorChanged += new EventHandler(OnTitleBarInactiveForeColorChanged);
			_titleBar.IconChanged += new EventHandler(OnTitleBarIconChanged);
			_titleBar.ImageChanged += new EventHandler(OnTitleBarImageChanged);
			_titleBar.ImageListChanged += new EventHandler(OnTitleBarImageListChanged);
			_titleBar.ImageIndexChanged += new EventHandler(OnTitleBarImageIndexChanged);
			_titleBar.ImageAlignmentChanged += new EventHandler(OnTitleBarImageAlignmentChanged);
			_titleBar.ImageToTextGapChanged	+= new EventHandler(OnTitleBarImageToTextGapChanged);
			_titleBar.GradientDirectionChanged += new EventHandler(OnTitleBarGradientDirectionChanged);
			_titleBar.GradientColoringChanged += new EventHandler(OnTitleBarGradientColoringChanged);
			_titleBar.GradientActiveColorChanged += new EventHandler(OnTitleBarGradientActiveColorChanged);
			_titleBar.GradientInactiveColorChanged += new EventHandler(OnTitleBarGradientInactiveColorChanged);
			_titleBar.EdgeToImageGapChanged += new EventHandler(OnTitleBarEdgeToImageGapChanged);
			_titleBar.EdgeToArrowGapChanged += new EventHandler(OnTitleBarEdgeToArrowGapChanged);
			_titleBar.ArrowAlignmentChanged += new EventHandler(OnTitleBarArrowAlignmentChanged);
			_titleBar.ButtonClick += new EventHandler(OnTitleBarButtonClick);
			_titleBar.TextRenderingHintChanged += new EventHandler(OnTitleBarTextRenderingHintChanged);
			
			// Add it to our collection of child controls
			_innerPanel.Controls.Add(_titleBar);
		}
		
		private void CreateChildPanel()
		{
			// Create the Panel that acts as the child
			_slide = new Panel();
			_slide.BackColor = SystemColors.ControlLight;

			// Add into the collection of children, after the titlebar			
			_innerPanel.Controls.Add(_slide);
		}

		private void PositionChildren()
		{
			// Cannot change positions during initalization
			if (!_init && (_titleBar != null))
			{
				// Start with total available space
				Rectangle space = ClientRectangle;
				
				// Reduce by the required border
				space.Inflate(-_borderSize.Width, -_borderSize.Height);
				
				// Position the inner panel using this size
				_innerPanel.SetBounds(space.X, space.Y, space.Width, space.Height);
				
				// Update space rectangle to reflect inner panel	
				space = _innerPanel.ClientRectangle;
				
				int vector = _titleBarLength;
				
				// Limit check depends on edge
				if ((Edge == TitleEdge.Top) || (Edge == TitleEdge.Bottom))
				{
					// Limit height to available space
					if (vector > space.Height)
						vector = space.Height;
				}
				else
				{
					// Limit width to available space
					if (vector > space.Width)
						vector = space.Width;
				}
				
				// Position the title bar first
				switch(Edge)
				{
					case TitleEdge.Top:
						_titleBar.SetBounds(space.Left, space.Top, space.Width, vector);
						space.Y += vector;
						space.Height -= vector;
						break;
					case TitleEdge.Bottom:
						_titleBar.SetBounds(space.Left, space.Bottom - vector, space.Width, _titleBarLength);
						space.Height -= vector;
						break;
					case TitleEdge.Left:
						_titleBar.SetBounds(space.Left, space.Top, vector, space.Height);
						space.X += vector;
						space.Width -= vector;
						break;
					case TitleEdge.Right:
						_titleBar.SetBounds(space.Right - vector, space.Top, vector, space.Height);
						space.Width -= vector;
						break;
				}

				// Are we currently sliding in or out of view
				if (_sliding)
				{
					// Do not want to change the size of the panel, just change its position
					switch(Edge)
					{
						case TitleEdge.Top:
							_slide.Location = new Point(space.Left, _innerPanel.Height - _slide.Height);
							break;
						case TitleEdge.Left:
							_slide.Location = new Point(_innerPanel.Width - _slide.Width, space.Top);
							break;
						case TitleEdge.Bottom:
						case TitleEdge.Right:
							_slide.Location = new Point(space.Left, space.Top);
							break;
					}

					// Should always be visible when sliding
					_slide.Visible = true;
						
				}
				else
				{				
					// Positon and visibilty depend on open state
					if (_internalOpen)
					{
						// Is there any space left for the child Panel?
						if ((space.Width > 0) && (space.Height > 0))
							_slide.SetBounds(space.Left, space.Top, space.Width, space.Height);
												
						// Ensure the panel can be seen
						_slide.Visible = true;
					}
					else
					{
						// Should not be visible
						_slide.Visible = false;
					}
				}
			}
		}

		private void OnTimerTick(object sender, EventArgs e)
		{
			// Move to next step
			_slideStep++;
			
			// If finished sliding....
			if (_slideStep >= _slideSteps)
			{
				// No need for the timer again
				_timer.Stop();
				
				// No longer sliding
				_sliding = false;
				
				// Ensure arrow is pointing correct way
				UpdateTitleBarArrow();
			}

			// Work out percentage of way towards target
			double percent = (double)_slideStep / (double)SlideSteps;

			// When the title is at the top or bottom of the control....
			if (Edge == TitleEdge.Top)
			{
				// Minimum height is always the title bar plus its borders
				int closedHeight = _titleBarLength + _borderSize.Height * 2;

				// Depending on opening/closing, we find new height
				if (_internalOpen)
				{
					// Set new height as percentage distance towards opened height
					this.Height = (int)(closedHeight + (_slide.Height * percent));
				}
				else
				{
					// Set new height as percentage distance towards closed height
					this.Height = (int)(closedHeight + _slide.Height - (_slide.Height * percent));
				}
			}
			else if (Edge == TitleEdge.Bottom)
			{
				// Minimum height is always the title bar plus its borders
				int closedHeight = _titleBarLength + _borderSize.Height * 2;

				int percentHeight = 0;
				
				// Depending on opening/closing, we find new height based on percentage completed
				if (_internalOpen)
					percentHeight = (int)(closedHeight + (_slide.Height * percent));
				else
					percentHeight = (int)(closedHeight + _slide.Height - (_slide.Height * percent));

				Rectangle oldBounds = this.Bounds;
			
				// We have to prevent ourself from Repainting along with all the child controls
				// until we have positioned ourself and all the child controls. Otherwise the 
				// act of repositioning the children causes drawing that causes flicker.
				User32.SendMessage(this.Handle, (int)Win32.Msgs.WM_SETREDRAW, 0, 0);
			
				// Set bounds so that the location and size are updated at same time
				this.SetBounds(this.Left, this.Bottom - percentHeight, this.Width, percentHeight);

				// Must position the children before updating bounds
				PositionChildren();

				User32.SendMessage(this.Handle, (int)Win32.Msgs.WM_SETREDRAW, 1, 0);
				
				// Ensure area occupied by the control is redrawn
				Parent.Invalidate(oldBounds);

				// We have to invalidate not just ourself but all the controls in the child 
				// hierarchy to ensure they are painted correctly. Using the WM_SETDRAW means 
				// they think they are uptodate with painting but in practice are not.				
				RecursiveInvalidate(this);
			}
			else if (Edge == TitleEdge.Left)
			{
				// Minimum width is always the title bar plus its borders
				int closedWidth = _titleBarLength + _borderSize.Width * 2;

				// Depending on opening/closing, we find new width
				if (_internalOpen)
				{
					// Set new height as percentage distance towards opened height
					this.Width = (int)(closedWidth + (_slide.Width * percent));
				}
				else
				{
					// Set new height as percentage distance towards closed height
					this.Width = (int)(closedWidth + _slide.Width - (_slide.Width * percent));
				}
			}
			else
			{
				// Minimum height is always the title bar plus its borders
				int closedWidth = _titleBarLength + _borderSize.Width * 2;

				int percentWidth = 0;
				
				// Depending on opening/closing, we find new height based on percentage completed
				if (_internalOpen)
					percentWidth = (int)(closedWidth + (_slide.Width * percent));
				else
					percentWidth = (int)(closedWidth + _slide.Width - (_slide.Width * percent));

				Rectangle oldBounds = this.Bounds;

				// We have to prevent ourself from Repainting along with all the child controls
				// until we have positioned ourself and all the child controls. Otherwise the 
				// act of repositioning the children causes drawing that causes flicker.
				User32.SendMessage(this.Handle, (int)Win32.Msgs.WM_SETREDRAW, 0, 0);
			
				// Set bounds so that the location and size are updated at same time
				this.SetBounds(this.Right - percentWidth, this.Top, percentWidth, this.Height);

				// Must position the children before updating bounds
				PositionChildren();

				User32.SendMessage(this.Handle, (int)Win32.Msgs.WM_SETREDRAW, 1, 0);
				
				// Ensure area occupied by the control is redrawn
				Parent.Invalidate(oldBounds);

				// We have to invalidate not just ourself but all the controls in the child 
				// hierarchy to ensure they are painted correctly. Using the WM_SETDRAW means 
				// they think they are uptodate with painting but in practice are not.				
				RecursiveInvalidate(this);
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

		private void UpdateTitleBarArrow()
		{
			if (Arrows)
			{
				switch(Edge)
				{
					case TitleEdge.Left:
						if (Open)
						_titleBar.ArrowButton = ArrowButton.LeftArrow;
						else
						_titleBar.ArrowButton = ArrowButton.RightArrow;
						break;
					case TitleEdge.Right:
						if (Open)
							_titleBar.ArrowButton = ArrowButton.RightArrow;
						else
							_titleBar.ArrowButton = ArrowButton.LeftArrow;
						break;
					case TitleEdge.Top:
						if (Open)
							_titleBar.ArrowButton = ArrowButton.UpArrow;
						else
							_titleBar.ArrowButton = ArrowButton.DownArrow;
						break;
					case TitleEdge.Bottom:
						if (Open)
							_titleBar.ArrowButton = ArrowButton.DownArrow;
						else
							_titleBar.ArrowButton = ArrowButton.UpArrow;
						break;
				}
			}
			else
			{
				_titleBar.ArrowButton = Open ? ArrowButton.Unpinned : ArrowButton.Pinned;
			}
		}

		private void OnTitleBarButtonClick(object sender, EventArgs e)
		{
			// Invert the open status of the control
			Open = !Open;
		}

		private void OnTitleBarMouseEnter(object sender, EventArgs e)
		{
			_actionInValid = false;
			_actionOutValid = false;
			_actionIn.Stop();
			_actionOut.Stop();
			
			// Only if closed do we need to open on hovering
			if (SlideOnHover && !Open)
			{
				_actionOut.Start();
				_actionOutValid = true;
			}
		}

		private void OnTitleBarMouseLeave(object sender, EventArgs e)
		{
			_actionInValid = false;
			_actionOutValid = false;
			_actionIn.Stop();
			_actionOut.Stop();
			
			// Only if closed do we reclose on hover leaving
			if (!Open)
			{
				_actionIn.Start();
				_actionInValid = true;
			}
		}
		
		private void OnActionOutTick(object sender, EventArgs e)
		{
			// Stop timer from occuring again
			_actionOut.Stop();
			
			// Is there actually anything to do?
			if (_actionOutValid && !_internalOpen)
			{
				// Change our internal display option
				_internalOpen = true;
				
				// We should start sliding					
				_sliding = true;

				// Start back at the beginning of sliding
				_slideStep = 0;

				// Stop any current timing		
				_timer.Stop();
						
				// Define the correct interval for number of steps
				_timer.Interval = SlideDuration / SlideSteps;
						
				// Start again
				_timer.Start();
			}
		}

		private void OnActionInTick(object sender, EventArgs e)
		{
			// Stop timer from occuring again
			_actionIn.Stop();
			
			// Is there actually anything to do?
			if (_actionInValid && _internalOpen)
			{
				// Change our internal display option
				_internalOpen = false;
				
				// We should start sliding					
				_sliding = true;

				// Start back at the beginning of sliding
				_slideStep = 0;

				// Stop any current timing		
				_timer.Stop();
						
				// Define the correct interval for number of steps
				_timer.Interval = SlideDuration / SlideSteps;
						
				// Start again
				_timer.Start();
			}
		}
	}
	
	/// <summary>
	/// Specifies an edge.
	/// </summary>
	public enum TitleEdge
	{
		/// <summary>
		/// Upper vertical edge.
		/// </summary>
		Top,

		/// <summary>
		/// Near horizontal edge.
		/// </summary>
		Left,

		/// <summary>
		/// Lower vertical edge.
		/// </summary>
		Bottom,

		/// <summary>
		/// Far horizontal edge.
		/// </summary>
		Right
	}
}
