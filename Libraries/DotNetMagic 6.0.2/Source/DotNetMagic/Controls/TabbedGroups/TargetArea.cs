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

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Represents a target area for dragging and dropping.
	/// </summary>
	public abstract class TargetArea
	{	
		/// <summary>
		/// Gets the target associated with a mouse location.
		/// </summary>
		/// <param name="mousePos">Mouse position.</param>
		/// <param name="target">Incoming target.</param>
		/// <returns>Target instance; otherwise null.</returns>
		public abstract Target FindTarget(Point mousePos, Target target);
		
		/// <summary>
		/// Dragging operation has finished.
		/// </summary>
		public abstract void Quit();
	}
	
	/// <summary>
	/// Represents a target area for dragging and dropping.
	/// </summary>
	public class TargetAreaSides : TargetArea
	{
		// Instance fields
		private bool _squares;
		private int _active;
		private bool _hotInside;
		private Rectangle _screenRect;
		private Target _left;
		private Target _right;
		private Target _top;
		private Target _bottom;
		private DropIndicators _dropLeft;
		private DropIndicators _dropRight;
		private DropIndicators _dropTop;
		private DropIndicators _dropBottom;
        private VisualStyle _style;
		
		/// <summary>
		/// Initialize a new instance of the TargetArea class.
		/// </summary>
		/// <param name="squares">Showing as squares or diamonds.</param>
		/// <param name="host">Hosting tabbed groups instance.</param>
		/// <param name="leaf">Leaf that is being dragged.</param>
        /// <param name="style">Drawing style.</param>
		public TargetAreaSides(bool squares, 
                               TabbedGroups host, 
                               TabGroupLeaf leaf,
                               VisualStyle style)
		{
            // Cache the drawing style
            _style = style;
            
            // Remember host for later
			_squares = squares;
		
            // Get the total size of the tabbed groups control itself in screen coordinates
            _screenRect = host.RectangleToScreen(host.ClientRectangle);

            int horzThird = _screenRect.Width / 3;
            int vertThird = _screenRect.Height / 3;
	        
            // Create the four display rectangles
            Rectangle leftDisplay = new Rectangle(_screenRect.X, _screenRect.Y, horzThird, _screenRect.Height);
            Rectangle rightDisplay = new Rectangle(_screenRect.Right - horzThird, _screenRect.Y, horzThird, _screenRect.Height);
            Rectangle topDisplay = new Rectangle(_screenRect.X, _screenRect.Y, _screenRect.Width, vertThird);
            Rectangle bottomDisplay = new Rectangle(_screenRect.X, _screenRect.Bottom - vertThird, _screenRect.Width, vertThird);

            // Create the four default targets which represent each control edge
            _left = new Target(leftDisplay, leftDisplay, leaf, Target.TargetActions.ControlLeft);
            _right = new Target(rightDisplay, rightDisplay, leaf, Target.TargetActions.ControlRight);
            _top = new Target(topDisplay, topDisplay, leaf, Target.TargetActions.ControlTop);
            _bottom = new Target(bottomDisplay, bottomDisplay, leaf, Target.TargetActions.ControlBottom);           

			// Not currently inside the hot area
			_hotInside = false;
		}
		
		/// <summary>
		/// Gets the target associated with a mouse location.
		/// </summary>
		/// <param name="mousePos">Mouse position.</param>
		/// <param name="target">Incoming target.</param>
		/// <returns>Target instance; otherwise null.</returns>
		public override Target FindTarget(Point mousePos, Target target)
		{
			// Is the mouse contained in the inside client rectangle?
			bool hotInside = _screenRect.Contains(mousePos);

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
					_dropLeft.ShowRelative(_screenRect);
					_dropRight.ShowRelative(_screenRect);
					_dropTop.ShowRelative(_screenRect);
					_dropBottom.ShowRelative(_screenRect);
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
				if (target == null)
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
						target = _left;
						break;
					case 2:
						target = _right;
						break;
					case 3:
						target = _top;
						break;
					case 4:
						target = _bottom;
						break;
				}
			}

			return target;
		}		

		/// <summary>
		/// Dragging operation has finished.
		/// </summary>
		public override void Quit()
		{
			Cleanup();
		}
		
		private void Cleanup()
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

	/// <summary>
	/// Represents a target area for dragging and dropping.
	/// </summary>
	public class TargetAreaLeaf : TargetArea
	{
		// Instance fields
		private bool _squares;
		private int _active;
		private bool _hotInside;
		private Rectangle _screenRect;
		private Target _left;
		private Target _right;
		private Target _top;
		private Target _bottom;
		private Target _tabbed;
		private DropIndicators _drop;
        private VisualStyle _style;
		
		/// <summary>
		/// Initialize a new instance of the TargetAreaLeaf class.
		/// </summary>
		/// <param name="squares">Showing as squares or diamonds.</param>
		/// <param name="source">Leaf that is being dragged.</param>
		/// <param name="leaf">Leaf that is a potential target.</param>
        /// <param name="style">Drawing style.</param>
		public TargetAreaLeaf(bool squares, 
                              TabGroupLeaf source, 
                              TabGroupLeaf leaf,
                              VisualStyle style)
		{
            // Cache the drawing style
            _style = style;

			// Remember host for later
			_squares = squares;
		
	        // Grab the underlying tab control
	        Controls.TabControl tc = leaf.GroupControl as Controls.TabControl;

            // Get the total size of the tab control itself in screen coordinates
            _screenRect = tc.RectangleToScreen(tc.ClientRectangle);

            // Can only create new groups if moving relative to a new group 
	        // or we have more than one page in the originating group
	        if ((leaf != source) || ((leaf == source) && source.TabPages.Count > 1))
	        {
	            int horzThird = _screenRect.Width / 3;
	            int vertThird = _screenRect.Height / 3;
	        
                // Create the four spacing rectangle
                Rectangle leftRect = new Rectangle(_screenRect.X, _screenRect.Y, horzThird, _screenRect.Height);
                Rectangle rightRect = new Rectangle(_screenRect.Right - horzThird, _screenRect.Y, horzThird, _screenRect.Height);
                Rectangle topRect = new Rectangle(_screenRect.X, _screenRect.Y, _screenRect.Width, vertThird);
                Rectangle bottomRect = new Rectangle(_screenRect.X, _screenRect.Bottom - vertThird, _screenRect.Width, vertThird);
                
                // Add each new target
                _left = new Target(leftRect, leftRect, leaf, Target.TargetActions.GroupLeft);
                _right = new Target(rightRect, rightRect, leaf, Target.TargetActions.GroupRight);
                _top = new Target(topRect, topRect, leaf, Target.TargetActions.GroupTop);
                _bottom = new Target(bottomRect, bottomRect, leaf, Target.TargetActions.GroupBottom);
            }
	        
            // We do not allow a page to be transfered to its own leaf!
            if (leaf != source)
            {
				// Is the destination leaf allowed to accept a drop?
				if (leaf.AllowDrop)
				{
					// Any remaining space is used to 
					_tabbed = new Target(_screenRect, _screenRect, leaf, Target.TargetActions.Transfer);
				}
            }

			// Not currently inside the hot area
			_hotInside = false;
		}
		
		/// <summary>
		/// Are any actions allowed.
		/// </summary>
		public bool AllowAction
		{
			get 
			{
				return (_left != null) || (_right != null) ||
					   (_top != null) || (_bottom != null) ||
					   (_tabbed != null);
			}
		}
		
		/// <summary>
		/// Gets the target associated with a mouse location.
		/// </summary>
		/// <param name="mousePos">Mouse position.</param>
		/// <param name="target">Incoming target.</param>
		/// <returns>Target instance; otherwise null.</returns>
		public override Target FindTarget(Point mousePos, Target target)
		{
			// Is the mouse contained in the inside client rectangle?
			bool hotInside = AllowAction && _screenRect.Contains(mousePos);

			// Change in hot state?
			if (hotInside != _hotInside)
			{
				// Do we need to create the feedback indicator?
				if (hotInside)
				{
					// Create the indicator feedback
					_drop = new DropIndicators(_squares, (_tabbed != null), 
														 (_left != null), 
														 (_right != null),
														 (_top != null),
														 (_bottom != null),
                                                         _style);

					// Ask the window to be shown within the provided rectangle
					_drop.ShowRelative(_screenRect);
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
				if (target == null)
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
						target = _left;
						break;
					case 2:
						target = _right;
						break;
					case 3:
						target = _top;
						break;
					case 4:
						target = _bottom;
						break;
					case 5:
						target = _tabbed;
						break;
				}
			}

			return target;
		}		

		/// <summary>
		/// Dragging operation has finished.
		/// </summary>
		public override void Quit()
		{
			Cleanup();
		}
		
		private void Cleanup()
		{
			// If the indicators are currently showing
			if (_hotInside)
			{
				// Remove indicators from display
				_drop.Hide();

				// Dispose of resources
				_drop.Dispose();

				_hotInside = false;
			}
		}
	}

	/// <summary>
	/// Provides a collection container for TargetArea instances.
	/// </summary>
	public class TargetAreaCollection : CollectionWithEvents
	{
		/// <summary>
		/// Adds the specified TargetArea object to the collection.
		/// </summary>
		/// <param name="value">The TargetArea object to add to the collection.</param>
		/// <returns>The TargetArea object added to the collection.</returns>
		public TargetArea Add(TargetArea value)
		{
			// Use base class to process actual collection operation
			base.List.Add(value as object);

			return value;
		}

		/// <summary>
		/// Adds an array of TargetArea objects to the collection.
		/// </summary>
		/// <param name="values">An array of TargetArea objects to add to the collection.</param>
		public void AddRange(TargetArea[] values)
		{
			// Use existing method to add each array entry
			foreach(TargetArea area in values)
				Add(area);
		}

		/// <summary>
		/// Removes a TargetArea from the collection.
		/// </summary>
		/// <param name="value">A TargetArea to remove from the collection.</param>
		public void Remove(TargetArea value)
		{
			// Use base class to process actual collection operation
			base.List.Remove(value as object);
		}

		/// <summary>
		/// Inserts a TargetArea instance into the collection at the specified location.
		/// </summary>
		/// <param name="index">The location in the collection where you want to add the TargetArea.</param>
		/// <param name="value">The TargetArea object to insert.</param>
		public void Insert(int index, TargetArea value)
		{
			// Use base class to process actual collection operation
			base.List.Insert(index, value as object);
		}

		/// <summary>
		/// Determines whether a TargetArea is in the collection.
		/// </summary>
		/// <param name="value">The TargetArea to locate in the collection.</param>
		/// <returns>true if item is found in the collection; otherwise, false.</returns>
		public bool Contains(TargetArea value)
		{
			// Use base class to process actual collection operation
			return base.List.Contains(value as object);
		}

		/// <summary>
		/// Gets the TargetArea at the specified index.
		/// </summary>
		public TargetArea this[int index]
		{
			// Use base class to process actual collection operation
			get { return (base.List[index] as TargetArea); }
		}

		/// <summary>
		/// Returns the index of the first occurrence of the given TargetArea.
		/// </summary>
		/// <param name="value">The TargetArea to locate.</param>
		/// <returns>Index of object; otherwise -1</returns>
		public int IndexOf(TargetArea value)
		{
			// Find the 0 based index of the requested entry
			return base.List.IndexOf(value);
		}
	}
}
