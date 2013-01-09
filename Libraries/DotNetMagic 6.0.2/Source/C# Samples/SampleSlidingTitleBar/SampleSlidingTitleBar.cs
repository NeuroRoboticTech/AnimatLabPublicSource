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

namespace SampleSlidingTitleBar
{
	/// <summary>
	/// Summary description for SampleSlidingTitleBar.
	/// </summary>
	public class SampleSlidingTitleBar : System.Windows.Forms.Form
	{
		private Image _image;
		private bool _picture;
		private PictureBox _pictureBox;
		private ExampleUserControl _buttons;
	
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Crownwood.DotNetMagic.Controls.SlidingTitleBar slidingTitleBar1;
		private System.Windows.Forms.GroupBox groupBoxEdge;
		private System.Windows.Forms.RadioButton radioTop;
		private System.Windows.Forms.RadioButton radioBottom;
		private System.Windows.Forms.RadioButton radioLeft;
		private System.Windows.Forms.RadioButton radioRight;
		private System.Windows.Forms.GroupBox groupSliding;
		private System.Windows.Forms.NumericUpDown upSteps;
		private System.Windows.Forms.Label labelSteps;
		private System.Windows.Forms.Label labelDuration;
		private System.Windows.Forms.NumericUpDown upDuration;
		private System.Windows.Forms.Label labelDelay;
		private System.Windows.Forms.CheckBox checkBoxOnHover;
		private System.Windows.Forms.NumericUpDown upDelay;
		private System.Windows.Forms.GroupBox groupBoxDisplay;
		private System.Windows.Forms.CheckBox checkBoxOpen;
		private System.Windows.Forms.CheckBox checkBoxArrows;
		private System.Windows.Forms.GroupBox groupBoxContent;
		private System.Windows.Forms.RadioButton radioButtonPicture;
		private System.Windows.Forms.RadioButton radioButtonButtons;
		private System.Windows.Forms.GroupBox groupBoxTitleBar;
		private System.Windows.Forms.RadioButton radioButtonCustom1;
		private System.Windows.Forms.RadioButton radioButtonDef;
		private System.Windows.Forms.RadioButton radioButtonCustom2;
		private System.Windows.Forms.RadioButton radioButtonCustom3;
        private RadioButton radioOffice2007Blue;
        private GroupBox groupBoxStyle;
        private RadioButton radioPlain;
        private RadioButton radioIDE2005;
        private RadioButton radioOffice2003;
        private RadioButton radioOffice2007Black;
        private RadioButton radioOffice2007Silver;
        private RadioButton radioMediaPlayerPurple;
        private RadioButton radioMediaPlayerOrange;
        private RadioButton radioMediaPlayerBlue;
		private System.ComponentModel.Container components = null;

		public SampleSlidingTitleBar()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			// Load the example picture used in the default slider content			
			_image = ResourceHelper.LoadBitmap(this.GetType(), "SampleSlidingTitleBar.ExamplePicture.jpg");
			
			// Create the picture box and button examples
			_pictureBox = new PictureBox();
			_pictureBox.Image = _image;
			_pictureBox.Dock = DockStyle.Fill;
			_buttons = new ExampleUserControl();
			_buttons.Bar = slidingTitleBar1;
			_buttons.Dock = DockStyle.Fill;

			// Always start with the picture example
			slidingTitleBar1.Panel.Controls.Add(_pictureBox);
			_picture = true;
			
			UpdateControls();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SampleSlidingTitleBar));
            this.slidingTitleBar1 = new Crownwood.DotNetMagic.Controls.SlidingTitleBar();
            this.groupBoxEdge = new System.Windows.Forms.GroupBox();
            this.radioRight = new System.Windows.Forms.RadioButton();
            this.radioLeft = new System.Windows.Forms.RadioButton();
            this.radioBottom = new System.Windows.Forms.RadioButton();
            this.radioTop = new System.Windows.Forms.RadioButton();
            this.groupSliding = new System.Windows.Forms.GroupBox();
            this.checkBoxOnHover = new System.Windows.Forms.CheckBox();
            this.upDelay = new System.Windows.Forms.NumericUpDown();
            this.labelDelay = new System.Windows.Forms.Label();
            this.upDuration = new System.Windows.Forms.NumericUpDown();
            this.labelDuration = new System.Windows.Forms.Label();
            this.labelSteps = new System.Windows.Forms.Label();
            this.upSteps = new System.Windows.Forms.NumericUpDown();
            this.groupBoxDisplay = new System.Windows.Forms.GroupBox();
            this.checkBoxArrows = new System.Windows.Forms.CheckBox();
            this.checkBoxOpen = new System.Windows.Forms.CheckBox();
            this.groupBoxContent = new System.Windows.Forms.GroupBox();
            this.radioButtonButtons = new System.Windows.Forms.RadioButton();
            this.radioButtonPicture = new System.Windows.Forms.RadioButton();
            this.groupBoxTitleBar = new System.Windows.Forms.GroupBox();
            this.radioButtonCustom3 = new System.Windows.Forms.RadioButton();
            this.radioButtonCustom2 = new System.Windows.Forms.RadioButton();
            this.radioButtonCustom1 = new System.Windows.Forms.RadioButton();
            this.radioButtonDef = new System.Windows.Forms.RadioButton();
            this.groupBoxStyle = new System.Windows.Forms.GroupBox();
            this.radioPlain = new System.Windows.Forms.RadioButton();
            this.radioIDE2005 = new System.Windows.Forms.RadioButton();
            this.radioOffice2003 = new System.Windows.Forms.RadioButton();
            this.radioOffice2007Black = new System.Windows.Forms.RadioButton();
            this.radioOffice2007Silver = new System.Windows.Forms.RadioButton();
            this.radioOffice2007Blue = new System.Windows.Forms.RadioButton();
            this.radioMediaPlayerPurple = new System.Windows.Forms.RadioButton();
            this.radioMediaPlayerOrange = new System.Windows.Forms.RadioButton();
            this.radioMediaPlayerBlue = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.slidingTitleBar1)).BeginInit();
            this.groupBoxEdge.SuspendLayout();
            this.groupSliding.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upSteps)).BeginInit();
            this.groupBoxDisplay.SuspendLayout();
            this.groupBoxContent.SuspendLayout();
            this.groupBoxTitleBar.SuspendLayout();
            this.groupBoxStyle.SuspendLayout();
            this.SuspendLayout();
            // 
            // slidingTitleBar1
            // 
            this.slidingTitleBar1.GradientDirection = Crownwood.DotNetMagic.Controls.GradientDirection.TopToBottom;
            this.slidingTitleBar1.Location = new System.Drawing.Point(190, 20);
            this.slidingTitleBar1.MouseOverColor = System.Drawing.Color.Empty;
            this.slidingTitleBar1.Name = "slidingTitleBar1";
            // 
            // 
            // 
            this.slidingTitleBar1.Panel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.slidingTitleBar1.Panel.Location = new System.Drawing.Point(0, 26);
            this.slidingTitleBar1.Panel.Name = "";
            this.slidingTitleBar1.Panel.Size = new System.Drawing.Size(264, 238);
            this.slidingTitleBar1.Panel.TabIndex = 1;
            this.slidingTitleBar1.Size = new System.Drawing.Size(266, 266);
            this.slidingTitleBar1.SlideSteps = 10;
            this.slidingTitleBar1.TabIndex = 6;
            this.slidingTitleBar1.Text = "SlidingTitleBar Example";
            this.slidingTitleBar1.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.slidingTitleBar1.OpenChanged += new System.EventHandler(this.checkBoxOpen_OpenChanged);
            // 
            // groupBoxEdge
            // 
            this.groupBoxEdge.Controls.Add(this.radioRight);
            this.groupBoxEdge.Controls.Add(this.radioLeft);
            this.groupBoxEdge.Controls.Add(this.radioBottom);
            this.groupBoxEdge.Controls.Add(this.radioTop);
            this.groupBoxEdge.Location = new System.Drawing.Point(12, 240);
            this.groupBoxEdge.Name = "groupBoxEdge";
            this.groupBoxEdge.Size = new System.Drawing.Size(128, 112);
            this.groupBoxEdge.TabIndex = 2;
            this.groupBoxEdge.TabStop = false;
            this.groupBoxEdge.Text = "TitleBar Edge";
            // 
            // radioRight
            // 
            this.radioRight.Location = new System.Drawing.Point(16, 82);
            this.radioRight.Name = "radioRight";
            this.radioRight.Size = new System.Drawing.Size(72, 24);
            this.radioRight.TabIndex = 3;
            this.radioRight.Text = "Right";
            this.radioRight.CheckedChanged += new System.EventHandler(this.radioRight_CheckedChanged);
            // 
            // radioLeft
            // 
            this.radioLeft.Location = new System.Drawing.Point(16, 61);
            this.radioLeft.Name = "radioLeft";
            this.radioLeft.Size = new System.Drawing.Size(72, 24);
            this.radioLeft.TabIndex = 2;
            this.radioLeft.Text = "Left";
            this.radioLeft.CheckedChanged += new System.EventHandler(this.radioLeft_CheckedChanged);
            // 
            // radioBottom
            // 
            this.radioBottom.Location = new System.Drawing.Point(16, 40);
            this.radioBottom.Name = "radioBottom";
            this.radioBottom.Size = new System.Drawing.Size(72, 24);
            this.radioBottom.TabIndex = 1;
            this.radioBottom.Text = "Bottom";
            this.radioBottom.CheckedChanged += new System.EventHandler(this.radioBottom_CheckedChanged);
            // 
            // radioTop
            // 
            this.radioTop.Location = new System.Drawing.Point(16, 19);
            this.radioTop.Name = "radioTop";
            this.radioTop.Size = new System.Drawing.Size(72, 24);
            this.radioTop.TabIndex = 0;
            this.radioTop.Text = "Top";
            this.radioTop.CheckedChanged += new System.EventHandler(this.radioTop_CheckedChanged);
            // 
            // groupSliding
            // 
            this.groupSliding.Controls.Add(this.checkBoxOnHover);
            this.groupSliding.Controls.Add(this.upDelay);
            this.groupSliding.Controls.Add(this.labelDelay);
            this.groupSliding.Controls.Add(this.upDuration);
            this.groupSliding.Controls.Add(this.labelDuration);
            this.groupSliding.Controls.Add(this.labelSteps);
            this.groupSliding.Controls.Add(this.upSteps);
            this.groupSliding.Location = new System.Drawing.Point(12, 12);
            this.groupSliding.Name = "groupSliding";
            this.groupSliding.Size = new System.Drawing.Size(128, 146);
            this.groupSliding.TabIndex = 0;
            this.groupSliding.TabStop = false;
            this.groupSliding.Text = "Sliding";
            // 
            // checkBoxOnHover
            // 
            this.checkBoxOnHover.Checked = true;
            this.checkBoxOnHover.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxOnHover.Location = new System.Drawing.Point(16, 109);
            this.checkBoxOnHover.Name = "checkBoxOnHover";
            this.checkBoxOnHover.Size = new System.Drawing.Size(104, 24);
            this.checkBoxOnHover.TabIndex = 6;
            this.checkBoxOnHover.Text = "Slide on Hover";
            this.checkBoxOnHover.CheckedChanged += new System.EventHandler(this.checkBoxOnHover_CheckedChanged);
            // 
            // upDelay
            // 
            this.upDelay.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.upDelay.Location = new System.Drawing.Point(62, 80);
            this.upDelay.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.upDelay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.upDelay.Name = "upDelay";
            this.upDelay.Size = new System.Drawing.Size(56, 20);
            this.upDelay.TabIndex = 5;
            this.upDelay.Value = new decimal(new int[] {
            333,
            0,
            0,
            0});
            this.upDelay.ValueChanged += new System.EventHandler(this.upDelay_ValueChanged);
            // 
            // labelDelay
            // 
            this.labelDelay.Location = new System.Drawing.Point(6, 80);
            this.labelDelay.Name = "labelDelay";
            this.labelDelay.Size = new System.Drawing.Size(48, 23);
            this.labelDelay.TabIndex = 4;
            this.labelDelay.Text = "Delay";
            this.labelDelay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // upDuration
            // 
            this.upDuration.Increment = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.upDuration.Location = new System.Drawing.Point(62, 51);
            this.upDuration.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.upDuration.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.upDuration.Name = "upDuration";
            this.upDuration.Size = new System.Drawing.Size(56, 20);
            this.upDuration.TabIndex = 3;
            this.upDuration.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.upDuration.ValueChanged += new System.EventHandler(this.upDuration_ValueChanged);
            // 
            // labelDuration
            // 
            this.labelDuration.Location = new System.Drawing.Point(6, 51);
            this.labelDuration.Name = "labelDuration";
            this.labelDuration.Size = new System.Drawing.Size(48, 23);
            this.labelDuration.TabIndex = 2;
            this.labelDuration.Text = "Duration";
            this.labelDuration.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelSteps
            // 
            this.labelSteps.Location = new System.Drawing.Point(6, 22);
            this.labelSteps.Name = "labelSteps";
            this.labelSteps.Size = new System.Drawing.Size(48, 23);
            this.labelSteps.TabIndex = 0;
            this.labelSteps.Text = "Steps";
            this.labelSteps.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // upSteps
            // 
            this.upSteps.Location = new System.Drawing.Point(62, 22);
            this.upSteps.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.upSteps.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.upSteps.Name = "upSteps";
            this.upSteps.Size = new System.Drawing.Size(56, 20);
            this.upSteps.TabIndex = 1;
            this.upSteps.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.upSteps.ValueChanged += new System.EventHandler(this.upSteps_ValueChanged);
            // 
            // groupBoxDisplay
            // 
            this.groupBoxDisplay.Controls.Add(this.checkBoxArrows);
            this.groupBoxDisplay.Controls.Add(this.checkBoxOpen);
            this.groupBoxDisplay.Location = new System.Drawing.Point(12, 164);
            this.groupBoxDisplay.Name = "groupBoxDisplay";
            this.groupBoxDisplay.Size = new System.Drawing.Size(128, 70);
            this.groupBoxDisplay.TabIndex = 1;
            this.groupBoxDisplay.TabStop = false;
            this.groupBoxDisplay.Text = "Display";
            // 
            // checkBoxArrows
            // 
            this.checkBoxArrows.Location = new System.Drawing.Point(16, 38);
            this.checkBoxArrows.Name = "checkBoxArrows";
            this.checkBoxArrows.Size = new System.Drawing.Size(94, 24);
            this.checkBoxArrows.TabIndex = 1;
            this.checkBoxArrows.Text = "Arrows";
            this.checkBoxArrows.CheckedChanged += new System.EventHandler(this.checkBoxArrows_CheckedChanged);
            // 
            // checkBoxOpen
            // 
            this.checkBoxOpen.Location = new System.Drawing.Point(16, 17);
            this.checkBoxOpen.Name = "checkBoxOpen";
            this.checkBoxOpen.Size = new System.Drawing.Size(94, 24);
            this.checkBoxOpen.TabIndex = 0;
            this.checkBoxOpen.Text = "Open";
            this.checkBoxOpen.CheckedChanged += new System.EventHandler(this.checkBoxOpen_CheckedChanged);
            // 
            // groupBoxContent
            // 
            this.groupBoxContent.Controls.Add(this.radioButtonButtons);
            this.groupBoxContent.Controls.Add(this.radioButtonPicture);
            this.groupBoxContent.Location = new System.Drawing.Point(12, 358);
            this.groupBoxContent.Name = "groupBoxContent";
            this.groupBoxContent.Size = new System.Drawing.Size(126, 114);
            this.groupBoxContent.TabIndex = 3;
            this.groupBoxContent.TabStop = false;
            this.groupBoxContent.Text = "Example Content";
            // 
            // radioButtonButtons
            // 
            this.radioButtonButtons.Location = new System.Drawing.Point(16, 39);
            this.radioButtonButtons.Name = "radioButtonButtons";
            this.radioButtonButtons.Size = new System.Drawing.Size(72, 24);
            this.radioButtonButtons.TabIndex = 1;
            this.radioButtonButtons.Text = "Buttons";
            this.radioButtonButtons.CheckedChanged += new System.EventHandler(this.radioButtonButtons_CheckedChanged);
            // 
            // radioButtonPicture
            // 
            this.radioButtonPicture.Location = new System.Drawing.Point(16, 18);
            this.radioButtonPicture.Name = "radioButtonPicture";
            this.radioButtonPicture.Size = new System.Drawing.Size(72, 24);
            this.radioButtonPicture.TabIndex = 0;
            this.radioButtonPicture.Text = "Picture";
            this.radioButtonPicture.CheckedChanged += new System.EventHandler(this.radioButtonPicture_CheckedChanged);
            // 
            // groupBoxTitleBar
            // 
            this.groupBoxTitleBar.Controls.Add(this.radioButtonCustom3);
            this.groupBoxTitleBar.Controls.Add(this.radioButtonCustom2);
            this.groupBoxTitleBar.Controls.Add(this.radioButtonCustom1);
            this.groupBoxTitleBar.Controls.Add(this.radioButtonDef);
            this.groupBoxTitleBar.Location = new System.Drawing.Point(144, 304);
            this.groupBoxTitleBar.Name = "groupBoxTitleBar";
            this.groupBoxTitleBar.Size = new System.Drawing.Size(120, 168);
            this.groupBoxTitleBar.TabIndex = 4;
            this.groupBoxTitleBar.TabStop = false;
            this.groupBoxTitleBar.Text = "Coloring";
            // 
            // radioButtonCustom3
            // 
            this.radioButtonCustom3.Location = new System.Drawing.Point(17, 63);
            this.radioButtonCustom3.Name = "radioButtonCustom3";
            this.radioButtonCustom3.Size = new System.Drawing.Size(96, 24);
            this.radioButtonCustom3.TabIndex = 2;
            this.radioButtonCustom3.Text = "Custom 3";
            this.radioButtonCustom3.CheckedChanged += new System.EventHandler(this.radioButtonCustom3_CheckedChanged);
            // 
            // radioButtonCustom2
            // 
            this.radioButtonCustom2.Location = new System.Drawing.Point(17, 85);
            this.radioButtonCustom2.Name = "radioButtonCustom2";
            this.radioButtonCustom2.Size = new System.Drawing.Size(96, 24);
            this.radioButtonCustom2.TabIndex = 3;
            this.radioButtonCustom2.Text = "Custom 2";
            this.radioButtonCustom2.CheckedChanged += new System.EventHandler(this.radioButtonCustom2_CheckedChanged);
            // 
            // radioButtonCustom1
            // 
            this.radioButtonCustom1.Location = new System.Drawing.Point(17, 41);
            this.radioButtonCustom1.Name = "radioButtonCustom1";
            this.radioButtonCustom1.Size = new System.Drawing.Size(96, 24);
            this.radioButtonCustom1.TabIndex = 1;
            this.radioButtonCustom1.Text = "Custom 1";
            this.radioButtonCustom1.CheckedChanged += new System.EventHandler(this.radioButtonCustom1_CheckedChanged);
            // 
            // radioButtonDef
            // 
            this.radioButtonDef.Checked = true;
            this.radioButtonDef.Location = new System.Drawing.Point(17, 19);
            this.radioButtonDef.Name = "radioButtonDef";
            this.radioButtonDef.Size = new System.Drawing.Size(96, 24);
            this.radioButtonDef.TabIndex = 0;
            this.radioButtonDef.TabStop = true;
            this.radioButtonDef.Text = "Default Colors";
            this.radioButtonDef.CheckedChanged += new System.EventHandler(this.radioButtonDef_CheckedChanged);
            // 
            // groupBoxStyle
            // 
            this.groupBoxStyle.Controls.Add(this.radioMediaPlayerPurple);
            this.groupBoxStyle.Controls.Add(this.radioMediaPlayerOrange);
            this.groupBoxStyle.Controls.Add(this.radioMediaPlayerBlue);
            this.groupBoxStyle.Controls.Add(this.radioPlain);
            this.groupBoxStyle.Controls.Add(this.radioIDE2005);
            this.groupBoxStyle.Controls.Add(this.radioOffice2003);
            this.groupBoxStyle.Controls.Add(this.radioOffice2007Black);
            this.groupBoxStyle.Controls.Add(this.radioOffice2007Silver);
            this.groupBoxStyle.Controls.Add(this.radioOffice2007Blue);
            this.groupBoxStyle.Location = new System.Drawing.Point(270, 304);
            this.groupBoxStyle.Name = "groupBoxStyle";
            this.groupBoxStyle.Size = new System.Drawing.Size(219, 168);
            this.groupBoxStyle.TabIndex = 5;
            this.groupBoxStyle.TabStop = false;
            this.groupBoxStyle.Text = "Visual Style";
            // 
            // radioPlain
            // 
            this.radioPlain.Location = new System.Drawing.Point(131, 63);
            this.radioPlain.Name = "radioPlain";
            this.radioPlain.Size = new System.Drawing.Size(77, 24);
            this.radioPlain.TabIndex = 8;
            this.radioPlain.Text = "Plain";
            this.radioPlain.CheckedChanged += new System.EventHandler(this.radioPlain_CheckedChanged);
            // 
            // radioIDE2005
            // 
            this.radioIDE2005.Location = new System.Drawing.Point(131, 41);
            this.radioIDE2005.Name = "radioIDE2005";
            this.radioIDE2005.Size = new System.Drawing.Size(77, 24);
            this.radioIDE2005.TabIndex = 7;
            this.radioIDE2005.Text = "IDE2005";
            this.radioIDE2005.CheckedChanged += new System.EventHandler(this.radioIDE2005_CheckedChanged);
            // 
            // radioOffice2003
            // 
            this.radioOffice2003.Location = new System.Drawing.Point(131, 18);
            this.radioOffice2003.Name = "radioOffice2003";
            this.radioOffice2003.Size = new System.Drawing.Size(77, 24);
            this.radioOffice2003.TabIndex = 6;
            this.radioOffice2003.Text = "Office2003";
            this.radioOffice2003.CheckedChanged += new System.EventHandler(this.radioOffice2003_CheckedChanged);
            // 
            // radioOffice2007Black
            // 
            this.radioOffice2007Black.Location = new System.Drawing.Point(10, 67);
            this.radioOffice2007Black.Name = "radioOffice2007Black";
            this.radioOffice2007Black.Size = new System.Drawing.Size(115, 24);
            this.radioOffice2007Black.TabIndex = 2;
            this.radioOffice2007Black.Text = "Office2007 Black";
            this.radioOffice2007Black.CheckedChanged += new System.EventHandler(this.radioOffice2007Black_CheckedChanged);
            // 
            // radioOffice2007Silver
            // 
            this.radioOffice2007Silver.Location = new System.Drawing.Point(10, 43);
            this.radioOffice2007Silver.Name = "radioOffice2007Silver";
            this.radioOffice2007Silver.Size = new System.Drawing.Size(115, 24);
            this.radioOffice2007Silver.TabIndex = 1;
            this.radioOffice2007Silver.Text = "Office2007 Silver";
            this.radioOffice2007Silver.CheckedChanged += new System.EventHandler(this.radioOffice2007Silver_CheckedChanged);
            // 
            // radioOffice2007Blue
            // 
            this.radioOffice2007Blue.Checked = true;
            this.radioOffice2007Blue.Location = new System.Drawing.Point(10, 19);
            this.radioOffice2007Blue.Name = "radioOffice2007Blue";
            this.radioOffice2007Blue.Size = new System.Drawing.Size(115, 24);
            this.radioOffice2007Blue.TabIndex = 0;
            this.radioOffice2007Blue.Text = "Office2007 Blue";
            this.radioOffice2007Blue.CheckedChanged += new System.EventHandler(this.radioOffice2007Blue_CheckedChanged);
            // 
            // radioMediaPlayerPurple
            // 
            this.radioMediaPlayerPurple.Location = new System.Drawing.Point(10, 139);
            this.radioMediaPlayerPurple.Name = "radioMediaPlayerPurple";
            this.radioMediaPlayerPurple.Size = new System.Drawing.Size(146, 24);
            this.radioMediaPlayerPurple.TabIndex = 5;
            this.radioMediaPlayerPurple.Text = "Media Player Purple";
            this.radioMediaPlayerPurple.CheckedChanged += new System.EventHandler(this.radioMediaPlayerPurple_CheckedChanged);
            // 
            // radioMediaPlayerOrange
            // 
            this.radioMediaPlayerOrange.Location = new System.Drawing.Point(10, 115);
            this.radioMediaPlayerOrange.Name = "radioMediaPlayerOrange";
            this.radioMediaPlayerOrange.Size = new System.Drawing.Size(146, 24);
            this.radioMediaPlayerOrange.TabIndex = 4;
            this.radioMediaPlayerOrange.Text = "Media Player Orange";
            this.radioMediaPlayerOrange.CheckedChanged += new System.EventHandler(this.radioMediaPlayerOrange_CheckedChanged);
            // 
            // radioMediaPlayerBlue
            // 
            this.radioMediaPlayerBlue.Location = new System.Drawing.Point(10, 91);
            this.radioMediaPlayerBlue.Name = "radioMediaPlayerBlue";
            this.radioMediaPlayerBlue.Size = new System.Drawing.Size(146, 24);
            this.radioMediaPlayerBlue.TabIndex = 3;
            this.radioMediaPlayerBlue.Text = "Media Player Blue";
            this.radioMediaPlayerBlue.CheckedChanged += new System.EventHandler(this.radioMediaPlayerBlue_CheckedChanged);
            // 
            // SampleSlidingTitleBar
            // 
            this.ClientSize = new System.Drawing.Size(501, 476);
            this.Controls.Add(this.groupBoxStyle);
            this.Controls.Add(this.groupBoxTitleBar);
            this.Controls.Add(this.groupBoxContent);
            this.Controls.Add(this.groupBoxDisplay);
            this.Controls.Add(this.groupSliding);
            this.Controls.Add(this.groupBoxEdge);
            this.Controls.Add(this.slidingTitleBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SampleSlidingTitleBar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DotNetMagic - SlidingTitleBar";
            ((System.ComponentModel.ISupportInitialize)(this.slidingTitleBar1)).EndInit();
            this.groupBoxEdge.ResumeLayout(false);
            this.groupSliding.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.upDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upSteps)).EndInit();
            this.groupBoxDisplay.ResumeLayout(false);
            this.groupBoxContent.ResumeLayout(false);
            this.groupBoxTitleBar.ResumeLayout(false);
            this.groupBoxStyle.ResumeLayout(false);
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
            Application.Run(new SampleSlidingTitleBar());
		}

		private void UpdateControls()
		{
			radioTop.Checked = (slidingTitleBar1.Edge == TitleEdge.Top);
			radioBottom.Checked = (slidingTitleBar1.Edge == TitleEdge.Bottom);
			radioLeft.Checked = (slidingTitleBar1.Edge == TitleEdge.Left);
			radioRight.Checked = (slidingTitleBar1.Edge == TitleEdge.Right);
			upSteps.Value = slidingTitleBar1.SlideSteps;
			upDuration.Value = slidingTitleBar1.SlideDuration;
			checkBoxOnHover.Checked = slidingTitleBar1.SlideOnHover;
			upDelay.Value = slidingTitleBar1.HoverDelay;
			checkBoxOpen.Checked = slidingTitleBar1.Open;
			checkBoxArrows.Checked = slidingTitleBar1.Arrows;
			radioButtonPicture.Checked = _picture;
			radioButtonButtons.Checked = !_picture;
		}
		
		private void radioTop_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radioTop.Checked)
			{
				slidingTitleBar1.Edge = TitleEdge.Top;
				UpdateControls();
			}
		}

		private void radioBottom_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radioBottom.Checked)
			{
				slidingTitleBar1.Edge = TitleEdge.Bottom;
				UpdateControls();
			}
		}

		private void radioLeft_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radioLeft.Checked)
			{
				slidingTitleBar1.Edge = TitleEdge.Left;
				UpdateControls();
			}
		}

		private void radioRight_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radioRight.Checked)
			{
				slidingTitleBar1.Edge = TitleEdge.Right;
				UpdateControls();
			}
		}

		private void upSteps_ValueChanged(object sender, System.EventArgs e)
		{
			slidingTitleBar1.SlideSteps = (int)upSteps.Value;
		}

		private void upDuration_ValueChanged(object sender, System.EventArgs e)
		{
			slidingTitleBar1.SlideDuration = (int)upDuration.Value;
		}

		private void upDelay_ValueChanged(object sender, System.EventArgs e)
		{
			slidingTitleBar1.HoverDelay = (int)upDelay.Value;
		}

		private void checkBoxOnHover_CheckedChanged(object sender, System.EventArgs e)
		{
			slidingTitleBar1.SlideOnHover = checkBoxOnHover.Checked;
		}

		private void checkBoxOpen_OpenChanged(object sender, System.EventArgs e)
		{
			 checkBoxOpen.Checked = slidingTitleBar1.Open;
		}

		private void checkBoxOpen_CheckedChanged(object sender, System.EventArgs e)
		{
			slidingTitleBar1.Open = checkBoxOpen.Checked;
		}

		private void checkBoxArrows_CheckedChanged(object sender, System.EventArgs e)
		{
			slidingTitleBar1.Arrows = checkBoxArrows.Checked;
		}

		private void radioButtonPicture_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radioButtonPicture.Checked)
			{
				slidingTitleBar1.Panel.Controls.Clear();
				slidingTitleBar1.Panel.Controls.Add(_pictureBox);
				_picture = true;
			}
		}

		private void radioButtonButtons_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radioButtonButtons.Checked)
			{
				slidingTitleBar1.Panel.Controls.Clear();
				slidingTitleBar1.Panel.Controls.Add(_buttons);
				_picture = false;
			}
		}

		private void radioButtonDef_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radioButtonDef.Checked)
			{
                slidingTitleBar1.ResetBorderColor();
                slidingTitleBar1.ResetTitleBackColor();
				slidingTitleBar1.ResetTitleForeColor();
				slidingTitleBar1.ResetInactiveBackColor();
				slidingTitleBar1.ResetInactiveForeColor();
				slidingTitleBar1.ResetGradientActiveColor();
				slidingTitleBar1.ResetGradientInactiveColor();
			}
		}

		private void radioButtonCustom1_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radioButtonCustom1.Checked)
			{
                slidingTitleBar1.BorderColor = Color.DarkRed;
				slidingTitleBar1.TitleBackColor = Color.DarkRed;
				slidingTitleBar1.TitleForeColor = Color.White;
				slidingTitleBar1.InactiveBackColor = ControlPaint.Light(Color.DarkRed);
				slidingTitleBar1.InactiveForeColor = Color.Gray;
				slidingTitleBar1.GradientActiveColor = Color.Pink;
				slidingTitleBar1.GradientInactiveColor = ControlPaint.Light(Color.Pink);
			}
		}

		private void radioButtonCustom2_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radioButtonCustom2.Checked)
			{
                slidingTitleBar1.BorderColor = Color.Tan;
                slidingTitleBar1.TitleBackColor = Color.Moccasin;
				slidingTitleBar1.TitleForeColor = Color.Black;
				slidingTitleBar1.InactiveBackColor = ControlPaint.Light(Color.Moccasin);
				slidingTitleBar1.InactiveForeColor = Color.Gray;
				slidingTitleBar1.GradientActiveColor = Color.Tan;
				slidingTitleBar1.GradientInactiveColor = ControlPaint.Light(Color.Tan);
			}		
		}

		private void radioButtonCustom3_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radioButtonCustom3.Checked)
			{
                slidingTitleBar1.BorderColor = Color.Blue;
                slidingTitleBar1.TitleBackColor = Color.Blue;
				slidingTitleBar1.TitleForeColor = Color.White;
				slidingTitleBar1.InactiveBackColor = ControlPaint.Light(Color.Blue);
				slidingTitleBar1.InactiveForeColor = Color.Black;
				slidingTitleBar1.GradientActiveColor = Color.BlueViolet;
				slidingTitleBar1.GradientInactiveColor = ControlPaint.Light(Color.BlueViolet);
			}
		}

        private void radioOffice2007Blue_CheckedChanged(object sender, EventArgs e)
        {
            if (radioOffice2007Blue.Checked)
            {
                slidingTitleBar1.Style = VisualStyle.Office2007Blue;
            }
        }

        private void radioOffice2007Silver_CheckedChanged(object sender, EventArgs e)
        {
            if (radioOffice2007Silver.Checked)
            {
                slidingTitleBar1.Style = VisualStyle.Office2007Silver;
            }
        }

        private void radioOffice2007Black_CheckedChanged(object sender, EventArgs e)
        {
            if (radioOffice2007Black.Checked)
            {
                slidingTitleBar1.Style = VisualStyle.Office2007Black;
            }
        }

        private void radioMediaPlayerBlue_CheckedChanged(object sender, EventArgs e)
        {
            if (radioMediaPlayerBlue.Checked)
            {
                slidingTitleBar1.Style = VisualStyle.MediaPlayerBlue;
            }
        }

        private void radioMediaPlayerOrange_CheckedChanged(object sender, EventArgs e)
        {
            if (radioMediaPlayerOrange.Checked)
            {
                slidingTitleBar1.Style = VisualStyle.MediaPlayerOrange;
            }
        }

        private void radioMediaPlayerPurple_CheckedChanged(object sender, EventArgs e)
        {
            if (radioMediaPlayerPurple.Checked)
            {
                slidingTitleBar1.Style = VisualStyle.MediaPlayerPurple;
            }
        }

        private void radioOffice2003_CheckedChanged(object sender, EventArgs e)
        {
            if (radioOffice2003.Checked)
            {
                slidingTitleBar1.Style = VisualStyle.Office2003;
            }
        }

        private void radioIDE2005_CheckedChanged(object sender, EventArgs e)
        {
            if (radioIDE2005.Checked)
            {
                slidingTitleBar1.Style = VisualStyle.IDE2005;
            }
        }

        private void radioPlain_CheckedChanged(object sender, EventArgs e)
        {
            if (radioPlain.Checked)
            {
                slidingTitleBar1.Style = VisualStyle.Plain;
            }
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
	}
}
