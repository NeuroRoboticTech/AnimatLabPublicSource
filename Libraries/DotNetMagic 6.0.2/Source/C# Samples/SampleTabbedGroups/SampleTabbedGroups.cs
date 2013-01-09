/// *****************************************************************************
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
using System.Reflection;
using Crownwood.DotNetMagic.Common;
using Crownwood.DotNetMagic.Controls;
using Crownwood.DotNetMagic.Docking;
using Crownwood.DotNetMagic.Forms;

namespace SampleTabbedGroups
{
	/// <summary>
	/// Summary description for SampleTabbedGroups.
	/// </summary>
	public class SampleTabbedGroups : DotNetMagicForm
	{
		// Internal state
	    private int _count = 1;
        private int _image = -1;
		private System.Windows.Forms.ImageList groupTabs;
		private Crownwood.DotNetMagic.Controls.TabbedGroups tabbedGroups1;
		private Crownwood.DotNetMagic.Controls.StatusBarControl statusBarControl1;
		private Crownwood.DotNetMagic.Controls.StatusPanel statusPanelText;
		private Crownwood.DotNetMagic.Controls.StatusPanel statusPanelDate;
		private System.Windows.Forms.Timer timer;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem menuStripStyles;
        private ToolStripMenuItem menuStripOffice2003;
        private ToolStripMenuItem menuStripSetup;
        private ToolStripMenuItem menuStripHeaderMode;
        private ToolStripMenuItem menuStripFeedback;
        private ToolStripMenuItem menuStripActions;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripOffice2003;
        private ToolStripButton toolStripIDE2005;
        private ToolStripButton toolStripPlain;
        private ToolStripContainer toolStripContainer1;
        private ToolStripMenuItem menuStripIDE2005;
        private ToolStripMenuItem menuStripPlain;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem menuStripExit;
        private ToolStripMenuItem menuStripAddPage;
        private ToolStripMenuItem menuStripRemovePage;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem menuStripInitializeSimple;
        private ToolStripMenuItem menuStripInitializeMedium;
        private ToolStripMenuItem menuStripInitalizeComplex;
        private ToolStripMenuItem menuStripHideAll;
        private ToolStripMenuItem menuStripShowAll;
        private ToolStripMenuItem menuStripShowActiveLeaf;
        private ToolStripMenuItem menuStripShowMouseOver;
        private ToolStripMenuItem menuStripShowActiveMouse;
        private ToolStripMenuItem menuStripShowMultipleTabs;
        private ToolStripMenuItem menuStripFocusRectangles;
        private ToolStripMenuItem menuStripSemiTransparent;
        private ToolStripMenuItem menuStripDiamondIndicators;
        private ToolStripMenuItem menuStripSquareIndicators;
        private ToolStripMenuItem menuStripRebalance;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripMenuItem menuStripProminent;
        private ToolStripMenuItem menuStripProminentGroupOnly;
        private ToolStripSeparator toolStripMenuItem4;
        private ToolStripMenuItem menuStripResizeThin;
        private ToolStripMenuItem menuStripResizeMedium;
        private ToolStripMenuItem menuStripResizeThick;
        private ToolStripSeparator toolStripMenuItem5;
        private ToolStripMenuItem menuStripResizeLock;
        private ToolStripMenuItem menuStripLayoutLock;
        private ToolStripButton toolStripOffice2007Blue;
        private ToolStripButton toolStripOffice2007Silver;
        private ToolStripButton toolStripOffice2007Black;
        private ToolStripMenuItem menuStripOffice2007Blue;
        private ToolStripMenuItem menuStripOffice2007Silver;
        private ToolStripMenuItem menuStripMediaPlayerPurple;
        private ToolStripMenuItem menuStripOffice2007Black;
        private ToolStripMenuItem menuStripMediaPlayerBlue;
        private ToolStripMenuItem menuStripMediaPlayerOrange;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton toolStripMediaPurple;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton toolStripMediaBlue;
        private ToolStripButton toolStripMediaOrange;
        private ToolStrip toolStrip2;
        private ToolStripButton toolStripAddPage;
        private ToolStripButton toolStripRemovePage;
		private System.ComponentModel.IContainer components;

		public SampleTabbedGroups()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			// Create the initial setup			
            menuStripInitializeMedium_Click(this, EventArgs.Empty);

			// Update the date and time in status bar immediately
			timer_Tick(this, EventArgs.Empty);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SampleTabbedGroups));
            this.groupTabs = new System.Windows.Forms.ImageList(this.components);
            this.tabbedGroups1 = new Crownwood.DotNetMagic.Controls.TabbedGroups();
            this.statusBarControl1 = new Crownwood.DotNetMagic.Controls.StatusBarControl();
            this.statusPanelText = new Crownwood.DotNetMagic.Controls.StatusPanel();
            this.statusPanelDate = new Crownwood.DotNetMagic.Controls.StatusPanel();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuStripStyles = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripOffice2007Blue = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripOffice2007Silver = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripOffice2007Black = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripMediaPlayerBlue = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripMediaPlayerOrange = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripMediaPlayerPurple = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripOffice2003 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripIDE2005 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripPlain = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStripExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripAddPage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripRemovePage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStripInitializeSimple = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripInitializeMedium = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripInitalizeComplex = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripHeaderMode = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripHideAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripShowAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripShowActiveLeaf = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripShowMouseOver = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripShowActiveMouse = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripShowMultipleTabs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripFeedback = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripFocusRectangles = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripSemiTransparent = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripDiamondIndicators = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripSquareIndicators = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripActions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripRebalance = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStripProminent = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripProminentGroupOnly = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStripResizeThin = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripResizeMedium = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripResizeThick = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStripResizeLock = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripLayoutLock = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripOffice2007Blue = new System.Windows.Forms.ToolStripButton();
            this.toolStripOffice2007Silver = new System.Windows.Forms.ToolStripButton();
            this.toolStripOffice2007Black = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMediaBlue = new System.Windows.Forms.ToolStripButton();
            this.toolStripMediaOrange = new System.Windows.Forms.ToolStripButton();
            this.toolStripMediaPurple = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripOffice2003 = new System.Windows.Forms.ToolStripButton();
            this.toolStripIDE2005 = new System.Windows.Forms.ToolStripButton();
            this.toolStripPlain = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripAddPage = new System.Windows.Forms.ToolStripButton();
            this.toolStripRemovePage = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedGroups1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupTabs
            // 
            this.groupTabs.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("groupTabs.ImageStream")));
            this.groupTabs.TransparentColor = System.Drawing.Color.Magenta;
            this.groupTabs.Images.SetKeyName(0, "");
            this.groupTabs.Images.SetKeyName(1, "");
            this.groupTabs.Images.SetKeyName(2, "");
            this.groupTabs.Images.SetKeyName(3, "");
            this.groupTabs.Images.SetKeyName(4, "");
            this.groupTabs.Images.SetKeyName(5, "");
            this.groupTabs.Images.SetKeyName(6, "");
            // 
            // tabbedGroups1
            // 
            this.tabbedGroups1.AllowDrop = true;
            this.tabbedGroups1.AtLeastOneLeaf = true;
            this.tabbedGroups1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabbedGroups1.ImageList = this.groupTabs;
            this.tabbedGroups1.Location = new System.Drawing.Point(5, 5);
            this.tabbedGroups1.Name = "tabbedGroups1";
            this.tabbedGroups1.ProminentLeaf = null;
            this.tabbedGroups1.ResizeBarColor = System.Drawing.SystemColors.Control;
            this.tabbedGroups1.Size = new System.Drawing.Size(724, 406);
            this.tabbedGroups1.TabIndex = 1;
            this.tabbedGroups1.ExternalDrop += new Crownwood.DotNetMagic.Controls.TabbedGroups.ExternalDropHandler(this.OnExternalDrop);
            this.tabbedGroups1.TabControlCreated += new Crownwood.DotNetMagic.Controls.TabbedGroups.TabControlCreatedHandler(this.OnTabControlCreated);
            // 
            // statusBarControl1
            // 
            this.statusBarControl1.Location = new System.Drawing.Point(0, 490);
            this.statusBarControl1.Name = "statusBarControl1";
            this.statusBarControl1.PadTop = 3;
            this.statusBarControl1.Size = new System.Drawing.Size(734, 24);
            this.statusBarControl1.StatusPanels.AddRange(new Crownwood.DotNetMagic.Controls.StatusPanel[] {
            this.statusPanelText,
            this.statusPanelDate});
            this.statusBarControl1.TabIndex = 3;
            // 
            // statusPanelText
            // 
            this.statusPanelText.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.statusPanelText.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.statusPanelText.AutoSizing = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.statusPanelText.Location = new System.Drawing.Point(2, 2);
            this.statusPanelText.Name = "statusPanelText";
            this.statusPanelText.Size = new System.Drawing.Size(706, 16);
            this.statusPanelText.TabIndex = 0;
            // 
            // statusPanelDate
            // 
            this.statusPanelDate.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.statusPanelDate.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.statusPanelDate.AutoSizing = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.statusPanelDate.Location = new System.Drawing.Point(2, 2);
            this.statusPanelDate.Name = "statusPanelDate";
            this.statusPanelDate.Size = new System.Drawing.Size(0, 16);
            this.statusPanelDate.TabIndex = 0;
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 900;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStripStyles,
            this.menuStripSetup,
            this.menuStripHeaderMode,
            this.menuStripFeedback,
            this.menuStripActions});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(734, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuStripStyles
            // 
            this.menuStripStyles.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStripOffice2007Blue,
            this.menuStripOffice2007Silver,
            this.menuStripOffice2007Black,
            this.menuStripMediaPlayerBlue,
            this.menuStripMediaPlayerOrange,
            this.menuStripMediaPlayerPurple,
            this.menuStripOffice2003,
            this.menuStripIDE2005,
            this.menuStripPlain,
            this.toolStripMenuItem1,
            this.menuStripExit});
            this.menuStripStyles.Name = "menuStripStyles";
            this.menuStripStyles.Size = new System.Drawing.Size(49, 20);
            this.menuStripStyles.Text = "Styles";
            // 
            // menuStripOffice2007Blue
            // 
            this.menuStripOffice2007Blue.Checked = true;
            this.menuStripOffice2007Blue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuStripOffice2007Blue.Name = "menuStripOffice2007Blue";
            this.menuStripOffice2007Blue.Size = new System.Drawing.Size(181, 22);
            this.menuStripOffice2007Blue.Text = "Office2007 Blue";
            this.menuStripOffice2007Blue.Click += new System.EventHandler(this.menuStripOffice2007Blue_Click);
            // 
            // menuStripOffice2007Silver
            // 
            this.menuStripOffice2007Silver.Name = "menuStripOffice2007Silver";
            this.menuStripOffice2007Silver.Size = new System.Drawing.Size(181, 22);
            this.menuStripOffice2007Silver.Text = "Office2007 Silver";
            this.menuStripOffice2007Silver.Click += new System.EventHandler(this.menuStripOffice2007Silver_Click);
            // 
            // menuStripOffice2007Black
            // 
            this.menuStripOffice2007Black.Name = "menuStripOffice2007Black";
            this.menuStripOffice2007Black.Size = new System.Drawing.Size(181, 22);
            this.menuStripOffice2007Black.Text = "Office2007 Black";
            this.menuStripOffice2007Black.Click += new System.EventHandler(this.toolStripOffice2007Black_Click);
            // 
            // menuStripMediaPlayerBlue
            // 
            this.menuStripMediaPlayerBlue.Name = "menuStripMediaPlayerBlue";
            this.menuStripMediaPlayerBlue.Size = new System.Drawing.Size(181, 22);
            this.menuStripMediaPlayerBlue.Text = "MediaPlayer Blue";
            this.menuStripMediaPlayerBlue.Click += new System.EventHandler(this.menuStripMediaPlayerBlue_Click);
            // 
            // menuStripMediaPlayerOrange
            // 
            this.menuStripMediaPlayerOrange.Name = "menuStripMediaPlayerOrange";
            this.menuStripMediaPlayerOrange.Size = new System.Drawing.Size(181, 22);
            this.menuStripMediaPlayerOrange.Text = "MediaPlayer Orange";
            this.menuStripMediaPlayerOrange.Click += new System.EventHandler(this.menuStripMediaPlayerOrange_Click);
            // 
            // menuStripMediaPlayerPurple
            // 
            this.menuStripMediaPlayerPurple.Name = "menuStripMediaPlayerPurple";
            this.menuStripMediaPlayerPurple.Size = new System.Drawing.Size(181, 22);
            this.menuStripMediaPlayerPurple.Text = "MediaPlayer Purple";
            this.menuStripMediaPlayerPurple.Click += new System.EventHandler(this.menuStripMediaPlayerPurple_Click);
            // 
            // menuStripOffice2003
            // 
            this.menuStripOffice2003.Name = "menuStripOffice2003";
            this.menuStripOffice2003.Size = new System.Drawing.Size(181, 22);
            this.menuStripOffice2003.Text = "Office2003";
            this.menuStripOffice2003.Click += new System.EventHandler(this.menuStripOffice2003_Click);
            // 
            // menuStripIDE2005
            // 
            this.menuStripIDE2005.Name = "menuStripIDE2005";
            this.menuStripIDE2005.Size = new System.Drawing.Size(181, 22);
            this.menuStripIDE2005.Text = "IDE2005";
            this.menuStripIDE2005.Click += new System.EventHandler(this.menuStripIDE2005_Click);
            // 
            // menuStripPlain
            // 
            this.menuStripPlain.Name = "menuStripPlain";
            this.menuStripPlain.Size = new System.Drawing.Size(181, 22);
            this.menuStripPlain.Text = "Plain";
            this.menuStripPlain.Click += new System.EventHandler(this.menuStripPlain_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(178, 6);
            // 
            // menuStripExit
            // 
            this.menuStripExit.Name = "menuStripExit";
            this.menuStripExit.Size = new System.Drawing.Size(181, 22);
            this.menuStripExit.Text = "Exit";
            this.menuStripExit.Click += new System.EventHandler(this.menuStripExit_Click);
            // 
            // menuStripSetup
            // 
            this.menuStripSetup.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStripAddPage,
            this.menuStripRemovePage,
            this.toolStripMenuItem2,
            this.menuStripInitializeSimple,
            this.menuStripInitializeMedium,
            this.menuStripInitalizeComplex});
            this.menuStripSetup.Name = "menuStripSetup";
            this.menuStripSetup.Size = new System.Drawing.Size(49, 20);
            this.menuStripSetup.Text = "Setup";
            // 
            // menuStripAddPage
            // 
            this.menuStripAddPage.Name = "menuStripAddPage";
            this.menuStripAddPage.Size = new System.Drawing.Size(167, 22);
            this.menuStripAddPage.Text = "Add Page";
            this.menuStripAddPage.Click += new System.EventHandler(this.menuStripAddPage_Click);
            // 
            // menuStripRemovePage
            // 
            this.menuStripRemovePage.Name = "menuStripRemovePage";
            this.menuStripRemovePage.Size = new System.Drawing.Size(167, 22);
            this.menuStripRemovePage.Text = "Remove Page";
            this.menuStripRemovePage.Click += new System.EventHandler(this.menuStripRemovePage_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(164, 6);
            // 
            // menuStripInitializeSimple
            // 
            this.menuStripInitializeSimple.Name = "menuStripInitializeSimple";
            this.menuStripInitializeSimple.Size = new System.Drawing.Size(167, 22);
            this.menuStripInitializeSimple.Text = "Initialize Simple";
            this.menuStripInitializeSimple.Click += new System.EventHandler(this.menuStripInitializeSimple_Click);
            // 
            // menuStripInitializeMedium
            // 
            this.menuStripInitializeMedium.Name = "menuStripInitializeMedium";
            this.menuStripInitializeMedium.Size = new System.Drawing.Size(167, 22);
            this.menuStripInitializeMedium.Text = "Initialize Medium";
            this.menuStripInitializeMedium.Click += new System.EventHandler(this.menuStripInitializeMedium_Click);
            // 
            // menuStripInitalizeComplex
            // 
            this.menuStripInitalizeComplex.Name = "menuStripInitalizeComplex";
            this.menuStripInitalizeComplex.Size = new System.Drawing.Size(167, 22);
            this.menuStripInitalizeComplex.Text = "Initialize Complex";
            this.menuStripInitalizeComplex.Click += new System.EventHandler(this.menuStripInitalizeComplex_Click);
            // 
            // menuStripHeaderMode
            // 
            this.menuStripHeaderMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStripHideAll,
            this.menuStripShowAll,
            this.menuStripShowActiveLeaf,
            this.menuStripShowMouseOver,
            this.menuStripShowActiveMouse,
            this.menuStripShowMultipleTabs});
            this.menuStripHeaderMode.Name = "menuStripHeaderMode";
            this.menuStripHeaderMode.Size = new System.Drawing.Size(88, 20);
            this.menuStripHeaderMode.Text = "HeaderMode";
            // 
            // menuStripHideAll
            // 
            this.menuStripHideAll.Name = "menuStripHideAll";
            this.menuStripHideAll.Size = new System.Drawing.Size(217, 22);
            this.menuStripHideAll.Text = "Hide All";
            this.menuStripHideAll.Click += new System.EventHandler(this.menuStripHideAll_Click);
            // 
            // menuStripShowAll
            // 
            this.menuStripShowAll.Checked = true;
            this.menuStripShowAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuStripShowAll.Name = "menuStripShowAll";
            this.menuStripShowAll.Size = new System.Drawing.Size(217, 22);
            this.menuStripShowAll.Text = "Show All";
            this.menuStripShowAll.Click += new System.EventHandler(this.menuStripShowAll_Click);
            // 
            // menuStripShowActiveLeaf
            // 
            this.menuStripShowActiveLeaf.Name = "menuStripShowActiveLeaf";
            this.menuStripShowActiveLeaf.Size = new System.Drawing.Size(217, 22);
            this.menuStripShowActiveLeaf.Text = "Show Active Leaf";
            this.menuStripShowActiveLeaf.Click += new System.EventHandler(this.menuStripShowActiveLeaf_Click);
            // 
            // menuStripShowMouseOver
            // 
            this.menuStripShowMouseOver.Name = "menuStripShowMouseOver";
            this.menuStripShowMouseOver.Size = new System.Drawing.Size(217, 22);
            this.menuStripShowMouseOver.Text = "Show Mouse Over";
            this.menuStripShowMouseOver.Click += new System.EventHandler(this.menuStripShowMouseOver_Click);
            // 
            // menuStripShowActiveMouse
            // 
            this.menuStripShowActiveMouse.Name = "menuStripShowActiveMouse";
            this.menuStripShowActiveMouse.Size = new System.Drawing.Size(217, 22);
            this.menuStripShowActiveMouse.Text = "Show Active + Mouse Over";
            this.menuStripShowActiveMouse.Click += new System.EventHandler(this.menuStripShowActiveMouse_Click);
            // 
            // menuStripShowMultipleTabs
            // 
            this.menuStripShowMultipleTabs.Name = "menuStripShowMultipleTabs";
            this.menuStripShowMultipleTabs.Size = new System.Drawing.Size(217, 22);
            this.menuStripShowMultipleTabs.Text = "Show Only Multiple Tabs";
            this.menuStripShowMultipleTabs.Click += new System.EventHandler(this.menuStripShowMultipleTabs_Click);
            // 
            // menuStripFeedback
            // 
            this.menuStripFeedback.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStripFocusRectangles,
            this.menuStripSemiTransparent,
            this.menuStripDiamondIndicators,
            this.menuStripSquareIndicators});
            this.menuStripFeedback.Name = "menuStripFeedback";
            this.menuStripFeedback.Size = new System.Drawing.Size(69, 20);
            this.menuStripFeedback.Text = "Feedback";
            // 
            // menuStripFocusRectangles
            // 
            this.menuStripFocusRectangles.Name = "menuStripFocusRectangles";
            this.menuStripFocusRectangles.Size = new System.Drawing.Size(202, 22);
            this.menuStripFocusRectangles.Text = "Focus Rectangles";
            this.menuStripFocusRectangles.Click += new System.EventHandler(this.menuStripFocusRectangles_Click);
            // 
            // menuStripSemiTransparent
            // 
            this.menuStripSemiTransparent.Name = "menuStripSemiTransparent";
            this.menuStripSemiTransparent.Size = new System.Drawing.Size(202, 22);
            this.menuStripSemiTransparent.Text = "Semi-transparent Blocks";
            this.menuStripSemiTransparent.Click += new System.EventHandler(this.menuStripSemiTransparent_Click);
            // 
            // menuStripDiamondIndicators
            // 
            this.menuStripDiamondIndicators.Name = "menuStripDiamondIndicators";
            this.menuStripDiamondIndicators.Size = new System.Drawing.Size(202, 22);
            this.menuStripDiamondIndicators.Text = "Diamond Indicators";
            this.menuStripDiamondIndicators.Click += new System.EventHandler(this.menuStripDiamondIndicators_Click);
            // 
            // menuStripSquareIndicators
            // 
            this.menuStripSquareIndicators.Checked = true;
            this.menuStripSquareIndicators.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuStripSquareIndicators.Name = "menuStripSquareIndicators";
            this.menuStripSquareIndicators.Size = new System.Drawing.Size(202, 22);
            this.menuStripSquareIndicators.Text = "Square Indicators";
            this.menuStripSquareIndicators.Click += new System.EventHandler(this.menuStripSquareIndicators_Click);
            // 
            // menuStripActions
            // 
            this.menuStripActions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStripRebalance,
            this.toolStripMenuItem3,
            this.menuStripProminent,
            this.menuStripProminentGroupOnly,
            this.toolStripMenuItem4,
            this.menuStripResizeThin,
            this.menuStripResizeMedium,
            this.menuStripResizeThick,
            this.toolStripMenuItem5,
            this.menuStripResizeLock,
            this.menuStripLayoutLock});
            this.menuStripActions.Name = "menuStripActions";
            this.menuStripActions.Size = new System.Drawing.Size(59, 20);
            this.menuStripActions.Text = "Actions";
            // 
            // menuStripRebalance
            // 
            this.menuStripRebalance.Name = "menuStripRebalance";
            this.menuStripRebalance.Size = new System.Drawing.Size(255, 22);
            this.menuStripRebalance.Text = "Rebalance";
            this.menuStripRebalance.Click += new System.EventHandler(this.menuStripRebalance_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(252, 6);
            // 
            // menuStripProminent
            // 
            this.menuStripProminent.Name = "menuStripProminent";
            this.menuStripProminent.Size = new System.Drawing.Size(255, 22);
            this.menuStripProminent.Text = "Prominent";
            this.menuStripProminent.Click += new System.EventHandler(this.menuStripProminent_Click);
            // 
            // menuStripProminentGroupOnly
            // 
            this.menuStripProminentGroupOnly.CheckOnClick = true;
            this.menuStripProminentGroupOnly.Name = "menuStripProminentGroupOnly";
            this.menuStripProminentGroupOnly.Size = new System.Drawing.Size(255, 22);
            this.menuStripProminentGroupOnly.Text = "Prominent Group Only";
            this.menuStripProminentGroupOnly.Click += new System.EventHandler(this.menuStripProminentGroupOnly_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(252, 6);
            // 
            // menuStripResizeThin
            // 
            this.menuStripResizeThin.Name = "menuStripResizeThin";
            this.menuStripResizeThin.Size = new System.Drawing.Size(255, 22);
            this.menuStripResizeThin.Text = "Make Resize Bar Thin (2 Pixels)";
            this.menuStripResizeThin.Click += new System.EventHandler(this.menuStripResizeThin_Click);
            // 
            // menuStripResizeMedium
            // 
            this.menuStripResizeMedium.Name = "menuStripResizeMedium";
            this.menuStripResizeMedium.Size = new System.Drawing.Size(255, 22);
            this.menuStripResizeMedium.Text = "Make Resize Bar Medium (5 Pixels)";
            this.menuStripResizeMedium.Click += new System.EventHandler(this.menuStripResizeMedium_Click);
            // 
            // menuStripResizeThick
            // 
            this.menuStripResizeThick.Name = "menuStripResizeThick";
            this.menuStripResizeThick.Size = new System.Drawing.Size(255, 22);
            this.menuStripResizeThick.Text = "Make Resize Bar Thick (10 Pixels)";
            this.menuStripResizeThick.Click += new System.EventHandler(this.menuStripResizeThick_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(252, 6);
            // 
            // menuStripResizeLock
            // 
            this.menuStripResizeLock.CheckOnClick = true;
            this.menuStripResizeLock.Name = "menuStripResizeLock";
            this.menuStripResizeLock.Size = new System.Drawing.Size(255, 22);
            this.menuStripResizeLock.Text = "ResizeLock";
            this.menuStripResizeLock.Click += new System.EventHandler(this.menuStripResizeLock_Click);
            // 
            // menuStripLayoutLock
            // 
            this.menuStripLayoutLock.CheckOnClick = true;
            this.menuStripLayoutLock.Name = "menuStripLayoutLock";
            this.menuStripLayoutLock.Size = new System.Drawing.Size(255, 22);
            this.menuStripLayoutLock.Text = "LayoutLock";
            this.menuStripLayoutLock.Click += new System.EventHandler(this.menuStripLayoutLock_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripOffice2007Blue,
            this.toolStripOffice2007Silver,
            this.toolStripOffice2007Black,
            this.toolStripSeparator2,
            this.toolStripMediaBlue,
            this.toolStripMediaOrange,
            this.toolStripMediaPurple,
            this.toolStripSeparator3,
            this.toolStripOffice2003,
            this.toolStripIDE2005,
            this.toolStripPlain});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(634, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripOffice2007Blue
            // 
            this.toolStripOffice2007Blue.Checked = true;
            this.toolStripOffice2007Blue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripOffice2007Blue.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripOffice2007Blue.Image = ((System.Drawing.Image)(resources.GetObject("toolStripOffice2007Blue.Image")));
            this.toolStripOffice2007Blue.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripOffice2007Blue.Name = "toolStripOffice2007Blue";
            this.toolStripOffice2007Blue.Size = new System.Drawing.Size(69, 22);
            this.toolStripOffice2007Blue.Text = "Office Blue";
            this.toolStripOffice2007Blue.Click += new System.EventHandler(this.toolStripOffice2007Blue_Click);
            // 
            // toolStripOffice2007Silver
            // 
            this.toolStripOffice2007Silver.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripOffice2007Silver.Image = ((System.Drawing.Image)(resources.GetObject("toolStripOffice2007Silver.Image")));
            this.toolStripOffice2007Silver.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripOffice2007Silver.Name = "toolStripOffice2007Silver";
            this.toolStripOffice2007Silver.Size = new System.Drawing.Size(74, 22);
            this.toolStripOffice2007Silver.Text = "Office Silver";
            this.toolStripOffice2007Silver.Click += new System.EventHandler(this.toolStripOffice2007Silver_Click);
            // 
            // toolStripOffice2007Black
            // 
            this.toolStripOffice2007Black.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripOffice2007Black.Image = ((System.Drawing.Image)(resources.GetObject("toolStripOffice2007Black.Image")));
            this.toolStripOffice2007Black.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripOffice2007Black.Name = "toolStripOffice2007Black";
            this.toolStripOffice2007Black.Size = new System.Drawing.Size(74, 22);
            this.toolStripOffice2007Black.Text = "Office Black";
            this.toolStripOffice2007Black.Click += new System.EventHandler(this.toolStripOffice2007Black_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripMediaBlue
            // 
            this.toolStripMediaBlue.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMediaBlue.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMediaBlue.Image")));
            this.toolStripMediaBlue.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMediaBlue.Name = "toolStripMediaBlue";
            this.toolStripMediaBlue.Size = new System.Drawing.Size(70, 22);
            this.toolStripMediaBlue.Text = "Media Blue";
            this.toolStripMediaBlue.Click += new System.EventHandler(this.toolStripMediaBlue_Click);
            // 
            // toolStripMediaOrange
            // 
            this.toolStripMediaOrange.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMediaOrange.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMediaOrange.Image")));
            this.toolStripMediaOrange.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMediaOrange.Name = "toolStripMediaOrange";
            this.toolStripMediaOrange.Size = new System.Drawing.Size(86, 22);
            this.toolStripMediaOrange.Text = "Media Orange";
            this.toolStripMediaOrange.Click += new System.EventHandler(this.toolStripMediaOrange_Click);
            // 
            // toolStripMediaPurple
            // 
            this.toolStripMediaPurple.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMediaPurple.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMediaPurple.Image")));
            this.toolStripMediaPurple.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMediaPurple.Name = "toolStripMediaPurple";
            this.toolStripMediaPurple.Size = new System.Drawing.Size(81, 22);
            this.toolStripMediaPurple.Text = "Media Purple";
            this.toolStripMediaPurple.Click += new System.EventHandler(this.toolStripMediaPurple_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripOffice2003
            // 
            this.toolStripOffice2003.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripOffice2003.Image = ((System.Drawing.Image)(resources.GetObject("toolStripOffice2003.Image")));
            this.toolStripOffice2003.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripOffice2003.Name = "toolStripOffice2003";
            this.toolStripOffice2003.Size = new System.Drawing.Size(67, 22);
            this.toolStripOffice2003.Text = "Office2003";
            this.toolStripOffice2003.Click += new System.EventHandler(this.toolStripOffice2003_Click);
            // 
            // toolStripIDE2005
            // 
            this.toolStripIDE2005.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripIDE2005.Image = ((System.Drawing.Image)(resources.GetObject("toolStripIDE2005.Image")));
            this.toolStripIDE2005.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripIDE2005.Name = "toolStripIDE2005";
            this.toolStripIDE2005.Size = new System.Drawing.Size(52, 22);
            this.toolStripIDE2005.Text = "IDE2005";
            this.toolStripIDE2005.Click += new System.EventHandler(this.toolStrip2005_Click);
            // 
            // toolStripPlain
            // 
            this.toolStripPlain.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripPlain.Image = ((System.Drawing.Image)(resources.GetObject("toolStripPlain.Image")));
            this.toolStripPlain.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripPlain.Name = "toolStripPlain";
            this.toolStripPlain.Size = new System.Drawing.Size(37, 22);
            this.toolStripPlain.Text = "Plain";
            this.toolStripPlain.Click += new System.EventHandler(this.toolStripPlain_Click);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.tabbedGroups1);
            this.toolStripContainer1.ContentPanel.Padding = new System.Windows.Forms.Padding(5);
            this.toolStripContainer1.ContentPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(734, 416);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 24);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(734, 466);
            this.toolStripContainer1.TabIndex = 6;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip2);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripAddPage,
            this.toolStripRemovePage});
            this.toolStrip2.Location = new System.Drawing.Point(3, 25);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(188, 25);
            this.toolStrip2.TabIndex = 6;
            // 
            // toolStripAddPage
            // 
            this.toolStripAddPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripAddPage.Image = ((System.Drawing.Image)(resources.GetObject("toolStripAddPage.Image")));
            this.toolStripAddPage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripAddPage.Name = "toolStripAddPage";
            this.toolStripAddPage.Size = new System.Drawing.Size(62, 22);
            this.toolStripAddPage.Text = "Add Page";
            this.toolStripAddPage.Click += new System.EventHandler(this.menuStripAddPage_Click);
            // 
            // toolStripRemovePage
            // 
            this.toolStripRemovePage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripRemovePage.Image = ((System.Drawing.Image)(resources.GetObject("toolStripRemovePage.Image")));
            this.toolStripRemovePage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripRemovePage.Name = "toolStripRemovePage";
            this.toolStripRemovePage.Size = new System.Drawing.Size(83, 22);
            this.toolStripRemovePage.Text = "Remove Page";
            this.toolStripRemovePage.Click += new System.EventHandler(this.menuStripRemovePage_Click);
            // 
            // SampleTabbedGroups
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(734, 514);
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.statusBarControl1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SampleTabbedGroups";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DotNetMagic - SampleTabbedGroups";
            ((System.ComponentModel.ISupportInitialize)(this.tabbedGroups1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
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
            Application.Run(new SampleTabbedGroups());
		}

		private int NextImage()
		{
			_image = ++_image % 7;
			return _image;
		}
		
		private void ChangeStyle(VisualStyle style)
		{
            // Update the DotNetMagicForm of new style
            Style = style;

			// Update each of the contained elements
			statusBarControl1.Style = style;
			tabbedGroups1.Style = style;
			
			TabGroupLeaf leaf = tabbedGroups1.FirstLeaf();
			while(leaf != null)
			{
				leaf.TabControl.ShowArrows = (style != VisualStyle.IDE2005);
				leaf.TabControl.ShowDropSelect = (style == VisualStyle.IDE2005); 
				leaf = tabbedGroups1.NextLeaf(leaf);
			}

            switch (style)
            {
                case VisualStyle.Office2007Blue:
                    menuStripOffice2007Blue.Checked = toolStripOffice2007Blue.Checked = true;
                    menuStripOffice2007Silver.Checked = toolStripOffice2007Silver.Checked = false;
                    menuStripOffice2007Black.Checked = toolStripOffice2007Black.Checked = false;
                    menuStripMediaPlayerBlue.Checked = toolStripMediaBlue.Checked = false;
                    menuStripMediaPlayerOrange.Checked = toolStripMediaOrange.Checked = false;
                    menuStripMediaPlayerPurple.Checked = toolStripMediaPurple.Checked = false;
                    menuStripOffice2003.Checked = toolStripOffice2003.Checked = false;
                    menuStripIDE2005.Checked = toolStripIDE2005.Checked = false;
                    menuStripPlain.Checked = toolStripPlain.Checked = false;
                    break;
                case VisualStyle.Office2007Silver:
                    menuStripOffice2007Blue.Checked = toolStripOffice2007Blue.Checked = false;
                    menuStripOffice2007Silver.Checked = toolStripOffice2007Silver.Checked = true;
                    menuStripOffice2007Black.Checked = toolStripOffice2007Black.Checked = false;
                    menuStripMediaPlayerBlue.Checked = toolStripMediaBlue.Checked = false;
                    menuStripMediaPlayerOrange.Checked = toolStripMediaOrange.Checked = false;
                    menuStripMediaPlayerPurple.Checked = toolStripMediaPurple.Checked = false;
                    menuStripOffice2003.Checked = toolStripOffice2003.Checked = false;
                    menuStripIDE2005.Checked = toolStripIDE2005.Checked = false;
                    menuStripPlain.Checked = toolStripPlain.Checked = false;
                    break;
                case VisualStyle.Office2007Black:
                    menuStripOffice2007Blue.Checked = toolStripOffice2007Blue.Checked = false;
                    menuStripOffice2007Silver.Checked = toolStripOffice2007Silver.Checked = false;
                    menuStripOffice2007Black.Checked = toolStripOffice2007Black.Checked = true;
                    menuStripMediaPlayerBlue.Checked = toolStripMediaBlue.Checked = false;
                    menuStripMediaPlayerOrange.Checked = toolStripMediaOrange.Checked = false;
                    menuStripMediaPlayerPurple.Checked = toolStripMediaPurple.Checked = false;
                    menuStripOffice2003.Checked = toolStripOffice2003.Checked = false;
                    menuStripIDE2005.Checked = toolStripIDE2005.Checked = false;
                    menuStripPlain.Checked = toolStripPlain.Checked = false;
                    break;
                case VisualStyle.MediaPlayerBlue:
                    menuStripOffice2007Blue.Checked = toolStripOffice2007Blue.Checked = false;
                    menuStripOffice2007Silver.Checked = toolStripOffice2007Silver.Checked = false;
                    menuStripOffice2007Black.Checked = toolStripOffice2007Black.Checked = false;
                    menuStripMediaPlayerBlue.Checked = toolStripMediaBlue.Checked = true;
                    menuStripMediaPlayerOrange.Checked = toolStripMediaOrange.Checked = false;
                    menuStripMediaPlayerPurple.Checked = toolStripMediaPurple.Checked = false;
                    menuStripOffice2003.Checked = toolStripOffice2003.Checked = false;
                    menuStripIDE2005.Checked = toolStripIDE2005.Checked = false;
                    menuStripPlain.Checked = toolStripPlain.Checked = false;
                    break;
                case VisualStyle.MediaPlayerOrange:
                    menuStripOffice2007Blue.Checked = toolStripOffice2007Blue.Checked = false;
                    menuStripOffice2007Silver.Checked = toolStripOffice2007Silver.Checked = false;
                    menuStripOffice2007Black.Checked = toolStripOffice2007Black.Checked = false;
                    menuStripMediaPlayerBlue.Checked = toolStripMediaBlue.Checked = false;
                    menuStripMediaPlayerOrange.Checked = toolStripMediaOrange.Checked = true;
                    menuStripMediaPlayerPurple.Checked = toolStripMediaPurple.Checked = false;
                    menuStripOffice2003.Checked = toolStripOffice2003.Checked = false;
                    menuStripIDE2005.Checked = toolStripIDE2005.Checked = false;
                    menuStripPlain.Checked = toolStripPlain.Checked = false;
                    break;
                case VisualStyle.MediaPlayerPurple:
                    menuStripOffice2007Blue.Checked = toolStripOffice2007Blue.Checked = false;
                    menuStripOffice2007Silver.Checked = toolStripOffice2007Silver.Checked = false;
                    menuStripOffice2007Black.Checked = toolStripOffice2007Black.Checked = false;
                    menuStripMediaPlayerBlue.Checked = toolStripMediaBlue.Checked = false;
                    menuStripMediaPlayerOrange.Checked = toolStripMediaOrange.Checked = false;
                    menuStripMediaPlayerPurple.Checked = toolStripMediaPurple.Checked = true;
                    menuStripOffice2003.Checked = toolStripOffice2003.Checked = false;
                    menuStripIDE2005.Checked = toolStripIDE2005.Checked = false;
                    menuStripPlain.Checked = toolStripPlain.Checked = false;
                    break;
                case VisualStyle.Office2003:
                    menuStripOffice2007Blue.Checked = toolStripOffice2007Blue.Checked = false;
                    menuStripOffice2007Silver.Checked = toolStripOffice2007Silver.Checked = false;
                    menuStripOffice2007Black.Checked = toolStripOffice2007Black.Checked = false;
                    menuStripMediaPlayerBlue.Checked = toolStripMediaBlue.Checked = false;
                    menuStripMediaPlayerOrange.Checked = toolStripMediaOrange.Checked = false;
                    menuStripMediaPlayerPurple.Checked = toolStripMediaPurple.Checked = false;
                    menuStripOffice2003.Checked = toolStripOffice2003.Checked = true;
                    menuStripIDE2005.Checked = toolStripIDE2005.Checked = false;
                    menuStripPlain.Checked = toolStripPlain.Checked = false;
                    break;
                case VisualStyle.IDE2005:
                    menuStripOffice2007Blue.Checked = toolStripOffice2007Blue.Checked = false;
                    menuStripOffice2007Silver.Checked = toolStripOffice2007Silver.Checked = false;
                    menuStripOffice2007Black.Checked = toolStripOffice2007Black.Checked = false;
                    menuStripMediaPlayerBlue.Checked = toolStripMediaBlue.Checked = false;
                    menuStripMediaPlayerOrange.Checked = toolStripMediaOrange.Checked = false;
                    menuStripMediaPlayerPurple.Checked = toolStripMediaPurple.Checked = false;
                    menuStripOffice2003.Checked = toolStripOffice2003.Checked = false;
                    menuStripIDE2005.Checked = toolStripIDE2005.Checked = true;
                    menuStripPlain.Checked = toolStripPlain.Checked = false;
                    break;
                case VisualStyle.Plain:
                    menuStripOffice2007Blue.Checked = toolStripOffice2007Blue.Checked = false;
                    menuStripOffice2007Silver.Checked = toolStripOffice2007Silver.Checked = false;
                    menuStripOffice2007Black.Checked = toolStripOffice2007Black.Checked = false;
                    menuStripMediaPlayerBlue.Checked = toolStripMediaBlue.Checked = false;
                    menuStripMediaPlayerOrange.Checked = toolStripMediaOrange.Checked = false;
                    menuStripMediaPlayerPurple.Checked = toolStripMediaPurple.Checked = false;
                    menuStripOffice2003.Checked = toolStripOffice2003.Checked = false;
                    menuStripIDE2005.Checked = toolStripIDE2005.Checked = false;
                    menuStripPlain.Checked = toolStripPlain.Checked = true;
                    break;
            }
        }

		private Crownwood.DotNetMagic.Controls.TabPage NewTabPage()
		{
			RichTextBox rtb = new RichTextBox();
			rtb.BorderStyle = BorderStyle.None;

			Crownwood.DotNetMagic.Controls.TabPage newPage = new Crownwood.DotNetMagic.Controls.TabPage("Page" + _count++, rtb, NextImage());
			newPage.Selected = true;
			
			return newPage;       
		}

		private void OnExternalDrop(Crownwood.DotNetMagic.Controls.TabbedGroups tg, 
									Crownwood.DotNetMagic.Controls.TabGroupLeaf tgl,
									Crownwood.DotNetMagic.Controls.TabControl tc,
									Crownwood.DotNetMagic.Controls.TabbedGroups.DragProvider dp)
        {
            // Create a new tab page
            Crownwood.DotNetMagic.Controls.TabPage tp = NewTabPage();
            
            // Define the text in this control
            (tp.Control as RichTextBox).Text = "Dragged from node '" + (string)dp.Tag + "'";
            
            // We want the new page to become selected
            tp.Selected = true;
            
            // Add new page into the destination tab control
            tgl.TabPages.Add(tp);
        }

		private void OnTabControlCreated(Crownwood.DotNetMagic.Controls.TabbedGroups tg, Crownwood.DotNetMagic.Controls.TabControl tc)
		{
			// Place a thin border between edge of the tab control and inside contents
			tc.ControlTopOffset = 3;			
			tc.ControlBottomOffset = 3;			
			tc.ControlLeftOffset = 3;			
			tc.ControlRightOffset = 3;		
		}
		
		private void timer_Tick(object sender, System.EventArgs e)
		{
			// Update the date and time section of the status bar
			statusPanelDate.Text = DateTime.Now.ToShortDateString();
		}

        private void menuStripExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void toolStripOffice2007Black_Click(object sender, EventArgs e)
        {
            ChangeStyle(VisualStyle.Office2007Black);
        }

        private void toolStripOffice2007Silver_Click(object sender, EventArgs e)
        {
            ChangeStyle(VisualStyle.Office2007Silver);
        }

        private void toolStripOffice2007Blue_Click(object sender, EventArgs e)
        {
            ChangeStyle(VisualStyle.Office2007Blue);
        }

        private void menuStripMediaPlayerBlue_Click(object sender, EventArgs e)
        {
            ChangeStyle(VisualStyle.MediaPlayerBlue);
        }

        private void menuStripMediaPlayerOrange_Click(object sender, EventArgs e)
        {
            ChangeStyle(VisualStyle.MediaPlayerOrange);
        }

        private void menuStripMediaPlayerPurple_Click(object sender, EventArgs e)
        {
            ChangeStyle(VisualStyle.MediaPlayerPurple);
        }

        private void toolStripOffice2003_Click(object sender, EventArgs e)
        {
            ChangeStyle(VisualStyle.Office2003);
        }

        private void toolStrip2005_Click(object sender, EventArgs e)
        {
            ChangeStyle(VisualStyle.IDE2005);
        }

        private void toolStripPlain_Click(object sender, EventArgs e)
        {
            ChangeStyle(VisualStyle.Plain);
        }

        private void menuStripOffice2007Black_Click(object sender, EventArgs e)
        {
            ChangeStyle(VisualStyle.Office2007Black);
        }

        private void menuStripOffice2007Silver_Click(object sender, EventArgs e)
        {
            ChangeStyle(VisualStyle.Office2007Silver);
        }

        private void toolStripMediaBlue_Click(object sender, EventArgs e)
        {
            ChangeStyle(VisualStyle.MediaPlayerBlue);
        }

        private void toolStripMediaOrange_Click(object sender, EventArgs e)
        {
            ChangeStyle(VisualStyle.MediaPlayerOrange);
        }

        private void toolStripMediaPurple_Click(object sender, EventArgs e)
        {
            ChangeStyle(VisualStyle.MediaPlayerPurple);
        }

        private void menuStripOffice2007Blue_Click(object sender, EventArgs e)
        {
            ChangeStyle(VisualStyle.Office2007Blue);
        }

        private void menuStripOffice2003_Click(object sender, EventArgs e)
        {
            ChangeStyle(VisualStyle.Office2003);
        }

        private void menuStripIDE2005_Click(object sender, EventArgs e)
        {
            ChangeStyle(VisualStyle.IDE2005);
        }

        private void menuStripPlain_Click(object sender, EventArgs e)
        {
            ChangeStyle(VisualStyle.Plain);
        }

        private void toolStripAddPage_Click(object sender, EventArgs e)
        {
            if (tabbedGroups1.ActiveLeaf != null)
                tabbedGroups1.ActiveLeaf.TabPages.Add(NewTabPage());
        }

        private void toolStripRemovePage_Click(object sender, EventArgs e)
        {
            Crownwood.DotNetMagic.Controls.TabControl tc = null;

            if (tabbedGroups1.ActiveLeaf != null)
                tc = tabbedGroups1.ActiveLeaf.GroupControl as Crownwood.DotNetMagic.Controls.TabControl;

            // Did we find a current tab control?
            if (tc != null)
            {
                // Does it have a selected tab?
                if (tc.SelectedTab != null)
                {
                    // Remove the page
                    tc.TabPages.Remove(tc.SelectedTab);
                }
            }
        }

        private void menuStripAddPage_Click(object sender, EventArgs e)
        {
            toolStripAddPage_Click(sender, e);
        }

        private void menuStripRemovePage_Click(object sender, EventArgs e)
        {
            toolStripRemovePage_Click(sender, e);
        }

        private void menuStripInitializeSimple_Click(object sender, EventArgs e)
        {
            // Remove all existing content
            tabbedGroups1.RootSequence.Clear();

            // Switch to ordering horizontal
            tabbedGroups1.RootDirection = LayoutDirection.Horizontal;

            // Reset count back again
            _count = 1;
            _image = -1;

            // Access the default leaf group
            TabGroupLeaf tgl1 = tabbedGroups1.RootSequence[0] as TabGroupLeaf;

            // Add a new leaf group in the same sequence
            TabGroupLeaf tgl2 = tabbedGroups1.RootSequence.AddNewLeaf();

            // Add a two pages to the leaf
            tgl1.TabPages.Add(NewTabPage());
            tgl2.TabPages.Add(NewTabPage());

            // Setup spacing
            tgl1.Space = 60;
            tgl2.Space = 40;

            // Ask control to reposition children according to new spacing
            tabbedGroups1.RootSequence.Reposition();
        }

        private void menuStripInitializeMedium_Click(object sender, EventArgs e)
        {
            // Remove all existing content
            tabbedGroups1.RootSequence.Clear();

            // Switch to ordering horizontal
            tabbedGroups1.RootDirection = LayoutDirection.Horizontal;

            // Reset count back again
            _count = 0;
            _image = -1;

            // Access the default leaf group
            TabGroupLeaf tgl1 = tabbedGroups1.RootSequence[0] as TabGroupLeaf;

            // Add a new sequence to end of root
            TabGroupSequence tgs = tabbedGroups1.RootSequence.AddNewSequence();

            // Add two leafs into the new sequence
            TabGroupLeaf tgl2 = tgs.AddNewLeaf() as TabGroupLeaf;
            TabGroupLeaf tgl3 = tgs.AddNewLeaf() as TabGroupLeaf;

            // Add a page two each tab leaf
            tgl1.TabPages.Add(NewTabPage());
            tgl2.TabPages.Add(NewTabPage());
            tgl3.TabPages.Add(NewTabPage());

            // Setup spacing
            tgl1.Space = 35;
            tgs.Space = 65;
            tgl2.Space = 30;
            tgl3.Space = 70;

            // Ask control to reposition children according to new spacing
            tabbedGroups1.RootSequence.Reposition();
        }

        private void menuStripInitalizeComplex_Click(object sender, EventArgs e)
        {
            // Remove all existing content
            tabbedGroups1.RootSequence.Clear();

            // Reset count back again
            _count = 0;
            _image = -1;

            // Switch to ordering vertical
            tabbedGroups1.RootDirection = LayoutDirection.Vertical;

            // Access the default leaf group
            TabGroupLeaf tgl1 = tabbedGroups1.RootSequence[0] as TabGroupLeaf;

            // Add a new sequence to end of root
            TabGroupSequence tgs1 = tabbedGroups1.RootSequence.AddNewSequence();
            TabGroupSequence tgs2 = tabbedGroups1.RootSequence.AddNewSequence();

            // Add two leafs into the first sequence
            TabGroupLeaf tgl2 = tgs1.AddNewLeaf() as TabGroupLeaf;
            TabGroupLeaf tgl3 = tgs1.AddNewLeaf() as TabGroupLeaf;

            // Add three leafs into the second sequence
            TabGroupLeaf tgl4 = tgs2.AddNewLeaf() as TabGroupLeaf;
            TabGroupLeaf tgl5 = tgs2.AddNewLeaf() as TabGroupLeaf;
            TabGroupLeaf tgl6 = tgs2.AddNewLeaf() as TabGroupLeaf;

            // Add a page two each tab leaf and two pages to some of them
            tgl1.TabPages.Add(NewTabPage());
            tgl2.TabPages.Add(NewTabPage());
            tgl2.TabPages.Add(NewTabPage());
            tgl3.TabPages.Add(NewTabPage());
            tgl3.TabPages.Add(NewTabPage());
            tgl4.TabPages.Add(NewTabPage());
            tgl5.TabPages.Add(NewTabPage());
            tgl6.TabPages.Add(NewTabPage());

            // Setup spacing
            tgl1.Space = 20;
            tgs1.Space = 30;
            tgs2.Space = 50;

            // Ask control to reposition children according to new spacing
            tabbedGroups1.RootSequence.Reposition();
        }

        private void UpdateHeaderMode()
        {
            menuStripHideAll.Checked = false;
            menuStripShowAll.Checked = false;
            menuStripShowActiveLeaf.Checked = false;
            menuStripShowMouseOver.Checked = false;
            menuStripShowActiveMouse.Checked = false;
            menuStripShowMultipleTabs.Checked = false;

            switch (tabbedGroups1.DisplayTabMode)
            {
                case DisplayTabModes.HideAll:
                    menuStripHideAll.Checked = true;
                    break;
                case DisplayTabModes.ShowAll:
                    menuStripShowAll.Checked = true;
                    break;
                case DisplayTabModes.ShowActiveLeaf:
                    menuStripShowActiveLeaf.Checked = true;
                    break;
                case DisplayTabModes.ShowMouseOver:
                    menuStripShowMouseOver.Checked = true;
                    break;
                case DisplayTabModes.ShowActiveAndMouseOver:
                    menuStripShowActiveMouse.Checked = true;
                    break;
                case DisplayTabModes.ShowOnlyMultipleTabs:
                    menuStripShowMultipleTabs.Checked = true;
                    break;
            }
        }

        private void menuStripHideAll_Click(object sender, EventArgs e)
        {
            tabbedGroups1.DisplayTabMode = DisplayTabModes.HideAll;
            UpdateHeaderMode();
        }

        private void menuStripShowAll_Click(object sender, EventArgs e)
        {
            tabbedGroups1.DisplayTabMode = DisplayTabModes.ShowAll;
            UpdateHeaderMode();
        }

        private void menuStripShowActiveLeaf_Click(object sender, EventArgs e)
        {
            tabbedGroups1.DisplayTabMode = DisplayTabModes.ShowActiveLeaf;
            UpdateHeaderMode();
        }

        private void menuStripShowMouseOver_Click(object sender, EventArgs e)
        {
            tabbedGroups1.DisplayTabMode = DisplayTabModes.ShowMouseOver;
            UpdateHeaderMode();
        }

        private void menuStripShowActiveMouse_Click(object sender, EventArgs e)
        {
            tabbedGroups1.DisplayTabMode = DisplayTabModes.ShowActiveAndMouseOver;
            UpdateHeaderMode();
        }

        private void menuStripShowMultipleTabs_Click(object sender, EventArgs e)
        {
            tabbedGroups1.DisplayTabMode = DisplayTabModes.ShowOnlyMultipleTabs;
            UpdateHeaderMode();
        }

        private void UpdateFeedback()
        {
            menuStripFocusRectangles.Checked = false;
            menuStripSemiTransparent.Checked = false;
            menuStripDiamondIndicators.Checked = false;
            menuStripSquareIndicators.Checked = false;

            switch (tabbedGroups1.FeedbackStyle)
            {
                case DragFeedbackStyle.Outline:
                    menuStripFocusRectangles.Checked = true;
                    break;
                case DragFeedbackStyle.Solid:
                    menuStripSemiTransparent.Checked = true;
                    break;
                case DragFeedbackStyle.Diamonds:
                    menuStripDiamondIndicators.Checked = true;
                    break;
                case DragFeedbackStyle.Squares:
                    menuStripSquareIndicators.Checked = true;
                    break;
            }
        }

        private void menuStripFocusRectangles_Click(object sender, EventArgs e)
        {
            tabbedGroups1.FeedbackStyle = DragFeedbackStyle.Outline;
            UpdateFeedback();
        }

        private void menuStripSemiTransparent_Click(object sender, EventArgs e)
        {
			tabbedGroups1.FeedbackStyle = DragFeedbackStyle.Solid;
            UpdateFeedback();
        }

        private void menuStripDiamondIndicators_Click(object sender, EventArgs e)
        {
			tabbedGroups1.FeedbackStyle = DragFeedbackStyle.Diamonds;
            UpdateFeedback();
        }

        private void menuStripSquareIndicators_Click(object sender, EventArgs e)
        {
            tabbedGroups1.FeedbackStyle = DragFeedbackStyle.Squares;
            UpdateFeedback();
        }

        private void menuStripRebalance_Click(object sender, EventArgs e)
        {
            tabbedGroups1.Rebalance();
        }

        private void menuStripProminent_Click(object sender, EventArgs e)
        {
            if (tabbedGroups1.ProminentLeaf != null)
                tabbedGroups1.ProminentLeaf = null;
            else
            {
                if (tabbedGroups1.ActiveLeaf != null)
                    tabbedGroups1.ProminentLeaf = tabbedGroups1.ActiveLeaf;
            }
        }

        private void menuStripProminentGroupOnly_Click(object sender, EventArgs e)
        {
            tabbedGroups1.ProminentOnly = !tabbedGroups1.ProminentOnly;
        }

        private void menuStripResizeThin_Click(object sender, EventArgs e)
        {
            tabbedGroups1.ResizeBarVector = 2;
        }

        private void menuStripResizeMedium_Click(object sender, EventArgs e)
        {
            tabbedGroups1.ResizeBarVector = 5;
        }

        private void menuStripResizeThick_Click(object sender, EventArgs e)
        {
            tabbedGroups1.ResizeBarVector = 10;
        }

        private void menuStripResizeLock_Click(object sender, EventArgs e)
        {
            tabbedGroups1.ResizeBarLock = !tabbedGroups1.ResizeBarLock;
        }

        private void menuStripLayoutLock_Click(object sender, EventArgs e)
        {
            tabbedGroups1.LayoutLock = !tabbedGroups1.LayoutLock;
        }
	}
}
