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
using System.Collections;
using System.Windows.Forms;
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Represents a target for dragging and dropping.
	/// </summary>
	public class Target
	{
		/// <summary>
		/// Specifies the action required.
		/// </summary>
	    public enum TargetActions
	    {
			/// <summary>
			/// Specifies page should be moved between leafs.
			/// </summary>
	        Transfer,

			/// <summary>
			/// Specifies new group be create on left side.
			/// </summary>
	        GroupLeft,
			
			/// <summary>
			/// Specifies new group be create on right side of a group.
			/// </summary>
	        GroupRight,

			/// <summary>
			/// Specifies new group be create on top side of a group.
			/// </summary>
	        GroupTop,

			/// <summary>
			/// Specifies new group be create on bottom side of a group.
			/// </summary>
	        GroupBottom,

			/// <summary>
			/// Specifies new group be create on left side of control of a group.
			/// </summary>
	        ControlLeft,

			/// <summary>
			/// Specifies new group be create on right side of control.
			/// </summary>
            ControlRight,

			/// <summary>
			/// Specifies new group be create on top side of control.
			/// </summary>
            ControlTop,

			/// <summary>
			/// Specifies new group be create on bottom side of control.
			/// </summary>
            ControlBottom,
        }
	    
	    // Instance fields
	    private Rectangle _hotRect;
        private Rectangle _drawRect;
        private TabGroupLeaf _leaf;
        private TargetActions _action;

		/// <summary>
		/// Initializes a new instance of the Target class.
		/// </summary>
		/// <param name="hotRect">Rectangle that represents active area of target.</param>
		/// <param name="drawRect">Rectangle that should be drawn when active.</param>
		/// <param name="leaf">Source leaf associated with target.</param>
		/// <param name="action">Action to be performed if target selected.</param>
        public Target(Rectangle hotRect, 
					  Rectangle drawRect, 
					  TabGroupLeaf leaf, 
					  TargetActions action)
        {
            // Define state
            _hotRect = hotRect;
            _drawRect = drawRect;
            _leaf = leaf;
            _action = action;
        }

		/// <summary>
		/// Gets the HotRect property.
		/// </summary>
        public Rectangle HotRect
        {
            get { return _hotRect; }
        }	    
        
		/// <summary>
		/// Gets the DrawRect property.
		/// </summary>
        public Rectangle DrawRect
        {
            get { return _drawRect; }
        }	    

		/// <summary>
		/// Gets the Leaf property.
		/// </summary>
        public TabGroupLeaf Leaf
        {
            get { return _leaf; }
        }

		/// <summary>
		/// Gets the Action property.
		/// </summary>
        public TargetActions Action
        {
            get { return _action; }
        }
	}
	
	/// <summary>
	/// Provides a collection container for Target instances.
	/// </summary>
	public class TargetCollection : CollectionWithEvents
	{
		/// <summary>
		/// Adds the specified Target object to the collection.
		/// </summary>
		/// <param name="value">The Target object to add to the collection.</param>
		/// <returns>The Target object added to the collection.</returns>
		public Target Add(Target value)
		{
			// Use base class to process actual collection operation
			base.List.Add(value as object);

			return value;
		}

		/// <summary>
		/// Adds an array of Target objects to the collection.
		/// </summary>
		/// <param name="values">An array of Target objects to add to the collection.</param>
		public void AddRange(Target[] values)
		{
			// Use existing method to add each array entry
			foreach(Target page in values)
				Add(page);
		}

		/// <summary>
		/// Removes a Target from the collection.
		/// </summary>
		/// <param name="value">A Target to remove from the collection.</param>
		public void Remove(Target value)
		{
			// Use base class to process actual collection operation
			base.List.Remove(value as object);
		}

		/// <summary>
		/// Inserts a Target instance into the collection at the specified location.
		/// </summary>
		/// <param name="index">The location in the collection where you want to add the Target.</param>
		/// <param name="value">The Target object to insert.</param>
		public void Insert(int index, Target value)
		{
			// Use base class to process actual collection operation
			base.List.Insert(index, value as object);
		}

		/// <summary>
		/// Determines whether a Target is in the collection.
		/// </summary>
		/// <param name="value">The Target to locate in the collection.</param>
		/// <returns>true if item is found in the collection; otherwise, false.</returns>
		public bool Contains(Target value)
		{
			// Use base class to process actual collection operation
			return base.List.Contains(value as object);
		}

		/// <summary>
		/// Determines the first Target instance that matches the given Point.
		/// </summary>
		/// <param name="pt">Point to test against each Target instance.</param>
		/// <returns>Matching Target if found; otherwise null</returns>
		public Target Contains(Point pt)
		{
			foreach(Target t in base.List)
			{
				if (t.HotRect.Contains(pt))
					return t;
			}

			return null;
		}

		/// <summary>
		/// Gets the Target at the specified index.
		/// </summary>
		public Target this[int index]
		{
			// Use base class to process actual collection operation
			get { return (base.List[index] as Target); }
		}

		/// <summary>
		/// Returns the index of the first occurrence of the given Target.
		/// </summary>
		/// <param name="value">The Target to locate.</param>
		/// <returns>Index of object; otherwise -1</returns>
		public int IndexOf(Target value)
		{
			// Find the 0 based index of the requested entry
			return base.List.IndexOf(value);
		}
	}
}
