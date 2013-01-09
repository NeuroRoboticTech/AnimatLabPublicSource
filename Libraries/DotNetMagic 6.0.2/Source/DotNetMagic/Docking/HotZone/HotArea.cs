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
	/// Define a hot area that results in visual feedback on drop information.
	/// </summary>
	public abstract class HotArea
	{
		// Instance fields
		private DockingManager _manager;

		/// <summary>
		/// Initialize a new instance of the HotAreaTabbed class.
		/// </summary>
		/// <param name="manager">Reference to docking manager.</param>
		public HotArea(DockingManager manager)
		{
			_manager = manager;
		}

		/// <summary>
		/// Gets access to the owning docking manager.
		/// </summary>
		public DockingManager Manager
		{
			get { return _manager; }
		}

		/// <summary>
		/// Find the new hot zone to use.
		/// </summary>
		/// <param name="mousePos">Screen mouse position.</param>
		/// <param name="hotZone">Incoming hot zone.</param>
		/// <param name="suppress">Suppress subsequent indicators.</param>
		/// <returns>New hot zone.</returns>
		public abstract HotZone FindHotZone(Point mousePos, HotZone hotZone, ref bool suppress);

		/// <summary>
		/// Cleanup because tracking has finished.
		/// </summary>
		public virtual void Cleanup()
		{
		}
	}
}
