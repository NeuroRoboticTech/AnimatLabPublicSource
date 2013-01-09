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
	public class HotAreaOutside : HotArea
	{
		// Instance fields
		private int _active;
		private bool _squares;
		private bool _hotInside;
		private Rectangle _outsideRect;
		private DropIndicators _dropLeft;
		private DropIndicators _dropRight;
		private DropIndicators _dropTop;
		private DropIndicators _dropBottom;
		private HotZoneReposition _hotLeft;
		private HotZoneReposition _hotRight;
		private HotZoneReposition _hotTop;
		private HotZoneReposition _hotBottom;
        private VisualStyle _style;

		/// <summary>
		/// Initialize a new instance of the HotAreaOutside class.
		/// </summary>
		/// <param name="squares">Squares or diamonds.</param>
		/// <param name="manager">Reference to docking manager.</param>
		/// <param name="outsideRect">Screen rectangle of outside container area.</param>
		/// <param name="vectorV">New vertical vector.</param>
		/// <param name="vectorH">New horizontal vector.</param>
        /// <param name="style">Drawing style.</param>
		public HotAreaOutside(bool squares,
							  DockingManager manager, 
							  Rectangle outsideRect,
							  int vectorV, 
							  int vectorH,
                              VisualStyle style)
			: base(manager)
		{
            // Cache the drawing style
            _style = style;

			// Remember initial values
			_squares = squares;
			_outsideRect = outsideRect;

			// Not currently inside the hot area
			_hotInside = false;

			// Create the hot zones, one per outside edge
			_hotLeft = new HotZoneReposition(outsideRect, new Rectangle(outsideRect.X, outsideRect.Y, vectorH, outsideRect.Height), State.DockLeft, false);
			_hotRight = new HotZoneReposition(outsideRect, new Rectangle(outsideRect.Right - vectorH, outsideRect.Y, vectorH, outsideRect.Height), State.DockRight, false);
			_hotTop = new HotZoneReposition(outsideRect, new Rectangle(outsideRect.X, outsideRect.Y, outsideRect.Width, vectorV), State.DockTop, false);
			_hotBottom = new HotZoneReposition(outsideRect, new Rectangle(outsideRect.X, outsideRect.Bottom - vectorV, outsideRect.Width, vectorV), State.DockBottom, false);
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
			bool hotInside = _outsideRect.Contains(mousePos) && !suppress;

			// Change in hot state?
			if (hotInside != _hotInside)
			{
				// Do we need to create the feedback indicators?
				if (hotInside)
				{
					// Create the different indicator instances
                    _dropLeft = new DropIndicators(_squares, false, true, false, false, false, _style);
                    _dropRight = new DropIndicators(_squares, false, false, true, false, false, _style);
                    _dropTop = new DropIndicators(_squares, false, false, false, true, false, _style);
                    _dropBottom = new DropIndicators(_squares, false, false, false, false, true, _style);

					// Ask the window to be shown within the provided rectangle
					_dropLeft.ShowRelative(_outsideRect);
					_dropRight.ShowRelative(_outsideRect);
					_dropTop.ShowRelative(_outsideRect);
					_dropBottom.ShowRelative(_outsideRect);
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
					active = _dropLeft.ScreenMouseMove(mousePos);
					if (active == 0) active = _dropRight.ScreenMouseMove(mousePos);
					if (active == 0) active = _dropTop.ScreenMouseMove(mousePos);
					if (active == 0) active = _dropBottom.ScreenMouseMove(mousePos);
				}

				// Only interested in a change of active value
				if (_active != active)
				{
					// Use new value
					_active = active;

					// If no longer need the indicators, remove them!
					if (_active == 0)
					{
						_dropLeft.MouseReset();
						_dropRight.MouseReset();
						_dropTop.MouseReset();
						_dropBottom.MouseReset();
					}
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
				// Remove indicators from display
				_dropLeft.Hide();
				_dropRight.Hide();
				_dropTop.Hide();
				_dropBottom.Hide();

				// Dispose of resources
				_dropLeft.Dispose();
				_dropRight.Dispose();
				_dropTop.Dispose();
				_dropBottom.Dispose();

				_hotInside = false;
			}
		}
	}
}
