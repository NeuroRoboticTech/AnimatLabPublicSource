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
using System.IO;
using System.Xml;
using System.Drawing;
using System.Windows.Forms;
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Group that represents a sequence of child groups.
	/// </summary>
	public class TabGroupSequence : TabGroupBase, IResizeSource
	{
	    // Class fields
	    private const int SPACE_PRECISION = 3;
	
        // Instance fields
        private bool _removing;
        private Control _control;
        private LayoutDirection _direction;
        private TabGroupBaseCollection _children;
        
		/// <summary>
		/// Initializes a new instance of the TabGroupSequence class.
		/// </summary>
		/// <param name="tabbedGroups">Parent control instance.</param>
        public TabGroupSequence(TabbedGroups tabbedGroups)
            : base(tabbedGroups)
        {
            // Root instance always defaults to being horizontal
            InternalConstruct(tabbedGroups, LayoutDirection.Horizontal);
        }
    
		/// <summary>
		/// Initializes a new instance of the TabGroupSequence class.
		/// </summary>
		/// <param name="tabbedGroups">Parent control instance.</param>
		/// <param name="parent">Parent group instance.</param>
        public TabGroupSequence(TabbedGroups tabbedGroups, TabGroupBase parent)
            : base(tabbedGroups, parent)
        {
		    InternalConstruct(null, LayoutDirection.Horizontal);
        }

		/// <summary>
		/// Initializes a new instance of the TabGroupSequence class.
		/// </summary>
		/// <param name="tabbedGroups">Parent control instance.</param>
		/// <param name="parent">Parent group instance.</param>
		/// <param name="direction">Which direction the group organises children.</param>
        public TabGroupSequence(TabbedGroups tabbedGroups, TabGroupBase parent, LayoutDirection direction)
            : base(tabbedGroups, parent)
        {
            InternalConstruct(null, direction);
        }

        private void InternalConstruct(Control control, LayoutDirection direction)
        {
            // Do we need to create our own window?
            if (control == null) 
            {
                // Yes, use a simple panel for organizing children onto
                _control = new Panel();
            }
            else
            {
                // No, use the constructor provided one
                _control = control;
            }
            
            // Hook into control events
            _control.Resize += new EventHandler(OnControlResize);
            _control.Disposed += new EventHandler(OnControlDisposed);
            
            // Assign initial values
            _direction = direction;
            
            // Create collection to remember our child objects
            _children = new TabGroupBaseCollection();
            
            // In the process of removing?
            _removing = false;
        }

		/// <summary>
		/// Releases all resources used by the group.
		/// </summary>
		public override void Dispose()
		{
			// Did we manage to create a control?
			if (_control != null)
			{
				// Remove all children
				Clear();

				// Must unhook to remove references
				_control.Disposed -= new EventHandler(OnControlDisposed);
				_control.Resize -= new EventHandler(OnControlResize);
			}
		}
        
		/// <summary>
		/// Gets the number of child items.
		/// </summary>
        public override int Count               
        { 
            get { return _children.Count; } 
        }
            
		/// <summary>
		/// Gets a value indicating whether the group is a leaf.
		/// </summary>
        public override bool IsLeaf             
        { 
            get { return false; } 
        }
        
		/// <summary>
		/// Gets a value indicating whether the group is a sequence.
		/// </summary>
        public override bool IsSequence         
        { 
            get { return true; } 
        }
        
		/// <summary>
		/// Gets the parent control instance.
		/// </summary>
        public override Control GroupControl    
        { 
            get { return _control; } 
        }
        
		/// <summary>
		/// Gets a valid indicating which direction children are organised.
		/// </summary>
        public LayoutDirection Direction
        {
            get { return _direction; }

            internal protected set { _direction = value; }
        }

		/// <summary>
		/// Gets the visual style for the group.
		/// </summary>
        public VisualStyle Style        
		{ 
			get { return _tabbedGroups.Style; } 
		}        

		/// <summary>
		/// Gets the vector used for the resize bars.
		/// </summary>
        public int ResizeBarVector      
		{ 
			get { return _tabbedGroups.ResizeBarVector; } 
		}

		/// <summary>
		/// Gets the color used for the resize bars.
		/// </summary>
        public Color ResizeBarColor     
		{ 
			get { return _tabbedGroups.ResizeBarColor; } 
		}        

		/// <summary>
		/// Gets the color used for the bachground.
		/// </summary>
        public Color BackgroundColor    
		{ 
			get { return _tabbedGroups.ResizeBarColor; } 
		}

		/// <summary>
		/// Create and add a new leaf group to end of children.
		/// </summary>
		/// <returns>New leaf group instance.</returns>
        public TabGroupLeaf AddNewLeaf()
        {
            // Create a new leaf instance with correct back references
            TabGroupLeaf tgl = new TabGroupLeaf(_tabbedGroups, this);
            
            // Add into the collection
            Add(tgl, true);
            
            // Return its position in collection
            return tgl;
        }

        /// <summary>
        /// Add a leaf to the end of the children.
        /// </summary>
        /// <param name="tgl">Leaf derived instance to be added.</param>
        /// <returns>Added leaf instance on success; otherwise null</returns>
        public TabGroupLeaf AddNewLeaf(TabGroupLeaf tgl)
        {
            // Leaf must have been created to work with this sequence
            if ((tgl.TabbedGroups == _tabbedGroups) && (tgl.Parent == this))
            {
                // Add into the collection
                Add(tgl, true);

                // Return its position in collection
                return tgl;
            }
            else
                return null;
        }

		/// <summary>
		/// Create and add a new sequence to end of the children.
		/// </summary>
		/// <returns>New sequence group instance.</returns>
        public TabGroupSequence AddNewSequence()
        {
            // Add new sequence with opposite direction to ourself
            return AddNewSequence(_direction == LayoutDirection.Vertical ? LayoutDirection.Horizontal : LayoutDirection.Vertical);
        }

        /// <summary>
        /// Add a sequence derived class at end of the children.
        /// </summary>
        /// <param name="tgs">Sequence derived instance to be added.</param>
        /// <returns>Added sequence instance on success; otherwise null</returns>
        public TabGroupSequence AddNewSequence(TabGroupSequence tgs)
        {
            // Sequence must have been created to work with this sequence
            if ((tgs.TabbedGroups == _tabbedGroups) && (tgs.Parent == this))
            {
                // Define the opposite direction to the parent sequence
                tgs.Direction = (_direction == LayoutDirection.Vertical ? LayoutDirection.Horizontal : LayoutDirection.Vertical);

                // Add into the collection
                Add(tgs, true);

                // Return its position in collection
                return tgs;
            }
            else
                return null;
        }
                
		/// <summary>
		/// Create and add a new sequence with given direction to children.
		/// </summary>
		/// <param name="direction">Direction of flow for new sequence.</param>
		/// <returns>New sequence group instance.</returns>
        public TabGroupSequence AddNewSequence(LayoutDirection direction)
        {
            // Create a new sequence instance with correct back references
            TabGroupSequence tgs = new TabGroupSequence(_tabbedGroups, this, direction);
            
            // Add into the collection
            Add(tgs, true);
            
            // Return its position in collection
            return tgs;
        }

        /// <summary>
        /// Add a sequence to the end of the collection of children.
        /// </summary>
        /// <param name="direction">Direction of flow for new sequence.</param>
        /// <param name="tgs">Sequence derived instance to be added.</param>
        /// <returns>Added sequence instance.</returns>
        public TabGroupSequence AddNewSequence(LayoutDirection direction, TabGroupSequence tgs)
        {
            // Sequence must have been created to work with this sequence
            if ((tgs.TabbedGroups == _tabbedGroups) && (tgs.Parent == this))
            {
                // Define direction
                tgs.Direction = direction;

                // Add into the collection
                Add(tgs, true);

                // Return its position in collection
                return tgs;
            }
            else
                return null;
        }

		/// <summary>
		/// Create and insert a new leaf group at specified index.
		/// </summary>
		/// <param name="index">Index position to insert at.</param>
		/// <returns>New leaf group instance.</returns>
        public TabGroupLeaf InsertNewLeaf(int index)
        {
            // Range check index
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", index, "Insert index must be at least 0");
                
            if (index >= _children.Count)
                throw new ArgumentOutOfRangeException("index", index, "Cannot insert after end of current entries");

            // Create a new leaf instance with correct back references
            TabGroupLeaf tgl = new TabGroupLeaf(_tabbedGroups, this);
            
            // Insert into correct collection position
            Insert(index, tgl);
            
            // Return its position in collection
            return tgl;                           
        }

        /// <summary>
        /// Add an existing leaf at the specified index.
        /// </summary>
        /// <param name="index">Index position to insert at.</param>
        /// <param name="tgl">Leaf derived class to insert.</param>
        /// <returns>Leaf instance inserted on success; otherwise null</returns>
        public TabGroupLeaf InsertNewLeaf(int index, TabGroupLeaf tgl)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", index, "Insert index must be at least 0");

            if (index >= _children.Count)
                throw new ArgumentOutOfRangeException("index", index, "Cannot insert after end of current entries");

            // Leaf must have been created to work with this sequence
            if ((tgl.TabbedGroups == _tabbedGroups) && (tgl.Parent == this))
            {
                // Insert into correct collection position
                Insert(index, tgl);

                // Return its position in collection
                return tgl;
            }
            else
                return null;
        }
            
		/// <summary>
		/// Create and insert a new sequence group at specified index.
		/// </summary>
		/// <param name="index">Index position to insert at.</param>
		/// <returns>New sequence group instance.</returns>
        public TabGroupSequence InsertNewSequence(int index)
        {
            // Insert new sequence with opposite direction to ourself
            return InsertNewSequence(index, _direction == LayoutDirection.Vertical ? LayoutDirection.Horizontal : LayoutDirection.Vertical);
        }

        /// <summary>
        /// Add a sequence derived instance at a specified index.
        /// </summary>
        /// <param name="index">Index position to insert at.</param>
        /// <param name="tgs">Sequence derived class to insert.</param>
        /// <returns>Sequence inserted.</returns>
        public TabGroupSequence InsertNewSequence(int index, TabGroupSequence tgs)
        {
            return InsertNewSequence(index, _direction == LayoutDirection.Vertical ? LayoutDirection.Horizontal : LayoutDirection.Vertical, tgs);
        }
            
		/// <summary>
		/// Create and insert a new sequence with given direction at specified index.
		/// </summary>
		/// <param name="index">Index position to insert at.</param>
		/// <param name="direction">Direction of flow for new sequence.</param>
		/// <returns>New sequence group instance.</returns>
        public TabGroupSequence InsertNewSequence(int index, LayoutDirection direction)
        {
            // Range check index
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", index, "Insert index must be at least 0");
                
            if (index >= _children.Count)
                throw new ArgumentOutOfRangeException("index", index, "Cannot insert after end of current entries");

            // Create a new leaf instance with correct back references
            TabGroupSequence tgs = new TabGroupSequence(_tabbedGroups, this, direction);
            
            // Insert into correct collection position
            Insert(index, tgs);
            
            // Return its position in collection
            return tgs;                           
        }

        /// <summary>
        /// Insert a sequence derived class at the specified index with specified direction.
        /// </summary>
        /// <param name="index">Index position to insert at.</param>
        /// <param name="direction">Direction of flow for sequence.</param>
        /// <param name="tgs">Sequence derived instance to insert.</param>
        /// <returns>Sequence inserted.</returns>
        public TabGroupSequence InsertNewSequence(int index, LayoutDirection direction, TabGroupSequence tgs)
        {
            // Range check index
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", index, "Insert index must be at least 0");

            if (index >= _children.Count)
                throw new ArgumentOutOfRangeException("index", index, "Cannot insert after end of current entries");

            // Sequence must have been created to work with this sequence
            if ((tgs.TabbedGroups == _tabbedGroups) && (tgs.Parent == this))
            {
                // Define the new direction
                tgs.Direction = direction;

                // Insert into correct collection position
                Insert(index, tgs);

                // Return its position in collection
                return tgs;
            }
            else
                return null;
        }

		/// <summary>
		/// Find and remove the specified group from children.
		/// </summary>
		/// <param name="group">Group instance to be removed.</param>
        public void Remove(TabGroupBase group)
        {
            // Convert from reference to index to use existing RemoveAt implementation
            RemoveAt(_children.IndexOf(group), true, true);
        }

		/// <summary>
		/// Remove the indexed group from children.
		/// </summary>
		/// <param name="index">Index position to remove.</param>
		/// <param name="reallocate">Should removed space be allocated to remaining children.</param>
		/// <param name="dispose">Should dispose of removed child.</param>
        public void RemoveAt(int index, bool reallocate, bool dispose)
        {
            // Range check index
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", index, "RemoveAt index must be at least 0");
                
            if (index >= _children.Count)
                throw new ArgumentOutOfRangeException("index", index, "Cannot remove entry after end of list");

            // Is the removed item the active leaf?
            if (_children[index] == _tabbedGroups.ActiveLeaf)
            {
                // Then request movement of the active leaf
                _tabbedGroups.MoveActiveToNearestFromLeaf(_children[index]);
            }

			TabGroupBase childRemoved = _children[index];

            // Inform control that a group is removed, so it can track number of leafs 
            _tabbedGroups.GroupRemoved(childRemoved, true);

            // Is this the only Window entry?
            if (_children.Count == 1)
            {
                // Remove Window from appearance
				Control panel = _control.Controls[0];
                ControlHelper.RemoveAt(_control.Controls, 0);
  
                if (dispose)
					panel.Dispose();
			}
            else
            {
                int posBar = 0;
				int posPanel = 0;

                // Calculate position of Window to remove
                if (index == 0)
                {
					posBar = 1;
					posPanel = 0;
                }
                else
                {
					posBar = index * 2 - 1;
					posPanel = posBar + 1;
				}

                // Remove Window and bar 
                Control bar = _control.Controls[posBar];
                Control panel = _control.Controls[posPanel];

                // Use helper method to circumvent form Close bug
                ControlHelper.RemoveAt(_control.Controls, Math.Min(posBar, posPanel));
                ControlHelper.RemoveAt(_control.Controls, Math.Min(posBar, posPanel));
                
                // Always dispose of the bar
                bar.Dispose();
                
                // Only dispose of panel is allowed
				if (dispose)
					panel.Dispose();
			}

            // How much space is removed entry taking up?
            Decimal space = _children[index].Space;

            // Remove child from collection
            _children.RemoveAt(index);

			// Do we need to reallocate the space to other groups?
			if (reallocate)
			{
				// Redistribute space to other groups
				RemoveWindowSpace(space);

				DebugSpace(this);

				// Update child layout to reflect new proportional spacing values
				RepositionChildren();
			}

            // Last page removed?            
            if (_children.Count == 0)
            {
                // All pages removed, do we need to compact?
                if (_tabbedGroups.AutoCompact)
                    _tabbedGroups.Compact();
            }
            
            // Give control chance to enfore leaf policy
            _tabbedGroups.EnforceAtLeastOneLeaf();
            
            // Mark layout as dirty
            if (_tabbedGroups.AutoCalculateDirty)
                _tabbedGroups.Dirty = true;

			if (dispose)
			{
				// Finally, get rid of resources
				childRemoved.Dispose();
			}
        }
        
		/// <summary>
		/// Returns the index of the first occurrence of the group.
		/// </summary>
		/// <param name="group">The group to locate.</param>
		/// <returns>Index of group; otherwise -1</returns>
        public int IndexOf(TabGroupBase group)
        {
            return _children.IndexOf(group);
        }
        
		/// <summary>
		/// Remove all child groups.
		/// </summary>
        public void Clear()
        {
			if (!_removing)
			{
				_removing = true;
				
				// Do we contain the active leaf?
				if (_children.IndexOf(_tabbedGroups.ActiveLeaf) >= 0)
				{
					// Then request movement of the active leaf to different group
					_tabbedGroups.MoveActiveToNearestFromSequence(this);
				}

				TabGroupBaseCollection copyChildren = new TabGroupBaseCollection();

				// Inform control about all removed children for leaf counting
				foreach(TabGroupBase tgb in _children)
				{
					// Make temporary copy of the child
					copyChildren.Add(tgb);

					// Remove processing
					_tabbedGroups.GroupRemoved(tgb, false);
				}
			
				// Cache number of child controls
				int count = _control.Controls.Count;
				
				// Remove each child and dispose as needed
				for(int i=0; i<count; i++)
				{
					// Always get the first child
					Control child = _control.Controls[0];
					ControlHelper.RemoveAt(_control.Controls, 0);
					child.Dispose();
				}

				// Remove children from collection
				_children.Clear();

				// Must repaint to reflect changes
				_control.Invalidate();

				_removing = false;

				// All pages removed, do we need to compact?
				if (_tabbedGroups.AutoCompact)
					_tabbedGroups.Compact();
		        
				// Give control chance to enfore leaf policy
				_tabbedGroups.EnforceAtLeastOneLeaf();
		        
				DebugSpace(this);

				// Mark layout as dirty
				if (_tabbedGroups.AutoCalculateDirty)
					_tabbedGroups.Dirty = true;

				// Inform control about all removed children for leaf counting
				foreach(TabGroupBase tgb in copyChildren)
					tgb.Dispose();
			}
		}
        
		/// <summary>
		/// Gets the group at the specified index.
		/// </summary>
        public TabGroupBase this[int index]
        {
            get { return _children[index]; }
        }

		/// <summary>
		/// Informs group of a notification.
		/// </summary>
		/// <param name="code">Which notification has occured.</param>
        public override void Notify(NotifyCode code)
        {
            // Handle codes of interest
            switch(code)
            {
                case NotifyCode.ProminentChanged:
                case NotifyCode.MinimumSizeChanged:
                    // Must reposition to take account of change
                    RepositionChildren();
                    break;
                case NotifyCode.StyleChanged:
                    // Inform each resize bar of change in style
                    foreach(Control c in _control.Controls)
                        if (c is ResizeBar)
                            (c as ResizeBar).Style = _tabbedGroups.Style;
                
                    // Reposition the children based on new resize bar size
                    RepositionChildren();
                    break;
                case NotifyCode.ResizeBarVectorChanged:
                    // Recalculate layout of childreen
                    RepositionChildren();
                    break;
                case NotifyCode.ResizeBarColorChanged:
                    // If we are showing at least one resize bar
                    if (_children.Count > 1)
                    {
						// Pass new value onto child resize bars
						foreach(Control c in _control.Controls)
							if (c is ResizeBar)
								(c as ResizeBar).PropogateNameValue(PropogateName.ResizeBarColor, _tabbedGroups.ResizeBarColor);

                        // Then must repaint in new color
                        _control.Invalidate();
                    }
                    break;
            }
            
            // Pass notification to children
            foreach(TabGroupBase child in _children)
                child.Notify(code);
        }
    
		/// <summary>
		/// Restore equal proportional spacing to all groups.
		/// </summary>
		/// <param name="recurse">Should recurse into all groups.</param>
        public void Rebalance(bool recurse)
        {
            if (_children.Count > 0)
            {
                // Calculate how much space to give each child
                Decimal newSpace = Decimal.Round(100m / _children.Count, SPACE_PRECISION);

                // Assign equal space to all entries        
                foreach(TabGroupBase group in _children)
                    group.Space = newSpace;

                Decimal remainderSpace = 100m - (newSpace * _children.Count);
                 
                // Allocate rounding errors to last child
                if (remainderSpace != 0)
                    _children[_children.Count - 1].Space += remainderSpace;
                        
                // Propogate effect into child sequences?
                if (recurse)
                {
                    foreach(TabGroupBase group in _children)
                        if (group.IsSequence)
                            (group as TabGroupSequence).Rebalance(recurse);
                }
            }
            
			DebugSpace(this);

            // Update child layout to reflect new proportional spacing values
            RepositionChildren();
        }

		/// <summary>
		/// Determine if the prominent leaf is inside this group.
		/// </summary>
		/// <param name="recurse">Should recurse into all groups.</param>
		/// <returns>true if contains prominent leaf; otherwise false</returns>
        public override bool ContainsProminent(bool recurse)
        {
            // Cache the currently selected prominent group
            TabGroupLeaf prominent = _tabbedGroups.ProminentLeaf;
            
            // If not defined then we cannot contain it!
            if (prominent == null)
                return false;
                
            // Check our own leaf nodes first
            foreach(TabGroupBase group in _children)
                if (group.IsLeaf)
                    if (group == prominent)
                        return true;

            // Need to check sub-sequences?
            if (recurse)
            {
                // Check our child sequences 
                foreach(TabGroupBase group in _children)
                    if (group.IsSequence)
                        if (group.ContainsProminent(recurse))
                            return true;
            }
                                    
            // Not found
            return false;                            
        }

		/// <summary>
		/// Request this group save its information and child groups.
		/// </summary>
		/// <param name="xmlOut">Write to save information into.</param>
        public override void SaveToXml(XmlTextWriter xmlOut)
        {
            // Output standard values appropriate for all Sequence instances
            xmlOut.WriteStartElement("Sequence");
            xmlOut.WriteAttributeString("Count", _children.Count.ToString());
            xmlOut.WriteAttributeString("Unique", Unique.ToString());
            xmlOut.WriteAttributeString("Space", ConversionHelper.DecimalToString(_space));
            xmlOut.WriteAttributeString("Direction", _direction.ToString());

            // Output each sub element
            foreach(TabGroupBase tgb in _children)
                tgb.SaveToXml(xmlOut);
                
            xmlOut.WriteEndElement();
        }

		/// <summary>
		/// Request this group load its information and child groups.
		/// </summary>
		/// <param name="xmlIn">Reader to load information from.</param>
        public override void LoadFromXml(XmlTextReader xmlIn)
        {
            // Grab the expected attributes
            string rawCount = xmlIn.GetAttribute(0);
            string rawUnique = xmlIn.GetAttribute(1);
            string rawSpace = xmlIn.GetAttribute(2);
            string rawDirection = xmlIn.GetAttribute(3);

            // Convert to correct types
            int count = Convert.ToInt32(rawCount);
            int unique = Convert.ToInt32(rawUnique);
            Decimal space = ConversionHelper.StringToDecimal(rawSpace);
            LayoutDirection direction = (rawDirection == "Horizontal" ? LayoutDirection.Horizontal : 
																		LayoutDirection.Vertical);
            
            // Update myself with new values
            Unique = unique;
            _space = space;
            _direction = direction;
            
            // Load each of the children
            for(int i=0; i<count; i++)
            {
                // Read the next Element
                if (!xmlIn.Read())
                    throw new ArgumentException("An element was expected but could not be read in");

                TabGroupBase newElement = null;

                // Is it another sequence?
                if (xmlIn.Name == "Sequence")
                    newElement = new TabGroupSequence(_tabbedGroups, this);
                else if (xmlIn.Name == "Leaf")
                    newElement = new TabGroupLeaf(_tabbedGroups, this);
                else
                    throw new ArgumentException("Unknown element was encountered");
            
                bool expectEndElement = !xmlIn.IsEmptyElement;

                // Load its config
                newElement.LoadFromXml(xmlIn);
                   
                // Add new element to the collection
                Add(newElement, true);

                // Do we expect and end element to occur?
                if (expectEndElement)
                {
                    // Move past the end element
                    if (!xmlIn.Read())
                        throw new ArgumentException("Could not read in next expected node");

                    // Check it has the expected name
                    if (xmlIn.NodeType != XmlNodeType.EndElement)
                        throw new ArgumentException("EndElement expected but not found");
                }
            }
        }

		/// <summary>
		/// Perform compacting of hierarchy using default flags.
		/// </summary>
        public void Compact()
        {
            Compact(_tabbedGroups.CompactOptions);
        }

		/// <summary>
		/// Perform compacting of hierarchy using specified flags.
		/// </summary>
		/// <param name="flags">Compacting options to be applied.</param>
        public void Compact(CompactFlags flags)
        {
            // Compact each child sequence
            foreach(TabGroupBase tgb in _children)
                if (tgb.IsSequence)
                    (tgb as TabGroupSequence).Compact(flags);
        
            // Remove dangling entries
            CompactRemoveEmptyTabLeafs(flags);
            CompactRemoveEmptyTabSequences(flags);
            
            // Integrate single entries
            CompactReduceSingleEntries(flags);
            
            // Integrate sub-sequences which run in same direction
            CompactReduceSameDirection(flags);

			DebugSpace(this);
        }
        
		/// <summary>
		/// Recalculate the correct size and position of child groups.
		/// </summary>
        public void Reposition()
        {
            // Update child layout to reflect new proportional spacing values
            RepositionChildren();
        }

		/// <summary>
		/// Determines if the ResizeBar instance is allowed to resize.
		/// </summary>
		/// <param name="bar">ResizeBar instance.</param>
		/// <returns>Is the instance allowed to resize.</returns>
		public bool CanResize(ResizeBar bar)
		{
			// Cannot resize when in prominent mode
			if (!_tabbedGroups.ResizeBarLock && (_tabbedGroups.ProminentLeaf == null))
			{
				// Find position of this ResizeBar in the Controls collection
				int barIndex = _control.Controls.IndexOf(bar);
                
				// Convert from control to children indexing
				int beforeIndex = (barIndex - 1) / 2;

				TabGroupBase before = _children[beforeIndex];
				TabGroupBase after = _children[beforeIndex + 1];

				// Only allow resize if groups of both sides of the bar are allowed to be resized
				if (!before.ResizeBarLock && !after.ResizeBarLock)
				{
					// As long as there is some space on one of the sides then a resize can redistribute it
					if ((before.Space > 0m) || (after.Space > 0m))
						return true;
				}

				return false;
			}
			else
			{
				// Must exit prominent mode before resize can occur
				return false;
			}
		}

		/// <summary>
		/// Determine the screen rectangle the bar can size into.
		/// </summary>
		/// <param name="bar">ResizeBar instance.</param>
		/// <param name="screenBoundary">Screen bounding rectangle.</param>
		/// <returns>Is the instance allowed to resize.</returns>
		public bool StartResizeOperation(ResizeBar bar, ref Rectangle screenBoundary)
		{
			bool ret = false;

			// Do we allow the resize operation?
			if (!_tabbedGroups.OnResizeStart(this))
			{
				// Find position of this ResizeBar in the Controls collection
				int barIndex = _control.Controls.IndexOf(bar);
	            
				// Convert from control to children indexing
				int beforeIndex = (barIndex - 1) / 2;

				// Get groups before and after the resize bar
				TabGroupBase before = _children[beforeIndex];
				TabGroupBase after = _children[beforeIndex + 1];

				// Resizing boundary is defaulted to whole control area
				screenBoundary = _control.RectangleToScreen(_control.ClientRectangle);

				// Find screen rectangle for the groups either side of the bar
				Rectangle rectBefore = before.GroupControl.RectangleToScreen(before.GroupControl.ClientRectangle);
				Rectangle rectAfter = after.GroupControl.RectangleToScreen(after.GroupControl.ClientRectangle);

				// Reduce the boundary in the appropriate direction
				if (_direction == LayoutDirection.Vertical)
				{
					screenBoundary.Y = rectBefore.Y + before.MinimumSize.Height;
					screenBoundary.Height -= screenBoundary.Bottom - rectAfter.Bottom;
					screenBoundary.Height -= after.MinimumSize.Height;
				}
				else
				{
					screenBoundary.X = rectBefore.X + before.MinimumSize.Width;
					screenBoundary.Width -= screenBoundary.Right - rectAfter.Right;
					screenBoundary.Width -= after.MinimumSize.Width;
				}

				// Allow resize operation to occur
				ret = true;
			}

			return ret;
		}
        
		/// <summary>
		/// Resize operation completed with delta change.
		/// </summary>
		/// <param name="bar">ResizeBar instance.</param>
		/// <param name="delta">Delta change to size.</param>
		public void EndResizeOperation(ResizeBar bar, int delta)
		{
			// Find position of this ResizeBar in the Controls collection
			int barIndex = _control.Controls.IndexOf(bar);
            
			// Convert from control to children indexing
			int beforeIndex = (barIndex - 1) / 2;

			// The Window relating to this bar must be the one before it in the collection
			TabGroupBase before = _children[beforeIndex];

			// Is the Window being expanded
			DeltaGroupSpace(before, delta);

			DebugSpace(this);

			// Mark layout as dirty
			if (_tabbedGroups.AutoCalculateDirty)
				_tabbedGroups.Dirty = true;

			// Raise event to notify change in size
			_tabbedGroups.OnResizeFinish(this);
		}

        internal TabGroupBase Add(TabGroupBase group, bool calcSpace)
        {
            // Remember reference
            _children.Add(group);

            // First group added to sequence?
            if (_children.Count == 1) 
            {
                // Add new child control
                _control.Controls.Add(group.GroupControl);
            }
            else
            {
                // Create a resizing bar
                ResizeBar bar = new ResizeBar(_direction, this);

                // Append resize bar between existing entries and new entry
                _control.Controls.Add(bar);
                
                // Append new group control
                _control.Controls.Add(group.GroupControl);
            }
            
            if (!_tabbedGroups.Initializing)
            {
                // Do we need to allocate it space?
                if (calcSpace)
                {
                    // Allocate space for the new child
                    AllocateSpace(group);

					// Double check that space is correctly calculated
					DebugSpace(this);
				}
                                
				// Update child layout to reflect new proportional spacing values
                RepositionChildren();
            }
            
            // Mark layout as dirty
            if (_tabbedGroups.AutoCalculateDirty)
                _tabbedGroups.Dirty = true;

            return group;
        }
    
        internal TabGroupBase Insert(int index, TabGroupBase group)
        {
            // Range check index
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", index, "Insert index must be at least 0");
                
            if (index >= _children.Count)
                throw new ArgumentOutOfRangeException("index", index, "Cannot insert after end of current entries");
        
            // Remember reference
            _children.Insert(index, group);

            // Create a resizing bar
            ResizeBar bar = new ResizeBar(_direction, this);

            // Append resize bar between existing entries and new entry
            _control.Controls.Add(bar);
                
            // Append new group control
            _control.Controls.Add(group.GroupControl);

            // Inserting at start of collection?
            if (index == 0)
            {
                // Reposition the bar and group to start of collection
                _control.Controls.SetChildIndex(bar, 0);
                _control.Controls.SetChildIndex(group.GroupControl, 0);
            }
            else
            {
                // Find correct index taking into account number of resize bars
                int	pos = index * 2 - 1;

                // Reposition the bar and Window to correct relative ordering
                _control.Controls.SetChildIndex(bar, pos++);
                _control.Controls.SetChildIndex(group.GroupControl, pos);
            }
            
            // Allocate space for the new child
            AllocateSpace(group);
            
			DebugSpace(this);

            // Update child layout to reflect new proportional spacing values
            RepositionChildren();

            // Mark layout as dirty
            if (_tabbedGroups.AutoCalculateDirty)
                _tabbedGroups.Dirty = true;

            return group;
        }

        internal void Replace(TabGroupBase orig, TabGroupBase replace)
        {
            // Find array position of old item
            int origPos = _children.IndexOf(orig);
            
            // Transfer across the space occupied
            replace.RealSpace = orig.RealSpace;
            
            // Is this the only Window entry?
            if (_children.Count == 1)
            {
                // Remove Window from appearance
                Control panel = _control.Controls[0];
                ControlHelper.RemoveAt(_control.Controls, 0);
                panel.Dispose();
			}
            else
            {
                int pos = 0;

                // Calculate position of Window to remove				
                if (origPos != 0)
                    pos = origPos * 2 - 1;

                // Remove Window and bar 
				Control bar = _control.Controls[pos];
				Control panel = _control.Controls[pos+1];

				// Use helper method to circumvent form Close bug
				ControlHelper.RemoveAt(_control.Controls, pos);
                ControlHelper.RemoveAt(_control.Controls, pos);

				// Must remember to dispose of controls
				bar.Dispose();
				panel.Dispose();
			}
            
            // Inserting at start of collection?
            if (origPos == 0)
            {
                if (_children.Count > 1)
                {
                    // Create a resizing bar
                    ResizeBar bar = new ResizeBar(_direction, this);

                    // Append resize bar between existing entries and new entry
                    _control.Controls.Add(bar);

                    // Reposition the bar and group to start of collection
                    _control.Controls.SetChildIndex(bar, 0);
                }            

                // Append new group control
                _control.Controls.Add(replace.GroupControl);

                // Reposition the bar and group to start of collection
                _control.Controls.SetChildIndex(replace.GroupControl, 0);
            }
            else
            {
                // Create a resizing bar
                ResizeBar bar = new ResizeBar(_direction, this);

                // Append resize bar between existing entries and new entry
                _control.Controls.Add(bar);

                // Append new group control
                _control.Controls.Add(replace.GroupControl);
                    
                // Find correct index taking into account number of resize bars
                int	pos = origPos * 2 - 1;

                // Reposition the bar and Window to correct relative ordering
                _control.Controls.SetChildIndex(bar, pos++);
                _control.Controls.SetChildIndex(replace.GroupControl, pos);
            }
            
            // Update parentage
            replace.SetParent(this);
            
            // Replace the entry
            _children[origPos] = replace;

			DebugSpace(this);
            
            // Update child layout to reflect new proportional spacing values
            RepositionChildren();

            // Mark layout as dirty
            if (_tabbedGroups.AutoCalculateDirty)
                _tabbedGroups.Dirty = true;
        }

		internal Control.ControlCollection ChildControls
		{
			get { return _control.Controls; }
		}

		internal void SetPixelLengthOfChild(TabGroupBase childLeaf, int pixelSize)
		{
			DebugSpace(this);

			// If we only have a single child then its impossible to ensure the requested
			// pixel size is enforced as there is no second window to shrink/expand as needed
			if (_control.Controls.Count == 1)
				return;

			// Get the minimum size allowed for the child
			Size minSize = childLeaf.MinimumSize;

			if (_direction == LayoutDirection.Horizontal)
			{
				// Enforce minimum size requirements
				if (pixelSize < minSize.Width)
					pixelSize = minSize.Width;

				// Subtract minimum size to find amount that needs to be allocated as 'Space'
				pixelSize -= minSize.Width;
			}
			else
			{
				// Enforce minimum size requirements
				if (pixelSize < minSize.Height)
					pixelSize = minSize.Height;

				// Subtract the minimum size to find amount that needs to be allocated as 'Space'
				pixelSize -= minSize.Height;
			}

			// Space available for allocation
			int availableLength = 0;
			
			// Values depend on sequence direction
			if (_direction == LayoutDirection.Vertical)
				availableLength = _control.ClientRectangle.Height;
			else
				availableLength = _control.ClientRectangle.Width;

			// Ensure this is not negative
			if (availableLength < 0)
				availableLength = 0;

			// Total length of mandatory sizing information
			int minimumLength = 0;

			// Find the total length needed to allocation minimum sizes and resize bars
			for(int index=0, child=0; index<_control.Controls.Count; index++)
			{
				ResizeBar bar = _control.Controls[index] as ResizeBar;

				// Is this a resize bar control?
				if (bar != null)
				{
					// Add space allocated to ResizeBars
					minimumLength += bar.Length;
				}
				else
				{
					Size minimal = _children[child++].MinimumSize;

					// Length needed is depends on direction 
					if (_direction == LayoutDirection.Vertical)
						minimumLength += minimal.Height;
					else
						minimumLength += minimal.Width;
				}
			}		

			// Reduce available space by the minimum required
			availableLength -= minimumLength;

			// Limit check that available space is never less than nothing!
			if (availableLength < 0)
				availableLength = 0;
	
			// Limit check, can only allocate a maximum of all the free space
			if (pixelSize > availableLength)
				pixelSize = availableLength;

			// What is the new percentage it needs?
			Decimal newPercent = 0m;

			// Is there any room to allow a percentage calculation
			if ((pixelSize > 0) && (availableLength > 0))
				newPercent = Decimal.Round((Decimal)pixelSize / (Decimal)availableLength * 100m, SPACE_PRECISION);

			// If there is a change in the percentage required
			if (newPercent != childLeaf.Space)
			{
				// Find the change in percentage
				Decimal change = newPercent - childLeaf.Space;

				// Assign new percentage to the child
				childLeaf.Space = newPercent;

				// If we need to remove space from other existing groups
				if (change > 0)
					RemoveSpaceExcept(childLeaf, change);
				else
					AddSpaceExcept(childLeaf, -change);

				DebugSpace(this);

				// Update child layout to reflect new proportional spacing values
				RepositionChildren();
			}
		}

		private void AddSpaceExcept(TabGroupBase childLeaf, Decimal space)
		{
			// Is there any space to reallocate?
			if (space > 0)
			{
				// Allocate space in a maximum number of rounds, if this is reached
				// then there must be a fractional value left that cannot be split down
				for(int i=0; i<49; i++)
				{
					// How much space should we add to each of the others
					Decimal freedSpace = space / (_children.Count - 1);

					foreach(TabGroupBase group in _children)
					{
						// Do not process the excepted item
						if (group != childLeaf)
						{
							// We only retain a sensible level of precision
							Decimal newSpace = Decimal.Round(group.Space + freedSpace, SPACE_PRECISION);

							// Reduce amount left to be allocated 
							space -= (newSpace - group.Space);

							// Assign back new space
							group.Space = newSpace;
						}
					}

					if (space == 0m)
						break;
				}

				// Still a small fraction to allocate?
				if (space != 0m)
				{
					// Give the extra fraction to another child
					foreach(TabGroupBase group in _children)
					{
						// Do not process the excepted item
						if (group != childLeaf)
						{
							group.Space += space;
							break;
						}
					}
				}
			}
		}
                
		private void RemoveSpaceExcept(TabGroupBase childLeaf, Decimal space)
		{
			// Is there any space to reallocate?
			if (space > 0)
			{
				// Allocate space in a maximum number of rounds, if this is reached
				// then there must be a fractional value left that cannot be split down
				for(int i=0; i<49; i++)
				{
					// How much space should we remove from each of the others
					Decimal freedSpace = space / (_children.Count - 1);

					foreach(TabGroupBase group in _children)
					{
						// Do not process the excepted item
						if (group != childLeaf)
						{
							Decimal free = freedSpace;

							// Can only take away all of its space
							if (group.Space < free)
								free = group.Space;

							// We only retain a sensible level of precision
							Decimal newSpace = Decimal.Round(group.Space - free, SPACE_PRECISION);

							// Reduce amount left to be allocated 
							space -= (group.Space - newSpace);

							// Assign back new space
							group.Space = newSpace;
						}
					}

					// Finished allocating?
					if (space == 0m)
						break;
				} 

				// Still a small fraction to allocate?
				if (space != 0m)
				{
					// Take the extra fraction from another child
					foreach(TabGroupBase group in _children)
					{
						// Do not process the excepted item
						if (group != childLeaf)
						{
							// Does it have enough space to reduce it?
							if (group.Space >= space)
							{
								group.Space -= space;
								break;
							}
						}
					}
				}
			}
		}

		private void RepositionChildren()
		{
			// Cannot reposition whilst processing children
			if (!_removing)
			{
				// Area available for repositioning
				Rectangle clientRect = _control.ClientRectangle;

				// Is there anything to reposition?
				if (_children.Count > 0)
				{
					// Space available for allocation
					int space;
				
					// Starting position of first control 
					int delta;

					// Values depend on sequence direction
					if (_direction == LayoutDirection.Vertical)
					{
						space = clientRect.Height;
						delta = clientRect.Top;
					}
					else
					{
						space = clientRect.Width;
						delta = clientRect.Left;
					}

					// Ensure this is not negative
					if (space < 0)
						space = 0;

					int barSpace = 0;
					int allocation = space;

					// Create temporary array of working values
					int[] positions = new int[_control.Controls.Count];

					// If showing a prominent leaf and its the only item that should be visible
					if ((_tabbedGroups.ProminentLeaf != null) && _tabbedGroups.ProminentOnly)
					{
						// Pass 1, allocate all space to the child that contains the prominent leaf
						AllocateProminentSpace(ref positions, space);

						// Pass 2, position the allocated control
						RepositionProminent(ref positions, clientRect, delta);
					}
					else
					{
						// Pass 1, allocate all the space needed for each ResizeBar and the 
						//         minimal amount of space that each group requests. 
						AllocateMandatorySizes(ref positions, ref barSpace, ref space);

						// Is there any more space left?
						if (space > 0)
						{
							// Pass 2, allocate any space left over according to the requested
							//         percent space that each group would like to achieve.
							AllocateRemainingSpace(ref positions, space);
						}

						// Pass 3, reposition the controls according to allocated values.
						RepositionChildren(ref positions, clientRect, delta);
					}
				}
			}
		}

		private void CompactRemoveEmptyTabLeafs(CompactFlags flags)
		{
			// Should we check for empty leaf nodes?
			if ((flags & CompactFlags.RemoveEmptyTabLeaf) != 0)
			{
				int count = _children.Count;
                
				for(int index=0; index<count; index++)
				{
					// Only interested in leaf entries
					if (_children[index].IsLeaf)
					{
						TabGroupLeaf tgl = (TabGroupLeaf)_children[index];
                        
						// Is this an empty leaf node?
						if (tgl.Count == 0)
						{
							// Update active leaf setting
							if (_tabbedGroups.ActiveLeaf == tgl)
							{
								TabGroupLeaf newLeaf = _tabbedGroups.NextLeaf(tgl);
                                
								if (newLeaf == null)
									newLeaf = _tabbedGroups.PreviousLeaf(tgl);
                                    
								_tabbedGroups.ActiveLeaf = newLeaf;
							}

							// Need to remove the redundant entry
							RemoveAt(index, true, true);
                            
							// Reduce number of entries left to check
							count--;
                            
							// Move backwards so the next increment stays on same index
							index--;
                        
							// Mark layout as dirty
							if (_tabbedGroups.AutoCalculateDirty)
								_tabbedGroups.Dirty = true;
						}
					}
				}
			}
		}

		private void CompactRemoveEmptyTabSequences(CompactFlags flags)
		{
			// Should we check for empty sequence nodes?
			if ((flags & CompactFlags.RemoveEmptyTabSequence) != 0)
			{
				int count = _children.Count;
                
				for(int index=0; index<count; index++)
				{
					// Only interested in sequence entries
					if (_children[index].IsSequence)
					{
						TabGroupSequence tgs = (TabGroupSequence)_children[index];
                        
						// Is this an empty sequence node?
						if (tgs.Count == 0)
						{
							// Need to remove the redundant entry
							RemoveAt(index, true, true);
                            
							// Reduce number of entries left to check
							count--;
                            
							// Move backwards so the next increment stays on same index
							index--;

							// Mark layout as dirty
							if (_tabbedGroups.AutoCalculateDirty)
								_tabbedGroups.Dirty = true;
						}
					}
				}
			}
		}

		private void CompactReduceSingleEntries(CompactFlags flags)
		{
			bool changed = false;
        
			// Should we check for single instance nodes?
			if ((flags & CompactFlags.ReduceSingleEntries) != 0)
			{
				int count = _children.Count;
                
				for(int index=0; index<count; index++)
				{
					// Only interested in sequence entries
					if (_children[index].IsSequence)
					{
						TabGroupSequence tgs = (TabGroupSequence)_children[index];
                        
						// Does this entry only have a single child
						if (tgs.Count == 1)
						{
							// Remember how much space the base entry occupies
							Decimal temp = tgs.RealSpace;
                            
							// Get reference to only child
							TabGroupBase child = tgs[0];
                            
							// Update parentage
							child.SetParent(this);
                            
							// Find the child control to be replaced
							int childPos = _control.Controls.IndexOf(tgs.GroupControl);
                            
							// Remove it
							Control removed = _control.Controls[childPos];
							ControlHelper.RemoveAt(_control.Controls, childPos);

							// Add new child control in its place
							_control.Controls.Add(child.GroupControl);
							_control.Controls.SetChildIndex(child.GroupControl, childPos);
                            
							// Replace the middle object with the child
							_children.RemoveAt(index);
							_children.Insert(index, child);
                            
							// Restore its correct spacing
							child.RealSpace = temp;

							// Now safe to dispose of the no longer needed control
							removed.Dispose();
							
							// Need controls repositioned
							changed = true;                

							// Mark layout as dirty
							if (_tabbedGroups.AutoCalculateDirty)
								_tabbedGroups.Dirty = true;
						}
					}
				}
			}

			// Change in contents requires entries to be repositioned
			if (changed)
				RepositionChildren();
		}
        
		private void CompactReduceSameDirection(CompactFlags flags)
		{
			bool changed = false;
        
			// Should we check for same direction sub-sequences?
			if ((flags & CompactFlags.ReduceSameDirection) != 0)
			{
				int count = _children.Count;
                
				for(int index=0; index<count; index++)
				{
					// Only interested in sequence entries
					if (_children[index].IsSequence)
					{
						TabGroupSequence tgs = (TabGroupSequence)_children[index];
                        
						// Does it run in same direction as ourself?
						if (_direction == tgs.Direction)
						{
							// Remember how much space the base entry occupies
							Decimal temp = tgs.RealSpace;

							// Find the child control to be replaced
							int childPos = _control.Controls.IndexOf(tgs.GroupControl);

							// Remove the actual control
							Control removed = _control.Controls[childPos];
							ControlHelper.RemoveAt(_control.Controls, childPos);

							// Remove the intermediate group
							_children.RemoveAt(index);
                            
							// Reflect change in size
							count--;

							Decimal totalAllocated = 0m;

							// Add in each sub group in turn
							int subCount = tgs.Count;
                            
							bool firstInsert = true;
                
							for(int subIndex=0; subIndex<subCount; subIndex++)
							{
								TabGroupBase tgb = tgs[subIndex];
                            
								// What percentage of original space did it have?
								Decimal orig = tgb.RealSpace;
                                
								// Give it the same proportion of new space
								Decimal update = Decimal.Round(temp / 100 * orig, SPACE_PRECISION);
                            
								// Keep total actually allocated
								totalAllocated += update;
                            
								// Use new proportion
								tgb.RealSpace = update;
                                
								// Update parentage
								tgb.SetParent(this);

								// Does new child control need a resizing bar?                            
								if ((childPos > 0) && !firstInsert)
								{
									// Create a resizing bar
									ResizeBar bar = new ResizeBar(_direction, this);

									_control.Controls.Add(bar);
									_control.Controls.SetChildIndex(bar, childPos++);
								}
                            
								// Add new child control in its place
								_control.Controls.Add(tgb.GroupControl);
								_control.Controls.SetChildIndex(tgb.GroupControl, childPos++);

								// Insert at current position
								_children.Insert(index, tgb);
                                
								// Adjust variables to reflect increased size
								index++;
								count++;
								firstInsert = false;
							}
                            
                            // Now safe to dispose of the removed control
                            removed.Dispose();
                            
							// Assign any remainder to last group     
							_children[index-1].RealSpace +=  temp - totalAllocated;      
                            
							// Need controls repositioned
							changed = true;                

							// Mark layout as dirty
							if (_tabbedGroups.AutoCalculateDirty)
								_tabbedGroups.Dirty = true;
						}
					}
				}
			}
            
			// Change in contents requires entries to be repositioned
			if (changed)
				RepositionChildren();
		}

		private void AllocateSpace(TabGroupBase newGroup)
        {
            // Is this the only group?
            if (_children.Count == 1)
            {
                // Give it all the space
                newGroup.Space = 100m;
            }
            else
            {
                // Calculate how much space it should have
                Decimal newSpace = 100m / _children.Count;

                // How much space should we steal from each of the others
                Decimal reduceSpace = newSpace / (_children.Count - 1);

                // Actual space acquired so far 
                Decimal allocatedSpace = 0m;

                foreach(TabGroupBase group in _children)
                {
                    // Only process existing entries, not the new one
                    if (group != newGroup)
                    {
                        // How much space does the group currently have
                        Decimal currentSpace = group.Space;

                        // How much space to steal from it
                        Decimal xferSpace = reduceSpace;

                        // If group has less space then we want, just steal all it has
                        if (currentSpace < xferSpace)
                            xferSpace = currentSpace;

                        // Transfer the space across
                        currentSpace -= xferSpace;
                        
                        // Round the sensible number of decimal places
                        currentSpace = Decimal.Round(currentSpace, SPACE_PRECISION);
                        
                        // Update window with new space allocation
                        group.Space = currentSpace;

                        // Total up total space of all entries except new one
                        allocatedSpace += currentSpace;
                    }
                }

                // Assign all remaining space to new entry
                newGroup.Space = 100m - allocatedSpace;
            }
        }            
        
        private void RemoveWindowSpace(Decimal space)
        {
            // Are there any children to process?
            if (_children.Count != 0)
            {
                // Is there only a single group left?
                if (_children.Count == 1)
                {
                    // Give it all the space
                    _children[0].Space = 100m;
                }
                else
                {
                    // Is there any space to reallocate?
                    if (space > 0)
                    {
                        // Total up all the values
                        Decimal totalAllocated = 0m;

                        // How much space should we add to each of the others
                        Decimal freedSpace = space / _children.Count;

                        foreach(TabGroupBase group in _children)
                        {
                            // We only retain a sensible level of precision
                            Decimal newSpace = Decimal.Round(group.Space + freedSpace, SPACE_PRECISION);

                            // Assign back new space
                            group.Space = newSpace;

                            // Total up all space so far 
                            totalAllocated += newSpace;
                        }

                        // Look for minor errors because not all fractions can be accurately represented in binary!
                        if (totalAllocated > 100m)
                        {
                            // Remove from first entry
                            _children[0].Space -= totalAllocated - 100m;
                        }
                        else if (totalAllocated < 100m)
                        {
                            // Add to first entry
                            _children[0].Space += 100m - totalAllocated;
                        }
                    }
                }
            }
        }

		private void AllocateProminentSpace(ref int[] positions, int space)
		{
			for(int index=0, child=0; index<_control.Controls.Count; index++)
			{
				Control c = _control.Controls[index];
                
				bool isResizeBar = (c is ResizeBar);

				// Not interested in considering the resize bars
				if (!isResizeBar)
				{
					// Does this child contain the prominent leaf?
					if (_children[child++].ContainsProminent(true))
					{
						// Give it all the space
						positions[index] = space;

						// No need for any more searching
						break;
					}
				}
			}			
		}

		private void RepositionProminent(ref int[] positions, Rectangle clientRect, int delta)
		{
			// Process each control 
			for(int index=0; index<_control.Controls.Count; index++)
			{
				// Delta length for this particular control
				int newDelta = positions[index];

				ResizeBar bar = _control.Controls[index] as ResizeBar;

				// Remove from view any resize bar
				if (bar != null)
					bar.SetBounds(0, 0, 0, 0);
				else
				{
					Control c = _control.Controls[index];

					if (c != null)
					{
						if (newDelta == 0)
							c.Hide();
						else
						{
							// Set new position/size based on direction
							if (_direction == LayoutDirection.Vertical)
								c.SetBounds(clientRect.X, delta, clientRect.Width, newDelta);
							else
								c.SetBounds(delta, clientRect.Y, newDelta,  clientRect.Height);

							if (!c.Visible)
								c.Show();
						}
					}
				}
			}			
		}

        private void AllocateMandatorySizes(ref int[] positions, ref int barSpace, ref int space)
        {
            // Process each control
            for(int index=0, child=0; index<_control.Controls.Count; index++)
            {
                ResizeBar bar = _control.Controls[index] as ResizeBar;

                // Is this a resize bar control?
                if (bar != null)
                {
                    // Length needed is dependant on direction 
                    positions[index] = bar.Length;

                    // Add up how much space was allocated to ResizeBars
                    barSpace += positions[index];
                }
                else
                {
                    Size minimal = _children[child++].MinimumSize;

                    // Length needed is depends on direction 
                    if (_direction == LayoutDirection.Vertical)
                        positions[index] = minimal.Height;
                    else
                        positions[index] = minimal.Width;
                }

                // Reduce available space by that just allocated
                space -= positions[index];
            }			
        }

        private void AllocateRemainingSpace(ref int[] positions, int windowSpace)
        {
            // Space allocated so far
            int allocated = 0;

			Decimal maxPercent = -1m;
			int maxIndex = -1;

            // Process each control
            for(int index=0, childIndex=0; index<_control.Controls.Count; index++)
            {
                Control c = _control.Controls[index];
                
                bool isResizeBar = (c is ResizeBar);

                // We do not allocate any more space for resize bars
                if (!isResizeBar)
                {
                    int extra;
					
					// Get percentage of space request by child
					Decimal childSpace = _children[childIndex++].Space;

                    // How much of remaining space does the group request to have?
                    extra = (int)(windowSpace / 100m * childSpace);

                    // Add the extra space to any existing space it has
                    positions[index] += extra;

                    // Keep count of all allocated so far
                    allocated += extra;

					// Is this the largest space so far?
					if (childSpace > maxPercent)
					{
						maxPercent = childSpace;
						maxIndex = index;
					}
                }
            }

			// Is there any rounding left overs?
			if (allocated < windowSpace)
			{
				// Then add this to the child with the largest percentage
				// (as the largest percentage child is least likely to notice the difference)
				positions[maxIndex] += (windowSpace - allocated);
			}
        }

        private void RepositionChildren(ref int[] positions, Rectangle clientRect, int delta)
        {
            // Process each control 
            for(int index=0; index<_control.Controls.Count; index++)
            {
                // Delta length for this particular control
                int newDelta = positions[index];

                ResizeBar bar = _control.Controls[index] as ResizeBar;

                if (bar != null)
                {
                    if (_direction == LayoutDirection.Vertical)
                    {
                        // Set new position
						bar.SetBounds(clientRect.X, delta, clientRect.Width, newDelta);

                        // Move delta down to next position
                        delta += newDelta;
                    }
                    else
                    {
                        // Set new position
						bar.SetBounds(delta, clientRect.Y, newDelta, clientRect.Height);

						// Move delta across to next position
                        delta += newDelta;
                    }
                }
                else
                {
                    Control c = _control.Controls[index];

                    if (c != null)
                    {
						// If it has no length then hide it
						if (newDelta == 0)
							c.Hide();

						// Set new position/size based on direction
						if (_direction == LayoutDirection.Vertical)
							c.SetBounds(clientRect.X, delta, clientRect.Width, newDelta);
						else
							c.SetBounds(delta, clientRect.Y, newDelta, clientRect.Height);

						// If has a length then ensure its showing
						if (newDelta != 0)
						{
							if (!c.Visible)
								c.Show();

							// Move delta to next position
							delta += newDelta;
						}
                    }
                }
            }			
        }
        
        private void OnControlResize(object sender, EventArgs e)
        {
            // Change the layout of the children to match new size
            RepositionChildren();
        }
        
		private void OnControlDisposed(object sender, EventArgs e)
		{
			// Cast to correct type
			Control c = sender as Control;
			
			// Cache number of child controls
			int count = c.Controls.Count;
			
			for(int i=0; i<count; i++)
			{
				// Always get the first child
				Control child = c.Controls[0];
				
				// Remove it from children
				ControlHelper.RemoveAt(c.Controls, 0);
				
				// Must dispose of resources
				child.Dispose();
			}
		}
		
		private void DeltaGroupSpace(TabGroupBase group, int vector)
        {
            Rectangle clientRect = _control.ClientRectangle;

            // Space available for allocation
            int space;

            // New pixel length of the modified group
            int newLength = vector;
			
            if (_direction == LayoutDirection.Vertical)
            {
                space = clientRect.Height;

                // New pixel size is requested change plus original 
                // height minus the minimal size that is always added
                newLength += group.GroupControl.Height;
                newLength -= group.MinimumSize.Height;
            }
            else
            {
                space = clientRect.Width;

                // New pixel size is requested change plus original 
                // width minus the minimal size that is always added
                newLength += group.GroupControl.Width;
                newLength -= group.MinimumSize.Width;
            }

            int barSpace = 0;

            // Create temporary array of working values
            int[] positions = new int[_control.Controls.Count];

            // Pass 1, allocate all the space needed for each ResizeBar and the 
            //         minimal amount of space that each Window requests. 
            AllocateMandatorySizes(ref positions, ref barSpace, ref space);

            // What is the new percentage it needs?
            Decimal newPercent = 0m;

            // Is there any room to allow a percentage calculation
            if ((newLength > 0) && (space > 0))
                newPercent = (Decimal)newLength / (Decimal)space * 100m;

            // What is the change in area
            Decimal reallocate = newPercent - group.Space;

            // Find the group after this one
            TabGroupBase nextGroup = _children[_children.IndexOf(group) + 1];

            if ((nextGroup.Space - reallocate) < 0m)
                reallocate = nextGroup.Space;
	
            // Modify the Window in question
            group.Space += reallocate;

            // Reverse modify the Window afterwards
            nextGroup.Space -= reallocate;
			
            // Update the visual appearance
            RepositionChildren();
        }
    }
}
