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
	/// Hot zone representing no action.
	/// </summary>
    public class HotZoneNull : HotZone
    {
		/// <summary>
		/// Initializes a new instance of the HotZoneFloating class.
		/// </summary>
		/// <param name="hotArea">Screen area that is hot.</param>
        public HotZoneNull(Rectangle hotArea)
            : base(hotArea, hotArea)
        {
        }

		/// <summary>
		/// Draw the zone indicator to the screen.
		/// </summary>
		/// <param name="dragFeedback">Feedback class.</param>
		/// <param name="mousePos">Screen position of mouse.</param>
		public override void DrawIndicator(DragFeedback dragFeedback, Point mousePos) {}

		/// <summary>
		/// Remove the zone indicator from the screen.
		/// </summary>
		/// <param name="dragFeedback">Feedback class.</param>
		/// <param name="mousePos">Screen position of mouse.</param>
		public override void RemoveIndicator(DragFeedback dragFeedback, Point mousePos) {}
    }
}
