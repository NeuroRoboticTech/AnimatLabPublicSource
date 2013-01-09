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
using Crownwood.DotNetMagic.Controls;

namespace SampleWizard
{
	public class SampleWizard : Crownwood.DotNetMagic.Forms.WizardDialog
	{
	    private Timer installTimer;
	    private int installCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label7;
		private Crownwood.DotNetMagic.Controls.WizardPage wizardStartPage;
		private Crownwood.DotNetMagic.Controls.WizardPage wizardInfo1;
		private Crownwood.DotNetMagic.Controls.WizardPage wizardLegal;
		private Crownwood.DotNetMagic.Controls.WizardPage wizardInstall;
		private Crownwood.DotNetMagic.Controls.WizardPage wizardFinish;
        private Crownwood.DotNetMagic.Controls.WizardPage wizardWarn;
        private Crownwood.DotNetMagic.Controls.WizardPage wizardInfo2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
		private System.ComponentModel.IContainer components = null;

		public SampleWizard()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// Add collection of required pages
			WizardControlProperty.WizardPages.AddRange(new WizardPage[]{wizardStartPage, wizardInfo1, wizardInfo2, wizardLegal, wizardWarn, wizardInstall, wizardFinish});

			// Preset the installation style of operation
            WizardControlProperty.Profile = Profiles.Install;
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

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SampleWizard));
            this.wizardStartPage = new Crownwood.DotNetMagic.Controls.WizardPage();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.wizardInfo1 = new Crownwood.DotNetMagic.Controls.WizardPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.wizardLegal = new Crownwood.DotNetMagic.Controls.WizardPage();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.wizardWarn = new Crownwood.DotNetMagic.Controls.WizardPage();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.wizardInstall = new Crownwood.DotNetMagic.Controls.WizardPage();
            this.label7 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.wizardFinish = new Crownwood.DotNetMagic.Controls.WizardPage();
            this.label10 = new System.Windows.Forms.Label();
            this.wizardInfo2 = new Crownwood.DotNetMagic.Controls.WizardPage();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.wizardStartPage.SuspendLayout();
            this.wizardInfo1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.wizardLegal.SuspendLayout();
            this.wizardWarn.SuspendLayout();
            this.wizardInstall.SuspendLayout();
            this.wizardFinish.SuspendLayout();
            this.wizardInfo2.SuspendLayout();
            this.SuspendLayout();
            // 
            // WizardControlField
            // 
            this.WizardControlField.AssignDefaultButton = true;
            // 
            // 
            // 
            this.WizardControlField.HeaderPanel.BackColor = System.Drawing.SystemColors.Window;
            this.WizardControlField.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.WizardControlField.HeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.WizardControlField.HeaderPanel.Name = "_panelTop";
            this.WizardControlField.HeaderPanel.Size = new System.Drawing.Size(404, 76);
            this.WizardControlField.HeaderPanel.TabIndex = 1;
            this.WizardControlField.Size = new System.Drawing.Size(404, 386);
            // 
            // 
            // 
            this.WizardControlField.TrailerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.WizardControlField.TrailerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.WizardControlField.TrailerPanel.Location = new System.Drawing.Point(0, 338);
            this.WizardControlField.TrailerPanel.Name = "_panelBottom";
            this.WizardControlField.TrailerPanel.Size = new System.Drawing.Size(404, 48);
            this.WizardControlField.TrailerPanel.TabIndex = 2;
            // 
            // wizardStartPage
            // 
            this.wizardStartPage.CaptionTitle = "Sample Description";
            this.wizardStartPage.Controls.Add(this.label3);
            this.wizardStartPage.Controls.Add(this.label2);
            this.wizardStartPage.Controls.Add(this.label1);
            this.wizardStartPage.FullPage = false;
            this.wizardStartPage.InactiveBackColor = System.Drawing.Color.Empty;
            this.wizardStartPage.InactiveTextBackColor = System.Drawing.Color.Empty;
            this.wizardStartPage.InactiveTextColor = System.Drawing.Color.Empty;
            this.wizardStartPage.Location = new System.Drawing.Point(0, 0);
            this.wizardStartPage.Name = "wizardStartPage";
            this.wizardStartPage.SelectBackColor = System.Drawing.Color.Empty;
            this.wizardStartPage.Selected = false;
            this.wizardStartPage.SelectTextBackColor = System.Drawing.Color.Empty;
            this.wizardStartPage.SelectTextColor = System.Drawing.Color.Empty;
            this.wizardStartPage.Size = new System.Drawing.Size(410, 214);
            this.wizardStartPage.SubTitle = "Start page explaining what this sample demonstrates";
            this.wizardStartPage.TabIndex = 3;
            this.wizardStartPage.Title = "Start";
            this.wizardStartPage.ToolTip = "Page";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(400, 84);
            this.label3.TabIndex = 3;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(400, 74);
            this.label2.TabIndex = 2;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(400, 66);
            this.label1.TabIndex = 1;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // wizardInfo1
            // 
            this.wizardInfo1.CaptionTitle = "Gather Info 1";
            this.wizardInfo1.Controls.Add(this.groupBox1);
            this.wizardInfo1.FullPage = false;
            this.wizardInfo1.InactiveBackColor = System.Drawing.Color.Empty;
            this.wizardInfo1.InactiveTextBackColor = System.Drawing.Color.Empty;
            this.wizardInfo1.InactiveTextColor = System.Drawing.Color.Empty;
            this.wizardInfo1.Location = new System.Drawing.Point(0, 0);
            this.wizardInfo1.Name = "wizardInfo1";
            this.wizardInfo1.SelectBackColor = System.Drawing.Color.Empty;
            this.wizardInfo1.Selected = false;
            this.wizardInfo1.SelectTextBackColor = System.Drawing.Color.Empty;
            this.wizardInfo1.SelectTextColor = System.Drawing.Color.Empty;
            this.wizardInfo1.Size = new System.Drawing.Size(410, 214);
            this.wizardInfo1.SubTitle = "This is the first of two pages for gathering input";
            this.wizardInfo1.TabIndex = 4;
            this.wizardInfo1.Title = "Info1";
            this.wizardInfo1.ToolTip = "Page";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(24, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(160, 152);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Example of Selection";
            // 
            // radioButton3
            // 
            this.radioButton3.Location = new System.Drawing.Point(32, 112);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(104, 24);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Text = "Debug Install";
            // 
            // radioButton2
            // 
            this.radioButton2.Location = new System.Drawing.Point(32, 72);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(104, 24);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "Server Install";
            // 
            // radioButton1
            // 
            this.radioButton1.Location = new System.Drawing.Point(32, 32);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(104, 24);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Text = "Client Install";
            // 
            // wizardLegal
            // 
            this.wizardLegal.CaptionTitle = "Standard Legal Notice";
            this.wizardLegal.Controls.Add(this.radioButton5);
            this.wizardLegal.Controls.Add(this.radioButton4);
            this.wizardLegal.Controls.Add(this.label4);
            this.wizardLegal.Controls.Add(this.textBox1);
            this.wizardLegal.FullPage = false;
            this.wizardLegal.InactiveBackColor = System.Drawing.Color.Empty;
            this.wizardLegal.InactiveTextBackColor = System.Drawing.Color.Empty;
            this.wizardLegal.InactiveTextColor = System.Drawing.Color.Empty;
            this.wizardLegal.Location = new System.Drawing.Point(0, 0);
            this.wizardLegal.Name = "wizardLegal";
            this.wizardLegal.SelectBackColor = System.Drawing.Color.Empty;
            this.wizardLegal.Selected = false;
            this.wizardLegal.SelectTextBackColor = System.Drawing.Color.Empty;
            this.wizardLegal.SelectTextColor = System.Drawing.Color.Empty;
            this.wizardLegal.Size = new System.Drawing.Size(410, 214);
            this.wizardLegal.SubTitle = "Force the user to agree a license agreement for product";
            this.wizardLegal.TabIndex = 5;
            this.wizardLegal.Title = "Legal";
            this.wizardLegal.ToolTip = "Page";
            // 
            // radioButton5
            // 
            this.radioButton5.Checked = true;
            this.radioButton5.Location = new System.Drawing.Point(216, 160);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(88, 24);
            this.radioButton5.TabIndex = 3;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "I Disagree";
            this.radioButton5.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
            // 
            // radioButton4
            // 
            this.radioButton4.Location = new System.Drawing.Point(120, 160);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(72, 24);
            this.radioButton4.TabIndex = 2;
            this.radioButton4.Text = "I Agree";
            this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(32, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 22);
            this.label4.TabIndex = 1;
            this.label4.Text = "Must Agree Terms";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(32, 32);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(352, 120);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "You must select \'I Agree\' before the \'Next\' button will enable itself. This custo" +
                "m action is not part of the WizardControl, see the sample code which is trivial." +
                "";
            // 
            // wizardWarn
            // 
            this.wizardWarn.CaptionTitle = "Warning, about to install";
            this.wizardWarn.Controls.Add(this.label6);
            this.wizardWarn.Controls.Add(this.label5);
            this.wizardWarn.FullPage = false;
            this.wizardWarn.InactiveBackColor = System.Drawing.Color.Empty;
            this.wizardWarn.InactiveTextBackColor = System.Drawing.Color.Empty;
            this.wizardWarn.InactiveTextColor = System.Drawing.Color.Empty;
            this.wizardWarn.Location = new System.Drawing.Point(0, 0);
            this.wizardWarn.Name = "wizardWarn";
            this.wizardWarn.SelectBackColor = System.Drawing.Color.Empty;
            this.wizardWarn.Selected = false;
            this.wizardWarn.SelectTextBackColor = System.Drawing.Color.Empty;
            this.wizardWarn.SelectTextColor = System.Drawing.Color.Empty;
            this.wizardWarn.Size = new System.Drawing.Size(410, 214);
            this.wizardWarn.SubTitle = "This warns user that installation is about to begin";
            this.wizardWarn.TabIndex = 6;
            this.wizardWarn.Title = "Warn";
            this.wizardWarn.ToolTip = "Page";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(40, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(360, 64);
            this.label6.TabIndex = 1;
            this.label6.Text = "Warn user that pressing \'Next\' will begin installation process.";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(40, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(280, 24);
            this.label5.TabIndex = 0;
            this.label5.Text = "Last page before installation.";
            // 
            // wizardInstall
            // 
            this.wizardInstall.CaptionTitle = "Installing";
            this.wizardInstall.Controls.Add(this.label7);
            this.wizardInstall.Controls.Add(this.progressBar1);
            this.wizardInstall.FullPage = false;
            this.wizardInstall.InactiveBackColor = System.Drawing.Color.Empty;
            this.wizardInstall.InactiveTextBackColor = System.Drawing.Color.Empty;
            this.wizardInstall.InactiveTextColor = System.Drawing.Color.Empty;
            this.wizardInstall.Location = new System.Drawing.Point(0, 0);
            this.wizardInstall.Name = "wizardInstall";
            this.wizardInstall.SelectBackColor = System.Drawing.Color.Empty;
            this.wizardInstall.Selected = false;
            this.wizardInstall.SelectTextBackColor = System.Drawing.Color.Empty;
            this.wizardInstall.SelectTextColor = System.Drawing.Color.Empty;
            this.wizardInstall.Size = new System.Drawing.Size(410, 214);
            this.wizardInstall.SubTitle = "Perform some fake installation process";
            this.wizardInstall.TabIndex = 7;
            this.wizardInstall.Title = "Install";
            this.wizardInstall.ToolTip = "Page";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(40, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 23);
            this.label7.TabIndex = 1;
            this.label7.Text = "Fake Installation";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(40, 48);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(328, 24);
            this.progressBar1.TabIndex = 0;
            // 
            // wizardFinish
            // 
            this.wizardFinish.CaptionTitle = "Intall Complete";
            this.wizardFinish.Controls.Add(this.label10);
            this.wizardFinish.FullPage = false;
            this.wizardFinish.InactiveBackColor = System.Drawing.Color.Empty;
            this.wizardFinish.InactiveTextBackColor = System.Drawing.Color.Empty;
            this.wizardFinish.InactiveTextColor = System.Drawing.Color.Empty;
            this.wizardFinish.Location = new System.Drawing.Point(0, 0);
            this.wizardFinish.Name = "wizardFinish";
            this.wizardFinish.SelectBackColor = System.Drawing.Color.Empty;
            this.wizardFinish.Selected = false;
            this.wizardFinish.SelectTextBackColor = System.Drawing.Color.Empty;
            this.wizardFinish.SelectTextColor = System.Drawing.Color.Empty;
            this.wizardFinish.Size = new System.Drawing.Size(410, 214);
            this.wizardFinish.SubTitle = "This page gives the success or failure of attempting the previous install process" +
                "";
            this.wizardFinish.TabIndex = 8;
            this.wizardFinish.Title = "Finished";
            this.wizardFinish.ToolTip = "Page";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(32, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(304, 104);
            this.label10.TabIndex = 0;
            this.label10.Text = "Installation has completed with success.";
            // 
            // wizardInfo2
            // 
            this.wizardInfo2.CaptionTitle = "Gather Info 2";
            this.wizardInfo2.Controls.Add(this.label9);
            this.wizardInfo2.Controls.Add(this.textBox3);
            this.wizardInfo2.Controls.Add(this.label8);
            this.wizardInfo2.Controls.Add(this.textBox2);
            this.wizardInfo2.FullPage = false;
            this.wizardInfo2.InactiveBackColor = System.Drawing.Color.Empty;
            this.wizardInfo2.InactiveTextBackColor = System.Drawing.Color.Empty;
            this.wizardInfo2.InactiveTextColor = System.Drawing.Color.Empty;
            this.wizardInfo2.Location = new System.Drawing.Point(0, 0);
            this.wizardInfo2.Name = "wizardInfo2";
            this.wizardInfo2.SelectBackColor = System.Drawing.Color.Empty;
            this.wizardInfo2.Selected = false;
            this.wizardInfo2.SelectTextBackColor = System.Drawing.Color.Empty;
            this.wizardInfo2.SelectTextColor = System.Drawing.Color.Empty;
            this.wizardInfo2.Size = new System.Drawing.Size(410, 214);
            this.wizardInfo2.SubTitle = "This is the second of two pages for gathering input";
            this.wizardInfo2.TabIndex = 9;
            this.wizardInfo2.Title = "Info2";
            this.wizardInfo2.ToolTip = "Page";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(48, 80);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(136, 23);
            this.label9.TabIndex = 3;
            this.label9.Text = "Enter Company Name";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(48, 104);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(160, 20);
            this.textBox3.TabIndex = 2;
            this.textBox3.Text = "ACNE Corp.";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(48, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 23);
            this.label8.TabIndex = 1;
            this.label8.Text = "Enter Username";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(48, 40);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(160, 20);
            this.textBox2.TabIndex = 0;
            this.textBox2.Text = "Anon";
            // 
            // SampleWizard
            // 
            this.ClientSize = new System.Drawing.Size(404, 386);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SampleWizard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.wizardStartPage.ResumeLayout(false);
            this.wizardInfo1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.wizardLegal.ResumeLayout(false);
            this.wizardLegal.PerformLayout();
            this.wizardWarn.ResumeLayout(false);
            this.wizardInstall.ResumeLayout(false);
            this.wizardFinish.ResumeLayout(false);
            this.wizardInfo2.ResumeLayout(false);
            this.wizardInfo2.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

        private void radioButton4_CheckedChanged(object sender, System.EventArgs e)
        {
            if (this.radioButton4.Checked)
            {
                this.WizardControlField.EnableNextButton = Crownwood.DotNetMagic.Controls.Status.Default;
               radioButton5.Checked = false;
            }
            else                    
            {
                this.WizardControlField.EnableNextButton = Crownwood.DotNetMagic.Controls.Status.No;
               radioButton5.Checked = true;
            }
        }

        private void radioButton5_CheckedChanged(object sender, System.EventArgs e)
        {
            if (this.radioButton5.Checked)
            {
                this.WizardControlField.EnableNextButton = Crownwood.DotNetMagic.Controls.Status.No;
                radioButton4.Checked = false;
            }
            else                    
            {
                this.WizardControlField.EnableNextButton = Crownwood.DotNetMagic.Controls.Status.Default;
                radioButton4.Checked = true;
            }
        }
    
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() 
        {
            Application.EnableVisualStyles();
            Application.Run(new SampleWizard());
        }

        protected override void OnWizardPageEnter(Crownwood.DotNetMagic.Controls.WizardPage wp, 
                                                  Crownwood.DotNetMagic.Controls.WizardControl wc)
        {
            // Asking for licence terms by entering page?
            if (wp.Name == "wizardLegal")
            {
                if (this.radioButton4.Checked)
                    wc.EnableNextButton = Crownwood.DotNetMagic.Controls.Status.Default;
                else
                    wc.EnableNextButton = Crownwood.DotNetMagic.Controls.Status.No;
            }
            
            // Started the installation process by entering page 5?
            if (wp.Name == "wizardInstall")
            {
                // Kick off a timer to represent progress
                installCount = 0;
                installTimer = new Timer();
                installTimer.Interval = 250;
                installTimer.Tick += new EventHandler(OnProgressTick);
                installTimer.Start();
            }
        }
        
        protected override void OnWizardPageLeave(Crownwood.DotNetMagic.Controls.WizardPage wp, 
                                                  Crownwood.DotNetMagic.Controls.WizardControl wc)
        {
            // Leaving page means we have to restore default status of next button
            if (wp.Name == "wizardLegal")
            {
                // Default the next button to disable
                wc.EnableNextButton = Crownwood.DotNetMagic.Controls.Status.Default;
            }
        }
            
        protected override void OnCancelClick(object sender, System.EventArgs e)
        {
            // Suspend any installation process if happening
            if (installTimer != null)
                installTimer.Stop();
        
            if (MessageBox.Show(this, "Sure you want to exit?", "Cancel Pressed", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                // Let base class close the form
                base.OnCancelClick(sender, e);
            }
            else
            {
                // Resume any installation process if happening
                if (installTimer != null)
                    installTimer.Start();
            }
        }

        private void OnProgressTick(object sender, EventArgs e)
        {
            installCount++;
            
            // Finished yet?
            if (installCount >= 20)
            {
                // No longer need to simulate actions
                installTimer.Stop();
                
                // Move to last page
                base.WizardControlField.SelectedIndex = base.WizardControlField.WizardPages.Count - 1;
            }
            else
            {
                // Update percentage completed
                progressBar1.Value = 100 / 20 * installCount;   
            }
        }
    }
}

