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

namespace Crownwood.DotNetMagic.Docking
{
	/// <summary>
	/// Manager for redocking of content instance.
	/// </summary>
    public class RedockerContent : Redocker
    {
		/// <summary>
		/// Specifies source of the content.
		/// </summary>
        public enum Source
        {
			/// <summary>
			/// Specifies the content is currently inside a window.
			/// </summary>
            ContentInsideWindow,

			/// <summary>
			/// Specifies a window content.
			/// </summary>
            WindowContent,

			/// <summary>
			/// Specifies the content is currently inside a floating form.
			/// </summary>
            FloatingForm
        }

        internal int _outerIndex;
        internal Rectangle _insideRect;
        internal Rectangle _outsideRect;
        internal ArrayList _topList;
        internal ArrayList _bottomList;
        internal ArrayList _leftList;
        internal ArrayList _rightList;
        internal ArrayList _fillList;

        // Instance fields
		private bool _squares;
		private Source _source;
		private Content _content;
        private WindowContent _windowContent;
        private Control _callingControl;
        private ScrollableControl _container;
        private DockingManager _dockingManager;
        private FloatingForm _floatingForm;
		private Point _offset;
		private int _vectorH;
		private int _vectorV;

		/// <summary>
		/// Initializes a new instance of the RedockerContent class.
		/// </summary>
		/// <param name="squares">Show squares or diamonds.</param>
		/// <param name="callingControl">Calling control instance.</param>
		/// <param name="c">Source content.</param>
		/// <param name="wc">WindowContent that contains content.</param>
		/// <param name="offset">Screen offset.</param>
        public RedockerContent(bool squares,
							   Control callingControl, 
							   Content c, 
							   WindowContent wc, 
							   Point offset)
        {
            InternalConstruct(squares,
							  callingControl, 
							  Source.ContentInsideWindow, 
							  c, wc, null, c.DockingManager, offset);
        }

		/// <summary>
		/// Initializes a new instance of the RedockerContent class.
		/// </summary>
		/// <param name="squares">Show squares or diamonds.</param>
		/// <param name="callingControl">Calling control instance.</param>
		/// <param name="wc">WindowContent that contains content.</param>
		/// <param name="offset">Screen offset.</param>
        public RedockerContent(bool squares,
							   Control callingControl, 
							   WindowContent wc, 
							   Point offset)
        {
            InternalConstruct(squares, 
							  callingControl, 
							  Source.WindowContent, 
							  null, wc, null, 
							  wc.DockingManager, offset);
        }

		/// <summary>
		/// Initializes a new instance of the RedockerContent class.
		/// </summary>
		/// <param name="squares">Show squares or diamonds.</param>
		/// <param name="ff">Floating form source.</param>
		/// <param name="offset">Screen offset.</param>
        public RedockerContent(bool squares, FloatingForm ff, Point offset)
        {
            InternalConstruct(squares, ff, Source.FloatingForm, null, null, ff, ff.DockingManager, offset);
        }

		/// <summary>
		/// Perform initialization.
		/// </summary>
		/// <param name="squares">Show squares or diamonds.</param>
		/// <param name="callingControl">Calling control instance.</param>
		/// <param name="source">Type of source.</param>
		/// <param name="c">Source content.</param>
		/// <param name="wc">WindowContent that contains content.</param>
		/// <param name="ff">Floating form source.</param>
		/// <param name="dm">DockingManager instance.</param>
		/// <param name="offset">Screen offset.</param>
        protected virtual void InternalConstruct(bool squares,
												 Control callingControl, 
												 Source source, 
												 Content c, 
												 WindowContent wc, 
												 FloatingForm ff,
												 DockingManager dm,
												 Point offset)
        {
            // Store the starting state
			_squares = squares;
            _insideRect = new Rectangle();
            _outsideRect = new Rectangle();
            _callingControl = callingControl;
            _source = source;
            _content = c;
            _windowContent = wc;
            _dockingManager = dm;
            _container = _dockingManager.Container;
            _floatingForm = ff;
            _offset = offset;

            // We do not allow docking in front of the outer index entry
            _outerIndex = FindOuterIndex();

            // Create lists of Controls which are docked against each edge	
            _topList = new ArrayList();
            _leftList = new ArrayList();
            _rightList = new ArrayList();
            _bottomList = new ArrayList();
            _fillList = new ArrayList();

            PreProcessControlsCollection();

            // Find the vectors required for calculating new sizes
            VectorDependsOnSourceAndState();

            // Begin tracking straight away
            EnterTrackingMode();
        }

		/// <summary>
		/// Gets the cached squares requirement.
		/// </summary>
		public bool Squares
		{
			get { return _squares; }
		}

		/// <summary>
		/// Gets new vertical vector.
		/// </summary>
		public int VectorV
		{
			get { return _vectorV; }
		}

		/// <summary>
		/// Gets new vertical vector.
		/// </summary>
		public int VectorH
		{
			get { return _vectorH; }
		}

		/// <summary>
		/// Gets the cached source type.
		/// </summary>
        public Source SourceType
        {
            get { return _source; }
        }

		/// <summary>
		/// Gets the cached container control.
		/// </summary>
        public ScrollableControl Container
        {
            get { return _container; }
        }

		/// <summary>
		/// Gets the cached source content.
		/// </summary>
        public Content Content
        {
            get { return _content; }
        }

		/// <summary>
		/// Gets the cached calling control.
		/// </summary>
        public Control CallingControl 
        {
            get { return _callingControl; }
        }

		/// <summary>
		/// Gets the cached source type.
		/// </summary>
		public Source DockingSource 
		{
			get { return _source; }
		}

		/// <summary>
		/// Gets the cached window content.
		/// </summary>
        public WindowContent WindowContent
        {
            get { return _windowContent; }
        }

		/// <summary>
		/// Gets the cached docking manager.
		/// </summary>
        public DockingManager DockingManager
        {
            get { return _dockingManager; }
        }

		/// <summary>
		/// Gets the cached floating form.
		/// </summary>
        public FloatingForm FloatingForm
        {
            get { return _floatingForm; }
        }

		/// <summary>
		/// Gets the cached offset point.
		/// </summary>
        public Point Offset
        {
            get { return _offset; }
        }

		/// <summary>
		/// Gets the new floating size.
		/// </summary>
		/// <returns></returns>
        public Size SizeDependsOnSource()
        {
            switch(SourceType)
            {
                case Source.WindowContent:
                    return WindowContent.Size;
                case Source.FloatingForm:
                    return FloatingForm.Size;
                case Source.ContentInsideWindow:
                default:
                    return Content.DisplaySize;
            }
        }

        private void PreProcessControlsCollection()
        {
            // Find space left after all docking windows has been positioned
            _insideRect = Container.ClientRectangle; 
            _outsideRect = _insideRect; 

            // We want lists of docked controls grouped by docking style
            foreach(Control item in Container.Controls)
            {
                bool ignoreType = (item is AutoHidePanel);

                int controlIndex = Container.Controls.IndexOf(item);

                bool outer = (controlIndex >= _outerIndex);

                if (item.Visible)
                {
                    if (item.Dock == DockStyle.Top)
                    {
                        _topList.Insert(0, item);

                        if (_insideRect.Y < item.Bottom)
                        {
                            _insideRect.Height -= item.Bottom - _insideRect.Y;
                            _insideRect.Y = item.Bottom;
                        }

                        if (outer || ignoreType)
                        {
                            if (_outsideRect.Y < item.Bottom)
                            {
                                _outsideRect.Height -= item.Bottom - _outsideRect.Y;
                                _outsideRect.Y = item.Bottom;
                            }
                        }
                    }

                    if (item.Dock == DockStyle.Left)
                    {
                        _leftList.Insert(0, item);

                        if (_insideRect.X < item.Right)
                        {
                            _insideRect.Width -= item.Right - _insideRect.X;
                            _insideRect.X = item.Right;
                        }

                        if (outer || ignoreType)
                        {
                            if (_outsideRect.X < item.Right)
                            {
                                _outsideRect.Width -= item.Right - _outsideRect.X;
                                _outsideRect.X = item.Right;
                            }
                        }
                    }

                    if (item.Dock == DockStyle.Bottom)
                    {
                        _bottomList.Insert(0, item);

                        if (_insideRect.Bottom > item.Top)
                            _insideRect.Height -= _insideRect.Bottom - item.Top;

                        if (outer || ignoreType)
                        {
                            if (_outsideRect.Bottom > item.Top)
                                _outsideRect.Height -= _outsideRect.Bottom - item.Top;
                        }
                    }

                    if (item.Dock == DockStyle.Right)
                    {
                        _rightList.Insert(0, item);

                        if (_insideRect.Right > item.Left)
                            _insideRect.Width -= _insideRect.Right - item.Left;

                        if (outer || ignoreType)
                        {
                            if (_outsideRect.Right > item.Left)
                                _outsideRect.Width -= _outsideRect.Right - item.Left;
                        }
                    }

                    if (item.Dock == DockStyle.Fill)
						_fillList.Insert(0, item);
                }
            }

            // Convert to screen coordinates
            _insideRect = Container.RectangleToScreen(_insideRect);
            _outsideRect = Container.RectangleToScreen(_outsideRect);
        }

        private void VectorDependsOnSourceAndState()
        {
            Size sourceSize = SizeDependsOnSource();

            switch(SourceType)
            {
                case Source.FloatingForm:
					// Make sure the vector is the smaller of the two dimensions
					if (sourceSize.Width > sourceSize.Height)
						sourceSize.Width = sourceSize.Height;

					if (sourceSize.Height > sourceSize.Width)
						sourceSize.Height = sourceSize.Width;

					// Do not let the new vector extend beyond halfway
					if (sourceSize.Width > (Container.Width / 2))
						sourceSize.Width = Container.Width / 2;

					if (sourceSize.Height > (Container.Height / 2))
						sourceSize.Height = Container.Height / 2;
					break;
                case Source.WindowContent:
                case Source.ContentInsideWindow:
                switch(WindowContent.State)
                {
                    case State.DockLeft:
                    case State.DockRight:
                        _vectorH = sourceSize.Width;
                        _vectorV = _vectorH;
                        return;
                    case State.DockTop:
                    case State.DockBottom:
                        _vectorH = sourceSize.Height;
                        _vectorV = _vectorH;
                        return;
                }
                break;
            }

            _vectorV = sourceSize.Height;
            _vectorH = sourceSize.Width;
        }

		private int FindOuterIndex()
        {
            // We do not allow docking in front of the outer index entry
            int outerIndex = Container.Controls.Count;
			
            Control outerControl = DockingManager.OuterControl;

            // If an outer control has been specified then use this as the limit
            if (outerControl != null)
                outerIndex = Container.Controls.IndexOf(outerControl);

            return outerIndex;
        }
	}
}
