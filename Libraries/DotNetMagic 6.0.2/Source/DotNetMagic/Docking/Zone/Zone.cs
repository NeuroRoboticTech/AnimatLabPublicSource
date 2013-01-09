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
	/// Base class for implementation of zones.
	/// </summary>
    [ToolboxItem(false)]
    public class Zone : Panel
    {
        // Instance fields
        private State _state;
        private bool _autoDispose;
        private DockingManager _manager;
        private WindowCollection _windows;

		/// <summary>
		/// Initializes a new instance of the Zone class.
		/// </summary>
		/// <param name="manager">Reference to docking manager.</param>
        public Zone(DockingManager manager)
        {
            InternalConstruct(manager, State.DockLeft);
        }

		/// <summary>
		/// Initializes a new instance of the Zone class.
		/// </summary>
		/// <param name="manager">Reference to docking manager.</param>
		/// <param name="state">Initial state of the zone.</param>
        public Zone(DockingManager manager, State state)
        {
            InternalConstruct(manager, state);
        }

        private void InternalConstruct(DockingManager manager, State state)
        {
            // Must provide a valid manager instance
            if (manager == null)
                throw new ArgumentNullException("DockingManager");

            // Remember initial state
            _state = state;
            _manager = manager;
            _autoDispose = true;

            // Get correct starting state from manager
            this.BackColor = _manager.BackColor;
            this.ForeColor = _manager.InactiveTextColor;

            // Create collection of windows
            _windows = new WindowCollection();

            // We want notification when contents are added/removed/cleared
            _windows.Clearing += new CollectionClear(OnWindowsClearing);
            _windows.Inserted += new CollectionChange(OnWindowInserted);
            _windows.Removing += new CollectionChange(OnWindowRemoving);
            _windows.Removed += new CollectionChange(OnWindowRemoved);
        }

		/// <summary>
		/// Gets and sets the zone state.
		/// </summary>
        public virtual State State
        {
            get { return _state; }
            set { _state = value; }
        }

		/// <summary>
		/// Gets and sets a value indicating the zone will auto dispose of itself.
		/// </summary>
        public bool AutoDispose
        {
            get { return _autoDispose; }
            set { _autoDispose = value; }
        }

		/// <summary>
		/// Gets a reference to the docking manager.
		/// </summary>
        public DockingManager DockingManager
        {
            get { return _manager; }
        }

		/// <summary>
		/// Gets and sets the collection of windows.
		/// </summary>
        public WindowCollection Windows
        {
            get { return _windows; }

            set
            {
                _windows.Clear();
                _windows = value;
            }
        }

        /// <summary>
        /// Force the repositioning of the contents in this zone.
        /// </summary>
        public virtual void RelayoutContents()
        {
        }

		/// <summary>
		/// Create a restore object for this zone.
		/// </summary>
		/// <param name="w">Window of interest.</param>
		/// <param name="child">Child object to attach to restore instance.</param>
		/// <param name="childRestore">Child restore instance to attach.</param>
		/// <returns></returns>
		public virtual Restore RecordRestore(Window w, 
											 object child, 
											 Restore childRestore) 
		{ 
			return null; 
		}
		
		/// <summary>
		/// Gets the minimum width needed for the zone.
		/// </summary>
		public virtual int MinimumWidth 
		{ 
			get { return 0; } 
		}
		
		/// <summary>
		/// Gets the minimum height needed for the zone.
		/// </summary>
		public virtual int MinimumHeight 
		{ 
			get { return 0; } 
		}

		/// <summary>
		/// Propogate a change in a framework property.
		/// </summary>
		/// <param name="name">Name of property changed.</param>
		/// <param name="value">New value of property.</param>
        public virtual void PropogateNameValue(PropogateName name, object value)
        {
            if (name == PropogateName.BackColor)
            {
                this.BackColor = (Color)value;
                Invalidate();
            }

            // Pass onto each of our child Windows
            foreach(Window w in _windows)
                w.PropogateNameValue(name, value);
        }

		/// <summary>
		/// Process the removal of all child windows.
		/// </summary>
        protected virtual void OnWindowsClearing()
        {
            foreach(Control c in Controls)
            {
                Window w = c as Window;

                // Remove back pointers from Windows to this Zone
                if (w != null)
                    w.ParentZone = null;
            }

            // Should we kill ourself?
            if (this.AutoDispose)
            {
                // Remove notification when contents are added/removed/cleared
                _windows.Clearing -= new CollectionClear(OnWindowsClearing);
                _windows.Inserted -= new CollectionChange(OnWindowInserted);
                _windows.Removing -= new CollectionChange(OnWindowRemoving);
                _windows.Removed -= new CollectionChange(OnWindowRemoved);

                this.Dispose();
            }
        }

		/// <summary>
		/// Process the addition of a new window instance.
		/// </summary>
		/// <param name="index">Position where inserted.</param>
		/// <param name="value">Reference to new instance.</param>
        protected virtual void OnWindowInserted(int index, object value)
        {
            Window w = value as Window;

            // Define back pointer from Window to its new Zone
            w.ParentZone = this;

            // Define the State the Window should adopt
            w.State = _state;
        }

		/// <summary>
		/// Process just before window instance is removed.
		/// </summary>
		/// <param name="index">Position where inserted.</param>
		/// <param name="value">Reference to new instance.</param>
        protected virtual void OnWindowRemoving(int index, object value)
        {
            Window w = value as Window;

            // Remove back pointer from Window to this Zone
            w.ParentZone = null;
        }

		/// <summary>
		/// Process just after window instance is removed.
		/// </summary>
		/// <param name="index">Position where inserted.</param>
		/// <param name="value">Reference to new instance.</param>
        protected virtual void OnWindowRemoved(int index, object value)
        {
            // Removed the last entry?
            if (_windows.Count == 0)
            {
                // Should we kill ourself?
                if (this.AutoDispose)
                    this.Dispose();
            }
        }
    }
}
