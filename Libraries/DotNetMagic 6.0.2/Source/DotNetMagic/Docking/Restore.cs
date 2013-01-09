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
using System.Xml;
using System.Drawing;
using System.Windows.Forms;
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Docking
{
	/// <summary>
	/// Base class for handling restore information.
	/// </summary>
    public class Restore
    {
		// Instance fields
		private Restore _child;

		/// <summary>
		/// Initializes a new instance of the Restore class.
		/// </summary>
		public Restore()
		{
			// Default state
			_child = null;
		}

		/// <summary>
		/// Initializes a new instance of the Restore class.
		/// </summary>
		/// <param name="child"></param>
		public Restore(Restore child)
		{
			// Remember parameter
			_child = child;
		}

		/// <summary>
		/// Gets and sets the child control.
		/// </summary>
        public Restore Child
        {
            get { return _child; }
            set { _child = value; }
        }

		/// <summary>
		/// Perform a restore using provided docking manager.
		/// </summary>
		/// <param name="dm">Reference to source.</param>
        public virtual void PerformRestore(DockingManager dm) {}

		/// <summary>
		/// Perform a restore using provided window.
		/// </summary>
		/// <param name="w">Reference to source.</param>
		public virtual void PerformRestore(Window w) {}

		/// <summary>
		/// Perform a restore using provided zone.
		/// </summary>
		/// <param name="z">Reference to source.</param>
        public virtual void PerformRestore(Zone z) {}

		/// <summary>
		/// Perform a restore.
		/// </summary>
        public virtual void PerformRestore() {}

		/// <summary>
		/// Reconnect the object after a load has completed.
		/// </summary>
		/// <param name="dm">Reference to docking manager.</param>
		public virtual void Reconnect(DockingManager dm)
		{
			if (_child != null)
				_child.Reconnect(dm);
		}

		/// <summary>
		/// Save the object state to a stream.
		/// </summary>
		/// <param name="xmlOut">Stream for saving.</param>
		public virtual void SaveToXml(XmlTextWriter xmlOut)
		{
			// Must define my type so loading can recreate my instance
			xmlOut.WriteAttributeString("Type", this.GetType().ToString());

			SaveInternalToXml(xmlOut);

			// Output the child object			
			xmlOut.WriteStartElement("Child");

			if (_child == null)
				xmlOut.WriteAttributeString("Type", "null");
			else
				_child.SaveToXml(xmlOut);

			xmlOut.WriteEndElement();
		}

		/// <summary>
		/// Load the object state from a stream.
		/// </summary>
		/// <param name="xmlIn">Stream for loading.</param>
		/// <param name="formatVersion">Format version number.</param>
		public virtual void LoadFromXml(XmlTextReader xmlIn, int formatVersion)
		{
			LoadInternalFromXml(xmlIn, formatVersion);

			// Move to next xml node
			if (!xmlIn.Read())
				throw new ArgumentException("Could not read in next expected node");

			// We skip over the element because of a bug introduced in version 1.0.7
			// where we added a new element but did not bump the format version number. Doh!
			if (xmlIn.Name == "CurrentContent")
			{
				// Move to next xml node
				if (!xmlIn.Read())
					throw new ArgumentException("Could not read in next expected node");
			}

			// Check it has the expected name
			if (xmlIn.Name != "Child")
				throw new ArgumentException("Node 'Child' expected but not found");

			string type = xmlIn.GetAttribute(0);
			
			if (type != "null")
				_child = CreateFromXml(xmlIn, false, formatVersion);

			// Move past the end element
			if (!xmlIn.Read())
				throw new ArgumentException("Could not read in next expected node");
		
			// Check it has the expected name
			if (xmlIn.NodeType != XmlNodeType.EndElement)
				throw new ArgumentException("EndElement expected but not found");
		}

		/// <summary>
		/// Save object specific information to stream.
		/// </summary>
		/// <param name="xmlOut">Stream for loading.</param>
		public virtual void SaveInternalToXml(XmlTextWriter xmlOut) {}

		/// <summary>
		/// Load object specific information from stream.
		/// </summary>
		/// <param name="xmlIn">Stream for loading.</param>
		/// <param name="formatVersion">Format version number.</param>
		public virtual void LoadInternalFromXml(XmlTextReader xmlIn, int formatVersion) {}

		/// <summary>
		/// Recreate a Restore derived object from the stream.
		/// </summary>
		/// <param name="xmlIn">Stream for loading.</param>
		/// <param name="readIn">Should move to next XML node.</param>
		/// <param name="formatVersion">Format version number.</param>
		/// <returns></returns>
		public static Restore CreateFromXml(XmlTextReader xmlIn, bool readIn, int formatVersion)
		{
			if (readIn)
			{
				// Move to next xml node
				if (!xmlIn.Read())
					throw new ArgumentException("Could not read in next expected node");
			}
			
			// Grab type name of the object to create
			string attrType = xmlIn.GetAttribute(0);

			// Convert any 'Magic' into 'DotNetMagic' so that older config files are compatible
			attrType = attrType.Replace(".Magic", ".DotNetMagic");

			// Convert from string to a Type description object
			Type newType = Type.GetType(attrType);

			// Create an instance of this object which must derive from Restore base class
			Restore newRestore = newType.Assembly.CreateInstance(attrType) as Restore;

			// Ask the object to load itself
			newRestore.LoadFromXml(xmlIn, formatVersion);

			return newRestore;
		}
	}

	/// <summary>
	/// Base class for handling content base restore information.
	/// </summary>
	public class RestoreContent : Restore
	{
		// Instance fields
		private String _title;
		private String _uniqueName;
		private Content _content;

		/// <summary>
		/// Initializes a new instance of the RestoreContent class.
		/// </summary>
		public RestoreContent()
			: base()
		{
			// Default state
			_title = "";
			_content = null;
		}

		/// <summary>
		/// Initializes a new instance of the RestoreContent class.
		/// </summary>
		/// <param name="content">Reference to source content.</param>
		public RestoreContent(Content content)
			: base()
		{
			// Remember parameter
			_title = content.Title;
			_uniqueName = content.UniqueName;
			_content = content;
		}

		/// <summary>
		/// Initializes a new instance of the RestoreContent class.
		/// </summary>
		/// <param name="child">Restore object to be chained.</param>
		/// <param name="content">Reference to source content.</param>
		public RestoreContent(Restore child, Content content)
			: base(child)
		{
			// Remember parameter
			_title = content.Title;
			_uniqueName = content.UniqueName;
			_content = content;
		}

		/// <summary>
		/// Gets the cached content instance.
		/// </summary>
		public Content Content
		{
			get { return _content; }
		}

		/// <summary>
		/// Reconnect the object after a load has completed.
		/// </summary>
		/// <param name="dm">Reference to docking manager.</param>
		public override void Reconnect(DockingManager dm)
		{
			// Connect to the current instance of required content object
			if ((_uniqueName != null) && (_uniqueName.Length > 0))
				_content = dm.Contents.FindUniqueName(_uniqueName);
			else
				_content = dm.Contents[_title];

			base.Reconnect(dm);
		}

		/// <summary>
		/// Save object specific information to stream.
		/// </summary>
		/// <param name="xmlOut">Stream for loading.</param>
		public override void SaveInternalToXml(XmlTextWriter xmlOut)
		{
			base.SaveInternalToXml(xmlOut);
			xmlOut.WriteStartElement("Content");
			xmlOut.WriteAttributeString("Name", _content.Title);
			xmlOut.WriteAttributeString("UniqueName", _content.UniqueName);
			xmlOut.WriteEndElement();				
		}

		/// <summary>
		/// Load object specific information from stream.
		/// </summary>
		/// <param name="xmlIn">Stream for loading.</param>
		/// <param name="formatVersion">Format version number.</param>
		public override void LoadInternalFromXml(XmlTextReader xmlIn, int formatVersion)
		{
			base.LoadInternalFromXml(xmlIn, formatVersion);

			// Move to next xml node
			if (!xmlIn.Read())
				throw new ArgumentException("Could not read in next expected node");

			// Check it has the expected name
			if (xmlIn.Name != "Content")
				throw new ArgumentException("Node 'Content' expected but not found");

			// Grab type name of the object to create
			_title = xmlIn.GetAttribute(0);
			
			// 'UniqueName' property added in version 8 format and above
			if (formatVersion >= 8)
				_uniqueName = xmlIn.GetAttribute(1);
			else
				_uniqueName = "";
		}
	}
	
	/// <summary>
	/// Restore of content state.
	/// </summary>
	public class RestoreContentState : RestoreContent
	{
		// Instance fields
		private State _state;

		/// <summary>
		/// Initializes a new instance of the RestoreContentState class.
		/// </summary>
		public RestoreContentState()
			: base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the RestoreContentState class.
		/// </summary>
		/// <param name="state">State of content.</param>
		/// <param name="content">Reference to source content.</param>
		public RestoreContentState(State state, Content content)
			: base(content)
		{
			// Remember parameter
			_state = state;
		}

		/// <summary>
		/// Initializes a new instance of the RestoreContentState class.
		/// </summary>
		/// <param name="child">Restore object to be chained.</param>
		/// <param name="state">State of content.</param>
		/// <param name="content">Reference to source content.</param>
		public RestoreContentState(Restore child, State state, Content content)
			: base(child, content)
		{
			// Remember parameter
			_state = state;
		}

		/// <summary>
		/// Gets the cached state value.
		/// </summary>
		public State State
		{
			get { return _state; }
		}

		/// <summary>
		/// Perform a restore using provided docking manager.
		/// </summary>
		/// <param name="dm">Reference to source.</param>
		public override void PerformRestore(DockingManager dm)
		{
			// Use the existing DockingManager method that will create a Window appropriate for 
			// this Content and then add a new Zone for hosting the Window. It will always place
			// the Zone at the inner most level
			dm.AddContentWithState(Content, _state);				
		}

		/// <summary>
		/// Save object specific information to stream.
		/// </summary>
		/// <param name="xmlOut">Stream for loading.</param>
		public override void SaveInternalToXml(XmlTextWriter xmlOut)
		{
			base.SaveInternalToXml(xmlOut);
			xmlOut.WriteStartElement("State");
			xmlOut.WriteAttributeString("Value", _state.ToString());
			xmlOut.WriteEndElement();				
		}

		/// <summary>
		/// Load object specific information from stream.
		/// </summary>
		/// <param name="xmlIn">Stream for loading.</param>
		/// <param name="formatVersion">Format version number.</param>
		public override void LoadInternalFromXml(XmlTextReader xmlIn, int formatVersion)
		{
			base.LoadInternalFromXml(xmlIn, formatVersion);

			// Move to next xml node
			if (!xmlIn.Read())
				throw new ArgumentException("Could not read in next expected node");

			// Check it has the expected name
			if (xmlIn.Name != "State")
				throw new ArgumentException("Node 'State' expected but not found");

			// Grab type state of the object to create
			string attrState = xmlIn.GetAttribute(0);

			// Convert from string to enumeration value
			_state = (State)Enum.Parse(typeof(State), attrState);
		}
	}
	
	/// <summary>
	/// Restore of an auto hidden content.
	/// </summary>
	public class RestoreAutoHideState : RestoreContentState
	{
	    // Instance fields
	    
		/// <summary>
		/// Initializes a new instance of the RestoreAutoHideState class.
		/// </summary>
	    public RestoreAutoHideState()
	        : base()
	    {
	    }
        
		/// <summary>
		/// Initializes a new instance of the RestoreAutoHideState class.
		/// </summary>
		/// <param name="state">State of content.</param>
		/// <param name="content">Reference to source content.</param>
        public RestoreAutoHideState(State state, Content content)
            : base(state, content)
        {
        }

		/// <summary>
		/// Initializes a new instance of the RestoreAutoHideState class.
		/// </summary>
		/// <param name="child">Restore object to be chained.</param>
		/// <param name="state">State of content.</param>
		/// <param name="content">Reference to source content.</param>
        public RestoreAutoHideState(Restore child, State state, Content content)
            : base(child, state, content)
        {
        }
    
		/// <summary>
		/// Perform a restore using provided docking manager.
		/// </summary>
		/// <param name="dm">Reference to source.</param>
		public override void PerformRestore(DockingManager dm)
        {
            // Create collection of Contents to auto hide
            ContentCollection cc = new ContentCollection();
            
            // In this case, there is only one
            cc.Add(Content);
        
            // Add to appropriate AutoHidePanel based on _state
            dm.AutoHideContents(cc, State, null);
        }
    }

	/// <summary>
	/// Restore of an auto hidden content with affinity.
	/// </summary>
    public class RestoreAutoHideAffinity : RestoreAutoHideState
    {
        // Instance fields
        private StringCollection _next;
        private StringCollection _previous;
        private StringCollection _nextAll;
        private StringCollection _previousAll;
		private bool _currentContent;

		/// <summary>
		/// Initializes a new instance of the RestoreAutoHideAffinity class.
		/// </summary>
        public RestoreAutoHideAffinity()
            : base()
        {
            // Must always point to valid reference
            _next = new StringCollection();
            _previous = new StringCollection();
            _nextAll = new StringCollection();
            _previousAll = new StringCollection();

			// Defaults
			_currentContent = false;
        }

		/// <summary>
		/// Initializes a new instance of the RestoreAutoHideAffinity class.
		/// </summary>
		/// <param name="child">Restore object to be chained.</param>
		/// <param name="state">State of content.</param>
		/// <param name="content">Reference to source content.</param>
		/// <param name="next">Collection of content names.</param>
		/// <param name="previous">Collection of content names.</param>
		/// <param name="nextAll">Collection of content names.</param>
		/// <param name="previousAll">Collection of content names.</param>
		/// <param name="currentContent">Is this content supposed to become the current one.</param>
		public RestoreAutoHideAffinity(Restore child, 
                                       State state,
                                       Content content, 
                                       StringCollection next,
                                       StringCollection previous,
                                       StringCollection nextAll,
                                       StringCollection previousAll,
									   bool currentContent)
        : base(child, state, content)
        {
            // Remember parameters
            _next = next;				
            _previous = previous;	
            _nextAll = nextAll;				
            _previousAll = previousAll;	
			_currentContent = currentContent;
        }

		/// <summary>
		/// Perform a restore using provided docking manager.
		/// </summary>
		/// <param name="dm">Reference to source.</param>
		public override void PerformRestore(DockingManager dm)
        {   
            // Get the correct target panel from state
            AutoHidePanel ahp = dm.AutoHidePanelForState(State);
            
            ahp.AddContent(Content, _next, _previous, _nextAll, _previousAll, _currentContent);
        }

		/// <summary>
		/// Save object specific information to stream.
		/// </summary>
		/// <param name="xmlOut">Stream for loading.</param>
		public override void SaveInternalToXml(XmlTextWriter xmlOut)
        {
            base.SaveInternalToXml(xmlOut);
            _next.SaveToXml("Next", xmlOut);
            _previous.SaveToXml("Previous", xmlOut);
            _nextAll.SaveToXml("NextAll", xmlOut);
            _previousAll.SaveToXml("PreviousAll", xmlOut);

			// Save if this content should become the current one
			xmlOut.WriteStartElement("CurrentContent");
			xmlOut.WriteAttributeString("Value", Convert.ToString(_currentContent));
			xmlOut.WriteEndElement();				
        }

		/// <summary>
		/// Load object specific information from stream.
		/// </summary>
		/// <param name="xmlIn">Stream for loading.</param>
		/// <param name="formatVersion">Format version number.</param>
		public override void LoadInternalFromXml(XmlTextReader xmlIn, int formatVersion)
        {
            base.LoadInternalFromXml(xmlIn, formatVersion);
            _next.LoadFromXml("Next", xmlIn);
            _previous.LoadFromXml("Previous", xmlIn);
            _nextAll.LoadFromXml("NextAll", xmlIn);
            _previousAll.LoadFromXml("PreviousAll", xmlIn);

			if (formatVersion >= 6)
			{
				// Move to next xml node
				if (!xmlIn.Read())
					throw new ArgumentException("Could not read in next expected node");

				// Check it has the expected name
				if (xmlIn.Name != "CurrentContent")
					throw new ArgumentException("Node 'CurrentContent' expected but not found");

				// Grab raw position information
				string attrValue = xmlIn.GetAttribute(0);

				// Convert from string to proper types
				_currentContent = Convert.ToBoolean(attrValue);
			}
			else
			{
				// Default for older version
				_currentContent = false;
			}
        }
    }

	/// <summary>
	/// Restore content with docking edge affinity.
	/// </summary>
	public class RestoreContentDockingAffinity : RestoreContentState
	{
		// Instance fields
		private Size _size;
		private Point _location;
		private StringCollection _best;
		private StringCollection _next;
		private StringCollection _previous;
		private StringCollection _nextAll;
		private StringCollection _previousAll;

		/// <summary>
		/// Initializes a new instance of the RestoreContentDockingAffinity class.
		/// </summary>
		public RestoreContentDockingAffinity()
			: base()
		{
			// Must always point to valid reference
			_best = new StringCollection();
			_next = new StringCollection();
			_previous = new StringCollection();
			_nextAll = new StringCollection();
			_previousAll = new StringCollection();
		}

		/// <summary>
		/// Initializes a new instance of the RestoreContentDockingAffinity class.
		/// </summary>
		/// <param name="child">Restore object to be chained.</param>
		/// <param name="state">State of content.</param>
		/// <param name="content">Reference to source content.</param>
		/// <param name="best">Collection of content names.</param>
		/// <param name="next">Collection of content names.</param>
		/// <param name="previous">Collection of content names.</param>
		/// <param name="nextAll">Collection of content names.</param>
		/// <param name="previousAll">Collection of content names.</param>
		public RestoreContentDockingAffinity(Restore child, 
										     State state, 
											 Content content, 
											 StringCollection best,
											 StringCollection next,
											 StringCollection previous,
											 StringCollection nextAll,
											 StringCollection previousAll)
			: base(child, state, content)
		{
			// Remember parameters
			_best = best;
			_next = next;
			_previous = previous;
			_nextAll = nextAll;
			_previousAll = previousAll;
			_size = content.DisplaySize;
			_location = content.DisplayLocation;
        }

		/// <summary>
		/// Perform a restore using provided docking manager.
		/// </summary>
		/// <param name="dm">Reference to source.</param>
		public override void PerformRestore(DockingManager dm)
		{
			int count = dm.Container.Controls.Count;

			int min = -1;
			int max = dm.OuterControlIndex();

			if (dm.InnerControl != null)
				min = dm.Container.Controls.IndexOf(dm.InnerControl);

			int beforeIndex = -1;
			int afterIndex = max;
			int beforeAllIndex = -1;
			int afterAllIndex = max;

			// Create a collection of the Zones in the appropriate direction
			for(int index=0; index<count; index++)
			{
				Zone z = dm.Container.Controls[index] as Zone;

				if (z != null)
				{
					StringCollection sc = ZoneHelper.ContentNames(z);
					
					if (State == z.State)
					{
						if (sc.Contains(_best))
						{
							// Can we delegate to a child Restore object
							if (Child != null)
								Child.PerformRestore(z);
							else
							{
								// Just add an appropriate Window to start of the Zone
								dm.AddContentToZone(Content, z, 0);
							}
							return;
						}

						// If the WindowContent contains a Content previous to the target
						if (sc.Contains(_previous))
						{
							if (index > beforeIndex)
								beforeIndex = index;
						}
						
						// If the WindowContent contains a Content next to the target
						if (sc.Contains(_next))
						{
							if (index < afterIndex)
								afterIndex = index;
						}
					}
					else
					{
						// If the WindowContent contains a Content previous to the target
						if (sc.Contains(_previousAll))
						{
							if (index > beforeAllIndex)
								beforeAllIndex = index;
						}
						
						// If the WindowContent contains a Content next to the target
						if (sc.Contains(_nextAll))
						{
							if (index < afterAllIndex)
								afterAllIndex = index;
						}
					}
				}
			}

			dm.Container.SuspendLayout();

			// Create a new Zone with correct State
			Zone newZ = dm.CreateZoneForContent(State);

			// Restore the correct content size/location values
			Content.DisplaySize = _size;
			Content.DisplayLocation = _location;

			// Add an appropriate Window to start of the Zone
			dm.AddContentToZone(Content, newZ, 0);

			// Did we find a valid 'before' Zone?
			if (beforeIndex != -1)
			{
				// Try and place more accurately according to other edge Zones
				if (beforeAllIndex > beforeIndex)
					beforeIndex = beforeAllIndex;

				// Check against limits
				if (beforeIndex >= max)
					beforeIndex = max - 1;

				dm.Container.Controls.SetChildIndex(newZ, beforeIndex + 1);
			}
			else
			{
				// Try and place more accurately according to other edge Zones
				if (afterAllIndex < afterIndex)
					afterIndex = afterAllIndex;

				// Check against limits
				if (afterIndex <= min)
					afterIndex = min + 1;
				
				if (afterIndex > min)
					dm.Container.Controls.SetChildIndex(newZ, afterIndex);
				else
				{
					// Set the Zone to be the least important of our Zones
					dm.ReorderZoneToInnerMost(newZ);
				}
			}

			dm.Container.ResumeLayout();
		}

		/// <summary>
		/// Save object specific information to stream.
		/// </summary>
		/// <param name="xmlOut">Stream for loading.</param>
		public override void SaveInternalToXml(XmlTextWriter xmlOut)
		{
			base.SaveInternalToXml(xmlOut);
			xmlOut.WriteStartElement("Position");
			xmlOut.WriteAttributeString("Size", ConversionHelper.SizeToString(_size));
			xmlOut.WriteAttributeString("Location", ConversionHelper.PointToString(_location));
			xmlOut.WriteEndElement();				
			_best.SaveToXml("Best", xmlOut);
			_next.SaveToXml("Next", xmlOut);
			_previous.SaveToXml("Previous", xmlOut);
			_nextAll.SaveToXml("NextAll", xmlOut);
			_previousAll.SaveToXml("PreviousAll", xmlOut);
		}

		/// <summary>
		/// Load object specific information from stream.
		/// </summary>
		/// <param name="xmlIn">Stream for loading.</param>
		/// <param name="formatVersion">Format version number.</param>
		public override void LoadInternalFromXml(XmlTextReader xmlIn, int formatVersion)
		{
			base.LoadInternalFromXml(xmlIn, formatVersion);

			// Move to next xml node
			if (!xmlIn.Read())
				throw new ArgumentException("Could not read in next expected node");

			// Check it has the expected name
			if (xmlIn.Name != "Position")
				throw new ArgumentException("Node 'Position' expected but not found");

			// Grab raw position information
			string attrSize = xmlIn.GetAttribute(0);
			string attrLocation = xmlIn.GetAttribute(1);

			// Convert from string to proper types
			_size = ConversionHelper.StringToSize(attrSize);
			_location = ConversionHelper.StringToPoint(attrLocation);

			_best.LoadFromXml("Best", xmlIn);
			_next.LoadFromXml("Next", xmlIn);
			_previous.LoadFromXml("Previous", xmlIn);
			_nextAll.LoadFromXml("NextAll", xmlIn);
			_previousAll.LoadFromXml("PreviousAll", xmlIn);
		}
	}

	/// <summary>
	/// Restore floating content object with affinity.
	/// </summary>
	public class RestoreContentFloatingAffinity : RestoreContentState
	{
		// Instance fields
		private Size _size;
		private Point _location;
		private StringCollection _best;
		private StringCollection _associates;

		/// <summary>
		/// Initializes a new instance of the RestoreContentFloatingAffinity class.
		/// </summary>
		public RestoreContentFloatingAffinity()
			: base()
		{
			// Must always point to valid reference
			_best = new StringCollection();
			_associates = new StringCollection();
		}

		/// <summary>
		/// Initializes a new instance of the RestoreContentFloatingAffinity class.
		/// </summary>
		/// <param name="child">Restore object to be chained.</param>
		/// <param name="state">State of content.</param>
		/// <param name="content">Reference to source content.</param>
		/// <param name="best">Collection of content names.</param>
		/// <param name="associates">Collection of content names.</param>
		public RestoreContentFloatingAffinity(Restore child, 
										      State state, 
											  Content content, 
											  StringCollection best,
											  StringCollection associates)
			: base(child, state, content)
		{
			// Remember parameters
			_best = best;
			_associates = associates;
			_size = content.DisplaySize;
			_location = content.DisplayLocation;

			string lookup = content.Title;
			
			// Use unique name in preference to the title
			if ((content.UniqueName != null) && (content.UniqueName.Length > 0))
				lookup = content.UniqueName;

			// Remove target from collection of friends
			if (_best.Contains(lookup))
				_best.Remove(lookup);

			// Remove target from collection of associates
			if (_associates.Contains(lookup))
				_associates.Remove(lookup);
		}

		/// <summary>
		/// Perform a restore using provided docking manager.
		/// </summary>
		/// <param name="dm">Reference to source.</param>
		public override void PerformRestore(DockingManager dm)
		{
			// Grab a list of all floating forms
			Form[] owned = dm.Container.FindForm().OwnedForms;

			FloatingForm target = null;

			// Find the match to one of our best friends
			foreach(Form f in owned)
			{
				FloatingForm ff = f as FloatingForm;

				if (ff != null)
				{
					if (ZoneHelper.ContentNames(ff.Zone).Contains(_best))
					{
						target = ff;
						break;
					}
				}
			}

			// If no friends then try associates as second best option
			if (target == null)
			{
				// Find the match to one of our best friends
				foreach(Form f in owned)
				{
					FloatingForm ff = f as FloatingForm;

					if (ff != null)
					{
						if (ZoneHelper.ContentNames(ff.Zone).Contains(_associates))
						{
							target = ff;
							break;
						}
					}
				}
			}

			// If we found a friend/associate, then restore to it
			if (target != null)
			{
				// We should have a child and be able to restore to its Zone
				Child.PerformRestore(target.Zone);
			}
			else
			{
				// Restore its location/size
				Content.DisplayLocation = _location;
				Content.DisplaySize = _size;

				// Use the docking manage method to create us a new Floating Window at correct size/location
				dm.AddContentWithState(Content, State.Floating);
			}
		}

		/// <summary>
		/// Save object specific information to stream.
		/// </summary>
		/// <param name="xmlOut">Stream for loading.</param>
		public override void SaveInternalToXml(XmlTextWriter xmlOut)
		{
			base.SaveInternalToXml(xmlOut);
			xmlOut.WriteStartElement("Position");
			xmlOut.WriteAttributeString("Size", ConversionHelper.SizeToString(_size));
			xmlOut.WriteAttributeString("Location", ConversionHelper.PointToString(_location));
			xmlOut.WriteEndElement();				
			_best.SaveToXml("Best", xmlOut);
			_associates.SaveToXml("Associates", xmlOut);
		}

		/// <summary>
		/// Load object specific information from stream.
		/// </summary>
		/// <param name="xmlIn">Stream for loading.</param>
		/// <param name="formatVersion">Format version number.</param>
		public override void LoadInternalFromXml(XmlTextReader xmlIn, int formatVersion)
		{
			base.LoadInternalFromXml(xmlIn, formatVersion);

			// Move to next xml node
			if (!xmlIn.Read())
				throw new ArgumentException("Could not read in next expected node");

			// Check it has the expected name
			if (xmlIn.Name != "Position")
				throw new ArgumentException("Node 'Position' expected but not found");

			// Grab raw position information
			string attrSize = xmlIn.GetAttribute(0);
			string attrLocation = xmlIn.GetAttribute(1);

			// Convert from string to proper types
			_size = ConversionHelper.StringToSize(attrSize);
			_location = ConversionHelper.StringToPoint(attrLocation);

			_best.LoadFromXml("Best", xmlIn);
			_associates.LoadFromXml("Associates", xmlIn);
		}
	}

	/// <summary>
	/// Restore content object with zone affinity.
	/// </summary>
	public class RestoreZoneAffinity : RestoreContent
	{
		// Instance fields
		private Decimal _space;
		private StringCollection _best;
		private StringCollection _next;
		private StringCollection _previous;

		/// <summary>
		/// Initializes a new instance of the RestoreZoneAffinity class.
		/// </summary>
		public RestoreZoneAffinity()
			: base()
		{
			// Default state
			_space = 50m;

			// Must always point to valid reference
			_best = new StringCollection();
			_next = new StringCollection();
			_previous = new StringCollection();
		}

		/// <summary>
		/// Initializes a new instance of the RestoreZoneAffinity class.
		/// </summary>
		/// <param name="child">Restore object to be chained.</param>
		/// <param name="content">Reference to source content.</param>
		/// <param name="best">Collection of content names.</param>
		/// <param name="next">Collection of content names.</param>
		/// <param name="previous">Collection of content names.</param>
		public RestoreZoneAffinity(Restore child, 
								   Content content, 
								   StringCollection best,
								   StringCollection next,
								   StringCollection previous)
			: base(child, content)
		{
			// Remember parameters
			_best = best;				
			_next = next;				
			_previous = previous;	
			
			if (content.Visible)			
				_space = content.ParentWindowContent.ZoneArea;
			else
				_space = 50m;
		}

		/// <summary>
		/// Perform a restore using provided zone.
		/// </summary>
		/// <param name="z">Reference to source.</param>
		public override void PerformRestore(Zone z)
		{
			int count = z.Windows.Count;
			int beforeIndex = - 1;
			int afterIndex = count;
		
			// Find the match to one of our best friends
			for(int index=0; index<count; index++)
			{
				WindowContent wc = z.Windows[index] as WindowContent;

				if (wc != null)
				{
					// If this WindowContent contains a best friend, then add ourself here as well
					if (wc.Contents.ContainsTitleOrUnique(_best))
					{
						if (Child == null)
						{
							// If we do not have a Restore object for the Window then just add
							// into the WindowContent at the end of the existing Contents
							wc.Contents.Add(Content);
						}
						else
						{
							// Get the child to restore as best as possible inside WindowContent
							Child.PerformRestore(wc);
						}

						return;
					}

					// If the WindowContent contains a Content previous to the target
					if (wc.Contents.ContainsTitleOrUnique(_previous))
					{
						if (index > beforeIndex)
							beforeIndex = index;
					}
					
					// If the WindowContent contains a Content next to the target
					if (wc.Contents.ContainsTitleOrUnique(_next))
					{
						if (index < afterIndex)
							afterIndex = index;
					}
				}
			}

			// If we get here then we did not find any best friends, this 
			// means we need to create a new WindowContent to host the Content.
			Window newW =  z.DockingManager.CreateWindowForContent(Content);

			ZoneSequence zs = z as ZoneSequence;

			// If this is inside a ZoneSequence instance
			if (zs != null)
			{
				// Do not reposition the Windows on the .Insert but instead ignore the
				// reposition and let it happen in the .ModifyWindowSpace. This reduces
				// the flicker that would occur otherwise
				zs.SuppressReposition();
			}

			// Need to find the best place in the order for the Content, start by
			// looking for the last 'previous' content and place immediately after it
			if (beforeIndex >= 0)
			{
				// Great, insert after it
				z.Windows.Insert(beforeIndex + 1, newW);
			}
			else
			{
				// No joy, so find the first 'next' content and place just before it, if
				// none are found then just add to the end of the collection.
				z.Windows.Insert(afterIndex, newW);
			}

			// If this is inside a ZoneSequence instance
			if (zs != null)
			{
				// We want to try and allocate the correct Zone space
				zs.ModifyWindowSpace(newW, _space);
			}
		}

		/// <summary>
		/// Save object specific information to stream.
		/// </summary>
		/// <param name="xmlOut">Stream for loading.</param>
		public override void SaveInternalToXml(XmlTextWriter xmlOut)
		{
			base.SaveInternalToXml(xmlOut);
			xmlOut.WriteStartElement("Space");
			xmlOut.WriteAttributeString("Value", ConversionHelper.DecimalToString(_space));
			xmlOut.WriteEndElement();				
			_best.SaveToXml("Best", xmlOut);
			_next.SaveToXml("Next", xmlOut);
			_previous.SaveToXml("Previous", xmlOut);
		}

		/// <summary>
		/// Load object specific information from stream.
		/// </summary>
		/// <param name="xmlIn">Stream for loading.</param>
		/// <param name="formatVersion">Format version number.</param>
		public override void LoadInternalFromXml(XmlTextReader xmlIn, int formatVersion)
		{
			base.LoadInternalFromXml(xmlIn, formatVersion);

			// Move to next xml node
			if (!xmlIn.Read())
				throw new ArgumentException("Could not read in next expected node");

			// Check it has the expected name
			if (xmlIn.Name != "Space")
				throw new ArgumentException("Node 'Space' expected but not found");

			// Grab raw position information
			string attrSpace = xmlIn.GetAttribute(0);

			// Convert from string to proper type
			_space = ConversionHelper.StringToDecimal(attrSpace);

			_best.LoadFromXml("Best", xmlIn);
			_next.LoadFromXml("Next", xmlIn);
			_previous.LoadFromXml("Previous", xmlIn);
		}
	}

	/// <summary>
	/// Restore content with window affinity.
	/// </summary>
	public class RestoreWindowContent : RestoreContent
	{
		// Instance fields
		private bool _selected;
		private StringCollection _next;
		private StringCollection _previous;

		/// <summary>
		/// Initializes a new instance of the RestoreWindowContent class.
		/// </summary>
		public RestoreWindowContent()
			: base()
		{
			// Must always point to valid reference
			_selected = false;
			_next = new StringCollection();
			_previous = new StringCollection();
		}

		/// <summary>
		/// Initializes a new instance of the RestoreWindowContent class.
		/// </summary>
		/// <param name="child">Restore object to be chained.</param>
		/// <param name="content">Reference to source content.</param>
		/// <param name="next">Collection of content names.</param>
		/// <param name="previous">Collection of content names.</param>
		/// <param name="selected">Was content selected.</param>
		public RestoreWindowContent(Restore child, 
									Content content, 
									StringCollection next, 
									StringCollection previous,
									bool selected)
			: base(child, content)
		{
			// Remember parameters
            _selected = selected;
            _next = next;
			_previous = previous;
		}

		/// <summary>
		/// Perform a restore using provided window.
		/// </summary>
		/// <param name="w">Reference to source.</param>
		public override void PerformRestore(Window w)
		{
			// We are only ever called for a WindowContent object
			WindowContent wc = w as WindowContent;

			int bestIndex = -1;

			foreach(String s in _previous)
			{
				if (wc.Contents.ContainsTitleOrUnique(s))
				{
					Content c = wc.Contents[s];
					
					// Check the unqiue name as well as the title
					if (c == null)
						c = wc.Contents.FindUniqueName(s);
				
					int previousIndex = wc.Contents.IndexOf(c);

					if (previousIndex > bestIndex)
						bestIndex = previousIndex;
				}
			}

			// Did we find a previous Content?
			if (bestIndex >= 0)
			{
				// Great, insert after it
				wc.Contents.Insert(bestIndex + 1, Content);
			}
			else
			{
				bestIndex = wc.Contents.Count;

				foreach(String s in _next)
				{
					if (wc.Contents.ContainsTitleOrUnique(s))
					{
						Content c = wc.Contents[s];
					
						// Check the unqiue name as well as the title
						if (c == null)
							c = wc.Contents.FindUniqueName(s);

						int nextIndex = wc.Contents.IndexOf(c);

						if (nextIndex < bestIndex)
							bestIndex = nextIndex;
					}
				}

				// Insert before the found entry (or at end if non found)
				wc.Contents.Insert(bestIndex, Content);
			}
			
			// Should this content become selected?
			if (_selected)
			    Content.BringToFront();
		}

		/// <summary>
		/// Save object specific information to stream.
		/// </summary>
		/// <param name="xmlOut">Stream for loading.</param>
		public override void SaveInternalToXml(XmlTextWriter xmlOut)
		{
			base.SaveInternalToXml(xmlOut);
			_next.SaveToXml("Next", xmlOut);
			_previous.SaveToXml("Previous", xmlOut);
        
            xmlOut.WriteStartElement("Selected");
            xmlOut.WriteAttributeString("Value", _selected.ToString());
            xmlOut.WriteEndElement();				
        }

		/// <summary>
		/// Load object specific information from stream.
		/// </summary>
		/// <param name="xmlIn">Stream for loading.</param>
		/// <param name="formatVersion">Format version number.</param>
		public override void LoadInternalFromXml(XmlTextReader xmlIn, int formatVersion)
		{
			base.LoadInternalFromXml(xmlIn, formatVersion);
			_next.LoadFromXml("Next", xmlIn);
			_previous.LoadFromXml("Previous", xmlIn);
        
            // _selected added in version 4 format
            if (formatVersion >= 4)
            {
                // Move to next xml node
                if (!xmlIn.Read())
                    throw new ArgumentException("Could not read in next expected node");

                // Check it has the expected name
                if (xmlIn.Name != "Selected")
                    throw new ArgumentException("Node 'Selected' expected but not found");

                // Convert attribute value to boolean value
                _selected = Convert.ToBoolean(xmlIn.GetAttribute(0));
            }
        }
	}
}
