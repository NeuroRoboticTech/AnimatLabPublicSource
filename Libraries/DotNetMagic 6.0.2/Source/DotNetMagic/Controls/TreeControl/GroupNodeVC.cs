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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.ComponentModel;
using Crownwood.DotNetMagic.Common;
using Microsoft.Win32;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Implements the group drawing implementation for a Node.
	/// </summary>
	public class GroupNodeVC : DefaultNodeVC
	{
		// Class fields
		private static Image _upArrow;
		private static Image _downArrow;
		private static int _arrowHeight;
		private static int _arrowWidth;
		private static int _arrowHeightSpace;
		private static int _arrowWidthSpace;
		private static ImageAttributes _imageAttr;

		static GroupNodeVC()
		{
			// Cache the two arrow images
			_upArrow = ResourceHelper.LoadBitmap(typeof(GroupNodeVC), "Crownwood.DotNetMagic.Controls.TitleBar.ImageUp.bmp", Point.Empty);
			_downArrow = ResourceHelper.LoadBitmap(typeof(GroupNodeVC), "Crownwood.DotNetMagic.Controls.TitleBar.ImageDown.bmp", Point.Empty);

			// Allocate enough space for the image plus spacing pixel borders
			_arrowWidth = _upArrow.Width;
			_arrowHeight = _upArrow.Height;
			_arrowWidthSpace = _arrowWidth + 2;
			_arrowHeightSpace = _arrowHeight + 2;

			// Create image attributes
			_imageAttr = new ImageAttributes();
		}

		/// <summary>
		/// Called when the VC is attached to the control.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		public override void Initialize(TreeControl tc)
		{
			// We need to monitor changes in the GroupAutoCollapse setting
			tc.GroupAutoCollapseChanged += new EventHandler(OnAutoCollapseChanged);
			
			// Make sure the GroupAutoCollapse is honored
			EnforceSingleExpandedGroup(tc);
		}

		/// <summary>
		/// Called when the VC is about to be detached from the control.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		public override void Detaching(TreeControl tc)
		{
			// Unhook from events
			tc.GroupAutoCollapseChanged -= new EventHandler(OnAutoCollapseChanged);
		}

		/// <summary>
		/// Calculate and return the size needed for drawing a Node instance.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <returns>Size instance with drawing size required.</returns>
		public override Size MeasureSize(TreeControl tc, Node n, Graphics g)
		{
			// Non-group nodes use the simple default processing
			if (!IsRootNode(tc, n))
				return base.MeasureSize(tc, n, g);

			// Do we need recalculate the node size?
			if (n.Cache.IsSizeDirty)
			{
				SizeF sizeF;

				// Do we need to ensure there is enough room for a bold version of text?
				if (tc.GroupUseHotFontStyle || tc.GroupUseSelectedFontStyle)
				{
					// Get access to a bold version of font we are using
					Font nodeFont = n.GetNodeFont();
					Font boldFont = (nodeFont == null) ? tc.GetGroupFontBoldItalic() : new Font(nodeFont, FontStyle.Bold);

					// Simple measurement of node text size
					sizeF = g.MeasureString(n.Text, boldFont);

					// If we had to create a bold version of node font, dispose of it
					if (nodeFont != null)
						boldFont.Dispose();
				}
				else
				{
					// Get access to a bold version of font we are using
					Font nodeFont = n.GetNodeFont();
					
					// If not defined, then get the tree level define group font
					if (nodeFont == null)
						nodeFont = tc.GroupFont;

					sizeF = g.MeasureString(n.Text, nodeFont);		
				}

				// Use the Font defined height and the variable calculated width
				Size newSize = new Size((int)(sizeF.Width + 1), (int)(n.GetNodeGroupFontHeight() * 1.25));

				// Add the extra left offset needed for groups
				newSize.Width += tc.GroupExtraLeft;

				// Add extra height for group nodes
				newSize.Height += tc.GroupExtraHeight;

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

				// Do we need enough space to draw an arrow?
				if (tc.GroupArrows)
				{
					// Allocate extra space for the arrow image
					newSize.Width += _arrowWidthSpace;

					// Make sure the node is tall enough for the arrow
					if (newSize.Height < _arrowHeightSpace)
						newSize.Height = _arrowHeightSpace;
				}

				// If we are using an image box...
				if (tc.GroupImageBox)
				{
					// ... then allocate enough width for it plus image gap to its right,
					// we do not add any height as the image will have to fit in the given space.
					newSize.Width += tc.GroupImageBoxWidth + tc.GroupImageBoxGap;
				}
				else
				{				
					// Calculate space required for drawing an image
					Size imageSize = GetImageSize(tc, n);

					// Do we need to allocate space for an image?
					if (imageSize != Size.Empty)
					{
						// Allocate extra width needed to draw an image and image gaps
						newSize.Width += tc.ImageGapLeft + imageSize.Width + tc.ImageGapRight;

						// Make sure the node is tall enough for the image
						if (newSize.Height < imageSize.Height)
							newSize.Height = imageSize.Height;
					}
				}

				n.Cache.Size = newSize;
			}

			// Return the size as cached by the node
			return n.Cache.Size;
		}

		/// <summary>
		/// Find if the provided rectangle intersects the bounds of the node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="rectangle">Rectangle to compare against.</param>
		/// <param name="recurse">Compare against children as well.</param>
		/// <returns>True if intersecting; otherwise false.</returns>
		public override bool IntersectsWith(TreeControl tc, Node n, Rectangle rectangle, bool recurse)
		{
			// Non group nodes use the standard processing
			if (!IsRootNode(tc, n))
				return base.IntersectsWith(tc, n, rectangle, recurse);			
			else
			{
				Rectangle bounds;
				
				// Get the appropriate bounds
				if (recurse)
					bounds = n.Cache.ChildBounds;
				else
					bounds = n.Cache.Bounds;
					
				// We need to draw group level nodes across whole width, so only test
				// the vertical position for a match and always assume the horizontal intersects
				if (((rectangle.Top >= bounds.Top) && (rectangle.Top <= bounds.Bottom)) ||
				    ((rectangle.Bottom >= bounds.Top) && (rectangle.Bottom <= bounds.Bottom)) ||
				    ((rectangle.Top < bounds.Top) && (rectangle.Bottom > bounds.Bottom)))
				    return true;
				else
					return false;
			}
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
		public override void Draw(TreeControl tc, Node n, Graphics g, Rectangle clipRectangle, int leftOffset, int rightOffset)
		{
			bool useDefaultDraw = true;
		
			// Non-group nodes use the simple default processing
			if (IsRootNode(tc, n))
			{
				bool hotNode = tc.GroupHotTrack && (tc.HotNode == n);
				bool dragOverNode = (tc.DragOverNode == n);
				bool selectedNode = n.IsSelected;

				// If you are not allowed to select group nodes then we use the selection 
				// colors for nodes that are expanded and not for those selected
				if (!tc.GroupNodesSelectable)
					selectedNode = n.Expanded;

				// Recover our total bounds
				Rectangle rect = n.Cache.Bounds;

				// Recover the actual control size in node space
				Point rightSide = tc.ClientToNodeSpace(new Point(tc.DrawRectangle.Right, 0));

				// Alter the drawing rect to cover whole control width
				rect.X = 0;
				rect.Width = rightSide.X > rect.Width ? rightSide.X : rect.Width;

				// Add group offset to the incoming one
				leftOffset += tc.GroupExtraLeft;

				// Reduce width by the right offset
				rect.Width -= rightOffset;

				// Is there any rectangle size to draw?
				if ((rect.Width > 0) && (rect.Height > 0))
				{
					// Are we using office specific drawing?
					if (tc.GroupColoring != GroupColoring.ControlProperties)
					{
						Color top = Color.Empty;
						Color bottom = Color.Empty;
						
						// Discover the correct colors to use
						GetGroupColouringOffice(tc, selectedNode, hotNode, ref top, ref bottom);

                        Rectangle linearRect = rect;
                        linearRect.Inflate(1, 1);

                        float angle;

                        switch (tc.GroupColoring)
                        {
                            case GroupColoring.Office2007BlueLight:
                            case GroupColoring.Office2007SilverLight:
                            case GroupColoring.Office2007BlackLight:
                            case GroupColoring.MediaPlayerBlueLight:
                            case GroupColoring.MediaPlayerOrangeLight:
                            case GroupColoring.MediaPlayerPurpleLight:
                                angle = 0f;
                                break;
                            default:
                                angle = 90f;
                                break;
                        }

						// Fill background in required gradient colour
                        using (LinearGradientBrush backBrush = new LinearGradientBrush(linearRect, top, bottom, angle))
                        {
                            if (angle == 0f)
                                backBrush.SetSigmaBellShape(0.5f);

                            g.FillRectangle(backBrush, rect);
                        }
					}
					else
					{
						// Select correct background color based on hot tracking and selection
						Color backColor = GetBackColor(tc, n, hotNode, selectedNode || dragOverNode);
			
						Brush backBrush;

						// Do we create a gradiant back brush?
						if (tc.GroupGradientBack)
						{
							// Inflate rect to wash out colours
							Rectangle gradientRect = rect;
							gradientRect.Inflate(rect.Width / 4, rect.Height / 4);

							Color start = Color.Empty;
							Color end = Color.Empty;

							// Find the correct gradient colors based on scheme
							tc.GradientColors(tc.GroupGradientColoring, backColor, ref start, ref end);
							
							// Gradient is based on the required angle
							backBrush = new LinearGradientBrush(gradientRect, start, end, (int)tc.GroupGradientAngle);
						}
						else
						{
							// We use just a simple solid colour
							backBrush = new SolidBrush(backColor);
						}

						// Fill the entire background with brush (not just text area)
						g.FillRectangle(backBrush, rect);

						backBrush.Dispose();
						backBrush = null;
					}
					
					// Draw any required lines around the border of the group
					Rectangle insideRect = DrawGroupBorder(tc, n, g, rect);
					
					// Do we need to draw taking into account the image box?
					if (tc.GroupImageBox)
					{
						// Find the size of the image box area
						Rectangle imageBox = new Rectangle(rect.Left, rect.Top, tc.GroupImageBoxWidth, rect.Height);
						
						// Find the size of the remaining space (to be used for text)
						Rectangle textBox = new Rectangle(imageBox.Right + tc.GroupImageBoxGap, rect.Top, 
														rect.Width - imageBox.Width - tc.GroupImageBoxGap, rect.Height);
					
						// Draw just the image box to begin with
						DrawImageBox(tc, n, g, imageBox);
						
						// Now draw any image inside the image box
						DrawImageInsideImageBox(tc, n, g, imageBox);
						
						// Do we need space for the arrow?
						if (tc.GroupArrows)
						{
							// Find the rectangle that contains the arrow
							Rectangle arrowBox = new Rectangle(textBox.Right - _arrowWidthSpace, textBox.Top,
															_arrowWidthSpace, textBox.Height);

							// Are we using office specific drawing?
							if (tc.GroupColoring != GroupColoring.ControlProperties)
							{
								// For office we override the hot node value and drag over
								hotNode = false;
								dragOverNode = false;
							}				

							// Draw the actual arrow image
							DrawArrow(tc, n, g, arrowBox, GetForeColor(tc, n, hotNode, selectedNode));

							// Reduce size of the textBox to remove arrow portion
							textBox.Width -= _arrowWidthSpace;
						}

						// Last we draw the text to the right hand side
						DrawText(tc, n, g, textBox, textBox.Left);
						
						// Move across from the image box
						int diff = imageBox.Right - insideRect.Left + 1;
						insideRect.X += diff;
						insideRect.Width -= diff;
												
						// No longer need the default draw code
						useDefaultDraw = false;
					}
					else
					{
						// Do we need space for the arrow?
						if (tc.GroupArrows)
						{
							// Find the rectangle that contains the arrow
							Rectangle arrowBox = new Rectangle(rect.Right - _arrowWidthSpace, rect.Top, _arrowWidthSpace, rect.Height);

							// Draw the actual arrow image
							DrawArrow(tc, n, g, arrowBox, GetForeColor(tc, n, hotNode, selectedNode));

							// Reduce size of the textBox to remove arrow portion
							rightOffset = _arrowWidthSpace;
						}
					}
					
					// Do we need to draw a focus indication
					if (tc.ContainsFocus && (tc.FocusNode == n))
						base.DrawFocusIndication(tc, n, g, insideRect);
				}
			}
			
			if (useDefaultDraw)
			{
				// Let base class draw any check mark, image and text
				base.Draw(tc, n, g, clipRectangle, leftOffset, rightOffset);
			}
		}
        
		/// <summary>
		/// Calculate the correct colors to use for the Office2003 group coloring.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="selectedNode">Is the node is selected state.</param>
		/// <param name="hotNode">Is the node being hot tracked.</param>
		/// <param name="top">Returned top color.</param>
		/// <param name="bottom">Returned bottom color.</param>
		public virtual void GetGroupColouringOffice(TreeControl tc,
													bool selectedNode, 
													bool hotNode, 
													ref Color top, 
													ref Color bottom)
		{
            switch (tc.GroupColoring)
            {
                case GroupColoring.Office2007BlueDark:
                case GroupColoring.Office2007SilverDark:
                case GroupColoring.Office2007BlackDark:
                    if (selectedNode && tc.ContainsFocus)
                    {
                        top = Office2007ColorTable.SelectedActiveLight(VisualStyleFromGroupColouring(tc.GroupColoring));
                        bottom = Office2007ColorTable.SelectedActiveDark(VisualStyleFromGroupColouring(tc.GroupColoring));
                    }
                    else
                    {
                        top = Office2007ColorTable.TitleActiveLight(VisualStyleFromGroupColouring(tc.GroupColoring));
                        bottom = Office2007ColorTable.TitleActiveDark(VisualStyleFromGroupColouring(tc.GroupColoring));
                    }
                    break;
                case GroupColoring.Office2007BlueLight:
                case GroupColoring.Office2007SilverLight:
                case GroupColoring.Office2007BlackLight:
                    if (selectedNode && tc.ContainsFocus)
                    {                        
                        top = Office2007ColorTable.SelectedActiveLight(VisualStyleFromGroupColouring(tc.GroupColoring));
                        bottom = Office2007ColorTable.SelectedActiveDark(VisualStyleFromGroupColouring(tc.GroupColoring));
                    }
                    else
                    {
                        top = Office2007ColorTable.TitleInactiveLight(VisualStyleFromGroupColouring(tc.GroupColoring));
                        bottom = Office2007ColorTable.TitleInactiveDark(VisualStyleFromGroupColouring(tc.GroupColoring));
                    }
                    break;
                case GroupColoring.MediaPlayerBlueDark:
                case GroupColoring.MediaPlayerOrangeDark:
                case GroupColoring.MediaPlayerPurpleDark:
                    if (selectedNode && tc.ContainsFocus)
                    {
                        top = MediaPlayerColorTable.SelectedActiveLight(VisualStyleFromGroupColouring(tc.GroupColoring));
                        bottom = MediaPlayerColorTable.SelectedActiveDark(VisualStyleFromGroupColouring(tc.GroupColoring));
                    }
                    else
                    {
                        top = MediaPlayerColorTable.TitleActiveLight(VisualStyleFromGroupColouring(tc.GroupColoring));
                        bottom = MediaPlayerColorTable.TitleActiveDark(VisualStyleFromGroupColouring(tc.GroupColoring));
                    }
                    break;
                case GroupColoring.MediaPlayerBlueLight:
                case GroupColoring.MediaPlayerOrangeLight:
                case GroupColoring.MediaPlayerPurpleLight:
                    if (selectedNode && tc.ContainsFocus)
                    {
                        top = MediaPlayerColorTable.SelectedActiveLight(VisualStyleFromGroupColouring(tc.GroupColoring));
                        bottom = MediaPlayerColorTable.SelectedActiveDark(VisualStyleFromGroupColouring(tc.GroupColoring));
                    }
                    else
                    {
                        top = MediaPlayerColorTable.TitleInactiveLight(VisualStyleFromGroupColouring(tc.GroupColoring));
                        bottom = MediaPlayerColorTable.TitleInactiveDark(VisualStyleFromGroupColouring(tc.GroupColoring));
                    }
                    break;
                default:
                    if (selectedNode)
                    {
                        if (hotNode)
                        {
                            top = tc.ColorDetails.TrackLightLightColor2;
                            bottom = tc.ColorDetails.TrackLightLightColor1;
                        }
                        else
                        {
                            if (tc.ContainsFocus)
                            {
                                top = tc.ColorDetails.TrackLightLightColor1;
                                bottom = tc.ColorDetails.TrackLightLightColor2;
                            }
                            else
                            {
                                if (tc.GroupColoring == GroupColoring.Office2003Light)
                                {
                                    top = tc.ColorDetails.BaseColor1;
                                    bottom = tc.ColorDetails.BaseColor2;
                                }
                                else
                                {
                                    top = tc.ColorDetails.DarkBaseColor;
                                    bottom = tc.ColorDetails.DarkBaseColor2;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (hotNode)
                        {
                            top = tc.ColorDetails.TrackLightColor1;
                            bottom = tc.ColorDetails.TrackLightColor2;
                        }
                        else
                        {
                            if (tc.GroupColoring == GroupColoring.Office2003Light)
                            {
                                top = tc.ColorDetails.BaseColor1;
                                bottom = tc.ColorDetails.BaseColor2;
                            }
                            else
                            {
                                top = tc.ColorDetails.DarkBaseColor;
                                bottom = tc.ColorDetails.DarkBaseColor2;
                            }
                        }
                    }
                    break;
            }
		}
		/// <summary>
		/// Draw focus indication around the provided rectangle.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <param name="rect">Rectangle to draw text inside.</param>
		public override void DrawFocusIndication(TreeControl tc, Node n, Graphics g, Rectangle rect)
		{		
			// We do nothing extra here
		}

		/// <summary>
		/// Draw the border around the group node in appropriate style.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <param name="rect">Rectangle that represents the node drawing area.</param>
		/// <returns>The area inside the group border.</returns>
		public virtual Rectangle DrawGroupBorder(TreeControl tc, Node n, Graphics g, Rectangle rect)
		{
			Rectangle insideRect = rect;
		
			// If there any border stype to draw at all?
			if (tc.GroupBorderStyle != GroupBorderStyle.None)
			{
				// Cache hot/selected information
				bool hotNode = tc.GroupHotTrack && (tc.HotNode == n);
				bool selectedNode = n.IsSelected;

				bool noBorder = (tc.BorderStyle == TreeBorderStyle.None);
			
				// Cache if the control has no sides drawn
				bool noLeftSide = (tc.BorderIndent.Left > 0) || noBorder;
				bool noRightSide = (tc.BorderIndent.Right > 0) || noBorder;
				
				// Recover the actual control size in node space
				Point topRight = tc.ClientToNodeSpace(new Point(tc.DrawRectangle.Right, tc.DrawRectangle.Top));

				// If we are allowed to draw the top edge
				if (tc.GroupBorderStyle != GroupBorderStyle.BottomEdge)
				{
					// If we are drawing the top node then we should draw the top border if 
					// the control has no borders at all or if there is a top indent.
					bool drawTop = (tc.IsFirstDisplayedNode(n)) && ((tc.BorderIndent.Top > 0) || noBorder);

					// Do we need to test if this is the first visible node?
					if (!drawTop && noBorder)
					{
						// Get the bounding rectangle of the node
						Rectangle nBounds = n.Cache.Bounds;

						// If this the first visible node 
						drawTop = (nBounds.Top <= topRight.Y) && (nBounds.Bottom >= topRight.Y);
					}
					
					// Do we need to search previous nodes?
					if (!drawTop)
					{
						// Then search siblings...
						for(int i=n.Index-1; i>=0; --i)
						{
							Node previous = n.ParentNodes[i];
							
							// For first previous node that is visible...
							if (previous.Visible)
							{
								// If it is expanded then might need a separator
								if (previous.Expanded)
								{
									// If there is a gap between end of previous node nad start of this one
									if (previous.Bounds.Bottom < (n.Bounds.Top - 1))
									{
										// Then definitely need a separator
										drawTop = true;
									}
								}
								
								break;
							}
						}
					}

					// Do we need to draw a top line separator?
					if (drawTop)
					{
						// Are we using office specific drawing?
						if (tc.GroupColoring != GroupColoring.ControlProperties)
						{
							Color top = Color.Empty;
							Color bottom = Color.Empty;

                            Color borderColor;

                            switch (tc.GroupColoring)
                            {
                                case GroupColoring.Office2007BlueLight:
                                case GroupColoring.Office2007BlueDark:
                                case GroupColoring.Office2007SilverLight:
                                case GroupColoring.Office2007SilverDark:
                                case GroupColoring.Office2007BlackLight:
                                case GroupColoring.Office2007BlackDark:
                                    borderColor = Office2007ColorTable.TitleBorderColorLight(VisualStyleFromGroupColouring(tc.GroupColoring));
                                    break;
                                case GroupColoring.MediaPlayerBlueLight:
                                case GroupColoring.MediaPlayerBlueDark:
                                case GroupColoring.MediaPlayerOrangeLight:
                                case GroupColoring.MediaPlayerOrangeDark:
                                case GroupColoring.MediaPlayerPurpleLight:
                                case GroupColoring.MediaPlayerPurpleDark:
                                    borderColor = MediaPlayerColorTable.TitleBorderColorLight(VisualStyleFromGroupColouring(tc.GroupColoring));
                                    break;
                                default:
                                    GetGroupColouringOffice(tc, selectedNode, hotNode, ref top, ref bottom);
                                    break;
                            }
					
							// Draw a separator line at top of the area
							using(Pen topPen = new Pen(bottom))
								g.DrawLine(topPen, rect.Left, rect.Top, rect.Right, rect.Top);
						}
						else
						{					
							// Draw a separator line at top of the area
							g.DrawLine(tc.GetCacheGroupLinePen(), rect.Left, rect.Top, rect.Right, rect.Top);
						}
												
						// Reduce the inner rectangle
						insideRect.Y++;
						insideRect.Height--;
					}
				}

				// Decided if we should exclude the edges from being drawn
				if ((tc.GroupBorderStyle == GroupBorderStyle.VerticalEdges) ||
					(tc.GroupBorderStyle == GroupBorderStyle.BottomEdge))
				{
					// Pretend we have sides as we do not want them drawn
					noLeftSide = false;
					noRightSide = false;
				}
						
				// The only case when we do not draw the bottom border is when we are the last node
				// to be displayed and we are positioned at the bottom of the display and there is a
				// border around the control. In that case we do not want a bottom line which only makes
				// the bottom of the control look funny.
				bool noBottom = (tc.IsLastDisplayedNode(n)) && (tc.BorderIndent.Bottom == 0) && !noBorder
								&& ((rect.Bottom - topRight.Y) == (tc.DrawRectangle.Bottom - 1));
						
				if (!noBottom)
				{				
					// Are we using office specific drawing?
					if (tc.GroupColoring != GroupColoring.ControlProperties)
					{
                        Color borderColor;

                        switch (tc.GroupColoring)
                        {
                            case GroupColoring.Office2007BlueLight:
                            case GroupColoring.Office2007BlueDark:
                            case GroupColoring.Office2007SilverLight:
                            case GroupColoring.Office2007SilverDark:
                            case GroupColoring.Office2007BlackLight:
                            case GroupColoring.Office2007BlackDark:
                                borderColor = Office2007ColorTable.TitleBorderColorDark(VisualStyleFromGroupColouring(tc.GroupColoring));
                                break;
                            case GroupColoring.MediaPlayerBlueLight:
                            case GroupColoring.MediaPlayerBlueDark:
                            case GroupColoring.MediaPlayerOrangeLight:
                            case GroupColoring.MediaPlayerOrangeDark:
                            case GroupColoring.MediaPlayerPurpleLight:
                            case GroupColoring.MediaPlayerPurpleDark:
                                borderColor = MediaPlayerColorTable.TitleBorderColorDark(VisualStyleFromGroupColouring(tc.GroupColoring));
                                break;
                            default:
                                borderColor = tc.ColorDetails.ActiveBorderColor;
                                break;
                        }

						// Draw a separator line at bottom of the area
                        using (Pen bottomPen = new Pen(borderColor))
							g.DrawLine(bottomPen, rect.Left, rect.Bottom - 1, rect.Right, rect.Bottom - 1);
					}
					else
					{					
						// Only draw the bottom separator when required
						g.DrawLine(tc.GetCacheGroupLinePen(), rect.Left, rect.Bottom - 1, rect.Right, rect.Bottom - 1);
					}
				}
				
				// Reduce the inner rectangle
				insideRect.Height--;

				// Do we need to test for drawing the left or right hand side separators?
				if ((tc.GroupBorderStyle != GroupBorderStyle.VerticalEdges) &&
				    (tc.GroupBorderStyle != GroupBorderStyle.BottomEdge))
				{				
					// If the drawing area is less than the inner area we need right border
					if (noRightSide || (tc.DrawRectangle.Right != tc.InnerRectangle.Right))
					{
						// Draw a separator at right of area
						g.DrawLine(tc.GetCacheGroupLinePen(), rect.Right - 1, rect.Top, 
															  rect.Right - 1, rect.Bottom - 1);
															  
						// Reduce the inner rectangle
						insideRect.Width--;
					}
					
					if (noLeftSide)
					{
						// Draw a separator at left of area
						g.DrawLine(tc.GetCacheGroupLinePen(), rect.Left, rect.Top, 
															  rect.Left, rect.Bottom - 1);

						// Reduce the inner rectangle
						insideRect.X++;
						insideRect.Width--;
					}
				}
			}
			
			return insideRect;
		}

		/// <summary>
		/// Draw just the image box for a group node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <param name="rect">Rectangle to draw image box inside.</param>
		public virtual void DrawImageBox(TreeControl tc, Node n, Graphics g, Rectangle rect)
		{
			// Cache the background color for drawing the image box background
			Color backColor;

			bool selectedNode = n.IsSelected;
			
			// If you are not allowed to select group nodes then we use the selection 
			// colors for nodes that are expanded and not for those selected
			if (!tc.GroupNodesSelectable)
				selectedNode = n.Expanded;
			
			// Color depends on selected state of node
			if (selectedNode)
				backColor = tc.GroupImageBoxSelectedBackColor;
			 else
				backColor = tc.GroupImageBoxBackColor;

			Brush backBrush;

			// Do we create a gradiant back brush?
			if (tc.GroupImageBoxGradientBack)
			{
				// Inflate rect to wash out colours
				Rectangle gradientRect = rect;
				gradientRect.Inflate(rect.Width / 4, rect.Height / 4);

				Color start = Color.Empty;
				Color end = Color.Empty;

				// Find the correct gradient colors based on scheme
				tc.GradientColors(tc.GroupImageBoxGradientColoring, backColor, ref start, ref end);
					
				// Gradient is based on the required angle
				backBrush = new LinearGradientBrush(gradientRect, start, end, (int)tc.GroupImageBoxGradientAngle);
			}
			else
			{
				// We use just a simple solid colour
				backBrush = new SolidBrush(backColor);
			}

			// Fill the entire background with brush (not just text area)
			g.FillRectangle(backBrush, rect);
		
			backBrush.Dispose();
			backBrush = null;

			// Do we need to draw the image box border?
			if (tc.GroupImageBoxBorder)
			{
				// Cache the line drawing pen
				Pen linePen = tc.GetCacheGroupImageBoxLinePen();
								
				// Recover the actual control size in node space
				Point topRight = tc.ClientToNodeSpace(new Point(tc.DrawRectangle.Right, tc.DrawRectangle.Top));

				bool noBorder = (tc.BorderStyle == TreeBorderStyle.None);

				// If we are drawing the top node then we should draw the top border if 
				// the control has no borders at all or if there is a top indent.
				bool drawTop = (tc.IsFirstDisplayedNode(n)) && ((tc.BorderIndent.Top > 0) || noBorder);

				// The only case when we do not draw the bottom border is when we are the last node
				// to be displayed and we are positioned at the bottom of the display and there is a
				// border around the control. In that case we do not want a bottom line which only makes
				// the bottom of the control look funny.
				bool noBottom = (tc.IsLastDisplayedNode(n)) && (tc.BorderIndent.Bottom == 0) && !noBorder
								&& ((rect.Bottom - topRight.Y) == (tc.InnerRectangle.Bottom - 1));

				// Always draw the right line of the image box
				g.DrawLine(linePen, rect.Right, rect.Bottom - 1, rect.Right, rect.Top);

				// Draw the bottom line?
				if (!noBottom)
					g.DrawLine(linePen, rect.Left, rect.Bottom - 1, rect.Right, rect.Bottom - 1);

				// Only need the left box line if we do not have a border 
				// or there is  an indent on the left hand side.
				if (noBorder || (tc.BorderIndent.Left > 0))
					g.DrawLine(linePen, rect.Left, rect.Top, rect.Left, rect.Bottom - 1);
								
				// Do we need to test if this is the first visible node?
				if (!drawTop && noBorder)
				{
					// Get the bounding rectangle of the node
					Rectangle nBounds = n.Cache.Bounds;

					// If this the first visible node 
					drawTop = (nBounds.Top <= topRight.Y) && (nBounds.Bottom >= topRight.Y);
				}

				// Do we need to search previous nodes?
				if (!drawTop)
				{
					// Then search siblings...
					for(int i=n.Index-1; i>=0; --i)
					{
						Node previous = n.ParentNodes[i];
								
						// For first previous node that is visible...
						if (previous.Visible)
						{
							// If it is expanded then might need a separator
							if (previous.Expanded)
							{
								// If there is a gap between end of previous node nad start of this one
								if (previous.Bounds.Bottom < (n.Bounds.Top - 1))
								{
									// Then definitely need a separator
									drawTop = true;
								}
							}

							break;
						}
					}
				}
				
				// Do we also need to draw the top line of the rectangle?
				if (drawTop)
					g.DrawLine(linePen, rect.Left, rect.Top, rect.Right, rect.Top);
			}
		}
		
		/// <summary>
		/// Draw the optional image inside the image box.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <param name="rect">Rectangle to draw image box inside.</param>
		public virtual void DrawImageInsideImageBox(TreeControl tc, Node n, Graphics g, Rectangle rect)
		{
			// Calculate space requested for drawing an image
			Size imageSize = GetImageSize(tc, n);
			
			// If there is an image to draw
			if (imageSize != Size.Empty)
			{
				int yOffset = 0;
				int xOffset = 0;
				int yHeight = imageSize.Height;
				int xWidth = imageSize.Width;

				// If the image is taller than the available space
				if (imageSize.Height > rect.Height)
					yHeight = rect.Height;
				else
					yOffset = (rect.Height - imageSize.Height) / 2;

				// If the image is wider than the available space
				if (imageSize.Width > rect.Height)
					xWidth = rect.Width;
				else
					xOffset = (rect.Width - imageSize.Width) / 2;

				// Request the image be drawn at calculated position
				DrawImage(tc, n, g, rect.X + xOffset, 
									rect.Y + yOffset, 
									xWidth, yHeight);
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
		public override void DrawText(TreeControl tc, Node n, Graphics g, Rectangle rect, int hotLeft)
		{
			// Do not draw text if the text is being edited
			if (tc.LabelEditNode != n)
			{		
				// Non-group nodes use the simple default processing
				if (!IsRootNode(tc, n))
				{
					// Use the rendering hint defined by the user
					g.TextRenderingHint = tc.TextRenderingHint;
				
					// Let base class do standard drawing
					base.DrawText(tc, n, g, rect, hotLeft);
				}
				else
				{
					bool hotNode = tc.GroupHotTrack && (tc.HotNode == n);
					bool dragOverNode = (tc.DragOverNode == n);
					bool selectedNode = n.IsSelected;

					// If you are not allowed to select group nodes then we use the selection 
					// colors for nodes that are expanded and not for those selected
					if (!tc.GroupNodesSelectable)
						selectedNode = n.Expanded;

					// Get appropriate color for drawing node foreground
					Color foreColor = GetForeColor(tc, n, hotNode, selectedNode || dragOverNode);

					// Use the node specific font as first preference
					Font drawFont = n.GetNodeFont();

					// If not defined, then get the tree level define group font
					if (drawFont == null)
						drawFont = tc.GroupFont;

					// Use the rendering hint defined by the user
					g.TextRenderingHint = tc.GroupTextRenderingHint;

					// Create the forground brush 
					using(SolidBrush foreBrush = new SolidBrush(foreColor))
					{
						bool drawn = false;

						// Is the node hot tracking?
						if (hotNode && tc.GroupUseHotFontStyle)
						{
							// ....then update font with hot font style
							using(Font hotFont = new Font(drawFont, tc.GroupHotFontStyle))
								g.DrawString(n.Text, hotFont, foreBrush, rect, _format);

							// Used custom font style for drawing
							drawn = true;
						}
						else
						{
							// Is the node selected?
							if (selectedNode && tc.GroupUseSelectedFontStyle)
							{
								// ....then update font with selected font style
								using(Font selectedFont = new Font(drawFont, tc.GroupSelectedFontStyle))
									g.DrawString(n.Text, selectedFont, foreBrush, rect, _format);

								// Used custom font style for drawing
								drawn = true;
							}
						}

						if (!drawn)
						{
							// Draw using the standard font
							g.DrawString(n.Text, drawFont, foreBrush, rect, _format);
						}
					}
				}
			}
		}

		/// <summary>
		/// Draw just the arrow for a group node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <param name="rect">Rectangle to draw image box inside.</param>
		/// <param name="color">Color to draw arrow with.</param>
		public virtual void DrawArrow(TreeControl tc, Node n, Graphics g, Rectangle rect, Color color)
		{
			// Only draw an arrow if the group has some visible children
			if (n.Nodes.VisibleCount > 0)
			{
				// Find the vertical offset for the arrow
				int offset = (rect.Height - _arrowHeightSpace) / 2;

				// Decide which image to draw
				Image arrow = n.Expanded ? _upArrow : _downArrow;

				ColorMap colorMap = new ColorMap();
				
				// Convert from white to required color
				colorMap.OldColor = Color.White;
				colorMap.NewColor = color;

				// Create remap attributes for use by button
				_imageAttr.SetRemapTable(new ColorMap[]{colorMap}, ColorAdjustType.Bitmap);

				// Now draw the actual arrow
				g.DrawImage(arrow, 
							new Rectangle(rect.Left, rect.Top + offset, _arrowWidth, _arrowHeight), 
							0, 0, _arrowWidth, _arrowHeight, 
							GraphicsUnit.Pixel, _imageAttr);
			}
		}

		/// <summary>
		/// Gets the required check states for this Node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <returns>Valid check states for node.</returns>
		public override CheckStates GetCheckStates(TreeControl tc, Node n)
		{
			// Non-group nodes use the simple default processing
			if (!IsRootNode(tc, n))
				return base.GetCheckStates(tc, n);

			CheckStates states;

			// Group nodes only use check box is explicity asked to
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
				default:
					states = CheckStates.None;
					break;
			}

			return states;
		}

		/// <summary>
		/// Returns a value indicating of the point is inside the node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="rect">Rectangle of node area.</param>
		/// <param name="pt">Point where the mouse is clicked.</param>
		/// <returns>true if inside area; otherwise false.</returns>
		public override bool ClickPointInNode(TreeControl tc, Node n, Rectangle rect, Point pt)
		{
			// All clicks are inside group nodes
			if (n.ParentNodes == tc.Nodes)
				return true;
			else
				return rect.Contains(pt);
		}

		/// <summary>
		/// Is the specified Node allowed to be selected?
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <returns>true if node is allowed to be selected; otherwise false.</returns>
		public override bool CanSelectNode(TreeControl tc, Node n)
		{
			// Non-group nodes use the simple default processing
			if (!IsRootNode(tc, n))
				return base.CanSelectNode(tc, n);
			else
				return tc.GroupNodesSelectable && n.Selectable;
		}
		
		/// <summary>
		/// Is the specified Node allowed to be collapsed?
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="key">True if keyboard action.</param>
		/// <param name="mouse">True if mouse action.</param>
		/// <returns>true if node is allowed to be collapsed; otherwise false.</returns>
		public override bool CanCollapseNode(TreeControl tc, Node n, bool key, bool mouse)
		{
			// Ask base class if it allows the node to be collapsed
			bool ret = base.CanCollapseNode(tc, n, key, mouse);
		
			// Non-group nodes use the simple default processing
			if (IsRootNode(tc, n))
			{
				// For root nodes they cannot be collapsed by the user using the 
				// keyboard when using auto collapsing and autoallocate properties
				if (mouse || (key && !(tc.GroupAutoCollapse && tc.GroupAutoAllocate)))
					ret = true;
				else
					ret = false;
			}
			
			return ret;
		}

		/// <summary>
		/// Return a value indicating of the given node should be expanded on a single click.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <returns>Expand action to be taken.</returns>
		public override ClickExpandAction CanExpandOnClick(TreeControl tc, Node n)
		{
			// If the node is not inside the root collection
			if (n.ParentNodes != tc.Nodes)
			{
				// Then use the node level property
				return tc.ClickExpand;
			}
			else
			{
				// Otherwise use the group level property
				return tc.GroupClickExpand;
			}
		}

		/// <summary>
		/// Is AutoEdit enabled for this Node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		public override bool CanAutoEdit(TreeControl tc, Node n)
		{
			// If the node is not inside the root collection
			if (n.ParentNodes != tc.Nodes)
			{
				// Then use the node level property
				return tc.AutoEdit;
			}
			else
			{
				// Otherwise use the group level property
				return tc.GroupAutoEdit;
			}
		}

		/// <summary>
		/// Return a value indicating of the given node should be expanded on a double click.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <returns>Expand action to be taken.</returns>
		public override ClickExpandAction CanExpandOnDoubleClick(TreeControl tc, Node n)
		{
			// If the node is not inside the root collection
			if (n.ParentNodes != tc.Nodes)
			{
				// Then use the node level property
				return tc.DoubleClickExpand;
			}
			else
			{
				// Otherwise use the group level property
				return tc.GroupDoubleClickExpand;
			}
		}

		/// <summary>
		/// Notification that the expanded state of a node has changed.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		public override void NodeExpandedChanged(TreeControl tc, Node n)
		{
			if (n.ParentNodes != tc.Nodes)
				base.NodeExpandedChanged(tc, n);
			else
			{
				// Has the node become expanded and tree wants to group auto collapse
				if (n.Expanded && tc.GroupAutoCollapse)
				{
					// Process all sibling nodes
					foreach(Node sibling in n.ParentNodes)
					{
						// For all other nodes, collapse them
						if (sibling != n)
							sibling.Collapse();
					}
										
					// Must reprocess the drawing calculations for nodes
					tc.InvalidateNodeDrawing();
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
			}
		}

		/// <summary>
		/// Find the displayed node for the given point.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="pt">Point in Client coordinates.</param>
		/// <returns>node reference or null.</returns>
		public override Node DragOverNodeFromPoint(TreeControl tc, Point pt)
		{
			// Grab the correct node for the vertical position
			Node n = tc.FindDisplayNodeFromY(pt.Y);
		
			// Group nodes match for anywhere on the correct line
			if (IsRootNode(tc, n))
				return n;
			else
			{
				// Otherwise we let the base class determine how to process
				return base.DragOverNodeFromPoint(tc, pt);
			}
		}

		/// <summary>
		/// Notification that dragging has entered the specified Node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="drgevent">Drag event information.</param>
		public override void DragEnter(TreeControl tc, Node n, DragEventArgs drgevent)
		{
			// Group nodes use the group specific events
			if (IsRootNode(tc, n))
				tc.OnGroupDragEnter(n, drgevent);
			else
				base.DragEnter(tc, n, drgevent);
		}
		
		/// <summary>
		/// Notification that user is dragging over the specified Node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="drgevent">Drag event information.</param>
		public override void DragOver(TreeControl tc, Node n, DragEventArgs drgevent)
		{
			// Group nodes use the group specific events
			if (IsRootNode(tc, n))
				tc.OnGroupDragOver(n, drgevent);
			else
				base.DragOver(tc, n, drgevent);
		}

		/// <summary>
		/// Notification that dragging has left the specified Node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		public override void DragLeave(TreeControl tc, Node n)
		{
			// Group nodes use the group specific events
			if (IsRootNode(tc, n))
				tc.OnGroupDragLeave(n);
			else
				base.DragLeave(tc, n);
		}

		/// <summary>
		/// Notification that user has dropped onto specified Node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="drgevent">Drag event information.</param>
		public override void DragDrop(TreeControl tc, Node n, DragEventArgs drgevent)
		{
			// Group nodes use the group specific events
			if (IsRootNode(tc, n))
				tc.OnGroupDragDrop(n, drgevent);
			else
				base.DragDrop(tc, n, drgevent);
		}

		/// <summary>
		/// Notification that user is hovering over a drag Node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		public override void DragHover(TreeControl tc, Node n)
		{
			// Group nodes use the group specific events
			if (IsRootNode(tc, n))
			{
				// Ask the tree control if we should expand on hovering
				if (tc.GroupExpandOnDragHover)
				{
					// If the node has children and is not expanded
					if (!n.Expanded && (n.Nodes.VisibleCount > 0))
					{
						// Then try and expand the node
						n.Expand();
					}
				}
			}
			else
				base.DragHover(tc, n);
		}
		
		/// <summary>
		/// Recover the Font used to draw the text in normal mode.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <returns>Font used to draw text in normal mode.</returns>
		public override Font GetNodeFont(TreeControl tc, Node n)
		{
			// Non-group nodes use the simple default processing
			if (!IsRootNode(tc, n))
				return base.GetNodeFont(tc, n);
			else
			{
				// Use the node specific font as first preference
				Font nodeFont = n.GetNodeFont();

				// If not defined, then get the tree level define group font
				if (nodeFont == null)
					nodeFont = tc.GroupFont;
					
				return nodeFont;
			}
		}
		
		/// <summary>
		/// Get the rectangle that represents drawing area for text.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <returns></returns>
		public override Rectangle GetTextRectangle(TreeControl tc, Node n)
		{
			// Non-group nodes use the simple default processing
			if (!IsRootNode(tc, n))
				return base.GetTextRectangle(tc, n);
			else
			{
				// Start with size of total drawing area
				Rectangle rect = n.Cache.Bounds;

				// Recover the actual control size in node space
				Point rightSide = tc.ClientToNodeSpace(new Point(tc.DrawRectangle.Right, 0));

				// Alter the drawing rect to cover whole control width
				rect.X = 0;
				rect.Width = rightSide.X > rect.Width ? rightSide.X : rect.Width;

				// Do we need to take into account the image box?
				if (tc.GroupImageBox)
				{
					// Reduce space by that taken up by image box area
					int imageBoxWidth = tc.GroupImageBoxWidth + tc.GroupImageBoxGap;
					rect.X += imageBoxWidth;
					rect.Width -= imageBoxWidth;
				}
				else
				{
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
				}

				// Do we need to take into account the group arrow?
				if (tc.GroupArrows)
					rect.Width -= _arrowWidthSpace;

				// Remaining space is for drawing text
				return rect;
			}
		}

		/// <summary>
		/// Get the foreground color to use based on node and tree settings.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="hotNode">Is this node being hot tracked.</param>
		/// <param name="selectedNode">Is this node selected.</param>
		/// <returns></returns>
		public virtual Color GetForeColor(TreeControl tc, Node n, bool hotNode, bool selectedNode)
		{
			Color foreColor;

            if (selectedNode)
                foreColor = tc.GroupSelectedForeColor;
            else
            {
                // Can only use hot node foreground, if a color has been specified
                if (hotNode && (tc.GroupHotForeColor != Color.Empty))
                    foreColor = tc.GroupHotForeColor;
                else
                {
                    // By preference we use the node level foreground color
                    foreColor = n.GetNodeForeColor();

                    // If non is explicitly defined then get the tree defined color
                    if (foreColor == Color.Empty)
                        foreColor = tc.GroupForeColor;
                }
            }

			return foreColor;
		}

		/// <summary>
		/// Get the background color to use based on node and tree settings.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="hotNode">Is this node being hot tracked.</param>
		/// <param name="selectedNode">Is this node selected.</param>
		/// <returns></returns>
		public virtual Color GetBackColor(TreeControl tc, Node n, bool hotNode, bool selectedNode)
		{
			Color backColor;
	
			if (selectedNode)
			{
				if (tc.ContainsFocus)
					backColor = tc.GroupSelectedBackColor;
				else
					backColor = tc.GroupSelectedNoFocusBackColor;
			}
			else
			{
				if (hotNode && (tc.GroupHotBackColor != Color.Empty))
					backColor = tc.GroupHotBackColor;
				else
					backColor = tc.GroupBackColor;
			}		

			return backColor;
		}

		private void EnforceSingleExpandedGroup(TreeControl tc)
		{
			bool found = false;
		
			// Scan all the root level nodes
			foreach(Node n in tc.Nodes)
			{
				// Not interested in hidden nodes
				if (n.Visible)
				{
					// If expanded then we need to take action
					if (n.Expanded)
					{
						// If the first expanded node, do nothing
						if (!found)
							found = true;
						else
						{
							// Otherwise we must close it up
							n.Collapse();
						}
					}
				}
			}
		}

        private VisualStyle VisualStyleFromGroupColouring(GroupColoring gc)
        {
            switch (gc)
            {
                case GroupColoring.MediaPlayerBlueLight:
                case GroupColoring.MediaPlayerBlueDark:
                    return VisualStyle.MediaPlayerBlue;
                case GroupColoring.MediaPlayerOrangeLight:
                case GroupColoring.MediaPlayerOrangeDark:
                    return VisualStyle.MediaPlayerOrange;
                case GroupColoring.MediaPlayerPurpleLight:
                case GroupColoring.MediaPlayerPurpleDark:
                    return VisualStyle.MediaPlayerPurple;
                case GroupColoring.Office2007BlueDark:
                case GroupColoring.Office2007BlueLight:
                    return VisualStyle.Office2007Blue;
                case GroupColoring.Office2007SilverDark:
                case GroupColoring.Office2007SilverLight:
                    return VisualStyle.Office2007Silver;
                case GroupColoring.Office2007BlackDark:
                case GroupColoring.Office2007BlackLight:
                    return VisualStyle.Office2007Black;
                case GroupColoring.Office2003Dark:
                case GroupColoring.Office2003Light:
                    return VisualStyle.Office2003;
                default:
                    return VisualStyle.Plain;
            }
        }

		private void OnAutoCollapseChanged(object sender, EventArgs e)
		{
			EnforceSingleExpandedGroup(sender as TreeControl);
		}
	}
}
