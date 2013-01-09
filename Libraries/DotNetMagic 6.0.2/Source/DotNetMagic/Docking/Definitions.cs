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
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Docking
{
	/// <summary>
	/// Specifies the current docking state of a content.
	/// </summary>
    public enum State
    {
		/// <summary>
		/// Specifies the content is floating.
		/// </summary>
        Floating,

		/// <summary>
		/// Specifies the content is docked against top edge.
		/// </summary>
        DockTop,

		/// <summary>
		/// Specifies the content is docked against bottom edge.
		/// </summary>
        DockBottom,

		/// <summary>
		/// Specifies the content is docked against left edge.
		/// </summary>
        DockLeft,

		/// <summary>
		/// Specifies the content is docked against right edge.
		/// </summary>
        DockRight
    }

	/// <summary>
	/// Interface exposed by a hot zone source.
	/// </summary>
    internal interface IHotZoneSource
    {
		/// <summary>
		/// Add the collection of hot zones.
		/// </summary>
		/// <param name="redock">Reference to a redocker instance.</param>
		/// <param name="collection">Collection of hot zones.</param>
        void AddHotZones(Redocker redock, HotZoneCollection collection);
    }

	/// <summary>
	/// Interface exposed by zone allowing maximizing of windows.
	/// </summary>
    public interface IZoneMaximizeWindow
    {
		/// <summary>
		/// Gets the direction of the zone.
		/// </summary>
        LayoutDirection Direction { get; }

		/// <summary>
		/// Is a window allowed to become maximized.
		/// </summary>
		/// <returns>Window allows to become maximized.</returns>
        bool IsMaximizeAvailable();

		/// <summary>
		/// Is the provided window the currently maximized one.
		/// </summary>
		/// <param name="w">Window to be checked.</param>
		/// <returns>true is maximized; false otherwise.</returns>
        bool IsWindowMaximized(Window w);

		/// <summary>
		/// Make the provided window the maximized one.
		/// </summary>
		/// <param name="w">Window to become maximized.</param>
        void MaximizeWindow(Window w);
		
		/// <summary>
		/// Remove any maximized window.
		/// </summary>
        void RestoreWindow();

		/// <summary>
		/// Occurs when a change in maximized state occurs.
		/// </summary>
        event EventHandler RefreshMaximize;
    }

    /// <summary>
    /// Represents the method that will handle a context menu request.
    /// </summary>
    public delegate void ContextHandler(ContentCollection cc, Point screenPos);
}
