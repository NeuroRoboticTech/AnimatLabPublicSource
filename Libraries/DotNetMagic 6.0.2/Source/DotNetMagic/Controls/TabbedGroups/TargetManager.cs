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
	/// Manage all targets used during a drag and drop operation.
	/// </summary>
	public abstract class TargetManager
	{
	    // Class fields
        private static Cursor _validCursor;
        private static Cursor _invalidCursor;
	
	    // Instance fields
		private bool _squares;
		private Target _lastTarget;
		private TabbedGroups _host;
        private TabGroupLeaf _leaf;
	    private Controls.TabControl _source;
	    
		/// <summary>
	    /// Initializes a static fields of the TargetManager class.
	    /// </summary>
	    static TargetManager()
	    {
            _validCursor = ResourceHelper.LoadCursor(Type.GetType("Crownwood.DotNetMagic.Controls.TabbedGroups"),
                                                                  "Crownwood.DotNetMagic.Resources.TabbedValid.cur");

            _invalidCursor = ResourceHelper.LoadCursor(Type.GetType("Crownwood.DotNetMagic.Controls.TabbedGroups"),
                                                                    "Crownwood.DotNetMagic.Resources.TabbedInvalid.cur");
        }
	    
		/// <summary>
		/// Initializes a new instance of the TargetManager class.
		/// </summary>
		/// <param name="squares">Show as squares or diamonds.</param>
		/// <param name="host">Control that is requesting target management.</param>
		/// <param name="leaf">Source leaf causing drag operation.</param>
		/// <param name="source">Source TabControl of drag operation.</param>
	    public TargetManager(bool squares,
							 TabbedGroups host, 
							 TabGroupLeaf leaf, 
							 Controls.TabControl source)
	    {
	        // Define state
	        _squares = squares;
	        _host = host;
	        _leaf = leaf;
	        _source = source;
			_lastTarget = null;
	    
	        // Setup targets
	        Initialize();
		}

		/// <summary>
		/// Gets the squares value.
		/// </summary>
		public bool Squares
		{
			get { return _squares; }
		}
				
		/// <summary>
		/// Gets the associated hosting control.
		/// </summary>
		public TabbedGroups Host
		{
			get { return _host; }
		}
		
		/// <summary>
		/// Gets the associated leaf instance.
		/// </summary>
		public TabGroupLeaf Leaf
		{
			get { return _leaf; }
		}

		/// <summary>
		/// Gets the associated source.
		/// </summary>
		public Controls.TabControl Source
		{
			get { return _source; }
		}

		/// <summary>
		/// Update display indication for new mouse position.
		/// </summary>
		/// <param name="mousePos">New mouse position.</param>
		public virtual void MouseMove(Point mousePos)
		{
			// Find the Target the mouse is currently over (if any)
			Target t = FindTarget(mousePos);
	    
			// Set appropriate cursor
			if (t != null)
				Source.Cursor = _validCursor;
			else
				Source.Cursor = _invalidCursor;
                
			if (t != _lastTarget)
			{
				// Update drag feedback for the new target
				UpdateFeedbackForTarget(t);
                
				// Remember for next time around
				_lastTarget = t;
			}
		}
        
		/// <summary>
		/// Finish drag operation and perform required action.
		/// </summary>
		public virtual void Exit()
		{
			// Remove any showing indicator
			Quit();

			if (_lastTarget != null)
			{
				// Perform action specific operation
				switch(_lastTarget.Action)
				{
					case Target.TargetActions.Transfer:
						// Transfer selecte page from source to destination
						Leaf.MovePageToLeaf(_lastTarget.Leaf);
						break;
					case Target.TargetActions.GroupLeft:                        
						_lastTarget.Leaf.NewHorizontalGroup(Leaf, true);
						break;
					case Target.TargetActions.GroupRight:
						_lastTarget.Leaf.NewHorizontalGroup(Leaf, false);
						break;
					case Target.TargetActions.GroupTop:
						_lastTarget.Leaf.NewVerticalGroup(Leaf, true);
						break;
					case Target.TargetActions.GroupBottom:
						_lastTarget.Leaf.NewVerticalGroup(Leaf, false);
						break;
					case Target.TargetActions.ControlLeft:
						Leaf.ControlHorizontalGroup(true);
						break;
					case Target.TargetActions.ControlRight:
						Leaf.ControlHorizontalGroup(false);
						break;
					case Target.TargetActions.ControlTop:
						Leaf.ControlVerticalGroup(true);
						break;
					case Target.TargetActions.ControlBottom:
						Leaf.ControlVerticalGroup(false);
						break;
				}

				// Layout controls properly
				Host.RootSequence.Reposition();
			}
		}
            
		/// <summary>
		/// Remove any visual indications.
		/// </summary>
		public abstract void Quit();
		
		/// <summary>
		/// Class specific initialization.
		/// </summary>
		protected abstract void Initialize();
		
		/// <summary>
		/// Gets the target associated with a mouse location.
		/// </summary>
		/// <param name="mousePos">Mouse position.</param>
		/// <returns>Target instance; otherwise null.</returns>
		protected abstract Target FindTarget(Point mousePos);
		
		/// <summary>
		/// Update feedback for the new target.
		/// </summary>
		/// <param name="t">Target instance.</param>
		protected abstract void UpdateFeedbackForTarget(Target t);
	}
}
