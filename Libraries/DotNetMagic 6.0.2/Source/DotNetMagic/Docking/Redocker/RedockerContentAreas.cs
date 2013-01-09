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
    public class RedockerContentAreas : RedockerContent
    {
		// Instance fields
		private HotZone _currentHotZone;
		private HotAreaCollection _hotAreas;
        private DragFeedbackSolid _dragFeedback;
		private DockingManager _dockingManager;

		/// <summary>
		/// Initializes a new instance of the RedockerContentAreas class.
		/// </summary>
		/// <param name="squares">Show squares or diamonds.</param>
		/// <param name="callingControl">Calling control instance.</param>
		/// <param name="c">Source content.</param>
		/// <param name="wc">WindowContent that contains content.</param>
		/// <param name="offset">Screen offset.</param>
        public RedockerContentAreas(bool squares,
									Control callingControl, 
									Content c, 
									WindowContent wc, 
									Point offset)
			: base(squares, callingControl, c, wc, offset)
        {
        }

		/// <summary>
		/// Initializes a new instance of the RedockerContentAreas class.
		/// </summary>
		/// <param name="squares">Show squares or diamonds.</param>
		/// <param name="callingControl">Calling control instance.</param>
		/// <param name="wc">WindowContent that contains content.</param>
		/// <param name="offset">Screen offset.</param>
        public RedockerContentAreas(bool squares,
									Control callingControl, 
									WindowContent wc, 
									Point offset)
			: base(squares, callingControl, wc, offset)
        {
		}

		/// <summary>
		/// Initializes a new instance of the RedockerContentAreas class.
		/// </summary>
		/// <param name="squares">Show squares or diamonds.</param>
		/// <param name="ff">Floating form source.</param>
		/// <param name="offset">Screen offset.</param>
        public RedockerContentAreas(bool squares,
									FloatingForm ff, 
									Point offset)
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
			_currentHotZone = null;
			_dockingManager = dm;
            _dragFeedback = new DragFeedbackSolid();

			// Let base class store information
			base.InternalConstruct(squares, callingControl, source, c, wc, ff, dm, offset);
        }

		/// <summary>
		/// Gets and sets the feedback class.
		/// </summary>
		protected DragFeedback DragFeedback
		{
			get { return _dragFeedback; }
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
                GenerateHotAreas();

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

				// Ensure any additional indicators are removed
				CleanupHotAreas();

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

				// Ensure any additional indicators are removed
				CleanupHotAreas();
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
				HotZone hz = FindHotZone(mousePos);

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

        private void GenerateHotAreas()
        {
			// Create the standard hot areas and add to the collection
			_hotAreas = new HotAreaCollection();

			// Moving over a floating window takes precedence over the main applicaion
			GenerateFloatingHotAreas();

			_hotAreas.Add(new HotAreaOutside(Squares, _dockingManager, _outsideRect, VectorV, VectorH, DockingManager.Style));

			// With InsideFill enable we cannot do inside fill operations
			if (!_dockingManager.InsideFill)
                _hotAreas.Add(new HotAreaInside(Squares, _dockingManager, _insideRect, VectorV, VectorH, DockingManager.Style));
			
			// Generate areas for each of the tabbed content instancess
			GenerateTabbedHotAreas(_leftList, false);
			GenerateTabbedHotAreas(_rightList, false);
			GenerateTabbedHotAreas(_topList, false);
			GenerateTabbedHotAreas(_bottomList, false);
            GenerateTabbedHotAreas(_fillList, false);
			
			if (_dockingManager.AllowFloating)
				_hotAreas.Add(new HotAreaFloating(this, _dockingManager));
        }
        
		private void GenerateFloatingHotAreas()
		{
            ScrollableControl main = _dockingManager.Container;
            
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
							// Cast to correct type
							FloatingForm ff = f as FloatingForm;

							// Create a list with just the zone inside it
							ArrayList controlList = new ArrayList();
							controlList.Add(ff.Zone);

							// Generate the hot areas for the contents of the zone
							GenerateTabbedHotAreas(controlList, true);
						}
					}
				}
			}
		}

		private void GenerateTabbedHotAreas(ArrayList controlList, bool suppress)
        {
			// Check each control in turn
			foreach(Control c in controlList)
			{	
				ZoneSequence zs = c as ZoneSequence;
				
				// Look for zone sequences
				if (zs != null)
				{
					// Find index within the list of child controls
					int controlIndex = Container.Controls.IndexOf(c);

					// Only interested in controls associated with docking
					if (controlIndex < _outerIndex)
					{
						// Process all children, looking for tabbed content
						foreach(Control zc in zs.Controls)
						{
							WindowContentTabbed wct = zc as WindowContentTabbed;
							
							// Only interested in providing indicators for the WCT
							if (wct != null)
							{
								// Not allowed to redock a content back to itself
								bool disallowTabbed = (WindowContent != null) && (WindowContent == wct);
								
                                // If the manager says tabbed not allowed, then disallow it
                                if (!DockingManager.AllowTabbed)
                                    disallowTabbed = true;

								// Generate new hot area for the tabbed content window
                                _hotAreas.Add(new HotAreaTabbed(Squares, this, _dockingManager, zs, wct, disallowTabbed, suppress, DockingManager.Style));
							}
						}
					}
				}
			}
		}

		private HotZone FindHotZone(Point mousePos)
		{
			bool suppress = false;
			HotZone hotZone = null;

			// Ask each hot area in turn for a hot zone to use
			foreach(HotArea area in _hotAreas)
				hotZone = area.FindHotZone(mousePos, hotZone, ref suppress);

			return hotZone;
		}

		private void CleanupHotAreas()
		{
			// Ask each hot area in turn to perform cleanup
			foreach(HotArea area in _hotAreas)
				area.Cleanup();
		}
    }
}
