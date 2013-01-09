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
using System.ComponentModel;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Represents a single node in the data hierarchy.
	/// </summary>
	public class NodeCache
	{
		// Instance fields - Back references
		private Node _parentNode;
		private TreeControl _TreeControl;

		// Instance fields - Drawing values
		private Size _size;
		private Rectangle _bounds;
		private Rectangle _childBounds;

		/// <summary>
		/// Initialize a new instance of the NodeCache class.
		/// </summary>
		public NodeCache()
		{
			// Default internal state
			_parentNode = null;
			_TreeControl = null;
			_size = Size.Empty;
			_bounds = Rectangle.Empty;
		}

		/// <summary>
		/// Gets and sets the associated TreeControl instance.
		/// </summary>
		public TreeControl TreeControl
		{
			get { return _TreeControl; }
			set { _TreeControl = value; }
		}

		/// <summary>
		/// Gets and sets the associated TreeControl instance.
		/// </summary>
		public Node ParentNode
		{
			get { return _parentNode; }
			set { _parentNode = value; }
		}

		/// <summary>
		/// Gets a value indicating if the size of the node needs recalculating.
		/// </summary>
		public bool IsSizeDirty
		{
			get { return _size == Size.Empty; }
		}

		/// <summary>
		/// Mark the node to indicate that its size need recalculating.
		/// </summary>
		public void InvalidateSize()
		{
			_size = Size.Empty;
		}

		/// <summary>
		/// Gets and sets the size of the node..
		/// </summary>
		public Size Size
		{
			get { return _size; }
			set { _size = value; }
		}

		/// <summary>
		/// Gets and sets the bounding rectangle of node.
		/// </summary>
		public Rectangle Bounds
		{
			get { return _bounds; }
			set { _bounds = value; }
		}

		/// <summary>
		/// Gets and sets the bounding rectangle of node and all children.
		/// </summary>
		public Rectangle ChildBounds
		{
			get { return _childBounds; }
			set { _childBounds = value; }
		}
	}
}
