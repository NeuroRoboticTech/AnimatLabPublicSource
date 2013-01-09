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
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Microsoft.Win32;
using Crownwood.DotNetMagic.Common;
using Crownwood.DotNetMagic.Controls;

namespace Crownwood.DotNetMagic.Docking
{
	/// <summary>
	/// Base class for caption detail implementations.
	/// </summary>
    [ToolboxItem(false)]
    public abstract class WindowDetailCaption : WindowDetail, IMessageFilter
    {
		/// <summary>
		/// Occurs when the user clicks the close detail button.
		/// </summary>
		public event EventHandler Close;

		/// <summary>
		/// Occurs when the user clicks the restore detail button.
		/// </summary>
		public event EventHandler Restore;

		/// <summary>
		/// Occurs when the user clicks the invert auto hide detail button.
		/// </summary>
		public event EventHandler InvertAutoHide;

		/// <summary>
		/// Occurs when the user requests a context menu.
		/// </summary>
        public event ContextHandler Context;

        internal static ImageAttributes _activeAttr = new ImageAttributes();
        internal static ImageAttributes _inactiveAttr = new ImageAttributes();
        internal WindowContent _wc;
        internal ButtonWithStyle _maxButton;
        internal ButtonWithStyle _closeButton;
        internal ButtonWithStyle _hideButton;
        internal RedockerContent _redocker;
        internal IZoneMaximizeWindow _maxInterface;
        internal bool _showCloseButton;
        internal bool _showHideButton;
        internal bool _ignoreHideButton;
        internal bool _pinnedImage;

		// Instance fields
		private bool _testForDrag;
		private Point _mouseDown;
		
		/// <summary>
		/// Initializes a new instance of the WindowDetailCaption class.
		/// </summary>
		/// <param name="manager">Reference to parent docking manager.</param>
        /// <param name="wc">Owning window content.</param>
        /// <param name="fixedSize">Fixed size of the detail.</param>
		/// <param name="closeHandler">Delegate for notifying close events.</param>
		/// <param name="restoreHandler">Delegate for notifying restore events.</param>
		/// <param name="invertAutoHideHandler">Delegate for auto hide invert events.</param>
		/// <param name="contextHandler">Delegate for notifying context menu events.</param>
        public WindowDetailCaption(DockingManager manager, 
                                   WindowContent wc,
                                   Size fixedSize, 
                                   EventHandler closeHandler, 
                                   EventHandler restoreHandler, 
                                   EventHandler invertAutoHideHandler, 
                                   ContextHandler contextHandler)
            : base(manager)
        {
            // Setup correct color remapping depending on initial colors
            DefineButtonRemapping();

            // Default state
            _wc = wc;
            _maxButton = null;
            _hideButton = null;
            _maxInterface = null;
            _redocker = null;
            _showCloseButton = true;
            _showHideButton = true;
            _ignoreHideButton = false;
            _pinnedImage = false;
            _testForDrag = false;
            
            // Prevent flicker with double buffering and all painting inside WM_PAINT
            SetStyle(ControlStyles.OptimizedDoubleBuffer | 
					 ControlStyles.AllPaintingInWmPaint |
					 ControlStyles.UserPaint, true);

            // Our size is always fixed at the required length in both directions
            // as one of the sizes will be provided for us because of our docking
            this.Size = fixedSize;

            if (closeHandler != null)
                this.Close += closeHandler;	

            if (restoreHandler != null)
                this.Restore += restoreHandler;	

            if (invertAutoHideHandler != null)
                this.InvertAutoHide += invertAutoHideHandler;
    
            if (contextHandler != null)
                this.Context += contextHandler;	

            // Let derived classes override the button creation
            CreateButtons();

            // Need to hook into message pump so that the ESCAPE key can be 
            // intercepted when in redocking mode
            Application.AddMessageFilter(this);
        }

		/// <summary>
		/// Sets the paret zone that contains this caption detail.
		/// </summary>
        public override Zone ParentZone
        {
            set
            {
                base.ParentZone = value;

                RecalculateMaximizeButton();
                RecalculateButtons();
            }
        }

		/// <summary>
		/// Raises the Close event.
		/// </summary>
        protected virtual void OnClose()
        {
            // Any attached event handlers?
            if (Close != null)
                Close(this, EventArgs.Empty);
        }

		/// <summary>
		/// Raises the InvertAutoHide event.
		/// </summary>
		protected virtual void OnInvertAutoHide()
        {
            // Any attached event handlers?
            if (InvertAutoHide != null)
                InvertAutoHide(this, EventArgs.Empty);
        }
        
		/// <summary>
		/// Raises the Restore event.
		/// </summary>
		protected virtual void OnRestore()
        {
            // Any attached event handlers?
            if (Restore != null)
                Restore(this, EventArgs.Empty);
        }

		/// <summary>
		/// Raises the Context event.
		/// </summary>
        /// <param name="cc">Collection of content instances.</param>
        /// <param name="screenPos">Screen location of mouse at time of requesting context menu.</param>
		public virtual void OnContext(ContentCollection cc, Point screenPos)
        {
            // Any attached event handlers?
            if (Context != null)
                Context(cc, screenPos);
        }

		/// <summary>
		/// Releases all resources used by the group.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
				if (_closeButton != null)
				{
					_closeButton.Click -= new EventHandler(OnButtonClose);
					_closeButton.GotFocus -= new EventHandler(OnButtonGotFocus);
				}

                if (_hideButton != null)
                {
                    _hideButton.Click -= new EventHandler(OnButtonHide);
                    _hideButton.GotFocus -= new EventHandler(OnButtonGotFocus);
                }
                
                if (_maxButton != null)
				{
					_maxButton.Click -= new EventHandler(OnButtonMax);
					_maxButton.GotFocus -= new EventHandler(OnButtonGotFocus);
				}
							
				// Remove message filter to remove circular reference
				Application.RemoveMessageFilter(this);
			}
            base.Dispose( disposing );
        }

		/// <summary>
		/// Handle a change in the image used when auto hidden property.
		/// </summary>
		/// <param name="autoHidden">New value of the property.</param>
        public override void NotifyAutoHideImage(bool autoHidden)
        {
            _pinnedImage = autoHidden;
            UpdateAutoHideImage();
        }

		/// <summary>
		/// Handle a change in showing the close button.
		/// </summary>
		/// <param name="show">New value of the property.</param>
		public override void NotifyCloseButton(bool show)
        {
            _showCloseButton = show;
            RecalculateButtons();
        }

		/// <summary>
		/// Handle a change in showing the hide button.
		/// </summary>
		/// <param name="show">New value of the property.</param>
		public override void NotifyHideButton(bool show)
        {
            // Ignore the AutoHide feature when in floating form
            _ignoreHideButton = (ParentWindow.State == State.Floating);
            
            _showHideButton = show;
            RecalculateButtons();
            Invalidate();
        }

		/// <summary>
		/// Handle a change in showing the caption bar.
		/// </summary>
		/// <param name="show">New value of the property.</param>
		public override void NotifyShowCaptionBar(bool show)
        {
            this.Visible = show;
        }
        
		/// <summary>
		/// Calculate if the maximize button should be available.
		/// </summary>
        protected void RecalculateMaximizeButton()
        {
            // Are we inside a Zone?
            if (this.ParentZone != null)
            {
                // Does the Zone support the maximizing of a Window?
                IZoneMaximizeWindow zmw = this.ParentZone as IZoneMaximizeWindow;

                if (zmw != null)
                {
                    AddMaximizeInterface(zmw);
                    return;
                }
            }

            RemoveMaximizeInterface();
        }

		/// <summary>
		/// Enable the maximize functionality.
		/// </summary>
		/// <param name="zmw"></param>
        protected void AddMaximizeInterface(IZoneMaximizeWindow zmw)
        {
            // Has the maximize button already been created?
            if (_maxInterface == null)
            {
				CreateMaxButton();

				// Define starting image
				_maxButton.Image = Images.Images[0];

				// Hook into button events
				_maxButton.Click += new EventHandler(OnButtonMax);
				_maxButton.GotFocus += new EventHandler(OnButtonGotFocus);

                OnAddMaximizeInterface();

                Controls.Add(_maxButton);

                // Remember the interface reference
                _maxInterface = zmw;

                // Hook into the interface change events
                _maxInterface.RefreshMaximize += new EventHandler(OnRefreshMaximize);

                RecalculateButtons();
                Invalidate();
            }
        }

		/// <summary>
		/// Create the button class specific to this implementation.
		/// </summary>
		protected virtual void CreateMaxButton()
		{
			// Create the InertButton
			_maxButton = new ButtonWithStyle();
		}

		/// <summary>
		/// Disable the maximize functionality.
		/// </summary>
        protected void RemoveMaximizeInterface()
        {
            if (_maxInterface != null)
            {
                // Unhook from the interface change events
                _maxInterface.RefreshMaximize -= new EventHandler(OnRefreshMaximize);

                // Remove the interface reference
                _maxInterface = null;

				// Use helper method to circumvent form Close bug
				ControlHelper.Remove(this.Controls, _maxButton);

                OnRemoveMaximizeInterface();

                // Unhook into button events
                _maxButton.Click -= new EventHandler(OnButtonMax);
                _maxButton.GotFocus -= new EventHandler(OnButtonGotFocus);

                // Kill the button which is no longer needed
                _maxButton.Dispose();
                _maxButton = null;

                RecalculateButtons();
                Invalidate();
            }
        }

		/// <summary>
		/// Refresh the state of the maximize functionality.
		/// </summary>
		/// <param name="sender">Source of event.</param>
		/// <param name="e">An EventArgs structure containing the event data.</param>
        protected void OnRefreshMaximize(object sender, EventArgs e)
        {
            UpdateMaximizeImage();
        }
	
		/// <summary>
		/// Process the user pressing the maximize/minimize button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        protected void OnButtonMax(object sender, EventArgs e)
        {
            if (this.ParentWindow != null)
            {
                if (_maxInterface.IsMaximizeAvailable())
                {
                    // Are we already maximized?
                    if (_maxInterface.IsWindowMaximized(this.ParentWindow))
                        _maxInterface.RestoreWindow();
                    else
                        _maxInterface.MaximizeWindow(this.ParentWindow);
                }
            }			
        }

		/// <summary>
		/// Process the user pressing the close button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        protected void OnButtonClose(Object sender, EventArgs e)
        {
            if (_showCloseButton)
                OnClose();
        }

		/// <summary>
		/// Process the user pressing the auto hide button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        protected void OnButtonHide(Object sender, EventArgs e)
        {
            // Plain button can still be pressed when disabled, so double check 
            // that an event should actually be generated
            if (_showHideButton && !_ignoreHideButton)
                OnInvertAutoHide();
        }

		/// <summary>
		/// Process one of our buttons getting the focus.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        protected void OnButtonGotFocus(object sender, EventArgs e)
        {
            // Inform parent window we have now got the focus
            if (this.ParentWindow != null)
                this.ParentWindow.WindowDetailGotFocus(this);
        }

		/// <summary>
		/// A double click of the caption causes the content to be restored.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnDoubleClick(EventArgs e)
		{
            // The double click event will cause the control to be destroyed as 
            // the Contents are restored to their alternative positions, so need to
            // double check the control is not already dead
            if (!IsDisposed)
            {
                // Are we currently in a redocking state?
                if (_redocker != null)
                {
                    // No longer need the object
                    _redocker = null;
                }
            }

			// Only restore if floating windows are allowed and allowed to change docking
			if (DockingManager.AllowFloating && DockingManager.AllowRedocking)
			{
				// Fire attached event handlers
				OnRestore();

				// Raise event to indicate a double click occured
				DockingManager.OnDoubleClickDock();
			}
		}

		/// <summary>
		/// Raises the MouseDown event.
		/// </summary>
		/// <param name="e">A MouseEventArgs structure containing event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            // The double click event will cause the control to be destroyed as 
            // the Contents are restored to their alternative positions, so need to
            // double check the control is not already dead
            if (!IsDisposed)
            {
				// Always quit when new another button pressed and already docking
				if (_redocker != null)
				{
					_redocker.QuitTrackingMode(e);

					// No longer need the object
					_redocker = null;
				}
				else
				{
					// Left mouse down begins a redocking action
					if (e.Button == MouseButtons.Left)
					{
						// Is the user allowed to redock?
						if (DockingManager.AllowRedocking)
						{					
							// User allowed redock this particular window?
							if (this.ParentWindow.RedockAllowed)
							{
								_testForDrag = true;
								_mouseDown = PointToScreen(new Point(e.X, e.Y));
							}
						}
					}

					this.Focus();
				}
            }
            
            base.OnMouseDown(e);
        }

		/// <summary>
		/// Raises the MouseMove event.
		/// </summary>
		/// <param name="e">A MouseEventArgs structure containing event data.</param>
		protected override void OnMouseMove(MouseEventArgs e)
        {
            // The double click event will cause the control to be destroyed as 
            // the Contents are restored to their alternative positions, so need to
            // double check the control is not already dead
            if (!IsDisposed)
            {
                // Redocker object needs mouse movements
                if (_redocker != null)
                    _redocker.OnMouseMove(e);
                else
                {
                    // Should we test for the start of dragging?
					if (_testForDrag)
					{
						// Centre the drag test at mouse down point
						Rectangle dragRect = new Rectangle(_mouseDown, Size.Empty);
						
						// Expand by drag rectangle size
						dragRect.Inflate(SystemInformation.DragSize);
						dragRect.Inflate(SystemInformation.DragSize);
					
						// If the mouse has gone outside the drag test
						if (!dragRect.Contains(PointToScreen(new Point(e.X, e.Y))))
						{
							WindowContent wc = this.ParentWindow as WindowContent;

							// Is our parent a WindowContent instance?				
							if (wc != null)
							{
								// Start redocking activity for the whole WindowContent
								_redocker = Redocker.CreateRedocker(DockingManager.FeedbackStyle, 
																	this, wc, new Point(e.X, e.Y));
																	
								// After the redocker is create, do not test for it again
								_testForDrag = false;
							}
						}
					}
                }   
            }

            base.OnMouseMove(e);
        }

		/// <summary>
		/// Raises the MouseUp event.
		/// </summary>
		/// <param name="e">A MouseEventArgs structure containing event data.</param>
		protected override void OnMouseUp(MouseEventArgs e)
        {
			// The double click event will cause the control to be destroyed as 
            // the Contents are restored to their alternative positions, so need to
            // double check the control is not already dead
            if (!IsDisposed)
            {
                // Are we currently in a redocking state?
                if (_redocker != null)
                {
                    // Let the redocker finish off
                    _redocker.OnMouseUp(e);

                    // No longer need the object
                    _redocker = null;
				}

                // Right mouse button can generate a Context event
                if (e.Button == MouseButtons.Right)
                {
					// Get screen coordinates of the mouse
					Point pt = this.PointToScreen(new Point(e.X, e.Y));
    				
					// Box to transfer as parameter
					OnContext(_wc.Contents, pt);
                }
                
                // After the mouse is released, cannot start a drag operation
                _testForDrag = false;
            }
            
            base.OnMouseUp(e);
        }

		/// <summary>
		/// Raises the Resize event.
		/// </summary>
		/// <param name="e">A MouseEventArgs structure containing event data.</param>
        protected override void OnResize(EventArgs e)
        {
            // Any resize of control should redraw all of it
            Invalidate();
            base.OnResize(e);
        }

		/// <summary>
		/// Calculate which button recolors should be remapped.
		/// </summary>
        protected virtual void DefineButtonRemapping() {}

		/// <summary>
		/// Process the maximize interface being added.
		/// </summary>
        protected virtual void OnAddMaximizeInterface() {}

		/// <summary>
		/// Process the maximize interface being removed.
		/// </summary>
        protected virtual void OnRemoveMaximizeInterface() {}

		/// <summary>
		/// Update the appearance of the maximize image.
		/// </summary>
        protected virtual void UpdateMaximizeImage() {}

		/// <summary>
		/// Update the appearance of the auto hide image.
		/// </summary>
        protected virtual void UpdateAutoHideImage() {}

		/// <summary>
		/// Calculate the visibility and position of buttons.
		/// </summary>
        protected virtual void RecalculateButtons() {}
        
		/// <summary>
		/// Create button required for caption.
		/// </summary>
        protected virtual void CreateButtons() 
        {
			// Attach events to button
            if (_closeButton != null)
            {
                _closeButton.Click += new EventHandler(OnButtonClose);
                _closeButton.GotFocus += new EventHandler(OnButtonGotFocus);
            }

            if (_hideButton != null)
            {
                _hideButton.Click += new EventHandler(OnButtonHide);
                _hideButton.GotFocus += new EventHandler(OnButtonGotFocus);
            }
        }

		/// <summary>
		/// Let derived classes image specific images.
		/// </summary>
		protected abstract ImageList Images { get; }

		/// <summary>
		/// Filters out a message before it is dispatched.
		/// </summary>
		/// <param name="m">The message to be dispatched. You cannot modify this message. </param>
		/// <returns>true to filter out; false otherwise.</returns>
        public bool PreFilterMessage(ref Message m)
        {
            // Has a key been pressed?
            if (m.Msg == (int)Win32.Msgs.WM_KEYDOWN)
            {
                // Is it the ESCAPE key?
                if ((int)m.WParam == (int)Win32.VirtualKeys.VK_ESCAPE)
                {                   
                    // Are we in redocking mode?
                    if (_redocker != null)
                    {
                        // Cancel the redocking activity
                        _redocker.QuitTrackingMode(null);

                        // No longer need the object
                        _redocker = null;
                        
                        return true;
                    }
                }
            }
            
            return false;
        }
    }

	/// <summary>
	/// Window caption detail implementation for Plain visual style.
	/// </summary>
    [ToolboxItem(false)]
    public class WindowDetailCaptionPlain : WindowDetailCaption
    {
        private enum ImageIndex
        {
            Close					= 0,
            EnabledHorizontalMax	= 1,
            EnabledHorizontalMin	= 2,
            EnabledVerticalMax		= 3,
            EnabledVerticalMin		= 4, 
            AutoHide		        = 5, 
            AutoShow		        = 6 
        }

        // Class constants
        private const int _inset = 3;
        private const int _offset = 5;
        private const int _fixedLength = 14;
        private const int _imageWidth = 10;
        private const int _imageHeight = 10;
        private const int _buttonWidth = 12;
        private const int _buttonHeight = 12;
        private const int _insetButton = 2;

		// Class fields
		private static ImageList _imagesPlain;

        // Instance fields
        private bool _dockLeft;
        private int _buttonOffset;

		static WindowDetailCaptionPlain()
		{
            // Create a strip of images by loading an embedded bitmap resource
            _imagesPlain = ResourceHelper.LoadBitmapStrip(Type.GetType("Crownwood.DotNetMagic.Docking.WindowDetailCaptionPlain"),
                                                         "Crownwood.DotNetMagic.Resources.ImagesCaptionPlain.bmp",
                                                         new Size(_imageWidth, _imageHeight),
                                                         new Point(0,0));
		}

		/// <summary>
		/// Initializes a new instance of the WindowDetailCaptionPlain class.
		/// </summary>
		/// <param name="manager">Reference to parent docking manager.</param>
        /// <param name="wc">Reference to owning window content.</param>
        /// <param name="closeHandler">Delegate for notifying close events.</param>
		/// <param name="restoreHandler">Delegate for notifying restore events.</param>
		/// <param name="invertAutoHideHandler">Delegate for auto hide invert events.</param>
		/// <param name="contextHandler">Delegate for notifying context menu events.</param>
        public WindowDetailCaptionPlain(DockingManager manager, 
                                        WindowContent wc,
                                        EventHandler closeHandler, 
                                        EventHandler restoreHandler, 
                                        EventHandler invertAutoHideHandler, 
                                        ContextHandler contextHandler)
            : base(manager, wc,
                   new Size(_fixedLength, _fixedLength), 
                   closeHandler, 
                   restoreHandler, 
                   invertAutoHideHandler, 
                   contextHandler)
        {        
            // Default to thinking we are docked on a left edge
            _dockLeft = false;

            // Modify painting to prevent overwriting the button control
            _buttonOffset = 1 + (_buttonWidth + _insetButton) * 2;
        }
        
		/// <summary>
		/// Handle a change in the state of the parent window.
		/// </summary>
		/// <param name="newState">New value of the property.</param>
        public override void ParentStateChanged(State newState)
        {
            // Should we dock to the left or top of our container?
            switch(newState)
            {
                case State.DockTop:
                case State.DockBottom:
					if (DockingManager.AllowSideCaptions)
						_dockLeft = true;
					else
						_dockLeft = false;
					break;
                case State.Floating:
                case State.DockLeft:
                case State.DockRight:
                    _dockLeft = false;
                    break;
            }

            // Ignore the AutoHide feature when in floating form
            _ignoreHideButton = (ParentWindow.State == State.Floating);

            RecalculateButtons();
        }

		/// <summary>
		/// Handle the detail being removed from the parent window.
		/// </summary>
		/// <param name="parent">Window parent we are removed from.</param>
		public override void RemovedFromParent(Window parent)
        {
            if (parent != null)
            {
                if (this.Dock != DockStyle.None)
                {
                    Size minSize = parent.MinimalSize;

                    if (this.Dock == DockStyle.Left)
                    {
                        // Remove our width from the minimum size of the parent
                        minSize.Width -= _fixedLength;
                    }
                    else
                    {
                        // Remove our height from the minimum size of the parent
                        minSize.Height -= _fixedLength;
                    }

                    parent.MinimalSize = minSize;
                }
            }
        }

		/// <summary>
		/// Handle the detail being added to a parent window.
		/// </summary>
		/// <param name="parent">Window parent we are added to.</param>
		public override void AddedToParent(Window parent)
        {
            if (parent != null)
            {
                if (this.Dock != DockStyle.None)
                {
                    Size minSize = parent.MinimalSize;

                    if (this.Dock == DockStyle.Left)
                    {
                        // Add our width from the minimum size of the parent
                        minSize.Width += _fixedLength;
                    }
                    else
                    {
                        // Add our height from the minimum size of the parent
                        minSize.Height += _fixedLength;
                    }

                    parent.MinimalSize = minSize;
                }
            }
        }
        
		/// <summary>
		/// Let derived classes image specific images.
		/// </summary>
		protected override ImageList Images
		{ 
			get { return _imagesPlain; }
		}

		/// <summary>
		/// Calculate which button recolors should be remapped.
		/// </summary>
		protected override void DefineButtonRemapping()
        {
            // Define use of current system colors
            ColorMap activeMap = new ColorMap();
            ColorMap inactiveMap = new ColorMap();
			
            activeMap.OldColor = Color.Black;
            activeMap.NewColor = DockingManager.InactiveTextColor;
            inactiveMap.OldColor = Color.Black;
            inactiveMap.NewColor = Color.FromArgb(128, DockingManager.InactiveTextColor);

            // Create remap attributes for use by button
            _activeAttr.SetRemapTable(new ColorMap[]{activeMap}, ColorAdjustType.Bitmap);
            _inactiveAttr.SetRemapTable(new ColorMap[]{inactiveMap}, ColorAdjustType.Bitmap);
        }

		/// <summary>
		/// Process the maximize interface being added.
		/// </summary>
		protected override void OnAddMaximizeInterface()
        {
            if (_maxButton != null)
            {
                _maxButton.Size = new Size(_buttonWidth, _buttonHeight);
				_maxButton.Style = VisualStyle.Plain;

                // Define the fixed remapping
                _maxButton.ImageAttributes = _inactiveAttr;
                
                // Reduce the lines drawing
                _buttonOffset += (_buttonWidth + _insetButton);
            }
        }

		/// <summary>
		/// Process the maximize interface being removed.
		/// </summary>
		protected override void OnRemoveMaximizeInterface()
        {
            // Reduce the lines drawing
            _buttonOffset -= (_buttonWidth + _insetButton);
        }

		/// <summary>
		/// Create button required for caption.
		/// </summary>
		protected override void CreateButtons()
        {
			// Define the ImageList and which ImageIndex to show initially
			_closeButton = new ButtonWithStyle();
			_closeButton.Image = Images.Images[(int)ImageIndex.Close];
			_closeButton.Style = VisualStyle.Plain;
			
			_hideButton = new ButtonWithStyle();
			_hideButton.Image = Images.Images[(int)ImageIndex.AutoHide];
			_hideButton.Style = VisualStyle.Plain;
			
			_closeButton.Size = new Size(_buttonWidth, _buttonHeight);
			_hideButton.Size = new Size(_buttonWidth, _buttonHeight);

			// Define the fixed remapping
			_closeButton.ImageAttributes = _activeAttr;
			_hideButton.ImageAttributes = _activeAttr;

			// Add to the display
			Controls.Add(_closeButton);
			Controls.Add(_hideButton);
            
            // Let base class perform common actions
            base.CreateButtons();
        }

		/// <summary>
		/// Update the appearance of the auto hide image.
		/// </summary>
		protected override void UpdateAutoHideImage()
        {
            if (_pinnedImage)
                _hideButton.Image = Images.Images[(int)ImageIndex.AutoShow];
            else
                _hideButton.Image= Images.Images[(int)ImageIndex.AutoHide];
        }
        
		/// <summary>
		/// Update the appearance of the maximize image.
		/// </summary>
		protected override void UpdateMaximizeImage()
        {
            if (_showCloseButton)
                _closeButton.ImageAttributes = _activeAttr;
            else    
                _closeButton.ImageAttributes = _inactiveAttr;

            if (_showHideButton && !_ignoreHideButton)
                _hideButton.ImageAttributes = _activeAttr;
            else
                _hideButton.ImageAttributes = _inactiveAttr;

            if (_maxButton != null)
            {
                if (_maxInterface.IsMaximizeAvailable())
                    _maxButton.ImageAttributes = _activeAttr;
                else
                    _maxButton.ImageAttributes = _inactiveAttr;

                bool maximized = _maxInterface.IsWindowMaximized(this.ParentWindow);

                if (_maxInterface.Direction == LayoutDirection.Vertical)
                {
                    if (maximized)
                        _maxButton.Image = Images.Images[(int)ImageIndex.EnabledVerticalMin];	
                    else
                        _maxButton.Image = Images.Images[(int)ImageIndex.EnabledVerticalMax];	
                }
                else
                {
                    if (maximized)
                        _maxButton.Image = Images.Images[(int)ImageIndex.EnabledHorizontalMin];	
                    else
                        _maxButton.Image = Images.Images[(int)ImageIndex.EnabledHorizontalMax];	
                }
            }
        }

		/// <summary>
		/// Calculate the visibility and position of buttons.
		/// </summary>
		protected override void RecalculateButtons()
        {
            if (_dockLeft)
            {
                if (this.Dock != DockStyle.Left)
                {
                    RemovedFromParent(this.ParentWindow);
                    this.Dock = DockStyle.Left;
                    AddedToParent(this.ParentWindow);
                }

                int iStart = _inset;

                // Button position is fixed, regardless of our size
                _closeButton.Location = new Point(_insetButton, iStart);
                _closeButton.Anchor = AnchorStyles.Top;
                _closeButton.Show();
                iStart += _buttonHeight + _insetButton;
                
                // Button position is fixed, regardless of our size
                _hideButton.Location = new Point(_insetButton, iStart);
                _hideButton.Anchor = AnchorStyles.Top;
                _hideButton.Show();
                iStart += _buttonHeight + _insetButton;

                if (_maxButton != null)
                {
                    // Button position is fixed, regardless of our size
                    _maxButton.Location = new Point(_insetButton, iStart);
                    _maxButton.Anchor = AnchorStyles.Top;
                }
            }
            else
            {
                if (this.Dock != DockStyle.Top)
                {
                    RemovedFromParent(this.ParentWindow);
                    this.Dock = DockStyle.Top;
                    AddedToParent(this.ParentWindow);
                }

                Size client = this.ClientSize;
                int iStart = _inset;

                // Button is positioned to right hand side of bar
                _closeButton.Location = new Point(client.Width - iStart - _buttonWidth, _insetButton);
                _closeButton.Anchor = AnchorStyles.Right;
                _closeButton.Show();
                iStart += _buttonWidth + _insetButton;
                
                // Next the AutoHide button is positioned
                _hideButton.Location = new Point(client.Width - iStart - _buttonWidth, _insetButton);
                _hideButton.Anchor = AnchorStyles.Right;
                _hideButton.Show();
                iStart += _buttonWidth + _insetButton;

                if (_maxButton != null)
                {
                    // Button position is fixed, regardless of our size
                    _maxButton.Location = new Point(client.Width - iStart - _buttonWidth, _insetButton);
                    _maxButton.Anchor = AnchorStyles.Right;
                }
            }

            UpdateMaximizeImage();
        }

		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data. </param>
		protected override void OnPaint(PaintEventArgs e)
        {
            Size ourSize = this.ClientSize;
            Point[] light = new Point[4];
            Point[] dark = new Point[4];
				
            // Depends on orientation
            if (_dockLeft)
            {
                int iBottom = ourSize.Height - _inset - 1;
                int iRight = _offset + 2;
                int iTop = _inset + _buttonOffset;

                light[3].X = light[2].X = light[0].X = _offset;
                light[2].Y = light[1].Y = light[0].Y = iTop;
                light[1].X = _offset + 1;
                light[3].Y = iBottom;
			
                dark[2].X = dark[1].X = dark[0].X = iRight;
                dark[3].Y = dark[2].Y = dark[1].Y = iBottom;
                dark[0].Y = iTop;
                dark[3].X = iRight - 1;
            }
            else
            {
                int iBottom = _offset + 2;
                int iRight = ourSize.Width - (_inset + _buttonOffset);
				
                light[3].X = light[2].X = light[0].X = _inset;
                light[1].Y = light[2].Y = light[0].Y = _offset;
                light[1].X = iRight;
                light[3].Y = _offset + 1;
			
                dark[2].X = dark[1].X = dark[0].X = iRight;
                dark[3].Y = dark[2].Y = dark[1].Y = iBottom;
                dark[0].Y = _offset;
                dark[3].X = _inset;
            }

            using (Pen lightPen = new Pen(ControlPaint.LightLight(DockingManager.BackColor)),
                       darkPen = new Pen(ControlPaint.Dark(DockingManager.BackColor)))
            {
                e.Graphics.DrawLine(lightPen, light[0], light[1]);
                e.Graphics.DrawLine(lightPen, light[2], light[3]);
                e.Graphics.DrawLine(darkPen, dark[0], dark[1]);
                e.Graphics.DrawLine(darkPen, dark[2], dark[3]);

                // Shift coordinates to draw section grab bar
                if (_dockLeft)
                {
                    for(int i=0; i<4; i++)
                    {
                        light[i].X += 4;
                        dark[i].X += 4;
                    }
                }
                else
                {
                    for(int i=0; i<4; i++)
                    {
                        light[i].Y += 4;
                        dark[i].Y += 4;
                    }
                }

                e.Graphics.DrawLine(lightPen, light[0], light[1]);
                e.Graphics.DrawLine(lightPen, light[2], light[3]);
                e.Graphics.DrawLine(darkPen, dark[0], dark[1]);
                e.Graphics.DrawLine(darkPen, dark[2], dark[3]);
            }
        }
    }

	/// <summary>
	/// Window caption detail implementation for IDE visual style.
	/// </summary>
    public class WindowDetailCaptionIDE : WindowDetailCaption
    {
        private enum ImageIndex
        {
            Close					= 0,
            EnabledVerticalMax		= 1,
            EnabledVerticalMin		= 2,
            AutoHide		        = 3, 
            AutoShow		        = 4 
        }

        // Class constants
        private const int _imageWidth = 12;
        private const int _imageHeight = 11;
        private const int _buttonWidth = 14;
        private const int _buttonHeight = 13;

        // Class fields
		private static ImageList _imagesIDE;
        private static int _fixedLength;

		// Instance fields
        private bool _dockLeft;
        
		static WindowDetailCaptionIDE()
		{
            // Create a strip of images by loading an embedded bitmap resource
            _imagesIDE = ResourceHelper.LoadBitmapStrip(Type.GetType("Crownwood.DotNetMagic.Docking.WindowDetailCaptionIDE"),
                                                        "Crownwood.DotNetMagic.Resources.ImagesCaptionIDE.bmp",
                                                        new Size(_imageWidth, _imageHeight),
                                                        new Point(0,0));

		}

		/// <summary>
		/// Initializes a new instance of the WindowDetailCaptionIDE class.
		/// </summary>
		/// <param name="manager">Reference to parent docking manager.</param>
        /// <param name="wc">Reference to owning window content.</param>
        /// <param name="closeHandler">Delegate for notifying close events.</param>
		/// <param name="restoreHandler">Delegate for notifying restore events.</param>
		/// <param name="invertAutoHideHandler">Delegate for auto hide invert events.</param>
		/// <param name="contextHandler">Delegate for notifying context menu events.</param>
        public WindowDetailCaptionIDE(DockingManager manager,
                                      WindowContent wc,
                                      EventHandler closeHandler, 
                                      EventHandler restoreHandler, 
                                      EventHandler invertAutoHideHandler, 
                                      ContextHandler contextHandler)
            : base(manager, wc,
                   new Size(_fixedLength, _fixedLength), 
                   closeHandler, 
                   restoreHandler, 
                   invertAutoHideHandler,
                   contextHandler)
        {
            // Default to thinking we are docked on a left edge
            DockLeft = false;

			// Use specificed font in the caption 
            UpdateCaptionHeight(manager.CaptionFont);
        }

		/// <summary>
		/// Is the heading docked to the left.
		/// </summary>
		public bool DockLeft
		{
			get { return _dockLeft; }
			set { _dockLeft = value; }
		}

		/// <summary>
		/// Gets the vertical inset height.
		/// </summary>
		public virtual int YInset
		{
			get { return 3; }
		}

		/// <summary>
		/// Gets the extra space needed for the caption height.
		/// </summary>
		public virtual int YInsetExtra
		{
			get { return 3; }
		}

		/// <summary>
		/// Gets the space between buttons.
		/// </summary>
		public virtual int ButtonSpacer
		{
			get { return 3; }
		}

		/// <summary>
		/// Gets the extra positioning space.
		/// </summary>
		public virtual int LocationExtra
		{
			get { return 0; }
		}

		/// <summary>
		/// Propogate change in a property.
		/// </summary>
		/// <param name="name">Property that has changed.</param>
		/// <param name="value">New value.</param>
        public override void PropogateNameValue(PropogateName name, object value)
        {
            base.PropogateNameValue(name, value);
        
            switch(name)
            {
                case PropogateName.CaptionFont:
                    UpdateCaptionHeight((Font)value);    
                    break;
                case PropogateName.ActiveTextColor:
                case PropogateName.InactiveTextColor:
                    DefineButtonRemapping();
                    Invalidate();
                    break;
            }
        }
        
		/// <summary>
		/// Handle the parent window getting focus.
		/// </summary>
		public override void WindowGotFocus()
        {
            SetButtonState();
            Invalidate();
        }

		/// <summary>
		/// Handle the parent window losing focus.
		/// </summary>
		public override void WindowLostFocus()
        {
            SetButtonState();
            Invalidate();
		}
      
		/// <summary>
		/// Handle a change in the caption bar text.
		/// </summary>
		/// <param name="title">New value of the property.</param>
		public override void NotifyFullTitleText(string title)
        {
            this.Text = title;
            Invalidate();
        }

		/// <summary>
		/// Handle a change in the state of the parent window.
		/// </summary>
		/// <param name="newState">New value of the property.</param>
		public override void ParentStateChanged(State newState)
        { 
            // Should we dock to the left or top of our container?
            switch(newState)
            {
                case State.DockTop:
                case State.DockBottom:
					if (DockingManager.AllowSideCaptions)
					{
						DockLeft = true;
			            this.Dock = DockStyle.Left;
			        }
			        else
			        {
						DockLeft = false;
						this.Dock = DockStyle.Top;
					}
                    break;
                case State.Floating:
                case State.DockLeft:
                case State.DockRight:
                    DockLeft = false;
		            this.Dock = DockStyle.Top;
                    break;
            }
			
			// Ignore the AutoHide feature when in floating form
            _ignoreHideButton = (ParentWindow.State == State.Floating);

            RecalculateButtons();
            Invalidate();
        }

		/// <summary>
		/// Handle the detail being removed from the parent window.
		/// </summary>
		/// <param name="parent">Window parent we are removed from.</param>
		public override void RemovedFromParent(Window parent)
        {
            if (parent != null)
            {
                if (this.Dock != DockStyle.None)
                {
                    Size minSize = parent.MinimalSize;

                    if (this.Dock == DockStyle.Left)
                    {
                        // Remove our width from the minimum size of the parent
                        minSize.Width -= _fixedLength;
                    }
                    else
                    {
                        // Remove our height from the minimum size of the parent
                        minSize.Height -= _fixedLength;
                    }

                    parent.MinimalSize = minSize;
                }
            }
        }

		/// <summary>
		/// Let derived classes image specific images.
		/// </summary>
		protected override ImageList Images 
		{ 
			get { return _imagesIDE; }
		}

		/// <summary>
		/// Calculate which button recolors should be remapped.
		/// </summary>
		protected override void DefineButtonRemapping()
        {
            // Define use of current system colors
            ColorMap activeMap = new ColorMap();
            ColorMap inactiveMap = new ColorMap();
			
            activeMap.OldColor = Color.Black;
            activeMap.NewColor = DockingManager.ActiveTextColor;
            inactiveMap.OldColor = Color.Black;
            inactiveMap.NewColor = DockingManager.InactiveTextColor;

            // Create remap attributes for use by button
            _activeAttr.SetRemapTable(new ColorMap[]{activeMap}, ColorAdjustType.Bitmap);
            _inactiveAttr.SetRemapTable(new ColorMap[]{inactiveMap}, ColorAdjustType.Bitmap);
        }

		/// <summary>
		/// Handle the detail being added to a parent window.
		/// </summary>
		/// <param name="parent">Window parent we are added to.</param>
		public override void AddedToParent(Window parent)
        {
            if (parent != null)
            {
                if (this.Dock != DockStyle.None)
                {
                    Size minSize = parent.MinimalSize;

                    if (this.Dock == DockStyle.Left)
                    {
                        // Add our width from the minimum size of the parent
                        minSize.Width += _fixedLength;
                    }
                    else
                    {
                        // Add our height from the minimum size of the parent
                        minSize.Height += _fixedLength;
                    }

                    parent.MinimalSize = minSize;
                }
            }
        }

		/// <summary>
		/// Process the maximize interface being added.
		/// </summary>
		protected override void OnAddMaximizeInterface()
        {
            if (_maxButton != null)
            {
                // Set the correct size for the button
                _maxButton.Size = new Size(_buttonWidth, _buttonHeight);
				_maxButton.Style = VisualStyle.Plain;

                // Define correct color setup
                _maxButton.BackColor = this.BackColor;
                _maxButton.ImageAttributes = _inactiveAttr;

                // Force the ImageAttribute for the button to be set
                SetButtonState();
                Invalidate();
			}
        }

		/// <summary>
		/// Update the appearance of the auto hide image.
		/// </summary>
		protected override void UpdateAutoHideImage()
        {
            if (_pinnedImage)
                _hideButton.Image = Images.Images[(int)ImageIndex.AutoShow];
            else
                _hideButton.Image = Images.Images[(int)ImageIndex.AutoHide];
        }

		/// <summary>
		/// Update the appearance of the maximize image.
		/// </summary>
		protected override void UpdateMaximizeImage()
        {
            if ((_maxButton != null) && (_maxInterface != null))
            {
                bool enabled = _maxInterface.IsMaximizeAvailable();

                if (!enabled)
                {
                    if (_maxButton.Visible)
                        _maxButton.Hide();
                }
                else
                {
                    bool maximized = _maxInterface.IsWindowMaximized(this.ParentWindow);

                    if (!_maxButton.Visible)
                        _maxButton.Show();

                    if (maximized)
                        _maxButton.Image = Images.Images[(int)ImageIndex.EnabledVerticalMin];	
                    else
                        _maxButton.Image = Images.Images[(int)ImageIndex.EnabledVerticalMax];	
                }
            }
        }

		/// <summary>
		/// Update the state of the caption buttons.
		/// </summary>
        protected virtual void SetButtonState()
        {
            if (this.ParentWindow != null)
            {
                if (this.ParentWindow.ContainsFocus)
                {
                    if (_closeButton.BackColor != DockingManager.ActiveColor)
                    {
                        _closeButton.BackColor = DockingManager.ActiveColor;
                        _closeButton.ImageAttributes = _activeAttr;
                        _closeButton.Invalidate();
                    }

                    if (_hideButton != null)
                    {
                        if (_hideButton.BackColor != DockingManager.ActiveColor)
                        {
                            _hideButton.BackColor = DockingManager.ActiveColor;
                            _hideButton.ImageAttributes = _activeAttr;
                            _hideButton.Invalidate();
                        }
                    }

                    if (_maxButton != null)
                    {
                        if (_maxButton.BackColor != DockingManager.ActiveColor)
                        {
                            _maxButton.BackColor = DockingManager.ActiveColor;
                            _maxButton.ImageAttributes = _activeAttr;
                            _maxButton.Invalidate();
                        }
                    }
                }
                else
                {
                    if (_closeButton.BackColor != this.BackColor)
                    {
                        _closeButton.BackColor = this.BackColor;
                        _closeButton.ImageAttributes = _inactiveAttr;
                        _closeButton.Invalidate();
                    }

                    if (_hideButton != null)
                    {
                        if (_hideButton.BackColor != this.BackColor)
                        {
                            _hideButton.BackColor = this.BackColor;
                            _hideButton.ImageAttributes = _inactiveAttr;
                            _hideButton.Invalidate();
                        }
                    }

                    if (_maxButton != null)
                    {
                        if (_maxButton.BackColor != this.BackColor)
                        {
                            _maxButton.BackColor = this.BackColor;
                            _maxButton.ImageAttributes = _inactiveAttr;
                            _maxButton.Invalidate();
                        }
                    }
                }
            }

            Invalidate();
        }

		/// <summary>
		/// Calculate the visibility and position of buttons.
		/// </summary>
		protected override void RecalculateButtons()
        {
			if (DockLeft)
			{
				int buttonX = LocationExtra + (_fixedLength - YInset * 2 - _buttonWidth) / 2 + YInset;
				int buttonY = ButtonSpacer;
	        
				if (_showCloseButton)
				{
					// Button position is fixed, regardless of our size
					_closeButton.SetBounds(buttonX, buttonY, _buttonWidth, _buttonHeight);
	    		
					// Let the location of the control be updated for us
					_closeButton.Anchor = AnchorStyles.Top;

					// Just in case currently hidden
					_closeButton.Show();
	                
					// Define start of next button
					buttonY += _buttonHeight;
				}
				else
					_closeButton.Hide();
	                        
				if (_showHideButton && !_ignoreHideButton)
				{
					// Button position is fixed, regardless of our size
					_hideButton.SetBounds(buttonX, buttonY, _buttonWidth, _buttonHeight);
				
					// Let the location of the control be updated for us
					_hideButton.Anchor = AnchorStyles.Top;

					// Just in case currently hidden
					_hideButton.Show();

					// Define start of next button
					buttonY += _buttonHeight;
	                
					UpdateAutoHideImage();
				}
				else
					_hideButton.Hide();
	            
				if (_maxButton != null)
				{
					// Button position is fixed, regardless of our size
					_maxButton.SetBounds(buttonX, buttonY, _buttonWidth, _buttonHeight);
	                
					// Let the location of the control be updated for us
					_maxButton.Anchor = AnchorStyles.Top;

					// Define start of next button
					buttonY += _buttonHeight;

					UpdateMaximizeImage();
				}
			}
			else
			{
				int buttonX = this.Width - _buttonWidth - ButtonSpacer;
				int buttonY = LocationExtra + (_fixedLength - YInset * 2 - _buttonHeight) / 2 + YInset;
	        
				if (_showCloseButton)
				{
					// Button position is fixed, regardless of our size
					_closeButton.SetBounds(buttonX, buttonY, _buttonWidth, _buttonHeight);
	    		
					// Let the location of the control be updated for us
					_closeButton.Anchor = AnchorStyles.Right;

					// Just in case currently hidden
					_closeButton.Show();
	                
					// Define start of next button
					buttonX -= _buttonWidth;
				}
				else
					_closeButton.Hide();
	                        
				if (_showHideButton && !_ignoreHideButton)
				{
					// Button position is fixed, regardless of our size
					_hideButton.SetBounds(buttonX, buttonY, _buttonWidth, _buttonHeight);
				
					// Let the location of the control be updated for us
					_hideButton.Anchor = AnchorStyles.Right;

					// Just in case currently hidden
					_hideButton.Show();

					// Define start of next button
					buttonX -= _buttonWidth;
	                
					UpdateAutoHideImage();
				}
				else
					_hideButton.Hide();
	            
				if (_maxButton != null)
				{
					// Button position is fixed, regardless of our size
					_maxButton.SetBounds(buttonX, buttonY, _buttonWidth, _buttonHeight);
	                
					// Let the location of the control be updated for us
					_maxButton.Anchor = AnchorStyles.Right;

					// Define start of next button
					buttonX -= _buttonWidth;

					UpdateMaximizeImage();
				}
			}

            Invalidate();
        }

		/// <summary>
		/// Create button required for caption.
		/// </summary>
		protected override void CreateButtons()
        {
			// Define the ImageList and which ImageIndex to show initially
			if (_closeButton == null)
			{
				_closeButton = new ButtonWithStyle();
				_closeButton.BackColor = this.BackColor;
			}

			_closeButton.Image = Images.Images[(int)ImageIndex.Close];
			_closeButton.Style = VisualStyle.Plain;
	        
			if (_hideButton == null)
			{
				_hideButton = new ButtonWithStyle();
				_hideButton.BackColor = this.BackColor;
			}

			_hideButton.Image = Images.Images[(int)ImageIndex.AutoHide];
			_hideButton.Style = VisualStyle.Plain;
			
			_closeButton.Size = new Size(_buttonWidth, _buttonHeight);
			_hideButton.Size = new Size(_buttonWidth, _buttonHeight);

			// Let the location of the control be updated for us
			_closeButton.Anchor = AnchorStyles.Right;
			_hideButton.Anchor = AnchorStyles.Right;

			// Define the button position relative to the size set in the constructor
			_closeButton.Location = new Point(_fixedLength - _buttonWidth - ButtonSpacer, 
											  (_fixedLength - YInset * 2 - _buttonHeight) / 2 + YInset);
	        
			_hideButton.Location = new Point(_fixedLength - (_buttonWidth - ButtonSpacer) * 2, 
											  (_fixedLength - YInset * 2 - _buttonHeight) / 2 + YInset);

			// Define correct color setup
			_closeButton.ImageAttributes = _inactiveAttr;
			_hideButton.ImageAttributes = _inactiveAttr;

			// Add to the display
			Controls.Add(_closeButton);
			Controls.Add(_hideButton);

            // Let base class perform common actions
            base.CreateButtons();
        }

		/// <summary>
		/// Adjust the height of the caption to reflect changed font.
		/// </summary>
		/// <param name="captionFont"></param>
		protected void UpdateCaptionHeight(Font captionFont)
		{
            // Dynamically calculate the required height of the caption area
            _fixedLength = (int)captionFont.GetHeight() + (YInset + YInsetExtra) * 2;
    
            int minHeight = (DockLeft ? (_buttonWidth + (YInset + YInsetExtra) * 2 + 1) :
										(_buttonHeight + (YInset + YInsetExtra) * 2 + 1));

            // Maintain a minimum height to allow correct showing of buttons
            if (_fixedLength < minHeight)
                _fixedLength = minHeight;

			this.Size = new Size(_fixedLength, _fixedLength);
			
			RecalculateButtons();

			Invalidate();
		}

		/// <summary>
		/// Paints the background of the control.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains information about the control to paint.</param>
		protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Overriden to prevent background being painted
        }

		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data. </param>
		protected override void OnPaint(PaintEventArgs e)
        {
            bool focused = false;

            if (this.ParentWindow != null)
                focused = this.ParentWindow.ContainsFocus;
			
			// Sometimes the min/max button is created and then an attempt is made to 
            // hide the button. But for some reason it does not always manage to hide 
            // the button. So forced to check here everytime to ensure its hidden.
            UpdateMaximizeImage();

            SetButtonState();

			Size ourSize = this.ClientSize;

            // Depends on orientation
            if (DockLeft)
			{
                int yEnd = ourSize.Height - ButtonSpacer;
				int xEnd = ourSize.Width - YInset * 2;

				Rectangle rectCaption = new Rectangle(YInset, 0, xEnd - YInset + 1, yEnd);

				using(SolidBrush backBrush = new SolidBrush(this.BackColor),
								activeBrush = new SolidBrush(DockingManager.ActiveColor),
								activeTextBrush = new SolidBrush(DockingManager.ActiveTextColor),
								inactiveBrush = new SolidBrush(DockingManager.InactiveTextColor))
				{
					// Is this control Active?
					if (focused)
					{
						// Fill the entire background area
						e.Graphics.FillRectangle(backBrush, e.ClipRectangle);
	    	
						// Use a solid filled background for text
						e.Graphics.FillRectangle(activeBrush, rectCaption);
	    			
						// Start drawing text a little from the left
						rectCaption.Height -= ButtonSpacer;
						rectCaption.X += 1;
						rectCaption.Width -= 2;

                        if (_showCloseButton)
                        {
					        rectCaption.Y += _closeButton.Width + ButtonSpacer;
					        rectCaption.Height -= _closeButton.Width + ButtonSpacer;
                        }

                        if (_showHideButton && !_ignoreHideButton)
                        {
					        rectCaption.Y += _hideButton.Width + ButtonSpacer;
					        rectCaption.Height -= _hideButton.Width + ButtonSpacer;
                        }

                        if ((_maxButton != null) && _maxInterface.IsMaximizeAvailable())
                        {
                            rectCaption.Y += _maxButton.Width + ButtonSpacer;
                            rectCaption.Height -= _maxButton.Width + ButtonSpacer;
                        }

                        using (StringFormat format = new StringFormat(StringFormatFlags.NoClip))
						{
							format.Alignment = (RightToLeft == RightToLeft.Yes ? StringAlignment.Far : StringAlignment.Near);
							format.LineAlignment = StringAlignment.Center;
							format.Trimming	= StringTrimming.EllipsisCharacter;
							format.FormatFlags = StringFormatFlags.NoWrap |
												 StringFormatFlags.DirectionVertical;

							DrawHelper.DrawReverseString(e.Graphics, this.Text, DockingManager.CaptionFont, rectCaption, activeTextBrush, format);
						}
					}
					else
					{
						// Fill the entire background area
						e.Graphics.FillRectangle(backBrush, e.ClipRectangle);
	    	            
						// Inactive and so use a rounded rectangle
						using (Pen dark = new Pen(ControlPaint.LightLight(DockingManager.InactiveTextColor)))
						{
							e.Graphics.DrawLine(dark, YInset, 1, YInset, yEnd - 2);
							e.Graphics.DrawLine(dark, xEnd, 1, xEnd, yEnd - 2);
							e.Graphics.DrawLine(dark, YInset + 1, 0, xEnd - 1, 0);
							e.Graphics.DrawLine(dark, YInset + 1, yEnd - 1, xEnd - 1, yEnd - 1);

							// Start drawing text a little from the left
							rectCaption.Height -= ButtonSpacer;
							rectCaption.X += 1;
							rectCaption.Width -= 2;

                            if (_showCloseButton)
                            {
					            rectCaption.Y += _closeButton.Width + ButtonSpacer;
					            rectCaption.Height -= _closeButton.Width + ButtonSpacer;
                            }

                            if (_showHideButton && !_ignoreHideButton)
                            {
					            rectCaption.Y += _hideButton.Width + ButtonSpacer;
					            rectCaption.Height -= _hideButton.Width + ButtonSpacer;
                            }

                            if ((_maxButton != null) && _maxInterface.IsMaximizeAvailable())
                            {
                                rectCaption.Y += _maxButton.Width + ButtonSpacer;
                                rectCaption.Height -= _maxButton.Width + ButtonSpacer;
                            }

							using(StringFormat format = new StringFormat(StringFormatFlags.NoClip))
							{
                                format.Alignment = (RightToLeft == RightToLeft.Yes ? StringAlignment.Far : StringAlignment.Near);
                                format.LineAlignment = StringAlignment.Center;
								format.Trimming	= StringTrimming.EllipsisCharacter;
								format.FormatFlags = StringFormatFlags.NoWrap |
													 StringFormatFlags.DirectionVertical;
								
								DrawHelper.DrawReverseString(e.Graphics, this.Text, DockingManager.CaptionFont, rectCaption, inactiveBrush, format);
							}
						}
					}	
				}
			}
			else
			{
                int xEnd = ourSize.Width - ButtonSpacer;
				int yEnd = ourSize.Height - YInset * 2;

				Rectangle rectCaption = new Rectangle(0, YInset, xEnd, yEnd - YInset + 1);

				using(SolidBrush backBrush = new SolidBrush(this.BackColor),
								activeBrush = new SolidBrush(DockingManager.ActiveColor),
								activeTextBrush = new SolidBrush(DockingManager.ActiveTextColor),
								inactiveBrush = new SolidBrush(DockingManager.InactiveTextColor))
				{
					// Is this control Active?
					if (focused)
					{
						// Fill the entire background area
						e.Graphics.FillRectangle(backBrush, e.ClipRectangle);
	    	
						// Use a solid filled background for text
						e.Graphics.FillRectangle(activeBrush, rectCaption);
	    			
						// Start drawing text a little from the left
						rectCaption.X += ButtonSpacer;
						rectCaption.Y += 1;
						rectCaption.Height -= 2;

                        if (_showCloseButton)
                            rectCaption.Width -= _closeButton.Width + ButtonSpacer;

                        if (_showHideButton && !_ignoreHideButton)
                            rectCaption.Width -= _hideButton.Width + ButtonSpacer;

                        if ((_maxButton != null) && _maxInterface.IsMaximizeAvailable())
                            rectCaption.Width -= _maxButton.Width + ButtonSpacer;

                        using (StringFormat format = new StringFormat(StringFormatFlags.NoClip))
                        {
                            format.Alignment = (RightToLeft == RightToLeft.Yes ? StringAlignment.Far : StringAlignment.Near);
                            e.Graphics.DrawString(this.Text, DockingManager.CaptionFont, activeTextBrush, rectCaption, format);
                        }
					}
					else
					{
						// Fill the entire background area
						e.Graphics.FillRectangle(backBrush, e.ClipRectangle);
	    	            
						// Inactive and so use a rounded rectangle
						using (Pen dark = new Pen(ControlPaint.LightLight(DockingManager.InactiveTextColor)))
						{
							e.Graphics.DrawLine(dark, 1, YInset, xEnd - 2, YInset);
							e.Graphics.DrawLine(dark, 1, yEnd, xEnd - 2, yEnd);
							e.Graphics.DrawLine(dark, 0, YInset + 1, 0, yEnd - 1);
							e.Graphics.DrawLine(dark, xEnd - 1, YInset + 1, xEnd - 1, yEnd - 1);

							// Start drawing text a little from the left
							rectCaption.X += ButtonSpacer;
							rectCaption.Y += 1;
							rectCaption.Height -= 2;

                            if (_showCloseButton)
                                rectCaption.Width -= _closeButton.Width + ButtonSpacer;

                            if (_showHideButton && !_ignoreHideButton)
                                rectCaption.Width -= _hideButton.Width + ButtonSpacer;

                            if ((_maxButton != null) && _maxInterface.IsMaximizeAvailable())
                                rectCaption.Width -= _maxButton.Width + ButtonSpacer;

							using(StringFormat format = new StringFormat(StringFormatFlags.NoClip))
							{
                                format.Alignment = (RightToLeft == RightToLeft.Yes ? StringAlignment.Far : StringAlignment.Near);
                                format.LineAlignment = StringAlignment.Center;
								format.Trimming	= StringTrimming.EllipsisCharacter;
								format.FormatFlags = StringFormatFlags.NoWrap;
								e.Graphics.DrawString(this.Text, DockingManager.CaptionFont, inactiveBrush, rectCaption, format);
							}
						}
					}	
				}
			}
            
            // Always get the button to repaint as we have painted over their area
            _closeButton.Refresh();
        }				
    }

	/// <summary>
	/// Window caption detail implementation for IDE2005 visual style.
	/// </summary>
    public class WindowDetailCaptionIDE2005 : WindowDetailCaptionIDE
    {
        // Class constants
        private const int _button2005Width = 14;
        private const int _button2005Height = 13;

		/// <summary>
		/// Initializes a new instance of the WindowDetailCaptionIDE2005 class.
		/// </summary>
		/// <param name="manager">Reference to parent docking manager.</param>
        /// <param name="wc">Reference to owning window content.</param>
        /// <param name="closeHandler">Delegate for notifying close events.</param>
		/// <param name="restoreHandler">Delegate for notifying restore events.</param>
		/// <param name="invertAutoHideHandler">Delegate for auto hide invert events.</param>
		/// <param name="contextHandler">Delegate for notifying context menu events.</param>
        public WindowDetailCaptionIDE2005(DockingManager manager,
                                          WindowContent wc,
										  EventHandler closeHandler, 
										  EventHandler restoreHandler, 
										  EventHandler invertAutoHideHandler, 
										  ContextHandler contextHandler)
            : base(manager, wc,
                   closeHandler, 
                   restoreHandler, 
                   invertAutoHideHandler,
                   contextHandler)
		{
		}

		/// <summary>
		/// Gets the vertical inset height.
		/// </summary>
		public override int YInset
		{
			get { return 3; }
		}

		/// <summary>
		/// Gets the extra space needed for the caption height.
		/// </summary>
		public override int YInsetExtra
		{
			get { return 0; }
		}

		/// <summary>
		/// Gets the extra positioning space.
		/// </summary>
		public override int LocationExtra
		{
			get { return 1; }
		}

		/// <summary>
		/// Process the maximize interface being added.
		/// </summary>
		protected override void OnAddMaximizeInterface()
        {
            if (_maxButton != null)
            {
                // Set the correct size for the button
                _maxButton.Size = new Size(_button2005Width, _button2005Height);
				_maxButton.Style = VisualStyle.Plain;

                // Define correct color setup
                _maxButton.ImageAttributes = _inactiveAttr;

                // Force the ImageAttribute for the button to be set
                SetButtonState();
                Invalidate();
			}
        }

		/// <summary>
		/// Create the button class specific to this implementation.
		/// </summary>
		protected override void CreateMaxButton()
		{
			// Create the InertButton
            _maxButton = DockingManager.Factory.CreateIDE2005CaptionButton(this);
			_maxButton.BackColor = Color.Transparent;
		}

		/// <summary>
		/// Create button required for caption.
		/// </summary>
		protected override void CreateButtons()
        {
			// Define the ImageList and which ImageIndex to show initially
            _closeButton = DockingManager.Factory.CreateIDE2005CaptionButton(this);
			_closeButton.BackColor = Color.Transparent;
            _hideButton = DockingManager.Factory.CreateIDE2005CaptionButton(this);
			_hideButton.BackColor = Color.Transparent;

            // Let base class perform common actions
            base.CreateButtons();
        }

		/// <summary>
		/// Update the state of the caption buttons.
		/// </summary>
        protected override void SetButtonState()
		{
            if (this.ParentWindow != null)
            {
                if (this.ParentWindow.ContainsFocus)
                {
                    _closeButton.ImageAttributes = _activeAttr;
                    _closeButton.Invalidate();

                    if (_hideButton != null)
                    {
                        _hideButton.ImageAttributes = _activeAttr;
                        _hideButton.Invalidate();
                    }

                    if (_maxButton != null)
                    {
                        _maxButton.ImageAttributes = _activeAttr;
                        _maxButton.Invalidate();
                    }
                }
                else
                {
                    _closeButton.ImageAttributes = _inactiveAttr;
                    _closeButton.Invalidate();

                    if (_hideButton != null)
                    {
                        _hideButton.ImageAttributes = _inactiveAttr;
                        _hideButton.Invalidate();
                    }

                    if (_maxButton != null)
                    {
                        _maxButton.ImageAttributes = _inactiveAttr;
                        _maxButton.Invalidate();
                    }
                }
            }
		}

		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data. </param>
		protected override void OnPaint(PaintEventArgs e)
        {
            bool focused = false;

            if (this.ParentWindow != null)
                focused = this.ParentWindow.ContainsFocus;
			
			// Sometimes the min/max button is created and then an attempt is made to 
            // hide the button. But for some reason it does not always manage to hide 
            // the button. So forced to check here everytime to ensure its hidden.
            UpdateMaximizeImage();

            SetButtonState();

			using(SolidBrush activeTextBrush = new SolidBrush(DockingManager.ActiveTextColor),
							 inactiveTextBrush = new SolidBrush(DockingManager.InactiveTextColor))
			{
				Color startColor;
				Color endColor;

				// Choose the correct gradient colors for the background
				if (focused)
				{
					Color baseColor = DockingManager.ActiveColor;
					startColor = ControlPaint.Light(SystemColors.Highlight);
					endColor = SystemColors.Highlight;
				}
				else
				{
					startColor = ColorHelper.MergeColors(SystemColors.Control, 0.5f, SystemColors.ControlDark, 0.5f);
					endColor = startColor;
				}

				// Draw as a gradient from left to right
				using(LinearGradientBrush backBrush = new LinearGradientBrush(new Rectangle(-1, -1, Width+1, Height+1), startColor, endColor, (DockLeft ? 0f : 90f)))
					e.Graphics.FillRectangle(backBrush, ClientRectangle);
		    	        
				Rectangle rectCaption = ClientRectangle;

				using(StringFormat format = new StringFormat(StringFormatFlags.NoClip))
				{
                    format.Alignment = (RightToLeft == RightToLeft.Yes ? StringAlignment.Far : StringAlignment.Near);
					format.LineAlignment = StringAlignment.Center;
					format.Trimming	= StringTrimming.EllipsisCharacter;
					format.FormatFlags = StringFormatFlags.NoWrap;

					// Depends on orientation
					if (DockLeft)
					{
						// Start drawing text a little from the left
						rectCaption.Height -= ButtonSpacer;
						rectCaption.X += 1;
						rectCaption.Width -= 2;

						// Reduce the width to account for close button
						rectCaption.Y += _closeButton.Width + ButtonSpacer;
						rectCaption.Height -= _closeButton.Width + ButtonSpacer;

                        if (_showCloseButton)
                        {
                            rectCaption.Y += _closeButton.Width + ButtonSpacer;
                            rectCaption.Height -= _closeButton.Width + ButtonSpacer;
                        }

                        if (_showHideButton && !_ignoreHideButton)
                        {
                            rectCaption.Y += _hideButton.Width + ButtonSpacer;
                            rectCaption.Height -= _hideButton.Width + ButtonSpacer;
                        }

                        if ((_maxButton != null) && _maxInterface.IsMaximizeAvailable())
                        {
                            rectCaption.Y += _maxButton.Width + ButtonSpacer;
                            rectCaption.Height -= _maxButton.Width + ButtonSpacer;
                        }

						// We need to draw in vertical orientation
						format.FormatFlags |= StringFormatFlags.DirectionVertical;

						if (focused)
							DrawHelper.DrawReverseString(e.Graphics, this.Text, DockingManager.CaptionFont, rectCaption, activeTextBrush, format);
						else
							DrawHelper.DrawReverseString(e.Graphics, this.Text, DockingManager.CaptionFont, rectCaption, inactiveTextBrush, format);
					}
					else
					{
						// Start drawing text a little from the left
						rectCaption.X += ButtonSpacer;
						rectCaption.Y += 1;
						rectCaption.Height -= 2;

                        if (_showCloseButton)
                            rectCaption.Width -= _closeButton.Width + ButtonSpacer;

                        if (_showHideButton && !_ignoreHideButton)
                            rectCaption.Width -= _hideButton.Width + ButtonSpacer;

                        if ((_maxButton != null) && _maxInterface.IsMaximizeAvailable())
                            rectCaption.Width -= _maxButton.Width + ButtonSpacer;

						if (focused)
							e.Graphics.DrawString(this.Text, DockingManager.CaptionFont, activeTextBrush, rectCaption, format);
						else
							e.Graphics.DrawString(this.Text, DockingManager.CaptionFont, inactiveTextBrush, rectCaption, format);
					}
				}

				// Draw the border around the three closed edges
				if (DockLeft)
				{
					e.Graphics.DrawLine(SystemPens.ControlDark, 0, 0, Width, 0);
					e.Graphics.DrawLine(SystemPens.ControlDark, 0, 0, 0, Height);
					e.Graphics.DrawLine(SystemPens.ControlDark, 0, Height - 1, Width, Height - 1);
				}
				else
				{
					e.Graphics.DrawLine(SystemPens.ControlDark, 0, 0, Width, 0);
					e.Graphics.DrawLine(SystemPens.ControlDark, 0, 0, 0, Height);
					e.Graphics.DrawLine(SystemPens.ControlDark, Width - 1, 0, Width - 1, Height - 1);
				}
			}
            
            // Always get the button to repaint as we have painted over their area
            _closeButton.Refresh();
        }				
	}

	/// <summary>
	/// Window caption detail implementation for Office2003 visual style.
	/// </summary>
    public class WindowDetailCaptionOffice2003 : WindowDetailCaption
    {
        private enum ImageIndex
        {
            Close					= 0,
            EnabledVerticalMax		= 1,
            EnabledVerticalMin		= 2,
            AutoHide		        = 3, 
            AutoShow		        = 4 
        }

        // Class constants
		private const int _xInset = 3;
		private const int _spotWidth = 6;
		private const int _yInset = 0;
        private const int _yInsetExtra = 5;
        private const int _imageWidth = 12;
        private const int _imageHeight = 11;
        private const int _buttonWidth = 14;
        private const int _buttonHeight = 13;
        private const int _buttonSpacer = 3;

        // Class fields
		private static ImageList _imagesOffice;
        private static int _fixedLength;
        
        // Instance fields
        private bool _dockLeft;
        private ColorDetails _colorDetails;
        
		static WindowDetailCaptionOffice2003()
		{
            // Create a strip of images by loading an embedded bitmap resource
            _imagesOffice = ResourceHelper.LoadBitmapStrip(Type.GetType("Crownwood.DotNetMagic.Docking.WindowDetailCaptionIDE"),
														   "Crownwood.DotNetMagic.Resources.ImagesCaptionOffice2003.bmp",
														   new Size(_imageWidth, _imageHeight),
														   new Point(0,0));
		}

		/// <summary>
		/// Initializes a new instance of the WindowDetailCaptionOffice2003 class.
		/// </summary>
		/// <param name="manager">Reference to parent docking manager.</param>
        /// <param name="wc">Reference to owning window content.</param>
        /// <param name="closeHandler">Delegate for notifying close events.</param>
		/// <param name="restoreHandler">Delegate for notifying restore events.</param>
		/// <param name="invertAutoHideHandler">Delegate for auto hide invert events.</param>
		/// <param name="contextHandler">Delegate for notifying context menu events.</param>
        public WindowDetailCaptionOffice2003(DockingManager manager,
                                             WindowContent wc,
                                             EventHandler closeHandler, 
											 EventHandler restoreHandler, 
											 EventHandler invertAutoHideHandler, 
											 ContextHandler contextHandler)
            : base(manager, wc,
                   new Size(_fixedLength, _fixedLength), 
                   closeHandler, 
                   restoreHandler, 
                   invertAutoHideHandler,
                   contextHandler)
        {
			// Get the color helper for drawing
			_colorDetails = new ColorDetails();
			
            // Default to thinking we are docked on a left edge
            _dockLeft = false;

			// Use specificed font in the caption 
            UpdateCaptionHeight(manager.CaptionFont);

			// We need to know when the system colours have changed
			Microsoft.Win32.SystemEvents.UserPreferenceChanged += 
				new Microsoft.Win32.UserPreferenceChangedEventHandler(OnUserPreferenceChanged); 
		}
        
        /// <summary>
        /// Gets access to the colouring details.
        /// </summary>
		public ColorDetails ColorDetails
		{
			get { return _colorDetails; }
		}

		/// <summary>
		/// Propogate change in a property.
		/// </summary>
		/// <param name="name">Property that has changed.</param>
		/// <param name="value">New value.</param>
        public override void PropogateNameValue(PropogateName name, object value)
        {
            base.PropogateNameValue(name, value);
        
            switch(name)
            {
                case PropogateName.CaptionFont:
                    UpdateCaptionHeight((Font)value);    
                    break;
                case PropogateName.ActiveTextColor:
                case PropogateName.InactiveTextColor:
                    DefineButtonRemapping();
                    Invalidate();
                    break;
            }
        }
        
		/// <summary>
		/// Handle the parent window getting focus.
		/// </summary>
		public override void WindowGotFocus()
        {
            SetButtonState();
            Invalidate();
        }

		/// <summary>
		/// Handle the parent window losing focus.
		/// </summary>
		public override void WindowLostFocus()
        {
            SetButtonState();
            Invalidate();
		}
      
		/// <summary>
		/// Handle a change in the caption bar text.
		/// </summary>
		/// <param name="title">New value of the property.</param>
		public override void NotifyFullTitleText(string title)
        {
            this.Text = title;
            Invalidate();
        }

		/// <summary>
		/// Handle a change in the state of the parent window.
		/// </summary>
		/// <param name="newState">New value of the property.</param>
		public override void ParentStateChanged(State newState)
        { 
            // Should we dock to the left or top of our container?
            switch(newState)
            {
                case State.DockTop:
                case State.DockBottom:
					if (DockingManager.AllowSideCaptions)
					{
						_dockLeft = true;
						this.Dock = DockStyle.Left;
					}
					else
					{
						_dockLeft = false;
						this.Dock = DockStyle.Top;
					}
                    break;
                case State.Floating:
                case State.DockLeft:
                case State.DockRight:
                    _dockLeft = false;
		            this.Dock = DockStyle.Top;
                    break;
            }
            
			// Ignore the AutoHide feature when in floating form
            _ignoreHideButton = (ParentWindow.State == State.Floating);

            RecalculateButtons();
            Invalidate();
        }

		/// <summary>
		/// Handle the detail being removed from the parent window.
		/// </summary>
		/// <param name="parent">Window parent we are removed from.</param>
		public override void RemovedFromParent(Window parent)
        {
            if (parent != null)
            {
                if (this.Dock != DockStyle.None)
                {
                    Size minSize = parent.MinimalSize;

                    if (this.Dock == DockStyle.Left)
                    {
                        // Remove our width from the minimum size of the parent
                        minSize.Width -= _fixedLength;
                    }
                    else
                    {
                        // Remove our height from the minimum size of the parent
                        minSize.Height -= _fixedLength;
                    }

                    parent.MinimalSize = minSize;
                }
            }
        }

		/// <summary>
		/// Handle the detail being added to a parent window.
		/// </summary>
		/// <param name="parent">Window parent we are added to.</param>
		public override void AddedToParent(Window parent)
		{
            if (parent != null)
            {
                if (this.Dock != DockStyle.None)
                {
                    Size minSize = parent.MinimalSize;

                    if (this.Dock == DockStyle.Left)
                    {
                        // Add our width from the minimum size of the parent
                        minSize.Width += _fixedLength;
                    }
                    else
                    {
                        // Add our height from the minimum size of the parent
                        minSize.Height += _fixedLength;
                    }

                    parent.MinimalSize = minSize;
                }
            }
		}
		
		/// <summary>
		/// Let derived classes image specific images.
		/// </summary>
		protected override ImageList Images
		{ 
			get { return _imagesOffice; }
		}

		/// <summary>
		/// Dispose of instance resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Must unhook otherwise the instance cannot be garbage collected
				Microsoft.Win32.SystemEvents.UserPreferenceChanged -= 
					new Microsoft.Win32.UserPreferenceChangedEventHandler(OnUserPreferenceChanged); 

				// Color details has resources that need releasing
				_colorDetails.Dispose();
			}
			
			base.Dispose (disposing);
		}

		/// <summary>
		/// Raises the MouseMove event.
		/// </summary>
		/// <param name="e">A MouseEventArgs structure containing event data.</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			Form parentForm = FindForm();
			
			if ((parentForm != null) && parentForm.ContainsFocus)
			{
				if (((!_dockLeft) && (e.X < (_xInset + _spotWidth))) ||
					((_dockLeft) && (e.Y > (Height - _xInset - _spotWidth))))
					Cursor = Cursors.SizeAll;
				else
					Cursor = Cursors.Default;
			}
			else
				Cursor = Cursors.Default;

			base.OnMouseMove(e);
		}

		/// <summary>
		/// Raises the MouseLeave event.
		/// </summary>
		/// <param name="e">An EventArgs structure containing event data.</param>
		protected override void OnMouseLeave(EventArgs e)
		{
			Cursor = Cursors.Default;
			base.OnMouseLeave(e);
		}

		/// <summary>
		/// Calculate which button recolors should be remapped.
		/// </summary>
		protected override void DefineButtonRemapping()
		{
			// Define use of current system colors
			ColorMap activeMap = new ColorMap();
			ColorMap inactiveMap = new ColorMap();
			
			activeMap.OldColor = Color.Black;
			activeMap.NewColor = DockingManager.ActiveTextColor;
			inactiveMap.OldColor = Color.Black;
			inactiveMap.NewColor = DockingManager.InactiveTextColor;

			// Create remap attributes for use by button
			_activeAttr.SetRemapTable(new ColorMap[]{activeMap}, ColorAdjustType.Bitmap);
			_inactiveAttr.SetRemapTable(new ColorMap[]{inactiveMap}, ColorAdjustType.Bitmap);
		}

		/// <summary>
		/// Process the maximize interface being added.
		/// </summary>
		protected override void OnAddMaximizeInterface()
        {
            if (_maxButton != null)
            {
                // Set the correct size for the button
                _maxButton.Size = new Size(_buttonWidth, _buttonHeight);
				_maxButton.Style = VisualStyle.Office2003;
				_maxButton.ImageAttributes = _inactiveAttr;

                // Force the ImageAttribute for the button to be set
                SetButtonState();
                Invalidate();
            }
        }

		/// <summary>
		/// Update the appearance of the auto hide image.
		/// </summary>
		protected override void UpdateAutoHideImage()
        {
            if (_pinnedImage)
                _hideButton.Image = Images.Images[(int)ImageIndex.AutoShow];
            else
                _hideButton.Image = Images.Images[(int)ImageIndex.AutoHide];
        }

		/// <summary>
		/// Update the appearance of the maximize image.
		/// </summary>
		protected override void UpdateMaximizeImage()
        {
            if ((_maxButton != null) && (_maxInterface != null))
            {
                bool enabled = _maxInterface.IsMaximizeAvailable();

                if (!enabled)
                {
                    if (_maxButton.Visible)
                        _maxButton.Hide();
                }
                else
                {
                    bool maximized = _maxInterface.IsWindowMaximized(this.ParentWindow);

                    if (!_maxButton.Visible)
                        _maxButton.Show();

                    if (maximized)
                        _maxButton.Image = Images.Images[(int)ImageIndex.EnabledVerticalMin];	
                    else
                        _maxButton.Image = Images.Images[(int)ImageIndex.EnabledVerticalMax];	
                }
            }
        }

		/// <summary>
		/// Update the state of the caption buttons.
		/// </summary>
        protected void SetButtonState()
        {
			if (this.ParentWindow != null)
			{
				if (this.ParentWindow.ContainsFocus && !DockingManager.DefaultActiveTextColor)
				{
					_closeButton.ImageAttributes = _activeAttr;
					_closeButton.Invalidate();

					if (_hideButton != null)
					{
						_hideButton.ImageAttributes = _activeAttr;
						_hideButton.Invalidate();
					}

					if (_maxButton != null)
					{
						_maxButton.ImageAttributes = _activeAttr;
						_maxButton.Invalidate();
					}
				}
				else
				{
					_closeButton.ImageAttributes = _inactiveAttr;
					_closeButton.Invalidate();

					if (_hideButton != null)
					{
						_hideButton.ImageAttributes = _inactiveAttr;
						_hideButton.Invalidate();
					}

					if (_maxButton != null)
					{
						_maxButton.ImageAttributes = _inactiveAttr;
						_maxButton.Invalidate();
					}
				}
			}
		}

		/// <summary>
		/// Calculate the visibility and position of buttons.
		/// </summary>
		protected override void RecalculateButtons()
        {
			if (_dockLeft)
			{
				int buttonX = (Width - _yInset * 2 - _buttonWidth) / 2 + _yInset + 1;
				int buttonY = _buttonSpacer;
	        
				if (_showCloseButton)
				{
					// Button position is fixed, regardless of our size
					_closeButton.SetBounds(buttonX, buttonY, _buttonWidth, _buttonHeight);
	    		
					// Let the location of the control be updated for us
					_closeButton.Anchor = AnchorStyles.Top;

					// Just in case currently hidden
					_closeButton.Show();
	                
					// Define start of next button
					buttonY += _buttonHeight;
				}
				else
					_closeButton.Hide();
	                        
				if (_showHideButton && !_ignoreHideButton)
				{
					// Button position is fixed, regardless of our size
					_hideButton.SetBounds(buttonX, buttonY, _buttonWidth, _buttonHeight);
				
					// Let the location of the control be updated for us
					_hideButton.Anchor = AnchorStyles.Top;

					// Just in case currently hidden
					_hideButton.Show();

					// Define start of next button
					buttonY += _buttonHeight;
	                
					UpdateAutoHideImage();
				}
				else
					_hideButton.Hide();
	            
				if (_maxButton != null)
				{
					// Button position is fixed, regardless of our size
					_maxButton.SetBounds(buttonX, buttonY, _buttonWidth, _buttonHeight);
	                
					// Let the location of the control be updated for us
					_maxButton.Anchor = AnchorStyles.Top;

					// Define start of next button
					buttonY += _buttonHeight;

					UpdateMaximizeImage();
				}
			}
			else
			{
				int buttonX = this.Width - _buttonWidth - _buttonSpacer;
				int buttonY = (Height - _yInset * 2 - _buttonHeight) / 2 + _yInset;
	        
				if (_showCloseButton)
				{
					// Button position is fixed, regardless of our size
					_closeButton.SetBounds(buttonX, buttonY, _buttonWidth, _buttonHeight);
	    		
					// Let the location of the control be updated for us
					_closeButton.Anchor = AnchorStyles.Right;

					// Just in case currently hidden
					_closeButton.Show();
	                
					// Define start of next button
					buttonX -= _buttonWidth;
				}
				else
					_closeButton.Hide();
	                        
				if (_showHideButton && !_ignoreHideButton)
				{
					// Button position is fixed, regardless of our size
					_hideButton.SetBounds(buttonX, buttonY, _buttonWidth, _buttonHeight);
				
					// Let the location of the control be updated for us
					_hideButton.Anchor = AnchorStyles.Right;

					// Just in case currently hidden
					_hideButton.Show();

					// Define start of next button
					buttonX -= _buttonWidth;
	                
					UpdateAutoHideImage();
				}
				else
					_hideButton.Hide();
	            
				if (_maxButton != null)
				{
					// Button position is fixed, regardless of our size
					_maxButton.SetBounds(buttonX, buttonY, _buttonWidth, _buttonHeight);
	                
					// Let the location of the control be updated for us
					_maxButton.Anchor = AnchorStyles.Right;

					// Define start of next button
					buttonX -= _buttonWidth;

					UpdateMaximizeImage();
				}
			}
        }

		/// <summary>
		/// Create the button class specific to this implementation.
		/// </summary>
		protected override void CreateMaxButton()
		{
			// Create the InertButton
			_maxButton = DockingManager.Factory.CreateOffice2003CaptionButton(this);
		}

		/// <summary>
		/// Create button required for caption.
		/// </summary>
		protected override void CreateButtons()
        {
			if (_closeButton == null)
			{
				// Define the ImageList and which ImageIndex to show initially
                _closeButton = DockingManager.Factory.CreateOffice2003CaptionButton(this);
				_closeButton.Image = Images.Images[(int)ImageIndex.Close];
				_closeButton.Style = VisualStyle.Office2003;

                _hideButton = DockingManager.Factory.CreateOffice2003CaptionButton(this);
				_hideButton.Image = Images.Images[(int)ImageIndex.AutoHide];
				_hideButton.Style = VisualStyle.Office2003;
				
				_closeButton.Size = new Size(_buttonWidth, _buttonHeight);
				_hideButton.Size = new Size(_buttonWidth, _buttonHeight);

				// Let the location of the control be updated for us
				_closeButton.Anchor = AnchorStyles.Right;
				_hideButton.Anchor = AnchorStyles.Right;

				// Define the button position relative to the size set in the constructor
				_closeButton.Location = new Point(_fixedLength - _buttonWidth - _buttonSpacer, 
												(_fixedLength - _yInset * 2 - _buttonHeight) / 2 + _yInset);
	            
				_hideButton.Location = new Point(_fixedLength - (_buttonWidth - _buttonSpacer) * 2, 
												(_fixedLength - _yInset * 2 - _buttonHeight) / 2 + _yInset);

				// Define correct color setup
				_closeButton.ImageAttributes = _inactiveAttr;
				_hideButton.ImageAttributes = _inactiveAttr;
				
				// Add to the display
				Controls.Add(_closeButton);
				Controls.Add(_hideButton);
			}

            // Let base class perform common actions
            base.CreateButtons();
        }

		/// <summary>
		/// Adjust the height of the caption to reflect changed font.
		/// </summary>
		/// <param name="captionFont"></param>
		protected void UpdateCaptionHeight(Font captionFont)
		{
            // Dynamically calculate the required height of the caption area
            _fixedLength = (int)captionFont.GetHeight() + (_yInset + _yInsetExtra) * 2;
    
            int minHeight = (_dockLeft ? (_buttonWidth + _yInset * 4 + 1) :
										 (_buttonHeight + _yInset * 4 + 1));

			// Maintain a minimum height to allow correct showing of buttons
            if (_fixedLength < minHeight)
                _fixedLength = minHeight;

			this.Size = new Size(_fixedLength, _fixedLength);
			
			RecalculateButtons();

			Invalidate();
		}

		/// <summary>
		/// Paints the background of the control.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains information about the control to paint.</param>
		protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Overriden to prevent background being painted
        }

		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data. </param>
		protected override void OnPaint(PaintEventArgs e)
        {
			// Sometimes the min/max button is created and then an attempt is made to 
            // hide the button. But for some reason it does not always manage to hide 
            // the button. So forced to check here everytime to ensure its hidden.
            UpdateMaximizeImage();

            SetButtonState();
            
			Color caption1;
			Color caption2;
			Color spotsColor1;
			Color spotsColor2;
			Color textColor;

			bool focused = false;

			if (this.ParentWindow != null)
				focused = this.ParentWindow.ContainsFocus;

			// Choose the appropriate caption and text colours
			if (focused)
			{
				if (DockingManager.DefaultActiveColor)
				{
					caption1 = _colorDetails.CaptionSelectColor1;
					caption2 = _colorDetails.CaptionSelectColor2;
				}
				else
				{
					caption1 = ControlPaint.Light(DockingManager.ActiveColor);
					caption2 = ControlPaint.Dark(DockingManager.ActiveColor);
				}
				
				if (DockingManager.DefaultActiveTextColor)
					textColor = DockingManager.InactiveTextColor;
				else
					textColor = DockingManager.ActiveTextColor;
			}
			else
			{
				if (DockingManager.DefaultBackColor)
				{
					caption1 = _colorDetails.CaptionColor1;
					caption2 = _colorDetails.CaptionColor2;
				}
				else
				{
					caption1 = ControlPaint.Light(DockingManager.BackColor);
					caption2 = ControlPaint.Dark(DockingManager.BackColor);
				}

				textColor = DockingManager.InactiveTextColor;
			}
			
			// Choose the appropriate spot colours
			if (DockingManager.DefaultBackColor)
			{
				spotsColor1 = _colorDetails.SpotColor1;
				spotsColor2 = _colorDetails.SpotColor2;
			}
			else
			{
				spotsColor1 = ControlPaint.DarkDark(DockingManager.BackColor);
				spotsColor2 = ControlPaint.LightLight(DockingManager.BackColor);
			}

			// Is there any rectangle size to draw?
			if ((ClientRectangle.Width > 0) && (ClientRectangle.Height > 0))
			{
				// Fill the entire background area
				using(LinearGradientBrush backBrush = new LinearGradientBrush(ClientRectangle, 
																			  caption1, 
																			  caption2, 
																			  (_dockLeft ? 0f : 90f)))
				{
					// Use signal bell curse to get more rounded look
					backBrush.SetSigmaBellShape(1f, 1f);
					
					// Draw the gradient background
					e.Graphics.FillRectangle(backBrush, ClientRectangle);
				}
			}
			
            Size ourSize = this.ClientSize;

            // Depends on orientation
            if (_dockLeft)
			{
                int yEnd = ourSize.Height - _buttonWidth - _buttonSpacer;
				int xEnd = ourSize.Width - _yInset * 2;
				
				Rectangle rectCaption = new Rectangle(_yInset, 0, xEnd - _yInset + 1, yEnd);

				// Draw the handle used on the left hand side to perform drag
				CommandDraw.DrawHandles(e.Graphics, 
										VisualStyle.Office2003, 
										new Rectangle(rectCaption.X, rectCaption.Bottom - _xInset - _spotWidth, rectCaption.Width, _spotWidth),
										_colorDetails.SpotColor1,
										_colorDetails.SpotColor2,
										false);

				// Start drawing text a little from the left
				rectCaption.Height -= _xInset + _spotWidth * 2;
				rectCaption.X += 1;
				rectCaption.Width -= 2;

                if (_showCloseButton)
                {
                    rectCaption.Y += _buttonWidth;
                    rectCaption.Height -= _buttonWidth;
                }

                if (_showHideButton && !_ignoreHideButton)
                {
                    rectCaption.Y += _buttonWidth;
                    rectCaption.Height -= _buttonWidth;
                }

				// Reduce width to account for maximize button
                if ((_maxButton != null) && _maxInterface.IsMaximizeAvailable())
                {
                    rectCaption.Y += _buttonWidth;
                    rectCaption.Height -= _buttonWidth;
				}
	    		
    			using(StringFormat format = new StringFormat())
				{
    				format.Alignment = StringAlignment.Near;
    				format.LineAlignment = StringAlignment.Center;
    				format.Trimming	= StringTrimming.EllipsisCharacter;
					format.FormatFlags = StringFormatFlags.NoWrap |
										 StringFormatFlags.DirectionVertical;
		    		
					using(Brush textBrush = new SolidBrush(textColor))
	    				DrawHelper.DrawReverseString(e.Graphics, this.Text, DockingManager.CaptionFont, rectCaption, textBrush, format);
				}
			}
			else
			{
                int xEnd = ourSize.Width - _buttonWidth - _buttonSpacer;
				int yEnd = ourSize.Height - _yInset * 2;

				Rectangle rectCaption = new Rectangle(0, _yInset, xEnd, yEnd - _yInset + 1);

				// Draw the handle used on the left hand side to perform drag
				CommandDraw.DrawHandles(e.Graphics, 
										VisualStyle.Office2003, 
										new Rectangle(rectCaption.X + _xInset, rectCaption.Y,  _spotWidth, rectCaption.Height),
										_colorDetails.SpotColor1,
										_colorDetails.SpotColor2,
										true);

				// Start drawing text a little from the left
				rectCaption.X += _xInset + _spotWidth;
				rectCaption.Y += 1;
				rectCaption.Height -= 2;

                if (_showCloseButton)
                    rectCaption.Width -= _buttonWidth;

                if (_showHideButton && !_ignoreHideButton)
                    rectCaption.Width -= _buttonWidth;

				// Reduce width to account for maximize button
				if ((_maxButton != null) && _maxInterface.IsMaximizeAvailable())
                    rectCaption.Width -= _buttonWidth;
	    		
    			using(StringFormat format = new StringFormat(StringFormatFlags.NoClip))
				{
    				format.Alignment = (RightToLeft == RightToLeft.Yes ? StringAlignment.Far : StringAlignment.Near);
    				format.LineAlignment = StringAlignment.Center;
    				format.Trimming	= StringTrimming.EllipsisCharacter;
					format.FormatFlags = StringFormatFlags.NoWrap;
					
    				using(Brush textBrush = new SolidBrush(textColor))
						e.Graphics.DrawString(this.Text, DockingManager.CaptionFont, textBrush, rectCaption, format);
				}
			}
            
            // Always get the button to repaint as we have painted over their area
            _closeButton.Refresh();
        }
        				
		private void OnUserPreferenceChanged(object sender, Microsoft.Win32.UserPreferenceChangedEventArgs e)
		{
			// Use as a signal to retest the theme information
			_colorDetails.Reset();
		}
	}

    /// <summary>
    /// Window caption detail implementation for Office2007 visual styles.
    /// </summary>
    public class WindowDetailCaptionOffice2007 : WindowDetailCaption
    {
        private enum ImageIndex
        {
            Close = 0,
            EnabledVerticalMax = 1,
            EnabledVerticalMin = 2,
            AutoHide = 3,
            AutoShow = 4
        }

        // Class constants
        private const int _xInset = 3;
        private const int _spotWidth = 6;
        private const int _yInset = 0;
        private const int _yInsetExtra = 5;
        private const int _imageWidth = 12;
        private const int _imageHeight = 11;
        private const int _buttonWidth = 14;
        private const int _buttonHeight = 13;
        private const int _buttonSpacer = 3;

        // Class fields
        private static ImageList _imagesOffice;
        private static int _fixedLength;

        // Instance fields
        private bool _dockLeft;
        private bool _apply2007ClearType;
        private ColorDetails _colorDetails;

        static WindowDetailCaptionOffice2007()
        {
            // Create a strip of images by loading an embedded bitmap resource
            _imagesOffice = ResourceHelper.LoadBitmapStrip(Type.GetType("Crownwood.DotNetMagic.Docking.WindowDetailCaptionIDE"),
                                                           "Crownwood.DotNetMagic.Resources.ImagesCaptionOffice2003.bmp",
                                                           new Size(_imageWidth, _imageHeight),
                                                           new Point(0, 0));
        }

        /// <summary>
        /// Initializes a new instance of the WindowDetailCaptionOffice2007 class.
        /// </summary>
        /// <param name="manager">Reference to parent docking manager.</param>
        /// <param name="wc">Reference to owning window content.</param>
        /// <param name="closeHandler">Delegate for notifying close events.</param>
        /// <param name="restoreHandler">Delegate for notifying restore events.</param>
        /// <param name="invertAutoHideHandler">Delegate for auto hide invert events.</param>
        /// <param name="contextHandler">Delegate for notifying context menu events.</param>
        public WindowDetailCaptionOffice2007(DockingManager manager,
                                             WindowContent wc,
                                             EventHandler closeHandler,
                                             EventHandler restoreHandler,
                                             EventHandler invertAutoHideHandler,
                                             ContextHandler contextHandler)
            : base(manager, wc,
                   new Size(_fixedLength, _fixedLength),
                   closeHandler,
                   restoreHandler,
                   invertAutoHideHandler,
                   contextHandler)
        {

            // Get the color helper for drawing
            _colorDetails = new ColorDetails();

            // Default to thinking we are docked on a left edge
            _dockLeft = false;

            // Default to using clear type on drawing text
            _apply2007ClearType = true;

            // Use specificed font in the caption 
            UpdateCaptionHeight(manager.CaptionFont);

            // We need to know when the system colours have changed
            Microsoft.Win32.SystemEvents.UserPreferenceChanged +=
                new Microsoft.Win32.UserPreferenceChangedEventHandler(OnUserPreferenceChanged);
        }

        /// <summary>
        /// Gets access to the colouring details.
        /// </summary>
        public ColorDetails ColorDetails
        {
            get { return _colorDetails; }
        }

        /// <summary>
        /// Gets and sets a value indicating if the Office 2007 style text should use ClearType.
        /// </summary>
        public bool Apply2007ClearType
        {
            get { return _apply2007ClearType; }
            set { _apply2007ClearType = value; }
        }

        /// <summary>
        /// Propogate change in a property.
        /// </summary>
        /// <param name="name">Property that has changed.</param>
        /// <param name="value">New value.</param>
        public override void PropogateNameValue(PropogateName name, object value)
        {
            base.PropogateNameValue(name, value);

            switch (name)
            {
                case PropogateName.CaptionFont:
                    UpdateCaptionHeight((Font)value);
                    break;
                case PropogateName.ActiveTextColor:
                case PropogateName.InactiveTextColor:
                    DefineButtonRemapping();
                    Invalidate();
                    break;
                case PropogateName.Apply2007ClearType:
                    Apply2007ClearType = (bool)value;
                    Invalidate();
                    break;
            }
        }

        /// <summary>
        /// Handle the parent window getting focus.
        /// </summary>
        public override void WindowGotFocus()
        {
            SetButtonState();
            Invalidate();
        }

        /// <summary>
        /// Handle the parent window losing focus.
        /// </summary>
        public override void WindowLostFocus()
        {
            SetButtonState();
            Invalidate();
        }

        /// <summary>
        /// Handle a change in the caption bar text.
        /// </summary>
        /// <param name="title">New value of the property.</param>
        public override void NotifyFullTitleText(string title)
        {
            this.Text = title;
            Invalidate();
        }

        /// <summary>
        /// Handle a change in the state of the parent window.
        /// </summary>
        /// <param name="newState">New value of the property.</param>
        public override void ParentStateChanged(State newState)
        {
            // Should we dock to the left or top of our container?
            switch (newState)
            {
                case State.DockTop:
                case State.DockBottom:
                    if (DockingManager.AllowSideCaptions)
                    {
                        _dockLeft = true;
                        this.Dock = DockStyle.Left;
                    }
                    else
                    {
                        _dockLeft = false;
                        this.Dock = DockStyle.Top;
                    }
                    break;
                case State.Floating:
                case State.DockLeft:
                case State.DockRight:
                    _dockLeft = false;
                    this.Dock = DockStyle.Top;
                    break;
            }

            // Ignore the AutoHide feature when in floating form
            _ignoreHideButton = (ParentWindow.State == State.Floating);

            RecalculateButtons();
            Invalidate();
        }

        /// <summary>
        /// Handle the detail being removed from the parent window.
        /// </summary>
        /// <param name="parent">Window parent we are removed from.</param>
        public override void RemovedFromParent(Window parent)
        {
            if (parent != null)
            {
                if (this.Dock != DockStyle.None)
                {
                    Size minSize = parent.MinimalSize;

                    if (this.Dock == DockStyle.Left)
                    {
                        // Remove our width from the minimum size of the parent
                        minSize.Width -= _fixedLength;
                    }
                    else
                    {
                        // Remove our height from the minimum size of the parent
                        minSize.Height -= _fixedLength;
                    }

                    parent.MinimalSize = minSize;
                }
            }
        }

        /// <summary>
        /// Handle the detail being added to a parent window.
        /// </summary>
        /// <param name="parent">Window parent we are added to.</param>
        public override void AddedToParent(Window parent)
        {
            if (parent != null)
            {
                if (this.Dock != DockStyle.None)
                {
                    Size minSize = parent.MinimalSize;

                    if (this.Dock == DockStyle.Left)
                    {
                        // Add our width from the minimum size of the parent
                        minSize.Width += _fixedLength;
                    }
                    else
                    {
                        // Add our height from the minimum size of the parent
                        minSize.Height += _fixedLength;
                    }

                    parent.MinimalSize = minSize;
                }
            }
        }

        /// <summary>
        /// Let derived classes image specific images.
        /// </summary>
        protected override ImageList Images
        {
            get { return _imagesOffice; }
        }

        /// <summary>
        /// Dispose of instance resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Must unhook otherwise the instance cannot be garbage collected
                Microsoft.Win32.SystemEvents.UserPreferenceChanged -=
                    new Microsoft.Win32.UserPreferenceChangedEventHandler(OnUserPreferenceChanged);

                // Color details has resources that need releasing
                _colorDetails.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Raises the MouseMove event.
        /// </summary>
        /// <param name="e">A MouseEventArgs structure containing event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            Form parentForm = FindForm();

            if ((parentForm != null) && parentForm.ContainsFocus)
            {
                if (((!_dockLeft) && (e.X < (_xInset + _spotWidth))) ||
                    ((_dockLeft) && (e.Y > (Height - _xInset - _spotWidth))))
                    Cursor = Cursors.SizeAll;
                else
                    Cursor = Cursors.Default;
            }
            else
                Cursor = Cursors.Default;

            base.OnMouseMove(e);
        }

        /// <summary>
        /// Raises the MouseLeave event.
        /// </summary>
        /// <param name="e">An EventArgs structure containing event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            Cursor = Cursors.Default;
            base.OnMouseLeave(e);
        }

        /// <summary>
        /// Calculate which button recolors should be remapped.
        /// </summary>
        protected override void DefineButtonRemapping()
        {
            // Define use of current system colors
            ColorMap activeMap = new ColorMap();
            ColorMap inactiveMap = new ColorMap();

            activeMap.OldColor = Color.Black;
            
            if (DockingManager.DefaultActiveTextColor)
                activeMap.NewColor = Office2007ColorTable.TabActiveTextColor(DockingManager.Style, OfficeStyle.LightEnhanced);
            else
                activeMap.NewColor = DockingManager.ActiveTextColor;

            inactiveMap.OldColor = Color.Black;

            if (DockingManager.DefaultInactiveTextColor)
                inactiveMap.NewColor = Office2007ColorTable.TitleActiveTextColor(DockingManager.Style);
            else
                inactiveMap.NewColor = DockingManager.InactiveTextColor;

            // Create remap attributes for use by button
            _activeAttr.SetRemapTable(new ColorMap[] { activeMap }, ColorAdjustType.Bitmap);
            _inactiveAttr.SetRemapTable(new ColorMap[] { inactiveMap }, ColorAdjustType.Bitmap);
        }

        /// <summary>
        /// Process the maximize interface being added.
        /// </summary>
        protected override void OnAddMaximizeInterface()
        {
            if (_maxButton != null)
            {
                // Set the correct size for the button
                _maxButton.Size = new Size(_buttonWidth, _buttonHeight);
                _maxButton.Style = VisualStyle.Office2003;
                _maxButton.ImageAttributes = _inactiveAttr;

                // Force the ImageAttribute for the button to be set
                SetButtonState();
                Invalidate();
            }
        }

        /// <summary>
        /// Update the appearance of the auto hide image.
        /// </summary>
        protected override void UpdateAutoHideImage()
        {
            if (_pinnedImage)
                _hideButton.Image = Images.Images[(int)ImageIndex.AutoShow];
            else
                _hideButton.Image = Images.Images[(int)ImageIndex.AutoHide];
        }

        /// <summary>
        /// Update the appearance of the maximize image.
        /// </summary>
        protected override void UpdateMaximizeImage()
        {
            if ((_maxButton != null) && (_maxInterface != null))
            {
                bool enabled = _maxInterface.IsMaximizeAvailable();

                if (!enabled)
                {
                    if (_maxButton.Visible)
                        _maxButton.Hide();
                }
                else
                {
                    bool maximized = _maxInterface.IsWindowMaximized(this.ParentWindow);

                    if (!_maxButton.Visible)
                        _maxButton.Show();

                    if (maximized)
                        _maxButton.Image = Images.Images[(int)ImageIndex.EnabledVerticalMin];
                    else
                        _maxButton.Image = Images.Images[(int)ImageIndex.EnabledVerticalMax];
                }
            }
        }

        /// <summary>
        /// Update the state of the caption buttons.
        /// </summary>
        protected void SetButtonState()
        {
            if (this.ParentWindow != null)
            {
                if (this.ParentWindow.ContainsFocus && !DockingManager.DefaultActiveTextColor)
                {
                    _closeButton.ImageAttributes = _activeAttr;
                    _closeButton.Invalidate();

                    if (_hideButton != null)
                    {
                        _hideButton.ImageAttributes = _activeAttr;
                        _hideButton.Invalidate();
                    }

                    if (_maxButton != null)
                    {
                        _maxButton.ImageAttributes = _activeAttr;
                        _maxButton.Invalidate();
                    }
                }
                else
                {
                    _closeButton.ImageAttributes = _inactiveAttr;
                    _closeButton.Invalidate();

                    if (_hideButton != null)
                    {
                        _hideButton.ImageAttributes = _inactiveAttr;
                        _hideButton.Invalidate();
                    }

                    if (_maxButton != null)
                    {
                        _maxButton.ImageAttributes = _inactiveAttr;
                        _maxButton.Invalidate();
                    }
                }
            }
        }

        /// <summary>
        /// Calculate the visibility and position of buttons.
        /// </summary>
        protected override void RecalculateButtons()
        {
            if (_dockLeft)
            {
                int buttonX = (Width - _yInset * 2 - _buttonWidth) / 2 + _yInset + 1;
                int buttonY = _buttonSpacer;

                if (_showCloseButton)
                {
                    // Button position is fixed, regardless of our size
                    _closeButton.SetBounds(buttonX, buttonY, _buttonWidth, _buttonHeight);

                    // Let the location of the control be updated for us
                    _closeButton.Anchor = AnchorStyles.Top;

                    // Just in case currently hidden
                    _closeButton.Show();

                    // Define start of next button
                    buttonY += _buttonHeight;
                }
                else
                    _closeButton.Hide();

                if (_showHideButton && !_ignoreHideButton)
                {
                    // Button position is fixed, regardless of our size
                    _hideButton.SetBounds(buttonX, buttonY, _buttonWidth, _buttonHeight);

                    // Let the location of the control be updated for us
                    _hideButton.Anchor = AnchorStyles.Top;

                    // Just in case currently hidden
                    _hideButton.Show();

                    // Define start of next button
                    buttonY += _buttonHeight;

                    UpdateAutoHideImage();
                }
                else
                    _hideButton.Hide();

                if (_maxButton != null)
                {
                    // Button position is fixed, regardless of our size
                    _maxButton.SetBounds(buttonX, buttonY, _buttonWidth, _buttonHeight);

                    // Let the location of the control be updated for us
                    _maxButton.Anchor = AnchorStyles.Top;

                    // Define start of next button
                    buttonY += _buttonHeight;

                    UpdateMaximizeImage();
                }
            }
            else
            {
                int buttonX = this.Width - _buttonWidth - _buttonSpacer;
                int buttonY = (Height - _yInset * 2 - _buttonHeight) / 2 + _yInset;

                if (_showCloseButton)
                {
                    // Button position is fixed, regardless of our size
                    _closeButton.SetBounds(buttonX, buttonY, _buttonWidth, _buttonHeight);

                    // Let the location of the control be updated for us
                    _closeButton.Anchor = AnchorStyles.Right;

                    // Just in case currently hidden
                    _closeButton.Show();

                    // Define start of next button
                    buttonX -= _buttonWidth;
                }
                else
                    _closeButton.Hide();

                if (_showHideButton && !_ignoreHideButton)
                {
                    // Button position is fixed, regardless of our size
                    _hideButton.SetBounds(buttonX, buttonY, _buttonWidth, _buttonHeight);

                    // Let the location of the control be updated for us
                    _hideButton.Anchor = AnchorStyles.Right;

                    // Just in case currently hidden
                    _hideButton.Show();

                    // Define start of next button
                    buttonX -= _buttonWidth;

                    UpdateAutoHideImage();
                }
                else
                    _hideButton.Hide();

                if (_maxButton != null)
                {
                    // Button position is fixed, regardless of our size
                    _maxButton.SetBounds(buttonX, buttonY, _buttonWidth, _buttonHeight);

                    // Let the location of the control be updated for us
                    _maxButton.Anchor = AnchorStyles.Right;

                    // Define start of next button
                    buttonX -= _buttonWidth;

                    UpdateMaximizeImage();
                }
            }
        }

        /// <summary>
        /// Create the button class specific to this implementation.
        /// </summary>
        protected override void CreateMaxButton()
        {
            // Create the InertButton
            _maxButton = DockingManager.Factory.CreateOffice2007CaptionButton(this);
        }

        /// <summary>
        /// Create button required for caption.
        /// </summary>
        protected override void CreateButtons()
        {
            if (_closeButton == null)
            {
                // Define the ImageList and which ImageIndex to show initially
                _closeButton = DockingManager.Factory.CreateOffice2007CaptionButton(this);
                _closeButton.Image = Images.Images[(int)ImageIndex.Close];
                _closeButton.Style = VisualStyle.Office2003;

                _hideButton = DockingManager.Factory.CreateOffice2007CaptionButton(this);
                _hideButton.Image = Images.Images[(int)ImageIndex.AutoHide];
                _hideButton.Style = VisualStyle.Office2003;

                _closeButton.Size = new Size(_buttonWidth, _buttonHeight);
                _hideButton.Size = new Size(_buttonWidth, _buttonHeight);

                // Let the location of the control be updated for us
                _closeButton.Anchor = AnchorStyles.Right;
                _hideButton.Anchor = AnchorStyles.Right;

                // Define the button position relative to the size set in the constructor
                _closeButton.Location = new Point(_fixedLength - _buttonWidth - _buttonSpacer,
                                                 (_fixedLength - _yInset * 2 - _buttonHeight) / 2 + _yInset);

                _hideButton.Location = new Point(_fixedLength - (_buttonWidth - _buttonSpacer) * 2,
                                                (_fixedLength - _yInset * 2 - _buttonHeight) / 2 + _yInset);

                // Define correct color setup
                _closeButton.ImageAttributes = _inactiveAttr;
                _hideButton.ImageAttributes = _inactiveAttr;

                // Add to the display
                Controls.Add(_closeButton);
                Controls.Add(_hideButton);
            }

            // Let base class perform common actions
            base.CreateButtons();
        }

        /// <summary>
        /// Adjust the height of the caption to reflect changed font.
        /// </summary>
        /// <param name="captionFont"></param>
        protected void UpdateCaptionHeight(Font captionFont)
        {
            // Dynamically calculate the required height of the caption area
            _fixedLength = (int)captionFont.GetHeight() + (_yInset + _yInsetExtra) * 2;

            int minHeight = (_dockLeft ? (_buttonWidth + _yInset * 4 + 1) :
                                         (_buttonHeight + _yInset * 4 + 1));

            // Maintain a minimum height to allow correct showing of buttons
            if (_fixedLength < minHeight)
                _fixedLength = minHeight;

            this.Size = new Size(_fixedLength, _fixedLength);

            RecalculateButtons();

            Invalidate();
        }

        /// <summary>
        /// Paints the background of the control.
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains information about the control to paint.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Overriden to prevent background being painted
        }

        /// <summary>
        /// Raises the Paint event.
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data. </param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Sometimes the min/max button is created and then an attempt is made to 
            // hide the button. But for some reason it does not always manage to hide 
            // the button. So forced to check here everytime to ensure its hidden.
            UpdateMaximizeImage();

            SetButtonState();

            Color caption1;
            Color caption2;
            Color spotsColor1;
            Color spotsColor2;
            Color textColor;
            Color borderColor;

            bool focused = false;

            if (this.ParentWindow != null)
                focused = this.ParentWindow.ContainsFocus;

            // Choose the appropriate caption and text colours
            if (focused)
            {
                if (DockingManager.DefaultActiveColor)
                {
                    caption1 = Office2007ColorTable.CheckedActiveLight(DockingManager.Style);
                    caption2 = Office2007ColorTable.CheckedActiveDark(DockingManager.Style);
                }
                else
                {
                    caption1 = ControlPaint.Light(DockingManager.ActiveColor);
                    caption2 = ControlPaint.Dark(DockingManager.ActiveColor);
                }

                textColor = Office2007ColorTable.TabActiveTextColor(DockingManager.Style, OfficeStyle.LightEnhanced);
                borderColor = Office2007ColorTable.BorderColor(DockingManager.Style);
            }
            else
            {
                if (DockingManager.DefaultBackColor)
                {
                    caption1 = Office2007ColorTable.TitleActiveLight(DockingManager.Style);
                    caption2 = Office2007ColorTable.TitleActiveDark(DockingManager.Style);
                }
                else
                {
                    caption1 = ControlPaint.Light(DockingManager.BackColor);
                    caption2 = ControlPaint.Dark(DockingManager.BackColor);
                }

                textColor = Office2007ColorTable.TitleActiveTextColor(DockingManager.Style);
                borderColor = Office2007ColorTable.BorderColor(DockingManager.Style);
            }

            // Choose the appropriate spot colours
            if (DockingManager.DefaultBackColor)
            {
                spotsColor1 = Office2007ColorTable.StatusBarGripDark(DockingManager.Style);
                spotsColor2 = Office2007ColorTable.StatusBarGripLight(DockingManager.Style);
            }
            else
            {
                spotsColor1 = ControlPaint.DarkDark(DockingManager.BackColor);
                spotsColor2 = ControlPaint.LightLight(DockingManager.BackColor);
            }

            // Is there any rectangle size to draw?
            if ((ClientRectangle.Width > 0) && (ClientRectangle.Height > 0))
            {
                // Fill the entire background area
                using (LinearGradientBrush backBrush = new LinearGradientBrush(ClientRectangle,
                                                                              caption1,
                                                                              caption2,
                                                                              (_dockLeft ? 0f : 90f)))
                {
                    // Draw the gradient background
                    e.Graphics.FillRectangle(backBrush, ClientRectangle);
                }
            }

            Size ourSize = this.ClientSize;

            // Depends on orientation
            if (_dockLeft)
            {
                using (Pen borderPen = new Pen(borderColor))
                    e.Graphics.DrawRectangle(borderPen, new Rectangle(0, 0, _fixedLength - 1, Height - 1));

                int yEnd = ourSize.Height - _buttonWidth - _buttonSpacer;
                int xEnd = ourSize.Width - _yInset * 2;

                Rectangle rectCaption = new Rectangle(_yInset, 0, xEnd - _yInset + 1, yEnd);

                // Draw the handle used on the left hand side to perform drag
                CommandDraw.DrawHandles(e.Graphics, VisualStyle.Office2003,
                                        new Rectangle(rectCaption.X, rectCaption.Bottom - _xInset - _spotWidth, rectCaption.Width, _spotWidth),
                                        spotsColor1, spotsColor2, false);

                // Start drawing text a little from the left
                rectCaption.Height -= _xInset + _spotWidth * 2;
                rectCaption.X += 1;
                rectCaption.Width -= 2;

                if (_showCloseButton)
                {
                    rectCaption.Y += _buttonWidth;
                    rectCaption.Height -= _buttonWidth;
                }

                if (_showHideButton && !_ignoreHideButton)
                {
                    rectCaption.Y += _buttonWidth;
                    rectCaption.Height -= _buttonWidth;
                }

                // Reduce width to account for maximize button
                if ((_maxButton != null) && _maxInterface.IsMaximizeAvailable())
                {
                    rectCaption.Y += _buttonWidth;
                    rectCaption.Height -= _buttonWidth;
                }

                using (StringFormat format = new StringFormat())
                {
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Center;
                    format.Trimming = StringTrimming.EllipsisCharacter;
                    format.FormatFlags = StringFormatFlags.NoWrap |
                                         StringFormatFlags.DirectionVertical;

                    using (Brush textBrush = new SolidBrush(textColor))
                        DrawHelper.DrawReverseString(e.Graphics, this.Text, DockingManager.CaptionFont, rectCaption, textBrush, format);
                }
            }
            else
            {
                using (Pen borderPen = new Pen(borderColor))
                    e.Graphics.DrawRectangle(borderPen, new Rectangle(0, 0, Width - 1, _fixedLength - 1));

                int xEnd = ourSize.Width - _buttonWidth - _buttonSpacer;
                int yEnd = ourSize.Height - _yInset * 2;

                Rectangle rectCaption = new Rectangle(0, _yInset, xEnd, yEnd - _yInset + 1);

                // Draw the handle used on the left hand side to perform drag
                CommandDraw.DrawHandles(e.Graphics, VisualStyle.Office2003,
                                        new Rectangle(rectCaption.X + _xInset, rectCaption.Y, _spotWidth, rectCaption.Height),
                                        spotsColor1, spotsColor2, true);

                // Start drawing text a little from the left
                rectCaption.X += _xInset + _spotWidth;
                rectCaption.Y += 1;
                rectCaption.Height -= 2;

                if (_showCloseButton)
                    rectCaption.Width -= _buttonWidth;

                if (_showHideButton && !_ignoreHideButton)
                    rectCaption.Width -= _buttonWidth;

                // Reduce width to account for maximize button
                if ((_maxButton != null) && _maxInterface.IsMaximizeAvailable())
                    rectCaption.Width -= _buttonWidth;

                using (StringFormat format = new StringFormat(StringFormatFlags.NoClip))
                {
                    format.Alignment = (RightToLeft == RightToLeft.Yes ? StringAlignment.Far : StringAlignment.Near);
                    format.LineAlignment = StringAlignment.Center;
                    format.Trimming = StringTrimming.EllipsisCharacter;
                    format.FormatFlags = StringFormatFlags.NoWrap;

                    using (Brush textBrush = new SolidBrush(textColor))
                    {
                        if (Apply2007ClearType)
                        {
                            using (UseClearTypeGridFit clearType = new UseClearTypeGridFit(e.Graphics))
                                e.Graphics.DrawString(this.Text, DockingManager.CaptionFont, textBrush, rectCaption, format);
                        }
                        else
                            e.Graphics.DrawString(this.Text, DockingManager.CaptionFont, textBrush, rectCaption, format);
                    }
                }
            }

            // Always get the button to repaint as we have painted over their area
            _closeButton.Refresh();
        }

        private void OnUserPreferenceChanged(object sender, Microsoft.Win32.UserPreferenceChangedEventArgs e)
        {
            // Use as a signal to retest the theme information
            _colorDetails.Reset();
        }
    }

    /// <summary>
    /// Derived class to draw the background to match the caption.
    /// </summary>
    [ToolboxItem(false)]
    public class Office2007CaptionButton : ButtonWithStyle
    {
        // Instance fields
        private WindowDetailCaptionOffice2007 _caption;

        /// <summary>
        /// Initialize a new instance of the Office2007CaptionButton class.
        /// </summary>
        /// <param name="caption"></param>
        public Office2007CaptionButton(WindowDetailCaptionOffice2007 caption)
        {
            // Remember back pointer
            _caption = caption;
        }

        /// <summary>
        /// Raises the PaintBackground event.
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // Find the parents caption client rectangle
            Rectangle captionRect = _caption.ClientRectangle;

            // Offset by our position within it
            captionRect.X -= this.Left;
            captionRect.Y -= this.Top;

            Color caption1;
            Color caption2;

            bool focused = false;

            if (_caption.ParentWindow != null)
                focused = _caption.ParentWindow.ContainsFocus;

            // Use correct colour depending on focused colour			
            if (focused)
            {
                if (_caption.DockingManager.DefaultActiveColor)
                {
                    caption1 = Office2007ColorTable.CheckedActiveLight(_caption.DockingManager.Style);
                    caption2 = Office2007ColorTable.CheckedActiveDark(_caption.DockingManager.Style);
                }
                else
                {
                    caption1 = ControlPaint.Light(_caption.DockingManager.ActiveColor);
                    caption2 = ControlPaint.Dark(_caption.DockingManager.ActiveColor);
                }
            }
            else
            {
                if (_caption.DockingManager.DefaultBackColor)
                {
                    caption1 = Office2007ColorTable.TitleActiveLight(_caption.DockingManager.Style);
                    caption2 = Office2007ColorTable.TitleActiveDark(_caption.DockingManager.Style);
                }
                else
                {
                    caption1 = ControlPaint.Light(_caption.DockingManager.BackColor);
                    caption2 = ControlPaint.Dark(_caption.DockingManager.BackColor);
                }
            }

            float angle = 90f;

            // Angle depends if used inside a left or top docking caption
            if ((Parent != null) && (Parent.Dock == DockStyle.Left))
                angle = 0f;

            // Is there any rectangle size to draw?
            if ((captionRect.Width > 0) && (captionRect.Height > 0))
            {
                // Fill the entire background area
                using (LinearGradientBrush backBrush = new LinearGradientBrush(captionRect,
                                                                               caption1,
                                                                               caption2,
                                                                               angle))
                {
                    // Draw the gradient background
                    pevent.Graphics.FillRectangle(backBrush, ClientRectangle);
                }
            }
        }
    }

    /// <summary>
    /// Window caption detail implementation for MediaPlayer visual styles.
    /// </summary>
    public class WindowDetailCaptionMediaPlayer : WindowDetailCaption
    {
        private enum ImageIndex
        {
            Close = 0,
            EnabledVerticalMax = 1,
            EnabledVerticalMin = 2,
            AutoHide = 3,
            AutoShow = 4
        }

        // Class constants
        private const int _xInset = 3;
        private const int _spotWidth = 6;
        private const int _yInset = 0;
        private const int _yInsetExtra = 5;
        private const int _imageWidth = 12;
        private const int _imageHeight = 11;
        private const int _buttonWidth = 14;
        private const int _buttonHeight = 13;
        private const int _buttonSpacer = 3;

        // Class fields
        private static ImageList _imagesOffice;
        private static int _fixedLength;

        // Instance fields
        private bool _dockLeft;
        private bool _applyMediaPlayerClearType;
        private ColorDetails _colorDetails;

        static WindowDetailCaptionMediaPlayer()
        {
            // Create a strip of images by loading an embedded bitmap resource
            _imagesOffice = ResourceHelper.LoadBitmapStrip(Type.GetType("Crownwood.DotNetMagic.Docking.WindowDetailCaptionIDE"),
                                                           "Crownwood.DotNetMagic.Resources.ImagesCaptionOffice2003.bmp",
                                                           new Size(_imageWidth, _imageHeight),
                                                           new Point(0, 0));
        }

        /// <summary>
        /// Initializes a new instance of the WindowDetailCaptionMediaPlayer class.
        /// </summary>
        /// <param name="manager">Reference to parent docking manager.</param>
        /// <param name="wc">Reference to owning window content.</param>
        /// <param name="closeHandler">Delegate for notifying close events.</param>
        /// <param name="restoreHandler">Delegate for notifying restore events.</param>
        /// <param name="invertAutoHideHandler">Delegate for auto hide invert events.</param>
        /// <param name="contextHandler">Delegate for notifying context menu events.</param>
        public WindowDetailCaptionMediaPlayer(DockingManager manager,
                                              WindowContent wc,
                                              EventHandler closeHandler,
                                              EventHandler restoreHandler,
                                              EventHandler invertAutoHideHandler,
                                              ContextHandler contextHandler)
            : base(manager, wc, 
                   new Size(_fixedLength, _fixedLength),
                   closeHandler,
                   restoreHandler,
                   invertAutoHideHandler,
                   contextHandler)
        {

            // Get the color helper for drawing
            _colorDetails = new ColorDetails();

            // Default to thinking we are docked on a left edge
            _dockLeft = false;

            // Default to using clear type on drawing text
            _applyMediaPlayerClearType = true;

            // Use specificed font in the caption 
            UpdateCaptionHeight(manager.CaptionFont);

            // We need to know when the system colours have changed
            Microsoft.Win32.SystemEvents.UserPreferenceChanged +=
                new Microsoft.Win32.UserPreferenceChangedEventHandler(OnUserPreferenceChanged);
        }

        /// <summary>
        /// Gets access to the colouring details.
        /// </summary>
        public ColorDetails ColorDetails
        {
            get { return _colorDetails; }
        }

        /// <summary>
        /// Gets and sets a value indicating if the Office 2007 style text should use ClearType.
        /// </summary>
        public bool ApplyMediaPlayerClearType
        {
            get { return _applyMediaPlayerClearType; }
            set { _applyMediaPlayerClearType = value; }
        }

        /// <summary>
        /// Propogate change in a property.
        /// </summary>
        /// <param name="name">Property that has changed.</param>
        /// <param name="value">New value.</param>
        public override void PropogateNameValue(PropogateName name, object value)
        {
            base.PropogateNameValue(name, value);

            switch (name)
            {
                case PropogateName.CaptionFont:
                    UpdateCaptionHeight((Font)value);
                    break;
                case PropogateName.ActiveTextColor:
                case PropogateName.InactiveTextColor:
                    DefineButtonRemapping();
                    Invalidate();
                    break;
                case PropogateName.ApplyMediaPlayerClearType:
                    ApplyMediaPlayerClearType = (bool)value;
                    Invalidate();
                    break;
            }
        }

        /// <summary>
        /// Handle the parent window getting focus.
        /// </summary>
        public override void WindowGotFocus()
        {
            SetButtonState();
            Invalidate();
        }

        /// <summary>
        /// Handle the parent window losing focus.
        /// </summary>
        public override void WindowLostFocus()
        {
            SetButtonState();
            Invalidate();
        }

        /// <summary>
        /// Handle a change in the caption bar text.
        /// </summary>
        /// <param name="title">New value of the property.</param>
        public override void NotifyFullTitleText(string title)
        {
            this.Text = title;
            Invalidate();
        }

        /// <summary>
        /// Handle a change in the state of the parent window.
        /// </summary>
        /// <param name="newState">New value of the property.</param>
        public override void ParentStateChanged(State newState)
        {
            // Should we dock to the left or top of our container?
            switch (newState)
            {
                case State.DockTop:
                case State.DockBottom:
                    if (DockingManager.AllowSideCaptions)
                    {
                        _dockLeft = true;
                        this.Dock = DockStyle.Left;
                    }
                    else
                    {
                        _dockLeft = false;
                        this.Dock = DockStyle.Top;
                    }
                    break;
                case State.Floating:
                case State.DockLeft:
                case State.DockRight:
                    _dockLeft = false;
                    this.Dock = DockStyle.Top;
                    break;
            }

            // Ignore the AutoHide feature when in floating form
            _ignoreHideButton = (ParentWindow.State == State.Floating);

            RecalculateButtons();
            Invalidate();
        }

        /// <summary>
        /// Handle the detail being removed from the parent window.
        /// </summary>
        /// <param name="parent">Window parent we are removed from.</param>
        public override void RemovedFromParent(Window parent)
        {
            if (parent != null)
            {
                if (this.Dock != DockStyle.None)
                {
                    Size minSize = parent.MinimalSize;

                    if (this.Dock == DockStyle.Left)
                    {
                        // Remove our width from the minimum size of the parent
                        minSize.Width -= _fixedLength;
                    }
                    else
                    {
                        // Remove our height from the minimum size of the parent
                        minSize.Height -= _fixedLength;
                    }

                    parent.MinimalSize = minSize;
                }
            }
        }

        /// <summary>
        /// Handle the detail being added to a parent window.
        /// </summary>
        /// <param name="parent">Window parent we are added to.</param>
        public override void AddedToParent(Window parent)
        {
            if (parent != null)
            {
                if (this.Dock != DockStyle.None)
                {
                    Size minSize = parent.MinimalSize;

                    if (this.Dock == DockStyle.Left)
                    {
                        // Add our width from the minimum size of the parent
                        minSize.Width += _fixedLength;
                    }
                    else
                    {
                        // Add our height from the minimum size of the parent
                        minSize.Height += _fixedLength;
                    }

                    parent.MinimalSize = minSize;
                }
            }
        }

        /// <summary>
        /// Let derived classes image specific images.
        /// </summary>
        protected override ImageList Images
        {
            get { return _imagesOffice; }
        }

        /// <summary>
        /// Dispose of instance resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Must unhook otherwise the instance cannot be garbage collected
                Microsoft.Win32.SystemEvents.UserPreferenceChanged -=
                    new Microsoft.Win32.UserPreferenceChangedEventHandler(OnUserPreferenceChanged);

                // Color details has resources that need releasing
                _colorDetails.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Raises the MouseMove event.
        /// </summary>
        /// <param name="e">A MouseEventArgs structure containing event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            Form parentForm = FindForm();

            if ((parentForm != null) && parentForm.ContainsFocus)
            {
                if (((!_dockLeft) && (e.X < (_xInset + _spotWidth))) ||
                    ((_dockLeft) && (e.Y > (Height - _xInset - _spotWidth))))
                    Cursor = Cursors.SizeAll;
                else
                    Cursor = Cursors.Default;
            }
            else
                Cursor = Cursors.Default;

            base.OnMouseMove(e);
        }

        /// <summary>
        /// Raises the MouseLeave event.
        /// </summary>
        /// <param name="e">An EventArgs structure containing event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            Cursor = Cursors.Default;
            base.OnMouseLeave(e);
        }

        /// <summary>
        /// Calculate which button recolors should be remapped.
        /// </summary>
        protected override void DefineButtonRemapping()
        {
            // Define use of current system colors
            ColorMap activeMap = new ColorMap();
            ColorMap inactiveMap = new ColorMap();

            activeMap.OldColor = Color.Black;

            if (DockingManager.DefaultActiveTextColor)
                activeMap.NewColor = MediaPlayerColorTable.TabActiveTextColor(DockingManager.Style, MediaPlayerStyle.LightEnhanced);
            else
                activeMap.NewColor = DockingManager.ActiveTextColor;

            inactiveMap.OldColor = Color.Black;

            if (DockingManager.DefaultInactiveTextColor)
                inactiveMap.NewColor = MediaPlayerColorTable.TitleActiveTextColor(DockingManager.Style);
            else
                inactiveMap.NewColor = DockingManager.InactiveTextColor;

            // Create remap attributes for use by button
            _activeAttr.SetRemapTable(new ColorMap[] { activeMap }, ColorAdjustType.Bitmap);
            _inactiveAttr.SetRemapTable(new ColorMap[] { inactiveMap }, ColorAdjustType.Bitmap);
        }

        /// <summary>
        /// Process the maximize interface being added.
        /// </summary>
        protected override void OnAddMaximizeInterface()
        {
            if (_maxButton != null)
            {
                // Set the correct size for the button
                _maxButton.Size = new Size(_buttonWidth, _buttonHeight);
                _maxButton.Style = DockingManager.Style;
                _maxButton.ImageAttributes = _inactiveAttr;

                // Force the ImageAttribute for the button to be set
                SetButtonState();
                Invalidate();
            }
        }

        /// <summary>
        /// Update the appearance of the auto hide image.
        /// </summary>
        protected override void UpdateAutoHideImage()
        {
            if (_pinnedImage)
                _hideButton.Image = Images.Images[(int)ImageIndex.AutoShow];
            else
                _hideButton.Image = Images.Images[(int)ImageIndex.AutoHide];
        }

        /// <summary>
        /// Update the appearance of the maximize image.
        /// </summary>
        protected override void UpdateMaximizeImage()
        {
            if ((_maxButton != null) && (_maxInterface != null))
            {
                bool enabled = _maxInterface.IsMaximizeAvailable();

                if (!enabled)
                {
                    if (_maxButton.Visible)
                        _maxButton.Hide();
                }
                else
                {
                    bool maximized = _maxInterface.IsWindowMaximized(this.ParentWindow);

                    if (!_maxButton.Visible)
                        _maxButton.Show();

                    if (maximized)
                        _maxButton.Image = Images.Images[(int)ImageIndex.EnabledVerticalMin];
                    else
                        _maxButton.Image = Images.Images[(int)ImageIndex.EnabledVerticalMax];
                }
            }
        }

        /// <summary>
        /// Update the state of the caption buttons.
        /// </summary>
        protected void SetButtonState()
        {
            if (this.ParentWindow != null)
            {
                if (this.ParentWindow.ContainsFocus && !DockingManager.DefaultActiveTextColor)
                {
                    _closeButton.ImageAttributes = _activeAttr;
                    _closeButton.Invalidate();

                    if (_hideButton != null)
                    {
                        _hideButton.ImageAttributes = _activeAttr;
                        _hideButton.Invalidate();
                    }

                    if (_maxButton != null)
                    {
                        _maxButton.ImageAttributes = _activeAttr;
                        _maxButton.Invalidate();
                    }
                }
                else
                {
                    _closeButton.ImageAttributes = _inactiveAttr;
                    _closeButton.Invalidate();

                    if (_hideButton != null)
                    {
                        _hideButton.ImageAttributes = _inactiveAttr;
                        _hideButton.Invalidate();
                    }

                    if (_maxButton != null)
                    {
                        _maxButton.ImageAttributes = _inactiveAttr;
                        _maxButton.Invalidate();
                    }
                }
            }
        }

        /// <summary>
        /// Calculate the visibility and position of buttons.
        /// </summary>
        protected override void RecalculateButtons()
        {
            if (_dockLeft)
            {
                int buttonX = (Width - _yInset * 2 - _buttonWidth) / 2 + _yInset + 1;
                int buttonY = _buttonSpacer;

                if (_showCloseButton)
                {
                    // Button position is fixed, regardless of our size
                    _closeButton.SetBounds(buttonX, buttonY, _buttonWidth, _buttonHeight);

                    // Let the location of the control be updated for us
                    _closeButton.Anchor = AnchorStyles.Top;

                    // Just in case currently hidden
                    _closeButton.Show();

                    // Define start of next button
                    buttonY += _buttonHeight;
                }
                else
                    _closeButton.Hide();

                if (_showHideButton && !_ignoreHideButton)
                {
                    // Button position is fixed, regardless of our size
                    _hideButton.SetBounds(buttonX, buttonY, _buttonWidth, _buttonHeight);

                    // Let the location of the control be updated for us
                    _hideButton.Anchor = AnchorStyles.Top;

                    // Just in case currently hidden
                    _hideButton.Show();

                    // Define start of next button
                    buttonY += _buttonHeight;

                    UpdateAutoHideImage();
                }
                else
                    _hideButton.Hide();

                if (_maxButton != null)
                {
                    // Button position is fixed, regardless of our size
                    _maxButton.SetBounds(buttonX, buttonY, _buttonWidth, _buttonHeight);

                    // Let the location of the control be updated for us
                    _maxButton.Anchor = AnchorStyles.Top;

                    // Define start of next button
                    buttonY += _buttonHeight;

                    UpdateMaximizeImage();
                }
            }
            else
            {
                int buttonX = this.Width - _buttonWidth - _buttonSpacer;
                int buttonY = (Height - _yInset * 2 - _buttonHeight) / 2 + _yInset;

                if (_showCloseButton)
                {
                    // Button position is fixed, regardless of our size
                    _closeButton.SetBounds(buttonX, buttonY, _buttonWidth, _buttonHeight);

                    // Let the location of the control be updated for us
                    _closeButton.Anchor = AnchorStyles.Right;

                    // Just in case currently hidden
                    _closeButton.Show();

                    // Define start of next button
                    buttonX -= _buttonWidth;
                }
                else
                    _closeButton.Hide();

                if (_showHideButton && !_ignoreHideButton)
                {
                    // Button position is fixed, regardless of our size
                    _hideButton.SetBounds(buttonX, buttonY, _buttonWidth, _buttonHeight);

                    // Let the location of the control be updated for us
                    _hideButton.Anchor = AnchorStyles.Right;

                    // Just in case currently hidden
                    _hideButton.Show();

                    // Define start of next button
                    buttonX -= _buttonWidth;

                    UpdateAutoHideImage();
                }
                else
                    _hideButton.Hide();

                if (_maxButton != null)
                {
                    // Button position is fixed, regardless of our size
                    _maxButton.SetBounds(buttonX, buttonY, _buttonWidth, _buttonHeight);

                    // Let the location of the control be updated for us
                    _maxButton.Anchor = AnchorStyles.Right;

                    // Define start of next button
                    buttonX -= _buttonWidth;

                    UpdateMaximizeImage();
                }
            }
        }

        /// <summary>
        /// Create the button class specific to this implementation.
        /// </summary>
        protected override void CreateMaxButton()
        {
            // Create the InertButton
            _maxButton = DockingManager.Factory.CreateMediaPlayerCaptionButton(this);
        }

        /// <summary>
        /// Create button required for caption.
        /// </summary>
        protected override void CreateButtons()
        {
            if (_closeButton == null)
            {
                // Define the ImageList and which ImageIndex to show initially
                _closeButton = DockingManager.Factory.CreateMediaPlayerCaptionButton(this);
                _closeButton.Image = Images.Images[(int)ImageIndex.Close];
                _closeButton.Style = DockingManager.Style;

                _hideButton = DockingManager.Factory.CreateMediaPlayerCaptionButton(this);
                _hideButton.Image = Images.Images[(int)ImageIndex.AutoHide];
                _hideButton.Style = DockingManager.Style;

                _closeButton.Size = new Size(_buttonWidth, _buttonHeight);
                _hideButton.Size = new Size(_buttonWidth, _buttonHeight);

                // Let the location of the control be updated for us
                _closeButton.Anchor = AnchorStyles.Right;
                _hideButton.Anchor = AnchorStyles.Right;

                // Define the button position relative to the size set in the constructor
                _closeButton.Location = new Point(_fixedLength - _buttonWidth - _buttonSpacer,
                                                 (_fixedLength - _yInset * 2 - _buttonHeight) / 2 + _yInset);

                _hideButton.Location = new Point(_fixedLength - (_buttonWidth - _buttonSpacer) * 2,
                                                (_fixedLength - _yInset * 2 - _buttonHeight) / 2 + _yInset);

                // Define correct color setup
                _closeButton.ImageAttributes = _inactiveAttr;
                _hideButton.ImageAttributes = _inactiveAttr;

                // Add to the display
                Controls.Add(_closeButton);
                Controls.Add(_hideButton);
            }

            // Let base class perform common actions
            base.CreateButtons();
        }

        /// <summary>
        /// Adjust the height of the caption to reflect changed font.
        /// </summary>
        /// <param name="captionFont"></param>
        protected void UpdateCaptionHeight(Font captionFont)
        {
            // Dynamically calculate the required height of the caption area
            _fixedLength = (int)captionFont.GetHeight() + (_yInset + _yInsetExtra) * 2;

            int minHeight = (_dockLeft ? (_buttonWidth + _yInset * 4 + 1) :
                                         (_buttonHeight + _yInset * 4 + 1));

            // Maintain a minimum height to allow correct showing of buttons
            if (_fixedLength < minHeight)
                _fixedLength = minHeight;

            this.Size = new Size(_fixedLength, _fixedLength);

            RecalculateButtons();

            Invalidate();
        }

        /// <summary>
        /// Paints the background of the control.
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains information about the control to paint.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Overriden to prevent background being painted
        }

        /// <summary>
        /// Raises the Paint event.
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data. </param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Sometimes the min/max button is created and then an attempt is made to 
            // hide the button. But for some reason it does not always manage to hide 
            // the button. So forced to check here everytime to ensure its hidden.
            UpdateMaximizeImage();

            SetButtonState();

            Color caption1;
            Color caption2;
            Color spotsColor1;
            Color spotsColor2;
            Color textColor;
            Color borderColor;

            bool focused = false;

            if (this.ParentWindow != null)
                focused = this.ParentWindow.ContainsFocus;

            // Choose the appropriate caption and text colours
            if (focused)
            {
                if (DockingManager.DefaultActiveColor)
                {
                    caption1 = MediaPlayerColorTable.CheckedActiveLight(DockingManager.Style);
                    caption2 = MediaPlayerColorTable.CheckedActiveDark(DockingManager.Style);
                }
                else
                {
                    caption1 = ControlPaint.Light(DockingManager.ActiveColor);
                    caption2 = ControlPaint.Dark(DockingManager.ActiveColor);
                }

            }
            else
            {
                if (DockingManager.DefaultBackColor)
                {
                    caption1 = MediaPlayerColorTable.TitleActiveLight(DockingManager.Style);
                    caption2 = MediaPlayerColorTable.TitleActiveDark(DockingManager.Style);
                }
                else
                {
                    caption1 = ControlPaint.Light(DockingManager.BackColor);
                    caption2 = ControlPaint.Dark(DockingManager.BackColor);
                }
            }

            textColor = MediaPlayerColorTable.TitleActiveTextColor(DockingManager.Style);
            borderColor = MediaPlayerColorTable.BorderColor(DockingManager.Style);

            // Choose the appropriate spot colours
            if (DockingManager.DefaultBackColor)
            {
                spotsColor1 = MediaPlayerColorTable.StatusBarGripDark(DockingManager.Style);
                spotsColor2 = MediaPlayerColorTable.StatusBarGripLight(DockingManager.Style);
            }
            else
            {
                spotsColor1 = ControlPaint.DarkDark(DockingManager.BackColor);
                spotsColor2 = ControlPaint.LightLight(DockingManager.BackColor);
            }

            // Is there any rectangle size to draw?
            if ((ClientRectangle.Width > 0) && (ClientRectangle.Height > 0))
            {
                // Fill the entire background area
                using (LinearGradientBrush backBrush = new LinearGradientBrush(ClientRectangle,
                                                                              caption1,
                                                                              caption2,
                                                                              (_dockLeft ? 0f : 90f)))
                {
                    // Draw the gradient background
                    e.Graphics.FillRectangle(backBrush, ClientRectangle);
                }
            }

            Size ourSize = this.ClientSize;

            // Depends on orientation
            if (_dockLeft)
            {
                using (Pen borderPen = new Pen(borderColor))
                    e.Graphics.DrawRectangle(borderPen, new Rectangle(0, 0, _fixedLength - 1, Height - 1));

                int yEnd = ourSize.Height - _buttonWidth - _buttonSpacer;
                int xEnd = ourSize.Width - _yInset * 2;

                Rectangle rectCaption = new Rectangle(_yInset, 0, xEnd - _yInset + 1, yEnd);

                // Draw the handle used on the left hand side to perform drag
                CommandDraw.DrawHandles(e.Graphics, VisualStyle.Office2003,
                                        new Rectangle(rectCaption.X, rectCaption.Bottom - _xInset - _spotWidth, rectCaption.Width, _spotWidth),
                                        spotsColor1, spotsColor2, false);

                // Start drawing text a little from the left
                rectCaption.Height -= _xInset + _spotWidth * 2;
                rectCaption.X += 1;
                rectCaption.Width -= 2;

                if (_showCloseButton)
                {
                    rectCaption.Y += _buttonWidth;
                    rectCaption.Height -= _buttonWidth;
                }

                if (_showHideButton && !_ignoreHideButton)
                {
                    rectCaption.Y += _buttonWidth;
                    rectCaption.Height -= _buttonWidth;
                }

                // Reduce width to account for maximize button
                if ((_maxButton != null) && _maxInterface.IsMaximizeAvailable())
                {
                    rectCaption.Y += _buttonWidth;
                    rectCaption.Height -= _buttonWidth;
                }

                using (StringFormat format = new StringFormat())
                {
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Center;
                    format.Trimming = StringTrimming.EllipsisCharacter;
                    format.FormatFlags = StringFormatFlags.NoWrap |
                                         StringFormatFlags.DirectionVertical;

                    using (Brush textBrush = new SolidBrush(textColor))
                        DrawHelper.DrawReverseString(e.Graphics, this.Text, DockingManager.CaptionFont, rectCaption, textBrush, format);
                }
            }
            else
            {
                using (Pen borderPen = new Pen(borderColor))
                    e.Graphics.DrawRectangle(borderPen, new Rectangle(0, 0, Width - 1, _fixedLength - 1));

                int xEnd = ourSize.Width - _buttonWidth - _buttonSpacer;
                int yEnd = ourSize.Height - _yInset * 2;

                Rectangle rectCaption = new Rectangle(0, _yInset, xEnd, yEnd - _yInset + 1);

                // Draw the handle used on the left hand side to perform drag
                CommandDraw.DrawHandles(e.Graphics, VisualStyle.Office2003,
                                        new Rectangle(rectCaption.X + _xInset, rectCaption.Y, _spotWidth, rectCaption.Height),
                                        spotsColor1, spotsColor2, true);

                // Start drawing text a little from the left
                rectCaption.X += _xInset + _spotWidth;
                rectCaption.Y += 1;
                rectCaption.Height -= 2;

                if (_showCloseButton)
                    rectCaption.Width -= _buttonWidth;

                if (_showHideButton && !_ignoreHideButton)
                    rectCaption.Width -= _buttonWidth;

                // Reduce width to account for maximize button
                if ((_maxButton != null) && _maxInterface.IsMaximizeAvailable())
                    rectCaption.Width -= _buttonWidth;

                using (StringFormat format = new StringFormat(StringFormatFlags.NoClip))
                {
                    format.Alignment = (RightToLeft == RightToLeft.Yes ? StringAlignment.Far : StringAlignment.Near);
                    format.LineAlignment = StringAlignment.Center;
                    format.Trimming = StringTrimming.EllipsisCharacter;
                    format.FormatFlags = StringFormatFlags.NoWrap;

                    using (Brush textBrush = new SolidBrush(textColor))
                    {
                        if (ApplyMediaPlayerClearType)
                        {
                            using (UseClearTypeGridFit clearType = new UseClearTypeGridFit(e.Graphics))
                                e.Graphics.DrawString(this.Text, DockingManager.CaptionFont, textBrush, rectCaption, format);
                        }
                        else
                            e.Graphics.DrawString(this.Text, DockingManager.CaptionFont, textBrush, rectCaption, format);
                    }
                }
            }

            // Always get the button to repaint as we have painted over their area
            _closeButton.Refresh();
        }

        private void OnUserPreferenceChanged(object sender, Microsoft.Win32.UserPreferenceChangedEventArgs e)
        {
            // Use as a signal to retest the theme information
            _colorDetails.Reset();
        }
    }

    /// <summary>
    /// Derived class to draw the background to match the caption.
    /// </summary>
    [ToolboxItem(false)]
    public class MediaPlayerCaptionButton : ButtonWithStyle
    {
        // Instance fields
        private WindowDetailCaptionMediaPlayer _caption;

        /// <summary>
        /// Initialize a new instance of the MediaPlayerCaptionButton class.
        /// </summary>
        /// <param name="caption"></param>
        public MediaPlayerCaptionButton(WindowDetailCaptionMediaPlayer caption)
        {
            // Remember back pointer
            _caption = caption;
            Style = caption.DockingManager.Style;
        }

        /// <summary>
        /// Raises the PaintBackground event.
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // Find the parents caption client rectangle
            Rectangle captionRect = _caption.ClientRectangle;

            // Offset by our position within it
            captionRect.X -= this.Left;
            captionRect.Y -= this.Top;

            Color caption1;
            Color caption2;

            bool focused = false;

            if (_caption.ParentWindow != null)
                focused = _caption.ParentWindow.ContainsFocus;

            // Use correct colour depending on focused colour			
            if (focused)
            {
                if (_caption.DockingManager.DefaultActiveColor)
                {
                    caption1 = MediaPlayerColorTable.CheckedActiveLight(_caption.DockingManager.Style);
                    caption2 = MediaPlayerColorTable.CheckedActiveDark(_caption.DockingManager.Style);
                }
                else
                {
                    caption1 = ControlPaint.Light(_caption.DockingManager.ActiveColor);
                    caption2 = ControlPaint.Dark(_caption.DockingManager.ActiveColor);
                }
            }
            else
            {
                if (_caption.DockingManager.DefaultBackColor)
                {
                    caption1 = MediaPlayerColorTable.TitleActiveLight(_caption.DockingManager.Style);
                    caption2 = MediaPlayerColorTable.TitleActiveDark(_caption.DockingManager.Style);
                }
                else
                {
                    caption1 = ControlPaint.Light(_caption.DockingManager.BackColor);
                    caption2 = ControlPaint.Dark(_caption.DockingManager.BackColor);
                }
            }

            float angle = 90f;

            // Angle depends if used inside a left or top docking caption
            if ((Parent != null) && (Parent.Dock == DockStyle.Left))
                angle = 0f;

            // Is there any rectangle size to draw?
            if ((captionRect.Width > 0) && (captionRect.Height > 0))
            {
                // Fill the entire background area
                using (LinearGradientBrush backBrush = new LinearGradientBrush(captionRect,
                                                                               caption1,
                                                                               caption2,
                                                                               angle))
                {
                    // Draw the gradient background
                    pevent.Graphics.FillRectangle(backBrush, ClientRectangle);
                }
            }
        }
    }

    /// <summary>
    /// Derived class to draw the background to match the caption.
    /// </summary>
    [ToolboxItem(false)]
    public class Office2003CaptionButton : ButtonWithStyle
    {
		// Instance fields
		private WindowDetailCaptionOffice2003 _caption;
		
		/// <summary>
		/// Initialize a new instance of the Office2003Button class.
		/// </summary>
		/// <param name="caption"></param>
		public Office2003CaptionButton(WindowDetailCaptionOffice2003 caption)
		{
			// Remember back pointer
			_caption = caption;
		}
    
		/// <summary>
		/// Raises the PaintBackground event.
		/// </summary>
		/// <param name="pevent"></param>
		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			// Find the parents caption client rectangle
			Rectangle captionRect = _caption.ClientRectangle;

			// Offset by our position within it
			captionRect.X -= this.Left;
			captionRect.Y -= this.Top;

			Color caption1;
			Color caption2;

			bool focused = false;

			if (_caption.ParentWindow != null)
				focused = _caption.ParentWindow.ContainsFocus;

			// Use correct colour depending on focused colour			
			if (focused)
			{
				if (_caption.DockingManager.DefaultActiveColor)
				{
					caption1 = _caption.ColorDetails.CaptionSelectColor1;
					caption2 = _caption.ColorDetails.CaptionSelectColor2;
				}
				else
				{
					caption1 = ControlPaint.Light(_caption.DockingManager.ActiveColor);
					caption2 = ControlPaint.Dark(_caption.DockingManager.ActiveColor);
				}
			}
			else
			{
				if (_caption.DockingManager.DefaultBackColor)
				{
					caption1 = _caption.ColorDetails.CaptionColor1;
					caption2 = _caption.ColorDetails.CaptionColor2;
				}	
				else
				{
					caption1 = ControlPaint.Light(_caption.DockingManager.BackColor);
					caption2 = ControlPaint.Dark(_caption.DockingManager.BackColor);
				}
			}

			float angle = 90f;

			// Angle depends if used inside a left or top docking caption
			if ((Parent != null) && (Parent.Dock == DockStyle.Left))
				angle = 0f;

			// Is there any rectangle size to draw?
			if ((captionRect.Width > 0) && (captionRect.Height > 0))
			{
				// Fill the entire background area
				using(LinearGradientBrush backBrush = new LinearGradientBrush(captionRect, 
																			  caption1, 
																			  caption2, 
																			  angle))
				{
					// Use signal bell curse to get more rounded look
					backBrush.SetSigmaBellShape(1f, 1f);
					
					// Draw the gradient background
					pevent.Graphics.FillRectangle(backBrush, ClientRectangle);
				}
			}
		}
    }

    /// <summary>
    /// Derived class to draw the background to match the caption.
    /// </summary>
    [ToolboxItem(false)]
    public class IDE2005CaptionButton : ButtonWithStyle
    {
		// Instance fields
		private WindowDetailCaptionIDE2005 _caption;
		
		/// <summary>
		/// Initialize a new instance of the IDE2005Button class.
		/// </summary>
		/// <param name="caption"></param>
		public IDE2005CaptionButton(WindowDetailCaptionIDE2005 caption)
		{
			// Remember back pointer
			_caption = caption;
		}
    
		/// <summary>
		/// Raises the PaintBackground event.
		/// </summary>
		/// <param name="pevent"></param>
		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			// Find the parents caption client rectangle
			Rectangle captionRect = _caption.ClientRectangle;

			// Offset by our position within it
			captionRect.X -= this.Left;
			captionRect.Y -= this.Top;

			bool focused = false;

			if (_caption.ParentWindow != null)
				focused = _caption.ParentWindow.ContainsFocus;

			if (!focused)
			{
				// When drawing the background without focus we extend an extra 15 
				// pixels so that the color does not bleed into the light color
				captionRect.Width += 15;
			}

			Color caption1;
			Color caption2;

			if (focused)
			{
				Color baseColor = _caption.DockingManager.ActiveColor;
				caption1 = ControlPaint.Light(SystemColors.Highlight);
				caption2 = SystemColors.Highlight;
			}
			else
			{
				caption1 = ColorHelper.MergeColors(SystemColors.Control, 0.5f, SystemColors.ControlDark, 0.5f);
				caption2 = caption1;
			}
			float angle = 90f;

			// Angle depends if used inside a left or top docking caption
			if ((Parent != null) && (Parent.Dock == DockStyle.Left))
				angle = 0f;

			// Is there any rectangle size to draw?
			if ((captionRect.Width > 0) && (captionRect.Height > 0))
			{
				// Fill the entire background area
				using(LinearGradientBrush backBrush = new LinearGradientBrush(captionRect, 
																			  caption1, 
																			  caption2, 
																			  angle))
				{
					// Draw the gradient background
					pevent.Graphics.FillRectangle(backBrush, ClientRectangle);
				}
			}
		}
    }
}
