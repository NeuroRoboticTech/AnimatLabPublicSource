// *****************************************************************************
// 
//  (c) Crownwood Software Ltd 2004-2006. All rights reserved. 
//	The software and associated documentation supplied hereunder are the 
//	proprietary information of Crownwood Software Ltd, Bracknell, 
//	Berkshire, England and are supplied subject to licence terms.
// 
//  Version 6.0.1.0 	www.dotnetmagic.com
// *****************************************************************************

using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Crownwood.DotNetMagic.Common;
using Crownwood.DotNetMagic.Controls;

namespace SampleTitleBar
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class SampleTitleBar : System.Windows.Forms.Form
	{
		private bool _notice;
		private Icon _icon;
		private Image _image;
		private Crownwood.DotNetMagic.Controls.TitleBar titleBar1;
		private System.Windows.Forms.GroupBox groupBoxText;
		private System.Windows.Forms.GroupBox groupBoxTextSep;
		private System.Windows.Forms.TextBox textBoxPre;
		private System.Windows.Forms.Label labelPreText;
		private System.Windows.Forms.Label labelCenterText;
		private System.Windows.Forms.TextBox textBoxCenter;
		private System.Windows.Forms.Label labelPostText;
		private System.Windows.Forms.TextBox textBoxPost;
		private System.Windows.Forms.TextBox textBoxPostSep;
		private System.Windows.Forms.Label labelPostSep;
		private System.Windows.Forms.TextBox textBoxPreSep;
		private System.Windows.Forms.Label labelPreSep;
		private System.Windows.Forms.GroupBox groupBoxTextAlignment;
		private System.Windows.Forms.RadioButton radioButtonTAN;
		private System.Windows.Forms.RadioButton radioButtonTAC;
		private System.Windows.Forms.RadioButton radioButtonTAF;
		private System.Windows.Forms.GroupBox groupBoxImageAlignment;
		private System.Windows.Forms.RadioButton radioButtonIAF;
		private System.Windows.Forms.RadioButton radioButtonIAN;
		private System.Windows.Forms.CheckBox checkBoxActive;
		private System.Windows.Forms.GroupBox groupBoxTB;
		private System.Windows.Forms.GroupBox groupBoxLineAlignment;
		private System.Windows.Forms.RadioButton radioButtonLAF;
		private System.Windows.Forms.RadioButton radioButtonLAC;
		private System.Windows.Forms.RadioButton radioButtonLAN;
		private System.Windows.Forms.ComboBox comboBoxImages;
		private System.Windows.Forms.CheckBox checkBoxHV;
		private System.Windows.Forms.GroupBox groupBoxGaps;
		private System.Windows.Forms.Label labelPL;
		private System.Windows.Forms.Label labelPR;
		private System.Windows.Forms.Label labelPT;
		private System.Windows.Forms.Label labelPB;
		private System.Windows.Forms.Label labelETI;
		private System.Windows.Forms.Label labelETA;
		private System.Windows.Forms.Label labelITT;
		private System.Windows.Forms.NumericUpDown numericUpDownPL;
		private System.Windows.Forms.NumericUpDown numericUpDownPR;
		private System.Windows.Forms.NumericUpDown numericUpDownPT;
		private System.Windows.Forms.NumericUpDown numericUpDownPB;
		private System.Windows.Forms.NumericUpDown numericUpDownETI;
		private System.Windows.Forms.NumericUpDown numericUpDownETA;
		private System.Windows.Forms.NumericUpDown numericUpDownITT;
		private System.Windows.Forms.GroupBox groupBox;
		private System.Windows.Forms.Label labelDirection;
		private System.Windows.Forms.Label labelColoring;
		private System.Windows.Forms.ComboBox comboBoxColoring;
		private System.Windows.Forms.ComboBox comboBoxDirection;
		private System.Windows.Forms.RadioButton radioButtonDef;
		private System.Windows.Forms.RadioButton radioButtonCustom1;
		private System.Windows.Forms.RadioButton radioButtonCustom2;
		private System.Windows.Forms.RadioButton radioButtonCustom3;
		private System.Windows.Forms.ComboBox comboBoxActAsButton;
		private System.Windows.Forms.Label labelActAsButton;
		private System.Windows.Forms.Label labelArrow;
		private System.Windows.Forms.ComboBox comboBoxArrow;
		private System.Windows.Forms.RadioButton radioButtonArrowFar;
		private System.Windows.Forms.RadioButton radioButtonArrowNear;
		private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioPlain;
		private System.Windows.Forms.RadioButton radioOffice2003;
        private System.Windows.Forms.RadioButton radioIDE2005;
        private RadioButton radioOffice2007Blue;
        private RadioButton radioOffice2007Silver;
        private RadioButton radioOffice2007Black;
        private RadioButton radioMediaPlayerBlue;
        private RadioButton radioMediaPlayerOrange;
        private RadioButton radioMediaPlayerPurple;
		private System.Windows.Forms.GroupBox groupBoxArrow;

		public SampleTitleBar()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			// Load example image resources
			_icon = ResourceHelper.LoadIcon(typeof(SampleTitleBar), "SampleTitleBar.AppIcon.ico");
			_image = ResourceHelper.LoadBitmap(typeof(SampleTitleBar), "SampleTitleBar.Example1.bmp");

			// Set the initial values from control
			RefreshControls();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SampleTitleBar));
            this.titleBar1 = new Crownwood.DotNetMagic.Controls.TitleBar();
            this.textBoxPre = new System.Windows.Forms.TextBox();
            this.labelPreText = new System.Windows.Forms.Label();
            this.labelCenterText = new System.Windows.Forms.Label();
            this.textBoxCenter = new System.Windows.Forms.TextBox();
            this.labelPostText = new System.Windows.Forms.Label();
            this.textBoxPost = new System.Windows.Forms.TextBox();
            this.groupBoxText = new System.Windows.Forms.GroupBox();
            this.groupBoxTextSep = new System.Windows.Forms.GroupBox();
            this.textBoxPostSep = new System.Windows.Forms.TextBox();
            this.labelPostSep = new System.Windows.Forms.Label();
            this.textBoxPreSep = new System.Windows.Forms.TextBox();
            this.labelPreSep = new System.Windows.Forms.Label();
            this.groupBoxTextAlignment = new System.Windows.Forms.GroupBox();
            this.radioButtonTAF = new System.Windows.Forms.RadioButton();
            this.radioButtonTAC = new System.Windows.Forms.RadioButton();
            this.radioButtonTAN = new System.Windows.Forms.RadioButton();
            this.groupBoxImageAlignment = new System.Windows.Forms.GroupBox();
            this.comboBoxImages = new System.Windows.Forms.ComboBox();
            this.radioButtonIAF = new System.Windows.Forms.RadioButton();
            this.radioButtonIAN = new System.Windows.Forms.RadioButton();
            this.checkBoxActive = new System.Windows.Forms.CheckBox();
            this.groupBoxTB = new System.Windows.Forms.GroupBox();
            this.checkBoxHV = new System.Windows.Forms.CheckBox();
            this.labelActAsButton = new System.Windows.Forms.Label();
            this.comboBoxActAsButton = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioOffice2007Blue = new System.Windows.Forms.RadioButton();
            this.radioOffice2007Silver = new System.Windows.Forms.RadioButton();
            this.radioOffice2007Black = new System.Windows.Forms.RadioButton();
            this.radioIDE2005 = new System.Windows.Forms.RadioButton();
            this.radioPlain = new System.Windows.Forms.RadioButton();
            this.radioOffice2003 = new System.Windows.Forms.RadioButton();
            this.groupBoxLineAlignment = new System.Windows.Forms.GroupBox();
            this.radioButtonLAF = new System.Windows.Forms.RadioButton();
            this.radioButtonLAC = new System.Windows.Forms.RadioButton();
            this.radioButtonLAN = new System.Windows.Forms.RadioButton();
            this.groupBoxGaps = new System.Windows.Forms.GroupBox();
            this.labelITT = new System.Windows.Forms.Label();
            this.numericUpDownITT = new System.Windows.Forms.NumericUpDown();
            this.labelETA = new System.Windows.Forms.Label();
            this.numericUpDownETA = new System.Windows.Forms.NumericUpDown();
            this.labelETI = new System.Windows.Forms.Label();
            this.numericUpDownETI = new System.Windows.Forms.NumericUpDown();
            this.labelPB = new System.Windows.Forms.Label();
            this.numericUpDownPB = new System.Windows.Forms.NumericUpDown();
            this.labelPT = new System.Windows.Forms.Label();
            this.numericUpDownPT = new System.Windows.Forms.NumericUpDown();
            this.labelPR = new System.Windows.Forms.Label();
            this.numericUpDownPR = new System.Windows.Forms.NumericUpDown();
            this.labelPL = new System.Windows.Forms.Label();
            this.numericUpDownPL = new System.Windows.Forms.NumericUpDown();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.radioButtonCustom3 = new System.Windows.Forms.RadioButton();
            this.radioButtonCustom2 = new System.Windows.Forms.RadioButton();
            this.radioButtonCustom1 = new System.Windows.Forms.RadioButton();
            this.radioButtonDef = new System.Windows.Forms.RadioButton();
            this.comboBoxDirection = new System.Windows.Forms.ComboBox();
            this.comboBoxColoring = new System.Windows.Forms.ComboBox();
            this.labelColoring = new System.Windows.Forms.Label();
            this.labelDirection = new System.Windows.Forms.Label();
            this.labelArrow = new System.Windows.Forms.Label();
            this.comboBoxArrow = new System.Windows.Forms.ComboBox();
            this.radioButtonArrowFar = new System.Windows.Forms.RadioButton();
            this.radioButtonArrowNear = new System.Windows.Forms.RadioButton();
            this.groupBoxArrow = new System.Windows.Forms.GroupBox();
            this.radioMediaPlayerBlue = new System.Windows.Forms.RadioButton();
            this.radioMediaPlayerOrange = new System.Windows.Forms.RadioButton();
            this.radioMediaPlayerPurple = new System.Windows.Forms.RadioButton();
            this.groupBoxText.SuspendLayout();
            this.groupBoxTextSep.SuspendLayout();
            this.groupBoxTextAlignment.SuspendLayout();
            this.groupBoxImageAlignment.SuspendLayout();
            this.groupBoxTB.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBoxLineAlignment.SuspendLayout();
            this.groupBoxGaps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownITT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownETA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownETI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPL)).BeginInit();
            this.groupBox.SuspendLayout();
            this.groupBoxArrow.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleBar1
            // 
            this.titleBar1.GradientColoring = Crownwood.DotNetMagic.Controls.GradientColoring.LightBackToDarkBack;
            this.titleBar1.Icon = ((System.Drawing.Icon)(resources.GetObject("titleBar1.Icon")));
            this.titleBar1.Location = new System.Drawing.Point(30, 30);
            this.titleBar1.MouseOverColor = System.Drawing.Color.Empty;
            this.titleBar1.Name = "titleBar1";
            this.titleBar1.Size = new System.Drawing.Size(250, 40);
            this.titleBar1.TabIndex = 2;
            this.titleBar1.Text = "Document";
            // 
            // textBoxPre
            // 
            this.textBoxPre.Location = new System.Drawing.Point(72, 32);
            this.textBoxPre.Name = "textBoxPre";
            this.textBoxPre.Size = new System.Drawing.Size(136, 20);
            this.textBoxPre.TabIndex = 3;
            this.textBoxPre.Text = "Application";
            this.textBoxPre.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // labelPreText
            // 
            this.labelPreText.Location = new System.Drawing.Point(24, 32);
            this.labelPreText.Name = "labelPreText";
            this.labelPreText.Size = new System.Drawing.Size(40, 16);
            this.labelPreText.TabIndex = 4;
            this.labelPreText.Text = "Pre";
            this.labelPreText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelCenterText
            // 
            this.labelCenterText.Location = new System.Drawing.Point(24, 64);
            this.labelCenterText.Name = "labelCenterText";
            this.labelCenterText.Size = new System.Drawing.Size(40, 16);
            this.labelCenterText.TabIndex = 6;
            this.labelCenterText.Text = "Center";
            this.labelCenterText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxCenter
            // 
            this.textBoxCenter.Location = new System.Drawing.Point(72, 64);
            this.textBoxCenter.Name = "textBoxCenter";
            this.textBoxCenter.Size = new System.Drawing.Size(136, 20);
            this.textBoxCenter.TabIndex = 5;
            this.textBoxCenter.Text = "Document";
            this.textBoxCenter.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // labelPostText
            // 
            this.labelPostText.Location = new System.Drawing.Point(16, 96);
            this.labelPostText.Name = "labelPostText";
            this.labelPostText.Size = new System.Drawing.Size(48, 16);
            this.labelPostText.TabIndex = 8;
            this.labelPostText.Text = "Post";
            this.labelPostText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPost
            // 
            this.textBoxPost.Location = new System.Drawing.Point(72, 96);
            this.textBoxPost.Name = "textBoxPost";
            this.textBoxPost.Size = new System.Drawing.Size(136, 20);
            this.textBoxPost.TabIndex = 7;
            this.textBoxPost.Text = "[design]";
            this.textBoxPost.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // groupBoxText
            // 
            this.groupBoxText.Controls.Add(this.labelPostText);
            this.groupBoxText.Controls.Add(this.textBoxCenter);
            this.groupBoxText.Controls.Add(this.textBoxPost);
            this.groupBoxText.Controls.Add(this.labelCenterText);
            this.groupBoxText.Controls.Add(this.textBoxPre);
            this.groupBoxText.Controls.Add(this.labelPreText);
            this.groupBoxText.Location = new System.Drawing.Point(504, 12);
            this.groupBoxText.Name = "groupBoxText";
            this.groupBoxText.Size = new System.Drawing.Size(224, 136);
            this.groupBoxText.TabIndex = 9;
            this.groupBoxText.TabStop = false;
            this.groupBoxText.Text = "Text";
            // 
            // groupBoxTextSep
            // 
            this.groupBoxTextSep.Controls.Add(this.textBoxPostSep);
            this.groupBoxTextSep.Controls.Add(this.labelPostSep);
            this.groupBoxTextSep.Controls.Add(this.textBoxPreSep);
            this.groupBoxTextSep.Controls.Add(this.labelPreSep);
            this.groupBoxTextSep.Location = new System.Drawing.Point(504, 154);
            this.groupBoxTextSep.Name = "groupBoxTextSep";
            this.groupBoxTextSep.Size = new System.Drawing.Size(224, 114);
            this.groupBoxTextSep.TabIndex = 10;
            this.groupBoxTextSep.TabStop = false;
            this.groupBoxTextSep.Text = "Separators";
            // 
            // textBoxPostSep
            // 
            this.textBoxPostSep.Location = new System.Drawing.Point(72, 64);
            this.textBoxPostSep.Name = "textBoxPostSep";
            this.textBoxPostSep.Size = new System.Drawing.Size(136, 20);
            this.textBoxPostSep.TabIndex = 5;
            this.textBoxPostSep.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // labelPostSep
            // 
            this.labelPostSep.Location = new System.Drawing.Point(24, 64);
            this.labelPostSep.Name = "labelPostSep";
            this.labelPostSep.Size = new System.Drawing.Size(40, 16);
            this.labelPostSep.TabIndex = 6;
            this.labelPostSep.Text = "Post";
            this.labelPostSep.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPreSep
            // 
            this.textBoxPreSep.Location = new System.Drawing.Point(72, 32);
            this.textBoxPreSep.Name = "textBoxPreSep";
            this.textBoxPreSep.Size = new System.Drawing.Size(136, 20);
            this.textBoxPreSep.TabIndex = 3;
            this.textBoxPreSep.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // labelPreSep
            // 
            this.labelPreSep.Location = new System.Drawing.Point(24, 32);
            this.labelPreSep.Name = "labelPreSep";
            this.labelPreSep.Size = new System.Drawing.Size(40, 16);
            this.labelPreSep.TabIndex = 4;
            this.labelPreSep.Text = "Pre";
            this.labelPreSep.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBoxTextAlignment
            // 
            this.groupBoxTextAlignment.Controls.Add(this.radioButtonTAF);
            this.groupBoxTextAlignment.Controls.Add(this.radioButtonTAC);
            this.groupBoxTextAlignment.Controls.Add(this.radioButtonTAN);
            this.groupBoxTextAlignment.Location = new System.Drawing.Point(12, 274);
            this.groupBoxTextAlignment.Name = "groupBoxTextAlignment";
            this.groupBoxTextAlignment.Size = new System.Drawing.Size(162, 106);
            this.groupBoxTextAlignment.TabIndex = 11;
            this.groupBoxTextAlignment.TabStop = false;
            this.groupBoxTextAlignment.Text = "Text Alignment";
            // 
            // radioButtonTAF
            // 
            this.radioButtonTAF.Location = new System.Drawing.Point(16, 70);
            this.radioButtonTAF.Name = "radioButtonTAF";
            this.radioButtonTAF.Size = new System.Drawing.Size(64, 24);
            this.radioButtonTAF.TabIndex = 2;
            this.radioButtonTAF.Text = "Far";
            this.radioButtonTAF.CheckedChanged += new System.EventHandler(this.OnAlignmentChanged);
            // 
            // radioButtonTAC
            // 
            this.radioButtonTAC.Location = new System.Drawing.Point(16, 46);
            this.radioButtonTAC.Name = "radioButtonTAC";
            this.radioButtonTAC.Size = new System.Drawing.Size(64, 24);
            this.radioButtonTAC.TabIndex = 1;
            this.radioButtonTAC.Text = "Center";
            this.radioButtonTAC.CheckedChanged += new System.EventHandler(this.OnAlignmentChanged);
            // 
            // radioButtonTAN
            // 
            this.radioButtonTAN.Checked = true;
            this.radioButtonTAN.Location = new System.Drawing.Point(16, 24);
            this.radioButtonTAN.Name = "radioButtonTAN";
            this.radioButtonTAN.Size = new System.Drawing.Size(64, 24);
            this.radioButtonTAN.TabIndex = 0;
            this.radioButtonTAN.TabStop = true;
            this.radioButtonTAN.Text = "Near";
            this.radioButtonTAN.CheckedChanged += new System.EventHandler(this.OnAlignmentChanged);
            // 
            // groupBoxImageAlignment
            // 
            this.groupBoxImageAlignment.Controls.Add(this.comboBoxImages);
            this.groupBoxImageAlignment.Controls.Add(this.radioButtonIAF);
            this.groupBoxImageAlignment.Controls.Add(this.radioButtonIAN);
            this.groupBoxImageAlignment.Location = new System.Drawing.Point(311, 274);
            this.groupBoxImageAlignment.Name = "groupBoxImageAlignment";
            this.groupBoxImageAlignment.Size = new System.Drawing.Size(164, 106);
            this.groupBoxImageAlignment.TabIndex = 12;
            this.groupBoxImageAlignment.TabStop = false;
            this.groupBoxImageAlignment.Text = "Image Alignment";
            // 
            // comboBoxImages
            // 
            this.comboBoxImages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxImages.Items.AddRange(new object[] {
            "None",
            "Bitmap",
            "Icon"});
            this.comboBoxImages.Location = new System.Drawing.Point(16, 74);
            this.comboBoxImages.Name = "comboBoxImages";
            this.comboBoxImages.Size = new System.Drawing.Size(88, 21);
            this.comboBoxImages.TabIndex = 3;
            this.comboBoxImages.SelectedIndexChanged += new System.EventHandler(this.OnImageChanged);
            // 
            // radioButtonIAF
            // 
            this.radioButtonIAF.Location = new System.Drawing.Point(16, 46);
            this.radioButtonIAF.Name = "radioButtonIAF";
            this.radioButtonIAF.Size = new System.Drawing.Size(64, 24);
            this.radioButtonIAF.TabIndex = 2;
            this.radioButtonIAF.Text = "Far";
            this.radioButtonIAF.CheckedChanged += new System.EventHandler(this.OnAlignmentChanged);
            // 
            // radioButtonIAN
            // 
            this.radioButtonIAN.Checked = true;
            this.radioButtonIAN.Location = new System.Drawing.Point(16, 24);
            this.radioButtonIAN.Name = "radioButtonIAN";
            this.radioButtonIAN.Size = new System.Drawing.Size(64, 24);
            this.radioButtonIAN.TabIndex = 0;
            this.radioButtonIAN.TabStop = true;
            this.radioButtonIAN.Text = "Near";
            this.radioButtonIAN.CheckedChanged += new System.EventHandler(this.OnAlignmentChanged);
            // 
            // checkBoxActive
            // 
            this.checkBoxActive.Location = new System.Drawing.Point(152, 142);
            this.checkBoxActive.Name = "checkBoxActive";
            this.checkBoxActive.Size = new System.Drawing.Size(128, 24);
            this.checkBoxActive.TabIndex = 13;
            this.checkBoxActive.Text = "Active / Inactive";
            this.checkBoxActive.CheckedChanged += new System.EventHandler(this.OnActiveChanged);
            // 
            // groupBoxTB
            // 
            this.groupBoxTB.Controls.Add(this.titleBar1);
            this.groupBoxTB.Controls.Add(this.checkBoxHV);
            this.groupBoxTB.Controls.Add(this.checkBoxActive);
            this.groupBoxTB.Controls.Add(this.labelActAsButton);
            this.groupBoxTB.Controls.Add(this.comboBoxActAsButton);
            this.groupBoxTB.Location = new System.Drawing.Point(202, 12);
            this.groupBoxTB.Name = "groupBoxTB";
            this.groupBoxTB.Size = new System.Drawing.Size(296, 256);
            this.groupBoxTB.TabIndex = 14;
            this.groupBoxTB.TabStop = false;
            this.groupBoxTB.Text = "TitleBar Instance";
            // 
            // checkBoxHV
            // 
            this.checkBoxHV.Location = new System.Drawing.Point(152, 166);
            this.checkBoxHV.Name = "checkBoxHV";
            this.checkBoxHV.Size = new System.Drawing.Size(128, 24);
            this.checkBoxHV.TabIndex = 15;
            this.checkBoxHV.Text = "Horizontal / Vertical";
            this.checkBoxHV.CheckedChanged += new System.EventHandler(this.OnHVChanged);
            // 
            // labelActAsButton
            // 
            this.labelActAsButton.Location = new System.Drawing.Point(152, 198);
            this.labelActAsButton.Name = "labelActAsButton";
            this.labelActAsButton.Size = new System.Drawing.Size(96, 16);
            this.labelActAsButton.TabIndex = 20;
            this.labelActAsButton.Text = "Act As Button";
            this.labelActAsButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxActAsButton
            // 
            this.comboBoxActAsButton.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxActAsButton.Items.AddRange(new object[] {
            "No",
            "Whole Control",
            "Just Arrow"});
            this.comboBoxActAsButton.Location = new System.Drawing.Point(152, 214);
            this.comboBoxActAsButton.Name = "comboBoxActAsButton";
            this.comboBoxActAsButton.Size = new System.Drawing.Size(121, 21);
            this.comboBoxActAsButton.TabIndex = 19;
            this.comboBoxActAsButton.SelectedIndexChanged += new System.EventHandler(this.OnActAsButtonChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioMediaPlayerBlue);
            this.groupBox1.Controls.Add(this.radioMediaPlayerOrange);
            this.groupBox1.Controls.Add(this.radioMediaPlayerPurple);
            this.groupBox1.Controls.Add(this.radioOffice2007Blue);
            this.groupBox1.Controls.Add(this.radioOffice2007Silver);
            this.groupBox1.Controls.Add(this.radioOffice2007Black);
            this.groupBox1.Controls.Add(this.radioIDE2005);
            this.groupBox1.Controls.Add(this.radioPlain);
            this.groupBox1.Controls.Add(this.radioOffice2003);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(184, 256);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Style";
            // 
            // radioOffice2007Blue
            // 
            this.radioOffice2007Blue.Checked = true;
            this.radioOffice2007Blue.Location = new System.Drawing.Point(16, 24);
            this.radioOffice2007Blue.Name = "radioOffice2007Blue";
            this.radioOffice2007Blue.Size = new System.Drawing.Size(162, 24);
            this.radioOffice2007Blue.TabIndex = 0;
            this.radioOffice2007Blue.Text = "Office2007 Blue";
            this.radioOffice2007Blue.CheckedChanged += new System.EventHandler(this.radioOffice2007Blue_CheckedChanged);
            // 
            // radioOffice2007Silver
            // 
            this.radioOffice2007Silver.Location = new System.Drawing.Point(16, 48);
            this.radioOffice2007Silver.Name = "radioOffice2007Silver";
            this.radioOffice2007Silver.Size = new System.Drawing.Size(162, 24);
            this.radioOffice2007Silver.TabIndex = 1;
            this.radioOffice2007Silver.Text = "Office2007 Silver";
            this.radioOffice2007Silver.CheckedChanged += new System.EventHandler(this.radioOffice2007Silver_CheckedChanged);
            // 
            // radioOffice2007Black
            // 
            this.radioOffice2007Black.Location = new System.Drawing.Point(16, 72);
            this.radioOffice2007Black.Name = "radioOffice2007Black";
            this.radioOffice2007Black.Size = new System.Drawing.Size(162, 24);
            this.radioOffice2007Black.TabIndex = 2;
            this.radioOffice2007Black.Text = "Office2007 Black";
            this.radioOffice2007Black.CheckedChanged += new System.EventHandler(this.radioOffice2007Black_CheckedChanged);
            // 
            // radioIDE2005
            // 
            this.radioIDE2005.Location = new System.Drawing.Point(16, 192);
            this.radioIDE2005.Name = "radioIDE2005";
            this.radioIDE2005.Size = new System.Drawing.Size(162, 24);
            this.radioIDE2005.TabIndex = 7;
            this.radioIDE2005.Text = "IDE2005";
            this.radioIDE2005.CheckedChanged += new System.EventHandler(this.radioIDE2005_CheckedChanged);
            // 
            // radioPlain
            // 
            this.radioPlain.Location = new System.Drawing.Point(16, 216);
            this.radioPlain.Name = "radioPlain";
            this.radioPlain.Size = new System.Drawing.Size(162, 24);
            this.radioPlain.TabIndex = 8;
            this.radioPlain.Text = "Plain";
            this.radioPlain.CheckedChanged += new System.EventHandler(this.radioPlain_CheckedChanged);
            // 
            // radioOffice2003
            // 
            this.radioOffice2003.Location = new System.Drawing.Point(16, 168);
            this.radioOffice2003.Name = "radioOffice2003";
            this.radioOffice2003.Size = new System.Drawing.Size(162, 24);
            this.radioOffice2003.TabIndex = 6;
            this.radioOffice2003.Text = "Office2003";
            this.radioOffice2003.CheckedChanged += new System.EventHandler(this.radioOffice2003_CheckedChanged);
            // 
            // groupBoxLineAlignment
            // 
            this.groupBoxLineAlignment.Controls.Add(this.radioButtonLAF);
            this.groupBoxLineAlignment.Controls.Add(this.radioButtonLAC);
            this.groupBoxLineAlignment.Controls.Add(this.radioButtonLAN);
            this.groupBoxLineAlignment.Location = new System.Drawing.Point(180, 274);
            this.groupBoxLineAlignment.Name = "groupBoxLineAlignment";
            this.groupBoxLineAlignment.Size = new System.Drawing.Size(125, 106);
            this.groupBoxLineAlignment.TabIndex = 12;
            this.groupBoxLineAlignment.TabStop = false;
            this.groupBoxLineAlignment.Text = "Line Alignment";
            // 
            // radioButtonLAF
            // 
            this.radioButtonLAF.Location = new System.Drawing.Point(16, 70);
            this.radioButtonLAF.Name = "radioButtonLAF";
            this.radioButtonLAF.Size = new System.Drawing.Size(64, 24);
            this.radioButtonLAF.TabIndex = 2;
            this.radioButtonLAF.Text = "Far";
            this.radioButtonLAF.CheckedChanged += new System.EventHandler(this.OnAlignmentChanged);
            // 
            // radioButtonLAC
            // 
            this.radioButtonLAC.Location = new System.Drawing.Point(16, 46);
            this.radioButtonLAC.Name = "radioButtonLAC";
            this.radioButtonLAC.Size = new System.Drawing.Size(64, 24);
            this.radioButtonLAC.TabIndex = 1;
            this.radioButtonLAC.Text = "Center";
            this.radioButtonLAC.CheckedChanged += new System.EventHandler(this.OnAlignmentChanged);
            // 
            // radioButtonLAN
            // 
            this.radioButtonLAN.Checked = true;
            this.radioButtonLAN.Location = new System.Drawing.Point(16, 24);
            this.radioButtonLAN.Name = "radioButtonLAN";
            this.radioButtonLAN.Size = new System.Drawing.Size(64, 24);
            this.radioButtonLAN.TabIndex = 0;
            this.radioButtonLAN.TabStop = true;
            this.radioButtonLAN.Text = "Near";
            this.radioButtonLAN.CheckedChanged += new System.EventHandler(this.OnAlignmentChanged);
            // 
            // groupBoxGaps
            // 
            this.groupBoxGaps.Controls.Add(this.labelITT);
            this.groupBoxGaps.Controls.Add(this.numericUpDownITT);
            this.groupBoxGaps.Controls.Add(this.labelETA);
            this.groupBoxGaps.Controls.Add(this.numericUpDownETA);
            this.groupBoxGaps.Controls.Add(this.labelETI);
            this.groupBoxGaps.Controls.Add(this.numericUpDownETI);
            this.groupBoxGaps.Controls.Add(this.labelPB);
            this.groupBoxGaps.Controls.Add(this.numericUpDownPB);
            this.groupBoxGaps.Controls.Add(this.labelPT);
            this.groupBoxGaps.Controls.Add(this.numericUpDownPT);
            this.groupBoxGaps.Controls.Add(this.labelPR);
            this.groupBoxGaps.Controls.Add(this.numericUpDownPR);
            this.groupBoxGaps.Controls.Add(this.labelPL);
            this.groupBoxGaps.Controls.Add(this.numericUpDownPL);
            this.groupBoxGaps.Location = new System.Drawing.Point(12, 388);
            this.groupBoxGaps.Name = "groupBoxGaps";
            this.groupBoxGaps.Size = new System.Drawing.Size(430, 152);
            this.groupBoxGaps.TabIndex = 16;
            this.groupBoxGaps.TabStop = false;
            this.groupBoxGaps.Text = "Gaps";
            // 
            // labelITT
            // 
            this.labelITT.Location = new System.Drawing.Point(168, 92);
            this.labelITT.Name = "labelITT";
            this.labelITT.Size = new System.Drawing.Size(88, 16);
            this.labelITT.TabIndex = 13;
            this.labelITT.Text = "Image To Text";
            this.labelITT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDownITT
            // 
            this.numericUpDownITT.Location = new System.Drawing.Point(264, 92);
            this.numericUpDownITT.Name = "numericUpDownITT";
            this.numericUpDownITT.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownITT.TabIndex = 12;
            this.numericUpDownITT.ValueChanged += new System.EventHandler(this.OnGapsChanged);
            // 
            // labelETA
            // 
            this.labelETA.Location = new System.Drawing.Point(168, 62);
            this.labelETA.Name = "labelETA";
            this.labelETA.Size = new System.Drawing.Size(88, 16);
            this.labelETA.TabIndex = 11;
            this.labelETA.Text = "Edge To Arrow";
            this.labelETA.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDownETA
            // 
            this.numericUpDownETA.Location = new System.Drawing.Point(264, 62);
            this.numericUpDownETA.Name = "numericUpDownETA";
            this.numericUpDownETA.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownETA.TabIndex = 10;
            this.numericUpDownETA.ValueChanged += new System.EventHandler(this.OnGapsChanged);
            // 
            // labelETI
            // 
            this.labelETI.Location = new System.Drawing.Point(168, 32);
            this.labelETI.Name = "labelETI";
            this.labelETI.Size = new System.Drawing.Size(88, 16);
            this.labelETI.TabIndex = 9;
            this.labelETI.Text = "Edge To Image";
            this.labelETI.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDownETI
            // 
            this.numericUpDownETI.Location = new System.Drawing.Point(264, 32);
            this.numericUpDownETI.Name = "numericUpDownETI";
            this.numericUpDownETI.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownETI.TabIndex = 8;
            this.numericUpDownETI.ValueChanged += new System.EventHandler(this.OnGapsChanged);
            // 
            // labelPB
            // 
            this.labelPB.Location = new System.Drawing.Point(8, 122);
            this.labelPB.Name = "labelPB";
            this.labelPB.Size = new System.Drawing.Size(64, 16);
            this.labelPB.TabIndex = 7;
            this.labelPB.Text = "Pad Bottom";
            this.labelPB.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDownPB
            // 
            this.numericUpDownPB.Location = new System.Drawing.Point(80, 122);
            this.numericUpDownPB.Name = "numericUpDownPB";
            this.numericUpDownPB.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownPB.TabIndex = 6;
            this.numericUpDownPB.ValueChanged += new System.EventHandler(this.OnGapsChanged);
            // 
            // labelPT
            // 
            this.labelPT.Location = new System.Drawing.Point(16, 92);
            this.labelPT.Name = "labelPT";
            this.labelPT.Size = new System.Drawing.Size(56, 16);
            this.labelPT.TabIndex = 5;
            this.labelPT.Text = "Pad Top";
            this.labelPT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDownPT
            // 
            this.numericUpDownPT.Location = new System.Drawing.Point(80, 92);
            this.numericUpDownPT.Name = "numericUpDownPT";
            this.numericUpDownPT.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownPT.TabIndex = 4;
            this.numericUpDownPT.ValueChanged += new System.EventHandler(this.OnGapsChanged);
            // 
            // labelPR
            // 
            this.labelPR.Location = new System.Drawing.Point(16, 62);
            this.labelPR.Name = "labelPR";
            this.labelPR.Size = new System.Drawing.Size(56, 16);
            this.labelPR.TabIndex = 3;
            this.labelPR.Text = "Pad Right";
            this.labelPR.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDownPR
            // 
            this.numericUpDownPR.Location = new System.Drawing.Point(80, 62);
            this.numericUpDownPR.Name = "numericUpDownPR";
            this.numericUpDownPR.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownPR.TabIndex = 2;
            this.numericUpDownPR.ValueChanged += new System.EventHandler(this.OnGapsChanged);
            // 
            // labelPL
            // 
            this.labelPL.Location = new System.Drawing.Point(16, 32);
            this.labelPL.Name = "labelPL";
            this.labelPL.Size = new System.Drawing.Size(56, 16);
            this.labelPL.TabIndex = 1;
            this.labelPL.Text = "Pad Left";
            this.labelPL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDownPL
            // 
            this.numericUpDownPL.Location = new System.Drawing.Point(80, 32);
            this.numericUpDownPL.Name = "numericUpDownPL";
            this.numericUpDownPL.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownPL.TabIndex = 0;
            this.numericUpDownPL.ValueChanged += new System.EventHandler(this.OnGapsChanged);
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.radioButtonCustom3);
            this.groupBox.Controls.Add(this.radioButtonCustom2);
            this.groupBox.Controls.Add(this.radioButtonCustom1);
            this.groupBox.Controls.Add(this.radioButtonDef);
            this.groupBox.Controls.Add(this.comboBoxDirection);
            this.groupBox.Controls.Add(this.comboBoxColoring);
            this.groupBox.Controls.Add(this.labelColoring);
            this.groupBox.Controls.Add(this.labelDirection);
            this.groupBox.Location = new System.Drawing.Point(448, 387);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(280, 152);
            this.groupBox.TabIndex = 18;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Background";
            // 
            // radioButtonCustom3
            // 
            this.radioButtonCustom3.Location = new System.Drawing.Point(144, 116);
            this.radioButtonCustom3.Name = "radioButtonCustom3";
            this.radioButtonCustom3.Size = new System.Drawing.Size(80, 24);
            this.radioButtonCustom3.TabIndex = 7;
            this.radioButtonCustom3.Text = "Custom 3";
            this.radioButtonCustom3.CheckedChanged += new System.EventHandler(this.OnCustom3Changed);
            // 
            // radioButtonCustom2
            // 
            this.radioButtonCustom2.Location = new System.Drawing.Point(144, 92);
            this.radioButtonCustom2.Name = "radioButtonCustom2";
            this.radioButtonCustom2.Size = new System.Drawing.Size(80, 24);
            this.radioButtonCustom2.TabIndex = 6;
            this.radioButtonCustom2.Text = "Custom 2";
            this.radioButtonCustom2.CheckedChanged += new System.EventHandler(this.OnCustom2Changed);
            // 
            // radioButtonCustom1
            // 
            this.radioButtonCustom1.Location = new System.Drawing.Point(32, 116);
            this.radioButtonCustom1.Name = "radioButtonCustom1";
            this.radioButtonCustom1.Size = new System.Drawing.Size(96, 24);
            this.radioButtonCustom1.TabIndex = 5;
            this.radioButtonCustom1.Text = "Custom 1";
            this.radioButtonCustom1.CheckedChanged += new System.EventHandler(this.OnCustom1Changed);
            // 
            // radioButtonDef
            // 
            this.radioButtonDef.Checked = true;
            this.radioButtonDef.Location = new System.Drawing.Point(32, 92);
            this.radioButtonDef.Name = "radioButtonDef";
            this.radioButtonDef.Size = new System.Drawing.Size(96, 24);
            this.radioButtonDef.TabIndex = 4;
            this.radioButtonDef.TabStop = true;
            this.radioButtonDef.Text = "Default Colors";
            this.radioButtonDef.CheckedChanged += new System.EventHandler(this.OnDefaultColorsChanged);
            // 
            // comboBoxDirection
            // 
            this.comboBoxDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDirection.Items.AddRange(new object[] {
            "None",
            "L to R",
            "TL to BR",
            "T to B",
            "TR to BL",
            "R to L",
            "BR to TL",
            "B to T",
            "BL to TR"});
            this.comboBoxDirection.Location = new System.Drawing.Point(88, 30);
            this.comboBoxDirection.Name = "comboBoxDirection";
            this.comboBoxDirection.Size = new System.Drawing.Size(136, 21);
            this.comboBoxDirection.TabIndex = 3;
            this.comboBoxDirection.SelectedIndexChanged += new System.EventHandler(this.OnDirectionChanged);
            // 
            // comboBoxColoring
            // 
            this.comboBoxColoring.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxColoring.Items.AddRange(new object[] {
            "Light To Back",
            "Light To Dark",
            "Back To Dark",
            "Back To Gradient",
            "Light To Gradient"});
            this.comboBoxColoring.Location = new System.Drawing.Point(88, 60);
            this.comboBoxColoring.Name = "comboBoxColoring";
            this.comboBoxColoring.Size = new System.Drawing.Size(136, 21);
            this.comboBoxColoring.TabIndex = 2;
            this.comboBoxColoring.SelectedIndexChanged += new System.EventHandler(this.OnColoringChanged);
            // 
            // labelColoring
            // 
            this.labelColoring.Location = new System.Drawing.Point(24, 60);
            this.labelColoring.Name = "labelColoring";
            this.labelColoring.Size = new System.Drawing.Size(56, 16);
            this.labelColoring.TabIndex = 1;
            this.labelColoring.Text = "Coloring";
            this.labelColoring.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelDirection
            // 
            this.labelDirection.Location = new System.Drawing.Point(24, 30);
            this.labelDirection.Name = "labelDirection";
            this.labelDirection.Size = new System.Drawing.Size(56, 16);
            this.labelDirection.TabIndex = 0;
            this.labelDirection.Text = "Direction";
            this.labelDirection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelArrow
            // 
            this.labelArrow.Location = new System.Drawing.Point(8, 24);
            this.labelArrow.Name = "labelArrow";
            this.labelArrow.Size = new System.Drawing.Size(48, 16);
            this.labelArrow.TabIndex = 21;
            this.labelArrow.Text = "Arrow";
            this.labelArrow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxArrow
            // 
            this.comboBoxArrow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxArrow.Items.AddRange(new object[] {
            "None",
            "Up",
            "Down",
            "Left",
            "Right",
            "Pinned",
            "Unpinned"});
            this.comboBoxArrow.Location = new System.Drawing.Point(64, 24);
            this.comboBoxArrow.Name = "comboBoxArrow";
            this.comboBoxArrow.Size = new System.Drawing.Size(121, 21);
            this.comboBoxArrow.TabIndex = 22;
            this.comboBoxArrow.SelectedIndexChanged += new System.EventHandler(this.OnArrowChanged);
            // 
            // radioButtonArrowFar
            // 
            this.radioButtonArrowFar.Checked = true;
            this.radioButtonArrowFar.Location = new System.Drawing.Point(64, 74);
            this.radioButtonArrowFar.Name = "radioButtonArrowFar";
            this.radioButtonArrowFar.Size = new System.Drawing.Size(96, 24);
            this.radioButtonArrowFar.TabIndex = 24;
            this.radioButtonArrowFar.TabStop = true;
            this.radioButtonArrowFar.Text = "Arrow Far";
            this.radioButtonArrowFar.CheckedChanged += new System.EventHandler(this.OnArrowRadioChanged);
            // 
            // radioButtonArrowNear
            // 
            this.radioButtonArrowNear.Location = new System.Drawing.Point(64, 50);
            this.radioButtonArrowNear.Name = "radioButtonArrowNear";
            this.radioButtonArrowNear.Size = new System.Drawing.Size(96, 24);
            this.radioButtonArrowNear.TabIndex = 23;
            this.radioButtonArrowNear.Text = "Arrow Near";
            this.radioButtonArrowNear.CheckedChanged += new System.EventHandler(this.OnArrowRadioChanged);
            // 
            // groupBoxArrow
            // 
            this.groupBoxArrow.Controls.Add(this.radioButtonArrowNear);
            this.groupBoxArrow.Controls.Add(this.comboBoxArrow);
            this.groupBoxArrow.Controls.Add(this.labelArrow);
            this.groupBoxArrow.Controls.Add(this.radioButtonArrowFar);
            this.groupBoxArrow.Location = new System.Drawing.Point(482, 274);
            this.groupBoxArrow.Name = "groupBoxArrow";
            this.groupBoxArrow.Size = new System.Drawing.Size(246, 106);
            this.groupBoxArrow.TabIndex = 25;
            this.groupBoxArrow.TabStop = false;
            this.groupBoxArrow.Text = "Arrow";
            // 
            // radioMediaPlayerBlue
            // 
            this.radioMediaPlayerBlue.Location = new System.Drawing.Point(16, 96);
            this.radioMediaPlayerBlue.Name = "radioMediaPlayerBlue";
            this.radioMediaPlayerBlue.Size = new System.Drawing.Size(162, 24);
            this.radioMediaPlayerBlue.TabIndex = 3;
            this.radioMediaPlayerBlue.Text = "Media Player Blue";
            this.radioMediaPlayerBlue.CheckedChanged += new System.EventHandler(this.radioMediaPlayerBlue_CheckedChanged);
            // 
            // radioMediaPlayerOrange
            // 
            this.radioMediaPlayerOrange.Location = new System.Drawing.Point(16, 120);
            this.radioMediaPlayerOrange.Name = "radioMediaPlayerOrange";
            this.radioMediaPlayerOrange.Size = new System.Drawing.Size(162, 24);
            this.radioMediaPlayerOrange.TabIndex = 4;
            this.radioMediaPlayerOrange.Text = "Media Player Orange";
            this.radioMediaPlayerOrange.CheckedChanged += new System.EventHandler(this.radioMediaPlayerOrange_CheckedChanged);
            // 
            // radioMediaPlayerPurple
            // 
            this.radioMediaPlayerPurple.Location = new System.Drawing.Point(16, 144);
            this.radioMediaPlayerPurple.Name = "radioMediaPlayerPurple";
            this.radioMediaPlayerPurple.Size = new System.Drawing.Size(162, 24);
            this.radioMediaPlayerPurple.TabIndex = 5;
            this.radioMediaPlayerPurple.Text = "Media Player Purple";
            this.radioMediaPlayerPurple.CheckedChanged += new System.EventHandler(this.radioMediaPlayerPurple_CheckedChanged);
            // 
            // SampleTitleBar
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(738, 551);
            this.Controls.Add(this.groupBoxArrow);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.groupBoxGaps);
            this.Controls.Add(this.groupBoxTB);
            this.Controls.Add(this.groupBoxTextAlignment);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxText);
            this.Controls.Add(this.groupBoxTextSep);
            this.Controls.Add(this.groupBoxImageAlignment);
            this.Controls.Add(this.groupBoxLineAlignment);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SampleTitleBar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DotNetMagic - SampleTitleBar";
            this.groupBoxText.ResumeLayout(false);
            this.groupBoxText.PerformLayout();
            this.groupBoxTextSep.ResumeLayout(false);
            this.groupBoxTextSep.PerformLayout();
            this.groupBoxTextAlignment.ResumeLayout(false);
            this.groupBoxImageAlignment.ResumeLayout(false);
            this.groupBoxTB.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBoxLineAlignment.ResumeLayout(false);
            this.groupBoxGaps.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownITT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownETA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownETI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPL)).EndInit();
            this.groupBox.ResumeLayout(false);
            this.groupBoxArrow.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
            Application.EnableVisualStyles();
            Application.Run(new SampleTitleBar());
		}

		private void RefreshControls()
		{
			_notice = false;

			// Initialize fields with control properties

			titleBar1.PreText = textBoxPre.Text;
			textBoxPreSep.Text = titleBar1.PreSeparator;
			titleBar1.Text = textBoxCenter.Text;
			textBoxPostSep.Text = titleBar1.PostSeparator;
			titleBar1.PostText = textBoxPost.Text;

			if (titleBar1.TextAlignment == StringAlignment.Near)  radioButtonTAN.Checked = true;
			if (titleBar1.TextAlignment == StringAlignment.Center)  radioButtonTAC.Checked = true;
			if (titleBar1.TextAlignment == StringAlignment.Far) radioButtonTAF.Checked = true;
			if (titleBar1.LineAlignment == StringAlignment.Near)  radioButtonLAN.Checked = true;
			if (titleBar1.LineAlignment == StringAlignment.Center)  radioButtonLAC.Checked = true;
			if (titleBar1.LineAlignment == StringAlignment.Far) radioButtonLAF.Checked = true;
			if (titleBar1.ImageAlignment == ImageAlignment.Near) radioButtonIAN.Checked = true;
			if (titleBar1.ImageAlignment == ImageAlignment.Far) radioButtonIAF.Checked = true;	

			// Setup gaps
			numericUpDownPL.Value = titleBar1.PadLeft;
			numericUpDownPR.Value = titleBar1.PadRight;
			numericUpDownPT.Value = titleBar1.PadTop;
			numericUpDownPB.Value = titleBar1.PadBottom;
			numericUpDownETI.Value = titleBar1.EdgeToImageGap;
			numericUpDownETA.Value = titleBar1.EdgeToArrowGap;
			numericUpDownITT.Value= titleBar1.ImageToTextGap;

			// Setup the image settings
			titleBar1.Icon = null;
			titleBar1.Image = _image;
			comboBoxImages.SelectedIndex = 1;

			comboBoxDirection.SelectedIndex = GradientToInt(titleBar1.GradientDirection);
			comboBoxColoring.SelectedIndex = (int)titleBar1.GradientColoring;
			comboBoxActAsButton.SelectedIndex = (int)titleBar1.ActAsButton;
			comboBoxArrow.SelectedIndex = (int)titleBar1.ArrowButton;
			checkBoxActive.Checked = titleBar1.Active;
			checkBoxHV.Checked = (titleBar1.Direction == LayoutDirection.Horizontal);

			_notice = true;
		}

		private void OnTextChanged(object sender, System.EventArgs e)
		{
			if (_notice)
			{
				// Update title bar with text related values
				titleBar1.PreText = textBoxPre.Text;
				titleBar1.PreSeparator = textBoxPreSep.Text;
				titleBar1.Text = textBoxCenter.Text;
				titleBar1.PostSeparator = textBoxPostSep.Text;
				titleBar1.PostText = textBoxPost.Text;
			}
		}

		private void OnAlignmentChanged(object sender, System.EventArgs e)
		{
			if (_notice)
			{
				if (radioButtonTAN.Checked) titleBar1.TextAlignment = StringAlignment.Near;
				if (radioButtonTAC.Checked) titleBar1.TextAlignment = StringAlignment.Center;
				if (radioButtonTAF.Checked) titleBar1.TextAlignment = StringAlignment.Far;
				if (radioButtonLAN.Checked) titleBar1.LineAlignment = StringAlignment.Near;
				if (radioButtonLAC.Checked) titleBar1.LineAlignment = StringAlignment.Center;
				if (radioButtonLAF.Checked) titleBar1.LineAlignment = StringAlignment.Far;
				if (radioButtonIAN.Checked) titleBar1.ImageAlignment = ImageAlignment.Near;
				if (radioButtonIAF.Checked) titleBar1.ImageAlignment = ImageAlignment.Far;
			}
		}

		private void OnImageChanged(object sender, System.EventArgs e)
		{
			if (_notice)
			{
				switch(comboBoxImages.SelectedIndex)
				{
					case 0:
						titleBar1.Icon = null;
						titleBar1.Image = null;
						break;
					case 1:
						titleBar1.Icon = null;
						titleBar1.Image = _image;
						break;
					case 2:
						titleBar1.Icon = _icon;
						titleBar1.Image = null;
						break;
				}
			}
		}

		private void OnActiveChanged(object sender, System.EventArgs e)
		{
			if (_notice)
				titleBar1.Active = checkBoxActive.Checked;
		}

		private void OnHVChanged(object sender, System.EventArgs e)
		{
			if (_notice)
			{
				titleBar1.Direction = checkBoxHV.Checked ? LayoutDirection.Horizontal : LayoutDirection.Vertical;

				if (checkBoxHV.Checked)
					titleBar1.SetBounds(30, 30, 250, 50);
				else
					titleBar1.SetBounds(30, 30, 50, 210);
			}
		}

		private void OnGapsChanged(object sender, System.EventArgs e)
		{
			if (_notice)
			{
				titleBar1.PadLeft = (int)numericUpDownPL.Value;
				titleBar1.PadRight = (int)numericUpDownPR.Value;
				titleBar1.PadTop = (int)numericUpDownPT.Value;
				titleBar1.PadBottom = (int)numericUpDownPB.Value;
				titleBar1.EdgeToImageGap = (int)numericUpDownETI.Value;
				titleBar1.EdgeToArrowGap = (int)numericUpDownETA.Value;
				titleBar1.ImageToTextGap = (int)numericUpDownITT.Value;
			}
		}

		private void OnDirectionChanged(object sender, System.EventArgs e)
		{
			if (_notice)
				titleBar1.GradientDirection = IntToGradient(comboBoxDirection.SelectedIndex);
		}

		private void OnColoringChanged(object sender, System.EventArgs e)
		{
			if (_notice)
				titleBar1.GradientColoring =(GradientColoring)comboBoxColoring.SelectedIndex;
		}


		private void OnActAsButtonChanged(object sender, System.EventArgs e)
		{
			if (_notice)
				titleBar1.ActAsButton =(ActAsButton)comboBoxActAsButton.SelectedIndex;
		}		

		private void OnArrowChanged(object sender, System.EventArgs e)
		{
			if (_notice)
				titleBar1.ArrowButton =(ArrowButton)comboBoxArrow.SelectedIndex;
		}

		private void OnArrowRadioChanged(object sender, System.EventArgs e)
		{
			if (_notice)
				if (radioButtonArrowFar.Checked)
					titleBar1.ArrowAlignment = ImageAlignment.Far;
				else
					titleBar1.ArrowAlignment = ImageAlignment.Near;
		}

        private void radioOffice2007Blue_CheckedChanged(object sender, EventArgs e)
        {
            if (_notice)
                titleBar1.Style = VisualStyle.Office2007Blue;
        }

        private void radioOffice2007Silver_CheckedChanged(object sender, EventArgs e)
        {
            if (_notice)
                titleBar1.Style = VisualStyle.Office2007Silver;
        }

        private void radioOffice2007Black_CheckedChanged(object sender, EventArgs e)
        {
            if (_notice)
                titleBar1.Style = VisualStyle.Office2007Black;
        }

        private void radioMediaPlayerBlue_CheckedChanged(object sender, EventArgs e)
        {
            if (_notice)
                titleBar1.Style = VisualStyle.MediaPlayerBlue;
        }

        private void radioMediaPlayerOrange_CheckedChanged(object sender, EventArgs e)
        {
            if (_notice)
                titleBar1.Style = VisualStyle.MediaPlayerOrange;
        }

        private void radioMediaPlayerPurple_CheckedChanged(object sender, EventArgs e)
        {
            if (_notice)
                titleBar1.Style = VisualStyle.MediaPlayerPurple;
        }

		private void radioOffice2003_CheckedChanged(object sender, System.EventArgs e)
		{	
			if (_notice)
				titleBar1.Style = VisualStyle.Office2003;
		}

		private void radioIDE2005_CheckedChanged(object sender, System.EventArgs e)
		{
			if (_notice)
				titleBar1.Style = VisualStyle.IDE2005;
		}

		private void radioPlain_CheckedChanged(object sender, System.EventArgs e)
		{
			if (_notice)
				titleBar1.Style = VisualStyle.Plain;
		}

		private int GradientToInt(GradientDirection dir)
		{
			switch(dir)
			{
				case GradientDirection.None:
				default:
					return 0;
				case GradientDirection.LeftToRight:
					return 1;
				case GradientDirection.TopLeftToBottomRight:
					return 2;
				case GradientDirection.TopToBottom:
					return 3;
				case GradientDirection.TopRightToBottomLeft:
					return 4;
				case GradientDirection.RightToLeft:
					return 5;
				case GradientDirection.BottomRightToTopLeft:
					return 6;
				case GradientDirection.BottomToTop:
					return 7;
				case GradientDirection.BottomLeftToTopRight:
					return 8;
			}
		}

		private GradientDirection IntToGradient(int val)
		{
			switch(val)
			{
				case 0:
				default:
					return GradientDirection.None;
				case 1:
					return GradientDirection.LeftToRight;
				case 2:
					return GradientDirection.TopLeftToBottomRight;
				case 3:
					return GradientDirection.TopToBottom;
				case 4:
					return GradientDirection.TopRightToBottomLeft;
				case 5:
					return GradientDirection.RightToLeft;
				case 6:
					return GradientDirection.BottomRightToTopLeft;
				case 7:
					return GradientDirection.BottomToTop;
				case 8:
					return GradientDirection.BottomLeftToTopRight;
			}
		}

		private void OnDefaultColorsChanged(object sender, System.EventArgs e)
		{
			if (_notice)
			{
				if (radioButtonDef.Checked)
				{
					titleBar1.ResetBackColor();
					titleBar1.ResetForeColor();
					titleBar1.ResetInactiveBackColor();
					titleBar1.ResetInactiveForeColor();
					titleBar1.ResetGradientActiveColor();
					titleBar1.ResetGradientInactiveColor();
				}
			}
		}

		private void OnCustom1Changed(object sender, System.EventArgs e)
		{
			if (_notice)
			{
				if (radioButtonCustom1.Checked)
				{
					titleBar1.BackColor = Color.DarkRed;
					titleBar1.ForeColor = Color.White;
					titleBar1.InactiveBackColor = ControlPaint.Light(Color.DarkRed);
					titleBar1.InactiveForeColor = Color.Gray;
					titleBar1.GradientActiveColor = Color.Pink;
					titleBar1.GradientInactiveColor = ControlPaint.Light(Color.Pink);
				}
			}
		}

		private void OnCustom2Changed(object sender, System.EventArgs e)
		{
			if (_notice)
			{
				if (radioButtonCustom2.Checked)
				{
					titleBar1.BackColor = Color.Moccasin;
					titleBar1.ForeColor = Color.Black;
					titleBar1.InactiveBackColor = ControlPaint.Light(Color.Moccasin);
					titleBar1.InactiveForeColor = Color.Gray;
					titleBar1.GradientActiveColor = Color.Tan;
					titleBar1.GradientInactiveColor = ControlPaint.Light(Color.Tan);
				}
			}
		}

		private void OnCustom3Changed(object sender, System.EventArgs e)
		{
			if (_notice)
			{
				if (radioButtonCustom3.Checked)
				{
					titleBar1.BackColor = Color.Green;
					titleBar1.ForeColor = Color.White;
					titleBar1.InactiveBackColor = ControlPaint.Light(Color.Green);
					titleBar1.InactiveForeColor = Color.Black;
					titleBar1.GradientActiveColor = Color.BlueViolet;
					titleBar1.GradientInactiveColor = ControlPaint.Light(Color.BlueViolet);
				}
			}
		}
	}
}
