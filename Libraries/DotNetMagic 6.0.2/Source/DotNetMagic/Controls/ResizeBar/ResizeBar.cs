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
using Microsoft.Win32;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Interface used by ResizeBar to control behaviour.
	/// </summary>
    public interface IResizeSource
    {
		/// <summary>
		/// Determines if the ResizeBar instance is allowed to resize.
		/// </summary>
		/// <param name="bar">ResizeBar instance.</param>
		/// <returns>Is the instance allowed to resize.</returns>
        bool CanResize(ResizeBar bar);
		
		/// <summary>
		/// Determine the screen rectangle the bar can size into.
		/// </summary>
		/// <param name="bar">ResizeBar instance.</param>
		/// <param name="screenBoundary">Screen bounding rectangle.</param>
		/// <returns>Is the instance allowed to resize.</returns>
        bool StartResizeOperation(ResizeBar bar, ref Rectangle screenBoundary);

		/// <summary>
		/// Resize operation completed with delta change.
		/// </summary>
		/// <param name="bar">ResizeBar instance.</param>
		/// <param name="delta">Delta change to size.</param>
		void EndResizeOperation(ResizeBar bar, int delta);

		/// <summary>
		/// Gets the color used to draw.
		/// </summary>
        Color ResizeBarColor { get; }

		/// <summary>
		/// Gets the background color used to draw.
		/// </summary>
		Color BackgroundColor { get; }

		/// <summary>
		/// Gets the width/height of the ResizeBar control.
		/// </summary>
        int ResizeBarVector { get; }

		/// <summary>
		/// Gets the visual style required for display.
		/// </summary>
        VisualStyle Style { get; }
    }

	/// <summary>
	/// Control used to resize relative spacing of areas.
	/// </summary>
    [ToolboxItem(false)]
    public class ResizeBar : Control
    {
        // Class constants
        private static int IDE_LENGTH = 4;
        private static int PLAIN_LENGTH = 6;

        // Instance fields
        private bool _resizing;
        private Point _pointStart;
        private Point _pointCurrent;
        private Rectangle _boundary;
		private Rectangle _lastRect;
        private VisualStyle _style;
		private LayoutDirection _direction;
		private IResizeSource _resizeSource;
		private ColorDetails _colorDetails;

		/// <summary>
		/// Initializes a new instance of the ResizeBar class.
		/// </summary>
		/// <param name="direction">Direction for resizing.</param>
		/// <param name="resizeSource">Source of additional information.</param>
        public ResizeBar(LayoutDirection direction, IResizeSource resizeSource)
        {
            // Define initial state
            _direction = direction;
            _resizing = false;
            _resizeSource = resizeSource;
            _colorDetails = new ColorDetails();
            _style = VisualStyle.Office2007Blue;

			// Always default to Control color
			this.BackColor = _resizeSource.ResizeBarColor;
			this.ForeColor = SystemColors.ControlText;

            UpdateStyle(_resizeSource.Style);

			// We need to know when the system colours have changed
			Microsoft.Win32.SystemEvents.UserPreferenceChanged += 
				new Microsoft.Win32.UserPreferenceChangedEventHandler(OnUserPreferenceChanged); 
		}

		/// <summary>
		/// Gets and sets the visual style.
		/// </summary>
        public VisualStyle Style
        {
            get { return _style; }

            set
            {
                if (_style != value)
                    UpdateStyle(value);
            }
        }

		/// <summary>
		/// Gets and sets the direction for resizing.
		/// </summary>
        public LayoutDirection Direction
        {
            get { return _direction; }

            set
            {
                if (_direction != value)
                {
                    _direction = value;
                    UpdateStyle(_style);
                }
            }
        }

		/// <summary>
		/// Gets and sets the vector of the control.
		/// </summary>
        public int Length
        {
            get
            {
                int vector = _resizeSource.ResizeBarVector;
                
                if (vector == -1)
                {
                    switch (_style)
                    {
                        case VisualStyle.IDE2005:
                        case VisualStyle.Office2003:
                        case VisualStyle.Office2007Blue:
                        case VisualStyle.Office2007Silver:
                        case VisualStyle.Office2007Black:
                        case VisualStyle.MediaPlayerBlue:
                        case VisualStyle.MediaPlayerOrange:
                        case VisualStyle.MediaPlayerPurple:
                            vector = IDE_LENGTH;
                            break;
                        default:
                            vector = PLAIN_LENGTH;
                            break;
                    }
                }
               
                return vector;
            }
            
            set
            {
                // If a change in vector...
                if (value != this.Length)
                {
                    // Force update of the height/width
                    UpdateStyle(_resizeSource.Style);
                }
            }
        }

		/// <summary>
		/// Process change in environment values.
		/// </summary>
		/// <param name="name">Name of environment variable that has changed.</param>
		/// <param name="value">New value for environment variable.</param>
        public virtual void PropogateNameValue(PropogateName name, object value)
        {		
            if (name == PropogateName.ResizeBarVector)
                this.Length = (int)value;

            if (name == PropogateName.ResizeBarColor)
            {
                this.BackColor = (Color)value;
                Invalidate();
            }
            
			if (name == PropogateName.Style)
				UpdateStyle((VisualStyle)value);
		}
        
		/// <summary>
		/// Raises the MouseDown event.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the event data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			// Currently in a resizing operation?
			if (_resizing)
			{
				// Reset resizing state
				EndResizeOperation(e);
			}
			else
			{
				// Mouse down occured inside control
				_resizing = StartResizeOperation(e);
			}

			base.OnMouseDown(e);
		}

		/// <summary>
		/// Raises the MouseUp event.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the event data.</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			// Currently in a resizing operation?
			if (_resizing)
			{
				// Reset resizing state
				EndResizeOperation(e);
			}

			base.OnMouseUp(e);
		}

		/// <summary>
		/// Raises the MouseMove event.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the event data.</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if ((_resizeSource != null) && (_resizeSource.CanResize(this)))
			{
				// Display the correct mouse shape
				if (_direction == LayoutDirection.Vertical)
					this.Cursor = Cursors.SizeNS;
				else
					this.Cursor = Cursors.SizeWE;
			}
			else
				this.Cursor = Cursors.Arrow;

			// Currently in a resizing operation?
			if (_resizing)
				UpdateResizePosition(e);

			base.OnMouseMove(e);
		}

		/// <summary>
		/// Process a change in system colors.
		/// </summary>
		/// <param name="e">An EventArgs structure containing the event data.</param>
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			_colorDetails.Reset();
			base.OnSystemColorsChanged(e);
		}

		/// <summary>
		/// Raises the PaintBackground event.
		/// </summary>
		/// <param name="pevent">A PaintEventArgs that contains the event data.</param>
		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
            // If using the default background color
            if (BackColor == SystemColors.Control)
            {
                switch (_style)
                {
                    case VisualStyle.Office2003:
                    case VisualStyle.IDE2005:
				        using(SolidBrush backBrush = new SolidBrush(_colorDetails.BaseColor))
					        pevent.Graphics.FillRectangle(backBrush, pevent.ClipRectangle);
				        return;
                    case VisualStyle.Office2007Blue:
                    case VisualStyle.Office2007Silver:
                    case VisualStyle.Office2007Black:
                        using (SolidBrush backBrush = new SolidBrush(Office2007ColorTable.LightBackground(_style)))
                            pevent.Graphics.FillRectangle(backBrush, pevent.ClipRectangle);
                        return;
                    case VisualStyle.MediaPlayerBlue:
                    case VisualStyle.MediaPlayerOrange:
                    case VisualStyle.MediaPlayerPurple:
                        using (SolidBrush backBrush = new SolidBrush(MediaPlayerColorTable.LightBackground(_style)))
                            pevent.Graphics.FillRectangle(backBrush, pevent.ClipRectangle);
                        return;
                } 
            }

            // Use background color
			base.OnPaintBackground(pevent);
		}

		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			// Plain style draws a 3D effect around edges
			if (_style == VisualStyle.Plain)
			{
				// Drawing is relative to client area
				Size ourSize = this.ClientSize;

				Point[] light = new Point[2];
				Point[] dark = new Point[2];
				Point[] black = new Point[2];

				// Painting depends on orientation
				if (_direction == LayoutDirection.Vertical)
				{
					// Draw as a horizontal bar
					dark[0].Y = dark[1].Y = ourSize.Height - 2;
					black[0].Y = black[1].Y = ourSize.Height - 1;
					light[1].X = dark[1].X = black[1].X = ourSize.Width;
				}
				else
				{
					// Draw as a vertical bar
					dark[0].X = dark[1].X = ourSize.Width - 2;
					black[0].X = black[1].X = ourSize.Width - 1;
					light[1].Y = dark[1].Y = black[1].Y = ourSize.Height;
				}

				using (Pen penLightLight = new Pen(ControlPaint.LightLight(_resizeSource.BackgroundColor)),
						   penDark = new Pen(ControlPaint.Dark(_resizeSource.BackgroundColor)),
						   penBlack = new Pen(ControlPaint.DarkDark(_resizeSource.BackgroundColor)))
				{
					e.Graphics.DrawLine(penLightLight, light[0], light[1]);
					e.Graphics.DrawLine(penDark, dark[0], dark[1]);
					e.Graphics.DrawLine(penBlack, black[0], black[1]);
				}	
			}
				
			// Let delegates fire through base
			base.OnPaint(e);
		}
		
		/// <summary>
		/// Dispose of instance resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Must unhook otherwise the instance cannot be garbage collected
				Microsoft.Win32.SystemEvents.UserPreferenceChanged -= 
					new Microsoft.Win32.UserPreferenceChangedEventHandler(OnUserPreferenceChanged); 

				// Color details has resources that need releasing
				_colorDetails.Dispose();
			}
			
			base.Dispose (disposing);
		}
		
		internal ColorDetails ColorDetails
		{
			get { return _colorDetails; }
		}
		
		private void UpdateStyle(VisualStyle newStyle)
        {
            _style = ColorHelper.ValidateStyle(newStyle);
            _colorDetails.Style = _style;

            int vector = this.Length;

            if (_direction == LayoutDirection.Vertical)
                this.Height = vector;
            else
                this.Width = vector;

            Invalidate();
        }

        private bool StartResizeOperation(MouseEventArgs e)
        {
            if (_resizeSource != null)
            {
                if (_resizeSource.CanResize(this))
                {
                    if (_resizeSource.StartResizeOperation(this, ref _boundary))
                    {
						// Reset the last rectangle draw
						_lastRect = Rectangle.Empty;

                        // Record the starting screen position
                        _pointStart = PointToScreen(new Point(e.X, e.Y));

                        // Record the current position being drawn
                        _pointCurrent = _pointStart;

                        // Draw the starting position
                        DrawResizeIndicator(_pointCurrent);

                        return true;
                    }
                }
            }

            return false;
        }

        private void EndResizeOperation(MouseEventArgs e)
        {
            if (_resizeSource != null)
            {
				// Set last drawn rect to nothing, otherwise drawing the rectangle in exactly
				// the same place as the last time around would cause a null effect due to the
				// code used to reduce flicker by only drawing the delta changes.
				_lastRect = Rectangle.Empty;

                // Undraw the current position
                DrawResizeIndicator(_pointCurrent);

                // Find new screen position
                Point newCurrent = PointToScreen(new Point(e.X, e.Y));

                // Limit the extent the bar can be moved
                ApplyBoundaryToPoint(ref newCurrent);

                // Calculate delta from initial resize
                Point delta = new Point(newCurrent.X - _pointStart.X, 
                                        newCurrent.Y - _pointStart.Y);

                // Inform the Zone of requested change
                if (_direction == LayoutDirection.Horizontal)
                    _resizeSource.EndResizeOperation(this, delta.X);
                else
                    _resizeSource.EndResizeOperation(this, delta.Y);
            }

            _resizing = false;
        }

        private void UpdateResizePosition(MouseEventArgs e)
        {
            // Find new screen position
            Point newCurrent = PointToScreen(new Point(e.X, e.Y));

            // Limit the extend the bar can be moved
            ApplyBoundaryToPoint(ref newCurrent);

            // Has change in position occured?
            if (newCurrent != _pointCurrent)
            {
                // Undraw the old position
                DrawResizeIndicator(_pointCurrent);

                // Record the new screen position
                _pointCurrent = newCurrent;

                // Draw the new position
                DrawResizeIndicator(_pointCurrent);
            }
        }
	
        private void ApplyBoundaryToPoint(ref Point newCurrent)
        {
            // Calculate mouse position delta from mouse down
            Point delta = new Point(newCurrent.X - _pointStart.X, 
                newCurrent.Y - _pointStart.Y);
			
            // Get our dimensions in screen coordinates
            Rectangle client = RectangleToScreen(this.ClientRectangle);

            if (_direction == LayoutDirection.Horizontal)
            {
                client.Offset(delta.X, 0);

                // Test against left hand edge
                if (client.X < _boundary.X)
                    newCurrent.X += _boundary.X - client.X;
				
                // Test against right hand edge
                if (client.Right > _boundary.Right)
                    newCurrent.X -= client.Right - _boundary.Right;
            }
            else
            {
                client.Offset(0, delta.Y);

                // Test against top edge
                if (client.Y < _boundary.Y)
                    newCurrent.Y += _boundary.Y - client.Y;
				
                // Test against bottom edge
                if (client.Bottom > _boundary.Bottom)
                    newCurrent.Y -= client.Bottom - _boundary.Bottom;
            }		
        }

		private void DrawResizeIndicator(Point screenPosition)
		{
			// Calculate mouse position delta from mouse down
			Point delta = new Point(screenPosition.X - _pointStart.X, 
									screenPosition.Y - _pointStart.Y);

			// Get our dimensions in screen coordinates
			Rectangle client = RectangleToScreen(this.ClientRectangle);

			if (_direction == LayoutDirection.Horizontal)
				client.Offset(delta.X, 0);
			else
				client.Offset(0, delta.Y);

			// Do we draw just a single rectangle?
			if (_lastRect == Rectangle.Empty)
				DrawHelper.DrawDragRectangle(client, 0);
			else
				DrawHelper.DrawDragRectangles(new Rectangle[]{_lastRect, client}, 0);

			// Remember the last rectangle drawn
			_lastRect = client;
		}
		
		private void OnUserPreferenceChanged(object sender, Microsoft.Win32.UserPreferenceChangedEventArgs e)
		{
			// Use as a signal to retest the theme information
			_colorDetails.Reset();
		}
	}
    
	/// <summary>
	/// ResizeBar control specialized to work with AutoHidePanel
	/// </summary>
    [ToolboxItem(false)]
    public class ResizeAutoBar : ResizeBar
    {
		/// <summary>
		/// Initializes a new instance of the ResizeAutoBar class.
		/// </summary>
		/// <param name="direction">Direction for resizing.</param>
		/// <param name="resizeSource">Source of additional information.</param>
        public ResizeAutoBar(LayoutDirection direction, IResizeSource resizeSource)
            : base(direction, resizeSource) 
        {
        }    
            
		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
			Color darkColor = ControlPaint.Dark(this.BackColor);
			Color darkDarkColor = ControlPaint.DarkDark(this.BackColor);
			Color lightlightColor = ControlPaint.LightLight(this.BackColor);

            // If the background color has not be overriden
            if (BackColor == SystemColors.Control)
            {
                switch (Style)
                {
                    case VisualStyle.IDE2005:
                    case VisualStyle.Office2003:
					    darkColor = ColorDetails.BaseColor2;
					    darkDarkColor = ColorDetails.OpenBorderColor;
					    lightlightColor = darkDarkColor;
                        break;
                    case VisualStyle.Office2007Blue:
                    case VisualStyle.Office2007Silver:
                    case VisualStyle.Office2007Black:
                        darkColor = Office2007ColorTable.BorderColor(Style);
                        darkDarkColor = Office2007ColorTable.LightBackground(Style);
                        lightlightColor = darkColor;
                        break;
                    case VisualStyle.MediaPlayerBlue:
                    case VisualStyle.MediaPlayerOrange:
                    case VisualStyle.MediaPlayerPurple:
                        darkColor = MediaPlayerColorTable.BorderColor(Style);
                        darkDarkColor = MediaPlayerColorTable.LightBackground(Style);
                        lightlightColor = darkColor;
                        break;
                }
			}
			
            switch(this.Dock)
            {
                case DockStyle.Right:
                    using(Pen penD = new Pen(darkColor),
                              penDD = new Pen(darkDarkColor))
                    {
                        e.Graphics.DrawLine(penD, this.Width - 2, 0, this.Width - 2, this.Height);
                        e.Graphics.DrawLine(penDD, this.Width - 1, 0, this.Width - 1, this.Height);
                    }
                    break;
                case DockStyle.Left:
                    using(Pen penLL = new Pen(lightlightColor))
                        e.Graphics.DrawLine(penLL, 1, 0, 1, this.Height);
                    break;
                case DockStyle.Bottom:
                    using(Pen penD = new Pen(darkColor),
                              penDD = new Pen(darkDarkColor))
                    {
                        e.Graphics.DrawLine(penD, 0, this.Height - 2, this.Width, this.Height - 2);
                        e.Graphics.DrawLine(penDD, 0, this.Height - 1, this.Width, this.Height - 1);
                    }
                    break;
                case DockStyle.Top:
                    using(Pen penLL = new Pen(lightlightColor))
                        e.Graphics.DrawLine(penLL, 0, 1, this.Width, 1);
                    break;
            }
        }
    }
}
