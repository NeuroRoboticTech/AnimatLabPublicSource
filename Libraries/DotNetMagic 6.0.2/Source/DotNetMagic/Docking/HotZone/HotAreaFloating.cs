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

namespace Crownwood.DotNetMagic.Docking
{
	/// <summary>
	/// Shows the floating position indicator if this area if needed.
	/// </summary>
	public class HotAreaFloating : HotArea
	{
		// Instance fields
		private HotZoneFloating _floatingZone;

		/// <summary>
		/// Initialize a new instance of the HotAreaInside class.
		/// </summary>
		/// <param name="redocker">Redocking implementation class.</param>
		/// <param name="manager">Reference to docking manager.</param>
		public HotAreaFloating(RedockerContent redocker,
							   DockingManager manager)
			: base(manager)
		{
			_floatingZone = new HotZoneFloating(Rectangle.Empty, 
												new Rectangle(Point.Empty, redocker.SizeDependsOnSource()), 
												redocker.Offset, 
											    redocker);
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
			// If no zone currently created
			if (hotZone == null)
				hotZone = _floatingZone;

			return hotZone;
		}
	}
}
