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
using Crownwood.DotNetMagic.Forms;
using Crownwood.DotNetMagic.Common;
using Crownwood.DotNetMagic.Controls;

namespace SampleStatusBarControl
{
	/// <summary>
	/// Summary description for SampleStatusBarControl.
	/// </summary>
	public class SampleStatusBarControl : DotNetMagicForm
	{
		// Instance fields
		private ImageList imageList;
	
		// Designer generated
		private Crownwood.DotNetMagic.Controls.StatusBarControl statusBarControl1;
		private System.Windows.Forms.NumericUpDown numericWidth;
		private System.Windows.Forms.Label labelWidth;
		private System.Windows.Forms.TextBox textBox;
		private System.Windows.Forms.Label labelText;
		private System.Windows.Forms.Label labelIcon;
		private System.Windows.Forms.ComboBox comboBoxIcon;
		private System.Windows.Forms.Label labelSize;
		private System.Windows.Forms.ComboBox comboBoxSize;
		private System.Windows.Forms.Button buttonAdd;
		private System.Windows.Forms.GroupBox groupBoxAdd;
		private System.Windows.Forms.Button buttonRemoveAll;
		private System.Windows.Forms.Button buttonRemoveFirst;
		private System.Windows.Forms.Button buttonAddTime;
		private System.Windows.Forms.Label labelBorder;
		private System.Windows.Forms.ComboBox comboBoxBorder;
		private System.Windows.Forms.Label labelAlignment;
		private System.Windows.Forms.ComboBox comboBoxAlignment;
		private System.Windows.Forms.Timer timerForPanels;
		private System.Windows.Forms.Button buttonAP;
		private System.Windows.Forms.GroupBox groupBoxStyles;
        private System.Windows.Forms.RadioButton radioOffice2003;
		private System.Windows.Forms.RadioButton radioPlain;
		private System.Windows.Forms.RadioButton radioIDE2005;
        private RadioButton radioOffice2007Blue;
        private RadioButton radioOffice2007Silver;
        private RadioButton radioOffice2007Black;
        private RadioButton radioMediaPlayerBlue;
        private RadioButton radioMediaPlayerOrange;
        private RadioButton radioMediaPlayerPurple;
		private System.ComponentModel.IContainer components;

		public SampleStatusBarControl()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			// Load the strip of simple images
			imageList = ResourceHelper.LoadBitmapStrip(typeof(SampleStatusBarControl), 
													   "SampleStatusBarControl.SampleImages.bmp", 
													   new Size(16, 16), 
													   Point.Empty);
			
			comboBoxIcon.SelectedIndex = 0;
			comboBoxSize.SelectedIndex = 0;
			comboBoxAlignment.SelectedIndex = 0;
			comboBoxBorder.SelectedIndex = 5;
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SampleStatusBarControl));
            this.statusBarControl1 = new Crownwood.DotNetMagic.Controls.StatusBarControl();
            this.numericWidth = new System.Windows.Forms.NumericUpDown();
            this.labelWidth = new System.Windows.Forms.Label();
            this.textBox = new System.Windows.Forms.TextBox();
            this.labelText = new System.Windows.Forms.Label();
            this.labelIcon = new System.Windows.Forms.Label();
            this.comboBoxIcon = new System.Windows.Forms.ComboBox();
            this.labelSize = new System.Windows.Forms.Label();
            this.comboBoxSize = new System.Windows.Forms.ComboBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.groupBoxAdd = new System.Windows.Forms.GroupBox();
            this.labelBorder = new System.Windows.Forms.Label();
            this.comboBoxBorder = new System.Windows.Forms.ComboBox();
            this.labelAlignment = new System.Windows.Forms.Label();
            this.comboBoxAlignment = new System.Windows.Forms.ComboBox();
            this.buttonRemoveAll = new System.Windows.Forms.Button();
            this.buttonRemoveFirst = new System.Windows.Forms.Button();
            this.buttonAddTime = new System.Windows.Forms.Button();
            this.timerForPanels = new System.Windows.Forms.Timer(this.components);
            this.buttonAP = new System.Windows.Forms.Button();
            this.groupBoxStyles = new System.Windows.Forms.GroupBox();
            this.radioOffice2007Blue = new System.Windows.Forms.RadioButton();
            this.radioOffice2007Silver = new System.Windows.Forms.RadioButton();
            this.radioOffice2007Black = new System.Windows.Forms.RadioButton();
            this.radioIDE2005 = new System.Windows.Forms.RadioButton();
            this.radioPlain = new System.Windows.Forms.RadioButton();
            this.radioOffice2003 = new System.Windows.Forms.RadioButton();
            this.radioMediaPlayerBlue = new System.Windows.Forms.RadioButton();
            this.radioMediaPlayerOrange = new System.Windows.Forms.RadioButton();
            this.radioMediaPlayerPurple = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.numericWidth)).BeginInit();
            this.groupBoxAdd.SuspendLayout();
            this.groupBoxStyles.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusBarControl1
            // 
            this.statusBarControl1.Location = new System.Drawing.Point(0, 276);
            this.statusBarControl1.Name = "statusBarControl1";
            this.statusBarControl1.Size = new System.Drawing.Size(555, 22);
            this.statusBarControl1.TabIndex = 7;
            // 
            // numericWidth
            // 
            this.numericWidth.Location = new System.Drawing.Point(16, 56);
            this.numericWidth.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numericWidth.Name = "numericWidth";
            this.numericWidth.Size = new System.Drawing.Size(48, 20);
            this.numericWidth.TabIndex = 1;
            this.numericWidth.Value = new decimal(new int[] {
            85,
            0,
            0,
            0});
            // 
            // labelWidth
            // 
            this.labelWidth.Location = new System.Drawing.Point(16, 32);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(40, 16);
            this.labelWidth.TabIndex = 0;
            this.labelWidth.Text = "Width";
            this.labelWidth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(80, 56);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(100, 20);
            this.textBox.TabIndex = 3;
            this.textBox.Text = "Example";
            // 
            // labelText
            // 
            this.labelText.Location = new System.Drawing.Point(80, 32);
            this.labelText.Name = "labelText";
            this.labelText.Size = new System.Drawing.Size(40, 16);
            this.labelText.TabIndex = 2;
            this.labelText.Text = "Text";
            this.labelText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelIcon
            // 
            this.labelIcon.Location = new System.Drawing.Point(16, 88);
            this.labelIcon.Name = "labelIcon";
            this.labelIcon.Size = new System.Drawing.Size(40, 16);
            this.labelIcon.TabIndex = 4;
            this.labelIcon.Text = "Icon";
            this.labelIcon.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxIcon
            // 
            this.comboBoxIcon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxIcon.Items.AddRange(new object[] {
            "None",
            "Example1",
            "Example2",
            "Example3"});
            this.comboBoxIcon.Location = new System.Drawing.Point(16, 112);
            this.comboBoxIcon.Name = "comboBoxIcon";
            this.comboBoxIcon.Size = new System.Drawing.Size(104, 21);
            this.comboBoxIcon.TabIndex = 5;
            // 
            // labelSize
            // 
            this.labelSize.Location = new System.Drawing.Point(136, 88);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(56, 16);
            this.labelSize.TabIndex = 6;
            this.labelSize.Text = "AutoSize";
            this.labelSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxSize
            // 
            this.comboBoxSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSize.Items.AddRange(new object[] {
            "None",
            "Contents",
            "Spring"});
            this.comboBoxSize.Location = new System.Drawing.Point(136, 112);
            this.comboBoxSize.Name = "comboBoxSize";
            this.comboBoxSize.Size = new System.Drawing.Size(104, 21);
            this.comboBoxSize.TabIndex = 7;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(448, 17);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(96, 23);
            this.buttonAdd.TabIndex = 2;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // groupBoxAdd
            // 
            this.groupBoxAdd.Controls.Add(this.labelBorder);
            this.groupBoxAdd.Controls.Add(this.comboBoxBorder);
            this.groupBoxAdd.Controls.Add(this.labelAlignment);
            this.groupBoxAdd.Controls.Add(this.comboBoxAlignment);
            this.groupBoxAdd.Controls.Add(this.textBox);
            this.groupBoxAdd.Controls.Add(this.comboBoxSize);
            this.groupBoxAdd.Controls.Add(this.labelWidth);
            this.groupBoxAdd.Controls.Add(this.comboBoxIcon);
            this.groupBoxAdd.Controls.Add(this.labelIcon);
            this.groupBoxAdd.Controls.Add(this.numericWidth);
            this.groupBoxAdd.Controls.Add(this.labelSize);
            this.groupBoxAdd.Controls.Add(this.labelText);
            this.groupBoxAdd.Location = new System.Drawing.Point(175, 12);
            this.groupBoxAdd.Name = "groupBoxAdd";
            this.groupBoxAdd.Size = new System.Drawing.Size(256, 253);
            this.groupBoxAdd.TabIndex = 1;
            this.groupBoxAdd.TabStop = false;
            this.groupBoxAdd.Text = "Add Values";
            // 
            // labelBorder
            // 
            this.labelBorder.Location = new System.Drawing.Point(136, 144);
            this.labelBorder.Name = "labelBorder";
            this.labelBorder.Size = new System.Drawing.Size(104, 16);
            this.labelBorder.TabIndex = 10;
            this.labelBorder.Text = "Border";
            this.labelBorder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxBorder
            // 
            this.comboBoxBorder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBorder.Items.AddRange(new object[] {
            "None",
            "Sunken",
            "Raised",
            "Dotted",
            "Dashed",
            "Solid"});
            this.comboBoxBorder.Location = new System.Drawing.Point(136, 168);
            this.comboBoxBorder.Name = "comboBoxBorder";
            this.comboBoxBorder.Size = new System.Drawing.Size(104, 21);
            this.comboBoxBorder.TabIndex = 11;
            // 
            // labelAlignment
            // 
            this.labelAlignment.Location = new System.Drawing.Point(16, 144);
            this.labelAlignment.Name = "labelAlignment";
            this.labelAlignment.Size = new System.Drawing.Size(104, 16);
            this.labelAlignment.TabIndex = 8;
            this.labelAlignment.Text = "Alignment";
            this.labelAlignment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxAlignment
            // 
            this.comboBoxAlignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAlignment.Items.AddRange(new object[] {
            "Near",
            "Center",
            "Far"});
            this.comboBoxAlignment.Location = new System.Drawing.Point(16, 168);
            this.comboBoxAlignment.Name = "comboBoxAlignment";
            this.comboBoxAlignment.Size = new System.Drawing.Size(104, 21);
            this.comboBoxAlignment.TabIndex = 9;
            // 
            // buttonRemoveAll
            // 
            this.buttonRemoveAll.Location = new System.Drawing.Point(448, 237);
            this.buttonRemoveAll.Name = "buttonRemoveAll";
            this.buttonRemoveAll.Size = new System.Drawing.Size(96, 23);
            this.buttonRemoveAll.TabIndex = 6;
            this.buttonRemoveAll.Text = "RemoveAll";
            this.buttonRemoveAll.Click += new System.EventHandler(this.buttonRemoveAll_Click);
            // 
            // buttonRemoveFirst
            // 
            this.buttonRemoveFirst.Location = new System.Drawing.Point(448, 205);
            this.buttonRemoveFirst.Name = "buttonRemoveFirst";
            this.buttonRemoveFirst.Size = new System.Drawing.Size(96, 23);
            this.buttonRemoveFirst.TabIndex = 5;
            this.buttonRemoveFirst.Text = "RemoveFirst";
            this.buttonRemoveFirst.Click += new System.EventHandler(this.buttonRemoveFirst_Click);
            // 
            // buttonAddTime
            // 
            this.buttonAddTime.Location = new System.Drawing.Point(448, 49);
            this.buttonAddTime.Name = "buttonAddTime";
            this.buttonAddTime.Size = new System.Drawing.Size(96, 23);
            this.buttonAddTime.TabIndex = 3;
            this.buttonAddTime.Text = "Add Time";
            this.buttonAddTime.Click += new System.EventHandler(this.buttonAddTime_Click);
            // 
            // timerForPanels
            // 
            this.timerForPanels.Enabled = true;
            this.timerForPanels.Interval = 300;
            this.timerForPanels.Tick += new System.EventHandler(this.OnTick);
            // 
            // buttonAP
            // 
            this.buttonAP.Location = new System.Drawing.Point(448, 81);
            this.buttonAP.Name = "buttonAP";
            this.buttonAP.Size = new System.Drawing.Size(96, 23);
            this.buttonAP.TabIndex = 4;
            this.buttonAP.Text = "Add Progress";
            this.buttonAP.Click += new System.EventHandler(this.buttonAP_Click);
            // 
            // groupBoxStyles
            // 
            this.groupBoxStyles.Controls.Add(this.radioMediaPlayerBlue);
            this.groupBoxStyles.Controls.Add(this.radioMediaPlayerOrange);
            this.groupBoxStyles.Controls.Add(this.radioMediaPlayerPurple);
            this.groupBoxStyles.Controls.Add(this.radioOffice2007Blue);
            this.groupBoxStyles.Controls.Add(this.radioOffice2007Silver);
            this.groupBoxStyles.Controls.Add(this.radioOffice2007Black);
            this.groupBoxStyles.Controls.Add(this.radioIDE2005);
            this.groupBoxStyles.Controls.Add(this.radioPlain);
            this.groupBoxStyles.Controls.Add(this.radioOffice2003);
            this.groupBoxStyles.Location = new System.Drawing.Point(12, 12);
            this.groupBoxStyles.Name = "groupBoxStyles";
            this.groupBoxStyles.Size = new System.Drawing.Size(157, 253);
            this.groupBoxStyles.TabIndex = 0;
            this.groupBoxStyles.TabStop = false;
            this.groupBoxStyles.Text = "Visual Style";
            // 
            // radioOffice2007Blue
            // 
            this.radioOffice2007Blue.Checked = true;
            this.radioOffice2007Blue.Location = new System.Drawing.Point(6, 24);
            this.radioOffice2007Blue.Name = "radioOffice2007Blue";
            this.radioOffice2007Blue.Size = new System.Drawing.Size(145, 24);
            this.radioOffice2007Blue.TabIndex = 0;
            this.radioOffice2007Blue.Text = "Office2007 Blue";
            this.radioOffice2007Blue.CheckedChanged += new System.EventHandler(this.radioStyle_CheckedChanged);
            // 
            // radioOffice2007Silver
            // 
            this.radioOffice2007Silver.Location = new System.Drawing.Point(6, 48);
            this.radioOffice2007Silver.Name = "radioOffice2007Silver";
            this.radioOffice2007Silver.Size = new System.Drawing.Size(145, 24);
            this.radioOffice2007Silver.TabIndex = 1;
            this.radioOffice2007Silver.Text = "Office2007 Silver";
            this.radioOffice2007Silver.CheckedChanged += new System.EventHandler(this.radioStyle_CheckedChanged);
            // 
            // radioOffice2007Black
            // 
            this.radioOffice2007Black.Location = new System.Drawing.Point(6, 72);
            this.radioOffice2007Black.Name = "radioOffice2007Black";
            this.radioOffice2007Black.Size = new System.Drawing.Size(145, 24);
            this.radioOffice2007Black.TabIndex = 2;
            this.radioOffice2007Black.Text = "Office2007 Black";
            this.radioOffice2007Black.CheckedChanged += new System.EventHandler(this.radioStyle_CheckedChanged);
            // 
            // radioIDE2005
            // 
            this.radioIDE2005.Location = new System.Drawing.Point(6, 192);
            this.radioIDE2005.Name = "radioIDE2005";
            this.radioIDE2005.Size = new System.Drawing.Size(145, 24);
            this.radioIDE2005.TabIndex = 7;
            this.radioIDE2005.Text = "IDE2005";
            this.radioIDE2005.CheckedChanged += new System.EventHandler(this.radioStyle_CheckedChanged);
            // 
            // radioPlain
            // 
            this.radioPlain.Location = new System.Drawing.Point(6, 216);
            this.radioPlain.Name = "radioPlain";
            this.radioPlain.Size = new System.Drawing.Size(145, 24);
            this.radioPlain.TabIndex = 8;
            this.radioPlain.Text = "Plain";
            this.radioPlain.CheckedChanged += new System.EventHandler(this.radioStyle_CheckedChanged);
            // 
            // radioOffice2003
            // 
            this.radioOffice2003.Location = new System.Drawing.Point(6, 168);
            this.radioOffice2003.Name = "radioOffice2003";
            this.radioOffice2003.Size = new System.Drawing.Size(145, 24);
            this.radioOffice2003.TabIndex = 6;
            this.radioOffice2003.Text = "Office2003";
            this.radioOffice2003.CheckedChanged += new System.EventHandler(this.radioStyle_CheckedChanged);
            // 
            // radioMediaPlayerBlue
            // 
            this.radioMediaPlayerBlue.Location = new System.Drawing.Point(6, 96);
            this.radioMediaPlayerBlue.Name = "radioMediaPlayerBlue";
            this.radioMediaPlayerBlue.Size = new System.Drawing.Size(145, 24);
            this.radioMediaPlayerBlue.TabIndex = 3;
            this.radioMediaPlayerBlue.Text = "Media Player Blue";
            this.radioMediaPlayerBlue.CheckedChanged += new System.EventHandler(this.radioStyle_CheckedChanged);
            // 
            // radioMediaPlayerOrange
            // 
            this.radioMediaPlayerOrange.Location = new System.Drawing.Point(6, 120);
            this.radioMediaPlayerOrange.Name = "radioMediaPlayerOrange";
            this.radioMediaPlayerOrange.Size = new System.Drawing.Size(145, 24);
            this.radioMediaPlayerOrange.TabIndex = 4;
            this.radioMediaPlayerOrange.Text = "Media Player Orange";
            this.radioMediaPlayerOrange.CheckedChanged += new System.EventHandler(this.radioStyle_CheckedChanged);
            // 
            // radioMediaPlayerPurple
            // 
            this.radioMediaPlayerPurple.Location = new System.Drawing.Point(6, 144);
            this.radioMediaPlayerPurple.Name = "radioMediaPlayerPurple";
            this.radioMediaPlayerPurple.Size = new System.Drawing.Size(145, 24);
            this.radioMediaPlayerPurple.TabIndex = 5;
            this.radioMediaPlayerPurple.Text = "Media Player Purple";
            this.radioMediaPlayerPurple.CheckedChanged += new System.EventHandler(this.radioStyle_CheckedChanged);
            // 
            // SampleStatusBarControl
            // 
            this.ClientSize = new System.Drawing.Size(555, 298);
            this.Controls.Add(this.groupBoxStyles);
            this.Controls.Add(this.buttonAP);
            this.Controls.Add(this.buttonAddTime);
            this.Controls.Add(this.buttonRemoveFirst);
            this.Controls.Add(this.buttonRemoveAll);
            this.Controls.Add(this.groupBoxAdd);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.statusBarControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(539, 286);
            this.Name = "SampleStatusBarControl";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DotNetMagic - SampleStatusBarControl";
            ((System.ComponentModel.ISupportInitialize)(this.numericWidth)).EndInit();
            this.groupBoxAdd.ResumeLayout(false);
            this.groupBoxAdd.PerformLayout();
            this.groupBoxStyles.ResumeLayout(false);
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
            Application.Run(new SampleStatusBarControl());
		}

		private void buttonAdd_Click(object sender, System.EventArgs e)
		{
			Add(0);
		}

		private void buttonAddTime_Click(object sender, System.EventArgs e)
		{
			Add(1);
		}
		
		private void buttonAP_Click(object sender, System.EventArgs e)
		{
			Add(2);
		}
		
		private void Add(int tag)
		{
			StatusPanel panel = new StatusPanel();
			panel.Text = textBox.Text;
			panel.RequestedWidth = (int)numericWidth.Value;
			
			switch(comboBoxSize.SelectedIndex)
			{
				case 0:
					panel.AutoSizing = StatusBarPanelAutoSize.None;
					break;
				case 1:
					panel.AutoSizing = StatusBarPanelAutoSize.Contents;
					break;
				case 2:
					panel.AutoSizing = StatusBarPanelAutoSize.Spring;
					break;
			}
			
			if (comboBoxIcon.SelectedIndex > 0)
				panel.Image = imageList.Images[comboBoxIcon.SelectedIndex];
			
			switch(comboBoxAlignment.SelectedIndex)
			{
				case 0:
					panel.Alignment = StringAlignment.Near;
					break;
				case 1:
					panel.Alignment = StringAlignment.Center;
					break;
				case 2:
					panel.Alignment = StringAlignment.Far;
					break;
			}
			
			switch(comboBoxBorder.SelectedIndex)
			{
				case 0:
					panel.PanelBorder = PanelBorder.None;
					break;
				case 1:
					panel.PanelBorder = PanelBorder.Sunken;
					break;
				case 2:
					panel.PanelBorder = PanelBorder.Raised;
					break;
				case 3:
					panel.PanelBorder = PanelBorder.Dotted;
					break;
				case 4:
					panel.PanelBorder = PanelBorder.Dashed;
					break;
				case 5:
					panel.PanelBorder = PanelBorder.Solid;
					break;
			}
			
			// Progress control overrides some values
			if (tag == 2)
			{
				panel.AutoSizing = StatusBarPanelAutoSize.None;
				
				ProgressBar bar = new ProgressBar();
				bar.Minimum = 0;
				bar.Maximum = 30;
				bar.Dock = DockStyle.Fill;
				panel.Controls.Add(bar);
			}
			
			panel.Tag = tag;
			
			statusBarControl1.StatusPanels.Add(panel);	
		}

		private void buttonRemoveFirst_Click(object sender, System.EventArgs e)
		{
			if (statusBarControl1.StatusPanels.Count > 0)
				statusBarControl1.StatusPanels.RemoveAt(0);	
		}

		private void buttonRemoveAll_Click(object sender, System.EventArgs e)
		{
			statusBarControl1.StatusPanels.Clear();
		}

		private void OnTick(object sender, System.EventArgs e)
		{
			// Scan each of the status panels
			foreach(StatusPanel panel in statusBarControl1.StatusPanels)
			{
				// Is it designated for time information?
				if ((int)panel.Tag == 1)
					panel.Text = DateTime.Now.ToLongTimeString();

				// Is it designated for progrss information?
				if ((int)panel.Tag == 2)
				{
					ProgressBar bar = panel.Controls[0] as ProgressBar;
					bar.Value++;
					if (bar.Value >= bar.Maximum)
						bar.Value = bar.Minimum;
				}
			}
		}

		private void radioStyle_CheckedChanged(object sender, System.EventArgs e)
		{
			VisualStyle style;
			
			if (radioOffice2003.Checked)
				style = VisualStyle.Office2003;
			else if (radioIDE2005.Checked)
				style = VisualStyle.IDE2005;
			else if (radioPlain.Checked)
				style = VisualStyle.Plain;
            else if (radioOffice2007Black.Checked)
                style = VisualStyle.Office2007Black;
            else if (radioOffice2007Silver.Checked)
                style = VisualStyle.Office2007Silver;
            else if (radioOffice2007Blue.Checked)
                style = VisualStyle.Office2007Blue;
            else if (radioMediaPlayerBlue.Checked)
                style = VisualStyle.MediaPlayerBlue;
            else if (radioMediaPlayerOrange.Checked)
                style = VisualStyle.MediaPlayerOrange;
            else
                style = VisualStyle.MediaPlayerPurple;
				
			statusBarControl1.Style = style;
            Style = style;
		}
	}
}
