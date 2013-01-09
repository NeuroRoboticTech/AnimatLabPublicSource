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
	/// Manager for redocking of content instance and using Solid as the feedback.
	/// </summary>
    public class RedockerContentSolid : RedockerContentZones
    {
		/// <summary>
		/// Initializes a new instance of the RedockerContentSolid class.
		/// </summary>
		/// <param name="squares">Show squares or diamonds.</param>
		/// <param name="ff">Floating form source.</param>
		/// <param name="offset">Screen offset.</param>
        public RedockerContentSolid(bool squares, FloatingForm ff, Point offset)
			: base(squares, ff, offset)
		{
		}

		/// <summary>
		/// Initializes a new instance of the RedockerContentSolid class.
		/// </summary>
		/// <param name="squares">Show squares or diamonds.</param>
		/// <param name="callingControl">Calling control instance.</param>
		/// <param name="wc">WindowContent that contains content.</param>
		/// <param name="offset">Screen offset.</param>
        public RedockerContentSolid(bool squares,
									Control callingControl, 
									WindowContent wc, 
									Point offset)
			: base(squares, callingControl, wc, offset)
        {
        }

		/// <summary>
		/// Initializes a new instance of the RedockerContentSolid class.
		/// </summary>
		/// <param name="squares">Show squares or diamonds.</param>
		/// <param name="callingControl">Calling control instance.</param>
		/// <param name="c">Source content.</param>
		/// <param name="wc">WindowContent that contains content.</param>
		/// <param name="offset">Screen offset.</param>
        public RedockerContentSolid(bool squares,
									Control callingControl, 
									Content c, 
									WindowContent wc, 
									Point offset)
			: base(squares, callingControl, c, wc, offset)
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
			// Create the outline specific feedback indicator
			DragFeedback = new DragFeedbackSolid();

			// Carry on with internal setup
			base.InternalConstruct(squares, callingControl, source, c, wc, ff, dm, offset);
        }
    }
}
