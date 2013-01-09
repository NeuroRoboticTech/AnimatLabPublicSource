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
using System.Windows.Forms;

namespace Crownwood.DotNetMagic.Controls
{
    /// <summary>
    /// Provides data for the TabControlType event.
    /// </summary>
    public class TGTabControlType
    {
        // Instance fields
        private Type _tabControlType;

        /// <summary>
        /// Initialize a new instance of the TGTabControlType.
        /// </summary>
        public TGTabControlType()
        {
            // Always default to the DotNetMagic TabControl
            _tabControlType = typeof(Controls.TabControl);
        }

        /// <summary>
        /// Gets or sets the TabControlType property for the event.
        /// </summary>
        public Type TabControlType
        {
            get { return _tabControlType; }
            
            set 
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                if (!value.IsSubclassOf(typeof(Controls.TabControl)))
                    throw new ApplicationException("Type must derive from the DotNetMagic TabControl");

                _tabControlType = value;
            }
        }
    }

	/// <summary>
	/// Provides data for the PageCloseRequest event.
	/// </summary>
    public class TGCloseRequestEventArgs
	{
		// Instance fields
	    private TabGroupLeaf _tgl;
	    private Controls.TabControl _tc;
	    private Controls.TabPage _tp;
	    private bool _cancel;
	
		/// <summary>
		/// Initializes a new instance of the TGCloseRequestEventArgs class.
		/// </summary>
		/// <param name="tgl">Source TabGroupLeaf for event.</param>
		/// <param name="tc">Source TabControl for the event.</param>
		/// <param name="tp">Source TabPage for the event.</param>
		public TGCloseRequestEventArgs(TabGroupLeaf tgl, 
									   Controls.TabControl tc, 
									   Controls.TabPage tp)
		{
		    // Definie initial state
		    _tgl = tgl;
		    _tc = tc;
		    _tp = tp;
		    _cancel = false;
		}
		
		/// <summary>
		/// Gets the source TabGroupLeaf for the event.
		/// </summary>
		public TabGroupLeaf Leaf
		{
		    get { return _tgl; }
		}
    
		/// <summary>
		/// Gets the source TabControl for the event.
		/// </summary>
        public Controls.TabControl TabControl
        {
            get { return _tc; }
        }

		/// <summary>
		/// Gets the source TabPage for the event.
		/// </summary>
        public Controls.TabPage TabPage
        {
            get { return _tp; }
        }
        
		/// <summary>
		/// Gets or sets the Cancel property for the event.
		/// </summary>
        public bool Cancel
        {
            get { return _cancel; }
            set { _cancel = value; }
        }
    }

	/// <summary>
	/// Provides data for the PageContextMenu event.
	/// </summary>
    public class TGContextMenuEventArgs : TGCloseRequestEventArgs
    {
		// Instance fields
        private ContextMenuStrip _contextMenuStrip;
	
		/// <summary>
		/// Initializes a new instance of the TGContextMenuEventArgs class.
		/// </summary>
		/// <param name="tgl">Source TabGroupLeaf for event.</param>
		/// <param name="tc">Source TabControl for event.</param>
		/// <param name="tp">Source TabPage for event.</param>
        /// <param name="contextMenuStrip">Source ContextMenuStrip for event.</param>
        public TGContextMenuEventArgs(TabGroupLeaf tgl, 
									  Controls.TabControl tc, 
                                      Controls.TabPage tp,
                                      ContextMenuStrip contextMenuStrip)
            : base(tgl, tc, tp)
        {
            // Definie initial state
            _contextMenuStrip = contextMenuStrip;
        }
		
		/// <summary>
		/// Gets the PopupMenu about to be displayed.
		/// </summary>
        public ContextMenuStrip ContextMenuStrip
        {
            get { return _contextMenuStrip; }
        }    
    }
    
	/// <summary>
	/// Provides data for the PageLoading event.
	/// </summary>
    public class TGPageLoadingEventArgs
    {
		// Instance fields
        private Controls.TabPage _tp;
        private XmlTextReader _xmlIn;
        private bool _cancel;
        
		/// <summary>
		/// Initializes a new instance of the TGPageLoadingEventArgs class.
		/// </summary>
		/// <param name="tp">Source TabPage for event.</param>
		/// <param name="xmlIn">Source XmlTextReader for event.</param>
        public TGPageLoadingEventArgs(Controls.TabPage tp, XmlTextReader xmlIn)
        {
            // Definie initial state
            _tp = tp;
            _xmlIn = xmlIn;
            _cancel = false;
        }
        
		/// <summary>
		/// Gets the source TabPage for the event.
		/// </summary>
        public Controls.TabPage TabPage
        {
            get { return _tp; }
        }
        
		/// <summary>
		/// Gets the source XmlTextReader for the event.
		/// </summary>
        public XmlTextReader XmlIn
        {
            get { return _xmlIn; }
        }
        
		/// <summary>
		/// Gets or sets the Cancel property for the event.
		/// </summary>
        public bool Cancel
        {
            get { return _cancel; }
            set { _cancel = value; }
        }
    }    

	/// <summary>
	/// Provides data for the PageSaving event.
	/// </summary>
    public class TGPageSavingEventArgs
    {
		// Instance fields
        private Controls.TabPage _tp;
        private XmlTextWriter _xmlOut;
        
		/// <summary>
		/// Initializes a new instance of the TGPageSavingEventArgs class.
		/// </summary>
		/// <param name="tp">Source TabPage for event.</param>
		/// <param name="xmlOut">Source XmlTextWriter for event.</param>
        public TGPageSavingEventArgs(Controls.TabPage tp, XmlTextWriter xmlOut)
        {
            // Definie initial state
            _tp = tp;
            _xmlOut = xmlOut;
        }
        
		/// <summary>
		/// Gets the source TabPage for the event.
		/// </summary>
        public Controls.TabPage TabPage
        {
            get { return _tp; }
        }
        
		/// <summary>
		/// Gets the source XmlTextWriter for the event.
		/// </summary>
        public XmlTextWriter XmlOut
        {
            get { return _xmlOut; }
        }
    }    
}
