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
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Docking
{
	/// <summary>
	/// Base class for specific hot zones.
	/// </summary>
    public class HotZone
    {
		// Class fields
		private const int DRAG_WIDTH = 4;

        // Instance fields
        private Rectangle _hotArea;
        private Rectangle _newSize;

		/// <summary>
		/// Initializes a new instance of the HotZone class.
		/// </summary>
		/// <param name="hotArea">Screen area that is hot.</param>
		/// <param name="newSize">Screen area to display with rectangle.</param>
        public HotZone(Rectangle hotArea, Rectangle newSize)
        {
            // Store initial state
            _hotArea = hotArea;
            _newSize = newSize;
        }

		/// <summary>
		/// Gets the screen hot area.
		/// </summary>
        public Rectangle HotArea
        {
            get { return _hotArea; }
        }

		/// <summary>
		/// Gets and sets the screen display rectangle area.
		/// </summary>
        public Rectangle NewSize
        {
            get { return _newSize; }
			set { _newSize = value; }
        }

		/// <summary>
		/// Gets the width of the drag rectangle.
		/// </summary>
		public int DragWidth
		{
			get { return DRAG_WIDTH; }
		}

		/// <summary>
		/// Apply the hot zone change.
		/// </summary>
		/// <param name="screenPos">Screen position when change applied.</param>
		/// <param name="parent">Parent redocker instance.</param>
		/// <returns>true is successful; otherwise false.</returns>
        public virtual bool ApplyChange(Point screenPos, Redocker parent) { return false; }

		/// <summary>
		/// Update the screen indication to reflect new mouse position.
		/// </summary>
		/// <param name="dragFeedback">Feedback class.</param>
		/// <param name="screenPos">New screen position.</param>
		/// <param name="parent">Parent redocker instance.</param>
        public virtual void UpdateForMousePosition(DragFeedback dragFeedback, 
												   Point screenPos,
												   Redocker parent) {}

		/// <summary>
		/// Draw the zone indicator to the screen.
		/// </summary>
		/// <param name="dragFeedback">Feedback class.</param>
		/// <param name="mousePos">Screen position of mouse.</param>
        public virtual void DrawIndicator(DragFeedback dragFeedback, Point mousePos) 
        {
            DrawFeedback(dragFeedback, _newSize);
        }
		
		/// <summary>
		/// Remove the zone indicator from the screen.
		/// </summary>
		/// <param name="dragFeedback">Feedback class.</param>
		/// <param name="mousePos">Screen position of mouse.</param>
        public virtual void RemoveIndicator(DragFeedback dragFeedback, Point mousePos) 
        {
            DrawFeedback(dragFeedback, _newSize);
        }

		/// <summary>
		/// Draw a reversible rectangle on the screen.
		/// </summary>
		/// <param name="dragFeedback">Feedback class.</param>
		/// <param name="rect">Screen bounding rectangle.</param>
        public virtual void DrawFeedback(DragFeedback dragFeedback, Rectangle rect)
        {
            dragFeedback.DragRectangle(rect);
        }
    }
}
