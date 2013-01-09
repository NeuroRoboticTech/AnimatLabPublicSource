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
using System.Diagnostics;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Base class with common functionality for tab group classes.
	/// </summary>
	public abstract class TabGroupBase : IDisposable
	{
		/// <summary>
		/// Specifies a notification code.
		/// </summary>
	    public enum NotifyCode
	    {
			/// <summary>
			/// Specifies the visual style has changed.
			/// </summary>
	        StyleChanged,
	        
			/// <summary>
			/// Specifies the Office2003 style has changed.
			/// </summary>
			OfficeStyleChanged,

            /// <summary>
            /// Specifies the Media Player style has changed.
            /// </summary>
            MediaPlayerStyleChanged,
            
            /// <summary>
			/// Specifies the IDE2005 style has changed.
			/// </summary>
			IDE2005StyleChanged,

			/// <summary>
			/// Specifies the prominent leaf has changed.
			/// </summary>
            ProminentChanged,

			/// <summary>
			/// Specifies the minimum group size has changed.
			/// </summary>
            MinimumSizeChanged,
			
			/// <summary>
			/// Specifies the resize bar vector has changed.
			/// </summary>
            ResizeBarVectorChanged,

			/// <summary>
			/// Specifies the resize bar color has changed.
			/// </summary>
	        ResizeBarColorChanged,

			/// <summary>
			/// Specifies the prominent background color has changed.
			/// </summary>
			ProminentBackColorChanged,

			/// <summary>
			/// Specifies the prominent foreground color has changed.
			/// </summary>
			ProminentForeColorChanged,

			/// <summary>
			/// Specifies the display tab mode has changed.
			/// </summary>
	        DisplayTabMode,

			/// <summary>
			/// Specifies the source image list is about to change.
			/// </summary>
	        ImageListChanging,

			/// <summary>
			/// Specifies the source image list has changed.
			/// </summary>
	        ImageListChanged,

			/// <summary>
			/// Specifies the mode used for drawing hotkeys in tab headers.
			/// </summary>
			HotkeyPrefixChanged
		}

        // Class fields
        private static int _count = 0;
	    
	    // Instance fields
	    private int _unique;
		private bool _resizeBarLock;
	    private object _tag;
		private TabGroupBase _parent;
        internal Size _minSize;
        internal Decimal _space;
        internal TabbedGroups _tabbedGroups;
	
		/// <summary>
		/// Initializes a new instance of the TabGroupBase class.
		/// </summary>
		/// <param name="tabbedGroups">Owning control instance.</param>
        public TabGroupBase(TabbedGroups tabbedGroups)
        {
            InternalConstruct(tabbedGroups, null);
        }
        
		/// <summary>
		/// Initializes a new instance of the TabGroupBase class.
		/// </summary>
		/// <param name="tabbedGroups">Owning control instance.</param>
		/// <param name="parent">Parent group instance.</param>
        public TabGroupBase(TabbedGroups tabbedGroups, TabGroupBase parent)
		{
		    InternalConstruct(tabbedGroups, parent);
		}
		
		private void InternalConstruct(TabbedGroups tabbedGroups, TabGroupBase parent)
		{
		    // Assign initial values
		    _tabbedGroups = tabbedGroups;
		    _parent = parent;
		    _unique = _count++;
			_resizeBarLock = false;
		    
		    // Defaults
		    _tag = null;
		    _space = 100m;
		    _minSize = new Size(_tabbedGroups.DefaultGroupMinimumWidth,
		                        _tabbedGroups.DefaultGroupMinimumHeight);
		}

		/// <summary>
		/// Releases all resources used by the group.
		/// </summary>
		public abstract void Dispose();

		/// <summary>
		/// Gets and sets the percentage space this group occupies inside parent group.
		/// </summary>
        public Decimal Space
        {
            get 
            {
                TabGroupLeaf prominent = _tabbedGroups.ProminentLeaf;
                
                // Are we in prominent mode?
                if (prominent != null)
                {
                    // If we are a child of the root sequence
                    if (_parent.Parent == null)
                    {
                        // Then our space is determined by the containment of the prominent leaf
                        if (this.ContainsProminent(true))
                            return 100m;
                        else
                            return 0m;
                    }
                    else
                    {
                        // Else, if we are inside a sequence that contains prominent leaf
                        if (_parent.ContainsProminent(true))
                        {
                            // Then we need to decide on all or nothing allocation
                            if (this.ContainsProminent(true))
                                return 100m;
                            else
                                return 0m;
                        }
                        else
                        {
                            // Otherwise, we will already be shrunk
                            return _space;                        
                        }
                    }
                }
                else
                    return _space; 
            }
            
            set { _space = value; }
        }

		/// <summary>
		/// Set the Space of this item so it occupies the specified number of pixels, if possible.
		/// </summary>
		/// <param name="pixelSize">Pixel length requested.</param>
		public void SetPixelLength(int pixelSize)
		{
			// Must use parent sequence to actually perform operation
			if (_parent != null)
			{
				// Parent must always be a sequence
				TabGroupSequence tgs = _parent as TabGroupSequence;

				// Resizing this object is requires knowledge of other siblings
				tgs.SetPixelLengthOfChild(this, pixelSize);
			}
		}

		/// <summary>
		/// Gets and sets the minimum screen size this group requires.
		/// </summary>
        public Size MinimumSize
        {
            get { return _minSize; }
            
            set
            {
                if (!_minSize.Equals(value))
                {
                    _minSize = value;
                    
                    // Inform parent it might need to resize its children
                    if (_parent != null)
                        _parent.Notify(NotifyCode.MinimumSizeChanged);
                }
            }
        }

		/// <summary>
		/// Gets or sets a value indicating if this group should be allowed to be resized by bar.
		/// </summary>
		public bool ResizeBarLock
		{
			get { return _resizeBarLock; }
			set { _resizeBarLock = value; }
		}

		/// <summary>
		/// Gets the parent group this instance is inside.
		/// </summary>
        public TabGroupBase Parent 
        {
            get { return _parent; }
        }

		/// <summary>
		/// Gets the parent control instance.
		/// </summary>
        public TabbedGroups TabbedGroups 
        {
            get { return _tabbedGroups; }
        }

		/// <summary>
		/// Gets and sets data associated with the instance.
		/// </summary>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
        
		/// <summary>
		/// Gets and sets a unique number used to help debugging.
		/// </summary>
        public int Unique
        {
            get { return _unique; }
			set { _unique = value; }
        }

        /// <summary>
        /// Gets the number of child items.
        /// </summary>
        public abstract int Count               { get; }

		/// <summary>
		/// Gets a value indicating whether the group is a leaf.
		/// </summary>
        public abstract bool IsLeaf             { get; }

		/// <summary>
		/// Gets a value indicating whether the group is a sequence.
		/// </summary>
        public abstract bool IsSequence         { get; }

		/// <summary>
		/// Gets the parent control instance.
		/// </summary>
        public abstract Control GroupControl    { get; }
        
		/// <summary>
		/// Informs group of a notification.
		/// </summary>
		/// <param name="code">Which notification has occured.</param>
        public abstract void Notify(NotifyCode code); 

		/// <summary>
		/// Returns a value indicating whether the group contains the prominent leaf.
		/// </summary>
		/// <param name="recurse">Should the group search child groups.</param>
		/// <returns>true if prominent leaf contained; otherwise false.</returns>
        public abstract bool ContainsProminent(bool recurse);

		/// <summary>
		/// Request this group save its information and child groups.
		/// </summary>
		/// <param name="xmlOut">Write to save information into.</param>
        public abstract void SaveToXml(XmlTextWriter xmlOut);

		/// <summary>
		/// Request this group load its information and child groups.
		/// </summary>
		/// <param name="xmlIn">Reader to load information from.</param>
        public abstract void LoadFromXml(XmlTextReader xmlIn);

		internal Decimal RealSpace
		{
			get { return _space; }
			set { _space = value; }
		}

		internal void SetParent(TabGroupBase tgb)
		{
			_parent = tgb;
		}
        
		[Conditional("DEBUG")]
		internal static void DebugSpace(TabGroupSequence tgs)
		{
			Decimal total = 0;
			for(int l=0; l<tgs.Count; l++)
				total += tgs[l].Space;

			if ((total != 100m) && (total != 0m))
			{
				Console.WriteLine("** ERROR ** Inaccurate Space = {0}", total);

				for(int l=0; l<tgs.Count; l++)
					Console.WriteLine("Base %:{0} ID:{1}", tgs[l].Space, tgs[l].Unique);

				Console.WriteLine("");

				System.Diagnostics.Debug.Assert(false);
			}
		}

		[Conditional("DEBUG")]
		internal static void DebugStruct(TabGroupSequence tgs, string title)
		{
			Console.WriteLine("Structure {0}", title);
			DebugStructure(tgs, 0);
			Console.WriteLine("");
		}
        
		[Conditional("DEBUG")]
		internal static void DebugStructure(TabGroupSequence tgs, int indent)
		{
			int parentId = -1;
			if (tgs.Parent != null)
				parentId = tgs.Parent.Unique;
         
			Decimal total = 0;
			for(int l=0; l<tgs.Count; l++)
				total += tgs[l].Space;

			for(int k=0; k<indent; k++)
				Console.Write(" ");

			Console.WriteLine("Sequence({0}) %:{1} ID:{2} P:{3} nChild:{4} Dir:{5}", 
				total, tgs.Space, tgs.Unique, parentId, tgs.Count, tgs.Direction); 
			
			indent++;
				
			for(int i=0; i<tgs.Count; i++)
			{
				TabGroupBase tgb = tgs[i];
                
				if (tgb is TabGroupSequence)
					DebugStructure(tgb as TabGroupSequence, indent);
				else
				{
					if (tgb.Parent != null)
						parentId = tgb.Parent.Unique;

					for(int j=0; j<indent; j++)
						Console.Write(" ");
					
					Console.WriteLine("Leaf %:{0} ID:{1} P:{2} nChild:{3}", 
						tgb.Space, tgb.Unique, parentId, (tgb.GroupControl as TabControl).TabPages.Count);
				}
			}
		}
	}
}
