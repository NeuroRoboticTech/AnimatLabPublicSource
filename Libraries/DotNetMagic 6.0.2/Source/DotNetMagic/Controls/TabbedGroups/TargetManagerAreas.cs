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
	/// Manage all targets used during a drag and drop operation for zone style redocking.
	/// </summary>
	public class TargetManagerAreas : TargetManager
	{
	    // Instance fields
        private TargetAreaCollection _targetAreas;
		private DragFeedbackSolid _dragFeedback;
	    
		/// <summary>
		/// Initializes a new instance of the TargetManagerAreas class.
		/// </summary>
		/// <param name="squares">Show as squares or diamonds.</param>
		/// <param name="host">Control that is requesting target management.</param>
		/// <param name="leaf">Source leaf causing drag operation.</param>
		/// <param name="source">Source TabControl of drag operation.</param>
	    public TargetManagerAreas(bool squares,
								  TabbedGroups host, 
							      TabGroupLeaf leaf, 
							      Controls.TabControl source)
			: base(squares, host, leaf, source)
	    {
	    }
        
		/// <summary>
		/// Remove any visual indications.
		/// </summary>
		public override void Quit()
		{
			// Remove drawing of any feedback indicator
			_dragFeedback.Quit();
		
			// Tell each area to quit
			foreach(TargetArea area in _targetAreas)
				area.Quit();
		}
            
		/// <summary>
		/// Class specific initialization.
		/// </summary>
		protected override void Initialize()
		{
			// Create collection to hold generated targets
			_targetAreas = new TargetAreaCollection();
	        
			// Create the top level drop targets
			_targetAreas.Add(new TargetAreaSides(Squares, Host, Leaf, Host.Style));
	        
			// Process each potential leaf in turn
			TabGroupLeaf tgl = Host.FirstLeaf();
	        
			while(tgl != null)
			{
				// Create all possible targets for this leaf
                _targetAreas.Add(new TargetAreaLeaf(Squares, Leaf, tgl, Host.Style));
	        
				// Enumerate all leafs
				tgl = Host.NextLeaf(tgl);
			}
			
			// Create the drag feedback drawing class
			_dragFeedback = new DragFeedbackSolid();
            _dragFeedback.Start(Host.Style);
		}
		
		/// <summary>
		/// Gets the target associated with a mouse location.
		/// </summary>
		/// <param name="mousePos">Mouse position.</param>
		/// <returns>Target instance; otherwise null.</returns>
		protected override Target FindTarget(Point mousePos)
		{
			Target target = null;
			
			// Ask each area in turn to update the target
			foreach(TargetArea area in _targetAreas)
				target = area.FindTarget(mousePos, target);
				
			return target;
		}
				
		/// <summary>
		/// Update feedback for the new target.
		/// </summary>
		/// <param name="t">Target instance.</param>
		protected override void UpdateFeedbackForTarget(Target t)
		{
			// Update drag feedback for the new target
			if (t != null)
				_dragFeedback.DragRectangle(t.DrawRect);
			else
				_dragFeedback.DragRectangle(Rectangle.Empty);
		}
	}
}
