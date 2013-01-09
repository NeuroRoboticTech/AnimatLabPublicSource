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
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Docking
{
	/// <summary>
	/// Handle the mouse over a tabbed content instance.
	/// </summary>
	public class HotAreaTabbed : HotArea
	{
		// Instance fields
		private int _active;
		private bool _squares;
		private bool _hotInside;
		private bool _disallowTabbed;
		private bool _disallowInsert;
		private bool _suppress;
		private Rectangle _tabbedRect;
		private ZoneSequence _zs;
		private WindowContentTabbed _wct;
		private DropIndicators _drop;
		private HotZoneSequence _hotLT;
		private HotZoneSequence _hotRB;
		private HotZoneTabbed _hotMiddle;
        private VisualStyle _style;

		/// <summary>
		/// Initialize a new instance of the HotAreaTabbed class.
		/// </summary>
		/// <param name="squares">Squares or diamonds.</param>
		/// <param name="redocker">Reference to the redocker.</param>
		/// <param name="manager">Reference to docking manager.</param>
		/// <param name="zs">Sequence containing the tabbed content.</param>
		/// <param name="wct">Tabbed content instance.</param>
		/// <param name="disallowTabbed">Allow to drop in tabbed area.</param>
		/// <param name="suppress">Suppress indicators if match found.</param>
        /// <param name="style">Drawing style.</param>
		public HotAreaTabbed(bool squares, 
							 RedockerContent redocker,
							 DockingManager manager,
							 ZoneSequence zs,
							 WindowContentTabbed wct,
							 bool disallowTabbed,
							 bool suppress,
                             VisualStyle style)
			: base(manager)
		{
            // Cache the drawing style
            _style = style;
            
            // Remember initial values
			_wct = wct;
			_zs = zs;
			_squares = squares;
			_suppress = suppress;
			_disallowTabbed = disallowTabbed;

			// Not currently inside the hot area
			_hotInside = false;
			_disallowInsert = false;

			// Calculate the tabbed rectangle in screen coordinates
			_tabbedRect = wct.RectangleToScreen(wct.ClientRectangle);

            // Check for situations that need extra attention...
            //
            //  (1) Do not allow a WindowContent from a ZoneSequence to be redocked into the
            //      same ZoneSequence. As removing it will cause the Zone to be destroyed and
            //      so it cannot be added back again. Is not logical anyway.
            //
            //  (2) If the source is in this ZoneSequence we might need to adjust the insertion
            //      index because the item being removed will reduce the count for when the insert
            //		takes place.

            WindowContent redockWC = redocker.WindowContent;

            if (_zs.Windows.Count == 1)
            {
                if (redocker.WindowContent != null)
                    if (redocker.WindowContent == _zs.Windows[0])
                        if ((redocker.Content == null) || (redocker.WindowContent.Contents.Count == 1))
                            _disallowInsert = true;
            }

			// Create the left/top and right/bottom hot zones
			if (!_disallowInsert)
			{
				Rectangle zoneRectLT = zs.RectangleToScreen(zs.ClientRectangle);
				Rectangle tabbedRect = zs.Windows[0].RectangleToScreen(zs.Windows[0].ClientRectangle);

				// Adjust the rectangle to give rough idea of new size
				if (zs.Direction == LayoutDirection.Vertical)
				{
					zoneRectLT.X = tabbedRect.X;
					zoneRectLT.Width = tabbedRect.Width;
					zoneRectLT.Height = zoneRectLT.Height / (zs.Windows.Count + 1);
				}
				else
				{
					zoneRectLT.Y = tabbedRect.Y;
					zoneRectLT.Height = tabbedRect.Height;
					zoneRectLT.Width = zoneRectLT.Width / (zs.Windows.Count + 1);
				}

				// Store the rectangle for use with the RB as well
				Rectangle zoneRectRB = zoneRectLT;
				
				// Find our current index as starting point for insertion
				int indexLT = zs.Windows.IndexOf(wct);

				// The RB is inserted after the current position
				int indexRB = indexLT + 1;

				// If inserting after the first window
				if (indexLT > 0)
				{
					// Move the indicator position to overlap the previous position
					if (zs.Direction == LayoutDirection.Vertical)
						zoneRectLT.Y += zs.Windows[indexLT].Top - (zoneRectLT.Height / 2);
					else
						zoneRectLT.X += zs.Windows[indexLT].Left - (zoneRectLT.Width / 2);
				}

				// Create the actual hot zone
				_hotLT = new HotZoneSequence(zoneRectLT, zoneRectLT, zs, indexLT);

				// If inserting before the last window
				if (indexRB < zs.Windows.Count)
				{
					// Move the indicator position to overlap the next position
					if (zs.Direction == LayoutDirection.Vertical)
						zoneRectRB.Y += zs.Windows[indexRB-1].Bottom - (zoneRectRB.Height / 2);
					else
						zoneRectRB.X += zs.Windows[indexRB-1].Right - (zoneRectRB.Width / 2);
				}
				else
				{
					// Move the indicator position to the end of our position
					if (zs.Direction == LayoutDirection.Vertical)
						zoneRectRB.Y += zs.Windows[indexRB-1].Bottom - zoneRectRB.Height;
					else
						zoneRectRB.X += zs.Windows[indexRB-1].Right - zoneRectRB.Width;
				}

				// Create the actual hot zone
				_hotRB = new HotZoneSequence(zoneRectRB, zoneRectRB, zs, indexRB);
			}

			// Create the insert into middle hot zone
			if (!_disallowTabbed)
				_hotMiddle = new HotZoneTabbed(_tabbedRect, _tabbedRect, wct, false);
		}

		/// <summary>
		/// Find the new hot zone to use.
		/// </summary>
		/// <param name="mousePos">Screen mouse position.</param>
		/// <param name="hotZone">Incoming hot zone.</param>
		/// <param name="suppress">Suppress subsequent indicators.</param>
		/// <returns>New hot zone.</returns>
		public override HotZone FindHotZone(Point mousePos, HotZone hotZone, ref bool suppress)
		{
			// Is the mouse contained in the inside hot rectangle?
			bool hotInside = _tabbedRect.Contains(mousePos) && !suppress;

			// Change in hot state?
			if (hotInside != _hotInside)
			{
				// Do we need to create the feedback indicator?
				if (hotInside)
				{
					// Indicators depends on direction of zone
					bool vert = (_zs.Direction == LayoutDirection.Vertical);
					
					// Create the indicator feedback
					_drop = new DropIndicators(_squares, 
											   !_disallowTabbed, 
											   !vert && !_disallowInsert, 
											   !vert && !_disallowInsert, 
											    vert && !_disallowInsert, 
											    vert && !_disallowInsert,
                                                _style);

					// Ask the window to be shown within the provided rectangle
					_drop.ShowRelative(_tabbedRect);
				}
				else
				{
					_active = 0;
					Cleanup();
				}

				// Update state
				_hotInside = hotInside;
			}

			// Check the mouse position against the available drop indicators
			if (_hotInside)
			{
				int active = 0;

				// Can only test against indicators if hot zone not already defined
				if (hotZone == null)
				{
					// Find the newly active indicator
					active = _drop.ScreenMouseMove(mousePos);
				}

				// Only interested in a change of active value
				if (_active != active)
				{
					// Use new value
					_active = active;

					// If no longer need the indicators, remove them!
					if (_active == 0)
						_drop.MouseReset();
				}

				// Use the current active hot zone
				switch(_active)
				{
					case 1:
					case 3:
						hotZone = _hotLT;
						break;
					case 2:
					case 4:
						hotZone = _hotRB;
						break;
					case 5:
						hotZone = _hotMiddle;
						break;
				}
			}

			// Do we suppress subsequenct indicators?
			if (_hotInside && _suppress)
				suppress = true;
			
			return hotZone;
		}
		
		/// <summary>
		/// Cleanup because tracking has finished.
		/// </summary>
		public override void Cleanup()
		{
			// If the indicators are currently showing
			if (_hotInside)
			{
				// Remove indicator from display
				_drop.Hide();
				_drop.Dispose();

				_hotInside = false;
			}
		}
	}
}
