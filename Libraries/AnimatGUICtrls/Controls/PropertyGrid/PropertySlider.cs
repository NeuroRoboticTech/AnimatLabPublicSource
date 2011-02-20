using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace AnimatGuiCtrls.Controls
{
	/// <summary>
	/// Summary description for PropertySlider.
	/// </summary>
	public class PropertySlider : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.TrackBar ctrlSlider;
		private System.Windows.Forms.PictureBox ctrlPropertyImage;
		private System.Windows.Forms.CheckBox ctrlPropertyName;
		private System.Windows.Forms.TextBox txtValue;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		protected PropertySpec m_PropertySpec;

		protected double m_dblSliderMin = 0;
		protected double m_dblSliderMax = 5;
		protected double m_dblCurrentValue = 1;
		
		protected string m_strPropertyName;

		public PropertySpec Property 
		{
			get{return m_PropertySpec;}
			set 
			{
				m_PropertySpec = value;
			}
		}

		public System.Windows.Forms.TrackBar Slider
		{get{return ctrlSlider;}}

		public System.Windows.Forms.PictureBox PropertyImage
		{get{return ctrlPropertyImage;}}

		public string PropertyName
		{
			get{return m_strPropertyName;}
			set {m_strPropertyName = value;}
		}

		public string SliderName
		{
			get{return ctrlPropertyName.Text;}
			set {ctrlPropertyName.Text = value;}
		}

		public double SliderMinimum
		{
			get{return m_dblSliderMin;}
			set 
			{
				if(value >= m_dblSliderMax)
					throw new System.Exception("You can not set a minimum value (" + value.ToString() + 
                                     ") that is higher than the current maximum value (" + m_dblSliderMax.ToString() + ".");

				m_dblSliderMin = value;
			}
		}

		public double SliderMaximum
		{
			get{return m_dblSliderMax;}
			set 
			{
				if(value <= m_dblSliderMin)
					throw new System.Exception("You can not set a maximum value (" + value.ToString() + 
						") that is lower than the current minimum value (" + m_dblSliderMin.ToString() + ".");

				m_dblSliderMax = value;
			}
		}

		public PropertySlider()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		/// <summary>
		/// Occurs when a PropertyGrid requests the value of a property.
		/// </summary>
		public event PropertySpecEventHandler GetValue;

		/// <summary>
		/// Occurs when the user changes the value of a property in a PropertyGrid.
		/// </summary>
		public event PropertySpecEventHandler SetValue;

		/// <summary>
		/// Raises the GetValue event.
		/// </summary>
		/// <param name="e">A PropertySpecEventArgs that contains the event data.</param>
		protected virtual void OnGetValue(PropertySpecEventArgs e)
		{
			if(GetValue != null)
				GetValue(this, e);
		}

		/// <summary>
		/// Raises the SetValue event.
		/// </summary>
		/// <param name="e">A PropertySpecEventArgs that contains the event data.</param>
		protected virtual void OnSetValue(PropertySpecEventArgs e)
		{
			if(SetValue != null)
				SetValue(this, e);
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
			this.ctrlSlider = new System.Windows.Forms.TrackBar();
			this.ctrlPropertyImage = new System.Windows.Forms.PictureBox();
			this.ctrlPropertyName = new System.Windows.Forms.CheckBox();
			this.txtValue = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.ctrlSlider)).BeginInit();
			this.SuspendLayout();
			// 
			// ctrlSlider
			// 
			this.ctrlSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ctrlSlider.Location = new System.Drawing.Point(0, 16);
			this.ctrlSlider.Name = "ctrlSlider";
			this.ctrlSlider.Size = new System.Drawing.Size(248, 45);
			this.ctrlSlider.TabIndex = 0;
			this.ctrlSlider.Scroll += new System.EventHandler(this.ctrlSlider_Scroll);
			// 
			// ctrlPropertyImage
			// 
			this.ctrlPropertyImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ctrlPropertyImage.Location = new System.Drawing.Point(248, 8);
			this.ctrlPropertyImage.Name = "ctrlPropertyImage";
			this.ctrlPropertyImage.Size = new System.Drawing.Size(30, 30);
			this.ctrlPropertyImage.TabIndex = 1;
			this.ctrlPropertyImage.TabStop = false;
			// 
			// ctrlPropertyName
			// 
			this.ctrlPropertyName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ctrlPropertyName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ctrlPropertyName.Location = new System.Drawing.Point(8, 0);
			this.ctrlPropertyName.Name = "ctrlPropertyName";
			this.ctrlPropertyName.Size = new System.Drawing.Size(232, 16);
			this.ctrlPropertyName.TabIndex = 2;
			this.ctrlPropertyName.Text = "Width";
			this.ctrlPropertyName.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// txtValue
			// 
			this.txtValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtValue.Location = new System.Drawing.Point(288, 16);
			this.txtValue.Name = "txtValue";
			this.txtValue.Size = new System.Drawing.Size(56, 20);
			this.txtValue.TabIndex = 3;
			this.txtValue.Text = "";
			this.txtValue.ModifiedChanged += new System.EventHandler(this.ctrlValue_Changed);
			this.txtValue.Leave += new System.EventHandler(this.ctrlValue_Changed);
			// 
			// PropertySlider
			// 
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.ctrlPropertyImage);
			this.Controls.Add(this.txtValue);
			this.Controls.Add(this.ctrlSlider);
			this.Controls.Add(this.ctrlPropertyName);
			this.Name = "PropertySlider";
			this.Size = new System.Drawing.Size(352, 48);
			((System.ComponentModel.ISupportInitialize)(this.ctrlSlider)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		protected double CalculateValueFromSlider()
		{
			return ((ctrlSlider.Value / (float) System.Math.Abs(ctrlSlider.Maximum - ctrlSlider.Minimum))  * System.Math.Abs(m_dblSliderMax-m_dblSliderMin)) + m_dblSliderMin;
		}

		protected int CalculateSliderValue(double dblVal)
		{
			double dblVal2 = (dblVal - m_dblSliderMin) / (double) System.Math.Abs(m_dblSliderMax-m_dblSliderMin);
			int iVal = (int) (dblVal2  * System.Math.Abs(ctrlSlider.Maximum - ctrlSlider.Minimum));

			return iVal;
		}

		protected void SetSliderValue(double dblVal)
		{
			txtValue.Text = dblVal.ToString();

			int iVal = CalculateSliderValue(dblVal);

			if(iVal < ctrlSlider.Minimum)
				ctrlSlider.Value = ctrlSlider.Minimum;
			else if(iVal > ctrlSlider.Maximum)
				ctrlSlider.Value = ctrlSlider.Maximum;
			else
				ctrlSlider.Value = iVal;

			m_dblCurrentValue = dblVal;
		}

		public void GetValues()
		{
			if(m_PropertySpec != null)
			{
				PropertySpecEventArgs args = new PropertySpecEventArgs(m_PropertySpec, 0);
				OnGetValue(args);
				
				double dblValue = Convert.ToDouble(args.Value);
				SetSliderValue(dblValue);
			}
		}

		protected void ctrlSlider_Scroll(object sender, System.EventArgs e)
		{
			try
			{
				if(m_PropertySpec != null)
				{
					double dblVal = CalculateValueFromSlider();

					PropertySpecEventArgs args = new PropertySpecEventArgs(m_PropertySpec, Convert.ChangeType(dblVal, Type.GetType(m_PropertySpec.TypeName)));
					OnSetValue(args);
					txtValue.Text = dblVal.ToString();

					m_dblCurrentValue = dblVal;
				}
			}
			catch(System.Exception ex)
			{
				string strVal = ex.Message; //Freaking error CS0168
				//System.Windows.Forms.MessageBox.Show(this, ex.Message, "Slider Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error); 
			}

		}

		protected void ctrlValue_Changed(object sender, System.EventArgs e)
		{
			try
			{
				if(txtValue.Text.Trim().Length == 0)
					throw new System.Exception("Invalid property value for '" + this.PropertyName + "', you must specify a value.");

				if(m_PropertySpec != null)
				{
					double dblVal = Convert.ToDouble(txtValue.Text);
					
					PropertySpecEventArgs args = new PropertySpecEventArgs(m_PropertySpec, Convert.ChangeType(dblVal, Type.GetType(m_PropertySpec.TypeName)));
					OnSetValue(args);
					SetSliderValue(dblVal);

					m_dblCurrentValue = dblVal;
				}
			}
			catch(System.Exception ex)
			{
				//System.Windows.Forms.MessageBox.Show(this, ex.Message, "Slider Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error); 
				string strVal = ex.Message; //Freaking error CS0168

				//Lets reset the value back to the one in the slider.
				txtValue.Text = m_dblCurrentValue.ToString();
			}

		}

	}
}
