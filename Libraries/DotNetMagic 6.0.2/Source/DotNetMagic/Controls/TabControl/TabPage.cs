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
using System.Text;
using System.Data;
using System.Drawing;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Represents a single tab page.
	/// </summary>
    [ToolboxItem(false)]
    [DefaultProperty("Title")]
    [DefaultEvent("PropertyChanged")]
    [Docking(DockingBehavior.Never)]
    [Designer(typeof(Crownwood.DotNetMagic.Controls.TabPageDesigner))]
    public class TabPage : Panel
    {
		/// <summary>
		/// Specifies which property has changed.
		/// </summary>
		public enum Property
        {
			/// <summary>
			/// Specifies that title property has changed.
			/// </summary>
            Title,

			/// <summary>
			/// Specifies that control property has changed.
			/// </summary>
            Control,

			/// <summary>
			/// Specifies that imsge index property has changed.
			/// </summary>
            ImageIndex,

			/// <summary>
			/// Specifies that image list property has changed.
			/// </summary>
            ImageList,

			/// <summary>
			/// Specified that the image property has changed.
			/// </summary>
			Image,

			/// <summary>
			/// Specifies that icon property has changed.
			/// </summary>
            Icon,
			
			/// <summary>
			/// Specifies that selected property has changed.
			/// </summary>
            Selected,

			/// <summary>
			/// Specifies that tooltip property has changed.
			/// </summary>
			ToolTip,

			/// <summary>
			/// Specifies that selected background tab colour.
			/// </summary>
			SelectBackColor,

			/// <summary>
			/// Specifies that selected text drawing color has changed.
			/// </summary>
			SelectTextColor,

			/// <summary>
			/// Specifies that selected text background drawing color has changed.
			/// </summary>
			SelectTextBackColor,

			/// <summary>
			/// Specifies that inactive background tab colour.
			/// </summary>
			InactiveBackColor,

			/// <summary>
			/// Specifies that inactive text drawing color has changed.
			/// </summary>
			InactiveTextColor,

			/// <summary>
			/// Specifies that inactive text background drawing color has changed.
			/// </summary>
			InactiveTextBackColor,

            /// <summary>
            /// Specifies a unique name assigned by the developer
            /// </summary>
            UniqueName
		}
        
		/// <summary>
		/// Represents the method that will handle the PropertyChanged events.
		/// </summary>
        public delegate void PropChangeHandler(TabPage page, Property prop, object oldValue);

        /// <summary>
        /// Occurs just after a property has been changed.
        /// </summary>
        public event PropChangeHandler PropertyChanged;

        // Instance fields
        private string _title;
        private string _uniqueName;
        private Control _control;
        private int _imageIndex;
        private ImageList _imageList;
        private Image _image;
        private Icon _icon;
        private bool _selected;
		private bool _shown;
		private Control _startFocus;
		private string _toolTip;
		private Color _selectBackColor;
		private Color _selectTextBackColor;
		private Color _selectTextColor;
		private Color _inactiveBackColor;
		private Color _inactiveTextBackColor;
		private Color _inactiveTextColor;

		/// <summary>
		/// Initializes a new instance of the TabPage class.
		/// </summary>
        public TabPage()
        {
            InternalConstruct("Page", null, null, -1, null, null);
        }

		/// <summary>
		/// Initializes a new instance of the TabPage class.
		/// </summary>
		/// <param name="title">Title for the page.</param>
		public TabPage(string title)
        {
            InternalConstruct(title, null, null, -1, null, null);
        }

		/// <summary>
		/// Initializes a new instance of the TabPage class.
		/// </summary>
		/// <param name="title">Title for the page.</param>
		/// <param name="control">Child control for display.</param>
        public TabPage(string title, Control control)
        {
            InternalConstruct(title, control, null, -1, null, null);
        }
			
		/// <summary>
		/// Initializes a new instance of the TabPage class.
		/// </summary>
		/// <param name="title">Title for the page.</param>
		/// <param name="control">Child control for display.</param>
		/// <param name="imageIndex">Index to use for drawing image.</param>
        public TabPage(string title, Control control, int imageIndex)
        {
            InternalConstruct(title, control, null, imageIndex, null, null);
        }

		/// <summary>
		/// Initializes a new instance of the TabPage class.
		/// </summary>
		/// <param name="title">Title for the page.</param>
		/// <param name="control">Child control for display.</param>
		/// <param name="imageList">Images the index relates to.</param>
		/// <param name="imageIndex">Index to use for drawing image.</param>
        public TabPage(string title, Control control, ImageList imageList, int imageIndex)
        {
            InternalConstruct(title, control, imageList, imageIndex, null, null);
        }

		/// <summary>
		/// Initializes a new instance of the TabPage class.
		/// </summary>
		/// <param name="title">Title for the page.</param>
		/// <param name="control">Child control for display.</param>
		/// <param name="image">Image to display with page.</param>
		public TabPage(string title, Control control, Image image)
		{
			InternalConstruct(title, control, null, -1, image, null);
		}

		/// <summary>
		/// Initializes a new instance of the TabPage class.
		/// </summary>
		/// <param name="title">Title for the page.</param>
		/// <param name="control">Child control for display.</param>
		/// <param name="icon">Icon to display with page.</param>
        public TabPage(string title, Control control, Icon icon)
        {
            InternalConstruct(title, control, null, -1, null, icon);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // If we have been assigned a control for display
                if (_control != null)
                {
                    // Must dispose of referenced control
                    _control.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        private void InternalConstruct(string title, 
                                       Control control, 
                                       ImageList imageList, 
                                       int imageIndex,
                                       Image image,
                                       Icon icon)
        {
            // Assign parameters to internal fields
            _title = title;
            _control = control;
            _imageIndex = imageIndex;
            _imageList = imageList;
            _image = image;
            _icon = icon;
			_toolTip = title;

            // Appropriate defaults
            _uniqueName = string.Empty;
            _selected = false;
			_startFocus = null;
			_selectBackColor = Color.Empty;
			_selectTextBackColor = Color.Empty;
			_selectTextColor = Color.Empty;
			_inactiveBackColor = Color.Empty;
			_inactiveTextBackColor = Color.Empty;
			_inactiveTextColor = Color.Empty;
			
			// Default to not being visible
			base.Visible = false;
		}
		
		/// <summary>
		/// Gets or sets a value indicating whether the control is displayed.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool Visible
		{
			get 
			{ 
				return base.Visible; 
			}
			
			set 
			{ 
				base.Visible = value; 
			}
		}
		
		private bool ShouldSerializeVisible()
		{
			return false;
		}

		/// <summary>
		/// Resets the value of the Visible property.
		/// </summary>
		public void ResetVisible()
		{
			base.Visible = false;
		}
				
		/// <summary>
		/// Gets and sets the title used for the page.
		/// </summary>
        [DefaultValue("Page")]
        [Localizable(true)]
        public string Title
        {
            get { return _title; }
		   
            set 
            { 
                if (_title != value)
                {
                    string oldValue = _title;
                    _title = value; 

                    OnPropertyChanged(Property.Title, oldValue);
                }
            }
        }

		/// <summary>
		/// Gets and sets the child control for the page.
		/// </summary>
        [DefaultValue(null)]
        public Control Control
        {
            get { return _control; }

            set 
            { 
                if (_control != value)
                {
                    Control oldValue = _control;
                    _control = value; 

                    OnPropertyChanged(Property.Control, oldValue);
                }
            }
        }

        /// <summary>
        /// Gets and sets the unique name of the page.
        /// </summary>
        [DefaultValue("")]
        public string UniqueName
        {
            get { return _uniqueName; }

            set
            {
                if (_uniqueName != value)
                {
                    string oldValue = _uniqueName;
                    _uniqueName = value;

                    OnPropertyChanged(Property.UniqueName, oldValue);
                }
            }
        }

		/// <summary>
		/// Gets and sets the image index of the page.
		/// </summary>
        [DefaultValue(-1)]
        public int ImageIndex
        {
            get { return _imageIndex; }
		
            set 
            { 
                if (_imageIndex != value)
                {
                    int oldValue = _imageIndex;
                    _imageIndex = value; 

                    OnPropertyChanged(Property.ImageIndex, oldValue);
                }
            }
        }

		/// <summary>
		/// Gets and sets the image list of the page.
		/// </summary>
        [DefaultValue(null)]
        public ImageList ImageList
        {
            get { return _imageList; }
		
            set 
            { 
                if (_imageList != value)
                {
                    ImageList oldValue = _imageList;
                    _imageList = value; 

                    OnPropertyChanged(Property.ImageList, oldValue);
                }
            }
        }

		/// <summary>
		/// Gets and sets the image of the page.
		/// </summary>
		[DefaultValue(null)]
		public Image Image
		{
			get { return _image; }
		
			set 
			{ 
				if (_image != value)
				{
					Image oldValue = _image;
					_image = value; 

					OnPropertyChanged(Property.Image, oldValue);
				}
			}
		}

		/// <summary>
		/// Gets and sets the icon of the page.
		/// </summary>
        [DefaultValue(null)]
        public Icon Icon
        {
            get { return _icon; }
		
            set 
            { 
                if (_icon != value)
                {
                    Icon oldValue = _icon;
                    _icon = value; 

                    OnPropertyChanged(Property.Icon, oldValue);
                }
            }
        }

		/// <summary>
		/// Gets and sets a value indicating if the page is selected.
		/// </summary>
        [DefaultValue(true)]
        public bool Selected
        {
            get { return _selected; }

            set
            {
                if (_selected != value)
                {
                    bool oldValue = _selected;
                    _selected = value;

                    OnPropertyChanged(Property.Selected, oldValue);
                }
            }
        }

		/// <summary>
		/// Gets and sets the tooltip used for the page.
		/// </summary>
		[Localizable(true)]
		public string ToolTip
		{
			get { return _toolTip; }
		   
			set 
			{ 
				if (_toolTip != value)
				{
					string oldValue = _toolTip;
					_toolTip = value; 

					OnPropertyChanged(Property.ToolTip, oldValue);
				}
			}
		}

		/// <summary>
		/// Gets and sets the page background color for selected page.
		/// </summary>
		public Color SelectBackColor
		{
			get { return _selectBackColor; }
		   
			set 
			{ 
				if (_selectBackColor != value)
				{
					Color oldValue = _selectBackColor;
					_selectBackColor = value; 

					OnPropertyChanged(Property.SelectBackColor, oldValue);
				}
			}
		}

		/// <summary>
		/// Gets and sets the text background color for selected page.
		/// </summary>
		public Color SelectTextBackColor
		{
			get { return _selectTextBackColor; }
		   
			set 
			{ 
				if (_selectTextBackColor != value)
				{
					Color oldValue = _selectTextBackColor;
					_selectTextBackColor = value; 

					OnPropertyChanged(Property.SelectTextBackColor, oldValue);
				}
			}
		}

		/// <summary>
		/// Gets and sets the text color for selected page.
		/// </summary>
		public Color SelectTextColor
		{
			get { return _selectTextColor; }
		   
			set 
			{ 
				if (_selectTextColor != value)
				{
					Color oldValue = _selectTextColor;
					_selectTextColor = value; 

					OnPropertyChanged(Property.SelectTextColor, oldValue);
				}
			}
		}

		/// <summary>
		/// Gets and sets the page background color for inactive page.
		/// </summary>
		public Color InactiveBackColor
		{
			get { return _inactiveBackColor; }
		   
			set 
			{ 
				if (_inactiveBackColor != value)
				{
					Color oldValue = _inactiveBackColor;
					_inactiveBackColor = value; 

					OnPropertyChanged(Property.InactiveBackColor, oldValue);
				}
			}
		}

		/// <summary>
		/// Gets and sets the text background color for inactive page.
		/// </summary>
		public Color InactiveTextBackColor
		{
			get { return _inactiveTextBackColor; }
		   
			set 
			{ 
				if (_inactiveTextBackColor != value)
				{
					Color oldValue = _inactiveTextBackColor;
					_inactiveTextBackColor = value; 

					OnPropertyChanged(Property.InactiveTextBackColor, oldValue);
				}
			}
		}

		/// <summary>
		/// Gets and sets the text color for inactive page.
		/// </summary>
		public Color InactiveTextColor
		{
			get { return _inactiveTextColor; }
		   
			set 
			{ 
				if (_inactiveTextColor != value)
				{
					Color oldValue = _inactiveTextColor;
					_inactiveTextColor = value; 

					OnPropertyChanged(Property.InactiveTextColor, oldValue);
				}
			}
		}

		/// <summary>
		/// Gets and sets the control that should receive focus when page becomes selected.
		/// </summary>
        [DefaultValue(null)]
        public Control StartFocus
        {
            get { return _startFocus; }
            set { _startFocus = value; }
        }

		/// <summary>
		/// Return a string representation of page.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return _title;
		}

		/// <summary>
		/// Raises the PropertyChanged event.
		/// </summary>
		/// <param name="prop">Property that has changed.</param>
		/// <param name="oldValue">Property value before it changed.</param>
        protected virtual void OnPropertyChanged(Property prop, object oldValue)
        {
            // Any attached event handlers?
            if (PropertyChanged != null)
                PropertyChanged(this, prop, oldValue);
        }
        
        internal bool Shown
        {
            get { return _shown; }
            set { _shown = value; }
        }
    }
}
