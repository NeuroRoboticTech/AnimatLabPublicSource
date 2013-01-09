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
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Crownwood.DotNetMagic.Win32;

namespace Crownwood.DotNetMagic.Common
{
	/// <summary>
	/// Collection of static helper routines for drawing visual elements.
	/// </summary>
    public sealed class DrawHelper
    {
        // Instance fields
        private static IntPtr _halfToneBrush = IntPtr.Zero;

		// Prevent instance from being created.
		private DrawHelper() {}

		/// <summary>
		/// Get the size required to draw the text using a raw sizing.
		/// </summary>
		/// <param name="g">Graphics object used to find size.</param>
		/// <param name="font">Font used to calculate size.</param>
		/// <param name="text">String to be measured.</param>
		/// <returns>Size of the rectangle large enough to enclose text.</returns>
		public static Size RawTextSize(Graphics g, Font font, string text)
		{
			Region[] regions = new Region[1];
			RectangleF rect = new RectangleF(0, 0, 9999, 9999);
			CharacterRange[] ranges  =  { new CharacterRange(0, text.Length) };

			using(StringFormat format = new StringFormat())
			{
				format.SetMeasurableCharacterRanges (ranges);
				regions = g.MeasureCharacterRanges (text, font, rect, format);
				rect = regions[0].GetBounds(g);
			}

			// Rough conversion to integer is good enough
			return new Size((int)(rect.Right + 1.0f), (int)font.GetHeight());
		}

		/// <summary>
        /// Draw a string in the reverse direction.
        /// </summary>
        /// <param name="g">Graphics object used for drawing.</param>
        /// <param name="drawText">Text string to be drawn.</param>
        /// <param name="drawFont">Font to draw string using.</param>
        /// <param name="drawRect">Destination rectangle for string.</param>
        /// <param name="drawBrush">String text drawing brush.</param>
        /// <param name="drawFormat">Format flags appropriate for this operation.</param>
        public static void DrawReverseString(Graphics g, 
                                             String drawText, 
                                             Font drawFont, 
                                             Rectangle drawRect,
                                             Brush drawBrush,
                                             StringFormat drawFormat)
        {
            GraphicsContainer container = g.BeginContainer();

            // The text will be rotated around the origin (0,0) and so needs moving
            // back into position by using a transform
            g.TranslateTransform(drawRect.Left * 2 + drawRect.Width, 
								 drawRect.Top * 2 + drawRect.Height);

            // Rotate the text by 180 degress to reverse the direction 
            g.RotateTransform(180);

            // Draw the string as normal and let then transforms do the work
            g.DrawString(drawText, drawFont, drawBrush, drawRect, drawFormat);

            g.EndContainer(container);
        }

        /// <summary>
        /// Draw a three pixel wide border to given a raised appearance.
        /// </summary>
        /// <param name="g">Graphics object used to perform drawing.</param>
        /// <param name="rect">Draw raised edge inside the rectangle.</param>
        /// <param name="lightLight">Color used for drawing light lines.</param>
        /// <param name="baseColor">Color used for drawing base lines.</param>
        /// <param name="dark">Color used for drawing dark lines.</param>
        /// <param name="darkDark">Color used for drawing very dark lines.</param>
        public static void DrawPlainRaisedBorder(Graphics g, 
                                                 Rectangle rect,
                                                 Color lightLight, 
                                                 Color baseColor,
                                                 Color dark, 
                                                 Color darkDark)
        {
            if ((rect.Width > 2) && (rect.Height > 2))
            {
                using(Pen ll = new Pen(lightLight),
                          b = new Pen(baseColor),
                          d = new Pen(dark),
                          dd = new Pen(darkDark))
                {
                    int left = rect.Left;
                    int top = rect.Top;
                    int right = rect.Right;
                    int bottom = rect.Bottom;

                    // Draw the top border
                    g.DrawLine(b, right-1, top, left, top);
                    g.DrawLine(ll, right-2, top+1, left+1, top+1);
                    g.DrawLine(b, right-3, top+2, left+2, top+2);

                    // Draw the left border
                    g.DrawLine(b, left, top, left, bottom-1);
                    g.DrawLine(ll, left+1, top+1, left+1, bottom-2);
                    g.DrawLine(b, left+2, top+2, left+2, bottom-3);
					
                    // Draw the right
                    g.DrawLine(dd, right-1, top+1, right-1, bottom-1);
                    g.DrawLine(d, right-2, top+2, right-2, bottom-2);
                    g.DrawLine(b, right-3, top+3, right-3, bottom-3);

                    // Draw the bottom
                    g.DrawLine(dd, right-1, bottom-1, left, bottom-1);
                    g.DrawLine(d, right-2, bottom-2, left+1, bottom-2);
                    g.DrawLine(b, right-3, bottom-3, left+2, bottom-3);
                }
            }
        }

        /// <summary>
        /// Draw a three pixel wide raised border at either the top or bottom of rectangle.
        /// </summary>
        /// <param name="g">Graphics object used to perform drawing.</param>
        /// <param name="rect">Draw raised edge inside the rectangle.</param>
        /// <param name="lightLight">Color used for drawing light lines.</param>
        /// <param name="baseColor">Color used for drawing base lines.</param>
        /// <param name="dark">Color used for drawing dark lines.</param>
        /// <param name="darkDark">Color used for drawing very dark lines.</param>
        /// <param name="drawTop">Should border be drawn at top or bottom edge.</param>
        public static void DrawPlainRaisedBorderTopOrBottom(Graphics g, 
                                                            Rectangle rect,
                                                            Color lightLight, 
                                                            Color baseColor,
                                                            Color dark, 
                                                            Color darkDark,
                                                            bool drawTop)
        {
            if ((rect.Width > 2) && (rect.Height > 2))
            {
                using(Pen ll = new Pen(lightLight),
                          b = new Pen(baseColor),
                          d = new Pen(dark),
                          dd = new Pen(darkDark))
                {
                    int left = rect.Left;
                    int top = rect.Top;
                    int right = rect.Right;
                    int bottom = rect.Bottom;

                    if (drawTop)
                    {
                        // Draw the top border
                        g.DrawLine(b, right-1, top, left, top);
                        g.DrawLine(ll, right-1, top+1, left, top+1);
                        g.DrawLine(b, right-1, top+2, left, top+2);
                    }
                    else
                    {
                        // Draw the bottom
                        g.DrawLine(dd, right-1, bottom-1, left, bottom-1);
                        g.DrawLine(d, right-1, bottom-2, left, bottom-2);
                        g.DrawLine(b, right-1, bottom-3, left, bottom-3);
                    }
                }
            }
        }

        /// <summary>
        /// Draw a three pixel wide border to given a sunken appearance.
        /// </summary>
        /// <param name="g">Graphics object used to perform drawing.</param>
        /// <param name="rect">Draw raised edge inside the rectangle.</param>
        /// <param name="lightLight">Color used for drawing light lines.</param>
        /// <param name="baseColor">Color used for drawing base lines.</param>
        /// <param name="dark">Color used for drawing dark lines.</param>
        /// <param name="darkDark">Color used for drawing very dark lines.</param>
        public static void DrawPlainSunkenBorder(Graphics g, 
                                                 Rectangle rect,
                                                 Color lightLight, 
                                                 Color baseColor,
                                                 Color dark, 
                                                 Color darkDark)
        {
            if ((rect.Width > 2) && (rect.Height > 2))
            {
                using(Pen ll = new Pen(lightLight),
                          b = new Pen(baseColor),
                          d = new Pen(dark),
                          dd = new Pen(darkDark))
                {
                    int left = rect.Left;
                    int top = rect.Top;
                    int right = rect.Right;
                    int bottom = rect.Bottom;

                    // Draw the top border
                    g.DrawLine(d, right-1, top, left, top);
                    g.DrawLine(dd, right-2, top+1, left+1, top+1);
                    g.DrawLine(b, right-3, top+2, left+2, top+2);

                    // Draw the left border
                    g.DrawLine(d, left, top, left, bottom-1);
                    g.DrawLine(dd, left+1, top+1, left+1, bottom-2);
                    g.DrawLine(b, left+2, top+2, left+2, bottom-3);
					
                    // Draw the right
                    g.DrawLine(ll, right-1, top+1, right-1, bottom-1);
                    g.DrawLine(b, right-2, top+2, right-2, bottom-2);
                    g.DrawLine(b, right-3, top+3, right-3, bottom-3);

                    // Draw the bottom
                    g.DrawLine(ll, right-1, bottom-1, left, bottom-1);
                    g.DrawLine(b, right-2, bottom-2, left+1, bottom-2);
                    g.DrawLine(b, right-3, bottom-3, left+2, bottom-3);
                }
            }
        }

        /// <summary>
        /// Draw a three pixel wide sunken border at either the top or bottom of rectangle.
        /// </summary>
        /// <param name="g">Graphics object used to perform drawing.</param>
        /// <param name="rect">Draw raised edge inside the rectangle.</param>
        /// <param name="lightLight">Color used for drawing light lines.</param>
        /// <param name="baseColor">Color used for drawing base lines.</param>
        /// <param name="dark">Color used for drawing dark lines.</param>
        /// <param name="darkDark">Color used for drawing very dark lines.</param>
        /// <param name="drawTop">Should border be drawn at top or bottom edge.</param>
        public static void DrawPlainSunkenBorderTopOrBottom(Graphics g, 
                                                            Rectangle rect,
                                                            Color lightLight, 
                                                            Color baseColor,
                                                            Color dark, 
                                                            Color darkDark,
                                                            bool drawTop)
        {
            if ((rect.Width > 2) && (rect.Height > 2))
            {
                using(Pen ll = new Pen(lightLight),
                          b = new Pen(baseColor),
                          d = new Pen(dark),
                          dd = new Pen(darkDark))
                {
                    int left = rect.Left;
                    int top = rect.Top;
                    int right = rect.Right;
                    int bottom = rect.Bottom;

                    if (drawTop)
                    {
                        // Draw the top border
                        g.DrawLine(d, right-1, top, left, top);
                        g.DrawLine(dd, right-1, top+1, left, top+1);
                        g.DrawLine(b, right-1, top+2, left, top+2);
                    }
                    else
                    {
                        // Draw the bottom
                        g.DrawLine(ll, right-1, bottom-1, left, bottom-1);
                        g.DrawLine(b, right-1, bottom-2, left, bottom-2);
                        g.DrawLine(b, right-1, bottom-3, left, bottom-3);
                    }
                }
            }
        }
        
        /// <summary>
        /// Draw a dragging rectangle inside the given rectangle with requested indent thickness.
        /// </summary>
        /// <param name="newRect">Draw inside this rectangle.</param>
        /// <param name="indent">Thickness of lines inside rectangle.</param>
        public static void DrawDragRectangle(Rectangle newRect, int indent)
        {
            DrawDragRectangles(new Rectangle[]{newRect}, indent);
        }

        /// <summary>
        /// Draw a multiple dragging rectangles with requested indent thickness.
        /// </summary>
        /// <param name="newRects">Draw inside each rectangle.</param>
        /// <param name="indent">Thickness of lines inside rectangle.</param>
        public static void DrawDragRectangles(Rectangle[] newRects, int indent)
        {
            if (newRects.Length > 0)
            {
                // Create the first region
                IntPtr newRegion = CreateRectangleRegion(newRects[0], indent);

                for(int index=1; index<newRects.Length; index++)
                {
                    // Create the extra region
                    IntPtr extraRegion = CreateRectangleRegion(newRects[index], indent);

                    // Remove the intersection of the existing and extra regions
                    Gdi32.CombineRgn(newRegion, newRegion, extraRegion, (int)Win32.CombineFlags.RGN_XOR);

                    // Remove unwanted intermediate objects
                    Gdi32.DeleteObject(extraRegion);
                }

                // Get hold of the DC for the desktop
                IntPtr hDC = User32.GetDC(IntPtr.Zero);

                // Define the area we are allowed to draw into
                Gdi32.SelectClipRgn(hDC, newRegion);

                Win32.RECT rectBox = new Win32.RECT();
				 
                // Get the smallest rectangle that encloses region
                Gdi32.GetClipBox(hDC, ref rectBox);

                IntPtr brushHandler = GetHalfToneBrush();

                // Select brush into the device context
                IntPtr oldHandle = Gdi32.SelectObject(hDC, brushHandler);

                // Blit to screen using provided pattern brush and invert with existing screen contents
                Gdi32.PatBlt(hDC, 
                             rectBox.left, 
                             rectBox.top, 
                             rectBox.right - rectBox.left, 
                             rectBox.bottom - rectBox.top, 
                             (uint)RasterOperations.PATINVERT);

                // Put old handle back again
                Gdi32.SelectObject(hDC, oldHandle);

                // Reset the clipping region
                Gdi32.SelectClipRgn(hDC, IntPtr.Zero);

                // Remove unwanted region object
                Gdi32.DeleteObject(newRegion);

                // Must remember to release the HDC resource!
                User32.ReleaseDC(IntPtr.Zero, hDC);
            }
        }

        /// <summary>
        /// Return an accurate measurement of text string dimensions.
        /// </summary>
        /// <param name="g">Reference to graphics object.</param>
        /// <param name="f">Font to use when measuring.</param>
        /// <param name="text">Text to be measured.</param>
        /// <returns>Integer based size of string.</returns>
        public static Size AccurateMeasureString(Graphics g, Font f, string text)
        {
            // Specify we are interested in entire range of characters in the text
            CharacterRange[] ranges = new CharacterRange[]{new CharacterRange(0, text.Length)};
			RectangleF rectF = new RectangleF(0, 0, 1000, 1000);

			// Define the range of characters for measuring
            using(StringFormat format = new StringFormat())
			{
				format.SetMeasurableCharacterRanges(ranges);
				Region[] regions = new Region[1];

				// Measure just the specified range of characters within given rectangle
				regions = g.MeasureCharacterRanges(text, f, rectF, format);

				// Get bounding rectangle that covers the returned region
				rectF = regions[0].GetBounds(g);
			}

            // Return an integer based size of area occupied by characters
            return new Size((int)(rectF.Right + 1.0f), (int)(rectF.Bottom + 1.0f));        
        }

        private static IntPtr CreateRectangleRegion(Rectangle rect, int indent)
        {
            Win32.RECT newWinRect = new Win32.RECT();
            newWinRect.left = rect.Left;
            newWinRect.top = rect.Top;
            newWinRect.right = rect.Right;
            newWinRect.bottom = rect.Bottom;

            // Create region for whole of the new rectangle
            IntPtr newOuter = Gdi32.CreateRectRgnIndirect(ref newWinRect);

            // If the rectangle is to small to make an inner object from, then just use the outer
            if ((indent <= 0) || (rect.Width <= indent) || (rect.Height <= indent))
                return newOuter;

            newWinRect.left += indent;
            newWinRect.top += indent;
            newWinRect.right -= indent;
            newWinRect.bottom -= indent;

            // Create region for the unwanted inside of the new rectangle
            IntPtr newInner = Gdi32.CreateRectRgnIndirect(ref newWinRect);

            Win32.RECT emptyWinRect = new Win32.RECT();
            emptyWinRect.left = 0;
            emptyWinRect.top = 0;
            emptyWinRect.right = 0;
            emptyWinRect.bottom = 0;

            // Create a destination region
            IntPtr newRegion = Gdi32.CreateRectRgnIndirect(ref emptyWinRect);

            // Remove the intersection of the outer and inner
            Gdi32.CombineRgn(newRegion, newOuter, newInner, (int)Win32.CombineFlags.RGN_XOR);

            // Remove unwanted intermediate objects
            Gdi32.DeleteObject(newOuter);
            Gdi32.DeleteObject(newInner);

            // Return the resultant region object
            return newRegion;
        }

        private static IntPtr GetHalfToneBrush()
        {
            if (_halfToneBrush == IntPtr.Zero)
            {	
                Bitmap bitmap = new Bitmap(8,8,PixelFormat.Format32bppArgb);

                Color white = Color.FromArgb(255,255,255,255);
                Color black = Color.FromArgb(255,0,0,0);

                bool flag=true;

                // Alternate black and white pixels across all lines
                for(int x=0; x<8; x++, flag = !flag)
                    for(int y=0; y<8; y++, flag = !flag)
                        bitmap.SetPixel(x, y, (flag ? white : black));

                IntPtr hBitmap = bitmap.GetHbitmap();

                Win32.LOGBRUSH brush = new Win32.LOGBRUSH();

                brush.lbStyle = (uint)Win32.BrushStyles.BS_PATTERN;
                brush.lbHatch = (uint)hBitmap;

                _halfToneBrush = Gdi32.CreateBrushIndirect(ref brush);
            }

            return _halfToneBrush;
        }
    }
}


