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
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Docking
{
	/// <summary>
	/// Base class for window that can contain docking units.
	/// </summary>
    [ToolboxItem(false)]
    public class Window : ContainerControl
    {
        // Instance fields
        private State _state;
        private Zone _parentZone;
        private WindowDetailCollection _windowDetails;
        private Decimal _zoneArea;
        private Size _minimalSize;
        private DockingManager _manager;
        private bool _autoDispose;
        private bool _redockAllowed;
        private bool _floatingCaption;
        private bool _contentCaption;
        private string _fullTitle;

        /// <summary>
        /// Occurs when the full title has changed.
        /// </summary>
        public event EventHandler FullTitleChanged; 

		/// <summary>
		/// Initializes a new instance of the Window class.
		/// </summary>
		/// <param name="manager">Reference to docking manager.</param>
        public Window(DockingManager manager)
        {
            // Must provide a valid manager instance
            if (manager == null)
                throw new ArgumentNullException("DockingManager");

            // Default object state
            _state = State.Floating;
            _parentZone = null;
            _zoneArea = 100m;
            _minimalSize = new Size(0,0);
            _manager = manager;
            _autoDispose = true;
            _fullTitle = "";
            _redockAllowed = true;
            _floatingCaption = true;
            _contentCaption = true;

            // Create collection of window details
            _windowDetails = new WindowDetailCollection();

            // We want notification when window details are added/removed/cleared
            _windowDetails.Clearing += new CollectionClear(OnDetailsClearing);
            _windowDetails.Inserted += new CollectionChange(OnDetailInserted);
            _windowDetails.Removing += new CollectionChange(OnDetailRemoving);
        }

		/// <summary>
		/// Gets a reference to the docking manager.
		/// </summary>
        public DockingManager DockingManager
        {
            get { return _manager; }
        }

		/// <summary>
		/// Gets and sets the docking state of the window.
		/// </summary>
        public State State
        {
            get { return _state; }
			
            set 
            {
                if (_state != value)
                {
                    _state = value;

                    // Inform each window detail of the change in state
                    foreach(WindowDetail wd in _windowDetails)
                        wd.ParentStateChanged(_state);
                }
            }
        }

		/// <summary>
		/// Gets and sets the parent zone that contains this window.
		/// </summary>
        public Zone ParentZone
        {
            get { return _parentZone; }
			
            set 
            { 
                if (_parentZone != value)
                {
                    _parentZone = value; 

                    // Inform each window detail of the change in zone
                    foreach(WindowDetail wd in _windowDetails)
                        wd.ParentZone = _parentZone;
                }
            }
        }

		/// <summary>
		/// Gets and sets the collection of details.
		/// </summary>
        public WindowDetailCollection WindowDetails
        {
            get { return _windowDetails; }
			
            set
            {
                _windowDetails.Clear();
                _windowDetails = value;
            }
        }

		/// <summary>
		/// Gets and sets the percentage space this window occupies in parent zone.
		/// </summary>
        public Decimal ZoneArea
        {
            get { return _zoneArea; }
            set { _zoneArea = value; }
        }

		/// <summary>
		/// Gets and sets the minimum screen size this window must occupy.
		/// </summary>
        public Size MinimalSize
        {
            get { return _minimalSize; }
            set { _minimalSize = value; }
        }

		/// <summary>
		/// Gets and sets the need to automatically dispose this window.
		/// </summary>
        public bool AutoDispose
        {
            get { return _autoDispose; }
            set { _autoDispose = value; }
        }

		/// <summary>
		/// Gets the text string to use in window title.
		/// </summary>
        public string FullTitle
        {
            get { return _fullTitle; }
        }

		/// <summary>
		/// Gets and sets a value indicating if the window can be redocked.
		/// </summary>
        public bool RedockAllowed
        {
            get { return _redockAllowed; }
            set { _redockAllowed = value; }
        }
		
		/// <summary>
		/// Process a change in the full title text.
		/// </summary>
		/// <param name="title">New full title text.</param>
        public virtual void NotifyFullTitleText(string title)
        {
            // Inform each detail of change in focus
            foreach(WindowDetail wd in _windowDetails)
                wd.NotifyFullTitleText(title);
                
            OnFullTitleChanged(title);
        }

		/// <summary>
		/// Process a change in the auto hide image.
		/// </summary>
		/// <param name="autoHidden">New auto hidden state.</param>
        public virtual void NotifyAutoHideImage(bool autoHidden)
        {
            // Inform each detail of change in caption bar
            foreach(WindowDetail wd in _windowDetails)
                wd.NotifyAutoHideImage(autoHidden);
        }

		/// <summary>
		/// Process a change in showing of caption bar.
		/// </summary>
		/// <param name="show">New caption bar state.</param>
        public virtual void NotifyShowCaptionBar(bool show)
        {
            // Remember the per-content requested caption
            _contentCaption = show;
        
            // If priority value always showing then we can let the
            // individual content decide on visibility. Otherwise
            // the priority forces it to remain hidden
            if (_floatingCaption)
            {
                // Inform each detail of change in caption bar
                foreach(WindowDetail wd in _windowDetails)
                    wd.NotifyShowCaptionBar(show);
            }
        }

		/// <summary>
		/// Process a change in close button state.
		/// </summary>
		/// <param name="show">New close button state.</param>
        public virtual void NotifyCloseButton(bool show)
        {
            // Inform each detail of change close button
            foreach(WindowDetail wd in _windowDetails)
                wd.NotifyCloseButton(show);
        }

		/// <summary>
		/// Process a change in hide button state.
		/// </summary>
		/// <param name="show">New hide button state.</param>
        public virtual void NotifyHideButton(bool show)
        {
            // Inform each detail of change close button
            foreach(WindowDetail wd in _windowDetails)
                wd.NotifyHideButton(show);
        }

		/// <summary>
		/// Process a change in content focus.
		/// </summary>
        public virtual void NotifyContentGotFocus()
        {
            // Inform each detail of change in focus
            foreach(WindowDetail wd in _windowDetails)
                wd.WindowGotFocus();

			_manager.OnWindowActivated(this);
        }

		/// <summary>
		/// Process a change in content focus.
		/// </summary>
        public virtual void NotifyContentLostFocus()
        {
            // Inform each detail of change in focus
            foreach(WindowDetail wd in _windowDetails)
                wd.WindowLostFocus();

			_manager.OnWindowDeactivated(this);
		}

		/// <summary>
		/// Process a change in content detail focus.
		/// </summary>
		/// <param name="wd">WindowDetail instance.</param>
        public virtual void WindowDetailGotFocus(WindowDetail wd)
        {
            NotifyContentGotFocus();
        }
		
		/// <summary>
		/// Process a change in content detail focus.
		/// </summary>
		/// <param name="wd">WindowDetail instance.</param>
        public virtual void WindowDetailLostFocus(WindowDetail wd)
        {
            NotifyContentLostFocus();
        }
        
		/// <summary>
		/// Hide all the details.
		/// </summary>
        public void HideDetails()
        {
            // Inform each detail of change in visibility
            foreach(WindowDetail wd in _windowDetails)
                wd.Hide();
                
            // Remember priority state for caption
            _floatingCaption = false;
        }

		/// <summary>
		/// Show all the details.
		/// </summary>
        public void ShowDetails()
        {
            // Inform each detail of change in visibility
            foreach(WindowDetail wd in _windowDetails)
                wd.Show();

            // Remember priority state for caption
            _floatingCaption = true;
            
            // If the content requested the caption be hidden
            if (!_contentCaption)
                NotifyShowCaptionBar(_contentCaption);
        }
        
		/// <summary>
		/// Raises the FullTitleChanged event.
		/// </summary>
		/// <param name="fullTitle">New string.</param>
        protected virtual void OnFullTitleChanged(String fullTitle)
        {
            _fullTitle = fullTitle;
            
            if (FullTitleChanged != null)
                FullTitleChanged((object)fullTitle, EventArgs.Empty);
        }

		/// <summary>
		/// Create a new restore instance for the window.
		/// </summary>
		/// <param name="child">Child object instance.</param>
		/// <returns>New restore instance.</returns>
		public virtual Restore RecordRestore(object child) 
		{
			// Do we have a Zone as our parent?
			if (_parentZone != null)
			{
				// Delegate to the Zone as we cannot help out
				return _parentZone.RecordRestore(this, child, null);
			}

			return null;
		}

		/// <summary>
		/// Propogate change in a property.
		/// </summary>
		/// <param name="name">Property that has changed.</param>
		/// <param name="value">New value.</param>
        public virtual void PropogateNameValue(PropogateName name, object value)
        {
            if (name == PropogateName.BackColor)
            {
                this.BackColor = (Color)value;
                Invalidate();
            }

            // Pass onto each of our child Windows
            foreach(WindowDetail wd in _windowDetails)
                wd.PropogateNameValue(name, value);
        }


		private void OnDetailsClearing()
		{
			// Inform each detail it no longer has a parent
			foreach(WindowDetail wd in _windowDetails)
			{
				// Inform each detail it no longer has a parent
				wd.ParentWindow = null;

				// Inform object that it is no longer in a Zone
				wd.ParentZone = null;
			}
		}

		private void OnDetailInserted(int index, object value)
		{
			WindowDetail wd = value as WindowDetail;

			// Inform object we are the new parent
			wd.ParentWindow = this;

			// Inform object that it is in a Zone
			wd.ParentZone = _parentZone;
		}

		private void OnDetailRemoving(int index, object value)
		{
			WindowDetail wd = value as WindowDetail;

			// Inform object it no longer has a parent
			wd.ParentWindow = null;
			
			// Inform object that it is no longer in a Zone
			wd.ParentZone = null;
		}
	}
}
