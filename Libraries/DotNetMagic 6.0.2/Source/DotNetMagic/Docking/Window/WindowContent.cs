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
using System.Windows.Forms;
using System.ComponentModel;
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Docking
{
	/// <summary>
	/// Base class for a Window that has a collection of contents.
	/// </summary>
    [ToolboxItem(false)]
    public abstract class WindowContent : Window
    {
        // Instance fields
        private ContentCollection _contents;
        private VisualStyle _style;

		/// <summary>
		/// Initializes a new instance of the WindowContent class.
		/// </summary>
		/// <param name="manager">Parent docking manager instance.</param>
		/// <param name="vs">Visual style for drawing.</param>
        public WindowContent(DockingManager manager, VisualStyle vs)
            : base(manager)
        {
            // Remember state
            _style = vs;
        
            // Create collection of window details
            _contents = new ContentCollection();

            // We want notification when contents are added/removed/cleared
            _contents.Clearing += new CollectionClear(OnContentsClearing);
            _contents.Inserted += new CollectionChange(OnContentInserted);
            _contents.Removing += new CollectionChange(OnContentRemoving);
            _contents.Removed += new CollectionChange(OnContentRemoved);
        }

		/// <summary>
		/// Gets and sets the collection of managed content instances.
		/// </summary>
        public ContentCollection Contents
        {
            get { return _contents; }
			
            set
            {
                _contents.Clear();
                _contents = value;
            }
        }

		/// <summary>
		/// Gets the visual style for drawing.
		/// </summary>
		public VisualStyle Style
		{
			get { return _style; }
		}

		/// <summary>
		/// Bring the specified content to the foreground of the window.
		/// </summary>
		/// <param name="c">Content to bring to foreground.</param>
		public virtual void BringContentToFront(Content c) {}

		/// <summary>
		/// Gets the content which is currently active and so in the foreground.
		/// </summary>
		public abstract Content CurrentContent { get; }

		/// <summary>
		/// Process the removing of all contents.
		/// </summary>
        protected virtual void OnContentsClearing()
        {
            foreach(Content c in _contents)
            {
                // Inform content of new parent content window
                c.ParentWindowContent = null;

                // Unhook from property change notification
                c.PropertyChanged -= new Content.PropChangeHandler(OnContentChanged);
            }

            // Should we kill ourself?
            if (this.AutoDispose)
                Suicide();
        }

		/// <summary>
		/// Process the inserting of a new content.
		/// </summary>
		/// <param name="index">Position of new content.</param>
		/// <param name="value">New content instance.</param>
        protected virtual void OnContentInserted(int index, object value)
        {
            Content content = value as Content;

            // Is this the first Content added?
            if (_contents.Count == 1)
            {
                // Use size of the Content to determine our size
                this.Size = content.DisplaySize;
            }

            // Inform content where it now resides
            content.ParentWindowContent = this;

            // Monitor changes in Content properties
            content.PropertyChanged += new Content.PropChangeHandler(OnContentChanged);
        }

		/// <summary>
		/// Process just before a content is removed.
		/// </summary>
		/// <param name="index">Index to be removed.</param>
		/// <param name="value">Content to be removed.</param>
        protected virtual void OnContentRemoving(int index, object value)
        {
            Content content = value as Content;

            // Inform content of new parent content window
            content.ParentWindowContent = null;

            // Unhook from monitoring changes in Content properties
            content.PropertyChanged -= new Content.PropChangeHandler(OnContentChanged);
        }

		/// <summary>
		/// Process just after a content has been removed.
		/// </summary>
		/// <param name="index">Index to be removed.</param>
		/// <param name="value">Content to be removed.</param>
        protected virtual void OnContentRemoved(int index, object value)
        {
            // Removed the last entry?
            if (_contents.Count == 0)
            {
                // Should we kill ourself?
                if (this.AutoDispose)
                    Suicide();
            }
        }

		/// <summary>
		/// Process a change in property of a contained content.
		/// </summary>
		/// <param name="obj">Content that has a changed property.</param>
		/// <param name="prop">The property that has changed.</param>
        protected virtual void OnContentChanged(Content obj, Content.Property prop) {}

		/// <summary>
		/// Graceful shutdown of the instance.
		/// </summary>
        protected void Suicide()
        {
            // Are we inside a Zone object?
            if (this.ParentZone != null)
                this.ParentZone.Windows.Remove(this);

            // Remover monitoring of events
            _contents.Clearing -= new CollectionClear(OnContentsClearing);
            _contents.Inserted -= new CollectionChange(OnContentInserted);
            _contents.Removing -= new CollectionChange(OnContentRemoving);
            _contents.Removed -= new CollectionChange(OnContentRemoved);

            this.Dispose();
        }
    }
}
