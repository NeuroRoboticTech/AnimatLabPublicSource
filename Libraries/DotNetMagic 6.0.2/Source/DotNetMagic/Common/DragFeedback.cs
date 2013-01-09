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

namespace Crownwood.DotNetMagic.Common
{
	/// <summary>
	/// Base class from which feedback implementation derived.
	/// </summary>
	public abstract class DragFeedback
	{
		/// <summary>
		/// Called at start of drag feedback process.
		/// </summary>
        /// <param name="style">Drawing style.</param>
		public abstract void Start(VisualStyle style);

		/// <summary>
		/// Called to request drag feedback for the new rectangle.
		/// </summary>
		/// <param name="newRect">New drag feedback rectangle.</param>
		public abstract void DragRectangle(Rectangle newRect);

		/// <summary>
		/// Called to request drag feedback for the new rectangles.
		/// </summary>
		/// <param name="newRects">New drag feedback rectangles.</param>
		public abstract void DragRectangles(Rectangle[] newRects);

		/// <summary>
		/// Called at end of drag feedback process.
		/// </summary>
		public abstract void Quit();
	}

	/// <summary>
	/// Drag feedback implemented as stippled line around area.
	/// </summary>
	public class DragFeedbackOutline : DragFeedback
	{
		// Constants
	    private const int _rectWidth = 4;

		// Instance fields
		private Rectangle[] _lastRects;

		/// <summary>
		/// Called at start of drag feedback process.
		/// </summary>
        /// <param name="style">Drawing style.</param>
        public override void Start(VisualStyle style)
		{
			// Do not have a last rectangle drawn
			_lastRects = null;
		}

		/// <summary>
		/// Called to request drag feedback for the new rectangle.
		/// </summary>
		/// <param name="newRect">New drag feedback rectangle.</param>
		public override void DragRectangle(Rectangle newRect)
		{
			// If we are provided with a valid rectangle to draw
			if (newRect.Size != Size.Empty)
			{
				// And there is some stored rectangles to undraw
				if (_lastRects != null)
				{
					// Create new list for old and the new rectangles
					Rectangle[] drawRects = new Rectangle[_lastRects.Length + 1];

					// Copy across old rects
					for(int i=0; i<_lastRects.Length; i++)
						drawRects[i] = _lastRects[i];

					// Add the new rect
					drawRects[_lastRects.Length] = newRect;

					// Draw all of them, thus removing the old and adding the new
					DrawHelper.DrawDragRectangles(drawRects, _rectWidth);
				}
				else
				{
					// Just need to draw the new rectangle
					DrawHelper.DrawDragRectangle(newRect, _rectWidth);
				}

				// Only need to remember the new one
				_lastRects = new Rectangle[]{newRect};
			}
			else
			{
				// And there is some stored rectangles to undraw
				if (_lastRects != null)
				{
					// Draw all of them, thus removing the old ones
					DrawHelper.DrawDragRectangles(_lastRects, _rectWidth);
				}

				// Nothing to remember
				_lastRects = null;
			}
		}

		/// <summary>
		/// Called to request drag feedback for the new rectangles.
		/// </summary>
		/// <param name="newRects">New drag feedback rectangles.</param>
		public override void DragRectangles(Rectangle[] newRects)
		{
			// And there is some stored rectangles to undraw
			if (_lastRects != null)
			{
				// Create new list for old and the new rectangles
				Rectangle[] drawRects = new Rectangle[_lastRects.Length + newRects.Length];

				// Copy across old rects
				for(int i=0; i<_lastRects.Length; i++)
					drawRects[i] = _lastRects[i];

				// Copy across new rects
				for(int i=0; i<newRects.Length; i++)
					drawRects[_lastRects.Length + i] = newRects[i];

				// Draw all of them, thus removing the old and adding the new
				DrawHelper.DrawDragRectangles(drawRects, _rectWidth);
			}
			else
			{
				// Just need to draw the new rectangle
				DrawHelper.DrawDragRectangles(newRects, _rectWidth);
			}

			// Remember new rectangles
			_lastRects = newRects;
		}

		/// <summary>
		/// Called at end of drag feedback process.
		/// </summary>
		public override void Quit()
		{
			// If there rectangles to undraw?
			if (_lastRects != null)
			{
				// Undraw by drawing all rects over themselves
				DrawHelper.DrawDragRectangles(_lastRects, _rectWidth);

				// Remove the rectangles
				_lastRects = null;
			}
		}
	}

	/// <summary>
	/// Drag feedback implemented as semi-transparent area.
	/// </summary>
	public class DragFeedbackSolid : DragFeedback
	{
		// Instance fields
		private DropSolid _solid;

		/// <summary>
		/// Called at start of drag feedback process.
		/// </summary>
        /// <param name="style">Drawing style.</param>
        public override void Start(VisualStyle style)
		{
			// Create a window for showing drop area semi-transparent
			_solid = new DropSolid(style);

			// Show the window sized as 1 pixel to get it drawing
			_solid.SetBounds(0, 0, 1, 1, BoundsSpecified.All);

			// Place window on the screen
			_solid.ShowWithoutActivate();

			// Force window to be painted
			_solid.Refresh();
		}

		/// <summary>
		/// Called to request drag feedback for the new rectangle.
		/// </summary>
		/// <param name="newRect">New drag feedback rectangle.</param>
		public override void DragRectangle(Rectangle newRect)
		{
			// Feedback just the provided rectangle
			_solid.SolidRect = newRect;
		}

		/// <summary>
		/// Called to request drag feedback for the new rectangles.
		/// </summary>
		/// <param name="newRects">New drag feedback rectangles.</param>
		public override void DragRectangles(Rectangle[] newRects)
		{
			// Feedback the aggregate of all the rectangles
			_solid.SolidRects = newRects;
		}

		/// <summary>
		/// Called at end of drag feedback process.
		/// </summary>
		public override void Quit()
		{
			// Cleanup the no longer required feedback window
			if (_solid != null)
			{
				_solid.Dispose();
				_solid = null;
			}
		}
	}
}
