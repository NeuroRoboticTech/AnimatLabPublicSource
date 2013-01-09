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
using System.Xml;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Crownwood.DotNetMagic.Common;
using Crownwood.DotNetMagic.Docking;
using Crownwood.DotNetMagic.Controls;
using Crownwood.DotNetMagic.Forms;

namespace SampleDocking
{
	/// <summary>
	/// Sample form showing how to use the docking system.
	/// </summary>
	public class SampleDockingForm : DotNetMagicForm
    {
        #region Instance Fields
        private int count = 0;
		private ImageList imageList;
		private DockingManager dockingManager;
		private byte[] slot1;
		private byte[] slot2;
		private bool allowContextMenu;
		private bool customContextMenu;
		private bool captionBars;
		private bool closeButtons;
		private int ignoreClose;
		private ArrayList _treeControls;
		private ArrayList _exampleForms;
        #endregion

        #region Designer Generated
        private MenuStrip menuStrip;
        private ToolStripMenuItem createToolStripMenuItem;
        private ToolStripMenuItem feedbackToolStripMenuItem;
        private ToolStripMenuItem manageToolStripMenuItem;
        private ToolStripMenuItem persistenceToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStrip toolStripOffice2007;
        private ToolStripContainer toolStripContainer;
        private Crownwood.DotNetMagic.Controls.TabControl tabControl;
        private Crownwood.DotNetMagic.Controls.TabPage tabPage1;
        private Crownwood.DotNetMagic.Controls.TabPage tabPage2;
        private Crownwood.DotNetMagic.Controls.TabPage tabPage3;
        private StatusStrip statusStrip1;
        private ToolStripMenuItem stylesToolStripMenuItem;
        private ToolStripMenuItem menuOffice2003;
        private ToolStripMenuItem menuIDE2005;
        private ToolStripMenuItem menuPlain;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem menuExit;
        private ToolStripMenuItem menuCreateTreeControl;
        private ToolStripMenuItem menuCreateTextBox;
        private ToolStripMenuItem menuCreateExampleForm;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem menuCreate3InRow;
        private ToolStripMenuItem menuCreate3InColumn;
        private ToolStripMenuItem menuCreate3InWindow;
        private ToolStripMenuItem menuCreate3AutoHidden;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripMenuItem menuCreate3Floating;
        private ToolStripMenuItem menuFocusRectangles;
        private ToolStripMenuItem menuSemitransparentBlocks;
        private ToolStripMenuItem menuDiamondIndicators;
        private ToolStripMenuItem menuSquareIndicators;
        private ToolStripMenuItem menuShowAll;
        private ToolStripMenuItem menuHideAll;
        private ToolStripSeparator toolStripMenuItem4;
        private ToolStripMenuItem menuAllowRedocking;
        private ToolStripMenuItem menuAllowFloating;
        private ToolStripSeparator toolStripMenuItem5;
        private ToolStripMenuItem menuDeleteAll;
        private ToolStripMenuItem menuSaveAsConfig1;
        private ToolStripMenuItem menuSaveAsConfig2;
        private ToolStripSeparator toolStripMenuItem6;
        private ToolStripMenuItem menuLoadFromConfig1;
        private ToolStripMenuItem menuLoadFromConfig2;
        private ToolStripSeparator toolStripMenuItem7;
        private ToolStripMenuItem menuSaveToByteArray1;
        private ToolStripMenuItem menuSaveToByteArray2;
        private ToolStripSeparator toolStripMenuItem8;
        private ToolStripMenuItem menuLoadFromByteArray1;
        private ToolStripMenuItem menuLoadFromByteArray2;
        private ToolStripMenuItem menuAllowContextMenu;
        private ToolStripMenuItem menuCustomizeContextMenu;
        private ToolStripSeparator toolStripMenuItem9;
        private ToolStripMenuItem menuDefaultResizeBar;
        private ToolStripMenuItem menu1PixelResizeBar;
        private ToolStripMenuItem menu5PixelResizeBar;
        private ToolStripMenuItem menu7PixelResizeBar;
        private ToolStripSeparator toolStripMenuItem10;
        private ToolStripMenuItem menuShowCaptionBars;
        private ToolStripMenuItem menuShowCloseButtons;
        private ToolStripSeparator toolStripMenuItem11;
        private ToolStripMenuItem menuAllowAllCloseButtons;
        private ToolStripMenuItem menuIgnoreTreeControlCloseButtons;
        private ToolStripMenuItem menuIgnoreTextBoxCloseButtons;
        private ToolStripMenuItem menuIgnoreExampleFormCloseButtons;
        private ToolStripMenuItem menuIgnoreAllCloseButtons;
        private ToolStripSeparator toolStripMenuItem12;
        private ToolStripMenuItem menuAllowMinMaxFunctionality;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private ToolStripMenuItem menuAllowResize;
        private ToolStripMenuItem menuOffice2007Blue;
        private ToolStripMenuItem menuOffice2007Silver;
        private ToolStripMenuItem menuOffice2007Black;
        private ToolStripButton toolOffice2007Blue;
        private ToolStripButton toolOffice2007Silver;
        private ToolStripButton toolOffice2007Black;
        private ToolStripMenuItem menuAllowTabbed;
        private ToolStripMenuItem menuMediaBlue;
        private ToolStripMenuItem menuMediaOrange;
        private ToolStripMenuItem menuMediaPurple;
        private ToolStrip toolStripVisible;
        private ToolStripButton toolStripShowAll;
        private ToolStripButton toolStripHideAll;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton toolMediaBlue;
        private ToolStripButton toolMediaOrange;
        private ToolStripButton toolMediaPurple;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripButton toolOffice2003;
        private ToolStripButton toolIDE2005;
        private ToolStripButton toolPlain;

        // Designer generated fields
		private System.ComponentModel.IContainer components;
        #endregion

        #region Construction / Dispose
        /// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SampleDockingForm());
		}

		public SampleDockingForm()
		{
			// Initialize instance fields
			allowContextMenu = true;
			customContextMenu = false;
			captionBars = true;
			closeButtons = true;
			ignoreClose = 0;

			// Required for Windows Form Designer support
			InitializeComponent();
			
			// Setup the docking manager instance
            dockingManager = new DockingManager(toolStripContainer.ContentPanel, VisualStyle.Office2007Blue);

			// Hook into the events generated by the docking manager
			dockingManager.ContextMenu += new DockingManager.ContextMenuHandler(OnContextMenu);
			dockingManager.ContentShown += new DockingManager.ContentHandler(OnContentShown);
			dockingManager.ContentHidden += new DockingManager.ContentHandler(OnContentHidden);
			dockingManager.ContentHiding += new DockingManager.ContentHidingHandler(OnContentHiding);
            dockingManager.ContentAutoHideOpening += new DockingManager.ContentHandler(OnContentAutoHideOpening);
            dockingManager.ContentAutoHideClosed += new DockingManager.ContentHandler(OnContentAutoHideClosed);
            dockingManager.LayoutChanged += new EventHandler(OnLayoutChanged);

			// Ensure correct inner and outer values for correct window positioning
			dockingManager.InnerControl = tabControl;

			// Setup custom config handling
			dockingManager.SaveCustomConfig += new DockingManager.SaveCustomConfigHandler(OnSaveConfig);
			dockingManager.LoadCustomConfig += new DockingManager.LoadCustomConfigHandler(OnLoadConfig);

			// Create the initial starting docking windows			
			menuCreateTreeControl_Click(this, EventArgs.Empty);
			menuCreateExampleForm_Click(this, EventArgs.Empty);
			menuCreate3AutoHidden_Click(this, EventArgs.Empty);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
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
        #endregion

        #region Windows Form Designer generated code
        /// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SampleDockingForm));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.stylesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOffice2007Blue = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOffice2007Silver = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOffice2007Black = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuMediaBlue = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMediaOrange = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMediaPurple = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuOffice2003 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuIDE2005 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPlain = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCreateTreeControl = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCreateTextBox = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCreateExampleForm = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuCreate3InRow = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCreate3InColumn = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCreate3InWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCreate3AutoHidden = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuCreate3Floating = new System.Windows.Forms.ToolStripMenuItem();
            this.feedbackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFocusRectangles = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSemitransparentBlocks = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDiamondIndicators = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSquareIndicators = new System.Windows.Forms.ToolStripMenuItem();
            this.manageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuShowAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHideAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuAllowRedocking = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAllowFloating = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAllowResize = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAllowTabbed = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuDeleteAll = new System.Windows.Forms.ToolStripMenuItem();
            this.persistenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveAsConfig1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveAsConfig2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.menuLoadFromConfig1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLoadFromConfig2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSaveToByteArray1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveToByteArray2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.menuLoadFromByteArray1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLoadFromByteArray2 = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAllowContextMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCustomizeContextMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.menuDefaultResizeBar = new System.Windows.Forms.ToolStripMenuItem();
            this.menu1PixelResizeBar = new System.Windows.Forms.ToolStripMenuItem();
            this.menu5PixelResizeBar = new System.Windows.Forms.ToolStripMenuItem();
            this.menu7PixelResizeBar = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
            this.menuShowCaptionBars = new System.Windows.Forms.ToolStripMenuItem();
            this.menuShowCloseButtons = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
            this.menuAllowAllCloseButtons = new System.Windows.Forms.ToolStripMenuItem();
            this.menuIgnoreTreeControlCloseButtons = new System.Windows.Forms.ToolStripMenuItem();
            this.menuIgnoreTextBoxCloseButtons = new System.Windows.Forms.ToolStripMenuItem();
            this.menuIgnoreExampleFormCloseButtons = new System.Windows.Forms.ToolStripMenuItem();
            this.menuIgnoreAllCloseButtons = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
            this.menuAllowMinMaxFunctionality = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripOffice2007 = new System.Windows.Forms.ToolStrip();
            this.toolOffice2007Blue = new System.Windows.Forms.ToolStripButton();
            this.toolOffice2007Silver = new System.Windows.Forms.ToolStripButton();
            this.toolOffice2007Black = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolMediaBlue = new System.Windows.Forms.ToolStripButton();
            this.toolMediaOrange = new System.Windows.Forms.ToolStripButton();
            this.toolMediaPurple = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolOffice2003 = new System.Windows.Forms.ToolStripButton();
            this.toolIDE2005 = new System.Windows.Forms.ToolStripButton();
            this.toolPlain = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.tabControl = new Crownwood.DotNetMagic.Controls.TabControl();
            this.tabPage1 = new Crownwood.DotNetMagic.Controls.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabPage2 = new Crownwood.DotNetMagic.Controls.TabPage();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.tabPage3 = new Crownwood.DotNetMagic.Controls.TabPage();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.toolStripVisible = new System.Windows.Forms.ToolStrip();
            this.toolStripShowAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripHideAll = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip.SuspendLayout();
            this.toolStripOffice2007.SuspendLayout();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.toolStripVisible.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList.Images.SetKeyName(0, "");
            this.imageList.Images.SetKeyName(1, "");
            this.imageList.Images.SetKeyName(2, "");
            this.imageList.Images.SetKeyName(3, "");
            this.imageList.Images.SetKeyName(4, "");
            this.imageList.Images.SetKeyName(5, "");
            this.imageList.Images.SetKeyName(6, "document.png");
            // 
            // menuStrip
            // 
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stylesToolStripMenuItem,
            this.createToolStripMenuItem,
            this.feedbackToolStripMenuItem,
            this.manageToolStripMenuItem,
            this.persistenceToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(744, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";
            // 
            // stylesToolStripMenuItem
            // 
            this.stylesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuOffice2007Blue,
            this.menuOffice2007Silver,
            this.menuOffice2007Black,
            this.toolStripSeparator1,
            this.menuMediaBlue,
            this.menuMediaOrange,
            this.menuMediaPurple,
            this.toolStripSeparator2,
            this.menuOffice2003,
            this.menuIDE2005,
            this.menuPlain,
            this.toolStripMenuItem1,
            this.menuExit});
            this.stylesToolStripMenuItem.Name = "stylesToolStripMenuItem";
            this.stylesToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.stylesToolStripMenuItem.Text = "Styles";
            // 
            // menuOffice2007Blue
            // 
            this.menuOffice2007Blue.Checked = true;
            this.menuOffice2007Blue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuOffice2007Blue.Name = "menuOffice2007Blue";
            this.menuOffice2007Blue.Size = new System.Drawing.Size(184, 22);
            this.menuOffice2007Blue.Text = "Office 2007 Blue";
            this.menuOffice2007Blue.Click += new System.EventHandler(this.toolOffice2007Blue_Click);
            // 
            // menuOffice2007Silver
            // 
            this.menuOffice2007Silver.Name = "menuOffice2007Silver";
            this.menuOffice2007Silver.Size = new System.Drawing.Size(184, 22);
            this.menuOffice2007Silver.Text = "Office 2007 Silver";
            this.menuOffice2007Silver.Click += new System.EventHandler(this.toolOffice2007Silver_Click);
            // 
            // menuOffice2007Black
            // 
            this.menuOffice2007Black.Name = "menuOffice2007Black";
            this.menuOffice2007Black.Size = new System.Drawing.Size(184, 22);
            this.menuOffice2007Black.Text = "Office 2007 Black";
            this.menuOffice2007Black.Click += new System.EventHandler(this.toolOffice2007Black_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(181, 6);
            // 
            // menuMediaBlue
            // 
            this.menuMediaBlue.Name = "menuMediaBlue";
            this.menuMediaBlue.Size = new System.Drawing.Size(184, 22);
            this.menuMediaBlue.Text = "Media Player Blue";
            this.menuMediaBlue.Click += new System.EventHandler(this.toolMediaBlue_Click);
            // 
            // menuMediaOrange
            // 
            this.menuMediaOrange.Name = "menuMediaOrange";
            this.menuMediaOrange.Size = new System.Drawing.Size(184, 22);
            this.menuMediaOrange.Text = "Media Player Orange";
            this.menuMediaOrange.Click += new System.EventHandler(this.toolMediaOrange_Click);
            // 
            // menuMediaPurple
            // 
            this.menuMediaPurple.Name = "menuMediaPurple";
            this.menuMediaPurple.Size = new System.Drawing.Size(184, 22);
            this.menuMediaPurple.Text = "Media Player Purple";
            this.menuMediaPurple.Click += new System.EventHandler(this.toolMediaPurple_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(181, 6);
            // 
            // menuOffice2003
            // 
            this.menuOffice2003.Name = "menuOffice2003";
            this.menuOffice2003.Size = new System.Drawing.Size(184, 22);
            this.menuOffice2003.Text = "Office 2003";
            this.menuOffice2003.Click += new System.EventHandler(this.toolOffice2003_Click);
            // 
            // menuIDE2005
            // 
            this.menuIDE2005.Name = "menuIDE2005";
            this.menuIDE2005.Size = new System.Drawing.Size(184, 22);
            this.menuIDE2005.Text = "IDE2005";
            this.menuIDE2005.Click += new System.EventHandler(this.toolIDE2005_Click);
            // 
            // menuPlain
            // 
            this.menuPlain.Name = "menuPlain";
            this.menuPlain.Size = new System.Drawing.Size(184, 22);
            this.menuPlain.Text = "Plain";
            this.menuPlain.Click += new System.EventHandler(this.toolPlain_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(181, 6);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(184, 22);
            this.menuExit.Text = "Exit";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // createToolStripMenuItem
            // 
            this.createToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCreateTreeControl,
            this.menuCreateTextBox,
            this.menuCreateExampleForm,
            this.toolStripMenuItem2,
            this.menuCreate3InRow,
            this.menuCreate3InColumn,
            this.menuCreate3InWindow,
            this.menuCreate3AutoHidden,
            this.toolStripMenuItem3,
            this.menuCreate3Floating});
            this.createToolStripMenuItem.Name = "createToolStripMenuItem";
            this.createToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.createToolStripMenuItem.Text = "Create";
            // 
            // menuCreateTreeControl
            // 
            this.menuCreateTreeControl.Name = "menuCreateTreeControl";
            this.menuCreateTreeControl.Size = new System.Drawing.Size(185, 22);
            this.menuCreateTreeControl.Text = "Create TreeControl";
            this.menuCreateTreeControl.Click += new System.EventHandler(this.menuCreateTreeControl_Click);
            // 
            // menuCreateTextBox
            // 
            this.menuCreateTextBox.Name = "menuCreateTextBox";
            this.menuCreateTextBox.Size = new System.Drawing.Size(185, 22);
            this.menuCreateTextBox.Text = "Create TextBox";
            this.menuCreateTextBox.Click += new System.EventHandler(this.menuCreateTextBox_Click);
            // 
            // menuCreateExampleForm
            // 
            this.menuCreateExampleForm.Name = "menuCreateExampleForm";
            this.menuCreateExampleForm.Size = new System.Drawing.Size(185, 22);
            this.menuCreateExampleForm.Text = "Create ExampleForm";
            this.menuCreateExampleForm.Click += new System.EventHandler(this.menuCreateExampleForm_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(182, 6);
            // 
            // menuCreate3InRow
            // 
            this.menuCreate3InRow.Name = "menuCreate3InRow";
            this.menuCreate3InRow.Size = new System.Drawing.Size(185, 22);
            this.menuCreate3InRow.Text = "Create 3 in Row";
            this.menuCreate3InRow.Click += new System.EventHandler(this.menuCreate3Row_Click);
            // 
            // menuCreate3InColumn
            // 
            this.menuCreate3InColumn.Name = "menuCreate3InColumn";
            this.menuCreate3InColumn.Size = new System.Drawing.Size(185, 22);
            this.menuCreate3InColumn.Text = "Create 3 in Column";
            this.menuCreate3InColumn.Click += new System.EventHandler(this.menuCreate3Column_Click);
            // 
            // menuCreate3InWindow
            // 
            this.menuCreate3InWindow.Name = "menuCreate3InWindow";
            this.menuCreate3InWindow.Size = new System.Drawing.Size(185, 22);
            this.menuCreate3InWindow.Text = "Create 3 in Window";
            this.menuCreate3InWindow.Click += new System.EventHandler(this.menuCreate3Window_Click);
            // 
            // menuCreate3AutoHidden
            // 
            this.menuCreate3AutoHidden.Name = "menuCreate3AutoHidden";
            this.menuCreate3AutoHidden.Size = new System.Drawing.Size(185, 22);
            this.menuCreate3AutoHidden.Text = "Create 3 AutoHidden";
            this.menuCreate3AutoHidden.Click += new System.EventHandler(this.menuCreate3AutoHidden_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(182, 6);
            // 
            // menuCreate3Floating
            // 
            this.menuCreate3Floating.Name = "menuCreate3Floating";
            this.menuCreate3Floating.Size = new System.Drawing.Size(185, 22);
            this.menuCreate3Floating.Text = "Create 3 Floating";
            this.menuCreate3Floating.Click += new System.EventHandler(this.menuCreate3Floating_Click);
            // 
            // feedbackToolStripMenuItem
            // 
            this.feedbackToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFocusRectangles,
            this.menuSemitransparentBlocks,
            this.menuDiamondIndicators,
            this.menuSquareIndicators});
            this.feedbackToolStripMenuItem.Name = "feedbackToolStripMenuItem";
            this.feedbackToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.feedbackToolStripMenuItem.Text = "Feedback";
            // 
            // menuFocusRectangles
            // 
            this.menuFocusRectangles.Name = "menuFocusRectangles";
            this.menuFocusRectangles.Size = new System.Drawing.Size(202, 22);
            this.menuFocusRectangles.Text = "Focus rectangles";
            this.menuFocusRectangles.Click += new System.EventHandler(this.menuFocusRectangles_Click);
            // 
            // menuSemitransparentBlocks
            // 
            this.menuSemitransparentBlocks.Name = "menuSemitransparentBlocks";
            this.menuSemitransparentBlocks.Size = new System.Drawing.Size(202, 22);
            this.menuSemitransparentBlocks.Text = "Semi-transparent blocks";
            this.menuSemitransparentBlocks.Click += new System.EventHandler(this.menuSemitransparentBlocks_Click);
            // 
            // menuDiamondIndicators
            // 
            this.menuDiamondIndicators.Name = "menuDiamondIndicators";
            this.menuDiamondIndicators.Size = new System.Drawing.Size(202, 22);
            this.menuDiamondIndicators.Text = "Diamond indicators";
            this.menuDiamondIndicators.Click += new System.EventHandler(this.menuDiamondIndicators_Click);
            // 
            // menuSquareIndicators
            // 
            this.menuSquareIndicators.Checked = true;
            this.menuSquareIndicators.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuSquareIndicators.Name = "menuSquareIndicators";
            this.menuSquareIndicators.Size = new System.Drawing.Size(202, 22);
            this.menuSquareIndicators.Text = "Square indicators";
            this.menuSquareIndicators.Click += new System.EventHandler(this.menuSquareIndicators_Click);
            // 
            // manageToolStripMenuItem
            // 
            this.manageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuShowAll,
            this.menuHideAll,
            this.toolStripMenuItem4,
            this.menuAllowRedocking,
            this.menuAllowFloating,
            this.menuAllowResize,
            this.menuAllowTabbed,
            this.toolStripMenuItem5,
            this.menuDeleteAll});
            this.manageToolStripMenuItem.Name = "manageToolStripMenuItem";
            this.manageToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.manageToolStripMenuItem.Text = "Manage";
            // 
            // menuShowAll
            // 
            this.menuShowAll.Name = "menuShowAll";
            this.menuShowAll.Size = new System.Drawing.Size(163, 22);
            this.menuShowAll.Text = "Show All";
            this.menuShowAll.Click += new System.EventHandler(this.menuShowAll_Click);
            // 
            // menuHideAll
            // 
            this.menuHideAll.Name = "menuHideAll";
            this.menuHideAll.Size = new System.Drawing.Size(163, 22);
            this.menuHideAll.Text = "Hide All";
            this.menuHideAll.Click += new System.EventHandler(this.menuHideAll_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(160, 6);
            // 
            // menuAllowRedocking
            // 
            this.menuAllowRedocking.Checked = true;
            this.menuAllowRedocking.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuAllowRedocking.Name = "menuAllowRedocking";
            this.menuAllowRedocking.Size = new System.Drawing.Size(163, 22);
            this.menuAllowRedocking.Text = "Allow Redocking";
            this.menuAllowRedocking.Click += new System.EventHandler(this.menuAllowRedocking_Click);
            // 
            // menuAllowFloating
            // 
            this.menuAllowFloating.Checked = true;
            this.menuAllowFloating.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuAllowFloating.Name = "menuAllowFloating";
            this.menuAllowFloating.Size = new System.Drawing.Size(163, 22);
            this.menuAllowFloating.Text = "Allow Floating";
            this.menuAllowFloating.Click += new System.EventHandler(this.menuAllowFloating_Click);
            // 
            // menuAllowResize
            // 
            this.menuAllowResize.Checked = true;
            this.menuAllowResize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuAllowResize.Name = "menuAllowResize";
            this.menuAllowResize.Size = new System.Drawing.Size(163, 22);
            this.menuAllowResize.Text = "Allow Resize";
            this.menuAllowResize.Click += new System.EventHandler(this.menuAllowResize_Click);
            // 
            // menuAllowTabbed
            // 
            this.menuAllowTabbed.Checked = true;
            this.menuAllowTabbed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuAllowTabbed.Name = "menuAllowTabbed";
            this.menuAllowTabbed.Size = new System.Drawing.Size(163, 22);
            this.menuAllowTabbed.Text = "Allow Tabbed";
            this.menuAllowTabbed.Click += new System.EventHandler(this.menuAllowTabbed_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(160, 6);
            // 
            // menuDeleteAll
            // 
            this.menuDeleteAll.Name = "menuDeleteAll";
            this.menuDeleteAll.Size = new System.Drawing.Size(163, 22);
            this.menuDeleteAll.Text = "Delete All";
            this.menuDeleteAll.Click += new System.EventHandler(this.menuDeleteAll_Click);
            // 
            // persistenceToolStripMenuItem
            // 
            this.persistenceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSaveAsConfig1,
            this.menuSaveAsConfig2,
            this.toolStripMenuItem6,
            this.menuLoadFromConfig1,
            this.menuLoadFromConfig2,
            this.toolStripMenuItem7,
            this.menuSaveToByteArray1,
            this.menuSaveToByteArray2,
            this.toolStripMenuItem8,
            this.menuLoadFromByteArray1,
            this.menuLoadFromByteArray2});
            this.persistenceToolStripMenuItem.Name = "persistenceToolStripMenuItem";
            this.persistenceToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.persistenceToolStripMenuItem.Text = "Persistence";
            // 
            // menuSaveAsConfig1
            // 
            this.menuSaveAsConfig1.Name = "menuSaveAsConfig1";
            this.menuSaveAsConfig1.Size = new System.Drawing.Size(196, 22);
            this.menuSaveAsConfig1.Text = "Save as Config1.xml";
            this.menuSaveAsConfig1.Click += new System.EventHandler(this.menuSaveConfig1_Click);
            // 
            // menuSaveAsConfig2
            // 
            this.menuSaveAsConfig2.Name = "menuSaveAsConfig2";
            this.menuSaveAsConfig2.Size = new System.Drawing.Size(196, 22);
            this.menuSaveAsConfig2.Text = "Save as Config2.xml";
            this.menuSaveAsConfig2.Click += new System.EventHandler(this.menuSaveConfig2_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(193, 6);
            // 
            // menuLoadFromConfig1
            // 
            this.menuLoadFromConfig1.Name = "menuLoadFromConfig1";
            this.menuLoadFromConfig1.Size = new System.Drawing.Size(196, 22);
            this.menuLoadFromConfig1.Text = "Load from Config1.xml";
            this.menuLoadFromConfig1.Click += new System.EventHandler(this.menuLoadConfig1_Click);
            // 
            // menuLoadFromConfig2
            // 
            this.menuLoadFromConfig2.Name = "menuLoadFromConfig2";
            this.menuLoadFromConfig2.Size = new System.Drawing.Size(196, 22);
            this.menuLoadFromConfig2.Text = "Load from Config2.xml";
            this.menuLoadFromConfig2.Click += new System.EventHandler(this.menuLoadConfig2_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(193, 6);
            // 
            // menuSaveToByteArray1
            // 
            this.menuSaveToByteArray1.Name = "menuSaveToByteArray1";
            this.menuSaveToByteArray1.Size = new System.Drawing.Size(196, 22);
            this.menuSaveToByteArray1.Text = "Save to byte array 1";
            this.menuSaveToByteArray1.Click += new System.EventHandler(this.menuSaveArray1_Click);
            // 
            // menuSaveToByteArray2
            // 
            this.menuSaveToByteArray2.Name = "menuSaveToByteArray2";
            this.menuSaveToByteArray2.Size = new System.Drawing.Size(196, 22);
            this.menuSaveToByteArray2.Text = "Save to byte array 2";
            this.menuSaveToByteArray2.Click += new System.EventHandler(this.menuSaveArray2_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(193, 6);
            // 
            // menuLoadFromByteArray1
            // 
            this.menuLoadFromByteArray1.Name = "menuLoadFromByteArray1";
            this.menuLoadFromByteArray1.Size = new System.Drawing.Size(196, 22);
            this.menuLoadFromByteArray1.Text = "Load from byte array 1";
            this.menuLoadFromByteArray1.Click += new System.EventHandler(this.menuLoadArray1_Click);
            // 
            // menuLoadFromByteArray2
            // 
            this.menuLoadFromByteArray2.Name = "menuLoadFromByteArray2";
            this.menuLoadFromByteArray2.Size = new System.Drawing.Size(196, 22);
            this.menuLoadFromByteArray2.Text = "Load from byte array 2";
            this.menuLoadFromByteArray2.Click += new System.EventHandler(this.menuLoadArray2_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAllowContextMenu,
            this.menuCustomizeContextMenu,
            this.toolStripMenuItem9,
            this.menuDefaultResizeBar,
            this.menu1PixelResizeBar,
            this.menu5PixelResizeBar,
            this.menu7PixelResizeBar,
            this.toolStripMenuItem10,
            this.menuShowCaptionBars,
            this.menuShowCloseButtons,
            this.toolStripMenuItem11,
            this.menuAllowAllCloseButtons,
            this.menuIgnoreTreeControlCloseButtons,
            this.menuIgnoreTextBoxCloseButtons,
            this.menuIgnoreExampleFormCloseButtons,
            this.menuIgnoreAllCloseButtons,
            this.toolStripMenuItem12,
            this.menuAllowMinMaxFunctionality});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // menuAllowContextMenu
            // 
            this.menuAllowContextMenu.Checked = true;
            this.menuAllowContextMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuAllowContextMenu.Name = "menuAllowContextMenu";
            this.menuAllowContextMenu.Size = new System.Drawing.Size(257, 22);
            this.menuAllowContextMenu.Text = "Allow Context Menu";
            this.menuAllowContextMenu.Click += new System.EventHandler(this.menuAllowContext_Click);
            // 
            // menuCustomizeContextMenu
            // 
            this.menuCustomizeContextMenu.Name = "menuCustomizeContextMenu";
            this.menuCustomizeContextMenu.Size = new System.Drawing.Size(257, 22);
            this.menuCustomizeContextMenu.Text = "Customize Context Menu";
            this.menuCustomizeContextMenu.Click += new System.EventHandler(this.menuCustomizeContext_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(254, 6);
            // 
            // menuDefaultResizeBar
            // 
            this.menuDefaultResizeBar.Checked = true;
            this.menuDefaultResizeBar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuDefaultResizeBar.Name = "menuDefaultResizeBar";
            this.menuDefaultResizeBar.Size = new System.Drawing.Size(257, 22);
            this.menuDefaultResizeBar.Text = "Default ResizeBar";
            this.menuDefaultResizeBar.Click += new System.EventHandler(this.menuResizeDefault_Click);
            // 
            // menu1PixelResizeBar
            // 
            this.menu1PixelResizeBar.Name = "menu1PixelResizeBar";
            this.menu1PixelResizeBar.Size = new System.Drawing.Size(257, 22);
            this.menu1PixelResizeBar.Text = "1 Pixel ResizeBar";
            this.menu1PixelResizeBar.Click += new System.EventHandler(this.menuResize1_Click);
            // 
            // menu5PixelResizeBar
            // 
            this.menu5PixelResizeBar.Name = "menu5PixelResizeBar";
            this.menu5PixelResizeBar.Size = new System.Drawing.Size(257, 22);
            this.menu5PixelResizeBar.Text = "5 Pixel ResizeBar";
            this.menu5PixelResizeBar.Click += new System.EventHandler(this.menuResize5_Click);
            // 
            // menu7PixelResizeBar
            // 
            this.menu7PixelResizeBar.Name = "menu7PixelResizeBar";
            this.menu7PixelResizeBar.Size = new System.Drawing.Size(257, 22);
            this.menu7PixelResizeBar.Text = "7 Pixel ResizeBar";
            this.menu7PixelResizeBar.Click += new System.EventHandler(this.menuResize7_Click);
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(254, 6);
            // 
            // menuShowCaptionBars
            // 
            this.menuShowCaptionBars.Checked = true;
            this.menuShowCaptionBars.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuShowCaptionBars.Name = "menuShowCaptionBars";
            this.menuShowCaptionBars.Size = new System.Drawing.Size(257, 22);
            this.menuShowCaptionBars.Text = "Show caption bars";
            this.menuShowCaptionBars.Click += new System.EventHandler(this.menuShowCaptions_Click);
            // 
            // menuShowCloseButtons
            // 
            this.menuShowCloseButtons.Checked = true;
            this.menuShowCloseButtons.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuShowCloseButtons.Name = "menuShowCloseButtons";
            this.menuShowCloseButtons.Size = new System.Drawing.Size(257, 22);
            this.menuShowCloseButtons.Text = "Show close buttons";
            this.menuShowCloseButtons.Click += new System.EventHandler(this.menuShowClose_Click);
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(254, 6);
            // 
            // menuAllowAllCloseButtons
            // 
            this.menuAllowAllCloseButtons.Checked = true;
            this.menuAllowAllCloseButtons.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuAllowAllCloseButtons.Name = "menuAllowAllCloseButtons";
            this.menuAllowAllCloseButtons.Size = new System.Drawing.Size(257, 22);
            this.menuAllowAllCloseButtons.Text = "Allow all close buttons";
            this.menuAllowAllCloseButtons.Click += new System.EventHandler(this.menuAllowAllClose_Click);
            // 
            // menuIgnoreTreeControlCloseButtons
            // 
            this.menuIgnoreTreeControlCloseButtons.Name = "menuIgnoreTreeControlCloseButtons";
            this.menuIgnoreTreeControlCloseButtons.Size = new System.Drawing.Size(257, 22);
            this.menuIgnoreTreeControlCloseButtons.Text = "Ignore TreeControl close buttons";
            this.menuIgnoreTreeControlCloseButtons.Click += new System.EventHandler(this.menuIgnoreTreeControlClose_Click);
            // 
            // menuIgnoreTextBoxCloseButtons
            // 
            this.menuIgnoreTextBoxCloseButtons.Name = "menuIgnoreTextBoxCloseButtons";
            this.menuIgnoreTextBoxCloseButtons.Size = new System.Drawing.Size(257, 22);
            this.menuIgnoreTextBoxCloseButtons.Text = "Ignore TextBox close buttons";
            this.menuIgnoreTextBoxCloseButtons.Click += new System.EventHandler(this.menuIgnoreTextBoxClose_Click);
            // 
            // menuIgnoreExampleFormCloseButtons
            // 
            this.menuIgnoreExampleFormCloseButtons.Name = "menuIgnoreExampleFormCloseButtons";
            this.menuIgnoreExampleFormCloseButtons.Size = new System.Drawing.Size(257, 22);
            this.menuIgnoreExampleFormCloseButtons.Text = "Ignore ExampleForm close buttons";
            this.menuIgnoreExampleFormCloseButtons.Click += new System.EventHandler(this.menuIgnoreExampleFormClose_Click);
            // 
            // menuIgnoreAllCloseButtons
            // 
            this.menuIgnoreAllCloseButtons.Name = "menuIgnoreAllCloseButtons";
            this.menuIgnoreAllCloseButtons.Size = new System.Drawing.Size(257, 22);
            this.menuIgnoreAllCloseButtons.Text = "Ignore all close buttons";
            this.menuIgnoreAllCloseButtons.Click += new System.EventHandler(this.menuIgnoreAllClose_Click);
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size(254, 6);
            // 
            // menuAllowMinMaxFunctionality
            // 
            this.menuAllowMinMaxFunctionality.Checked = true;
            this.menuAllowMinMaxFunctionality.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuAllowMinMaxFunctionality.Name = "menuAllowMinMaxFunctionality";
            this.menuAllowMinMaxFunctionality.Size = new System.Drawing.Size(257, 22);
            this.menuAllowMinMaxFunctionality.Text = "Allow Min/Max functionality";
            this.menuAllowMinMaxFunctionality.Click += new System.EventHandler(this.menuAllowMinMaxFunctionality_Click);
            // 
            // toolStripOffice2007
            // 
            this.toolStripOffice2007.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripOffice2007.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolOffice2007Blue,
            this.toolOffice2007Silver,
            this.toolOffice2007Black,
            this.toolStripSeparator3,
            this.toolMediaBlue,
            this.toolMediaOrange,
            this.toolMediaPurple,
            this.toolStripSeparator4,
            this.toolOffice2003,
            this.toolIDE2005,
            this.toolPlain});
            this.toolStripOffice2007.Location = new System.Drawing.Point(3, 24);
            this.toolStripOffice2007.Name = "toolStripOffice2007";
            this.toolStripOffice2007.Size = new System.Drawing.Size(640, 25);
            this.toolStripOffice2007.TabIndex = 1;
            this.toolStripOffice2007.Text = "toolStrip1";
            // 
            // toolOffice2007Blue
            // 
            this.toolOffice2007Blue.Checked = true;
            this.toolOffice2007Blue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolOffice2007Blue.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolOffice2007Blue.Image = ((System.Drawing.Image)(resources.GetObject("toolOffice2007Blue.Image")));
            this.toolOffice2007Blue.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolOffice2007Blue.Name = "toolOffice2007Blue";
            this.toolOffice2007Blue.Size = new System.Drawing.Size(69, 22);
            this.toolOffice2007Blue.Text = "Office Blue";
            this.toolOffice2007Blue.Click += new System.EventHandler(this.toolOffice2007Blue_Click);
            // 
            // toolOffice2007Silver
            // 
            this.toolOffice2007Silver.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolOffice2007Silver.Image = ((System.Drawing.Image)(resources.GetObject("toolOffice2007Silver.Image")));
            this.toolOffice2007Silver.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolOffice2007Silver.Name = "toolOffice2007Silver";
            this.toolOffice2007Silver.Size = new System.Drawing.Size(74, 22);
            this.toolOffice2007Silver.Text = "Office Silver";
            this.toolOffice2007Silver.ToolTipText = "Office 2007 - Silver";
            this.toolOffice2007Silver.Click += new System.EventHandler(this.toolOffice2007Silver_Click);
            // 
            // toolOffice2007Black
            // 
            this.toolOffice2007Black.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolOffice2007Black.Image = ((System.Drawing.Image)(resources.GetObject("toolOffice2007Black.Image")));
            this.toolOffice2007Black.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolOffice2007Black.Name = "toolOffice2007Black";
            this.toolOffice2007Black.Size = new System.Drawing.Size(74, 22);
            this.toolOffice2007Black.Text = "Office Black";
            this.toolOffice2007Black.ToolTipText = "Office 2007 - Black";
            this.toolOffice2007Black.Click += new System.EventHandler(this.toolOffice2007Black_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolMediaBlue
            // 
            this.toolMediaBlue.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolMediaBlue.Image = ((System.Drawing.Image)(resources.GetObject("toolMediaBlue.Image")));
            this.toolMediaBlue.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolMediaBlue.Name = "toolMediaBlue";
            this.toolMediaBlue.Size = new System.Drawing.Size(70, 22);
            this.toolMediaBlue.Text = "Media Blue";
            this.toolMediaBlue.ToolTipText = "Media Player - Blue";
            this.toolMediaBlue.Click += new System.EventHandler(this.toolMediaBlue_Click);
            // 
            // toolMediaOrange
            // 
            this.toolMediaOrange.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolMediaOrange.Image = ((System.Drawing.Image)(resources.GetObject("toolMediaOrange.Image")));
            this.toolMediaOrange.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolMediaOrange.Name = "toolMediaOrange";
            this.toolMediaOrange.Size = new System.Drawing.Size(86, 22);
            this.toolMediaOrange.Text = "Media Orange";
            this.toolMediaOrange.ToolTipText = "Media Player - Orange";
            this.toolMediaOrange.Click += new System.EventHandler(this.toolMediaOrange_Click);
            // 
            // toolMediaPurple
            // 
            this.toolMediaPurple.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolMediaPurple.Image = ((System.Drawing.Image)(resources.GetObject("toolMediaPurple.Image")));
            this.toolMediaPurple.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolMediaPurple.Name = "toolMediaPurple";
            this.toolMediaPurple.Size = new System.Drawing.Size(81, 22);
            this.toolMediaPurple.Text = "Media Purple";
            this.toolMediaPurple.ToolTipText = "Media Player - Purple";
            this.toolMediaPurple.Click += new System.EventHandler(this.toolMediaPurple_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolOffice2003
            // 
            this.toolOffice2003.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolOffice2003.Image = ((System.Drawing.Image)(resources.GetObject("toolOffice2003.Image")));
            this.toolOffice2003.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolOffice2003.Name = "toolOffice2003";
            this.toolOffice2003.Size = new System.Drawing.Size(70, 22);
            this.toolOffice2003.Text = "Office 2003";
            this.toolOffice2003.Click += new System.EventHandler(this.toolOffice2003_Click);
            // 
            // toolIDE2005
            // 
            this.toolIDE2005.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolIDE2005.Image = ((System.Drawing.Image)(resources.GetObject("toolIDE2005.Image")));
            this.toolIDE2005.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolIDE2005.Name = "toolIDE2005";
            this.toolIDE2005.Size = new System.Drawing.Size(55, 22);
            this.toolIDE2005.Text = "IDE 2005";
            this.toolIDE2005.Click += new System.EventHandler(this.toolIDE2005_Click);
            // 
            // toolPlain
            // 
            this.toolPlain.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolPlain.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolPlain.Name = "toolPlain";
            this.toolPlain.Size = new System.Drawing.Size(37, 22);
            this.toolPlain.Text = "Plain";
            this.toolPlain.Click += new System.EventHandler(this.toolPlain_Click);
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.Controls.Add(this.tabControl);
            this.toolStripContainer.ContentPanel.Padding = new System.Windows.Forms.Padding(2);
            this.toolStripContainer.ContentPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(744, 418);
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.Size = new System.Drawing.Size(744, 492);
            this.toolStripContainer.TabIndex = 2;
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.menuStrip);
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.toolStripOffice2007);
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.toolStripVisible);
            // 
            // tabControl
            // 
            this.tabControl.Appearance = Crownwood.DotNetMagic.Controls.VisualAppearance.MultiDocument;
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.ImageList = this.imageList;
            this.tabControl.Location = new System.Drawing.Point(2, 2);
            this.tabControl.MediaPlayerDockSides = false;
            this.tabControl.Name = "tabControl";
            this.tabControl.OfficeDockSides = false;
            this.tabControl.SelectedIndex = 0;
            this.tabControl.ShowClose = false;
            this.tabControl.ShowDropSelect = false;
            this.tabControl.Size = new System.Drawing.Size(740, 414);
            this.tabControl.TabIndex = 0;
            this.tabControl.TabPages.AddRange(new Crownwood.DotNetMagic.Controls.TabPage[] {
            this.tabPage1,
            this.tabPage2,
            this.tabPage3});
            this.tabControl.TextTips = true;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Window;
            this.tabPage1.Controls.Add(this.textBox1);
            this.tabPage1.ImageIndex = 6;
            this.tabPage1.InactiveBackColor = System.Drawing.Color.Empty;
            this.tabPage1.InactiveTextBackColor = System.Drawing.Color.Empty;
            this.tabPage1.InactiveTextColor = System.Drawing.Color.Empty;
            this.tabPage1.Location = new System.Drawing.Point(1, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.SelectBackColor = System.Drawing.Color.Empty;
            this.tabPage1.SelectTextBackColor = System.Drawing.Color.Empty;
            this.tabPage1.SelectTextColor = System.Drawing.Color.Empty;
            this.tabPage1.Size = new System.Drawing.Size(738, 389);
            this.tabPage1.TabIndex = 4;
            this.tabPage1.Title = "Page One";
            this.tabPage1.ToolTip = "The first page";
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(738, 389);
            this.textBox1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Window;
            this.tabPage2.Controls.Add(this.textBox2);
            this.tabPage2.ImageIndex = 6;
            this.tabPage2.InactiveBackColor = System.Drawing.Color.Empty;
            this.tabPage2.InactiveTextBackColor = System.Drawing.Color.Empty;
            this.tabPage2.InactiveTextColor = System.Drawing.Color.Empty;
            this.tabPage2.Location = new System.Drawing.Point(1, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.SelectBackColor = System.Drawing.Color.Empty;
            this.tabPage2.Selected = false;
            this.tabPage2.SelectTextBackColor = System.Drawing.Color.Empty;
            this.tabPage2.SelectTextColor = System.Drawing.Color.Empty;
            this.tabPage2.Size = new System.Drawing.Size(738, 389);
            this.tabPage2.TabIndex = 5;
            this.tabPage2.Title = "Page Two";
            this.tabPage2.ToolTip = "The second page";
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox2.Location = new System.Drawing.Point(0, 0);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(738, 389);
            this.textBox2.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Window;
            this.tabPage3.Controls.Add(this.textBox3);
            this.tabPage3.ImageIndex = 6;
            this.tabPage3.InactiveBackColor = System.Drawing.Color.Empty;
            this.tabPage3.InactiveTextBackColor = System.Drawing.Color.Empty;
            this.tabPage3.InactiveTextColor = System.Drawing.Color.Empty;
            this.tabPage3.Location = new System.Drawing.Point(1, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.SelectBackColor = System.Drawing.Color.Empty;
            this.tabPage3.Selected = false;
            this.tabPage3.SelectTextBackColor = System.Drawing.Color.Empty;
            this.tabPage3.SelectTextColor = System.Drawing.Color.Empty;
            this.tabPage3.Size = new System.Drawing.Size(738, 389);
            this.tabPage3.TabIndex = 6;
            this.tabPage3.Title = "Page Three";
            this.tabPage3.ToolTip = "The third page";
            // 
            // textBox3
            // 
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox3.Location = new System.Drawing.Point(0, 0);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(738, 389);
            this.textBox3.TabIndex = 1;
            // 
            // toolStripVisible
            // 
            this.toolStripVisible.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripVisible.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripShowAll,
            this.toolStripHideAll});
            this.toolStripVisible.Location = new System.Drawing.Point(3, 49);
            this.toolStripVisible.Name = "toolStripVisible";
            this.toolStripVisible.Size = new System.Drawing.Size(153, 25);
            this.toolStripVisible.TabIndex = 2;
            // 
            // toolStripShowAll
            // 
            this.toolStripShowAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripShowAll.Image = ((System.Drawing.Image)(resources.GetObject("toolStripShowAll.Image")));
            this.toolStripShowAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripShowAll.Name = "toolStripShowAll";
            this.toolStripShowAll.Size = new System.Drawing.Size(57, 22);
            this.toolStripShowAll.Text = "Show All";
            this.toolStripShowAll.Click += new System.EventHandler(this.toolShowAll_Click);
            // 
            // toolStripHideAll
            // 
            this.toolStripHideAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripHideAll.Image = ((System.Drawing.Image)(resources.GetObject("toolStripHideAll.Image")));
            this.toolStripHideAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripHideAll.Name = "toolStripHideAll";
            this.toolStripHideAll.Size = new System.Drawing.Size(53, 22);
            this.toolStripHideAll.Text = "Hide All";
            this.toolStripHideAll.Click += new System.EventHandler(this.toolHideAll_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 492);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.statusStrip1.Size = new System.Drawing.Size(744, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip";
            // 
            // SampleDockingForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(744, 514);
            this.Controls.Add(this.toolStripContainer);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "SampleDockingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DotNetMagic - SampleDocking";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStripOffice2007.ResumeLayout(false);
            this.toolStripOffice2007.PerformLayout();
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.toolStripVisible.ResumeLayout(false);
            this.toolStripVisible.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        #region Change VisualStyle
        private void ChangeStyle(VisualStyle style)
		{
            // Work out the actual stlye to use based on the operating system
            style = ColorHelper.ValidateStyle(style);

            // Apply the style to the form
            Style = style;

            // Update each of the contained elements
            dockingManager.Style = style;
            tabControl.Style = style;

            // We want to show the different page navigation methods for the different modes
            switch (style)
            {
                case VisualStyle.Office2007Blue:
                case VisualStyle.Office2007Silver:
                case VisualStyle.Office2007Black:
                case VisualStyle.MediaPlayerBlue:
                case VisualStyle.MediaPlayerOrange:
                case VisualStyle.MediaPlayerPurple:
                case VisualStyle.Office2003:
                    tabControl.ShowArrows = false;
                    tabControl.ShowDropSelect = true;
                    break;
                default:
                    tabControl.ShowArrows = true;
                    tabControl.ShowDropSelect = false;
                    break;
            }
			
			// Change appearance of any created tree controls
			if (_treeControls != null)
			{
				foreach(TreeControl tc in _treeControls)
				{
					if (!tc.IsDisposed)
					{
                        switch (style)
                        {
                            case VisualStyle.Office2003:
                                tc.SetTreeControlStyle(TreeControlStyles.GroupOfficeLight);
                                break;
                            case VisualStyle.Office2007Blue:
                                tc.SetTreeControlStyle(TreeControlStyles.GroupOfficeBlueLight);
                                break;
                            case VisualStyle.Office2007Silver:
                                tc.SetTreeControlStyle(TreeControlStyles.GroupOfficeSilverLight);
                                break;
                            case VisualStyle.Office2007Black:
                                tc.SetTreeControlStyle(TreeControlStyles.GroupOfficeBlackLight);
                                break;
                            case VisualStyle.MediaPlayerBlue:
                                tc.SetTreeControlStyle(TreeControlStyles.GroupMediaBlueLight);
                                break;
                            case VisualStyle.MediaPlayerOrange:
                                tc.SetTreeControlStyle(TreeControlStyles.GroupMediaOrangeLight);
                                break;
                            case VisualStyle.MediaPlayerPurple:
                                tc.SetTreeControlStyle(TreeControlStyles.GroupMediaPurpleLight);
                                break;
                            default:
                                tc.SetTreeControlStyle(TreeControlStyles.Group);
                                break;
                        }
					}

                    // We do not want a border around the tree as the docking window provides one
                    tc.BorderStyle = TreeBorderStyle.None;
				}
			}
		}
        #endregion

        #region Create Docking Windows
        private Content CreateTextBoxContent()
		{
			// Create the TextBox for use in the new docking window
			TextBox tb = new TextBox();
            tb.Text = "A simple TextBox instance.";
			tb.BorderStyle = BorderStyle.None;
			tb.Multiline = true;
		
			// Create a new docking Content instance with our new TextBox
			return dockingManager.Contents.Add(tb, "TextBox " + count, imageList, count++ % 6);
		}

		private Content CreateTreeControlContent()
		{
			// Create the TreeControl for use in the new docking window
			TreeControl tc = new TreeControl();

            switch (dockingManager.Style)
            {
                case VisualStyle.Office2003:
                    tc.SetTreeControlStyle(TreeControlStyles.GroupOfficeLight);
                    break;
                case VisualStyle.Office2007Blue:
                    tc.SetTreeControlStyle(TreeControlStyles.GroupOfficeBlueLight);
                    break;
                case VisualStyle.Office2007Silver:
                    tc.SetTreeControlStyle(TreeControlStyles.GroupOfficeSilverLight);
                    break;
                case VisualStyle.Office2007Black:
                    tc.SetTreeControlStyle(TreeControlStyles.GroupOfficeBlackLight);
                    break;
                default:
                    tc.SetTreeControlStyle(TreeControlStyles.GroupOfficeLight);
                    tc.GroupColoring = GroupColoring.ControlProperties;
                    break;
            }

            tc.BorderStyle = TreeBorderStyle.None;
            tc.Indicators = Indicators.AtGroup;
			
			// Create top level groups
			for(int i=0; i<3; i++)
			{
				Node group = new Node("Group " + i);
				group.Expanded = true;
				tc.Nodes.Add(group); 

				// Create some dummy entries in teh group
				for(int j=0; j<3; j++)
				{
					// Create a new child node
					Node child = new Node("Node " + (i*3+j));
					
					// Give an image and indicator different for each child node
					child.Image = imageList.Images[(i*3+j) % 7];
					child.Indicator = (Indicator)(i*5)+j;
					
					// Give some groups checkbox/radio buttons
					if (i == 1)
						child.CheckStates = NodeCheckStates.Radio;
					else if (i == 2)
						child.CheckStates = NodeCheckStates.TwoStateCheck;

					group.Nodes.Add(child); 
				}
			}
			
			// Maintain a list of all the tree controls created			
			if (_treeControls == null)
				_treeControls = new ArrayList();
			
			// Need to remember so we can switch visual styles	
			_treeControls.Add(tc);
			
			// Create a new docking Content instance with our new TreeControl
			Content c = dockingManager.Contents.Add(tc, "TreeControl " + count, imageList, count++ % 6);
            c.FloatingSize = new Size(220, 300);

            return c;
        }

        private Content CreateExampleFormContent()
		{
			// Create the ExampleForm for use in the new docking window
			ExampleForm ef = new ExampleForm();
			
			// Maintain a list of all the example forms created			
			if (_exampleForms == null)
				_exampleForms = new ArrayList();
			
			// Need to remember so we can switch visual styles	
			_exampleForms.Add(ef);
		
			// Create a new docking Content instance with our new ExampleForm
			Content c = dockingManager.Contents.Add(ef, "ExampleForm " + count, imageList, count++ % 6);
            c.FloatingSize = new Size(220, 150);

            return c;
        }

        private void InitializeContent(Content c)
        {
            c.CaptionBar = captionBars;
            c.CloseButton = closeButtons;
        }

        #endregion

        #region Menu/Tool Event Handlers
        private void menuCreateTextBox_Click(object sender, System.EventArgs e)
		{
			// Create a TextBox content instance
			Content c = CreateTextBoxContent();
        
			// Setup initial state to match menu selections
			InitializeContent(c);
        
			// Request a new docking window be created for the above Content on the right edge
			dockingManager.AddContentWithState(c, Crownwood.DotNetMagic.Docking.State.DockRight);
		}

		private void menuCreateTreeControl_Click(object sender, System.EventArgs e)
		{
			// Create a TreeControl content instance
			Content c = CreateTreeControlContent();
        
			// Setup initial state to match menu selections
			InitializeContent(c);
        
			// Request a new docking window be created for the above Content on the left edge
			dockingManager.AddContentWithState(c, Crownwood.DotNetMagic.Docking.State.DockLeft);
		}

		private void menuCreateExampleForm_Click(object sender, System.EventArgs e)
		{
			// Create an ExampleForm content instance
			Content c = CreateExampleFormContent();
        
			// Setup initial state to match menu selections
			InitializeContent(c);
        
			// Request a new docking window be created for the above Content on the bottom edge
			dockingManager.AddContentWithState(c, Crownwood.DotNetMagic.Docking.State.DockBottom);
		}
		
		private void menuCreate3Row_Click(object sender, System.EventArgs e)
		{
			// Create three different content instances
			Content cA = CreateTextBoxContent();
			Content cB = CreateTreeControlContent();
			Content cC = CreateExampleFormContent();
		
			// Setup initial state to match menu selections
			InitializeContent(cA);
			InitializeContent(cB);
			InitializeContent(cC);

			// Request a new Docking window be created for the first content on the bottom edge
			WindowContent wc = dockingManager.AddContentWithState(cA, Crownwood.DotNetMagic.Docking.State.DockBottom) as WindowContent;
        
			// Add two other content into the same Zone
			dockingManager.AddContentToZone(cB, wc.ParentZone, 1);
			dockingManager.AddContentToZone(cC, wc.ParentZone, 2);
		}

		private void menuCreate3Column_Click(object sender, System.EventArgs e)
		{
			// Create three different content instances
			Content cA = CreateTextBoxContent();
			Content cB = CreateTreeControlContent();
			Content cC = CreateExampleFormContent();
		
			// Setup initial state to match menu selections
			InitializeContent(cA);
			InitializeContent(cB);
			InitializeContent(cC);

            // Request that three content be added in a vertical stack on the left
            WindowContent wc1 = dockingManager.AddContentWithState(cA, Crownwood.DotNetMagic.Docking.State.DockLeft) as WindowContent;
            WindowContent wc2 = dockingManager.AddContentToZone(cB, wc1.ParentZone, 1) as WindowContent;
            WindowContent wc3 = dockingManager.AddContentToZone(cC, wc1.ParentZone, 2) as WindowContent;

            // Alter the relative spacing (make sure they add upto 100!)
            wc1.ZoneArea = 50;
            wc2.ZoneArea = 25;
            wc3.ZoneArea = 25;

            // Force the relayout of content based on new ZoneArea values
            wc1.ParentZone.RelayoutContents();
        }

		private void menuCreate3Window_Click(object sender, System.EventArgs e)
		{
			// Create three different content instances
			Content cA = CreateTextBoxContent();
			Content cB = CreateTreeControlContent();
			Content cC = CreateExampleFormContent();
		
			// Setup initial state to match menu selections
			InitializeContent(cA);
			InitializeContent(cB);
			InitializeContent(cC);

			// Request a new Docking window be created for the first content on the right edge
			WindowContent wc = dockingManager.AddContentWithState(cA, Crownwood.DotNetMagic.Docking.State.DockRight) as WindowContent;
        
			// Add two other content into the same Zone
			dockingManager.AddContentToWindowContent(cB, wc);
			dockingManager.AddContentToWindowContent(cC, wc);
		}

		private void menuCreate3AutoHidden_Click(object sender, System.EventArgs e)
		{
			// Create three different content instances
			Content cA = CreateTextBoxContent();
			Content cB = CreateTreeControlContent();
			Content cC = CreateExampleFormContent();
		
			// Setup initial state to match menu selections
			InitializeContent(cA);
			InitializeContent(cB);
			InitializeContent(cC);

			// Prevent flicker where the contents are added and display and then a fraction of a 
			// second later they become auto hidden. By suspending and then resuming the layout this
			// small flicker can be avoided.
			this.SuspendLayout();

			// Request a new Docking window be created for the first content on the right edge
			WindowContent wc = dockingManager.AddContentWithState(cA, Crownwood.DotNetMagic.Docking.State.DockRight) as WindowContent;
        
			// Add two other content into the same WindowContent
			dockingManager.AddContentToWindowContent(cB, wc);
			dockingManager.AddContentToWindowContent(cC, wc);
		
			// Move all contents in the same window as cA into autohide mode
			dockingManager.ToggleContentAutoHide(cA);
		
			this.ResumeLayout();
		}

		private void menuCreate3Floating_Click(object sender, System.EventArgs e)
		{
			// Create three different content instances
			Content cA = CreateTextBoxContent();
			Content cB = CreateTreeControlContent();
			Content cC = CreateExampleFormContent();
		
			// Make the floating window larger than the default
			cA.FloatingSize = new Size(250, 350);
		
			// Setup initial state to match menu selections
			InitializeContent(cA);
			InitializeContent(cB);
			InitializeContent(cC);

			// Request a new Docking window be created for the first content as floating
			WindowContent wc = dockingManager.AddContentWithState(cA, Crownwood.DotNetMagic.Docking.State.Floating) as WindowContent;
        
			// Add second content into the same Window
			dockingManager.AddContentToWindowContent(cB, wc);
        
			// Add third into same Zone
			dockingManager.AddContentToZone(cC, wc.ParentZone, 1);
		}

        private void toolOffice2007Blue_Click(object sender, EventArgs e)
        {
            ChangeStyle(VisualStyle.Office2007Blue);
            toolOffice2007Blue.Checked = menuOffice2007Blue.Checked = true;
            toolOffice2007Silver.Checked = menuOffice2007Silver.Checked = false;
            toolOffice2007Black.Checked = menuOffice2007Black.Checked = false;
            toolMediaBlue.Checked = menuMediaBlue.Checked = false;
            toolMediaOrange.Checked = menuMediaOrange.Checked = false;
            toolMediaPurple.Checked = menuMediaPurple.Checked = false;
            toolOffice2003.Checked = menuOffice2003.Checked = false;
            toolIDE2005.Checked = menuIDE2005.Checked = false;
            toolPlain.Checked = menuPlain.Checked = false;
        }

        private void toolOffice2007Silver_Click(object sender, EventArgs e)
        {
            ChangeStyle(VisualStyle.Office2007Silver);
            toolOffice2007Blue.Checked = menuOffice2007Blue.Checked = false;
            toolOffice2007Silver.Checked = menuOffice2007Silver.Checked = true;
            toolOffice2007Black.Checked = menuOffice2007Black.Checked = false;
            toolMediaBlue.Checked = menuMediaBlue.Checked = false;
            toolMediaOrange.Checked = menuMediaOrange.Checked = false;
            toolMediaPurple.Checked = menuMediaPurple.Checked = false;
            toolOffice2003.Checked = menuOffice2003.Checked = false;
            toolIDE2005.Checked = menuIDE2005.Checked = false;
            toolPlain.Checked = menuPlain.Checked = false;
        }

        private void toolOffice2007Black_Click(object sender, EventArgs e)
        {
            ChangeStyle(VisualStyle.Office2007Black);
            toolOffice2007Blue.Checked = menuOffice2007Blue.Checked = false;
            toolOffice2007Silver.Checked = menuOffice2007Silver.Checked = false;
            toolOffice2007Black.Checked = menuOffice2007Black.Checked = true;
            toolOffice2003.Checked = menuOffice2003.Checked = false;
            toolMediaBlue.Checked = menuMediaBlue.Checked = false;
            toolMediaOrange.Checked = menuMediaOrange.Checked = false;
            toolMediaPurple.Checked = menuMediaPurple.Checked = false;
            toolIDE2005.Checked = menuIDE2005.Checked = false;
            toolPlain.Checked = menuPlain.Checked = false;
        }

        private void toolMediaBlue_Click(object sender, EventArgs e)
        {
            ChangeStyle(VisualStyle.MediaPlayerBlue);
            toolOffice2007Blue.Checked = menuOffice2007Blue.Checked = false;
            toolOffice2007Silver.Checked = menuOffice2007Silver.Checked = false;
            toolOffice2007Black.Checked = menuOffice2007Black.Checked = false;
            toolMediaBlue.Checked = menuMediaBlue.Checked = true;
            toolMediaOrange.Checked = menuMediaOrange.Checked = false;
            toolMediaPurple.Checked = menuMediaPurple.Checked = false;
            toolOffice2003.Checked = menuOffice2003.Checked = false;
            toolIDE2005.Checked = menuIDE2005.Checked = false;
            toolPlain.Checked = menuPlain.Checked = false;
        }

        private void toolMediaOrange_Click(object sender, EventArgs e)
        {
            ChangeStyle(VisualStyle.MediaPlayerOrange);
            toolOffice2007Blue.Checked = menuOffice2007Blue.Checked = false;
            toolOffice2007Silver.Checked = menuOffice2007Silver.Checked = false;
            toolOffice2007Black.Checked = menuOffice2007Black.Checked = false;
            toolMediaBlue.Checked = menuMediaBlue.Checked = false;
            toolMediaOrange.Checked = menuMediaOrange.Checked = true;
            toolMediaPurple.Checked = menuMediaPurple.Checked = false;
            toolOffice2003.Checked = menuOffice2003.Checked = false;
            toolIDE2005.Checked = menuIDE2005.Checked = false;
            toolPlain.Checked = menuPlain.Checked = false;
        }

        private void toolMediaPurple_Click(object sender, EventArgs e)
        {
            ChangeStyle(VisualStyle.MediaPlayerPurple);
            toolOffice2007Blue.Checked = menuOffice2007Blue.Checked = false;
            toolOffice2007Silver.Checked = menuOffice2007Silver.Checked = false;
            toolOffice2007Black.Checked = menuOffice2007Black.Checked = false;
            toolMediaBlue.Checked = menuMediaBlue.Checked = false;
            toolMediaOrange.Checked = menuMediaOrange.Checked = false;
            toolMediaPurple.Checked = menuMediaPurple.Checked = true;
            toolOffice2003.Checked = menuOffice2003.Checked = false;
            toolIDE2005.Checked = menuIDE2005.Checked = false;
            toolPlain.Checked = menuPlain.Checked = false;
        }

        private void toolOffice2003_Click(object sender, EventArgs e)
        {
            ChangeStyle(VisualStyle.Office2003);
            toolOffice2007Blue.Checked = menuOffice2007Blue.Checked = false;
            toolOffice2007Silver.Checked = menuOffice2007Silver.Checked = false;
            toolOffice2007Black.Checked = menuOffice2007Black.Checked = false;
            toolMediaBlue.Checked = menuMediaBlue.Checked = false;
            toolMediaOrange.Checked = menuMediaOrange.Checked = false;
            toolMediaPurple.Checked = menuMediaPurple.Checked = false;
            toolOffice2003.Checked = menuOffice2003.Checked = true;
            toolIDE2005.Checked = menuIDE2005.Checked = false;
            toolPlain.Checked = menuPlain.Checked = false;
        }

        private void toolIDE2005_Click(object sender, EventArgs e)
        {
            ChangeStyle(VisualStyle.IDE2005);
            toolOffice2007Blue.Checked = menuOffice2007Blue.Checked = false;
            toolOffice2007Silver.Checked = menuOffice2007Silver.Checked = false;
            toolOffice2007Black.Checked = menuOffice2007Black.Checked = false;
            toolMediaBlue.Checked = menuMediaBlue.Checked = false;
            toolMediaOrange.Checked = menuMediaOrange.Checked = false;
            toolMediaPurple.Checked = menuMediaPurple.Checked = false;
            toolIDE2005.Checked = menuIDE2005.Checked = true;
            toolOffice2003.Checked = menuOffice2003.Checked = false;
            toolPlain.Checked = menuPlain.Checked = false;
        }

        private void toolPlain_Click(object sender, EventArgs e)
        {
            ChangeStyle(VisualStyle.Plain);
            toolOffice2007Blue.Checked = menuOffice2007Blue.Checked = false;
            toolOffice2007Silver.Checked = menuOffice2007Silver.Checked = false;
            toolOffice2007Black.Checked = menuOffice2007Black.Checked = false;
            toolMediaBlue.Checked = menuMediaBlue.Checked = false;
            toolMediaOrange.Checked = menuMediaOrange.Checked = false;
            toolMediaPurple.Checked = menuMediaPurple.Checked = false;
            toolPlain.Checked = menuPlain.Checked = true;
            toolOffice2003.Checked = menuOffice2003.Checked = false;
            toolIDE2005.Checked = menuIDE2005.Checked = false;
        }

		private void menuHideAll_Click(object sender, System.EventArgs e)
		{
			dockingManager.HideAllContents();
		}

		private void toolHideAll_Click(object sender, System.EventArgs e)
		{
			dockingManager.HideAllContents();
		}

		private void menuShowAll_Click(object sender, System.EventArgs e)
		{
			dockingManager.ShowAllContents();
		}

		private void toolShowAll_Click(object sender, System.EventArgs e)
		{
			dockingManager.ShowAllContents();
		}

        private void menuFocusRectangles_Click(object sender, EventArgs e)
        {
            dockingManager.FeedbackStyle = DragFeedbackStyle.Outline;
            menuFocusRectangles.Checked = true;
            menuSemitransparentBlocks.Checked = false;
            menuDiamondIndicators.Checked = false;
            menuSquareIndicators.Checked = false;
        }

        private void menuSemitransparentBlocks_Click(object sender, EventArgs e)
        {
            dockingManager.FeedbackStyle = DragFeedbackStyle.Solid;
            menuSemitransparentBlocks.Checked = true;
            menuFocusRectangles.Checked = false;
            menuDiamondIndicators.Checked = false;
            menuSquareIndicators.Checked = false;
        }

        private void menuDiamondIndicators_Click(object sender, EventArgs e)
        {
            dockingManager.FeedbackStyle = DragFeedbackStyle.Diamonds;
            menuDiamondIndicators.Checked = true;
            menuSemitransparentBlocks.Checked = false;
            menuFocusRectangles.Checked = false;
            menuSquareIndicators.Checked = false;
        }

        private void menuSquareIndicators_Click(object sender, EventArgs e)
        {
            dockingManager.FeedbackStyle = DragFeedbackStyle.Squares;
            menuSquareIndicators.Checked = true;
            menuDiamondIndicators.Checked = false;
            menuSemitransparentBlocks.Checked = false;
            menuFocusRectangles.Checked = false;
        }

		private void menuAllowRedocking_Click(object sender, System.EventArgs e)
		{
			dockingManager.AllowRedocking = !dockingManager.AllowRedocking;
            menuAllowRedocking.Checked = !menuAllowRedocking.Checked;
		}

        private void menuAllowFloating_Click(object sender, System.EventArgs e)
        {
            dockingManager.AllowFloating = !dockingManager.AllowFloating;
            menuAllowFloating.Checked = !menuAllowFloating.Checked;
        }

        private void menuAllowResize_Click(object sender, EventArgs e)
        {
            dockingManager.AllowResize = !dockingManager.AllowResize;
            menuAllowResize.Checked = !menuAllowResize.Checked;
        }

        private void menuAllowTabbed_Click(object sender, EventArgs e)
        {
            dockingManager.AllowTabbed = !dockingManager.AllowTabbed;
            menuAllowTabbed.Checked = !menuAllowTabbed.Checked;
        }

        private void menuDeleteAll_Click(object sender, System.EventArgs e)
		{
			dockingManager.Contents.Clear();
		}

		private void menuSaveConfig1_Click(object sender, System.EventArgs e)
		{
			dockingManager.SaveConfigToFile("config1.xml");
		}

		private void menuSaveConfig2_Click(object sender, System.EventArgs e)
		{
			dockingManager.SaveConfigToFile("config2.xml");
		}

		private void menuLoadConfig1_Click(object sender, System.EventArgs e)
		{
			dockingManager.LoadConfigFromFile("config1.xml");
		}

		private void menuLoadConfig2_Click(object sender, System.EventArgs e)
		{
			dockingManager.LoadConfigFromFile("config2.xml");
		}

		private void menuSaveArray1_Click(object sender, System.EventArgs e)
		{
			slot1 = dockingManager.SaveConfigToArray();
		}

		private void menuSaveArray2_Click(object sender, System.EventArgs e)
		{
			slot2 = dockingManager.SaveConfigToArray();
		}

		private void menuLoadArray1_Click(object sender, System.EventArgs e)
		{
			if (slot1 != null)
				dockingManager.LoadConfigFromArray(slot1);
		}

		private void menuLoadArray2_Click(object sender, System.EventArgs e)
		{
			if (slot2 != null)
				dockingManager.LoadConfigFromArray(slot2);
		}

		private void menuAllowContext_Click(object sender, System.EventArgs e)
		{
			allowContextMenu = (allowContextMenu == false);
            menuAllowContextMenu.Checked = allowContextMenu;
		}

        private void menuCustomizeContext_Click(object sender, EventArgs e)
        {
            customContextMenu = (customContextMenu == false);
            menuCustomizeContextMenu.Checked = !menuCustomizeContextMenu.Checked;
        }

		private void menuResizeDefault_Click(object sender, System.EventArgs e)
		{
			dockingManager.ResizeBarVector = -1;
            menuDefaultResizeBar.Checked = true;
            menu1PixelResizeBar.Checked = false;
            menu5PixelResizeBar.Checked = false;
            menu7PixelResizeBar.Checked = false;
		}

		private void menuResize1_Click(object sender, System.EventArgs e)
		{
			dockingManager.ResizeBarVector = 1;
            menu1PixelResizeBar.Checked = true;
            menuDefaultResizeBar.Checked = false;
            menu5PixelResizeBar.Checked = false;
            menu7PixelResizeBar.Checked = false;
        }

		private void menuResize5_Click(object sender, System.EventArgs e)
		{
			dockingManager.ResizeBarVector = 5;
            menu5PixelResizeBar.Checked = true;
            menu1PixelResizeBar.Checked = false;
            menuDefaultResizeBar.Checked = false;
            menu7PixelResizeBar.Checked = false;
        }		

		private void menuResize7_Click(object sender, System.EventArgs e)
		{
			dockingManager.ResizeBarVector = 7;
            menu7PixelResizeBar.Checked = true;
            menu5PixelResizeBar.Checked = false;
            menu1PixelResizeBar.Checked = false;
            menuDefaultResizeBar.Checked = false;
        }

		private void menuShowCaptions_Click(object sender, System.EventArgs e)
		{
			captionBars = (captionBars == false);
        
			foreach(Content c in dockingManager.Contents)
				c.CaptionBar = captionBars;

            menuShowCaptionBars.Checked = captionBars;
        }

		private void menuShowClose_Click(object sender, System.EventArgs e)
		{
			closeButtons = (closeButtons == false);
        
			foreach(Content c in dockingManager.Contents)
				c.CloseButton = closeButtons;

            menuShowCloseButtons.Checked = closeButtons;
		}

		private void menuAllowAllClose_Click(object sender, System.EventArgs e)
		{
			ignoreClose	= 0;
            menuAllowAllCloseButtons.Checked = true;
            menuIgnoreTreeControlCloseButtons.Checked = false;
            menuIgnoreTextBoxCloseButtons.Checked = false;
            menuIgnoreExampleFormCloseButtons.Checked = false;
            menuIgnoreAllCloseButtons.Checked = false;
		}

		private void menuIgnoreTreeControlClose_Click(object sender, System.EventArgs e)
		{
			ignoreClose	= 1;
            menuIgnoreTreeControlCloseButtons.Checked = true;
            menuAllowAllCloseButtons.Checked = false;
            menuIgnoreTextBoxCloseButtons.Checked = false;
            menuIgnoreExampleFormCloseButtons.Checked = false;
            menuIgnoreAllCloseButtons.Checked = false;
        }

		private void menuIgnoreTextBoxClose_Click(object sender, System.EventArgs e)
		{
			ignoreClose	= 2;
            menuIgnoreTextBoxCloseButtons.Checked = true;
            menuAllowAllCloseButtons.Checked = false;
            menuIgnoreTreeControlCloseButtons.Checked = false;
            menuIgnoreExampleFormCloseButtons.Checked = false;
            menuIgnoreAllCloseButtons.Checked = false;
        }

        private void menuIgnoreExampleFormClose_Click(object sender, System.EventArgs e)
        {
            ignoreClose = 3;
            menuIgnoreExampleFormCloseButtons.Checked = true;
            menuIgnoreTextBoxCloseButtons.Checked = false;
            menuAllowAllCloseButtons.Checked = false;
            menuIgnoreTreeControlCloseButtons.Checked = false;
            menuIgnoreAllCloseButtons.Checked = false;
        }

        private void menuIgnoreAllClose_Click(object sender, System.EventArgs e)
        {
            ignoreClose = 4;
            menuIgnoreAllCloseButtons.Checked = true;
            menuIgnoreExampleFormCloseButtons.Checked = false;
            menuIgnoreTextBoxCloseButtons.Checked = false;
            menuAllowAllCloseButtons.Checked = false;
            menuIgnoreTreeControlCloseButtons.Checked = false;
        }

        private void menuAllowMinMaxFunctionality_Click(object sender, EventArgs e)
        {
            dockingManager.ZoneMinMax = (dockingManager.ZoneMinMax == false);
            menuAllowMinMaxFunctionality.Checked = dockingManager.ZoneMinMax;
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region Docking Manager Event Handlers
        private void OnSaveConfig(XmlTextWriter xmlOut)
		{
			// Add an extra node into the config to store some useless information
			xmlOut.WriteStartElement("MyCustomElement");
			xmlOut.WriteAttributeString("UselessData1", "Hello");
			xmlOut.WriteAttributeString("UselessData2", "World!");
			xmlOut.WriteEndElement();
		}

		private void OnLoadConfig(XmlTextReader xmlIn)
		{
			// We are expecting our custom element to be the current one
			if (xmlIn.Name == "MyCustomElement")
			{
				// Read in both the expected attributes
				string attr1 = xmlIn.GetAttribute(0);
				string attr2 = xmlIn.GetAttribute(1);
            
				// Must move past our element
				xmlIn.Read();
			}
		}

		protected void OnContentShown(Content c, EventArgs cea)
		{
			Console.WriteLine("OnContentShown {0}", c.Title);
		}

		protected void OnContentHidden(Content c, EventArgs cea)
		{
			Console.WriteLine("OnContentHidden {0}", c.Title);
		}
    
		protected void OnContentHiding(Content c, CancelEventArgs cea)
		{
			Console.WriteLine("OnContentHiding {0}", c.Title);
    
			switch(ignoreClose)
			{
				case 0:
					// Allow all, do nothing
					break;
				case 1:
					// Ignore TreeControl
					cea.Cancel = c.Control is TreeControl;
					break;
				case 2:
					// Ignore TextBox
					cea.Cancel = c.Control is TextBox;
					break;
				case 3:
					// Ignore ExampleForm
					cea.Cancel = c.Control is ExampleForm;
					break;
				case 4:
					// Ignore all, cancel
					cea.Cancel = true;
					break;
			}
		}

        protected void OnContentAutoHideOpening(Content c, EventArgs cea)
        {
            Console.WriteLine("OnContentAutoHideOpening {0}", c.Title);
        }

        protected void OnContentAutoHideClosed(Content c, EventArgs cea)
        {
            Console.WriteLine("OnContentAutoHideClosed {0}", c.Title);
        }

        protected void OnLayoutChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Docking layout changed");
        }

		private void OnContextMenu(ContentCollection cc,
                                   ContextMenuStrip cms, 
                                   CancelEventArgs cea)
		{
			// Show the PopupMenu be cancelled and not shown?
			if (!allowContextMenu)
				cea.Cancel = true;
			else
			{        
				if (customContextMenu)
				{
					// Remove the Show All and Hide All commands
                    cms.Items.RemoveAt(cms.Items.Count - 1);
                    cms.Items.RemoveAt(cms.Items.Count - 1);
                
					// Add a custom entry at the start
                    cms.Items.Insert(0, new ToolStripMenuItem("Custom 1"));
                    cms.Items.Insert(1, new ToolStripSeparator());
                
					// Add a couple of custom commands at the end
                    cms.Items.Add(new ToolStripMenuItem("Custom 2"));
                    cms.Items.Add(new ToolStripMenuItem("Custom 3"));
				}
			}
        }
        #endregion
    }
}
