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
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Docking
{
    /// <summary>
    /// Creates the appropriate control or form for the docking system.
    /// </summary>
    public class DockingFactory
    {
		/// <summary>
		/// Initializes a new instance of the FloatingForm class.
		/// </summary>
		/// <param name="dockingManager">Parent docking manager instance.</param>
		/// <param name="zone">Zone to be hosted.</param>
		/// <param name="contextHandler">Delegate for handling context menus.</param>
        public virtual FloatingForm CreateFloatingForm(DockingManager dockingManager, Zone zone, ContextHandler contextHandler)
        {
            return new FloatingForm(dockingManager, zone, contextHandler);
        }

		/// <summary>
		/// Initializes a new instance of the ZoneSequence class.
		/// </summary>
		/// <param name="manager">Reference to a docking manager.</param>
		/// <param name="state">Initial state of the zone.</param>
		/// <param name="style">Visual style for drawing.</param>
		/// <param name="direction">Direction to arrange child windows.</param>
		/// <param name="zoneMinMax">Allowed to minimize and maximize windows.</param>
        public virtual ZoneSequence CreateZoneSequence(DockingManager manager, State state, VisualStyle style, LayoutDirection direction, bool zoneMinMax)
        {
            return new ZoneSequence(manager, state, style, direction, zoneMinMax);
        }

		/// <summary>
		/// Initializes a new instance of the WindowContent class.
		/// </summary>
		/// <param name="manager">Parent docking manager instance.</param>
		/// <param name="vs">Visual style for drawing.</param>
        public virtual WindowContentTabbed CreateWindowContentTabbed(DockingManager manager, VisualStyle vs)
        {
            return new WindowContentTabbed(manager, vs);
        }

		/// <summary>
		/// Initializes a new instance of the WindowDetailCaptionPlain class.
		/// </summary>
		/// <param name="manager">Reference to parent docking manager.</param>
        /// <param name="wc">Owning window content instance.</param>
        /// <param name="closeHandler">Delegate for notifying close events.</param>
		/// <param name="restoreHandler">Delegate for notifying restore events.</param>
		/// <param name="invertAutoHideHandler">Delegate for auto hide invert events.</param>
		/// <param name="contextHandler">Delegate for notifying context menu events.</param>
        public virtual WindowDetailCaptionPlain CreateWindowDetailCaptionPlain(DockingManager manager,
                                                                               WindowContent wc,
                                                                               EventHandler closeHandler, 
                                                                               EventHandler restoreHandler, 
                                                                               EventHandler invertAutoHideHandler, 
                                                                               ContextHandler contextHandler)

        {
            return new WindowDetailCaptionPlain(manager, wc, closeHandler, restoreHandler, invertAutoHideHandler, contextHandler);
        }

		/// <summary>
		/// Initializes a new instance of the WindowDetailCaptionIDE2005 class.
		/// </summary>
		/// <param name="manager">Reference to parent docking manager.</param>
        /// <param name="wc">Owning window content instance.</param>
        /// <param name="closeHandler">Delegate for notifying close events.</param>
		/// <param name="restoreHandler">Delegate for notifying restore events.</param>
		/// <param name="invertAutoHideHandler">Delegate for auto hide invert events.</param>
		/// <param name="contextHandler">Delegate for notifying context menu events.</param>
        public virtual WindowDetailCaptionIDE2005 CreateWindowDetailCaptionIDE2005(DockingManager manager,
                                                                                   WindowContent wc,
										                                           EventHandler closeHandler, 
										                                           EventHandler restoreHandler, 
										                                           EventHandler invertAutoHideHandler, 
										                                           ContextHandler contextHandler)
        {
            return new WindowDetailCaptionIDE2005(manager, wc, closeHandler, restoreHandler, invertAutoHideHandler, contextHandler);
        }


		/// <summary>
		/// Initializes a new instance of the WindowDetailCaptionOffice2003 class.
		/// </summary>
		/// <param name="manager">Reference to parent docking manager.</param>
        /// <param name="wc">Owning window content instance.</param>
        /// <param name="closeHandler">Delegate for notifying close events.</param>
		/// <param name="restoreHandler">Delegate for notifying restore events.</param>
		/// <param name="invertAutoHideHandler">Delegate for auto hide invert events.</param>
		/// <param name="contextHandler">Delegate for notifying context menu events.</param>
        public virtual WindowDetailCaptionOffice2003 CreateWindowDetailCaptionOffice2003(DockingManager manager,
                                                                                         WindowContent wc,
                                                                                         EventHandler closeHandler,
                                                                                         EventHandler restoreHandler,
                                                                                         EventHandler invertAutoHideHandler,
                                                                                         ContextHandler contextHandler)
        {
            return new WindowDetailCaptionOffice2003(manager, wc, closeHandler, restoreHandler, invertAutoHideHandler, contextHandler);
        }

        /// <summary>
        /// Initializes a new instance of the WindowDetailCaptionOffice2007 class.
        /// </summary>
        /// <param name="manager">Reference to parent docking manager.</param>
        /// <param name="wc">Owning window content instance.</param>
        /// <param name="closeHandler">Delegate for notifying close events.</param>
        /// <param name="restoreHandler">Delegate for notifying restore events.</param>
        /// <param name="invertAutoHideHandler">Delegate for auto hide invert events.</param>
        /// <param name="contextHandler">Delegate for notifying context menu events.</param>
        public virtual WindowDetailCaptionOffice2007 CreateWindowDetailCaptionOffice2007(DockingManager manager,
                                                                                         WindowContent wc,
                                                                                         EventHandler closeHandler,
                                                                                         EventHandler restoreHandler,
                                                                                         EventHandler invertAutoHideHandler,
                                                                                         ContextHandler contextHandler)
        {
            return new WindowDetailCaptionOffice2007(manager, wc, closeHandler, restoreHandler, invertAutoHideHandler, contextHandler);
        }

        /// <summary>
        /// Initializes a new instance of the WindowDetailCaptionMediaPlayer class.
        /// </summary>
        /// <param name="manager">Reference to parent docking manager.</param>
        /// <param name="wc">Owning window content instance.</param>
        /// <param name="closeHandler">Delegate for notifying close events.</param>
        /// <param name="restoreHandler">Delegate for notifying restore events.</param>
        /// <param name="invertAutoHideHandler">Delegate for auto hide invert events.</param>
        /// <param name="contextHandler">Delegate for notifying context menu events.</param>
        public virtual WindowDetailCaptionMediaPlayer CreateWindowDetailCaptionMediaPlayer(DockingManager manager,
                                                                                           WindowContent wc,
                                                                                           EventHandler closeHandler,
                                                                                           EventHandler restoreHandler,
                                                                                           EventHandler invertAutoHideHandler,
                                                                                           ContextHandler contextHandler)
        {
            return new WindowDetailCaptionMediaPlayer(manager, wc, closeHandler, restoreHandler, invertAutoHideHandler, contextHandler);
        }

        /// <summary>
        /// Initialize a new instance of the Office2007Button class.
        /// </summary>
        /// <param name="caption"></param>
        public virtual Office2007CaptionButton CreateOffice2007CaptionButton(WindowDetailCaptionOffice2007 caption)
        {
            return new Office2007CaptionButton(caption);
        }

        /// <summary>
        /// Initialize a new instance of the MediaPlayerButton class.
        /// </summary>
        /// <param name="caption"></param>
        public virtual MediaPlayerCaptionButton CreateMediaPlayerCaptionButton(WindowDetailCaptionMediaPlayer caption)
        {
            return new MediaPlayerCaptionButton(caption);
        }

        /// <summary>
		/// Initialize a new instance of the Office2003Button class.
		/// </summary>
		/// <param name="caption"></param>
		public virtual Office2003CaptionButton CreateOffice2003CaptionButton(WindowDetailCaptionOffice2003 caption)
        {
            return new Office2003CaptionButton(caption);
        }

        /// <summary>
		/// Initialize a new instance of the IDE2005Button class.
		/// </summary>
		/// <param name="caption"></param>
        public virtual IDE2005CaptionButton CreateIDE2005CaptionButton(WindowDetailCaptionIDE2005 caption)
        {
            return new IDE2005CaptionButton(caption);
        }

        /// <summary>
		/// Initializes a new instance of the AutoHidePanel class.
		/// </summary>
		/// <param name="manager">Parent docking manager.</param>
		/// <param name="dockEdge">Edge we are docked against.</param>
        public virtual AutoHidePanel CreateAutoHidePanel(DockingManager manager, DockStyle dockEdge)
        {
            return new AutoHidePanel(manager, dockEdge);
        }

		/// <summary>
		/// Initializes a new instance of the AutoHostPanel class.
		/// </summary>
		/// <param name="manager">Parent docking manager.</param>
		/// <param name="autoHidePanel">Parent auto hide panel.</param>
		/// <param name="borderEdge">Edge we are docked against.</param>
        public virtual AutoHidePanel.AutoHostPanel CreateAutoHostPanel(DockingManager manager, AutoHidePanel autoHidePanel, Edge borderEdge)
        {
            return new AutoHidePanel.AutoHostPanel(manager, autoHidePanel, borderEdge);
        }

        /// <summary>
		/// Initializes a new instance of the TabStub class.
		/// </summary>
		/// <param name="style">Visual style for drawing.</param>
		/// <param name="stubsShowAll">Initial stubs value.</param>
        public virtual TabStub CreateTabStub(VisualStyle style, bool stubsShowAll)
        {
            return new TabStub(style, stubsShowAll);
        }
    }
}
