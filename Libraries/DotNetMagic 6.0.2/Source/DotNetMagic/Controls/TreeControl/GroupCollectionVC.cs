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
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using Crownwood.DotNetMagic.Common;
using Microsoft.Win32;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Implements the group drawing implementation for a NodeCollection.
	/// </summary>
	public class GroupCollectionVC : DefaultCollectionVC
	{
		/// <summary>
		/// Helper function that indicates if given collection is the real root.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="nc">Reference to NodeCollection instance.</param>
		/// <returns></returns>
		public virtual bool IsRealRootCollection(TreeControl tc, NodeCollection nc)
		{
			return (tc.Nodes == nc);
		}

		/// <summary>
		/// Helper function that indicates if given collection is the root.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="nc">Reference to NodeCollection instance.</param>
		/// <returns></returns>
		public override bool IsRootCollection(TreeControl tc, NodeCollection nc)
		{
			return (nc.ParentNode != null) && 
				   (nc.ParentNode.ParentNodes == tc.Nodes);
		}

		/// <summary>
		/// Defines the bounding rectangle of the collection.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="nc">Reference to NodeCollection instance.</param>
		/// <param name="bounds">Bounding rectangle of collection.</param>
		public override void SetBounds(TreeControl tc, NodeCollection nc, Rectangle bounds)
		{
			// If the root below the group node
			if (IsRootCollection(tc, nc))
			{
				// Move the group across by the extra left indent
				bounds.X += tc.GroupIndentLeft;
				bounds.Width -= tc.GroupIndentLeft;
			}

			// Let call continue to base
			base.SetBounds(tc, nc, bounds);
		}

		/// <summary>
		/// Calculate and return the extra space needed for drawing a NodeCollection instance.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="nc">Reference to NodeCollection instance.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <returns>Extra space needed for each edge of the collection area.</returns>
		public override Edges MeasureEdges(TreeControl tc, NodeCollection nc, Graphics g)
		{
			// At the real root we have no edges
			if (IsRealRootCollection(tc, nc))
				return Edges.Empty;
			else
			{
				// Get the size of edges from base class processing
				Edges retEdges = base.MeasureEdges(tc, nc, g);

				if (nc.VisibleCount > 0)
				{
					// At the root level below a group
					if (IsRootCollection(tc, nc))
					{
						// Add on the group extra
						retEdges.Left += tc.GroupIndentLeft;
						retEdges.Top += tc.GroupIndentTop;
						retEdges.Bottom += tc.GroupIndentBottom;
						
						// Do we need to account for the image box column?
						if (tc.GroupImageBox && tc.GroupImageBoxColumn)
						{
							// Add on the width of the image box
							retEdges.Left += tc.GroupImageBoxWidth;
						}
					}
				}
				
				return retEdges;
			}
		}

		/// <summary>
		/// Draw the NodeCollection instance.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="nc">Reference to NodeCollection instance.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <param name="clipRectangle">Clipping rectangle used when drawing.</param>
		/// <param name="preDraw">True when drawing before children and fales for drawing after.</param>
		public override void Draw(TreeControl tc, 
								  NodeCollection nc, 
								  Graphics g, 
								  Rectangle clipRectangle, 
								  bool preDraw)
		{
			// The real root never has anything to draw
			if (!IsRealRootCollection(tc, nc))
			{
				// Let base class do its stuff
				base.Draw(tc, nc, g, clipRectangle, preDraw);

				// Only draw the extra separating lines and image column after children are drawn
				if (!preDraw)
				{
					// The root within each group might have something to do
					if (IsRootCollection(tc, nc))
					{
						// Do we need to account for the image box column?
						if (tc.GroupImageBox && tc.GroupImageBoxColumn)
						{
							// Recover the actual control drawing area in node space
							Rectangle drawRect = tc.ClientToNodeSpace(tc.DrawRectangle);

							// Get the bounding rectangle of the collection
							Rectangle ncBounds = nc.Cache.Bounds;

							// If this the first visible collection 
							bool firstVisibleCollection = (ncBounds.Top <= drawRect.Top) && (ncBounds.Bottom >= drawRect.Top);
							
							// Adjust backwards to account for group indent
							ncBounds.X -= tc.GroupIndentLeft;
							
							// Adjust to just the image column width
							ncBounds.Width = tc.GroupImageBoxWidth;
							
							// Draw the image column color
							g.FillRectangle(tc.GetCacheGroupImageBoxColumnBrush(), ncBounds);
							
							// Draw the separator line on right hand side of column
							g.DrawLine(tc.GetCacheGroupImageBoxLinePen(), ncBounds.Right, ncBounds.Top, 
																		  ncBounds.Right, ncBounds.Bottom - 1);
																		  
							// We draw top and bottom collection separators, if we have no border
							if (tc.BorderStyle == TreeBorderStyle.None)
							{
								// Draw the separator line on right side of control
								g.DrawLine(tc.GetCacheGroupImageBoxLinePen(), drawRect.Right - 1, ncBounds.Top, 
																			  drawRect.Right - 1, ncBounds.Bottom - 1);

								// The first collection drawn on the control...									
								if (firstVisibleCollection)
								{
									// ...needs a top line separator
									g.DrawLine(tc.GetCacheGroupImageBoxLinePen(), ncBounds.Right, drawRect.Top, 
																				  drawRect.Right - 1, drawRect.Top);
								}

								bool lastSibling = true;
								
								// Then search siblings...
								for(int i=nc.ParentNode.Index+1; i<nc.ParentNode.ParentNodes.Count; i++)
								{
									Node next = nc.ParentNode.ParentNodes[i];
									
									// For a node that is visible...
									if (next.Visible)
									{
										lastSibling = false;
										break;
									}
								}
								
								// If the last sibling, or collection runs off end of control
								if (lastSibling || (ncBounds.Bottom > drawRect.Bottom))
								{
									// Find the bottom line for drawing
									int bottom = ncBounds.Bottom - 1;
									
									// If we party run off end of the control the use end of control instead
									if (bottom >= drawRect.Bottom)
										bottom = drawRect.Bottom - 1;							
								
									// ...then draw separator line at bottom of collection
									g.DrawLine(tc.GetCacheGroupImageBoxLinePen(), ncBounds.Right, bottom, 
																				  drawRect.Right - 1, bottom);
								}
							}
						}
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
		public override void AdjustBeforeDrawing(TreeControl tc, NodeCollection nc, ref Rectangle ncBounds)
		{
			// The root within each group might have something to do
			if (IsRootCollection(tc, nc))
			{
				// Do we need to account for the image box column?
				if (tc.GroupImageBox && tc.GroupImageBoxColumn)
				{
					// Do not want base class to draw over the image column
					ncBounds.X += tc.GroupImageBoxWidth;
					ncBounds.Width -= tc.GroupImageBoxWidth;
				}
			}
		}
		
		/// <summary>
		/// Notification that the size of the control has changed.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		public override void SizeChanged(TreeControl tc)
		{
			// Are we requested to allocate spare space?
			if (tc.GroupAutoAllocate)
			{
				// Then changing the size of the control will alter how much space is 
				// reallocate and so need to recalculate all the nodes.
				tc.InvalidateNodeDrawing();
			}
		}

		/// <summary>
		/// Final change to perform drawing.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <param name="displayNodes">Collection of nodes in display.</param>
		public override void PostDrawNodes(TreeControl tc, Graphics g, ArrayList displayNodes)
		{
			// If we are trying to show an image box
			if (tc.GroupImageBox)
			{
				// Find total size covered by the root collection
				Rectangle rootBounds = tc.Nodes.Cache.Bounds;
				
				// Recover the actual control drawing area in node space
				Rectangle drawRect = tc.ClientToNodeSpace(tc.DrawRectangle);
				
				// If there is blank space at the bottom of the nodes
				if (rootBounds.Bottom < drawRect.Bottom)
				{
					// Create rectangle for the undrawn area
					Rectangle fillArea = new Rectangle(drawRect.Left, 
													   rootBounds.Bottom, 
													   drawRect.Width,
													   drawRect.Bottom - rootBounds.Bottom);
													   
					// If we have a border around the control
					if (tc.BorderStyle != TreeBorderStyle.None)
					{
						// Reduce with to just the image column
						fillArea.Width = tc.GroupImageBoxWidth;
						
						// Fill area in image in image column color													   
						g.FillRectangle(tc.GetCacheGroupImageBoxColumnBrush(), fillArea);
						
						// Draw column separator line on right hand side
						g.DrawLine(tc.GetCacheGroupImageBoxLinePen(), fillArea.Right, fillArea.Top,
																	  fillArea.Right, fillArea.Bottom);
					}
					else
					{
						// No border, so just fill entire area in image column color													   
						g.FillRectangle(tc.GetCacheGroupImageBoxColumnBrush(), fillArea);
					}
				}
			}
		}

		/// <summary>
		/// Called once all the nodes have been calculated ready for display.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="displayNodes">Collection of nodes in display.</param>
		public override void PostCalculateNodes(TreeControl tc, ArrayList displayNodes)
		{
			// Are we requested to allocate spare space?
			if (tc.GroupAutoAllocate)
			{	
				// Find total size covered by the root collection
				int rootHeight = tc.Nodes.Cache.Bounds.Height;
				
				// Find height available for drawing
				int drawHeight = tc.DrawRectangle.Height;
				
				// If using pixel level vertical scrolling
				if (tc.VerticalGranularity == VerticalGranularity.Pixel)
				{
					// Then we only have work to do if root is less then draw height
					if (rootHeight < drawHeight)
					{
						// Find the difference that needs allocating
						int allocate = drawHeight - rootHeight;

						Node rootNode = null;
						
						// Process through all the nodes
						for(int i=tc.Nodes.Count-1; i>=0; --i)
						{
							Node n = tc.Nodes[i];
							
							// Is this the expanded and visible node?
							if (n.Visible && n.Expanded)
							{
								// We need to process this node
								rootNode = n;
								break;
							}
						}
						
						// Did we find a match?
						if (rootNode != null)
						{
							// Increate size of the root collection, selected node child 
							// bounds and the selected nodes collection of nodes bounds
							Rectangle nodesBounds = tc.Nodes.Cache.Bounds;
							Rectangle rootBounds = rootNode.Cache.ChildBounds;
							Rectangle childBounds = rootNode.Nodes.Cache.Bounds;
							nodesBounds.Height += allocate;
							rootBounds.Height += allocate;
							childBounds.Height += allocate;
							tc.Nodes.Cache.Bounds =  nodesBounds;
							rootNode.Cache.ChildBounds =  rootBounds;
							rootNode.Nodes.Cache.Bounds = childBounds;
							
							Node nextNode = null;
							
							// Find the next visible root node after the selected one
							for(int i=tc.Nodes.IndexOf(rootNode) + 1; i<=tc.Nodes.Count-1; i++)
							{
								// The node has to be visible
								if (tc.Nodes[i].Visible)
								{
									nextNode = tc.Nodes[i];
									break;
								}
							}
							
							// Are there are any other nodes to shift down?
							if (nextNode != null)
							{
								bool found = false;
							
								// Process all nodes in turn looking for those to shift down
								foreach(Node n in displayNodes)
								{
									if (!found)
									{
										if (n == nextNode)
											found = true;
									}
									
									if (found)
									{
										// Shift display of node downwards
										Rectangle nBounds = n.Cache.Bounds;
										Rectangle nChildBounds = n.Cache.ChildBounds;
										nBounds.Offset(0, allocate);
										nChildBounds.Offset(0, allocate);
										n.Cache.Bounds = nBounds;
										n.Cache.ChildBounds = nChildBounds;
									}
								}
							}
						}
					}
				}
			}
		}
	}
}
