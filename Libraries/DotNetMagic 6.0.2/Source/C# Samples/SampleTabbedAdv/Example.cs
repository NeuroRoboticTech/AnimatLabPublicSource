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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Crownwood.DotNetMagic.Common;
using Crownwood.DotNetMagic.Controls;

namespace SampleTabbedAdv
{
	/// <summary>
	/// Summary description for Example.
	/// </summary>
	public class Example : System.Windows.Forms.UserControl
	{
		// Private field
		private EventHandler _arrowClick;

		// Designer generated
		private Crownwood.DotNetMagic.Controls.TitleBar titleBar1;
		private System.Windows.Forms.RichTextBox richTextBox1;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Example()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		// New constructor
		public Example(string preText, string text, string postText,
					   ArrowButton arrow, EventHandler arrowClick)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			
			// Set initial values
			titleBar1.PreText = preText;
			titleBar1.Text = text;
			titleBar1.PostText = postText;
			titleBar1.ArrowButton = arrow;
			
			// Remember callback event handler
			_arrowClick = arrowClick;

			if ((arrow == ArrowButton.UpArrow) ||
				(arrow == ArrowButton.DownArrow) || 
				(arrow == ArrowButton.None))
				titleBar1.Dock = DockStyle.Top;
			
			if (arrow == ArrowButton.RightArrow)
				titleBar1.Dock = DockStyle.Right;

			if (arrow == ArrowButton.LeftArrow)
				titleBar1.Dock = DockStyle.Left;
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
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
			this.titleBar1 = new Crownwood.DotNetMagic.Controls.TitleBar();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// titleBar1
			// 
			this.titleBar1.ArrowButton = Crownwood.DotNetMagic.Controls.ArrowButton.DownArrow;
			this.titleBar1.Location = new System.Drawing.Point(0, 0);
			this.titleBar1.MouseOverColor = System.Drawing.Color.Empty;
			this.titleBar1.Name = "titleBar";
			this.titleBar1.Size = new System.Drawing.Size(24, 24);
			this.titleBar1.TabIndex = 0;
			this.titleBar1.Text = "titleBar1";
			this.titleBar1.ButtonClick += new System.EventHandler(this.OnArrowClick);
			// 
			// richTextBox
			// 
			this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox1.Location = new System.Drawing.Point(0, 0);
			this.richTextBox1.Name = "richTextBox";
			this.richTextBox1.Size = new System.Drawing.Size(368, 216);
			this.richTextBox1.TabIndex = 1;
			this.richTextBox1.Text = "";
			// 
			// Example
			// 
			this.Controls.Add(this.richTextBox1);
			this.Controls.Add(this.titleBar1);
			this.Name = "Example";
			this.Size = new System.Drawing.Size(368, 216);
			this.ResumeLayout(false);

		}
		#endregion

		// Fire constructor provided event user clicks titlebar arrow
		private void OnArrowClick(object sender, System.EventArgs e)
		{
			if (_arrowClick != null)
				_arrowClick(sender, e);
		}
		
		// Allow direct access to the titlebar
		public Crownwood.DotNetMagic.Controls.TitleBar TitleBar
		{
			get { return titleBar1; }
		}

		// Allow direct access to the richtextbox
		public RichTextBox RichTextBox
		{
			get { return richTextBox1; }
		}
		
		// Caller can discover minimum requested size
		public Size MinimumRequestedSize
		{
			get { return new Size(23, 23); }
		}
	}
}
