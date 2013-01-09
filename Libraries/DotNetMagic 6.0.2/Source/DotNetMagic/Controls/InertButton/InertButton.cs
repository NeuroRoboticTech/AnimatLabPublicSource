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
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.ComponentModel;
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Simple button class that does not take the focus.
	/// </summary>
    [ToolboxBitmap(typeof(InertButton))]
    [DefaultProperty("PopupStyle")]
    public class InertButton : Control
    {
        // Instance fields
        private bool _mouseOver;
        private bool _mouseCapture;
        private bool _popupStyle;
		private int _borderWidth;
		private int _imageIndexEnabled;
        private int _imageIndexDisabled;
        private ImageList _imageList;
        private ImageAttributes _imageAttr;
        private MouseButtons _mouseButton;
		
		/// <summary>
		/// Initializes a new instance of the InertButton class.
		/// </summary>
        public InertButton()
        {
            InternalConstruct(null, -1, -1, null);
        }

		/// <summary>
		/// Initializes a new instance of the InertButton class.
		/// </summary>
		/// <param name="imageList">ImageList to use as source of images.</param>
		/// <param name="imageIndexEnabled">Index into list for enabled image.</param>
        public InertButton(ImageList imageList, int imageIndexEnabled)
        {
            InternalConstruct(imageList, imageIndexEnabled, -1, null);
        }

		/// <summary>
		/// Initializes a new instance of the InertButton class.
		/// </summary>
		/// <param name="imageList">ImageList to use as source of images.</param>
		/// <param name="imageIndexEnabled">Index into list for enabled image.</param>
		/// <param name="imageIndexDisabled">Index into list for disabled image.</param>
        public InertButton(ImageList imageList, int imageIndexEnabled, int imageIndexDisabled)
        {
            InternalConstruct(imageList, imageIndexEnabled, imageIndexDisabled, null);
        }

		/// <summary>
		/// Initializes a new instance of the InertButton class.
		/// </summary>
		/// <param name="imageList">ImageList to use as source of images.</param>
		/// <param name="imageIndexEnabled">Index into list for enabled image.</param>
		/// <param name="imageIndexDisabled">Index into list for disabled image.</param>
		/// <param name="imageAttr">Extra attributes to apply when drawing images.</param>
        public InertButton(ImageList imageList, int imageIndexEnabled, int imageIndexDisabled, ImageAttributes imageAttr)
        {
            InternalConstruct(imageList, imageIndexEnabled, imageIndexDisabled, imageAttr);
        }
		
        private void InternalConstruct(ImageList imageList, 
									   int imageIndexEnabled, 
									   int imageIndexDisabled, 
									   ImageAttributes imageAttr)
        {
			// NAG processing
			NAG.NAG_Start();
			
			// Remember parameters
            _imageList = imageList;
            _imageIndexEnabled = imageIndexEnabled;
            _imageIndexDisabled = imageIndexDisabled;
            _imageAttr = imageAttr;

            // Set initial state
            _borderWidth = 2;
            _mouseOver = false;
            _mouseCapture = false;
            _popupStyle = true;
            _mouseButton = MouseButtons.None;

            // Prevent drawing flicker by blitting from memory in WM_PAINT
            SetStyle(ControlStyles.OptimizedDoubleBuffer | 
				     ControlStyles.AllPaintingInWmPaint |
					 ControlStyles.UserPaint, true);

            // Prevent base class from trying to generate double click events and
            // so testing clicks against the double click time and rectangle. Getting
            // rid of this allows the user to press then button very quickly.
            SetStyle(ControlStyles.StandardDoubleClick, false);

            // Should not be allowed to select this control
            SetStyle(ControlStyles.Selectable, false);
        }

		/// <summary>
		/// Gets or sets the image list used for sourcing images.
		/// </summary>
        [Category("Appearance")]
        [DefaultValue(null)]
        public ImageList ImageList
        {
            get { return _imageList; }

            set
            {
                if (_imageList != value)
                {
                    _imageList = value;
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Gets or sets the index used for the enabled image.
		/// </summary>
        [Category("Appearance")]
        [DefaultValue(-1)]
        public int ImageIndexEnabled
        {
            get { return _imageIndexEnabled; }

            set
            {
                if (_imageIndexEnabled != value)
                {
                    _imageIndexEnabled = value;
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Gets or sets the index used for the disabled image.
		/// </summary>
        [Category("Appearance")]
        [DefaultValue(-1)]
        public int ImageIndexDisabled
        {
            get { return _imageIndexDisabled; }

            set
            {
                if (_imageIndexDisabled != value)
                {
                    _imageIndexDisabled = value;
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Gets or sets the extra attributes applied to drawing images.
		/// </summary>
        [Category("Appearance")]
        [DefaultValue(null)]
        public ImageAttributes ImageAttributes
        {
            get { return _imageAttr; }
			
            set
            {
                if (_imageAttr != value)
                {
                    _imageAttr = value;
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Gets or sets the border width around image.
		/// </summary>
        [Category("Appearance")]
        [DefaultValue(2)]
        public int BorderWidth
        {
            get { return _borderWidth; }

            set
            {
                if (_borderWidth != value)
                {
                    _borderWidth = value;
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Gets or sets the style used for drawing the button.
		/// </summary>
        [Category("Appearance")]
        [DefaultValue(true)]
        public bool PopupStyle
        {
            get { return _popupStyle; }

            set
            {
                if (_popupStyle != value)
                {
                    _popupStyle = value;
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Raises the MouseDown event.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!_mouseCapture)
            {
                // Mouse is over the button and being pressed, so enter the button depressed 
                // state and also remember the original button that was pressed. As we only 
                // generate an event when the same button is released.
                _mouseOver = true;
                _mouseCapture = true;
                _mouseButton = e.Button;

                // Redraw to show button state
                Invalidate();
            }

            base.OnMouseDown(e);
        }

		/// <summary>
		/// Raises the MouseUp event.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            // Are we waiting for this button to go up?
            if (e.Button == _mouseButton)
            {
                // Set state back to become normal
                _mouseOver = false;
                _mouseCapture = false;

                // Redraw to show button state
                Invalidate();
            }
            else
            {
                // We don't want to lose capture of mouse
                Capture = true;
            }

            base.OnMouseUp(e);
        }

		/// <summary>
		/// Raises the MouseMove event.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            // Is mouse point inside our client rectangle
            bool over = this.ClientRectangle.Contains(new Point(e.X, e.Y));

            // If entering the button area or leaving the button area...
            if (over != _mouseOver)
            {
                // Update state
                _mouseOver = over;

                // Redraw to show button state
                Invalidate();
            }

            base.OnMouseMove(e);
        }

		/// <summary>
		/// Raises the MouseEnter event.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the event data.</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            // Update state to reflect mouse over the button area
            _mouseOver = true;

            // Redraw to show button state
            Invalidate();

            base.OnMouseEnter(e);
        }

		/// <summary>
		/// Raises the MouseLeave event.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            // Update state to reflect mouse not over the button area
            _mouseOver = false;

            // Redraw to show button state
            Invalidate();

            base.OnMouseLeave(e);
        }

		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Do we have an image list for use?
            if (_imageList != null)
            {
                // Is the button disabled?
                if (!this.Enabled)
                {
                    // Do we have an image for showing when disabled?
                    if (_imageIndexDisabled != -1)
                    {
                        // Any caller supplied attributes to modify drawing?
                        if (null == _imageAttr)
                        {
                            // No, so use the simple DrawImage method
							Image drawImage = _imageList.Images[_imageIndexDisabled];
                            e.Graphics.DrawImage(drawImage, new Point(1,1));
							drawImage.Dispose();
                        }
                        else
                        {
                            // Yes, need to use the more complex DrawImage method instead
                            Image drawImage = _imageList.Images[_imageIndexDisabled];

                            // Three points provided are upper-left, upper-right and 
                            // lower-left of the destination parallelogram. 
                            Point[] pts = new Point[3];
                            pts[0].X = 1;
                            pts[0].Y = 1;
                            pts[1].X = pts[0].X + drawImage.Width;
                            pts[1].Y = pts[0].Y;
                            pts[2].X = pts[0].X;
                            pts[2].Y = pts[1].Y + drawImage.Height;

                            e.Graphics.DrawImage(drawImage, pts,
                                                 new Rectangle(0, 0, drawImage.Width, drawImage.Height),
                                                 GraphicsUnit.Pixel, _imageAttr);

							drawImage.Dispose();
                        }
                    }
                    else
                    {
                        // No disbled image, how about an enabled image we can draw grayed?
                        if (_imageIndexEnabled != -1)
                        {
                            // Yes, draw the enabled image but with color drained away
							Image drawImage = _imageList.Images[_imageIndexEnabled];
                            ControlPaint.DrawImageDisabled(e.Graphics, drawImage, 1, 1, this.BackColor);
							drawImage.Dispose();
                        }
                        else
                        {
                            // No images at all. Do nothing.
                        }
                    }
                }
                else
                {
                    // Button is enabled, any caller supplied attributes to modify drawing?
                    if (null == _imageAttr)
                    {
                        // No, so use the simple DrawImage method
						Image drawImage = _imageList.Images[_imageIndexEnabled];
                        e.Graphics.DrawImage(drawImage, (_mouseOver &&  _mouseCapture ? new Point(2,2) : new Point(1,1)));
						drawImage.Dispose();
                    }
                    else
                    {
                        // Yes, need to use the more complex DrawImage method instead
                        Image drawImage = _imageList.Images[_imageIndexEnabled];

                        // Three points provided are upper-left, upper-right and 
                        // lower-left of the destination parallelogram. 
                        Point[] pts = new Point[3];
                        pts[0].X = (_mouseOver && _mouseCapture) ? 2 : 1;
                        pts[0].Y = (_mouseOver && _mouseCapture) ? 2 : 1;
                        pts[1].X = pts[0].X + drawImage.Width;
                        pts[1].Y = pts[0].Y;
                        pts[2].X = pts[0].X;
                        pts[2].Y = pts[1].Y + drawImage.Height;

                        e.Graphics.DrawImage(drawImage, pts,
                                             new Rectangle(0, 0, drawImage.Width, drawImage.Height),
                                             GraphicsUnit.Pixel, _imageAttr);

						drawImage.Dispose();
                    }

                    ButtonBorderStyle bs;

                    // Decide on the type of border to draw around image
                    if (_popupStyle)
                    {
                        if (_mouseOver && this.Enabled)
                            bs = (_mouseCapture ? ButtonBorderStyle.Inset : ButtonBorderStyle.Outset);
                        else
                            bs = ButtonBorderStyle.Solid;
                    }
                    else
                    {
                        if (this.Enabled)
                            bs = ((_mouseOver && _mouseCapture) ? ButtonBorderStyle.Inset : ButtonBorderStyle.Outset);
                        else
                            bs = ButtonBorderStyle.Solid;
                    }

                    ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, 
                                            this.BackColor, _borderWidth, bs,
                                            this.BackColor, _borderWidth, bs,
                                            this.BackColor, _borderWidth, bs,
                                            this.BackColor, _borderWidth, bs);
                }
            }

            base.OnPaint(e);
        }
    }
}
