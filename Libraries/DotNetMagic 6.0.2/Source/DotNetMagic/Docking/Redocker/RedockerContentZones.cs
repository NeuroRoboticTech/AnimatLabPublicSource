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
using System.Windows.Forms;
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Docking
{
	/// <summary>
	/// Manager for redocking of content instance.
	/// </summary>
    public class RedockerContentZones : RedockerContent
    {
        // Class constants
        private static int _hotVectorFromEdge = 2;
        private static int _hotVectorBeforeControl = 5;

		// Instance fields
        private HotZone _currentHotZone;
        private HotZoneCollection _hotZones;
		private DragFeedback _dragFeedback;

		/// <summary>
		/// Initializes a new instance of the RedockerContentZones class.
		/// </summary>
		/// <param name="squares">Show squares or diamonds.</param>
		/// <param name="callingControl">Calling control instance.</param>
		/// <param name="c">Source content.</param>
		/// <param name="wc">WindowContent that contains content.</param>
		/// <param name="offset">Screen offset.</param>
        public RedockerContentZones(bool squares,
									Control callingControl, 
									Content c, 
									WindowContent wc, 
									Point offset)
			: base(squares, callingControl, c, wc, offset)
        {
        }

		/// <summary>
		/// Initializes a new instance of the RedockerContentZones class.
		/// </summary>
		/// <param name="squares">Show squares or diamonds.</param>
		/// <param name="callingControl">Calling control instance.</param>
		/// <param name="wc">WindowContent that contains content.</param>
		/// <param name="offset">Screen offset.</param>
        public RedockerContentZones(bool squares,
									Control callingControl, 
									WindowContent wc, 
									Point offset)
			: base(squares, callingControl, wc, offset)
        {
        }

		/// <summary>
		/// Initializes a new instance of the RedockerContentZones class.
		/// </summary>
		/// <param name="squares">Show squares or diamonds.</param>
		/// <param name="ff">Floating form source.</param>
		/// <param name="offset">Screen offset.</param>
        public RedockerContentZones(bool squares, FloatingForm ff, Point offset)
			: base(squares, ff, offset)
        {
        }

		/// <summary>
		/// Perform initialization.
		/// </summary>
		/// <param name="squares">Show squares or diamonds.</param>
		/// <param name="callingControl">Calling control instance.</param>
		/// <param name="source">Type of source.</param>
		/// <param name="c">Source content.</param>
		/// <param name="wc">WindowContent that contains content.</param>
		/// <param name="ff">Floating form source.</param>
		/// <param name="dm">DockingManager instance.</param>
		/// <param name="offset">Screen offset.</param>
        protected override void InternalConstruct(bool squares,
												  Control callingControl, 
												  Source source, 
												  Content c, 
												  WindowContent wc, 
												  FloatingForm ff,
												  DockingManager dm,
												  Point offset)
        {
			// Initialize zone specific details
			_hotZones = null;
            _currentHotZone = null;

			// Let base class store information
			base.InternalConstruct(squares, callingControl, source, c, wc, ff, dm, offset);
        }

		/// <summary>
		/// Gets and sets the feedback class.
		/// </summary>
		protected DragFeedback DragFeedback
		{
			get { return _dragFeedback; }
			set { _dragFeedback = value; }
		}

		/// <summary>
		/// Enter hot tracking mode.
		/// </summary>
		public override void EnterTrackingMode()
        {
            // Have we entered tracking mode?
            if (!Tracking)
            {
                base.EnterTrackingMode();

                // Source must provide a valid manager instance
                if (DockingManager == null)
                    throw new ArgumentNullException("DockingManager");

                // Generate the hot spots that represent actions
                GenerateHotZones();

				// Start the feedback processing
				DragFeedback.Start(DockingManager.Style);
			}
        }

		/// <summary>
		/// Exit hot tracking mode.
		/// </summary>
		/// <param name="e">Mouse event information that triggered call.</param>
		/// <returns></returns>
		public override bool ExitTrackingMode(MouseEventArgs e)
        {
            // Have we exiting tracking mode?
            if (Tracking)
            {
                base.ExitTrackingMode(e);
	
				// Remove feedback from display
				DragFeedback.Quit();

				// Is there a current HotZone active?
                if (_currentHotZone != null)
                {
					// Convert from Control coordinates to screen coordinates
					Point mousePos = CallingControl.PointToScreen(new Point(e.X, e.Y));
					
					// Let the zone apply whatever change it represents
					bool ret = _currentHotZone.ApplyChange(mousePos, this);
					
					// If a change occured, need to recalculate sizing
					if (ret)
						DockingManager.CheckResized();
					
					return ret;
                }
            }

            return false;
        }

		/// <summary>
		/// Quit hot tracking mode.
		/// </summary>
		/// <param name="e">Mouse event information that triggered call.</param>
		public override void QuitTrackingMode(MouseEventArgs e)
        {
            // Have we quit tracking mode?
            if (Tracking)
            {
				if (CallingControl.Handle != IntPtr.Zero)
                {
                    // Remove any visible tracking indicator
                    if (_currentHotZone != null)
                        _currentHotZone.RemoveIndicator(DragFeedback, new Point(0,0));
                }
                                    
                base.QuitTrackingMode(e);

				// Remove feedback from display
				DragFeedback.Quit();
            }
        }

		/// <summary>
		/// Process a change in mouse position.
		/// </summary>
		/// <param name="e">Mouse event information that triggered call.</param>
		public override void OnMouseMove(MouseEventArgs e)
        {
			if (CallingControl.Handle != IntPtr.Zero)
			{
				// Convert from Control coordinates to screen coordinates
				Point mousePos = CallingControl.PointToScreen(new Point(e.X, e.Y));

				// Find HotZone this position is inside
				HotZone hz = _hotZones.Contains(mousePos);

				if (hz != _currentHotZone)
				{
					if (_currentHotZone != null)
						_currentHotZone.RemoveIndicator(DragFeedback, mousePos);

					_currentHotZone = hz;

					if (_currentHotZone != null)
						_currentHotZone.DrawIndicator(DragFeedback, mousePos);
				}
				else
				{
					if (_currentHotZone != null)
						_currentHotZone.UpdateForMousePosition(DragFeedback, mousePos, this);
				}
			}

            base.OnMouseMove(e);
        }

		/// <summary>
		/// Process a mouse button up call.
		/// </summary>
		/// <param name="e">Mouse event information that triggered call.</param>
		/// <returns>true if redocking action occured;false otherwise.</returns>
		public override bool OnMouseUp(MouseEventArgs e)
        {
			if (CallingControl.Handle != IntPtr.Zero)
			{
				if (_currentHotZone != null)
					_currentHotZone.RemoveIndicator(DragFeedback, CallingControl.PointToScreen(new Point(e.X, e.Y)));
			}

            return base.OnMouseUp(e);
        }

        private void GenerateHotZones()
        {
            // Need the client rectangle for the whole Form
            Rectangle formClient = Container.RectangleToScreen(Container.ClientRectangle);

            // Create a fresh collection for HotZones
            _hotZones = new HotZoneCollection();

            GenerateHotZonesForTop(_topList, formClient, VectorV, _outerIndex);
            GenerateHotZonesForLeft(_leftList, formClient, VectorH, _outerIndex);
            GenerateHotZonesForRight(_rightList, formClient, VectorH, _outerIndex);
            GenerateHotZonesForBottom(_bottomList, formClient, VectorV, _outerIndex);
            GenerateHotZonesForFill(_fillList, _outerIndex);

			// Allow provide the floating zone if we are allowed floating windows
			if (DockingManager.AllowFloating)
				GenerateHotZonesForFloating(SizeDependsOnSource());
        }

        private void GenerateHotZonesForLeft(ArrayList leftList, Rectangle formClient, int vector, int outerIndex)
        {
            foreach(Control c in leftList)
            {	
                bool ignoreType = (c is AutoHidePanel);
            
                int controlIndex = Container.Controls.IndexOf(c);

                if (!ignoreType && (controlIndex < outerIndex))
                {
                    // Grab the screen rectangle of the Control being processed
                    Rectangle hotArea = c.RectangleToScreen(c.ClientRectangle);

                    // Create the rectangle for the hot area
                    hotArea.Width = _hotVectorBeforeControl;

                    // Create the rectangle for the insertion indicator
                    Rectangle newSize = new Rectangle(hotArea.X, hotArea.Y, vector, hotArea.Height);
					
					hotArea.X += _hotVectorFromEdge;

                    // Create the new HotZone used to reposition a docking content/windowcontent
                    _hotZones.Add(new HotZoneReposition(hotArea, newSize, State.DockLeft, controlIndex));

                    IHotZoneSource ag = c as IHotZoneSource;

                    // Does this control expose an interface for its own HotZones?
                    if (ag != null)
                        ag.AddHotZones(this, _hotZones);
                }
            }

            // Grab the screen rectangle of the Control being processed
            Rectangle fullArea = _outsideRect;

            // Create the rectangle for the hot area
            fullArea.Width = _hotVectorFromEdge;

            // Create the rectangle for the insertion indicator
            Rectangle fillSize = new Rectangle(fullArea.X, fullArea.Y, vector, fullArea.Height);

            _hotZones.Add(new HotZoneReposition(fullArea, fillSize, State.DockLeft, false));

			// If performing our own InsideFill then do not dock at inner positions
			if (!DockingManager.InsideFill)
			{
				// Create the HotArea at the left side of the inner rectangle
				Rectangle innerHotArea = new Rectangle(_insideRect.X, _insideRect.Y, _hotVectorBeforeControl, _insideRect.Height);

				// Create the rectangle for tgqhe insertion indicator
				Rectangle innerNewSize = new Rectangle(innerHotArea.X, innerHotArea.Y, vector, innerHotArea.Height);

				// Create a HotZone for docking to the Left at the innermost position
				_hotZones.Add(new HotZoneReposition(innerHotArea, innerNewSize, State.DockLeft, true));
			}
        }

        private void GenerateHotZonesForRight(ArrayList rightList, Rectangle formClient, int vector, int outerIndex)
        {
            foreach(Control c in rightList)
            {	
                bool ignoreType = (c is AutoHidePanel);
            
                int controlIndex = Container.Controls.IndexOf(c);

                if (!ignoreType && (controlIndex < outerIndex))
                {
                    // Grab the screen rectangle of the Control being processed
                    Rectangle hotArea = c.RectangleToScreen(c.ClientRectangle);

                    // Create the rectangle for the hot area
                    hotArea.X = hotArea.Right - _hotVectorBeforeControl;
                    hotArea.Width = _hotVectorBeforeControl;

                    // Create the rectangle for the insertion indicator
                    Rectangle newSize = new Rectangle(hotArea.Right - vector, hotArea.Y, vector, hotArea.Height);
					
					hotArea.X -= _hotVectorFromEdge;

                    // Create the new HotZone used to reposition a docking content/windowcontent
                    _hotZones.Add(new HotZoneReposition(hotArea, newSize, State.DockRight, controlIndex));

                    IHotZoneSource ag = c as IHotZoneSource;

                    // Does this control expose an interface for its own HotZones?
                    if (ag != null)
                        ag.AddHotZones(this, _hotZones);
                }
            }

            // Grab the screen rectangle of the Control being processed
            Rectangle fullArea = _outsideRect;

            // Create the rectangle for the hot area
            fullArea.X = fullArea.Right - _hotVectorFromEdge;
            fullArea.Width = _hotVectorFromEdge;

            // Create the rectangle for the insertion indicator
            Rectangle fillSize = new Rectangle(fullArea.Right - vector, fullArea.Y, vector, fullArea.Height);

            _hotZones.Add(new HotZoneReposition(fullArea, fillSize, State.DockRight, false));

			// If performing our own InsideFill then do not dock at inner positions
			if (!DockingManager.InsideFill)
			{
				// Create the HotArea at the right side of the inner rectangle
				Rectangle innerHotArea = new Rectangle(_insideRect.Right - _hotVectorBeforeControl, _insideRect.Y, _hotVectorBeforeControl, _insideRect.Height);

				// Create the rectangle for the insertion indicator
				Rectangle innerNewSize = new Rectangle(innerHotArea.Right - vector, innerHotArea.Y, vector, innerHotArea.Height);

				// Create a HotZone for docking to the Left at the innermost position
				_hotZones.Add(new HotZoneReposition(innerHotArea, innerNewSize, State.DockRight, true));
			}
        }

        private void GenerateHotZonesForTop(ArrayList topList, Rectangle formClient, int vector, int outerIndex)
        {
            foreach(Control c in topList)
            {	
                bool ignoreType = (c is AutoHidePanel);
            
                int controlIndex = Container.Controls.IndexOf(c);

                if (!ignoreType && (controlIndex < outerIndex))
                {
                    // Grab the screen rectangle of the Control being processed
                    Rectangle hotArea = c.RectangleToScreen(c.ClientRectangle);

                    // Create the rectangle for the hot area
                    hotArea.Height = _hotVectorBeforeControl;

                    // Create the rectangle for the insertion indicator
                    Rectangle newSize = new Rectangle(hotArea.X, hotArea.Y, hotArea.Width, vector);
					
					hotArea.Y += _hotVectorFromEdge;

                    // Create the new HotZone used to reposition a docking content/windowcontent
                    _hotZones.Add(new HotZoneReposition(hotArea, newSize, State.DockTop, controlIndex));

                    IHotZoneSource ag = c as IHotZoneSource;

                    // Does this control expose an interface for its own HotZones?
                    if (ag != null)
                        ag.AddHotZones(this, _hotZones);
                }
            }

            // Grab the screen rectangle of the Control being processed
            Rectangle fullArea = _outsideRect;

            // Create the rectangle for the hot area
            fullArea.Height = _hotVectorFromEdge;

            // Create the rectangle for the insertion indicator
            Rectangle fillSize = new Rectangle(fullArea.X, fullArea.Y, fullArea.Width, vector);

            _hotZones.Add(new HotZoneReposition(fullArea, fillSize, State.DockTop, false));

			// If performing our own InsideFill then do not dock at inner positions
			if (!DockingManager.InsideFill)
			{
				// Create the HotArea at the left side of the inner rectangle
				Rectangle innerHotArea = new Rectangle(_insideRect.X, _insideRect.Y, _insideRect.Width, _hotVectorBeforeControl);

				// Create the rectangle for the insertion indicator
				Rectangle innerNewSize = new Rectangle(innerHotArea.X, innerHotArea.Y, innerHotArea.Width, vector);

				// Create a HotZone for docking to the Left at the innermost position
				_hotZones.Add(new HotZoneReposition(innerHotArea, innerNewSize, State.DockTop, true));
			}
        }

        private void GenerateHotZonesForBottom(ArrayList bottomList, Rectangle formClient, int vector, int outerIndex)
        {
            foreach(Control c in bottomList)
            {	
                bool ignoreType = (c is AutoHidePanel);
            
                int controlIndex = Container.Controls.IndexOf(c);

                if (!ignoreType && (controlIndex < outerIndex))
                {
                    // Grab the screen rectangle of the Control being processed
                    Rectangle hotArea = c.RectangleToScreen(c.ClientRectangle);

                    // Create the rectangle for the hot area
                    hotArea.Y = hotArea.Bottom - _hotVectorBeforeControl;
                    hotArea.Height = _hotVectorBeforeControl;

                    // Create the rectangle for the insertion indicator
                    Rectangle newSize = new Rectangle(hotArea.X, hotArea.Bottom - vector, hotArea.Width, vector);
					
					hotArea.Y -= _hotVectorFromEdge;

                    // Create the new HotZone used to reposition a docking content/windowcontent
                    _hotZones.Add(new HotZoneReposition(hotArea, newSize, State.DockBottom, controlIndex));

                    IHotZoneSource ag = c as IHotZoneSource;

                    // Does this control expose an interface for its own HotZones?
                    if (ag != null)
                        ag.AddHotZones(this, _hotZones);
                }
            }

            // Grab the screen rectangle of the Control being processed
            Rectangle fullArea = _outsideRect;

            // Create the rectangle for the hot area
            fullArea.Y = fullArea.Bottom - _hotVectorFromEdge;
            fullArea.Height = _hotVectorFromEdge;

            // Create the rectangle for the insertion indicator
            Rectangle fillSize = new Rectangle(fullArea.X, fullArea.Bottom - vector, fullArea.Width, vector);

            _hotZones.Add(new HotZoneReposition(fullArea, fillSize, State.DockBottom, false));

			// If performing our own InsideFill then do not dock at inner positions
			if (!DockingManager.InsideFill)
			{
				// Create the HotArea at the bottom of the inner rectangle
				Rectangle innerHotArea = new Rectangle(_insideRect.X, _insideRect.Bottom - _hotVectorBeforeControl, _insideRect.Width, _hotVectorBeforeControl);

				// Create the rectangle for the insertion indicator
				Rectangle innerNewSize = new Rectangle(innerHotArea.X, innerHotArea.Bottom - vector, innerHotArea.Width, vector);

				// Create a HotZone for docking to the Left at the innermost position
				_hotZones.Add(new HotZoneReposition(innerHotArea, innerNewSize, State.DockBottom, true));
			}
        }

        private void GenerateHotZonesForFill(ArrayList fillList, int outerIndex)
        {
            foreach(Control c in fillList)
            {	
                bool ignoreType = (c is AutoHidePanel);
            
                int controlIndex = Container.Controls.IndexOf(c);

                if (controlIndex < outerIndex)
                {
                    IHotZoneSource ag = c as IHotZoneSource;

                    // Does this control expose an interface for its own HotZones?
                    if (ag != null)
                        ag.AddHotZones(this, _hotZones);
                }
            }
        }

        private void GenerateHotZonesForFloating(Size sourceSize)
        {
            ScrollableControl main = DockingManager.Container;
            
			// Double check we have a defined container
			if (main != null)
			{
				Form mainForm = main.FindForm();

				// Double check we are part of a larger Form
				if (mainForm != null)
				{
					foreach(Form f in mainForm.OwnedForms)
					{
						// Cannot redock entire Floating form onto itself
						if ((f is FloatingForm) && (f != FloatingForm))
						{
							IHotZoneSource ag = f as IHotZoneSource;

							// Does this Form expose an interface for its own HotZones?
							if (ag != null)
								ag.AddHotZones(this, _hotZones);
						}
					}
				}
			}
             
            // Applies to the entire desktop area
            Rectangle hotArea = SystemInformation.VirtualScreen;

            // Position is determined by HotZone dynamically but the size is defined now
            Rectangle newSize = new Rectangle(0, 0, sourceSize.Width, sourceSize.Height);

            // Generate a catch all HotZone for floating a Content
            _hotZones.Add(new HotZoneFloating(hotArea, newSize, Offset, this)); 
        }
    }
}
