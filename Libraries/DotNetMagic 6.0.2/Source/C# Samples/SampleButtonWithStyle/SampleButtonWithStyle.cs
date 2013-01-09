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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Crownwood.DotNetMagic.Common;
using Crownwood.DotNetMagic.Controls;

using Crownwood.DotNetMagic.Docking;

namespace SampleButtonWithStyle
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class SampleButtonWithStyle : System.Windows.Forms.Form
	{
        private System.Windows.Forms.GroupBox groupBoxStyle;
		private System.Windows.Forms.RadioButton radioButtonPlain;
		private System.Windows.Forms.GroupBox groupBoxDirection;
		private System.Windows.Forms.RadioButton radioButtonHorz;
		private System.Windows.Forms.RadioButton radioButtonVertical;
		private System.Windows.Forms.CheckBox checkBoxPushed;
		private System.Windows.Forms.GroupBox groupBoxButtonStyle;
		private System.Windows.Forms.RadioButton radioButtonPushButton;
		private System.Windows.Forms.RadioButton radioButtonToggleButton;
		private System.Windows.Forms.GroupBox groupBoxTextEdge;
		private System.Windows.Forms.RadioButton radioButtonTop;
		private System.Windows.Forms.RadioButton radioButtonBottom;
		private System.Windows.Forms.RadioButton radioButtonLeft;
		private System.Windows.Forms.RadioButton radioButtonRight;
		private System.Windows.Forms.CheckBox checkBoxEnabled;
		private System.Windows.Forms.GroupBox groupBoxExample;
		private Crownwood.DotNetMagic.Controls.ButtonWithStyle buttonText;
		private Crownwood.DotNetMagic.Controls.ButtonWithStyle buttonImage;
		private Crownwood.DotNetMagic.Controls.ButtonWithStyle buttonTextAndImage;
		private Crownwood.DotNetMagic.Controls.ButtonWithStyle buttonOne;
		private Crownwood.DotNetMagic.Controls.ButtonWithStyle buttonTwo;
		private Crownwood.DotNetMagic.Controls.ButtonWithStyle buttonThree;
		private Crownwood.DotNetMagic.Controls.ButtonWithStyle buttonFour;
		private Crownwood.DotNetMagic.Controls.ButtonWithStyle buttonEight;
		private Crownwood.DotNetMagic.Controls.ButtonWithStyle buttonSix;
		private Crownwood.DotNetMagic.Controls.ButtonWithStyle buttonSeven;
		private Crownwood.DotNetMagic.Controls.ButtonWithStyle buttonFive;
		private System.Windows.Forms.RadioButton radioButtonOffice2003;
		private System.Windows.Forms.RadioButton radioButtonIDE2005;
		private System.Windows.Forms.CheckBox checkBoxBorder;
        private RadioButton radioButtonOffice2007Blue;
        private RadioButton radioButtonOffice2007Silver;
        private RadioButton radioButtonOffice2007Black;
        private RadioButton radioButtonMediaPlayerPurple;
        private RadioButton radioButtonMediaPlayerOrange;
        private RadioButton radioButtonMediaPlayerBlue;
        private GroupBox groupBox1;

		public SampleButtonWithStyle()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SampleButtonWithStyle));
            this.groupBoxStyle = new System.Windows.Forms.GroupBox();
            this.radioButtonMediaPlayerPurple = new System.Windows.Forms.RadioButton();
            this.radioButtonMediaPlayerOrange = new System.Windows.Forms.RadioButton();
            this.radioButtonMediaPlayerBlue = new System.Windows.Forms.RadioButton();
            this.radioButtonOffice2007Blue = new System.Windows.Forms.RadioButton();
            this.radioButtonOffice2007Silver = new System.Windows.Forms.RadioButton();
            this.radioButtonOffice2007Black = new System.Windows.Forms.RadioButton();
            this.radioButtonIDE2005 = new System.Windows.Forms.RadioButton();
            this.radioButtonOffice2003 = new System.Windows.Forms.RadioButton();
            this.radioButtonPlain = new System.Windows.Forms.RadioButton();
            this.groupBoxDirection = new System.Windows.Forms.GroupBox();
            this.radioButtonVertical = new System.Windows.Forms.RadioButton();
            this.radioButtonHorz = new System.Windows.Forms.RadioButton();
            this.checkBoxPushed = new System.Windows.Forms.CheckBox();
            this.groupBoxButtonStyle = new System.Windows.Forms.GroupBox();
            this.radioButtonToggleButton = new System.Windows.Forms.RadioButton();
            this.radioButtonPushButton = new System.Windows.Forms.RadioButton();
            this.groupBoxTextEdge = new System.Windows.Forms.GroupBox();
            this.radioButtonRight = new System.Windows.Forms.RadioButton();
            this.radioButtonLeft = new System.Windows.Forms.RadioButton();
            this.radioButtonBottom = new System.Windows.Forms.RadioButton();
            this.radioButtonTop = new System.Windows.Forms.RadioButton();
            this.checkBoxEnabled = new System.Windows.Forms.CheckBox();
            this.groupBoxExample = new System.Windows.Forms.GroupBox();
            this.checkBoxBorder = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonEight = new Crownwood.DotNetMagic.Controls.ButtonWithStyle();
            this.buttonSix = new Crownwood.DotNetMagic.Controls.ButtonWithStyle();
            this.buttonSeven = new Crownwood.DotNetMagic.Controls.ButtonWithStyle();
            this.buttonFive = new Crownwood.DotNetMagic.Controls.ButtonWithStyle();
            this.buttonFour = new Crownwood.DotNetMagic.Controls.ButtonWithStyle();
            this.buttonThree = new Crownwood.DotNetMagic.Controls.ButtonWithStyle();
            this.buttonTwo = new Crownwood.DotNetMagic.Controls.ButtonWithStyle();
            this.buttonOne = new Crownwood.DotNetMagic.Controls.ButtonWithStyle();
            this.buttonText = new Crownwood.DotNetMagic.Controls.ButtonWithStyle();
            this.buttonImage = new Crownwood.DotNetMagic.Controls.ButtonWithStyle();
            this.buttonTextAndImage = new Crownwood.DotNetMagic.Controls.ButtonWithStyle();
            this.groupBoxStyle.SuspendLayout();
            this.groupBoxDirection.SuspendLayout();
            this.groupBoxButtonStyle.SuspendLayout();
            this.groupBoxTextEdge.SuspendLayout();
            this.groupBoxExample.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxStyle
            // 
            this.groupBoxStyle.Controls.Add(this.radioButtonMediaPlayerPurple);
            this.groupBoxStyle.Controls.Add(this.radioButtonMediaPlayerOrange);
            this.groupBoxStyle.Controls.Add(this.radioButtonMediaPlayerBlue);
            this.groupBoxStyle.Controls.Add(this.radioButtonOffice2007Blue);
            this.groupBoxStyle.Controls.Add(this.radioButtonOffice2007Silver);
            this.groupBoxStyle.Controls.Add(this.radioButtonOffice2007Black);
            this.groupBoxStyle.Controls.Add(this.radioButtonIDE2005);
            this.groupBoxStyle.Controls.Add(this.radioButtonOffice2003);
            this.groupBoxStyle.Controls.Add(this.radioButtonPlain);
            this.groupBoxStyle.Location = new System.Drawing.Point(12, 12);
            this.groupBoxStyle.Name = "groupBoxStyle";
            this.groupBoxStyle.Size = new System.Drawing.Size(164, 246);
            this.groupBoxStyle.TabIndex = 0;
            this.groupBoxStyle.TabStop = false;
            this.groupBoxStyle.Text = "Style";
            // 
            // radioButtonMediaPlayerPurple
            // 
            this.radioButtonMediaPlayerPurple.Location = new System.Drawing.Point(14, 139);
            this.radioButtonMediaPlayerPurple.Name = "radioButtonMediaPlayerPurple";
            this.radioButtonMediaPlayerPurple.Size = new System.Drawing.Size(132, 24);
            this.radioButtonMediaPlayerPurple.TabIndex = 5;
            this.radioButtonMediaPlayerPurple.Text = "Media Player Purple";
            this.radioButtonMediaPlayerPurple.CheckedChanged += new System.EventHandler(this.radioMediaPlayerPurple_CheckedChanged);
            // 
            // radioButtonMediaPlayerOrange
            // 
            this.radioButtonMediaPlayerOrange.Location = new System.Drawing.Point(16, 115);
            this.radioButtonMediaPlayerOrange.Name = "radioButtonMediaPlayerOrange";
            this.radioButtonMediaPlayerOrange.Size = new System.Drawing.Size(132, 24);
            this.radioButtonMediaPlayerOrange.TabIndex = 4;
            this.radioButtonMediaPlayerOrange.Text = "Media Player Orange";
            this.radioButtonMediaPlayerOrange.CheckedChanged += new System.EventHandler(this.radioButtonMediaPlayerOrange_CheckedChanged);
            // 
            // radioButtonMediaPlayerBlue
            // 
            this.radioButtonMediaPlayerBlue.Location = new System.Drawing.Point(16, 91);
            this.radioButtonMediaPlayerBlue.Name = "radioButtonMediaPlayerBlue";
            this.radioButtonMediaPlayerBlue.Size = new System.Drawing.Size(132, 24);
            this.radioButtonMediaPlayerBlue.TabIndex = 3;
            this.radioButtonMediaPlayerBlue.Text = "Media Player Blue";
            this.radioButtonMediaPlayerBlue.CheckedChanged += new System.EventHandler(this.radioButtonMediaPlayerBlue_CheckedChanged);
            // 
            // radioButtonOffice2007Blue
            // 
            this.radioButtonOffice2007Blue.Checked = true;
            this.radioButtonOffice2007Blue.Location = new System.Drawing.Point(16, 19);
            this.radioButtonOffice2007Blue.Name = "radioButtonOffice2007Blue";
            this.radioButtonOffice2007Blue.Size = new System.Drawing.Size(132, 24);
            this.radioButtonOffice2007Blue.TabIndex = 0;
            this.radioButtonOffice2007Blue.TabStop = true;
            this.radioButtonOffice2007Blue.Text = "Office2007 Blue";
            this.radioButtonOffice2007Blue.Click += new System.EventHandler(this.radioButtonOffice2007Blue_Click);
            // 
            // radioButtonOffice2007Silver
            // 
            this.radioButtonOffice2007Silver.Location = new System.Drawing.Point(16, 43);
            this.radioButtonOffice2007Silver.Name = "radioButtonOffice2007Silver";
            this.radioButtonOffice2007Silver.Size = new System.Drawing.Size(132, 24);
            this.radioButtonOffice2007Silver.TabIndex = 1;
            this.radioButtonOffice2007Silver.Text = "Office2007 Silver";
            this.radioButtonOffice2007Silver.Click += new System.EventHandler(this.radioButtonOffice2007Silver_Click);
            // 
            // radioButtonOffice2007Black
            // 
            this.radioButtonOffice2007Black.Location = new System.Drawing.Point(16, 67);
            this.radioButtonOffice2007Black.Name = "radioButtonOffice2007Black";
            this.radioButtonOffice2007Black.Size = new System.Drawing.Size(132, 24);
            this.radioButtonOffice2007Black.TabIndex = 2;
            this.radioButtonOffice2007Black.Text = "Office2007 Black";
            this.radioButtonOffice2007Black.Click += new System.EventHandler(this.radioButtonOffice2007Black_Click);
            // 
            // radioButtonIDE2005
            // 
            this.radioButtonIDE2005.Location = new System.Drawing.Point(16, 187);
            this.radioButtonIDE2005.Name = "radioButtonIDE2005";
            this.radioButtonIDE2005.Size = new System.Drawing.Size(132, 24);
            this.radioButtonIDE2005.TabIndex = 7;
            this.radioButtonIDE2005.Text = "IDE2005";
            this.radioButtonIDE2005.Click += new System.EventHandler(this.radioButtonIDE2005_Click);
            // 
            // radioButtonOffice2003
            // 
            this.radioButtonOffice2003.Location = new System.Drawing.Point(16, 163);
            this.radioButtonOffice2003.Name = "radioButtonOffice2003";
            this.radioButtonOffice2003.Size = new System.Drawing.Size(132, 24);
            this.radioButtonOffice2003.TabIndex = 6;
            this.radioButtonOffice2003.Text = "Office2003";
            this.radioButtonOffice2003.Click += new System.EventHandler(this.radioButtonOffice2003_Click);
            // 
            // radioButtonPlain
            // 
            this.radioButtonPlain.Location = new System.Drawing.Point(16, 211);
            this.radioButtonPlain.Name = "radioButtonPlain";
            this.radioButtonPlain.Size = new System.Drawing.Size(132, 24);
            this.radioButtonPlain.TabIndex = 8;
            this.radioButtonPlain.Text = "Plain";
            this.radioButtonPlain.Click += new System.EventHandler(this.radioButtonPlain_Click);
            // 
            // groupBoxDirection
            // 
            this.groupBoxDirection.Controls.Add(this.radioButtonVertical);
            this.groupBoxDirection.Controls.Add(this.radioButtonHorz);
            this.groupBoxDirection.Location = new System.Drawing.Point(182, 149);
            this.groupBoxDirection.Name = "groupBoxDirection";
            this.groupBoxDirection.Size = new System.Drawing.Size(128, 109);
            this.groupBoxDirection.TabIndex = 2;
            this.groupBoxDirection.TabStop = false;
            this.groupBoxDirection.Text = "Direction";
            // 
            // radioButtonVertical
            // 
            this.radioButtonVertical.Location = new System.Drawing.Point(16, 48);
            this.radioButtonVertical.Name = "radioButtonVertical";
            this.radioButtonVertical.Size = new System.Drawing.Size(80, 24);
            this.radioButtonVertical.TabIndex = 1;
            this.radioButtonVertical.Text = "Vertical";
            this.radioButtonVertical.Click += new System.EventHandler(this.radioButtonVertical_Click);
            // 
            // radioButtonHorz
            // 
            this.radioButtonHorz.Checked = true;
            this.radioButtonHorz.Location = new System.Drawing.Point(16, 24);
            this.radioButtonHorz.Name = "radioButtonHorz";
            this.radioButtonHorz.Size = new System.Drawing.Size(80, 24);
            this.radioButtonHorz.TabIndex = 0;
            this.radioButtonHorz.TabStop = true;
            this.radioButtonHorz.Text = "Horizontal";
            this.radioButtonHorz.Click += new System.EventHandler(this.radioButtonHorz_Click);
            // 
            // checkBoxPushed
            // 
            this.checkBoxPushed.Location = new System.Drawing.Point(32, 75);
            this.checkBoxPushed.Name = "checkBoxPushed";
            this.checkBoxPushed.Size = new System.Drawing.Size(80, 24);
            this.checkBoxPushed.TabIndex = 2;
            this.checkBoxPushed.Text = "Pushed";
            this.checkBoxPushed.CheckedChanged += new System.EventHandler(this.checkBoxPushed_CheckedChanged);
            // 
            // groupBoxButtonStyle
            // 
            this.groupBoxButtonStyle.Controls.Add(this.radioButtonToggleButton);
            this.groupBoxButtonStyle.Controls.Add(this.radioButtonPushButton);
            this.groupBoxButtonStyle.Controls.Add(this.checkBoxPushed);
            this.groupBoxButtonStyle.Location = new System.Drawing.Point(182, 12);
            this.groupBoxButtonStyle.Name = "groupBoxButtonStyle";
            this.groupBoxButtonStyle.Size = new System.Drawing.Size(128, 131);
            this.groupBoxButtonStyle.TabIndex = 1;
            this.groupBoxButtonStyle.TabStop = false;
            this.groupBoxButtonStyle.Text = "ButtonStyle";
            // 
            // radioButtonToggleButton
            // 
            this.radioButtonToggleButton.Location = new System.Drawing.Point(16, 49);
            this.radioButtonToggleButton.Name = "radioButtonToggleButton";
            this.radioButtonToggleButton.Size = new System.Drawing.Size(96, 24);
            this.radioButtonToggleButton.TabIndex = 1;
            this.radioButtonToggleButton.Text = "ToggleButton";
            this.radioButtonToggleButton.CheckedChanged += new System.EventHandler(this.radioButtonToggleButton_CheckedChanged);
            // 
            // radioButtonPushButton
            // 
            this.radioButtonPushButton.Checked = true;
            this.radioButtonPushButton.Location = new System.Drawing.Point(16, 24);
            this.radioButtonPushButton.Name = "radioButtonPushButton";
            this.radioButtonPushButton.Size = new System.Drawing.Size(88, 24);
            this.radioButtonPushButton.TabIndex = 0;
            this.radioButtonPushButton.TabStop = true;
            this.radioButtonPushButton.Text = "PushButton";
            this.radioButtonPushButton.CheckedChanged += new System.EventHandler(this.radioButtonPushButton_CheckedChanged);
            // 
            // groupBoxTextEdge
            // 
            this.groupBoxTextEdge.Controls.Add(this.radioButtonRight);
            this.groupBoxTextEdge.Controls.Add(this.radioButtonLeft);
            this.groupBoxTextEdge.Controls.Add(this.radioButtonBottom);
            this.groupBoxTextEdge.Controls.Add(this.radioButtonTop);
            this.groupBoxTextEdge.Location = new System.Drawing.Point(316, 10);
            this.groupBoxTextEdge.Name = "groupBoxTextEdge";
            this.groupBoxTextEdge.Size = new System.Drawing.Size(128, 133);
            this.groupBoxTextEdge.TabIndex = 3;
            this.groupBoxTextEdge.TabStop = false;
            this.groupBoxTextEdge.Text = "TextEdge";
            // 
            // radioButtonRight
            // 
            this.radioButtonRight.Checked = true;
            this.radioButtonRight.Location = new System.Drawing.Point(16, 92);
            this.radioButtonRight.Name = "radioButtonRight";
            this.radioButtonRight.Size = new System.Drawing.Size(80, 24);
            this.radioButtonRight.TabIndex = 3;
            this.radioButtonRight.TabStop = true;
            this.radioButtonRight.Text = "Right";
            this.radioButtonRight.CheckedChanged += new System.EventHandler(this.radioButtonRight_CheckedChanged);
            // 
            // radioButtonLeft
            // 
            this.radioButtonLeft.Location = new System.Drawing.Point(16, 70);
            this.radioButtonLeft.Name = "radioButtonLeft";
            this.radioButtonLeft.Size = new System.Drawing.Size(80, 24);
            this.radioButtonLeft.TabIndex = 2;
            this.radioButtonLeft.Text = "Left";
            this.radioButtonLeft.CheckedChanged += new System.EventHandler(this.radioButtonLeft_CheckedChanged);
            // 
            // radioButtonBottom
            // 
            this.radioButtonBottom.Location = new System.Drawing.Point(16, 48);
            this.radioButtonBottom.Name = "radioButtonBottom";
            this.radioButtonBottom.Size = new System.Drawing.Size(80, 24);
            this.radioButtonBottom.TabIndex = 1;
            this.radioButtonBottom.Text = "Bottom";
            this.radioButtonBottom.CheckedChanged += new System.EventHandler(this.radioButtonBottom_CheckedChanged);
            // 
            // radioButtonTop
            // 
            this.radioButtonTop.Location = new System.Drawing.Point(16, 26);
            this.radioButtonTop.Name = "radioButtonTop";
            this.radioButtonTop.Size = new System.Drawing.Size(80, 24);
            this.radioButtonTop.TabIndex = 0;
            this.radioButtonTop.Text = "Top";
            this.radioButtonTop.CheckedChanged += new System.EventHandler(this.radioButtonTop_CheckedChanged);
            // 
            // checkBoxEnabled
            // 
            this.checkBoxEnabled.Checked = true;
            this.checkBoxEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnabled.Location = new System.Drawing.Point(15, 48);
            this.checkBoxEnabled.Name = "checkBoxEnabled";
            this.checkBoxEnabled.Size = new System.Drawing.Size(72, 24);
            this.checkBoxEnabled.TabIndex = 1;
            this.checkBoxEnabled.Text = "Enabled";
            this.checkBoxEnabled.CheckedChanged += new System.EventHandler(this.checkBoxEnabled_CheckedChanged);
            // 
            // groupBoxExample
            // 
            this.groupBoxExample.Controls.Add(this.buttonEight);
            this.groupBoxExample.Controls.Add(this.buttonSix);
            this.groupBoxExample.Controls.Add(this.buttonSeven);
            this.groupBoxExample.Controls.Add(this.buttonFive);
            this.groupBoxExample.Controls.Add(this.buttonFour);
            this.groupBoxExample.Controls.Add(this.buttonThree);
            this.groupBoxExample.Controls.Add(this.buttonTwo);
            this.groupBoxExample.Controls.Add(this.buttonOne);
            this.groupBoxExample.Controls.Add(this.buttonText);
            this.groupBoxExample.Controls.Add(this.buttonImage);
            this.groupBoxExample.Controls.Add(this.buttonTextAndImage);
            this.groupBoxExample.Location = new System.Drawing.Point(451, 12);
            this.groupBoxExample.Name = "groupBoxExample";
            this.groupBoxExample.Size = new System.Drawing.Size(418, 246);
            this.groupBoxExample.TabIndex = 5;
            this.groupBoxExample.TabStop = false;
            this.groupBoxExample.Text = "Button Instances";
            // 
            // checkBoxBorder
            // 
            this.checkBoxBorder.Location = new System.Drawing.Point(15, 23);
            this.checkBoxBorder.Name = "checkBoxBorder";
            this.checkBoxBorder.Size = new System.Drawing.Size(96, 24);
            this.checkBoxBorder.TabIndex = 0;
            this.checkBoxBorder.Text = "Show Border";
            this.checkBoxBorder.CheckedChanged += new System.EventHandler(this.checkBoxBorder_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxBorder);
            this.groupBox1.Controls.Add(this.checkBoxEnabled);
            this.groupBox1.Location = new System.Drawing.Point(316, 151);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(127, 107);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Other";
            // 
            // buttonEight
            // 
            this.buttonEight.Image = ((System.Drawing.Image)(resources.GetObject("buttonEight.Image")));
            this.buttonEight.Location = new System.Drawing.Point(353, 171);
            this.buttonEight.Name = "buttonEight";
            this.buttonEight.Size = new System.Drawing.Size(40, 40);
            this.buttonEight.TabIndex = 10;
            this.buttonEight.Text = "8";
            // 
            // buttonSix
            // 
            this.buttonSix.Image = ((System.Drawing.Image)(resources.GetObject("buttonSix.Image")));
            this.buttonSix.Location = new System.Drawing.Point(260, 171);
            this.buttonSix.Name = "buttonSix";
            this.buttonSix.Size = new System.Drawing.Size(40, 40);
            this.buttonSix.TabIndex = 8;
            this.buttonSix.Text = "6";
            // 
            // buttonSeven
            // 
            this.buttonSeven.Image = ((System.Drawing.Image)(resources.GetObject("buttonSeven.Image")));
            this.buttonSeven.Location = new System.Drawing.Point(306, 171);
            this.buttonSeven.Name = "buttonSeven";
            this.buttonSeven.Size = new System.Drawing.Size(40, 40);
            this.buttonSeven.TabIndex = 9;
            this.buttonSeven.Text = "7";
            // 
            // buttonFive
            // 
            this.buttonFive.Image = ((System.Drawing.Image)(resources.GetObject("buttonFive.Image")));
            this.buttonFive.Location = new System.Drawing.Point(213, 171);
            this.buttonFive.Name = "buttonFive";
            this.buttonFive.Size = new System.Drawing.Size(40, 40);
            this.buttonFive.TabIndex = 7;
            this.buttonFive.Text = "5";
            // 
            // buttonFour
            // 
            this.buttonFour.Image = ((System.Drawing.Image)(resources.GetObject("buttonFour.Image")));
            this.buttonFour.Location = new System.Drawing.Point(167, 171);
            this.buttonFour.Name = "buttonFour";
            this.buttonFour.Size = new System.Drawing.Size(40, 40);
            this.buttonFour.TabIndex = 6;
            this.buttonFour.Text = "4";
            // 
            // buttonThree
            // 
            this.buttonThree.Image = ((System.Drawing.Image)(resources.GetObject("buttonThree.Image")));
            this.buttonThree.Location = new System.Drawing.Point(74, 171);
            this.buttonThree.Name = "buttonThree";
            this.buttonThree.Size = new System.Drawing.Size(40, 40);
            this.buttonThree.TabIndex = 4;
            this.buttonThree.Text = "2";
            // 
            // buttonTwo
            // 
            this.buttonTwo.Image = ((System.Drawing.Image)(resources.GetObject("buttonTwo.Image")));
            this.buttonTwo.Location = new System.Drawing.Point(120, 171);
            this.buttonTwo.Name = "buttonTwo";
            this.buttonTwo.Size = new System.Drawing.Size(40, 40);
            this.buttonTwo.TabIndex = 5;
            this.buttonTwo.Text = "3";
            // 
            // buttonOne
            // 
            this.buttonOne.Image = ((System.Drawing.Image)(resources.GetObject("buttonOne.Image")));
            this.buttonOne.Location = new System.Drawing.Point(27, 171);
            this.buttonOne.Name = "buttonOne";
            this.buttonOne.Size = new System.Drawing.Size(40, 40);
            this.buttonOne.TabIndex = 3;
            this.buttonOne.Text = "1";
            // 
            // buttonText
            // 
            this.buttonText.Location = new System.Drawing.Point(21, 35);
            this.buttonText.Name = "buttonText";
            this.buttonText.Size = new System.Drawing.Size(120, 107);
            this.buttonText.TabIndex = 0;
            this.buttonText.Text = "JustText";
            // 
            // buttonImage
            // 
            this.buttonImage.Image = ((System.Drawing.Image)(resources.GetObject("buttonImage.Image")));
            this.buttonImage.Location = new System.Drawing.Point(151, 35);
            this.buttonImage.Name = "buttonImage";
            this.buttonImage.Size = new System.Drawing.Size(120, 107);
            this.buttonImage.TabIndex = 1;
            // 
            // buttonTextAndImage
            // 
            this.buttonTextAndImage.Image = ((System.Drawing.Image)(resources.GetObject("buttonTextAndImage.Image")));
            this.buttonTextAndImage.Location = new System.Drawing.Point(278, 35);
            this.buttonTextAndImage.Name = "buttonTextAndImage";
            this.buttonTextAndImage.Size = new System.Drawing.Size(120, 107);
            this.buttonTextAndImage.TabIndex = 2;
            this.buttonTextAndImage.Text = "Both";
            // 
            // SampleButtonWithStyle
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(878, 264);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxExample);
            this.Controls.Add(this.groupBoxTextEdge);
            this.Controls.Add(this.groupBoxButtonStyle);
            this.Controls.Add(this.groupBoxDirection);
            this.Controls.Add(this.groupBoxStyle);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SampleButtonWithStyle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DotNetMagic - SampleButtonWithStyle";
            this.groupBoxStyle.ResumeLayout(false);
            this.groupBoxDirection.ResumeLayout(false);
            this.groupBoxButtonStyle.ResumeLayout(false);
            this.groupBoxTextEdge.ResumeLayout(false);
            this.groupBoxExample.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
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
			Application.Run(new SampleButtonWithStyle());
		}

        private void radioButtonOffice2007Blue_Click(object sender, EventArgs e)
        {
            buttonText.Style = VisualStyle.Office2007Blue;
            buttonImage.Style = VisualStyle.Office2007Blue;
            buttonTextAndImage.Style = VisualStyle.Office2007Blue;
            buttonOne.Style = VisualStyle.Office2007Blue;
            buttonTwo.Style = VisualStyle.Office2007Blue;
            buttonThree.Style = VisualStyle.Office2007Blue;
            buttonFour.Style = VisualStyle.Office2007Blue;
            buttonFive.Style = VisualStyle.Office2007Blue;
            buttonSix.Style = VisualStyle.Office2007Blue;
            buttonSeven.Style = VisualStyle.Office2007Blue;
            buttonEight.Style = VisualStyle.Office2007Blue;
        }

        private void radioButtonOffice2007Silver_Click(object sender, EventArgs e)
        {
            buttonText.Style = VisualStyle.Office2007Silver;
            buttonImage.Style = VisualStyle.Office2007Silver;
            buttonTextAndImage.Style = VisualStyle.Office2007Silver;
            buttonOne.Style = VisualStyle.Office2007Silver;
            buttonTwo.Style = VisualStyle.Office2007Silver;
            buttonThree.Style = VisualStyle.Office2007Silver;
            buttonFour.Style = VisualStyle.Office2007Silver;
            buttonFive.Style = VisualStyle.Office2007Silver;
            buttonSix.Style = VisualStyle.Office2007Silver;
            buttonSeven.Style = VisualStyle.Office2007Silver;
            buttonEight.Style = VisualStyle.Office2007Silver;
        }

        private void radioButtonOffice2007Black_Click(object sender, EventArgs e)
        {
            buttonText.Style = VisualStyle.Office2007Black;
            buttonImage.Style = VisualStyle.Office2007Black;
            buttonTextAndImage.Style = VisualStyle.Office2007Black;
            buttonOne.Style = VisualStyle.Office2007Black;
            buttonTwo.Style = VisualStyle.Office2007Black;
            buttonThree.Style = VisualStyle.Office2007Black;
            buttonFour.Style = VisualStyle.Office2007Black;
            buttonFive.Style = VisualStyle.Office2007Black;
            buttonSix.Style = VisualStyle.Office2007Black;
            buttonSeven.Style = VisualStyle.Office2007Black;
            buttonEight.Style = VisualStyle.Office2007Black;
        }

        private void radioButtonMediaPlayerBlue_CheckedChanged(object sender, EventArgs e)
        {
            buttonText.Style = VisualStyle.MediaPlayerBlue;
            buttonImage.Style = VisualStyle.MediaPlayerBlue;
            buttonTextAndImage.Style = VisualStyle.MediaPlayerBlue;
            buttonOne.Style = VisualStyle.MediaPlayerBlue;
            buttonTwo.Style = VisualStyle.MediaPlayerBlue;
            buttonThree.Style = VisualStyle.MediaPlayerBlue;
            buttonFour.Style = VisualStyle.MediaPlayerBlue;
            buttonFive.Style = VisualStyle.MediaPlayerBlue;
            buttonSix.Style = VisualStyle.MediaPlayerBlue;
            buttonSeven.Style = VisualStyle.MediaPlayerBlue;
            buttonEight.Style = VisualStyle.MediaPlayerBlue;
        }

        private void radioButtonMediaPlayerOrange_CheckedChanged(object sender, EventArgs e)
        {
            buttonText.Style = VisualStyle.MediaPlayerOrange;
            buttonImage.Style = VisualStyle.MediaPlayerOrange;
            buttonTextAndImage.Style = VisualStyle.MediaPlayerOrange;
            buttonOne.Style = VisualStyle.MediaPlayerOrange;
            buttonTwo.Style = VisualStyle.MediaPlayerOrange;
            buttonThree.Style = VisualStyle.MediaPlayerOrange;
            buttonFour.Style = VisualStyle.MediaPlayerOrange;
            buttonFive.Style = VisualStyle.MediaPlayerOrange;
            buttonSix.Style = VisualStyle.MediaPlayerOrange;
            buttonSeven.Style = VisualStyle.MediaPlayerOrange;
            buttonEight.Style = VisualStyle.MediaPlayerOrange;
        }

        private void radioMediaPlayerPurple_CheckedChanged(object sender, EventArgs e)
        {
            buttonText.Style = VisualStyle.MediaPlayerPurple;
            buttonImage.Style = VisualStyle.MediaPlayerPurple;
            buttonTextAndImage.Style = VisualStyle.MediaPlayerPurple;
            buttonOne.Style = VisualStyle.MediaPlayerPurple;
            buttonTwo.Style = VisualStyle.MediaPlayerPurple;
            buttonThree.Style = VisualStyle.MediaPlayerPurple;
            buttonFour.Style = VisualStyle.MediaPlayerPurple;
            buttonFive.Style = VisualStyle.MediaPlayerPurple;
            buttonSix.Style = VisualStyle.MediaPlayerPurple;
            buttonSeven.Style = VisualStyle.MediaPlayerPurple;
            buttonEight.Style = VisualStyle.MediaPlayerPurple;
        }

		private void radioButtonOffice2003_Click(object sender, System.EventArgs e)
		{
			buttonText.Style = VisualStyle.Office2003;
			buttonImage.Style = VisualStyle.Office2003;
			buttonTextAndImage.Style = VisualStyle.Office2003;
			buttonOne.Style = VisualStyle.Office2003;
			buttonTwo.Style = VisualStyle.Office2003;
			buttonThree.Style = VisualStyle.Office2003;
			buttonFour.Style = VisualStyle.Office2003;
			buttonFive.Style = VisualStyle.Office2003;
			buttonSix.Style = VisualStyle.Office2003;
			buttonSeven.Style = VisualStyle.Office2003;
			buttonEight.Style = VisualStyle.Office2003;
		}	

		private void radioButtonIDE2005_Click(object sender, System.EventArgs e)
		{
			buttonText.Style = VisualStyle.IDE2005;
			buttonImage.Style = VisualStyle.IDE2005;
			buttonTextAndImage.Style = VisualStyle.IDE2005;
			buttonOne.Style = VisualStyle.IDE2005;
			buttonTwo.Style = VisualStyle.IDE2005;
			buttonThree.Style = VisualStyle.IDE2005;
			buttonFour.Style = VisualStyle.IDE2005;
			buttonFive.Style = VisualStyle.IDE2005;
			buttonSix.Style = VisualStyle.IDE2005;
			buttonSeven.Style = VisualStyle.IDE2005;
			buttonEight.Style = VisualStyle.IDE2005;
		}

		private void radioButtonPlain_Click(object sender, System.EventArgs e)
		{
			buttonText.Style = VisualStyle.Plain;
			buttonImage.Style = VisualStyle.Plain;
			buttonTextAndImage.Style = VisualStyle.Plain;
			buttonOne.Style = VisualStyle.Plain;
			buttonTwo.Style = VisualStyle.Plain;
			buttonThree.Style = VisualStyle.Plain;
			buttonFour.Style = VisualStyle.Plain;
			buttonFive.Style = VisualStyle.Plain;
			buttonSix.Style = VisualStyle.Plain;
			buttonSeven.Style = VisualStyle.Plain;
			buttonEight.Style = VisualStyle.Plain;
		}

		private void radioButtonHorz_Click(object sender, System.EventArgs e)
		{
			buttonText.Direction = LayoutDirection.Horizontal;
			buttonImage.Direction = LayoutDirection.Horizontal;
			buttonTextAndImage.Direction = LayoutDirection.Horizontal;
			buttonOne.Direction = LayoutDirection.Horizontal;
			buttonTwo.Direction = LayoutDirection.Horizontal;
			buttonThree.Direction = LayoutDirection.Horizontal;
			buttonFour.Direction = LayoutDirection.Horizontal;
			buttonFive.Direction = LayoutDirection.Horizontal;
			buttonSix.Direction = LayoutDirection.Horizontal;
			buttonSeven.Direction = LayoutDirection.Horizontal;
			buttonEight.Direction = LayoutDirection.Horizontal;
		}

		private void radioButtonVertical_Click(object sender, System.EventArgs e)
		{
			buttonText.Direction = LayoutDirection.Vertical;
			buttonImage.Direction = LayoutDirection.Vertical;
			buttonTextAndImage.Direction = LayoutDirection.Vertical;
			buttonOne.Direction = LayoutDirection.Vertical;
			buttonTwo.Direction = LayoutDirection.Vertical;
			buttonThree.Direction = LayoutDirection.Vertical;
			buttonFour.Direction = LayoutDirection.Vertical;
			buttonFive.Direction = LayoutDirection.Vertical;
			buttonSix.Direction = LayoutDirection.Vertical;
			buttonSeven.Direction = LayoutDirection.Vertical;
			buttonEight.Direction = LayoutDirection.Vertical;
		}

		private void radioButtonTop_CheckedChanged(object sender, System.EventArgs e)
		{
			buttonText.TextEdge = TextEdge.Top;
			buttonImage.TextEdge = TextEdge.Top;
			buttonTextAndImage.TextEdge = TextEdge.Top;
			buttonOne.TextEdge = TextEdge.Top;
			buttonTwo.TextEdge = TextEdge.Top;
			buttonThree.TextEdge = TextEdge.Top;
			buttonFour.TextEdge = TextEdge.Top;
			buttonFive.TextEdge = TextEdge.Top;
			buttonSix.TextEdge = TextEdge.Top;
			buttonSeven.TextEdge = TextEdge.Top;
			buttonEight.TextEdge = TextEdge.Top;
		}

		private void radioButtonBottom_CheckedChanged(object sender, System.EventArgs e)
		{
			buttonText.TextEdge = TextEdge.Bottom;
			buttonImage.TextEdge = TextEdge.Bottom;
			buttonTextAndImage.TextEdge = TextEdge.Bottom;
			buttonOne.TextEdge = TextEdge.Bottom;
			buttonTwo.TextEdge = TextEdge.Bottom;
			buttonThree.TextEdge = TextEdge.Bottom;
			buttonFour.TextEdge = TextEdge.Bottom;
			buttonFive.TextEdge = TextEdge.Bottom;
			buttonSix.TextEdge = TextEdge.Bottom;
			buttonSeven.TextEdge = TextEdge.Bottom;
			buttonEight.TextEdge = TextEdge.Bottom;
		}

		private void radioButtonLeft_CheckedChanged(object sender, System.EventArgs e)
		{
			buttonText.TextEdge = TextEdge.Left;
			buttonImage.TextEdge = TextEdge.Left;
			buttonTextAndImage.TextEdge = TextEdge.Left;
			buttonOne.TextEdge = TextEdge.Left;
			buttonTwo.TextEdge = TextEdge.Left;
			buttonThree.TextEdge = TextEdge.Left;
			buttonFour.TextEdge = TextEdge.Left;
			buttonFive.TextEdge = TextEdge.Left;
			buttonSix.TextEdge = TextEdge.Left;
			buttonSeven.TextEdge = TextEdge.Left;
			buttonEight.TextEdge = TextEdge.Left;
		}

		private void radioButtonRight_CheckedChanged(object sender, System.EventArgs e)
		{
			buttonText.TextEdge = TextEdge.Right;
			buttonImage.TextEdge = TextEdge.Right;
			buttonTextAndImage.TextEdge = TextEdge.Right;
			buttonOne.TextEdge = TextEdge.Right;
			buttonTwo.TextEdge = TextEdge.Right;
			buttonThree.TextEdge = TextEdge.Right;
			buttonFour.TextEdge = TextEdge.Right;
			buttonFive.TextEdge = TextEdge.Right;
			buttonSix.TextEdge = TextEdge.Right;
			buttonSeven.TextEdge = TextEdge.Right;
			buttonEight.TextEdge = TextEdge.Right;
		}

		private void checkBoxEnabled_CheckedChanged(object sender, System.EventArgs e)
		{
			buttonText.Enabled = checkBoxEnabled.Checked;
			buttonImage.Enabled = checkBoxEnabled.Checked;
			buttonTextAndImage.Enabled = checkBoxEnabled.Checked;
			buttonOne.Enabled = checkBoxEnabled.Checked;
			buttonTwo.Enabled = checkBoxEnabled.Checked;
			buttonThree.Enabled = checkBoxEnabled.Checked;
			buttonFour.Enabled = checkBoxEnabled.Checked;
			buttonFive.Enabled = checkBoxEnabled.Checked;
			buttonSix.Enabled = checkBoxEnabled.Checked;
			buttonSeven.Enabled = checkBoxEnabled.Checked;
			buttonEight.Enabled = checkBoxEnabled.Checked;
		}

		private void radioButtonPushButton_CheckedChanged(object sender, System.EventArgs e)
		{
			buttonText.ButtonStyle = ButtonStyle.PushButton;
			buttonImage.ButtonStyle = ButtonStyle.PushButton;
			buttonTextAndImage.ButtonStyle = ButtonStyle.PushButton;
			buttonOne.ButtonStyle = ButtonStyle.PushButton;
			buttonTwo.ButtonStyle = ButtonStyle.PushButton;
			buttonThree.ButtonStyle = ButtonStyle.PushButton;
			buttonFour.ButtonStyle = ButtonStyle.PushButton;
			buttonFive.ButtonStyle = ButtonStyle.PushButton;
			buttonSix.ButtonStyle = ButtonStyle.PushButton;
			buttonSeven.ButtonStyle = ButtonStyle.PushButton;
			buttonEight.ButtonStyle = ButtonStyle.PushButton;
		}

		private void radioButtonToggleButton_CheckedChanged(object sender, System.EventArgs e)
		{
			buttonText.ButtonStyle = ButtonStyle.ToggleButton;
			buttonImage.ButtonStyle = ButtonStyle.ToggleButton;
			buttonTextAndImage.ButtonStyle = ButtonStyle.ToggleButton;
			buttonOne.ButtonStyle = ButtonStyle.ToggleButton;
			buttonTwo.ButtonStyle = ButtonStyle.ToggleButton;
			buttonThree.ButtonStyle = ButtonStyle.ToggleButton;
			buttonFour.ButtonStyle = ButtonStyle.ToggleButton;
			buttonFive.ButtonStyle = ButtonStyle.ToggleButton;
			buttonSix.ButtonStyle = ButtonStyle.ToggleButton;
			buttonSeven.ButtonStyle = ButtonStyle.ToggleButton;
			buttonEight.ButtonStyle = ButtonStyle.ToggleButton;
		}

		private void checkBoxPushed_CheckedChanged(object sender, System.EventArgs e)
		{
			buttonText.Pushed = checkBoxPushed.Checked;
			buttonImage.Pushed = checkBoxPushed.Checked;
			buttonTextAndImage.Pushed = checkBoxPushed.Checked;
			buttonOne.Pushed = checkBoxPushed.Checked;
			buttonTwo.Pushed = checkBoxPushed.Checked;
			buttonThree.Pushed = checkBoxPushed.Checked;
			buttonFour.Pushed = checkBoxPushed.Checked;
			buttonFive.Pushed = checkBoxPushed.Checked;
			buttonSix.Pushed = checkBoxPushed.Checked;
			buttonSeven.Pushed = checkBoxPushed.Checked;
			buttonEight.Pushed = checkBoxPushed.Checked;
		}

		private void checkBoxBorder_CheckedChanged(object sender, System.EventArgs e)
		{
			buttonText.AlwaysDrawBorder = checkBoxBorder.Checked;
			buttonImage.AlwaysDrawBorder = checkBoxBorder.Checked;
			buttonTextAndImage.AlwaysDrawBorder = checkBoxBorder.Checked;
			buttonOne.AlwaysDrawBorder = checkBoxBorder.Checked;
			buttonTwo.AlwaysDrawBorder = checkBoxBorder.Checked;
			buttonThree.AlwaysDrawBorder = checkBoxBorder.Checked;
			buttonFour.AlwaysDrawBorder = checkBoxBorder.Checked;
			buttonFive.AlwaysDrawBorder = checkBoxBorder.Checked;
			buttonSix.AlwaysDrawBorder = checkBoxBorder.Checked;
			buttonSeven.AlwaysDrawBorder = checkBoxBorder.Checked;
			buttonEight.AlwaysDrawBorder = checkBoxBorder.Checked;
		}
    }
}
