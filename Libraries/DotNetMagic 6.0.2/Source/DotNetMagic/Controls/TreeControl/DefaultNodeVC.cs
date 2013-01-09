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
using System.Text;
using System.Drawing;
using System.Collections;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;
using Crownwood.DotNetMagic.Common;
using Microsoft.Win32;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Implements the default drawing implementation for a Node.
	/// </summary>
	public class DefaultNodeVC : INodeVC
	{
        // Static fields
        internal static StringFormat _format;
        internal static float _initialTabStop = 40f;
        internal static float[] _tabStops = new float[] { 40f, 40f, 40f, 40f, 40f, 40f, 40f, 40f, 40f, 40f };
		// Instance fields
		private bool _mouseDownSelected;
		private bool _mouseDoubleClick;
		
		/// <summary>
		/// Initialize class wide fields.
		/// </summary>
		static DefaultNodeVC()
		{
			// Cache the format used to draw the title text
			_format = new StringFormat();
			_format.Alignment = StringAlignment.Near;
			_format.LineAlignment = StringAlignment.Center;
            _format.SetTabStops(_initialTabStop, _tabStops);
		}

		/// <summary>
		/// Called when the VC is attached to the control.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		public virtual void Initialize(TreeControl tc)
		{
			// Nothing to do by default
		}

		/// <summary>
		/// Called when the VC is about to be detached from the control.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		public virtual void Detaching(TreeControl tc)
		{
			// Nothing to do by default
		}

		/// <summary>
		/// Helper function that indicates if given node is at the root.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <returns></returns>
		public virtual bool IsRootNode(TreeControl tc, Node n)
		{
			return (n != null) && (n.ParentNodes == tc.Nodes);
		}

		/// <summary>
		/// Calculate and return the size needed for drawing a Node instance.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <returns>Size instance with drawing size required.</returns>
		public virtual Size MeasureSize(TreeControl tc, Node n, Graphics g)
		{
			// Do we need recalculate the node size?
			if (n.Cache.IsSizeDirty)
			{
				// Use the size for the text as starting point for calculation
				Size newSize = GetTextSize(tc, n, g);

				// Calculate space required for drawing a checkbox
				Size checkSize = GetCheckSize(tc, n);

				// Do we need to allocate space for checkbox?
				if (checkSize != Size.Empty)
				{
					// Allocate extra width needed to draw an image and image gaps
					newSize.Width += tc.CheckGapLeft + checkSize.Width + tc.CheckGapRight;

					// Make sure the node is tall enough for the checkbox
					if (newSize.Height < checkSize.Height)
						newSize.Height = checkSize.Height;
				}

				// Calculate space required for drawing an image
				Size imageSize = GetImageSize(tc , n);

				// Do we need to allocate space for an image?
				if (imageSize != Size.Empty)
				{
					// Allocate extra width needed to draw an image and image gaps
					newSize.Width += tc.ImageGapLeft + imageSize.Width + tc.ImageGapRight;

					// Make sure the node is tall enough for the image
					if (newSize.Height < imageSize.Height)
						newSize.Height = imageSize.Height;
				}

				n.Cache.Size = newSize;
			}

			// Enforce the maximum allowed node size
			if (n.Cache.Size.Height > tc.MaximumNodeHeight)
				n.Cache.Size = new Size(n.Cache.Size.Width, tc.MaximumNodeHeight);

			// Enforce the minimum allowed node size
			if (n.Cache.Size.Height < tc.MinimumNodeHeight)
				n.Cache.Size = new Size(n.Cache.Size.Width, tc.MinimumNodeHeight);

			// Return the size as cached by the node
			return n.Cache.Size;
		}
		
		/// <summary>
		/// Defines the top left position of the node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="topLeft">Top left position of the node.</param>
		/// <returns>Bounding rectangle created for node.</returns>
		public virtual Rectangle SetPosition(TreeControl tc, Node n, Point topLeft)
		{
			// Update cache bounds using new top left starting point
			n.Cache.Bounds = new Rectangle(topLeft, n.Cache.Size);

			// Return new bounding rectangle to the caller
			return n.Cache.Bounds;
		}

		/// <summary>
		/// Defines the bounding rectangle of the node and all children as well.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="bounds">Bounding rectangle of collection.</param>
		public virtual void SetChildBounds(TreeControl tc, Node n, Rectangle bounds)
		{
			// Remember bounding rectangle for Node and all children as well
			n.Cache.ChildBounds = bounds;
		}

		/// <summary>
		/// Find if the provided rectangle intersects the bounds of the node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="rectangle">Rectangle to compare against.</param>
		/// <param name="recurse">Compare against children as well.</param>
		/// <returns>True if intersecting; otherwise false.</returns>
		public virtual bool IntersectsWith(TreeControl tc, Node n, Rectangle rectangle, bool recurse)
		{
			if (recurse)
				return n.Cache.ChildBounds.IntersectsWith(rectangle);
			else
				return n.Cache.Bounds.IntersectsWith(rectangle);
		}

		/// <summary>
		/// Draw the Node instance.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <param name="clipRectangle">Clipping rectangle used when drawing.</param>
		/// <param name="leftOffset">Starting left offset for drawing.</param>
		/// <param name="rightOffset">Right offset for drawing.</param>
		public virtual void Draw(TreeControl tc, Node n, Graphics g, Rectangle clipRectangle, int leftOffset, int rightOffset)
		{
			// Start with size of total drawing area
			Rectangle rect = n.Cache.Bounds;

			// Reduce by any left/right offset
			rect.X += leftOffset;
			rect.Width -= (leftOffset + rightOffset);

			// Do we need to adjust width so we extend to right hand limit?
			if (tc.ExtendToRight)
			{
				// Recover the actual control size in node space
				Point rightSide = tc.ClientToNodeSpace(new Point(tc.DrawRectangle.Right, 0));

				// Extend to the right limit if there is any extra space to the right
				if (rightSide.X > rect.Right)
					rect.Width += rightSide.X - rect.Right;
			}

			// Calculate space required for drawing a checkbox
			Size checkSize = GetCheckSize(tc, n);

			// Do we need to allocate space for checkbox?
			if (checkSize != Size.Empty)
			{
				// Find the rectangle of the checkbox within the child
				Rectangle rectCheck = GetCheckRectangle(tc, checkSize, rect);

				// Request the image be drawn at calculated position
				DrawCheck(tc, n, g, rectCheck);

				// Reduce drawing rectangle by space occupied by checkbox
				int checkSpace = tc.CheckGapLeft + checkSize.Width +  tc.CheckGapRight;
				rect.X += checkSpace;
				rect.Width -= checkSpace;
			}

			int hotLeft = rect.Left;
				
			// Calculate space required for drawing an image
			Size imageSize = GetImageSize(tc, n);

			// Do we need to draw an image?
			if (imageSize != Size.Empty)
			{
				int yOffset = 0;
				int yHeight = imageSize.Height;

				// If the image is taller than the available space
				if (imageSize.Height > rect.Height)
				{
					// Do not adjust yOffset but limit drawing to height of node
					yHeight = rect.Height;
				}
				else
				{
					// Calculate offset for drawing in centre of node height
					yOffset = (rect.Height - imageSize.Height) / 2;
				}

				// Request the image be drawn at calculated position
				DrawImage(tc, n, g, rect.X + tc.ImageGapLeft, rect.Y + yOffset, imageSize.Width, yHeight);

				// Reduce drawing rectangle by space occupied by the image and gaps around image
				int imageSpace = tc.ImageGapLeft + imageSize.Width +  tc.ImageGapRight;
				rect.X += imageSpace;
				rect.Width -= imageSpace;
			}

			// Draw text in the updated rectangle
			DrawText(tc, n, g, rect, hotLeft);

			// Do we need to draw a focus indication
			if (tc.ContainsFocus && (tc.FocusNode == n))
				DrawFocusIndication(tc, n, g, rect);
		}		

		/// <summary>
		/// Draw the appropriate checkbox at the specified position.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <param name="drawRect">Bounds of checkbox drawing.</param>
		public virtual void DrawCheck(TreeControl tc, Node n, Graphics g, Rectangle drawRect)
		{
			// Is the mouse over the checkbox?
			bool hotTrack = drawRect.Contains(tc.HotPoint);

			// Find out the required method of drawing
			CheckStates states = GetCheckStates(tc, n);

			// Are we drawing as a themed checkbox?
			if (tc.IsControlThemed && (tc.CheckDrawStyle == DrawStyle.Themed))
			{
				// Ask TreeControl to draw for us, using the appropriate theme
				tc.DrawThemedCheckbox(g, drawRect, n.CheckState, states, hotTrack);		
			}
			else
			{
				// Are we drawing a radio button?
				bool radioButton = (states == CheckStates.Radio);

				// Should we draw in the gradient style?
				bool gradient = (tc.CheckDrawStyle == DrawStyle.Gradient);

				// Use border brush to fill entire area
				if (radioButton)
					g.FillEllipse(tc.GetCacheCheckBorderBrush(), drawRect);
				else
					g.FillRectangle(tc.GetCacheCheckBorderBrush(), drawRect);

				// Shrink to use the inside size
				drawRect.Inflate(-tc.CheckBorderWidth, -tc.CheckBorderWidth);

				if (gradient)
				{
					// Modify box very slightly to make gradient look better
					Rectangle checkRectCopy = drawRect;
					checkRectCopy.Inflate(1, 1);
						
					// Is there any rectangle size to draw?
					if ((checkRectCopy.Width > 0) && (checkRectCopy.Height > 0))
					{
						// Create graduated brush using inside color and box size
						using(Brush brush = new LinearGradientBrush(checkRectCopy, 
																	hotTrack ? tc.CheckInsideHotColor : 
																	tc.CheckInsideColor,
																	tc.BackColor,
																	45))
						{
							// Fill inside with required brush first
							if (radioButton)
								g.FillEllipse(brush, drawRect);
							else
								g.FillRectangle(brush, drawRect);
						}
					}
				}
				else
				{
					// Now fill the inside color
					if (radioButton)
						g.FillEllipse(hotTrack ? tc.GetCacheCheckInsideHotBrush() : 
												 tc.GetCacheCheckInsideBrush(), drawRect);
					else
						g.FillRectangle(hotTrack ? tc.GetCacheCheckInsideHotBrush() : 
												   tc.GetCacheCheckInsideBrush(), drawRect);
				}

				// Perform extra drawing to show checked or mixed states
				switch(n.CheckState)
				{
					case CheckState.Mixed:
						if (!radioButton)
						{
							// Deflate rect to construct inner rectangle
							drawRect.Inflate(-2, -2);

							// Fill inside block to show mixed state
							g.FillRectangle(hotTrack ? tc.GetCacheCheckMixedHotBrush() :
													   tc.GetCacheCheckMixedBrush(), drawRect);
						}
						break;
					case CheckState.Checked:
						if (radioButton)
						{
							Brush centreBrush = hotTrack ? tc.GetCacheCheckTickHotBrush() : 
														   tc.GetCacheCheckTickBrush();

							// Deflate rect to construct inner circle
							drawRect.Inflate(-2, -2);

							// Draw the inner circle to show as checked
							g.FillEllipse(centreBrush, drawRect);
						}
						else
						{
							// Calculate x and y positions
							int x1 = drawRect.Left + (int)((double)drawRect.Width / 11 * 2);
							int x2 = drawRect.Left + (int)((double)drawRect.Width / 11 * 4);
							int x3 = drawRect.Left + (int)((double)drawRect.Width / 11 * 8);
							int y1 = drawRect.Top + (int)((double)drawRect.Height / 11 * 6);
							int y2 = drawRect.Top + (int)((double)drawRect.Height / 11 * 8);
							int y3 = drawRect.Top + (int)((double)drawRect.Height / 11 * 4);

							Pen tickPen = hotTrack ? tc.GetCacheCheckTickHotPen() : 
													 tc.GetCacheCheckTickPen();

							// Draw the two connecting lines for a tick
							g.DrawLine(tickPen, x1, y1, x2, y2);
							g.DrawLine(tickPen, x2, y2, x3, y3);
							g.DrawLine(tickPen, x1, y1-1, x2, y2-1);
							g.DrawLine(tickPen, x2, y2-1, x3, y3-1);
							g.DrawLine(tickPen, x1, y1-2, x2, y2-2);
							g.DrawLine(tickPen, x2, y2-2, x3, y3-2);
						}
						break;
				}
			}
		}

		/// <summary>
		/// Draw the appropriate image at the specified position.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <param name="x">X position for drawing.</param>
		/// <param name="y">Y position for drawing.</param>
		/// <param name="width">Width allowed for drawing.</param>
		/// <param name="height">Height allowed for drawing.</param>
		public virtual void DrawImage(TreeControl tc, Node n, Graphics g, int x, int y, int width, int height)
		{
			bool completed = false;
			
			// Try and use selected images where provided
			if (n.IsSelected)
			{
				// Any Node defined Icon takes first precedence
				if (n.SelectedIcon != null)
				{
					g.DrawIcon(n.SelectedIcon, new Rectangle(x, y, width, height));
					completed = true;
				}
				else
				{
					// Any Node defined Image is next in precedence
					if (n.SelectedImage != null)
					{
						g.DrawImage(n.SelectedImage, new Rectangle(x, y, width, height));
						completed = true;
					}
					else
					{
						// A value of -1 means no image can be drawn 
						int imageIndex = -1;

						// Always used the node index by preference, otherwise we 
						// have to fallback on the TreeControl default image index instead
						if (n.SelectedImageIndex >= 0)
							imageIndex = n.SelectedImageIndex;
						else
							imageIndex = tc.SelectedImageIndex;

						// If the index is above the valid range then use -1 to indicate not valid
						if ((imageIndex >= 0) && (imageIndex <= (tc.ImageList.Images.Count - 1)))
						{
							// Draw the image from image list on left side of the node
							Image drawImage = tc.ImageList.Images[imageIndex];
							g.DrawImage(drawImage, new Rectangle(x, y, width, height));
							drawImage.Dispose();
							completed = true;
						}
					}
				}
			}

			// If an image has not already been drawn
			if (!completed)
			{			
				// Any Node defined Icon takes first precedence
				if (n.Icon != null)
					g.DrawIcon(n.Icon, new Rectangle(x, y, width, height));
				else
				{
					// Any Node defined Image is next in precedence
					if (n.Image != null)
						g.DrawImage(n.Image, new Rectangle(x, y, width, height));
					else
					{
						// A value of -1 means no image can be drawn 
						int imageIndex = -1;

						// Always used the node index by preference, otherwise we 
						// have to fallback on the TreeControl default image index instead
						if (n.ImageIndex >= 0)
							imageIndex = n.ImageIndex;
						else
							imageIndex = tc.ImageIndex;

						// If the index is above the valid range then use -1 to indicate not valid
						if ((imageIndex >= 0) && (imageIndex <= (tc.ImageList.Images.Count - 1)))
						{
							// Draw the image from image list on left side of the node
							Image drawImage = tc.ImageList.Images[imageIndex];
							g.DrawImage(drawImage, new Rectangle(x, y, width, height));
							drawImage.Dispose();
						}
					}
				}
			}
		}

		/// <summary>
		/// Draw the appropriate text in provided rectangle.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <param name="rect">Rectangle to draw text inside.</param>
		/// <param name="hotLeft">Left position for drawing as hot tracked.</param>
		public virtual void DrawText(TreeControl tc, Node n, Graphics g, Rectangle rect, int hotLeft)
		{
			// Do not draw text if the text is being edited
			if (tc.LabelEditNode != n)
			{
				bool dragOverNode = (tc.DragOverNode == n); 
				bool selectedNode = n.IsSelected;
				bool hotNode = (tc.HotNode == n);
				bool focus = tc.ContainsFocus;

				// This is the hot node, but is the point in a hot position
				if (hotNode)
				{
					// Calculate the rectangle used for hot tracking node
					Rectangle hotRect = new Rectangle(hotLeft, rect.Top, rect.Right - hotLeft, rect.Height);

					// Are we the current hot node AND the hot point is in the text area
					hotNode = hotRect.Contains(tc.HotPoint);
				}
				
				// The selected background color takes precendence over hot tracking
				if (selectedNode || dragOverNode)
				{
					if (focus || dragOverNode)
						g.FillRectangle(tc.GetCacheSelectedBackBrush(), rect);
					else
						g.FillRectangle(tc.GetCacheSelectedNoFocusBackBrush(), rect);
				}
				else
				{
					// Can only draw hot node background, if a color has been specified
					if (hotNode && (tc.HotBackColor != Color.Empty))
						g.FillRectangle(tc.GetCacheHotBackBrush(), rect);
					else 
					{
						// If the node color is not the same as the control background color
						if (n.BackColor != tc.BackColor)
						{
							// Use the node defined background color
							using(SolidBrush backBrush = new SolidBrush(n.BackColor))
								g.FillRectangle(backBrush, rect);
						}
					}
				}

				Color foreColor;

				if (selectedNode || dragOverNode)
				{
					if (focus || dragOverNode)
						foreColor = tc.SelectedForeColor;
					else
						foreColor = n.ForeColor;
				}
				else if (hotNode && (tc.HotForeColor != Color.Empty))
					foreColor = tc.HotForeColor;
				else
					foreColor = n.ForeColor;

				// Create the forground brush 
				using(SolidBrush foreBrush = new SolidBrush(foreColor))
				{
					bool drawn = false;

					// If drawing text in hot tracking....
					if (hotNode && tc.UseHotFontStyle)
					{
						// ....then update font with hot font style
						using(Font hotFont = new Font(n.NodeFont, tc.HotFontStyle))
							g.DrawString(n.Text, hotFont, foreBrush, rect, _format);

						// Used custom font style for drawing
						drawn = true;
					}
					else
					{
						if (selectedNode && tc.UseSelectedFontStyle)
						{
							// ....then update font with selected font style
							using(Font selectedFont = new Font(n.NodeFont, tc.SelectedFontStyle))
								g.DrawString(n.Text, selectedFont, foreBrush, rect, _format);

							// Used custom font style for drawing
							drawn = true;
						}
					}

					if (!drawn)
					{
						// Draw using standard font
						g.DrawString(n.Text, n.NodeFont, foreBrush, rect, _format);
					}
				}
			}
		}
		
		/// <summary>
		/// Draw focus indication around the provided rectangle.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <param name="rect">Rectangle to draw text inside.</param>
		public virtual void DrawFocusIndication(TreeControl tc, Node n, Graphics g, Rectangle rect)
		{
			// Do not draw focus rectangle if the text is being edited
			if (tc.LabelEditNode != n)
				ControlPaint.DrawFocusRectangle(g, rect);
		}

		/// <summary>
		/// Gets the required check states for this Node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <returns>Valid check states for node.</returns>
		public virtual CheckStates GetCheckStates(TreeControl tc, Node n)
		{
			CheckStates states = CheckStates.None;

			// Do we inherit the value from the TreeControl control?
			if (n.CheckStates == NodeCheckStates.Inherit)
				states = tc.CheckStates;
			else
			{
				switch(n.CheckStates)
				{
					case NodeCheckStates.TwoStateCheck:
						states = CheckStates.TwoStateCheck;
						break;
					case NodeCheckStates.ThreeStateCheck:
						states = CheckStates.ThreeStateCheck;
						break;
					case NodeCheckStates.Radio:
						states = CheckStates.Radio;
						break;
				}
			}

			return states;
		}

		/// <summary>
		/// Get the size of any text that needs to be displayed.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <returns>new Size instance; Size.Empty if no image required.</returns>
		public virtual Size GetTextSize(TreeControl tc, Node n, Graphics g)
		{
			SizeF sizeF;

			// Do we need to ensure there is enough room for a bold version of text?
			if (tc.UseHotFontStyle || tc.UseSelectedFontStyle)
			{
				// Get access to a bold version of font we are using
				Font nodeFont = n.GetNodeFont();
				Font boldFont = (nodeFont == null) ? tc.GetFontBoldItalic() : new Font(nodeFont, FontStyle.Bold);

				// Simple measurement of node text size
				sizeF = g.MeasureString(n.Text, boldFont, int.MaxValue, _format);

				// If we had to create a bold version of node font, dispose of it
				if (nodeFont != null)
					boldFont.Dispose();
			}
			else
			{
                sizeF = g.MeasureString(n.Text, n.NodeFont, int.MaxValue, _format);
			}

			// Use the Font defined height and the variable calculated width
			return new Size((int)(sizeF.Width + 1), (int)(n.GetNodeFontHeight() * 1.25));
		}

		/// <summary>
		/// Get the size of any checkbox that needs to be displayed.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <returns>new Size instance; Size.Empty if no checkbox required.</returns>
		public virtual Size GetCheckSize(TreeControl tc, Node n)
		{
			// Find the required check states
			CheckStates states = GetCheckStates(tc, n);

			// If no checkbox is required
			if (states == CheckStates.None)
				return Size.Empty;
				
			// We always return a square size
			return new Size(tc.CheckLength, tc.CheckLength);
		}

		/// <summary>
		/// Get the size of any image that needs to be displayed.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <returns>new Size instance; Size.Empty if no image required.</returns>
		public virtual Size GetImageSize(TreeControl tc, Node n)
		{
			// If selected then try and use the selected specific values
			if (n.IsSelected)
			{
				if (n.SelectedIcon != null)
					return n.SelectedIcon.Size;

				if (n.SelectedImage != null)
					return n.SelectedImage.Size;
			}

			if (n.Icon != null)
				return n.Icon.Size;

			if (n.Image != null)
				return n.Image.Size;

			if (tc.ImageList != null)
				return tc.ImageList.ImageSize;

			// No image found
			return Size.Empty;
		}

		/// <summary>
		/// Gets the rectangle occupied by checkbox within the client rectangle.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="checkSize">Size of the checkbox.</param>
		/// <param name="client">Bounds of the node client.</param>
		/// <returns>Bounds of the checkbox.</returns>
		public virtual Rectangle GetCheckRectangle(TreeControl tc, Size checkSize, Rectangle client)
		{
			int yOffset = 0;
			int yHeight = checkSize.Height;

			// If the checkbox is taller than the available space
			if (checkSize.Height > client.Height)
			{
				// Do not adjust yOffset but limit drawing to height of node
				yHeight = client.Height;
			}
			else
			{
				// Calculate offset for drawing in centre of node height
				yOffset = (client.Height - checkSize.Height) / 2;
			}

			// Always use the smallest edge
			int length = (yHeight < checkSize.Width) ? yHeight : checkSize.Width;

			// Return the rectangle occupied by the checkbox
			return new Rectangle(client.X + tc.CheckGapLeft,
								 client.Y + yOffset, 
								 length, 
								 length);
		}

		/// <summary>
		/// Return a value indicating of the given node should be expanded on a single click.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <returns>Expand action to be taken.</returns>
		public virtual ClickExpandAction CanExpandOnClick(TreeControl tc, Node n)
		{
			// Does the click cause the node to be expanded
			return tc.ClickExpand;
		}

		/// <summary>
		/// Is AutoEdit enabled for this Node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		public virtual bool CanAutoEdit(TreeControl tc, Node n)
		{
			return tc.AutoEdit;
		}

		/// <summary>
		/// Gets a value indicating if tooltips are allowed for this node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <returns>True if allowed; otherwise false.</returns>
		public virtual bool CanToolTip(TreeControl tc, Node n)
		{
			return true;
		}

		/// <summary>
		/// Return a value indicating of the given node should be expanded on a double click.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <returns>Expand action to be taken.</returns>
		public virtual ClickExpandAction CanExpandOnDoubleClick(TreeControl tc, Node n)
		{
			// Does the click cause the node to be expanded
			return tc.DoubleClickExpand;
		}

		/// <summary>
		/// Process a mouse down on the Node instance.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="button">Button that is pressed down.</param>
		/// <param name="pt">Point being pressed down.</param>
		/// <returns>true if the point was processed; otherwise false.</returns>
		public virtual bool MouseDown(TreeControl tc, Node n, MouseButtons button, Point pt)
		{
			// Start with size of total drawing area
			Rectangle rect = n.Cache.Bounds;

			// Calculate space required for any checkbox
			Size checkSize = GetCheckSize(tc, n);

			// If there a checkbox currently showing?
			if (checkSize != Size.Empty)
			{
				// Find the rectangle of the checkbox within the child
				Rectangle rectCheck = GetCheckRectangle(tc, checkSize, rect);

				// Only left mouse button will change checkbox
				if (button == MouseButtons.Left)
				{
					// Is the mouse click in the check 
					if (rectCheck.Contains(pt))
					{
						CheckStates states = GetCheckStates(tc, n);
					
						// Are we processing a radio button?
						if (states == CheckStates.Radio)
						{
							// Only interested if not currently checked
							if (n.CheckState != CheckState.Checked)
							{
								// Set it to become checked, the node itself will ensure
								// that all siblings are unchecked so only one at the same
								// level is radio button checked at a time
								n.CheckState = CheckState.Checked;
							}
						}
						else
						{
							// Move check onto next state
							switch(n.CheckState)
							{
								case CheckState.Checked:
									if (states != CheckStates.ThreeStateCheck)
										n.CheckState = CheckState.Unchecked;
									else
										n.CheckState = CheckState.Mixed;
									break;
								case CheckState.Mixed:
									n.CheckState = CheckState.Unchecked;
									break;
								case CheckState.Unchecked:
									n.CheckState = CheckState.Checked;
									break;
							}
						}

						return true;
					}
				}

				// Reduce rectangle by space occupied by the checkbox
				int checkSpace = tc.CheckGapLeft + checkSize.Width +  tc.CheckGapRight;
				rect.X += checkSpace;
				rect.Width -= checkSpace;
			}

			// If click is in the remainder of the node
			if (ClickPointInNode(tc, n, rect, pt))
			{
				// Only left mouse button will change checkbox
				if (button == MouseButtons.Left)
				{
					ClickExpandAction action = CanExpandOnClick(tc, n);

					switch(action)
					{
						case ClickExpandAction.Expand:
							// Expand the node if not already expanded
							if (CanExpandNode(tc, n, false, true))
								n.Expand();
							break;
						case ClickExpandAction.Toggle:
							// Invert the current toggled state
							if (n.Expanded)
							{
								// Collapse the node if allowed
								if (CanCollapseNode(tc, n, false, true))
									n.Collapse();
							}
							else
							{
								// Expand the node if allowed
								if (CanExpandNode(tc, n, false, true))
									n.Expand();
							}
							break;
						case ClickExpandAction.None:
							break;
					}
				}
								
				// Remember if the node is selected before the mouse down was processed
				_mouseDownSelected = n.IsSelected;

				// Are we allowed to select the node?
				if (tc.SelectMode != SelectMode.None)
				{
					// Do not select the node if it is already selected 
					if (!n.IsSelected)
						tc.SelectNode(n);
					else
					{
						// Instead we just give it the focus
						tc.SetFocusNode(n);
					}
				}
				else
				{
					// But we can still make it the focus
					tc.SetFocusNode(n);
				}
				
				return true;
			}

			// Not processed
			return false;
		}

		/// <summary>
		/// Process a mouse up on the Node instance.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="button">Button that is going up.</param>
		/// <param name="pt">Point being pressed down.</param>
		/// <returns>true if the point was processed; otherwise false.</returns>
		public virtual bool MouseUp(TreeControl tc, Node n, MouseButtons button, Point pt)
		{
			// Start with size of total drawing area
			Rectangle rect = n.Cache.Bounds;

			// If click is in the remainder of the node
			if (ClickPointInNode(tc, n, rect, pt))
			{
				// Only a left mouse button up can select a node or cause auto editing
				if (button == MouseButtons.Left)
				{
					// Are we allowed to select the node?
					if (tc.SelectMode != SelectMode.None)
					{
						bool prevent = false;
						
						// If the node has already changed state during mouse down
						if (n.IsSelected != _mouseDownSelected)
						{
							// If the user is pressing the CTRL key
							if (KeyHelper.CTRLPressed)
								prevent = true;
						}
						
						// If the node is already selected then we should process the select
						// again because if the user is using the CTRL or SHIFT keys then it
						// might need to alter the selection. The only exception to this rule
						// is if the user presses CTRL and the node has already changed state.
						if (n.IsSelected && !prevent)
							tc.SelectNode(n);							

						// If the node is now selected and was already selected on the mouse down
						// then this select select of the node should cause it to test for auto edit
						if (n.IsSelected && _mouseDownSelected)
						{							
							// Only auto edit if the node is allowed to be edited and it is the only selected node
							if (CanAutoEdit(tc, n) && (tc.SelectedCount == 1))
							{
								// Find the rectangle for just the text and not the checkbox and image
								Rectangle textRect = GetTextRectangle(tc, n);
								
								// Only edit if the user clicked on the text area
								if (textRect.Contains(pt))
								{
									// Do not perform an edit when caused by a double click
									if (!_mouseDoubleClick)
										tc.BeginAutoEdit(n);
								}
							}
						}
					}
				}

				// Only right mouse button will show context menu
				if (button == MouseButtons.Right)
				{
					// Do we need to show context menu ourself?
					if (tc.OnShowContextMenuNode(n))
					{
						// Grab any context menu from the control
						ContextMenuStrip cm = tc.ContextMenuNode;
						
						// If we have one to show...
						if (cm != null)
						{
							//...then show it now!
							cm.Show(tc, tc.NodeSpaceToClient(pt));
						}
					}
				}				

				// Reset the double click flag			
				_mouseDoubleClick = false;
				
				return true;
			}
			
			// Reset the double click flag			
			_mouseDoubleClick = false;

			// Not processed
			return false;
		}
			
		/// <summary>
		/// Process a double click on the Node instance.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="pt">Point being pressed down.</param>
		/// <returns>true if the point was processed; otherwise false.</returns>
		public virtual bool DoubleClick(TreeControl tc, Node n, Point pt)
		{
			// Remember a double click has occured
			_mouseDoubleClick = true;
			
			// Start with size of total drawing area
			Rectangle rect = n.Cache.Bounds;

			// Calculate space required for any checkbox
			Size checkSize = GetCheckSize(tc, n);

			// If there a checkbox currently showing?
			if (checkSize != Size.Empty)
			{
				// Reduce rectangle by space occupied by the checkbox
				int checkSpace = tc.CheckGapLeft + checkSize.Width +  tc.CheckGapRight;
				rect.X += checkSpace;
				rect.Width -= checkSpace;
			}
			
			// If click is in the node rectangle
			if (ClickPointInNode(tc, n, rect, pt))
			{
				ClickExpandAction action = CanExpandOnDoubleClick(tc, n);

				switch(action)
				{
					case ClickExpandAction.Expand:
						// Expand the node if allowed
						if (CanExpandNode(tc, n, false, true))
							n.Expand();
						break;
					case ClickExpandAction.Toggle:
						// Invert the current toggled state
						if (n.Expanded)
						{
							// Collapse the node if allowed
							if (CanCollapseNode(tc, n, false, true))
								n.Collapse();
						}
						else
						{
							// Expand the node if allowed
							if (CanExpandNode(tc, n, false, true))
								n.Expand();
						}
						break;
					case ClickExpandAction.None:
						break;
				}

				// Cancel any auto edit currently running as timer
				tc.CancelAutoEdit();

				return true;
			}

			// Not processed
			return false;
		}
		
		/// <summary>
		/// Returns a value indicating if the point is inside the node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="rect">Rectangle of node area.</param>
		/// <param name="pt">Point where the mouse is clicked.</param>
		/// <returns>true if inside area; otherwise false.</returns>
		public virtual bool ClickPointInNode(TreeControl tc, Node n, Rectangle rect, Point pt)
		{
			// If node extends to right then select if the point is anywhere in 
			// the height of the node and to the right of where the node begins.
			if (tc.ExtendToRight)
				return (pt.X >= rect.Left) && (pt.Y >= rect.Top) && (pt.Y <= rect.Bottom);
			else
				return rect.Contains(pt);
		}

		/// <summary>
		/// Notification that the expanded state of a node has changed.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		public virtual void NodeExpandedChanged(TreeControl tc, Node n)
		{
			// Has the node become expanded and tree wants to auto collapse
			if (n.Expanded && tc.AutoCollapse)
			{
				// Process all sibling nodes
				foreach(Node sibling in n.ParentNodes)
				{
					// For all other nodes, collapse them
					if (sibling != n)
						sibling.Collapse();
				}
			}
			
			// If node has become collapsed and their are some selected nodes
			if (!n.Expanded && (tc.SelectedCount > 0))
			{
				// Ensure all children of the node are deselected as they
				// are no longer showing and so cannot be in the selected state
				foreach(Node child in n.Nodes)
					if (child.Visible)
						tc.DeselectNode(child, true);
			}

			// Ask control to move the focus to new calculated position
			tc.NodeExpandedChanged(n);
		}
		
		/// <summary>
		/// Notification that the visible state of a node has changed.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		public virtual void NodeVisibleChanged(TreeControl tc, Node n)
		{
			// If node has become invisible...
			if (!n.Visible)
			{
				// If there is only a single node selected and it is this one, then we
				// want the new focused node to also be the new selected one as well
				bool makeSelected = (tc.SelectedCount == 1) && (n.IsSelected);
				
				//...and their are some selected nodes
				if (tc.SelectedCount > 0)
				{
					// Ensure all children of the node are deselected as they
					// are no longer showing and so cannot be in the selected state
					tc.DeselectNode(n, true);
				}
				
				// Ask control to move the focus to new calculated position
				tc.NodeVisibleChanged(n, makeSelected);
			}
		}

		/// <summary>
		/// Notification that the selectable state of a node has changed.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		public virtual void NodeSelectableChanged(TreeControl tc, Node n)
		{
			// If the node is no longer selectable
			if (!n.Selectable)
			{
				// Ensure the node is not selected
				tc.DeselectNode(n, false);
			}
		}

		/// <summary>
		/// Notification that the CheckState of a node has changed.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		public virtual void NodeCheckStateChanged(TreeControl tc, Node n)
		{
			// If the node has been checked and is a radio button style
			if ((n.CheckState == CheckState.Checked) && (GetCheckStates(tc, n) == CheckStates.Radio))
			{
				// Then we want to make all other siblings nodes unchecked
				foreach(Node sibling in n.ParentNodes)
				{
					// Do not change the original node
					if (sibling != n)
					{
						// Do not change the state unless it is also a radio button
						if ((sibling.CheckStates == NodeCheckStates.Radio) ||
						    ((sibling.CheckStates == NodeCheckStates.Inherit) && (tc.CheckStates == CheckStates.Radio)))
						{						
							// Set back to not checked
							sibling.CheckState = CheckState.Unchecked;
						}
					}
				}
			}
		}

		/// <summary>
		/// Notification that specified node is being removed.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		public virtual void NodeRemoving(TreeControl tc, Node n)
		{
			// If there is only a single node selected and it is this one, then we
			// want the new focused node to also be the new selected one as well
			bool makeSelected = (tc.SelectedCount == 1) && (n.IsSelected);

			// Ensure all children of the node are deselected as they
			// are no longer showing and so cannot be in the selected state
			tc.DeselectNode(n, true);

			// Was this node the one with the focus
			if (tc.FocusNode == n)
			{
				// Ask control to move the focus to new calculated position
				tc.NodeContentRemoved(makeSelected);
			}
		}

		/// <summary>
		/// Is the specified Node allowed to be selected?
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <returns>true if node is allowed to be selected; otherwise false.</returns>
		public virtual bool CanSelectNode(TreeControl tc, Node n)
		{
            // By default we allow the selection because of 'SelectSameLevel'
            bool sameLevel = true;

            // Do we need to check that an existing selection is in the same node level?
            if (tc.SelectSameLevel && (tc.SelectedNode != null))
            {
                // Default to not allowing the selection
                sameLevel = false;

                // If the current selection and this node have the same parent
                if ((n.Parent != null) && (n.TreeControl.SelectedNode.Parent != null))
                {
                    // And are on the same full path from the root, then allow
                    if (n.TreeControl.SelectedNode.Parent.FullPath == n.Parent.FullPath)
                        sameLevel = true;
                }
                else if ((n.Parent == null) && (n.TreeControl.SelectedNode.Parent == null))
                {
                    // Current selection and this node are both at the root, so allow
                    sameLevel = true;
                }
            }

            // Does the tree allow selectable nodes and the node itself is selectable plus
            // does the 'SelectSameLevel' allow the selection to occur
            return tc.NodesSelectable && n.Selectable && sameLevel;
		}

		/// <summary>
		/// Is the specified Node allowed to be collapsed?
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="key">True if keyboard action.</param>
		/// <param name="mouse">True if mouse action.</param>
		/// <returns>true if node is allowed to be collapsed; otherwise false.</returns>
		public virtual bool CanCollapseNode(TreeControl tc, Node n, bool key, bool mouse)
		{
			// By default use the user supplied value
			return tc.CanUserExpandCollapse;
		}

		/// <summary>
		/// Is the specified Node allowed to be expanded?
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="key">True if keyboard action.</param>
		/// <param name="mouse">True if mouse action.</param>
		/// <returns>true if node is allowed to be collapsed; otherwise false.</returns>
		public virtual bool CanExpandNode(TreeControl tc, Node n, bool key, bool mouse)
		{
			// By default use the user supplied value
			return tc.CanUserExpandCollapse;
		}

		/// <summary>
		/// Initiate the editing of the Node label.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		public virtual void BeginEditNode(TreeControl tc, Node n)
		{
			// Does the tree allow node labels to be edited?
			if (tc.LabelEdit)
			{
				// Ensure that the node is being displayed
				// (cannot edit a node that is not showing)
				if (tc.IsNodeDisplayed(n))
				{
					// Alter scrolling so node is displayed
					if (tc.EnsureDisplayed(n))
					{
						// Get the node rectangle that covers just the text
						Rectangle textRect = GetTextRectangle(tc, n);

						// Recover the font that should be used for measuring
						Font nodeFont = GetNodeFont(tc, n);

						using(Graphics g = tc.CreateGraphics())
						{						
							// Simple measurement of node text size
							SizeF sizeF = g.MeasureString(n.Text + "W", nodeFont);
							
							// Find the width of a minimum string
							SizeF minF = g.MeasureString("01234", nodeFont);

							// Use the largest of the two
							if (minF.Width > sizeF.Width)
								sizeF.Width = minF.Width;
								
							// Use the actual text height and not the node height
							if (minF.Height < textRect.Height)
							{
								int diff = textRect.Height - (int)minF.Height;
								
								// Reduce height by area not needed
								textRect.Height -= diff;
								
								// Move down by half the difference to keep centered
								textRect.Y += (1 + diff / 2);
							}

							// Use the required size border widths plus an extra space extra
							textRect.Width = (int)(sizeF.Width) + (SystemInformation.BorderSize.Width * 2);
							
							// Ask the control to show edit box for given node and rectangle
							tc.BeginEditLabel(n, textRect);
						}
					}
				}
			}
		}

		/// <summary>
		/// Get the rectangle that represents drawing area for text.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <returns></returns>
		public virtual Rectangle GetTextRectangle(TreeControl tc, Node n)
		{
			// Start with size of total drawing area
			Rectangle rect = n.Cache.Bounds;

			// Calculate space required for drawing a checkbox
			Size checkSize = GetCheckSize(tc, n);

			// Do we need to allocate space for checkbox?
			if (checkSize != Size.Empty)
			{
				// Reduce drawing rectangle by space occupied by checkbox
				int checkSpace = tc.CheckGapLeft + checkSize.Width +  tc.CheckGapRight;
				rect.X += checkSpace;
				rect.Width -= checkSpace;
			}

			// Calculate space required for drawing an image
			Size imageSize = GetImageSize(tc, n);

			// Do we need to draw an image?
			if (imageSize != Size.Empty)
			{
				// Reduce drawing rectangle by space occupied by the image and gaps around image
				int imageSpace = tc.ImageGapLeft + imageSize.Width +  tc.ImageGapRight;
				rect.X += imageSpace;
				rect.Width -= imageSpace;
			}

			// Remaining space is for drawing text
			return rect;
		}

		/// <summary>
		/// Notification that the size of the control has changed.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		public virtual void SizeChanged(TreeControl tc)
		{
			// We have nothing extra to do for default nodes.
		}

		/// <summary>
		/// Find the displayed node for the given point.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="pt">Point in Client coordinates.</param>
		/// <returns>node reference or null.</returns>
		public virtual Node DragOverNodeFromPoint(TreeControl tc, Point pt)
		{
			// If we are treating nodes as taking up the entire width
			if (tc.ExtendToRight)
				return tc.FindDisplayNodeFromY(pt.Y);
			else
			{
				// Only interested in nodes that are an exact match for point
				return tc.FindDisplayNodeFromPoint(pt);
			}
		}
			
		/// <summary>
		/// Notification that dragging has entered the specified Node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="drgevent">Drag event information.</param>
		public virtual void DragEnter(TreeControl tc, Node n, DragEventArgs drgevent)
		{
			// By default we just pass on the event
			tc.OnNodeDragEnter(n, drgevent);
		}
		
		/// <summary>
		/// Notification that user is dragging over the specified Node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="drgevent">Drag event information.</param>
		public virtual void DragOver(TreeControl tc, Node n, DragEventArgs drgevent)
		{
			// By default we just pass on the event
			tc.OnNodeDragOver(n, drgevent);
		}

		/// <summary>
		/// Notification that dragging has left the specified Node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		public virtual void DragLeave(TreeControl tc, Node n)
		{
			// By default we just pass on the event
			tc.OnNodeDragLeave(n);
		}

		/// <summary>
		/// Notification that user has dropped onto specified Node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="drgevent">Drag event information.</param>
		public virtual void DragDrop(TreeControl tc, Node n, DragEventArgs drgevent)
		{
			// By default we just pass on the event
			tc.OnNodeDragDrop(n, drgevent);
		}
		
		/// <summary>
		/// Notification that user is hovering over a drag Node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		public virtual  void DragHover(TreeControl tc, Node n)
		{
			// Ask the tree control if we should expand on hovering
			if (tc.ExpandOnDragHover)
			{
				// If the node has children and is not expanded
				if (!n.Expanded && (n.Nodes.VisibleCount > 0))
				{
					// Then try and expand the node
					n.Expand();
				}
			}
		}

		/// <summary>
		/// Final change to perform drawing.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <param name="displayNodes">Collection of nodes in display.</param>
		public virtual void PostDrawNodes(TreeControl tc, Graphics g, ArrayList displayNodes)
		{
			// We have nothing extra to do for default nodes.
		}

		/// <summary>
		/// Called once all the nodes have been calculated ready for display.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="displayNodes">Collection of nodes in display.</param>
		public virtual void PostCalculateNodes(TreeControl tc, ArrayList displayNodes)
		{
			// We have nothing extra to do for default nodes.
		}
		
		/// <summary>
		/// Recover the Font used to draw the text in normal mode.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <returns>Font used to draw text in normal mode.</returns>
		public virtual Font GetNodeFont(TreeControl tc, Node n)
		{
			// Just use the node defined font.
			return n.NodeFont;
		}
	}
}
