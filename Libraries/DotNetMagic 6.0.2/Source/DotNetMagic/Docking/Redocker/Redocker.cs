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
	/// Base class used for managing redocking activity.
	/// </summary>
    public class Redocker
    {
        // Instance fields
        private bool _tracking;

		/// <summary>
		/// Initializes a new instance of the Redocker class.
		/// </summary>
        public Redocker()
        {
            // Default the state
            _tracking = false;
        }

		/// <summary>
		/// Gets a value indicating is tracking is occuring.
		/// </summary>
        public bool Tracking
        {
            get { return _tracking; }
        }

		/// <summary>
		/// Enter hot tracking mode.
		/// </summary>
        public virtual void EnterTrackingMode()
        {
            if (!_tracking)
                _tracking = true;
        }

		/// <summary>
		/// Exit hot tracking mode.
		/// </summary>
		/// <param name="e">Mouse event information that triggered call.</param>
		/// <returns></returns>
        public virtual bool ExitTrackingMode(MouseEventArgs e)
        {
            if (_tracking)
                _tracking = false;

            return false;
        }

		/// <summary>
		/// Quit hot tracking mode.
		/// </summary>
		/// <param name="e">Mouse event information that triggered call.</param>
        public virtual void QuitTrackingMode(MouseEventArgs e)
        {
            if (_tracking)
                _tracking = false;
        }

		/// <summary>
		/// Process a change in mouse position.
		/// </summary>
		/// <param name="e">Mouse event information that triggered call.</param>
        public virtual void OnMouseMove(MouseEventArgs e) {}

		/// <summary>
		/// Process a mouse button up call.
		/// </summary>
		/// <param name="e">Mouse event information that triggered call.</param>
		/// <returns>true if redocking action occured;false otherwise.</returns>
        public virtual bool OnMouseUp(MouseEventArgs e)
        {
            if (_tracking)
            {
                if (e.Button == MouseButtons.Left)
                    return ExitTrackingMode(e);
                else
                    QuitTrackingMode(e);
            }

            return false;
        }

		/// <summary>
		/// Create appropriate redocker for type of feedback.
		/// </summary>
		/// <param name="feedbackStyle">Feedback style requested.</param>
		/// <param name="ff">Floating form source.</param>
		/// <param name="offset">Screen offset.</param>
		/// <returns>Redocker instance.</returns>
		public static RedockerContent CreateRedocker(DragFeedbackStyle feedbackStyle,
													 FloatingForm ff, 
													 Point offset)
		{
			switch(feedbackStyle)
			{
				case DragFeedbackStyle.Squares:
					return new RedockerContentAreas(true, ff, offset);
				case DragFeedbackStyle.Diamonds:
					return new RedockerContentAreas(false, ff, offset);
				case DragFeedbackStyle.Solid:
					return new RedockerContentSolid(false, ff, offset);
				case DragFeedbackStyle.Outline:
				default:
					return new RedockerContentOutline(false, ff, offset);
			}
		}

		/// <summary>
		/// Initializes a new instance of the RedockerContent class.
		/// </summary>
		/// <param name="feedbackStyle">Feedback style requested.</param>
		/// <param name="callingControl">Calling control instance.</param>
		/// <param name="wc">WindowContent that contains content.</param>
		/// <param name="offset">Screen offset.</param>
        public static RedockerContent CreateRedocker(DragFeedbackStyle feedbackStyle,
													 Control callingControl, 
													 WindowContent wc, 
													 Point offset)
        {
			switch(feedbackStyle)
			{
				case DragFeedbackStyle.Squares:
					return new RedockerContentAreas(true, callingControl, wc, offset);
				case DragFeedbackStyle.Diamonds:
					return new RedockerContentAreas(false, callingControl, wc, offset);
				case DragFeedbackStyle.Solid:
					return new RedockerContentSolid(false, callingControl, wc, offset);
				case DragFeedbackStyle.Outline:
				default:
					return new RedockerContentOutline(false, callingControl, wc, offset);
			}
		}

		/// <summary>
		/// Initializes a new instance of the RedockerContent class.
		/// </summary>
		/// <param name="feedbackStyle">Feedback style requested.</param>
		/// <param name="callingControl">Calling control instance.</param>
		/// <param name="c">Source content.</param>
		/// <param name="wc">WindowContent that contains content.</param>
		/// <param name="offset">Screen offset.</param>
        public static RedockerContent CreateRedocker(DragFeedbackStyle feedbackStyle,
													 Control callingControl, 
													 Content c, 
													 WindowContent wc, 
													 Point offset)
        {
			switch(feedbackStyle)
			{
				case DragFeedbackStyle.Squares:
					return new RedockerContentAreas(true, callingControl, c, wc, offset);
				case DragFeedbackStyle.Diamonds:
					return new RedockerContentAreas(false, callingControl, c, wc, offset);
				case DragFeedbackStyle.Solid:
					return new RedockerContentSolid(false, callingControl, c, wc, offset);
				case DragFeedbackStyle.Outline:
				default:
					return new RedockerContentOutline(false, callingControl, c, wc, offset);
			}
		}
    }
}
