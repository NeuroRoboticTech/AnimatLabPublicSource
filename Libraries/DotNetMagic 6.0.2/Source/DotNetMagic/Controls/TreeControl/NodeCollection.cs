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
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Provides a collection container for Node instances.
	/// </summary>
	[DefaultProperty("ParentNode")]
	[DefaultEvent("Inserted")]
	public class NodeCollection : CollectionWithEvents, ICloneable
    {
		// Instance fields - Internal state
		private int _visibleCount;
		private NodeCache _cache;
		private INodeCollectionVC _vc;

		/// <summary>
		/// Occurs after the VC property value has changed.
		/// </summary>
		public event EventHandler VCChanged;

		/// <summary>
		/// Initialize a new instance of the NodeCollection class.
		/// </summary>
		public NodeCollection()
		{
			CommonConstruct();
		}

		/// <summary>
		/// Initialize a new instance of the NodeCollection class.
		/// </summary>
		public NodeCollection(TreeControl tl)
		{
			CommonConstruct();

			// But it does have a back reference to the containing control
			Cache.TreeControl = tl;
		}

		/// <summary>
		/// Initialize a new instance of the NodeCollection class.
		/// </summary>
		/// <param name="parentNode">Node this collection is attached to.</param>
		public NodeCollection(Node parentNode)
		{
			CommonConstruct();

			// Remember the node this collection is attached to.
			Cache.ParentNode = parentNode;
		}

		private void CommonConstruct()
		{
			// Create object that manages cached information
			_cache = new NodeCache();

			// Do not have a VC defined
			_vc = null;

			// Have no children and so none are visible
			_visibleCount = 0;
		}

		/// <summary>
		/// Gets the Node instance this collection is attached to.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Node ParentNode
		{
			get { return Cache.ParentNode; }
		}

		/// <summary>
		/// Gets the TreeControl instance this collection is attached to.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TreeControl TreeControl
		{
			get { return Cache.TreeControl; }
		}

		/// <summary>
		/// Gets the TreeControl instance this collection is attached to.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public INodeCollectionVC VC
		{
			get 
			{ 
				if (_vc != null)
					return _vc;

				if (Cache.TreeControl != null)
					return Cache.TreeControl.CollectionVC;
				
				return null;
			}

			set 
			{ 
				if (_vc != value)
				{
					_vc = value; 
					OnVCChanged();
				}
			}
		}

		/// <summary>
		/// Gets the size of the node collection.
		/// </summary>
		[Browsable(false)]
		public Size Size
		{
			get { return Cache.Size; }
		}

		/// <summary>
		/// Gets the bounds of the node collection.
		/// </summary>
		[Browsable(false)]
		public Rectangle Bounds 
		{
			get { return Cache.Bounds; }
		}

		/// <summary>
		/// Gets the bounds of the node collection including children nodes.
		/// </summary>
		[Browsable(false)]
		public Rectangle ChildBounds 
		{
			get { return Cache.ChildBounds; }
		}

		/// <summary>
		/// Gets the index of the first visible node, or -1 if none are visible.
		/// </summary>
		public int FirstVisibleIndex
		{
			get
			{
				// Scan from end of list back to front, for a visible node
				for(int i=0; i<Count; i++)
					if (this[i].Visible)
						return i;

				return -1;
			}
		}

		/// <summary>
		/// Gets the index of the last visible node, or -1 if none are visible.
		/// </summary>
		public int LastVisibleIndex
		{
			get
			{
				// Scan from end of list back to front, for a visible node
				for(int i=Count-1; i>=0; --i)
					if (this[i].Visible)
						return i;

				return -1;
			}
		}

		/// <summary>
		/// Gets the number of visible Nodes.
		/// </summary>
		public int VisibleCount
		{
			get { return _visibleCount; }
		}

		/// <summary>
		/// Adds the specified Node object to the collection.
		/// </summary>
		/// <param name="value">The Node object to add to the collection.</param>
		/// <returns>The Node object added to the collection.</returns>
        public Node Add(Node value)
        {
            // Use base class to process actual collection operation
            base.List.Add(value as object);

			return value;
        }

        /// <summary>
        /// Adds the specified Node object to the collection.
        /// </summary>
        /// <param name="value">The Node object to add to the collection.</param>
        /// <param name="bSort">If true it will sort the collection after adding the new node.</param>
        /// <returns>The Node object added to the collection.</returns>
        public Node Add(Node value, Boolean bSort)
        {
            // Use base class to process actual collection operation
            base.List.Add(value as object);

            if(bSort) Sort();
            return value;
        }
		/// <summary>
		/// Adds an array of Node objects to the collection.
		/// </summary>
		/// <param name="values">An array of Node objects to add to the collection.</param>
        public void AddRange(Node[] values)
        {
            // Use existing method to add each array entry
            foreach(Node page in values)
                Add(page);
        }

		/// <summary>
		/// Removes a Node from the collection.
		/// </summary>
		/// <param name="value">A Node to remove from the collection.</param>
        public void Remove(Node value)
        {
			// Use base class to process actual collection operation
            base.List.Remove(value as object);
		}
		
		/// <summary>
		/// Inserts a Node instance into the collection at the specified location.
		/// </summary>
		/// <param name="index">The location in the collection where you want to add the Node.</param>
		/// <param name="value">The Node object to insert.</param>
        public void Insert(int index, Node value)
        {
			// Use base class to process actual collection operation
            base.List.Insert(index, value as object);
		}

		/// <summary>
		/// Determines whether a Node is in the collection.
		/// </summary>
		/// <param name="value">The Node to locate in the collection.</param>
		/// <returns>true if item is found in the collection; otherwise, false.</returns>
        public bool Contains(Node value)
        {
            // Use base class to process actual collection operation
            return base.List.Contains(value as object);
        }

		/// <summary>
		/// Gets the Node at the specified index.
		/// </summary>
        public Node this[int index]
        {
            // Use base class to process actual collection operation
            get { return (base.List[index] as Node); }
        }

		/// <summary>
		/// Returns the index of the first occurrence of the given Node.
		/// </summary>
		/// <param name="value">The Node to locate.</param>
		/// <returns>Index of object; otherwise -1</returns>
        public int IndexOf(Node value)
        {
            // Find the 0 based index of the requested entry
            return base.List.IndexOf(value);
        }

		/// <summary>
		/// Gets reference to the first Node in this hierarchy.
		/// </summary>
		/// <returns>null if no Node found; otherwise reference to node.</returns>
		public Node GetFirstNode()
		{
			Node firstNode = null;

			// Are there any child nodes to process?
			if (Count > 0)
				firstNode = this[0];

			return firstNode;
		}

		/// <summary>
		/// Gets reference to the first displayed Node in this hierarchy.
		/// </summary>
		/// <returns>null if no Node found; otherwise reference to node.</returns>
		public Node GetFirstDisplayedNode()
		{
			Node firstNode = null;

			// Are there any child nodes to process?
			if (Count > 0)
			{
				// Search all our nodes...
				foreach(Node n in this)
				{
					// ...for one that is visible
					if (n.Visible)
					{
						firstNode = n;
						break;
					}
				}
			}

			return firstNode;
		}

		/// <summary>
		/// Gets reference to the last Node in this hierarchy.
		/// </summary>
		/// <returns>null if no Node found; otherwise reference to node.</returns>
		public Node GetLastNode()
		{
			Node lastNode = null;

			// Are there any child nodes to process?
			if (Count > 0)
			{
				int lastIndex = Count - 1;

				// Ask the last node in the collection for its own last node
				lastNode = this[lastIndex].Nodes.GetLastNode();

				// If last node has no children then just use the last node itself
				if (lastNode == null)
					lastNode = this[lastIndex];
			}

			return lastNode;
		}

		/// <summary>
		/// Gets reference to the last displayed Node in this hierarchy.
		/// </summary>
		/// <returns>null if no Node found; otherwise reference to node.</returns>
		public Node GetLastDisplayedNode()
		{
			Node lastNode = null;

			// Are there any child nodes to process?
			if (Count > 0)
			{
				// Seach all our nodes backwards...
				for(int i=Count-1; i>=0; --i)
				{
					// ...for one that is visible
					if (this[i].Visible)
					{
						lastNode = this[i];
						break;
					}
				}

				// Can only search into the last node			
				if ((lastNode != null) && lastNode.Expanded)
				{
					// Then get the last node from the one we found
					Node lastChild = lastNode.Nodes.GetLastDisplayedNode();
					
					// If this contains a last node then use it instead
					if (lastChild != null)
						lastNode = lastChild;
				}
			}

			return lastNode;
		}

		/// <summary>
		/// Creates a deep copy of the NodeCollection.
		/// </summary>
		/// <returns>New NodeCollection instance.</returns>
		public object Clone()
		{
			// Create new collection to return
			NodeCollection copy = new NodeCollection();
			
			// Copy each contained node in turn
			foreach(Node n in this)
				copy.Add((Node)n.Clone());
		
			return copy;
		}

		/// <summary>
		/// Called when the new item has been inserted.
		/// </summary>
		/// <param name="index">Index of newly inserted item.</param>
		/// <param name="value">Reference of inserted item.</param>
		protected override void OnInsertComplete(int index, object value)
		{
			base.OnInsertComplete (index, value);

			Node insertedNode = value as Node;

			// Update child node with its new references
			insertedNode.SetReferences(ParentNode, this, TreeControl);

			// Update visible count
			if (insertedNode.Visible)
				_visibleCount++;

			// Hook into change of visibility
			insertedNode.VisibleChanged += new EventHandler(OnNodeVisibleChanged);

			// Ask the tree control to redraw us
			if (Cache.TreeControl != null)
				Cache.TreeControl.InvalidateNodeDrawing();
		}

		/// <summary>
		/// Called when the item has been removed.
		/// </summary>
		/// <param name="index">Index of removed item.</param>
		/// <param name="value">Reference of removed item.</param>
		protected override void OnRemove(int index, object value)
		{		
			Node removedNode = value as Node;
			
			// Mark the node as being removed
			removedNode.Removing = true;

			INodeVC vc = removedNode.VC;

			// If we have a valid view controller and parent control
			if ((vc != null) && (Cache.TreeControl != null))
			{
				// Inform controller of change in contents
				vc.NodeRemoving(Cache.TreeControl, removedNode);
			}

			base.OnRemove(index, value);
		}
		
		/// <summary>
		/// Called when the item has been removed.
		/// </summary>
		/// <param name="index">Index of removed item.</param>
		/// <param name="value">Reference of removed item.</param>
		protected override void OnRemoveComplete(int index, object value)
		{
			Node removedNode = value as Node;

			// Unhook from events
			removedNode.VisibleChanged -= new EventHandler(OnNodeVisibleChanged);

			// Update visible count
			if (removedNode.Visible)
				--_visibleCount;

			// Unattach the node from references
			removedNode.SetReferences(null, null, null);

			// Ask the tree control to redraw us
			if (Cache.TreeControl != null)
				Cache.TreeControl.InvalidateNodeDrawing();

			base.OnRemoveComplete(index, value);
		}

		/// <summary>
		/// Called when an item has been replaced.
		/// </summary>
		/// <param name="index">Index of replaced item.</param>
		/// <param name="oldValue">Reference of removed item.</param>
		/// <param name="newValue">Reference of added item.</param>
		protected override void OnSetComplete(int index, object oldValue, object newValue)
		{
			Node oldNode = oldValue as Node;
			Node newNode = newValue as Node;

			// Unattach the old node from references
			oldNode.SetReferences(null, null, null);

			// Update new node with its new references
			newNode.SetReferences(ParentNode, this, TreeControl);

			// Update visible count
			if (oldNode.Visible) --_visibleCount;
			if (newNode.Visible) _visibleCount++;

			// Change around hooks
			oldNode.VisibleChanged -= new EventHandler(OnNodeVisibleChanged);
			newNode.VisibleChanged += new EventHandler(OnNodeVisibleChanged);

			// Ask the tree control to redraw us
			if (Cache.TreeControl != null)
				Cache.TreeControl.InvalidateNodeDrawing();

			base.OnSetComplete(index, oldValue, newValue);

		}
		
		/// <summary>
		/// Called when the contents are about to be cleared down.
		/// </summary>
		protected override void OnClear()
		{
			INodeCollectionVC vc = this.VC;

			// If we have a valid view controller and parent control
			if ((vc != null) && (Cache.TreeControl != null))
			{
				// Inform controller of change in contents
				vc.NodeCollectionClearing(Cache.TreeControl, this);

				// Ask the tree control to redraw us
				Cache.TreeControl.InvalidateNodeDrawing();
			}

			// Unattach the nodes from references
			foreach(Node node in this)
			{
				// Unhook from events
				node.VisibleChanged -= new EventHandler(OnNodeVisibleChanged);

				// Unattach the old node from references
				node.SetReferences(null, null, null);
			}

			// Update visible count
			_visibleCount = 0;

			// Must call base class so events are fired
			base.OnClear();
		}	

		/// <summary>
		/// Raises the VCChanged event.
		/// </summary>
		protected virtual void OnVCChanged()
		{
			if (VCChanged != null)
				VCChanged(this, EventArgs.Empty);

			// Ask the tree control to redraw us
			if (Cache.TreeControl != null)
				Cache.TreeControl.InvalidateNodeDrawing();
		}

		/// <summary>
		/// Gets a value indicating if the size of the node needs recalculating.
		/// </summary>
		protected internal NodeCache Cache
		{
			get { return _cache; }
		}

		internal void SetTreeControl(TreeControl tl)
		{
			// Cache back reference
			Cache.TreeControl = tl;

			// Recurse to tell each of the child nodes as well
			foreach(Node node in this)
				node.SetTreeControl(tl);
		}

		internal int ChildFromY(int y)
		{
			// If we have no children, then must be positioned at start
			if (Count == 0)
				return 0;

			// Find first and last valid testing indexes
			int top = 0;
			int bottom = Count - 1;

			do
			{
				// Find the halfway point for testing
				int test = (top + (bottom - top) / 2);

				// Get value at the testing position
				int testY = this[test].Cache.ChildBounds.Bottom;

				// Is the range just a single item?
				if (top == bottom)
					return (testY < y) ? bottom + 1 : bottom;

				// Bottom of the child before the testing position?
				if (testY < y)
					top = test + 1; // Yes, so we search the bottom half
				else
					bottom = test;	// No, so search the top half

			} while(true);
		}

		private void OnNodeVisibleChanged(object sender, EventArgs e)
		{
			// Cast to correct type
			Node n = sender as Node;

			// Update change in visibiliy
			if (n.Visible)
				_visibleCount++;
			else
				--_visibleCount;
		}

        public void Sort()
        {
            System.Collections.IComparer sorter = new TextSortHelper();

            InnerList.Sort(sorter);
        }

        public void SortTree()
        {
            Sort();

            foreach (Node node in this)
                node.Nodes.SortTree();
        }

        private class TextSortHelper : System.Collections.IComparer
        {
            public int Compare(object x, object y)
            {

                Node p1 = (Node)x;

                Node p2 = (Node)y;

                return p1.Text.CompareTo(p2.Text);

            }

        }
	}
}
