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
	/// Hot zone representing a content becoming a floating window.
	/// </summary>
    public class HotZoneFloating : HotZone
    {
        // Instance fields
        private Point _offset;
        private Point _drawPos;
        private Rectangle _drawRect;
        private RedockerContent _redocker;

		/// <summary>
		/// Initializes a new instance of the HotZoneFloating class.
		/// </summary>
		/// <param name="hotArea">Screen area that is hot.</param>
		/// <param name="newSize">Screen area to display with rectangle.</param>
		/// <param name="offset">Cursor offset when dragging started.</param>
		/// <param name="redocker">Parent redocker instance.</param>
        public HotZoneFloating(Rectangle hotArea, Rectangle newSize, Point offset, RedockerContent redocker)
            : base(hotArea, newSize)
        {
            // Store initial state
            _offset = offset;
            _redocker = redocker;

			if (redocker.DockingSource != RedockerContent.Source.FloatingForm)
			{
				Size floatSize = CalculateFloatingSize();
				float widthPercentage = (float)floatSize.Width / (float)NewSize.Width;
				float heightPercentage = (float)floatSize.Height / (float)NewSize.Height;

				NewSize = new Rectangle(NewSize.Left, NewSize.Top, floatSize.Width, floatSize.Height + SystemInformation.ToolWindowCaptionHeight);

				_offset.X = (int)((float) _offset.X * widthPercentage);
				_offset.Y = (int)((float) _offset.Y * heightPercentage);
			}
			
			// We do not want the indicator to be too far away from the cursor, so limit check the offset
			if (_offset.X > newSize.Width)
				_offset.X = newSize.Width;

			if (_offset.Y > newSize.Height)
				_offset.Y = newSize.Height;
        }

		/// <summary>
		/// Apply the hot zone change.
		/// </summary>
		/// <param name="screenPos">Screen position when change applied.</param>
		/// <param name="parent">Parent redocker instance.</param>
		/// <returns>true is successful; otherwise false.</returns>
        public override bool ApplyChange(Point screenPos, Redocker parent)
        {
            // Should always be the appropriate type
            RedockerContent redock = parent as RedockerContent;
        
            DockingManager dockingManager = redock.DockingManager;

            Zone newZone = null;
            
            // Manageing Zones should remove display AutoHide windows
            dockingManager.RemoveShowingAutoHideWindows();

            switch(redock.DockingSource)
            {
                case RedockerContent.Source.WindowContent:
                    // Perform State specific Restore actions
                    foreach(Content c in redock.WindowContent.Contents)
                        c.ContentBecomesFloating();

                    // Remove WindowContent from old Zone
                    if (redock.WindowContent.ParentZone != null)
                        redock.WindowContent.ParentZone.Windows.Remove(redock.WindowContent);

                    // We need to create a Zone for containing the transfered content
                    newZone = dockingManager.CreateZoneForContent(State.Floating);
                    
                    // Add into new Zone
                    newZone.Windows.Add(redock.WindowContent);
                    break;
                case RedockerContent.Source.ContentInsideWindow:
                    {
                        // Perform State specific Restore actions
                        redock.Content.ContentBecomesFloating();

                        // Remove Content from existing WindowContent
                        if (redock.Content.ParentWindowContent != null)
                            redock.Content.ParentWindowContent.Contents.Remove(redock.Content);
    				
                        // Create a new Window to host Content
                        Window w = dockingManager.CreateWindowForContent(redock.Content);

                        // We need to create a Zone for containing the transfered content
                        newZone = dockingManager.CreateZoneForContent(State.Floating);
                        
                        // Add into Zone
                        newZone.Windows.Add(w);
                    }
                    break;
                case RedockerContent.Source.FloatingForm:
                    redock.FloatingForm.Location = new Point(screenPos.X - _offset.X,
                                                             screenPos.Y - _offset.Y);

                    return false;
            }
        
			dockingManager.UpdateInsideFill();

            // Create a new floating form
            DockingManager._floatingFormContainer = dockingManager.Container.FindForm();
            FloatingForm floating = redock.DockingManager.Factory.CreateFloatingForm(redock.DockingManager, 
                                                                                     newZone,
                                                                                     new ContextHandler(dockingManager.OnShowContextMenu));

            // Find screen location/size            
            _drawRect = new Rectangle(screenPos.X, screenPos.Y, NewSize.Width, NewSize.Height);

            // Adjust for mouse starting position relative to source control
            _drawRect.X -= _offset.X;
            _drawRect.Y -= _offset.Y;

            // Define its location/size
			floating.SetBounds(_drawRect.Left, _drawRect.Top, _drawRect.Width, _drawRect.Height); 

            // Show it!
            floating.RequestShow();

            // Notify a change in the layout of the docking windows
            dockingManager.OnLayoutChanged(EventArgs.Empty);

            return true;
        }

		/// <summary>
		/// Update the screen indication to reflect new mouse position.
		/// </summary>
		/// <param name="dragFeedback">Feedback class.</param>
		/// <param name="screenPos">New screen position.</param>
		/// <param name="parent">Parent redocker instance.</param>
        public override void UpdateForMousePosition(DragFeedback dragFeedback,
													Point screenPos, 
													Redocker parent)
        {
            // Remember the current mouse pos
            Point newPos = screenPos;

            // Calculate the new drawing rectangle
            Rectangle newRect = new Rectangle(newPos.X, newPos.Y, NewSize.Width, NewSize.Height);

            // Adjust for mouse starting position relative to source control
            newRect.X -= _offset.X;
            newRect.Y -= _offset.Y;

            dragFeedback.DragRectangle(newRect);

			// Remember new values
			_drawPos = newPos;
			_drawRect = newRect;
        }

		/// <summary>
		/// Draw the zone indicator to the screen.
		/// </summary>
		/// <param name="dragFeedback">Feedback class.</param>
		/// <param name="mousePos">Screen position of mouse.</param>
        public override void DrawIndicator(DragFeedback dragFeedback,Point mousePos) 
        {
            // Remember the current mouse pos
            _drawPos = mousePos;

            // Calculate the new drawing rectangle
            _drawRect = new Rectangle(_drawPos.X, _drawPos.Y, NewSize.Width, NewSize.Height);

            // Adjust for mouse starting position relative to source control
            _drawRect.X -= _offset.X;
            _drawRect.Y -= _offset.Y;

            DrawFeedback(dragFeedback, _drawRect);
        }
		
		/// <summary>
		/// Remove the zone indicator from the screen.
		/// </summary>
		/// <param name="dragFeedback">Feedback class.</param>
		/// <param name="mousePos">Screen position of mouse.</param>
        public override void RemoveIndicator(DragFeedback dragFeedback,Point mousePos) 
        {			
            DrawFeedback(dragFeedback, _drawRect);
        }
        
        private Size CalculateFloatingSize()
        {
            Size floatingSize = new Size(0,0);

            // Get specific redocker type
            RedockerContent redock = _redocker as RedockerContent;

            switch(redock.DockingSource)
            {
                case RedockerContent.Source.WindowContent:
                    // Find the largest requested floating size
                    foreach(Content c in redock.WindowContent.Contents)
                    {
                        if (c.FloatingSize.Width > floatingSize.Width)
                            floatingSize.Width = c.FloatingSize.Width;
                    
                        if (c.FloatingSize.Height > floatingSize.Height)
                            floatingSize.Height = c.FloatingSize.Height;
                    }

                    // Apply same size to all Content objects
                    foreach(Content c in redock.WindowContent.Contents)
                        c.FloatingSize = floatingSize;
                    break;
                case RedockerContent.Source.ContentInsideWindow:
                    // Whole Form is size requested by single Content
                    floatingSize = redock.Content.FloatingSize;
                    break;
                case RedockerContent.Source.FloatingForm:
                    // Use the requested size
                    floatingSize.Width = NewSize.Width;
                    floatingSize.Height = NewSize.Height;
                    break;
            }

            return floatingSize;
        }
    }
}
