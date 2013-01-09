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
	/// Base class for details that adorn a window.
	/// </summary>
    [ToolboxItem(false)]
    public class WindowDetail : Control
    {
        // Instance fields
        private Zone _parentZone;
        private Window _parentWindow;
        private DockingManager _manager;

		/// <summary>
		/// Initializes a new instance of the WindowDetail class.
		/// </summary>
		/// <param name="manager">Reference to docking manager.</param>
        public WindowDetail(DockingManager manager)
        {
            // Default the state
            _parentZone = null;
            _parentWindow = null;
            _manager = manager;
            
            // Get correct starting state from manager
            this.BackColor = _manager.BackColor;            
            this.ForeColor = _manager.InactiveTextColor;
        }

		/// <summary>
		/// Gets reference to the docking manager.
		/// </summary>
		public DockingManager DockingManager
		{
			get { return _manager; }
		}

		/// <summary>
		/// Gets and sets the parent zone.
		/// </summary>
        public virtual Zone ParentZone
        { 
            get { return _parentZone; }
            set { _parentZone = value; }
        }

		/// <summary>
		/// Gets and sets the parent window.
		/// </summary>
        public Window ParentWindow
        { 
            get { return _parentWindow; }
			
            set 
            {
                // Call virtual function for derived classes to override
                RemovedFromParent(_parentWindow);

                if (value == null)
                {
                    if (_parentWindow != null)
                    {
                        // Remove ourself from old parent window

						// Use helper method to circumvent form Close bug
						ControlHelper.Remove(_parentWindow.Controls, this);
                    }
                }
                else
                {
                    if ((_parentWindow != null) && (_parentWindow != value))
                    {
                        // Call virtual function for derived classes to override
                        RemovedFromParent(_parentWindow);

                        // Remove ourself from old parent window

						// Use helper method to circumvent form Close bug
						ControlHelper.Remove(_parentWindow.Controls, this);
                    }
	
                    // Add ourself to the new parent window
                    value.Controls.Add(this);
                }

                // Remember the new parent identity
                _parentWindow = value;

                if (_parentWindow != null)
                {
                    // Update to reflect new parent window state
                    ParentStateChanged(_parentWindow.State);
                }

                // Call virtual function for derived classes to override
                AddedToParent(_parentWindow);
			}
        }

		/// <summary>
		/// Handle the parent window getting focus.
		/// </summary>
        public virtual void WindowGotFocus() 
		{
			_manager.OnWindowDetailActivated(this);
		}

		/// <summary>
		/// Handle the parent window losing focus.
		/// </summary>
        public virtual void WindowLostFocus() 
		{
			_manager.OnWindowDetailDeactivated(this);
		}

		/// <summary>
		/// Handle a change in the image used when auto hidden property.
		/// </summary>
		/// <param name="autoHidden">New value of the property.</param>
        public virtual void NotifyAutoHideImage(bool autoHidden) {}

		/// <summary>
		/// Handle a change in showing the close button.
		/// </summary>
		/// <param name="show">New value of the property.</param>
        public virtual void NotifyCloseButton(bool show) {}

		/// <summary>
		/// Handle a change in showing the hide button.
		/// </summary>
		/// <param name="show">New value of the property.</param>
        public virtual void NotifyHideButton(bool show) {}

		/// <summary>
		/// Handle a change in showing the caption bar.
		/// </summary>
		/// <param name="show">New value of the property.</param>
        public virtual void NotifyShowCaptionBar(bool show) {}
		
		/// <summary>
		/// Handle a change in the caption bar text.
		/// </summary>
		/// <param name="title">New value of the property.</param>
        public virtual void NotifyFullTitleText(string title) {}

		/// <summary>
		/// Handle a change in the state of the parent window.
		/// </summary>
		/// <param name="newState">New value of the property.</param>
        public virtual void ParentStateChanged(State newState) {}

		/// <summary>
		/// Handle the detail being removed from the parent window.
		/// </summary>
		/// <param name="parent">Window parent we are removed from.</param>
        public virtual void RemovedFromParent(Window parent) {}

		/// <summary>
		/// Handle the detail being added to a parent window.
		/// </summary>
		/// <param name="parent">Window parent we are added to.</param>
        public virtual void AddedToParent(Window parent) {}

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
        }

		/// <summary>
		/// Raises the GotFocus event.
		/// </summary>
		/// <param name="e">An EventArgs structure containing the event data.</param>
        protected override void OnGotFocus(EventArgs e)
        {
            // Inform parent window we have the focus
            if (_parentWindow != null)
                _parentWindow.WindowDetailGotFocus(this);

            base.OnGotFocus(e);
        }

		/// <summary>
		/// Raises the LostFocus event.
		/// </summary>
		/// <param name="e">An EventArgs structure containing the event data.</param>
        protected override void OnLostFocus(EventArgs e)
        {
            // Inform parent window we have lost focus
            if (_parentWindow != null)
                _parentWindow.WindowDetailLostFocus(this);

            base.OnLostFocus(e);
        }		
    }
}
