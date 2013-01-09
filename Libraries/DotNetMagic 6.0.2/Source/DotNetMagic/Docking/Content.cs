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
using System.IO;
using System.Xml;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using Crownwood.DotNetMagic.Common;
using Crownwood.DotNetMagic.Controls;

namespace Crownwood.DotNetMagic.Docking
{
	/// <summary>
	/// Individual docking unit.
	/// </summary>
    public class Content
    {
		/// <summary>
		/// Specifies the property that has changed.
		/// </summary>
		public enum Property
        {
			/// <summary>
			/// Specifies the control property has changed.
			/// </summary>
            Control,

			/// <summary>
			/// Specifies the title property has changed.
			/// </summary>
            Title,

			/// <summary>
			/// Specifies the full title property has changed.
			/// </summary>
            FullTitle,

			/// <summary>
			/// Specifies the image list property has changed.
			/// </summary>
            ImageList,

			/// <summary>
			/// Specifies the image index property has changed.
			/// </summary>
            ImageIndex,
			
			/// <summary>
			/// Specifies the icon property has changed.
			/// </summary>
			Icon,

			/// <summary>
			/// Specifies the caption bar property has changed.
			/// </summary>
			CaptionBar,

			/// <summary>
			/// Specifies the close button property has changed.
			/// </summary>
            CloseButton,

			/// <summary>
			/// Specifies the help button property has changed.
			/// </summary>
            HideButton,

			/// <summary>
			/// Specifies the display size property has changed.
			/// </summary>
            DisplaySize,

			/// <summary>
			/// Specifies the auto hide size property has changed.
			/// </summary>
            AutoHideSize,

			/// <summary>
			/// Specifies the floating size property has changed.
			/// </summary>
            FloatingSize,

			/// <summary>
			/// Specifies the display location property has changed.
			/// </summary>
            DisplayLocation,
            
            /// <summary>
            /// Specifies the Office2003 automatic background color has changed.
            /// </summary>
            Office2003BackColor,

            /// <summary>
            /// Specifies the Office2007 automatic background color has changed.
            /// </summary>
            Office2007BackColor,

            /// <summary>
            /// Specifies the MediaPlayer automatic background color has changed.
            /// </summary>
            MediaPlayerBackColor,

            /// <summary>
            /// Specifies the unique name of the content has changed.
            /// </summary>
            UniqueName
        }

		/// <summary>
		/// Represents the method that will handle a property change.
		/// </summary>
		public delegate void PropChangeHandler(Content obj, Property prop);

        // Class constant
        private static int _defaultDisplaySize = 150;
        private static int _defaultAutoHideSize = 150;
        private static int _defaultFloatingSize = 150;
        private static int _defaultLocation = 150;
		private static int _counter = 0;

        // Instance fields
        private Control _control;
        private string _title;
        private string _fullTitle;
        private string _uniqueName;
		private Icon _icon;
		private ImageList _imageList;
        private int _imageIndex;
		private int _order;
		private Size _displaySize;
        private Size _autoHideSize;
        private Size _floatingSize;
		private Point _displayLocation;
        private DockingManager _manager;
        private bool _docked;
        private bool _autoHidden;
        private bool _visible;
        private bool _captionBar;
        private bool _closeButton;
        private bool _hideButton;
		private bool _closeOnHide;
		private object _tag;
        private AutoHidePanel _autoHidePanel;
        private WindowContent _parentWindowContent;
		private Restore _defaultRestore;
		private Restore _autoHideRestore;
		private Restore _dockingRestore;
		private Restore _floatingRestore;
		private OfficeColor _office2003BackColor;
        private OfficeColor _office2007BackColor;
        private MediaPlayerColor _mediaPlayerBackColor;

        /// <summary>
        /// Occurs just before a property changes value.
        /// </summary>
        public event PropChangeHandler PropertyChanging;

		/// <summary>
		/// Occurs just after a property changes value.
		/// </summary>
        public event PropChangeHandler PropertyChanged;

		/// <summary>
		/// Initializes a new instance of the Content class.
		/// </summary>
		/// <param name="xmlIn">Steam containing information to be loaded.</param>
		/// <param name="formatVersion">Version number.</param>
        public Content(XmlTextReader xmlIn, int formatVersion)
        {
            // Define the initial object state
            _control = null;
            _title = "";
            _fullTitle = "";
            _uniqueName = "";
            _imageList = null;
			_icon = null;
            _imageIndex = -1;
            _manager = null;
            _parentWindowContent = null;
            _displaySize = new Size(_defaultDisplaySize, _defaultDisplaySize);
            _autoHideSize = new Size(_defaultAutoHideSize, _defaultAutoHideSize);
            _floatingSize = new Size(_defaultFloatingSize, _defaultFloatingSize);
            _displayLocation = new Point(_defaultLocation, _defaultLocation);
			_order = _counter++;
			_tag = null;
			_visible = false;
			_defaultRestore = null;
			_autoHideRestore = null;
			_floatingRestore = null;
			_dockingRestore = null;
			_autoHidePanel = null;
			_docked = true;
			_captionBar = true;
			_closeButton = true;
            _hideButton = true;
            _autoHidden = false;
			_closeOnHide = false;
			_office2003BackColor = OfficeColor.Disable;
            _office2007BackColor = OfficeColor.Disable;
            _mediaPlayerBackColor = MediaPlayerColor.Disable;

			// Overwrite default with values read in
			LoadFromXml(xmlIn, formatVersion, true);
        }

		/// <summary>
		/// Initializes a new instance of the Content class.
		/// </summary>
		/// <param name="manager">Reference to parent docking manager.</param>
        public Content(DockingManager manager)
        {
            InternalConstruct(manager, null, "", null, -1, null);
        }

		/// <summary>
		/// Initializes a new instance of the Content class.
		/// </summary>
		/// <param name="manager">Reference to parent docking manager.</param>
		/// <param name="control">Control to use inside content.</param>
        public Content(DockingManager manager, Control control)
        {
            InternalConstruct(manager, control, "", null, -1, null);
        }

		/// <summary>
		/// Initializes a new instance of the Content class.
		/// </summary>
		/// <param name="manager">Reference to parent docking manager.</param>
		/// <param name="control">Control to use inside content.</param>
		/// <param name="title">Title to use inside content.</param>
        public Content(DockingManager manager, Control control, string title)
        {
            InternalConstruct(manager, control, title, null, -1, null);
        }

		/// <summary>
		/// Initializes a new instance of the Content class.
		/// </summary>
		/// <param name="manager">Reference to parent docking manager.</param>
		/// <param name="control">Control to use inside content.</param>
		/// <param name="title">Title to use inside content.</param>
		/// <param name="imageList">ImageList to use inside content.</param>
		/// <param name="imageIndex">ImageIndex to use inside content.</param>
        public Content(DockingManager manager, Control control, string title, ImageList imageList, int imageIndex)
        {
            InternalConstruct(manager, control, title, imageList, imageIndex, null);
        }

		/// <summary>
		/// Initializes a new instance of the Content class.
		/// </summary>
		/// <param name="manager">Reference to parent docking manager.</param>
		/// <param name="control">Control to use inside content.</param>
		/// <param name="title">Title to use inside content.</param>
		/// <param name="icon">Icon to use inside content.</param>
		public Content(DockingManager manager, Control control, string title, Icon icon)
		{
			InternalConstruct(manager, control, title, null, -1, icon);
		}
		
		private void InternalConstruct(DockingManager manager, 
                                       Control control, 
                                       string title, 
                                       ImageList imageList, 
                                       int imageIndex,
									   Icon icon)
        {
            // Must provide a valid manager instance
            if (manager == null)
                throw new ArgumentNullException("DockingManager");

            // Define the initial object state
            _control = control;
            _title = title;
            _uniqueName = "";
            _imageList = imageList;
            _imageIndex = imageIndex;
			_icon = icon;
            _manager = manager;
            _parentWindowContent = null;
            _order = _counter++;
            _visible = false;
            _displaySize = new Size(_defaultDisplaySize, _defaultDisplaySize);
            _autoHideSize = new Size(_defaultAutoHideSize, _defaultAutoHideSize);
            _floatingSize = new Size(_defaultFloatingSize, _defaultFloatingSize);
            _displayLocation = new Point(_defaultLocation, _defaultLocation);
			_defaultRestore = new RestoreContentState(State.DockLeft, this);
			_floatingRestore = new RestoreContentState(State.Floating, this);
            _autoHideRestore = new RestoreAutoHideState(State.DockLeft, this);
            _dockingRestore = _defaultRestore;
            _autoHidePanel = null;
			_tag = null;
			_docked = true;
            _captionBar = true;
            _closeButton = true;
            _hideButton = true;
            _autoHidden = false;
			_closeOnHide = false;
            _fullTitle = title;

			// A control that is a Panel of Form should have its back color auto set            
            if ((_control != null) && ((_control is Form) || (_control is Panel)))
            {
                _office2003BackColor = OfficeColor.Base;
                _office2007BackColor = OfficeColor.Base;
                _mediaPlayerBackColor = MediaPlayerColor.Base;
            }
            else
            {
                _office2003BackColor = OfficeColor.Disable;
                _office2007BackColor = OfficeColor.Disable;
                _mediaPlayerBackColor = MediaPlayerColor.Disable;
            }
        }

		/// <summary>
		/// Gets and set access to the parent docking manager instance.
		/// </summary>
        public DockingManager DockingManager
        {
            get { return _manager; }
			set { _manager = value; }
        }

		/// <summary>
		/// Gets and sets the control to use for display.
		/// </summary>
        public Control Control
        {
            get { return _control; }
			
            set 
            {
                if (_control != value)
                {
                    OnPropertyChanging(Property.Control);
                    _control = value;

					// A control that is a Panel of Form should have its back color auto set            
                    if ((_control != null) && ((_control is Form) || (_control is Panel)))
                    {
                        Office2003BackColor = OfficeColor.Base;
                        Office2007BackColor = OfficeColor.Base;
                        MediaPlayerBackColor = MediaPlayerColor.Base;
                    }
                    else
                    {
                        Office2003BackColor = OfficeColor.Disable;
                        Office2007BackColor = OfficeColor.Disable;
                        MediaPlayerBackColor = MediaPlayerColor.Disable;
                    }
					
					OnPropertyChanged(Property.Control);
                }
            }
        }

		/// <summary>
		/// Gets and sets the short title text.
		/// </summary>
        public string Title
        {
            get { return _title; }

            set 
            {
                if (_title != value)
                {
                    OnPropertyChanging(Property.Title);
                    _title = value;
                    OnPropertyChanged(Property.Title);
                }
            }
        }
        
		/// <summary>
		/// Gets and sets the long title text.
		/// </summary>
        public string FullTitle
        {
            get { return _fullTitle; }
            
            set
            {
                if (_fullTitle != value)
                {
                    OnPropertyChanging(Property.FullTitle);
                    _fullTitle = value;
                    OnPropertyChanged(Property.FullTitle);
                }
            }
        }

		/// <summary>
		/// Gets and sets the unique name.
		/// </summary>
		public string UniqueName
		{
			get { return _uniqueName; }
            
			set
			{
				if (_uniqueName != value)
				{
					OnPropertyChanging(Property.UniqueName);
					_uniqueName = value;
					OnPropertyChanged(Property.UniqueName);
				}
			}
		}

		/// <summary>
		/// Gets and sets the image list to use as a source for images.
		/// </summary>
        public ImageList ImageList
        {
            get { return _imageList; }

            set 
            {
                if(_imageList != value) 
                {
                    OnPropertyChanging(Property.ImageList);
                    _imageList = value; 
                    OnPropertyChanged(Property.ImageList);
                }
            }
        }

		/// <summary>
		/// Gets and sets the index into the associated image list.
		/// </summary>
        public int ImageIndex
        {
            get { return _imageIndex; }

            set 
            {
                if (_imageIndex != value)
                {
                    OnPropertyChanging(Property.ImageIndex);
                    _imageIndex = value;
                    OnPropertyChanged(Property.ImageIndex);
                }
            }
        }

		/// <summary>
		/// Gets and sets the associated icon.
		/// </summary>
		public Icon Icon
		{
			get { return _icon; }

			set 
			{
				if (_icon != value)
				{
					OnPropertyChanging(Property.Icon);
					_icon = value;
					OnPropertyChanged(Property.Icon);
				}
			}
		}
		
		/// <summary>
		/// Gets and sets a value indicating if a caption bar is required.
		/// </summary>
		public bool CaptionBar
        {
            get { return _captionBar; }
            
            set
            {
                if (_captionBar != value)
                {
                    OnPropertyChanging(Property.CaptionBar);
                    _captionBar = value;
                    OnPropertyChanged(Property.CaptionBar);
                }
            }
        }

		/// <summary>
		/// Gets and sets a value indicating if a close button is required.
		/// </summary>
        public bool CloseButton
        {
            get { return _closeButton; }
            
            set
            {
                if (_closeButton != value)
                {
                    OnPropertyChanging(Property.CloseButton);
                    _closeButton = value;
                    OnPropertyChanged(Property.CloseButton);
                }
            }
        }

		/// <summary>
		/// Gets and sets if the hide button should be displayed.
		/// </summary>
        public bool HideButton
        {
            get { return _hideButton; }
            
            set
            {
                if (_hideButton != value)
                {
                    OnPropertyChanging(Property.HideButton);
                    _hideButton = value;
                    OnPropertyChanged(Property.HideButton);
                }
            }
        }

		/// <summary>
		/// Gets and sets the default docked display size.
		/// </summary>
        public Size DisplaySize
        {
            get { return _displaySize; }

            set 
            {
                if (_displaySize != value)
                {
                    OnPropertyChanging(Property.DisplaySize);
                    _displaySize = value;
                    OnPropertyChanged(Property.DisplaySize);
                    if (DockingManager != null)
                        DockingManager.OnLayoutChanged(EventArgs.Empty);
                }
            }
        }
        
		/// <summary>
		/// Gets and sets the default auto hidden display size.
		/// </summary>
        public Size AutoHideSize
        {
            get { return _autoHideSize; }
            
            set
            {
                if (_autoHideSize != value)
                {
                    OnPropertyChanging(Property.AutoHideSize);
                    _autoHideSize = value;
                    OnPropertyChanged(Property.AutoHideSize);
                    if (DockingManager != null)
                        DockingManager.OnLayoutChanged(EventArgs.Empty);
                }
            }
        }

		/// <summary>
		/// Gets and sets the default floating display size.
		/// </summary>
        public Size FloatingSize
        {
            get { return _floatingSize; }

            set 
            {
                if (_floatingSize != value)
                {
                    OnPropertyChanging(Property.FloatingSize);
                    _floatingSize = value;
                    OnPropertyChanged(Property.FloatingSize);
                    if (DockingManager != null)
                        DockingManager.OnLayoutChanged(EventArgs.Empty);
                }
            }
        }

		/// <summary>
		/// Gets and sets the default floating window location.
		/// </summary>
        public Point DisplayLocation
        {
            get { return _displayLocation; }

            set 
            {
                if (_displayLocation != value)
                {
                    OnPropertyChanging(Property.DisplayLocation);
                    _displayLocation = value;
                    OnPropertyChanged(Property.DisplayLocation);
                    if (DockingManager != null)
                        DockingManager.OnLayoutChanged(EventArgs.Empty);
                }
            }
        }
        
		/// <summary>
		/// Gets the unique value giving the creation order of content instances.
		/// </summary>
		public int Order
		{
			get { return _order; }
		}

		/// <summary>
		/// Gets and sets a value indicating if the content should be removed when closed.
		/// </summary>
		public bool CloseOnHide
		{
			get { return _closeOnHide; }
			set { _closeOnHide = value; }
		}

		/// <summary>
		/// Gets and sets the automatic Office2003 background color action.
		/// </summary>
		public OfficeColor Office2003BackColor
		{
			get { return _office2003BackColor; }
			
			set 
			{ 
				if (_office2003BackColor != value)
				{
					OnPropertyChanging(Property.Office2003BackColor);
					_office2003BackColor = value; 
					OnPropertyChanged(Property.Office2003BackColor);
				}
			}
		}

        /// <summary>
        /// Gets and sets the automatic Office2007 background color action.
        /// </summary>
        public OfficeColor Office2007BackColor
        {
            get { return _office2007BackColor; }

            set
            {
                if (_office2007BackColor != value)
                {
                    OnPropertyChanging(Property.Office2007BackColor);
                    _office2007BackColor = value;
                    OnPropertyChanged(Property.Office2007BackColor);
                }
            }
        }

        /// <summary>
        /// Gets and sets the automatic MediaPlayer background color action.
        /// </summary>
        public MediaPlayerColor MediaPlayerBackColor
        {
            get { return _mediaPlayerBackColor; }

            set
            {
                if (_mediaPlayerBackColor != value)
                {
                    OnPropertyChanging(Property.MediaPlayerBackColor);
                    _mediaPlayerBackColor = value;
                    OnPropertyChanged(Property.MediaPlayerBackColor);
                }
            }
        }

        /// <summary>
		/// Gets and sets user defined data.
		/// </summary>
		public object Tag
		{
			get { return _tag; }
			set { _tag = value; }
		}

		/// <summary>
		/// Gets a value indicating is the content is currently hidden.
		/// </summary>
		public bool Visible
		{
			get { return _visible; }
		}

		/// <summary>
		/// Gets and sets an object that represents the default docking location. 
		/// </summary>
		public Restore DefaultRestore
		{
			get { return _defaultRestore; }
			set { _defaultRestore = value; }
		}

		/// <summary>
		/// Gets and sets an object that represents the saved auto hidden location. 
		/// </summary>
        public Restore AutoHideRestore
        {
            get { return _autoHideRestore; }
            set { _autoHideRestore = value; }
        }
        
		/// <summary>
		/// Gets and sets an object that represents the saved docked location.
		/// </summary>
        public Restore DockingRestore
		{
			get { return _dockingRestore; }
			set { _dockingRestore = value; }
		}

		/// <summary>
		/// Gets and sets an object that represents the saved floating location.
		/// </summary>
		public Restore FloatingRestore
		{
			get { return _floatingRestore; }
			set { _floatingRestore = value; }
		}

		/// <summary>
		/// Gets and sets the parent window that contains this content.
		/// </summary>
        public WindowContent ParentWindowContent
        {
            get { return _parentWindowContent; }
            
			set 
			{ 
				if (_parentWindowContent != value)
				{
					_parentWindowContent = value; 

                    // Recalculate the visibility value
                    UpdateVisibility();
				}
			}
        }

		/// <summary>
		/// Bring this content to the front of any tabbed content window.
		/// </summary>
		public void BringToFront()
		{
		    if (!_visible)
		    {
			    // Use docking manager to ensure we are Visible
			    _manager.ShowContent(this);
            }
            
            if (_autoHidden)
            {
                // Request docking manager bring to window into view
                _manager.BringAutoHideIntoView(this);
            }
            else
            {
			    // Ask the parent WindowContent to ensure we are the active Content
			    _parentWindowContent.BringContentToFront(this);
	        }
		}

		/// <summary>
		/// Record the current position of the content.
		/// </summary>
		/// <returns>Object representing position.</returns>
		public Restore RecordRestore()
		{
			if (_parentWindowContent != null)
			{
			    if (_autoHidden)
                    return RecordAutoHideRestore();
                else
                {		
					if (_parentWindowContent.ParentZone != null)
					{
						Form parentForm = _parentWindowContent.ParentZone.FindForm();

						// Cannot record restore information if not in a Form
						if (parentForm != null)
						{
							// Decide which restore actually needs recording
							if (parentForm is FloatingForm)
								return RecordFloatingRestore();
							else
								return RecordDockingRestore();
						}	
					}
		        }
			}

			return null;
		}

		/// <summary>
		/// Record the current auto hidden position of the content.
		/// </summary>
		/// <returns>Object representing position.</returns>
        public Restore RecordAutoHideRestore()
        {
            // Remove any existing restore object
            _autoHideRestore = null;
                
            // We should be inside a parent window
            if (_parentWindowContent != null)
            {
                // And in the auto hidden state
                if (_autoHidden)
                {
                    // Get access to the AutoHostPanel that contains use
                    AutoHidePanel ahp = _parentWindowContent.DockingManager.AutoHidePanelForContent(this);
                    
                    // Request the ahp create a relevant restore object for us
                    _autoHideRestore = ahp.RestoreObjectForContent(this);
                }
            }
        
            return _autoHideRestore;
        }

		/// <summary>
		/// Record the current edge docked position of the content.
		/// </summary>
		/// <returns>Object representing position.</returns>
		public Restore RecordDockingRestore()
		{
			// Remove any existing Restore object
			_dockingRestore = null;

			// Do we have a parent window we are inside?
			if (_parentWindowContent != null)
			{
				// Ask the parent to provide a Restore object for us
				_dockingRestore = _parentWindowContent.RecordRestore(this);
			}

			// If we cannot get a valid Restore object from the parent then we have no choice 
			// but to use the default restore which is less accurate but better than nothing
			if (_dockingRestore == null)
				_dockingRestore = _defaultRestore;

			return _dockingRestore;
		}
		
		/// <summary>
		/// Record the current floating position of the content.
		/// </summary>
		/// <returns>Object representing position.</returns>
		public Restore RecordFloatingRestore()
		{
			// Remove any existing Restore object
			_floatingRestore = null;

			// Do we have a parent window we are inside?
			if (_parentWindowContent != null)
			{
				// Ask the parent to provide a Restore object for us
				_floatingRestore = _parentWindowContent.RecordRestore(this);
			}

			// If we cannot get a valid Restore object from the parent then we have no choice 
			// but to use the default restore which is less accurate but better than nothing
			if (_floatingRestore == null)
				_floatingRestore = _defaultRestore;

			return _floatingRestore;
		}

		/// <summary>
		/// Gets a value indicating if the Content is docked against an edge.
		/// </summary>
		public bool IsDocked
		{
			get { return _docked; }
		}

		/// <summary>
		/// Gets a value indicating if the Content is in AutoHide.
		/// </summary>
		public bool IsAutoHidden
		{
			get { return _autoHidden; }
		}

		/// <summary>
		/// Saves content information into an array of bytes using Encoding.Unicode.
		/// </summary>
		/// <returns>Array of bytes.</returns>
		public byte[] SaveContentToArray()
		{
			return SaveContentToArray(Encoding.Unicode);	
		}

		/// <summary>
		/// Saves content information into an array of bytes using caller provided encoding object.
		/// </summary>
		/// <param name="encoding">Encoding object.</param>
		/// <returns>Array of bytes.</returns>
		public byte[] SaveContentToArray(Encoding encoding)
		{
			// Create a memory based stream
			MemoryStream ms = new MemoryStream();
			
			// Save into the file stream
			SaveContentToStream(ms, encoding);

			// Must remember to close
			ms.Close();

			// Return an array of bytes that contain the streamed XML
			return ms.GetBuffer();
		}

		/// <summary>
		/// Saves content information into a named file using Encoding.Unicode.
		/// </summary>
		/// <param name="filename">Filename to create.</param>
		public void SaveContentToFile(string filename)
		{
			SaveContentToFile(filename, Encoding.Unicode);
		}

		/// <summary>
		/// Saves content information into a named file using caller provided encoding object.
		/// </summary>
		/// <param name="filename">Filename to create.</param>
		/// <param name="encoding">Encoding object.</param>
		public void SaveContentToFile(string filename, Encoding encoding)
		{
			// Create/Overwrite existing file
			FileStream fs = new FileStream(filename, FileMode.Create);
			
			try
			{
				// Save into the file stream
				SaveContentToStream(fs, encoding);		
			}
			catch
			{
				// Must remember to close
				fs.Close();
			}
		}

		/// <summary>
		/// Saves content information into a stream object using caller provided encoding object.
		/// </summary>
		/// <param name="stream">Destination stream for saving.</param>
		/// <param name="encoding">Encoding object.</param>
		public void SaveContentToStream(Stream stream, Encoding encoding)
		{
			XmlTextWriter xmlOut = new XmlTextWriter(stream, encoding); 

			// Use indenting for readability
			xmlOut.Formatting = Formatting.Indented;
			
			// Always begin file with identification and warning
			xmlOut.WriteStartDocument();
			xmlOut.WriteComment(" DotNetMagic, The User Interface library for .NET (www.crownwood.net) ");
			xmlOut.WriteComment(" Modifying this generated file will probably render it invalid ");

			// Use existing method to perform actual output to Xml
			SaveContentToXml(xmlOut);

			// Terminate the document        
			xmlOut.WriteEndDocument();

			// This should flush all actions and close the file
			xmlOut.Close();			
		}

		/// <summary>
		/// Saves content information using provided xml writer object.
		/// </summary>
		/// <param name="xmlOut">Xml writer object.</param>
		public void SaveContentToXml(XmlTextWriter xmlOut)
		{
			// Associate a version number with the root element so that future version of the code
			// will be able to be backwards compatible or at least recognise out of date versions
			xmlOut.WriteStartElement("ContentConfig");
			xmlOut.WriteAttributeString("FormatVersion", "8");

			// Save the content information
			SaveToXml(xmlOut);

			// Terminate the root element      
			xmlOut.WriteEndElement();
		}
		
		/// <summary>
		/// Loads content information from given array of bytes.
		/// </summary>
		/// <param name="buffer">Array of byes.</param>
		public void LoadContentFromArray(byte[] buffer)
		{
			// Create a memory based stream
			MemoryStream ms = new MemoryStream(buffer);
			
			// Save into the file stream
			LoadContentFromStream(ms);

			// Must remember to close
			ms.Close();
		}

		/// <summary>
		/// Loads layout information from given filename.
		/// </summary>
		/// <param name="filename">Filename to open.</param>
		public void LoadContentFromFile(string filename)
		{
			// Open existing file
			FileStream fs = new FileStream(filename, FileMode.Open);
			
			try
			{
				// Load from the file stream
				LoadContentFromStream(fs);		
			}
			catch
			{
				// Must remember to close
				fs.Close();
			}
		}
		
		/// <summary>
		/// Loads content information from given stream object.
		/// </summary>
		/// <param name="stream">Stream to load from.</param>
		public void LoadContentFromStream(Stream stream)
		{
			XmlTextReader xmlIn = new XmlTextReader(stream); 

			// Ignore whitespace, not interested
			xmlIn.WhitespaceHandling = WhitespaceHandling.None;

			// Moves the reader to the root element.
			xmlIn.MoveToContent();

			// Use existing method to perform actual input from Xml
			LoadContentFromXml(xmlIn);

			// Remember to close the no longer needed reader.
			xmlIn.Close();			
		}
		
		/// <summary>
		/// Loads content information using provided xml reader object.
		/// </summary>
		/// <param name="xmlIn">Reader for loading config.</param>
		public void LoadContentFromXml(XmlTextReader xmlIn)
		{
			// Double check this has the correct element name
			if (xmlIn.Name != "ContentConfig")
				throw new ArgumentException("Root element must be 'ContentConfig'");

			// Load the format version number
			string version = xmlIn.GetAttribute(0);

			// Convert format version from string to double
			int formatVersion = (int)Convert.ToDouble(version);
            
			// Should only be version number 5 upwards
			if (formatVersion < 5)
				throw new ArgumentException("Cannot load version numbers before 5.");

			// Read the next Element
			if (!xmlIn.Read())
				throw new ArgumentException("An element was expected but could not be read in");

			// Load the content information into this object
			LoadFromXml(xmlIn, formatVersion, false);
		} 

		/// <summary>
		/// Raises the PropertyChanging event.
		/// </summary>
		/// <param name="prop">Property has is changing.</param>
		protected virtual void OnPropertyChanging(Property prop)
		{
			// Any attached event handlers?
			if (PropertyChanging != null)
				PropertyChanging(this, prop);
		}

		/// <summary>
		/// Raises the PropertyChangedevent.
		/// </summary>
		/// <param name="prop">Property that has changed.</param>
		protected virtual void OnPropertyChanged(Property prop)
		{
			// Any attached event handlers?
			if (PropertyChanged != null)
				PropertyChanged(this, prop);
		}

		internal AutoHidePanel AutoHidePanel
		{
			get { return _autoHidePanel; }
            
			set 
			{
				if (_autoHidePanel != value)
				{
					_autoHidePanel = value; 
                
					// Recalculate the visibility value
					UpdateVisibility();
				}
			}
		}

		internal bool AutoHidden
		{
			get { return _autoHidden; }

			set 
			{ 
				if (_autoHidden != value)
				{
					_autoHidden = value; 

					// Recalculate the visibility value
					UpdateVisibility();
				}                
			}
		}

		internal bool Docked
		{
			get { return _docked; }
			
            set 
            {
                if (_docked != value)
                {
                    _docked = value;
                    if (DockingManager != null)
                        DockingManager.OnLayoutChanged(EventArgs.Empty);
                }
            }
		}

		internal void ContentBecomesFloating()
		{
			_docked = false;

			if (_parentWindowContent != null)
			{
				switch(_parentWindowContent.State)
				{
					case State.DockLeft:
					case State.DockRight:
					case State.DockTop:
					case State.DockBottom:
						// Record the current position before content is moved
						RecordDockingRestore();
						break;
					case State.Floating:
					default:
						// Do nothing, already floating
						break;
				}
			}
		}

        internal void ContentLeavesFloating()
        {
			_docked = true;

            if (_parentWindowContent != null)
            {
                switch(_parentWindowContent.State)
                {
                    case State.DockLeft:
                    case State.DockRight:
                    case State.DockTop:
                    case State.DockBottom:
                        // Do nothing, already floating
                        break;
                    case State.Floating:
                    default:
                        // Record the current position before content is moved
                        RecordFloatingRestore();
                        break;
                }
            }
        }

		internal void ReconnectRestore()
		{
			_defaultRestore.Reconnect(_manager);
			_autoHideRestore.Reconnect(_manager);
			_dockingRestore.Reconnect(_manager);
			_floatingRestore.Reconnect(_manager);
		}

		internal void SaveToXml(XmlTextWriter xmlOut)
		{
			// Output standard values appropriate for all Content 
			xmlOut.WriteStartElement("Content");
			xmlOut.WriteAttributeString("Name", _title);
			xmlOut.WriteAttributeString("Visible", _visible.ToString());
            xmlOut.WriteAttributeString("Docked", _docked.ToString());
            xmlOut.WriteAttributeString("AutoHidden", _autoHidden.ToString());
            xmlOut.WriteAttributeString("CaptionBar", _captionBar.ToString());
            xmlOut.WriteAttributeString("CloseButton", _closeButton.ToString());
            xmlOut.WriteAttributeString("DisplaySize", ConversionHelper.SizeToString(_displaySize));
			xmlOut.WriteAttributeString("DisplayLocation", ConversionHelper.PointToString(_displayLocation));
            xmlOut.WriteAttributeString("AutoHideSize", ConversionHelper.SizeToString(_autoHideSize));
            xmlOut.WriteAttributeString("FloatingSize", ConversionHelper.SizeToString(_floatingSize));
            xmlOut.WriteAttributeString("FullTitle", _fullTitle);
            xmlOut.WriteAttributeString("Office2003BackColor", Office2003BackColor.ToString());
            xmlOut.WriteAttributeString("UniqueName", _uniqueName);
            xmlOut.WriteAttributeString("Office2007BackColor", Office2007BackColor.ToString());
            xmlOut.WriteAttributeString("MediaPlayerBackColor", MediaPlayerBackColor.ToString());

			// Save the Default Restore object to Xml
			xmlOut.WriteStartElement("DefaultRestore");
			_defaultRestore.SaveToXml(xmlOut);
			xmlOut.WriteEndElement();

            // Save the AutoHideRestore object to Xml
            xmlOut.WriteStartElement("AutoHideRestore");
            _autoHideRestore.SaveToXml(xmlOut);
            xmlOut.WriteEndElement();
            
            // Save the DockingRestore object to Xml
			xmlOut.WriteStartElement("DockingRestore");
			_dockingRestore.SaveToXml(xmlOut);
			xmlOut.WriteEndElement();

			// Save the floating Restore object to Xml
			xmlOut.WriteStartElement("FloatingRestore");
			_floatingRestore.SaveToXml(xmlOut);
			xmlOut.WriteEndElement();

			xmlOut.WriteEndElement();
		}

		internal void LoadFromXml(XmlTextReader xmlIn, int formatVersion, bool fullConfig)
		{
			// Read in the attribute values
			string attrTitle = xmlIn.GetAttribute(0);
			string attrVisible = xmlIn.GetAttribute(1);
            string attrDocked = xmlIn.GetAttribute(2);
            string attrAutoHide = xmlIn.GetAttribute(3);
            string attrCaptionBar = xmlIn.GetAttribute(4);
            string attrCloseButton = xmlIn.GetAttribute(5);
            string attrDisplaySize = xmlIn.GetAttribute(6);
			string attrDisplayLocation = xmlIn.GetAttribute(7);
            string attrAutoHideSize = xmlIn.GetAttribute(8);
            string attrFloatingSize = xmlIn.GetAttribute(9);
            string attrFullTitle = attrTitle;
            string attrAutoOffice2003 = "Disable";
			string attrUniqueName = "";
            string attrAutoOffice2007 = "Disable";
            string attrAutoMediaPlayer = "Disable";
            
			// 'FullTitle' property added in version 5 format and above
            if (formatVersion >= 5)
                attrFullTitle = xmlIn.GetAttribute(10);

			// 'AutoOffice2003' property added in version 7 format and above
			if (formatVersion >= 7)
				attrAutoOffice2003 = xmlIn.GetAttribute(11);

			// 'UniqueName' property added in version 8 format and above
			if (formatVersion >= 8)
				attrUniqueName = xmlIn.GetAttribute(12);

            // 'AutoOffice2007' property added in version 9 format and above
            if (formatVersion >= 9)
                attrAutoOffice2007 = xmlIn.GetAttribute(13);

            // 'AutoMediaPlayer' property added in version 10 format and above
            if (formatVersion >= 10)
                attrAutoMediaPlayer = xmlIn.GetAttribute(14);

            // Convert to correct types
			_title = attrTitle;
            _captionBar = Convert.ToBoolean(attrCaptionBar);
            _closeButton = Convert.ToBoolean(attrCloseButton);
            _displaySize = ConversionHelper.StringToSize(attrDisplaySize);
            _displayLocation = ConversionHelper.StringToPoint(attrDisplayLocation);
            _autoHideSize = ConversionHelper.StringToSize(attrAutoHideSize);
            _floatingSize = ConversionHelper.StringToSize(attrFloatingSize);
            _fullTitle = attrFullTitle;
            _office2003BackColor = (OfficeColor)Enum.Parse(typeof(OfficeColor), attrAutoOffice2003);
            _uniqueName = attrUniqueName;
            _office2007BackColor = (OfficeColor)Enum.Parse(typeof(OfficeColor), attrAutoOffice2007);
            _mediaPlayerBackColor = (MediaPlayerColor)Enum.Parse(typeof(MediaPlayerColor), attrAutoMediaPlayer);

			// Only a full config should reinstate the visible value			
			if (fullConfig)
			{
				_visible = Convert.ToBoolean(attrVisible);
			}
			
			// Can only override the current docking state if we are not visible
			// or doing a full configuration reload in which case it does not matter
			if (fullConfig || !_visible)
			{			
				_docked = Convert.ToBoolean(attrDocked);
				_autoHidden = Convert.ToBoolean(attrAutoHide);
			}

			// Load the Restore objects
			_defaultRestore = Restore.CreateFromXml(xmlIn, true, formatVersion);
			_autoHideRestore  = Restore.CreateFromXml(xmlIn, true, formatVersion);
			_dockingRestore  = Restore.CreateFromXml(xmlIn, true, formatVersion);
			_floatingRestore = Restore.CreateFromXml(xmlIn, true, formatVersion);

			// If not a full restore then just reloading content details, so reconnect now
			if (!fullConfig)
				ReconnectRestore();

			// Move past the end element
			if (!xmlIn.Read())
				throw new ArgumentException("Could not read in next expected node");
		
			// Check it has the expected name
			if (xmlIn.NodeType != XmlNodeType.EndElement)
				throw new ArgumentException("EndElement expected but not found");
		}
		
		private void UpdateVisibility()
        {
			if (_autoHidden)
				_visible = (_autoHidePanel != null);
			else
	            _visible = (_parentWindowContent != null);
        }
	} 
}
