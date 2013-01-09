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
	public abstract class TargetManagerZones : TargetManager
	{
	    // Class fields
        private const int _hotVector = 4;
	
	    // Instance fields
        private TargetCollection _targets;
	    
		/// <summary>
		/// Initializes a new instance of the TargetManagerZones class.
		/// </summary>
		/// <param name="squares">Show as squares or diamonds.</param>
		/// <param name="host">Control that is requesting target management.</param>
		/// <param name="leaf">Source leaf causing drag operation.</param>
		/// <param name="source">Source TabControl of drag operation.</param>
	    public TargetManagerZones(bool squares,
								  TabbedGroups host, 
							      TabGroupLeaf leaf, 
							      Controls.TabControl source)
			: base(squares, host, leaf, source)
	    {
	    }
            
		/// <summary>
		/// Class specific initialization.
		/// </summary>
		protected override void Initialize()
		{
			// Create collection to hold generated targets
			_targets = new TargetCollection();
	        
			// Create the top level drop targets
			CreateControlTargets(Host, Leaf);
	        
			// Process each potential leaf in turn
			TabGroupLeaf tgl = Host.FirstLeaf();
	        
			while(tgl != null)
			{
				// Create all possible targets for this leaf
				CreateLeafTargets(tgl);
	        
				// Enumerate all leafs
				tgl = Host.NextLeaf(tgl);
			}
		}

		/// <summary>
		/// Gets the target associated with a mouse location.
		/// </summary>
		/// <param name="mousePos"></param>
		/// <returns>Target instance; otherwise null.</returns>
		protected override Target FindTarget(Point mousePos)
		{
			return _targets.Contains(mousePos);
		}

		private void CreateControlTargets(TabbedGroups host, TabGroupLeaf leaf)
        {
            // Get the total size of the tabbed groups control itself in screen coordinates
            Rectangle totalSize = host.RectangleToScreen(host.ClientRectangle);

            int horzThird = totalSize.Width / 3;
            int vertThird = totalSize.Height / 3;
	        
            // Create the four display rectangles
            Rectangle leftDisplay = new Rectangle(totalSize.X, totalSize.Y, horzThird, totalSize.Height);
            Rectangle rightDisplay = new Rectangle(totalSize.Right - horzThird, totalSize.Y, horzThird, totalSize.Height);
            Rectangle topDisplay = new Rectangle(totalSize.X, totalSize.Y, totalSize.Width, vertThird);
            Rectangle bottomDisplay = new Rectangle(totalSize.X, totalSize.Bottom - vertThird, totalSize.Width, vertThird);

            // Create the four hot rectangles
            Rectangle leftHot = new Rectangle(totalSize.X, totalSize.Y, _hotVector, totalSize.Height);
            Rectangle rightHot = new Rectangle(totalSize.Right - _hotVector, totalSize.Y, _hotVector, totalSize.Height);
            Rectangle topHot = new Rectangle(totalSize.X, totalSize.Y, totalSize.Width, _hotVector);
            Rectangle bottomHot = new Rectangle(totalSize.X, totalSize.Bottom - _hotVector, totalSize.Width, _hotVector);

            // Create the four default targets which represent each control edge
            _targets.Add(new Target(leftHot, leftDisplay, leaf, Target.TargetActions.ControlLeft));
            _targets.Add(new Target(rightHot, rightDisplay, leaf, Target.TargetActions.ControlRight));
            _targets.Add(new Target(topHot, topDisplay, leaf, Target.TargetActions.ControlTop));
            _targets.Add(new Target(bottomHot, bottomDisplay, leaf, Target.TargetActions.ControlBottom));           
        }
	    
	    private void CreateLeafTargets(TabGroupLeaf leaf)
	    {
	        // Grab the underlying tab control
	        Controls.TabControl tc = leaf.GroupControl as Controls.TabControl;

            // Get the total size of the tab control itself in screen coordinates
            Rectangle totalSize = tc.RectangleToScreen(tc.ClientRectangle);

            // We do not allow a page to be transfered to its own leaf!
            if (leaf != Leaf)
            {
				// Is the destination leaf allowed to accept a drop?
				if (leaf.AllowDrop)
				{
					Rectangle tabsSize = tc.RectangleToScreen(tc.TabsAreaRect);

					// Give priority to the tabs area being used to transfer page
					_targets.Add(new Target(tabsSize, totalSize, leaf, Target.TargetActions.Transfer));
				}
            }
	        
            // Can only create new groups if moving relative to a new group 
	        // or we have more than one page in the originating group
	        if ((leaf != Leaf) || ((leaf == Leaf) && Leaf.TabPages.Count > 1))
	        {
	            int horzThird = totalSize.Width / 3;
	            int vertThird = totalSize.Height / 3;
	        
                // Create the four spacing rectangle
                Rectangle leftRect = new Rectangle(totalSize.X, totalSize.Y, horzThird, totalSize.Height);
                Rectangle rightRect = new Rectangle(totalSize.Right - horzThird, totalSize.Y, horzThird, totalSize.Height);
                Rectangle topRect = new Rectangle(totalSize.X, totalSize.Y, totalSize.Width, vertThird);
                Rectangle bottomRect = new Rectangle(totalSize.X, totalSize.Bottom - vertThird, totalSize.Width, vertThird);
                
                // Add each new target
                _targets.Add(new Target(leftRect, leftRect, leaf, Target.TargetActions.GroupLeft));
                _targets.Add(new Target(rightRect, rightRect, leaf, Target.TargetActions.GroupRight));
                _targets.Add(new Target(topRect, topRect, leaf, Target.TargetActions.GroupTop));
                _targets.Add(new Target(bottomRect, bottomRect, leaf, Target.TargetActions.GroupBottom));
            }
	        
            // We do not allow a page to be transfered to its own leaf!
            if (leaf != Leaf)
            {
				// Is the destination leaf allowed to accept a drop?
				if (leaf.AllowDrop)
				{
					// Any remaining space is used to 
					_targets.Add(new Target(totalSize, totalSize, leaf, Target.TargetActions.Transfer));
				}
            }
        }
	}
	
	/// <summary>
	/// Manage all targets used during a drag and drop operation for outline style redocking.
	/// </summary>
	public class TargetManagerSolid : TargetManagerZones
	{
		// Instance fields
		private DragFeedbackSolid _dragFeedback;
	    
		/// <summary>
		/// Initializes a new instance of the TargetManagerSolid class.
		/// </summary>
		/// <param name="squares">Show as squares or diamonds.</param>
		/// <param name="host">Control that is requesting target management.</param>
		/// <param name="leaf">Source leaf causing drag operation.</param>
		/// <param name="source">Source TabControl of drag operation.</param>
		public TargetManagerSolid(bool squares,
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
		}
		
		/// <summary>
		/// Class specific initialization.
		/// </summary>
		protected override void Initialize()
		{
			// Let base class generate targes
			base.Initialize();

			// Create the drag feedback drawing class
			_dragFeedback = new DragFeedbackSolid();
			_dragFeedback.Start(Host.Style);
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
	
	/// <summary>
	/// Manage all targets used during a drag and drop operation for outline style redocking.
	/// </summary>
	public class TargetManagerOutline : TargetManagerZones
	{
	    // Instance fields
		private DragFeedback _dragFeedback;
	    
		/// <summary>
		/// Initializes a new instance of the TargetManagerOutline class.
		/// </summary>
		/// <param name="squares">Show as squares or diamonds.</param>
		/// <param name="host">Control that is requesting target management.</param>
		/// <param name="leaf">Source leaf causing drag operation.</param>
		/// <param name="source">Source TabControl of drag operation.</param>
	    public TargetManagerOutline(bool squares,
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
		}
		
		/// <summary>
		/// Class specific initialization.
		/// </summary>
		protected override void Initialize()
		{
			// Let base class generate targes
			base.Initialize();

			// Create the drag feedback drawing class
			_dragFeedback = new DragFeedbackOutline();
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
