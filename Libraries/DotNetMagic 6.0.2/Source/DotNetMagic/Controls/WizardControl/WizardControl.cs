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
using System.Collections;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel;
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Specifies the state of a button.
	/// </summary>
	public enum Status
	{
		/// <summary>
		/// Specifies the button default its state.
		/// </summary>
		Default,

		/// <summary>
		/// Specifies the button be visible.
		/// </summary>
		Yes,
			
		/// <summary>
		/// Specifies the button be hidden.
		/// </summary>
		No
	}
	    
	/// <summary>
	/// Specifies how buttons are defaulted.
	/// </summary>
	public enum Profiles
	{
		/// <summary>
		/// Specifies wizard acts like an installer.
		/// </summary>
		Install,

		/// <summary>
		/// Specifies wizard acts a item configurer.
		/// </summary>
		Configure,

		/// <summary>
		/// Specifies wizard acts like a controller.
		/// </summary>
		Controller
	}
	    
	/// <summary>
	/// Manage a collection of wizard pages.
	/// </summary>
    [ToolboxBitmap(typeof(WizardControl))]
    [DefaultProperty("Profile")]
    [Designer(typeof(WizardControlDesigner))]
    public class WizardControl : UserControl
	{
	    // Class wide constants
	    private const int PANEL_GAP = 10;
	    private const int BUTTON_GAP = 10;
	    private static Image _standardPicture;
	
	    // Instance fields
        private Image _picture;
        private string _title;
        private Font _fontTitle;
        private Font _fontSubTitle;
        private Color _colorTitle;
        private Color _colorSubTitle;
        private Profiles _profile;
        private bool _assignDefault;
        private WizardPage _selectedPage;
        private Status _showUpdate, _enableUpdate;
        private Status _showCancel, _enableCancel;
        private Status _showBack, _enableBack;
        private Status _showNext, _enableNext;
        private Status _showFinish, _enableFinish;
        private Status _showClose, _enableClose;
        private Status _showHelp, _enableHelp;
        private WizardPageCollection _wizardPages;
	    
	    // Instance designer fields
        private System.Windows.Forms.Panel _panelTop;
        private System.Windows.Forms.Panel _panelBottom;
        private System.Windows.Forms.Button _buttonUpdate;
        private System.Windows.Forms.Button _buttonCancel;
        private System.Windows.Forms.Button _buttonBack;
        private System.Windows.Forms.Button _buttonNext;
        private System.Windows.Forms.Button _buttonFinish;
        private System.Windows.Forms.Button _buttonClose;
        private System.Windows.Forms.Button _buttonHelp;
        private Crownwood.DotNetMagic.Controls.TabControl _tabControl;
		private System.Windows.Forms.Timer timerResize;
        private OfficeExtender officeExtender;
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// Represents the method that will handle some WizardControl events.
		/// </summary>
        public delegate void WizardPageHandler(WizardPage wp, WizardControl wc);

		/// <summary>
		/// Occurs when a new wizard page is entered.
		/// </summary>
        public event WizardPageHandler WizardPageEnter;
        
		/// <summary>
		/// Occurs when a wizard page is left.
		/// </summary>
		public event WizardPageHandler WizardPageLeave;
        
		/// <summary>
		/// Occurs when the caption title has changed.
		/// </summary>
		public event EventHandler WizardCaptionTitleChanged;
        
		/// <summary>
		/// Occurs when the selected wizard page changes.
		/// </summary>
		public event EventHandler SelectionChanged;
        
		/// <summary>
		/// Occurs when the update button is pressed.
		/// </summary>
		public event EventHandler UpdateClick;
        
		/// <summary>
		/// Occurs when the cancel button is pressed.
		/// </summary>
		public event EventHandler CancelClick;
        
		/// <summary>
		/// Occurs when the finish button is pressed.
		/// </summary>
		public event EventHandler FinishClick;
        
		/// <summary>
		/// Occurs when the close button is pressed.
		/// </summary>
		public event EventHandler CloseClick;
        
		/// <summary>
		/// Occurs when the help button is pressed.
		/// </summary>
		public event EventHandler HelpClick;
        
		/// <summary>
		/// Occurs when the next button is pressed.
		/// </summary>
		public event CancelEventHandler NextClick;
        
		/// <summary>
		/// Occurs when the back button is pressed.
		/// </summary>
		public event CancelEventHandler BackClick;

        static WizardControl()
        {
            // Create a strip of images by loading an embedded bitmap resource
            _standardPicture = ResourceHelper.LoadBitmap(Type.GetType("Crownwood.DotNetMagic.Controls.WizardControl"),
                                                         "Crownwood.DotNetMagic.Resources.WizardPicture.bmp");
        }

		/// <summary>
		/// Initializes a new instance of the WizardControl class.
		/// </summary>
		public WizardControl()
		{
			// NAG processing
			NAG.NAG_Start();

			InitializeComponent();
			
			// No page currently selected
			_selectedPage = null;
			
	        // Hook into tab control events
	        _tabControl.SelectionChanged += new SelectTabHandler(OnTabSelectionChanged);

            // Create our collection of wizard pages
            _wizardPages = new WizardPageCollection();
            
            // Hook into collection events
            _wizardPages.Cleared += new CollectionClear(OnWizardCleared);
            _wizardPages.Inserted += new CollectionChange(OnWizardInserted);
            _wizardPages.Removed += new CollectionChange(OnWizardRemoved);

            // Hook into drawing events
            _panelTop.Resize += new EventHandler(OnRepaintPanels);
            _panelTop.Paint += new PaintEventHandler(OnPaintTopPanel);
            _panelBottom.Resize += new EventHandler(OnRepaintPanels);
            _panelBottom.Paint += new PaintEventHandler(OnPaintBottomPanel);

            // Initialize state
            _showUpdate = _enableUpdate = Status.Default;
            _showCancel = _enableUpdate = Status.Default;
            _showBack = _enableBack = Status.Default;
            _showNext = _enableNext = Status.Default;
            _showFinish = _enableFinish = Status.Default;
            _showClose = _enableClose = Status.Default;
            _showHelp = _enableHelp = Status.Default;
            
            // Default properties
            ResetProfile();
            ResetTitle();
            ResetTitleFont();
            ResetTitleColor();
            ResetSubTitleFont();
            ResetSubTitleColor();
            ResetPicture();
            ResetAssignDefaultButton();
            ResetStyle();
            
            // Position and enable/disable control button state
            UpdateControlButtons();
   		}

		/// <summary>
		/// Releases all resources used by the Control.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this._tabControl = new Crownwood.DotNetMagic.Controls.TabControl();
            this._panelTop = new System.Windows.Forms.Panel();
            this._panelBottom = new System.Windows.Forms.Panel();
            this._buttonUpdate = new System.Windows.Forms.Button();
            this._buttonBack = new System.Windows.Forms.Button();
            this._buttonNext = new System.Windows.Forms.Button();
            this._buttonCancel = new System.Windows.Forms.Button();
            this._buttonFinish = new System.Windows.Forms.Button();
            this._buttonClose = new System.Windows.Forms.Button();
            this._buttonHelp = new System.Windows.Forms.Button();
            this.timerResize = new System.Windows.Forms.Timer(this.components);
            this.officeExtender = new Crownwood.DotNetMagic.Controls.OfficeExtender(this.components);
            this._panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tabControl
            // 
            this._tabControl.Appearance = Crownwood.DotNetMagic.Controls.VisualAppearance.MultiDocument;
            this._tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabControl.IDE2005PixelBorder = false;
            this._tabControl.Location = new System.Drawing.Point(0, 80);
            this._tabControl.Multiline = true;
            this._tabControl.MultilineFullWidth = true;
            this._tabControl.Name = "_tabControl";
            this._tabControl.OfficeDockSides = false;
            this._tabControl.OfficePixelBorder = false;
            this._tabControl.ShowArrows = false;
            this._tabControl.ShowClose = false;
            this._tabControl.ShowDropSelect = false;
            this._tabControl.Size = new System.Drawing.Size(500, 172);
            this._tabControl.TabIndex = 0;
            this._tabControl.TextTips = true;
            this._tabControl.AllowCtrlTab = false;
            // 
            // _panelTop
            // 
            this._panelTop.BackColor = System.Drawing.SystemColors.Window;
            this._panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._panelTop.Location = new System.Drawing.Point(0, 0);
            this._panelTop.Name = "_panelTop";
            this._panelTop.Size = new System.Drawing.Size(500, 80);
            this._panelTop.TabIndex = 1;
            // 
            // _panelBottom
            // 
            this._panelBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._panelBottom.Controls.Add(this._buttonUpdate);
            this._panelBottom.Controls.Add(this._buttonBack);
            this._panelBottom.Controls.Add(this._buttonNext);
            this._panelBottom.Controls.Add(this._buttonCancel);
            this._panelBottom.Controls.Add(this._buttonFinish);
            this._panelBottom.Controls.Add(this._buttonClose);
            this._panelBottom.Controls.Add(this._buttonHelp);
            this._panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._panelBottom.Location = new System.Drawing.Point(0, 252);
            this._panelBottom.Name = "_panelBottom";
            this._panelBottom.Size = new System.Drawing.Size(500, 48);
            this._panelBottom.TabIndex = 2;
            // 
            // _buttonUpdate
            // 
            this._buttonUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonUpdate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._buttonUpdate.Location = new System.Drawing.Point(84, 14);
            this._buttonUpdate.Name = "_buttonUpdate";
            this._buttonUpdate.Size = new System.Drawing.Size(75, 23);
            this._buttonUpdate.TabIndex = 4;
            this._buttonUpdate.Text = "Update";
            this._buttonUpdate.Click += new System.EventHandler(this.OnButtonUpdate);
            // 
            // _buttonBack
            // 
            this._buttonBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonBack.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._buttonBack.Location = new System.Drawing.Point(132, 14);
            this._buttonBack.Name = "_buttonBack";
            this._buttonBack.Size = new System.Drawing.Size(75, 23);
            this._buttonBack.TabIndex = 3;
            this._buttonBack.Text = "< Back";
            this._buttonBack.Click += new System.EventHandler(this.OnButtonBack);
            // 
            // _buttonNext
            // 
            this._buttonNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonNext.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._buttonNext.Location = new System.Drawing.Point(196, 14);
            this._buttonNext.Name = "_buttonNext";
            this._buttonNext.Size = new System.Drawing.Size(75, 23);
            this._buttonNext.TabIndex = 2;
            this._buttonNext.Text = "Next >";
            this._buttonNext.Click += new System.EventHandler(this.OnButtonNext);
            // 
            // _buttonCancel
            // 
            this._buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._buttonCancel.Location = new System.Drawing.Point(260, 14);
            this._buttonCancel.Name = "_buttonCancel";
            this._buttonCancel.Size = new System.Drawing.Size(75, 23);
            this._buttonCancel.TabIndex = 1;
            this._buttonCancel.Text = "Cancel";
            this._buttonCancel.Click += new System.EventHandler(this.OnButtonCancel);
            // 
            // _buttonFinish
            // 
            this._buttonFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonFinish.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._buttonFinish.Location = new System.Drawing.Point(324, 14);
            this._buttonFinish.Name = "_buttonFinish";
            this._buttonFinish.Size = new System.Drawing.Size(75, 23);
            this._buttonFinish.TabIndex = 0;
            this._buttonFinish.Text = "Finish";
            this._buttonFinish.Click += new System.EventHandler(this.OnButtonFinish);
            // 
            // _buttonClose
            // 
            this._buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._buttonClose.Location = new System.Drawing.Point(380, 14);
            this._buttonClose.Name = "_buttonClose";
            this._buttonClose.Size = new System.Drawing.Size(75, 23);
            this._buttonClose.TabIndex = 0;
            this._buttonClose.Text = "Close";
            this._buttonClose.Click += new System.EventHandler(this.OnButtonClose);
            // 
            // _buttonHelp
            // 
            this._buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonHelp.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._buttonHelp.Location = new System.Drawing.Point(436, 14);
            this._buttonHelp.Name = "_buttonHelp";
            this._buttonHelp.Size = new System.Drawing.Size(75, 23);
            this._buttonHelp.TabIndex = 0;
            this._buttonHelp.Text = "Help";
            this._buttonHelp.Click += new System.EventHandler(this.OnButtonHelp);
            // 
            // timerResize
            // 
            this.timerResize.Enabled = true;
            this.timerResize.Interval = 1;
            this.timerResize.Tick += new System.EventHandler(this.timerResize_Tick);
            // 
            // officeExtender
            // 
            this.officeExtender.Office2007Variant = Crownwood.DotNetMagic.Controls.Office2007Variant.Blue;
            // 
            // WizardControl
            // 
            this.Controls.Add(this._tabControl);
            this.Controls.Add(this._panelTop);
            this.Controls.Add(this._panelBottom);
            this.Name = "WizardControl";
            this.Size = new System.Drawing.Size(500, 300);
            this.Load += new System.EventHandler(this.WizardControl_Load);
            this._panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

        /// <summary>
        /// Gets and sets the visual style for styling the WizardControl.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(typeof(VisualStyle), "Office2007Blue")]
        [Description("VisualStyle that should be applied to the WizardControl.")]
        public VisualStyle Style
        {
            get { return TabControl.Style; }

            set
            {
                // Push style into the tab control itself
                TabControl.Style = value;

                officeExtender.SetOffice2003BackColor(_panelBottom, Crownwood.DotNetMagic.Controls.OfficeColor.Disable);
                officeExtender.SetOffice2007BackColor(_panelBottom, Crownwood.DotNetMagic.Controls.OfficeColor.Disable);
                officeExtender.SetMediaPlayerBackColor(_panelBottom, Crownwood.DotNetMagic.Controls.MediaPlayerColor.Disable);

                // Ensure the panels automatically reflect the style appropriate colors
                switch (ColorHelper.ValidateStyle(value))
                {
                    case VisualStyle.Plain:
                    case VisualStyle.IDE2005:
                        _panelBottom.BackColor = SystemColors.Control;
                        break;
                    case VisualStyle.Office2003:
                        officeExtender.SetOffice2003BackColor(_panelBottom, Crownwood.DotNetMagic.Controls.OfficeColor.Base);
                        break;
                    case VisualStyle.Office2007Blue:
                        officeExtender.SetOffice2007BackColor(_panelBottom, Crownwood.DotNetMagic.Controls.OfficeColor.Base);
                        officeExtender.Office2007Variant = Office2007Variant.Blue;
                        break;
                    case VisualStyle.Office2007Silver:
                        officeExtender.SetOffice2007BackColor(_panelBottom, Crownwood.DotNetMagic.Controls.OfficeColor.Base);
                        officeExtender.Office2007Variant = Office2007Variant.Silver;
                        break;
                    case VisualStyle.Office2007Black:
                        officeExtender.SetOffice2007BackColor(_panelBottom, Crownwood.DotNetMagic.Controls.OfficeColor.Dark);
                        officeExtender.Office2007Variant = Office2007Variant.Black;
                        break;
                    case VisualStyle.MediaPlayerBlue:
                        officeExtender.SetMediaPlayerBackColor(_panelBottom, Crownwood.DotNetMagic.Controls.MediaPlayerColor.Dark);
                        officeExtender.MediaPlayerVariant = MediaPlayerVariant.Blue;
                        break;
                    case VisualStyle.MediaPlayerOrange:
                        officeExtender.SetMediaPlayerBackColor(_panelBottom, Crownwood.DotNetMagic.Controls.MediaPlayerColor.Dark);
                        officeExtender.MediaPlayerVariant = MediaPlayerVariant.Orange;
                        break;
                    case VisualStyle.MediaPlayerPurple:
                        officeExtender.SetMediaPlayerBackColor(_panelBottom, Crownwood.DotNetMagic.Controls.MediaPlayerColor.Dark);
                        officeExtender.MediaPlayerVariant = MediaPlayerVariant.Purple;
                        break;
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
		/// Gets access to the underlying TabControl.
		/// </summary>
        [Category("Wizard")]
        [Description("Access to underlying TabControl instance")]
        public Controls.TabControl TabControl
        {
            get { return _tabControl; }
        }

		/// <summary>
		/// Gets access to teh underlying header panel.
		/// </summary>
        [Category("Wizard")]
        [Description("Access to underlying header panel")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)] 
		public Panel HeaderPanel
        {
            get { return _panelTop; }
        }

		/// <summary>
		/// Gets access to the underlying trailer panel.
		/// </summary>
        [Category("Wizard")]
        [Description("Access to underlying trailer panel")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]         
		public Panel TrailerPanel
        {
            get { return _panelBottom; }
        }

		/// <summary>
		/// Gets the collection of WizardPages.
		/// </summary>
		[Localizable(true)]
		[Category("Wizard")]
        [Description("Collection of wizard pages")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public WizardPageCollection WizardPages
		{
		    get { return _wizardPages; }
		}
		
		/// <summary>
		/// Gets and sets the profile that determines default operation of buttons.
		/// </summary>
        [Category("Wizard")]
        [Description("Determine default operation of buttons")]
        [DefaultValue(typeof(Profiles), "Configure")]
        public Profiles Profile
        {
            get { return _profile; }
		    
            set 
            {
                if (_profile != value)
                {
                    _profile = value;
		            
                    switch(_profile)
                    {
                        case Profiles.Install:
                        case Profiles.Configure:
                            // Current page selection determines if full page is needed
                            if (_tabControl.SelectedIndex != -1)
                            {
                                // Get the selected wizard page
                                WizardPage wp = _wizardPages[_tabControl.SelectedIndex];
                
                                // Should we be presented in full page?
                                if (wp.FullPage)
                                    EnterFullPage();
                                else
                                {
                                    // Controller profile is not allowed to be outside of FullMode
                                    if (_profile != Profiles.Controller)
                                        LeaveFullPage();
                                }
                            }
                            else
                                LeaveFullPage();
                            
                            _tabControl.HideTabsMode = HideTabsModes.HideAlways; 
                            break;
                        case Profiles.Controller:
                            // Controller is always full page
                            EnterFullPage();
                                
                            _tabControl.HideTabsMode = HideTabsModes.ShowAlways;
                            break;
                    }
		            
                    // Position and enable/disable control button state
                    UpdateControlButtons();
                }
            }
        }

		/// <summary>
		/// Resets the Profile property to its default value.
		/// </summary>
        public void ResetProfile()
        {
            Profile = Profiles.Configure;
        }
        
		/// <summary>
		/// Gets and sets the image using in the header.
		/// </summary>
		[Localizable(true)]
		[Category("Wizard")]
        [Description("Main panel picture")]
        public Image Picture
        {
            get { return _picture; }
            
            set
            {
                _picture = value;
                _panelTop.Invalidate();
            }
        }

		/// <summary>
		/// Decide if the picture needs to be persisted.
		/// </summary>
		/// <returns>Should picture be persisted.</returns>
        protected bool ShouldSerializePicture()
        {
            return !_picture.Equals(_standardPicture);
        }
        
		/// <summary>
		/// Resets the Picture property to its default value.
		/// </summary>
        public void ResetPicture()
        {
            Picture = _standardPicture;
        }
        
		/// <summary>
		/// Gets and sets the main title text in header.
		/// </summary>
        [Category("Wizard")]
		[Description("Main title text")]
		[Localizable(true)]
		public string Title
		{
		    get { return _title; }
		    
		    set
		    {
		        _title = value;
		        _panelTop.Invalidate();
		    }
		}
		
		/// <summary>
		/// Decide if the main title text needs to be persisted.
		/// </summary>
		/// <returns>Should main title text be persisted.</returns>
		protected bool ShouldSerializeTitle()
		{
			return !_title.Equals("Welcome to the Wizard Control");
		}
    
		/// <summary>
		/// Resets the Title property to its default value.
		/// </summary>
        public void ResetTitle()
        {
            Title = "Welcome to the Wizard Control";
        }

		/// <summary>
		/// Gets and sets the font used to draw the main title.
		/// </summary>
        [Category("Wizard")]
		[Description("Font for drawing main title text")]
		public Font TitleFont
		{
		    get { return _fontTitle; }
		    
		    set
		    {
		        _fontTitle = value;
		        _panelTop.Invalidate();
		    }
		}
		
		/// <summary>
		/// Resets the TitleFont property to its default value.
		/// </summary>
        public void ResetTitleFont()
        {
            TitleFont = new Font("Tahoma", 10, FontStyle.Bold);
        }

		/// <summary>
		/// Decide if the main title font needs to be persisted.
		/// </summary>
		/// <returns>Should main title font be persisted.</returns>
        protected bool ShouldSerializeTitleFont()
        {
            return !_fontTitle.Equals(new Font("Tahoma", 10, FontStyle.Bold));
        }
    
		/// <summary>
		/// Gets and sets the font used to draw the sub-title.
		/// </summary>
        [Category("Wizard")]
        [Description("Font for drawing main sub-title text")]
        public Font SubTitleFont
        {
            get { return _fontSubTitle; }
		    
            set
            {
                _fontSubTitle = value;
                _panelTop.Invalidate();
            }
        }
		
		/// <summary>
		/// Resets the SubTitleFont property to its default value.
		/// </summary>
        public void ResetSubTitleFont()
        {
            _fontSubTitle = new Font("Tahoma", 8, FontStyle.Regular);
        }

		/// <summary>
		/// Decide if the sub-title font needs to be persisted.
		/// </summary>
		/// <returns>Should sub-title font be persisted.</returns>
        protected bool ShouldSerializeSubTitleFont()
        {
            return !_fontSubTitle.Equals(new Font("Tahoma", 8, FontStyle.Regular));
        }

		/// <summary>
		/// Gets and sets the color of the main title text.
		/// </summary>
        [Category("Wizard")]
        [Description("Color for drawing main title text")]
        public Color TitleColor
		{
		    get { return _colorTitle; }
		    
		    set
		    {
		        _colorTitle = value;
		        _panelTop.Invalidate();
		    }
		}

		/// <summary>
		/// Resets the TitleColor property to its default value.
		/// </summary>
		public void ResetTitleColor()
		{
		    TitleColor = base.ForeColor;
		}

		/// <summary>
		/// Decide if the color of the main title text needs to be persisted.
		/// </summary>
		/// <returns>Should the color of the main title text be persisted.</returns>
        protected bool ShouldSerializeTitleColor()
        {
            return !_colorTitle.Equals(base.ForeColor);
        }
		
		/// <summary>
		/// Gets and sets a value indicating if a default button should be assigned.
		/// </summary>
        [Category("Wizard")]
        [Description("Determine if a default button should be auto-assigned")]
        [DefaultValue(false)]
        public bool AssignDefaultButton
        {
            get { return _assignDefault; }
            
            set
            {
                if (_assignDefault != value)
                {
                    _assignDefault = value;
                    AutoAssignDefaultButton();
                }
            }
        }

		/// <summary>
		/// Resets the AssignDefaultButton property to its default value.
		/// </summary>
        public void ResetAssignDefaultButton()
        {
            AssignDefaultButton = false;
        }

		/// <summary>
		/// Gets and sets the color for drawing the sub-title text.
		/// </summary>
        [Category("Wizard")]
        [Description("Color for drawing main sub-title text")]
        public Color SubTitleColor
        {
            get { return _colorSubTitle; }
		    
            set
            {
                _colorSubTitle = value;
                _panelTop.Invalidate();
            }
        }

		/// <summary>
		/// Resets the SubTitleColor property to its default value.
		/// </summary>
        public void ResetSubTitleColor()
        {
            SubTitleColor = base.ForeColor;
        }

		/// <summary>
		/// Decide if the color of the sub-title text needs to be persisted.
		/// </summary>
		/// <returns>Should the color of the sub-title title text be persisted.</returns>
        protected bool ShouldSerializeSubTitleColor()
        {
            return !_colorSubTitle.Equals(base.ForeColor);
        }

		/// <summary>
		/// Gets access to the update button.
		/// </summary>
		[Category("Control Buttons")]
		[Description("Modify default button properties")]
		[Browsable(false)]
		public Button UpdateButton
		{
			get { return _buttonUpdate; }
		}

		/// <summary>
		/// Gets and sets the visibility of the update button.
		/// </summary>
		[Category("Control Buttons")]
        [Description("Define visibility of Update button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status ShowUpdateButton
        {
            get { return _showUpdate; }
            
            set 
            { 
                if (_showUpdate != value)
                {
                    _showUpdate = value;
                    UpdateControlButtons();
                }
            }
        }

		/// <summary>
		/// Gets and sets the selectability of the update button.
		/// </summary>
        [Category("Control Buttons")]
        [Description("Define selectability of Update button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status EnableUpdateButton
        {
            get { return _enableUpdate; }
            
            set 
            { 
                if (_enableUpdate != value)
                {
                    _enableUpdate = value;
                    UpdateControlButtons();
                }
            }
        }

		/// <summary>
		/// Gets and sets the text displayed on the update button.
		/// </summary>
        [Category("Control Buttons")]
        [Description("Modify the text for the Update control button")]
        [DefaultValue("Update")]
        [Localizable(true)]
        public string ButtonUpdateText
        {
            get { return _buttonUpdate.Text; }
            set { _buttonUpdate.Text = value; }
        }

		/// <summary>
		/// Gets access to the cancel button.
		/// </summary>
		[Category("Control Buttons")]
		[Description("Modify default button properties")]
		[Browsable(false)]
		public Button CancelButton
		{
			get { return _buttonCancel; }
		}
		
		/// <summary>
		/// Gets and sets the visibility of the cancel button.
		/// </summary>
		[Category("Control Buttons")]
        [Description("Define visibility of Cancel button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status ShowCancelButton
        {
            get { return _showCancel; }
            
            set 
            { 
                if (_showCancel != value)
                {
                    _showCancel = value;
                    UpdateControlButtons();
                }
            }
        }

		/// <summary>
		/// Gets and sets the selectability of the cancel button.
		/// </summary>
        [Category("Control Buttons")]
        [Description("Define selectability of Cancel button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status EnableCancelButton
        {
            get { return _enableCancel; }
            
            set 
            { 
                if (_enableCancel != value)
                {
                    _enableCancel = value;
                    UpdateControlButtons();
                }
            }
        }

		/// <summary>
		/// Gets and sets the text displayed on the cancel button.
		/// </summary>
        [Category("Control Buttons")]
        [Description("Modify the text for the Cancel control button")]
        [DefaultValue("Cancel")]
        [Localizable(true)]
        public string ButtonCancelText
        {
            get { return _buttonCancel.Text; }
            set { _buttonCancel.Text = value; }
        }

		/// <summary>
		/// Gets access to the back button.
		/// </summary>
		[Category("Control Buttons")]
		[Description("Modify default button properties")]
		[Browsable(false)]
		public Button BackButton
		{
			get { return _buttonBack; }
		}
		
		/// <summary>
		/// Gets and sets the visibility of the back button.
		/// </summary>
		[Category("Control Buttons")]
        [Description("Define visibility of Back button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status ShowBackButton
        {
            get { return _showBack; }
            
            set 
            { 
                if (_showBack != value)
                {
                    _showBack = value;
                    UpdateControlButtons();
                }
            }
        }

		/// <summary>
		/// Gets and sets the selectability of the back button.
		/// </summary>
        [Category("Control Buttons")]
        [Description("Define selectability of Back button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status EnableBackButton
        {
            get { return _enableBack; }
            
            set 
            { 
                if (_enableBack != value)
                {
                    _enableBack = value;
                    UpdateControlButtons();
                }
            }
        }

		/// <summary>
		/// Gets and sets the text displayed on the back button.
		/// </summary>
        [Category("Control Buttons")]
        [Description("Modify the text for the Back control button")]
        [DefaultValue("< Back")]
        [Localizable(true)]
        public string ButtonBackText
        {
            get { return _buttonBack.Text; }
            set { _buttonBack.Text = value; }
        }

		/// <summary>
		/// Gets access to the next button.
		/// </summary>
		[Category("Control Buttons")]
		[Description("Modify default button properties")]
		[Browsable(false)]
		public Button NextButton
		{
			get { return _buttonNext; }
		}
		
		/// <summary>
		/// Gets and sets the visibility of the next button.
		/// </summary>
		[Category("Control Buttons")]
        [Description("Define visibility of Next button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status ShowNextButton
        {
            get { return _showNext; }
            
            set 
            { 
                if (_showNext != value)
                {
                    _showNext = value;
                    UpdateControlButtons();
                }
            }
        }

		/// <summary>
		/// Gets and sets the selectability of the next button.
		/// </summary>
        [Category("Control Buttons")]
        [Description("Define selectability of Next button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status EnableNextButton
        {
            get { return _enableBack; }
            
            set 
            { 
                if (_enableNext != value)
                {
                    _enableNext = value;
                    UpdateControlButtons();
                }
            }
        }

		/// <summary>
		/// Gets and sets the text displayed on the next button.
		/// </summary>
        [Category("Control Buttons")]
        [Description("Modify the text for the Next control button")]
        [DefaultValue("Next >")]
        [Localizable(true)]
        public string ButtonNextText
        {
            get { return _buttonNext.Text; }
            set { _buttonNext.Text = value; }
        }

		/// <summary>
		/// Gets access to the finish button.
		/// </summary>
		[Category("Control Buttons")]
		[Description("Modify default button properties")]
		[Browsable(false)]
		public Button FinishButton
		{
			get { return _buttonFinish; }
		}
		
		/// <summary>
		/// Gets and sets the visibility of the finish button.
		/// </summary>
		[Category("Control Buttons")]
        [Description("Define visibility of Finish button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status ShowFinishButton
        {
            get { return _showFinish; }
            
            set 
            { 
                if (_showFinish != value)
                {
                    _showFinish = value;
                    UpdateControlButtons();
                }
            }
        }

		/// <summary>
		/// Gets and sets the selectability of the finish button.
		/// </summary>
        [Category("Control Buttons")]
        [Description("Define selectability of Finish button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status EnableFinishButton
        {
            get { return _enableFinish; }
            
            set 
            { 
                if (_enableFinish != value)
                {
                    _enableFinish = value;
                    UpdateControlButtons();
                }
            }
        }

		/// <summary>
		/// Gets and sets the text displayed on the finish button.
		/// </summary>
        [Category("Control Buttons")]
        [Description("Modify the text for the Finish control button")]
        [DefaultValue("Finish")]
        [Localizable(true)]
        public string ButtonFinishText
        {
            get { return _buttonFinish.Text; }
            set { _buttonFinish.Text = value; }
        }
        
		/// <summary>
		/// Gets access to the close button.
		/// </summary>
		[Category("Control Buttons")]
		[Description("Modify default button properties")]
		[Browsable(false)]
		public Button CloseButton
		{
			get { return _buttonClose; }
		}
		
		/// <summary>
		/// Gets and sets the visibility of the close button.
		/// </summary>
		[Category("Control Buttons")]
        [Description("Define visibility of Close button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status ShowCloseButton
        {
            get { return _showClose; }
            
            set 
            { 
                if (_showClose != value)
                {
                    _showClose = value;
                    UpdateControlButtons();
                }
            }
        }

		/// <summary>
		/// Gets and sets the selectability of the close button.
		/// </summary>
        [Category("Control Buttons")]
        [Description("Define selectability of Close button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status EnableCloseButton
        {
            get { return _enableClose; }
            
            set 
            { 
                if (_enableClose != value)
                {
                    _enableClose = value;
                    UpdateControlButtons();
                }
            }
        }

		/// <summary>
		/// Gets and sets the text displayed on the close button.
		/// </summary>
        [Category("Control Buttons")]
        [Description("Modify the text for the Close control button")]
        [DefaultValue("Close")]
        [Localizable(true)]
        public string ButtonCloseText
        {
            get { return _buttonClose.Text; }
            set { _buttonClose.Text = value; }
        }

		/// <summary>
		/// Gets access to the help button.
		/// </summary>
		[Category("Control Buttons")]
		[Description("Modify default button properties")]
		[Browsable(false)]
		public Button HelpButton
		{
			get { return _buttonHelp; }
		}
		
		/// <summary>
		/// Gets and sets the visibility of the help button.
		/// </summary>
		[Category("Control Buttons")]
        [Description("Define visibility of Help button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status ShowHelpButton
        {
            get { return _showHelp; }
            
            set 
            { 
                if (_showHelp != value)
                {
                    _showHelp = value;
                    UpdateControlButtons();
                }
            }
        }

		/// <summary>
		/// Gets and sets the selectability of the help button.
		/// </summary>
        [Category("Control Buttons")]
        [Description("Define selectability of Help button")]
        [DefaultValue(typeof(Status), "Default")]
        public Status EnableHelpButton
        {
            get { return _enableHelp; }
            
            set 
            { 
                if (_enableHelp != value)
                {
                    _enableHelp = value;
                    UpdateControlButtons();
                }
            }
        }

		/// <summary>
		/// Gets and sets the text displayed on the help button.
		/// </summary>
        [Category("Control Buttons")]
        [Description("Modify the text for the Help control button")]
        [DefaultValue("Help")]
        [Localizable(true)]
        public string ButtonHelpText
        {
            get { return _buttonHelp.Text; }
            set { _buttonHelp.Text = value; }
        }

		/// <summary>
		/// Gets and sets the index of hte currently selected page.
		/// </summary>
        [Category("Wizard")]
        [Description("Index of currently selected WizardPage")]
        public int SelectedIndex
        {
            get { return _tabControl.SelectedIndex; }
            set { _tabControl.SelectedIndex = value; }
        }
        
		/// <summary>
		/// Raises the WizardPageEnter event.
		/// </summary>
		/// <param name="wp">A WizardPage reference.</param>
        protected virtual void OnWizardPageEnter(WizardPage wp)
        {
            if (WizardPageEnter != null)
                WizardPageEnter(wp, this);
        }

		/// <summary>
		/// Raises the WizardPageLeave event.
		/// </summary>
		/// <param name="wp">A WizardPage reference.</param>
        protected virtual void OnWizardPageLeave(WizardPage wp)
        {
            if (WizardPageLeave != null)
                WizardPageLeave(wp, this);
        }

		/// <summary>
		/// Raises the SelectionChanged event.
		/// </summary>
		/// <param name="e">An EventArgs structure containing event data.</param>
        protected virtual void OnSelectionChanged(EventArgs e)
        {
            if (SelectionChanged != null)
                SelectionChanged(this, e);
        }
                
		/// <summary>
		/// Raises the CloseClick event.
		/// </summary>
		/// <param name="e">An EventArgs structure containing event data.</param>
        protected virtual void OnCloseClick(EventArgs e)
        {
            if (CloseClick != null)
                CloseClick(this, e);
        }

		/// <summary>
		/// Raises the FinishClick event.
		/// </summary>
		/// <param name="e">An EventArgs structure containing event data.</param>
        protected virtual void OnFinishClick(EventArgs e)
        {
            if (FinishClick != null)
                FinishClick(this, e);
        }
    
		/// <summary>
		/// Raises the NextClick event.
		/// </summary>
		/// <param name="e">An EventArgs structure containing event data.</param>
        protected virtual void OnNextClick(CancelEventArgs e)
        {
            if (NextClick != null)
                NextClick(this, e);
        }
    
		/// <summary>
		/// Raises the BackClick event.
		/// </summary>
		/// <param name="e">An EventArgs structure containing event data.</param>
        protected virtual void OnBackClick(CancelEventArgs e)
        {
            if (BackClick != null)
                BackClick(this, e);
        }

		/// <summary>
		/// Raises the CancelClick event.
		/// </summary>
		/// <param name="e">An EventArgs structure containing event data.</param>
        protected virtual void OnCancelClick(EventArgs e)
        {
            if (CancelClick != null)
                CancelClick(this, e);
        }
        
		/// <summary>
		/// Raises the CancelClick event.
		/// </summary>
		/// <param name="e">An EventArgs structure containing event data.</param>
        protected virtual void OnUpdateClick(EventArgs e)
        {
            if (UpdateClick != null)
                UpdateClick(this, e);
        }
        
		/// <summary>
		/// Raises the HelpClick event.
		/// </summary>
		/// <param name="e">An EventArgs structure containing event data.</param>
        protected virtual void OnHelpClick(EventArgs e)
        {
            if (HelpClick != null)
                HelpClick(this, e);
        }

		/// <summary>
		/// Raises the Resize event.
		/// </summary>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected override void OnResize(EventArgs e)
		{
			this.PerformLayout();
		}

        private void UpdateControlButtons()
        {
            // Track next button inserted position
            int x = this.Width - BUTTON_GAP - _buttonFinish.Width;
            
            bool showHelp = ShouldShowHelp();
            bool showClose = ShouldShowClose();
            bool showFinish = ShouldShowFinish();
            bool showNext = ShouldShowNext();
            bool showBack = ShouldShowBack();
            bool showCancel = ShouldShowCancel();
            bool showUpdate = ShouldShowUpdate();
            
            if (showHelp) 
            {
                _buttonHelp.Left = x;
                x -= _buttonHelp.Width + BUTTON_GAP;
                _buttonHelp.Enabled = ShouldEnableHelp();
                _buttonHelp.Show();
            }
            else
                _buttonHelp.Hide();

            if (showClose) 
            {
                _buttonClose.Left = x;
                x -= _buttonClose.Width + BUTTON_GAP;
                _buttonClose.Enabled = ShouldEnableClose();
                _buttonClose.Show();
            }
            else
                _buttonClose.Hide();

            if (showFinish) 
            {
                _buttonFinish.Left = x;
                x -= _buttonFinish.Width + BUTTON_GAP;
                _buttonFinish.Enabled = ShouldEnableFinish();
                _buttonFinish.Show();
            }
            else
                _buttonFinish.Hide();

            if (showNext) 
            {
                _buttonNext.Left = x;
                x -= _buttonNext.Width + BUTTON_GAP;
                _buttonNext.Enabled = ShouldEnableNext();
                _buttonNext.Show();
            }
            else
                _buttonNext.Hide();

            if (showBack) 
            {
                _buttonBack.Left = x;
                x -= _buttonBack.Width + BUTTON_GAP;
                _buttonBack.Enabled = ShouldEnableBack();
                _buttonBack.Show();
            }
            else
                _buttonBack.Hide();

            if (showCancel) 
            {
                _buttonCancel.Left = x;
                x -= _buttonCancel.Width + BUTTON_GAP;
                _buttonCancel.Enabled = ShouldEnableCancel();
                _buttonCancel.Show();
            }
            else
                _buttonCancel.Hide();

            if (showUpdate) 
            {
                _buttonUpdate.Left = x;
                x -= _buttonUpdate.Width + BUTTON_GAP;
                _buttonUpdate.Enabled = ShouldEnableUpdate();
                _buttonUpdate.Show();
            }
            else
                _buttonUpdate.Hide();
                
            AutoAssignDefaultButton();
        }
        
        private void AutoAssignDefaultButton()
        {
            // Get our parent Form instance
            Form parentForm = this.FindForm();
            
            // Cannot assign a default button if we are not on a Form
            if (parentForm != null)
            {
                // Can only assign a particular button if we have been requested 
                // to auto- assign and we are on a selected page
                if (_assignDefault && (_tabControl.SelectedIndex >= 0))
                {
                    // Button default depends on the profile mode
                    switch(_profile)
                    {
                        case Profiles.Install:
                            // Is this the last page?
                            if (_tabControl.SelectedIndex == (_tabControl.TabPages.Count - 1))
                            {
                                // Then use the Close button
                                parentForm.AcceptButton = _buttonClose;
                            }
                            else
                            {
                                // Is this the second from last page?
                                if (_tabControl.SelectedIndex == (_tabControl.TabPages.Count - 2))
                                {
                                    // Then use the Cancel button
                                    parentForm.AcceptButton = _buttonCancel;
                                }
                                else
                                {
                                    // Then use the Next button
                                    parentForm.AcceptButton = _buttonNext;
                                }
                            }
                            break;
                        case Profiles.Configure:
                            // Is this the last page?
                            if (_tabControl.SelectedIndex == (_tabControl.TabPages.Count - 1))
                            {
                                // Then always use the Finish button
                                parentForm.AcceptButton = _buttonFinish;
                            }
                            else
                            {
                                // Else we are not on last page, use the Next button
                                parentForm.AcceptButton = _buttonNext;
                            }
                            break;
                        case Profiles.Controller:
                            // Always use the Update button
                            parentForm.AcceptButton = _buttonUpdate;
                            break;
                    }
                }
                else
                {
                    // Remove any assigned default button
                    parentForm.AcceptButton = null;
                }
            }
        }
        
        private bool ShouldShowClose()
        {
            bool ret = false;
        
            switch(_showClose)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                switch(_profile)
                {
                    case Profiles.Install:
                        // Must have at least one page
                        if (_tabControl.SelectedIndex != -1)
                        {
                            // Cannot 'Close' unless on the last page
                            if (_tabControl.SelectedIndex == (_tabControl.TabPages.Count - 1))
                                ret = true;
                        }
                        break;
                    case Profiles.Configure:
                        break;
                    case Profiles.Controller:
                        break;
                }
                    break;
            }

            return ret;
        }

        private bool ShouldEnableClose()
        {
            bool ret = false;
        
            switch(_enableClose)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                    ret = true;
                    break;
            }

            return ret;
        }

        private bool ShouldShowFinish()
        {
            bool ret = false;
        
            switch(_showFinish)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                    switch(_profile)
                    {
                        case Profiles.Install:
                            break;
                        case Profiles.Configure:
                            ret = true;
                            break;
                        case Profiles.Controller:
                            break;
                    }
                    break;
            }

            return ret;
        }

        private bool ShouldEnableFinish()
        {
            bool ret = false;
        
            switch(_enableFinish)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                    ret = true;
                    break;
            }

            return ret;
        }

        private bool ShouldShowNext()
        {
            bool ret = false;

            switch(_showNext)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                    switch(_profile)
                    {
                        case Profiles.Install:
                            // Must have at least one page
                            if (_tabControl.SelectedIndex != -1)
                            {
                                // Cannot 'Next' when at the last or second to last pages
                                if (_tabControl.SelectedIndex < (_tabControl.TabPages.Count - 2))
                                    ret = true;
                            }
                            break;
                        case Profiles.Configure:
                            ret = true;
                            break;
                        case Profiles.Controller:
                            break;
                    }
                    break;
            }

            return ret;
        }

        private bool ShouldEnableNext()
        {
            bool ret = false;

            switch(_enableNext)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                    switch(_profile)
                    {
                        case Profiles.Install:
                            // Must have at least one page
                            if (_tabControl.SelectedIndex != -1)
                            {
                                // Cannot 'Next' when at the last or second to last pages
                                if (_tabControl.SelectedIndex < (_tabControl.TabPages.Count - 2))
                                    ret = true;
                            }
                            break;
                        case Profiles.Configure:
                        case Profiles.Controller:
                            // Must have at least one page
                            if (_tabControl.SelectedIndex != -1)
                            {
                                // Cannot 'Next' when at the last or second to last pages
                                if (_tabControl.SelectedIndex < (_tabControl.TabPages.Count - 1))
                                    ret = true;
                            }
                            break;
                    }
                    break;
            }

            return ret;
        }

        private bool ShouldShowBack()
        {
            bool ret = false;

            switch(_showBack)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                    switch(_profile)
                    {
                        case Profiles.Install:
                            // Cannot 'Back' when one the first page or on the last two special pages
                            if ((_tabControl.SelectedIndex > 0) && (_tabControl.SelectedIndex < (_tabControl.TabPages.Count - 2)))
                                ret = true;
                            break;
                        case Profiles.Configure:
                            ret = true;
                            break;
                        case Profiles.Controller:
                            break;
                    }
                    break;
            }

            return ret;
        }

        private bool ShouldEnableBack()
        {
            bool ret = false;

            switch(_enableBack)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                    // Cannot 'Back' when one the first page
                    if (_tabControl.SelectedIndex > 0)
                        ret = true;
                    break;
            }

            return ret;
        }

        private bool ShouldShowCancel()
        {
            bool ret = false;

            switch(_showCancel)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                    switch(_profile)
                    {
                        case Profiles.Install:
                            // Must have at least one page
                            if (_tabControl.SelectedIndex != -1)
                            {
                                // Cannot 'Cancel' on the last page of an Install
                                if (_tabControl.SelectedIndex < (_tabControl.TabPages.Count - 1))
                                    ret = true;
                            }
                            break;
                        case Profiles.Configure:
                            ret = true;
                            break;
                        case Profiles.Controller:
                            ret = true;
                            break;
                    }
                    break;
            }

            return ret;
        }

        private bool ShouldEnableCancel()
        {
            bool ret = false;

            switch(_enableCancel)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                    ret = true;
                    break;
            }

            return ret;
        }

        private bool ShouldShowUpdate()
        {
            bool ret = false;

            switch(_showUpdate)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                    switch(_profile)
                    {
                        case Profiles.Install:
                            break;
                        case Profiles.Configure:
                            break;
                        case Profiles.Controller:
                            ret = true;
                            break;
                    }
                    break;
            }

            return ret;
        }

        private bool ShouldEnableUpdate()
        {
            bool ret = false;

            switch(_enableUpdate)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                    ret = true;
                    break;
            }

            return ret;
        }

        private bool ShouldEnableHelp()
        {
            bool ret = false;

            switch(_enableHelp)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                    ret = true;
                    break;
            }

            return ret;
        }

        private bool ShouldShowHelp()
        {
            bool ret = false;

            switch(_showHelp)
            {
                case Status.No:
                    break;
                case Status.Yes:
                    ret = true;
                    break;
                case Status.Default:
                    break;
            }

            return ret;
        }

        private void LeaveFullPage()
        {
            _panelTop.Show();
            _tabControl.Top = _panelTop.Height;
            _tabControl.Height = _panelBottom.Top - _panelTop.Height - 1;
        }
        
        private void EnterFullPage()
        {
            _panelTop.Hide();
            _tabControl.Top = 0;
            _tabControl.Height = _panelBottom.Top - 1;
        }

		private void OnTabSelectionChanged(Crownwood.DotNetMagic.Controls.TabControl tabControl, 
										   Crownwood.DotNetMagic.Controls.TabPage oldPage, 
										   Crownwood.DotNetMagic.Controls.TabPage newPage)
        {
            // Update buttons to reflect change
            UpdateControlButtons();
            
            if (tabControl.SelectedIndex != -1)
            {
                // Get the selected wizard page
                WizardPage wp = _wizardPages[tabControl.SelectedIndex];
                
                // Should we be presented in full page?
                if (wp.FullPage)
                    EnterFullPage();
                else
                {
                    // Controller profile is not allowed to be outside of FullMode
                    if (_profile != Profiles.Controller)
                        LeaveFullPage();
                }
            }
            else
            {
                // Controller profile is not allowed to be outside of FullMode
                if (_profile != Profiles.Controller)
                    LeaveFullPage();
            }
            
            // Update manual drawn text
            _panelTop.Invalidate();
            
            // Generate raw selection changed event
            OnSelectionChanged(EventArgs.Empty);
            
            // Generate page leave event if currently on a valid page
            if (_selectedPage != null)
            {
                OnWizardPageLeave(_selectedPage);
                _selectedPage = null;
            }
            
            // Remember which is the newly seleced page
            if (_tabControl.SelectedIndex != -1)
                _selectedPage = _wizardPages[_tabControl.SelectedIndex] as WizardPage;
            
            // Generate page enter event is now on a valid page
            if (_selectedPage != null)
                OnWizardPageEnter(_selectedPage);
        }

        private void OnButtonHelp(object sender, EventArgs e)
        {
            // Fire event for interested handlers
            OnHelpClick(EventArgs.Empty);
        }

        private void OnButtonClose(object sender, EventArgs e)
        {
            // Fire event for interested handlers
            OnCloseClick(EventArgs.Empty);
        }

        private void OnButtonFinish(object sender, EventArgs e)
        {
            // Fire event for interested handlers
            OnFinishClick(EventArgs.Empty);
        }

        private void OnButtonNext(object sender, EventArgs e)
        {
            if (_buttonNext.Enabled)
            {
                CancelEventArgs ce = new CancelEventArgs(false);

                // Give handlers chance to cancel this action
                OnNextClick(ce);

                if (!ce.Cancel)
                {
                    // Move to the next page if there is one
                    if (_tabControl.SelectedIndex < _tabControl.TabPages.Count - 1)
                        _tabControl.SelectedIndex++;
                }
            }
        }

        private void OnButtonBack(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs(false);
            
            // Give handlers chance to cancel this action
            OnBackClick(ce);
            
            if (!ce.Cancel)
            {
                // Move to the next page if there is one
                if (_tabControl.SelectedIndex > 0)
                    _tabControl.SelectedIndex--;
            }
        }

        private void OnButtonCancel(object sender, EventArgs e)
        {
            // Fire event for interested handlers
            OnCancelClick(EventArgs.Empty);
        }

        private void OnButtonUpdate(object sender, EventArgs e)
        {
            // Fire event for interested handlers
            OnUpdateClick(EventArgs.Empty);
        }

        private void OnWizardCleared()
        {
            // Unhook from event handlers for each page
            foreach(WizardPage wp in _tabControl.TabPages)
            {
                wp.FullPageChanged -= new EventHandler(OnWizardFullPageChanged);
                wp.SubTitleChanged -= new EventHandler(OnWizardSubTitleChanged);
                wp.CaptionTitleChanged -= new EventHandler(OnWizardCaptionTitleChanged);
            }
        
            // Reflect change on underlying tab control
            _tabControl.TabPages.Clear();

            // Update buttons to reflect status
            UpdateControlButtons();
        }
        
        private void OnWizardInserted(int index, object value)
        {
            WizardPage wp = value as WizardPage;
           
           // Monitor property changes
           wp.FullPageChanged += new EventHandler(OnWizardFullPageChanged);
           wp.SubTitleChanged += new EventHandler(OnWizardSubTitleChanged);
           wp.CaptionTitleChanged += new EventHandler(OnWizardCaptionTitleChanged);
        
            // Reflect change on underlying tab control
            _tabControl.TabPages.Insert(index, wp);

            // Update buttons to reflect status
            UpdateControlButtons();
        }
        
        private void OnWizardRemoved(int index, object value)
        {
            WizardPage wp = _tabControl.TabPages[index] as WizardPage;
        
            // Unhook from event handlers
            wp.FullPageChanged -= new EventHandler(OnWizardFullPageChanged);
            wp.SubTitleChanged -= new EventHandler(OnWizardSubTitleChanged);
            wp.CaptionTitleChanged -= new EventHandler(OnWizardCaptionTitleChanged);

            // Reflect change on underlying tab control
            _tabControl.TabPages.RemoveAt(index);

            // Update buttons to reflect status
            UpdateControlButtons();
        }
        
        private void OnWizardFullPageChanged(object sender, EventArgs e)
        {
            WizardPage wp = sender as WizardPage;
            
            // Is it the current page that has changed FullPage?
            if (_tabControl.SelectedIndex == _wizardPages.IndexOf(wp))
            {
                // Should we be presented in full page?
                if (wp.FullPage)
                    EnterFullPage();
                else
                {
                    // Controller profile is not allowed to be outside of FullMode
                    if (_profile != Profiles.Controller)
                        LeaveFullPage();
                }
            }
        }

        private void OnWizardSubTitleChanged(object sender, EventArgs e)
        {
            WizardPage wp = sender as WizardPage;
           
            // Is it the current page that has changed sub title?
            if (_tabControl.SelectedIndex == _wizardPages.IndexOf(wp))
            {
                // Force the sub title to be updated now
                _panelTop.Invalidate();
            }
        }        
        
        private void OnWizardCaptionTitleChanged(object sender, EventArgs e)
        {
            // Generate event so any dialog containing use can be notify
            if (WizardCaptionTitleChanged != null)
                WizardCaptionTitleChanged(this, e);
        }
    
        private void OnRepaintPanels(object sender, EventArgs e)
        {
            _panelTop.Invalidate();
            _panelBottom.Invalidate();
        }

        private void OnPaintTopPanel(object sender, PaintEventArgs pe)
        {
            int right = _panelTop.Width;
        
            // Any picture to draw?
            if (_picture != null)
            {
                // Calculate starting Y position to give equal space above and below image
                int Y = (int)((_panelTop.Height - _picture.Height) / 2);
                
                pe.Graphics.DrawImage(_picture, _panelTop.Width - _picture.Width - Y, Y, _picture.Width, _picture.Height);
                
                // Adjust right side by width of width and gaps around it
                right -= _picture.Width + Y + PANEL_GAP;
            }
        
            // Create main title drawing rectangle
            RectangleF drawRectF = new Rectangle(PANEL_GAP, PANEL_GAP, right - PANEL_GAP, _fontTitle.Height);
                                                
            using(StringFormat drawFormat = new StringFormat())
			{
				drawFormat.Alignment = StringAlignment.Near;
				drawFormat.LineAlignment = StringAlignment.Center;
				drawFormat.Trimming = StringTrimming.EllipsisCharacter;
				drawFormat.FormatFlags = StringFormatFlags.NoClip |
										 StringFormatFlags.NoWrap;
	            
				using(SolidBrush mainTitleBrush = new SolidBrush(_colorTitle))
					pe.Graphics.DrawString(_title, _fontTitle, mainTitleBrush, drawRectF, drawFormat);            
	             
				// Is there a selected tab for display?   
				if (_tabControl.SelectedIndex != -1)
				{                
					// Adjust rectangle for rest of the drawing text space
					drawRectF.Y = drawRectF.Bottom + (PANEL_GAP / 2);
					drawRectF.X += PANEL_GAP;
					drawRectF.Width -= PANEL_GAP;
					drawRectF.Height = _panelTop.Height - drawRectF.Y - (PANEL_GAP / 2);

					// No longer want to prevent word wrap to extra lines
					drawFormat.LineAlignment = StringAlignment.Near;
					drawFormat.FormatFlags = StringFormatFlags.NoClip;

					WizardPage wp = _tabControl.TabPages[_tabControl.SelectedIndex] as WizardPage;

					using(SolidBrush subTitleBrush = new SolidBrush(_colorSubTitle))
						pe.Graphics.DrawString(wp.SubTitle, _fontSubTitle, subTitleBrush, drawRectF, drawFormat);
				}

                using (Pen lightPen = new Pen(ControlPaint.LightLight(_panelBottom.BackColor)),
                            darkPen = new Pen(ControlPaint.Light(ControlPaint.Dark(_panelBottom.BackColor))))
				{
					pe.Graphics.DrawLine(darkPen, 0, _panelTop.Height - 2, _panelTop.Width, _panelTop.Height - 2);
					pe.Graphics.DrawLine(lightPen, 0, _panelTop.Height - 1, _panelTop.Width, _panelTop.Height - 1);
				}            
			}
        }
        
        private void OnPaintBottomPanel(object sender, PaintEventArgs pe)
        {
            using (Pen lightPen = new Pen(ControlPaint.LightLight(_panelBottom.BackColor)),
                       darkPen = new Pen(ControlPaint.Light(ControlPaint.Dark(_panelBottom.BackColor))))
            {
                pe.Graphics.DrawLine(darkPen, 0, 0, _panelBottom.Width, 0);
                pe.Graphics.DrawLine(lightPen, 0, 1, _panelBottom.Width, 1);
            }            
        }

		private void WizardControl_Load(object sender, System.EventArgs e)
		{
			timerResize.Start();
		}

		private void timerResize_Tick(object sender, System.EventArgs e)
		{
			timerResize.Stop();
			
			// Reduce of header by one to force resize
			_panelTop.Height--;
		}
    }
}
