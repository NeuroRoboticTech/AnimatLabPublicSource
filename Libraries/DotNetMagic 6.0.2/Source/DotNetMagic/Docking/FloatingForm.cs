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
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using Crownwood.DotNetMagic.Win32;
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Docking
{
	/// <summary>
	/// Form used to host a floating zone.
	/// </summary>
	public class FloatingForm : Form, IHotZoneSource, IMessageFilter
	{
		// Class constants
		private const int HITTEST_CAPTION = 2;

		// Instance variables
        private Zone _zone;
		private bool _intercept;
		private RedockerContent _redocker;
        private DockingManager _dockingManager;

		/// <summary>
		/// Occurs when context menu is required.
		/// </summary>
		public event ContextHandler Context;
        
		/// <summary>
		/// Initializes a new instance of the FloatingForm class.
		/// </summary>
		/// <param name="dockingManager">Parent docking manager instance.</param>
		/// <param name="zone">Zone to be hosted.</param>
		/// <param name="contextHandler">Delegate for handling context menus.</param>
        public FloatingForm(DockingManager dockingManager, Zone zone, ContextHandler contextHandler)
        {
            // Is the floating form allowed to be resized by the user
            if (!dockingManager.AllowResize)
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
            else
                this.FormBorderStyle = FormBorderStyle.Sizable;

            // Try and hook into the owners visible change event
            if (this.Owner != null)
            {
                // Copy across the right to left settings
                RightToLeft = Owner.RightToLeft;
                RightToLeftLayout = Owner.RightToLeftLayout;

                // Monitor change in the owning Form visibility
                this.Owner.VisibleChanged += new EventHandler(OnOwnerVisibleChanged);
            }

            // The caller is responsible for setting our initial screen location
            this.StartPosition = FormStartPosition.Manual;

            // Ask the docking manager if we should have an icon in the taskbar showing
            this.ShowInTaskbar = dockingManager.FloatingInTaskBar;

			// If the docking manager has a specific icon to be used, then use it
			if (dockingManager.FloatingTaskBarIcon != null)
				this.Icon = dockingManager.FloatingTaskBarIcon;
        
            // Make sure the main Form owns us
            this.Owner = dockingManager.Container.FindForm();

			// Never show by default
			this.Visible = false;
            
            // Need to know when the Zone is removed
            this.ControlRemoved += new ControlEventHandler(OnZoneRemoved);
            
            // Add the Zone as the only content of the Form
            Controls.Add(zone);

            // Default state
            _redocker = null;
            _intercept = false;
            _zone = zone;
            _dockingManager = dockingManager;

            // Assign any event handler for context menu
            if (contextHandler != null)
                this.Context += contextHandler;	

            // Default color
            this.BackColor = _dockingManager.BackColor;
            this.ForeColor = _dockingManager.InactiveTextColor;

            // Monitor changes in the Zone content
            _zone.Windows.Inserted += new CollectionChange(OnWindowInserted);
            _zone.Windows.Removing += new CollectionChange(OnWindowRemoving);
            _zone.Windows.Removed += new CollectionChange(OnWindowRemoved);
			_zone.Windows.Clearing += new CollectionClear(OnWindowClearing);

            if (_zone.Windows.Count == 1)
            {
                // The first Window to be added. Tell it to hide details
                _zone.Windows[0].HideDetails(); 
                
                // Monitor change in window title
                _zone.Windows[0].FullTitleChanged += new EventHandler(OnFullTitleChanged);  

				WindowContent wc = _zone.Windows[0] as WindowContent;

				// Hook into changes in window contents
				wc.Contents.Inserted += new CollectionChange(OnContentsInserted);
				wc.Contents.Removing += new CollectionChange(OnContentsRemoving);
				wc.Contents.Clearing += new CollectionClear(OnContentsClearing);

				// Notice changes to properties
				foreach(Content c in wc.Contents)
					c.PropertyChanged += new Crownwood.DotNetMagic.Docking.Content.PropChangeHandler(OnContentPropertyChanged);

				// Decide if the form close button is needed
				ProbeContentWithoutClose();

                // Grab any existing title
                this.Text = _zone.Windows[0].FullTitle;
            }
            
            // Need to hook into message pump so that the ESCAPE key can be 
            // intercepted when in redocking mode
            Application.AddMessageFilter(this);
        }

		/// <summary>
		/// Gets the parent docking manager instance.
		/// </summary>
        public DockingManager DockingManager
        {
            get { return _dockingManager; }
        }

		/// <summary>
		/// Gets the hosted zone instance.
		/// </summary>
        public Zone Zone
        {
            get { return this.Controls[0] as Zone; }
		}

		/// <summary>
		/// Caller is requesting the FloatingForm is made visible.
		/// </summary>
		public void RequestShow()
		{
			if (this.Owner != null)
			{
				// We only become visible if our owning parent is
				if (this.Owner.Visible)
					this.Show();
			}
			else
				this.Show();
		}

		/// <summary>
		/// Process propogation of values.
		/// </summary>
		/// <param name="name">Property being notified.</param>
		/// <param name="value">New value of property.</param>
        public void PropogateNameValue(PropogateName name, object value)
        {
            if (this.Zone != null)
                this.Zone.PropogateNameValue(name, value);

			// If a change in taskbar state then update immediately
			if (name == PropogateName.FloatingInTaskBar)
				this.ShowInTaskbar = (bool)value;

			// If the docking manager has a specific icon to be used, then use it
			if (name == PropogateName.FloatingTaskBarIcon)
			{
				// Can only set a valid icon into Form
				if (value != null)
					this.Icon = _dockingManager.FloatingTaskBarIcon;
			}

            // Is the floating form allowed to be resized by the user
            if (name == PropogateName.AllowResize)
            {
                if (!(bool)value)
                    this.FormBorderStyle = FormBorderStyle.FixedDialog;
                else
                    this.FormBorderStyle = FormBorderStyle.Sizable;
            }
        }
        
		/// <summary>
		/// Add our collection of hotzones into redocker.
		/// </summary>
		/// <param name="redock">Redocker target.</param>
		/// <param name="collection">Collection of hotzones.</param>
        public void AddHotZones(Redocker redock, HotZoneCollection collection)
        {
            RedockerContent redocker = redock as RedockerContent;

            // Allow the contained Zone a chance to expose HotZones
            foreach(Control c in this.Controls)
            {
                IHotZoneSource ag = c as IHotZoneSource;

                // Does this control expose an interface for its own HotZones?
                if (ag != null)
                    ag.AddHotZones(redock, collection);
            }
        }
           
		/// <summary>
		/// Disposes of the resources (other than memory) used by the class.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged; false to release only unmanaged. </param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Remove message filter to remove circular reference
				Application.RemoveMessageFilter(this);
			}

			base.Dispose(disposing);
		}

		/// <summary>
		/// Process a new window being inserted into zone.
		/// </summary>
		/// <param name="index">Index position of insertion.</param>
		/// <param name="value">New window instance.</param>
        protected void OnWindowInserted(int index, object value)
        {
            if (_zone.Windows.Count == 1)
            {
                // The first Window to be added. Tell it to hide details
                _zone.Windows[0].HideDetails();                
                
                // Monitor change in window title
                _zone.Windows[0].FullTitleChanged += new EventHandler(OnFullTitleChanged);  

                // Grab any existing title
                this.Text = _zone.Windows[0].FullTitle;
            }
            else if (_zone.Windows.Count == 2)
            {
				int pos = 0;
			
				// If the new Window is inserted at beginning then update the second Window
				if (index == 0)
					pos++;

                // The second Window to be added. Tell the first to now show details
                _zone.Windows[pos].ShowDetails();                
                
                // Monitor change in window title
                _zone.Windows[pos].FullTitleChanged -= new EventHandler(OnFullTitleChanged);  

                // Remove any caption title
                this.Text = "";
            }

			WindowContent wc = value as WindowContent;

			// Hook into changes in window contents
			wc.Contents.Inserted += new CollectionChange(OnContentsInserted);
			wc.Contents.Removing += new CollectionChange(OnContentsRemoving);
			wc.Contents.Clearing += new CollectionClear(OnContentsClearing);

			// Notice changes to properties
			foreach(Content c in wc.Contents)
				c.PropertyChanged += new Crownwood.DotNetMagic.Docking.Content.PropChangeHandler(OnContentPropertyChanged);

			// Decide if the form close button is needed
			ProbeContentWithoutClose();
        }
           
		/// <summary>
		/// Process a window before being removed from zone.
		/// </summary>
		/// <param name="index">Index position being removed.</param>
		/// <param name="value">Window instance.</param>
        protected void OnWindowRemoving(int index, object value)
        {
            if (_zone.Windows.Count == 1)
            {   
                // The first Window to be removed. Tell it to show details as we want 
                // to restore the Window state before it might be moved elsewhere
                _zone.Windows[0].ShowDetails();                
                
                // Monitor change in window title
                _zone.Windows[0].FullTitleChanged -= new EventHandler(OnFullTitleChanged);  
                
                // Remove any existing title text
                this.Text = "";
            }

			WindowContent wc = value as WindowContent;

			// Remove hooks into changs in window contents
			wc.Contents.Inserted -= new CollectionChange(OnContentsInserted);
			wc.Contents.Removing -= new CollectionChange(OnContentsRemoving);
			wc.Contents.Clearing-= new CollectionClear(OnContentsClearing);

			// Notice changes to properties
			foreach(Content c in wc.Contents)
				c.PropertyChanged -= new Crownwood.DotNetMagic.Docking.Content.PropChangeHandler(OnContentPropertyChanged);
		}

		/// <summary>
		/// Process a window after it has been removed from zone.
		/// </summary>
		/// <param name="index">Index position being removed.</param>
		/// <param name="value">Window instance.</param>
        protected void OnWindowRemoved(int index, object value)
        {
            if (_zone.Windows.Count == 1)
            {   
                // Window removed leaving just one left. Tell it to hide details
                _zone.Windows[0].HideDetails();                

                // Monitor change in window title
                _zone.Windows[0].FullTitleChanged += new EventHandler(OnFullTitleChanged);  
                
                // Grab any existing title text
                this.Text = _zone.Windows[0].FullTitle;
            }

			// Decide if the form close button is needed
			ProbeContentWithoutClose();
        }

		/// <summary>
		/// Process the contents about to be removed from the zone.
		/// </summary>
		protected void OnWindowClearing()
		{
			foreach(WindowContent wc in _zone.Windows)
			{
				// Remove hooks into changs in window contents
				wc.Contents.Inserted -= new CollectionChange(OnContentsInserted);
				wc.Contents.Removing -= new CollectionChange(OnContentsRemoving);
				wc.Contents.Clearing -= new CollectionClear(OnContentsClearing);

				// Notice changes to properties
				foreach(Content c in wc.Contents)
					c.PropertyChanged -= new Crownwood.DotNetMagic.Docking.Content.PropChangeHandler(OnContentPropertyChanged);
			}

			// Decide if the form close button is needed
			ProbeContentWithoutClose();
		}
        
		/// <summary>
		/// Modify the floating form title.
		/// </summary>
		/// <param name="sender">Source string.</param>
		/// <param name="e">An EventArgs structure containing event data.</param>
        protected void OnFullTitleChanged(object sender, EventArgs e)
        {
            // Unbox sent string
            this.Text = (string)sender;
        }
        
		/// <summary>
		/// Process the removal of zone.
		/// </summary>
		/// <param name="sender">Object source.</param>
		/// <param name="e">A ControlEventArgs structure containing event data.</param>
        protected void OnZoneRemoved(object sender, ControlEventArgs e)
        {
			// Is it the Zone being removed for a hidden button used to help
			// remove controls without hitting the 'form refuses to close' bug
			if (e.Control == _zone)
			{
				if (_zone.Windows.Count == 1)
				{   
					// The first Window to be removed. Tell it to show details as we want 
					// to restore the Window state before it might be moved elsewhere
					_zone.Windows[0].ShowDetails();                

					// Remove monitor change in window title
					_zone.Windows[0].FullTitleChanged -= new EventHandler(OnFullTitleChanged);  
				}
        
				// Monitor changes in the Zone content
				_zone.Windows.Inserted -= new CollectionChange(OnWindowInserted);
				_zone.Windows.Removing -= new CollectionChange(OnWindowRemoving);
				_zone.Windows.Removed -= new CollectionChange(OnWindowRemoved);

				// No longer required, commit suicide
				this.Dispose();
			}
        }
        
		/// <summary>
		/// Gets the required creation parameters when the control handle is created.
		/// </summary>
        protected override CreateParams CreateParams 
        {
            get 
            {
                // Let base class fill in structure first
                CreateParams cp = base.CreateParams;

                // Create a top level popup window
                cp.Style |= unchecked((int)(Win32.WindowStyles.WS_POPUP |
                                            Win32.WindowStyles.WS_CLIPSIBLINGS |
                                            Win32.WindowStyles.WS_CLIPCHILDREN));

                // It must a tool window for a small caption height
                cp.ExStyle |= (int)Win32.WindowExStyles.WS_EX_TOOLWINDOW |
                              (int)Win32.WindowExStyles.WS_EX_WINDOWEDGE;

                // We need a parent setting the same as the owner
                cp.Parent = DockingManager._floatingFormContainer.Handle;

                return cp;
            }
        }

		/// <summary>
		/// Raises the Context event.
		/// </summary>
        /// <param name="cc">The collection of content instances.</param>
        /// <param name="screenPos">Screen position of mouse.</param>
        public virtual void OnContext(ContentCollection cc, Point screenPos)
        {
            // Any attached event handlers?
            if (Context != null)
                Context(cc, screenPos);
        }

		/// <summary>
		/// Floating form is about to exit.
		/// </summary>
		public void ExitFloating()
		{
			if (_zone != null)
			{
				ContentCollection cc = ZoneHelper.Contents(_zone);

				// Record restore object for each Content
				foreach(Content c in cc)
				{
					c.RecordFloatingRestore();
					c.Docked = true;
				}
			}
		}

		/// <summary>
		/// Restore the zone.
		/// </summary>
        protected void Restore()
        {
			if (_zone != null)
			{
				ContentCollection cc = ZoneHelper.Contents(_zone);

				// Record restore object for each Content
				foreach(Content c in cc)
				{
					c.RecordFloatingRestore();
					c.Docked = true;
				}

				// Ensure each content is removed from any Parent
				foreach(Content c in cc)
					_dockingManager.HideContent(c, false, true, false);
				
				// Now restore each of the Content
				foreach(Content c in cc)
					_dockingManager.ShowContent(c);

				// Finished all changes, no its safe to update inside fill
				_dockingManager.UpdateInsideFill();
				_dockingManager.CheckResized();
			}

			this.Close();
        }

		/// <summary>
		/// Update each zone content with new position.
		/// </summary>
		/// <param name="e">An EventArgs structure containing the event data.</param>
		protected override void OnMove(EventArgs e)
		{
			Point newPos = this.Location;
			
			// Grab the aggregate collection of all Content objects in the Zone
			ContentCollection cc = ZoneHelper.Contents(_zone);
			
			// Update each one with the new FloatingForm location
			foreach(Content c in cc)
				c.DisplayLocation = newPos;			

			base.OnMove(e);
		}

		/// <summary>
		/// Close each of the content that are allowed to be removed.
		/// </summary>
		/// <param name="e">A CancelEventArgs structure contained event data.</param>
		protected override void OnClosing(CancelEventArgs e)
		{
			if (_zone != null)
			{
				ContentCollection cc = ZoneHelper.Contents(_zone);

				// Record restore object for each Content
				foreach(Content c in cc)
					c.RecordRestore();

				// Ensure each content is removed from any Parent
				foreach(Content c in cc)
                {
					// Is content allowed to be hidden?
					if (!_dockingManager.OnContentHiding(c))
					{
						// Hide the content always
						_dockingManager.HideContent(c, false, true, true);
					}
					else
					{
						// At least one Content refuses to die, so do not
						// let the whole floating form be closed down
						e.Cancel = true;
					}
                }
			}

			// Must set focus back to the main application Window
			if (this.Owner != null)
			{
				// Unhook from owner events
				this.Owner.VisibleChanged -= new EventHandler(OnOwnerVisibleChanged);

				// Set focus back again
				this.Owner.Activate();
			}

			base.OnClosing(e);

			// Get the main application to check if restored content affects sizing
			_dockingManager.CheckResized();
		}

		/// <summary>
		/// Update each zone content with new size.
		/// </summary>
		/// <param name="e"></param>
        protected override void OnResize(System.EventArgs e)
        {
            // Grab the aggregate collection of all Content objects in the Zone
            ContentCollection cc = ZoneHelper.Contents(_zone);
			
			// Do not include the caption height of the tool window in the saved height
			Size newSize = new Size(this.Width, this.Height - SystemInformation.ToolWindowCaptionHeight);
			
            // Update each one with the new FloatingForm location
            foreach(Content c in cc)
                c.FloatingSize = newSize;

            base.OnResize(e);
        }

		/// <summary>
		/// Filters out a message before it is dispatched.
		/// </summary>
		/// <param name="m">The message to be dispatched. You cannot modify this message.</param>
		/// <returns></returns>
        public bool PreFilterMessage(ref Message m)
        {
            // Has a key been pressed?
            if (m.Msg == (int)Win32.Msgs.WM_KEYDOWN)
            {
                // Is it the ESCAPE key?
                if ((int)m.WParam == (int)Win32.VirtualKeys.VK_ESCAPE)
                {                   
                    // Are we in a redocking activity?
                    if (_intercept)
                    {
                        // Quite redocking
                        _redocker.QuitTrackingMode(null);

                        // Release capture
                        this.Capture = false;
                    
                        // Reset state
                        _intercept = false;

                        return true;
                    }
                }
            }
            
            return false;
        }

		/// <summary>
		/// Processes Windows messages.
		/// </summary>
		/// <param name="m">The Windows Message to process. </param>
        protected override void WndProc(ref Message m)
		{
			// Want to notice when the window is maximized
			if (m.Msg == (int)Win32.Msgs.WM_NCLBUTTONDBLCLK)
			{
				// Is the user allowed to redock?
				if (_dockingManager.AllowRedocking)
				{
                    // Give user a chance to cancel the default action
                    CancelEventArgs ce = new CancelEventArgs(false);
                    _dockingManager.OnDoubleClickFloatingTitle(this, ce);

                    // If the action was not cancelled
                    if (!ce.Cancel)
                    {
                        // Redock and kill ourself
                        Restore();

                        // Raise event to indicate a double click occured
                        _dockingManager.OnDoubleClickDock();
                    }
				}
				
				// We do not want to let the base process the message as the 
				// restore might fail due to lack of permission to restore to 
				// old state.  In that case we do not want to maximize the window
				return;
			}
			else if (m.Msg == (int)Win32.Msgs.WM_NCLBUTTONDOWN)
			{
				if (!_intercept)
				{
					// Perform a hit test against our own window to determine 
					// which area the mouse press is over at the moment.
					uint result = User32.SendMessage(this.Handle, (int)Win32.Msgs.WM_NCHITTEST, 0, (uint)m.LParam);
                
					// Only want to override the behaviour of moving the window via the caption box
					if (result == HITTEST_CAPTION)
					{
						// Is the user allowed to redock?
						if (_dockingManager.AllowRedocking)
						{
							// Remember new state
							_intercept = true;
	                    
							// Capture the mouse until the mouse us is received
							this.Capture = true;
	                        
							// Ensure that we gain focus and look active
							this.Activate();

							// Raise event to indicate a floating form has become active
							_dockingManager.OnFloatingFormActivated(this);

							// Get mouse position to inscreen coordinates
							Win32.POINT mousePos;
							mousePos.x = (short)((uint)m.LParam & 0x0000FFFFU);
							mousePos.y = (short)(uint)(((uint)m.LParam & 0xFFFF0000U) >> 16);

							// Begin a redocking activity
							_redocker = Redocker.CreateRedocker(DockingManager.FeedbackStyle,
																this, new Point(mousePos.x - DesktopBounds.X, 
																				mousePos.y - DesktopBounds.Y));
	                        
	                        
							return;
						}
					}
				}
			}
			else if (m.Msg == (int)Win32.Msgs.WM_MOUSEMOVE)
			{
				if (_intercept)
				{
					Win32.POINT mousePos;
					mousePos.x = (short)((uint)m.LParam & 0x0000FFFFU);
					mousePos.y = (short)(uint)(((uint)m.LParam & 0xFFFF0000U) >> 16);

					_redocker.OnMouseMove(new MouseEventArgs(MouseButtons.Left, 
															 0, mousePos.x, mousePos.y, 0));
                
					return;
				}
			}
			else if ((m.Msg == (int)Win32.Msgs.WM_RBUTTONDOWN) ||
					 (m.Msg == (int)Win32.Msgs.WM_MBUTTONDOWN))
			{
				if (_intercept)
				{
					Win32.POINT mousePos;
					mousePos.x = (short)((uint)m.LParam & 0x0000FFFFU);
					mousePos.y = (short)(uint)(((uint)m.LParam & 0xFFFF0000U) >> 16);

					_redocker.QuitTrackingMode(new MouseEventArgs(MouseButtons.Left, 
																  0, mousePos.x, mousePos.y, 0));
                
					// Release capture
					this.Capture = false;
                    
					// Reset state
					_intercept = false;

					return;
				}
			}
			else if (m.Msg == (int)Win32.Msgs.WM_LBUTTONUP)
			{
				if (_intercept)
				{
					Win32.POINT mousePos;
					mousePos.x = (short)((uint)m.LParam & 0x0000FFFFU);
					mousePos.y = (short)(uint)(((uint)m.LParam & 0xFFFF0000U) >> 16);
		
					_redocker.OnMouseUp(new MouseEventArgs(MouseButtons.Left, 0, 
						                                   mousePos.x, mousePos.y, 0));

					// Release capture
					this.Capture = false;
                    
					// Reset state
					_intercept = false;

					return;
				}
			} 
			else if ((m.Msg == (int)Win32.Msgs.WM_NCRBUTTONUP) ||
				     (m.Msg == (int)Win32.Msgs.WM_NCMBUTTONDOWN) ||
				     (m.Msg == (int)Win32.Msgs.WM_NCMBUTTONUP) ||
			         (m.Msg == (int)Win32.Msgs.WM_RBUTTONDOWN) ||
				     (m.Msg == (int)Win32.Msgs.WM_RBUTTONUP) ||
			         (m.Msg == (int)Win32.Msgs.WM_MBUTTONDOWN) ||
				     (m.Msg == (int)Win32.Msgs.WM_MBUTTONUP))
			{
			    // Prevent middle and right mouse buttons from interrupting
			    // the correct operation of left mouse dragging
			    return;
			} 
			else if (m.Msg == (int)Win32.Msgs.WM_NCRBUTTONDOWN)
			{
			    if (!_intercept)
			    {
				    // Get screen coordinates of the mouse
                    Win32.POINT mousePos;
                    mousePos.x = (short)((uint)m.LParam & 0x0000FFFFU);
                    mousePos.y = (short)(uint)(((uint)m.LParam & 0xFFFF0000U) >> 16);
        			
				    OnContext(ZoneHelper.Contents(_zone), new Point(mousePos.x, mousePos.y));
                    return;		
                }
			}

			base.WndProc(ref m);
		}

        /// <summary>
        /// This member overrides Control.ProcessCmdKey.
        /// </summary>
        /// <param name="msg">Windows message</param>
        /// <param name="keyData">KryData for the message.</param>
        /// <returns>True if processed; otherwise false.</returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (base.ProcessCmdKey(ref msg, keyData))
                return true;
            else
            {
                // Get the form that owns the docking manager
                Form ownerForm = _dockingManager.Container.FindForm();

                if (ownerForm != null)
                {
                    // Get access to the type information for docking container
                    Type tp = ownerForm.GetType();

                    // We want a protected virtual method
                    MethodInfo mi = tp.GetMethod("ProcessCmdKey",
                                                 BindingFlags.NonPublic |
                                                 BindingFlags.Instance);

                    if (mi != null)
                    {
                        // Remember the original window handle
                        IntPtr temp = msg.HWnd;

                        // Fake the message as being for the docking container
                        // (if message is left for the floating form it will not be recognized by
                        //  the toolstrip manager as relevant to the toolstrips on main form)
                        msg.HWnd = _dockingManager.Container.Handle;

                        // Call with our own provided parameters
                        object ret = mi.Invoke(ownerForm, new object[] { msg, keyData });

                        // Put back original handle
                        msg.HWnd = temp;

                        if (ret != null)
                            return (bool)ret;
                    }
                }
            }

            return false;
        } 

		private void OnContentsInserted(int index, object value)
		{
			// Cast to correct type
			Content c = value as Content;

			// Notice changes to properties
			c.PropertyChanged += new Crownwood.DotNetMagic.Docking.Content.PropChangeHandler(OnContentPropertyChanged);

			// Decide if the form close button is needed
			ProbeContentWithoutClose();
		}

		private void OnContentsRemoving(int index, object value)
		{
			// Cast to correct type
			Content c = value as Content;

			// Notice changes to properties
			c.PropertyChanged -= new Crownwood.DotNetMagic.Docking.Content.PropChangeHandler(OnContentPropertyChanged);

			// Decide if the form close button is needed
			ProbeContentWithoutClose();
		}

		private void OnContentsClearing()
		{
			// Decide if the form close button is needed
			ProbeContentWithoutClose();
		}

		private void OnContentPropertyChanged(Content c, Crownwood.DotNetMagic.Docking.Content.Property prop)
		{
			// We are interested in changes to the close button property
			if (prop == Crownwood.DotNetMagic.Docking.Content.Property.CloseButton)
			{
				// Decide if the form close button is needed
				ProbeContentWithoutClose();
			}
		}

		private void ProbeContentWithoutClose()
		{
			bool foundNoClose = false;

			// Process each window inside the zone
			foreach(WindowContent wc in _zone.Windows)
			{
				// Process each content in the window
				foreach(Content c in wc.Contents)
				{
					if (!c.CloseButton)
					{
						foundNoClose = true;
						break;
					}
				}
			}

			// If any of the content cannot be closed, then prevent closing form
			this.ControlBox = !foundNoClose;
		}

		private void OnOwnerVisibleChanged(object sender, EventArgs e)
		{
			if (this.Owner != null)
			{
				// Change our visible state to match that of owner Form
				this.Visible = this.Owner.Visible;
			}
		}
	}
}
