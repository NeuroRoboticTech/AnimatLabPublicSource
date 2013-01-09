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

namespace SampleTabbedAdv
{
	/// <summary>
	/// Summary description for SampleTabbedAdv.
	/// </summary>
	public class SampleTabbedAdv : DotNetMagicForm
	{
		// Instance fields
		private int _scenario;
		private Decimal _scenario1Top;
		private Decimal _scenario1Bottom;
		private Decimal _scenario2Left;
		private Decimal _scenario2Bottom;
		private ImageList imageList;
	
		// Designer generated
		private Crownwood.DotNetMagic.Controls.TabbedGroups tabbedGroups1;
		private Crownwood.DotNetMagic.Controls.StatusBarControl statusBarControl1;
		private Crownwood.DotNetMagic.Controls.StatusPanel statusPanelText;
		private Crownwood.DotNetMagic.Controls.StatusPanel statusPanelTime;
		private System.Windows.Forms.Timer timerForTime;
        private MenuStrip menuStrip;
        private ToolStripMenuItem scenariosToolStripMenuItem;
        private ToolStripMenuItem scenario1ToolStripMenuItem;
        private ToolStripMenuItem scenario2ToolStripMenuItem;
        private ToolStripMenuItem scenario3ToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private Panel panel1;
        private OfficeExtender officeExtender;
		private System.ComponentModel.IContainer components;

		public SampleTabbedAdv()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
			// Load the strip of simple images
			imageList = ResourceHelper.LoadBitmapStrip(typeof(SampleTabbedAdv), 
													   "SampleTabbedAdv.SampleImages.bmp", 
													   new Size(16, 16), 
													   Point.Empty);
			
			// Create initial scenario
			CreateScenario1();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SampleTabbedAdv));
            this.tabbedGroups1 = new Crownwood.DotNetMagic.Controls.TabbedGroups();
            this.statusBarControl1 = new Crownwood.DotNetMagic.Controls.StatusBarControl();
            this.statusPanelText = new Crownwood.DotNetMagic.Controls.StatusPanel();
            this.statusPanelTime = new Crownwood.DotNetMagic.Controls.StatusPanel();
            this.timerForTime = new System.Windows.Forms.Timer(this.components);
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.scenariosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scenario1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scenario2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scenario3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.officeExtender = new Crownwood.DotNetMagic.Controls.OfficeExtender(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.tabbedGroups1)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabbedGroups1
            // 
            this.tabbedGroups1.AllowDrop = true;
            this.tabbedGroups1.AtLeastOneLeaf = true;
            this.tabbedGroups1.DisplayTabMode = Crownwood.DotNetMagic.Controls.DisplayTabModes.HideAll;
            this.tabbedGroups1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabbedGroups1.Location = new System.Drawing.Point(5, 5);
            this.tabbedGroups1.Name = "tabbedGroups1";
            this.officeExtender.SetOffice2003BackColor(this.tabbedGroups1, Crownwood.DotNetMagic.Controls.OfficeColor.Disable);
            this.officeExtender.SetOffice2007BackColor(this.tabbedGroups1, Crownwood.DotNetMagic.Controls.OfficeColor.Disable);
            this.tabbedGroups1.ProminentLeaf = null;
            this.tabbedGroups1.ResizeBarColor = System.Drawing.SystemColors.Control;
            this.tabbedGroups1.Size = new System.Drawing.Size(398, 346);
            this.tabbedGroups1.TabIndex = 0;
            this.tabbedGroups1.TabControlCreated += new Crownwood.DotNetMagic.Controls.TabbedGroups.TabControlCreatedHandler(this.OnTabCreated);
            // 
            // statusBarControl1
            // 
            this.statusBarControl1.Location = new System.Drawing.Point(0, 380);
            this.statusBarControl1.Name = "statusBarControl1";
            this.officeExtender.SetOffice2003BackColor(this.statusBarControl1, Crownwood.DotNetMagic.Controls.OfficeColor.Disable);
            this.officeExtender.SetOffice2007BackColor(this.statusBarControl1, Crownwood.DotNetMagic.Controls.OfficeColor.Disable);
            this.statusBarControl1.PadTop = 4;
            this.statusBarControl1.Size = new System.Drawing.Size(408, 22);
            this.statusBarControl1.StatusPanels.AddRange(new Crownwood.DotNetMagic.Controls.StatusPanel[] {
            this.statusPanelText,
            this.statusPanelTime});
            this.statusBarControl1.TabIndex = 1;
            // 
            // statusPanelText
            // 
            this.statusPanelText.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.statusPanelText.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.statusPanelText.AutoSizing = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.statusPanelText.Location = new System.Drawing.Point(2, 2);
            this.statusPanelText.Name = "statusPanelText";
            this.officeExtender.SetOffice2003BackColor(this.statusPanelText, Crownwood.DotNetMagic.Controls.OfficeColor.Disable);
            this.officeExtender.SetOffice2007BackColor(this.statusPanelText, Crownwood.DotNetMagic.Controls.OfficeColor.Disable);
            this.statusPanelText.Size = new System.Drawing.Size(380, 13);
            this.statusPanelText.TabIndex = 0;
            // 
            // statusPanelTime
            // 
            this.statusPanelTime.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.statusPanelTime.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.statusPanelTime.AutoSizing = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.statusPanelTime.Location = new System.Drawing.Point(2, 2);
            this.statusPanelTime.Name = "statusPanelTime";
            this.officeExtender.SetOffice2003BackColor(this.statusPanelTime, Crownwood.DotNetMagic.Controls.OfficeColor.Disable);
            this.officeExtender.SetOffice2007BackColor(this.statusPanelTime, Crownwood.DotNetMagic.Controls.OfficeColor.Disable);
            this.statusPanelTime.Size = new System.Drawing.Size(0, 13);
            this.statusPanelTime.TabIndex = 0;
            // 
            // timerForTime
            // 
            this.timerForTime.Enabled = true;
            this.timerForTime.Interval = 300;
            this.timerForTime.Tick += new System.EventHandler(this.OnTimeTick);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scenariosToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.officeExtender.SetOffice2003BackColor(this.menuStrip, Crownwood.DotNetMagic.Controls.OfficeColor.Disable);
            this.officeExtender.SetOffice2007BackColor(this.menuStrip, Crownwood.DotNetMagic.Controls.OfficeColor.Disable);
            this.menuStrip.Size = new System.Drawing.Size(408, 24);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip1";
            // 
            // scenariosToolStripMenuItem
            // 
            this.scenariosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scenario1ToolStripMenuItem,
            this.scenario2ToolStripMenuItem,
            this.scenario3ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.scenariosToolStripMenuItem.Name = "scenariosToolStripMenuItem";
            this.scenariosToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.scenariosToolStripMenuItem.Text = "Scenarios";
            // 
            // scenario1ToolStripMenuItem
            // 
            this.scenario1ToolStripMenuItem.Name = "scenario1ToolStripMenuItem";
            this.scenario1ToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.scenario1ToolStripMenuItem.Text = "Scenario 1";
            this.scenario1ToolStripMenuItem.Click += new System.EventHandler(this.scenario1ToolStripMenuItem_Click);
            // 
            // scenario2ToolStripMenuItem
            // 
            this.scenario2ToolStripMenuItem.Name = "scenario2ToolStripMenuItem";
            this.scenario2ToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.scenario2ToolStripMenuItem.Text = "Scenario 2";
            this.scenario2ToolStripMenuItem.Click += new System.EventHandler(this.scenario2ToolStripMenuItem_Click);
            // 
            // scenario3ToolStripMenuItem
            // 
            this.scenario3ToolStripMenuItem.Name = "scenario3ToolStripMenuItem";
            this.scenario3ToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.scenario3ToolStripMenuItem.Text = "Scenario 3";
            this.scenario3ToolStripMenuItem.Click += new System.EventHandler(this.scenario3ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(132, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.panel1.Controls.Add(this.tabbedGroups1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.officeExtender.SetOffice2003BackColor(this.panel1, Crownwood.DotNetMagic.Controls.OfficeColor.Disable);
            this.officeExtender.SetOffice2007BackColor(this.panel1, Crownwood.DotNetMagic.Controls.OfficeColor.Base);
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(408, 356);
            this.panel1.TabIndex = 3;
            // 
            // officeExtender
            // 
            this.officeExtender.Office2007Variant = Crownwood.DotNetMagic.Controls.Office2007Variant.Blue;
            // 
            // SampleTabbedAdv
            // 
            this.ClientSize = new System.Drawing.Size(408, 402);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusBarControl1);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "SampleTabbedAdv";
            this.officeExtender.SetOffice2003BackColor(this, Crownwood.DotNetMagic.Controls.OfficeColor.Disable);
            this.officeExtender.SetOffice2007BackColor(this, Crownwood.DotNetMagic.Controls.OfficeColor.Disable);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DotNetMagic - SampleTabbedAdv";
            ((System.ComponentModel.ISupportInitialize)(this.tabbedGroups1)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
            Application.EnableVisualStyles();
            Application.Run(new SampleTabbedAdv());
		}
		
		private void CreateScenario1()
		{
			// Which scenario are we creating			
			_scenario = 1;
			
			// Clear out all current contents
			tabbedGroups1.RootSequence.Clear();

			// Set direction to be vertical
			tabbedGroups1.RootDirection = LayoutDirection.Vertical;
			
			// Get access to the defaulted tab left
			TabGroupLeaf tgl1 = tabbedGroups1.RootSequence[0] as TabGroupLeaf;
			
			// Add two more tab leafs
			TabGroupLeaf tgl2 = tabbedGroups1.RootSequence.AddNewLeaf();
			TabGroupLeaf tgl3 = tabbedGroups1.RootSequence.AddNewLeaf();
			
			// Create a 'Example' for each one
			Example e1 = new Example("Scenario1", "Top", "Optional Area", ArrowButton.DownArrow, new EventHandler(OnArrowClick1Top));
			Example e2 = new Example("Scenario1", "Middle", "Mandatory Area", ArrowButton.None, null);
			Example e3 = new Example("Scenario1", "Bottom", "Optional Area", ArrowButton.UpArrow, new EventHandler(OnArrowClick1Bottom));
			
			// Set each one into its group leaf
			tgl1.TabPages.Add(new Crownwood.DotNetMagic.Controls.TabPage(string.Empty, e1));
			tgl2.TabPages.Add(new Crownwood.DotNetMagic.Controls.TabPage(string.Empty, e2));
			tgl3.TabPages.Add(new Crownwood.DotNetMagic.Controls.TabPage(string.Empty, e3));
			
			// Make sure that only the last area has any space
			tgl1.Space = 0;
			tgl2.Space = 100;
			tgl3.Space = 0;
			
			// Remember how much space top and bottom would like when expanded
			_scenario1Top = 25;
			_scenario1Bottom = 25;
			
			// Define the minimum size of each example
            tgl1.MinimumSize = e1.MinimumRequestedSize;
            tgl2.MinimumSize = e2.MinimumRequestedSize;
            tgl3.MinimumSize = e3.MinimumRequestedSize;

            // Prevent user from resizing collapses top and bottom areas			
			tgl1.ResizeBarLock = true;
			tgl3.ResizeBarLock = true;
			
			e1.RichTextBox.Text = "This top area can be expanded or collapsed by using the arrow button on the title bar. " +
								  "Once expanded you can resize the relative spacing between this area and the middle by using the resize bar between the two.";  

			e2.RichTextBox.Text = "This scenario demonstrates how to combine the TabbedGroups control with the TitleBar control in order to provide three working areas.\n\n" +
							      "The middle area is intended to be mandatory and is always fully visible and available for the user to interact with.\n\n" +
							      "Top and bottom areas are intended to be optional and allow the user to collpase them down to just a title bar when they are not required.";
							   
			e3.RichTextBox.Text = "This bottom area can be expanded or collapsed by using the arrow button on the title bar. " +
							      "Once expanded you can resize the relative spacing between this area and the middle by using the resize bar between the two.";  
 			
			// Set the default bar vector
			tabbedGroups1.ResetResizeBarVector();

			statusPanelText.Text = "Three groups in vertical group";
			
			// Now get them all positioned correctly
			tabbedGroups1.RootSequence.Reposition();
		}

		private void OnArrowClick1Top(object sender, System.EventArgs e)
		{
			// Get access to the top and middle areas
			TabGroupLeaf tgl1 = tabbedGroups1.RootSequence[0] as TabGroupLeaf;
			TabGroupLeaf tgl2 = tabbedGroups1.RootSequence[1] as TabGroupLeaf;
		
			// Are we expanding the area?
			if (tgl1.Space == 0)
			{
				// How much space does top want if allowed?
				Decimal alloc = _scenario1Top;
				
				// Limit check because middle can only provide what it has
				if (alloc > tgl2.Space)
					alloc = tgl2.Space;
					
				// Give top its space allocation
				tgl1.Space = alloc;
				
				// Remove allocation from middle
				tgl2.Space -= alloc;
				
				// Make arrow point upwards as next click collpases it
				(tgl1.TabPages[0].Control as Example).TitleBar.ArrowButton = ArrowButton.UpArrow;

				// Expanded means you can resize it				
				tgl1.ResizeBarLock = false;
			}
			else
			{
				// Remember how much space top would like when expanded
				_scenario1Top = tgl1.Space;
			
				// Realloc top space to the middle mandatory area
				tgl2.Space += tgl1.Space;
				
				// Collapse the top
				tgl1.Space = 0;

				// Make arrow point downwards as next click expands it
				(tgl1.TabPages[0].Control as Example).TitleBar.ArrowButton = ArrowButton.DownArrow;

				// Collapsed means you cannot resize it				
				tgl1.ResizeBarLock = true;
			}
			
			// Reflect changes immediately
			tabbedGroups1.RootSequence.Reposition();
		}

		private void OnArrowClick1Bottom(object sender, System.EventArgs e)
		{
			// Get access to the middle and bottom areas
			TabGroupLeaf tgl2 = tabbedGroups1.RootSequence[1] as TabGroupLeaf;
			TabGroupLeaf tgl3 = tabbedGroups1.RootSequence[2] as TabGroupLeaf;
		
			// Are we expanding the area?
			if (tgl3.Space == 0)
			{
				// How much space does bottom want if allowed?
				Decimal alloc = _scenario1Bottom;
				
				// Limit check because middle can only provide what it has
				if (alloc > tgl2.Space)
					alloc = tgl2.Space;
					
				// Give bottom its space allocation
				tgl3.Space = alloc;
				
				// Remove allocation from middle
				tgl2.Space -= alloc;
				
				// Make arrow point upwards as next click collapses it
				(tgl3.TabPages[0].Control as Example).TitleBar.ArrowButton = ArrowButton.DownArrow;

				// Expanded means you can resize it				
				tgl3.ResizeBarLock = false;
			}
			else
			{
				// Remember how much space bottom would like when expanded
				_scenario1Top = tgl3.Space;
			
				// Realloc bottom space to the middle mandatory area
				tgl2.Space += tgl3.Space;
				
				// Collapse the bottom
				tgl3.Space = 0;

				// Make arrow point downwards as next click expands it
				(tgl3.TabPages[0].Control as Example).TitleBar.ArrowButton = ArrowButton.UpArrow;

				// Collapsed means you cannot resize it				
				tgl3.ResizeBarLock = true;
			}
			
			// Reflect changes immediately
			tabbedGroups1.RootSequence.Reposition();
		}

		private void CreateScenario2()
		{
			// Which scenario are we creating			
			_scenario = 2;

			// Clear out all current contents
			tabbedGroups1.RootSequence.Clear();
			
			// Set direction to be Horizontal
			tabbedGroups1.RootDirection = LayoutDirection.Horizontal;
			
			// Get access to the defaulted tab left
			TabGroupLeaf tgl1 = tabbedGroups1.RootSequence[0] as TabGroupLeaf;
			
			// Create a new sequence for holding two verical groups
			TabGroupSequence tgs1 = tabbedGroups1.RootSequence.AddNewSequence();
			
			// Add two more tab leafs to the new sequence
			TabGroupLeaf tgl2 = tgs1.AddNewLeaf();
			TabGroupLeaf tgl3 = tgs1.AddNewLeaf();
			
			// Create a 'Example' for each one
			Example e1 = new Example("Scenario2", "Left", "Optional Area", ArrowButton.RightArrow, new EventHandler(OnArrowClick2Left));
			Example e2 = new Example("Scenario2", "Top", "Mandatory Area", ArrowButton.None, null);
			Example e3 = new Example("Scenario2", "Bottom", "Optional Area", ArrowButton.UpArrow, new EventHandler(OnArrowClick2Bottom));
			
			// Set each one into its group leaf
			tgl1.TabPages.Add(new Crownwood.DotNetMagic.Controls.TabPage(string.Empty, e1));
			tgl2.TabPages.Add(new Crownwood.DotNetMagic.Controls.TabPage(string.Empty, e2));
			tgl3.TabPages.Add(new Crownwood.DotNetMagic.Controls.TabPage(string.Empty, e3));
			
			// Make sure that only the last area has any space
			tgl1.Space = 0;
			tgs1.Space = 100;
			tgl2.Space = 100;
			tgl3.Space = 0;
			
			// Remember how much space left and bottom would like when expanded
			_scenario2Left = 25;
			_scenario2Bottom = 25;
			
			// Define the minimum size of each example
            tgl1.MinimumSize = e1.MinimumRequestedSize;
            tgl2.MinimumSize = e2.MinimumRequestedSize;
            tgl3.MinimumSize = e3.MinimumRequestedSize;

            // Prevent user from resizing collapses top and bottom areas			
			tgl1.ResizeBarLock = true;
			tgl3.ResizeBarLock = true;
			
			// Set different titlebar colours for some variety
			e1.TitleBar.BackColor = e2.TitleBar.BackColor = e3.TitleBar.BackColor = Color.DarkRed;
			e1.TitleBar.ForeColor = e2.TitleBar.ForeColor = e3.TitleBar.ForeColor = Color.White;
			e1.TitleBar.GradientActiveColor = e2.TitleBar.GradientActiveColor = e3.TitleBar.GradientActiveColor = Color.Pink;
			e1.TitleBar.MouseOverColor = e2.TitleBar.MouseOverColor = e3.TitleBar.MouseOverColor = Color.Yellow;
			
			// Place in centre of bar for variety
			e1.TitleBar.TextAlignment = e2.TitleBar.TextAlignment = e3.TitleBar.TextAlignment = StringAlignment.Center;
			
			// Change gradient for variety as well
			e1.TitleBar.GradientDirection = e2.TitleBar.GradientDirection = e3.TitleBar.GradientDirection = GradientDirection.TopLeftToBottomRight;
			
			e1.RichTextBox.Text = "This left area can be expanded or collapsed by using the arrow button on the title bar. " +
								  "Once expanded you can resize the relative spacing between this area and the right by using the resize bar between the two.";  

			e2.RichTextBox.Text = "This scenario expands on the first by producing a more complex arrangement where one is off to the left side.\n\n" +
							      "The top area is intended to be mandatory and is always fully visible and available for the user to interact with.\n\n" +
							      "Left and bottom areas are intended to be optional and allow the user to collpase them down to just a title bar when they are not required.";
							   
			e3.RichTextBox.Text = "This bottom area can be expanded or collapsed by using the arrow button on the title bar. " +
							      "Once expanded you can resize the relative spacing between this area and the top by using the resize bar between the two.";  

			// Set the default bar vector
			tabbedGroups1.ResetResizeBarVector();
			
			statusPanelText.Text = "Three groups but one is vertical";

			// Now get them all positioned correctly
			tabbedGroups1.RootSequence.Reposition();
		}

		private void OnArrowClick2Left(object sender, System.EventArgs e)
		{
			// Get access to the left and right areas
			TabGroupLeaf tgl1 = tabbedGroups1.RootSequence[0] as TabGroupLeaf;
			TabGroupSequence tgs1 = tabbedGroups1.RootSequence[1] as TabGroupSequence;
		
			// Are we expanding the area?
			if (tgl1.Space == 0)
			{
				// Give left its space allocation
				tgl1.Space = _scenario2Left;
				
				// Remove allocation from right
				tgs1.Space -= _scenario2Left;
				
				// Make arrow point left as next click collpases it
				(tgl1.TabPages[0].Control as Example).TitleBar.ArrowButton = ArrowButton.LeftArrow;

				// Expanded means you can resize it				
				tgl1.ResizeBarLock = false;
			}
			else
			{
				// Remember how much space left would like when expanded
				_scenario2Left = tgl1.Space;
			
				// Realloc top space to the right area
				tgs1.Space += tgl1.Space;
				
				// Collapse the top
				tgl1.Space = 0;

				// Make arrow point downwards as next click expands it
				(tgl1.TabPages[0].Control as Example).TitleBar.ArrowButton = ArrowButton.RightArrow;

				// Collapsed means you cannot resize it				
				tgl1.ResizeBarLock = true;
			}
			
			// Reflect changes immediately
			tabbedGroups1.RootSequence.Reposition();
		}

		private void OnArrowClick2Bottom(object sender, System.EventArgs e)
		{
			TabGroupSequence tgs1 = tabbedGroups1.RootSequence[1] as TabGroupSequence;
		
			// Get access to the middle and bottom areas
			TabGroupLeaf tgl2 = tgs1[0] as TabGroupLeaf;
			TabGroupLeaf tgl3 = tgs1[1] as TabGroupLeaf;
		
			// Are we expanding the area?
			if (tgl3.Space == 0)
			{
				// Give bottom its space allocation
				tgl3.Space = _scenario2Bottom;
				
				// Remove allocation from middle
				tgl2.Space -= _scenario2Bottom;
				
				// Make arrow point upwards as next click collapses it
				(tgl3.TabPages[0].Control as Example).TitleBar.ArrowButton = ArrowButton.DownArrow;

				// Expanded means you can resize it				
				tgl3.ResizeBarLock = false;
			}
			else
			{
				// Remember how much space bottom would like when expanded
				_scenario2Bottom = tgl3.Space;
			
				// Realloc bottom space to the middle mandatory area
				tgl2.Space += tgl3.Space;
				
				// Collapse the bottom
				tgl3.Space = 0;

				// Make arrow point downwards as next click expands it
				(tgl3.TabPages[0].Control as Example).TitleBar.ArrowButton = ArrowButton.UpArrow;

				// Collapsed means you cannot resize it				
				tgl3.ResizeBarLock = true;
			}
			
			// Reflect changes immediately
			tabbedGroups1.RootSequence.Reposition();
			tgs1.Reposition();
		}

		private void CreateScenario3()
		{
			// Which scenario are we creating			
			_scenario = 3;

			// Clear out all current contents
			tabbedGroups1.RootSequence.Clear();

			// Set direction to be vertical
			tabbedGroups1.RootDirection = LayoutDirection.Vertical;
			
			// Get access to the defaulted tab left
			TabGroupLeaf tgl1 = tabbedGroups1.RootSequence[0] as TabGroupLeaf;
			
			// Add several more leafs
			TabGroupLeaf tgl2 = tabbedGroups1.RootSequence.AddNewLeaf();
			TabGroupLeaf tgl3 = tabbedGroups1.RootSequence.AddNewLeaf();
			TabGroupLeaf tgl4 = tabbedGroups1.RootSequence.AddNewLeaf();
			TabGroupLeaf tgl5 = tabbedGroups1.RootSequence.AddNewLeaf();
			
			// Create a 'Example' for each one
			Example e1 = new Example("Scenario3", "One", string.Empty, ArrowButton.None, new EventHandler(OnTitleClick3));
			Example e2 = new Example("Scenario3", "Two", string.Empty, ArrowButton.None, new EventHandler(OnTitleClick3));
			Example e3 = new Example("Scenario3", "Three", string.Empty, ArrowButton.None, new EventHandler(OnTitleClick3));
			Example e4 = new Example("Scenario3", "Four", string.Empty, ArrowButton.None, new EventHandler(OnTitleClick3));
			Example e5 = new Example("Scenario3", "Five", string.Empty, ArrowButton.None, new EventHandler(OnTitleClick3));
			
			// Set each one into its group leaf
			tgl1.TabPages.Add(new Crownwood.DotNetMagic.Controls.TabPage(string.Empty, e1));
			tgl2.TabPages.Add(new Crownwood.DotNetMagic.Controls.TabPage(string.Empty, e2));
			tgl3.TabPages.Add(new Crownwood.DotNetMagic.Controls.TabPage(string.Empty, e3));
			tgl4.TabPages.Add(new Crownwood.DotNetMagic.Controls.TabPage(string.Empty, e4));
			tgl5.TabPages.Add(new Crownwood.DotNetMagic.Controls.TabPage(string.Empty, e5));
			
			// Make sure that only the last area has any space
			tgl1.Space = 0;
			tgl2.Space = 0;
			tgl3.Space = 0;
			tgl4.Space = 100;
			tgl5.Space = 0;
			
			// Define the minimum size of each example
            tgl1.MinimumSize = e1.MinimumRequestedSize;
            tgl2.MinimumSize = e2.MinimumRequestedSize;
            tgl3.MinimumSize = e3.MinimumRequestedSize;
            tgl4.MinimumSize = e3.MinimumRequestedSize;
            tgl5.MinimumSize = e3.MinimumRequestedSize;

            // Prevent user from resizing			
			tgl1.ResizeBarLock = true;
			tgl2.ResizeBarLock = true;
			tgl3.ResizeBarLock = true;
			tgl4.ResizeBarLock = true;
			tgl5.ResizeBarLock = true;
			
			// Do not want any sizing spaces between groups
			tabbedGroups1.ResizeBarVector = 0;
			
			// Make entire titlebar act as a button
			e1.TitleBar.ActAsButton = ActAsButton.WholeControl;
			e2.TitleBar.ActAsButton = ActAsButton.WholeControl;
			e3.TitleBar.ActAsButton = ActAsButton.WholeControl;
			e4.TitleBar.ActAsButton = ActAsButton.WholeControl;
			e5.TitleBar.ActAsButton = ActAsButton.WholeControl;
			
			// Set different titlebar colours for some variety
			e1.TitleBar.BackColor = e2.TitleBar.BackColor = e3.TitleBar.BackColor = e4.TitleBar.BackColor = e5.TitleBar.BackColor = Color.SlateBlue;
			e1.TitleBar.ForeColor = e2.TitleBar.ForeColor = e3.TitleBar.ForeColor = e4.TitleBar.ForeColor = e5.TitleBar.ForeColor = Color.White;
			e1.TitleBar.InactiveBackColor = e2.TitleBar.InactiveBackColor = e3.TitleBar.InactiveBackColor = e4.TitleBar.InactiveBackColor = e5.TitleBar.InactiveBackColor = ControlPaint.Light(Color.SlateBlue);
			e1.TitleBar.InactiveForeColor = e2.TitleBar.InactiveForeColor = e3.TitleBar.InactiveForeColor = e4.TitleBar.InactiveForeColor = e5.TitleBar.InactiveForeColor = ControlPaint.LightLight(Color.SlateBlue);
			e1.TitleBar.GradientActiveColor = e2.TitleBar.GradientActiveColor = e3.TitleBar.GradientActiveColor = e4.TitleBar.GradientActiveColor = e5.TitleBar.GradientActiveColor = Color.DarkSlateBlue;
			e1.TitleBar.GradientInactiveColor = e2.TitleBar.GradientInactiveColor = e3.TitleBar.GradientInactiveColor = e4.TitleBar.GradientInactiveColor = e5.TitleBar.GradientInactiveColor = ControlPaint.Light(Color.DarkSlateBlue);
			e1.TitleBar.MouseOverColor = e2.TitleBar.MouseOverColor = e3.TitleBar.MouseOverColor = e4.TitleBar.MouseOverColor = e5.TitleBar.MouseOverColor = Color.Yellow;

			// Set imagelist into each of the titlebars
			e1.TitleBar.ImageList = imageList;
			e2.TitleBar.ImageList = imageList;
			e3.TitleBar.ImageList = imageList;
			e4.TitleBar.ImageList = imageList;
			e5.TitleBar.ImageList = imageList;
			
			// Give each titlebar a different image to draw
			e1.TitleBar.ImageIndex = 0;
			e2.TitleBar.ImageIndex = 1;
			e3.TitleBar.ImageIndex = 2;
			e4.TitleBar.ImageIndex = 3;
			e4.TitleBar.ImageIndex = 4;

			string explanation = "By placing several groups directly below each other and also removing the resize bars you can achieve a list type effect.\n\n" +
								 "More features of the TitleBar are demonstrated by attaching images to them and changing the Active state of them depending on which is selected.";
								 
			e1.RichTextBox.Text = explanation;
			e2.RichTextBox.Text = explanation;
			e3.RichTextBox.Text = explanation;
			e4.RichTextBox.Text = explanation;
			e5.RichTextBox.Text = explanation;
								  
			// Only the currently selected one should be active
			e1.TitleBar.Active = e2.TitleBar.Active = e3.TitleBar.Active = e5.TitleBar.Active = false;

			statusPanelText.Text = "Five groups acting like a list";

			// Now get them all positioned correctly
			tabbedGroups1.RootSequence.Reposition();
		}
		
		private void OnTitleClick3(object sender, System.EventArgs e)
		{
			// Remember which title bar sent message
			TitleBar tbClick = sender as TitleBar;
		
			int index = 0;
			
			// Start search from the first leaf
			TabGroupLeaf tgl = tabbedGroups1.FirstLeaf();
			
			// Keep going till be get a match...
			while(tgl != null)
			{
				// Extract the example instance from page
				TitleBar tb = (tgl.TabPages[0].Control as Example).TitleBar;
			
				// Is the source of the click?
				if (tb == tbClick)
					break;
					
				// Move on to the next leaf in turn
				tgl = tabbedGroups1.NextLeaf(tgl);

				// Track index of found entry
				index++;
			}
			
			int test = 0;
			
			// Process all leafs again from begining
			tgl = tabbedGroups1.FirstLeaf();
			
			// Keep going till be get a match...
			while(tgl != null)
			{
				// Extract the example instance from page
				TitleBar tb = (tgl.TabPages[0].Control as Example).TitleBar;

				// Set active state depending on if it was the one clicked
				tb.Active = (index == test);
				
				// Set space depending on it it was the one clicked		
				tgl.Space = (index == test) ? 100 : 0;	
					
				// Move on to the next leaf in turn
				tgl = tabbedGroups1.NextLeaf(tgl);

				// Track index of found entry
				test++;
			}

			// Reflect changes immediately
			tabbedGroups1.RootSequence.Reposition();
		}

		private void OnTabCreated(Crownwood.DotNetMagic.Controls.TabbedGroups tg, Crownwood.DotNetMagic.Controls.TabControl tc)
		{
			// Do not want borders around scenario 3 tab controls
			if (_scenario == 3)
				tc.IDEPixelBorder = false;

			// Remove the default one pixel border between positioned 
			// control and the TabControl border area
			tc.ControlLeftOffset = -1;
			tc.ControlRightOffset = -1;
			tc.ControlTopOffset = -1;
			tc.ControlBottomOffset = -1;
		}
		
		private void OnTimeTick(object sender, System.EventArgs e)
		{
			// Update status bar with the current time
			statusPanelTime.Text = DateTime.Now.ToShortDateString();
		}

        private void scenario1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateScenario1();
        }

        private void scenario2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateScenario2();
        }

        private void scenario3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateScenario3();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
	}
}
