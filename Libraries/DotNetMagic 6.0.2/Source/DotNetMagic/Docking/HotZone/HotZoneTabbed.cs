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
	/// Hot zone representing a content being moved into a content tab.
	/// </summary>
    public class HotZoneTabbed : HotZone
    {
        // Class constants
        private const int _tabPageLeft = 9;
        private const int _tabPageHeight = 25;
        private const int _tabPageWidth= 45;

        // Instance fields
        private bool _itself;
        private Rectangle _tabRect;
        private Rectangle _tabRectTL;
        private Rectangle _tabRectTR;
        private WindowContentTabbed _wct;

		/// <summary>
		/// Initializes a new instance of the HotZoneTabbed class.
		/// </summary>
		/// <param name="hotArea">Screen area that is hot.</param>
		/// <param name="newSize">New size of hot zone.</param>
		/// <param name="wct">Tabbed content instance.</param>
		/// <param name="itself">Being moved within itself.</param>
        public HotZoneTabbed(Rectangle hotArea, Rectangle newSize, WindowContentTabbed wct, bool itself)
            : base(hotArea, newSize)
        {
            // Remember state
            _wct = wct;
            _itself = itself;

            // Instead of a single rectangle for the dragging indicator we want to provide
            // two rectangles. One for the main area and another to show a tab extending
            // below it. This ensures the user can tell that it will be added as a new tab
            // page of the control.

            int tabHeight = _tabPageHeight;

            // Make sure the tab rectangle does not extend past end of control
            if (newSize.Height < (tabHeight + DragWidth))
                tabHeight = newSize.Height -  DragWidth * 3;

            // Create the tab page extension
            _tabRect = new Rectangle(newSize.X + _tabPageLeft,
                                     newSize.Bottom - tabHeight - DragWidth,
                                     _tabPageWidth,
                                     tabHeight + DragWidth);

            // Make sure tab rectangle does not draw off right side of control
            if (_tabRect.Right > newSize.Right)
                _tabRect.Width -= _tabRect.Right - newSize.Right;

            // We want the intersection between the top left and top right corners to be displayed
            _tabRectTL = new Rectangle(_tabRect.X, _tabRect.Y, DragWidth, DragWidth);
            _tabRectTR = new Rectangle(_tabRect.Right - DragWidth, _tabRect.Y, DragWidth, DragWidth);

            // Reduce the main area by the height of the above item
			NewSize = new Rectangle(newSize.Left, newSize.Top, newSize.Width, newSize.Height - tabHeight);
        }

		/// <summary>
		/// Apply the hot zone change.
		/// </summary>
		/// <param name="screenPos">Screen position when change applied.</param>
		/// <param name="parent">Parent redocker instance.</param>
		/// <returns>true is successful; otherwise false.</returns>
        public override bool ApplyChange(Point screenPos, Redocker parent)
        {
            // If docking back to itself then refuse to apply the change, this will cause the
            // WindowContentTabbed object to put back the content which is the desired effect
            if (_itself)
                return false;

            // We are only called from the RedockerContent class
            RedockerContent redock = parent as RedockerContent;

            DockingManager dockingManager = redock.DockingManager;

			bool becomeFloating = (_wct.ParentZone.State == State.Floating);

            // Reduce flicker during transition
            dockingManager.Container.SuspendLayout();

            // Manageing Zones should remove display AutoHide windows
            dockingManager.RemoveShowingAutoHideWindows();

            switch(redock.DockingSource)
            {
                case RedockerContent.Source.WindowContent:
					{
						// Perform State specific Restore actions
						if (becomeFloating)
						{
							foreach(Content c in redock.WindowContent.Contents)
								c.ContentBecomesFloating();
						}
                        else
                        {
                            // If the source is leaving the Floating state then need to record Restore positions
                            if (redock.WindowContent.State == State.Floating)
                            {
                                foreach(Content c in redock.WindowContent.Contents)
                                    c.ContentLeavesFloating();
                            }
                        }

						int count = redock.WindowContent.Contents.Count;

						for(int index=0; index<count; index++)
						{
							Content c = redock.WindowContent.Contents[0];

							// Remove Content from previous WindowContent
							redock.WindowContent.Contents.RemoveAt(0);

							// Add into new WindowContent
							_wct.Contents.Add(c);
							_wct.BringContentToFront(c);
						}
					}
                    break;
                case RedockerContent.Source.ContentInsideWindow:
					{
						// Perform State specific Restore actions
						if (becomeFloating)
							redock.Content.ContentBecomesFloating();
                        else
                        {
                            // If the source is leaving the Floating state then need to record Restore position
                            if (redock.Content.ParentWindowContent.State == State.Floating)
                                redock.Content.ContentLeavesFloating();
                        }

						// Remove Content from existing WindowContent
						if (redock.Content.ParentWindowContent != null)
							redock.Content.ParentWindowContent.Contents.Remove(redock.Content);

						_wct.Contents.Add(redock.Content);
						_wct.BringContentToFront(redock.Content);
					}
                    break;
                case RedockerContent.Source.FloatingForm:
					{
						// Perform State specific Restore actions
						if (!becomeFloating)
						{
							// Make every Content object in the Floating Zone 
							// record its current state as the Floating state 
							redock.FloatingForm.ExitFloating();
						}

						int wCount = redock.FloatingForm.Zone.Windows.Count;
                    
						for(int wIndex=0; wIndex<wCount; wIndex++)
						{
							WindowContent wc = redock.FloatingForm.Zone.Windows[0] as WindowContent;

							if (wc != null)
							{
								int cCount = wc.Contents.Count;
                            
								for(int cIndex=0; cIndex<cCount; cIndex++)
								{
									// Get reference to first content in collection
									Content c = wc.Contents[0];

									// Remove from old WindowContent
									wc.Contents.RemoveAt(0);

									// Add into new WindowContentTabbed
									_wct.Contents.Add(c);
									_wct.BringContentToFront(c);
								}
							}
						}
					}
                    break;
            }

			dockingManager.UpdateInsideFill();

            // Reduce flicker during transition
            dockingManager.Container.ResumeLayout();

            // Notify a change in the layout of the docking windows
            dockingManager.OnLayoutChanged(EventArgs.Empty);

            return true;
        }

		/// <summary>
		/// Draw a reversible rectangle on the screen.
		/// </summary>
		/// <param name="dragFeedback">Feedback class.</param>
		/// <param name="rect">Screen bounding rectangle.</param>
        public override void DrawFeedback(DragFeedback dragFeedback, Rectangle rect)
        {
			dragFeedback.DragRectangles(new Rectangle[]{rect, _tabRect, _tabRectTL, _tabRectTR});
        }
    }
}
