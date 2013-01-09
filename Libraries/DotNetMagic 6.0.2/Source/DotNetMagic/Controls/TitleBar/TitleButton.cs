// *****************************************************************************
// 
//  (c) Crownwood Software Ltd 2004-2006. All rights reserved. 
//	The software and associated documentation supplied hereunder are the 
//	proprietary information of Crownwood Software Ltd, Bracknell, 
//	Berkshire, England and are supplied subject to licence terms.
// 
//  Version 6.0.1.0 	www.crownwood.net
// *****************************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Definition of a button to appear on the TitleBar.
	/// </summary>
    [ToolboxItem(false)]
	public class TitleButton : Component
	{
		// Instance fields
		private Image _image;
		private bool _enabled;
		private bool _visible;
		private int _pictureBorder;
        private ButtonWithStyle _control;

		/// <summary>
		/// Occurs when user clicks the button.
		/// </summary>
		public event EventHandler Click;

		/// <summary>
		/// Occurs when a property changes value.
		/// </summary>
		public event EventHandler PropertyChanged;

		/// <summary>
		/// Initialize a new instance of the TitleButton class.
		/// </summary>
		public TitleButton()
		{
			// Default state
			_enabled = true;
			_visible = true;
			_image = null;
			_pictureBorder = 3;
		}

		/// <summary>
		/// Gets and sets the enabled state of button.
		/// </summary>
		[Category("Behavior")]
		[Description("Indicates whether the button is enabled.")]
		[DefaultValue(true)]
		public bool Enabled
		{
			get { return _enabled; }

			set
			{
				if (_enabled != value)
				{
					_enabled = value;
					OnPropertyChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Resets the Enabled property.
		/// </summary>
		public void ResetEnabled()
		{
			Enabled = true;
		}

		/// <summary>
		/// Gets and sets the visible state of button.
		/// </summary>
		[Category("Behavior")]
		[Description("Determines whether the button is visible or hidden.")]
		[DefaultValue(true)]
		public bool Visible
		{
			get { return _visible; }

			set
			{
				if (_visible != value)
				{
					_visible = value;
					OnPropertyChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Resets the Visible property.
		/// </summary>
		public void ResetVisible()
		{
			Visible = true;
		}

		/// <summary>
		/// Gets and sets the Image for the button.
		/// </summary>
		[Category("Appearance")]
		[Description("The image for display.")]
		[DefaultValue(null)]
		public Image Image
		{
			get { return _image; }

			set
			{
				if (_image != value)
				{
					_image = value;
					OnPropertyChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Resets the Image property.
		/// </summary>
		public void ResetImage()
		{
			Image = null;
		}

		/// <summary>
		/// Gets and sets the border space around the image.
		/// </summary>
		[Category("Appearance")]
		[Description("Defines the border space around the image.")]
		[DefaultValue(3)]
		public int PictureBorder
		{
			get { return _pictureBorder; }

			set
			{
				if (_pictureBorder != value)
				{
					_pictureBorder = value;
					OnPropertyChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Resets the PictureBorder property.
		/// </summary>
		public void ResetPictureBorder()
		{
			PictureBorder = 3;
		}

		/// <summary>
		/// Fires the Click event.
		/// </summary>
		public void PerformClick()
		{
			OnClick(EventArgs.Empty);
		}

		/// <summary>
		/// Gets and sets the associated ButtonWithStyle instance.
		/// </summary>
        internal ButtonWithStyle Control
		{
			get { return _control; }
			set { _control = value; }
		}

		/// <summary>
		/// Raises the Click event.
		/// </summary>
		/// <param name="e">An EventArgs containing event data.</param>
		protected virtual void OnClick(EventArgs e)
		{
			if (Click != null)
				Click(this, e);
		}

		/// <summary>
		/// Raises the PropertyChanged event.
		/// </summary>
		/// <param name="e">An EventArgs containing event data.</param>
		protected virtual void OnPropertyChanged(EventArgs e)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, e);
		}
	}
}
