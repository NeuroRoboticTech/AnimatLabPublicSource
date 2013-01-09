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
	/// Handle the mouse over the client area of a docking container.
	/// </summary>
	public class HotAreaInside : HotArea
	{
		// Instance fields
		private int _active;
		private bool _squares;
		private bool _hotInside;
		private Rectangle _insideRect;
		private DropIndicators _drop;
		private HotZoneReposition _hotLeft;
		private HotZoneReposition _hotRight;
		private HotZoneReposition _hotTop;
		private HotZoneReposition _hotBottom;
        private VisualStyle _style;

		/// <summary>
		/// Initialize a new instance of the HotAreaInside class.
		/// </summary>
		/// <param name="squares">Squares or diamonds.</param>
		/// <param name="manager">Reference to docking manager.</param>
		/// <param name="insideRect">Screen rectangle of inside container area.</param>
		/// <param name="vectorV">New vertical vector.</param>
		/// <param name="vectorH">New horizontal vector.</param>
        /// <param name="style">Drawing style.</param>
        public HotAreaInside(bool squares,
							 DockingManager manager, 
							 Rectangle insideRect,
							 int vectorV, 
							 int vectorH,
                             VisualStyle style)
			: base(manager)
		{
            // Cache the drawing style
            _style = style;
            
            // Remember initial values
			_squares = squares;
			_insideRect = insideRect;

			// Not currently inside the hot area
			_hotInside = false;

			// Create the hot zones, one per outside edge
			_hotLeft = new HotZoneReposition(insideRect, new Rectangle(insideRect.X, insideRect.Y, vectorH, insideRect.Height), State.DockLeft, true);
			_hotRight = new HotZoneReposition(insideRect, new Rectangle(insideRect.Right - vectorH, insideRect.Y, vectorH, insideRect.Height), State.DockRight, true);
			_hotTop = new HotZoneReposition(insideRect, new Rectangle(insideRect.X, insideRect.Y, insideRect.Width, vectorV), State.DockTop, true);
			_hotBottom = new HotZoneReposition(insideRect, new Rectangle(insideRect.X, insideRect.Bottom - vectorV, insideRect.Width, vectorV), State.DockBottom, true);
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
			bool hotInside = _insideRect.Contains(mousePos) && !suppress;

			// Change in hot state?
			if (hotInside != _hotInside)
			{
				// Do we need to create the feedback indicator?
				if (hotInside)
				{
					// Create the indicator feedback
                    _drop = new DropIndicators(_squares, false, true, true, true, true, _style);

					// Ask the window to be shown within the provided rectangle
					_drop.ShowRelative(_insideRect);
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
						hotZone = _hotLeft;
						break;
					case 2:
						hotZone = _hotRight;
						break;
					case 3:
						hotZone = _hotTop;
						break;
					case 4:
						hotZone = _hotBottom;
						break;
				}
			}

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
