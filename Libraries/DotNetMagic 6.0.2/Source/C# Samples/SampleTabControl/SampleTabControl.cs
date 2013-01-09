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
using System.IO;
using System.Drawing;
using System.Resources;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Crownwood.DotNetMagic.Common;
using Crownwood.DotNetMagic.Controls;
using Crownwood.DotNetMagic.Win32;

namespace SampleTabControl
{
    public class SampleTabControl : System.Windows.Forms.Form
    {
        private static int _count = 0;
        
		private bool _reenter;
        private ImageList _internalImages;
        private string[] _strings = new string[]{"P&roperties", 
                                                 "Solution Explo&rer", 
                                                 "&Task List", 
                                                 "&Command Window", 
                                                 "Callstack", 
                                                 "B&reakpoints", 
                                                 "Output"};
        private string[] _tooltips = new string[]{"Object Properties", 
                                                  "View Solution Explorer", 
                                                  "Current Task List", 
                                                  "Command Window details", 
                                                  "Current callstack", 
                                                  "Defined breakpoints", 
                                                  "Output details"};
        private System.Windows.Forms.Button addPage;
        private System.Windows.Forms.Button removePage;
        private System.Windows.Forms.Button clearAll;
        private System.Windows.Forms.CheckBox positionAtTop;
        private System.Windows.Forms.CheckBox hotTrack;
        private System.Windows.Forms.CheckBox shrinkPages;
        private System.Windows.Forms.CheckBox showClose;
        private System.Windows.Forms.CheckBox showArrows;
        private System.Windows.Forms.CheckBox insetPlain;
        private System.Windows.Forms.CheckBox insetPagesOnly;
        private System.Windows.Forms.CheckBox selectedTextOnly;
        private System.Windows.Forms.CheckBox hoverSelect;
        private System.Windows.Forms.CheckBox idePixelBorder;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.RadioButton radioPlain;
        private System.Windows.Forms.RadioButton radioMultiBox;
        private System.Windows.Forms.RadioButton radioMultiForm;
        private System.Windows.Forms.RadioButton radioMultiDocument;
        private System.Windows.Forms.CheckBox idePixelArea;
        private System.Windows.Forms.CheckBox multiLine;
        private Crownwood.DotNetMagic.Controls.TabControl tabControl;
        private System.Windows.Forms.RadioButton tabHideUsingLogic;
        private System.Windows.Forms.RadioButton tabHideAlways;
        private System.Windows.Forms.RadioButton tabShowAlways;
        private System.Windows.Forms.RadioButton tabHideWithoutMouse;
        private System.Windows.Forms.CheckBox multilineFullWidth;
		private System.Windows.Forms.CheckBox checkBoxTooltips;
		private System.Windows.Forms.GroupBox tabGroupStyle;
		private System.Windows.Forms.GroupBox tabGroupAppearance;
		private System.Windows.Forms.GroupBox groupBoxHideTabsMode;
		private System.Windows.Forms.GroupBox groupBoxOffset;
		private System.Windows.Forms.GroupBox groupBoxSingleMulti;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.GroupBox groupBoxTabControl;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.GroupBox groupBoxDrag;
		private System.Windows.Forms.CheckBox checkBoxReorder;
		private System.Windows.Forms.CheckBox checkBoxDragOut;
		private System.Windows.Forms.CheckBox checkBoxTextTips;
		private System.Windows.Forms.RadioButton radioOffice2003;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.ComboBox comboBoxOfficeStyle;
		private System.Windows.Forms.CheckBox checkOfficeHeaderBorder;
		private System.Windows.Forms.CheckBox checkOfficePixelBorder;
		private System.Windows.Forms.RadioButton radioIDE2005;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.CheckBox checkIDE2005HeaderBorder;
		private System.Windows.Forms.CheckBox checkIDE2005PixelBorder;
		private System.Windows.Forms.ComboBox comboBoxIDE2005Style;
        private System.Windows.Forms.CheckBox showDropSelect;
        private RadioButton radioOffice2007Black;
        private RadioButton radioOffice2007Silver;
        private RadioButton radioOffice2007Blue;
        private RadioButton radioMediaPlayerPurple;
        private RadioButton radioMediaPlayerOrange;
        private RadioButton radioMediaPlayerBlue;
        private GroupBox groupBox6;
        private CheckBox checkMediaPlayerPixelBorder;
        private CheckBox checkMediaPlayerHeaderBorder;
        private ComboBox comboBoxMediaPlayerStyle;
		private System.Windows.Forms.CheckBox checkIDE2005TabJoined;

        public SampleTabControl()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // Create a strip of images by loading an embedded bitmap resource
            _internalImages = ResourceHelper.LoadBitmapStrip(this.GetType(),
                                                             "SampleTabControl.SampleImages.bmp",
                                                             new Size(16,16),
                                                             new Point(0,0));


            // Hook into the close button
            tabControl.ClosePressed += new EventHandler(OnRemovePage);
	
            UpdateControls();
        }

        protected override void Dispose( bool disposing )
        {
            base.Dispose( disposing );
        }

		#region Windows Form Designer generated code
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SampleTabControl));
            this.selectedTextOnly = new System.Windows.Forms.CheckBox();
            this.positionAtTop = new System.Windows.Forms.CheckBox();
            this.radioMultiBox = new System.Windows.Forms.RadioButton();
            this.removePage = new System.Windows.Forms.Button();
            this.hotTrack = new System.Windows.Forms.CheckBox();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.hoverSelect = new System.Windows.Forms.CheckBox();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.radioMultiForm = new System.Windows.Forms.RadioButton();
            this.showClose = new System.Windows.Forms.CheckBox();
            this.shrinkPages = new System.Windows.Forms.CheckBox();
            this.addPage = new System.Windows.Forms.Button();
            this.clearAll = new System.Windows.Forms.Button();
            this.tabGroupStyle = new System.Windows.Forms.GroupBox();
            this.radioMediaPlayerPurple = new System.Windows.Forms.RadioButton();
            this.radioMediaPlayerOrange = new System.Windows.Forms.RadioButton();
            this.radioMediaPlayerBlue = new System.Windows.Forms.RadioButton();
            this.radioOffice2007Black = new System.Windows.Forms.RadioButton();
            this.radioOffice2007Silver = new System.Windows.Forms.RadioButton();
            this.radioOffice2007Blue = new System.Windows.Forms.RadioButton();
            this.radioIDE2005 = new System.Windows.Forms.RadioButton();
            this.radioOffice2003 = new System.Windows.Forms.RadioButton();
            this.radioPlain = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.insetPlain = new System.Windows.Forms.CheckBox();
            this.insetPagesOnly = new System.Windows.Forms.CheckBox();
            this.showArrows = new System.Windows.Forms.CheckBox();
            this.radioMultiDocument = new System.Windows.Forms.RadioButton();
            this.tabGroupAppearance = new System.Windows.Forms.GroupBox();
            this.idePixelBorder = new System.Windows.Forms.CheckBox();
            this.idePixelArea = new System.Windows.Forms.CheckBox();
            this.multiLine = new System.Windows.Forms.CheckBox();
            this.groupBoxHideTabsMode = new System.Windows.Forms.GroupBox();
            this.tabHideWithoutMouse = new System.Windows.Forms.RadioButton();
            this.tabHideUsingLogic = new System.Windows.Forms.RadioButton();
            this.tabHideAlways = new System.Windows.Forms.RadioButton();
            this.tabShowAlways = new System.Windows.Forms.RadioButton();
            this.multilineFullWidth = new System.Windows.Forms.CheckBox();
            this.checkBoxTooltips = new System.Windows.Forms.CheckBox();
            this.groupBoxOffset = new System.Windows.Forms.GroupBox();
            this.groupBoxSingleMulti = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.showDropSelect = new System.Windows.Forms.CheckBox();
            this.checkBoxTextTips = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBoxTabControl = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl = new Crownwood.DotNetMagic.Controls.TabControl();
            this.groupBoxDrag = new System.Windows.Forms.GroupBox();
            this.checkBoxDragOut = new System.Windows.Forms.CheckBox();
            this.checkBoxReorder = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkOfficePixelBorder = new System.Windows.Forms.CheckBox();
            this.checkOfficeHeaderBorder = new System.Windows.Forms.CheckBox();
            this.comboBoxOfficeStyle = new System.Windows.Forms.ComboBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.checkIDE2005TabJoined = new System.Windows.Forms.CheckBox();
            this.checkIDE2005PixelBorder = new System.Windows.Forms.CheckBox();
            this.checkIDE2005HeaderBorder = new System.Windows.Forms.CheckBox();
            this.comboBoxIDE2005Style = new System.Windows.Forms.ComboBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.checkMediaPlayerPixelBorder = new System.Windows.Forms.CheckBox();
            this.checkMediaPlayerHeaderBorder = new System.Windows.Forms.CheckBox();
            this.comboBoxMediaPlayerStyle = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.tabGroupStyle.SuspendLayout();
            this.tabGroupAppearance.SuspendLayout();
            this.groupBoxHideTabsMode.SuspendLayout();
            this.groupBoxOffset.SuspendLayout();
            this.groupBoxSingleMulti.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBoxTabControl.SuspendLayout();
            this.groupBoxDrag.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // selectedTextOnly
            // 
            this.selectedTextOnly.Location = new System.Drawing.Point(16, 160);
            this.selectedTextOnly.Name = "selectedTextOnly";
            this.selectedTextOnly.Size = new System.Drawing.Size(120, 24);
            this.selectedTextOnly.TabIndex = 1;
            this.selectedTextOnly.Text = "Selected Text Only";
            this.selectedTextOnly.CheckedChanged += new System.EventHandler(this.selectedTextOnly_CheckedChanged);
            // 
            // positionAtTop
            // 
            this.positionAtTop.Location = new System.Drawing.Point(16, 16);
            this.positionAtTop.Name = "positionAtTop";
            this.positionAtTop.Size = new System.Drawing.Size(112, 24);
            this.positionAtTop.TabIndex = 1;
            this.positionAtTop.Text = "Position At Top";
            this.positionAtTop.CheckedChanged += new System.EventHandler(this.positionAtTop_CheckedChanged);
            // 
            // radioMultiBox
            // 
            this.radioMultiBox.Location = new System.Drawing.Point(11, 66);
            this.radioMultiBox.Name = "radioMultiBox";
            this.radioMultiBox.Size = new System.Drawing.Size(88, 24);
            this.radioMultiBox.TabIndex = 0;
            this.radioMultiBox.Text = "MultiBox";
            this.radioMultiBox.CheckedChanged += new System.EventHandler(this.appearanceMultiBox_CheckChanged);
            // 
            // removePage
            // 
            this.removePage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removePage.BackColor = System.Drawing.SystemColors.Control;
            this.removePage.Location = new System.Drawing.Point(112, 196);
            this.removePage.Name = "removePage";
            this.removePage.Size = new System.Drawing.Size(88, 24);
            this.removePage.TabIndex = 0;
            this.removePage.Text = "RemovePage";
            this.removePage.UseVisualStyleBackColor = true;
            this.removePage.Click += new System.EventHandler(this.OnRemovePage);
            // 
            // hotTrack
            // 
            this.hotTrack.Location = new System.Drawing.Point(16, 184);
            this.hotTrack.Name = "hotTrack";
            this.hotTrack.Size = new System.Drawing.Size(96, 24);
            this.hotTrack.TabIndex = 1;
            this.hotTrack.Text = "HotTrack";
            this.hotTrack.CheckedChanged += new System.EventHandler(this.Highlight_CheckedChanged);
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(88, 68);
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(48, 20);
            this.numericUpDown3.TabIndex = 2;
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.numericUpDown3_ValueChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(88, 20);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(48, 20);
            this.numericUpDown1.TabIndex = 2;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.Location = new System.Drawing.Point(88, 92);
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(48, 20);
            this.numericUpDown4.TabIndex = 2;
            this.numericUpDown4.ValueChanged += new System.EventHandler(this.numericUpDown4_ValueChanged);
            // 
            // hoverSelect
            // 
            this.hoverSelect.Location = new System.Drawing.Point(16, 136);
            this.hoverSelect.Name = "hoverSelect";
            this.hoverSelect.Size = new System.Drawing.Size(104, 24);
            this.hoverSelect.TabIndex = 1;
            this.hoverSelect.Text = "Hover Select";
            this.hoverSelect.CheckedChanged += new System.EventHandler(this.hoverSelect_CheckedChanged);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(88, 44);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(48, 20);
            this.numericUpDown2.TabIndex = 2;
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // radioMultiForm
            // 
            this.radioMultiForm.Location = new System.Drawing.Point(11, 42);
            this.radioMultiForm.Name = "radioMultiForm";
            this.radioMultiForm.Size = new System.Drawing.Size(88, 24);
            this.radioMultiForm.TabIndex = 0;
            this.radioMultiForm.Text = "MultiForm";
            this.radioMultiForm.CheckedChanged += new System.EventHandler(this.appearanceMultiForm_CheckChanged);
            // 
            // showClose
            // 
            this.showClose.Location = new System.Drawing.Point(16, 64);
            this.showClose.Name = "showClose";
            this.showClose.Size = new System.Drawing.Size(96, 24);
            this.showClose.TabIndex = 1;
            this.showClose.Text = "Show Close";
            this.showClose.CheckedChanged += new System.EventHandler(this.showClose_CheckedChanged);
            // 
            // shrinkPages
            // 
            this.shrinkPages.Location = new System.Drawing.Point(16, 40);
            this.shrinkPages.Name = "shrinkPages";
            this.shrinkPages.Size = new System.Drawing.Size(96, 24);
            this.shrinkPages.TabIndex = 1;
            this.shrinkPages.Text = "Shrink pages";
            this.shrinkPages.CheckedChanged += new System.EventHandler(this.shrinkPages_CheckedChanged);
            // 
            // addPage
            // 
            this.addPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addPage.Location = new System.Drawing.Point(16, 196);
            this.addPage.Name = "addPage";
            this.addPage.Size = new System.Drawing.Size(88, 24);
            this.addPage.TabIndex = 0;
            this.addPage.Text = "AddPage";
            this.addPage.UseVisualStyleBackColor = true;
            this.addPage.Click += new System.EventHandler(this.OnAddPage);
            // 
            // clearAll
            // 
            this.clearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.clearAll.BackColor = System.Drawing.SystemColors.Control;
            this.clearAll.Location = new System.Drawing.Point(208, 196);
            this.clearAll.Name = "clearAll";
            this.clearAll.Size = new System.Drawing.Size(88, 24);
            this.clearAll.TabIndex = 0;
            this.clearAll.Text = "RemoveAll";
            this.clearAll.UseVisualStyleBackColor = true;
            this.clearAll.Click += new System.EventHandler(this.OnClearAll);
            // 
            // tabGroupStyle
            // 
            this.tabGroupStyle.Controls.Add(this.radioMediaPlayerPurple);
            this.tabGroupStyle.Controls.Add(this.radioMediaPlayerOrange);
            this.tabGroupStyle.Controls.Add(this.radioMediaPlayerBlue);
            this.tabGroupStyle.Controls.Add(this.radioOffice2007Black);
            this.tabGroupStyle.Controls.Add(this.radioOffice2007Silver);
            this.tabGroupStyle.Controls.Add(this.radioOffice2007Blue);
            this.tabGroupStyle.Controls.Add(this.radioIDE2005);
            this.tabGroupStyle.Controls.Add(this.radioOffice2003);
            this.tabGroupStyle.Controls.Add(this.radioPlain);
            this.tabGroupStyle.Location = new System.Drawing.Point(8, 8);
            this.tabGroupStyle.Name = "tabGroupStyle";
            this.tabGroupStyle.Size = new System.Drawing.Size(144, 243);
            this.tabGroupStyle.TabIndex = 0;
            this.tabGroupStyle.TabStop = false;
            this.tabGroupStyle.Text = "Style";
            // 
            // radioMediaPlayerPurple
            // 
            this.radioMediaPlayerPurple.Location = new System.Drawing.Point(11, 141);
            this.radioMediaPlayerPurple.Name = "radioMediaPlayerPurple";
            this.radioMediaPlayerPurple.Size = new System.Drawing.Size(133, 24);
            this.radioMediaPlayerPurple.TabIndex = 8;
            this.radioMediaPlayerPurple.Text = "Media Player Purple";
            this.radioMediaPlayerPurple.CheckedChanged += new System.EventHandler(this.radioMediaPlayerPurple_CheckedChanged);
            // 
            // radioMediaPlayerOrange
            // 
            this.radioMediaPlayerOrange.Location = new System.Drawing.Point(11, 117);
            this.radioMediaPlayerOrange.Name = "radioMediaPlayerOrange";
            this.radioMediaPlayerOrange.Size = new System.Drawing.Size(133, 24);
            this.radioMediaPlayerOrange.TabIndex = 7;
            this.radioMediaPlayerOrange.Text = "Media Player Orange";
            this.radioMediaPlayerOrange.CheckedChanged += new System.EventHandler(this.radioMediaPlayerOrange_CheckedChanged);
            // 
            // radioMediaPlayerBlue
            // 
            this.radioMediaPlayerBlue.Location = new System.Drawing.Point(11, 93);
            this.radioMediaPlayerBlue.Name = "radioMediaPlayerBlue";
            this.radioMediaPlayerBlue.Size = new System.Drawing.Size(133, 24);
            this.radioMediaPlayerBlue.TabIndex = 6;
            this.radioMediaPlayerBlue.Text = "Media Player Blue";
            this.radioMediaPlayerBlue.CheckedChanged += new System.EventHandler(this.radioMediaPlayerBlue_CheckedChanged);
            // 
            // radioOffice2007Black
            // 
            this.radioOffice2007Black.Location = new System.Drawing.Point(11, 69);
            this.radioOffice2007Black.Name = "radioOffice2007Black";
            this.radioOffice2007Black.Size = new System.Drawing.Size(133, 24);
            this.radioOffice2007Black.TabIndex = 5;
            this.radioOffice2007Black.Text = "Office2007 Black";
            this.radioOffice2007Black.CheckedChanged += new System.EventHandler(this.radioOffice2007Black_CheckedChanged);
            // 
            // radioOffice2007Silver
            // 
            this.radioOffice2007Silver.Location = new System.Drawing.Point(11, 45);
            this.radioOffice2007Silver.Name = "radioOffice2007Silver";
            this.radioOffice2007Silver.Size = new System.Drawing.Size(133, 24);
            this.radioOffice2007Silver.TabIndex = 4;
            this.radioOffice2007Silver.Text = "Office2007 Silver";
            this.radioOffice2007Silver.CheckedChanged += new System.EventHandler(this.radioOffice2007Silver_CheckedChanged);
            // 
            // radioOffice2007Blue
            // 
            this.radioOffice2007Blue.Location = new System.Drawing.Point(11, 21);
            this.radioOffice2007Blue.Name = "radioOffice2007Blue";
            this.radioOffice2007Blue.Size = new System.Drawing.Size(133, 24);
            this.radioOffice2007Blue.TabIndex = 3;
            this.radioOffice2007Blue.Text = "Office2007 Blue";
            this.radioOffice2007Blue.CheckedChanged += new System.EventHandler(this.radioOffice2007Blue_CheckedChanged);
            // 
            // radioIDE2005
            // 
            this.radioIDE2005.Location = new System.Drawing.Point(11, 189);
            this.radioIDE2005.Name = "radioIDE2005";
            this.radioIDE2005.Size = new System.Drawing.Size(133, 24);
            this.radioIDE2005.TabIndex = 2;
            this.radioIDE2005.Text = "IDE2005";
            this.radioIDE2005.CheckedChanged += new System.EventHandler(this.radioIDE2005_CheckedChanged);
            // 
            // radioOffice2003
            // 
            this.radioOffice2003.Location = new System.Drawing.Point(11, 165);
            this.radioOffice2003.Name = "radioOffice2003";
            this.radioOffice2003.Size = new System.Drawing.Size(133, 24);
            this.radioOffice2003.TabIndex = 1;
            this.radioOffice2003.Text = "Office2003";
            this.radioOffice2003.CheckedChanged += new System.EventHandler(this.radioOffice2003_CheckedChanged);
            // 
            // radioPlain
            // 
            this.radioPlain.Location = new System.Drawing.Point(11, 213);
            this.radioPlain.Name = "radioPlain";
            this.radioPlain.Size = new System.Drawing.Size(133, 24);
            this.radioPlain.TabIndex = 0;
            this.radioPlain.Text = "Plain";
            this.radioPlain.CheckedChanged += new System.EventHandler(this.stylePlain_CheckChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "Bottom Offset";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "Left Offset";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Right Offset";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "Top Offset";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // insetPlain
            // 
            this.insetPlain.Location = new System.Drawing.Point(123, 24);
            this.insetPlain.Name = "insetPlain";
            this.insetPlain.Size = new System.Drawing.Size(88, 24);
            this.insetPlain.TabIndex = 1;
            this.insetPlain.Text = "Inset Plain";
            this.insetPlain.CheckedChanged += new System.EventHandler(this.insetPlain_CheckedChanged);
            // 
            // insetPagesOnly
            // 
            this.insetPagesOnly.Location = new System.Drawing.Point(11, 24);
            this.insetPagesOnly.Name = "insetPagesOnly";
            this.insetPagesOnly.Size = new System.Drawing.Size(112, 24);
            this.insetPagesOnly.TabIndex = 4;
            this.insetPagesOnly.Text = "Inset Pages Only";
            this.insetPagesOnly.CheckedChanged += new System.EventHandler(this.insetPagesOnly_CheckedChanged);
            // 
            // showArrows
            // 
            this.showArrows.Location = new System.Drawing.Point(16, 88);
            this.showArrows.Name = "showArrows";
            this.showArrows.Size = new System.Drawing.Size(96, 24);
            this.showArrows.TabIndex = 1;
            this.showArrows.Text = "Show Arrows";
            this.showArrows.CheckedChanged += new System.EventHandler(this.showArrows_CheckedChanged);
            // 
            // radioMultiDocument
            // 
            this.radioMultiDocument.Location = new System.Drawing.Point(11, 19);
            this.radioMultiDocument.Name = "radioMultiDocument";
            this.radioMultiDocument.Size = new System.Drawing.Size(104, 24);
            this.radioMultiDocument.TabIndex = 0;
            this.radioMultiDocument.Text = "MultiDocument";
            this.radioMultiDocument.CheckedChanged += new System.EventHandler(this.appearanceMultiDocument_CheckChanged);
            // 
            // tabGroupAppearance
            // 
            this.tabGroupAppearance.Controls.Add(this.radioMultiBox);
            this.tabGroupAppearance.Controls.Add(this.radioMultiForm);
            this.tabGroupAppearance.Controls.Add(this.radioMultiDocument);
            this.tabGroupAppearance.Location = new System.Drawing.Point(8, 257);
            this.tabGroupAppearance.Name = "tabGroupAppearance";
            this.tabGroupAppearance.Size = new System.Drawing.Size(144, 93);
            this.tabGroupAppearance.TabIndex = 0;
            this.tabGroupAppearance.TabStop = false;
            this.tabGroupAppearance.Text = "Appearance";
            // 
            // idePixelBorder
            // 
            this.idePixelBorder.Location = new System.Drawing.Point(16, 24);
            this.idePixelBorder.Name = "idePixelBorder";
            this.idePixelBorder.Size = new System.Drawing.Size(112, 24);
            this.idePixelBorder.TabIndex = 5;
            this.idePixelBorder.Text = "IDE Pixel Border";
            this.idePixelBorder.CheckedChanged += new System.EventHandler(this.idePixelBorder_CheckedChanged);
            // 
            // idePixelArea
            // 
            this.idePixelArea.Location = new System.Drawing.Point(136, 24);
            this.idePixelArea.Name = "idePixelArea";
            this.idePixelArea.Size = new System.Drawing.Size(100, 24);
            this.idePixelArea.TabIndex = 6;
            this.idePixelArea.Text = "IDE Pixel Area";
            this.idePixelArea.CheckedChanged += new System.EventHandler(this.idePixelArea_CheckedChanged);
            // 
            // multiLine
            // 
            this.multiLine.Location = new System.Drawing.Point(16, 16);
            this.multiLine.Name = "multiLine";
            this.multiLine.Size = new System.Drawing.Size(96, 24);
            this.multiLine.TabIndex = 7;
            this.multiLine.Text = "MultiLine";
            this.multiLine.CheckedChanged += new System.EventHandler(this.multiLine_CheckedChanged);
            // 
            // groupBoxHideTabsMode
            // 
            this.groupBoxHideTabsMode.Controls.Add(this.tabHideWithoutMouse);
            this.groupBoxHideTabsMode.Controls.Add(this.tabHideUsingLogic);
            this.groupBoxHideTabsMode.Controls.Add(this.tabHideAlways);
            this.groupBoxHideTabsMode.Controls.Add(this.tabShowAlways);
            this.groupBoxHideTabsMode.Location = new System.Drawing.Point(160, 360);
            this.groupBoxHideTabsMode.Name = "groupBoxHideTabsMode";
            this.groupBoxHideTabsMode.Size = new System.Drawing.Size(160, 120);
            this.groupBoxHideTabsMode.TabIndex = 2;
            this.groupBoxHideTabsMode.TabStop = false;
            this.groupBoxHideTabsMode.Text = "HideTabsMode";
            // 
            // tabHideWithoutMouse
            // 
            this.tabHideWithoutMouse.Location = new System.Drawing.Point(16, 88);
            this.tabHideWithoutMouse.Name = "tabHideWithoutMouse";
            this.tabHideWithoutMouse.Size = new System.Drawing.Size(120, 24);
            this.tabHideWithoutMouse.TabIndex = 1;
            this.tabHideWithoutMouse.Text = "HideWithoutMouse";
            this.tabHideWithoutMouse.CheckedChanged += new System.EventHandler(this.tabHideWithoutMouse_CheckedChanged);
            // 
            // tabHideUsingLogic
            // 
            this.tabHideUsingLogic.Location = new System.Drawing.Point(16, 64);
            this.tabHideUsingLogic.Name = "tabHideUsingLogic";
            this.tabHideUsingLogic.Size = new System.Drawing.Size(104, 24);
            this.tabHideUsingLogic.TabIndex = 0;
            this.tabHideUsingLogic.Text = "HideUsingLogic";
            this.tabHideUsingLogic.CheckedChanged += new System.EventHandler(this.tabHideUsingLogic_CheckedChanged);
            // 
            // tabHideAlways
            // 
            this.tabHideAlways.Location = new System.Drawing.Point(16, 40);
            this.tabHideAlways.Name = "tabHideAlways";
            this.tabHideAlways.Size = new System.Drawing.Size(88, 24);
            this.tabHideAlways.TabIndex = 0;
            this.tabHideAlways.Text = "HideAlways";
            this.tabHideAlways.CheckedChanged += new System.EventHandler(this.tabHideAlways_CheckedChanged);
            // 
            // tabShowAlways
            // 
            this.tabShowAlways.Location = new System.Drawing.Point(16, 16);
            this.tabShowAlways.Name = "tabShowAlways";
            this.tabShowAlways.Size = new System.Drawing.Size(104, 24);
            this.tabShowAlways.TabIndex = 2;
            this.tabShowAlways.Text = "Show Always";
            this.tabShowAlways.CheckedChanged += new System.EventHandler(this.tabShowAlways_CheckedChanged);
            // 
            // multilineFullWidth
            // 
            this.multilineFullWidth.Location = new System.Drawing.Point(16, 40);
            this.multilineFullWidth.Name = "multilineFullWidth";
            this.multilineFullWidth.Size = new System.Drawing.Size(128, 24);
            this.multilineFullWidth.TabIndex = 9;
            this.multilineFullWidth.Text = "MultiLine Full Width";
            this.multilineFullWidth.CheckedChanged += new System.EventHandler(this.multilineFullWidth_CheckedChanged);
            // 
            // checkBoxTooltips
            // 
            this.checkBoxTooltips.Location = new System.Drawing.Point(16, 208);
            this.checkBoxTooltips.Name = "checkBoxTooltips";
            this.checkBoxTooltips.Size = new System.Drawing.Size(128, 24);
            this.checkBoxTooltips.TabIndex = 10;
            this.checkBoxTooltips.Text = "Tooltips";
            this.checkBoxTooltips.CheckedChanged += new System.EventHandler(this.checkBoxTooltips_CheckedChanged);
            // 
            // groupBoxOffset
            // 
            this.groupBoxOffset.Controls.Add(this.numericUpDown1);
            this.groupBoxOffset.Controls.Add(this.label1);
            this.groupBoxOffset.Controls.Add(this.label2);
            this.groupBoxOffset.Controls.Add(this.numericUpDown2);
            this.groupBoxOffset.Controls.Add(this.numericUpDown3);
            this.groupBoxOffset.Controls.Add(this.label3);
            this.groupBoxOffset.Controls.Add(this.label4);
            this.groupBoxOffset.Controls.Add(this.numericUpDown4);
            this.groupBoxOffset.Location = new System.Drawing.Point(8, 356);
            this.groupBoxOffset.Name = "groupBoxOffset";
            this.groupBoxOffset.Size = new System.Drawing.Size(144, 120);
            this.groupBoxOffset.TabIndex = 3;
            this.groupBoxOffset.TabStop = false;
            this.groupBoxOffset.Text = "Page Offset";
            // 
            // groupBoxSingleMulti
            // 
            this.groupBoxSingleMulti.Controls.Add(this.multilineFullWidth);
            this.groupBoxSingleMulti.Controls.Add(this.multiLine);
            this.groupBoxSingleMulti.Location = new System.Drawing.Point(160, 8);
            this.groupBoxSingleMulti.Name = "groupBoxSingleMulti";
            this.groupBoxSingleMulti.Size = new System.Drawing.Size(160, 72);
            this.groupBoxSingleMulti.TabIndex = 4;
            this.groupBoxSingleMulti.TabStop = false;
            this.groupBoxSingleMulti.Text = "Single / Multiline";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.idePixelArea);
            this.groupBox1.Controls.Add(this.idePixelBorder);
            this.groupBox1.Location = new System.Drawing.Point(328, 230);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(244, 56);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "IDE Style Specific";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.showDropSelect);
            this.groupBox2.Controls.Add(this.checkBoxTextTips);
            this.groupBox2.Controls.Add(this.positionAtTop);
            this.groupBox2.Controls.Add(this.shrinkPages);
            this.groupBox2.Controls.Add(this.showClose);
            this.groupBox2.Controls.Add(this.showArrows);
            this.groupBox2.Controls.Add(this.hoverSelect);
            this.groupBox2.Controls.Add(this.selectedTextOnly);
            this.groupBox2.Controls.Add(this.hotTrack);
            this.groupBox2.Controls.Add(this.checkBoxTooltips);
            this.groupBox2.Location = new System.Drawing.Point(160, 88);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(160, 264);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Operation";
            // 
            // showDropSelect
            // 
            this.showDropSelect.Location = new System.Drawing.Point(16, 112);
            this.showDropSelect.Name = "showDropSelect";
            this.showDropSelect.Size = new System.Drawing.Size(120, 24);
            this.showDropSelect.TabIndex = 12;
            this.showDropSelect.Text = "Show Drop Select";
            this.showDropSelect.CheckedChanged += new System.EventHandler(this.showDropSelect_CheckedChanged);
            // 
            // checkBoxTextTips
            // 
            this.checkBoxTextTips.Location = new System.Drawing.Point(16, 232);
            this.checkBoxTextTips.Name = "checkBoxTextTips";
            this.checkBoxTextTips.Size = new System.Drawing.Size(128, 24);
            this.checkBoxTextTips.TabIndex = 11;
            this.checkBoxTextTips.Text = "Texttips";
            this.checkBoxTextTips.CheckedChanged += new System.EventHandler(this.checkBoxTextTips_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.insetPagesOnly);
            this.groupBox3.Controls.Add(this.insetPlain);
            this.groupBox3.Location = new System.Drawing.Point(579, 230);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(214, 56);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Plain Style Specific";
            // 
            // groupBoxTabControl
            // 
            this.groupBoxTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxTabControl.Controls.Add(this.button1);
            this.groupBoxTabControl.Controls.Add(this.tabControl);
            this.groupBoxTabControl.Controls.Add(this.addPage);
            this.groupBoxTabControl.Controls.Add(this.removePage);
            this.groupBoxTabControl.Controls.Add(this.clearAll);
            this.groupBoxTabControl.Location = new System.Drawing.Point(329, 299);
            this.groupBoxTabControl.Name = "groupBoxTabControl";
            this.groupBoxTabControl.Size = new System.Drawing.Size(465, 236);
            this.groupBoxTabControl.TabIndex = 12;
            this.groupBoxTabControl.TabStop = false;
            this.groupBoxTabControl.Text = "TabControl Instance";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.Location = new System.Drawing.Point(304, 196);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 24);
            this.button1.TabIndex = 12;
            this.button1.Text = "Color Tabs";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.colorTabText);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl.Location = new System.Drawing.Point(16, 32);
            this.tabControl.MediaPlayerDockSides = false;
            this.tabControl.Name = "tabControl";
            this.tabControl.OfficeDockSides = false;
            this.tabControl.ShowDropSelect = false;
            this.tabControl.Size = new System.Drawing.Size(433, 148);
            this.tabControl.TabIndex = 11;
            this.tabControl.TextTips = true;
            this.tabControl.SelectionChanged += new Crownwood.DotNetMagic.Controls.SelectTabHandler(this.OnSelectionChanged);
            this.tabControl.DoubleClickTab += new Crownwood.DotNetMagic.Controls.DoubleClickTabHandler(this.tabControl_DoubleClickTab);
            this.tabControl.SelectionChanging += new Crownwood.DotNetMagic.Controls.SelectTabHandler(this.OnSelectionChanging);
            // 
            // groupBoxDrag
            // 
            this.groupBoxDrag.Controls.Add(this.checkBoxDragOut);
            this.groupBoxDrag.Controls.Add(this.checkBoxReorder);
            this.groupBoxDrag.Location = new System.Drawing.Point(8, 482);
            this.groupBoxDrag.Name = "groupBoxDrag";
            this.groupBoxDrag.Size = new System.Drawing.Size(312, 53);
            this.groupBoxDrag.TabIndex = 13;
            this.groupBoxDrag.TabStop = false;
            this.groupBoxDrag.Text = "Dragging";
            // 
            // checkBoxDragOut
            // 
            this.checkBoxDragOut.Location = new System.Drawing.Point(129, 19);
            this.checkBoxDragOut.Name = "checkBoxDragOut";
            this.checkBoxDragOut.Size = new System.Drawing.Size(104, 24);
            this.checkBoxDragOut.TabIndex = 6;
            this.checkBoxDragOut.Text = "Drag Outside";
            this.checkBoxDragOut.CheckedChanged += new System.EventHandler(this.OnAllowDragOut);
            // 
            // checkBoxReorder
            // 
            this.checkBoxReorder.Location = new System.Drawing.Point(16, 19);
            this.checkBoxReorder.Name = "checkBoxReorder";
            this.checkBoxReorder.Size = new System.Drawing.Size(96, 24);
            this.checkBoxReorder.TabIndex = 5;
            this.checkBoxReorder.Text = "Allow Reorder";
            this.checkBoxReorder.CheckedChanged += new System.EventHandler(this.OnAllowReorder);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkOfficePixelBorder);
            this.groupBox4.Controls.Add(this.checkOfficeHeaderBorder);
            this.groupBox4.Controls.Add(this.comboBoxOfficeStyle);
            this.groupBox4.Location = new System.Drawing.Point(328, 8);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(464, 56);
            this.groupBox4.TabIndex = 14;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Office Style Specific";
            // 
            // checkOfficePixelBorder
            // 
            this.checkOfficePixelBorder.Location = new System.Drawing.Point(320, 24);
            this.checkOfficePixelBorder.Name = "checkOfficePixelBorder";
            this.checkOfficePixelBorder.Size = new System.Drawing.Size(120, 24);
            this.checkOfficePixelBorder.TabIndex = 2;
            this.checkOfficePixelBorder.Text = "Office Pixel Border";
            this.checkOfficePixelBorder.CheckedChanged += new System.EventHandler(this.checkOfficePixelBorder_CheckedChanged);
            // 
            // checkOfficeHeaderBorder
            // 
            this.checkOfficeHeaderBorder.Location = new System.Drawing.Point(176, 24);
            this.checkOfficeHeaderBorder.Name = "checkOfficeHeaderBorder";
            this.checkOfficeHeaderBorder.Size = new System.Drawing.Size(136, 24);
            this.checkOfficeHeaderBorder.TabIndex = 1;
            this.checkOfficeHeaderBorder.Text = "Office Header Border";
            this.checkOfficeHeaderBorder.CheckedChanged += new System.EventHandler(this.checkOfficeHeaderBorder_CheckedChanged);
            // 
            // comboBoxOfficeStyle
            // 
            this.comboBoxOfficeStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOfficeStyle.Items.AddRange(new object[] {
            "SoftWhite",
            "LightWhite",
            "DarkWhite",
            "SoftEnhanced",
            "LightEnhanced",
            "DarkEnhanced",
            "Light",
            "Dark"});
            this.comboBoxOfficeStyle.Location = new System.Drawing.Point(16, 24);
            this.comboBoxOfficeStyle.Name = "comboBoxOfficeStyle";
            this.comboBoxOfficeStyle.Size = new System.Drawing.Size(144, 21);
            this.comboBoxOfficeStyle.TabIndex = 0;
            this.comboBoxOfficeStyle.SelectedIndexChanged += new System.EventHandler(this.comboBoxOfficeStyle_SelectedIndexChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.checkIDE2005TabJoined);
            this.groupBox5.Controls.Add(this.checkIDE2005PixelBorder);
            this.groupBox5.Controls.Add(this.checkIDE2005HeaderBorder);
            this.groupBox5.Controls.Add(this.comboBoxIDE2005Style);
            this.groupBox5.Location = new System.Drawing.Point(328, 144);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(464, 80);
            this.groupBox5.TabIndex = 11;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "IDE2005 Style Specific";
            // 
            // checkIDE2005TabJoined
            // 
            this.checkIDE2005TabJoined.Location = new System.Drawing.Point(176, 48);
            this.checkIDE2005TabJoined.Name = "checkIDE2005TabJoined";
            this.checkIDE2005TabJoined.Size = new System.Drawing.Size(144, 24);
            this.checkIDE2005TabJoined.TabIndex = 7;
            this.checkIDE2005TabJoined.Text = "IDE2005 Tab Joined";
            this.checkIDE2005TabJoined.CheckedChanged += new System.EventHandler(this.checkIDE2005TabJoined_CheckedChanged);
            // 
            // checkIDE2005PixelBorder
            // 
            this.checkIDE2005PixelBorder.Location = new System.Drawing.Point(320, 24);
            this.checkIDE2005PixelBorder.Name = "checkIDE2005PixelBorder";
            this.checkIDE2005PixelBorder.Size = new System.Drawing.Size(136, 24);
            this.checkIDE2005PixelBorder.TabIndex = 5;
            this.checkIDE2005PixelBorder.Text = "IDE2005 Pixel Border";
            this.checkIDE2005PixelBorder.CheckedChanged += new System.EventHandler(this.checkIDE2005PixelBorder_CheckedChanged);
            // 
            // checkIDE2005HeaderBorder
            // 
            this.checkIDE2005HeaderBorder.Location = new System.Drawing.Point(176, 24);
            this.checkIDE2005HeaderBorder.Name = "checkIDE2005HeaderBorder";
            this.checkIDE2005HeaderBorder.Size = new System.Drawing.Size(144, 24);
            this.checkIDE2005HeaderBorder.TabIndex = 6;
            this.checkIDE2005HeaderBorder.Text = "IDE2005 Header Border";
            this.checkIDE2005HeaderBorder.CheckedChanged += new System.EventHandler(this.checkIDE2005HeaderBorder_CheckedChanged);
            // 
            // comboBoxIDE2005Style
            // 
            this.comboBoxIDE2005Style.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxIDE2005Style.Items.AddRange(new object[] {
            "Standard",
            "StandardDark",
            "Enhanced",
            "EnhancedDark"});
            this.comboBoxIDE2005Style.Location = new System.Drawing.Point(16, 24);
            this.comboBoxIDE2005Style.Name = "comboBoxIDE2005Style";
            this.comboBoxIDE2005Style.Size = new System.Drawing.Size(144, 21);
            this.comboBoxIDE2005Style.TabIndex = 3;
            this.comboBoxIDE2005Style.SelectedIndexChanged += new System.EventHandler(this.comboBoxIDE2005Style_SelectedIndexChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.checkMediaPlayerPixelBorder);
            this.groupBox6.Controls.Add(this.checkMediaPlayerHeaderBorder);
            this.groupBox6.Controls.Add(this.comboBoxMediaPlayerStyle);
            this.groupBox6.Location = new System.Drawing.Point(330, 77);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(464, 56);
            this.groupBox6.TabIndex = 15;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Media Player Style Specific";
            // 
            // checkMediaPlayerPixelBorder
            // 
            this.checkMediaPlayerPixelBorder.Location = new System.Drawing.Point(320, 24);
            this.checkMediaPlayerPixelBorder.Name = "checkMediaPlayerPixelBorder";
            this.checkMediaPlayerPixelBorder.Size = new System.Drawing.Size(120, 24);
            this.checkMediaPlayerPixelBorder.TabIndex = 2;
            this.checkMediaPlayerPixelBorder.Text = "Media Pixel Border";
            this.checkMediaPlayerPixelBorder.CheckedChanged += new System.EventHandler(this.checkMediaPlayerPixelBorder_CheckedChanged);
            // 
            // checkMediaPlayerHeaderBorder
            // 
            this.checkMediaPlayerHeaderBorder.Location = new System.Drawing.Point(176, 24);
            this.checkMediaPlayerHeaderBorder.Name = "checkMediaPlayerHeaderBorder";
            this.checkMediaPlayerHeaderBorder.Size = new System.Drawing.Size(136, 24);
            this.checkMediaPlayerHeaderBorder.TabIndex = 1;
            this.checkMediaPlayerHeaderBorder.Text = "Media Header Border";
            this.checkMediaPlayerHeaderBorder.CheckedChanged += new System.EventHandler(this.checkMediaPlayerHeaderBorder_CheckedChanged);
            // 
            // comboBoxMediaPlayerStyle
            // 
            this.comboBoxMediaPlayerStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMediaPlayerStyle.Items.AddRange(new object[] {
            "SoftWhite",
            "LightWhite",
            "DarkWhite",
            "SoftEnhanced",
            "LightEnhanced",
            "DarkEnhanced",
            "Light",
            "Dark"});
            this.comboBoxMediaPlayerStyle.Location = new System.Drawing.Point(16, 24);
            this.comboBoxMediaPlayerStyle.Name = "comboBoxMediaPlayerStyle";
            this.comboBoxMediaPlayerStyle.Size = new System.Drawing.Size(144, 21);
            this.comboBoxMediaPlayerStyle.TabIndex = 0;
            this.comboBoxMediaPlayerStyle.SelectedIndexChanged += new System.EventHandler(this.comboBoxMediaPlayerStyle_SelectedIndexChanged);
            // 
            // SampleTabControl
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(799, 547);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBoxDrag);
            this.Controls.Add(this.groupBoxTabControl);
            this.Controls.Add(this.tabGroupStyle);
            this.Controls.Add(this.tabGroupAppearance);
            this.Controls.Add(this.groupBoxHideTabsMode);
            this.Controls.Add(this.groupBoxOffset);
            this.Controls.Add(this.groupBoxSingleMulti);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(805, 573);
            this.Name = "SampleTabControl";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DotNetMagic - SampleTabControl";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.tabGroupStyle.ResumeLayout(false);
            this.tabGroupAppearance.ResumeLayout(false);
            this.groupBoxHideTabsMode.ResumeLayout(false);
            this.groupBoxOffset.ResumeLayout(false);
            this.groupBoxSingleMulti.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBoxTabControl.ResumeLayout(false);
            this.groupBoxDrag.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

        protected void UpdateControls()
        {
			if (!_reenter)
			{
				_reenter = true;	
				
				switch(tabControl.Style)
				{
                    case VisualStyle.Office2007Blue:
                        radioOffice2007Blue.Select();
                        break;
                    case VisualStyle.Office2007Silver:
                        radioOffice2007Silver.Select();
                        break;
                    case VisualStyle.Office2007Black:
                        radioOffice2007Black.Select();
                        break;
                    case VisualStyle.Office2003:
						radioOffice2003.Select();
						break;
					case VisualStyle.IDE2005:
						radioIDE2005.Select();
						break;
					case VisualStyle.Plain:
						radioPlain.Select();
						break;
				}
	            
				switch(tabControl.Appearance)
				{
					case Crownwood.DotNetMagic.Controls.VisualAppearance.MultiDocument:
						radioMultiDocument.Select();
						break;
					case Crownwood.DotNetMagic.Controls.VisualAppearance.MultiForm:
						radioMultiForm.Select();
						break;
					case Crownwood.DotNetMagic.Controls.VisualAppearance.MultiBox:
						radioMultiBox.Select();
						break;
				}

				switch(tabControl.HideTabsMode)
				{
					case Crownwood.DotNetMagic.Controls.HideTabsModes.ShowAlways:
						tabShowAlways.Checked = true;
						break;
					case Crownwood.DotNetMagic.Controls.HideTabsModes.HideAlways:
						tabHideAlways.Checked = true;
						break;
					case Crownwood.DotNetMagic.Controls.HideTabsModes.HideUsingLogic:
						tabHideUsingLogic.Checked = true;
						break;
					case Crownwood.DotNetMagic.Controls.HideTabsModes.HideWithoutMouse:
						tabHideWithoutMouse.Checked = true;
						break;
				}

				switch(tabControl.OfficeStyle)
				{
					case OfficeStyle.SoftWhite:
						comboBoxOfficeStyle.SelectedIndex = 0;
						break;
					case OfficeStyle.LightWhite:
						comboBoxOfficeStyle.SelectedIndex = 1;
						break;
					case OfficeStyle.DarkWhite:
						comboBoxOfficeStyle.SelectedIndex = 2;
						break;
					case OfficeStyle.SoftEnhanced:
						comboBoxOfficeStyle.SelectedIndex = 3;
						break;
					case OfficeStyle.LightEnhanced:
						comboBoxOfficeStyle.SelectedIndex = 4;
						break;
					case OfficeStyle.DarkEnhanced:
						comboBoxOfficeStyle.SelectedIndex = 5;
						break;
					case OfficeStyle.Light:
						comboBoxOfficeStyle.SelectedIndex = 6;
						break;
					case OfficeStyle.Dark:
						comboBoxOfficeStyle.SelectedIndex = 7;
						break;
				}

                switch (tabControl.MediaPlayerStyle)
                {
                    case MediaPlayerStyle.SoftWhite:
                        comboBoxMediaPlayerStyle.SelectedIndex = 0;
                        break;
                    case MediaPlayerStyle.LightWhite:
                        comboBoxMediaPlayerStyle.SelectedIndex = 1;
                        break;
                    case MediaPlayerStyle.DarkWhite:
                        comboBoxMediaPlayerStyle.SelectedIndex = 2;
                        break;
                    case MediaPlayerStyle.SoftEnhanced:
                        comboBoxMediaPlayerStyle.SelectedIndex = 3;
                        break;
                    case MediaPlayerStyle.LightEnhanced:
                        comboBoxMediaPlayerStyle.SelectedIndex = 4;
                        break;
                    case MediaPlayerStyle.DarkEnhanced:
                        comboBoxMediaPlayerStyle.SelectedIndex = 5;
                        break;
                    case MediaPlayerStyle.Light:
                        comboBoxMediaPlayerStyle.SelectedIndex = 6;
                        break;
                    case MediaPlayerStyle.Dark:
                        comboBoxMediaPlayerStyle.SelectedIndex = 7;
                        break;
                }
                switch 
                    (tabControl.IDE2005Style)
				{
					case IDE2005Style.Standard:
						comboBoxIDE2005Style.SelectedIndex = 0;
						break;
					case IDE2005Style.StandardDark:
						comboBoxIDE2005Style.SelectedIndex = 1;
						break;
					case IDE2005Style.Enhanced:
						comboBoxIDE2005Style.SelectedIndex = 2;
						break;
					case IDE2005Style.EnhancedDark:
						comboBoxIDE2005Style.SelectedIndex = 3;
						break;
				}
				
				hotTrack.Checked = tabControl.HotTrack;
				positionAtTop.Checked = tabControl.PositionTop;
				shrinkPages.Checked = tabControl.ShrinkPagesToFit;
				showClose.Checked = tabControl.ShowClose;
				showArrows.Checked = tabControl.ShowArrows;
				showDropSelect.Checked = tabControl.ShowDropSelect;
				insetPlain.Checked = tabControl.InsetPlain;
				idePixelBorder.Checked = tabControl.IDEPixelBorder;
				idePixelArea.Checked = tabControl.IDEPixelArea;
				insetPagesOnly.Checked = tabControl.InsetBorderPagesOnly;
				selectedTextOnly.Checked = tabControl.SelectedTextOnly;
				hoverSelect.Checked = tabControl.HoverSelect;
				multiLine.Checked = tabControl.Multiline;
				multilineFullWidth.Checked = tabControl.MultilineFullWidth;
				checkBoxTooltips.Checked = tabControl.ToolTips;
				checkBoxTextTips.Checked = tabControl.TextTips;
				numericUpDown1.Value = tabControl.ControlLeftOffset;
				numericUpDown2.Value = tabControl.ControlRightOffset;
				numericUpDown3.Value = tabControl.ControlTopOffset;
				numericUpDown4.Value = tabControl.ControlBottomOffset;
				checkBoxReorder.Checked = tabControl.AllowDragReorder;
				checkBoxDragOut.Checked = tabControl.DragOutside;
				checkOfficeHeaderBorder.Checked = tabControl.OfficeHeaderBorder;
				checkOfficePixelBorder.Checked = tabControl.OfficePixelBorder;
                checkMediaPlayerHeaderBorder.Checked = tabControl.MediaPlayerHeaderBorder;
                checkMediaPlayerPixelBorder.Checked = tabControl.MediaPlayerPixelBorder;
                checkIDE2005HeaderBorder.Checked = tabControl.IDE2005HeaderBorder;
				checkIDE2005PixelBorder.Checked = tabControl.IDE2005PixelBorder;
				checkIDE2005TabJoined.Checked = tabControl.IDE2005TabJoined;
			}
		}
		
        protected void OnAddPage(object sender, EventArgs e)
        {
            Control controlToAdd = null;

            switch(_count)
            {
                case 0:
                case 2:
                case 4:
                case 6:
                    controlToAdd = new DummyForm();
                    break;

                case 1:
                case 5:
                    RichTextBox rtb = new RichTextBox();
                    rtb.BorderStyle = BorderStyle.None;
                    rtb.Text = "The quick brown fox jumped over the lazy dog.";
                    controlToAdd = rtb;
                    break;

                case 3:
                    controlToAdd = new DummyPanel();
                    break;
            }

            // Define background color of the tab page
            controlToAdd.BackColor = SystemColors.Window;

            Crownwood.DotNetMagic.Controls.TabPage page;

            // Create a new page with the appropriate control for display, title text and image
            page = new Crownwood.DotNetMagic.Controls.TabPage(_strings[_count], controlToAdd, _internalImages.Images[_count]);

			// Define correct tooltip for page
			page.ToolTip = _tooltips[_count];
            
			// Make this page become selected when added
            page.Selected = true;

            tabControl.TabPages.Add(page);
			
            // Update the count for creating new pages
            _count++;
            if (_count > 6)
                _count = 0;
        }

        protected void OnRemovePage(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex >= 0)
            {
                Crownwood.DotNetMagic.Controls.TabPage tp = tabControl.SelectedTab;
                tabControl.TabPages.RemoveAt(tabControl.SelectedIndex);
                tp.Dispose();
            }
        }

        protected void OnClearAll(object sender, EventArgs e)
        {
            for (int i = tabControl.TabPages.Count - 1; i >= 0; i--)
            {
                Crownwood.DotNetMagic.Controls.TabPage tp = tabControl.TabPages[i];
                tabControl.TabPages.RemoveAt(i);
                tp.Dispose();
            }
        }

        protected void colorTabText(object sender, System.EventArgs e)
		{
            int color = 0;

            foreach (Crownwood.DotNetMagic.Controls.TabPage page in tabControl.TabPages)
            {
                switch (color)
                {
                    case 0:
                        page.SelectTextColor = Color.White;
                        page.SelectTextBackColor = Color.Blue;
                        page.InactiveTextColor = Color.White;
                        page.InactiveTextBackColor = Color.Blue;
                        break;
                    case 1:
                        page.SelectTextColor = Color.White;
                        page.SelectTextBackColor = Color.Red;
                        page.InactiveTextColor = Color.White;
                        page.InactiveTextBackColor = Color.Red;
                        break;
                    case 2:
                        page.SelectTextColor = Color.White;
                        page.SelectTextBackColor = Color.Green;
                        page.InactiveTextColor = Color.White;
                        page.InactiveTextBackColor = Color.Green;
                        break;
                    case 3:
                        page.SelectTextColor = Color.White;
                        page.SelectTextBackColor = Color.Black;
                        page.InactiveTextColor = Color.White;
                        page.InactiveTextBackColor = Color.Black;
                        break;
                }

                color++;
                if (color == 4)
                    color = 0;
            }
		}

        private void radioOffice2007Black_CheckedChanged(object sender, EventArgs e)
        {
            if (radioOffice2007Black.Checked)
            {
                tabControl.Style = VisualStyle.Office2007Black;
                UpdateControls();
            }
        }

        private void radioOffice2007Silver_CheckedChanged(object sender, EventArgs e)
        {
            if (radioOffice2007Silver.Checked)
            {
                tabControl.Style = VisualStyle.Office2007Silver;
                UpdateControls();
            }
        }

        private void radioOffice2007Blue_CheckedChanged(object sender, EventArgs e)
        {
            if (radioOffice2007Blue.Checked)
            {
                tabControl.Style = VisualStyle.Office2007Blue;
                UpdateControls();
            }
        }

        private void radioMediaPlayerBlue_CheckedChanged(object sender, EventArgs e)
        {
            if (radioMediaPlayerBlue.Checked)
            {
                tabControl.Style = VisualStyle.MediaPlayerBlue;
                UpdateControls();
            }
        }

        private void radioMediaPlayerOrange_CheckedChanged(object sender, EventArgs e)
        {
            if (radioMediaPlayerOrange.Checked)
            {
                tabControl.Style = VisualStyle.MediaPlayerOrange;
                UpdateControls();
            }
        }

        private void radioMediaPlayerPurple_CheckedChanged(object sender, EventArgs e)
        {
            if (radioMediaPlayerPurple.Checked)
            {
                tabControl.Style = VisualStyle.MediaPlayerPurple;
                UpdateControls();
            }
        }

		private void radioOffice2003_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radioOffice2003.Checked)
			{
				tabControl.Style = VisualStyle.Office2003;
				UpdateControls();
			}
		}

		private void radioIDE2005_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radioIDE2005.Checked)
			{
				tabControl.Style = VisualStyle.IDE2005;
				UpdateControls();
			}
		}

		protected void stylePlain_CheckChanged(object sender, System.EventArgs e)
		{					
			if (radioPlain.Checked)
			{
				tabControl.Style = VisualStyle.Plain;
				UpdateControls();
			}
        }

        protected void appearanceMultiBox_CheckChanged(object sender, EventArgs e)
        {
			if (radioMultiBox.Checked)
			{
				tabControl.Appearance = Crownwood.DotNetMagic.Controls.VisualAppearance.MultiBox;
				UpdateControls();
			}
        }

		protected void appearanceMultiForm_CheckChanged(object sender, EventArgs e)
		{
			if (radioMultiForm.Checked)
			{
				tabControl.Appearance = Crownwood.DotNetMagic.Controls.VisualAppearance.MultiForm;
				UpdateControls();
			}
        }

        protected void appearanceMultiDocument_CheckChanged(object sender, EventArgs e)
        {
			if (radioMultiDocument.Checked)
			{
				tabControl.Appearance = Crownwood.DotNetMagic.Controls.VisualAppearance.MultiDocument;
				UpdateControls();
			}
        }

        protected void positionAtTop_CheckedChanged(object sender, System.EventArgs e)
        {
			tabControl.PositionTop = positionAtTop.Checked;
			UpdateControls();
        }

        protected void Highlight_CheckedChanged(object sender, System.EventArgs e)
        {
			tabControl.HotTrack = hotTrack.Checked;
			UpdateControls();
        }

        protected void shrinkPages_CheckedChanged(object sender, System.EventArgs e)
        {
			tabControl.ShrinkPagesToFit = shrinkPages.Checked;
			UpdateControls();
        }

        protected void showClose_CheckedChanged(object sender, System.EventArgs e)
        {
			tabControl.ShowClose = showClose.Checked;
			UpdateControls();
        }

        protected void showArrows_CheckedChanged(object sender, System.EventArgs e)
        {
			tabControl.ShowArrows = showArrows.Checked;
			UpdateControls();
        }

		private void showDropSelect_CheckedChanged(object sender, System.EventArgs e)
		{
			tabControl.ShowDropSelect = showDropSelect.Checked;
			UpdateControls();
		}

        protected void insetPlain_CheckedChanged(object sender, System.EventArgs e)
        {
			tabControl.InsetPlain = insetPlain.Checked;
			UpdateControls();
        }

        protected void idePixelBorder_CheckedChanged(object sender, System.EventArgs e)
        {
			tabControl.IDEPixelBorder = idePixelBorder.Checked;
			UpdateControls();
        }

        protected void idePixelArea_CheckedChanged(object sender, System.EventArgs e)
        {
			tabControl.IDEPixelArea = idePixelArea.Checked;
			UpdateControls();
        }

        protected void insetPagesOnly_CheckedChanged(object sender, System.EventArgs e)
        {
			tabControl.InsetBorderPagesOnly = insetPagesOnly.Checked;
			UpdateControls();
        }

        protected void selectedTextOnly_CheckedChanged(object sender, System.EventArgs e)
        {
	        tabControl.SelectedTextOnly = selectedTextOnly.Checked;
		    UpdateControls();
        }

        protected void numericUpDown1_ValueChanged(object sender, System.EventArgs e)
        {
            tabControl.ControlLeftOffset = (int)numericUpDown1.Value;
            UpdateControls();
        }

        protected void numericUpDown3_ValueChanged(object sender, System.EventArgs e)
        {
            tabControl.ControlTopOffset = (int)numericUpDown3.Value;
            UpdateControls();
        }

        protected void numericUpDown2_ValueChanged(object sender, System.EventArgs e)
        {
            tabControl.ControlRightOffset = (int)numericUpDown2.Value;
            UpdateControls();
        }

        protected void numericUpDown4_ValueChanged(object sender, System.EventArgs e)
        {
            tabControl.ControlBottomOffset = (int)numericUpDown4.Value;
            UpdateControls();
        }

        private void tabShowAlways_CheckedChanged(object sender, System.EventArgs e)
        {
			if (tabShowAlways.Checked)
			{
				tabControl.HideTabsMode = Crownwood.DotNetMagic.Controls.HideTabsModes.ShowAlways;
				UpdateControls();
			}
        }

        private void tabHideAlways_CheckedChanged(object sender, System.EventArgs e)
        {
			if (tabHideAlways.Checked)
			{
				tabControl.HideTabsMode = Crownwood.DotNetMagic.Controls.HideTabsModes.HideAlways;
				UpdateControls();
			}
        }

        private void tabHideUsingLogic_CheckedChanged(object sender, System.EventArgs e)
        {        
			if (tabHideUsingLogic.Checked)
			{
				tabControl.HideTabsMode = Crownwood.DotNetMagic.Controls.HideTabsModes.HideUsingLogic;
			    UpdateControls();
			}
        }

        private void tabHideWithoutMouse_CheckedChanged(object sender, System.EventArgs e)
        {
			if (tabHideWithoutMouse.Checked)        
			{
				tabControl.HideTabsMode = Crownwood.DotNetMagic.Controls.HideTabsModes.HideWithoutMouse;
				UpdateControls();
			}
        }

        protected void hoverSelect_CheckedChanged(object sender, System.EventArgs e)
        {
			tabControl.HoverSelect = hoverSelect.Checked;
			UpdateControls();
        }

        private void multiLine_CheckedChanged(object sender, System.EventArgs e)
        {
			tabControl.Multiline = multiLine.Checked;
			UpdateControls();
        }
        
        private void multilineFullWidth_CheckedChanged(object sender, System.EventArgs e)
        {
			tabControl.MultilineFullWidth = multilineFullWidth.Checked;
			UpdateControls();
        }

		private void checkBoxTooltips_CheckedChanged(object sender, System.EventArgs e)
		{
			tabControl.ToolTips = checkBoxTooltips.Checked;
			UpdateControls();
		}

		private void checkBoxTextTips_CheckedChanged(object sender, System.EventArgs e)
		{
			tabControl.TextTips = checkBoxTextTips.Checked;
			UpdateControls();
		}

		private void comboBoxOfficeStyle_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			tabControl.OfficeStyle = (OfficeStyle)Enum.Parse(typeof(OfficeStyle), comboBoxOfficeStyle.Text);
			UpdateControls();
		}

        private void comboBoxMediaPlayerStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabControl.MediaPlayerStyle = (MediaPlayerStyle)Enum.Parse(typeof(MediaPlayerStyle), comboBoxMediaPlayerStyle.Text);
            UpdateControls();
        }

		private void comboBoxIDE2005Style_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			tabControl.IDE2005Style = (IDE2005Style)Enum.Parse(typeof(IDE2005Style), comboBoxIDE2005Style.Text);
			UpdateControls();
		}

		private void checkOfficeHeaderBorder_CheckedChanged(object sender, System.EventArgs e)
		{
			tabControl.OfficeHeaderBorder = checkOfficeHeaderBorder.Checked;
			UpdateControls();
		}

		private void checkOfficePixelBorder_CheckedChanged(object sender, System.EventArgs e)
		{
			tabControl.OfficePixelBorder = checkOfficePixelBorder.Checked;
			UpdateControls();
		}

        private void checkMediaPlayerHeaderBorder_CheckedChanged(object sender, EventArgs e)
        {
            tabControl.MediaPlayerHeaderBorder = checkMediaPlayerHeaderBorder.Checked;
            UpdateControls();
        }

        private void checkMediaPlayerPixelBorder_CheckedChanged(object sender, EventArgs e)
        {
            tabControl.MediaPlayerPixelBorder = checkMediaPlayerPixelBorder.Checked;
            UpdateControls();
        }

		private void checkIDE2005PixelBorder_CheckedChanged(object sender, System.EventArgs e)
		{
			tabControl.IDE2005PixelBorder = checkIDE2005PixelBorder.Checked;
			UpdateControls();
		}

		private void checkIDE2005HeaderBorder_CheckedChanged(object sender, System.EventArgs e)
		{
			tabControl.IDE2005HeaderBorder = checkIDE2005HeaderBorder.Checked;
			UpdateControls();
		}

		private void checkIDE2005TabJoined_CheckedChanged(object sender, System.EventArgs e)
		{
			tabControl.IDE2005TabJoined = checkIDE2005TabJoined.Checked;
			UpdateControls();
		}

		private void OnAllowReorder(object sender, System.EventArgs e)
		{
			tabControl.AllowDragReorder = checkBoxReorder.Checked;
			UpdateControls();
		}

		private void OnAllowDragOut(object sender, System.EventArgs e)
		{
			tabControl.DragOutside = checkBoxDragOut.Checked;
			UpdateControls();
		}

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() 
        {
            Application.EnableVisualStyles();
            Application.Run(new SampleTabControl());
        }

		private void OnSelectionChanged(Crownwood.DotNetMagic.Controls.TabControl sender, 
										Crownwood.DotNetMagic.Controls.TabPage oldPage, 
										Crownwood.DotNetMagic.Controls.TabPage newPage)
		{
			Console.WriteLine("Changed {0} {1}", oldPage, newPage);
		}

		private void OnSelectionChanging(Crownwood.DotNetMagic.Controls.TabControl sender, Crownwood.DotNetMagic.Controls.TabPage oldPage, 
										 Crownwood.DotNetMagic.Controls.TabPage newPage)
		{
			Console.WriteLine("Changing {0} {1}", oldPage, newPage);
		}

        private void tabControl_DoubleClickTab(Crownwood.DotNetMagic.Controls.TabControl sender, Crownwood.DotNetMagic.Controls.TabPage page)
        {
            Console.WriteLine("DoubleClickTab {0}", page.Title);
        }
    }

    public class DummyForm : Form
    {
        private Button _dummy1 = new Button();
        private Button _dummy2 = new Button();
        private GroupBox _dummyBox = new GroupBox();
        private RadioButton _dummy3 = new RadioButton();
        private RadioButton _dummy4 = new RadioButton();

        public DummyForm()
        {
            _dummy1.Text = "Dummy 1";
            _dummy1.Size = new Size(90,25);
            _dummy1.Location = new Point(10,10);

            _dummy2.Text = "Dummy 2";
            _dummy2.Size = new Size(90,25);
            _dummy2.Location = new Point(110,10);

            _dummyBox.Text = "Form GroupBox";
            _dummyBox.Size = new Size(190, 67);
            _dummyBox.Location = new Point(10, 45);

            _dummy3.Text = "Dummy 3";
            _dummy3.Size = new Size(100,22);
            _dummy3.Location = new Point(10, 20);

            _dummy4.Text = "Dummy 4";
            _dummy4.Size = new Size(100,22);
            _dummy4.Location = new Point(10, 42);
            _dummy4.Checked = true;

            _dummyBox.Controls.AddRange(new Control[]{_dummy3, _dummy4});

            Controls.AddRange(new Control[]{_dummy1, _dummy2, _dummyBox});

            // Define then control to be selected when first is activated for first time
            this.ActiveControl = _dummy4;
        }
    }
}

public class DummyPanel : Panel
{
    private GroupBox _dummyBox = new GroupBox();
    private RadioButton _dummy3 = new RadioButton();
    private RadioButton _dummy4 = new RadioButton();

    public DummyPanel()
    {
        _dummyBox.Text = "Panel GroupBox";
        _dummyBox.Size = new Size(190, 67);
        _dummyBox.Location = new Point(10, 10);

        _dummy3.Text = "RadioButton 3";
        _dummy3.Size = new Size(100,22);
        _dummy3.Location = new Point(10, 20);

        _dummy4.Text = "RadioButton 4";
        _dummy4.Size = new Size(100,22);
        _dummy4.Location = new Point(10, 42);
        _dummy4.Checked = true;

        _dummyBox.Controls.AddRange(new Control[]{_dummy3, _dummy4});

        Controls.AddRange(new Control[]{_dummyBox});
    }
}
