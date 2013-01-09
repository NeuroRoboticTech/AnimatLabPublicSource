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
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using Crownwood.DotNetMagic.Common;
using Microsoft.Win32;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Implements the default drawing implementation for a NodeCollection.
	/// </summary>
	public class DefaultCollectionVC : INodeCollectionVC
	{
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
		/// Helper function that indicates if given collection is the root.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="nc">Reference to NodeCollection instance.</param>
		/// <returns></returns>
		public virtual bool IsRootCollection(TreeControl tc, NodeCollection nc)
		{
			return (tc.Nodes == nc);
		}

		/// <summary>
		/// Calculate and return the extra space needed for drawing a NodeCollection instance.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="nc">Reference to NodeCollection instance.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <returns>Extra space needed for each edge of the collection area.</returns>
		public virtual Edges MeasureEdges(TreeControl tc, NodeCollection nc, Graphics g)
		{
			// Defualt left column width
			int leftWidth = tc.ColumnWidth;

			// If the root collection...
			if (IsRootCollection(tc, nc))
			{
				// ...does not have any lines or boxes then no point in 
				// having the column spacing taking up room, so remove it
				if (!ShowLines(tc, nc) && !ShowBoxes(tc, nc))
					leftWidth = 0;
					
				// Do we need extra space for indicators
				if (tc.Indicators != Indicators.None)
					leftWidth += tc.IndicatorSize.Width;
			}
			else
			{
				// Do we need extra space for indicators
				if (tc.Indicators == Indicators.AtGroup)
					leftWidth += tc.IndicatorSize.Width;
			}
			
			return new Edges(leftWidth, 0, 0, 0);
		}

		/// <summary>
		/// Defines the bounding rectangle of the collection.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="nc">Reference to NodeCollection instance.</param>
		/// <param name="bounds">Bounding rectangle of collection.</param>
		public virtual void SetBounds(TreeControl tc, NodeCollection nc, Rectangle bounds)
		{
			// Update cache bounds using new value
			nc.Cache.Bounds = bounds;
		}

		/// <summary>
		/// Find if the provided rectangle intersects the bounds of the node collection.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="nc">Reference to NodeCollection instance.</param>
		/// <param name="rectangle">Rectangle to compare against.</param>
		/// <returns>True if intersecting; otherwise false.</returns>
		public virtual bool IntersectsWith(TreeControl tc, NodeCollection nc, Rectangle rectangle)
		{
			return nc.Cache.Bounds.IntersectsWith(rectangle);
		}

		/// <summary>
		/// Draw the NodeCollection instance.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="nc">Reference to NodeCollection instance.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <param name="clipRectangle">Clipping rectangle used when drawing.</param>
		/// <param name="preDraw">True when drawing before children and fales for drawing after.</param>
		public virtual void Draw(TreeControl tc, 
								 NodeCollection nc, 
								 Graphics g, 
								 Rectangle clipRectangle, 
								 bool preDraw)
		{
			if (preDraw)
			{
				// Only draw lines and boxes if there is at least one child node
				if (nc.VisibleCount > 0)
				{
					// Find out if lines and boxes needed drawing
					bool showLines = ShowLines(tc, nc);
					bool showBoxes = ShowBoxes(tc, nc);
					bool showIndicators = (IsRootCollection(tc, nc) && (tc.Indicators != Indicators.None)) ||
										  (!IsRootCollection(tc, nc) && (tc.Indicators == Indicators.AtGroup));

					// Is there any drawing to perform?
					if (showLines || showBoxes || showIndicators)
					{
						Pen lineDashPen = tc.GetCacheLineDashPen();
						
						// Cache the whole rectangle used for drawing
						Rectangle ncBounds = nc.Cache.Bounds;
						
						// Find the size of space taken up with indicators
						Size indicatorSize = showIndicators ? tc.IndicatorSize : Size.Empty;
						
						// Perform any adjustment for drawing within the bounds
						AdjustBeforeDrawing(tc, nc, ref ncBounds);

						// Cache the mid and right hand side of the lines column
						int xMid = ncBounds.Left + indicatorSize.Width + (tc.ColumnWidth / 2) - 1;
						int xRight = ncBounds.Left + indicatorSize.Width + tc.ColumnWidth;
						
						// Need to adjust drawing child connecting lines, so that wider than one
						// pixel pens start from left of vertical for top and bottom children, but
						// are placed to the right of middle children.
						int xMidAdjustL = xMid - (tc.LineWidth / 2);
						int xMidAdjustR = xMid + 1;
						
						// Find the first and last visible nodes
						int firstVisible = nc.FirstVisibleIndex;
						int lastVisible = nc.LastVisibleIndex;

                        // Must have at least one visible child
                        if ((firstVisible >= 0) && (lastVisible >= 0))
                        {
                            // Find the first and last nodes mid points
                            int yMidFirst = nc[firstVisible].Cache.Bounds.Top + (nc[firstVisible].Cache.Bounds.Height / 2);
                            int yMidLast = nc[lastVisible].Cache.Bounds.Top + (nc[lastVisible].Cache.Bounds.Height / 2);

                            // No point drawing line after end of clipping rectangle
                            if (yMidLast > clipRectangle.Bottom)
                                yMidLast = clipRectangle.Bottom;

                            // Do we need to draw the long vertical line?
                            if (showLines)
                            {
                                // Draw the long vertical line down the middle
                                if (IsRootCollection(tc, nc))
                                {
                                    // No point drawing line before start of clipping rectangle
                                    if (yMidFirst < clipRectangle.Top)
                                    {
                                        yMidFirst = clipRectangle.Top;

                                        // Adjust starting point up so dot/dash lines are consistent
                                        switch (tc.LineDashStyle)
                                        {
                                            case LineDashStyle.Dot:
                                                yMidFirst -= yMidFirst % (2 * tc.LineWidth);
                                                break;
                                            case LineDashStyle.Dash:
                                                yMidFirst -= yMidFirst % (4 * tc.LineWidth);
                                                break;
                                        }
                                    }

                                    // We are root collection, so draw from the first node to the last
                                    g.DrawLine(lineDashPen, xMid, yMidFirst, xMid, yMidLast);
                                }
                                else
                                {
                                    // Not at root, so draw from top of column to the last node
                                    g.DrawLine(lineDashPen, xMid, ncBounds.Top, xMid, yMidLast);
                                }
                            }

                            // Process each child node in turn, drawing horizontal line and any box
                            for (int i = nc.ChildFromY(clipRectangle.Top); i < nc.Count; i++)
                                if (DrawNode(tc, nc[i], g, clipRectangle, showLines, showBoxes,
                                             showIndicators, (i == 0) || (i == (nc.Count - 1)),
                                             lineDashPen, xMidAdjustL, xMidAdjustR, xMid, xRight,
                                             ncBounds.Left))
                                    break;
                        }
					}
				}
			}
		}
		
		/// <summary>
		/// Draw any lines/boxes/indicators for a given node and child nodes.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <param name="clipRectangle">Clipping rectangle used when drawing.</param>
		/// <param name="showLines">Should lines for drawn for node.</param>
		/// <param name="showBoxes">Should boxes for drawn for node.</param>
		/// <param name="showIndicators">Should indicators for drawn for node.</param>
		/// <param name="firstOrLast">Is node the first of last in collection.</param>
		/// <param name="lineDashPen">Pen used to drawing lines.</param>
		/// <param name="xMidAdjustL">Left adjusted position for column</param>
		/// <param name="xMidAdjustR">Right adjusted position for column.</param>
		/// <param name="xMid">Middle position for column.</param>
		/// <param name="xRight">Right position for column.</param>
		/// <param name="xLeft">Left position for column.</param>
		/// <returns>true if node is outside of clipping rectangle; otherwise false.</returns>
		public virtual bool DrawNode(TreeControl tc, 
									 Node n, 
									 Graphics g,
									 Rectangle clipRectangle,
									 bool showLines,
									 bool showBoxes,
									 bool showIndicators,
									 bool firstOrLast,
									 Pen lineDashPen,
									 int xMidAdjustL,
									 int xMidAdjustR,
									 int xMid,
									 int xRight,
									 int xLeft)
		{
			// Only draw line/boxe/indicator for child if it is visible
			if (n.Visible)
			{
				// Get bounding rectangle for the child
				Rectangle nBounds = n.Cache.Bounds;

				// If this child is past end of clipping region, no point in processing
				// any more children because none of them will be inside clipping region
				if (nBounds.Top > clipRectangle.Bottom)
					return true;

				// Find the mid vertical point
				int yMidLast = nBounds.Top + (nBounds.Height / 2);

				// Draw the horizontal line
				if (showLines)
				{
					// Is this the first or last child?
					if (firstOrLast)
						g.DrawLine(lineDashPen, xMidAdjustL, yMidLast, xRight, yMidLast);
					else
						g.DrawLine(lineDashPen, xMidAdjustR, yMidLast, xRight, yMidLast);
				}

				// Draw any expand/collapse box needed
				if (showBoxes)
					DrawExpandCollapseBox(tc, n, g, xMid, yMidLast);
									
				// Do we need to draw indicators?
				if (showIndicators)
				{
					// Do we need to draw an indicator for the node?
					if (n.Indicator != Indicator.None)
						DrawIndicator(tc, n, g, xLeft, yMidLast);
						
					// Only need to recurse for root level indicators
					if (tc.Indicators == Indicators.AtRoot)
					{
						if (n.Expanded)
						{
							// Must process all children recursively for any indicators
							for(int j=0; j<n.Nodes.Count; j++)
							{
								Node nj = n.Nodes[j];
								
								// Only interested in those that are visible
								if (nj.Visible)
								{
									// Only draw any indicator for the child node
									if (DrawNode(tc, nj, g, clipRectangle, false, false, 
												 showIndicators, (j == 0) || (j == (n.Nodes.Count - 1)), 
												 lineDashPen, xMidAdjustL, xMidAdjustR, xMid, xRight, xLeft))
										break;
								}
							}
						}
					}
				}
			}
			
			return false;
		}
		
		/// <summary>
		/// Draw an appropriate image for a node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <param name="x">Left X position for drawing indicator.</param>
		/// <param name="y">Middle Y position for drawing indicator.</param>
		public virtual void DrawIndicator(TreeControl tc, Node n, Graphics g, int x, int y)
		{
			Image image = tc.GetIndicatorImage(n.Indicator);
			g.DrawImage(image, x, y - (tc.IndicatorSize.Height / 2));
			image.Dispose();
		}

		/// <summary>
		/// Draw an expand/collapse box for the given node using provided centre point.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <param name="x">X centre of box.</param>
		/// <param name="y">Y centre of box.</param>
		public virtual void DrawExpandCollapseBox(TreeControl tc, Node n, Graphics g, int x, int y)
		{
			bool hasChildren = (n.Nodes.VisibleCount > 0);
		
			// Do we need to draw a box for this child node?
			if (tc.BoxShownAlways || hasChildren)
			{
				// Create rectangle at centre of box position
				Rectangle box = new Rectangle(x, y, 0, 0);

				// If we theme information and the boxes need drawing themed
				if (tc.IsControlThemed && (tc.BoxDrawStyle == DrawStyle.Themed))
				{
					// Themes cannot support the drawing of an empty expand/collapse box
					if (hasChildren)
					{
						// Find how far to expand left and up
						int halfV = tc.GlyphThemeSize.Height / 2;
						int halfH = tc.GlyphThemeSize.Width / 2;

						// Move box so that centre of box is centred
						box.Y -= halfV;
						box.X -= halfH;

						// Expand it to required size
						box.Size = tc.GlyphThemeSize;

						// Ask the tree to draw the themed box in required state
						tc.DrawThemedBox(g, box, n.IsExpanded);
					}
				}
				else
				{
					// Should we draw in the gradient style?
					bool gradient = (tc.BoxDrawStyle == DrawStyle.Gradient);

					// Find how far to expand left and up
					int half = tc.BoxLength / 2;

					// Expand it to required size
					box.X -= half;
					box.Y -= half;
					box.Width = tc.BoxLength - 1;
					box.Height = tc.BoxLength - 1;

					if (gradient)
					{
						// Modify box very slightly to make gradient look better
						Rectangle boxCopy = box;
						boxCopy.Inflate(1, 1);
						
						// Create graduated brush using inside color and box size
						using(Brush brush = new LinearGradientBrush(boxCopy, 
																    tc.BoxInsideColor,
																    tc.BackColor,
																	225))
						{
							// Fill inside with required brush first
							g.FillRectangle(brush, box);
						}

						// Draw the border as all except the four corner pixels
						using(Pen pen = new Pen(tc.BoxBorderColor, 1),
							      lightPen = new Pen(ControlPaint.Light(tc.BoxBorderColor, 1)))
						{
							g.DrawLine(pen, box.Left + 1, box.Bottom, box.Right - 1, box.Bottom);
							g.DrawLine(pen, box.Right, box.Top + 1, box.Right, box.Bottom - 1);
							g.DrawLine(lightPen, box.Left + 1, box.Top, box.Right - 1, box.Top);
							g.DrawLine(lightPen, box.Left, box.Top + 1, box.Left, box.Bottom - 1);
						}
					}
					else
					{
						// Fill inside with required brush first
						g.FillRectangle(tc.GetCacheBoxInsideBrush(), box);

						// Draw border in required color around box
						g.DrawRectangle(tc.GetCacheBoxBorderPen(), box);
					}

					// Only draw a plus/minus sign if node has children
					if (hasChildren)
					{
						// Cannot draw a sign if the box is too small to show it
						if (tc.BoxLength >= 7)
						{
							Pen boxSignPen = tc.GetCacheBoxSignPen();

							// Find length of the line for drawing sign
							int sign = (tc.BoxLength / 3) - 1;

							// Always draw the minus part of sign
							g.DrawLine(boxSignPen, box.X + half, box.Y + half, box.X + half - sign, box.Y + half);
							g.DrawLine(boxSignPen, box.X + half, box.Y + half, box.X + half + sign, box.Y + half);

							// Only draw the line to make it a plus sign, if not expanded
							if (!n.IsExpanded)
							{
								g.DrawLine(boxSignPen, box.X + half, box.Y + half, box.X + half, box.Y + half - sign);
								g.DrawLine(boxSignPen, box.X + half, box.Y + half, box.X + half, box.Y + half + sign);
							}
						}
					}
					else
					{
						// Otherwise we just draw a dot at the center
						g.DrawLine(tc.GetCacheBoxSignPen(), new PointF(box.X + half, box.Y + half), 
															new PointF(box.X + half + 0.1f, box.Y + half));
					}
				}
			}
		}	

		/// <summary>
		/// Adjust the total bounds of the collection into the bounds available for drawing.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="nc">Reference to NodeCollection instance.</param>
		/// <param name="ncBounds">Drawing bounds to draw within.</param>
		public virtual void AdjustBeforeDrawing(TreeControl tc, NodeCollection nc, ref Rectangle ncBounds)
		{
			// By default we having no changes to make
		}

		/// <summary>
		/// Should lines be drawn for the node collection.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="nc">Reference to NodeCollection instance.</param>
		/// <returns>true if lines are to be drawn; otherwise false.</returns>
		public virtual bool ShowLines(TreeControl tc, NodeCollection nc)
		{
			// Must have some width in order to draw lines
			if (tc.ColumnWidth == 0)
				return false;
		
			switch(tc.LineVisibility)
			{
				case LineBoxVisibility.Nowhere:
					return false;
				case LineBoxVisibility.OnlyAtRoot:
					return IsRootCollection(tc, nc);
				case LineBoxVisibility.OnlyBelowRoot:
					return !IsRootCollection(tc, nc);
				case LineBoxVisibility.Everywhere:
					return true;
				default:
					// Should never happen!
					Debug.Assert(false);
					return true;
			}
		}

		/// <summary>
		/// Should boxes be drawn for the node collection.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="nc">Reference to NodeCollection instance.</param>
		/// <returns>true if lines are to be drawn; otherwise false.</returns>
		public virtual bool ShowBoxes(TreeControl tc, NodeCollection nc)
		{
			// Must have some width in order to draw boxes
			if (tc.ColumnWidth == 0)
				return false;

			switch(tc.BoxVisibility)
			{
				case LineBoxVisibility.Nowhere:
					return false;
				case LineBoxVisibility.OnlyAtRoot:
					return IsRootCollection(tc, nc);
				case LineBoxVisibility.OnlyBelowRoot:
					return !IsRootCollection(tc, nc);
				case LineBoxVisibility.Everywhere:
					return true;
				default:
					// Should never happen!
					Debug.Assert(false);
					return true;
			}
		}

		/// <summary>
		/// Process a left mouse down on the Node instance.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="nc">Reference to NodeCollection instance.</param>
		/// <param name="n">Reference to Node within collection.</param>
		/// <param name="button">Button that is pressed down.</param>
		/// <param name="pt">Point being pressed down.</param>
		/// <returns>true if the point was processed; otherwise false.</returns>
		public virtual bool MouseDown(TreeControl tc, NodeCollection nc, Node n, MouseButtons button, Point pt)
		{
			// Left mouse is used to toggel expand/collapse boxes
			if (button == MouseButtons.Left)
			{
				// Can only toggle if this collection has the node of interest
				if (n != null)
				{
					// Can only toggle if the node has children
					if (n.Nodes.VisibleCount > 0)
					{
						// Can only toggle if there is box to toggle
						if (ShowBoxes(tc, nc))
						{
							// Find the box rectangle
							Rectangle rectBox = GetExpandCollapseBox(tc, nc, n);

							// Make a little bigger so user does not have to be precise
							rectBox.Inflate(2, 2);

							// If the click is on the box
							if (rectBox.Contains(pt))
							{
								// Toggle the expanded state
								if (n.Expanded)
								{
									// Collapse the node if allowed
									if (n.VC.CanCollapseNode(tc, n, false, true))
										n.Collapse();
								}
								else
								{
									// Expand the node if allowed
									if (n.VC.CanExpandNode(tc, n, false, true))
										n.Expand();
								}
								
								// Message handled
								return true;
							}
						}
					}
				}
			}

			// Not processed
			return false;
		}

		/// <summary>
		/// Process a left mouse down on the Node instance.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="nc">Reference to NodeCollection instance.</param>
		/// <param name="n">Reference to Node within collection.</param>
		/// <param name="pt">Point being pressed down.</param>
		/// <returns>true if the point was processed; otherwise false.</returns>
		public virtual bool DoubleClick(TreeControl tc, NodeCollection nc, Node n, Point pt)
		{
			// Not processed
			return false;
		}

		/// <summary>
		/// Gets the bounding rectangle of just the expand/collapse box
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="nc">Reference to NodeCollection instance.</param>
		/// <param name="n">Reference to Node within collection.</param>
		/// <returns>Bounding rectangle of the box.</returns>
		public virtual Rectangle GetExpandCollapseBox(TreeControl tc, NodeCollection nc, Node n)
		{
			Rectangle ncBounds = nc.Cache.Bounds;
			Rectangle nBounds = n.Cache.Bounds;

			// Perform any adjustment for drawing within the bounds
			AdjustBeforeDrawing(tc, nc, ref ncBounds);

			bool showIndicators = (IsRootCollection(tc, nc) && (tc.Indicators != Indicators.None)) ||
								  (!IsRootCollection(tc, nc) && (tc.Indicators == Indicators.AtGroup));
	
			// Find the size of space taken up with indicators
			Size indicatorSize = showIndicators ? tc.IndicatorSize : Size.Empty;

			// Find midpoint of the box
			int xMid = ncBounds.Left + indicatorSize.Width + (tc.ColumnWidth / 2) - 1;
			int yMid = nBounds.Top + (nBounds.Height / 2);

			// Create rectangle at centre of box position
			Rectangle box = new Rectangle(xMid, yMid, 0, 0);

			// If we theme information and the boxes need drawing themed
			if (tc.IsControlThemed && (tc.BoxDrawStyle == DrawStyle.Themed))
			{
				// Find how far to expand left and up
				int halfV = tc.GlyphThemeSize.Height / 2;
				int halfH = tc.GlyphThemeSize.Width / 2;

				// Move box so that centre of box is centred
				box.Y -= halfV;
				box.X -= halfH;

				// Expand it to required size
				box.Size = tc.GlyphThemeSize;
			}
			else
			{
				// Find how far to expand left and up
				int half = tc.BoxLength / 2;

				// Expand it to required size
				box.X -= half;
				box.Y -= half;
				box.Width = tc.BoxLength - 1;
				box.Height = tc.BoxLength - 1;
			}

			return box;
		}

		/// <summary>
		/// Notification that all child nodes are being removed.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="nc">Reference to NodeCollection instance.</param>
		public virtual void NodeCollectionClearing(TreeControl tc, NodeCollection nc)
		{
			// Ensure all children of the node are deselected as they
			// are no longer showing and so cannot be in the selected state
			foreach(Node n in nc)
				tc.DeselectNode(n, true);
				
			// Ask control to move the focus to new calculated position
			tc.NodeContentCleared(false);
		}

		/// <summary>
		/// Notification that the size of the control has changed.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		public virtual void SizeChanged(TreeControl tc)
		{
			// We have nothing extra to do for default collections.
		}

		/// <summary>
		/// Final change to perform drawing.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <param name="displayNodes">Collection of nodes in display.</param>
		public virtual void PostDrawNodes(TreeControl tc, Graphics g, ArrayList displayNodes)
		{
			// We have nothing extra to do for default collections.
		}

		/// <summary>
		/// Called once all the nodes have been calculated ready for display.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="displayNodes">Collection of nodes in display.</param>
		public virtual void PostCalculateNodes(TreeControl tc, ArrayList displayNodes)
		{
			// We have nothing extra to do for default collections.
		}
	}
}
