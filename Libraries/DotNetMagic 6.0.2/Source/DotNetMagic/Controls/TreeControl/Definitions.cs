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
using System.Windows.Forms;
using System.ComponentModel;
using Crownwood.DotNetMagic.Common;
using Microsoft.Win32;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Interface required to draw individual Node instance.
	/// </summary>
	public interface INodeVC
	{
		/// <summary>
		/// Called when the VC is attached to the control.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		void Initialize(TreeControl tc);

		/// <summary>
		/// Called when the VC is about to be detached from the control.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		void Detaching(TreeControl tc);

		/// <summary>
		/// Calculate and return the size needed for drawing a Node instance.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <returns>Size instance with drawing size required.</returns>
		Size MeasureSize(TreeControl tc, Node n, Graphics g);

		/// <summary>
		/// Defines the top left position of the node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="topLeft">Top left position of the node.</param>
		/// <returns>Bounding rectangle created for node.</returns>
		Rectangle SetPosition(TreeControl tc, Node n, Point topLeft);

		/// <summary>
		/// Defines the bounding rectangle of the node and all children as well.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="bounds">Bounding rectangle of collection.</param>
		void SetChildBounds(TreeControl tc, Node n, Rectangle bounds);

		/// <summary>
		/// Find if the provided rectangle intersects the bounds of the node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="rectangle">Rectangle to compare against.</param>
		/// <param name="recurse">Compare against children as well.</param>
		/// <returns>True if intersecting; otherwise false.</returns>
		bool IntersectsWith(TreeControl tc, Node n, Rectangle rectangle, bool recurse);

		/// <summary>
		/// Draw the Node instance.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <param name="clipRectangle">Clipping rectangle used when drawing.</param>
		/// <param name="leftOffset">Starting left offset for drawing.</param>
		/// <param name="rightOffset">Right offset for drawing.</param>
		void Draw(TreeControl tc, Node n, Graphics g, Rectangle clipRectangle, int leftOffset, int rightOffset);

		/// <summary>
		/// Process a mouse down on the Node instance.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="button">Button that is pressed down.</param>
		/// <param name="pt">Point being pressed down.</param>
		/// <returns>true if the point was processed; otherwise false.</returns>
		bool MouseDown(TreeControl tc, Node n, MouseButtons button, Point pt);
			
		/// <summary>
		/// Process a mouse up on the Node instance.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="button">Button that is going up.</param>
		/// <param name="pt">Point being pressed down.</param>
		/// <returns>true if the point was processed; otherwise false.</returns>
		bool MouseUp(TreeControl tc, Node n, MouseButtons button, Point pt);

		/// <summary>
		/// Process a double click on the Node instance.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="pt">Point being pressed down.</param>
		/// <returns>true if the point was processed; otherwise false.</returns>
		bool DoubleClick(TreeControl tc, Node n, Point pt);

		/// <summary>
		/// Notification that the expanded state of a node has changed.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		void NodeExpandedChanged(TreeControl tc, Node n);
		
		/// <summary>
		/// Notification that the visible state of a node has changed.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		void NodeVisibleChanged(TreeControl tc, Node n);

		/// <summary>
		/// Notification that the selectable state of a node has changed.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		void NodeSelectableChanged(TreeControl tc, Node n);

		/// <summary>
		/// Notification that the CheckState of a node has changed.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		void NodeCheckStateChanged(TreeControl tc, Node n);

		/// <summary>
		/// Notification that specified node is being removed.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		void NodeRemoving(TreeControl tc, Node n);

		/// <summary>
		/// Is the specified Node allowed to be selected?
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <returns>true if node is allowed to be selected; otherwise false.</returns>
		bool CanSelectNode(TreeControl tc, Node n);

		/// <summary>
		/// Is the specified Node allowed to be collapsed?
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="key">True if keyboard action.</param>
		/// <param name="mouse">True if mouse action.</param>
		/// <returns>true if node is allowed to be collapsed; otherwise false.</returns>
		bool CanCollapseNode(TreeControl tc, Node n, bool key, bool mouse);

		/// <summary>
		/// Is the specified Node allowed to be expanded?
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="key">True if keyboard action.</param>
		/// <param name="mouse">True if mouse action.</param>
		/// <returns>true if node is allowed to be collapsed; otherwise false.</returns>
		bool CanExpandNode(TreeControl tc, Node n, bool key, bool mouse);

		/// <summary>
		/// Is AutoEdit enabled for this Node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		bool CanAutoEdit(TreeControl tc, Node n);

		/// <summary>
		/// Gets a value indicating if tooltips are allowed for this node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <returns>True if allowed; otherwise false.</returns>
		bool CanToolTip(TreeControl tc, Node n);

		/// <summary>
		/// Initiate the editing of the Node label.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		void BeginEditNode(TreeControl tc, Node n);
		
		/// <summary>
		/// Notification that the size of the control has changed.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		void SizeChanged(TreeControl tc);

		/// <summary>
		/// Find the displayed node for the given point.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="pt">Point in Client coordinates.</param>
		/// <returns>node reference or null.</returns>
		Node DragOverNodeFromPoint(TreeControl tc, Point pt);

		/// <summary>
		/// Notification that dragging has entered the specified Node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="drgevent">Drag event information.</param>
		void DragEnter(TreeControl tc, Node n, DragEventArgs drgevent);
		
		/// <summary>
		/// Notification that user is dragging over the specified Node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="drgevent">Drag event information.</param>
		void DragOver(TreeControl tc, Node n, DragEventArgs drgevent);

		/// <summary>
		/// Notification that dragging has left the specified Node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		void DragLeave(TreeControl tc, Node n);

		/// <summary>
		/// Notification that user has dropped onto specified Node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <param name="drgevent">Drag event information.</param>
		void DragDrop(TreeControl tc, Node n, DragEventArgs drgevent);
		
		/// <summary>
		/// Notification that user is hovering over a drag Node.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		void DragHover(TreeControl tc, Node n);

		/// <summary>
		/// Final change to perform drawing.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <param name="displayNodes">Collection of nodes in display.</param>
		void PostDrawNodes(TreeControl tc, Graphics g, ArrayList displayNodes);

		/// <summary>
		/// Called once all the nodes have been calculated ready for display.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="displayNodes">Collection of nodes in display.</param>
		void PostCalculateNodes(TreeControl tc, ArrayList displayNodes);
		
		/// <summary>
		/// Recover the Font used to draw the text in normal mode.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <returns>Font used to draw text in normal mode.</returns>
		Font GetNodeFont(TreeControl tc, Node n);

		/// <summary>
		/// Get the rectangle that represents drawing area for text.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="n">Reference to Node instance.</param>
		/// <returns>Client spaced rectangle for the text area.</returns>
		Rectangle GetTextRectangle(TreeControl tc, Node n);
	}

	/// <summary>
	/// Interface required to draw individual NodeCollection instance.
	/// </summary>
	public interface INodeCollectionVC
	{
		/// <summary>
		/// Called when the VC is attached to the control.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		void Initialize(TreeControl tc);

		/// <summary>
		/// Called when the VC is about to be detached from the control.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		void Detaching(TreeControl tc);

		/// <summary>
		/// Calculate and return the extra space needed for drawing a NodeCollection instance.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="nc">Reference to NodeCollection instance.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <returns>Extra space needed for each edge of the collection area.</returns>
		Edges MeasureEdges(TreeControl tc, NodeCollection nc, Graphics g);

		/// <summary>
		/// Defines the bounding rectangle of the collection.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="nc">Reference to NodeCollection instance.</param>
		/// <param name="bounds">Bounding rectangle of collection.</param>
		void SetBounds(TreeControl tc, NodeCollection nc, Rectangle bounds);

		/// <summary>
		/// Find if the provided rectangle intersects the bounds of the node collection.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="nc">Reference to NodeCollection instance.</param>
		/// <param name="rectangle">Rectangle to compare against.</param>
		/// <returns>True if intersecting; otherwise false.</returns>
		bool IntersectsWith(TreeControl tc, NodeCollection nc, Rectangle rectangle);

		/// <summary>
		/// Draw the NodeCollection instance.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="nc">Reference to NodeCollection instance.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <param name="clipRectangle">Clipping rectangle used when drawing.</param>
		/// <param name="preDraw">True when drawing before children and fales for drawing after.</param>
		void Draw(TreeControl tc, NodeCollection nc, Graphics g, Rectangle clipRectangle, bool preDraw);

		/// <summary>
		/// Process a mouse down on the NodeCollection instance.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="nc">Reference to NodeCollection instance.</param>
		/// <param name="n">Reference to Node within collection.</param>
		/// <param name="button">Button that is pressed down.</param>
		/// <param name="pt">Point being pressed down.</param>
		/// <returns>true if the point was processed; otherwise false.</returns>
		bool MouseDown(TreeControl tc, NodeCollection nc, Node n, MouseButtons button, Point pt);

		/// <summary>
		/// Process a double click on the NodeCollection instance.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="nc">Reference to NodeCollection instance.</param>
		/// <param name="n">Reference to Node within collection.</param>
		/// <param name="pt">Point being pressed down.</param>
		/// <returns>true if the point was processed; otherwise false.</returns>
		bool DoubleClick(TreeControl tc, NodeCollection nc, Node n, Point pt);

		/// <summary>
		/// Notification that all child nodes are being removed.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="nc">Reference to NodeCollection instance.</param>
		void NodeCollectionClearing(TreeControl tc, NodeCollection nc);

		/// <summary>
		/// Notification that the size of the control has changed.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		void SizeChanged(TreeControl tc);

		/// <summary>
		/// Final chance to perform drawing.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="g">Reference to Graphics instance to draw into.</param>
		/// <param name="displayNodes">Collection of nodes in display.</param>
		void PostDrawNodes(TreeControl tc, Graphics g, ArrayList displayNodes);

		/// <summary>
		/// Called once all the nodes have been calculated ready for display.
		/// </summary>
		/// <param name="tc">Reference to owning TreeControl.</param>
		/// <param name="displayNodes">Collection of nodes in display.</param>
		void PostCalculateNodes(TreeControl tc, ArrayList displayNodes);
	}
}
