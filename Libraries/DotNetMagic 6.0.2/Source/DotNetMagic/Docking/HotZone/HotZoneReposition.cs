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
	/// Hot zone representing a content being positioned to an edge.
	/// </summary>
    public class HotZoneReposition : HotZone
    {
        private enum Position
        {
            Inner,
            Index,
            Outer
        }

        // Instance fields
        private int _newIndex;
        private Position _position;
        private State _state;

		/// <summary>
		/// Initializes a new instance of the HotZoneFloating class.
		/// </summary>
		/// <param name="hotArea">Screen area that is hot.</param>
		/// <param name="newSize">New size of hot zone.</param>
		/// <param name="state">New state of content.</param>
		/// <param name="inner">Repositioned to the inner edge.</param>
        public HotZoneReposition(Rectangle hotArea, Rectangle newSize, State state, bool inner)
            : base(hotArea, newSize)
        {
            // Minus one means create at the innermost position
            InternalConstruct((inner ? Position.Inner : Position.Outer), state, -1);
        }

		/// <summary>
		/// Initializes a new instance of the HotZoneFloating class.
		/// </summary>
		/// <param name="hotArea">Screen area that is hot.</param>
		/// <param name="newSize">New size of hot zone.</param>
		/// <param name="state">New state of content.</param>
		/// <param name="newIndex">New index within a zone.</param>
        public HotZoneReposition(Rectangle hotArea, Rectangle newSize, State state, int newIndex)
            : base(hotArea, newSize)
        {
            InternalConstruct(Position.Index, state, newIndex);
        }

        private void InternalConstruct(Position position, State state, int newIndex)
        {
            // Store initial state
            _position = position;
            _newIndex = newIndex;
            _state = state;
        }

		/// <summary>
		/// Apply the hot zone change.
		/// </summary>
		/// <param name="screenPos">Screen position when change applied.</param>
		/// <param name="parent">Parent redocker instance.</param>
		/// <returns>true is successful; otherwise false.</returns>
        public override bool ApplyChange(Point screenPos, Redocker parent)
        {
            // We are only called from the RedockerContent class
            RedockerContent redock = parent as RedockerContent;

            DockingManager dockingManager = redock.DockingManager;

            // Reduce flicker during transition
            dockingManager.Container.SuspendLayout();

            // Need to create a new Zone
            Zone zone;
            
            if (redock.DockingSource == RedockerContent.Source.FloatingForm)
			{
				// Make every Content object in the Floating Zone 
				// record its current state as the Floating state 
				redock.FloatingForm.ExitFloating();

                zone = redock.FloatingForm.Zone;
			}
            else
                zone = dockingManager.CreateZoneForContent(_state);

            // Insert Zone at end of Controls collection
            dockingManager.Container.Controls.Add(zone);

            // Adjust ordering
            switch(_position)
            {
                case Position.Inner:
                    dockingManager.ReorderZoneToInnerMost(zone);
                    break;
                case Position.Index:
                    // Manageing Zones should remove display AutoHide windows
                    dockingManager.RemoveShowingAutoHideWindows();
                
                    // Place new Zone AFTER the one given, so need to increase index by one
                    dockingManager.Container.Controls.SetChildIndex(zone, _newIndex + 1);
                    break;
                case Position.Outer:
                    dockingManager.ReorderZoneToOuterMost(zone);
                    break;
            }

            switch(redock.DockingSource)
            {
                case RedockerContent.Source.WindowContent:
                    // Remove WindowContent from old Zone
                    if (redock.WindowContent.ParentZone != null)
                    {
                        // If the source is leaving the Floating state then need to record Restore positions
                        if (redock.WindowContent.State == State.Floating)
                        {
                            foreach(Content c in redock.WindowContent.Contents)
                                c.ContentLeavesFloating();
                        }
                        
                        redock.WindowContent.ParentZone.Windows.Remove(redock.WindowContent);
                    }

                    // Add into new Zone
                    zone.Windows.Add(redock.WindowContent);
                    break;
                case RedockerContent.Source.ContentInsideWindow:
                    {
                        // Remove Content from existing WindowContent
                        if (redock.Content.ParentWindowContent != null)
                        {
                            // If the source is leaving the Floating state then need to record Restore position
                            if (redock.Content.ParentWindowContent.State == State.Floating)
                                redock.Content.ContentLeavesFloating();

                            redock.Content.ParentWindowContent.Contents.Remove(redock.Content);
                        }
    				
                        // Create a new WindowContent to host Content
                        Window w = dockingManager.CreateWindowForContent(redock.Content);

                        // Add into Zone
                        zone.Windows.Add(w);
                    }
                    break;
                case RedockerContent.Source.FloatingForm:
                    DockStyle ds;
                    LayoutDirection direction;
                
                    dockingManager.ValuesFromState(_state, out ds, out direction);

                    // Define correct docking style to match state
                    zone.Dock = ds;
                    
                    ZoneSequence zs = zone as ZoneSequence;
                    
                    // Define correct display direction to match state
                    if (zs != null)
                        zs.Direction = direction;

                    // Ensure the Zone recalculates contents according to new state
                    zone.State = _state;
                    break;
            }

            // Define correct size of the new Zone
            switch(_state)
            {
                case State.DockLeft:
                case State.DockRight:
                    zone.Width = NewSize.Width;
                    break;
                case State.DockTop:
                case State.DockBottom:
                    zone.Height = NewSize.Height;
                    break;
            }

			dockingManager.UpdateInsideFill();

            // Reduce flicker during transition
            dockingManager.Container.ResumeLayout();

            // Notify a change in the layout of the docking windows
            dockingManager.OnLayoutChanged(EventArgs.Empty);

            return true;
        }
    }
}
