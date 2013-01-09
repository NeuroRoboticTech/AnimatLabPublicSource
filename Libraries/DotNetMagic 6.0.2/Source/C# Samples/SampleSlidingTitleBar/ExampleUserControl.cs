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

namespace SampleSlidingTitleBar
{
	/// <summary>
	/// Summary description for ExampleUserControl.
	/// </summary>
	public class ExampleUserControl : System.Windows.Forms.UserControl
	{
		// Instance fields
		private SlidingTitleBar _bar;
		
		// Designer generated
		private System.Windows.Forms.TextBox textBoxNumber;
		private System.Windows.Forms.Button number7;
		private System.Windows.Forms.Button number8;
		private System.Windows.Forms.Button number9;
		private System.Windows.Forms.Button number4;
		private System.Windows.Forms.Button number5;
		private System.Windows.Forms.Button number6;
		private System.Windows.Forms.Button number1;
		private System.Windows.Forms.Button number2;
		private System.Windows.Forms.Button number3;
		private System.Windows.Forms.Button number0;
		private System.Windows.Forms.Button decimalPoint;
		private System.Windows.Forms.Button opMinus;
		private System.Windows.Forms.Button opPlus;
		private System.Windows.Forms.Button opEquals;
		private System.Windows.Forms.Button buttonCloseUp;
		
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ExampleUserControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}
		
		public SlidingTitleBar Bar
		{
			set { _bar = value; }
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
			this.textBoxNumber = new System.Windows.Forms.TextBox();
			this.number7 = new System.Windows.Forms.Button();
			this.number8 = new System.Windows.Forms.Button();
			this.number9 = new System.Windows.Forms.Button();
			this.number4 = new System.Windows.Forms.Button();
			this.number5 = new System.Windows.Forms.Button();
			this.number6 = new System.Windows.Forms.Button();
			this.number1 = new System.Windows.Forms.Button();
			this.number2 = new System.Windows.Forms.Button();
			this.number3 = new System.Windows.Forms.Button();
			this.number0 = new System.Windows.Forms.Button();
			this.decimalPoint = new System.Windows.Forms.Button();
			this.opMinus = new System.Windows.Forms.Button();
			this.opPlus = new System.Windows.Forms.Button();
			this.opEquals = new System.Windows.Forms.Button();
			this.buttonCloseUp = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// textBoxNumber
			// 
			this.textBoxNumber.Location = new System.Drawing.Point(24, 24);
			this.textBoxNumber.Name = "textBoxNumber";
			this.textBoxNumber.Size = new System.Drawing.Size(192, 20);
			this.textBoxNumber.TabIndex = 0;
			this.textBoxNumber.Text = "Press a button";
			this.textBoxNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// number7
			// 
			this.number7.BackColor = System.Drawing.Color.IndianRed;
			this.number7.ForeColor = System.Drawing.Color.White;
			this.number7.Location = new System.Drawing.Point(24, 64);
			this.number7.Name = "number7";
			this.number7.Size = new System.Drawing.Size(40, 23);
			this.number7.TabIndex = 1;
			this.number7.Text = "7";
			this.number7.Click += new System.EventHandler(this.number7_Click);
			// 
			// number8
			// 
			this.number8.BackColor = System.Drawing.Color.IndianRed;
			this.number8.ForeColor = System.Drawing.Color.White;
			this.number8.Location = new System.Drawing.Point(72, 64);
			this.number8.Name = "number8";
			this.number8.Size = new System.Drawing.Size(40, 23);
			this.number8.TabIndex = 2;
			this.number8.Text = "8";
			this.number8.Click += new System.EventHandler(this.number8_Click);
			// 
			// number9
			// 
			this.number9.BackColor = System.Drawing.Color.IndianRed;
			this.number9.ForeColor = System.Drawing.Color.White;
			this.number9.Location = new System.Drawing.Point(120, 64);
			this.number9.Name = "number9";
			this.number9.Size = new System.Drawing.Size(40, 23);
			this.number9.TabIndex = 3;
			this.number9.Text = "9";
			this.number9.Click += new System.EventHandler(this.number9_Click);
			// 
			// number4
			// 
			this.number4.BackColor = System.Drawing.Color.IndianRed;
			this.number4.ForeColor = System.Drawing.Color.White;
			this.number4.Location = new System.Drawing.Point(24, 96);
			this.number4.Name = "number4";
			this.number4.Size = new System.Drawing.Size(40, 23);
			this.number4.TabIndex = 4;
			this.number4.Text = "4";
			this.number4.Click += new System.EventHandler(this.number4_Click);
			// 
			// number5
			// 
			this.number5.BackColor = System.Drawing.Color.IndianRed;
			this.number5.ForeColor = System.Drawing.Color.White;
			this.number5.Location = new System.Drawing.Point(72, 96);
			this.number5.Name = "number5";
			this.number5.Size = new System.Drawing.Size(40, 23);
			this.number5.TabIndex = 5;
			this.number5.Text = "5";
			this.number5.Click += new System.EventHandler(this.number5_Click);
			// 
			// number6
			// 
			this.number6.BackColor = System.Drawing.Color.IndianRed;
			this.number6.ForeColor = System.Drawing.Color.White;
			this.number6.Location = new System.Drawing.Point(120, 96);
			this.number6.Name = "number6";
			this.number6.Size = new System.Drawing.Size(40, 23);
			this.number6.TabIndex = 6;
			this.number6.Text = "6";
			this.number6.Click += new System.EventHandler(this.number6_Click);
			// 
			// number1
			// 
			this.number1.BackColor = System.Drawing.Color.IndianRed;
			this.number1.ForeColor = System.Drawing.Color.White;
			this.number1.Location = new System.Drawing.Point(24, 128);
			this.number1.Name = "number1";
			this.number1.Size = new System.Drawing.Size(40, 23);
			this.number1.TabIndex = 7;
			this.number1.Text = "1";
			this.number1.Click += new System.EventHandler(this.number1_Click);
			// 
			// number2
			// 
			this.number2.BackColor = System.Drawing.Color.IndianRed;
			this.number2.ForeColor = System.Drawing.Color.White;
			this.number2.Location = new System.Drawing.Point(72, 128);
			this.number2.Name = "number2";
			this.number2.Size = new System.Drawing.Size(40, 23);
			this.number2.TabIndex = 8;
			this.number2.Text = "2";
			this.number2.Click += new System.EventHandler(this.number2_Click);
			// 
			// number3
			// 
			this.number3.BackColor = System.Drawing.Color.IndianRed;
			this.number3.ForeColor = System.Drawing.Color.White;
			this.number3.Location = new System.Drawing.Point(120, 128);
			this.number3.Name = "number3";
			this.number3.Size = new System.Drawing.Size(40, 23);
			this.number3.TabIndex = 9;
			this.number3.Text = "3";
			this.number3.Click += new System.EventHandler(this.number3_Click);
			// 
			// number0
			// 
			this.number0.BackColor = System.Drawing.Color.IndianRed;
			this.number0.ForeColor = System.Drawing.Color.White;
			this.number0.Location = new System.Drawing.Point(24, 160);
			this.number0.Name = "number0";
			this.number0.Size = new System.Drawing.Size(88, 23);
			this.number0.TabIndex = 10;
			this.number0.Text = "0";
			this.number0.Click += new System.EventHandler(this.number0_Click);
			// 
			// decimalPoint
			// 
			this.decimalPoint.BackColor = System.Drawing.Color.IndianRed;
			this.decimalPoint.ForeColor = System.Drawing.Color.White;
			this.decimalPoint.Location = new System.Drawing.Point(120, 160);
			this.decimalPoint.Name = "decimalPoint";
			this.decimalPoint.Size = new System.Drawing.Size(40, 23);
			this.decimalPoint.TabIndex = 11;
			this.decimalPoint.Text = ".";
			this.decimalPoint.Click += new System.EventHandler(this.decimalPoint_Click);
			// 
			// opMinus
			// 
			this.opMinus.BackColor = System.Drawing.Color.IndianRed;
			this.opMinus.ForeColor = System.Drawing.Color.White;
			this.opMinus.Location = new System.Drawing.Point(176, 64);
			this.opMinus.Name = "opMinus";
			this.opMinus.Size = new System.Drawing.Size(40, 23);
			this.opMinus.TabIndex = 12;
			this.opMinus.Text = "-";
			this.opMinus.Click += new System.EventHandler(this.opMinus_Click);
			// 
			// opPlus
			// 
			this.opPlus.BackColor = System.Drawing.Color.IndianRed;
			this.opPlus.ForeColor = System.Drawing.Color.White;
			this.opPlus.Location = new System.Drawing.Point(176, 96);
			this.opPlus.Name = "opPlus";
			this.opPlus.Size = new System.Drawing.Size(40, 23);
			this.opPlus.TabIndex = 13;
			this.opPlus.Text = "+";
			this.opPlus.Click += new System.EventHandler(this.opPlus_Click);
			// 
			// opEquals
			// 
			this.opEquals.BackColor = System.Drawing.Color.IndianRed;
			this.opEquals.ForeColor = System.Drawing.Color.White;
			this.opEquals.Location = new System.Drawing.Point(176, 160);
			this.opEquals.Name = "opEquals";
			this.opEquals.Size = new System.Drawing.Size(40, 23);
			this.opEquals.TabIndex = 14;
			this.opEquals.Text = "=";
			this.opEquals.Click += new System.EventHandler(this.opEquals_Click);
			// 
			// buttonCloseUp
			// 
			this.buttonCloseUp.BackColor = System.Drawing.Color.IndianRed;
			this.buttonCloseUp.ForeColor = System.Drawing.Color.White;
			this.buttonCloseUp.Location = new System.Drawing.Point(24, 200);
			this.buttonCloseUp.Name = "buttonCloseUp";
			this.buttonCloseUp.Size = new System.Drawing.Size(192, 23);
			this.buttonCloseUp.TabIndex = 15;
			this.buttonCloseUp.Text = "Close When Slide Open";
			this.buttonCloseUp.Click += new System.EventHandler(this.buttonCloseUp_Click);
			// 
			// ExampleUserControl
			// 
			this.BackColor = System.Drawing.Color.RosyBrown;
			this.Controls.Add(this.buttonCloseUp);
			this.Controls.Add(this.opEquals);
			this.Controls.Add(this.opPlus);
			this.Controls.Add(this.opMinus);
			this.Controls.Add(this.decimalPoint);
			this.Controls.Add(this.number0);
			this.Controls.Add(this.number3);
			this.Controls.Add(this.number2);
			this.Controls.Add(this.number1);
			this.Controls.Add(this.number6);
			this.Controls.Add(this.number5);
			this.Controls.Add(this.number4);
			this.Controls.Add(this.number9);
			this.Controls.Add(this.number8);
			this.Controls.Add(this.number7);
			this.Controls.Add(this.textBoxNumber);
			this.Name = "ExampleUserControl";
			this.Size = new System.Drawing.Size(240, 240);
			this.ResumeLayout(false);

		}
		#endregion

		private void number7_Click(object sender, System.EventArgs e)
		{
			textBoxNumber.Text = "Pressed 7";
		}

		private void number8_Click(object sender, System.EventArgs e)
		{
			textBoxNumber.Text = "Pressed 8";
		}		

		private void number9_Click(object sender, System.EventArgs e)
		{
			textBoxNumber.Text = "Pressed 9";		
		}

		private void number4_Click(object sender, System.EventArgs e)
		{
			textBoxNumber.Text = "Pressed 4";		
		}

		private void number5_Click(object sender, System.EventArgs e)
		{
			textBoxNumber.Text = "Pressed 5";
		}

		private void number6_Click(object sender, System.EventArgs e)
		{
			textBoxNumber.Text = "Pressed 6";		
		}

		private void number0_Click(object sender, System.EventArgs e)
		{
			textBoxNumber.Text = "Pressed 0";		
		}

		private void number1_Click(object sender, System.EventArgs e)
		{
			textBoxNumber.Text = "Pressed 1";		
		}

		private void number2_Click(object sender, System.EventArgs e)
		{
			textBoxNumber.Text = "Pressed 2";		
		}

		private void number3_Click(object sender, System.EventArgs e)
		{
			textBoxNumber.Text = "Pressed 3";		
		}

		private void decimalPoint_Click(object sender, System.EventArgs e)
		{
			textBoxNumber.Text = "Pressed .";		
		}

		private void opMinus_Click(object sender, System.EventArgs e)
		{
			textBoxNumber.Text = "Pressed -";		
		}

		private void opPlus_Click(object sender, System.EventArgs e)
		{
			textBoxNumber.Text = "Pressed +";		
		}

		private void opEquals_Click(object sender, System.EventArgs e)
		{
			textBoxNumber.Text = "Pressed =";		
		}

		private void buttonCloseUp_Click(object sender, System.EventArgs e)
		{
			_bar.CloseUp();
		}
	}
}
